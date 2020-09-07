namespace FTSwfTools.SwfTags {
	public class EnableTelemetryTag : SwfTagBase {
		public byte[] SHA256PasswordHash;

		public override SwfTagType TagType {
			get { return SwfTagType.EnableTelemetry; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"EnableTelemetryTag. " +
				"SHA256PasswordHash: {0}",
				SHA256PasswordHash.Length > 0);
		}

		public static EnableTelemetryTag Create(SwfStreamReader reader) {
			reader.ReadUInt16(); // reserved
			var sha256 = reader.IsEOF
				? new byte[0]
				: reader.ReadBytes(32);
			return new EnableTelemetryTag{
				SHA256PasswordHash = sha256};
		}
	}
}