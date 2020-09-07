using UnityEngine;

namespace FTRuntime.Yields {
	public class SwfWaitStopPlaying : CustomYieldInstruction {
		SwfClipController _waitCtrl;

		public SwfWaitStopPlaying(SwfClipController ctrl) {
			Subscribe(ctrl);
		}

		public SwfWaitStopPlaying Reuse(SwfClipController ctrl) {
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

		SwfWaitStopPlaying Subscribe(SwfClipController ctrl) {
			Unsubscribe();
			if ( ctrl ) {
				_waitCtrl = ctrl;
				ctrl.OnStopPlayingEvent += OnStopPlaying;
			}
			return this;
		}

		void Unsubscribe() {
			if ( _waitCtrl != null ) {
				_waitCtrl.OnStopPlayingEvent -= OnStopPlaying;
				_waitCtrl = null;
			}
		}

		void OnStopPlaying(SwfClipController ctrl) {
			Unsubscribe();
		}
	}
}