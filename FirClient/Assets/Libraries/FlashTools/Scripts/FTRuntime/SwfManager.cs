using UnityEngine;
using FTRuntime.Internal;
using System.Collections.Generic;

namespace FTRuntime {
	[ExecuteInEditMode, DisallowMultipleComponent]
	public class SwfManager : MonoBehaviour {
		SwfAssocList<SwfClip>           _clips           = new SwfAssocList<SwfClip>();
		SwfAssocList<SwfClipController> _controllers     = new SwfAssocList<SwfClipController>();
		SwfList<SwfClipController>      _safeUpdates     = new SwfList<SwfClipController>();

		bool                            _isPaused        = false;
		bool                            _useUnscaledDt   = false;
		float                           _rateScale       = 1.0f;
		HashSet<string>                 _groupPauses     = new HashSet<string>();
		HashSet<string>                 _groupUnscales   = new HashSet<string>();
		Dictionary<string, float>       _groupRateScales = new Dictionary<string, float>();

		// ---------------------------------------------------------------------
		//
		// Instance
		//
		// ---------------------------------------------------------------------

		static SwfManager _instance;

		/// <summary>
		/// Get cached manager instance from scene or create it (if allowed)
		/// </summary>
		/// <returns>The manager instance</returns>
		/// <param name="allow_create">If set to <c>true</c> allow create</param>
		public static SwfManager GetInstance(bool allow_create) {
			if ( !_instance ) {
				_instance = FindObjectOfType<SwfManager>();
				if ( allow_create && !_instance ) {
					var go = new GameObject("[SwfManager]");
					_instance = go.AddComponent<SwfManager>();
				}
			}
			return _instance;
		}

		// ---------------------------------------------------------------------
		//
		// Properties
		//
		// ---------------------------------------------------------------------

		/// <summary>
		/// Get animation clip count on scene
		/// </summary>
		/// <value>Clip count</value>
		public int clipCount {
			get { return _clips.Count; }
		}

		/// <summary>
		/// Get animation clip controller count on scene
		/// </summary>
		/// <value>Clip controller count</value>
		public int controllerCount {
			get { return _controllers.Count; }
		}

		/// <summary>
		/// Get or set a value indicating whether animation updates is paused
		/// </summary>
		/// <value><c>true</c> if is paused; otherwise, <c>false</c></value>
		public bool isPaused {
			get { return _isPaused; }
			set { _isPaused = value; }
		}

		/// <summary>
		/// Get or set a value indicating whether animation updates is playing
		/// </summary>
		/// <value><c>true</c> if is playing; otherwise, <c>false</c></value>
		public bool isPlaying {
			get { return !_isPaused; }
			set { _isPaused = !value; }
		}

		/// <summary>
		/// Get or set a value indicating whether animation updates uses unscaled delta time
		/// </summary>
		/// <value><c>true</c> if uses unscaled delta time; otherwise, <c>false</c></value>
		public bool useUnscaledDt {
			get { return _useUnscaledDt; }
			set { _useUnscaledDt = value; }
		}

		/// <summary>
		/// Get or set the global animation rate scale
		/// </summary>
		/// <value>Global rate scale</value>
		public float rateScale {
			get { return _rateScale; }
			set { _rateScale = Mathf.Clamp(value, 0.0f, float.MaxValue); }
		}

		// ---------------------------------------------------------------------
		//
		// Functions
		//
		// ---------------------------------------------------------------------

		/// <summary>
		/// Pause animation updates
		/// </summary>
		public void Pause() {
			isPaused = true;
		}

		/// <summary>
		/// Resume animation updates
		/// </summary>
		public void Resume() {
			isPlaying = true;
		}

		/// <summary>
		/// Pause the group of animations by name
		/// </summary>
		/// <param name="group_name">Group name</param>
		public void PauseGroup(string group_name) {
			if ( !string.IsNullOrEmpty(group_name) ) {
				_groupPauses.Add(group_name);
			}
		}

		/// <summary>
		/// Resume the group of animations by name
		/// </summary>
		/// <param name="group_name">Group name</param>
		public void ResumeGroup(string group_name) {
			if ( !string.IsNullOrEmpty(group_name) ) {
				_groupPauses.Remove(group_name);
			}
		}

		/// <summary>
		/// Determines whether group of animations is paused
		/// </summary>
		/// <returns><c>true</c> if group is paused; otherwise, <c>false</c></returns>
		/// <param name="group_name">Group name</param>
		public bool IsGroupPaused(string group_name) {
			return _groupPauses.Contains(group_name);
		}


		/// <summary>
		/// Determines whether group of animations is playing
		/// </summary>
		/// <returns><c>true</c> if group is playing; otherwise, <c>false</c></returns>
		/// <param name="group_name">Group name</param>
		public bool IsGroupPlaying(string group_name) {
			return !IsGroupPaused(group_name);
		}

		/// <summary>
		/// Set the group of animations use unscaled delta time
		/// </summary>
		/// <param name="group_name">Group name</param>
		/// <param name="yesno"><c>true</c> if group will use unscaled delta time; otherwise, <c>false</c></param>
		public void SetGroupUseUnscaledDt(string group_name, bool yesno) {
			if ( !string.IsNullOrEmpty(group_name) ) {
				if ( yesno ) {
					_groupUnscales.Add(group_name);
				} else {
					_groupUnscales.Remove(group_name);
				}
			}
		}

		/// <summary>
		/// Determines whether group of animations uses unscaled delta time
		/// </summary>
		/// <returns><c>true</c> if group uses unscaled delta time; otherwise, <c>false</c></returns>
		/// <param name="group_name">Group name</param>
		public bool IsGroupUseUnscaledDt(string group_name) {
			return _groupUnscales.Contains(group_name);
		}

		/// <summary>
		/// Set the group of animations rate scale
		/// </summary>
		/// <param name="group_name">Group name</param>
		/// <param name="rate_scale">Rate scale</param>
		public void SetGroupRateScale(string group_name, float rate_scale) {
			if ( !string.IsNullOrEmpty(group_name) ) {
				_groupRateScales[group_name] = Mathf.Clamp(rate_scale, 0.0f, float.MaxValue);
			}
		}

		/// <summary>
		/// Get the group of animations rate scale
		/// </summary>
		/// <returns>The group rate scale</returns>
		/// <param name="group_name">Group name</param>
		public float GetGroupRateScale(string group_name) {
			float rate_scale;
			return _groupRateScales.TryGetValue(group_name, out rate_scale)
				? rate_scale
				: 1.0f;
		}

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		internal void AddClip(SwfClip clip) {
			_clips.Add(clip);
		}

		internal void RemoveClip(SwfClip clip) {
			_clips.Remove(clip);
		}

		internal void GetAllClips(List<SwfClip> clips) {
			_clips.AssignTo(clips);
		}

		internal void AddController(SwfClipController controller) {
			_controllers.Add(controller);
		}

		internal void RemoveController(SwfClipController controller) {
			_controllers.Remove(controller);
		}

		void GrabEnabledClips() {
			var clips = FindObjectsOfType<SwfClip>();
			for ( int i = 0, e = clips.Length; i < e; ++i ) {
				var clip = clips[i];
				if ( clip.enabled ) {
					_clips.Add(clip);
				}
			}
		}

		void GrabEnabledControllers() {
			var controllers = FindObjectsOfType<SwfClipController>();
			for ( int i = 0, e = controllers.Length; i < e; ++i ) {
				var controller = controllers[i];
				if ( controller.enabled ) {
					_controllers.Add(controller);
				}
			}
		}

		void DropClips() {
			_clips.Clear();
		}

		void DropControllers() {
			_controllers.Clear();
		}

		void LateUpdateClips() {
			for ( int i = 0, e = _clips.Count; i < e; ++i ) {
				var clip = _clips[i];
				if ( clip ) {
					clip.Internal_UpdateMesh();
				}
			}
		}

		void LateUpdateControllers(float scaled_dt, float unscaled_dt) {
			_controllers.AssignTo(_safeUpdates);
			for ( int i = 0, e = _safeUpdates.Count; i < e; ++i ) {
				var ctrl = _safeUpdates[i];
				if ( ctrl ) {
					var group_name  = ctrl.groupName;
					if ( string.IsNullOrEmpty(group_name) ) {
						ctrl.Internal_Update(scaled_dt, unscaled_dt);
					} else if ( IsGroupPlaying(group_name) ) {
						var group_rate_scale = GetGroupRateScale(group_name);
						ctrl.Internal_Update(
							group_rate_scale * (IsGroupUseUnscaledDt(group_name) ? unscaled_dt : scaled_dt),
							group_rate_scale * unscaled_dt);
					}
				}
			}
			_safeUpdates.Clear();
		}

		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		void OnEnable() {
			GrabEnabledClips();
			GrabEnabledControllers();
		}

		void OnDisable() {
			DropClips();
			DropControllers();
		}

		void LateUpdate() {
			if ( isPlaying ) {
				LateUpdateControllers(
					rateScale * (useUnscaledDt ? Time.unscaledDeltaTime : Time.deltaTime),
					rateScale * Time.unscaledDeltaTime);
			}
			LateUpdateClips();
		}
	}
}