// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Run/CurvedWorld/Reflective" {
    Properties {
        // Diffuse texture
        _MainTex ("Base (RGB)", 2D)								= "white" {}
        _SkyReflectionMap ("Sky Reflection Map (RGB)", CUBE)	= "white" {}
        _Color ("Color", Color)									= (1,1,1,1)
        _Gloss ("Glossiness", Range (0.001, 1))					= 0.5
        _CurvatureX ("Curve", Float)							= 0.001
        _CurvatureY ("Stretch", Float)							= 0.001
        _CurvatureZ ("Horizon", Float)							= 0.001
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
 
        CGPROGRAM
        // Surface shader function is called surf, and vertex preprocessor function is called vert
        // addshadow used to add shadow collector and caster passes following vertex modification
        #pragma surface surf BlinnPhong vertex:vert addshadowvert 

        // Access the shaderlab properties
        uniform sampler2D _MainTex;
        uniform samplerCUBE  _SkyReflectionMap;
        uniform float _Gloss;
        uniform float _CurvatureX;
        uniform float _CurvatureY;
        uniform float _CurvatureZ;
        uniform float4 _Color;
		uniform float4x4 _Rotation;
 
        // Basic input structure to the shader function
        // requires only a single set of UV texture mapping coordinates
        struct Input {
            float2 uv_MainTex;
            float2 uv_SkyReflectionMap;
			float4 screenPos; //will contain screen space position for reflection effects. Used by WetStreet shader in Dark Unity for example
			float3 worldRefl;
			//float4 color : COLOR;
        };
 
        // This is where the curvature is applied
        void vert( inout appdata_full v)
        {
            // Transform the vertex coordinates from model space into world space
            float4 vv = mul( unity_ObjectToWorld, v.vertex );
 
            // Now adjust the coordinates to be relative to the camera position
            vv.xyz -= _WorldSpaceCameraPos.xyz;
 
            // Reduce the y coordinate (i.e. lower the "height") of each vertex based
            // on the square of the distance from the camera in the z axis, multiplied
            // by the chosen curvature factor
            vv = float4( (vv.z * vv.z) * - _CurvatureX/10, (vv.z * vv.z) * - _CurvatureZ/10, (vv.z * vv.z) * - _CurvatureY/10, 0.0f );
 
            // Now apply the offset back to the vertices in model space
            v.vertex += mul(unity_WorldToObject, vv);
        }
 
        // This is just a default surface shader
        void surf (Input IN, inout SurfaceOutput o) {

            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;

            o.Albedo *= _Color.rgb;
			
			//float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			//screenUV *= float2(8,6);
            //half4 r = tex2D (_SkyReflectionMap, IN.uv_SkyReflectionMap);
            //o.Albedo = r;
			//o.Albedo *= tex2D (_SkyReflectionMap, screenUV).rgb * 2;
			
			o.Emission = texCUBE (_SkyReflectionMap, mul(_Rotation, float4(IN.worldRefl,0))).rgb * _Gloss;
			//o.Emission = texCUBE (_SkyReflectionMap, IN.worldRefl).rgb * _Gloss;

            o.Alpha = c.a;     
			o.Specular = _Gloss;
        }
        ENDCG
    }
    FallBack "Diffuse"
}