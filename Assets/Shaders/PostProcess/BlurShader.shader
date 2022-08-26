Shader "PostProcess/BlurShader"
{
	Properties
	{
		[HideInInspector] _MainTex("Texture", 2D) = "white" {}
		_BlurSize("Blur Size", Int) = 1
	}

	Subshader
	{
		Cull Off
		ZWrite Off
		ZTest Always

		Pass{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag



			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			int _BlurSize;
				

		struct appdata {
			float4 vertex : POSITION;
			float2 uv : TEXCOORD0;
		};

		struct v2f {
			float4 position : SV_POSITION;
			float2 uv : TEXCOORD0;
		};

		//the vertex shader
		v2f vert(appdata v) {
			v2f o;
			//convert the vertex positions from object space to clip space so they can be rendered
			o.position = UnityObjectToClipPos(v.vertex);
			o.uv = v.uv;
			return o;
		}

		//the fragment shader
		fixed4 frag(v2f i) : SV_TARGET{
			//get source color from texture


			float4 col;//= tex2D(_MainTex, i.uv);



			for (int k = -_BlurSize; k <= _BlurSize; k++) {
				for (int j = -_BlurSize; j <= _BlurSize; j++) {

					float2 coord = i.uv + float2(j * _MainTex_TexelSize.x,k * _MainTex_TexelSize.y);
					
					col += tex2D(_MainTex, coord);

				}

			}

			col /= (_BlurSize*2 +1) * (_BlurSize*2 +1);

			//col.a = 1;

			return col;
		}
			ENDCG

		}
	}
}
