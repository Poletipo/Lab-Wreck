Shader "PostProcess/BlurShader"
{
	Properties
	{
		[HideInInspector] _MainTex("Texture", 2D) = "white" {}
		_BlurSize("Blur Size", Int) = 1
		_SampleCount("Sample Count", Int) = 5
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
				int _SampleCount;


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
				float4 col;

			if (_BlurSize == 0) {
				return tex2D(_MainTex, i.uv);
			}

			uint sampleCount = _SampleCount > _BlurSize ? _BlurSize : _SampleCount;

			float sampleDistance = _BlurSize / sampleCount;

			int iterations = 0;

			for (int k = -_BlurSize; k <= _BlurSize; k += sampleDistance) {
				for (int j = -_BlurSize; j <= _BlurSize; j += sampleDistance) {

					float2 coord = i.uv + float2(j * _MainTex_TexelSize.x,k * _MainTex_TexelSize.y);

					col += tex2D(_MainTex, coord);
					iterations++;
				}

			}

			col /= iterations;

			//col.a = 1;

			return col;
		}
			ENDCG

		}
		}
}
