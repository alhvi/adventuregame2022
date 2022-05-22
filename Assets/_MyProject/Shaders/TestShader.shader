Shader "Custom/TestShader"
{
	Properties
	{
		_MainTex("Textura", 2D) = "white" {}
		_Amount("Extrude amount", Range(0,1)) = 0
	}
		SubShader
		{
			Tags { "RenderType " = "Opaque"}
			CGPROGRAM
			#pragma surface surf1 Lambert vertex:vert
			struct Input {
				float2 uv_MainTex :TEXCOORD0;
			};

			sampler2D _MainTex;
			float _Amount;

			void vert(inout appdata_full v) {
				v.vertex.xyz += _Amount * v.normal;

			}

			void surf1(Input IN, inout SurfaceOutput o) {
				o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			}
			ENDCG
		}
			Fallback "Diffuse"

}
