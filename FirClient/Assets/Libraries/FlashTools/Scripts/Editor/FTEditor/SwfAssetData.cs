using UnityEngine;

using System.Collections.Generic;

namespace FTEditor {
	[System.Serializable]
	struct SwfVec2Data {
		public float x;
		public float y;

		public SwfVec2Data(float x, float y) {
			this.x = x;
			this.y = y;
		}

		public Vector2 ToUVector2() {
			return new Vector2(x, y);
		}

		public static SwfVec2Data one {
			get { return new SwfVec2Data(1.0f, 1.0f); }
		}

		public static SwfVec2Data zero {
			get { return new SwfVec2Data(0.0f, 0.0f); }
		}
	}

	[System.Serializable]
	struct SwfVec4Data {
		public float x;
		public float y;
		public float z;
		public float w;

		public SwfVec4Data(float x, float y, float z, float w) {
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public Vector4 ToUVector4() {
			return new Vector4(x, y, z, w);
		}

		public static SwfVec4Data one {
			get { return new SwfVec4Data(1.0f, 1.0f, 1.0f, 1.0f); }
		}

		public static SwfVec4Data zero {
			get { return new SwfVec4Data(0.0f, 0.0f, 0.0f, 0.0f); }
		}
	}

	[System.Serializable]
	struct SwfRectData {
		public float xMin;
		public float xMax;
		public float yMin;
		public float yMax;

		public SwfRectData(float w, float h) {
			this.xMin = 0;
			this.yMin = 0;
			this.xMax = w;
			this.yMax = h;
		}

		public SwfRectData(float x, float y, float w, float h) {
			this.xMin = x;
			this.yMin = y;
			this.xMax = x + w;
			this.yMax = y + h;
		}

		public float width {
			get {
				return xMax - xMin;
			}
		}

		public float height {
			get {
				return yMax - yMin;
			}
		}

		public float area {
			get {
				return width * height;
			}
		}

		public static SwfRectData identity {
			get {
				return new SwfRectData(0.0f, 0.0f, 0.0f, 0.0f);
			}
		}

		public static SwfRectData FromURect(Rect rect) {
			return new SwfRectData(rect.xMin, rect.yMin, rect.width, rect.height);
		}
	}

	[System.Serializable]
	struct SwfRectIntData {
		public int xMin;
		public int xMax;
		public int yMin;
		public int yMax;

		public SwfRectIntData(int w, int h) {
			this.xMin = 0;
			this.yMin = 0;
			this.xMax = w;
			this.yMax = h;
		}

		public SwfRectIntData(int x, int y, int w, int h) {
			this.xMin = x;
			this.yMin = y;
			this.xMax = x + w;
			this.yMax = y + h;
		}

		public int width {
			get {
				return xMax - xMin;
			}
		}

		public int height {
			get {
				return yMax - yMin;
			}
		}

		public int area {
			get {
				return width * height;
			}
		}

		public static SwfRectIntData identity {
			get {
				return new SwfRectIntData(0, 0, 0, 0);
			}
		}

		public static SwfRectIntData FromURect(RectInt rect) {
			return new SwfRectIntData(rect.xMin, rect.yMin, rect.width, rect.height);
		}
	}

	[System.Serializable]
	struct SwfMatrixData {
		public SwfVec2Data sc;
		public SwfVec2Data sk;
		public SwfVec2Data tr;

		public static SwfMatrixData identity {
			get {
				return new SwfMatrixData{
					sc = SwfVec2Data.one,
					sk = SwfVec2Data.zero,
					tr = SwfVec2Data.zero};
			}
		}

		public Matrix4x4 ToUMatrix() {
			var mat = Matrix4x4.identity;
			mat.m00 = sc.x;
			mat.m11 = sc.y;
			mat.m10 = sk.x;
			mat.m01 = sk.y;
			mat.m03 = tr.x;
			mat.m13 = tr.y;
			return mat;
		}

		public static SwfMatrixData FromUMatrix(Matrix4x4 mat) {
			return new SwfMatrixData{
				sc = new SwfVec2Data(mat.m00, mat.m11),
				sk = new SwfVec2Data(mat.m10, mat.m01),
				tr = new SwfVec2Data(mat.m03, mat.m13)};
		}
	}

	[System.Serializable]
	struct SwfBlendModeData {
		public enum Types : byte {
			Normal,
			Layer,
			Multiply,
			Screen,
			Lighten,
			Darken,     // GrabPass
			Difference, // GrabPass
			Add,
			Subtract,
			Invert,     // GrabPass
			Overlay,    // GrabPass
			Hardlight   // GrabPass
		}
		public Types type;

		public SwfBlendModeData(Types type) {
			this.type = type;
		}

		public static SwfBlendModeData identity {
			get {
				return new SwfBlendModeData{
					type = Types.Normal};
			}
		}

		public static SwfBlendModeData operator*(
			SwfBlendModeData a, SwfBlendModeData b)
		{
			return (a.type == Types.Normal || a.type == Types.Layer) ? b : a;
		}
	}

	[System.Serializable]
	struct SwfColorTransData {
		public SwfVec4Data mulColor;
		public SwfVec4Data addColor;

		public static SwfColorTransData identity {
			get {
				return new SwfColorTransData{
					mulColor = SwfVec4Data.one,
					addColor = SwfVec4Data.zero};
			}
		}

		public static SwfColorTransData operator*(
			SwfColorTransData a, SwfColorTransData b)
		{
			return new SwfColorTransData{
				mulColor = new SwfVec4Data(
					b.mulColor.x * a.mulColor.x,
					b.mulColor.y * a.mulColor.y,
					b.mulColor.z * a.mulColor.z,
					b.mulColor.w * a.mulColor.w),
				addColor = new SwfVec4Data(
					b.addColor.x * a.mulColor.x + a.addColor.x,
					b.addColor.y * a.mulColor.y + a.addColor.y,
					b.addColor.z * a.mulColor.z + a.addColor.z,
					b.addColor.w * a.mulColor.w + a.addColor.w)};
		}
	}

	[System.Serializable]
	class SwfInstanceData {
		public enum Types {
			Mask,
			Group,
			Masked,
			MaskReset
		}
		public Types                 Type        = Types.Group;
		public ushort                ClipDepth   = 0;
		public ushort                Bitmap      = 0;
		public SwfMatrixData         Matrix      = SwfMatrixData.identity;
		public SwfBlendModeData      BlendMode   = SwfBlendModeData.identity;
		public SwfColorTransData     ColorTrans  = SwfColorTransData.identity;
	}

	[System.Serializable]
	class SwfFrameData {
		public string                Anchor      = string.Empty;
		public List<string>          Labels      = new List<string>();
		public List<SwfInstanceData> Instances   = new List<SwfInstanceData>();
	}

	[System.Serializable]
	class SwfSymbolData {
		public string                Name        = string.Empty;
		public List<SwfFrameData>    Frames      = new List<SwfFrameData>();
	}

	[System.Serializable]
	class SwfBitmapData {
		public ushort                Id          = 0;
		public byte[]                ARGB32      = new byte[0];
		public ushort                Redirect    = 0;
		public int                   RealWidth   = 0;
		public int                   RealHeight  = 0;
		public SwfRectData           SourceRect  = SwfRectData.identity;
		public SwfRectIntData        TrimmedRect = SwfRectIntData.identity;
	}

	[System.Serializable]
	class SwfAssetData {
		public float                 FrameRate   = 0.0f;
		public List<SwfSymbolData>   Symbols     = new List<SwfSymbolData>();
		public List<SwfBitmapData>   Bitmaps     = new List<SwfBitmapData>();
	}
}