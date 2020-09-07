namespace FTSwfTools.SwfTypes {
	public struct SwfRect {
		public float XMin;
		public float XMax;
		public float YMin;
		public float YMax;

		public static SwfRect identity {
			get {
				return new SwfRect{
					XMin = 0,
					XMax = 0,
					YMin = 0,
					YMax = 0};
			}
		}

		public static SwfRect Read(SwfStreamReader reader) {
			var bits = reader.ReadUnsignedBits(5);
			var xmin = reader.ReadSignedBits(bits) / 20.0f;
			var xmax = reader.ReadSignedBits(bits) / 20.0f;
			var ymin = reader.ReadSignedBits(bits) / 20.0f;
			var ymax = reader.ReadSignedBits(bits) / 20.0f;
			reader.AlignToByte();
			return new SwfRect{
				XMin = xmin,
				XMax = xmax,
				YMin = ymin,
				YMax = ymax};
		}

		public override string ToString() {
			return string.Format(
				"SwfRect. " +
				"XMin: {0}, XMax: {1}, YMin: {2}, YMax: {3}",
				XMin, XMax, YMin, YMax);
		}
	}
}