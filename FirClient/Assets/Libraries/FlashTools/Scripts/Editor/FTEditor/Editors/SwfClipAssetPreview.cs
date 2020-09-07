using UnityEngine;
using UnityEditor;

using System.Linq;

using FTRuntime;

namespace FTEditor.Editors {
	class SwfClipAssetPreview : ObjectPreview {
		int                   _sequence     = 0;
		MaterialPropertyBlock _matPropBlock = null;
		PreviewRenderUtility  _previewUtils = null;

		Sprite targetSprite {
			get {
				var clip = target as SwfClipAsset;
				return clip ? clip.Sprite : null;
			}
		}

		Texture2D targetAtlas {
			get {
				var sprite = targetSprite;
				return sprite ? sprite.texture : null;
			}
		}

		Texture2D targetAtlasA {
			get {
				var sprite = targetSprite;
				return sprite ? sprite.associatedAlphaSplitTexture : null;
			}
		}

		SwfClipAsset.Frame targetFrame {
			get {
				var clip = target as SwfClipAsset;
				return GetFrameForClip(clip, _sequence);
			}
		}

		SwfClipAsset.Sequence targetSequence {
			get {
				var clip = target as SwfClipAsset;
				return GetSequenceForClip(clip, _sequence);
			}
		}

		bool isTargetValidForPreview {
			get {
				var atlas    = targetAtlas;
				var frame    = targetFrame;
				var sequence = targetSequence;
				return
					atlas &&
					frame != null &&
					sequence != null &&
					frame.CachedMesh && frame.CachedMesh.vertexCount > 0;
			}
		}

		static SwfClipAsset.Frame GetFrameForClip(SwfClipAsset clip, int sequence_index) {
			var sequence = GetSequenceForClip(clip, sequence_index);
			var frames = sequence != null && sequence.Frames != null && sequence.Frames.Count > 0
				? sequence.Frames
				: null;
			var frame_time = (float)(EditorApplication.timeSinceStartup * clip.FrameRate);
			return frames != null
				? frames[Mathf.FloorToInt(frame_time) % frames.Count]
				: null;
		}

		static SwfClipAsset.Sequence GetSequenceForClip(SwfClipAsset clip, int sequence_index) {
			return clip && clip.Sequences != null && clip.Sequences.Count > 0
				? clip.Sequences[Mathf.Abs(sequence_index) % clip.Sequences.Count]
				: null;
		}

		static Bounds CalculateBoundsForSequence(SwfClipAsset.Sequence sequence) {
			var bounds = sequence != null && sequence.Frames != null && sequence.Frames.Count > 0
				? sequence.Frames
					.Where (p => !!p.CachedMesh)
					.Select(p => p.CachedMesh.bounds)
				: new Bounds[0];
			var result = bounds.Any() ? bounds.First() : new Bounds();
			foreach ( var bound in bounds ) {
				result.Encapsulate(bound);
			}
			return result;
		}

		static void ConfigureCameraForSequence(Camera camera, SwfClipAsset.Sequence sequence) {
			var bounds              = CalculateBoundsForSequence(sequence);
			camera.orthographic     = true;
			camera.orthographicSize = Mathf.Max(
				Mathf.Abs(bounds.extents.x),
				Mathf.Abs(bounds.extents.y));
			camera.transform.position = new Vector3(
				bounds.center.x,
				bounds.center.y,
				-10.0f);
		}

		static Camera GetCameraFromPreviewUtils(PreviewRenderUtility previewUtils) {
			var cameraField = previewUtils.GetType().GetField("m_Camera");
			var cameraFieldValue = cameraField != null
				? cameraField.GetValue(previewUtils) as Camera
				: null;
			if ( cameraFieldValue ) {
				return cameraFieldValue;
			}
			var cameraProperty = previewUtils.GetType().GetProperty("camera");
			var cameraPropertyValue = cameraProperty != null
				? cameraProperty.GetValue(previewUtils, null) as Camera
				: null;
			if ( cameraPropertyValue ) {
				return cameraPropertyValue;
			}
			return null;
		}

		// ---------------------------------------------------------------------
		//
		// Functions
		//
		// ---------------------------------------------------------------------

		public void SetSequence(string sequence_name) {
			var clip = target as SwfClipAsset;
			_sequence = clip && clip.Sequences != null
				? Mathf.Max(0, clip.Sequences.FindIndex(p => p.Name == sequence_name))
				: 0;
		}

		public void Shutdown() {
			_matPropBlock.Clear();
			_previewUtils.Cleanup();
		}

		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		public override void Initialize(Object[] targets) {
			base.Initialize(targets);
			if ( _matPropBlock == null ) {
				_matPropBlock = new MaterialPropertyBlock();
			}
			if ( _previewUtils == null ) {
				_previewUtils = new PreviewRenderUtility();
			}
		}

		public override bool HasPreviewGUI() {
			return true;
		}

		public override void OnPreviewSettings() {
			var clip = m_Targets.Length == 1
				? m_Targets[0] as SwfClipAsset
				: null;

			if ( !clip || clip.Sequences == null ) {
				return;
			}

			if ( clip.Sequences.Count > 1 ) {
				if ( GUILayout.Button("<", EditorStyles.miniButton) ) {
					--_sequence;
					if ( _sequence < 0 ) {
						_sequence = clip.Sequences.Count - 1;
					}
				}
			}

			var sequence = GetSequenceForClip(clip, _sequence);
			if ( sequence != null && !string.IsNullOrEmpty(sequence.Name) ) {
				GUILayout.Label(sequence.Name, EditorStyles.whiteLabel);
			}

			if ( clip.Sequences.Count > 1 ) {
				if ( GUILayout.Button(">", EditorStyles.miniButton) ) {
					++_sequence;
					if ( _sequence >= clip.Sequences.Count ) {
						_sequence = 0;
					}
				}
			}
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background) {
			if ( Event.current.type == EventType.Repaint ) {
				if ( isTargetValidForPreview ) {
					_previewUtils.BeginPreview(r, background);
					{
						_matPropBlock.SetTexture(
							"_MainTex",
							targetAtlas ? targetAtlas : Texture2D.whiteTexture);
						if ( targetAtlasA ) {
							_matPropBlock.SetTexture("_AlphaTex", targetAtlasA);
							_matPropBlock.SetFloat("_ExternalAlpha", 1.0f);
						} else {
							_matPropBlock.SetTexture("_AlphaTex", Texture2D.whiteTexture);
							_matPropBlock.SetFloat("_ExternalAlpha", 0.0f);
						}
						var camera = GetCameraFromPreviewUtils(_previewUtils);
						if ( camera ) {
							ConfigureCameraForSequence(camera, targetSequence);
							var frame = targetFrame;
							for ( var i = 0; i < frame.Materials.Length; ++i ) {
								_previewUtils.DrawMesh(
									frame.CachedMesh,
									Matrix4x4.identity,
									frame.Materials[i],
									i,
									_matPropBlock);
							}
							camera.Render();
						}
					}
					_previewUtils.EndAndDrawPreview(r);
				}
			}
		}
	}
}