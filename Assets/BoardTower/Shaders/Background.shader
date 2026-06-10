Shader "Custom/Background"
{
    Properties
    {
        _Speed("Speed", Float) = 0.1
        _BaseColor("Base Color", Color) = (0.02, 0.03, 0.08, 1)
        _NoiseColor("Noise Color", Color) = (0.4, 0.1, 0.7, 1)
        _HighlightColor("Highlight Color", Color) = (1, 0.3, 0.8, 1)
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline"
        }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float _Speed;
                float4 _BaseColor;
                float4 _NoiseColor;
                float4 _HighlightColor;
            CBUFFER_END

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                float a = hash(i);
                float b = hash(i + float2(1, 0));
                float c = hash(i + float2(0, 1));
                float d = hash(i + float2(1, 1));

                float2 u = f * f * (3 - 2 * f);

                return lerp(
                    lerp(a, b, u.x),
                    lerp(c, d, u.x),
                    u.y
                );
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv * 2 - 1;

                float r = length(uv);
                float a = atan2(uv.y, uv.x);

                a += r * 4;

                float2 p = float2(cos(a), sin(a)) * r * 6;

                p += _Time.y * _Speed;

                float n = noise(p);

                float3 col = lerp(
                    _BaseColor.rgb,
                    _NoiseColor.rgb,
                    n
                );

                col = lerp(
                    col,
                    _HighlightColor.rgb,
                    pow(n, 4)
                );

                float vignette = smoothstep(1.3, 0.1, r);

                col *= vignette;

                return half4(col, 1);
            }

            ENDHLSL
        }
    }
    
    FallBack "Sprites/Default"
}