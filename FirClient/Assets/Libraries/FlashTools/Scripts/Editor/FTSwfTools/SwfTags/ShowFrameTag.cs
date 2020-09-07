namespace FTSwfTools.SwfTags {
	public class ShowFrameTag : SwfTagBase {
		public override SwfTagType TagType {
			get { return SwfTagType.ShowFrame; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return "ShowFrameTag.";
		}

		public static ShowFrameTag Create(SwfStreamReader reader) {
			return new ShowFrameTag();
		}
	}
}