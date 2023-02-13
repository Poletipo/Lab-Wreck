Shader "LabWreck/SkewedShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SkewedValue ("Skewed Amount", Float) = 0.0
		_Tiling ("Tiling Amount", Vector) = (1,1,0)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{
            Cull off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float _SkewedValue;
			float2 _Tiling;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				i.uv.x += i.uv.y * _SkewedValue;
				i.uv.x *= _Tiling.x;
				i.uv.y *= _Tiling.y;
				float4 _Color = tex2D(_MainTex,i.uv);

				return _Color;
			}
			ENDCG
		}
	}
}
