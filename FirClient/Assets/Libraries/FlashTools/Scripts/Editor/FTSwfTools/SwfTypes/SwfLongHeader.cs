namespace FTSwfTools.SwfTypes {
	public struct SwfLongHeader {
		public SwfShortHeader ShortHeader;
		public SwfRect        FrameSize;
		public float          FrameRate;
		public ushort         FrameCount;

		public static SwfLongHeader Read(SwfStreamReader reader) {
			return new SwfLongHeader{
				ShortHeader = SwfShortHeader.Read(reader),
				FrameSize   = SwfRect.Read(reader),
				FrameRate   = reader.ReadFixedPoint_8_8(),
				FrameCount  = reader.ReadUInt16()};
		}

		public override string ToString() {
			return string.Format(
				"SwfLongHeader. " +
				"Format: {0}, Version: {1}, FileLength: {2}, " +
				"FrameSize: {3}, FrameRate: {4}, FrameCount: {5}",
				ShortHeader.Format, ShortHeader.Version, ShortHeader.FileLength,
				FrameSize, FrameRate, FrameCount);
		}
	}
}