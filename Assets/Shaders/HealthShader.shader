Shader "LabWreck/HealthShader"
{
    Properties
    {
        [NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
        _HealthValue ("Health Value", Range(0,1)) = 0.5
        _LowHealthColor ("Low Health Color", Color) = (1,1,1,1)
        _FullHealthColor ("Full Health Color", Color) = (0,0,0,1)
        _BGColor ("Background Color", Color) = (0,0,0,1)
    }
    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "Queue"="Transparent"
        }

        Pass
        {
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "ShaderUtilities.cginc"

            #define TAU 6.2831853071795865

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _HealthValue;
            float4 _FullHealthColor;
            float4 _LowHealthColor;
            float4 _BGColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {


                float t = i.uv.x < _HealthValue;

                float4 col = tex2D(_MainTex,float2(_HealthValue,i.uv.y));
                
                if(_HealthValue < 0.2){
                    float flash = cos(_Time.y * 10) * 0.4 +1;
                    
                    col = float4(col.xyz * flash,1);
                    
                }

                float4 outColor = lerp(_BGColor, col, t);



                return outColor;
            }
            ENDCG
        }
    }
}
