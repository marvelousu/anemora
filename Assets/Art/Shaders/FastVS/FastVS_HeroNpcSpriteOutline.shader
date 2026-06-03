Shader "Anemora/FastVS/HeroNpcSpriteOutline"
{
    Properties
    {
        [MainTexture]_BaseMap("Base Map", 2D) = "white" {}
        [MainColor]_BaseColor("Outline Color", Color) = (0.08, 0.10, 0.14, 0.78)
        _MainTex("Main Tex", 2D) = "white" {}
        _OutlineWidthTexels("Outline Width Texels", Range(0.25, 3.0)) = 1.10
        _OutlineAlpha("Outline Alpha", Range(0, 1)) = 0.78
        _Cutoff("Alpha Cutoff", Range(0.01, 0.55)) = 0.16
        [HideInInspector]_Surface("__surface", Float) = 1
        [HideInInspector]_AlphaClip("__alphaClip", Float) = 0
        [HideInInspector]_ZWrite("__zw", Float) = 0
        [HideInInspector]_Cull("__cull", Float) = 0
        [HideInInspector]_ZTest("__ztest", Float) = 4
        [HideInInspector]_SrcBlend("__src", Float) = 5
        [HideInInspector]_DstBlend("__dst", Float) = 10
        [HideInInspector]_QueueControl("__queueControl", Float) = 1
        [HideInInspector]_QueueOffset("__queueOffset", Float) = 18
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            ZTest [_ZTest]
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseMap_TexelSize;

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _BaseColor;
            float _OutlineWidthTexels;
            float _OutlineAlpha;
            float _Cutoff;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;
                return output;
            }

            float SampleAlpha(float2 uv)
            {
                return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv).a;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                float2 stepUv = _BaseMap_TexelSize.xy * max(_OutlineWidthTexels, 0.25);
                float centerAlpha = SampleAlpha(input.uv);
                float neighborAlpha = 0.0;
                neighborAlpha = max(neighborAlpha, SampleAlpha(input.uv + float2(stepUv.x, 0.0)));
                neighborAlpha = max(neighborAlpha, SampleAlpha(input.uv - float2(stepUv.x, 0.0)));
                neighborAlpha = max(neighborAlpha, SampleAlpha(input.uv + float2(0.0, stepUv.y)));
                neighborAlpha = max(neighborAlpha, SampleAlpha(input.uv - float2(0.0, stepUv.y)));
                neighborAlpha = max(neighborAlpha, SampleAlpha(input.uv + float2(stepUv.x, stepUv.y)));
                neighborAlpha = max(neighborAlpha, SampleAlpha(input.uv + float2(-stepUv.x, stepUv.y)));
                neighborAlpha = max(neighborAlpha, SampleAlpha(input.uv + float2(stepUv.x, -stepUv.y)));
                neighborAlpha = max(neighborAlpha, SampleAlpha(input.uv + float2(-stepUv.x, -stepUv.y)));

                float outsideCenter = 1.0 - smoothstep(_Cutoff * 0.55, _Cutoff * 1.10, centerAlpha);
                float neighborHit = smoothstep(_Cutoff * 0.60, _Cutoff * 1.05, neighborAlpha);
                float outlineMask = saturate(outsideCenter * neighborHit);
                float alpha = outlineMask * _OutlineAlpha * _BaseColor.a;
                clip(alpha - 0.002);
                return half4((half3)_BaseColor.rgb, (half)alpha);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
