using UnityEngine;
using FTRuntime.Internal;

namespace FTRuntime {
	[ExecuteInEditMode, DisallowMultipleComponent]
	[RequireComponent(typeof(SwfClip))]
	public class SwfClipController : MonoBehaviour {

		SwfClip _clip      = null;
		bool    _isPlaying = false;
		float   _tickTimer = 0.0f;

		// ---------------------------------------------------------------------
		//
		// Events
		//
		// ---------------------------------------------------------------------

		/// <summary>
		/// Occurs when the controller stops played clip
		/// </summary>
		public event System.Action<SwfClipController> OnStopPlayingEvent;

		/// <summary>
		/// Occurs when the controller plays stopped clip
		/// </summary>
		public event System.Action<SwfClipController> OnPlayStoppedEvent;

		/// <summary>
		/// Occurs when the controller rewinds played clip
		/// </summary>
		public event System.Action<SwfClipController> OnRewindPlayingEvent;

		// ---------------------------------------------------------------------
		//
		// Serialized fields
		//
		// ---------------------------------------------------------------------

		[SerializeField]
		bool _autoPlay = true;

		[SerializeField]
		bool _useUnscaledDt = false;

		[SerializeField, SwfFloatRange(0.0f, float.MaxValue)]
		float _rateScale = 1.0f;

		[SerializeField]
		string _groupName = string.Empty;

		[SerializeField]
		PlayModes _playMode = PlayModes.Forward;

		[SerializeField]
		LoopModes _loopMode = LoopModes.Loop;

		// ---------------------------------------------------------------------
		//
		// Properties
		//
		// ---------------------------------------------------------------------

		/// <summary>
		/// Controller play modes
		/// </summary>
		public enum PlayModes {
			/// <summary>
			/// Forward play mode
			/// </summary>
			Forward,
			/// <summary>
			/// Backward play mode
			/// </summary>
			Backward
		}

		/// <summary>
		/// Controller loop modes
		/// </summary>
		public enum LoopModes {
			/// <summary>
			/// Once loop mode
			/// </summary>
			Once,
			/// <summary>
			/// Repeat loop mode
			/// </summary>
			Loop
		}

		/// <summary>
		/// Gets or sets a value indicating whether controller play after awake on scene
		/// </summary>
		/// <value><c>true</c> if auto play; otherwise, <c>false</c></value>
		public bool autoPlay {
			get { return _autoPlay; }
			set { _autoPlay = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether controller uses unscaled delta time
		/// </summary>
		/// <value><c>true</c> if uses unscaled delta time; otherwise, <c>false</c></value>
		public bool useUnscaledDt {
			get { return _useUnscaledDt; }
			set { _useUnscaledDt = value; }
		}

		/// <summary>
		/// Gets or sets the controller rate scale
		/// </summary>
		/// <value>The rate scale</value>
		public float rateScale {
			get { return _rateScale; }
			set { _rateScale = Mathf.Clamp(value, 0.0f, float.MaxValue); }
		}

		/// <summary>
		/// Gets or sets the controller group name
		/// </summary>
		/// <value>The group name</value>
		public string groupName {
			get { return _groupName; }
			set { _groupName = value; }
		}

		/// <summary>
		/// Gets or sets the controller play mode
		/// </summary>
		/// <value>The play mode</value>
		public PlayModes playMode {
			get { return _playMode; }
			set { _playMode = value; }
		}

		/// <summary>
		/// Gets or sets the controller loop mode
		/// </summary>
		/// <value>The loop mode</value>
		public LoopModes loopMode {
			get { return _loopMode; }
			set { _loopMode = value; }
		}

		/// <summary>
		/// Gets the controller clip
		/// </summary>
		/// <value>The clip</value>
		public SwfClip clip {
			get { return _clip; }
		}

		/// <summary>
		/// Gets a value indicating whether controller is playing
		/// </summary>
		/// <value><c>true</c> if is playing; otherwise, <c>false</c></value>
		public bool isPlaying {
			get { return _isPlaying; }
		}

		/// <summary>
		/// Gets a value indicating whether controller is stopped
		/// </summary>
		/// <value><c>true</c> if is stopped; otherwise, <c>false</c></value>
		public bool isStopped {
			get { return !_isPlaying; }
		}

		// ---------------------------------------------------------------------
		//
		// Functions
		//
		// ---------------------------------------------------------------------

		/// <summary>
		/// Changes the animation frame with stops it
		/// </summary>
		/// <param name="frame">The new current frame</param>
		public void GotoAndStop(int frame) {
			if ( clip ) {
				clip.currentFrame = frame;
			}
			Stop(false);
		}

		/// <summary>
		/// Changes the animation sequence and frame with stops it
		/// </summary>
		/// <param name="sequence">The new sequence</param>
		/// <param name="frame">The new current frame</param>
		public void GotoAndStop(string sequence, int frame) {
			if ( clip ) {
				clip.sequence = sequence;
			}
			GotoAndStop(frame);
		}

		/// <summary>
		/// Changes the animation frame with plays it
		/// </summary>
		/// <param name="frame">The new current frame</param>
		public void GotoAndPlay(int frame) {
			if ( clip ) {
				clip.currentFrame = frame;
			}
			Play(false);
		}

		/// <summary>
		/// Changes the animation sequence and frame with plays it
		/// </summary>
		/// <param name="sequence">The new sequence</param>
		/// <param name="frame">The new current frame</param>
		public void GotoAndPlay(string sequence, int frame) {
			if ( clip ) {
				clip.sequence = sequence;
			}
			GotoAndPlay(frame);
		}

		/// <summary>
		/// Stop with specified rewind action
		/// </summary>
		/// <param name="rewind">If set to <c>true</c> rewind animation to begin frame</param>
		public void Stop(bool rewind) {
			var is_playing = isPlaying;
			if ( is_playing ) {
				_isPlaying = false;
				_tickTimer = 0.0f;
			}
			if ( rewind ) {
				Rewind();
			}
			if ( is_playing && OnStopPlayingEvent != null ) {
				OnStopPlayingEvent(this);
			}
		}

		/// <summary>
		/// Changes the animation sequence and stop controller with rewind
		/// </summary>
		/// <param name="sequence">The new sequence</param>
		public void Stop(string sequence) {
			if ( clip ) {
				clip.sequence = sequence;
			}
			Stop(true);
		}

		/// <summary>
		/// Play with specified rewind action
		/// </summary>
		/// <param name="rewind">If set to <c>true</c> rewind animation to begin frame</param>
		public void Play(bool rewind) {
			var is_stopped = isStopped;
			if ( is_stopped ) {
				_isPlaying = true;
				_tickTimer = 0.0f;
			}
			if ( rewind ) {
				Rewind();
			}
			if ( is_stopped && OnPlayStoppedEvent != null ) {
				OnPlayStoppedEvent(this);
			}
		}

		/// <summary>
		/// Changes the animation sequence and play controller with rewind
		/// </summary>
		/// <param name="sequence">The new sequence</param>
		public void Play(string sequence) {
			if ( clip ) {
				clip.sequence = sequence;
			}
			Play(true);
		}

		/// <summary>
		/// Rewind animation to begin frame
		/// </summary>
		public void Rewind() {
			switch ( playMode ) {
			case PlayModes.Forward:
				if ( clip ) {
					clip.ToBeginFrame();
				}
				break;
			case PlayModes.Backward:
				if ( clip ) {
					clip.ToEndFrame();
				}
				break;
			default:
				throw new UnityException(string.Format(
					"SwfClipController. Incorrect play mode: {0}",
					playMode));
			}
			if ( isPlaying && OnRewindPlayingEvent != null ) {
				OnRewindPlayingEvent(this);
			}
		}

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		internal void Internal_Update(float scaled_dt, float unscaled_dt) {
			while ( isPlaying && clip ) {
				var dt = useUnscaledDt ? unscaled_dt : scaled_dt;
				var frame_rate = clip.frameRate * rateScale;
				if ( dt > 0.0f && frame_rate > 0.0f ) {
					_tickTimer += frame_rate * dt;
					if ( _tickTimer >= 1.0f ) {
						var unused_dt = (_tickTimer - 1.0f) / frame_rate;
						_tickTimer = 0.0f;
						TimerTick();
						scaled_dt   = unused_dt * (scaled_dt   / dt);
						unscaled_dt = unused_dt * (unscaled_dt / dt);
					} else {
						break;
					}
				} else {
					break;
				}
			}
		}

		void TimerTick() {
			if ( !NextClipFrame() ) {
				switch ( loopMode ) {
				case LoopModes.Once:
					Stop(false);
					break;
				case LoopModes.Loop:
					Rewind();
					break;
				default:
					throw new UnityException(string.Format(
						"SwfClipController. Incorrect loop mode: {0}",
						loopMode));
				}
			}
		}

		bool NextClipFrame() {
			switch ( playMode ) {
			case PlayModes.Forward:
				return clip ? clip.ToNextFrame() : false;
			case PlayModes.Backward:
				return clip ? clip.ToPrevFrame() : false;
			default:
				throw new UnityException(string.Format(
					"SwfClipController. Incorrect play mode: {0}",
					playMode));
			}
		}

		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		void Awake() {
			_clip = GetComponent<SwfClip>();
		}

		void OnEnable() {
			var swf_manager = SwfManager.GetInstance(true);
			if ( swf_manager ) {
				swf_manager.AddController(this);
			}
			if ( autoPlay && Application.isPlaying ) {
				Play(false);
			}
		}

		void OnDisable() {
			Stop(false);
			var swf_manager = SwfManager.GetInstance(false);
			if ( swf_manager ) {
				swf_manager.RemoveController(this);
			}
		}
	}
}