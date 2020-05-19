using FTSwfTools.SwfTypes;

namespace FTSwfTools.SwfTags {
	public class DefineSpriteTag : SwfTagBase {
		public ushort         SpriteId;
		public ushort         FrameCount;
		public SwfControlTags ControlTags;

		public override SwfTagType TagType {
			get { return SwfTagType.DefineSprite; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"DefineSpriteTag. " +
				"SpriteId: {0}, FrameCount: {1}, ControlTags: {2}",
				SpriteId, FrameCount, ControlTags.Tags.Count);
		}

		public static DefineSpriteTag Create(SwfStreamReader reader) {
			var tag         = new DefineSpriteTag();
			tag.SpriteId    = reader.ReadUInt16();
			tag.FrameCount  = reader.ReadUInt16();
			tag.ControlTags = SwfControlTags.Read(reader);
			return tag;
		}
	}
}