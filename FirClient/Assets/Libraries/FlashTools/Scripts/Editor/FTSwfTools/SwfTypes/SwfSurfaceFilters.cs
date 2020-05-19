using System.Collections.Generic;

namespace FTSwfTools.SwfTypes {
	public struct SwfSurfaceFilters {
		public abstract class Filter {
			public enum Types {
				DropShadow,
				Blur,
				Glow,
				Bevel,
				GradientGlow,
				Convolution,
				ColorMatrix,
				GradientBevel
			}
			public abstract Types Type { get; }
		}

		public class DropShadowFilter : Filter {
			public override Types Type {
				get { return Types.DropShadow; }
			}
			public SwfColor   DropShadowColor;
			public float      BlurX;
			public float      BlurY;
			public float      Angle;
			public float      Distance;
			public float      Strength;
			public bool       InnerShadow;
			public bool       Knockout;
			public bool       CompositeSource;
			public uint       Passes;
		}

		public class BlurFilter : Filter {
			public override Types Type {
				get { return Types.Blur; }
			}
			public float      BlurX;
			public float      BlurY;
			public uint       Passes;
		}

		public class GlowFilter : Filter {
			public override Types Type {
				get { return Types.Glow; }
			}
			public SwfColor   GlowColor;
			public float      BlurX;
			public float      BlurY;
			public float      Strength;
			public bool       InnerGlow;
			public bool       Knockout;
			public bool       CompositeSource;
			public uint       Passes;
		}

		public class BevelFilter : Filter {
			public override Types Type {
				get { return Types.Bevel; }
			}
			public SwfColor   ShadowColor;
			public SwfColor   HighlightColor;
			public float      BlurX;
			public float      BlurY;
			public float      Angle;
			public float      Distance;
			public float      Strength;
			public bool       InnerShadow;
			public bool       Knockout;
			public bool       CompositeSource;
			public bool       OnTop;
			public uint       Passes;
		}

		public class GradientGlowFilter : Filter {
			public override Types Type {
				get { return Types.GradientGlow; }
			}
			public SwfColor[] GradientColors;
			public byte[]     GradientRatio;
			public float      BlurX;
			public float      BlurY;
			public float      Angle;
			public float      Distance;
			public float      Strength;
			public bool       InnerShadow;
			public bool       Knockout;
			public bool       CompositeSource;
			public bool       OnTop;
			public uint       Passes;
		}

		public class ConvolutionFilter : Filter {
			public override Types Type {
				get { return Types.Convolution; }
			}
			public byte       MatrixX;
			public byte       MatrixY;
			public float      Divisor;
			public float      Bias;
			public float[]    Matrix;
			public SwfColor   DefaultColor;
			public bool       Clamp;
			public bool       PreserveAlpha;
		}

		public class ColorMatrixFilter : Filter {
			public override Types Type {
				get { return Types.ColorMatrix; }
			}
			public float[]    Matrix;
		}

		public class GradientBevelFilter : Filter {
			public override Types Type {
				get { return Types.GradientBevel; }
			}
			public SwfColor[] GradientColors;
			public byte[]     GradientRatio;
			public float      BlurX;
			public float      BlurY;
			public float      Angle;
			public float      Distance;
			public float      Strength;
			public bool       InnerShadow;
			public bool       Knockout;
			public bool       CompositeSource;
			public bool       OnTop;
			public uint       Passes;
		}

		public List<Filter> Filters;

		public static SwfSurfaceFilters identity {
			get {
				return new SwfSurfaceFilters{
					Filters = new List<Filter>()};
			}
		}

		public static SwfSurfaceFilters Read(SwfStreamReader reader) {
			var filter_count = reader.ReadByte();
			var filters      = new List<Filter>(filter_count);
			for ( var i = 0; i < filter_count; ++i ) {
				filters.Add(ReadFilter(reader));
			}
			return new SwfSurfaceFilters{
				Filters = filters};
		}

		public override string ToString() {
			return string.Format(
				"SwfSurfaceFilters. " +
				"Filters: {0}",
				Filters.Count);
		}

		// ---------------------------------------------------------------------
		//
		// ReadFilters
		//
		// ---------------------------------------------------------------------

		static Filter ReadFilter(SwfStreamReader reader) {
			var type_id = reader.ReadByte();
			return CreateFilterFromTypeId(type_id, reader);
		}

		static Filter CreateFilterFromTypeId(byte type_id, SwfStreamReader reader) {
			switch ( type_id ) {
			case 0: return ReadConcreteFilter(new DropShadowFilter   (), reader);
			case 1: return ReadConcreteFilter(new BlurFilter         (), reader);
			case 2: return ReadConcreteFilter(new GlowFilter         (), reader);
			case 3: return ReadConcreteFilter(new BevelFilter        (), reader);
			case 4: return ReadConcreteFilter(new GradientGlowFilter (), reader);
			case 5: return ReadConcreteFilter(new ConvolutionFilter  (), reader);
			case 6: return ReadConcreteFilter(new ColorMatrixFilter  (), reader);
			case 7: return ReadConcreteFilter(new GradientBevelFilter(), reader);
			default:
				throw new System.Exception(string.Format(
					"Incorrect surface filter type id: {0}", type_id));
			}
		}

		static Filter ReadConcreteFilter(DropShadowFilter filter, SwfStreamReader reader) {
			filter.DropShadowColor = SwfColor.Read(reader, true);
			filter.BlurX           = reader.ReadFixedPoint_16_16();
			filter.BlurY           = reader.ReadFixedPoint_16_16();
			filter.Angle           = reader.ReadFixedPoint_16_16();
			filter.Distance        = reader.ReadFixedPoint_16_16();
			filter.Strength        = reader.ReadFixedPoint_8_8();
			filter.InnerShadow     = reader.ReadBit();
			filter.Knockout        = reader.ReadBit();
			filter.CompositeSource = reader.ReadBit();
			filter.Passes          = reader.ReadUnsignedBits(5);
			return filter;
		}

		static Filter ReadConcreteFilter(BlurFilter filter, SwfStreamReader reader) {
			filter.BlurX           = reader.ReadFixedPoint_16_16();
			filter.BlurY           = reader.ReadFixedPoint_16_16();
			filter.Passes          = reader.ReadUnsignedBits(5);
			reader.ReadUnsignedBits(3); // reserved
			return filter;
		}

		static Filter ReadConcreteFilter(GlowFilter filter, SwfStreamReader reader) {
			filter.GlowColor       = SwfColor.Read(reader, true);
			filter.BlurX           = reader.ReadFixedPoint_16_16();
			filter.BlurY           = reader.ReadFixedPoint_16_16();
			filter.Strength        = reader.ReadFixedPoint_8_8();
			filter.InnerGlow       = reader.ReadBit();
			filter.Knockout        = reader.ReadBit();
			filter.CompositeSource = reader.ReadBit();
			filter.Passes          = reader.ReadUnsignedBits(5);
			return filter;
		}

		static Filter ReadConcreteFilter(BevelFilter filter, SwfStreamReader reader) {
			filter.ShadowColor     = SwfColor.Read(reader, true);
			filter.HighlightColor  = SwfColor.Read(reader, true);
			filter.BlurX           = reader.ReadFixedPoint_16_16();
			filter.BlurY           = reader.ReadFixedPoint_16_16();
			filter.Angle           = reader.ReadFixedPoint_16_16();
			filter.Distance        = reader.ReadFixedPoint_16_16();
			filter.Strength        = reader.ReadFixedPoint_8_8();
			filter.InnerShadow     = reader.ReadBit();
			filter.Knockout        = reader.ReadBit();
			filter.CompositeSource = reader.ReadBit();
			filter.OnTop           = reader.ReadBit();
			filter.Passes          = reader.ReadUnsignedBits(4);
			return filter;
		}

		static Filter ReadConcreteFilter(GradientGlowFilter filter, SwfStreamReader reader) {
			var num_colors         = reader.ReadByte();
			filter.GradientColors  = new SwfColor[num_colors];
			for ( var i = 0; i < num_colors; ++i ) {
				filter.GradientColors[i] = SwfColor.Read(reader, true);
			}
			filter.GradientRatio   = new byte[num_colors];
			for ( var i = 0; i < num_colors; ++i ) {
				filter.GradientRatio[i] = reader.ReadByte();
			}
			filter.BlurX           = reader.ReadFixedPoint_16_16();
			filter.BlurY           = reader.ReadFixedPoint_16_16();
			filter.Angle           = reader.ReadFixedPoint_16_16();
			filter.Distance        = reader.ReadFixedPoint_16_16();
			filter.Strength        = reader.ReadFixedPoint_8_8();
			filter.InnerShadow     = reader.ReadBit();
			filter.Knockout        = reader.ReadBit();
			filter.CompositeSource = reader.ReadBit();
			filter.OnTop           = reader.ReadBit();
			filter.Passes          = reader.ReadUnsignedBits(4);
			return filter;
		}

		static Filter ReadConcreteFilter(ConvolutionFilter filter, SwfStreamReader reader) {
			filter.MatrixX         = reader.ReadByte();
			filter.MatrixY         = reader.ReadByte();
			filter.Divisor         = reader.ReadFloat32();
			filter.Bias            = reader.ReadFloat32();
			filter.Matrix          = new float[filter.MatrixX * filter.MatrixY];
			for ( var i = 0; i < filter.Matrix.Length; ++i ) {
				filter.Matrix[i] = reader.ReadFloat32();
			}
			filter.DefaultColor    = SwfColor.Read(reader, true);
			reader.ReadUnsignedBits(6); // reserved
			filter.Clamp           = reader.ReadBit();
			filter.PreserveAlpha   = reader.ReadBit();
			return filter;
		}

		static Filter ReadConcreteFilter(ColorMatrixFilter filter, SwfStreamReader reader) {
			filter.Matrix          = new float[20];
			for ( var i = 0; i < filter.Matrix.Length; ++i ) {
				filter.Matrix[i] = reader.ReadFloat32();
			}
			return filter;
		}

		static Filter ReadConcreteFilter(GradientBevelFilter filter, SwfStreamReader reader) {
			var num_colors         = reader.ReadByte();
			filter.GradientColors  = new SwfColor[num_colors];
			for ( var i = 0; i < num_colors; ++i ) {
				filter.GradientColors[i] = SwfColor.Read(reader, true);
			}
			filter.GradientRatio   = new byte[num_colors];
			for ( var i = 0; i < num_colors; ++i ) {
				filter.GradientRatio[i] = reader.ReadByte();
			}
			filter.BlurX           = reader.ReadFixedPoint_16_16();
			filter.BlurY           = reader.ReadFixedPoint_16_16();
			filter.Angle           = reader.ReadFixedPoint_16_16();
			filter.Distance        = reader.ReadFixedPoint_16_16();
			filter.Strength        = reader.ReadFixedPoint_8_8();
			filter.InnerShadow     = reader.ReadBit();
			filter.Knockout        = reader.ReadBit();
			filter.CompositeSource = reader.ReadBit();
			filter.OnTop           = reader.ReadBit();
			filter.Passes          = reader.ReadUnsignedBits(4);
			return filter;
		}
	}
}