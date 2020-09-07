using FTSwfTools.SwfTypes;

namespace FTSwfTools.SwfTags {
	public class DefineShape3Tag : SwfTagBase {
		public ushort             ShapeId;
		public SwfRect            ShapeBounds;
		public SwfShapesWithStyle Shapes;

		public override SwfTagType TagType {
			get { return SwfTagType.DefineShape3; }
		}

		public override TResult AcceptVistor<TArg, TResult>(SwfTagVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public override string ToString() {
			return string.Format(
				"DefineShape3Tag. " +
				"ShapeId: {0}, ShapeBounds: {1}, Shapes: {2}",
				ShapeId, ShapeBounds, Shapes);
		}

		public static DefineShape3Tag Create(SwfStreamReader reader) {
			var tag         = new DefineShape3Tag();
			tag.ShapeId     = reader.ReadUInt16();
			tag.ShapeBounds = SwfRect.Read(reader);
			tag.Shapes      = SwfShapesWithStyle.Read(reader, SwfShapesWithStyle.ShapeStyleType.Shape3);
			return tag;
		}
	}
}