using System.Text;
using FTSwfTools.SwfTypes;

namespace FTSwfTools.SwfTags {
	public class PlaceObject3Tag : SwfTagBase {
		public bool              HasClipActions;
		public bool              HasClipDepth;
		public bool              HasName;
		public bool              HasRatio;
		public bool              HasColorTransform;
		public bool              HasMatrix;
		public bool              HasCharacter;
		public bool              Move;
		public bool              OpaqueBackground;
		public bool              HasVisible;
		public bool              HasImage;
		public bool              HasClassName;
		public bool              HasCacheAsBitmap;
		public bool              HasBlendMode;
		public bool              HasFilterList;
		public ushort            Depth;
		public string            ClassName;
		public ushort            CharacterId;
		public SwfMatrix         Matrix;
		public SwfColorTransform ColorTransform;
		public ushort            Ratio;
		public string            Name;
		public ushort            ClipDepth;
		public SwfSurfaceFilters SurfaceFilters;
		public SwfBlendMode      BlendMode;
		public bool              BitmapCache;
		public bool              Visible;
		public SwfColor          BackgroundColor;
		public SwfClipActions    ClipActions;

		public override SwfTagType TagType {
			get { return SwfTagType.PlaceObject3; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			var sb = new StringBuilder(1024);
			sb.Append("PlaceObject3Tag. ");
			sb.AppendFormat("Move: {0} Depth: {1}", Move, Depth);
			if ( HasCharacter ) {
				sb.AppendFormat(", CharacterId: {0}", CharacterId);
			}
			if ( HasMatrix ) {
				sb.AppendFormat(", Matrix: {0}", Matrix);
			}
			if ( HasColorTransform ) {
				sb.AppendFormat(", ColorTransform: {0}", ColorTransform);
			}
			if ( HasRatio ) {
				sb.AppendFormat(", Ratio: {0}", Ratio);
			}
			if ( HasName ) {
				sb.AppendFormat(", Name: {0}", Name);
			}
			if ( HasClipDepth ) {
				sb.AppendFormat(", ClipDepth: {0}", ClipDepth);
			}
			if ( HasFilterList ) {
				sb.AppendFormat(", SurfaceFilters: {0}", SurfaceFilters);
			}
			if ( HasBlendMode ) {
				sb.AppendFormat(", BlendMode: {0}", BlendMode);
			}
			if ( HasCacheAsBitmap ) {
				sb.AppendFormat(", BitmapCache: {0}", BitmapCache);
			}
			if ( HasVisible ) {
				sb.AppendFormat(", Visible: {0}", Visible);
			}
			if ( HasClipActions ) {
				sb.AppendFormat(", ClipActions: {0}", ClipActions);
			}
			return sb.ToString();
		}

		public static PlaceObject3Tag Create(SwfStreamReader reader) {
			var tag               = new PlaceObject3Tag();
			tag.HasClipActions    = reader.ReadBit();
			tag.HasClipDepth      = reader.ReadBit();
			tag.HasName           = reader.ReadBit();
			tag.HasRatio          = reader.ReadBit();
			tag.HasColorTransform = reader.ReadBit();
			tag.HasMatrix         = reader.ReadBit();
			tag.HasCharacter      = reader.ReadBit();
			tag.Move              = reader.ReadBit();
			reader.ReadBit(); // reserved
			tag.OpaqueBackground  = reader.ReadBit();
			tag.HasVisible        = reader.ReadBit();
			tag.HasImage          = reader.ReadBit();
			tag.HasClassName      = reader.ReadBit();
			tag.HasCacheAsBitmap  = reader.ReadBit();
			tag.HasBlendMode      = reader.ReadBit();
			tag.HasFilterList     = reader.ReadBit();
			tag.Depth             = reader.ReadUInt16();

			tag.ClassName         = tag.HasClassName
				? reader.ReadString()
				: string.Empty;

			tag.CharacterId       = tag.HasCharacter
				? reader.ReadUInt16()
				: (ushort)0;

			tag.Matrix            = tag.HasMatrix
				? SwfMatrix.Read(reader)
				: SwfMatrix.identity;

			tag.ColorTransform    = tag.HasColorTransform
				? SwfColorTransform.Read(reader, true)
				: SwfColorTransform.identity;

			tag.Ratio             = tag.HasRatio
				? reader.ReadUInt16()
				: (ushort)0;

			tag.Name              = tag.HasName
				? reader.ReadString()
				: string.Empty;

			tag.ClipDepth         = tag.HasClipDepth
				? reader.ReadUInt16()
				: (ushort)0;

			tag.SurfaceFilters    = tag.HasFilterList
				? SwfSurfaceFilters.Read(reader)
				: SwfSurfaceFilters.identity;

			tag.BlendMode         = tag.HasBlendMode
				? SwfBlendMode.Read(reader)
				: SwfBlendMode.identity;

			tag.BitmapCache       = tag.HasCacheAsBitmap
				? (0 != reader.ReadByte())
				: false;

			tag.Visible           = tag.HasVisible && !reader.IsEOF
				? (0 != reader.ReadByte())
				: true;

			tag.BackgroundColor   = tag.HasVisible && !reader.IsEOF
				? SwfColor.Read(reader, true)
				: SwfColor.identity;

			tag.ClipActions       = tag.HasClipActions && !reader.IsEOF
				? SwfClipActions.Read(reader)
				: SwfClipActions.identity;

			return tag;
		}
	}
}