using UnityEngine;
using UnityEditor;

using System.Linq;
using System.Collections.Generic;

using FTRuntime;

namespace FTEditor.Editors {
	[CustomEditor(typeof(SwfClip)), CanEditMultipleObjects]
	class SwfClipEditor : Editor {
		bool                                     _outdated = false;
		List<SwfClip>                            _clips    = new List<SwfClip>();
		Dictionary<SwfClip, SwfClipAssetPreview> _previews = new Dictionary<SwfClip, SwfClipAssetPreview>();

		void AllClipsForeachWithUndo(System.Action<SwfClip> act) {
			Undo.RecordObjects(_clips.ToArray(), "Inspector");
			foreach ( var clip in _clips ) {
				act(clip);
				EditorUtility.SetDirty(clip);
			}
		}

		int GetMinClipsFrameCount() {
			return _clips.Count > 0
				? _clips.Min(clip => clip.frameCount)
				: 0;
		}

		string GetClipsFrameCountForView() {
			return _clips.Aggregate(string.Empty, (acc, clip) => {
				var frame_count     = clip.frameCount;
				var frame_count_str = frame_count.ToString();
				return string.IsNullOrEmpty(acc)
					? frame_count_str
					: (acc != frame_count_str ? "--" : acc);
			});
		}

		string GetClipsCurrentFrameForView() {
			return _clips.Aggregate(string.Empty, (acc, clip) => {
				var current_frame     = clip.currentFrame + 1;
				var current_frame_str = current_frame.ToString();
				return string.IsNullOrEmpty(acc)
					? current_frame_str
					: (acc != current_frame_str ? "--" : acc);
			});
		}

		List<string> GetAllSequences(bool include_empty) {
			var result       = new List<string>();
			var result_clips = _clips
				.Where (p => p.clip && p.clip.Sequences.Count > 0)
				.Select(p => p.clip.Sequences)
				.Where (p => p.All(s => !string.IsNullOrEmpty(s.Name)))
				.ToList();
			if ( result_clips.Count > 0 ) {
				result = result_clips.First()
					.Select(p => p.Name)
					.ToList();
				var sequences_enum = result_clips
					.Select(p => p.Select(s => s.Name));
				foreach ( var sequences in sequences_enum ) {
					result = result
						.Where(p => sequences.Contains(p))
						.ToList();
				}
				if ( include_empty ) {
					result.Add(string.Empty);
				}
			}
			return result;
		}

		void DrawGUINotes() {
			if ( _outdated ) {
				SwfEditorUtils.DrawOutdatedGUINotes("SwfClip", _clips);
			}
		}

		void DrawSequence() {
			var all_sequences = GetAllSequences(true);
			if ( all_sequences.Count > 0 ) {
				var sequence_prop = SwfEditorUtils.GetPropertyByName(serializedObject, "_sequence");
				SwfEditorUtils.DoWithMixedValue(
					sequence_prop.hasMultipleDifferentValues, () => {
						var sequence_index = EditorGUILayout.Popup(
							"Sequence",
							sequence_prop.hasMultipleDifferentValues
								? all_sequences.FindIndex(p => string.IsNullOrEmpty(p))
								: all_sequences.FindIndex(p => p == sequence_prop.stringValue),
							all_sequences.ToArray());
						if ( sequence_index >= 0 && sequence_index < all_sequences.Count ) {
							var new_sequence = all_sequences[sequence_index];
							if ( !string.IsNullOrEmpty(new_sequence) ) {
								if ( sequence_prop.hasMultipleDifferentValues ) {
									sequence_prop.stringValue = string.Empty;
								}
								sequence_prop.stringValue = new_sequence;
								sequence_prop.serializedObject.ApplyModifiedProperties();
							}
						}
					});
			}
		}

		void DrawCurrentFrame() {
			var min_frame_count = GetMinClipsFrameCount();
			if ( min_frame_count > 1 ) {
				EditorGUILayout.IntSlider(
					SwfEditorUtils.GetPropertyByName(serializedObject, "_currentFrame"),
					0,
					min_frame_count - 1,
					"Current frame");
				DrawClipControls();
			}
		}

		void DrawClipControls() {
			EditorGUILayout.Space();
			SwfEditorUtils.DoCenterHorizontalGUI(() => {
				if ( GUILayout.Button(new GUIContent("<<", "to begin frame")) ) {
					AllClipsForeachWithUndo(p => p.ToBeginFrame());
				}
				if ( GUILayout.Button(new GUIContent("<", "to prev frame")) ) {
					AllClipsForeachWithUndo(p => p.ToPrevFrame());
				}
				GUILayout.Label(string.Format(
					"{0}/{1}",
					GetClipsCurrentFrameForView(), GetClipsFrameCountForView()));
				if ( GUILayout.Button(new GUIContent(">", "to next frame")) ) {
					AllClipsForeachWithUndo(p => p.ToNextFrame());
				}
				if ( GUILayout.Button(new GUIContent(">>", "to end frame")) ) {
					AllClipsForeachWithUndo(p => p.ToEndFrame());
				}
			});
		}

		void SetupPreviews() {
			ShutdownPreviews();
			_previews = targets
				.OfType<SwfClip>()
				.Where(p => p.clip)
				.ToDictionary(p => p, p => {
					var preview = new SwfClipAssetPreview();
					preview.Initialize(new Object[] { p.clip });
					return preview;
				});
		}

		void ShutdownPreviews() {
			foreach ( var p in _previews ) {
		        p.Value.Shutdown();
			}
			_previews.Clear();
		}

		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		void OnEnable() {
			_clips = targets.OfType<SwfClip>().ToList();
			_outdated = SwfEditorUtils.CheckForOutdatedAsset(_clips);
			SetupPreviews();
		}

		void OnDisable() {
			ShutdownPreviews();
			_clips.Clear();
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();
			DrawGUINotes();
			DrawDefaultInspector();
			DrawSequence();
			DrawCurrentFrame();
			if ( GUI.changed ) {
				serializedObject.ApplyModifiedProperties();
				SetupPreviews();
			}
		}

		public override bool RequiresConstantRepaint() {
			return _clips.Count > 0;
		}

		public override bool HasPreviewGUI() {
			return _clips.Count > 0;
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background) {
			if ( Event.current.type == EventType.Repaint ) {
				SwfClipAssetPreview preview;
				var clip = target as SwfClip;
				if ( _previews.TryGetValue(clip, out preview) ) {
					preview.SetSequence(clip.sequence);
					preview.DrawPreview(r);
				}
			}
		}
	}
}