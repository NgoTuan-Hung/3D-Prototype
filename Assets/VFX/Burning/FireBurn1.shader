Shader "Unlit/FireBurn1"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _A1 ("A", Float) = 0.3693
        _B1 ("B", Float) = 46.875
        _C1 ("C", Float) = 23409.0909
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float A1;
            float B1;
            float C1;

            float rand(float2 co) 
            {
                return frac(sin(dot(co.xy, float2(A1, B1))) * C1);
            }

            float rand1(float2 co) 
            {
                return frac(sin(dot(co.xy, float2(0.3693, 46.875))) * 23409.0909);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                
                return float4(rand1(i.uv).xxx, 0.);
            }
            ENDCG
        }
    }
}
