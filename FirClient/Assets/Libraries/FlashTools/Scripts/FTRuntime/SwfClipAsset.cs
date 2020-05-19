using UnityEngine;
using FTRuntime.Internal;
using System.Collections.Generic;

namespace FTRuntime {
	public class SwfClipAsset : ScriptableObject {
		[System.Serializable]
		public class SubMeshData {
			public int StartVertex;
			public int IndexCount;
		}

		[System.Serializable]
		public class MeshData {
			public SubMeshData[] SubMeshes = new SubMeshData[0];
			public Vector2[]     Vertices  = new Vector2[0];
			public uint[]        UVs       = new uint[0];
			public uint[]        AddColors = new uint[0];
			public uint[]        MulColors = new uint[0];
		}

		[System.Serializable]
		public class Frame {
			public string[]   Labels    = new string[0];
			public MeshData   MeshData  = new MeshData();
			public Material[] Materials = new Material[0];

			public Frame() {
				Labels    = new string[0];
				MeshData  = new MeshData();
				Materials = new Material[0];
			}

			public Frame(string[] labels, MeshData mesh_data, Material[] materials) {
				Labels    = labels;
				MeshData  = mesh_data;
				Materials = materials;
			}

			Mesh _cachedMesh = null;
			public Mesh CachedMesh {
				get {
					if ( !_cachedMesh ) {
						_cachedMesh = new Mesh();
						_cachedMesh.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor;
						SwfUtils.FillGeneratedMesh(_cachedMesh, MeshData);
					}
					return _cachedMesh;
				}
			}
		}

		[System.Serializable]
		public class Sequence {
			public string      Name   = string.Empty;
			public List<Frame> Frames = new List<Frame>();
		}

		[SwfReadOnly]
		public string          Name;
		[SwfReadOnly]
		public Sprite          Sprite;
		[SwfReadOnly]
		public float           FrameRate;
		[HideInInspector]
		public string          AssetGUID;
		[HideInInspector]
		public List<Sequence>  Sequences;

		void Reset() {
			Name      = string.Empty;
			Sprite    = null;
			FrameRate = 1.0f;
			AssetGUID = string.Empty;
			Sequences = new List<Sequence>();
		}
	}
}