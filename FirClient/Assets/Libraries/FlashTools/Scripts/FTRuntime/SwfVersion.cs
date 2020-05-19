namespace FTRuntime {
	public static class SwfVersion {
		public const int Major    = 1;
		public const int Minor    = 3;
		public const int Revision = 15;

		public static string AsString {
			get {
				return string.Format(
					"{0}.{1}.{2}",
					Major, Minor, Revision);
			}
		}
	}
}
