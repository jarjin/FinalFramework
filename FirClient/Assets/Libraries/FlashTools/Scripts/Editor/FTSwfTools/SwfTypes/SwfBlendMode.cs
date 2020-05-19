namespace FTSwfTools.SwfTypes {
	public struct SwfBlendMode {
		public enum Mode {
			Normal,
			Layer,
			Multiply,
			Screen,
			Lighten,
			Darken,
			Difference,
			Add,
			Subtract,
			Invert,
			Alpha,
			Erase,
			Overlay,
			Hardlight
		}
		public Mode Value;

		public static SwfBlendMode identity {
			get {
				return new SwfBlendMode{
					Value = Mode.Normal};
			}
		}

		public static SwfBlendMode Read(SwfStreamReader reader) {
			var mode_id = reader.ReadByte();
			return new SwfBlendMode{
				Value = ModeFromByte(mode_id)};
		}

		public override string ToString() {
			return string.Format(
				"SwfBlendMode. " +
				"Mode: {0}",
				Value);
		}

		static Mode ModeFromByte(byte mode_id) {
			switch ( mode_id ) {
			case  0: // Mode.Normal too
			case  1: return Mode.Normal;
			case  2: return Mode.Layer;
			case  3: return Mode.Multiply;
			case  4: return Mode.Screen;
			case  5: return Mode.Lighten;
			case  6: return Mode.Darken;
			case  7: return Mode.Difference;
			case  8: return Mode.Add;
			case  9: return Mode.Subtract;
			case 10: return Mode.Invert;
			case 11: return Mode.Alpha;
			case 12: return Mode.Erase;
			case 13: return Mode.Overlay;
			case 14: return Mode.Hardlight;
			default:
				throw new System.Exception(string.Format(
					"Incorrect blend mode id: {0}",
					mode_id));
			}
		}
	}
}