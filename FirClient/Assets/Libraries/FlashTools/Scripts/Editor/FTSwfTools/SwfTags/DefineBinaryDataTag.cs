namespace FTSwfTools.SwfTags {
	public class DefineBinaryDataTag : SwfTagBase {
		public ushort Tag;
		public byte[] Data;

		public override SwfTagType TagType {
			get { return SwfTagType.DefineBinaryData; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return "DefineBinaryDataTag.";
		}

		public static DefineBinaryDataTag Create(SwfStreamReader reader) {
			var tag = reader.ReadUInt16();
			reader.ReadUInt32(); // reserved
			var data = reader.ReadRest();
			return new DefineBinaryDataTag{
				Tag  = tag,
				Data = data};
		}
	}
}