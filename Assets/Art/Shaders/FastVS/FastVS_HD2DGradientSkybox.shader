Shader "Anemora/FastVS/HD2DGradientSkybox"
{
    Properties
    {
        _DayHorizon("Day Horizon", Color) = (0.58, 0.72, 0.90, 1)
        _DayZenith("Day Zenith", Color) = (0.18, 0.34, 0.64, 1)
        _SunsetHorizon("Sunset Horizon", Color) = (1.00, 0.54, 0.26, 1)
        _SunsetZenith("Sunset Zenith", Color) = (0.26, 0.24, 0.46, 1)
        _NightHorizon("Night Horizon", Color) = (0.07, 0.10, 0.20, 1)
        _NightZenith("Night Zenith", Color) = (0.015, 0.025, 0.060, 1)
        [HDR]_SunDiscColor("Sun Disc Color", Color) = (1.0, 0.78, 0.42, 1)
        [HDR]_SunHaloColor("Sun Halo Color", Color) = (1.0, 0.45, 0.20, 1)
        [HDR]_MoonColor("Moon Color", Color) = (0.72, 0.82, 1.0, 1)
        _SunDiscSize("Sun Disc Size", Range(0.002, 0.08)) = 0.018
        _SunHaloSize("Sun Halo Size", Range(0.02, 0.35)) = 0.16
        _MoonSize("Moon Size", Range(0.004, 0.08)) = 0.028
        _MoonPhase("Moon Phase", Range(-1, 1)) = 0.42
        _BandCount("Band Count", Range(2, 32)) = 11
        _BandStrength("Band Strength", Range(0, 1)) = 0.32
        _GradientExposure("Gradient Exposure", Range(0.25, 2.5)) = 1.0
        [HDR]_StarColor("Star Color", Color) = (0.74, 0.84, 1.0, 1)
        _StarDensity("Star Density", Range(24, 220)) = 132
        _StarThreshold("Star Threshold", Range(0.90, 0.998)) = 0.974
        _StarPointSize("Star Point Size", Range(0.015, 0.22)) = 0.075
        _StarIntensity("Star Intensity", Range(0, 3)) = 1.28
        _StarTwinkleStrength("Star Twinkle Strength", Range(0, 1)) = 0.42
        _StarTwinkleSpeed("Star Twinkle Speed", Range(0.05, 3)) = 0.68
        _StarHorizonFadeStart("Star Horizon Fade Start", Range(0, 0.2)) = 0.025
        _StarHorizonFadeEnd("Star Horizon Fade End", Range(0.05, 0.7)) = 0.28
        _StarNightOpacity("Star Night Opacity", Range(0, 1)) = 0.92
        _StarMilkyWayIntensity("Star Milky Way Intensity", Range(0, 0.5)) = 0.055
        _StarReviewTime("Star Review Time", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Background"
            "RenderType" = "Background"
            "PreviewType" = "Skybox"
        }

        Pass
        {
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _DayHorizon;
            float4 _DayZenith;
            float4 _SunsetHorizon;
            float4 _SunsetZenith;
            float4 _NightHorizon;
            float4 _NightZenith;
            float4 _SunDiscColor;
            float4 _SunHaloColor;
            float4 _MoonColor;
            float _SunDiscSize;
            float _SunHaloSize;
            float _MoonSize;
            float _MoonPhase;
            float _BandCount;
            float _BandStrength;
            float _GradientExposure;
            float4 _StarColor;
            float _StarDensity;
            float _StarThreshold;
            float _StarPointSize;
            float _StarIntensity;
            float _StarTwinkleStrength;
            float _StarTwinkleSpeed;
            float _StarHorizonFadeStart;
            float _StarHorizonFadeEnd;
            float _StarNightOpacity;
            float _StarMilkyWayIntensity;
            float _StarReviewTime;
            float4 _AnemoraHd2dSkySunDirection;
            float4 _AnemoraHd2dSkyMoonDirection;

            float Hash21(float2 p)
            {
                p = frac(p * float2(123.34, 456.21));
                p += dot(p, p + 45.32);
                return frac(p.x * p.y);
            }

            float StarPoint(float2 cell, float2 localUv)
            {
                float seed = Hash21(cell);
                float mask = step(_StarThreshold, seed);
                float2 offset = float2(Hash21(cell + 17.17), Hash21(cell + 91.71));
                float distanceToStar = distance(localUv, offset);
                float starShape = 1.0 - smoothstep(_StarPointSize * 0.52, _StarPointSize, distanceToStar);
                return mask * saturate(starShape);
            }

            float3 ProceduralStarField(float3 viewDirection, float nightWeight)
            {
                float upperHemisphere = step(0.0, viewDirection.y);
                float horizonMask = smoothstep(_StarHorizonFadeStart, _StarHorizonFadeEnd, viewDirection.y) * upperHemisphere;
                float visibleNight = nightWeight * _StarNightOpacity;
                float azimuth = atan2(viewDirection.x, viewDirection.z) * 0.15915494 + 0.5;
                float2 starUv = float2(azimuth, saturate(viewDirection.y)) * max(1.0, _StarDensity);
                float2 cell = floor(starUv);
                float2 localUv = frac(starUv);
                float star = StarPoint(cell, localUv);
                float seed = Hash21(cell + 5.31);
                float twinkle = lerp(1.0, 0.45 + 0.55 * sin((_StarReviewTime * _StarTwinkleSpeed) + seed * 6.2831853), _StarTwinkleStrength);
                float milkyWay = smoothstep(0.90, 1.0, Hash21(floor(float2(azimuth * 17.0, viewDirection.y * 7.0)))) * _StarMilkyWayIntensity;
                return _StarColor.rgb * (star * twinkle + milkyWay) * horizonMask * visibleNight * _StarIntensity;
            }

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 directionOS : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.directionOS = input.positionOS.xyz;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float3 viewDirection = normalize(input.directionOS);
                float skyT = saturate(viewDirection.y * 0.5 + 0.5);
                float bandCount = max(2.0, _BandCount);
                float bandedT = floor(skyT * bandCount) / max(1.0, bandCount - 1.0);
                skyT = lerp(skyT, bandedT, saturate(_BandStrength));

                float3 sunDirection = normalize(_AnemoraHd2dSkySunDirection.xyz);
                float sunHeight = sunDirection.y;
                float nightWeight = saturate(-sunHeight * 2.25 + 0.05);
                float sunsetWeight = saturate(1.0 - abs(sunHeight) * 4.2) * (1.0 - nightWeight * 0.65);
                float3 horizon = lerp(_DayHorizon.rgb, _SunsetHorizon.rgb, sunsetWeight);
                float3 zenith = lerp(_DayZenith.rgb, _SunsetZenith.rgb, sunsetWeight);
                horizon = lerp(horizon, _NightHorizon.rgb, nightWeight);
                zenith = lerp(zenith, _NightZenith.rgb, nightWeight);
                float3 color = lerp(horizon, zenith, skyT) * _GradientExposure;
                color += ProceduralStarField(viewDirection, nightWeight);

                float sunDot = dot(viewDirection, sunDirection);
                float sunDisc = smoothstep(1.0 - _SunDiscSize, 1.0 - _SunDiscSize * 0.22, sunDot);
                float sunHalo = smoothstep(1.0 - _SunHaloSize, 1.0 - _SunDiscSize, sunDot);
                float sunVisibility = smoothstep(-0.04, 0.12, sunHeight);
                color += (_SunHaloColor.rgb * sunHalo * 0.55 + _SunDiscColor.rgb * sunDisc) * sunVisibility;

                float3 moonDirection = normalize(_AnemoraHd2dSkyMoonDirection.xyz);
                float moonDot = dot(viewDirection, moonDirection);
                float moonDisc = smoothstep(1.0 - _MoonSize, 1.0 - _MoonSize * 0.18, moonDot);
                float3 crescentCutDirection = normalize(moonDirection + float3(_MoonPhase * 0.18, 0.0, 0.0));
                float crescentCut = smoothstep(1.0 - _MoonSize * 1.22, 1.0 - _MoonSize * 0.20, dot(viewDirection, crescentCutDirection));
                float moonCrescent = saturate(moonDisc - crescentCut * 0.82);
                float moonVisibility = 1.0 - smoothstep(-0.09, 0.06, sunHeight);
                color += _MoonColor.rgb * moonCrescent * moonVisibility;

                return half4(color, 1.0);
            }
            ENDHLSL
        }
    }
}
