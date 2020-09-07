using UnityEngine;
using System.Collections.Generic;

namespace FTRuntime.Internal {
	static class SwfUtils {

		//
		// UnpackUV/UnpackFColorFromUInts
		//

		const ushort UShortMax          = ushort.MaxValue;
		const float  FColorPrecision    = 1.0f / 512.0f;
		const float  InvFColorPrecision = 1.0f / FColorPrecision;

		public static void UnpackUV(uint pack, out float u, out float v) {
			u = (float)((pack >> 16) & 0xFFFF) / UShortMax;
			v = (float)((pack      ) & 0xFFFF) / UShortMax;
		}

		public static void UnpackFColorFromUInts(
			uint pack0, uint pack1,
			out float c0, out float c1, out float c2, out float c3)
		{
			c0 = (short)((pack0 >> 16) & 0xFFFF) / InvFColorPrecision;
			c1 = (short)((pack0      ) & 0xFFFF) / InvFColorPrecision;
			c2 = (short)((pack1 >> 16) & 0xFFFF) / InvFColorPrecision;
			c3 = (short)((pack1      ) & 0xFFFF) / InvFColorPrecision;
		}

		//
		// Shader properties
		//

		public static int TintShaderProp {
			get {
				ShaderPropsCache.LazyInitialize();
				return ShaderPropsCache.TintShaderProp;
			}
		}

		public static int MainTexShaderProp {
			get {
				ShaderPropsCache.LazyInitialize();
				return ShaderPropsCache.MainTexShaderProp;
			}
		}

		public static int AlphaTexShaderProp {
			get {
				ShaderPropsCache.LazyInitialize();
				return ShaderPropsCache.AlphaTexShaderProp;
			}
		}

		public static int ExternalAlphaShaderProp {
			get {
				ShaderPropsCache.LazyInitialize();
				return ShaderPropsCache.ExternalAlphaShaderProp;
			}
		}

		//
		// GetComponent<T>
		//

		public static T GetComponent<T>(GameObject obj, bool allow_create) where T : Component {
			var comp = obj.GetComponent<T>();
			if ( allow_create && !comp ) {
				comp = obj.AddComponent<T>();
			}
			return comp;
		}

		//
		// FillGeneratedMesh
		//

		public static void FillGeneratedMesh(Mesh mesh, SwfClipAsset.MeshData mesh_data) {
			if ( mesh_data.SubMeshes.Length > 0 ) {
				mesh.subMeshCount = mesh_data.SubMeshes.Length;

				GeneratedMeshCache.FillVertices(mesh_data.Vertices);
				mesh.SetVertices(GeneratedMeshCache.Vertices);

				for ( int i = 0, e = mesh_data.SubMeshes.Length; i < e; ++i ) {
					GeneratedMeshCache.FillTriangles(
						mesh_data.SubMeshes[i].StartVertex,
						mesh_data.SubMeshes[i].IndexCount);
					mesh.SetTriangles(GeneratedMeshCache.Indices, i);
				}

				GeneratedMeshCache.FillUVs(mesh_data.UVs);
				mesh.SetUVs(0, GeneratedMeshCache.UVs);

				GeneratedMeshCache.FillAddColors(mesh_data.AddColors);
				mesh.SetUVs(1, GeneratedMeshCache.AddColors);

				GeneratedMeshCache.FillMulColors(mesh_data.MulColors);
				mesh.SetColors(GeneratedMeshCache.MulColors);
			}
		}

		// ShaderPropsCache

		static class ShaderPropsCache {
			public static int TintShaderProp          = 0;
			public static int MainTexShaderProp       = 0;
			public static int AlphaTexShaderProp      = 0;
			public static int ExternalAlphaShaderProp = 0;

			static bool _initialized = false;
			public static void LazyInitialize() {
				if ( !_initialized ) {
					_initialized            = true;
					TintShaderProp          = Shader.PropertyToID("_Tint");
					MainTexShaderProp       = Shader.PropertyToID("_MainTex");
					AlphaTexShaderProp      = Shader.PropertyToID("_AlphaTex");
					ExternalAlphaShaderProp = Shader.PropertyToID("_ExternalAlpha");
				}
			}
		}

		//
		// GeneratedMeshCache
		//

		static class GeneratedMeshCache {
			const int PreallocatedVertices = 500;

			public static List<int> Indices = new List<int>(PreallocatedVertices * 6 / 4);
			public static void FillTriangles(int start_vertex, int index_count) {
				Indices.Clear();
				if ( Indices.Capacity < index_count ) {
					Indices.Capacity = index_count * 2;
				}
				for ( var i = 0; i < index_count; i += 6 ) {
					Indices.Add(start_vertex + 2);
					Indices.Add(start_vertex + 1);
					Indices.Add(start_vertex + 0);
					Indices.Add(start_vertex + 0);
					Indices.Add(start_vertex + 3);
					Indices.Add(start_vertex + 2);
					start_vertex += 4;
				}
			}

			static        Vector3       Vertex   = Vector3.zero;
			public static List<Vector3> Vertices = new List<Vector3>(PreallocatedVertices);
			public static void FillVertices(Vector2[] vertices) {
				Vertices.Clear();
				if ( Vertices.Capacity < vertices.Length ) {
					Vertices.Capacity = vertices.Length * 2;
				}
				for ( int i = 0, e = vertices.Length; i < e; ++i ) {
					var vert = vertices[i];
					Vertex.x = vert.x;
					Vertex.y = vert.y;
					Vertices.Add(Vertex);
				}
			}

			static        Vector2       UV0 = Vector2.zero;
			static        Vector2       UV1 = Vector2.zero;
			static        Vector2       UV2 = Vector2.zero;
			static        Vector2       UV3 = Vector2.zero;
			public static List<Vector2> UVs = new List<Vector2>(PreallocatedVertices);
			public static void FillUVs(uint[] uvs) {
				UVs.Clear();
				if ( UVs.Capacity < uvs.Length * 2 ) {
					UVs.Capacity = uvs.Length * 2 * 2;
				}
				for ( int i = 0, e = uvs.Length; i < e; i += 2 ) {
					float min_x, min_y, max_x, max_y;
					UnpackUV(uvs[i+0], out min_x, out min_y);
					UnpackUV(uvs[i+1], out max_x, out max_y);

					UV0.x = min_x; UV0.y = min_y;
					UV1.x = max_x; UV1.y = min_y;
					UV2.x = max_x; UV2.y = max_y;
					UV3.x = min_x; UV3.y = max_y;

					UVs.Add(UV0);
					UVs.Add(UV1);
					UVs.Add(UV2);
					UVs.Add(UV3);
				}
			}

			static        Vector4       AddColor  = Vector4.one;
			public static List<Vector4> AddColors = new List<Vector4>(PreallocatedVertices);
			public static void FillAddColors(uint[] colors) {
				AddColors.Clear();
				if ( AddColors.Capacity < colors.Length * 2 ) {
					AddColors.Capacity = colors.Length * 2 * 2;
				}
				for ( int i = 0, e = colors.Length; i < e; i += 2 ) {
					UnpackFColorFromUInts(
						colors[i+0], colors[i+1],
						out AddColor.x, out AddColor.y,
						out AddColor.z, out AddColor.w);
					AddColors.Add(AddColor);
					AddColors.Add(AddColor);
					AddColors.Add(AddColor);
					AddColors.Add(AddColor);
				}
			}

			static        Color       MulColor  = Color.white;
			public static List<Color> MulColors = new List<Color>(PreallocatedVertices);
			public static void FillMulColors(uint[] colors) {
				MulColors.Clear();
				if ( MulColors.Capacity < colors.Length * 2 ) {
					MulColors.Capacity = colors.Length * 2 * 2;
				}
				for ( int i = 0, e = colors.Length; i < e; i += 2 ) {
					UnpackFColorFromUInts(
						colors[i+0], colors[i+1],
						out MulColor.r, out MulColor.g,
						out MulColor.b, out MulColor.a);
					MulColors.Add(MulColor);
					MulColors.Add(MulColor);
					MulColors.Add(MulColor);
					MulColors.Add(MulColor);
				}
			}
		}
	}
}