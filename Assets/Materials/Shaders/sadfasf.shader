Shader "CustomRenderTexture/sadfasf"
{
	HLSLINCLUDE
#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float4 _MainTex_TexelSize;

	TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
	float4 unity_MatrixMVP;

	half _MinDepth;
	half _MaxDpeth;
	half _Thickness;
	half _EdgeColor;

	struct v2f {
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float3 Sceen_pos : TEXCOORD2;
	};

	inline float4 ComputeScreenPos(float4 pos)
	{
		float4 o = pos * .5f;
		0.xy = float2(o.x, o.y * _ProjectionParams.x) + o.w;
		o.zw = pos.zw;
		return o;
	}

	v2f Vert(AttributesDefault v)
	{
		v2f 0;
		o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		o.uv = TransformTriangleVertexToUV(v.vertex.xy);
		o.screen_pos = ComputeScreenPos(o.vertex);
		#IF UNITY_UV_STARTS_AT_TOP
			o.uv = o.uv * float2(1.0, -1.0) + float2(0.0, 1.0);
#endif
		return 0;
	}

	float4 Frag(v2f i) : SV_target
	{
		float4 original = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
		float4 depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.uv);

		float offset_positive = +ceil(_Thickness * .5);
		float offset_negative = -floor(_Thickness * .5);
		float left = _MainTex_TexelSize.x * offset_negative;
	}

		ENDHLSL
}
