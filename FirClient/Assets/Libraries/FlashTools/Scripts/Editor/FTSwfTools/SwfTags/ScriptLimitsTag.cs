namespace FTSwfTools.SwfTags {
	public class ScriptLimitsTag : SwfTagBase {
		public ushort MaxRecursionDepth;
		public ushort ScriptTimeoutSeconds;

		public override SwfTagType TagType {
			get { return SwfTagType.ScriptLimits; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"ScriptLimitsTag. " +
				"MaxRecursionDepth: {0}, ScriptTimeoutSeconds: {1}",
				MaxRecursionDepth, ScriptTimeoutSeconds);
		}

		public static ScriptLimitsTag Create(SwfStreamReader reader) {
			return new ScriptLimitsTag{
				MaxRecursionDepth    = reader.ReadUInt16(),
				ScriptTimeoutSeconds = reader.ReadUInt16()};
		}
	}
}