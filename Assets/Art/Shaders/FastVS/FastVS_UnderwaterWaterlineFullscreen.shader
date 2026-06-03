Shader "Anemora/FastVS/UnderwaterWaterlineFullscreen"
{
    Properties
    {
        _LocalIntensity ("Local Intensity", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off
        Cull Off
        ZTest Always

        Pass
        {
            Name "FastVSUnderwaterWaterline"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half _LocalIntensity;
            CBUFFER_END

            float _AnemoraHd2dUnderwaterBlend;
            float4 _AnemoraHd2dUnderwaterTint;
            float4 _AnemoraHd2dUnderwaterFogDistortion;
            float4 _AnemoraHd2dUnderwaterWaterline;
            float4 _AnemoraHd2dUnderwaterCaustics;

            half4 SampleSource(float2 uv)
            {
                return SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearClamp, uv, _BlitMipLevel);
            }

            half Luma(half3 color)
            {
                return dot(color, half3(0.2126h, 0.7152h, 0.0722h));
            }

            float CheapWave(float2 uv, float timeOffset)
            {
                float a = sin((uv.y * 31.0f) + (timeOffset * 3.7f));
                float b = sin(((uv.x + uv.y) * 19.0f) - (timeOffset * 2.4f));
                float c = sin(((uv.x * 17.0f) - (uv.y * 11.0f)) + timeOffset);
                return ((a * 0.50f) + (b * 0.32f) + (c * 0.18f));
            }

            half4 Frag(Varyings input) : SV_Target0
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord.xy;
                half blend = saturate((half)(_AnemoraHd2dUnderwaterBlend * _LocalIntensity));
                if (blend <= 0.001h)
                {
                    return SampleSource(uv);
                }

                float timeOffset = _AnemoraHd2dUnderwaterCaustics.y;
                float waveX = CheapWave(uv, timeOffset);
                float waveY = CheapWave(uv.yx + 0.37f, timeOffset * 1.21f);
                float2 distortion = float2(waveX, waveY * 0.58f) *
                    _BlitTexture_TexelSize.xy *
                    _AnemoraHd2dUnderwaterFogDistortion.z *
                    blend;

                half4 source = SampleSource(saturate(uv + distortion));
                float rawDepth = SampleSceneDepth(uv);
                float depth01 = saturate(Linear01Depth(rawDepth, _ZBufferParams));

                half3 gray = Luma(source.rgb).xxx;
                half desaturation = saturate((half)(_AnemoraHd2dUnderwaterFogDistortion.w * blend));
                half3 softened = lerp(source.rgb, gray, desaturation);

                float fogAmount = saturate(_AnemoraHd2dUnderwaterFogDistortion.x * (0.28f + depth01 * 0.72f) * blend);
                half tintAmount = saturate((half)((_AnemoraHd2dUnderwaterFogDistortion.y + fogAmount * 0.45f) * blend));
                half3 tint = (half3)_AnemoraHd2dUnderwaterTint.rgb;
                half3 result = lerp(softened, tint, tintAmount);

                float waterlineDistance = abs(uv.y - _AnemoraHd2dUnderwaterWaterline.x);
                float waterline = 1.0f - smoothstep(0.0f, max(_AnemoraHd2dUnderwaterWaterline.y, 0.001f), waterlineDistance);
                result = lerp(result, saturate(tint + half3(0.18h, 0.22h, 0.20h)), (half)(waterline * _AnemoraHd2dUnderwaterWaterline.z * blend));

                float rays = pow(saturate(sin((uv.x * 26.0f) + (uv.y * 7.0f) + timeOffset * 4.2f) * 0.5f + 0.5f), 9.0f);
                rays *= 1.0f - smoothstep(0.25f, 1.0f, uv.y);
                result += tint * (half)(rays * _AnemoraHd2dUnderwaterCaustics.x * blend);

                float edgeDistance = distance(uv, float2(0.5f, 0.5f)) * 1.42f;
                float edgeMask = smoothstep(0.48f, 1.0f, edgeDistance);
                result = lerp(result, tint * 0.58h, (half)(edgeMask * _AnemoraHd2dUnderwaterWaterline.w * blend));

                return half4(saturate(result), source.a);
            }
            ENDHLSL
        }
    }
}
