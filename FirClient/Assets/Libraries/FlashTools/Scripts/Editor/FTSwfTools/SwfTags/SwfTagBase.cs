namespace FTSwfTools.SwfTags {
	public enum SwfTagType {
		// -----------------------------
		// Display list
		// -----------------------------

		PlaceObject   = 4,
		PlaceObject2  = 26,
		PlaceObject3  = 70,
		RemoveObject  = 5,
		RemoveObject2 = 28,
		ShowFrame     = 1,

		// -----------------------------
		// Control
		// -----------------------------

		SetBackgroundColor           = 9,
		FrameLabel                   = 43,
		Protect                      = 24,
		End                          = 0,
		ExportAssets                 = 56,
		ImportAssets                 = 57, // Unsupported
		EnableDebugger               = 58,
		EnableDebugger2              = 64,
		ScriptLimits                 = 65,
		SetTabIndex                  = 66, // Unsupported
		ImportAssets2                = 71, // Unsupported
		SymbolClass                  = 76,
		Metadata                     = 77,
		DefineScalingGrid            = 78, // Unsupported
		DefineSceneAndFrameLabelData = 86,

		// -----------------------------
		// Actions
		// -----------------------------

		DoAction     = 12, // Unsupported
		DoInitAction = 59, // Unsupported
		DoABC        = 82,

		// -----------------------------
		// Shape
		// -----------------------------

		DefineShape  = 2,
		DefineShape2 = 22,
		DefineShape3 = 32,
		DefineShape4 = 83,

		// -----------------------------
		// Bitmaps
		// -----------------------------

		DefineBits          = 6,  // Unsupported
		JPEGTables          = 8,  // Unsupported
		DefineBitsJPEG2     = 21, // Unsupported
		DefineBitsJPEG3     = 35, // Unsupported
		DefineBitsLossless  = 20,
		DefineBitsLossless2 = 36,
		DefineBitsJPEG4     = 90, // Unsupported

		// -----------------------------
		// Shape Morphing
		// -----------------------------

		DefineMorphShape  = 46, // Unsupported
		DefineMorphShape2 = 84, // Unsupported

		// -----------------------------
		// Fonts and Text
		// -----------------------------

		DefineFont = 10,           // Unsupported
		DefineFontInfo = 13,       // Unsupported
		DefineFontInfo2 = 62,      // Unsupported
		DefineFont2 = 48,          // Unsupported
		DefineFont3 = 75,          // Unsupported
		DefineFontAlignZones = 73, // Unsupported
		DefineFontName = 88,       // Unsupported
		DefineText = 11,           // Unsupported
		DefineText2 = 33,          // Unsupported
		DefineEditText = 37,       // Unsupported
		CSMTextSettings = 74,      // Unsupported
		DefineFont4 = 91,          // Unsupported

		// -----------------------------
		// Sounds
		// -----------------------------

		DefineSound = 14,      // Unsupported
		StartSound = 15,       // Unsupported
		StartSound2 = 89,      // Unsupported
		SoundStreamHead = 18,  // Unsupported
		SoundStreamHead2 = 45, // Unsupported
		SoundStreamBlock = 19, // Unsupported

		// -----------------------------
		// Buttons
		// -----------------------------

		DefineButton = 7,        // Unsupported
		DefineButton2 = 34,      // Unsupported
		DefineButtonCxform = 23, // Unsupported
		DefineButtonSound = 17,  // Unsupported

		// -----------------------------
		// Sprites and Movie Clips
		// -----------------------------

		DefineSprite = 39,

		// -----------------------------
		// Video
		// -----------------------------

		DefineVideoStream = 60, // Unsupported
		VideoFrame        = 61, // Unsupported

		// -----------------------------
		// Metadata
		// -----------------------------

		FileAttributes   = 69,
		EnableTelemetry  = 93,
		DefineBinaryData = 87,

		// -----------------------------
		// Unknown
		// -----------------------------

		Unknown
	}

	public abstract class SwfTagBase {
		struct SwfTagData {
			public int    TagId;
			public byte[] TagData;
		}

		public abstract SwfTagType TagType { get; }
		public abstract TResult AcceptVistor<TArg, TResult>(
			SwfTagVisitor<TArg, TResult> visitor, TArg arg);

		public static SwfTagBase Read(SwfStreamReader reader) {
			var type_and_size = reader.ReadUInt16();
			var tag_id        = type_and_size >> 6;
			var short_size    = type_and_size & 0x3f;
			var size          = short_size < 0x3f ? (uint)short_size : reader.ReadUInt32();
			var tag_data      = reader.ReadBytes(size);
			return Create(new SwfTagData{
				TagId   = tag_id,
				TagData = tag_data});
		}

		static SwfTagBase Create(SwfTagData tag_data) {
			var reader = new SwfStreamReader(tag_data.TagData);
			switch ( tag_data.TagId ) {
			// Display list
			case (int)SwfTagType.PlaceObject:                  return PlaceObjectTag.Create(reader);
			case (int)SwfTagType.PlaceObject2:                 return PlaceObject2Tag.Create(reader);
			case (int)SwfTagType.PlaceObject3:                 return PlaceObject3Tag.Create(reader);
			case (int)SwfTagType.RemoveObject:                 return RemoveObjectTag.Create(reader);
			case (int)SwfTagType.RemoveObject2:                return RemoveObject2Tag.Create(reader);
			case (int)SwfTagType.ShowFrame:                    return ShowFrameTag.Create(reader);
			// Control
			case (int)SwfTagType.SetBackgroundColor:           return SetBackgroundColorTag.Create(reader);
			case (int)SwfTagType.FrameLabel:                   return FrameLabelTag.Create(reader);
			case (int)SwfTagType.Protect:                      return ProtectTag.Create(reader);
			case (int)SwfTagType.End:                          return EndTag.Create(reader);
			case (int)SwfTagType.ExportAssets:                 return ExportAssetsTag.Create(reader);
			case (int)SwfTagType.ImportAssets:                 return UnsupportedTag.Create(SwfTagType.ImportAssets);
			case (int)SwfTagType.EnableDebugger:               return EnableDebuggerTag.Create(reader);
			case (int)SwfTagType.EnableDebugger2:              return EnableDebugger2Tag.Create(reader);
			case (int)SwfTagType.ScriptLimits:                 return ScriptLimitsTag.Create(reader);
			case (int)SwfTagType.SetTabIndex:                  return UnsupportedTag.Create(SwfTagType.SetTabIndex);
			case (int)SwfTagType.ImportAssets2:                return UnsupportedTag.Create(SwfTagType.ImportAssets2);
			case (int)SwfTagType.SymbolClass:                  return SymbolClassTag.Create(reader);
			case (int)SwfTagType.Metadata:                     return MetadataTag.Create(reader);
			case (int)SwfTagType.DefineScalingGrid:            return UnsupportedTag.Create(SwfTagType.DefineScalingGrid);
			case (int)SwfTagType.DefineSceneAndFrameLabelData: return DefineSceneAndFrameLabelDataTag.Create(reader);
			// Actions
			case (int)SwfTagType.DoAction:                     return UnsupportedTag.Create(SwfTagType.DoAction);
			case (int)SwfTagType.DoInitAction:                 return UnsupportedTag.Create(SwfTagType.DoInitAction);
			case (int)SwfTagType.DoABC:                        return DoABCTag.Create(reader);
			// Shape
			case (int)SwfTagType.DefineShape:                  return DefineShapeTag.Create(reader);
			case (int)SwfTagType.DefineShape2:                 return DefineShape2Tag.Create(reader);
			case (int)SwfTagType.DefineShape3:                 return DefineShape3Tag.Create(reader);
			case (int)SwfTagType.DefineShape4:                 return DefineShape4Tag.Create(reader);
			// Bitmaps
			case (int)SwfTagType.DefineBits:                   return UnsupportedTag.Create(SwfTagType.DefineBits);
			case (int)SwfTagType.JPEGTables:                   return UnsupportedTag.Create(SwfTagType.JPEGTables);
			case (int)SwfTagType.DefineBitsJPEG2:              return UnsupportedTag.Create(SwfTagType.DefineBitsJPEG2);
			case (int)SwfTagType.DefineBitsJPEG3:              return UnsupportedTag.Create(SwfTagType.DefineBitsJPEG3);
			case (int)SwfTagType.DefineBitsLossless:           return DefineBitsLosslessTag.Create(reader);
			case (int)SwfTagType.DefineBitsLossless2:          return DefineBitsLossless2Tag.Create(reader);
			case (int)SwfTagType.DefineBitsJPEG4:              return UnsupportedTag.Create(SwfTagType.DefineBitsJPEG4);
			// Shape Morphing
			case (int)SwfTagType.DefineMorphShape:             return UnsupportedTag.Create(SwfTagType.DefineMorphShape);
			case (int)SwfTagType.DefineMorphShape2:            return UnsupportedTag.Create(SwfTagType.DefineMorphShape2);
			// Fonts and Text
			case (int)SwfTagType.DefineFont:                   return UnsupportedTag.Create(SwfTagType.DefineFont);
			case (int)SwfTagType.DefineFontInfo:               return UnsupportedTag.Create(SwfTagType.DefineFontInfo);
			case (int)SwfTagType.DefineFontInfo2:              return UnsupportedTag.Create(SwfTagType.DefineFontInfo2);
			case (int)SwfTagType.DefineFont2:                  return UnsupportedTag.Create(SwfTagType.DefineFont2);
			case (int)SwfTagType.DefineFont3:                  return UnsupportedTag.Create(SwfTagType.DefineFont3);
			case (int)SwfTagType.DefineFontAlignZones:         return UnsupportedTag.Create(SwfTagType.DefineFontAlignZones);
			case (int)SwfTagType.DefineFontName:               return UnsupportedTag.Create(SwfTagType.DefineFontName);
			case (int)SwfTagType.DefineText:                   return UnsupportedTag.Create(SwfTagType.DefineText);
			case (int)SwfTagType.DefineText2:                  return UnsupportedTag.Create(SwfTagType.DefineText2);
			case (int)SwfTagType.DefineEditText:               return UnsupportedTag.Create(SwfTagType.DefineEditText);
			case (int)SwfTagType.CSMTextSettings:              return UnsupportedTag.Create(SwfTagType.CSMTextSettings);
			case (int)SwfTagType.DefineFont4:                  return UnsupportedTag.Create(SwfTagType.DefineFont4);
			// Sounds
			case (int)SwfTagType.DefineSound:                  return UnsupportedTag.Create(SwfTagType.DefineSound);
			case (int)SwfTagType.StartSound:                   return UnsupportedTag.Create(SwfTagType.StartSound);
			case (int)SwfTagType.StartSound2:                  return UnsupportedTag.Create(SwfTagType.StartSound2);
			case (int)SwfTagType.SoundStreamHead:              return UnsupportedTag.Create(SwfTagType.SoundStreamHead);
			case (int)SwfTagType.SoundStreamHead2:             return UnsupportedTag.Create(SwfTagType.SoundStreamHead2);
			case (int)SwfTagType.SoundStreamBlock:             return UnsupportedTag.Create(SwfTagType.SoundStreamBlock);
			// Buttons
			case (int)SwfTagType.DefineButton:                 return UnsupportedTag.Create(SwfTagType.DefineButton);
			case (int)SwfTagType.DefineButton2:                return UnsupportedTag.Create(SwfTagType.DefineButton2);
			case (int)SwfTagType.DefineButtonCxform:           return UnsupportedTag.Create(SwfTagType.DefineButtonCxform);
			case (int)SwfTagType.DefineButtonSound:            return UnsupportedTag.Create(SwfTagType.DefineButtonSound);
			// Sprites and Movie Clips
			case (int)SwfTagType.DefineSprite:                 return DefineSpriteTag.Create(reader);
			// Video
			case (int)SwfTagType.DefineVideoStream:            return UnsupportedTag.Create(SwfTagType.DefineVideoStream);
			case (int)SwfTagType.VideoFrame:                   return UnsupportedTag.Create(SwfTagType.VideoFrame);
			// Metadata
			case (int)SwfTagType.FileAttributes:               return FileAttributesTag.Create(reader);
			case (int)SwfTagType.EnableTelemetry:              return EnableTelemetryTag.Create(reader);
			case (int)SwfTagType.DefineBinaryData:             return DefineBinaryDataTag.Create(reader);
			default:                                           return UnknownTag.Create(tag_data.TagId);
			}
		}
	}
}