#ifndef FASTVS_SHARED_VEGETATION_WIND_CONTROL_INCLUDED
#define FASTVS_SHARED_VEGETATION_WIND_CONTROL_INCLUDED

TEXTURE2D(_BendMap);
SAMPLER(sampler_BendMap);

float4 _WindDirection;
float _WindMain;
float _WindTurbulence;
float _WindPulseFrequency;
float4 _SeasonTint;
float4 _TimeOfDayTint;
float _Witheredness;
float4 _BendMapWorldRect;
float _BendMapStrength;
#ifndef FASTVS_SHARED_VEGETATION_WIND_CONTROL_SKIP_CONTROL_WEIGHT_DECLS
float _VegetationControlWeight;
float _WeatherDriftWeight;
#endif

float3 FastVsResolveSharedWindDirection()
{
    float3 wind = float3(_WindDirection.x, 0.0, _WindDirection.z);
    float useWind = step(0.001, dot(wind, wind));
    wind = lerp(float3(1.0, 0.0, 0.0), wind, useWind);
    return normalize(wind + float3(0.0001, 0.0, 0.0));
}

float2 FastVsResolveBendMapUv(float3 positionWS)
{
    float2 size = max(_BendMapWorldRect.zw, float2(0.001, 0.001));
    return ((positionWS.xz - _BendMapWorldRect.xy) / size) + 0.5;
}

float4 FastVsSampleBendMapRaw(float3 positionWS)
{
    float2 uv = FastVsResolveBendMapUv(positionWS);
    float inside = step(0.0, uv.x) * step(0.0, uv.y) * step(uv.x, 1.0) * step(uv.y, 1.0);
    float4 sampleValue = SAMPLE_TEXTURE2D_LOD(_BendMap, sampler_BendMap, saturate(uv), 0);
    sampleValue.b *= inside * saturate(_BendMapStrength);
    return sampleValue;
}

float3 FastVsSampleBendMapVector(float3 positionWS)
{
    float4 sampleValue = FastVsSampleBendMapRaw(positionWS);
    float intensity = saturate(sampleValue.b);
    float2 direction = normalize((sampleValue.rg * 2.0 - 1.0) + float2(0.0001, 0.0001));
    return float3(direction * intensity, intensity);
}

float FastVsSampleBendMap(float3 positionWS)
{
    return 1.0 - saturate(FastVsSampleBendMapRaw(positionWS).b) * 0.72;
}

float3 FastVsApplySharedWindControlWithPhase(float3 positionWS, float2 uv, float localWindStrength, float vegetationWeight, float weatherDriftWeight, float windPhaseOffset)
{
    float vegetation = saturate(vegetationWeight);
    float weather = saturate(weatherDriftWeight);
    float combinedWeight = saturate(vegetation + weather);
    if (combinedWeight <= 0.0001)
    {
        return positionWS;
    }

    float3 direction = FastVsResolveSharedWindDirection();
    float windMain = max(_WindMain, 0.0);
    float windTurbulence = max(_WindTurbulence, 0.0);
    float pulseFrequency = max(_WindPulseFrequency, 0.85);
    float phase = dot(positionWS.xz, float2(0.31, 0.47)) + windPhaseOffset;
    float pulse = sin((_Time.y * pulseFrequency) + phase);
    float gust = sin((_Time.y * (1.35 + windTurbulence * 1.70)) + phase * 1.90);
    float bendFactor = FastVsSampleBendMap(positionWS);
    float3 trample = FastVsSampleBendMapVector(positionWS);
    float vegetationSway = (pulse + gust * windTurbulence * 0.65) * max(localWindStrength, 0.0) * (0.55 + windMain) * saturate(uv.y) * vegetation * bendFactor;
    float weatherDrift = (0.10 + pulse * 0.035 + gust * 0.025) * (0.45 + windMain) * weather;
    float vertical = saturate(uv.y);
    positionWS.xz += trample.xy * vertical * vegetation * 0.36;
    positionWS.y -= trample.z * vertical * vegetation * 0.075;
    positionWS.xz += direction.xz * (vegetationSway + weatherDrift);
    return positionWS;
}

float3 FastVsApplySharedWindControl(float3 positionWS, float2 uv, float localWindStrength, float vegetationWeight, float weatherDriftWeight)
{
    return FastVsApplySharedWindControlWithPhase(positionWS, uv, localWindStrength, vegetationWeight, weatherDriftWeight, 0.0);
}

half3 FastVsApplySharedVegetationTint(half3 rgb, float vegetationWeight)
{
    half weight = saturate((half)vegetationWeight);
    if (weight <= 0.0001h)
    {
        return rgb;
    }

    half3 neutral = half3(1.0h, 1.0h, 1.0h);
    half3 seasonRaw = half3(_SeasonTint.rgb);
    half3 todRaw = half3(_TimeOfDayTint.rgb);
    half seasonMask = step(0.001h, dot(max(seasonRaw, half3(0.0h, 0.0h, 0.0h)), max(seasonRaw, half3(0.0h, 0.0h, 0.0h))));
    half todMask = step(0.001h, dot(max(todRaw, half3(0.0h, 0.0h, 0.0h)), max(todRaw, half3(0.0h, 0.0h, 0.0h))));
    half3 seasonTint = lerp(neutral, max(seasonRaw, half3(0.001h, 0.001h, 0.001h)), seasonMask);
    half3 todTint = lerp(neutral, max(todRaw, half3(0.001h, 0.001h, 0.001h)), todMask);
    rgb *= lerp(neutral, seasonTint, weight * 0.18h);
    rgb *= lerp(neutral, todTint, weight * 0.12h);
    rgb = lerp(rgb, rgb * half3(0.78h, 0.66h, 0.46h), saturate((half)_Witheredness) * weight);
    return rgb;
}

#endif
