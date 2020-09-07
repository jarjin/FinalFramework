using UnityEngine;

namespace FTRuntime.Yields {
	public class SwfWaitRewindPlaying : CustomYieldInstruction {
		SwfClipController _waitCtrl;

		public SwfWaitRewindPlaying(SwfClipController ctrl) {
			Subscribe(ctrl);
		}

		public SwfWaitRewindPlaying Reuse(SwfClipController ctrl) {
			return Subscribe(ctrl);
		}

		public override bool keepWaiting {
			get {
				return _waitCtrl != null;
			}
		}

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		SwfWaitRewindPlaying Subscribe(SwfClipController ctrl) {
			Unsubscribe();
			if ( ctrl ) {
				_waitCtrl = ctrl;
				ctrl.OnRewindPlayingEvent += OnRewindPlaying;
			}
			return this;
		}

		void Unsubscribe() {
			if ( _waitCtrl != null ) {
				_waitCtrl.OnRewindPlayingEvent -= OnRewindPlaying;
				_waitCtrl = null;
			}
		}

		void OnRewindPlaying(SwfClipController ctrl) {
			Unsubscribe();
		}
	}
}