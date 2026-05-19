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
        private const string Hd2dSsaoFeatureName = "FastVS HD2D Soft Contact Occlusion";

        public static void ApplyFastVsHd2dRenderAssets()
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
                "Fast VS HD2D render assets applied: " +
                "shadowDistance=30, softShadows=on, rendererFeature=PortalStencilFeature+FastVS HD2D Soft Contact Occlusion, " +
                "volumeProfile=Bloom/ColorAdjustments/Vignette/Tonemapping.");
        }

        private static void ApplyPipelineSettings(UniversalRenderPipelineAsset pipelineAsset)
        {
            var serialized = new SerializedObject(pipelineAsset);
            var shadowDistance = serialized.FindProperty("m_ShadowDistance");
            if (shadowDistance == null)
            {
                throw new InvalidOperationException("Could not find serialized property 'm_ShadowDistance' on UniversalRenderPipeline.asset.");
            }

            shadowDistance.floatValue = 30f;
            SetBool(serialized, "m_SoftShadowsSupported", true);
            SetInt(serialized, "m_MainLightShadowmapResolution", 2048);
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ApplyRendererSettings(UniversalRendererData rendererData)
        {
            var portalFeature = FindFeature(rendererData, typeof(PortalStencilFeature)) as PortalStencilFeature;
            var ssaoFeature = FindFeature(rendererData, typeof(ScreenSpaceAmbientOcclusion)) as ScreenSpaceAmbientOcclusion;

            if (ssaoFeature == null)
            {
                ssaoFeature = ScriptableObject.CreateInstance<ScreenSpaceAmbientOcclusion>();
                ssaoFeature.name = Hd2dSsaoFeatureName;
                ssaoFeature.SetActive(true);
                AssetDatabase.AddObjectToAsset(ssaoFeature, rendererData);
            }
            else
            {
                ssaoFeature.name = Hd2dSsaoFeatureName;
                ssaoFeature.SetActive(true);
            }

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

            if (!portalAdded && portalFeature != null)
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
            rendererData.SetDirty();
        }

        private static void ApplySsaoSettings(ScreenSpaceAmbientOcclusion ssaoFeature)
        {
            var serialized = new SerializedObject(ssaoFeature);
            var settings = FindProperty(serialized, "m_Settings", "settings");

            SetEnumByName(settings, "AOMethod", "BlueNoise", "m_AOMethod");
            SetBool(settings, "Downsample", true, "m_Downsample");
            SetEnumByName(settings, "Source", "DepthNormals", "m_Source");
            SetEnumByName(settings, "Samples", "Low", "m_Samples");
            SetEnumByName(settings, "BlurQuality", "Medium", "m_BlurQuality");
            SetFloat(settings, "Intensity", 0.75f, "m_Intensity");
            SetFloat(settings, "Radius", 0.035f, "m_Radius");
            SetFloat(settings, "DirectLightingStrength", 0.25f, "m_DirectLightingStrength");
            SetFloat(settings, "Falloff", 120f, "m_Falloff");

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ApplyVolumeProfileSettings(VolumeProfile volumeProfile)
        {
            var bloom = EnsureVolumeComponent<Bloom>(volumeProfile);
            bloom.active = true;
            bloom.threshold.overrideState = true;
            bloom.threshold.value = 0.82f;
            bloom.intensity.overrideState = true;
            bloom.intensity.value = 0.10f;
            bloom.scatter.overrideState = true;
            bloom.scatter.value = 0.45f;

            var colorAdjustments = EnsureVolumeComponent<ColorAdjustments>(volumeProfile);
            colorAdjustments.active = true;
            colorAdjustments.postExposure.overrideState = true;
            colorAdjustments.postExposure.value = 0.0f;
            colorAdjustments.contrast.overrideState = true;
            colorAdjustments.contrast.value = 6f;
            colorAdjustments.saturation.overrideState = true;
            colorAdjustments.saturation.value = 3f;

            var vignette = EnsureVolumeComponent<Vignette>(volumeProfile);
            vignette.active = true;
            vignette.intensity.overrideState = true;
            vignette.intensity.value = 0.06f;
            vignette.smoothness.overrideState = true;
            vignette.smoothness.value = 0.35f;

            var tonemapping = EnsureVolumeComponent<Tonemapping>(volumeProfile);
            tonemapping.active = true;
            tonemapping.mode.overrideState = true;
            tonemapping.mode.value = TonemappingMode.Neutral;

            if (volumeProfile.TryGet<DepthOfField>(out var depthOfField))
            {
                depthOfField.active = false;
                depthOfField.SetAllOverridesTo(false);
                EditorUtility.SetDirty(depthOfField);
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
