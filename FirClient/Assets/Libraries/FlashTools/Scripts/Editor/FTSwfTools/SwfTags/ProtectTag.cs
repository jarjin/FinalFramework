namespace FTSwfTools.SwfTags {
	public class ProtectTag : SwfTagBase {
		public string MD5Password;

		public override SwfTagType TagType {
			get { return SwfTagType.Protect; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"ProtectTag. " +
				"MD5Password: {0}",
				MD5Password);
		}

		public static ProtectTag Create(SwfStreamReader reader) {
			var md5_password = reader.IsEOF
				? string.Empty
				: reader.ReadString();
			return new ProtectTag{
				MD5Password = md5_password};
		}
	}
}