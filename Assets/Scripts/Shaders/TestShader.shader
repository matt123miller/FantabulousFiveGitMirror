Shader "Custom/TestShader" {
	Properties
	{
		_MainTex("Texture", 2D) = "White" {}
		_Color("Color", Color) = (1,1,1,1)
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"

				"PreviewType" = "Plane"
			}

			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct appdata members uv)
#pragma exclude_renderers d3d11 xbox360
			// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct appdata members uv)
			//#pragma exclude_renderers d3d11 xbox360
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

						struct appdata {
					float4 vertex : POSITION;
					float2 uv = TEXCOORD0;
			};
		// This is passed from the vert to the frag function
		struct v2f {
			float4 vertex : SV_POSITION;
			float2 uv = TEXCOORD;
		 };

		v2f vert(appdata v) {
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv = v.uv;
			return o;
				}

		float4 frag(v2f i) : SV_Target
		{
			float4 texColour = float4(i.uv.r, i.uv.g, i.uv.b, 1);
			return texColour * _Color;
		}
			ENDCG
			}

		}
}
