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
        _DirectionalLightStrength("Directional Light Strength", Range(0, 0.35)) = 0.12
        _ShadowReceiveStrength("Shadow Receive Strength", Range(0, 0.45)) = 0.18
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
                half lightGrade = lerp(1.0h - (half)_DirectionalLightStrength, 1.0h + ((half)_DirectionalLightStrength * 0.35h), ndotl);
                half shadowGrade = lerp(1.0h - (half)_ShadowReceiveStrength, 1.0h, (half)mainLight.shadowAttenuation);

                half3 rgb = baseSample.rgb * grade * lightGrade * shadowGrade;
                return half4(rgb, baseSample.a);
            }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}
