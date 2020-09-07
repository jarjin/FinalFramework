namespace FTSwfTools.SwfTags {
	public class RemoveObject2Tag : SwfTagBase {
		public ushort Depth;

		public override SwfTagType TagType {
			get { return SwfTagType.RemoveObject2; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"RemoveObject2Tag. " +
				"Depth: {0}",
				Depth);
		}

		public static RemoveObject2Tag Create(SwfStreamReader reader) {
			return new RemoveObject2Tag{
				Depth = reader.ReadUInt16()};
		}
	}
}