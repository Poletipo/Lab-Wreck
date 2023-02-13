Shader "LabWreck/RippleShader"
{
    Properties
    {
        _RippleSize ("Size", Range(1,30)) = 5
        _RippleSpeed ("Speed", Range(0,10)) = 5
        _RippleHeigth ("Heigth", Range(0,10)) = 5
    }
    SubShader
    {
        Tags {"RenderType"= "Opaque"}

        Pass{

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            #define TAU 6.2831853071795865

            struct appdata{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f{
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };

            float _RippleSize;
            float _RippleSpeed;
            float _RippleHeigth;

            float RadialWaves(float2 uv){
                
                float t = length(uv);

                t = cos((t - _Time * _RippleSpeed) * TAU * _RippleSize  ) * 0.5 + 0.5;

                return t;
            }


            v2f vert (appdata i){
                v2f o;
                o.uv = i.uv*2-1;
                o.normal = UnityObjectToWorldDir(i.normal);

                i.vertex.y += RadialWaves(o.uv) *_RippleHeigth; 



                o.vertex = UnityObjectToClipPos(i.vertex);

                return o;
            }

            float4 frag(v2f i) : SV_Target{
                
                float t = RadialWaves(i.uv);
                clip(1-length(i.uv));
                return t;

                //return float4(i.uv,0,1);

                //return float4(i.normal,1);
            }

            ENDCG
        }
    }
}
