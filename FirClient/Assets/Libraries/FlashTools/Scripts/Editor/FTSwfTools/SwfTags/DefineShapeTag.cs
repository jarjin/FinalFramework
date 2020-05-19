using FTSwfTools.SwfTypes;

namespace FTSwfTools.SwfTags {
	public class DefineShapeTag : SwfTagBase {
		public ushort             ShapeId;
		public SwfRect            ShapeBounds;
		public SwfShapesWithStyle Shapes;

		public override SwfTagType TagType {
			get { return SwfTagType.DefineShape; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"DefineShapeTag. " +
				"ShapeId: {0}, ShapeBounds: {1}, Shapes: {2}",
				ShapeId, ShapeBounds, Shapes);
		}

		public static DefineShapeTag Create(SwfStreamReader reader) {
			var tag         = new DefineShapeTag();
			tag.ShapeId     = reader.ReadUInt16();
			tag.ShapeBounds = SwfRect.Read(reader);
			tag.Shapes      = SwfShapesWithStyle.Read(reader, SwfShapesWithStyle.ShapeStyleType.Shape);
			return tag;
		}
	}
}