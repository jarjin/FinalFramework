using System.Collections.Generic;
using FTSwfTools.SwfTypes;

namespace FTSwfTools {

	using LibraryDefines   = SortedDictionary<ushort, SwfLibraryDefine>;
	using DisplayInstances = SortedDictionary<ushort, SwfDisplayInstance>;

	//
	// SwfLibrary
	//

	public enum SwfLibraryDefineType {
		Shape,
		Bitmap,
		Sprite
	}

	public abstract class SwfLibraryDefine {
		public string ExportName = string.Empty;
		public abstract SwfLibraryDefineType Type { get; }
	}

	public class SwfLibraryShapeDefine : SwfLibraryDefine {
		public ushort[]    Bitmaps  = new ushort[0];
		public SwfMatrix[] Matrices = new SwfMatrix[0];

		public override SwfLibraryDefineType Type {
			get { return SwfLibraryDefineType.Shape; }
		}
	}

	public class SwfLibraryBitmapDefine : SwfLibraryDefine {
		public int    Width    = 0;
		public int    Height   = 0;
		public byte[] ARGB32   = new byte[0];
		public ushort Redirect = 0;

		public override SwfLibraryDefineType Type {
			get { return SwfLibraryDefineType.Bitmap; }
		}
	}

	public class SwfLibrarySpriteDefine : SwfLibraryDefine {
		public SwfControlTags ControlTags = SwfControlTags.identity;

		public override SwfLibraryDefineType Type {
			get { return SwfLibraryDefineType.Sprite; }
		}
	}

	public class SwfLibrary {
		public LibraryDefines Defines = new LibraryDefines();

		public bool HasDefine<T>(ushort define_id) where T : SwfLibraryDefine {
			return FindDefine<T>(define_id) != null;
		}

		public T FindDefine<T>(ushort define_id) where T : SwfLibraryDefine {
			SwfLibraryDefine def;
			if ( Defines.TryGetValue(define_id, out def) ) {
				return def as T;
			}
			return null;
		}
	}

	//
	// SwfDisplayList
	//

	public enum SwfDisplayInstanceType {
		Shape,
		Bitmap,
		Sprite
	}

	public abstract class SwfDisplayInstance {
		public abstract SwfDisplayInstanceType Type { get; }

		public ushort            Id;
		public ushort            Depth;
		public ushort            ClipDepth;
		public bool              Visible;
		public SwfMatrix         Matrix;
		public SwfBlendMode      BlendMode;
		public SwfSurfaceFilters FilterList;
		public SwfColorTransform ColorTransform;
	}

	public class SwfDisplayShapeInstance : SwfDisplayInstance {
		public override SwfDisplayInstanceType Type {
			get { return SwfDisplayInstanceType.Shape; }
		}
	}

	public class SwfDisplayBitmapInstance : SwfDisplayInstance {
		public override SwfDisplayInstanceType Type {
			get { return SwfDisplayInstanceType.Bitmap; }
		}
	}

	public class SwfDisplaySpriteInstance : SwfDisplayInstance {
		public int            CurrentTag  = 0;
		public SwfDisplayList DisplayList = new SwfDisplayList();

		public override SwfDisplayInstanceType Type {
			get { return SwfDisplayInstanceType.Sprite; }
		}

		public void Reset() {
			CurrentTag  = 0;
			DisplayList = new SwfDisplayList();
		}
	}

	public class SwfDisplayList {
		public DisplayInstances Instances    = new DisplayInstances();
		public List<string>     FrameLabels  = new List<string>();
		public List<string>     FrameAnchors = new List<string>();
	}
}