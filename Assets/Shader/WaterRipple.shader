Shader "Unlit/WaterRipple"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Wavelength("Wavelength", float) = 3
		_Amplitude("Amplitude", float) = 0.1
		_Speed("_Speed", float) = 1.82
		_Speed("_StartPos",Vector) = (0,0,0,0)

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull Off
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex 	: POSITION;
				float2 uv 		: TEXCOORD0;
				float3 normal 	: NORMAL;
				
			};

			struct v2f
			{
				float2 uv 		: TEXCOORD0;
				float4 vertex 	: SV_POSITION;
				fixed3 color	: COLOR0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			// wave 
			float _Wavelength;
			float _Amplitude;
			float _Speed;
			Vector _StartPos;

			float rippleSin(float2 offPos, float2 org)
			{
				float d = distance (offPos, org);
				float w = 2 / _Wavelength;
				float miu = _Speed * w;

				float t = _Time.w;
				float r = d * w - t * miu;
				float h = _Amplitude * sin (r);
				return h;
			}


			v2f vert (appdata v)
			{
				v2f o;

				v.vertex.y +=  

				o.vertex 	= UnityObjectToClipPos(v.vertex);
				o.uv 		= TRANSFORM_TEX(v.uv, _MainTex);
				o.color 	= mul(UNITY_MATRIX_IT_MV,v.normal) *0.5 +0.5;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
//				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 col = fixed4(i.color, 1);

				return col;
			}
			ENDCG
		}
	}
}
