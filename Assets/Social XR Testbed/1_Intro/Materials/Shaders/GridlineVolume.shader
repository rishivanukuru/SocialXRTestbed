Shader "Custom/GridVolume" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

			// Expose parameters for the minimum x, minimum z,
			// maximum x, and maximum z of the rendered volume.
			_Corners("Min XZ / Max XZ", Vector) = (-1, -1, 1, 1)
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			// Allow back sides of the object to render.
			Cull Off

			CGPROGRAM

			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
				float3 worldPos;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;

			// Read the min xz/ max xz material properties.
			float4 _Corners;

			void surf(Input IN, inout SurfaceOutputStandard o) {

				// Calculate a signed distance from the clipping volume.
				float2 offset;
				offset = IN.worldPos.xz - _Corners.zw;
				float outOfBounds = max(offset.x, offset.y);
				offset = _Corners.xy - IN.worldPos.xz;
				outOfBounds = max(outOfBounds, max(offset.x, offset.y));
				// Reject fragments that are outside the clipping volume.
				clip(-outOfBounds);

				// Default surface shading.
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			ENDCG
		}
			// Note that the non-clipped Diffuse material will be used for shadows.
			// If you need correct shadowing with clipped material, add a shadow pass
			// that includes the same clipping logic as above.
				FallBack "Diffuse"
}

/*Shader "Custom/GridlineVolume"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_Corners("Min XZ / Max XZ", Vector) = (-1, -1, 1, 1)
	}
		SubShader{

				Tags { "RenderType" = "Opaque" }
				LOD 200

				Cull Off

				CGPROGRAM

				#pragma target 3.0


				sampler2D _MainTex;

				struct Input {
					float2 uv_MainTex;
					float3 worldPos;
				};

				half _Glossiness;
				half _Metallic;
				fixed4 _Color;

				// Read the min xz/ max xz material properties.
				float4 _Corners;

				void surf(Input IN, inout SurfaceOutputStandard o) {

					// Calculate a signed distance from the clipping volume.
					float2 offset;
					offset = IN.worldPos.xz - _Corners.zw;
					float outOfBounds = max(offset.x, offset.y);
					offset = _Corners.xy - IN.worldPos.xz;
					outOfBounds = max(outOfBounds, max(offset.x, offset.y));
					// Reject fragments that are outside the clipping volume.
					clip(-outOfBounds);

					// Default surface shading.
					fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
					o.Albedo = c.rgb;
					o.Metallic = _Metallic;
					o.Smoothness = _Glossiness;
					o.Alpha = c.a;
				}

				ENDCG
		}
		Fallback "diffuse"
}



*/

/*
Zwrite off
ColorMask 0
Cull off

Stencil{
	Ref 1
	Comp Always
	Pass Replace
}

Pass{

}
*/

/*
Properties
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o)
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
*/