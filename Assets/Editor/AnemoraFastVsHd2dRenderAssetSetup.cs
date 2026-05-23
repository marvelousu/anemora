using System;
using System.Collections.Generic;
using Anemora.TimeManagement.Portal;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dRenderAssetSetup
    {
        private const string SettingsDirectory = "Assets/Settings";
        private const string PipelineAssetPath = SettingsDirectory + "/UniversalRenderPipeline.asset";
        private const string RendererDataPath = SettingsDirectory + "/UniversalRenderPipeline_Renderer.asset";
        private const string VolumeProfilePath = SettingsDirectory + "/DefaultVolumeProfile.asset";
        private const string PortalStencilFeatureName = "PortalStencilFeature";
        private const string Hd2dSsaoFeatureName = "FastVS HD2D Soft Contact Occlusion";

        public static void ApplyShadingFoundationV1()
        {
            EnsureFolder(SettingsDirectory);

            var pipelineAsset = LoadRequiredAsset<UniversalRenderPipelineAsset>(PipelineAssetPath);
            var rendererData = LoadRequiredAsset<UniversalRendererData>(RendererDataPath);
            var volumeProfile = LoadOrCreateVolumeProfile(VolumeProfilePath);

            ApplyPipelineSettings(pipelineAsset);
            ApplyRendererSettings(rendererData);
            ApplyVolumeProfileSettings(volumeProfile);

            EditorUtility.SetDirty(pipelineAsset);
            EditorUtility.SetDirty(rendererData);
            EditorUtility.SetDirty(volumeProfile);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(
                "Shading Foundation v1 applied: " +
                "shadowDistance=35, mainShadowmap=4096, ssao=PortalStencilFeature+BlueNoise/DepthNormals, " +
                "volumeProfile=slightly darker faded Bloom/ColorAdjustments/Vignette with Neutral tonemapping.");
        }

        public static void ApplyFastVsHd2dRenderAssets()
        {
            ApplyShadingFoundationV1();
        }

        private static void ApplyPipelineSettings(UniversalRenderPipelineAsset pipelineAsset)
        {
            var serialized = new SerializedObject(pipelineAsset);
            TrySetFloat(serialized, "m_ShadowDistance", 35f, "shadowDistance");
            TrySetBool(serialized, "m_MainLightShadowsSupported", true, "mainLightShadowsSupported");
            TrySetInt(serialized, "m_MainLightShadowmapResolution", 4096, "mainLightShadowmapResolution");
            TrySetInt(serialized, "m_AdditionalLightsRenderingMode", 1, "additionalLightsRenderingMode");
            TrySetInt(serialized, "m_AdditionalLightsPerObjectLimit", 4, "additionalLightsPerObjectLimit");
            TrySetBool(serialized, "m_AdditionalLightShadowsSupported", true, "additionalLightShadowsSupported");
            TrySetBool(serialized, "m_RequireDepthTexture", true, "requireDepthTexture");
            TrySetBool(serialized, "m_RequireOpaqueTexture", true, "requireOpaqueTexture");
            TrySetBool(serialized, "m_SoftShadowsSupported", true, "softShadowsSupported");
            TrySetInt(serialized, "m_SoftShadowQuality", 3, "softShadowQuality");
            TrySetInt(serialized, "m_ShadowCascadeCount", 2, "shadowCascadeCount");
            TrySetFloat(serialized, "m_Cascade2Split", 0.35f, "cascade2Split");
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ApplyRendererSettings(UniversalRendererData rendererData)
        {
            var portalFeature = FindFeature(rendererData, typeof(PortalStencilFeature)) as PortalStencilFeature;
            if (portalFeature == null)
            {
                throw new InvalidOperationException("Could not find PortalStencilFeature on UniversalRenderPipeline_Renderer.asset.");
            }

            portalFeature.name = PortalStencilFeatureName;
            portalFeature.SetActive(true);
            EditorUtility.SetDirty(portalFeature);

            var ssaoFeature = FindFeature(rendererData, typeof(ScreenSpaceAmbientOcclusion)) as ScreenSpaceAmbientOcclusion;

            if (ssaoFeature == null)
            {
                ssaoFeature = ScriptableObject.CreateInstance<ScreenSpaceAmbientOcclusion>();
                ssaoFeature.name = Hd2dSsaoFeatureName;
                AssetDatabase.AddObjectToAsset(ssaoFeature, rendererData);
            }

            ssaoFeature.name = Hd2dSsaoFeatureName;
            ssaoFeature.SetActive(true);

            ApplySsaoSettings(ssaoFeature);
            ssaoFeature.Create();
            EditorUtility.SetDirty(ssaoFeature);

            var orderedFeatures = new List<ScriptableRendererFeature>(rendererData.rendererFeatures.Count + 1);
            var portalAdded = false;
            var ssaoAdded = false;

            for (var i = 0; i < rendererData.rendererFeatures.Count; i++)
            {
                var feature = rendererData.rendererFeatures[i];
                if (feature == null)
                {
                    continue;
                }

                if (feature is PortalStencilFeature)
                {
                    if (!portalAdded && portalFeature != null)
                    {
                        orderedFeatures.Add(portalFeature);
                        portalAdded = true;
                    }

                    continue;
                }

                if (feature is ScreenSpaceAmbientOcclusion)
                {
                    if (!ssaoAdded)
                    {
                        orderedFeatures.Add(ssaoFeature);
                        ssaoAdded = true;
                    }

                    continue;
                }

                orderedFeatures.Add(feature);
            }

            if (!portalAdded)
            {
                orderedFeatures.Insert(0, portalFeature);
                portalAdded = true;
            }

            if (!ssaoAdded)
            {
                if (portalAdded)
                {
                    orderedFeatures.Insert(1, ssaoFeature);
                }
                else
                {
                    orderedFeatures.Add(ssaoFeature);
                }
            }

            var serialized = new SerializedObject(rendererData);
            var featureList = serialized.FindProperty("m_RendererFeatures");
            var featureMap = serialized.FindProperty("m_RendererFeatureMap");

            featureList.arraySize = orderedFeatures.Count;
            featureMap.arraySize = orderedFeatures.Count;

            for (var i = 0; i < orderedFeatures.Count; i++)
            {
                var feature = orderedFeatures[i];
                featureList.GetArrayElementAtIndex(i).objectReferenceValue = feature;
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(feature, out _, out long localId))
                {
                    featureMap.GetArrayElementAtIndex(i).longValue = localId;
                }
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(rendererData);
        }

        private static void ApplySsaoSettings(ScreenSpaceAmbientOcclusion ssaoFeature)
        {
            var serialized = new SerializedObject(ssaoFeature);
            var settings = FindProperty(serialized, "m_Settings", "settings");

            TrySetEnumByPreferredNames(settings, "AOMethod", new[] { "BlueNoise" }, "m_AOMethod");
            TrySetBool(settings, "Downsample", true, "m_Downsample");
            TrySetEnumByPreferredNames(settings, "Source", new[] { "DepthNormals" }, "m_Source");
            TrySetEnumByPreferredNames(settings, "Samples", new[] { "High", "Medium" }, "m_Samples");
            TrySetEnumByPreferredNames(settings, "BlurQuality", new[] { "High", "Medium" }, "m_BlurQuality");
            TrySetFloat(settings, "Intensity", 0.58f, "m_Intensity");
            TrySetFloat(settings, "Radius", 0.085f, "m_Radius");
            TrySetFloat(settings, "DirectLightingStrength", 0.08f, "m_DirectLightingStrength");
            TrySetFloat(settings, "Falloff", 80f, "m_Falloff");

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ApplyVolumeProfileSettings(VolumeProfile volumeProfile)
        {
            var bloom = EnsureVolumeComponent<Bloom>(volumeProfile);
            bloom.active = true;
            bloom.threshold.overrideState = true;
            bloom.threshold.value = 0.78f;
            bloom.intensity.overrideState = true;
            bloom.intensity.value = 0.08f;
            bloom.scatter.overrideState = true;
            bloom.scatter.value = 0.47f;

            var colorAdjustments = EnsureVolumeComponent<ColorAdjustments>(volumeProfile);
            colorAdjustments.active = true;
            colorAdjustments.postExposure.overrideState = true;
            colorAdjustments.postExposure.value = -0.08f;
            colorAdjustments.contrast.overrideState = true;
            colorAdjustments.contrast.value = 6f;
            colorAdjustments.saturation.overrideState = true;
            colorAdjustments.saturation.value = -6f;

            var vignette = EnsureVolumeComponent<Vignette>(volumeProfile);
            vignette.active = true;
            vignette.intensity.overrideState = true;
            vignette.intensity.value = 0.045f;
            vignette.smoothness.overrideState = true;
            vignette.smoothness.value = 0.35f;

            var tonemapping = EnsureVolumeComponent<Tonemapping>(volumeProfile);
            tonemapping.active = true;
            tonemapping.mode.overrideState = true;
            tonemapping.mode.value = TonemappingMode.Neutral;

            ApplyOptionalColorGrade(volumeProfile);

            if (volumeProfile.TryGet<DepthOfField>(out var depthOfField))
            {
                depthOfField.active = false;
                depthOfField.SetAllOverridesTo(false);
                EditorUtility.SetDirty(depthOfField);
            }

            if (volumeProfile.TryGet<FilmGrain>(out var filmGrain))
            {
                filmGrain.active = false;
                filmGrain.SetAllOverridesTo(false);
                EditorUtility.SetDirty(filmGrain);
            }

            EditorUtility.SetDirty(bloom);
            EditorUtility.SetDirty(colorAdjustments);
            EditorUtility.SetDirty(vignette);
            EditorUtility.SetDirty(tonemapping);
        }

        private static T EnsureVolumeComponent<T>(VolumeProfile volumeProfile) where T : VolumeComponent
        {
            if (!volumeProfile.TryGet(out T component))
            {
                component = volumeProfile.Add<T>(false);
            }
            else
            {
                component.SetAllOverridesTo(false);
            }

            component.active = true;
            return component;
        }

        private static void ApplyOptionalColorGrade(VolumeProfile volumeProfile)
        {
            if (volumeProfile.TryGet<ShadowsMidtonesHighlights>(out var shadowsMidtonesHighlights))
            {
                shadowsMidtonesHighlights.active = true;
                shadowsMidtonesHighlights.SetAllOverridesTo(false);

                shadowsMidtonesHighlights.shadows.overrideState = true;
                shadowsMidtonesHighlights.shadows.value = new Vector4(0.985f, 0.995f, 1.015f, 0f);
                shadowsMidtonesHighlights.midtones.overrideState = true;
                shadowsMidtonesHighlights.midtones.value = new Vector4(1f, 1f, 1f, 0f);
                shadowsMidtonesHighlights.highlights.overrideState = true;
                shadowsMidtonesHighlights.highlights.value = new Vector4(1.012f, 1.006f, 0.992f, 0f);
                shadowsMidtonesHighlights.shadowsStart.overrideState = true;
                shadowsMidtonesHighlights.shadowsStart.value = 0.0f;
                shadowsMidtonesHighlights.shadowsEnd.overrideState = true;
                shadowsMidtonesHighlights.shadowsEnd.value = 0.30f;
                shadowsMidtonesHighlights.highlightsStart.overrideState = true;
                shadowsMidtonesHighlights.highlightsStart.value = 0.58f;
                shadowsMidtonesHighlights.highlightsEnd.overrideState = true;
                shadowsMidtonesHighlights.highlightsEnd.value = 1.0f;

                EditorUtility.SetDirty(shadowsMidtonesHighlights);
                return;
            }

            if (volumeProfile.TryGet<LiftGammaGain>(out var liftGammaGain))
            {
                liftGammaGain.active = true;
                liftGammaGain.SetAllOverridesTo(false);

                liftGammaGain.lift.overrideState = true;
                liftGammaGain.lift.value = new Vector4(0.985f, 0.995f, 1.01f, 0f);
                liftGammaGain.gamma.overrideState = true;
                liftGammaGain.gamma.value = new Vector4(1f, 1f, 1f, 0f);
                liftGammaGain.gain.overrideState = true;
                liftGammaGain.gain.value = new Vector4(1.01f, 1.005f, 0.995f, 0f);

                EditorUtility.SetDirty(liftGammaGain);
            }
        }

        private static ScriptableRendererFeature FindFeature(UniversalRendererData rendererData, Type featureType)
        {
            for (var i = 0; i < rendererData.rendererFeatures.Count; i++)
            {
                var feature = rendererData.rendererFeatures[i];
                if (feature != null && feature.GetType() == featureType)
                {
                    return feature;
                }
            }

            return null;
        }

        private static VolumeProfile LoadOrCreateVolumeProfile(string path)
        {
            var profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if (profile != null)
            {
                return profile;
            }

            profile = ScriptableObject.CreateInstance<VolumeProfile>();
            profile.name = "DefaultVolumeProfile";
            AssetDatabase.CreateAsset(profile, path);
            return profile;
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

        private static void SetBool(SerializedProperty parent, string fieldName, bool value, params string[] fallbackNames)
        {
            var property = FindPropertyRelative(parent, fieldName, fallbackNames);
            property.boolValue = value;
        }

        private static void SetBool(SerializedObject serialized, string fieldName, bool value, params string[] fallbackNames)
        {
            var property = FindProperty(serialized, fieldName, fallbackNames);
            property.boolValue = value;
        }

        private static void SetInt(SerializedProperty parent, string fieldName, int value, params string[] fallbackNames)
        {
            var property = FindPropertyRelative(parent, fieldName, fallbackNames);
            property.intValue = value;
        }

        private static void SetInt(SerializedObject serialized, string fieldName, int value, params string[] fallbackNames)
        {
            var property = FindProperty(serialized, fieldName, fallbackNames);
            property.intValue = value;
        }

        private static void SetFloat(SerializedProperty parent, string fieldName, float value, params string[] fallbackNames)
        {
            var property = FindPropertyRelative(parent, fieldName, fallbackNames);
            property.floatValue = value;
        }

        private static bool TrySetBool(SerializedObject serialized, string fieldName, bool value, params string[] fallbackNames)
        {
            if (!TryFindProperty(serialized, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            property.boolValue = value;
            return true;
        }

        private static bool TrySetBool(SerializedProperty parent, string fieldName, bool value, params string[] fallbackNames)
        {
            if (!TryFindPropertyRelative(parent, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            property.boolValue = value;
            return true;
        }

        private static bool TrySetInt(SerializedObject serialized, string fieldName, int value, params string[] fallbackNames)
        {
            if (!TryFindProperty(serialized, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            property.intValue = value;
            return true;
        }

        private static bool TrySetInt(SerializedProperty parent, string fieldName, int value, params string[] fallbackNames)
        {
            if (!TryFindPropertyRelative(parent, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            property.intValue = value;
            return true;
        }

        private static bool TrySetFloat(SerializedObject serialized, string fieldName, float value, params string[] fallbackNames)
        {
            if (!TryFindProperty(serialized, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            property.floatValue = value;
            return true;
        }

        private static bool TrySetFloat(SerializedProperty parent, string fieldName, float value, params string[] fallbackNames)
        {
            if (!TryFindPropertyRelative(parent, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            property.floatValue = value;
            return true;
        }

        private static bool TrySetEnumByPreferredNames(SerializedProperty parent, string fieldName, string[] preferredEnumNames, params string[] fallbackNames)
        {
            if (preferredEnumNames == null || preferredEnumNames.Length == 0)
            {
                return false;
            }

            if (!TryFindPropertyRelative(parent, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            for (var i = 0; i < preferredEnumNames.Length; i++)
            {
                var candidateName = preferredEnumNames[i];
                for (var j = 0; j < property.enumNames.Length; j++)
                {
                    if (string.Equals(property.enumNames[j], candidateName, StringComparison.OrdinalIgnoreCase))
                    {
                        property.enumValueIndex = j;
                        return true;
                    }
                }
            }

            return false;
        }

        private static void SetEnumByPreferredNames(SerializedProperty parent, string fieldName, string[] preferredEnumNames, params string[] fallbackNames)
        {
            if (preferredEnumNames == null || preferredEnumNames.Length == 0)
            {
                throw new InvalidOperationException($"Could not find enum values for nested serialized property '{fieldName}'.");
            }

            var property = FindPropertyRelative(parent, fieldName, fallbackNames);
            if (property == null)
            {
                throw new InvalidOperationException($"Could not find nested serialized property '{fieldName}'.");
            }

            for (var i = 0; i < preferredEnumNames.Length; i++)
            {
                var candidateName = preferredEnumNames[i];
                for (var j = 0; j < property.enumNames.Length; j++)
                {
                    if (string.Equals(property.enumNames[j], candidateName, StringComparison.OrdinalIgnoreCase))
                    {
                        property.enumValueIndex = j;
                        return;
                    }
                }
            }

            throw new InvalidOperationException($"None of the requested enum values were found on property '{property.propertyPath}'.");
        }

        private static void SetEnumByName(SerializedProperty parent, string fieldName, string enumName, params string[] fallbackNames)
        {
            var property = FindPropertyRelative(parent, fieldName, fallbackNames);
            for (var i = 0; i < property.enumNames.Length; i++)
            {
                if (string.Equals(property.enumNames[i], enumName, StringComparison.OrdinalIgnoreCase))
                {
                    property.enumValueIndex = i;
                    return;
                }
            }

            throw new InvalidOperationException($"Enum value '{enumName}' was not found on property '{property.propertyPath}'.");
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

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            var parts = path.Split('/');
            var current = parts[0];
            for (var i = 1; i < parts.Length; i++)
            {
                var next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }
    }
}
