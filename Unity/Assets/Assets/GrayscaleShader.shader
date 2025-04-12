Shader "Custom/GrayscaleShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [Toggle] _PixelSnap ("Pixel Snap", Float) = 0
        _GrayscaleAmount ("Grayscale Amount", Range(0, 1)) = 1
        _Brightness ("Brightness", Range(1, 5)) = 2
    }

    SubShader
    {
        Tags
        {
            "CanUseSpriteAtlas" = "True"
            "IGNOREPROJECTOR" = "True"
            "PreviewType" = "Plane"
            "QUEUE" = "Transparent"
            "RenderType" = "Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _PixelSnap;
            float _GrayscaleAmount;
            float _Brightness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;

                if (_PixelSnap > 0)
                    o.vertex = UnityPixelSnap(o.vertex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;

                float luminance = Luminance(col.rgb);
                col.rgb = lerp(col.rgb, luminance.xxx, _GrayscaleAmount);

                // Brightness adjustment
                col.rgb *= _Brightness;

                return col * _Color;
            }
            ENDCG
        }
    }

    Fallback "Diffuse"
}