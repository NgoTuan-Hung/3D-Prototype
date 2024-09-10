Shader "Custom/ShaderVariantExample"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            // Define keywords
            #pragma multi_compile _ DIFFUSE_SPECULAR
            #pragma multi_compile _ OPAQUE_TRANSPARENT

            // Declare variables
            uniform float4 _Color;
            uniform sampler2D _MainTex;

            struct appdata_t
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float4 color : COLOR;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, i.pos.xy);
                half4 color = _Color * texColor;
                
                #ifdef DIFFUSE_SPECULAR
                    // Apply a simple diffuse lighting model
                    color.rgb *= 0.5;
                #endif

                #ifdef OPAQUE_TRANSPARENT
                    // Make the shader render as transparent or opaque
                    color.a = 0.5;
                #endif

                return color;
            }
            ENDCG
        }
    }
}
