Shader "Anemora/FastVS/SurfaceRampLit"
{
    Properties
    {
        [MainTexture]_BaseMap("Base Map", 2D) = "white" {}
        [MainColor]_BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _MainTex("Main Tex", 2D) = "white" {}
        [Toggle]_UseVertexSplat("Use Vertex RGBA Splat", Float) = 0
        [Toggle]_UseHeightAwareSplat("Use Height-Aware Splat", Float) = 0
        _SplatTransitionSharpness("Splat Transition Sharpness", Range(0, 1)) = 0.62
        _SplatHeightStrength("Splat Height Strength", Range(0, 1)) = 0.42
        [NoScaleOffset]_SplatRMap("Splat R Grass Map", 2D) = "white" {}
        [NoScaleOffset]_SplatGMap("Splat G Dirt Map", 2D) = "white" {}
        [NoScaleOffset]_SplatBMap("Splat B Path Map", 2D) = "white" {}
        [NoScaleOffset]_SplatAMap("Splat A Rock Map", 2D) = "white" {}
        _SplatRColor("Splat R Grass Color", Color) = (0.52, 0.64, 0.36, 1)
        _SplatGColor("Splat G Dirt Color", Color) = (0.60, 0.45, 0.30, 1)
        _SplatBColor("Splat B Path Color", Color) = (0.72, 0.62, 0.44, 1)
        _SplatAColor("Splat A Rock Color", Color) = (0.62, 0.60, 0.55, 1)
        [Toggle]_UseSplatATriplanar("Use Splat A Triplanar Rock", Float) = 0
        _SplatATriplanarScale("Splat A Triplanar World Scale", Range(0.05, 4)) = 1.2
        _SplatATriplanarBlendStart("Splat A Triplanar Blend Start", Range(0, 1)) = 0.58
        _SplatATriplanarBlendEnd("Splat A Triplanar Blend End", Range(0, 1)) = 0.82
        [Toggle]_UseGroundMacroVariation("Use Ground Macro Variation", Float) = 0
        _GroundMacroVariationScale("Ground Macro Variation Scale", Range(0.01, 1)) = 0.16
        _GroundMacroVariationStrength("Ground Macro Variation Strength", Range(0, 0.4)) = 0.22
        _GroundMacroHueStrength("Ground Macro Hue Strength", Range(0, 0.2)) = 0.055
        [Toggle]_UsePainterlyGroundValue("Use Painterly Ground Value/AO", Float) = 0
        _PainterlyGroundCenter("Painterly Ground Center XZ", Vector) = (20.8, 15.8, 0, 0)
        _PainterlyGroundRadius("Painterly Ground Radius", Range(1, 40)) = 12
        _PainterlyGroundValueStrength("Painterly Ground Value Strength", Range(0, 0.35)) = 0.12
        _PainterlyGroundAOStrength("Painterly Ground AO Strength", Range(0, 0.4)) = 0.18
        _PainterlyGroundWarmTint("Painterly Ground Warm Center Tint", Color) = (1.04, 1.02, 0.94, 1)
        _PainterlyGroundCoolTint("Painterly Ground Cool Recess Tint", Color) = (0.86, 0.91, 1.0, 1)
        [Toggle]_UseAerialRampTint("Use Aerial Ramp Tint", Float) = 0
        _AerialTint("Aerial Tint", Color) = (0.54, 0.64, 0.74, 1)
        _AerialTintDistance("Aerial Tint Distance Start/End", Vector) = (4.5, 17, 0, 0)
        _AerialTintStrength("Aerial Tint Strength", Range(0, 0.28)) = 0.16
        _SurfaceRampStrength("Surface Ramp Strength", Range(0, 0.5)) = 0.2
        _TopLight("Top Light", Color) = (1.05, 1.03, 0.97, 1)
        _SideShade("Side Shade", Color) = (0.95, 0.98, 1.03, 1)
        _FloorShade("Floor Shade", Color) = (0.92, 0.94, 0.97, 1)
        _Metallic("Metallic", Range(0, 1)) = 0
        _Smoothness("Smoothness", Range(0, 1)) = 0.16
        _SpecularHighlights("Specular Highlights", Float) = 0
        _SpecularPower("Stylized Specular Power", Range(4, 96)) = 46
        _SpecularStep("Stylized Specular Step", Range(0, 1)) = 0.70
        _SpecularTint("Stylized Specular Tint", Color) = (1.0, 0.86, 0.62, 1)
        _DirectionalLightStrength("Directional Light Strength", Range(0, 0.8)) = 0.12
        _ShadowReceiveStrength("Shadow Receive Strength", Range(0, 0.70)) = 0.35
        _ShadowTextureStrength("Shadow Texture Strength", Range(0, 0.5)) = 0
        [NoScaleOffset] _EmissionMap("Emission Map", 2D) = "black" {}
        [HDR] _EmissionColor("Emission Color", Color) = (0, 0, 0, 0)
        _EmissionIntensity("Emission Intensity", Range(0, 20)) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
            "RenderType" = "Opaque"
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            ZWrite On
            ZTest LEqual
            Cull Back

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _LIGHT_COOKIES
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_EmissionMap);
            SAMPLER(sampler_EmissionMap);

            TEXTURE2D(_SplatRMap);
            SAMPLER(sampler_SplatRMap);
            TEXTURE2D(_SplatGMap);
            SAMPLER(sampler_SplatGMap);
            TEXTURE2D(_SplatBMap);
            SAMPLER(sampler_SplatBMap);
            TEXTURE2D(_SplatAMap);
            SAMPLER(sampler_SplatAMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _MainTex_ST;
            float4 _BaseColor;
            float _UseVertexSplat;
            float _UseHeightAwareSplat;
            float _SplatTransitionSharpness;
            float _SplatHeightStrength;
            float4 _SplatRColor;
            float4 _SplatGColor;
            float4 _SplatBColor;
            float4 _SplatAColor;
            float _UseSplatATriplanar;
            float _SplatATriplanarScale;
            float _SplatATriplanarBlendStart;
            float _SplatATriplanarBlendEnd;
            float _UseGroundMacroVariation;
            float _GroundMacroVariationScale;
            float _GroundMacroVariationStrength;
            float _GroundMacroHueStrength;
            float _UsePainterlyGroundValue;
            float4 _PainterlyGroundCenter;
            float _PainterlyGroundRadius;
            float _PainterlyGroundValueStrength;
            float _PainterlyGroundAOStrength;
            float4 _PainterlyGroundWarmTint;
            float4 _PainterlyGroundCoolTint;
            float _UseAerialRampTint;
            float4 _AerialTint;
            float4 _AerialTintDistance;
            float _AerialTintStrength;
            float _SurfaceRampStrength;
            float4 _TopLight;
            float4 _SideShade;
            float4 _FloorShade;
            float _Metallic;
            float _Smoothness;
            float _SpecularHighlights;
            float _SpecularPower;
            float _SpecularStep;
            float4 _SpecularTint;
            float _DirectionalLightStrength;
            float _ShadowReceiveStrength;
            float _ShadowTextureStrength;
            float4 _EmissionColor;
            float _EmissionIntensity;
            CBUFFER_END
            float4 _AnemoraHd2dSunKeyColor;
            float _AnemoraHd2dSunKeyIntensity;
            float _AnemoraHd2dReadabilityFloor;
            float4 _AnemoraHd2dReadabilityFloorTint;
            float _AnemoraHd2dReadabilityFloorStrength;
            float _AnemoraHd2dWindowEmissionStrength;
            float4 _AnemoraHd2dAerialRampTint;
            float4 _AnemoraHd2dAerialRampTintDistance;
            float _AnemoraHd2dAerialRampTintStrength;
            float _AnemoraHd2dSnowAmount;
            float4 _AnemoraHd2dSnowColor;
            float _AnemoraHd2dSnowTopPower;
            float _AnemoraHd2dSnowNoiseScale;
            float _AnemoraHd2dSnowNoiseStrength;
            float _AnemoraHd2dRainWetness;
            float _AnemoraHd2dRainWetDarken;
            float _AnemoraHd2dRainSpecBoost;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                half3 normalWS : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
                half4 color : COLOR;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionHCS = TransformWorldToHClip(output.positionWS);
                output.uv = input.uv;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.color = input.color;
                return output;
            }

            float FastVsSurfaceHash21(float2 p)
            {
                return frac(sin(dot(p, float2(127.1f, 311.7f))) * 43758.5453123f);
            }

            float FastVsSurfaceValueNoise(float2 p)
            {
                float2 cell = floor(p);
                float2 local = frac(p);
                float2 blend = local * local * (3.0f - (2.0f * local));
                float a = FastVsSurfaceHash21(cell);
                float b = FastVsSurfaceHash21(cell + float2(1.0f, 0.0f));
                float c = FastVsSurfaceHash21(cell + float2(0.0f, 1.0f));
                float d = FastVsSurfaceHash21(cell + float2(1.0f, 1.0f));
                return lerp(lerp(a, b, blend.x), lerp(c, d, blend.x), blend.y);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;
                half3 normalWS = SafeNormalize(input.normalWS);
                half4 baseSample = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv) * _BaseColor;
                half4 rawWeights = saturate(input.color);
                half weightSum = max(dot(rawWeights, half4(1.0h, 1.0h, 1.0h, 1.0h)), 0.001h);
                half4 splatWeights = rawWeights / weightSum;
                half4 splatR = SAMPLE_TEXTURE2D(_SplatRMap, sampler_SplatRMap, uv) * _SplatRColor;
                half4 splatG = SAMPLE_TEXTURE2D(_SplatGMap, sampler_SplatGMap, uv) * _SplatGColor;
                half4 splatB = SAMPLE_TEXTURE2D(_SplatBMap, sampler_SplatBMap, uv) * _SplatBColor;
                half4 splatA = SAMPLE_TEXTURE2D(_SplatAMap, sampler_SplatAMap, uv) * _SplatAColor;
                half3 triplanarBlendWeights = pow(abs(normalWS), half3(4.0h, 4.0h, 4.0h));
                triplanarBlendWeights /= max(dot(triplanarBlendWeights, half3(1.0h, 1.0h, 1.0h)), 0.001h);
                float splatATriplanarScale = max(_SplatATriplanarScale, 0.001f);
                half4 splatATriplanar =
                    (SAMPLE_TEXTURE2D(_SplatAMap, sampler_SplatAMap, input.positionWS.zy * splatATriplanarScale) * triplanarBlendWeights.x) +
                    (SAMPLE_TEXTURE2D(_SplatAMap, sampler_SplatAMap, input.positionWS.xz * splatATriplanarScale) * triplanarBlendWeights.y) +
                    (SAMPLE_TEXTURE2D(_SplatAMap, sampler_SplatAMap, input.positionWS.xy * splatATriplanarScale) * triplanarBlendWeights.z);
                splatATriplanar *= _SplatAColor;
                half triplanarStart = saturate((half)_SplatATriplanarBlendStart);
                half triplanarEnd = max(saturate((half)_SplatATriplanarBlendEnd), triplanarStart + 0.001h);
                half slopeTriplanarMask = (1.0h - smoothstep(triplanarStart, triplanarEnd, abs(normalWS.y))) * step(0.5h, (half)_UseSplatATriplanar);
                splatA = lerp(splatA, splatATriplanar, slopeTriplanarMask);
                half4 splatHeight = half4(
                    dot(splatR.rgb, half3(0.2126h, 0.7152h, 0.0722h)),
                    dot(splatG.rgb, half3(0.2126h, 0.7152h, 0.0722h)),
                    dot(splatB.rgb, half3(0.2126h, 0.7152h, 0.0722h)),
                    dot(splatA.rgb, half3(0.2126h, 0.7152h, 0.0722h)));
                half heightStrength = saturate((half)_SplatHeightStrength);
                half transitionWidth = lerp(0.46h, 0.055h, saturate((half)_SplatTransitionSharpness));
                half4 heightScore = rawWeights + ((splatHeight - 0.5h) * heightStrength);
                half maxHeightScore = max(max(heightScore.r, heightScore.g), max(heightScore.b, heightScore.a));
                half4 heightWeights = saturate((heightScore - maxHeightScore + transitionWidth) / max(transitionWidth, 0.001h)) * rawWeights;
                heightWeights /= max(dot(heightWeights, half4(1.0h, 1.0h, 1.0h, 1.0h)), 0.001h);
                splatWeights = lerp(splatWeights, heightWeights, step(0.5h, (half)_UseHeightAwareSplat));
                half4 splatSample =
                    (splatR * splatWeights.r) +
                    (splatG * splatWeights.g) +
                    (splatB * splatWeights.b) +
                    (splatA * splatWeights.a);
                half macroEnabled = step(0.5h, (half)_UseGroundMacroVariation);
                float macroScale = max(_GroundMacroVariationScale, 0.001f);
                float2 macroWorldUv = input.positionWS.xz * macroScale;
                half macroA = (half)FastVsSurfaceValueNoise(macroWorldUv);
                half macroB = (half)FastVsSurfaceValueNoise((macroWorldUv * 2.73f) + 17.31f);
                half macroNoise = lerp(macroA, macroB, 0.34h);
                half macroDelta = (macroNoise - 0.5h) * 2.0h;
                half macroStrength = saturate((half)_GroundMacroVariationStrength) * macroEnabled;
                half macroHueStrength = saturate((half)_GroundMacroHueStrength) * macroEnabled;
                half3 macroHue = half3(
                    1.0h + (macroDelta * macroHueStrength),
                    1.0h + (macroDelta * macroHueStrength * 0.34h),
                    1.0h - (macroDelta * macroHueStrength * 0.48h));
                splatSample.rgb *= max(0.55h, 1.0h + (macroDelta * macroStrength));
                splatSample.rgb *= macroHue;
                baseSample = lerp(baseSample, splatSample * _BaseColor, step(0.5h, (half)_UseVertexSplat));
                float3 surfaceBreakupWorld = input.positionWS;
                half floorBreakupWeight = saturate(-normalWS.y);
                half wallBreakupWeight = saturate(1.0h - abs(normalWS.y));
                float wallBreakupAxisWeight = saturate(abs(normalWS.x) / max(abs(normalWS.x) + abs(normalWS.z), 0.0001f));
                float2 wallBreakupUv = lerp(surfaceBreakupWorld.xy, surfaceBreakupWorld.zy, wallBreakupAxisWeight);
                float2 surfaceBreakupUv = lerp(surfaceBreakupWorld.xz, wallBreakupUv, wallBreakupWeight);
                float2 surfaceBreakupCoarseCell = floor(surfaceBreakupUv * 0.74f + float2(17.23f, 41.67f));
                float surfaceBreakupCoarse = frac(sin(dot(surfaceBreakupCoarseCell, float2(12.9898f, 78.233f))) * 43758.5453f);
                float2 surfaceBreakupFineCell = floor(surfaceBreakupUv * 2.63f + float2(63.31f, 7.19f));
                float surfaceBreakupFine = frac(sin(dot(surfaceBreakupFineCell, float2(39.3468f, 11.1351f))) * 24634.6345f);
                float surfaceMaterialNoise = lerp(lerp(0.96f, 1.04f, surfaceBreakupCoarse), lerp(0.985f, 1.015f, surfaceBreakupFine), 0.35f);
                half surfaceBreakupGrade = lerp(1.0h, (half)surfaceMaterialNoise, saturate(0.18h + floorBreakupWeight * 0.08h + wallBreakupWeight * 0.05h));
                baseSample.rgb *= surfaceBreakupGrade;
                half painterlyGroundEnabled = step(0.5h, (half)_UsePainterlyGroundValue);
                float painterlyRadius = max(_PainterlyGroundRadius, 0.001f);
                float painterlyDistance = length(input.positionWS.xz - _PainterlyGroundCenter.xy) / painterlyRadius;
                half painterlyEdge = (half)smoothstep(0.22f, 1.0f, saturate(painterlyDistance));
                half painterlyCenter = 1.0h - painterlyEdge;
                half painterlyNoise = (half)FastVsSurfaceValueNoise((input.positionWS.xz * 0.28f) + float2(9.31f, 27.17f));
                half vertexPaintAo = saturate(1.0h - input.color.a) * (1.0h - step(0.5h, (half)_UseVertexSplat));
                half recessAo = saturate((painterlyNoise - 0.54h) * 2.2h) * (0.35h + painterlyEdge * 0.65h);
                half painterlyAo = saturate(max(vertexPaintAo, recessAo * 0.42h) * (half)_PainterlyGroundAOStrength);
                half painterlyValue = 1.0h + (painterlyCenter * (half)_PainterlyGroundValueStrength) - (painterlyEdge * (half)_PainterlyGroundValueStrength * 0.62h) - painterlyAo;
                half3 painterlyTint = lerp((half3)_PainterlyGroundWarmTint.rgb, (half3)_PainterlyGroundCoolTint.rgb, painterlyEdge);
                baseSample.rgb = lerp(baseSample.rgb, baseSample.rgb * max(0.55h, painterlyValue) * painterlyTint, painterlyGroundEnabled);
                half snowAmount = saturate((half)_AnemoraHd2dSnowAmount);
                half snowTopMask = pow(saturate(normalWS.y), max((half)_AnemoraHd2dSnowTopPower, 0.001h));
                half snowNoise = (half)FastVsSurfaceValueNoise((input.positionWS.xz * max(_AnemoraHd2dSnowNoiseScale, 0.001f)) + float2(5.17f, 11.83f));
                half snowBreakup = lerp(1.0h - saturate((half)_AnemoraHd2dSnowNoiseStrength), 1.0h, snowNoise);
                half snowMask = saturate(snowAmount * snowTopMask * snowBreakup);
                half3 snowColor = max((half3)_AnemoraHd2dSnowColor.rgb, half3(0.001h, 0.001h, 0.001h));
                baseSample.rgb = lerp(baseSample.rgb, max(baseSample.rgb, snowColor), snowMask);
                half rainWetness = saturate((half)_AnemoraHd2dRainWetness);
                half rainTopMask = saturate(normalWS.y * 0.86h + 0.14h);
                half rainWetMask = saturate(rainWetness * rainTopMask);
                half rainDarken = saturate((half)_AnemoraHd2dRainWetDarken);
                baseSample.rgb = lerp(baseSample.rgb, baseSample.rgb * max(0.42h, 1.0h - rainDarken), rainWetMask);
                half surfaceRampStrength = saturate((half)_SurfaceRampStrength);
                half topWeight = saturate(normalWS.y);
                half sideWeight = saturate(1.0h - abs(normalWS.y));
                half floorWeight = saturate(-normalWS.y);

                half3 neutral = half3(1.0h, 1.0h, 1.0h);
                half3 grade = neutral;
                grade *= lerp(neutral, (half3)_TopLight.rgb, topWeight * surfaceRampStrength);
                grade *= lerp(neutral, (half3)_SideShade.rgb, sideWeight * surfaceRampStrength);
                grade *= lerp(neutral, (half3)_FloorShade.rgb, floorWeight * surfaceRampStrength * 0.45h);

                float4 shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                Light mainLight = GetMainLight(shadowCoord, input.positionWS, half4(1.0h, 1.0h, 1.0h, 1.0h));
                half ndotl = saturate(dot(normalWS, mainLight.direction));
                half litResponse = smoothstep(0.12h, 0.86h, ndotl);
                half lightGrade = lerp(1.0h - (half)_DirectionalLightStrength, 1.0h + ((half)_DirectionalLightStrength * 0.72h), litResponse);
                half shadowTextureStrength = saturate((half)_ShadowTextureStrength);
                float2 shadowNoiseCell = floor(input.positionWS.xz * 3.75f + input.positionWS.y * 0.65f);
                half shadowNoise = (half)frac(sin(dot(shadowNoiseCell, float2(12.9898f, 78.233f))) * 43758.5453f);
                half shadowAttenuation = (half)mainLight.shadowAttenuation;
                half shadowResponse = smoothstep(0.10h, 0.95h, saturate(shadowAttenuation + ((shadowNoise - 0.5h) * shadowTextureStrength * 0.24h)));
                half shadowFloorLift = shadowTextureStrength * (0.52h + shadowNoise * 0.16h) * (1.0h - shadowResponse);
                half shadowGrade = lerp(saturate(1.0h - (half)_ShadowReceiveStrength + shadowFloorLift), 1.0h, shadowResponse);
                half3 mainLightColor = half3(mainLight.color.r, mainLight.color.g, mainLight.color.b);
                mainLightColor = lerp(mainLightColor, half3(_AnemoraHd2dSunKeyColor.rgb), step(0.001h, (half)_AnemoraHd2dSunKeyIntensity));
                half lightCookieLuma = dot(mainLightColor, half3(0.2126h, 0.7152h, 0.0722h));
                half lightCookieResponse = smoothstep(0.36h, 0.88h, lightCookieLuma);
                half cookieSunGrade = lerp(0.76h, 1.08h, lightCookieResponse);
                half cookieInfluence = saturate((half)_DirectionalLightStrength * litResponse);
                lightGrade *= lerp(1.0h, cookieSunGrade, cookieInfluence);
                shadowGrade *= lerp(0.93h, 1.04h, lightCookieResponse * cookieInfluence);
                half3 sunTint = lerp(half3(0.92h, 0.90h, 0.84h), saturate(mainLightColor + half3(0.06h, 0.02h, -0.02h)), litResponse * saturate((half)_DirectionalLightStrength * 1.25h));
                sunTint = lerp(sunTint * half3(0.76h, 0.76h, 0.72h), sunTint, saturate(lightCookieResponse * 0.92h + 0.08h));
                half3 texturedShadowTint = half3(0.42h, 0.46h, 0.55h) * lerp(0.92h, 1.10h, shadowNoise * shadowTextureStrength + (1.0h - shadowTextureStrength) * 0.5h);
                half3 shadowTint = lerp(texturedShadowTint, half3(1.0h, 1.0h, 1.0h), shadowResponse);

                half3 rgb = baseSample.rgb * grade * lightGrade * shadowGrade * lerp(shadowTint, sunTint, litResponse);
                half specMask = saturate((half)_SpecularHighlights);
                float3 viewDirWS = SafeNormalize(GetCameraPositionWS() - input.positionWS);
                float3 halfDirWS = SafeNormalize((float3)mainLight.direction + viewDirWS);
                half blinnSpec = pow(saturate(dot(normalWS, halfDirWS)), max((half)_SpecularPower, 0.001h));
                half breakupSpec = smoothstep(saturate((half)_SpecularStep * 0.72h), 1.0h, litResponse * lerp(0.72h, 1.0h, (half)surfaceBreakupFine));
                half steppedSpec = max(smoothstep((half)_SpecularStep, saturate((half)_SpecularStep + 0.08h), blinnSpec), breakupSpec * 0.42h);
                half sunFacingSpec = smoothstep((half)_SpecularStep * 0.42h, saturate((half)_SpecularStep + 0.18h), saturate(dot(normalWS, mainLight.direction)));
                half viewLightSpec = saturate(dot(viewDirWS, (float3)mainLight.direction) * 0.5h + 0.5h);
                half stylizedSpec = max(steppedSpec, sunFacingSpec * lerp(0.34h, 0.62h, viewLightSpec));
                half smoothSpec = saturate((half)_Smoothness);
                half metallicBoost = lerp(0.72h, 1.24h, saturate((half)_Metallic));
                half3 specSunTint = lerp((half3)_SpecularTint.rgb, saturate(mainLightColor + half3(0.08h, 0.04h, -0.02h)), 0.72h);
                rgb += specSunTint * stylizedSpec * specMask * (0.38h + smoothSpec) * metallicBoost * max(shadowResponse, 0.42h) * max(litResponse, 0.32h);
                half rainSpecBoost = saturate((half)_AnemoraHd2dRainSpecBoost) * rainWetMask;
                rgb += specSunTint * stylizedSpec * rainSpecBoost * max(shadowResponse, 0.36h) * max(litResponse, 0.24h);
                half useGlobalAerialTint = step(0.001h, (half)_AnemoraHd2dAerialRampTintStrength);
                float4 aerialTintDistanceData = lerp(_AerialTintDistance, _AnemoraHd2dAerialRampTintDistance, useGlobalAerialTint);
                float4 aerialTintData = lerp(_AerialTint, _AnemoraHd2dAerialRampTint, useGlobalAerialTint);
                half aerialTintStrength = lerp(saturate((half)_AerialTintStrength), saturate((half)_AnemoraHd2dAerialRampTintStrength), useGlobalAerialTint);
                float aerialTintStart = min(aerialTintDistanceData.x, aerialTintDistanceData.y - 0.001f);
                float aerialTintEnd = max(aerialTintDistanceData.y, aerialTintStart + 0.001f);
                half aerialTintEnabled = step(0.5h, (half)_UseAerialRampTint);
                half aerialTintAmount = saturate((half)smoothstep(aerialTintStart, aerialTintEnd, distance(GetCameraPositionWS(), input.positionWS)) * aerialTintStrength * aerialTintEnabled);
                half3 aerialTint = max((half3)aerialTintData.rgb, half3(0.001h, 0.001h, 0.001h));
                half aerialTintLuma = max(dot(aerialTint, half3(0.2126h, 0.7152h, 0.0722h)), 0.001h);
                half aerialSourceLuma = max(dot(rgb, half3(0.2126h, 0.7152h, 0.0722h)), 0.001h);
                half3 aerialPreservedLuma = aerialTint * (aerialSourceLuma / aerialTintLuma);
                rgb = lerp(rgb, aerialPreservedLuma, aerialTintAmount);
                half3 emissionColor = half3(_EmissionColor.r, _EmissionColor.g, _EmissionColor.b);
                half windowEmissionStrength = max((half)_AnemoraHd2dWindowEmissionStrength, 0.0h);
                half3 emission = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, uv).rgb * emissionColor * (half)_EmissionIntensity * windowEmissionStrength;
                rgb += emission;
                half readabilityFloor = saturate((half)_AnemoraHd2dReadabilityFloor);
                half readabilityStrength = saturate((half)_AnemoraHd2dReadabilityFloorStrength);
                half rgbLuma = dot(rgb, half3(0.2126h, 0.7152h, 0.0722h));
                half floorMask = saturate((readabilityFloor - rgbLuma) / max(readabilityFloor, 0.001h));
                half3 readabilityTint = max(half3(_AnemoraHd2dReadabilityFloorTint.rgb), half3(0.001h, 0.001h, 0.001h));
                half readabilityTintLuma = max(dot(readabilityTint, half3(0.2126h, 0.7152h, 0.0722h)), 0.001h);
                half3 floorColor = readabilityTint * (readabilityFloor / readabilityTintLuma);
                rgb = lerp(rgb, max(rgb, floorColor), floorMask * readabilityStrength);
                return half4(rgb, baseSample.a);
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
            Cull Back

            HLSLPROGRAM
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _MainTex_ST;
            float4 _BaseColor;
            float _UseVertexSplat;
            float _UseHeightAwareSplat;
            float _SplatTransitionSharpness;
            float _SplatHeightStrength;
            float4 _SplatRColor;
            float4 _SplatGColor;
            float4 _SplatBColor;
            float4 _SplatAColor;
            float _UseSplatATriplanar;
            float _SplatATriplanarScale;
            float _SplatATriplanarBlendStart;
            float _SplatATriplanarBlendEnd;
            float _UseGroundMacroVariation;
            float _GroundMacroVariationScale;
            float _GroundMacroVariationStrength;
            float _GroundMacroHueStrength;
            float _UsePainterlyGroundValue;
            float4 _PainterlyGroundCenter;
            float _PainterlyGroundRadius;
            float _PainterlyGroundValueStrength;
            float _PainterlyGroundAOStrength;
            float4 _PainterlyGroundWarmTint;
            float4 _PainterlyGroundCoolTint;
            float _UseAerialRampTint;
            float4 _AerialTint;
            float4 _AerialTintDistance;
            float _AerialTintStrength;
            float _SurfaceRampStrength;
            float4 _TopLight;
            float4 _SideShade;
            float4 _FloorShade;
            float _Metallic;
            float _Smoothness;
            float _SpecularHighlights;
            float _SpecularPower;
            float _SpecularStep;
            float4 _SpecularTint;
            float _DirectionalLightStrength;
            float _ShadowReceiveStrength;
            float _ShadowTextureStrength;
            float4 _EmissionColor;
            float _EmissionIntensity;
            CBUFFER_END

            struct DepthOnlyAttributes
            {
                float4 positionOS : POSITION;
            };

            struct DepthOnlyVaryings
            {
                float4 positionCS : SV_POSITION;
            };

            DepthOnlyVaryings DepthOnlyVertex(DepthOnlyAttributes input)
            {
                DepthOnlyVaryings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionCS = TransformWorldToHClip(positionWS);
                return output;
            }

            half4 DepthOnlyFragment(DepthOnlyVaryings input) : SV_TARGET
            {
                return 0;
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _MainTex_ST;
            float4 _BaseColor;
            float _UseVertexSplat;
            float _UseHeightAwareSplat;
            float _SplatTransitionSharpness;
            float _SplatHeightStrength;
            float4 _SplatRColor;
            float4 _SplatGColor;
            float4 _SplatBColor;
            float4 _SplatAColor;
            float _UseSplatATriplanar;
            float _SplatATriplanarScale;
            float _SplatATriplanarBlendStart;
            float _SplatATriplanarBlendEnd;
            float _UseGroundMacroVariation;
            float _GroundMacroVariationScale;
            float _GroundMacroVariationStrength;
            float _GroundMacroHueStrength;
            float _UsePainterlyGroundValue;
            float4 _PainterlyGroundCenter;
            float _PainterlyGroundRadius;
            float _PainterlyGroundValueStrength;
            float _PainterlyGroundAOStrength;
            float4 _PainterlyGroundWarmTint;
            float4 _PainterlyGroundCoolTint;
            float _UseAerialRampTint;
            float4 _AerialTint;
            float4 _AerialTintDistance;
            float _AerialTintStrength;
            float _SurfaceRampStrength;
            float4 _TopLight;
            float4 _SideShade;
            float4 _FloorShade;
            float _Metallic;
            float _Smoothness;
            float _SpecularHighlights;
            float _SpecularPower;
            float _SpecularStep;
            float4 _SpecularTint;
            float _DirectionalLightStrength;
            float _ShadowReceiveStrength;
            float _ShadowTextureStrength;
            float4 _EmissionColor;
            float _EmissionIntensity;
            CBUFFER_END

            float3 _LightDirection;
            float3 _LightPosition;

            struct ShadowAttributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct ShadowVaryings
            {
                float4 positionCS : SV_POSITION;
            };

            ShadowVaryings ShadowPassVertex(ShadowAttributes input)
            {
                ShadowVaryings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

                #if defined(_CASTING_PUNCTUAL_LIGHT_SHADOW)
                    float3 lightDirectionWS = normalize(_LightPosition - positionWS);
                #else
                    float3 lightDirectionWS = _LightDirection;
                #endif

                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
                #if UNITY_REVERSED_Z
                    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #else
                    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #endif

                output.positionCS = positionCS;
                return output;
            }

            half4 ShadowPassFragment(ShadowVaryings input) : SV_TARGET
            {
                return 0;
            }
            ENDHLSL
        }
    }
}
