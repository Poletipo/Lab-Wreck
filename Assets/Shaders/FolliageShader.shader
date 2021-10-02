Shader "Custom/FolliageShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Alpha ("Alpha", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {
        "RenderType"="Transparent"
        "Queue"="Transparent+1000"
        }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha


        Pass{
            //Cull OFF

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _Alpha;

            v2f vert (appdata i){
                v2f o;

                o.uv = i.uv;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.color = _Alpha;
                return o;
            }

            float4 frag(v2f i) : SV_TARGET{
                
                float4 col = tex2D(_MainTex, i.uv);
                col.a = tex2D(_MainTex, i.uv).a;
                return col;
            }


            ENDCG

        }
    }
}
