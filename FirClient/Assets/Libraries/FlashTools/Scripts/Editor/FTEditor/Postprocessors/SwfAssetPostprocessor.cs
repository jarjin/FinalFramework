using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using FTRuntime;

namespace FTEditor.Postprocessors {
	class SwfAssetPostprocessor : AssetPostprocessor {
		static SwfEditorUtils.ProgressBar _progressBar = new SwfEditorUtils.ProgressBar();

		static void OnPostprocessAllAssets(
			string[] imported_assets,
			string[] deleted_assets,
			string[] moved_assets,
			string[] moved_from_asset_paths)
		{
			var assets = imported_assets
				.Where(p => Path.GetExtension(p).ToLower().Equals(".asset"))
				.Select(p => AssetDatabase.LoadAssetAtPath<SwfAsset>(p))
				.Where(p => p && !p.Atlas);
			if ( assets.Any() ) {
				EditorApplication.delayCall += () => {
					foreach ( var asset in assets ) {
						SwfAssetProcess(asset);
					}
					AssetDatabase.SaveAssets();
				};
			}
		}

		static void SwfAssetProcess(SwfAsset asset) {
			try {
				EditorUtility.SetDirty(asset);
				var asset_data = SwfEditorUtils.DecompressAsset<SwfAssetData>(asset.Data, progress => {
					_progressBar.UpdateProgress("decompress swf asset", progress);
				});
				asset.Atlas = LoadAssetAtlas(asset);
				if ( asset.Atlas ) {
					ConfigureAtlas(asset);
					ConfigureClips(asset, asset_data);
					Debug.LogFormat(
						asset,
						"<b>[FlashTools]</b> SwfAsset has been successfully converted:\nPath: {0}",
						AssetDatabase.GetAssetPath(asset));
				} else {
					_progressBar.UpdateTitle(asset.name);
					var new_data = ConfigureBitmaps(asset, asset_data);
					asset.Data = SwfEditorUtils.CompressAsset(new_data, progress => {
						_progressBar.UpdateProgress("compress swf asset", progress);
					});
				}
			} catch ( Exception e ) {
				Debug.LogErrorFormat(
					asset,
					"<b>[FlashTools]</b> Postprocess swf asset error: {0}\nPath: {1}",
					e.Message, AssetDatabase.GetAssetPath(asset));
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
			} finally {
				if ( asset ) {
					UpdateAssetClips(asset);
				}
				_progressBar.HideProgress();
			}
		}

		static Texture2D LoadAssetAtlas(SwfAsset asset) {
			return AssetDatabase.LoadAssetAtPath<Texture2D>(
				GetAtlasPath(asset));
		}

		static string GetAtlasPath(SwfAsset asset) {
			if ( asset.Atlas ) {
				return AssetDatabase.GetAssetPath(asset.Atlas);
			} else {
				var asset_path = AssetDatabase.GetAssetPath(asset);
				return Path.ChangeExtension(asset_path, "._Atlas_.png");
			}
		}

		// ---------------------------------------------------------------------
		//
		// ConfigureBitmaps
		//
		// ---------------------------------------------------------------------

		static SwfAssetData ConfigureBitmaps(SwfAsset asset, SwfAssetData data) {
			var textures = new List<KeyValuePair<ushort, Texture2D>>(data.Bitmaps.Count);
			for ( var i = 0; i < data.Bitmaps.Count; ++i ) {
				_progressBar.UpdateProgress(
					"configure bitmaps",
					(float)(i + 1) / data.Bitmaps.Count);
				var bitmap = data.Bitmaps[i];
				if ( bitmap.Redirect == 0 ) {
					textures.Add(new KeyValuePair<ushort, Texture2D>(
						bitmap.Id,
						LoadTextureFromData(bitmap, asset.Settings)));
				}
			}
			var rects = PackAndSaveBitmapsAtlas(
				GetAtlasPath(asset),
				textures.Select(p => p.Value).ToArray(),
				asset.Settings);
			for ( var i = 0; i < data.Bitmaps.Count; ++i ) {
				var bitmap        = data.Bitmaps[i];
				var texture_key   = bitmap.Redirect > 0 ? bitmap.Redirect : bitmap.Id;
				bitmap.SourceRect = SwfRectData.FromURect(
					rects[textures.FindIndex(p => p.Key == texture_key)]);
			}
			return data;
		}

		static Texture2D LoadTextureFromData(SwfBitmapData bitmap, SwfSettingsData settings) {
			var argb32 = settings.BitmapTrimming
				? TrimBitmapByRect(bitmap, bitmap.TrimmedRect)
				: bitmap.ARGB32;
			var widht = settings.BitmapTrimming
				? bitmap.TrimmedRect.width
				: bitmap.RealWidth;
			var height = settings.BitmapTrimming
				? bitmap.TrimmedRect.height
				: bitmap.RealHeight;
			var texture = new Texture2D(
				widht, height,
				TextureFormat.ARGB32, false);
			texture.LoadRawTextureData(argb32);
			return texture;
		}

		static byte[] TrimBitmapByRect(SwfBitmapData bitmap, SwfRectIntData rect) {
			var argb32 = new byte[rect.area * 4];
			for ( var i = 0; i < rect.height; ++i ) {
				var src_index = rect.xMin + (rect.yMin + i) * bitmap.RealWidth;
				var dst_index = i * rect.width;
				Array.Copy(
					bitmap.ARGB32, src_index * 4,
					argb32, dst_index * 4,
					rect.width * 4);
			}
			return argb32;
		}

		struct BitmapsAtlasInfo {
			public Texture2D Atlas;
			public Rect[]    Rects;
		}

		static Rect[] PackAndSaveBitmapsAtlas(
			string atlas_path, Texture2D[] textures, SwfSettingsData settings)
		{
			_progressBar.UpdateProgress("pack bitmaps", 0.25f);
			var atlas_info = PackBitmapsAtlas(textures, settings);
			RevertTexturePremultipliedAlpha(atlas_info.Atlas);
			_progressBar.UpdateProgress("save atlas", 0.5f);
			File.WriteAllBytes(atlas_path, atlas_info.Atlas.EncodeToPNG());
			GameObject.DestroyImmediate(atlas_info.Atlas, true);
			_progressBar.UpdateProgress("import atlas", 0.75f);
			AssetDatabase.ImportAsset(atlas_path);
			return atlas_info.Rects;
		}

		static BitmapsAtlasInfo PackBitmapsAtlas(
			Texture2D[] textures, SwfSettingsData settings)
		{
			var atlas_padding  = Mathf.Max(0,  settings.AtlasPadding);
			var max_atlas_size = Mathf.Max(32, settings.AtlasPowerOfTwo
				? Mathf.ClosestPowerOfTwo(settings.MaxAtlasSize)
				: settings.MaxAtlasSize);
			var atlas = new Texture2D(0, 0);
			var rects = atlas.PackTextures(textures, atlas_padding, max_atlas_size);
			while ( rects == null ) {
				max_atlas_size = Mathf.NextPowerOfTwo(max_atlas_size + 1);
				rects = atlas.PackTextures(textures, atlas_padding, max_atlas_size);
			}
			return settings.AtlasForceSquare && atlas.width != atlas.height
				? BitmapsAtlasToSquare(atlas, rects)
				: new BitmapsAtlasInfo{Atlas = atlas, Rects = rects};
		}

		static BitmapsAtlasInfo BitmapsAtlasToSquare(Texture2D atlas, Rect[] rects) {
			var atlas_size  = Mathf.Max(atlas.width, atlas.height);
			var atlas_scale = new Vector2(atlas.width, atlas.height) / atlas_size;
			var new_atlas   = new Texture2D(atlas_size, atlas_size, TextureFormat.ARGB32, false);
			for ( var i = 0; i < rects.Length; ++i ) {
				var new_position = rects[i].position;
				new_position.Scale(atlas_scale);
				var new_size = rects[i].size;
				new_size.Scale(atlas_scale);
				rects[i] = new Rect(new_position, new_size);
			}
			var fill_pixels = new Color32[atlas_size * atlas_size];
			for ( var i = 0; i < atlas_size * atlas_size; ++i ) {
				fill_pixels[i] = new Color(1,1,1,0);
			}
			new_atlas.SetPixels32(fill_pixels);
			new_atlas.SetPixels32(0, 0, atlas.width, atlas.height, atlas.GetPixels32());
			new_atlas.Apply();
			GameObject.DestroyImmediate(atlas, true);
			return new BitmapsAtlasInfo{
				Atlas = new_atlas,
				Rects = rects};
		}

		static void RevertTexturePremultipliedAlpha(Texture2D texture) {
			var pixels = texture.GetPixels();
			for ( var i = 0; i < pixels.Length; ++i ) {
				var c = pixels[i];
				if ( c.a > 0 ) {
					c.r /= c.a;
					c.g /= c.a;
					c.b /= c.a;
				}
				pixels[i] = c;
			}
			texture.SetPixels(pixels);
			texture.Apply();
		}

		// ---------------------------------------------------------------------
		//
		// ConfigureAtlas
		//
		// ---------------------------------------------------------------------

		static void ConfigureAtlas(SwfAsset asset) {
			var atlas_importer                 = GetBitmapsAtlasImporter(asset);
			atlas_importer.textureType         = TextureImporterType.Sprite;
			atlas_importer.spriteImportMode    = SpriteImportMode.Single;
			atlas_importer.spritePixelsPerUnit = asset.Settings.PixelsPerUnit;
			atlas_importer.mipmapEnabled       = asset.Settings.GenerateMipMaps;
			atlas_importer.filterMode          = SwfAtlasFilterToImporterFilter(asset.Settings.AtlasTextureFilter);
			atlas_importer.textureCompression  = SwfAtlasFormatToImporterCompression(asset.Settings.AtlasTextureFormat);

			var atlas_settings = new TextureImporterSettings();
			atlas_importer.ReadTextureSettings(atlas_settings);
			atlas_settings.spriteMeshType = SpriteMeshType.FullRect;
			atlas_importer.SetTextureSettings(atlas_settings);

			atlas_importer.SaveAndReimport();
		}

		static TextureImporter GetBitmapsAtlasImporter(SwfAsset asset) {
			var atlas_path     = AssetDatabase.GetAssetPath(asset.Atlas);
			var atlas_importer = UnityEditor.AssetImporter.GetAtPath(atlas_path) as TextureImporter;
			if ( !atlas_importer ) {
				throw new UnityException(string.Format(
					"atlas texture importer not found ({0})",
					atlas_path));
			}
			return atlas_importer;
		}

		static FilterMode SwfAtlasFilterToImporterFilter(
			SwfSettingsData.AtlasFilter filter)
		{
			switch ( filter ) {
			case SwfSettingsData.AtlasFilter.Point:
				return FilterMode.Point;
			case SwfSettingsData.AtlasFilter.Bilinear:
				return FilterMode.Bilinear;
			case SwfSettingsData.AtlasFilter.Trilinear:
				return FilterMode.Trilinear;
			default:
				throw new UnityException(string.Format(
					"incorrect swf atlas filter ({0})",
					filter));
			}
		}

		static TextureImporterCompression SwfAtlasFormatToImporterCompression(
			SwfSettingsData.AtlasFormat format)
		{
			switch ( format ) {
			case SwfSettingsData.AtlasFormat.AutomaticCompressed:
				return TextureImporterCompression.Compressed;
			case SwfSettingsData.AtlasFormat.AutomaticTruecolor:
				return TextureImporterCompression.Uncompressed;
			default:
				throw new UnityException(string.Format(
					"incorrect swf atlas format ({0})",
					format));
			}
		}

		// ---------------------------------------------------------------------
		//
		// ConfigureClips
		//
		// ---------------------------------------------------------------------

		static SwfAssetData ConfigureClips(SwfAsset asset, SwfAssetData data) {
			for ( var i = 0; i < data.Symbols.Count; ++i ) {
				_progressBar.UpdateProgress(
					"configure clips",
					(float)(i + 1) / data.Symbols.Count);
				ConfigureClip(asset, data, data.Symbols[i]);
			}
			return data;
		}

		static void ConfigureClip(SwfAsset asset, SwfAssetData data, SwfSymbolData symbol) {
			var asset_guid  = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));
			var clip_assets = SwfEditorUtils.LoadAllAssetsDBByFilter<SwfClipAsset>("t:SwfClipAsset")
				.Where(p => p.AssetGUID == asset_guid && p.Name == symbol.Name);
			if ( clip_assets.Any() ) {
				foreach ( var clip_asset in clip_assets ) {
					ConfigureClipAsset(clip_asset, asset, data, symbol);
				}
			} else {
				var asset_path      = AssetDatabase.GetAssetPath(asset);
				var clip_asset_path = Path.ChangeExtension(asset_path, symbol.Name + ".asset");
				SwfEditorUtils.LoadOrCreateAsset<SwfClipAsset>(clip_asset_path, (new_clip_asset, created) => {
					ConfigureClipAsset(new_clip_asset, asset, data, symbol);
					return true;
				});
			}
		}

		static void ConfigureClipAsset(
			SwfClipAsset clip_asset, SwfAsset asset, SwfAssetData data, SwfSymbolData symbol)
		{
			var asset_guid       = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));
			var asset_atlas      = AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GetAssetPath(asset.Atlas));
			clip_asset.Name      = symbol.Name;
			clip_asset.Sprite    = asset_atlas;
			clip_asset.FrameRate = data.FrameRate;
			clip_asset.AssetGUID = asset_guid;
			clip_asset.Sequences = LoadClipSequences(asset, data, symbol);
			EditorUtility.SetDirty(clip_asset);
		}

		static List<SwfClipAsset.Sequence> LoadClipSequences(
			SwfAsset asset, SwfAssetData data, SwfSymbolData symbol)
		{
			var sequences = new List<SwfClipAsset.Sequence>();
			if ( IsValidAssetsForFrame(asset, symbol) ) {
				foreach ( var frame in symbol.Frames ) {
					var baked_frame = BakeClipFrame(asset, data, frame);
					if ( !string.IsNullOrEmpty(frame.Anchor) &&
						(sequences.Count < 1 || sequences.Last().Name != frame.Anchor) )
					{
						sequences.Add(new SwfClipAsset.Sequence{Name = frame.Anchor});
					} else if ( sequences.Count < 1 ) {
						sequences.Add(new SwfClipAsset.Sequence{Name = "Default"});
					}
					sequences.Last().Frames.Add(baked_frame);
				}
			}
			return sequences;
		}

		static bool IsValidAssetsForFrame(
			SwfAsset asset, SwfSymbolData symbol)
		{
			return
				asset && asset.Atlas && asset.Data != null &&
				symbol != null && symbol.Frames != null;
		}

		class BakedGroup {
			public SwfInstanceData.Types  Type;
			public SwfBlendModeData.Types BlendMode;
			public int                    ClipDepth;
			public int                    StartVertex;
			public int                    TriangleCount;
			public Material               Material;
		}

		static SwfClipAsset.Frame BakeClipFrame(
			SwfAsset asset, SwfAssetData data, SwfFrameData frame)
		{
			List<uint>       baked_uvs       = new List<uint>();
			List<uint>       baked_mulcolors = new List<uint>();
			List<uint>       baked_addcolors = new List<uint>();
			List<Vector2>    baked_vertices  = new List<Vector2>();
			List<BakedGroup> baked_groups    = new List<BakedGroup>();
			List<Material>   baked_materials = new List<Material>();

			foreach ( var inst in frame.Instances ) {
				var bitmap = inst != null
					? FindBitmapFromAssetData(data, inst.Bitmap)
					: null;
				while ( bitmap != null && bitmap.Redirect > 0 ) {
					bitmap = FindBitmapFromAssetData(data, bitmap.Redirect);
				}
				if ( bitmap != null ) {
					var br = asset.Settings.BitmapTrimming
						? bitmap.TrimmedRect
						: new SwfRectIntData(bitmap.RealWidth, bitmap.RealHeight);

					var v0 = new Vector2(br.xMin, br.yMin);
					var v1 = new Vector2(br.xMax, br.yMin);
					var v2 = new Vector2(br.xMax, br.yMax);
					var v3 = new Vector2(br.xMin, br.yMax);

					var matrix =
						Matrix4x4.Scale(new Vector3(1.0f, -1.0f, 1.0f) / asset.Settings.PixelsPerUnit) *
						inst.Matrix.ToUMatrix() *
						Matrix4x4.Scale(new Vector3(1.0f / 20.0f, 1.0f / 20.0f, 1.0f));

					baked_vertices.Add(matrix.MultiplyPoint3x4(v0));
					baked_vertices.Add(matrix.MultiplyPoint3x4(v1));
					baked_vertices.Add(matrix.MultiplyPoint3x4(v2));
					baked_vertices.Add(matrix.MultiplyPoint3x4(v3));

					var source_rect = bitmap.SourceRect;
					baked_uvs.Add(SwfEditorUtils.PackUV(source_rect.xMin, source_rect.yMin));
					baked_uvs.Add(SwfEditorUtils.PackUV(source_rect.xMax, source_rect.yMax));

					uint mul_pack0, mul_pack1;
					SwfEditorUtils.PackFColorToUInts(
						inst.ColorTrans.mulColor.ToUVector4(),
						out mul_pack0, out mul_pack1);
					baked_mulcolors.Add(mul_pack0);
					baked_mulcolors.Add(mul_pack1);

					uint add_pack0, add_pack1;
					SwfEditorUtils.PackFColorToUInts(
						inst.ColorTrans.addColor.ToUVector4(),
						out add_pack0, out add_pack1);
					baked_addcolors.Add(add_pack0);
					baked_addcolors.Add(add_pack1);

					if ( baked_groups.Count == 0 ||
						baked_groups[baked_groups.Count - 1].Type      != inst.Type           ||
						baked_groups[baked_groups.Count - 1].BlendMode != inst.BlendMode.type ||
						baked_groups[baked_groups.Count - 1].ClipDepth != inst.ClipDepth )
					{
						baked_groups.Add(new BakedGroup{
							Type          = inst.Type,
							BlendMode     = inst.BlendMode.type,
							ClipDepth     = inst.ClipDepth,
							StartVertex   = baked_vertices.Count - 4,
							TriangleCount = 0,
							Material      = null
						});
					}

					baked_groups.Last().TriangleCount += 6;
				}
			}

			for ( var i = 0; i < baked_groups.Count; ++i ) {
				var group = baked_groups[i];
				switch ( group.Type ) {
				case SwfInstanceData.Types.Mask:
					group.Material = SwfMaterialCache.GetIncrMaskMaterial();
					break;
				case SwfInstanceData.Types.Group:
					group.Material = SwfMaterialCache.GetSimpleMaterial(group.BlendMode);
					break;
				case SwfInstanceData.Types.Masked:
					group.Material = SwfMaterialCache.GetMaskedMaterial(group.BlendMode, group.ClipDepth);
					break;
				case SwfInstanceData.Types.MaskReset:
					group.Material = SwfMaterialCache.GetDecrMaskMaterial();
					break;
				default:
					throw new UnityException(string.Format(
						"SwfAssetPostprocessor. Incorrect instance type: {0}",
						group.Type));
				}
				if ( group.Material ) {
					baked_materials.Add(group.Material);
				} else {
					throw new UnityException(string.Format(
						"SwfAssetPostprocessor. Material for baked group ({0}) not found",
						group.Type));
				}
			}

			var mesh_data = new SwfClipAsset.MeshData{
				SubMeshes = baked_groups
					.Select(p => new SwfClipAsset.SubMeshData{
						StartVertex = p.StartVertex,
						IndexCount  = p.TriangleCount})
					.ToArray(),
				Vertices  = baked_vertices .ToArray(),
				UVs       = baked_uvs      .ToArray(),
				AddColors = baked_addcolors.ToArray(),
				MulColors = baked_mulcolors.ToArray()};

			return new SwfClipAsset.Frame(
				frame.Labels.ToArray(),
				mesh_data,
				baked_materials.ToArray());
		}

		static SwfBitmapData FindBitmapFromAssetData(SwfAssetData data, int bitmap_id) {
			for ( var i = 0; i < data.Bitmaps.Count; ++i ) {
				var bitmap = data.Bitmaps[i];
				if ( bitmap.Id == bitmap_id ) {
					return bitmap;
				}
			}
			return null;
		}

		// ---------------------------------------------------------------------
		//
		// UpdateAssetClips
		//
		// ---------------------------------------------------------------------

		static void UpdateAssetClips(SwfAsset asset) {
			var asset_guid  = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));
			var scene_clips = GameObject.FindObjectsOfType<SwfClip>()
				.Where (p => p && p.clip && p.clip.AssetGUID == asset_guid)
				.ToList();
			for ( var i = 0; i < scene_clips.Count; ++i ) {
				_progressBar.UpdateProgress(
					"update scene clips",
					(float)(i + 1) / scene_clips.Count);
				scene_clips[i].Internal_UpdateAllProperties();
			}
		}
	}
}