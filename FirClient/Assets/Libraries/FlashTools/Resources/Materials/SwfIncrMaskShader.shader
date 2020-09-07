Shader "FlashTools/SwfIncrMask" {
	Properties {
		[PerRendererData] _MainTex       ("Main Texture"  , 2D)    = "white" {}
		[PerRendererData] _AlphaTex      ("Alpha Texture" , 2D)    = "white" {}
		[PerRendererData] _ExternalAlpha ("External Alpha", Float) = 0
	}

	SubShader {
		Tags {
			"Queue"             = "Transparent"
			"IgnoreProjector"   = "True"
			"RenderType"        = "Transparent"
			"PreviewType"       = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull      Off
		Lighting  Off
		ZWrite    Off

		ColorMask 0
		Blend     One OneMinusSrcAlpha

		Pass {
			Stencil {
				Ref  0
				Comp always
				Pass IncrSat
			}
		CGPROGRAM
			fixed4    _Tint;
			sampler2D _MainTex;
			sampler2D _AlphaTex;
			sampler2D _GrabTexture;
			float     _ExternalAlpha;

			#include "UnityCG.cginc"
			#include "SwfBaseCG.cginc"

			#pragma vertex swf_mask_vert
			#pragma fragment swf_mask_frag
		ENDCG
		}
	}
}