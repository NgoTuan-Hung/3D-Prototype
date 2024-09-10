Shader "Custom/ShaderKeyword"
{
    Properties
    {
        //[Toggle] _RED ("Make red", Float) = 0
        [KeywordEnum(ON, OFF)] _RED ("Make red", Float) = 0
        [KeywordEnum(ON, OFF)] _BLUE ("Make blue", Float) = 0
        [KeywordEnum(ON, OFF)] _GREEN ("Make green", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Add the keyword RED_ON, for when the toggle is on
            // Unity automatically adds a keyword for when the toggle is off 
            #pragma shader_feature _RED_ON _RED_OFF
            #pragma shader_feature _BLUE_ON _BLUE_OFF
            #pragma shader_feature _GREEN_ON _GREEN_OFF

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = fixed4(0, 0, 0, 1);
                #if _RED_ON
                    col += fixed4(1, 0, 0, 1);
                #else
                    col += fixed4(0, 0, 0, 1);
                #endif

                #if _BLUE_ON
                    col += fixed4(0, 0, 1, 1);
                #else
                    col += fixed4(0, 0, 0, 1);
                #endif

                #if _GREEN_ON
                    col += fixed4(0, 1, 0, 1);
                #else
                    col += fixed4(0, 0, 0, 1);
                #endif

                return col;
            }
            ENDCG
        }
    }
}