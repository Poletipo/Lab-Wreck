Shader "PostProcess/NegativeShader"
{
	Properties
	{
		[HideInInspector] _MainTex("Texture", 2D) = "white" {}
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
			fixed4 col = tex2D(_MainTex, i.uv);
			return 1-col;
		}

			ENDCG

		}
	}
}
