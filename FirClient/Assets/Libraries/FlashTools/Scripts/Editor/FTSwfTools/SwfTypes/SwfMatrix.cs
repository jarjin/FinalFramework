namespace FTSwfTools.SwfTypes {
	public struct SwfMatrix {
		public float ScaleX;
		public float ScaleY;
		public float RotateSkew0;
		public float RotateSkew1;
		public float TranslateX;
		public float TranslateY;

		public static SwfMatrix identity {
			get {
				return new SwfMatrix {
					ScaleX      = 1,
					ScaleY      = 1,
					RotateSkew0 = 0,
					RotateSkew1 = 0,
					TranslateX  = 0,
					TranslateY  = 0};
			}
		}

		public static SwfMatrix Read(SwfStreamReader reader) {
			var matrix = SwfMatrix.identity;
			var has_scale = reader.ReadBit();
			if ( has_scale ) {
				var bits      = (byte)reader.ReadUnsignedBits(5);
				matrix.ScaleX = reader.ReadFixedPoint16(bits);
				matrix.ScaleY = reader.ReadFixedPoint16(bits);
			} else {
				matrix.ScaleX =
				matrix.ScaleY = 1.0f;
			}
			var has_rotate = reader.ReadBit();
			if ( has_rotate ) {
				var bits           = (byte)reader.ReadUnsignedBits(5);
				matrix.RotateSkew0 = reader.ReadFixedPoint16(bits);
				matrix.RotateSkew1 = reader.ReadFixedPoint16(bits);
			} else {
				matrix.RotateSkew0 =
				matrix.RotateSkew1 = 0.0f;
			}
			var translate_bits = (byte)reader.ReadUnsignedBits(5);
			matrix.TranslateX  = reader.ReadSignedBits(translate_bits) / 20.0f;
			matrix.TranslateY  = reader.ReadSignedBits(translate_bits) / 20.0f;
			reader.AlignToByte();
			return matrix;
		}

		public override string ToString() {
			return string.Format(
				"SwfMatrix. " +
				"ScaleX: {0}, ScaleY: {1}, " +
				"RotateSkew0: {2}, RotateSkew1: {3}, " +
				"TranslateX: {4}, TranslateY: {5}",
				ScaleX, ScaleY,
				RotateSkew0, RotateSkew1,
				TranslateX, TranslateY);
		}
	}
}