// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/FogOfWar" {
     Properties {
         _MainTex ("Base layer (RGB)", 2D) = "white" {}
		 		 
         _CenterX ("CenterX", Float) = 0
         _CenterY ("CenterY", Float) = 0

         _Radius ("Radius", Float) = 2
         _Angle ("Angle", Range(0.0, 180.0)) = 30.0

         _Range ("Range", Float) = 1.0

         _Alpha ("Alpha", Range(0.0, 1.0)) = 1.0
     }
     
     SubShader {
         Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
         
         Lighting Off 
         Fog { Mode Off }
         ZWrite Off
         Blend SrcAlpha OneMinusSrcAlpha
         LOD 100
         
             
         CGINCLUDE
         #pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
         #include "UnityCG.cginc"
         sampler2D _MainTex;
         //sampler2D _DetailTex;
     
         float4 _MainTex_ST;
         //float4 _DetailTex_ST;
         
         float _CenterX;
         float _CenterY;
		 
         float _Radius;
         float _Angle;
         float _Range;
         float _Alpha;
         
         struct v2f {
             float4 pos : SV_POSITION;
             float2 uv : TEXCOORD0;
             //float2 uv2 : TEXCOORD1;
             fixed4 color : TEXCOORD1;        
         };
     
         
         v2f vert (appdata_full v)
         {
			// Transform the vertex coordinates from model space into world space
			float4 vv = mul( unity_ObjectToWorld, v.vertex );

			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = TRANSFORM_TEX(v.texcoord.xy,_MainTex);

			float x;
			float y;

			x = _Radius * sin(_Angle * 3.14 / 180);
			y = _Radius * cos(_Angle * 3.14 / 180);
			
			float rx;
			float ry;
			rx = pow( ( pow( _Radius, 2) - pow( _Radius, 2)) , 1/2 );
			ry = pow( ( pow( _Radius, 2) - pow( rx, 2)) , 1/2 );
			
			
			//Simple radius alpha/*
			o.color = fixed4(1, 1, 1, 
			vv.x <= _CenterX + rx + _Range && vv.x >= _CenterX + rx - _Range &&
			vv.z <= _CenterY + ry + _Range && vv.z >= _CenterY + ry - _Range
			? 0:1
			);
			/**/
			
			return o;
         }
         ENDCG
     
     
         Pass {
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag
             #pragma fragmentoption ARB_precision_hint_fastest        
             fixed4 frag (v2f i) : COLOR
             {
                 fixed4 o;
                 fixed4 tex = tex2D (_MainTex, i.uv);
                 //fixed4 tex2 = tex2D (_DetailTex, i.uv2);
                 
                 //o = (tex * tex2) * i.color;
                 o = tex * i.color;
                 
                 return o;
             }
             ENDCG 
         }    
     }
     }