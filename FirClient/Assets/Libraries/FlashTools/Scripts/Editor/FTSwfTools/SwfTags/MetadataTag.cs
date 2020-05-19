namespace FTSwfTools.SwfTags {
	public class MetadataTag : SwfTagBase {
		public string Metadata;

		public override SwfTagType TagType {
			get { return SwfTagType.Metadata; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"MetadataTag." + 
				"Metadata: {0}",
				Metadata.Length);
		}

		public static MetadataTag Create(SwfStreamReader reader) {
			return new MetadataTag{
				Metadata = reader.ReadString()};
		}
	}
}