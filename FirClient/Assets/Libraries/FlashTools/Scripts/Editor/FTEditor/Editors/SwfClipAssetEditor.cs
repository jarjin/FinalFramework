using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

using System.IO;
using System.Linq;
using System.Collections.Generic;

using FTRuntime;

namespace FTEditor.Editors {
	[CustomEditor(typeof(SwfClipAsset)), CanEditMultipleObjects]
	class SwfClipAssetEditor : Editor {
		bool                _outdated = false;
		List<SwfClipAsset>  _clips    = new List<SwfClipAsset>();
		SwfClipAssetPreview _preview  = null;

		static string GetClipPath(SwfClipAsset clip) {
			return clip
				? AssetDatabase.GetAssetPath(clip)
				: string.Empty;
		}

		static string GetPrefabPath(SwfClipAsset clip) {
			var clip_path = GetClipPath(clip);
			return string.IsNullOrEmpty(clip_path)
				? string.Empty
				: Path.ChangeExtension(clip_path, ".prefab");
		}

		static int GetFrameCount(SwfClipAsset clip) {
			return clip != null ? clip.Sequences.Aggregate(0, (acc, seq) => {
				return seq.Frames.Count + acc;
			}) : 0;
		}

		//
		//
		//

		static GameObject CreateClipGO(SwfClipAsset clip) {
			if ( clip ) {
				var clip_go = new GameObject(clip.name);
				clip_go.AddComponent<MeshFilter>();
				clip_go.AddComponent<MeshRenderer>();
				clip_go.AddComponent<SortingGroup>();
				clip_go.AddComponent<SwfClip>().clip = clip;
				clip_go.AddComponent<SwfClipController>();
				return clip_go;
			}
			return null;
		}

		static GameObject CreateClipPrefab(SwfClipAsset clip) {
			GameObject result = null;
			var clip_go = CreateClipGO(clip);
			if ( clip_go ) {
				var prefab_path = GetPrefabPath(clip);
				if ( !string.IsNullOrEmpty(prefab_path) ) {
					var prefab = AssetDatabase.LoadMainAssetAtPath(prefab_path);
					if ( !prefab ) {
						prefab = PrefabUtility.CreateEmptyPrefab(prefab_path);
					}
					result = PrefabUtility.ReplacePrefab(
						clip_go,
						prefab,
						ReplacePrefabOptions.ConnectToPrefab);
				}
				GameObject.DestroyImmediate(clip_go, true);
			}
			return result;
		}

		static GameObject CreateClipOnScene(SwfClipAsset clip) {
			var clip_go = CreateClipGO(clip);
			if ( clip_go ) {
				Undo.RegisterCreatedObjectUndo(clip_go, "Instance SwfClip");
			}
			return clip_go;
		}

		//
		//
		//

		void CreateAllClipsPrefabs() {
			var objects = _clips
				.Select (p => CreateClipPrefab(p))
				.Where  (p => !!p)
				.ToArray();
			Selection.objects = objects;
			foreach ( var obj in objects ) {
				EditorGUIUtility.PingObject(obj);
			}
		}

		void CreateAllClipsOnScene() {
			var objects = _clips
				.Select (p => CreateClipOnScene(p))
				.Where  (p => !!p)
				.ToArray();
			Selection.objects = objects;
			foreach ( var obj in objects ) {
				EditorGUIUtility.PingObject(obj);
			}
		}

		//
		//
		//

		void DrawGUIFrameCount() {
			var counts      = _clips.Select(p => GetFrameCount(p));
			var mixed_value = counts.GroupBy(p => p).Count() > 1;
			SwfEditorUtils.DoWithEnabledGUI(false, () => {
				SwfEditorUtils.DoWithMixedValue(
					mixed_value, () => {
						EditorGUILayout.IntField("Frame count", counts.First());
					});
			});
		}

		void DrawGUISequences() {
			SwfEditorUtils.DoWithEnabledGUI(false, () => {
				var sequences_prop = SwfEditorUtils.GetPropertyByName(
					serializedObject, "Sequences");
				if ( sequences_prop.isArray ) {
					SwfEditorUtils.DoWithMixedValue(
						sequences_prop.hasMultipleDifferentValues, () => {
							EditorGUILayout.IntField("Sequence count", sequences_prop.arraySize);
						});
				}
			});
		}

		void DrawGUISourceAsset() {
			var asset_guids = _clips.Select(p => p.AssetGUID);
			var mixed_value = asset_guids.GroupBy(p => p).Count() > 1;
			SwfEditorUtils.DoWithEnabledGUI(false, () => {
				SwfEditorUtils.DoWithMixedValue(
					mixed_value, () => {
						var source_asset = AssetDatabase.LoadAssetAtPath<SwfAsset>(
							AssetDatabase.GUIDToAssetPath(asset_guids.First()));
						EditorGUILayout.ObjectField(
							"Source Asset", source_asset, typeof(SwfAsset), false);
					});
			});
		}

		void DrawGUIControls() {
			SwfEditorUtils.DoHorizontalGUI(() => {
				if ( GUILayout.Button("Create prefab") ) {
					CreateAllClipsPrefabs();
				}
				if ( GUILayout.Button("Instance to scene") ) {
					CreateAllClipsOnScene();
				}
			});
		}

		void DrawGUINotes() {
			SwfEditorUtils.DrawMasksGUINotes();
			if ( _outdated ) {
				SwfEditorUtils.DrawOutdatedGUINotes("SwfClipAsset", _clips);
			}
		}

		//
		//
		//

		void SetupPreviews() {
			ShutdownPreviews();
			_preview = new SwfClipAssetPreview();
			_preview.Initialize(targets
				.OfType<SwfClipAsset>()
				.Where(p => p)
				.ToArray());
		}

		void ShutdownPreviews() {
			if ( _preview != null ) {
				_preview.Shutdown();
				_preview = null;
			}
		}

		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		void OnEnable() {
			_clips = targets.OfType<SwfClipAsset>().ToList();
			_outdated = SwfEditorUtils.CheckForOutdatedAsset(_clips);
			SetupPreviews();
		}

		void OnDisable() {
			ShutdownPreviews();
			_clips.Clear();
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();
			DrawDefaultInspector();
			DrawGUIFrameCount();
			DrawGUISequences();
			DrawGUISourceAsset();
			DrawGUIControls();
			DrawGUINotes();
			if ( GUI.changed ) {
				serializedObject.ApplyModifiedProperties();
			}
		}

		public override bool RequiresConstantRepaint() {
			return _clips.Count > 0;
		}

		public override bool HasPreviewGUI() {
			return _clips.Count > 0;
		}

		public override void OnPreviewSettings() {
			if ( _preview != null ) {
				_preview.OnPreviewSettings();
			}
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background) {
			if ( _preview != null ) {
				_preview.OnPreviewGUI(r, background);
			}
		}
	}
}