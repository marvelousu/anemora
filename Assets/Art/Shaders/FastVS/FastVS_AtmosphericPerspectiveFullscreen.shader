Shader "Anemora/FastVS/AtmosphericPerspectiveFullscreen"
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
            Name "FastVSAtmosphericPerspective"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half _LocalIntensity;
            CBUFFER_END

            float _AnemoraHd2dAtmosphericFogStrength;
            float4 _AnemoraHd2dAtmosphericFogNearColor;
            float4 _AnemoraHd2dAtmosphericFogFarColor;
            float4 _AnemoraHd2dAtmosphericFogDistance;
            float4 _AnemoraHd2dAtmosphericFogHeight;
            float _AnemoraHd2dAtmosphericFogHeightStrength;
            float _AnemoraHd2dAtmosphericFogGradientBlend;
            TEXTURE2D(_AnemoraHd2dAtmosphericFogGradientTex);
            SAMPLER(sampler_AnemoraHd2dAtmosphericFogGradientTex);

            half4 SampleSource(float2 uv)
            {
                return SAMPLE_TEXTURE2D_X_LOD(_BlitTexture, sampler_LinearClamp, uv, _BlitMipLevel);
            }

            half3 LuminanceRgb(half3 color)
            {
                half luma = dot(color, half3(0.2126h, 0.7152h, 0.0722h));
                return luma.xxx;
            }

            half4 Frag(Varyings input) : SV_Target0
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv = input.texcoord.xy;
                half4 source = SampleSource(uv);
                float rawDepth = SampleSceneDepth(uv);

                #if UNITY_REVERSED_Z
                if (rawDepth <= 0.0001f)
                {
                    return source;
                }
                #else
                if (rawDepth >= 0.9999f)
                {
                    return source;
                }
                #endif

                float3 positionWS = ComputeWorldSpacePosition(uv, rawDepth, UNITY_MATRIX_I_VP);
                float cameraDistance = distance(_WorldSpaceCameraPos, positionWS);
                float distanceStart = min(_AnemoraHd2dAtmosphericFogDistance.x, _AnemoraHd2dAtmosphericFogDistance.y - 0.001f);
                float distanceEnd = max(_AnemoraHd2dAtmosphericFogDistance.y, distanceStart + 0.001f);
                float distanceFactor = smoothstep(distanceStart, distanceEnd, cameraDistance);

                float heightStart = min(_AnemoraHd2dAtmosphericFogHeight.x, _AnemoraHd2dAtmosphericFogHeight.y - 0.001f);
                float heightEnd = max(_AnemoraHd2dAtmosphericFogHeight.y, heightStart + 0.001f);
                float lowAirFactor = 1.0f - smoothstep(heightStart, heightEnd, positionWS.y);
                float heightFactor = lerp(1.0f, lowAirFactor, saturate(_AnemoraHd2dAtmosphericFogHeightStrength));

                half3 gradientSample = SAMPLE_TEXTURE2D(
                    _AnemoraHd2dAtmosphericFogGradientTex,
                    sampler_AnemoraHd2dAtmosphericFogGradientTex,
                    float2(distanceFactor, 0.5f)).rgb;
                half3 fallbackGradient = lerp(
                    (half3)_AnemoraHd2dAtmosphericFogNearColor.rgb,
                    (half3)_AnemoraHd2dAtmosphericFogFarColor.rgb,
                    (half)distanceFactor);
                half3 fogColor = lerp(fallbackGradient, gradientSample, saturate((half)_AnemoraHd2dAtmosphericFogGradientBlend));

                half amount = saturate((half)(distanceFactor * heightFactor * _AnemoraHd2dAtmosphericFogStrength) * _LocalIntensity);
                half3 softenedSource = lerp(source.rgb, LuminanceRgb(source.rgb), amount * 0.10h);
                half3 result = lerp(softenedSource, fogColor, amount);
                return half4(result, source.a);
            }
            ENDHLSL
        }
    }
}
