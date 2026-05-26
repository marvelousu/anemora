Shader "Anemora/FastVS/SurfaceRampLit"
{
    Properties
    {
        [MainTexture]_BaseMap("Base Map", 2D) = "white" {}
        [MainColor]_BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _MainTex("Main Tex", 2D) = "white" {}
        _SurfaceRampStrength("Surface Ramp Strength", Range(0, 0.5)) = 0.2
        _TopLight("Top Light", Color) = (1.05, 1.03, 0.97, 1)
        _SideShade("Side Shade", Color) = (0.95, 0.98, 1.03, 1)
        _FloorShade("Floor Shade", Color) = (0.92, 0.94, 0.97, 1)
        _Metallic("Metallic", Range(0, 1)) = 0
        _Smoothness("Smoothness", Range(0, 1)) = 0.16
        _SpecularHighlights("Specular Highlights", Float) = 0
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
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _LIGHT_COOKIES

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseMap_ST;

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            TEXTURE2D(_EmissionMap);
            SAMPLER(sampler_EmissionMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
            float _SurfaceRampStrength;
            float4 _TopLight;
            float4 _SideShade;
            float4 _FloorShade;
            float _Metallic;
            float _Smoothness;
            float _SpecularHighlights;
            float _DirectionalLightStrength;
            float _ShadowReceiveStrength;
            float _ShadowTextureStrength;
            float4 _EmissionColor;
            float _EmissionIntensity;
            CBUFFER_END

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
                half3 normalWS : TEXCOORD1;
                float3 positionWS : TEXCOORD2;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionHCS = TransformWorldToHClip(output.positionWS);
                output.uv = input.uv;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;
                half4 baseSample = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv) * _BaseColor;
                half3 normalWS = SafeNormalize(input.normalWS);
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
                #if defined(_MAIN_LIGHT_SHADOWS) || defined(_MAIN_LIGHT_SHADOWS_CASCADE)
                    float3 lightDirWS = normalize((float3)mainLight.direction);
                    float3 shadowUp = abs(lightDirWS.y) > 0.88f ? float3(1.0f, 0.0f, 0.0f) : float3(0.0f, 1.0f, 0.0f);
                    float3 shadowTangent = normalize(cross(shadowUp, lightDirWS));
                    float3 shadowBitangent = normalize(cross(lightDirWS, shadowTangent));
                    float shadowSoftRadius = lerp(0.025f, 0.18f, (float)shadowTextureStrength);
                    half softShadowAttenuation = shadowAttenuation;
                    softShadowAttenuation += (half)MainLightRealtimeShadow(TransformWorldToShadowCoord(input.positionWS + shadowTangent * shadowSoftRadius));
                    softShadowAttenuation += (half)MainLightRealtimeShadow(TransformWorldToShadowCoord(input.positionWS - shadowTangent * shadowSoftRadius));
                    softShadowAttenuation += (half)MainLightRealtimeShadow(TransformWorldToShadowCoord(input.positionWS + shadowBitangent * shadowSoftRadius * 0.72f));
                    softShadowAttenuation += (half)MainLightRealtimeShadow(TransformWorldToShadowCoord(input.positionWS - shadowBitangent * shadowSoftRadius * 0.72f));
                    softShadowAttenuation += (half)MainLightRealtimeShadow(TransformWorldToShadowCoord(input.positionWS + (shadowTangent + shadowBitangent * 0.55f) * shadowSoftRadius));
                    softShadowAttenuation += (half)MainLightRealtimeShadow(TransformWorldToShadowCoord(input.positionWS - (shadowTangent + shadowBitangent * 0.55f) * shadowSoftRadius));
                    softShadowAttenuation += (half)MainLightRealtimeShadow(TransformWorldToShadowCoord(input.positionWS + (shadowTangent - shadowBitangent * 0.55f) * shadowSoftRadius));
                    softShadowAttenuation += (half)MainLightRealtimeShadow(TransformWorldToShadowCoord(input.positionWS - (shadowTangent - shadowBitangent * 0.55f) * shadowSoftRadius));
                    softShadowAttenuation *= 0.111111h;
                    shadowAttenuation = lerp(shadowAttenuation, softShadowAttenuation, saturate(shadowTextureStrength * 1.35h));
                #endif
                half shadowResponse = smoothstep(0.10h, 0.95h, saturate(shadowAttenuation + ((shadowNoise - 0.5h) * shadowTextureStrength * 0.24h)));
                half shadowFloorLift = shadowTextureStrength * (0.52h + shadowNoise * 0.16h) * (1.0h - shadowResponse);
                half shadowGrade = lerp(saturate(1.0h - (half)_ShadowReceiveStrength + shadowFloorLift), 1.0h, shadowResponse);
                half3 mainLightColor = half3(mainLight.color.r, mainLight.color.g, mainLight.color.b);
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
                half3 emissionColor = half3(_EmissionColor.r, _EmissionColor.g, _EmissionColor.b);
                half3 emission = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, uv).rgb * emissionColor * (half)_EmissionIntensity;
                rgb += emission;
                return half4(rgb, baseSample.a);
            }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}
