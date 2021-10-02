Shader "LabWreck/SDFExample"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
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

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 coord = float2(i.uv.x * 8, i.uv.y);
                float2 ref = float2(clamp(coord.x, 0.5, 7.5), 0.5);
                float sdf = distance(coord, ref) *2 -1;
                clip(-sdf);
                
                return float4(coord, 0,1);


                return _Color;
            }
            ENDCG
        }
    }
}
