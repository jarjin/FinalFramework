using UnityEngine;
using FTRuntime.Internal;

namespace FTRuntime {
	[System.Serializable]
	public struct SwfSettingsData {
		public enum AtlasFilter {
			Point,
			Bilinear,
			Trilinear
		}

		public enum AtlasFormat {
			AutomaticCompressed = 0,
			AutomaticTruecolor  = 2
		}

		[SwfPowerOfTwoIf(5, 13, "AtlasPowerOfTwo")]
		public int         MaxAtlasSize;
		[SwfIntRange(0, int.MaxValue)]
		public int         AtlasPadding;
		[SwfFloatRange(float.Epsilon, float.MaxValue)]
		public float       PixelsPerUnit;
		public bool        BitmapTrimming;
		public bool        GenerateMipMaps;
		public bool        AtlasPowerOfTwo;
		public bool        AtlasForceSquare;
		public AtlasFilter AtlasTextureFilter;
		public AtlasFormat AtlasTextureFormat;

		public static SwfSettingsData identity {
			get {
				return new SwfSettingsData{
					MaxAtlasSize       = 2048,
					AtlasPadding       = 1,
					PixelsPerUnit      = 100.0f,
					BitmapTrimming     = true,
					GenerateMipMaps    = false,
					AtlasPowerOfTwo    = true,
					AtlasForceSquare   = true,
					AtlasTextureFilter = AtlasFilter.Bilinear,
					AtlasTextureFormat = AtlasFormat.AutomaticTruecolor};
			}
		}

		public bool CheckEquals(SwfSettingsData other) {
			return
				MaxAtlasSize       == other.MaxAtlasSize &&
				AtlasPadding       == other.AtlasPadding &&
				Mathf.Approximately(PixelsPerUnit, other.PixelsPerUnit) &&
				BitmapTrimming     == other.BitmapTrimming &&
				GenerateMipMaps    == other.GenerateMipMaps &&
				AtlasPowerOfTwo    == other.AtlasPowerOfTwo &&
				AtlasForceSquare   == other.AtlasForceSquare &&
				AtlasTextureFilter == other.AtlasTextureFilter &&
				AtlasTextureFormat == other.AtlasTextureFormat;
		}
	}

	public class SwfSettings : ScriptableObject {
		public SwfSettingsData Settings;

		void Reset() {
			Settings = SwfSettingsData.identity;
		}
	}
}