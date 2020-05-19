using UnityEngine;
using UnityEngine.Rendering;
using FTRuntime.Internal;

namespace FTRuntime {
	[ExecuteInEditMode, DisallowMultipleComponent]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(SortingGroup))]
	public class SwfClip : MonoBehaviour {

		MeshFilter            _meshFilter   = null;
		MeshRenderer          _meshRenderer = null;
		SortingGroup          _sortingGroup = null;

		bool                  _dirtyMesh    = true;
		SwfClipAsset.Sequence _curSequence  = null;
		MaterialPropertyBlock _curPropBlock = null;

		// ---------------------------------------------------------------------
		//
		// Events
		//
		// ---------------------------------------------------------------------

		/// <summary>
		/// Occurs when clip changes
		/// </summary>
		public event System.Action<SwfClip> OnChangeClipEvent;

		/// <summary>
		/// Occurs when sequence changes
		/// </summary>
		public event System.Action<SwfClip> OnChangeSequenceEvent;

		/// <summary>
		/// Occurs when current frame changes
		/// </summary>
		public event System.Action<SwfClip> OnChangeCurrentFrameEvent;

		// ---------------------------------------------------------------------
		//
		// Serialized fields
		//
		// ---------------------------------------------------------------------

		[Header("Sorting")]

		[SerializeField, SwfSortingLayer]
		string _sortingLayer = string.Empty;

		[SerializeField]
		int _sortingOrder = 0;

		[Header("Animation")]

		[SerializeField]
		Color _tint = Color.white;

		[SerializeField]
		SwfClipAsset _clip = null;

		[SerializeField, HideInInspector]
		string _sequence = string.Empty;

		[SerializeField, HideInInspector]
		int _currentFrame = 0;

		// ---------------------------------------------------------------------
		//
		// Properties
		//
		// ---------------------------------------------------------------------

		/// <summary>
		/// Gets or sets the animation mesh renderer sorting layer
		/// </summary>
		/// <value>The sorting layer</value>
		public string sortingLayer {
			get { return _sortingLayer; }
			set {
				_sortingLayer = value;
				ChangeSortingProperties();
			}
		}

		/// <summary>
		/// Gets or sets the animation mesh renderer sorting order
		/// </summary>
		/// <value>The sorting order</value>
		public int sortingOrder {
			get { return _sortingOrder; }
			set {
				_sortingOrder = value;
				ChangeSortingProperties();
			}
		}

		/// <summary>
		/// Gets or sets the animation tint color
		/// </summary>
		/// <value>The tint color</value>
		public Color tint {
			get { return _tint; }
			set {
				_tint = value;
				ChangeTint();
			}
		}

		/// <summary>
		/// Gets or sets the animation asset (reset sequence and current frame)
		/// </summary>
		/// <value>The animation asset</value>
		public SwfClipAsset clip {
			get { return _clip; }
			set {
				_clip         = value;
				_sequence     = string.Empty;
				_currentFrame = 0;
				ChangeClip();
				EmitChangeEvents(true, true, true);
			}
		}

		/// <summary>
		/// Gets or sets the animation sequence (reset current frame)
		/// </summary>
		/// <value>The animation sequence</value>
		public string sequence {
			get { return _sequence; }
			set {
				_sequence     = value;
				_currentFrame = 0;
				ChangeSequence();
				EmitChangeEvents(false, true, true);
			}
		}

		/// <summary>
		/// Gets or sets the animation current frame
		/// </summary>
		/// <value>The animation current frame</value>
		public int currentFrame {
			get { return _currentFrame; }
			set {
				_currentFrame = value;
				ChangeCurrentFrame();
				EmitChangeEvents(false, false, true);
			}
		}

		/// <summary>
		/// Gets the current animation sequence frame count
		/// </summary>
		/// <value>The frame count</value>
		public int frameCount {
			get {
				return _curSequence != null && _curSequence.Frames != null
					? _curSequence.Frames.Count
					: 0;
			}
		}

		/// <summary>
		/// Gets the animation frame rate
		/// </summary>
		/// <value>The frame rate</value>
		public float frameRate {
			get {
				return clip
					? clip.FrameRate
					: 0.0f;
			}
		}

		/// <summary>
		/// Gets the current frame label count
		/// </summary>
		/// <value>The frame label count</value>
		public int currentLabelCount {
			get {
				var baked_frame  = GetCurrentBakedFrame();
				var frame_labels = baked_frame != null ? baked_frame.Labels : null;
				return frame_labels != null ? frame_labels.Length : 0;
			}
		}

		/// <summary>
		/// Gets the current frame mesh bounding volume in local space
		/// (Since 1.3.8)
		/// </summary>
		/// <value>The bounding volume in local space</value>
		public Bounds currentLocalBounds {
			get {
				var frame = GetCurrentBakedFrame();
				return frame != null
					? frame.CachedMesh.bounds
					: new Bounds();
			}
		}

		/// <summary>
		/// Gets the current frame mesh bounding volume in world space
		/// (Since 1.3.8)
		/// </summary>
		/// <value>The bounding volume in world space</value>
		public Bounds currentWorldBounds {
			get {
				Internal_UpdateMesh();
				return _meshRenderer
					? _meshRenderer.bounds
					: new Bounds();
			}
		}

		// ---------------------------------------------------------------------
		//
		// Functions
		//
		// ---------------------------------------------------------------------

		/// <summary>
		/// Rewind current sequence to begin frame
		/// </summary>
		public void ToBeginFrame() {
			currentFrame = 0;
		}

		/// <summary>
		/// Rewind current sequence to end frame
		/// </summary>
		public void ToEndFrame() {
			currentFrame = frameCount > 0
				? frameCount - 1
				: 0;
		}

		/// <summary>
		/// Rewind current sequence to previous frame
		/// </summary>
		/// <returns><c>true</c>, if animation was rewound, <c>false</c> otherwise</returns>
		public bool ToPrevFrame() {
			if ( currentFrame > 0 ) {
				--currentFrame;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Rewind current sequence to next frame
		/// </summary>
		/// <returns><c>true</c>, if animation was rewound, <c>false</c> otherwise</returns>
		public bool ToNextFrame() {
			if ( currentFrame < frameCount - 1 ) {
				++currentFrame;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the current frame label by index
		/// </summary>
		/// <returns>The current frame label</returns>
		/// <param name="index">Current frame label index</param>
		public string GetCurrentFrameLabel(int index) {
			var baked_frame  = GetCurrentBakedFrame();
			var frame_labels = baked_frame != null ? baked_frame.Labels : null;
			return frame_labels != null && index >= 0 && index < frame_labels.Length
				? frame_labels[index]
				: string.Empty;
		}

		// ---------------------------------------------------------------------
		//
		// Internal
		//
		// ---------------------------------------------------------------------

		internal void Internal_UpdateMesh() {
			if ( _meshFilter && _meshRenderer && _dirtyMesh ) {
				var baked_frame = GetCurrentBakedFrame();
				if ( baked_frame != null ) {
					_meshFilter  .sharedMesh      = baked_frame.CachedMesh;
					_meshRenderer.sharedMaterials = baked_frame.Materials;
				} else {
					_meshFilter  .sharedMesh      = null;
					_meshRenderer.sharedMaterials = new Material[0];
				}
				_dirtyMesh = false;
			}
		}

		/// <summary>
		/// Update all animation properties (for internal use only)
		/// </summary>
		public void Internal_UpdateAllProperties() {
			ClearCache(false);
			ChangeTint();
			ChangeClip();
			ChangeSequence();
			ChangeCurrentFrame();
			ChangeSortingProperties();
		}

		void ClearCache(bool allow_to_create_components) {
			_meshFilter   = SwfUtils.GetComponent<MeshFilter>  (gameObject, allow_to_create_components);
			_meshRenderer = SwfUtils.GetComponent<MeshRenderer>(gameObject, allow_to_create_components);
			_sortingGroup = SwfUtils.GetComponent<SortingGroup>(gameObject, allow_to_create_components);
			_dirtyMesh    = true;
			_curSequence  = null;
			_curPropBlock = null;
		}

		void ChangeTint() {
			UpdatePropBlock();
		}

		void ChangeClip() {
			if ( _meshRenderer ) {
				_meshRenderer.enabled = !!clip;
			}
			ChangeSequence();
			UpdatePropBlock();
		}

		void ChangeSequence() {
			_curSequence = null;
			if ( clip && clip.Sequences != null ) {
				if ( !string.IsNullOrEmpty(sequence) ) {
					for ( int i = 0, e = clip.Sequences.Count; i < e; ++i ) {
						var clip_sequence = clip.Sequences[i];
						if ( clip_sequence != null && clip_sequence.Name == sequence ) {
							_curSequence = clip_sequence;
							break;
						}
					}
					if ( _curSequence == null ) {
						Debug.LogWarningFormat(this,
							"<b>[FlashTools]</b> Sequence '{0}' not found",
							sequence);
					}
				}
				if ( _curSequence == null ) {
					for ( int i = 0, e = clip.Sequences.Count; i < e; ++i ) {
						var clip_sequence = clip.Sequences[i];
						if ( clip_sequence != null ) {
							_sequence    = clip_sequence.Name;
							_curSequence = clip_sequence;
							break;
						}
					}
				}
			}
			ChangeCurrentFrame();
		}

		void ChangeCurrentFrame() {
			_dirtyMesh    = true;
			_currentFrame = frameCount > 0
				? Mathf.Clamp(currentFrame, 0, frameCount - 1)
				: 0;
		}

		void ChangeSortingProperties() {
			if ( _meshRenderer ) {
				_meshRenderer.sortingOrder     = sortingOrder;
				_meshRenderer.sortingLayerName = sortingLayer;
			}
			if ( _sortingGroup ) {
				_sortingGroup.sortingOrder     = sortingOrder;
				_sortingGroup.sortingLayerName = sortingLayer;
			}
		}

		void UpdatePropBlock() {
			if ( _meshRenderer ) {
				if ( _curPropBlock == null ) {
					_curPropBlock = new MaterialPropertyBlock();
				}
				_meshRenderer.GetPropertyBlock(_curPropBlock);
				_curPropBlock.SetColor(SwfUtils.TintShaderProp, tint);
				var sprite = clip ? clip.Sprite : null;
				var atlas  = sprite && sprite.texture ? sprite.texture : Texture2D.whiteTexture;
				var atlasA = sprite ? sprite.associatedAlphaSplitTexture : null;
				_curPropBlock.SetTexture(
					SwfUtils.MainTexShaderProp,
					atlas ? atlas : Texture2D.whiteTexture);
				if ( atlasA ) {
					_curPropBlock.SetTexture(SwfUtils.AlphaTexShaderProp, atlasA);
					_curPropBlock.SetFloat(SwfUtils.ExternalAlphaShaderProp, 1.0f);
				} else {
					_curPropBlock.SetTexture(SwfUtils.AlphaTexShaderProp, Texture2D.whiteTexture);
					_curPropBlock.SetFloat(SwfUtils.ExternalAlphaShaderProp, 0.0f);
				}
				_meshRenderer.SetPropertyBlock(_curPropBlock);
			}
		}

		void EmitChangeEvents(bool clip, bool sequence, bool current_frame) {
			if ( clip && OnChangeClipEvent != null ) {
				OnChangeClipEvent(this);
			}
			if ( sequence && OnChangeSequenceEvent != null ) {
				OnChangeSequenceEvent(this);
			}
			if ( current_frame && OnChangeCurrentFrameEvent != null ) {
				OnChangeCurrentFrameEvent(this);
			}
		}

		SwfClipAsset.Frame GetCurrentBakedFrame() {
			var frames = _curSequence != null ? _curSequence.Frames : null;
			return frames != null && currentFrame >= 0 && currentFrame < frames.Count
				? frames[currentFrame]
				: null;
		}

		// ---------------------------------------------------------------------
		//
		// Messages
		//
		// ---------------------------------------------------------------------

		void Start() {
			ClearCache(true);
			Internal_UpdateAllProperties();
			EmitChangeEvents(true, true, true);
		}

		void OnEnable() {
			var swf_manager = SwfManager.GetInstance(true);
			if ( swf_manager ) {
				swf_manager.AddClip(this);
			}
		}

		void OnDisable() {
			var swf_manager = SwfManager.GetInstance(false);
			if ( swf_manager ) {
				swf_manager.RemoveClip(this);
			}
		}

		void Reset() {
			Internal_UpdateAllProperties();
		}

		void OnValidate() {
			Internal_UpdateAllProperties();
		}
	}
}