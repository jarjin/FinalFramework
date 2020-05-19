namespace FTSwfTools.SwfTags {
	public class FileAttributesTag : SwfTagBase {
		public override SwfTagType TagType {
			get { return SwfTagType.FileAttributes; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return "FileAttributesTag.";
		}

		public static FileAttributesTag Create(SwfStreamReader reader) {
			return new FileAttributesTag();
		}
	}
}