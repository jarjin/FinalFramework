namespace FTSwfTools.SwfTypes {
	public struct SwfClipActions {
		public static SwfClipActions identity {
			get {
				return new SwfClipActions();
			}
		}

		public static SwfClipActions Read(SwfStreamReader reader) {
			throw new System.Exception("Clip actions is unsupported");
		}

		public override string ToString() {
			return "SwfClipActions.";
		}
	}
}