Shader "Custom/OutlineShader"
{
	Properties{
	_MainTex("Albedo", 2D) = "white" {}
	_OutlineColor("OutlineColor", Color) = (1,1,1,1)
	_OutlineWidth("OutlineWidth", Range(1, 5)) = 1.01
	}

	SubShader{
		Tags { "RenderType" = "Opaque" }
		Cull front

		// Pass1
		CGPROGRAM
		#pragma surface surf NoLighting vertex:vert noshadow noambient

		sampler2D _MainTex;
		struct Input {
			float2 uv_MainTex;
			fixed4 color : Color;
		};

		fixed4 _OutlineColor;
		float _OutlineWidth;

		void vert(inout appdata_full v)
		{
			v.vertex.xyz *= _OutlineWidth;
		}

		void surf(Input In, inout SurfaceOutput o)
		{

		}

		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			return _OutlineColor;
		}
		ENDCG

		// Pass2
		Cull back
		CGPROGRAM
		#pragma surface surf Lambert
					
		fixed4 _Color;
		sampler2D _MainTex;
			struct Input {
				float2 uv_MainTex;
				fixed4 color : Color;
			};

		void surf(Input In, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, In.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
		FallBack "Diffuse"

}
