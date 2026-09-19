Shader "Custom/CRT"
{
    Properties
    {
        _ScanlineStrength ("Scanline Strength", Range(0, 1)) = 0.08
        _ScanlineCount ("Scanline Count", Float) = 180
        _VignetteStrength ("Vignette Strength", Range(0, 1)) = 0.08
        _RGBShift ("RGB Shift", Range(0, 2)) = 0.0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }

        Pass
        {
            Name "CRT"

            ZWrite Off
            ZTest Always
            Cull Off

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D_X(_BlitTexture);

            float _ScanlineStrength;
            float _ScanlineCount;
            float _VignetteStrength;
            float _RGBShift;

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

                output.positionCS =
                    GetFullScreenTriangleVertexPosition(input.vertexID);

                output.uv =
                    GetFullScreenTriangleTexCoord(input.vertexID);

                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;

                // --------------------------------
                // RGB Shift
                // --------------------------------

                float2 shift = float2(
                    _RGBShift / _ScreenParams.x,
                    0.0
                );

                half r = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv + shift
                ).r;

                half g = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv
                ).g;

                half b = SAMPLE_TEXTURE2D_X(
                    _BlitTexture,
                    sampler_LinearClamp,
                    uv - shift
                ).b;

                half3 color = half3(r, g, b);

                // --------------------------------
                // Scanline
                // --------------------------------

                float scanline =
                    sin(uv.y * _ScanlineCount * 3.14159265);

                scanline =
                    scanline * 0.5 + 0.5;

                float scanlineFactor =
                    lerp(
                        1.0 - _ScanlineStrength,
                        1.0,
                        scanline
                    );

                color *= scanlineFactor;

                // --------------------------------
                // Vignette
                // --------------------------------

                float2 centeredUV =
                    uv * 2.0 - 1.0;

                float distanceFromCenter =
                    dot(centeredUV, centeredUV);

                float vignette =
                    1.0 - distanceFromCenter * _VignetteStrength;

                vignette = saturate(vignette);

                color *= vignette;

                return half4(color, 1.0);
            }

            ENDHLSL
        }
    }
}