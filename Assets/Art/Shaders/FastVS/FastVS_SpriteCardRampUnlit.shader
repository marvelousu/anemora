Shader "Anemora/FastVS/SpriteCardRampUnlit"
{
    Properties
    {
        [MainTexture]_BaseMap("Base Map", 2D) = "white" {}
        [MainColor]_BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _MainTex("Main Tex", 2D) = "white" {}
        _RampStrength("Ramp Strength", Range(0, 0.5)) = 0.18
        _TopLight("Top Light", Color) = (1.08, 1.03, 0.96, 1)
        _SideShade("Side Shade", Color) = (0.94, 0.97, 1.03, 1)
        _FloorShade("Floor Shade", Color) = (0.89, 0.92, 0.96, 1)
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
            Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            float4 _BaseMap_ST;

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;

            float4 _BaseColor;
            half _RampStrength;
            float4 _TopLight;
            float4 _SideShade;
            float4 _FloorShade;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = input.uv;
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv * _BaseMap_ST.xy + _BaseMap_ST.zw;
                half4 baseSample = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, uv) * _BaseColor;

                if (baseSample.a <= 0.01h)
                {
                    return half4(0.0h, 0.0h, 0.0h, 0.0h);
                }

                float2 frameUv = float2(
                    saturate((uv.x - _BaseMap_ST.z) / max(_BaseMap_ST.x, 1e-5)),
                    saturate((uv.y - _BaseMap_ST.w) / max(_BaseMap_ST.y, 1e-5)));

                float warmKey = saturate((1.0 - frameUv.x) * frameUv.y);
                float coolSide = saturate(frameUv.x * (1.0 - frameUv.y * 0.18));
                float floorShade = saturate((1.0 - frameUv.y) * (1.0 - frameUv.y));
                float strength = saturate(_RampStrength);

                half3 neutral = half3(1.0h, 1.0h, 1.0h);
                half3 grade = neutral;
                grade *= lerp(neutral, (half3)_TopLight.rgb, (half)(warmKey * strength));
                grade *= lerp(neutral, (half3)_SideShade.rgb, (half)(coolSide * strength));
                grade *= lerp(neutral, (half3)_FloorShade.rgb, (half)(floorShade * strength));

                half3 rgb = baseSample.rgb * grade;
                return half4(rgb, baseSample.a);
            }
            ENDHLSL
        }
    }
}
