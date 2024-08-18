Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Color ("Color", Color) = (1,1,1,1)
        _UVScaling ("UV Scaling", Vector) = (1,1,1,1)
        _TimeScale ("Time Scale", Float) = 0.1
        _Duration ("Duration", Float) = 2
    }
    SubShader
    {
        Tags {"Queue"="Transparent"}
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            

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
            float4 _Color;
            float2 _UVScaling;
            float _TimeScale;
            float _Duration;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float periodic(float x, float period)
            {
                return (x % period) / period;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                i.uv = clamp(i.uv / _UVScaling.xy, 0.0, 1.0) - float2(1., 0.);
                fixed4 col = tex2D(_MainTex, float2(clamp(i.uv.x + 2 * periodic(_Time.y * _TimeScale, _Duration), 0., 1.), i.uv.y)) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
