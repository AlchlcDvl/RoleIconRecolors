Shader "Custom/GrayscaleShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _GrayscaleAmount ("Grayscale Amount", Range(0, 1)) = 1.0
        _Brightness ("Brightness", Range(0, 5)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

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
            float _GrayscaleAmount;
            float _Brightness; // Brightness property

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // Convert to grayscale
                float grayscale = (col.r + col.g + col.b) / 3.0;

                // Brighten the grayscale value
                grayscale *= _Brightness;

                // Interpolate between original color and grayscale
                fixed4 grayscaleCol = lerp(col, fixed4(grayscale, grayscale, grayscale, col.a), _GrayscaleAmount);

                return grayscaleCol * _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}