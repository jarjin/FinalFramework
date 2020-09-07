namespace FTSwfTools.SwfTypes {
	public struct SwfFillStyleType {
		public enum Type {
			SolidColor,
			LinearGradient,
			RadialGradient,
			FocalGradient,
			RepeatingBitmap,
			ClippedBitmap,
			NonSmoothedRepeatingBitmap,
			NonSmoothedClippedBitmap
		}
		public Type Value;

		public static SwfFillStyleType identity {
			get {
				return new SwfFillStyleType{
					Value = Type.SolidColor};
			}
		}

		public static SwfFillStyleType Read(SwfStreamReader reader) {
			var type_id = reader.ReadByte();
			return new SwfFillStyleType{
				Value = TypeFromByte(type_id)};
		}

		public override string ToString() {
			return string.Format(
				"SwfFillStyleType. " +
				"Type: {0}",
				Value);
		}

		public bool IsSolidType {
			get { return Value == Type.SolidColor; }
		}

		public bool IsBitmapType {
			get { return
				Value == Type.RepeatingBitmap ||
				Value == Type.ClippedBitmap ||
				Value == Type.NonSmoothedRepeatingBitmap ||
				Value == Type.NonSmoothedClippedBitmap;
			}
		}

		public bool IsGradientType {
			get { return
				Value == Type.LinearGradient ||
				Value == Type.RadialGradient ||
				Value == Type.FocalGradient;
			}
		}

		static Type TypeFromByte(byte type_id) {
			switch ( type_id ) {
			case 0x00: return Type.SolidColor;
			case 0x10: return Type.LinearGradient;
			case 0x12: return Type.RadialGradient;
			case 0x13: return Type.FocalGradient;
			case 0x40: return Type.RepeatingBitmap;
			case 0x41: return Type.ClippedBitmap;
			case 0x42: return Type.NonSmoothedRepeatingBitmap;
			case 0x43: return Type.NonSmoothedClippedBitmap;
			default:
				throw new System.Exception(string.Format(
					"Incorrect fill stype type id: {0}",
					type_id));
			}
		}
	}
}