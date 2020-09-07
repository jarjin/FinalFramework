#ifndef SWF_BASE_CG_INCLUDED
#define SWF_BASE_CG_INCLUDED

//
// blending functions
//

inline fixed4 swf_darken(fixed4 ca, fixed4 cb) {
	fixed4 r = min(ca, cb);
	r.a = cb.a;
	return r;
}

inline fixed4 swf_difference(fixed4 ca, fixed4 cb) {
	fixed4 r = abs(ca - cb);
	r.a = cb.a;
	return r;
}

inline fixed4 swf_invert(fixed4 ca, fixed4 cb) {
	fixed4 r = 1.0 - ca;
	r.a = cb.a;
	return r;
}

inline fixed4 swf_overlay(fixed4 ca, fixed4 cb) {
	fixed4 r = ca > 0.5 ? 1.0 - 2.0 * (1.0 - ca) * (1.0 - cb) : 2.0 * ca * cb;
	r.a = cb.a;
	return r;
}

inline fixed4 swf_hardlight(fixed4 ca, fixed4 cb) {
	fixed4 r = cb > 0.5 ? 1.0 - (1.0 - ca) * (1.0 - 2.0 * (cb - 0.5)) : ca * (2.0 * cb);
	r.a = cb.a;
	return r;
}

inline fixed4 grab_blend(sampler2D grab_tex, float4 screenpos, fixed4 c) {
	float2 grab_uv = screenpos.xy / screenpos.w;
	grab_uv = (grab_uv + 1.0) * 0.5;
#if UNITY_UV_STARTS_AT_TOP
	grab_uv.y = 1.0 - grab_uv.y;
#endif
	fixed4 grab_c = tex2D(grab_tex, grab_uv);
	#if SWF_DARKEN_BLEND
		c = swf_darken(grab_c, c);
	#elif SWF_DIFFERENCE_BLEND
		c = swf_difference(grab_c, c);
	#elif SWF_INVERT_BLEND
		c = swf_invert(grab_c, c);
	#elif SWF_OVERLAY_BLEND
		c = swf_overlay(grab_c, c);
	#elif SWF_HARDLIGHT_BLEND
		c = swf_hardlight(grab_c, c);
	#endif
	return c;
}

//
// structs
//

struct swf_appdata_t {
	float4 vertex    : POSITION;
	float2 uv        : TEXCOORD0;
	float4 mulcolor  : COLOR;
	float4 addcolor  : TEXCOORD1;
};

struct swf_grab_appdata_t {
	float4 vertex    : POSITION;
	float2 uv        : TEXCOORD0;
	float4 mulcolor  : COLOR;
	float4 addcolor  : TEXCOORD1;
};

struct swf_mask_appdata_t {
	float4 vertex    : POSITION;
	float2 uv        : TEXCOORD0;
};

struct swf_v2f_t {
	float4 vertex    : SV_POSITION;
	float2 uv        : TEXCOORD0;
	fixed4 mulcolor  : COLOR;
	fixed4 addcolor  : TEXCOORD1;
};

struct swf_grab_v2f_t {
	float4 vertex    : SV_POSITION;
	float2 uv        : TEXCOORD0;
	fixed4 mulcolor  : COLOR;
	fixed4 addcolor  : TEXCOORD1;
	float4 screenpos : TEXCOORD2;
};

struct swf_mask_v2f_t {
	float4 vertex    : SV_POSITION;
	float2 uv        : TEXCOORD0;
};

//
// vert functions
//

inline swf_v2f_t swf_vert(swf_appdata_t IN) {
	swf_v2f_t OUT;
	OUT.vertex    = UnityObjectToClipPos(IN.vertex);
	OUT.uv        = IN.uv;
	OUT.mulcolor  = IN.mulcolor * _Tint;
	OUT.addcolor  = IN.addcolor;
	return OUT;
}

inline swf_grab_v2f_t swf_grab_vert(swf_grab_appdata_t IN) {
	swf_grab_v2f_t OUT;
	OUT.vertex    = UnityObjectToClipPos(IN.vertex);
	OUT.uv        = IN.uv;
	OUT.mulcolor  = IN.mulcolor * _Tint;
	OUT.addcolor  = IN.addcolor;
	OUT.screenpos = OUT.vertex;
	return OUT;
}

inline swf_mask_v2f_t swf_mask_vert(swf_mask_appdata_t IN) {
	swf_mask_v2f_t OUT;
	OUT.vertex    = UnityObjectToClipPos(IN.vertex);
	OUT.uv        = IN.uv;
	return OUT;
}

//
// frag functions
//

inline fixed4 swf_frag(swf_v2f_t IN) : SV_Target {
	fixed4 c = tex2D(_MainTex, IN.uv);
	fixed4 a = tex2D(_AlphaTex, IN.uv).r;
	c.a = lerp(c.a, a.r, _ExternalAlpha);
	c = c * IN.mulcolor + step(0.01, c.a) * IN.addcolor;
	c.rgb *= c.a;
	return c;
}

inline fixed4 swf_grab_frag(swf_grab_v2f_t IN) : SV_Target {
	fixed4 c = tex2D(_MainTex, IN.uv);
	fixed4 a = tex2D(_AlphaTex, IN.uv).r;
	c.a = lerp(c.a, a.r, _ExternalAlpha);
	c = c * IN.mulcolor + step(0.01, c.a) * IN.addcolor;
	c = grab_blend(_GrabTexture, IN.screenpos, c);
	c.rgb *= c.a;
	return c;
}

inline fixed4 swf_mask_frag(swf_mask_v2f_t IN) : SV_Target {
	fixed4 c = tex2D(_MainTex, IN.uv);
	fixed4 a = tex2D(_AlphaTex, IN.uv).r;
	c.a = lerp(c.a, a.r, _ExternalAlpha);
	clip(c.a - 0.01);
	return c;
}

#endif // SWF_BASE_CG_INCLUDED