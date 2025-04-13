Shader "Custom/GrayscaleShader"
{
    Properties
    {
        _Stencil ("Stencil ID", Range(0, 255)) = 1
        [Enum(UnityEngine.Rendering.ColorWriteMask)] _ColorMask ("Color Mask", Int) = 15
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Int) = 8
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilOp ("Stencil Operation", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilFail ("Stencil Fail", Int) = 0
        [Enum(UnityEngine.Rendering.StencilOp)] _StencilZFail ("Stencil ZFail", Int) = 0
        _StencilWriteMask ("Stencil Write Mask", Range(0, 255)) = 255
        _StencilReadMask ("Stencil Read Mask", Range(0, 255)) = 255
        _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
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

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            Fail [_StencilFail]
            ZFail [_StencilZFail]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off
            ColorMask [_ColorMask]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
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
            float _GrayscaleAmount;
            float _Brightness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
                float luminance = Luminance(col.rgb);
                col.rgb = lerp(col.rgb, luminance.xxx, _GrayscaleAmount);
                col.rgb *= _Brightness;
                return col * _Color;
            }

            ENDCG
        }
    }

    Fallback "Sprites/Default"
}