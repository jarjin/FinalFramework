using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections.Generic;

using FTRuntime;

namespace FTEditor.Editors {
	[CustomEditor(typeof(SwfManager))]
	class SwfManagerEditor : Editor {
		SwfManager              _manager       = null;
		List<SwfClipController> _controllers   = new List<SwfClipController>();
		bool                    _groupsFoldout = true;

		void DrawCounts() {
			SwfEditorUtils.DoWithEnabledGUI(false, () => {
				EditorGUILayout.IntField(
					"Clip count",
					_manager.clipCount);
				EditorGUILayout.IntField(
					"Controller count",
					_manager.controllerCount);
			});
		}

		void DrawControls() {
			SwfEditorUtils.DoRightHorizontalGUI(() => {
				if ( _manager.useUnscaledDt && GUILayout.Button("Use Scaled Dt") ) {
					_manager.useUnscaledDt = false;
				}
				if ( !_manager.useUnscaledDt && GUILayout.Button("Use Unscaled Dt") ) {
					_manager.useUnscaledDt = true;
				}
				if ( _manager.isPaused && GUILayout.Button("Resume") ) {
					_manager.Resume();
				}
				if ( _manager.isPlaying && GUILayout.Button("Pause") ) {
					_manager.Pause();
				}
			});
		}

		void DrawGroupControls() {
			var group_names = GetAllGroupNames();
			if ( group_names.Count > 0 ) {
				_groupsFoldout = EditorGUILayout.Foldout(_groupsFoldout, "Groups");
				if ( _groupsFoldout ) {
					foreach ( var group_name in group_names ) {
						SwfEditorUtils.DoWithEnabledGUI(false, () => {
							EditorGUILayout.TextField("Name", group_name);
						});
						{
							EditorGUI.BeginChangeCheck();
							var new_rate_scale = EditorGUILayout.FloatField(
								"Rate Scale", _manager.GetGroupRateScale(group_name));
							if ( EditorGUI.EndChangeCheck() ) {
								_manager.SetGroupRateScale(group_name, new_rate_scale);
							}
						}
						{
							EditorGUI.BeginChangeCheck();
							var new_user_unscaled_dt = EditorGUILayout.Toggle(
								"Use Unscaled Dt", _manager.IsGroupUseUnscaledDt(group_name));
							if ( EditorGUI.EndChangeCheck() ) {
								_manager.SetGroupUseUnscaledDt(group_name, new_user_unscaled_dt);
							}
						}
						SwfEditorUtils.DoRightHorizontalGUI(() => {
							if ( _manager.IsGroupPaused(group_name) && GUILayout.Button("Resume") ) {
								_manager.ResumeGroup(group_name);
							}
							if ( _manager.IsGroupPlaying(group_name) && GUILayout.Button("Pause") ) {
								_manager.PauseGroup(group_name);
							}
						});
					}
				}
			}
		}

		HashSet<string> GetAllGroupNames() {
			var result = new HashSet<string>();
			for ( int i = 0, e = _controllers.Count; i < e; ++i ) {
				var ctrl = _controllers[i];
				if ( !string.IsNullOrEmpty(ctrl.groupName) ) {
					result.Add(ctrl.groupName);
				}
			}
			return result;
		}

		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		void OnEnable() {
			_manager     = target as SwfManager;
			_controllers = FindObjectsOfType<SwfClipController>().ToList();
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();
			DrawDefaultInspector();
			DrawCounts();
			if ( Application.isPlaying ) {
				DrawControls();
				DrawGroupControls();
			}
			if ( GUI.changed ) {
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}