using UnityEngine;
using UnityEditor;

using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;

using Ionic.Zlib;
using IZ = Ionic.Zlib;

using FTRuntime;

namespace FTEditor {
	static class SwfEditorUtils {

		// ---------------------------------------------------------------------
		//
		// Packing
		//
		// ---------------------------------------------------------------------

		const ushort UShortMax       = ushort.MaxValue;
		const float  FColorPrecision = 1.0f / 512.0f;

		public static uint PackUV(float u, float v) {
			var uu = (uint)(Mathf.Clamp01(u) * UShortMax);
			var vv = (uint)(Mathf.Clamp01(v) * UShortMax);
			return (uu << 16) + vv;
		}

		public static ushort PackFloatColorToUShort(float v) {
			return (ushort)Mathf.Clamp(
				v * (1.0f / FColorPrecision),
				short.MinValue,
				short.MaxValue);
		}

		public static uint PackUShortsToUInt(ushort x, ushort y) {
			var xx = (uint)x;
			var yy = (uint)y;
			return (xx << 16) + yy;
		}

		public static void PackFColorToUInts(
			Color v,
			out uint pack0, out uint pack1)
		{
			PackFColorToUInts(v.r, v.g, v.b, v.a, out pack0, out pack1);
		}

		public static void PackFColorToUInts(
			Vector4 v,
			out uint pack0, out uint pack1)
		{
			PackFColorToUInts(v.x, v.y, v.z, v.w, out pack0, out pack1);
		}

		public static void PackFColorToUInts(
			float v0, float v1, float v2, float v3,
			out uint pack0, out uint pack1)
		{
			pack0 = PackUShortsToUInt(
				PackFloatColorToUShort(v0),
				PackFloatColorToUShort(v1));
			pack1 = PackUShortsToUInt(
				PackFloatColorToUShort(v2),
				PackFloatColorToUShort(v3));
		}

		// ---------------------------------------------------------------------
		//
		// Inspector
		//
		// ---------------------------------------------------------------------

		public static void DoWithMixedValue(bool mixed, System.Action act) {
			var last_show_mixed_value = EditorGUI.showMixedValue;
			EditorGUI.showMixedValue = mixed;
			try {
				act();
			} finally {
				EditorGUI.showMixedValue = last_show_mixed_value;
			}
		}

		public static void DoWithEnabledGUI(bool enabled, System.Action act) {
			EditorGUI.BeginDisabledGroup(!enabled);
			try {
				act();
			} finally {
				EditorGUI.EndDisabledGroup();
			}
		}

		public static void DoHorizontalGUI(System.Action act) {
			GUILayout.BeginHorizontal();
			try {
				act();
			} finally {
				GUILayout.EndHorizontal();
			}
		}

		public static void DoRightHorizontalGUI(System.Action act) {
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			try {
				act();
			} finally {
				GUILayout.EndHorizontal();
			}
		}

		public static void DoCenterHorizontalGUI(System.Action act) {
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			try {
				act();
			} finally {
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
		}

		public static SerializedProperty GetPropertyByName(SerializedObject obj, string name) {
			var prop = obj.FindProperty(name);
			if ( prop == null ) {
				throw new UnityException(string.Format(
					"SwfEditorUtils. Not found property: {0}",
					name));
			}
			return prop;
		}

		// ---------------------------------------------------------------------
		//
		// Assets
		//
		// ---------------------------------------------------------------------

		public static SwfSettings GetSettingsHolder() {
			var holder = LoadFirstAssetDBByFilter<SwfSettings>("t:SwfSettings");
			if ( !holder ) {
				throw new UnityException(
					"SwfEditorUtils. SwfSettings asset not found");
			}
			return holder;
		}

		public static T LoadOrCreateAsset<T>(string asset_path, System.Func<T, bool, bool> act) where T : ScriptableObject {
			var asset = AssetDatabase.LoadAssetAtPath<T>(asset_path);
			if ( asset ) {
				if ( act(asset, false) ) {
					EditorUtility.SetDirty(asset);
				}
			} else {
				asset = ScriptableObject.CreateInstance<T>();
				if ( act(asset, true) ) {
					AssetDatabase.CreateAsset(asset, asset_path);
				} else {
					ScriptableObject.DestroyImmediate(asset);
				}
			}
			return asset;
		}

		public static T LoadFirstAssetDBByFilter<T>(string filter) where T : UnityEngine.Object {
			var guids = AssetDatabase.FindAssets(filter);
			foreach ( var guid in guids ) {
				var path  = AssetDatabase.GUIDToAssetPath(guid);
				var asset = AssetDatabase.LoadAssetAtPath<T>(path);
				if ( asset ) {
					return asset;
				}
			}
			return null;
		}

		public static T[] LoadAllAssetsDBByFilter<T>(string filter) where T : UnityEngine.Object {
			return AssetDatabase.FindAssets(filter)
				.Select (p => AssetDatabase.GUIDToAssetPath(p))
				.Select (p => AssetDatabase.LoadAssetAtPath<T>(p))
				.Where  (p => !!p)
				.ToArray();
		}

		public static byte[] CompressAsset<T>(T asset, System.Action<float> progress_act) {
			var bytes  = AssetToBytes(asset);
			var result = CompressBuffer(bytes, progress_act);
			return result;
		}

		public static T DecompressAsset<T>(byte[] data, System.Action<float> progress_act) {
			var bytes  = DecompressBuffer(data, progress_act);
			var result = BytesToAsset<T>(bytes);
			return result;
		}

		static byte[] AssetToBytes<T>(T asset) {
			var formatter = new BinaryFormatter();
			using ( var stream = new MemoryStream() ) {
				formatter.Serialize(stream, asset);
				return stream.ToArray();
			}
		}

		static T BytesToAsset<T>(byte[] bytes) {
			var formatter = new BinaryFormatter();
			using ( var stream = new MemoryStream(bytes) ) {
				return (T)formatter.Deserialize(stream);
			}
		}

		static byte[] CompressBuffer(byte[] bytes, System.Action<float> progress_act) {
			using ( var output = new MemoryStream() ) {
				using ( var compressor = new ZlibStream(output, IZ.CompressionMode.Compress, IZ.CompressionLevel.Default) ) {
					var n = 0;
					while ( n < bytes.Length ) {
						var count = Mathf.Min(4 * 1024, bytes.Length - n);
						compressor.Write(bytes, n, count);
						n += count;
						if ( progress_act != null ) {
							progress_act((float)n / bytes.Length);
						}
					}
				}
				return output.ToArray();
			}
		}

		static byte[] DecompressBuffer(byte[] compressed_bytes, System.Action<float> progress_act) {
			using ( var input = new MemoryStream(compressed_bytes) ) {
				using ( var decompressor = new ZlibStream(input, CompressionMode.Decompress) ) {
					using ( var output = new MemoryStream() ) {
						int n;
						var buffer = new byte[4 * 1024];
						while ( (n = decompressor.Read(buffer, 0, buffer.Length)) != 0 ) {
							output.Write(buffer, 0, n);
							if ( progress_act != null ) {
								progress_act((float)decompressor.Position / input.Length);
							}
						}
						return output.ToArray();
					}
				}
			}
		}

		// ---------------------------------------------------------------------
		//
		// Demo
		//
		// ---------------------------------------------------------------------

	#if FT_VERSION_DEMO
		public static bool IsDemoEnded {
			get {
				var guids = AssetDatabase.FindAssets("t:SwfAsset");
				return guids.Length >= 5;
			}
		}
	#else
		public static bool IsDemoEnded {
			get {
				return false;
			}
		}
	#endif

		// ---------------------------------------------------------------------
		//
		// ProgressBar
		//
		// ---------------------------------------------------------------------

		public class ProgressBar {
			string _title = string.Empty;

			public void UpdateTitle(string title) {
				_title = title;
			}

			public void UpdateProgress(string info, float progress) {
				var bar_title    = string.IsNullOrEmpty(_title)
					? "Flash Tools Process"
					: string.Format("Flash Tools Process: {0}", _title);
				var bar_info     = string.Format("{0}...", info);
				var bar_progress = Mathf.Clamp01(progress);
				EditorUtility.DisplayProgressBar(bar_title, bar_info, bar_progress);
			}

			public void HideProgress() {
				EditorUtility.ClearProgressBar();
			}
		}

		// ---------------------------------------------------------------------
		//
		// FileHash
		//
		// ---------------------------------------------------------------------

		public static string GetFileHashWithVersion(string path) {
			return string.Format(
				"{0}={1}",
				GetFileHash(path), SwfVersion.AsString);
		}

		static string GetVersionFromFileHashWithVersion(string hash) {
			var index = hash.LastIndexOf('=');
			return index != -1
				? hash.Substring(index + 1)
				: string.Empty;
		}

		static string GetFileHash(string path) {
			try {
				using ( var sha256 = SHA256.Create() ) {
					var file_bytes = File.ReadAllBytes(path);
					var hash_bytes = sha256.ComputeHash(file_bytes);
					return
						System.Convert.ToBase64String(hash_bytes) +
						file_bytes.LongLength.ToString();
				}
			} catch ( System.Exception ) {
				return string.Empty;
			}
		}

		// ---------------------------------------------------------------------
		//
		// Outdated assets
		//
		// ---------------------------------------------------------------------

		public static bool CheckForOutdatedAsset(SwfClip clip) {
			return clip
				&& CheckForOutdatedAsset(clip.clip);
		}

		public static bool CheckForOutdatedAsset(SwfClipAsset clip_asset) {
			return clip_asset
				&& CheckForOutdatedAsset(AssetDatabase.LoadAssetAtPath<SwfAsset>(
					AssetDatabase.GUIDToAssetPath(clip_asset.AssetGUID)));
		}

		public static bool CheckForOutdatedAsset(SwfAsset asset) {
			return asset
				&& GetVersionFromFileHashWithVersion(asset.Hash) != SwfVersion.AsString;
		}

		public static bool CheckForOutdatedAsset(IEnumerable<SwfClip> clips) {
			var iter = clips.GetEnumerator();
			while ( iter.MoveNext() ) {
				if ( CheckForOutdatedAsset(iter.Current) ) {
					return true;
				}
			}
			return false;
		}

		public static bool CheckForOutdatedAsset(IEnumerable<SwfClipAsset> clip_assets) {
			var iter = clip_assets.GetEnumerator();
			while ( iter.MoveNext() ) {
				if ( CheckForOutdatedAsset(iter.Current) ) {
					return true;
				}
			}
			return false;
		}

		public static bool CheckForOutdatedAsset(IEnumerable<SwfAsset> assets) {
			var iter = assets.GetEnumerator();
			while ( iter.MoveNext() ) {
				if ( CheckForOutdatedAsset(iter.Current) ) {
					return true;
				}
			}
			return false;
		}

		// ---------------------------------------------------------------------
		//
		// GUI notes
		//
		// ---------------------------------------------------------------------

		public static void DrawMasksGUINotes() {
			EditorGUILayout.Separator();
			EditorGUILayout.HelpBox(
				"Masks and blends of animation may not be displayed correctly in the preview window. " +
				"Instance animation to the scene, to see how it will look like the animation in the game.",
				MessageType.Info);
		}

		public static void DrawOutdatedGUINotes(string target, IEnumerable<SwfClip> clips) {
			DrawOutdatedGUINotes(target, clips
				.Select(p => p ? p.clip : null));
		}

		public static void DrawOutdatedGUINotes(string target, IEnumerable<SwfClipAsset> clips) {
			DrawOutdatedGUINotes(target, clips
				.Select(p => {
					return p
						? AssetDatabase.LoadAssetAtPath<SwfAsset>(AssetDatabase.GUIDToAssetPath(p.AssetGUID))
						: null;
				}));
		}

		public static void DrawOutdatedGUINotes(string target, IEnumerable<SwfAsset> assets) {
			var asset_count = assets.Count(p => p);
			if ( asset_count == 1 ) {
				var asset = assets.FirstOrDefault(p => p);
				if ( asset ) {
					var asset_version = GetVersionFromFileHashWithVersion(asset.Hash);
					if ( asset_version != SwfVersion.AsString ) {
						EditorGUILayout.Separator();
						EditorGUILayout.HelpBox(string.Format(
							"The {0} was created in the {1} version of Flash Animation Toolset, and it's outdated.\n" +
							"Please, reimport the source .swf file. It's may be essential to correctness working.\n" +
							"You can do it in Tools/FlashTools menu.",
							target, asset_version),
							MessageType.Error);
					}
				}
			} else if ( asset_count > 1 ) {
				var any_outdated = assets
					.Any(p => GetVersionFromFileHashWithVersion(p.Hash) != SwfVersion.AsString);
				if ( any_outdated ) {
					EditorGUILayout.Separator();
					EditorGUILayout.HelpBox(string.Format(
						"Some {0} is outdated.\n" +
						"Please, reimport the source .swf files. It's may be essential to correctness working.\n" +
						"You can do it in Tools/FlashTools menu.",
						target),
						MessageType.Error);
				}
			}
		}

		// ---------------------------------------------------------------------
		//
		// Menu
		//
		// ---------------------------------------------------------------------

		[MenuItem("Tools/FlashTools/Open settings...")]
		static void Tools_FlashTools_OpenSettings() {
			var settings_holder = SwfEditorUtils.GetSettingsHolder();
			Selection.objects = new Object[]{settings_holder};
			EditorGUIUtility.PingObject(settings_holder);
		}

		[MenuItem("Tools/FlashTools/Reimport all swf files")]
		static void Tools_FlashTools_ReimportAllSwfFiles() {
			var swf_paths = GetAllSwfFilePaths();
			var title     = "Reimport";
			var message   = string.Format(
				"Do you really want to reimport all ({0}) swf files?",
				swf_paths.Length);
			if ( EditorUtility.DisplayDialog(title, message, "Ok", "Cancel") ) {
				foreach ( var swf_path in swf_paths ) {
					AssetDatabase.ImportAsset(swf_path);
				}
			}
		}

		[MenuItem("Tools/FlashTools/Reconvert all swf assets")]
		static void Tools_FlashTools_ReconvertAllSwfAssets() {
			Tools_FlashTools_ReimportAllSwfFiles();
			var swf_assets = GetAllSwfAssets();
			foreach ( var swf_asset in swf_assets ) {
				if ( swf_asset.Atlas ) {
					AssetDatabase.DeleteAsset(
						AssetDatabase.GetAssetPath(swf_asset.Atlas));
					swf_asset.Atlas = null;
				}
				EditorUtility.SetDirty(swf_asset);
				AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(swf_asset));
			}
		}

		[MenuItem("Tools/FlashTools/Pregenerate all materials")]
		static void PregenerateAllMaterials() {
			var blend_modes = System.Enum.GetValues(typeof(SwfBlendModeData.Types));
			foreach ( SwfBlendModeData.Types blend_mode in blend_modes ) {
				SwfMaterialCache.GetSimpleMaterial(blend_mode);
				for ( var i = 0; i < 10; ++i ) {
					SwfMaterialCache.GetMaskedMaterial(blend_mode, i);
				}
			}
			SwfMaterialCache.GetIncrMaskMaterial();
			SwfMaterialCache.GetDecrMaskMaterial();
		}

		static SwfAsset[] GetAllSwfAssets() {
			return SwfEditorUtils.LoadAllAssetsDBByFilter<SwfAsset>("t:SwfAsset");
		}

		static string[] GetAllSwfFilePaths() {
			return AssetDatabase.GetAllAssetPaths()
				.Where(p => Path.GetExtension(p).ToLower().Equals(".swf"))
				.ToArray();
		}
	}
}