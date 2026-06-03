Shader "Anemora/FastVS/DepthGradientWater"
{
    Properties
    {
        [MainTexture]_BaseMap("Pixel Ripple Map", 2D) = "white" {}
        _MainTex("Main Tex", 2D) = "white" {}
        _FlatColor("Flat Baseline Color", Color) = (0.22, 0.47, 0.59, 1)
        _ShallowColor("Shallow Shore Color", Color) = (0.34, 0.92, 0.86, 1)
        _DeepColor("Deep Center Color", Color) = (0.03, 0.20, 0.36, 1)
        _DepthGradientStrength("Depth Gradient Strength", Range(0, 1)) = 1
        _DeepDistance("Scene Deep Distance Meters", Range(0.05, 5)) = 1.2
        _ObjectDeepDistance("Object-Space Deep Distance", Range(0.05, 0.5)) = 0.28
        _SceneDepthWeight("Scene Depth Weight", Range(0, 1)) = 0.55
        _ObjectDepthWeight("Object Depth Weight", Range(0, 1)) = 1
        _PixelRippleStrength("Pixel Ripple Strength", Range(0, 0.4)) = 0.12
        _FoamStrength("Depth-Intersection Foam Strength", Range(0, 1)) = 0.78
        _FoamDistance("Foam Distance", Range(0.02, 0.6)) = 0.34
        _FoamColor("Foam Color", Color) = (0.86, 1.0, 0.92, 1)
        _FoamCutoff("Foam Toon Cutoff", Range(0, 1)) = 0.52
        _FoamNoiseScale("Foam Voronoi Noise Scale", Range(1, 64)) = 24
        _FoamNoiseStrength("Foam Noise Breakup", Range(0, 0.6)) = 0.34
        _FoamScrollSpeed("Foam Scroll Speed", Range(0, 3)) = 0.65
        _FoamInnerOffset("Inner Foam Line Offset", Range(0.02, 0.6)) = 0.13
        _FoamInnerWidth("Inner Foam Line Width", Range(0.005, 0.2)) = 0.035
        _FoamTimeOffset("Foam Review Time Offset", Range(0, 8)) = 0
        _FlowMap("Directional Flow Map RG + Strength B", 2D) = "gray" {}
        _DirectionalFlowEnabled("Directional Flow Enabled", Range(0, 1)) = 0
        _FlowStrength("Directional Flow Strength", Range(0, 2)) = 0.82
        _FlowSpeed("Directional Flow Speed", Range(0, 3)) = 0.95
        _FlowTimeOffset("Directional Flow Review Time Offset", Range(0, 8)) = 0
        _FlowFoamAdvectionStrength("Foam Flow Advection Strength", Range(0, 3)) = 1.25
        _FlowSpecularAdvectionStrength("Specular Flow Advection Strength", Range(0, 3)) = 0.72
        _FlowPhaseBlendSharpness("Dual-Phase Flow Blend Sharpness", Range(0.1, 4)) = 1
        _FakeRefractionEnabled("Fake Refraction Enabled", Range(0, 1)) = 0
        _RefractionStrengthPixels("Fake Refraction Strength Pixels", Range(0, 8)) = 2.4
        _RefractionNoiseScale("Fake Refraction Noise Scale", Range(1, 64)) = 18
        _RefractionScrollSpeed("Fake Refraction Scroll Speed", Range(0, 3)) = 0.82
        _RefractionTimeOffset("Fake Refraction Review Time Offset", Range(0, 8)) = 0
        _RefractionDepthFade("Fake Refraction Depth Fade", Range(0.01, 1)) = 0.28
        _RefractionSceneBlend("Fake Refraction Scene Blend", Range(0, 1)) = 0.34
        _RefractionEdgeGuard("Fake Refraction Shore Edge Guard", Range(0, 1)) = 0.78
        _WaterSpecularStrength("Stylized Water Specular Strength", Range(0, 0.7)) = 0.24
        _WaterSpecularPower("Stylized Water Specular Power", Range(4, 96)) = 34
        _WaterSpecularCutoff("Stylized Water Specular Cutoff", Range(0, 1)) = 0.58
        _WaterSpecularNoiseScale("Stylized Water Glint Noise Scale", Range(1, 64)) = 18
        _WaterSpecularScrollSpeed("Stylized Water Glint Scroll Speed", Range(0, 3)) = 0.45
        _WaterSpecularColor("Stylized Water Specular Color", Color) = (1.0, 0.92, 0.72, 1)
        _ToonWaterSpecularEnabled("Toon-Stepped Water Specular Enabled", Range(0, 1)) = 0
        _ToonWaterSpecularIntensity("Toon-Stepped Water Specular Intensity", Range(0, 1)) = 0.52
        _ToonWaterSpecularSteps("Toon-Stepped Water Specular Bands", Range(1, 4)) = 2
        _ToonWaterSpecularCutoff("Toon-Stepped Water Specular Cutoff", Range(0, 1)) = 0.42
        _VertexRippleEnabled("Gentle Vertex Ripple Enabled", Range(0, 1)) = 0
        _VertexRippleAmplitude("Gentle Vertex Ripple Amplitude", Range(0, 0.12)) = 0.032
        _VertexRippleFrequency("Gentle Vertex Ripple Frequency", Range(0.1, 8)) = 3.2
        _VertexRippleSpeed("Gentle Vertex Ripple Speed", Range(0, 4)) = 0.72
        _VertexRippleTimeOffset("Gentle Vertex Ripple Review Time Offset", Range(0, 8)) = 0
        _VertexRippleNormalStrength("Gentle Vertex Ripple Normal Strength", Range(0, 1)) = 0.55
        _ReflectionEnabled("Tilted Camera Reflection Enabled", Range(0, 1)) = 0
        _ReflectionStrength("Tilted Camera Reflection Strength", Range(0, 1)) = 0.48
        _ReflectionFresnelPower("Tilted Camera Reflection Fresnel Power", Range(0.25, 6)) = 2.1
        _ReflectionFresnelBias("Tilted Camera Reflection Fresnel Bias", Range(0, 0.5)) = 0.10
        _ReflectionRoughness("Tilted Camera Reflection Roughness", Range(0, 1)) = 0.24
        _ReflectionTint("Tilted Camera Reflection Tint", Color) = (0.74, 0.90, 1.0, 1)
        _ReflectionSkyFallback("Tilted Camera Reflection Sky Fallback", Range(0, 0.6)) = 0.18
        _Alpha("Surface Alpha", Range(0, 1)) = 0.96
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
            ZTest LEqual
            Cull Back

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_FlowMap);
            SAMPLER(sampler_FlowMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _MainTex_ST;
            float4 _FlowMap_ST;
            float4 _FlatColor;
            float4 _ShallowColor;
            float4 _DeepColor;
            float _DepthGradientStrength;
            float _DeepDistance;
            float _ObjectDeepDistance;
            float _SceneDepthWeight;
            float _ObjectDepthWeight;
            float _PixelRippleStrength;
            float _FoamStrength;
            float _FoamDistance;
            float4 _FoamColor;
            float _FoamCutoff;
            float _FoamNoiseScale;
            float _FoamNoiseStrength;
            float _FoamScrollSpeed;
            float _FoamInnerOffset;
            float _FoamInnerWidth;
            float _FoamTimeOffset;
            float _DirectionalFlowEnabled;
            float _FlowStrength;
            float _FlowSpeed;
            float _FlowTimeOffset;
            float _FlowFoamAdvectionStrength;
            float _FlowSpecularAdvectionStrength;
            float _FlowPhaseBlendSharpness;
            float _FakeRefractionEnabled;
            float _RefractionStrengthPixels;
            float _RefractionNoiseScale;
            float _RefractionScrollSpeed;
            float _RefractionTimeOffset;
            float _RefractionDepthFade;
            float _RefractionSceneBlend;
            float _RefractionEdgeGuard;
            float _WaterSpecularStrength;
            float _WaterSpecularPower;
            float _WaterSpecularCutoff;
            float _WaterSpecularNoiseScale;
            float _WaterSpecularScrollSpeed;
            float4 _WaterSpecularColor;
            float _ToonWaterSpecularEnabled;
            float _ToonWaterSpecularIntensity;
            float _ToonWaterSpecularSteps;
            float _ToonWaterSpecularCutoff;
            float _VertexRippleEnabled;
            float _VertexRippleAmplitude;
            float _VertexRippleFrequency;
            float _VertexRippleSpeed;
            float _VertexRippleTimeOffset;
            float _VertexRippleNormalStrength;
            float _ReflectionEnabled;
            float _ReflectionStrength;
            float _ReflectionFresnelPower;
            float _ReflectionFresnelBias;
            float _ReflectionRoughness;
            float4 _ReflectionTint;
            float _ReflectionSkyFallback;
            float _Alpha;
            CBUFFER_END
            float4 _AnemoraHd2dSunKeyColor;
            float _AnemoraHd2dSunKeyIntensity;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionOS : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float surfaceEyeDepth : TEXCOORD4;
                float3 positionWS : TEXCOORD5;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                float vertexRippleMask = saturate(_VertexRippleEnabled) * saturate(abs(normalWS.y));
                float vertexRippleTime = (_Time.y * _VertexRippleSpeed) + _VertexRippleTimeOffset;
                float2 rippleDirA = normalize(float2(0.86f, 0.51f));
                float2 rippleDirB = normalize(float2(-0.38f, 0.93f));
                float ripplePhaseA = dot(positionWS.xz, rippleDirA) * _VertexRippleFrequency + vertexRippleTime;
                float ripplePhaseB = dot(positionWS.xz, rippleDirB) * (_VertexRippleFrequency * 0.63f) - (vertexRippleTime * 1.27f);
                float rippleWaveA = sin(ripplePhaseA);
                float rippleWaveB = sin(ripplePhaseB);
                float rippleAmplitude = _VertexRippleAmplitude * vertexRippleMask;
                positionWS.y += ((rippleWaveA * 0.62f) + (rippleWaveB * 0.38f)) * rippleAmplitude;
                float rippleGradX =
                    (cos(ripplePhaseA) * rippleDirA.x * _VertexRippleFrequency * 0.62f) +
                    (cos(ripplePhaseB) * rippleDirB.x * _VertexRippleFrequency * 0.63f * 0.38f);
                float rippleGradZ =
                    (cos(ripplePhaseA) * rippleDirA.y * _VertexRippleFrequency * 0.62f) +
                    (cos(ripplePhaseB) * rippleDirB.y * _VertexRippleFrequency * 0.63f * 0.38f);
                float3 rippleNormalWS = SafeNormalize(float3(-rippleGradX * _VertexRippleAmplitude, 1.0f, -rippleGradZ * _VertexRippleAmplitude));
                normalWS = SafeNormalize(lerp(normalWS, rippleNormalWS, vertexRippleMask * saturate(_VertexRippleNormalStrength)));
                output.positionHCS = TransformWorldToHClip(positionWS);
                output.uv = input.uv;
                output.positionOS = input.positionOS.xyz;
                output.normalWS = normalWS;
                output.screenPos = ComputeScreenPos(output.positionHCS);
                output.surfaceEyeDepth = -TransformWorldToView(positionWS).z;
                output.positionWS = positionWS;
                return output;
            }

            float2 Hash22(float2 p)
            {
                p = float2(dot(p, float2(127.1f, 311.7f)), dot(p, float2(269.5f, 183.3f)));
                return frac(sin(p) * 43758.5453f);
            }

            float VoronoiFoamNoise(float2 uv, float timeValue)
            {
                float2 cell = floor(uv);
                float2 local = frac(uv);
                float nearest = 8.0f;

                [unroll]
                for (int y = -1; y <= 1; y++)
                {
                    [unroll]
                    for (int x = -1; x <= 1; x++)
                    {
                        float2 neighbor = float2((float)x, (float)y);
                        float2 randomPoint = Hash22(cell + neighbor);
                        randomPoint = 0.5f + 0.5f * sin(timeValue + 6.28318f * randomPoint);
                        float2 delta = neighbor + randomPoint - local;
                        nearest = min(nearest, dot(delta, delta));
                    }
                }

                return saturate(1.0f - sqrt(nearest));
            }

            float DirectionalWaterFlowPhaseBlend(float phase, float sharpness)
            {
                float phaseTriangle = abs(frac(phase) * 2.0f - 1.0f);
                return saturate(pow(phaseTriangle, max(sharpness, 0.001f)));
            }

            float2 DecodeDirectionalWaterFlow(float4 flowSample)
            {
                float2 rawFlow = (flowSample.rg * 2.0f) - 1.0f;
                float magnitude = length(rawFlow);
                float2 direction = magnitude > 0.001f ? rawFlow / magnitude : float2(0.0f, 1.0f);
                return direction * saturate(magnitude) * saturate(flowSample.b);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 screenUv = input.screenPos.xy / max(input.screenPos.w, 0.0001f);
                float rawSceneDepth = SampleSceneDepth(screenUv);
                float sceneDepth01 = 0.0f;

                #if UNITY_REVERSED_Z
                bool validSceneDepth = rawSceneDepth > 0.0001f;
                #else
                bool validSceneDepth = rawSceneDepth < 0.9999f;
                #endif

                if (validSceneDepth)
                {
                    float sceneEye = LinearEyeDepth(rawSceneDepth, _ZBufferParams);
                    float waterEye = input.surfaceEyeDepth;
                    sceneDepth01 = saturate(max(sceneEye - waterEye, 0.0f) / max(_DeepDistance, 0.001f));
                }

                float edgeDistance = 0.5f - max(abs(input.positionOS.x), abs(input.positionOS.z));
                float objectDepth01 = saturate(edgeDistance / max(_ObjectDeepDistance, 0.001f));
                float depth01 = saturate(max(sceneDepth01 * _SceneDepthWeight, objectDepth01 * _ObjectDepthWeight));
                depth01 = smoothstep(0.0f, 1.0f, depth01);

                float foamDepth01 = min(validSceneDepth ? sceneDepth01 : 1.0f, objectDepth01);
                float foamDepthMask = saturate(1.0f - (foamDepth01 / max(_FoamDistance, 0.001f)));
                float foamTime = (_Time.y * _FoamScrollSpeed) + _FoamTimeOffset;
                float flowTime = (_Time.y * _FlowSpeed) + _FlowTimeOffset;
                float phaseA = frac(flowTime);
                float phaseB = frac(flowTime + 0.5f);
                float phaseBlend = DirectionalWaterFlowPhaseBlend(phaseA, _FlowPhaseBlendSharpness);
                float flowEnabled = saturate(_DirectionalFlowEnabled);
                float2 flowMapUv = (input.uv * _FlowMap_ST.xy) + _FlowMap_ST.zw;
                float2 flowVector = DecodeDirectionalWaterFlow(SAMPLE_TEXTURE2D(_FlowMap, sampler_FlowMap, flowMapUv)) * saturate(_FlowStrength) * flowEnabled;

                float2 foamUvBase = (input.uv * _FoamNoiseScale) + float2(foamTime * 0.37f, -foamTime * 0.23f);
                float2 foamUvA = foamUvBase + (flowVector * phaseA * _FlowFoamAdvectionStrength);
                float2 foamUvB = foamUvBase + (flowVector * phaseB * _FlowFoamAdvectionStrength);
                float outerDefault = VoronoiFoamNoise(foamUvBase, foamTime);
                float outerFlow = lerp(VoronoiFoamNoise(foamUvA, foamTime), VoronoiFoamNoise(foamUvB, foamTime + 4.13f), phaseBlend);
                float outerVoronoi = lerp(outerDefault, outerFlow, flowEnabled);
                float2 innerUvBase = foamUvBase + float2(3.17f, 5.29f);
                float2 innerUvA = innerUvBase + (flowVector * phaseA * _FlowFoamAdvectionStrength * 1.17f);
                float2 innerUvB = innerUvBase + (flowVector * phaseB * _FlowFoamAdvectionStrength * 1.17f);
                float innerDefault = VoronoiFoamNoise(innerUvBase, foamTime * 1.31f);
                float innerFlow = lerp(VoronoiFoamNoise(innerUvA, foamTime * 1.31f), VoronoiFoamNoise(innerUvB, (foamTime * 1.31f) + 2.71f), phaseBlend);
                float innerVoronoi = lerp(innerDefault, innerFlow, flowEnabled);
                float brokenFoam = foamDepthMask + ((outerVoronoi - 0.5f) * _FoamNoiseStrength);
                float outerFoam = step(_FoamCutoff, brokenFoam);
                float innerLine = saturate(1.0f - (abs(foamDepth01 - _FoamInnerOffset) / max(_FoamInnerWidth, 0.001f)));
                float brokenInner = innerLine + ((innerVoronoi - 0.5f) * _FoamNoiseStrength);
                float innerFoam = step(_FoamCutoff, brokenInner) * 0.65f;
                float foamMask = saturate((outerFoam + innerFoam) * _FoamStrength);

                float2 rippleUv = input.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;
                half3 rippleSample = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, rippleUv).rgb;
                half rippleLuma = dot(rippleSample, half3(0.2126h, 0.7152h, 0.0722h));
                half rippleGrade = lerp(1.0h, lerp(0.86h, 1.14h, rippleLuma), (half)_PixelRippleStrength);

                half3 gradient = lerp((half3)_ShallowColor.rgb, (half3)_DeepColor.rgb, (half)depth01);
                gradient *= rippleGrade;
                half3 color = lerp((half3)_FlatColor.rgb, gradient, saturate((half)_DepthGradientStrength));
                color = lerp(color, (half3)_FoamColor.rgb, (half)(foamMask * saturate(_FoamColor.a)));
                Light mainLight = GetMainLight();
                half3 mainLightColor = half3(mainLight.color.r, mainLight.color.g, mainLight.color.b);
                mainLightColor = lerp(mainLightColor, half3(_AnemoraHd2dSunKeyColor.rgb), step(0.001h, (half)_AnemoraHd2dSunKeyIntensity));
                float3 normalWS = SafeNormalize(input.normalWS);
                float3 viewDirWS = SafeNormalize(GetCameraPositionWS() - input.positionWS);
                float3 halfDirWS = SafeNormalize((float3)mainLight.direction + viewDirWS);
                half blinnSpec = pow(saturate(dot(normalWS, halfDirWS)), max((half)_WaterSpecularPower, 0.001h));
                float glintTime = (_Time.y + _FoamTimeOffset) * _WaterSpecularScrollSpeed;
                float2 glintUvBase = (input.uv * _WaterSpecularNoiseScale) + float2(glintTime * 0.41f, -glintTime * 0.29f);
                float2 glintUvA = glintUvBase + (flowVector * phaseA * _FlowSpecularAdvectionStrength);
                float2 glintUvB = glintUvBase + (flowVector * phaseB * _FlowSpecularAdvectionStrength);
                float2 glintCell = floor(lerp(glintUvBase, lerp(glintUvA, glintUvB, phaseBlend), flowEnabled));
                half glintNoise = (half)frac(sin(dot(glintCell, float2(19.19f, 73.73f))) * 43758.5453f);
                half ndotl = saturate(dot(normalWS, mainLight.direction));
                half glintSeed = saturate(max(blinnSpec * 1.35h, ndotl * 0.34h) + (glintNoise * 0.38h) + (rippleLuma * 0.26h));
                half glint = smoothstep((half)_WaterSpecularCutoff, 1.0h, glintSeed);
                half3 specTint = lerp((half3)_WaterSpecularColor.rgb, saturate(mainLightColor + half3(0.10h, 0.04h, -0.02h)), 0.62h);
                color += specTint * glint * saturate((half)_WaterSpecularStrength) * smoothstep(0.04h, 0.62h, ndotl) * lerp(1.0h, 0.58h, (half)foamMask);
                half toonGlintSeed = saturate((glintSeed - (half)_ToonWaterSpecularCutoff) / max(1.0h - (half)_ToonWaterSpecularCutoff, 0.001h));
                half toonSteps = max((half)_ToonWaterSpecularSteps, 1.0h);
                half toonBand = floor(toonGlintSeed * toonSteps) / toonSteps;
                toonBand = saturate(toonBand + step(0.995h, toonGlintSeed) / toonSteps);
                color += specTint * toonBand * saturate((half)_ToonWaterSpecularIntensity) * saturate((half)_ToonWaterSpecularEnabled) * smoothstep(0.03h, 0.56h, ndotl) * lerp(1.0h, 0.65h, (half)foamMask);
                float refractionEnabled = saturate(_FakeRefractionEnabled);
                float refractionTime = ((_Time.y * _RefractionScrollSpeed) + _RefractionTimeOffset);
                float2 refractionNoiseUv = (input.uv * _RefractionNoiseScale) + float2(refractionTime * 0.31f, -refractionTime * 0.23f);
                float refractionNoiseX = VoronoiFoamNoise(refractionNoiseUv, refractionTime);
                float refractionNoiseY = VoronoiFoamNoise(refractionNoiseUv + float2(4.71f, 2.93f), refractionTime * 1.17f);
                float2 refractionVector = (float2(refractionNoiseX, refractionNoiseY) * 2.0f) - 1.0f;
                float refractionDepthMask = saturate(depth01 / max(_RefractionDepthFade, 0.001f));
                float refractionEdgeGuard = lerp(1.0f, saturate(1.0f - foamDepthMask), saturate(_RefractionEdgeGuard));
                float2 refractionPixelSize = 1.0f / max(_ScreenParams.xy, float2(1.0f, 1.0f));
                float2 refractionOffset = refractionVector * _RefractionStrengthPixels * refractionPixelSize * refractionDepthMask * refractionEdgeGuard * refractionEnabled;
                float2 refractionUv = clamp(screenUv + refractionOffset, refractionPixelSize * 2.0f, 1.0f - (refractionPixelSize * 2.0f));
                half3 refractedScene = SampleSceneColor(refractionUv).rgb;
                half3 straightScene = SampleSceneColor(screenUv).rgb;
                half3 refractionScene = lerp(straightScene, refractedScene, (half)saturate(_RefractionSceneBlend));
                half3 refractionComposite = lerp(color, (color * 0.76h) + (refractionScene * 0.24h), (half)saturate(_RefractionSceneBlend));
                color = lerp(color, refractionComposite, (half)refractionEnabled);
                half reflectionEnabled = saturate((half)_ReflectionEnabled);
                half noV = saturate(dot(normalWS, viewDirWS));
                half reflectionFresnel = saturate((half)_ReflectionFresnelBias + pow(saturate(1.0h - noV), max((half)_ReflectionFresnelPower, 0.001h)));
                half reflectionMask = saturate((half)_ReflectionStrength) * reflectionFresnel * reflectionEnabled;
                half3 reflectionDirWS = SafeNormalize(reflect(-viewDirWS, normalWS));
                half3 environmentReflection = GlossyEnvironmentReflection(reflectionDirWS, input.positionWS, saturate((half)_ReflectionRoughness), 1.0h, screenUv);
                half environmentLuma = dot(environmentReflection, half3(0.2126h, 0.7152h, 0.0722h));
                half3 skyFallbackReflection = (half3)_ReflectionTint.rgb * saturate((half)_ReflectionSkyFallback);
                half3 tiltedCameraReflection = max(environmentReflection * (half3)_ReflectionTint.rgb, skyFallbackReflection * step(environmentLuma, 0.015h));
                color = lerp(color, tiltedCameraReflection, reflectionMask);
                half alpha = lerp(1.0h, (half)_Alpha, saturate((half)_DepthGradientStrength));
                return half4(color, alpha);
            }
            ENDHLSL
        }
    }
}
