Shader "Unlit/energy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed ("Speed", Float) = 15.0
        _Freq ("Frequency", Float) = 8.0
        _MaxHeight ("Max Height", Float) = 0.3
        _Thickness ("Thickness", Float) = 0.005
        _Bloom ("Bloom", Float) = 0.65
        _Wobble ("Wobble", Float) = 0.1
        _RemoveBlackColor("Remove Black Color", Float) = 0.1
        _VectorColor("VectorColor", Vector) = (0.5, 0.05, 0.15, 0.)
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
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
            float _Speed;
            float _Freq;
            float _MaxHeight;
            float _Thickness;
            float _Bloom;
            float _Wobble;
            float _RemoveBlackColor;
            float3 _VectorColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float beam(float2 uv, float max_height, float offset, float speed, float freq, float thickness) 
            {
                uv.y -= 0.5;
            
                float height = max_height * (_Wobble + min(1. - uv.x, 1.));
            
                // Ramp makes the left hand side stay at/near 0
                float ramp = smoothstep(0., 2.0 / freq, uv.x);
            
                height *= ramp;
                uv.y += sin(uv.x * freq - _Time.y * speed + offset) * height;
            
                float f = thickness / abs(uv.y);
                f = pow(f, _Bloom);
                
                return f;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
    
                float f = beam(uv, _MaxHeight, 0., _Speed, _Freq * 1.5, _Thickness * 0.5) + 
                        beam(uv, _MaxHeight, _Time.y, _Speed, _Freq, _Thickness) +
                        beam(uv, _MaxHeight, _Time.y + 0.5, _Speed + 0.2, _Freq * 0.9, _Thickness * 0.5) + 
                        beam(uv, 0., 0., _Speed, _Freq, _Thickness * 3.0);
                
                fixed4 col = float4(f * _VectorColor, 1.0);
                float cond = min(min(step(_RemoveBlackColor, col.r), step(_RemoveBlackColor, col.g)), step(_RemoveBlackColor, col.b));
                col = col * cond + (1. - cond) * float4(0., 0., 0., 0.);

                return col;
            }
            ENDCG
        }
    }
}
