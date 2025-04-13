Shader "Custom/ImageFlipperShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        [MaterialToggle] _FlipX ("Flip X Axis", Float) = 0
        [MaterialToggle] _FlipY ("Flip Y Axis", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FlipX;
            float _FlipY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.x = lerp(uv.x, 1.0 - uv.x, _FlipX);
                uv.y = lerp(uv.y, 1.0 - uv.y, _FlipY);

                fixed4 col = tex2D(_MainTex, uv);
                col.rgb *= col.a;
                return col;
            }
            ENDCG
        }
    }

    Fallback "Sprites/Default"
}