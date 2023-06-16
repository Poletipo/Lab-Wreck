Shader "LabWreck/SimpleLightShader"
{
    Properties{
        _Color ("Color", Color) = (0,0,0,0)
        //_Color ("Color", Color) = "white" {}
    }
    SubShader{
        Tags {"renderType" = "Opaque"}
    
        Pass{
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            struct vertex_data{
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f{
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color;

            v2f vert (vertex_data v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET{


                return _Color * i.uv.x;

                return  fixed4(i.uv,0,0);
            }

            ENDCG
        }

    
    }


}