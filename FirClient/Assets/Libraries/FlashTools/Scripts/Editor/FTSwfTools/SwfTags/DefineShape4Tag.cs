using FTSwfTools.SwfTypes;

namespace FTSwfTools.SwfTags {
	public class DefineShape4Tag : SwfTagBase {
		public ushort             ShapeId;
		public SwfRect            ShapeBounds;
		public SwfRect            EdgeBounds;
		public byte               Flags;
		public SwfShapesWithStyle Shapes;

		public override SwfTagType TagType {
			get { return SwfTagType.DefineShape4; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"DefineShape4Tag. " +
				"ShapeId: {0}, ShapeBounds: {1}, EdgeBounds: {2}, Flags: {3}, Shapes: {4}",
				ShapeId, ShapeBounds, EdgeBounds, Flags, Shapes);
		}

		public static DefineShape4Tag Create(SwfStreamReader reader) {
			var tag         = new DefineShape4Tag();
			tag.ShapeId     = reader.ReadUInt16();
			tag.ShapeBounds = SwfRect.Read(reader);
			tag.EdgeBounds  = SwfRect.Read(reader);
			tag.Flags       = reader.ReadByte();
			tag.Shapes      = SwfShapesWithStyle.Read(reader, SwfShapesWithStyle.ShapeStyleType.Shape4);
			return tag;
		}
	}
}