using UnityEngine;

namespace FTRuntime.Yields {
	public class SwfWaitStopOrRewindPlaying : CustomYieldInstruction {
		SwfClipController _waitCtrl;

		public SwfWaitStopOrRewindPlaying(SwfClipController ctrl) {
			Subscribe(ctrl);
		}

		public SwfWaitStopOrRewindPlaying Reuse(SwfClipController ctrl) {
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

		SwfWaitStopOrRewindPlaying Subscribe(SwfClipController ctrl) {
			Unsubscribe();
			if ( ctrl ) {
				_waitCtrl = ctrl;
				ctrl.OnStopPlayingEvent   += OnStopOrRewindPlaying;
				ctrl.OnRewindPlayingEvent += OnStopOrRewindPlaying;
			}
			return this;
		}

		void Unsubscribe() {
			if ( _waitCtrl != null ) {
				_waitCtrl.OnStopPlayingEvent   -= OnStopOrRewindPlaying;
				_waitCtrl.OnRewindPlayingEvent -= OnStopOrRewindPlaying;
				_waitCtrl = null;
			}
		}

		void OnStopOrRewindPlaying(SwfClipController ctrl) {
			Unsubscribe();
		}
	}
}