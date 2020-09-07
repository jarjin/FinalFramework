namespace FTSwfTools.SwfTags {
	public class RemoveObjectTag : SwfTagBase {
		public ushort CharacterId;
		public ushort Depth;

		public override SwfTagType TagType {
			get { return SwfTagType.RemoveObject; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"RemoveObjectTag. " +
				"CharacterId: {0}, Depth: {1}",
				CharacterId, Depth);
		}

		public static RemoveObjectTag Create(SwfStreamReader reader) {
			return new RemoveObjectTag{
				CharacterId = reader.ReadUInt16(),
				Depth       = reader.ReadUInt16()};
		}
	}
}