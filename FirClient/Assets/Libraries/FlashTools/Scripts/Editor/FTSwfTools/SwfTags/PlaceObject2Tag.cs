using System.Text;
using FTSwfTools.SwfTypes;

namespace FTSwfTools.SwfTags {
	public class PlaceObject2Tag : SwfTagBase {
		public bool              HasClipActions;
		public bool              HasClipDepth;
		public bool              HasName;
		public bool              HasRatio;
		public bool              HasColorTransform;
		public bool              HasMatrix;
		public bool              HasCharacter;
		public bool              Move;
		public ushort            Depth;
		public ushort            CharacterId;
		public SwfMatrix         Matrix;
		public SwfColorTransform ColorTransform;
		public ushort            Ratio;
		public string            Name;
		public ushort            ClipDepth;
		public SwfClipActions    ClipActions;

		public override SwfTagType TagType {
			get { return SwfTagType.PlaceObject2; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			var sb = new StringBuilder(1024);
			sb.Append("PlaceObject2Tag. ");
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
			if ( HasClipActions ) {
				sb.AppendFormat(", ClipActions: {0}", ClipActions);
			}
			return sb.ToString();
		}

		public static PlaceObject2Tag Create(SwfStreamReader reader) {
			var tag               = new PlaceObject2Tag();
			tag.HasClipActions    = reader.ReadBit();
			tag.HasClipDepth      = reader.ReadBit();
			tag.HasName           = reader.ReadBit();
			tag.HasRatio          = reader.ReadBit();
			tag.HasColorTransform = reader.ReadBit();
			tag.HasMatrix         = reader.ReadBit();
			tag.HasCharacter      = reader.ReadBit();
			tag.Move              = reader.ReadBit();
			tag.Depth             = reader.ReadUInt16();

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

			tag.ClipActions       = tag.HasClipActions
				? SwfClipActions.Read(reader)
				: SwfClipActions.identity;

			return tag;
		}
	}
}