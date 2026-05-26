Shader "Anemora/FastVS/OutlineFullscreen"
{
    Properties
    {
        _Intensity ("Intensity", Range(0, 1)) = 0.18
        _Threshold ("Threshold", Range(0.01, 0.4)) = 0.115
        _Radius ("Radius", Range(0.5, 3)) = 1.25
        _DepthWeight ("Depth Weight", Range(0, 1)) = 0.35
        _ColorWeight ("Color Weight", Range(0, 1)) = 0.65
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off
        Cull Off
        ZTest Always

        Pass
        {
            Name "FastVSStage7Outline"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half _Intensity;
                half _Threshold;
                half _Radius;
                half _DepthWeight;
                half _ColorWeight;
            CBUFFER_END

            half4 SampleSource(float2 uv)
            {
                return SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearClamp, uv, _BlitMipLevel);
            }

            half Luma(half3 color)
            {
                return dot(color, half3(0.2126h, 0.7152h, 0.0722h));
            }

            half LinearDepth01(float2 uv)
            {
                return (half)Linear01Depth(SampleSceneDepth(uv), _ZBufferParams);
            }

            half4 Frag(Varyings input) : SV_Target0
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord.xy;
                float2 texel = _BlitTexture_TexelSize.xy * _Radius;

                half4 center = SampleSource(uv);
                half4 left = SampleSource(uv - float2(texel.x, 0.0));
                half4 right = SampleSource(uv + float2(texel.x, 0.0));
                half4 down = SampleSource(uv - float2(0.0, texel.y));
                half4 up = SampleSource(uv + float2(0.0, texel.y));

                half colorEdge =
                    abs(Luma(left.rgb) - Luma(right.rgb)) +
                    abs(Luma(down.rgb) - Luma(up.rgb)) +
                    length(left.rgb - right.rgb) * 0.18h +
                    length(down.rgb - up.rgb) * 0.18h;

                half depthCenter = LinearDepth01(uv);
                half depthEdge =
                    abs(depthCenter - LinearDepth01(uv - float2(texel.x, 0.0))) +
                    abs(depthCenter - LinearDepth01(uv + float2(texel.x, 0.0))) +
                    abs(depthCenter - LinearDepth01(uv - float2(0.0, texel.y))) +
                    abs(depthCenter - LinearDepth01(uv + float2(0.0, texel.y)));

                depthEdge = saturate(depthEdge * 48.0h);

                half weightedEdge = (colorEdge * _ColorWeight) + (depthEdge * _DepthWeight);
                half outlineMask = saturate((weightedEdge - _Threshold) * 4.0h);
                half brightGuard = 1.0h - smoothstep(0.82h, 1.0h, Luma(center.rgb));
                outlineMask *= brightGuard;

                half3 outlineColor = half3(0.085h, 0.070h, 0.052h);
                half amount = saturate(outlineMask * _Intensity);
                return half4(lerp(center.rgb, outlineColor, amount), center.a);
            }
            ENDHLSL
        }
    }
}
