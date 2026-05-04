Shader "Anemora/Portal/PortalMask"
{
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            Cull Off
            ZTest LEqual
            ZWrite Off
            ColorMask 0

            Stencil
            {
                Ref 8
                ReadMask 8
                WriteMask 8
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                return half4(0.0h, 0.0h, 0.0h, 0.0h);
            }
            ENDHLSL
        }

        Pass
        {
            Name "AnemoraPortalMask"
            Tags { "LightMode" = "AnemoraPortalMask" }

            Cull Off
            ZTest LEqual
            ZWrite Off
            ColorMask 0

            Stencil
            {
                Ref 8
                ReadMask 8
                WriteMask 8
                Comp Always
                Pass Replace
            }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                return half4(0.0h, 0.0h, 0.0h, 0.0h);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
