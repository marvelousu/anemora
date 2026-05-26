Shader "Anemora/FastVS/TiltShiftFullscreen"
{
    Properties
    {
        _Intensity ("Intensity", Range(0, 1)) = 0.68
        _Radius ("Radius", Range(0, 6)) = 2.25
        _SharpCenter ("Sharp Center", Range(0, 1)) = 0.50
        _SharpWidth ("Sharp Width", Range(0, 0.5)) = 0.18
        _Feather ("Feather", Range(0.01, 0.5)) = 0.34
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off
        Cull Off
        ZTest Always

        Pass
        {
            Name "FastVSStage7TiltShift"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half _Intensity;
                half _Radius;
                half _SharpCenter;
                half _SharpWidth;
                half _Feather;
            CBUFFER_END

            half4 SampleSource(float2 uv)
            {
                return SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearClamp, uv, _BlitMipLevel);
            }

            half GetBlurMask(half distanceFromFocus)
            {
                half mask = smoothstep(_SharpWidth, _SharpWidth + _Feather, distanceFromFocus);
                return saturate(mask * mask * (1.35h - 0.35h * mask));
            }

            half4 SampleBlurRing(float2 uv, float2 texelOffset, half ringScale, half centerWeight, half axisWeight, half diagonalWeight)
            {
                float2 offset = texelOffset * ringScale;
                half4 blur = 0.0h;

                blur += SampleSource(uv) * centerWeight;
                blur += SampleSource(uv + float2(offset.x, 0.0)) * axisWeight;
                blur += SampleSource(uv - float2(offset.x, 0.0)) * axisWeight;
                blur += SampleSource(uv + float2(0.0, offset.y)) * axisWeight;
                blur += SampleSource(uv - float2(0.0, offset.y)) * axisWeight;
                blur += SampleSource(uv + offset) * diagonalWeight;
                blur += SampleSource(uv - offset) * diagonalWeight;
                blur += SampleSource(uv + float2(offset.x, -offset.y)) * diagonalWeight;
                blur += SampleSource(uv + float2(-offset.x, offset.y)) * diagonalWeight;

                return blur;
            }

            half4 Frag(Varyings input) : SV_Target0
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord.xy;
                half4 sharp = SampleSource(uv);
                half distanceFromFocus = abs((half)uv.y - _SharpCenter);
                half blurMask = GetBlurMask(distanceFromFocus);
                half radius = _Radius * lerp(0.55h, 1.0h, blurMask);
                float2 texel = _BlitTexture_TexelSize.xy * radius;

                half4 blurNear = SampleBlurRing(uv, texel, 0.95h, 0.08h, 0.11h, 0.12h);
                half4 blurFar = SampleBlurRing(uv, texel, 1.85h, 0.04h, 0.12h, 0.12h);
                half4 blur = lerp(blurNear, blurFar, blurMask * blurMask);

                half amount = saturate(blurMask * _Intensity);
                return lerp(sharp, blur, amount);
            }
            ENDHLSL
        }
    }
}
