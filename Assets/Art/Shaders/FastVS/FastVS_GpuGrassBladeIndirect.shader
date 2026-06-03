Shader "Anemora/FastVS/GpuGrassBladeIndirect"
{
    Properties
    {
        _GroundTint("Ground Tint", Color) = (0.36, 0.48, 0.23, 1)
        _TipTint("Tip Tint", Color) = (0.70, 0.80, 0.42, 1)
        _ShadowTint("Shadow Tint", Color) = (0.20, 0.27, 0.18, 1)
        _WindStrength("Wind Strength", Range(0, 0.25)) = 0.075
        _WindSpeed("Wind Speed", Range(0.01, 4)) = 1.25
        _PixelSnapStrength("Pixel Snap Strength", Range(0, 1)) = 0.28
        _AlphaCutoff("Alpha Cutoff", Range(0, 0.2)) = 0.02
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "AlphaTest"
            "RenderType" = "TransparentCutout"
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            ZTest LEqual
            Cull Off

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT

            #define UNITY_INDIRECT_DRAW_ARGS IndirectDrawIndexedArgs
            #include "UnityIndirect.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct BladeData
            {
                float4 positionHeight;
                float4 angleWidthPhaseVariant;
            };

            StructuredBuffer<BladeData> _AnemoraHd2dGrassVisibleBladeData;
            float4x4 _AnemoraHd2dGrassLocalToWorld;

            CBUFFER_START(UnityPerMaterial)
            float4 _GroundTint;
            float4 _TipTint;
            float4 _ShadowTint;
            float4 _FallbackWindDirection;
            float _WindStrength;
            float _WindSpeed;
            float _PixelSnapStrength;
            float _AlphaCutoff;
            CBUFFER_END

            float4 _WindDirection;
            float _WindMain;
            float _WindTurbulence;
            float _WindPulseFrequency;
            float4 _SeasonTint;
            float4 _TimeOfDayTint;
            float _Witheredness;
            TEXTURE2D(_BendMap);
            SAMPLER(sampler_BendMap);
            float4 _BendMapWorldRect;
            float _BendMapStrength;
            float _AnemoraHd2dReadabilityFloor;
            float4 _AnemoraHd2dReadabilityFloorTint;
            float _AnemoraHd2dReadabilityFloorStrength;

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float variant : TEXCOORD2;
            };

            float3 ResolveWindDirection()
            {
                float3 globalWind = float3(_WindDirection.x, 0.0, _WindDirection.z);
                float3 fallbackWind = float3(_FallbackWindDirection.x, 0.0, _FallbackWindDirection.z);
                float useGlobal = step(0.001, dot(globalWind, globalWind));
                float3 direction = lerp(fallbackWind, globalWind, useGlobal);
                return normalize(direction + float3(0.0001, 0.0, 0.0));
            }

            float FastVsSmooth01(float edge0, float edge1, float value)
            {
                float range = max(edge1 - edge0, 0.0001);
                float t = saturate((value - edge0) / range);
                return t * t * (3.0 - 2.0 * t);
            }

            float4 SampleInteractiveBendMap(float3 positionWS)
            {
                float2 size = max(_BendMapWorldRect.zw, float2(0.001, 0.001));
                float2 uv = ((positionWS.xz - _BendMapWorldRect.xy) / size) + 0.5;
                float inside = step(0.0, uv.x) * step(0.0, uv.y) * step(uv.x, 1.0) * step(uv.y, 1.0);
                float4 sampleValue = SAMPLE_TEXTURE2D_LOD(_BendMap, sampler_BendMap, saturate(uv), 0);
                sampleValue.b *= inside * saturate(_BendMapStrength);
                return sampleValue;
            }

            float3 ResolveInteractiveBendVector(float3 positionWS)
            {
                float4 sampleValue = SampleInteractiveBendMap(positionWS);
                float intensity = saturate(sampleValue.b);
                float2 direction = normalize((sampleValue.rg * 2.0 - 1.0) + float2(0.0001, 0.0001));
                return float3(direction * intensity, intensity);
            }

            float4 ApplyPixelSnap(float4 positionHCS)
            {
                float2 uv = (positionHCS.xy / max(positionHCS.w, 0.0001)) * 0.5 + 0.5;
                float2 snapped = (round(uv * _ScreenParams.xy) / _ScreenParams.xy) * 2.0 - 1.0;
                positionHCS.xy = lerp(positionHCS.xy, snapped * positionHCS.w, saturate(_PixelSnapStrength));
                return positionHCS;
            }

            Varyings Vert(Attributes input, uint svInstanceID : SV_InstanceID)
            {
                InitIndirectDrawArgs(0);
                uint instanceID = GetIndirectInstanceID(svInstanceID);
                BladeData blade = _AnemoraHd2dGrassVisibleBladeData[instanceID];

                float3 centerOS = blade.positionHeight.xyz;
                float height = blade.positionHeight.w;
                float angle = blade.angleWidthPhaseVariant.x;
                float width = blade.angleWidthPhaseVariant.y;
                float phase = blade.angleWidthPhaseVariant.z;
                float variant = blade.angleWidthPhaseVariant.w;
                float vertical = saturate(input.uv.y);
                float taper = lerp(1.0, 0.18, vertical);

                float s;
                float c;
                sincos(angle, s, c);
                float3 rightOS = float3(c, 0.0, s);
                float3 forwardOS = float3(-s, 0.0, c);
                float3 windDirectionOS = ResolveWindDirection();
                float windMain = max(_WindMain, 0.0);
                float windTurbulence = max(_WindTurbulence, 0.0);
                float pulseFrequency = max(_WindPulseFrequency, 0.85);
                float3 centerWS = mul(_AnemoraHd2dGrassLocalToWorld, float4(centerOS, 1.0)).xyz;
                float3 trample = ResolveInteractiveBendVector(centerWS);
                float wave = sin((_Time.y * (_WindSpeed + windMain * 0.65)) + phase + dot(centerOS.xz, float2(0.31, 0.47)));
                float pulse = sin((_Time.y * pulseFrequency) + phase * 1.73);
                float trampleFlatten = saturate(trample.z);
                height *= lerp(1.0, 0.42, trampleFlatten);
                float bend = ((wave * _WindStrength * (0.65 + windMain)) + (pulse * windTurbulence * 0.08)) * vertical * vertical * (1.0 - trampleFlatten * 0.72);

                float3 localOffset =
                    rightOS * ((input.uv.x - 0.5) * width * taper) +
                    float3(0.0, vertical * height, 0.0) +
                    windDirectionOS * bend +
                    forwardOS * (width * 0.18 * vertical * vertical);

                float3 positionWS = mul(_AnemoraHd2dGrassLocalToWorld, float4(centerOS + localOffset, 1.0)).xyz;
                positionWS.xz += trample.xy * vertical * 0.34;
                positionWS.y -= trample.z * vertical * 0.055;

                Varyings output;
                output.positionWS = positionWS;
                output.positionHCS = ApplyPixelSnap(TransformWorldToHClip(positionWS));
                output.uv = input.uv;
                output.variant = variant;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float vertical = saturate(input.uv.y);
                float bladeHalfWidth = lerp(0.54, 0.055, vertical);
                clip(bladeHalfWidth - abs(input.uv.x - 0.5) - _AlphaCutoff);

                half3 neutral = half3(1.0h, 1.0h, 1.0h);
                half3 ground = half3(_GroundTint.rgb);
                half3 tip = half3(_TipTint.rgb);
                half3 shadow = half3(_ShadowTint.rgb);
                half3 rgb = lerp(ground, tip, (half)smoothstep(0.04, 0.95, vertical));
                rgb *= lerp(0.86h, 1.12h, (half)input.variant);

                Light mainLight = GetMainLight();
                half ndotl = saturate(dot(half3(0.0h, 1.0h, 0.0h), mainLight.direction) * 0.5h + 0.5h);
                half3 lightTint = lerp(shadow, saturate(half3(mainLight.color.rgb) + half3(0.08h, 0.05h, -0.02h)), ndotl);
                rgb *= lerp(neutral, lightTint, 0.34h);

                half witheredness = saturate((half)_Witheredness);
                half3 seasonTint = max(half3(_SeasonTint.rgb), half3(0.001h, 0.001h, 0.001h));
                half3 todTint = max(half3(_TimeOfDayTint.rgb), half3(0.001h, 0.001h, 0.001h));
                rgb *= lerp(neutral, seasonTint, step(0.001h, dot(seasonTint, seasonTint)) * 0.18h);
                rgb *= lerp(neutral, todTint, step(0.001h, dot(todTint, todTint)) * 0.12h);
                rgb = lerp(rgb, rgb * half3(0.78h, 0.66h, 0.46h), witheredness);

                half readabilityFloor = saturate((half)_AnemoraHd2dReadabilityFloor);
                half readabilityStrength = saturate((half)_AnemoraHd2dReadabilityFloorStrength);
                half rgbLuma = dot(rgb, half3(0.2126h, 0.7152h, 0.0722h));
                half floorMask = saturate((readabilityFloor - rgbLuma) / max(readabilityFloor, 0.001h));
                half3 readabilityTint = max(half3(_AnemoraHd2dReadabilityFloorTint.rgb), half3(0.001h, 0.001h, 0.001h));
                half readabilityTintLuma = max(dot(readabilityTint, half3(0.2126h, 0.7152h, 0.0722h)), 0.001h);
                half3 floorColor = readabilityTint * (readabilityFloor / readabilityTintLuma);
                rgb = lerp(rgb, max(rgb, floorColor), floorMask * readabilityStrength);

                return half4(rgb, 1.0h);
            }
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Off

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex DepthOnlyVert
            #pragma fragment DepthOnlyFrag

            #define UNITY_INDIRECT_DRAW_ARGS IndirectDrawIndexedArgs
            #include "UnityIndirect.cginc"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct BladeData
            {
                float4 positionHeight;
                float4 angleWidthPhaseVariant;
            };

            StructuredBuffer<BladeData> _AnemoraHd2dGrassVisibleBladeData;
            float4x4 _AnemoraHd2dGrassLocalToWorld;

            CBUFFER_START(UnityPerMaterial)
            float4 _GroundTint;
            float4 _TipTint;
            float4 _ShadowTint;
            float4 _FallbackWindDirection;
            float _WindStrength;
            float _WindSpeed;
            float _PixelSnapStrength;
            float _AlphaCutoff;
            CBUFFER_END

            float4 _WindDirection;
            float _WindMain;
            float _WindTurbulence;
            float _WindPulseFrequency;
            TEXTURE2D(_BendMap);
            SAMPLER(sampler_BendMap);
            float4 _BendMapWorldRect;
            float _BendMapStrength;

            struct DepthOnlyAttributes
            {
                float3 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct DepthOnlyVaryings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float3 ResolveDepthOnlyWindDirection()
            {
                float3 globalWind = float3(_WindDirection.x, 0.0, _WindDirection.z);
                float3 fallbackWind = float3(_FallbackWindDirection.x, 0.0, _FallbackWindDirection.z);
                float useGlobal = step(0.001, dot(globalWind, globalWind));
                float3 direction = lerp(fallbackWind, globalWind, useGlobal);
                return normalize(direction + float3(0.0001, 0.0, 0.0));
            }

            float3 ResolveDepthOnlyInteractiveBendVector(float3 positionWS)
            {
                float2 size = max(_BendMapWorldRect.zw, float2(0.001, 0.001));
                float2 bendUv = ((positionWS.xz - _BendMapWorldRect.xy) / size) + 0.5;
                float inside = step(0.0, bendUv.x) * step(0.0, bendUv.y) * step(bendUv.x, 1.0) * step(bendUv.y, 1.0);
                float4 sampleValue = SAMPLE_TEXTURE2D_LOD(_BendMap, sampler_BendMap, saturate(bendUv), 0);
                float intensity = saturate(sampleValue.b * inside * saturate(_BendMapStrength));
                float2 direction = normalize((sampleValue.rg * 2.0 - 1.0) + float2(0.0001, 0.0001));
                return float3(direction * intensity, intensity);
            }

            float4 ApplyDepthOnlyPixelSnap(float4 positionHCS)
            {
                float2 uv = (positionHCS.xy / max(positionHCS.w, 0.0001)) * 0.5 + 0.5;
                float2 snapped = (round(uv * _ScreenParams.xy) / _ScreenParams.xy) * 2.0 - 1.0;
                positionHCS.xy = lerp(positionHCS.xy, snapped * positionHCS.w, saturate(_PixelSnapStrength));
                return positionHCS;
            }

            DepthOnlyVaryings DepthOnlyVert(DepthOnlyAttributes input, uint svInstanceID : SV_InstanceID)
            {
                InitIndirectDrawArgs(0);
                uint instanceID = GetIndirectInstanceID(svInstanceID);
                BladeData blade = _AnemoraHd2dGrassVisibleBladeData[instanceID];

                float3 centerOS = blade.positionHeight.xyz;
                float height = blade.positionHeight.w;
                float angle = blade.angleWidthPhaseVariant.x;
                float width = blade.angleWidthPhaseVariant.y;
                float phase = blade.angleWidthPhaseVariant.z;
                float vertical = saturate(input.uv.y);
                float taper = lerp(1.0, 0.18, vertical);

                float s;
                float c;
                sincos(angle, s, c);
                float3 rightOS = float3(c, 0.0, s);
                float3 forwardOS = float3(-s, 0.0, c);
                float3 windDirectionOS = ResolveDepthOnlyWindDirection();
                float windMain = max(_WindMain, 0.0);
                float windTurbulence = max(_WindTurbulence, 0.0);
                float pulseFrequency = max(_WindPulseFrequency, 0.85);
                float3 centerWS = mul(_AnemoraHd2dGrassLocalToWorld, float4(centerOS, 1.0)).xyz;
                float3 trample = ResolveDepthOnlyInteractiveBendVector(centerWS);
                float wave = sin((_Time.y * (_WindSpeed + windMain * 0.65)) + phase + dot(centerOS.xz, float2(0.31, 0.47)));
                float pulse = sin((_Time.y * pulseFrequency) + phase * 1.73);
                float trampleFlatten = saturate(trample.z);
                height *= lerp(1.0, 0.42, trampleFlatten);
                float bend = ((wave * _WindStrength * (0.65 + windMain)) + (pulse * windTurbulence * 0.08)) * vertical * vertical * (1.0 - trampleFlatten * 0.72);

                float3 localOffset =
                    rightOS * ((input.uv.x - 0.5) * width * taper) +
                    float3(0.0, vertical * height, 0.0) +
                    windDirectionOS * bend +
                    forwardOS * (width * 0.18 * vertical * vertical);

                float3 positionWS = mul(_AnemoraHd2dGrassLocalToWorld, float4(centerOS + localOffset, 1.0)).xyz;
                positionWS.xz += trample.xy * vertical * 0.34;
                positionWS.y -= trample.z * vertical * 0.055;

                DepthOnlyVaryings output;
                output.positionHCS = ApplyDepthOnlyPixelSnap(TransformWorldToHClip(positionWS));
                output.uv = input.uv;
                return output;
            }

            half4 DepthOnlyFrag(DepthOnlyVaryings input) : SV_TARGET
            {
                float vertical = saturate(input.uv.y);
                float bladeHalfWidth = lerp(0.54, 0.055, vertical);
                clip(bladeHalfWidth - abs(input.uv.x - 0.5) - _AlphaCutoff);
                return 0;
            }
            ENDHLSL
        }
    }
}
