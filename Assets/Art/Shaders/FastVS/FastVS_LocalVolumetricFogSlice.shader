Shader "Anemora/FastVS/LocalVolumetricFogSlice"
{
    Properties
    {
        _FogColor("Fog Color", Color) = (0.70, 0.86, 1.00, 0.45)
        _FogDensity("Fog Density", Range(0, 1)) = 0.25
        _EdgeFeather("Edge Feather", Range(0.05, 0.95)) = 0.42
        _NoiseStrength("Noise Strength", Range(0, 1)) = 0.18
        _NoiseScale("Noise Scale", Range(0.2, 12)) = 4.2
        _HeightFade("Height Fade", Range(0, 1.5)) = 0.34
        _PortalGlow("Portal Glow", Range(0, 1)) = 0.0
        _TimeOffset("Review Time Offset", Range(0, 8)) = 0.0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent+35"
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
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _FogColor;
            float _FogDensity;
            float _EdgeFeather;
            float _NoiseStrength;
            float _NoiseScale;
            float _HeightFade;
            float _PortalGlow;
            float _TimeOffset;
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
                float3 positionOS : TEXCOORD1;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                output.positionOS = input.positionOS.xyz;
                return output;
            }

            float Hash21(float2 p)
            {
                p = frac(p * float2(123.34f, 456.21f));
                p += dot(p, p + 45.32f);
                return frac(p.x * p.y);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 centeredUv = (input.uv - 0.5f) * 2.0f;
                float radial = length(centeredUv);
                float edge = saturate((1.0f - radial) / max(_EdgeFeather, 0.001f));
                edge = smoothstep(0.0f, 1.0f, edge);

                float timeValue = _Time.y + _TimeOffset;
                float baseNoise = Hash21(floor((input.uv * _NoiseScale) + float2(timeValue * 0.21f, -timeValue * 0.17f)));
                float waveNoise = 0.5f + 0.5f * sin((input.uv.x * 19.1f + input.uv.y * 13.7f) * _NoiseScale + timeValue * 0.73f);
                float noise = lerp(1.0f, lerp(baseNoise, waveNoise, 0.55f), saturate(_NoiseStrength));

                float vertical = saturate(input.uv.y);
                float heightMask = lerp(1.0f, 1.0f - abs(vertical - 0.58f) * 0.72f, saturate(_HeightFade));
                float alpha = saturate(_FogDensity * _FogColor.a * edge * noise * heightMask);
                float3 glowColor = _FogColor.rgb + _PortalGlow * float3(0.18f, 0.26f, 0.32f) * edge;
                return half4((half3)glowColor, (half)alpha);
            }
            ENDHLSL
        }
    }
}
