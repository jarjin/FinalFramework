using System.Linq;
using System.Collections.Generic;

using FTSwfTools.SwfTags;
using FTSwfTools.SwfTypes;

namespace FTSwfTools {
	public class SwfContextExecuter : SwfTagVisitor<SwfDisplayList, SwfDisplayList> {
		public SwfLibrary            Library    = null;
		public int                   CurrentTag = 0;
		public System.Action<string> WarningLog = null;

		public SwfContextExecuter(SwfLibrary library, int current_tag, System.Action<string> warning_log) {
			Library    = library;
			CurrentTag = current_tag;
			WarningLog = warning_log;
		}

		public bool NextFrame(List<SwfTagBase> tags, SwfDisplayList dl) {
			dl.FrameLabels.Clear();
			dl.FrameAnchors.Clear();
			while ( CurrentTag < tags.Count ) {
				var tag = tags[CurrentTag++];
				tag.AcceptVistor(this, dl);
				if ( tag.TagType == SwfTagType.ShowFrame ) {
					ChildrenNextFrameLooped(dl);
					return true;
				}
			}
			ChildrenNextFrameLooped(dl);
			return false;
		}

		public SwfDisplayList Visit(PlaceObjectTag tag, SwfDisplayList dl) {
			var is_shape  = Library.HasDefine<SwfLibraryShapeDefine >(tag.CharacterId);
			var is_bitmap = Library.HasDefine<SwfLibraryBitmapDefine>(tag.CharacterId);
			var is_sprite = Library.HasDefine<SwfLibrarySpriteDefine>(tag.CharacterId);
			SwfDisplayInstance new_inst = null;
			if ( is_shape ) {
				new_inst = new SwfDisplayShapeInstance();
			} else if ( is_bitmap ) {
				new_inst = new SwfDisplayBitmapInstance();
			} else if ( is_sprite ) {
				new_inst = new SwfDisplaySpriteInstance();
			}
			if ( new_inst != null ) {
				new_inst.Id             = tag.CharacterId;
				new_inst.Depth          = tag.Depth;
				new_inst.ClipDepth      = 0;
				new_inst.Visible        = true;
				new_inst.Matrix         = tag.Matrix;
				new_inst.BlendMode      = SwfBlendMode.identity;
				new_inst.FilterList     = SwfSurfaceFilters.identity;
				new_inst.ColorTransform = tag.ColorTransform;
				dl.Instances.Add(new_inst.Depth, new_inst);
			}
			return dl;
		}

		public SwfDisplayList Visit(PlaceObject2Tag tag, SwfDisplayList dl) {
			var is_shape  = tag.HasCharacter && Library.HasDefine<SwfLibraryShapeDefine >(tag.CharacterId);
			var is_bitmap = tag.HasCharacter && Library.HasDefine<SwfLibraryBitmapDefine>(tag.CharacterId);
			var is_sprite = tag.HasCharacter && Library.HasDefine<SwfLibrarySpriteDefine>(tag.CharacterId);
			if ( tag.HasCharacter ) {
				SwfDisplayInstance old_inst = null;
				if ( tag.Move ) { // replace character
					if ( dl.Instances.TryGetValue(tag.Depth, out old_inst) ) {
						dl.Instances.Remove(tag.Depth);
					}
				}
				// new character
				SwfDisplayInstance new_inst = null;
				if ( is_shape ) {
					new_inst = new SwfDisplayShapeInstance();
				} else if ( is_bitmap ) {
					new_inst = new SwfDisplayBitmapInstance();
				} else if ( is_sprite ) {
					new_inst = new SwfDisplaySpriteInstance();
				}
				if ( new_inst != null ) {
					new_inst.Id             = tag.CharacterId;
					new_inst.Depth          = tag.Depth;
					new_inst.ClipDepth      = tag.HasClipDepth      ? tag.ClipDepth      : (old_inst != null ? old_inst.ClipDepth      : (ushort)0);
					new_inst.Visible        = true;
					new_inst.Matrix         = tag.HasMatrix         ? tag.Matrix         : (old_inst != null ? old_inst.Matrix         : SwfMatrix.identity);
					new_inst.BlendMode      = SwfBlendMode.identity;
					new_inst.FilterList     = SwfSurfaceFilters.identity;
					new_inst.ColorTransform = tag.HasColorTransform ? tag.ColorTransform : (old_inst != null ? old_inst.ColorTransform : SwfColorTransform.identity);
					dl.Instances.Add(new_inst.Depth, new_inst);
				}
			} else if ( tag.Move ) { // move character
				SwfDisplayInstance inst;
				if ( dl.Instances.TryGetValue(tag.Depth, out inst) ) {
					if ( tag.HasClipDepth ) {
						inst.ClipDepth = tag.ClipDepth;
					}
					if ( tag.HasMatrix ) {
						inst.Matrix = tag.Matrix;
					}
					if ( tag.HasColorTransform ) {
						inst.ColorTransform = tag.ColorTransform;
					}
				}
			}
			return dl;
		}

		public SwfDisplayList Visit(PlaceObject3Tag tag, SwfDisplayList dl) {
			var is_shape  = tag.HasCharacter && Library.HasDefine<SwfLibraryShapeDefine >(tag.CharacterId);
			var is_bitmap = tag.HasCharacter && Library.HasDefine<SwfLibraryBitmapDefine>(tag.CharacterId);
			var is_sprite = tag.HasCharacter && Library.HasDefine<SwfLibrarySpriteDefine>(tag.CharacterId);
			if ( tag.HasCharacter ) {
				SwfDisplayInstance old_inst = null;
				if ( tag.Move ) { // replace character
					if ( dl.Instances.TryGetValue(tag.Depth, out old_inst) ) {
						dl.Instances.Remove(tag.Depth);
					}
				}
				// new character
				SwfDisplayInstance new_inst = null;
				if ( is_shape ) {
					new_inst = new SwfDisplayShapeInstance();
				} else if ( is_bitmap ) {
					new_inst = new SwfDisplayBitmapInstance();
				} else if ( is_sprite ) {
					new_inst = new SwfDisplaySpriteInstance();
				}
				if ( new_inst != null ) {
					new_inst.Id             = tag.CharacterId;
					new_inst.Depth          = tag.Depth;
					new_inst.ClipDepth      = tag.HasClipDepth      ? tag.ClipDepth      : (old_inst != null ? old_inst.ClipDepth      : (ushort)0);
					new_inst.Visible        = tag.HasVisible        ? tag.Visible        : (old_inst != null ? old_inst.Visible        : true);
					new_inst.Matrix         = tag.HasMatrix         ? tag.Matrix         : (old_inst != null ? old_inst.Matrix         : SwfMatrix.identity);
					new_inst.BlendMode      = tag.HasBlendMode      ? tag.BlendMode      : (old_inst != null ? old_inst.BlendMode      : SwfBlendMode.identity);
					new_inst.FilterList     = tag.HasFilterList     ? tag.SurfaceFilters : (old_inst != null ? old_inst.FilterList     : SwfSurfaceFilters.identity);
					new_inst.ColorTransform = tag.HasColorTransform ? tag.ColorTransform : (old_inst != null ? old_inst.ColorTransform : SwfColorTransform.identity);
					dl.Instances.Add(new_inst.Depth, new_inst);
				}
			} else if ( tag.Move ) { // move character
				SwfDisplayInstance inst;
				if ( dl.Instances.TryGetValue(tag.Depth, out inst) ) {
					if ( tag.HasClipDepth ) {
						inst.ClipDepth = tag.ClipDepth;
					}
					if ( tag.HasVisible ) {
						inst.Visible = tag.Visible;
					}
					if ( tag.HasMatrix ) {
						inst.Matrix = tag.Matrix;
					}
					if ( tag.HasBlendMode ) {
						inst.BlendMode = tag.BlendMode;
					}
					if ( tag.HasFilterList ) {
						inst.FilterList = tag.SurfaceFilters;
					}
					if ( tag.HasColorTransform ) {
						inst.ColorTransform = tag.ColorTransform;
					}
				}
			}
			return dl;
		}

		public SwfDisplayList Visit(RemoveObjectTag tag, SwfDisplayList dl) {
			dl.Instances.Remove(tag.Depth);
			return dl;
		}

		public SwfDisplayList Visit(RemoveObject2Tag tag, SwfDisplayList dl) {
			dl.Instances.Remove(tag.Depth);
			return dl;
		}

		public SwfDisplayList Visit(ShowFrameTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(SetBackgroundColorTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(FrameLabelTag tag, SwfDisplayList dl) {
			const string anchor_prefix = "FT_ANCHOR:";
			if ( tag.Name.StartsWith(anchor_prefix) ) {
				dl.FrameAnchors.Add(tag.Name.Remove(0, anchor_prefix.Length).Trim());
			} else if ( tag.AnchorFlag == 0 ) {
				dl.FrameLabels.Add(tag.Name.Trim());
			} else {
				dl.FrameAnchors.Add(tag.Name.Trim());
			}
			return dl;
		}

		public SwfDisplayList Visit(ProtectTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(EndTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(ExportAssetsTag tag, SwfDisplayList dl) {
			foreach ( var asset_tag in tag.AssetTags ) {
				var define = Library.FindDefine<SwfLibraryDefine>(asset_tag.Tag);
				if ( define != null ) {
					define.ExportName = asset_tag.Name.Trim();
				}
			}
			return dl;
		}

		public SwfDisplayList Visit(EnableDebuggerTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(EnableDebugger2Tag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(ScriptLimitsTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(SymbolClassTag tag, SwfDisplayList dl) {
			foreach ( var symbol_tag in tag.SymbolTags ) {
				var define = Library.FindDefine<SwfLibraryDefine>(symbol_tag.Tag);
				if ( define != null ) {
					define.ExportName = symbol_tag.Name.Trim();
				}
			}
			return dl;
		}

		public SwfDisplayList Visit(MetadataTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(DefineSceneAndFrameLabelDataTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(DoABCTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(DefineShapeTag tag, SwfDisplayList dl) {
			AddShapesToLibrary(tag.ShapeId, tag.Shapes);
			return dl;
		}

		public SwfDisplayList Visit(DefineShape2Tag tag, SwfDisplayList dl) {
			AddShapesToLibrary(tag.ShapeId, tag.Shapes);
			return dl;
		}

		public SwfDisplayList Visit(DefineShape3Tag tag, SwfDisplayList dl) {
			AddShapesToLibrary(tag.ShapeId, tag.Shapes);
			return dl;
		}

		public SwfDisplayList Visit(DefineShape4Tag tag, SwfDisplayList dl) {
			AddShapesToLibrary(tag.ShapeId, tag.Shapes);
			return dl;
		}

		public SwfDisplayList Visit(DefineBitsLosslessTag tag, SwfDisplayList dl) {
			AddBitmapToLibrary(
				tag.CharacterId,
				tag.BitmapWidth,
				tag.BitmapHeight,
				tag.ToARGB32());
			return dl;
		}

		public SwfDisplayList Visit(DefineBitsLossless2Tag tag, SwfDisplayList dl) {
			AddBitmapToLibrary(
				tag.CharacterId,
				tag.BitmapWidth,
				tag.BitmapHeight,
				tag.ToARGB32());
			return dl;
		}

		public SwfDisplayList Visit(DefineSpriteTag tag, SwfDisplayList dl) {
			AddSpriteToLibrary(
				tag.SpriteId,
				tag.ControlTags);
			return dl;
		}

		public SwfDisplayList Visit(FileAttributesTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(EnableTelemetryTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(DefineBinaryDataTag tag, SwfDisplayList dl) {
			return dl;
		}

		public SwfDisplayList Visit(UnknownTag tag, SwfDisplayList dl) {
			TagToWarningLog(tag);
			return dl;
		}

		public SwfDisplayList Visit(UnsupportedTag tag, SwfDisplayList dl) {
			TagToWarningLog(tag);
			return dl;
		}

		//
		//
		//

		void TagToWarningLog(SwfTagBase tag) {
			if ( WarningLog != null ) {
				WarningLog(string.Format("{0}", tag));
			}
		}

		void AddShapesToLibrary(ushort define_id, SwfShapesWithStyle shapes) {
			var bitmap_styles = shapes.FillStyles.Where(p => p.Type.IsBitmapType);
			var define = new SwfLibraryShapeDefine{
				Bitmaps  = bitmap_styles.Select(p => p.BitmapId    ).ToArray(),
				Matrices = bitmap_styles.Select(p => p.BitmapMatrix).ToArray()
			};
			Library.Defines.Add(define_id, define);
		}

		void AddBitmapToLibrary(ushort define_id, int width, int height, byte[] argb32) {
			var duplicated = FindDuplicatedBitmap(argb32);
			var define = new SwfLibraryBitmapDefine{
				Width    = width,
				Height   = height,
				ARGB32   = duplicated > 0 ? new byte[0] : argb32,
				Redirect = duplicated};
			Library.Defines.Add(define_id, define);
		}

		void AddSpriteToLibrary(ushort define_id, SwfControlTags control_tags) {
			var define = new SwfLibrarySpriteDefine{
				ControlTags = control_tags
			};
			Library.Defines.Add(define_id, define);
		}

		ushort FindDuplicatedBitmap(byte[] argb32) {
			foreach ( var define in Library.Defines ) {
				var bitmap = define.Value as SwfLibraryBitmapDefine;
				if ( bitmap != null && bitmap.ARGB32.Length == argb32.Length ) {
					if ( bitmap.ARGB32.SequenceEqual(argb32) ) {
						return define.Key;
					}
				}
			}
			return 0;
		}

		bool IsSpriteTimelineEnd(SwfDisplaySpriteInstance sprite) {
			var sprite_def = Library.FindDefine<SwfLibrarySpriteDefine>(sprite.Id);
			if ( sprite_def != null && sprite.CurrentTag < sprite_def.ControlTags.Tags.Count ) {
				return false;
			}
			var children = sprite.DisplayList.Instances.Values
				.Where (p => p.Type == SwfDisplayInstanceType.Sprite)
				.Select(p => p as SwfDisplaySpriteInstance);
			foreach ( var child in children ) {
				if ( !IsSpriteTimelineEnd(child) ) {
					return false;
				}
			}
			return true;
		}

		void ChildrenNextFrameLooped(SwfDisplayList dl) {
			var sprites = dl.Instances.Values
				.Where (p => p.Type == SwfDisplayInstanceType.Sprite)
				.Select(p => p as SwfDisplaySpriteInstance);
			foreach ( var sprite in sprites ) {
				var sprite_def = Library.FindDefine<SwfLibrarySpriteDefine>(sprite.Id);
				if ( sprite_def != null ) {
					if ( IsSpriteTimelineEnd(sprite) ) {
						sprite.Reset();
					}
					var sprite_executer = new SwfContextExecuter(Library, sprite.CurrentTag, WarningLog);
					sprite_executer.NextFrame(sprite_def.ControlTags.Tags, sprite.DisplayList);
					sprite.CurrentTag = sprite_executer.CurrentTag;
				}
			}
		}
	}
}