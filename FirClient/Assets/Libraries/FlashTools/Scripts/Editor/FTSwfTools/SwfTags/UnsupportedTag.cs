namespace FTSwfTools.SwfTags {
	public class UnsupportedTag : SwfTagBase {
		SwfTagType _tagType;

		public override SwfTagType TagType {
			get { return _tagType; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"UnsupportedTag. " +
				"TagType: {0}",
				TagType);
		}

		public static UnsupportedTag Create(SwfTagType tag_type) {
			return new UnsupportedTag{
				_tagType = tag_type};
		}
	}
}