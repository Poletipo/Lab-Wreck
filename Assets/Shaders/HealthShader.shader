Shader "LabWreck/HealthShader"
{
    Properties
    {
        [NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
        _HealthValue ("Health Value", Range(0,1)) = 0.5
        _BorderSize ("Border Size", Range(0,1)) = 0.1
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
            float _BorderSize;
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
                float2 coord = float2(i.uv.x * 8, i.uv.y);
                float2 ref = float2(clamp(coord.x, 0.5, 7.5), 0.5);
                float sdf = distance(coord, ref) *2 -1;
                clip(-sdf);

                float borderSdf = sdf + _BorderSize;
                float borderMask = step(0,-borderSdf);



                float t = i.uv.x < _HealthValue;

                float4 col = tex2D(_MainTex,float2(_HealthValue,i.uv.y));
                
                if(_HealthValue < 0.2){
                    float flash = cos(_Time.y * 10) * 0.4 +1;
                    
                    col = float4(col.xyz * flash,1);
                    
                }

                float4 outColor = lerp(_BGColor, col, t);

                outColor = float4(outColor.rgb * borderMask,1);

                return outColor;
            }
            ENDCG
        }
    }
}
