using FTSwfTools.SwfTypes;

namespace FTSwfTools.SwfTags {
	public class DoABCTag : SwfTagBase {
		public bool   ExecuteImmediately;
		public string Name;
		public byte[] ABCBytes;

		public override SwfTagType TagType {
			get { return SwfTagType.DoABC; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return "DoABCTag.";
		}

		public static DoABCTag Create(SwfStreamReader reader) {
			const int kDoAbcLazyInitializeFlag = 1;
			var flags     = reader.ReadUInt32();
			var name      = reader.ReadString();
			var abc_bytes = reader.ReadRest();
			return new DoABCTag{
				ExecuteImmediately = (flags & kDoAbcLazyInitializeFlag) == 0,
				Name               = name,
				ABCBytes           = abc_bytes};
		}
	}
}