Shader "Anemora/FastVS/CausticsDecal"
{
    Properties
    {
        _CausticHighlightColor("Caustic Highlight Color", Color) = (0.78, 1.0, 0.88, 0.55)
        _CausticShadowColor("Caustic Shadow Color", Color) = (0.03, 0.18, 0.24, 0.25)
        _CausticEnabled("Caustic Enabled", Range(0, 1)) = 0
        _CausticIntensity("Caustic Intensity", Range(0, 2)) = 0.62
        _CausticScaleA("Caustic Scale A", Range(1, 64)) = 18
        _CausticScaleB("Caustic Scale B", Range(1, 64)) = 33
        _CausticSpeedA("Caustic Speed A", Range(0, 4)) = 0.34
        _CausticSpeedB("Caustic Speed B", Range(0, 4)) = 0.21
        _CausticCutoff("Caustic Toon Cutoff", Range(0, 1)) = 0.58
        _CausticEdgeFeather("Caustic Footprint Edge Feather", Range(0.01, 0.5)) = 0.18
        _CausticDepthFade("Caustic Depth Fade", Range(0, 1)) = 0.74
        _CausticTimeOffset("Caustic Review Time Offset", Range(0, 8)) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent+20"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest LEqual
            Cull Back

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _CausticHighlightColor;
            float4 _CausticShadowColor;
            float _CausticEnabled;
            float _CausticIntensity;
            float _CausticScaleA;
            float _CausticScaleB;
            float _CausticSpeedA;
            float _CausticSpeedB;
            float _CausticCutoff;
            float _CausticEdgeFeather;
            float _CausticDepthFade;
            float _CausticTimeOffset;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            float2 Hash22(float2 p)
            {
                p = float2(dot(p, float2(127.1f, 311.7f)), dot(p, float2(269.5f, 183.3f)));
                return frac(sin(p) * 43758.5453f);
            }

            float CausticVoronoiRidge(float2 uv, float timeValue)
            {
                float2 cell = floor(uv);
                float2 local = frac(uv);
                float nearest = 8.0f;

                [unroll]
                for (int y = -1; y <= 1; y++)
                {
                    [unroll]
                    for (int x = -1; x <= 1; x++)
                    {
                        float2 neighbor = float2((float)x, (float)y);
                        float2 randomPoint = Hash22(cell + neighbor);
                        randomPoint = 0.5f + 0.5f * sin(timeValue + 6.28318f * randomPoint);
                        float2 delta = neighbor + randomPoint - local;
                        nearest = min(nearest, dot(delta, delta));
                    }
                }

                float distanceToCell = sqrt(nearest);
                float ridge = 1.0f - smoothstep(0.10f, 0.28f, abs(distanceToCell - 0.34f));
                return saturate(ridge);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float timeValue = _CausticTimeOffset + _Time.y;
                float2 centeredUv = (input.uv - 0.5f) * 2.0f;
                float footprint = max(abs(centeredUv.x), abs(centeredUv.y));
                float edgeMask = 1.0f - smoothstep(1.0f - _CausticEdgeFeather, 1.0f, footprint);
                float depthMask = lerp(1.0f, 1.0f - saturate(length(centeredUv) * 0.52f), saturate(_CausticDepthFade));

                float2 uvA = (input.uv * _CausticScaleA) + float2(timeValue * _CausticSpeedA, -timeValue * _CausticSpeedA * 0.63f);
                float2 uvB = (input.uv * _CausticScaleB) + float2(-timeValue * _CausticSpeedB * 0.47f, timeValue * _CausticSpeedB);
                float patternA = CausticVoronoiRidge(uvA, timeValue * 0.91f);
                float patternB = CausticVoronoiRidge(uvB, timeValue * 1.27f + 2.17f);
                float dapple = min(patternA, patternB);
                float toonPattern = step(_CausticCutoff, dapple) * saturate(_CausticEnabled);
                float alpha = toonPattern * edgeMask * depthMask * _CausticIntensity * _CausticHighlightColor.a;
                half3 color = lerp((half3)_CausticShadowColor.rgb, (half3)_CausticHighlightColor.rgb, (half)toonPattern);
                return half4(color, saturate((half)alpha));
            }
            ENDHLSL
        }
    }
}
