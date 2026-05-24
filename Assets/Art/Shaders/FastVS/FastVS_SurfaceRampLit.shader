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
        _DirectionalLightStrength("Directional Light Strength", Range(0, 0.60)) = 0.12
        _ShadowReceiveStrength("Shadow Receive Strength", Range(0, 0.70)) = 0.18
        _ShadowTextureStrength("Shadow Texture Strength", Range(0, 0.5)) = 0
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

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseMap_ST;

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

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
                Light mainLight = GetMainLight(shadowCoord);
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
                half3 sunTint = lerp(half3(0.92h, 0.84h, 0.72h), saturate(mainLightColor + half3(0.10h, 0.03h, -0.08h)), litResponse * saturate((half)_DirectionalLightStrength * 1.25h));
                half3 texturedShadowTint = half3(0.70h, 0.62h, 0.50h) * lerp(0.96h, 1.12h, shadowNoise * shadowTextureStrength + (1.0h - shadowTextureStrength) * 0.5h);
                half3 shadowTint = lerp(texturedShadowTint, half3(1.0h, 1.0h, 1.0h), shadowResponse);

                half3 rgb = baseSample.rgb * grade * lightGrade * shadowGrade * lerp(shadowTint, sunTint, litResponse);
                return half4(rgb, baseSample.a);
            }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}
