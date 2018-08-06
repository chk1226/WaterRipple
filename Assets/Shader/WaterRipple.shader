// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "My/WaterRipple"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Frequency("Frequency", float) 		= 2.5
		_Amplitude("Amplitude", float) 		= 0.1
		_Speed("_Speed", float) 			= 1.82
		_StartPos0("_StartPos1",Vector) 	= (0,0,0,0)
		_StartPos1("_StartPos2",Vector) 	= (0,0,0,0)
		
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

			#define MAX_RIPPLE 2

			// wave 
			float _Frequency;
			float _Amplitude;
			float _Speed;
			//float2 _StartPos[MAX_RIPPLE];
			float _ScaleAry[MAX_RIPPLE];
			float _Distance[MAX_RIPPLE];
			Vector _StartPos0;
			Vector _StartPos1;
			

//			float rippleSin(float2 offPos, float2 org)
//			{
//				float d = distance (offPos, org);
//				float w = 2 / _Wavelength;
//				float miu = _Speed * w;

//				float t = _Time.w;
//				float r = d * w - t * miu;
//				float h = _Amplitude * sin (r);
//				return h;
//			}

			float rippleSin(float2 pos, float2 startPos, float dist)
			{
				float offsetvert = (pos.x - startPos.x) * (pos.x - startPos.x) + (pos.y - startPos.y) * (pos.y - startPos.y);

				if(offsetvert > dist*dist)
				{
					return 0;
				}

				return _Amplitude * sin(-_Time.w * _Speed + offsetvert * _Frequency);
			}

			v2f vert (appdata v)
			{
				v2f o;

				float value1 = rippleSin(float2(v.vertex.x, v.vertex.z), float2(_StartPos0.x, _StartPos0.z), _Distance[0]);
				float value2 = rippleSin(float2(v.vertex.x, v.vertex.z), float2(_StartPos1.x, _StartPos1.z), _Distance[1]);


				v.vertex.y += value1 * _ScaleAry[0];
				v.normal.y += value1 * _ScaleAry[0];
				v.vertex.y += value2 * _ScaleAry[1];
				v.normal.y += value2 * _ScaleAry[1];


				o.vertex 	= UnityObjectToClipPos(v.vertex);
				o.uv 		= TRANSFORM_TEX(v.uv, _MainTex);
				o.color 	= mul(UNITY_MATRIX_IT_MV,v.normal) * 0.5 + 0.5;
				//o.color 	= v.normal * 0.5 + 0.5;
				
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
