Shader "FlashTools/SwfMaskedGrab" {
	Properties {
		[PerRendererData] _MainTex       ("Main Texture"  , 2D   ) = "white" {}
		[PerRendererData] _AlphaTex      ("Alpha Texture" , 2D   ) = "white" {}
		[PerRendererData] _ExternalAlpha ("External Alpha", Float) = 0
		[PerRendererData] _Tint          ("Tint"          , Color) = (1,1,1,1)

		_StencilID ("Stencil ID", Int) = 0
		[Enum(UnityEngine.Rendering.BlendOp  )] _BlendOp  ("BlendOp" , Int) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("SrcBlend", Int) = 1
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("DstBlend", Int) = 10
	}

	SubShader {
		Tags {
			"Queue"             = "Transparent"
			"IgnoreProjector"   = "True"
			"RenderType"        = "Transparent"
			"PreviewType"       = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull     Off
		Lighting Off
		ZWrite   Off

		BlendOp [_BlendOp]
		Blend [_SrcBlend] [_DstBlend]

		GrabPass { }

		Pass {
			Stencil {
				Ref  [_StencilID]
				Comp Equal
			}
		CGPROGRAM
			fixed4    _Tint;
			sampler2D _MainTex;
			sampler2D _AlphaTex;
			sampler2D _GrabTexture;
			float     _ExternalAlpha;

			#pragma multi_compile SWF_DARKEN_BLEND SWF_DIFFERENCE_BLEND SWF_INVERT_BLEND SWF_OVERLAY_BLEND SWF_HARDLIGHT_BLEND

			#include "UnityCG.cginc"
			#include "SwfBaseCG.cginc"

			#pragma vertex swf_grab_vert
			#pragma fragment swf_grab_frag
		ENDCG
		}
	}
}