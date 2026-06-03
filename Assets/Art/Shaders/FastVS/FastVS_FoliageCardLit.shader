Shader "Anemora/FastVS/FoliageCardLit"
{
    Properties
    {
        [MainTexture]_BaseMap("Base Map", 2D) = "white" {}
        [MainColor]_BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _MainTex("Main Tex", 2D) = "white" {}
        [NoScaleOffset]_EmissionMap("Emission Map", 2D) = "black" {}
        [HDR]_EmissionColor("Emission Color", Color) = (0, 0, 0, 0)
        _EmissionIntensity("Emission Intensity", Range(0, 20)) = 0
        _Cutoff("Alpha Cutoff", Range(0, 1)) = 0.15
        [HideInInspector]_Surface("__surface", Float) = 0
        [HideInInspector]_AlphaClip("__alphaClip", Float) = 1
        [HideInInspector]_ZWrite("__zw", Float) = 1
        [HideInInspector]_Cull("__cull", Float) = 0
        [HideInInspector]_SrcBlend("__src", Float) = 1
        [HideInInspector]_DstBlend("__dst", Float) = 0
        [HideInInspector]_QueueControl("__queueControl", Float) = 1
        [HideInInspector]_QueueOffset("__queueOffset", Float) = 0
        _RampStrength("Ramp Strength", Range(0, 0.5)) = 0.18
        _TopLight("Top Light", Color) = (1.08, 1.03, 0.96, 1)
        _SideShade("Side Shade", Color) = (0.94, 0.97, 1.03, 1)
        _FloorShade("Floor Shade", Color) = (0.89, 0.92, 0.96, 1)
        _PaperEdgeStrength("Paper Edge Strength", Range(0, 0.35)) = 0.10
        _PaperRimStrength("Paper Rim Strength", Range(0, 0.25)) = 0.07
        _PaperLowerShadeStrength("Paper Lower Shade Strength", Range(0, 0.25)) = 0.08
        _WorldLightStrength("World Light Strength", Range(0, 0.25)) = 0.08
        _WorldShadowReceiveStrength("World Shadow Receive Strength", Range(0, 0.20)) = 0.05
        _WindStrength("Wind Strength", Range(0, 0.3)) = 0.06
        _WindSpeed("Wind Speed", Range(0, 4)) = 1.4
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

            ZWrite [_ZWrite]
            ZTest LEqual
            Cull [_Cull]

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _LIGHT_COOKIES

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Assets/Art/Shaders/FastVS/FastVS_SharedVegetationWindControl.hlsl"

            #pragma multi_compile_instancing

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseMap_TexelSize;

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_EmissionMap);
            SAMPLER(sampler_EmissionMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _MainTex_ST;
            float4 _BaseColor;
            float4 _EmissionColor;
            float _EmissionIntensity;
            float _Cutoff;
            float _Surface;
            float _AlphaClip;
            float _ZWrite;
            float _Cull;
            float _SrcBlend;
            float _DstBlend;
            float _QueueControl;
            float _QueueOffset;
            float _RampStrength;
            float4 _TopLight;
            float4 _SideShade;
            float4 _FloorShade;
            float _PaperEdgeStrength;
            float _PaperRimStrength;
            float _PaperLowerShadeStrength;
            float _WorldLightStrength;
            float _WorldShadowReceiveStrength;
            float _WindStrength;
            float _WindSpeed;
            CBUFFER_END
            float4 _AnemoraHd2dSunKeyColor;
            float _AnemoraHd2dSunKeyIntensity;
            float _AnemoraHd2dReadabilityFloor;
            float4 _AnemoraHd2dReadabilityFloorTint;
            float _AnemoraHd2dReadabilityFloorStrength;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
            };

            float3 ApplyFoliageWind(float3 positionWS, float2 uv)
            {
                return FastVsApplySharedWindControl(positionWS, uv, _WindStrength, 1.0, 0.0);
            }

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionWS = ApplyFoliageWind(output.positionWS, input.uv);
                output.positionHCS = TransformWorldToHClip(output.positionWS);
                output.uv = input.uv;
                return output;
            }

            float SampleBaseMapAlphaWithinFrame(float2 sampleUv, float2 frameMin, float2 frameMax, float2 frameTexel)
            {
                float2 halfTexel = frameTexel * 0.5;
                float2 clampedUv = clamp(sampleUv, frameMin + halfTexel, frameMax - halfTexel);
                return SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, clampedUv).a;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;
                half4 baseSample = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv) * _BaseColor;
                clip(baseSample.a - _Cutoff);

                float2 frameMin = _BaseMap_ST.zw;
                float2 frameMax = frameMin + _BaseMap_ST.xy;
                float2 frameScale = max(_BaseMap_ST.xy, 1e-5);
                float2 frameTexel = min(_BaseMap_TexelSize.xy * 1.25, frameScale * 0.49);

                float2 frameUv = float2(
                    saturate((uv.x - _BaseMap_ST.z) / max(_BaseMap_ST.x, 1e-5)),
                    saturate((uv.y - _BaseMap_ST.w) / max(_BaseMap_ST.y, 1e-5)));

                float alphaLeft = SampleBaseMapAlphaWithinFrame(uv - float2(frameTexel.x, 0.0), frameMin, frameMax, frameTexel);
                float alphaRight = SampleBaseMapAlphaWithinFrame(uv + float2(frameTexel.x, 0.0), frameMin, frameMax, frameTexel);
                float alphaTop = SampleBaseMapAlphaWithinFrame(uv + float2(0.0, frameTexel.y), frameMin, frameMax, frameTexel);
                float alphaBottom = SampleBaseMapAlphaWithinFrame(uv - float2(0.0, frameTexel.y), frameMin, frameMax, frameTexel);
                float alphaTopLeft = SampleBaseMapAlphaWithinFrame(uv + float2(-frameTexel.x, frameTexel.y), frameMin, frameMax, frameTexel);
                float alphaBottomRight = SampleBaseMapAlphaWithinFrame(uv + float2(frameTexel.x, -frameTexel.y), frameMin, frameMax, frameTexel);

                float edgeLeft = saturate((baseSample.a - alphaLeft) * 10.0);
                float edgeRight = saturate((baseSample.a - alphaRight) * 10.0);
                float edgeTop = saturate((baseSample.a - alphaTop) * 10.0);
                float edgeBottom = saturate((baseSample.a - alphaBottom) * 10.0);
                float edgeTopLeft = saturate((baseSample.a - alphaTopLeft) * 8.0);
                float edgeBottomRight = saturate((baseSample.a - alphaBottomRight) * 8.0);
                float cutoutEdge = saturate(max(max(edgeLeft, edgeRight), max(edgeTop, edgeBottom)));
                cutoutEdge = saturate(max(cutoutEdge, max(edgeTopLeft, edgeBottomRight)));
                float edgeAccent = saturate(cutoutEdge * 1.5);

                float warmKey = saturate((1.0 - frameUv.x) * frameUv.y);
                float coolSide = saturate(frameUv.x * (1.0 - frameUv.y * 0.18));
                float floorShade = saturate((1.0 - frameUv.y) * (1.0 - frameUv.y));
                float strength = saturate(_RampStrength);
                float paperEdgeStrength = saturate(_PaperEdgeStrength);
                float paperRimStrength = saturate(_PaperRimStrength);
                float paperLowerShadeStrength = saturate(_PaperLowerShadeStrength);

                half3 neutral = half3(1.0h, 1.0h, 1.0h);
                half3 grade = neutral;
                grade *= lerp(neutral, (half3)_TopLight.rgb, (half)(warmKey * strength));
                grade *= lerp(neutral, (half3)_SideShade.rgb, (half)(coolSide * strength));
                grade *= lerp(neutral, (half3)_FloorShade.rgb, (half)(floorShade * strength));
                grade *= lerp(neutral, half3(1.035h, 1.015h, 0.990h), (half)(edgeAccent * warmKey * paperRimStrength));
                grade *= lerp(neutral, half3(0.970h, 0.980h, 1.020h), (half)(edgeAccent * coolSide * paperEdgeStrength));
                grade *= lerp(neutral, half3(0.965h, 0.975h, 1.015h), (half)(edgeAccent * saturate((1.0 - frameUv.y) * (0.55 + frameUv.x * 0.45)) * paperLowerShadeStrength));

                float4 shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                Light mainLight = GetMainLight(shadowCoord, input.positionWS, half4(1.0h, 1.0h, 1.0h, 1.0h));
                half worldLightStrength = saturate((half)_WorldLightStrength);
                half worldShadowReceiveStrength = saturate((half)_WorldShadowReceiveStrength);
                half3 mainLightColor = half3(mainLight.color.r, mainLight.color.g, mainLight.color.b);
                mainLightColor = lerp(mainLightColor, half3(_AnemoraHd2dSunKeyColor.rgb), step(0.001h, (half)_AnemoraHd2dSunKeyIntensity));
                half lightCookieLuma = dot(mainLightColor, half3(0.2126h, 0.7152h, 0.0722h));
                half lightCookieResponse = smoothstep(0.36h, 0.88h, lightCookieLuma);
                half3 mainTint = lerp(neutral, saturate(mainLightColor + half3(0.06h, 0.03h, -0.04h)), worldLightStrength);
                half shadowGrade = lerp(1.0h - worldShadowReceiveStrength, 1.0h, (half)mainLight.shadowAttenuation);
                shadowGrade *= lerp(0.92h, 1.06h, lightCookieResponse * worldLightStrength);

                half3 rgb = baseSample.rgb * grade * mainTint * shadowGrade;
                rgb = FastVsApplySharedVegetationTint(rgb, 1.0);
                half3 emissionSample = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, uv).rgb;
                half3 emissionColor = half3(_EmissionColor.r, _EmissionColor.g, _EmissionColor.b);
                rgb += emissionSample * emissionColor * (half)_EmissionIntensity;
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
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite [_ZWrite]
            ZTest LEqual
            ColorMask 0
            Cull [_Cull]

            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Assets/Art/Shaders/FastVS/FastVS_SharedVegetationWindControl.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _MainTex_ST;
            float4 _BaseColor;
            float4 _EmissionColor;
            float _EmissionIntensity;
            float _Cutoff;
            float _Surface;
            float _AlphaClip;
            float _ZWrite;
            float _Cull;
            float _SrcBlend;
            float _DstBlend;
            float _QueueControl;
            float _QueueOffset;
            float _RampStrength;
            float4 _TopLight;
            float4 _SideShade;
            float4 _FloorShade;
            float _PaperEdgeStrength;
            float _PaperRimStrength;
            float _PaperLowerShadeStrength;
            float _WorldLightStrength;
            float _WorldShadowReceiveStrength;
            float _WindStrength;
            float _WindSpeed;
            CBUFFER_END
            float3 _LightDirection;
            float3 _LightPosition;

            struct ShadowAttributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct ShadowVaryings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float3 ApplyFoliageWind(float3 positionWS, float2 uv)
            {
                return FastVsApplySharedWindControl(positionWS, uv, _WindStrength, 1.0, 0.0);
            }

            ShadowVaryings ShadowPassVertex(ShadowAttributes input)
            {
                ShadowVaryings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                positionWS = ApplyFoliageWind(positionWS, input.uv);
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
                output.uv = input.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;
                return output;
            }

            half4 ShadowPassFragment(ShadowVaryings input) : SV_TARGET
            {
                half alpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv).a * (half)_BaseColor.a;
                clip(alpha - _Cutoff);
                return 0;
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
            Cull [_Cull]

            HLSLPROGRAM
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/Art/Shaders/FastVS/FastVS_SharedVegetationWindControl.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _MainTex_ST;
            float4 _BaseColor;
            float4 _EmissionColor;
            float _EmissionIntensity;
            float _Cutoff;
            float _Surface;
            float _AlphaClip;
            float _ZWrite;
            float _Cull;
            float _SrcBlend;
            float _DstBlend;
            float _QueueControl;
            float _QueueOffset;
            float _RampStrength;
            float4 _TopLight;
            float4 _SideShade;
            float4 _FloorShade;
            float _PaperEdgeStrength;
            float _PaperRimStrength;
            float _PaperLowerShadeStrength;
            float _WorldLightStrength;
            float _WorldShadowReceiveStrength;
            float _WindStrength;
            float _WindSpeed;
            CBUFFER_END

            struct DepthOnlyAttributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct DepthOnlyVaryings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            DepthOnlyVaryings DepthOnlyVertex(DepthOnlyAttributes input)
            {
                DepthOnlyVaryings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                positionWS = FastVsApplySharedWindControl(positionWS, input.uv, _WindStrength, 1.0, 0.0);
                output.positionCS = TransformWorldToHClip(positionWS);
                output.uv = input.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;
                return output;
            }

            half4 DepthOnlyFragment(DepthOnlyVaryings input) : SV_TARGET
            {
                half alpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv).a * (half)_BaseColor.a;
                clip(alpha - _Cutoff);
                return 0;
            }
            ENDHLSL
        }
    }
}
