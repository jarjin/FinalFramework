using FTSwfTools.SwfTypes;

namespace FTSwfTools.SwfTags {
	public class PlaceObjectTag : SwfTagBase {
		public ushort            CharacterId;
		public ushort            Depth;
		public SwfMatrix         Matrix;
		public SwfColorTransform ColorTransform;

		public override SwfTagType TagType {
			get { return SwfTagType.PlaceObject; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"PlaceObjectTag. " +
				"CharacterId: {0}, Depth: {1}, Matrix: {2}, ColorTransform: {3}",
				CharacterId, Depth, Matrix, ColorTransform);
		}

		public static PlaceObjectTag Create(SwfStreamReader reader) {
			var tag            = new PlaceObjectTag();
			tag.CharacterId    = reader.ReadUInt16();
			tag.Depth          = reader.ReadUInt16();
			tag.Matrix         = SwfMatrix.Read(reader);
			tag.ColorTransform = reader.IsEOF
				? SwfColorTransform.identity
				: SwfColorTransform.Read(reader, false);
			return tag;
		}
	}
}