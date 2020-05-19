using UnityEngine;

namespace FTRuntime.Yields {
	public class SwfWaitPlayStopped : CustomYieldInstruction {
		SwfClipController _waitCtrl;

		public SwfWaitPlayStopped(SwfClipController ctrl) {
			Subscribe(ctrl);
		}

		public SwfWaitPlayStopped Reuse(SwfClipController ctrl) {
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

		SwfWaitPlayStopped Subscribe(SwfClipController ctrl) {
			Unsubscribe();
			if ( ctrl ) {
				_waitCtrl = ctrl;
				ctrl.OnPlayStoppedEvent += OnPlayStopped;
			}
			return this;
		}

		void Unsubscribe() {
			if ( _waitCtrl != null ) {
				_waitCtrl.OnPlayStoppedEvent -= OnPlayStopped;
				_waitCtrl = null;
			}
		}

		void OnPlayStopped(SwfClipController ctrl) {
			Unsubscribe();
		}
	}
}