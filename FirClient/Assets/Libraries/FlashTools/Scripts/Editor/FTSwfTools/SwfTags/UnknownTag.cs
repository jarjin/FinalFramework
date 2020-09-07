namespace FTSwfTools.SwfTags {
	public class UnknownTag : SwfTagBase {
		public int _tagId;

		public int TagId {
			get { return _tagId; }
		}

		public override SwfTagType TagType {
			get { return SwfTagType.Unknown; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"UnknownTag. " +
				"TagId: {0}",
				TagId);
		}

		public static UnknownTag Create(int tag_id) {
			return new UnknownTag{
				_tagId = tag_id};
		}
	}
}