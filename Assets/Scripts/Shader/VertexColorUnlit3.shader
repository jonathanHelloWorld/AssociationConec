// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Vertex color unlit3" {
	Properties{
		_MainTex("Texture", 2D) = "white" {}
		_SubTex("Fill Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		pass 
		{
			CGPROGRAM
			#pragma target 3.0
			#pragma vertex wfiVertCol
			#pragma fragment passThrough
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _SubTex;
			uniform float4 _Color;

			struct VertOut
			{
				float2 uv : TEXCOORD0;
				float4 position : POSITION;
				float4 color : COLOR;
			};

			struct VertIn
			{
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
				float4 color : COLOR;
			};

			VertOut wfiVertCol(VertIn input, float3 normal : NORMAL, float2 uv : TEXCOORD0, float4 color : COLOR)
			{
				VertOut output;
				output.position = UnityObjectToClipPos(input.vertex);
				output.color = color;
				output.uv = input.uv;

				return output;
			}

			struct FragOut
			{
				float4 color : COLOR;
			};

			/**/
			FragOut passThrough(VertOut i)
			{
				FragOut output;
				half4 c = tex2D(_MainTex, i.uv);
				half4 cb = tex2D(_SubTex, i.uv);
				output.color = i.color * _Color * c;
				output.color.a = 
					c.a * (i.color.a*-1+1) + //else
					cb.a * i.color.a *_Color.a + c.a;
				//output.color = _Color;//* color;
				return output;
			}
			/**/

			ENDCG
		}
	}
	FallBack "Diffuse"
}