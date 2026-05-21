using System;
using System.Collections.Generic;
using Anemora.TimeManagement.Portal;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dShadingFoundationAudit
    {
        private const string SettingsDirectory = "Assets/Settings";
        private const string PipelineAssetPath = SettingsDirectory + "/UniversalRenderPipeline.asset";
        private const string RendererDataPath = SettingsDirectory + "/UniversalRenderPipeline_Renderer.asset";
        private const string VolumeProfilePath = SettingsDirectory + "/DefaultVolumeProfile.asset";

        [MenuItem("Tools/Anemora/Verify Shading Foundation V1")]
        public static void VerifyShadingFoundationV1()
        {
            var issues = new List<string>();

            ValidatePipeline(LoadRequiredAsset<UniversalRenderPipelineAsset>(PipelineAssetPath), issues);
            ValidateRenderer(LoadRequiredAsset<UniversalRendererData>(RendererDataPath), issues);
            ValidateVolume(LoadRequiredAsset<VolumeProfile>(VolumeProfilePath), issues);

            if (issues.Count > 0)
            {
                throw new InvalidOperationException("Shading Foundation v1 audit failed:\n- " + string.Join("\n- ", issues));
            }

            Debug.Log("Shading Foundation v1 audit passed.");
        }

        private static void ValidatePipeline(UniversalRenderPipelineAsset pipelineAsset, List<string> issues)
        {
            var serialized = new SerializedObject(pipelineAsset);

            RequireFloat(serialized, "m_ShadowDistance", 35f, 0.01f, issues, "Pipeline shadow distance must be 35.", "shadowDistance");
            RequireBool(serialized, "m_MainLightShadowsSupported", true, issues, "Main light shadows must be supported.", "mainLightShadowsSupported");
            RequireInt(serialized, "m_MainLightShadowmapResolution", 4096, issues, "Main light shadowmap resolution must be 4096.", "mainLightShadowmapResolution");
            RequireInt(serialized, "m_AdditionalLightsRenderingMode", 1, issues, "Additional lights rendering mode must be enabled.", "additionalLightsRenderingMode");
            RequireInt(serialized, "m_AdditionalLightsPerObjectLimit", 4, issues, "Additional lights per-object limit must be 4.", "additionalLightsPerObjectLimit");
            RequireBool(serialized, "m_AdditionalLightShadowsSupported", true, issues, "Additional light shadows must be supported.", "additionalLightShadowsSupported");
            RequireBool(serialized, "m_RequireDepthTexture", true, issues, "Depth texture must be required.", "requireDepthTexture");
            RequireBool(serialized, "m_RequireOpaqueTexture", true, issues, "Opaque texture must be required.", "requireOpaqueTexture");
            RequireBool(serialized, "m_SoftShadowsSupported", true, issues, "Soft shadows must be enabled.", "softShadowsSupported");
            RequireInt(serialized, "m_SoftShadowQuality", 3, issues, "Soft shadow quality must be high.", "softShadowQuality");
            RequireInt(serialized, "m_ShadowCascadeCount", 2, issues, "Shadow cascade count must be 2.", "shadowCascadeCount");
            RequireFloat(serialized, "m_Cascade2Split", 0.35f, 0.03f, issues, "Cascade 2 split must be near 0.35.", "cascade2Split");
            RequireBool(serialized, "m_SupportsHDR", true, issues, "HDR must remain enabled.", "supportsHDR");
        }

        private static void ValidateRenderer(UniversalRendererData rendererData, List<string> issues)
        {
            var portalCount = 0;
            var ssaoCount = 0;

            for (var i = 0; i < rendererData.rendererFeatures.Count; i++)
            {
                var feature = rendererData.rendererFeatures[i];
                if (feature == null)
                {
                    continue;
                }

                if (feature is PortalStencilFeature portalFeature)
                {
                    portalCount++;
                    if (i != 0)
                    {
                        issues.Add("PortalStencilFeature must be the first renderer feature.");
                    }

                    if (!portalFeature.isActive)
                    {
                        issues.Add("PortalStencilFeature must be active.");
                    }
                    continue;
                }

                if (feature is ScreenSpaceAmbientOcclusion ssaoFeature)
                {
                    ssaoCount++;
                    if (i != 1)
                    {
                        issues.Add("ScreenSpaceAmbientOcclusion must be the second renderer feature.");
                    }

                    if (!ssaoFeature.isActive)
                    {
                        issues.Add("ScreenSpaceAmbientOcclusion must be active.");
                    }

                    ValidateSsaoSettings(ssaoFeature, issues);
                }
            }

            if (portalCount != 1)
            {
                issues.Add($"Expected exactly one PortalStencilFeature, found {portalCount}.");
            }

            if (ssaoCount != 1)
            {
                issues.Add($"Expected exactly one ScreenSpaceAmbientOcclusion feature, found {ssaoCount}.");
            }
        }

        private static void ValidateSsaoSettings(ScreenSpaceAmbientOcclusion ssaoFeature, List<string> issues)
        {
            var serialized = new SerializedObject(ssaoFeature);
            var settings = FindProperty(serialized, "m_Settings", "settings");
            if (settings == null)
            {
                issues.Add("SSAO settings block was not found.");
                return;
            }

            RequireEnumName(settings, "AOMethod", "BlueNoise", issues, "SSAO AOMethod must be BlueNoise.", "m_AOMethod");
            RequireEnumName(settings, "Source", "DepthNormals", issues, "SSAO source must be DepthNormals.", "m_Source");
            RequireEnumNameOneOf(settings, "Samples", new[] { "High", "Medium" }, issues, "SSAO samples must be Medium or High.", "m_Samples");
            RequireEnumNameOneOf(settings, "BlurQuality", new[] { "High", "Medium" }, issues, "SSAO blur quality must be Medium or High.", "m_BlurQuality");
            RequireFloat(settings, "Intensity", 0.58f, 0.03f, issues, "SSAO intensity must be near 0.58.", "m_Intensity");
            RequireFloat(settings, "Radius", 0.085f, 0.01f, issues, "SSAO radius must be near 0.085.", "m_Radius");
            RequireFloat(settings, "DirectLightingStrength", 0.08f, 0.02f, issues, "SSAO direct lighting strength must be near 0.08.", "m_DirectLightingStrength");
            RequireFloat(settings, "Falloff", 80f, 5f, issues, "SSAO falloff must be near 80.", "m_Falloff");
        }

        private static void ValidateVolume(VolumeProfile volumeProfile, List<string> issues)
        {
            if (!volumeProfile.TryGet<Bloom>(out var bloom))
            {
                issues.Add("Bloom component is missing.");
            }
            else
            {
                RequireBool(bloom.active, true, issues, "Bloom must be active.");
                RequireBool(bloom.threshold.overrideState, true, issues, "Bloom threshold override must be enabled.");
                RequireFloat(bloom.threshold.value, 0.80f, 0.03f, issues, "Bloom threshold must be near 0.80.");
                RequireBool(bloom.intensity.overrideState, true, issues, "Bloom intensity override must be enabled.");
                RequireFloat(bloom.intensity.value, 0.07f, 0.02f, issues, "Bloom intensity must be near 0.07.");
                RequireBool(bloom.scatter.overrideState, true, issues, "Bloom scatter override must be enabled.");
                RequireFloat(bloom.scatter.value, 0.45f, 0.03f, issues, "Bloom scatter must be near 0.45.");
            }

            if (!volumeProfile.TryGet<ColorAdjustments>(out var colorAdjustments))
            {
                issues.Add("ColorAdjustments component is missing.");
            }
            else
            {
                RequireBool(colorAdjustments.active, true, issues, "ColorAdjustments must be active.");
                RequireBool(colorAdjustments.postExposure.overrideState, true, issues, "ColorAdjustments post exposure override must be enabled.");
                RequireFloat(colorAdjustments.postExposure.value, 0f, 0.001f, issues, "ColorAdjustments post exposure must stay at 0.");
                RequireBool(colorAdjustments.contrast.overrideState, true, issues, "ColorAdjustments contrast override must be enabled.");
                RequireFloat(colorAdjustments.contrast.value, 8f, 0.5f, issues, "ColorAdjustments contrast must be near 8.");
                RequireBool(colorAdjustments.saturation.overrideState, true, issues, "ColorAdjustments saturation override must be enabled.");
                RequireFloat(colorAdjustments.saturation.value, 2f, 0.5f, issues, "ColorAdjustments saturation must be near 2.");
            }

            if (!volumeProfile.TryGet<Vignette>(out var vignette))
            {
                issues.Add("Vignette component is missing.");
            }
            else
            {
                RequireBool(vignette.active, true, issues, "Vignette must be active.");
                RequireBool(vignette.intensity.overrideState, true, issues, "Vignette intensity override must be enabled.");
                if (vignette.intensity.value > 0.045f)
                {
                    issues.Add("Vignette intensity must be at or below 0.045.");
                }
            }

            if (!volumeProfile.TryGet<Tonemapping>(out var tonemapping))
            {
                issues.Add("Tonemapping component is missing.");
            }
            else
            {
                RequireBool(tonemapping.active, true, issues, "Tonemapping must be active.");
                RequireBool(tonemapping.mode.overrideState, true, issues, "Tonemapping mode override must be enabled.");
                RequireEnumValue(tonemapping.mode.value, TonemappingMode.Neutral, issues, "Tonemapping must remain Neutral.");
            }

            if (volumeProfile.TryGet<DepthOfField>(out var depthOfField) && depthOfField.active)
            {
                issues.Add("DepthOfField must be disabled.");
            }

            if (volumeProfile.TryGet<FilmGrain>(out var filmGrain) && filmGrain.active)
            {
                issues.Add("FilmGrain must be disabled.");
            }

            if (volumeProfile.TryGet<ShadowsMidtonesHighlights>(out var shadowsMidtonesHighlights))
            {
                ValidateShadowsMidtonesHighlights(shadowsMidtonesHighlights, issues);
            }
            else if (volumeProfile.TryGet<LiftGammaGain>(out var liftGammaGain))
            {
                ValidateLiftGammaGain(liftGammaGain, issues);
            }
        }

        private static void ValidateShadowsMidtonesHighlights(ShadowsMidtonesHighlights component, List<string> issues)
        {
            RequireBool(component.active, true, issues, "ShadowsMidtonesHighlights must be active when present.");
            RequireBool(component.shadows.overrideState, true, issues, "ShadowsMidtonesHighlights shadows override must be enabled.");
            RequireBool(component.midtones.overrideState, true, issues, "ShadowsMidtonesHighlights midtones override must be enabled.");
            RequireBool(component.highlights.overrideState, true, issues, "ShadowsMidtonesHighlights highlights override must be enabled.");
            RequireFloat(component.shadows.value.x, 0.985f, 0.03f, issues, "ShadowsMidtonesHighlights shadows tint is off target.");
            RequireFloat(component.shadows.value.y, 0.995f, 0.03f, issues, "ShadowsMidtonesHighlights shadows tint is off target.");
            RequireFloat(component.shadows.value.z, 1.015f, 0.03f, issues, "ShadowsMidtonesHighlights shadows tint is off target.");
            RequireFloat(component.midtones.value.x, 1f, 0.03f, issues, "ShadowsMidtonesHighlights midtones should stay neutral.");
            RequireFloat(component.midtones.value.y, 1f, 0.03f, issues, "ShadowsMidtonesHighlights midtones should stay neutral.");
            RequireFloat(component.midtones.value.z, 1f, 0.03f, issues, "ShadowsMidtonesHighlights midtones should stay neutral.");
            RequireFloat(component.highlights.value.x, 1.012f, 0.03f, issues, "ShadowsMidtonesHighlights highlights tint is off target.");
            RequireFloat(component.highlights.value.y, 1.006f, 0.03f, issues, "ShadowsMidtonesHighlights highlights tint is off target.");
            RequireFloat(component.highlights.value.z, 0.992f, 0.03f, issues, "ShadowsMidtonesHighlights highlights tint is off target.");
            RequireFloat(component.shadowsStart.value, 0f, 0.03f, issues, "ShadowsMidtonesHighlights shadows start must stay near 0.");
            RequireFloat(component.shadowsEnd.value, 0.30f, 0.03f, issues, "ShadowsMidtonesHighlights shadows end must stay near 0.30.");
            RequireFloat(component.highlightsStart.value, 0.58f, 0.03f, issues, "ShadowsMidtonesHighlights highlights start must stay near 0.58.");
            RequireFloat(component.highlightsEnd.value, 1f, 0.03f, issues, "ShadowsMidtonesHighlights highlights end must stay near 1.");
        }

        private static void ValidateLiftGammaGain(LiftGammaGain component, List<string> issues)
        {
            RequireBool(component.active, true, issues, "LiftGammaGain must be active when present.");
            RequireBool(component.lift.overrideState, true, issues, "LiftGammaGain lift override must be enabled.");
            RequireBool(component.gain.overrideState, true, issues, "LiftGammaGain gain override must be enabled.");
            RequireFloat(component.lift.value.x, 0.985f, 0.03f, issues, "LiftGammaGain lift tint is off target.");
            RequireFloat(component.lift.value.y, 0.995f, 0.03f, issues, "LiftGammaGain lift tint is off target.");
            RequireFloat(component.lift.value.z, 1.01f, 0.03f, issues, "LiftGammaGain lift tint is off target.");
            RequireFloat(component.gain.value.x, 1.01f, 0.03f, issues, "LiftGammaGain gain tint is off target.");
            RequireFloat(component.gain.value.y, 1.005f, 0.03f, issues, "LiftGammaGain gain tint is off target.");
            RequireFloat(component.gain.value.z, 0.995f, 0.03f, issues, "LiftGammaGain gain tint is off target.");
        }

        private static void RequireBool(SerializedObject serialized, string fieldName, bool expected, List<string> issues, string message, params string[] fallbackNames)
        {
            if (!TryGetBool(serialized, fieldName, out var actual, fallbackNames))
            {
                issues.Add(message + $" (missing property '{fieldName}').");
                return;
            }

            if (actual != expected)
            {
                issues.Add(message + $" (found {actual} on '{fieldName}').");
            }
        }

        private static void RequireBool(SerializedProperty parent, string fieldName, bool expected, List<string> issues, string message, params string[] fallbackNames)
        {
            if (!TryGetBool(parent, fieldName, out var actual, fallbackNames))
            {
                issues.Add(message + $" (missing property '{fieldName}').");
                return;
            }

            if (actual != expected)
            {
                issues.Add(message + $" (found {actual} on '{fieldName}').");
            }
        }

        private static void RequireInt(SerializedObject serialized, string fieldName, int expected, List<string> issues, string message, params string[] fallbackNames)
        {
            if (!TryGetInt(serialized, fieldName, out var actual, fallbackNames))
            {
                issues.Add(message + $" (missing property '{fieldName}').");
                return;
            }

            if (actual != expected)
            {
                issues.Add(message + $" (found {actual} on '{fieldName}').");
            }
        }

        private static void RequireInt(SerializedProperty parent, string fieldName, int expected, List<string> issues, string message, params string[] fallbackNames)
        {
            if (!TryGetInt(parent, fieldName, out var actual, fallbackNames))
            {
                issues.Add(message + $" (missing property '{fieldName}').");
                return;
            }

            if (actual != expected)
            {
                issues.Add(message + $" (found {actual} on '{fieldName}').");
            }
        }

        private static void RequireFloat(SerializedObject serialized, string fieldName, float expected, float tolerance, List<string> issues, string message, params string[] fallbackNames)
        {
            if (!TryGetFloat(serialized, fieldName, out var actual, fallbackNames))
            {
                issues.Add(message + $" (missing property '{fieldName}').");
                return;
            }

            if (Mathf.Abs(actual - expected) > tolerance)
            {
                issues.Add(message + $" (found {actual} on '{fieldName}').");
            }
        }

        private static void RequireFloat(SerializedProperty parent, string fieldName, float expected, float tolerance, List<string> issues, string message, params string[] fallbackNames)
        {
            if (!TryGetFloat(parent, fieldName, out var actual, fallbackNames))
            {
                issues.Add(message + $" (missing property '{fieldName}').");
                return;
            }

            if (Mathf.Abs(actual - expected) > tolerance)
            {
                issues.Add(message + $" (found {actual} on '{fieldName}').");
            }
        }

        private static void RequireBool(bool actual, bool expected, List<string> issues, string message)
        {
            if (actual != expected)
            {
                issues.Add(message + $" (found {actual}).");
            }
        }

        private static void RequireFloat(float actual, float expected, float tolerance, List<string> issues, string message)
        {
            if (Mathf.Abs(actual - expected) > tolerance)
            {
                issues.Add(message + $" (found {actual}).");
            }
        }

        private static void RequireEnumName(SerializedProperty parent, string fieldName, string expectedName, List<string> issues, string message, params string[] fallbackNames)
        {
            if (!TryGetEnumName(parent, fieldName, out var actualName, fallbackNames))
            {
                issues.Add(message + $" (missing property '{fieldName}').");
                return;
            }

            if (!string.Equals(actualName, expectedName, StringComparison.OrdinalIgnoreCase))
            {
                issues.Add(message + $" (found {actualName} on '{fieldName}').");
            }
        }

        private static void RequireEnumNameOneOf(SerializedProperty parent, string fieldName, string[] expectedNames, List<string> issues, string message, params string[] fallbackNames)
        {
            if (!TryGetEnumName(parent, fieldName, out var actualName, fallbackNames))
            {
                issues.Add(message + $" (missing property '{fieldName}').");
                return;
            }

            for (var i = 0; i < expectedNames.Length; i++)
            {
                if (string.Equals(actualName, expectedNames[i], StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            issues.Add(message + $" (found {actualName} on '{fieldName}').");
        }

        private static void RequireEnumValue<T>(T actual, T expected, List<string> issues, string message) where T : Enum
        {
            if (!EqualityComparer<T>.Default.Equals(actual, expected))
            {
                issues.Add(message + $" (found {actual}).");
            }
        }

        private static bool TryGetBool(SerializedObject serialized, string fieldName, out bool value, params string[] fallbackNames)
        {
            value = default;
            if (!TryFindProperty(serialized, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            value = property.boolValue;
            return true;
        }

        private static bool TryGetBool(SerializedProperty parent, string fieldName, out bool value, params string[] fallbackNames)
        {
            value = default;
            if (!TryFindPropertyRelative(parent, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            value = property.boolValue;
            return true;
        }

        private static bool TryGetInt(SerializedObject serialized, string fieldName, out int value, params string[] fallbackNames)
        {
            value = default;
            if (!TryFindProperty(serialized, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            value = property.intValue;
            return true;
        }

        private static bool TryGetInt(SerializedProperty parent, string fieldName, out int value, params string[] fallbackNames)
        {
            value = default;
            if (!TryFindPropertyRelative(parent, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            value = property.intValue;
            return true;
        }

        private static bool TryGetFloat(SerializedObject serialized, string fieldName, out float value, params string[] fallbackNames)
        {
            value = default;
            if (!TryFindProperty(serialized, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            value = property.floatValue;
            return true;
        }

        private static bool TryGetFloat(SerializedProperty parent, string fieldName, out float value, params string[] fallbackNames)
        {
            value = default;
            if (!TryFindPropertyRelative(parent, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            value = property.floatValue;
            return true;
        }

        private static bool TryGetEnumName(SerializedProperty parent, string fieldName, out string name, params string[] fallbackNames)
        {
            name = null;
            if (!TryFindPropertyRelative(parent, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            if (property.enumValueIndex < 0 || property.enumValueIndex >= property.enumNames.Length)
            {
                return false;
            }

            name = property.enumNames[property.enumValueIndex];
            return true;
        }

        private static SerializedProperty FindProperty(SerializedObject serialized, string fieldName, params string[] fallbackNames)
        {
            var property = serialized.FindProperty(fieldName);
            if (property != null)
            {
                return property;
            }

            for (var i = 0; i < fallbackNames.Length; i++)
            {
                property = serialized.FindProperty(fallbackNames[i]);
                if (property != null)
                {
                    return property;
                }
            }

            throw new InvalidOperationException($"Could not find serialized property '{fieldName}' on {serialized.targetObject.name}.");
        }

        private static bool TryFindProperty(SerializedObject serialized, string fieldName, out SerializedProperty property, params string[] fallbackNames)
        {
            property = serialized.FindProperty(fieldName);
            if (property != null)
            {
                return true;
            }

            for (var i = 0; i < fallbackNames.Length; i++)
            {
                property = serialized.FindProperty(fallbackNames[i]);
                if (property != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static SerializedProperty FindPropertyRelative(SerializedProperty parent, string fieldName, params string[] fallbackNames)
        {
            if (parent == null)
            {
                throw new InvalidOperationException($"Could not find nested serialized property '{fieldName}'.");
            }

            var property = parent.FindPropertyRelative(fieldName);
            if (property != null)
            {
                return property;
            }

            for (var i = 0; i < fallbackNames.Length; i++)
            {
                property = parent.FindPropertyRelative(fallbackNames[i]);
                if (property != null)
                {
                    return property;
                }
            }

            throw new InvalidOperationException($"Could not find nested serialized property '{fieldName}' under '{parent.propertyPath}'.");
        }

        private static bool TryFindPropertyRelative(SerializedProperty parent, string fieldName, out SerializedProperty property, params string[] fallbackNames)
        {
            property = null;
            if (parent == null)
            {
                return false;
            }

            property = parent.FindPropertyRelative(fieldName);
            if (property != null)
            {
                return true;
            }

            for (var i = 0; i < fallbackNames.Length; i++)
            {
                property = parent.FindPropertyRelative(fallbackNames[i]);
                if (property != null)
                {
                    return true;
                }
            }

            return false;
        }

        private static T LoadRequiredAsset<T>(string path) where T : UnityEngine.Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                throw new InvalidOperationException($"Missing required asset at {path}.");
            }

            return asset;
        }
    }
}
