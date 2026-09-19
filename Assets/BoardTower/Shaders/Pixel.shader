Shader "Custom/Pixel"
{
    Properties
    {
        _PixelWidth ("Pixel Width", Float) = 320
        _PixelHeight ("Pixel Height", Float) = 180
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "Pixel"

            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D_X(_BlitTexture);
            SAMPLER(sampler_BlitTexture);

            float _PixelWidth;
            float _PixelHeight;

            struct Attributes
            {
                uint vertexID : SV_VertexID;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;

                output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
                output.uv = GetFullScreenTriangleTexCoord(input.vertexID);

                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 resolution = _ScreenParams.xy;

                float2 pixelSize = resolution / float2(
                    _PixelWidth,
                    _PixelHeight
                );

                float2 pixelUV =
                    floor(input.uv * float2(_PixelWidth, _PixelHeight))
                    / float2(_PixelWidth, _PixelHeight);

                return SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_BlitTexture,
                    pixelUV
                );
            }

            ENDHLSL
        }
    }
}