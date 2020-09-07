using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

using System.IO;
using System.Collections.Generic;

using FTRuntime;

namespace FTEditor {
	class SwfMaterialCache {

		const string SwfSimpleShaderName     = "SwfSimpleShader";
		const string SwfMaskedShaderName     = "SwfMaskedShader";
		const string SwfSimpleGrabShaderName = "SwfSimpleGrabShader";
		const string SwfMaskedGrabShaderName = "SwfMaskedGrabShader";
		const string SwfIncrMaskShaderName   = "SwfIncrMaskShader";
		const string SwfDecrMaskShaderName   = "SwfDecrMaskShader";

		static Dictionary<string, Shader> ShaderCache = new Dictionary<string, Shader>();
		static Shader GetShaderByName(string shader_name) {
			Shader shader;
			if ( !ShaderCache.TryGetValue(shader_name, out shader) || !shader ) {
				shader = SafeLoadShader(shader_name);
				ShaderCache.Add(shader_name, shader);
			}
			shader.hideFlags = HideFlags.HideInInspector;
			return shader;
		}

		static Dictionary<string, Material> MaterialCache = new Dictionary<string, Material>();
		static Material GetMaterialByPath(
			string                          material_path,
			Shader                          material_shader,
			System.Func<Material, Material> fill_material)
		{
			Material material;
			if ( !MaterialCache.TryGetValue(material_path, out material) || !material ) {
				material = SafeLoadMaterial(material_path, material_shader, fill_material);
				MaterialCache.Add(material_path, material);
			}
			material.hideFlags = HideFlags.HideInInspector;
			return material;
		}

		// ---------------------------------------------------------------------
		//
		// Functions
		//
		// ---------------------------------------------------------------------

		public static Material GetSimpleMaterial(
			SwfBlendModeData.Types blend_mode)
		{
			return LoadOrCreateMaterial(
				SelectShader(false, blend_mode),
				(dir_path, filename) => {
					return string.Format(
						"{0}/{1}_{2}.mat",
						dir_path, filename, blend_mode);
				},
				material => FillMaterial(material, blend_mode, 0));
		}

		public static Material GetMaskedMaterial(
			SwfBlendModeData.Types blend_mode,
			int                    stencil_id)
		{
			return LoadOrCreateMaterial(
				SelectShader(true, blend_mode),
				(dir_path, filename) => {
					return string.Format(
						"{0}/{1}_{2}_{3}.mat",
						dir_path, filename, blend_mode, stencil_id);
				},
				material => FillMaterial(material, blend_mode, stencil_id));
		}

		public static Material GetIncrMaskMaterial() {
			return LoadOrCreateMaterial(
				GetShaderByName(SwfIncrMaskShaderName),
				(dir_path, filename) => {
					return string.Format(
						"{0}/{1}.mat",
						dir_path, filename);
				},
				material => material);
		}

		public static Material GetDecrMaskMaterial() {
			return LoadOrCreateMaterial(
				GetShaderByName(SwfDecrMaskShaderName),
				(dir_path, filename) => {
					return string.Format(
						"{0}/{1}.mat",
						dir_path, filename);
				},
				material => material);
		}

		// ---------------------------------------------------------------------
		//
		// Private
		//
		// ---------------------------------------------------------------------

		static Shader SafeLoadShader(string shader_name) {
			var filter = string.Format("t:Shader {0}", shader_name);
			var shader = SwfEditorUtils.LoadFirstAssetDBByFilter<Shader>(filter);
			if ( !shader ) {
				throw new UnityException(string.Format(
					"SwfMaterialCache. Shader not found: {0}",
					shader_name));
			}
			return shader;
		}

		static Material SafeLoadMaterial(
			string                          material_path,
			Shader                          material_shader,
			System.Func<Material, Material> fill_material)
		{
			var material = AssetDatabase.LoadAssetAtPath<Material>(material_path);
			if ( !material ) {
				material = fill_material(new Material(material_shader));
				material.hideFlags = HideFlags.HideInInspector;
				AssetDatabase.CreateAsset(material, material_path);
			}
			return material;
		}

		static Material LoadOrCreateMaterial(
			Shader                              shader,
			System.Func<string, string, string> path_factory,
			System.Func<Material, Material>     fill_material)
		{
			var shader_path   = AssetDatabase.GetAssetPath(shader);
			var shader_dir    = Path.GetDirectoryName(shader_path);
			var generated_dir = Path.Combine(shader_dir, "Generated");
			if ( !AssetDatabase.IsValidFolder(generated_dir) ) {
				AssetDatabase.CreateFolder(shader_dir, "Generated");
			}
			var material_path = path_factory(
				generated_dir,
				Path.GetFileNameWithoutExtension(shader_path));
			return GetMaterialByPath(material_path, shader, fill_material);
		}

		static Shader SelectShader(bool masked, SwfBlendModeData.Types blend_mode) {
			switch ( blend_mode ) {
			case SwfBlendModeData.Types.Normal:
			case SwfBlendModeData.Types.Layer:
			case SwfBlendModeData.Types.Multiply:
			case SwfBlendModeData.Types.Screen:
			case SwfBlendModeData.Types.Lighten:
			case SwfBlendModeData.Types.Add:
			case SwfBlendModeData.Types.Subtract:
				return GetShaderByName(masked ? SwfMaskedShaderName : SwfSimpleShaderName);
			case SwfBlendModeData.Types.Darken:
			case SwfBlendModeData.Types.Difference:
			case SwfBlendModeData.Types.Invert:
			case SwfBlendModeData.Types.Overlay:
			case SwfBlendModeData.Types.Hardlight:
				return GetShaderByName(masked ? SwfMaskedGrabShaderName : SwfSimpleGrabShaderName);
			default:
				throw new UnityException(string.Format(
					"SwfMaterialCache. Incorrect blend mode: {0}",
					blend_mode));
			}
		}

		static Material FillMaterial(
			Material               material,
			SwfBlendModeData.Types blend_mode,
			int                    stencil_id)
		{
			switch ( blend_mode ) {
			case SwfBlendModeData.Types.Normal:
				material.SetInt("_BlendOp" , (int)BlendOp.Add);
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
				break;
			case SwfBlendModeData.Types.Layer:
				material.SetInt("_BlendOp" , (int)BlendOp.Add);
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
				break;
			case SwfBlendModeData.Types.Multiply:
				material.SetInt("_BlendOp" , (int)BlendOp.Add);
				material.SetInt("_SrcBlend", (int)BlendMode.DstColor);
				material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
				break;
			case SwfBlendModeData.Types.Screen:
				material.SetInt("_BlendOp" , (int)BlendOp.Add);
				material.SetInt("_SrcBlend", (int)BlendMode.OneMinusDstColor);
				material.SetInt("_DstBlend", (int)BlendMode.One);
				break;
			case SwfBlendModeData.Types.Lighten:
				material.SetInt("_BlendOp" , (int)BlendOp.Max);
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
				break;
			case SwfBlendModeData.Types.Darken:
				material.SetInt("_BlendOp" , (int)BlendOp.Add);
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
				material.EnableKeyword("SWF_DARKEN_BLEND");
				break;
			case SwfBlendModeData.Types.Difference:
				material.SetInt("_BlendOp" , (int)BlendOp.Add);
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
				material.EnableKeyword("SWF_DIFFERENCE_BLEND");
				break;
			case SwfBlendModeData.Types.Add:
				material.SetInt("_BlendOp" , (int)BlendOp.Add);
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.One);
				break;
			case SwfBlendModeData.Types.Subtract:
				material.SetInt("_BlendOp" , (int)BlendOp.ReverseSubtract);
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.One);
				break;
			case SwfBlendModeData.Types.Invert:
				material.SetInt("_BlendOp" , (int)BlendOp.Add);
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
				material.EnableKeyword("SWF_INVERT_BLEND");
				break;
			case SwfBlendModeData.Types.Overlay:
				material.SetInt("_BlendOp" , (int)BlendOp.Add);
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
				material.EnableKeyword("SWF_OVERLAY_BLEND");
				break;
			case SwfBlendModeData.Types.Hardlight:
				material.SetInt("_BlendOp" , (int)BlendOp.Add);
				material.SetInt("_SrcBlend", (int)BlendMode.One);
				material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
				material.EnableKeyword("SWF_HARDLIGHT_BLEND");
				break;
			default:
				throw new UnityException(string.Format(
					"SwfMaterialCache. Incorrect blend mode: {0}",
					blend_mode));
			}
			material.SetInt("_StencilID", stencil_id);
			return material;
		}
	}
}