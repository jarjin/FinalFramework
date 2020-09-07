using System.Collections.Generic;

namespace FTSwfTools.SwfTypes {
	public struct SwfShapesWithStyle {
		public enum ShapeStyleType {
			Shape,
			Shape2,
			Shape3,
			Shape4
		}

		public struct FillStyle {
			public SwfFillStyleType Type;
			public ushort           BitmapId;
			public SwfMatrix        BitmapMatrix;

			public override string ToString() {
				return string.Format(
					"FillStyle. Type: {0}, BitmapId: {1}, BitmapMatrix: {2}",
					Type, BitmapId, BitmapMatrix);
			}
		}

		public List<FillStyle> FillStyles;

		public static SwfShapesWithStyle identity {
			get {
				return new SwfShapesWithStyle{
					FillStyles = new List<FillStyle>()};
			}
		}

		public static SwfShapesWithStyle Read(SwfStreamReader reader, ShapeStyleType style_type) {
			var shapes = SwfShapesWithStyle.identity;
			switch ( style_type ) {
			case ShapeStyleType.Shape:
				shapes.FillStyles = ReadFillStyles(reader, false, false);
				SkipLineStyles(reader, false, false, false);
				ReadShapeRecords(reader, shapes.FillStyles, false, false, false);
				break;
			case ShapeStyleType.Shape2:
				shapes.FillStyles = ReadFillStyles(reader, true, false);
				SkipLineStyles(reader, true, false, false);
				ReadShapeRecords(reader, shapes.FillStyles, true, false, false);
				break;
			case ShapeStyleType.Shape3:
				shapes.FillStyles = ReadFillStyles(reader, true, true);
				SkipLineStyles(reader, true, true, false);
				ReadShapeRecords(reader, shapes.FillStyles, true, true, false);
				break;
			case ShapeStyleType.Shape4:
				shapes.FillStyles = ReadFillStyles(reader, true, true);
				SkipLineStyles(reader, true, true, true);
				ReadShapeRecords(reader, shapes.FillStyles, true, true, true);
				break;
			default:
				throw new System.Exception(string.Format(
					"Unsupported ShapeStyleType: {0}", style_type));
			}
			return shapes;
		}

		public override string ToString() {
			return string.Format(
				"SwfShapesWithStyle. " +
				"FillStyles: {0}",
				FillStyles.Count);
		}

		// ---------------------------------------------------------------------
		//
		// FillStyles
		//
		// ---------------------------------------------------------------------

		static List<FillStyle> ReadFillStyles(
			SwfStreamReader reader, bool allow_big_array, bool with_alpha)
		{
			ushort count = reader.ReadByte();
			if ( allow_big_array && count == 255 ) {
				count = reader.ReadUInt16();
			}
			var styles = new List<FillStyle>(count);
			for ( var i = 0; i < count; ++i ) {
				styles.Add(ReadFillStyle(reader, with_alpha));
			}
			return styles;
		}

		// -----------------------------
		// FillStyle
		// -----------------------------

		static FillStyle ReadFillStyle(SwfStreamReader reader, bool with_alpha) {
			var fill_style  = new FillStyle();
			fill_style.Type = SwfFillStyleType.Read(reader);
			if ( fill_style.Type.IsSolidType ) {
				SwfColor.Read(reader, with_alpha);
			}
			if ( fill_style.Type.IsGradientType ) {
				SwfMatrix.Read(reader); // GradientMatrix
				switch ( fill_style.Type.Value ) {
				case SwfFillStyleType.Type.LinearGradient:
				case SwfFillStyleType.Type.RadialGradient:
					SkipGradient(reader, with_alpha); // Gradient
					break;
				case SwfFillStyleType.Type.FocalGradient:
					SkipFocalGradient(reader, with_alpha); // FocalGradient
					break;
				}
			}
			if ( fill_style.Type.IsBitmapType ) {
				fill_style.BitmapId     = reader.ReadUInt16();
				fill_style.BitmapMatrix = SwfMatrix.Read(reader);
			} else {
				throw new System.Exception(
					"Imported .swf file contains vector graphics. " +
					"You should use Tools/FlashExport.jsfl script for prepare .fla file");
			}
			return fill_style;
		}

		// -----------------------------
		// Gradient
		// -----------------------------

		static void SkipGradient(SwfStreamReader reader, bool with_alpha) {
			reader.ReadUnsignedBits(2); // SpreadMode
			reader.ReadUnsignedBits(2); // InterpolationMode
			var count = reader.ReadUnsignedBits(4);
			for ( var i = 0; i < count; ++i ) {
				reader.ReadByte(); // Ratio
				SwfColor.Read(reader, with_alpha);
			}
		}

		// -----------------------------
		// FocalGradient
		// -----------------------------

		static void SkipFocalGradient(SwfStreamReader reader, bool with_alpha) {
			reader.ReadUnsignedBits(2); // SpreadMode
			reader.ReadUnsignedBits(2); // InterpolationMode
			var count = reader.ReadUnsignedBits(4);
			for ( var i = 0; i < count; ++i ) {
				reader.ReadByte(); // Ratio
				SwfColor.Read(reader, with_alpha);
			}
			reader.ReadFixedPoint_8_8(); // FocalPoint
		}

		// ---------------------------------------------------------------------
		//
		// LineStyles
		//
		// ---------------------------------------------------------------------

		static void SkipLineStyles(
			SwfStreamReader reader, bool allow_big_array, bool with_alpha, bool line2_type)
		{
			ushort count = reader.ReadByte();
			if ( allow_big_array && count == 255 ) {
				count = reader.ReadUInt16();
			}
			for ( var i = 0; i < count; ++i ) {
				if ( line2_type ) {
					SkipLine2Style(reader);
				} else {
					SkipLineStyle(reader, with_alpha);
				}
			}
		}

		// -----------------------------
		// LineStyles
		// -----------------------------

		static void SkipLineStyle(SwfStreamReader reader, bool with_alpha) {
			reader.ReadUInt16(); // Width
			SwfColor.Read(reader, with_alpha);
		}

		static void SkipLine2Style(SwfStreamReader reader) {
			reader.ReadUInt16();          // Width
			reader.ReadUnsignedBits(2);   // StartCapStyle
			var join_style    = reader.ReadUnsignedBits(2);
			var has_fill_flag = reader.ReadBit();
			reader.ReadBit();             // NoHScaleFlag
			reader.ReadBit();             // NoVScaleFlag
			reader.ReadBit();             // PixelHintingFlag
			reader.ReadUnsignedBits(5);   // Reserved
			reader.ReadBit();             // NoClose
			reader.ReadUnsignedBits(2);   // EndCapStyle
			if ( join_style == 2 ) {
				reader.ReadFixedPoint_8_8(); // MiterLimitFactor
			}
			if ( has_fill_flag ) {
				ReadFillStyle(reader, true); // FillStyle
			} else {
				SwfColor.Read(reader, true);
			}
		}

		// ---------------------------------------------------------------------
		//
		// ShapeRecords
		//
		// ---------------------------------------------------------------------

		static void ReadShapeRecords(
			SwfStreamReader reader, List<FillStyle> fill_styles,
			bool allow_big_array, bool with_alpha, bool line2_type)
		{
			var fill_style_bits = reader.ReadUnsignedBits(4);
			var line_style_bits = reader.ReadUnsignedBits(4);
			while ( !ReadShapeRecord(
				reader, fill_styles,
				ref fill_style_bits, ref line_style_bits,
				allow_big_array, with_alpha, line2_type) )
			{
				continue;
			}
		}

		static bool ReadShapeRecord(
			SwfStreamReader reader, List<FillStyle> fill_styles,
			ref uint fill_style_bits, ref uint line_style_bits,
			bool allow_big_array, bool with_alpha, bool line2_type)
		{
			var is_edge = reader.ReadBit();
			if ( is_edge ) {
				var straight = reader.ReadBit();
				if ( straight ) {
					SkipStraigtEdgeShapeRecord(reader);
				} else {
					SkipCurvedEdgeShapeRecord(reader);
				}
				return false;
			} else {
				var state_new_styles    = reader.ReadBit();
				var state_line_style    = reader.ReadBit();
				var state_fill_style1   = reader.ReadBit();
				var state_fill_style0   = reader.ReadBit();
				var state_move_to       = reader.ReadBit();
				var is_end_shape_record =
					!state_new_styles  && !state_line_style  &&
					!state_fill_style0 && !state_fill_style1 && !state_move_to;
				if ( is_end_shape_record ) {
					return true;
				} else {
					if ( state_move_to ) {
						var move_bits = reader.ReadUnsignedBits(5);
						reader.ReadSignedBits(move_bits); // move_delta_x
						reader.ReadSignedBits(move_bits); // move_delta_y
					}
					if ( state_fill_style0 ) {
						reader.ReadUnsignedBits(fill_style_bits); // fill_style_0
					}
					if ( state_fill_style1 ) {
						reader.ReadUnsignedBits(fill_style_bits); // fill_style_1
					}
					if ( state_line_style ) {
						reader.ReadUnsignedBits(line_style_bits); // line_style
					}
					if ( state_new_styles ) {
						reader.AlignToByte();
						fill_styles.AddRange(ReadFillStyles(reader, allow_big_array, with_alpha));
						SkipLineStyles(reader, allow_big_array, with_alpha, line2_type);
						fill_style_bits = reader.ReadUnsignedBits(4);
						line_style_bits = reader.ReadUnsignedBits(4);
					}
					return false;
				}
			}
		}

		static void SkipStraigtEdgeShapeRecord(SwfStreamReader reader) {
			var num_bits          = reader.ReadUnsignedBits(4) + 2;
			var general_line_flag = reader.ReadBit();
			var vert_line_flag = general_line_flag ? false : reader.ReadBit();
			if ( general_line_flag || !vert_line_flag ) {
				reader.ReadSignedBits(num_bits); // delta_x
			}
			if ( general_line_flag || vert_line_flag ) {
				reader.ReadSignedBits(num_bits); // delta_y
			}
		}

		static void SkipCurvedEdgeShapeRecord(SwfStreamReader reader) {
			var num_bits = reader.ReadUnsignedBits(4) + 2;
			reader.ReadSignedBits(num_bits); // control_delta_x
			reader.ReadSignedBits(num_bits); // control_delta_y
			reader.ReadSignedBits(num_bits); // anchor_delta_x
			reader.ReadSignedBits(num_bits); // anchor_delta_y
		}
	}
}