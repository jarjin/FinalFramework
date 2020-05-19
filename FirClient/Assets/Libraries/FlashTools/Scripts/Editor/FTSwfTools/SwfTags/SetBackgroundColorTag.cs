using FTSwfTools.SwfTypes;

namespace FTSwfTools.SwfTags {
	public class SetBackgroundColorTag : SwfTagBase {
		public SwfColor BackgroundColor;

		public override SwfTagType TagType {
			get { return SwfTagType.SetBackgroundColor; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"SetBackgroundColorTag. " +
				"BackgroundColor: {0}",
				BackgroundColor);
		}

		public static SetBackgroundColorTag Create(SwfStreamReader reader) {
			return new SetBackgroundColorTag{
				BackgroundColor = SwfColor.Read(reader, false)};
		}
	}
}