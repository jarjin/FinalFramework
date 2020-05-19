namespace FTSwfTools.SwfTags {
	public class EnableDebugger2Tag : SwfTagBase {
		public string MD5PasswordHash;

		public override SwfTagType TagType {
			get { return SwfTagType.EnableDebugger2; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"EnableDebugger2Tag. " +
				"MD5PasswordHash: {0}",
				MD5PasswordHash.Length > 0);
		}

		public static EnableDebugger2Tag Create(SwfStreamReader reader) {
			reader.ReadUInt16(); // reserved
			var md5 = reader.IsEOF
				? string.Empty
				: reader.ReadString();
			return new EnableDebugger2Tag{
				MD5PasswordHash = md5};
		}
	}
}