using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Anemora.FastVS;
using Anemora.TimeManagement.Portal;
using UnityEditor;
using UnityEditor.Rendering;
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
        private const string MaterialDirectory = "Assets/Art/Materials/FastVS/HouseSlice";
        private const string Stage5LutDirectory = "Assets/Art/Textures/FastVS/HouseSlice";
        private const string Stage5DaylightLutPath = Stage5LutDirectory + "/LUT_Daylight_Plaza.png";
        private const string Stage5IndoorWarmLutPath = Stage5LutDirectory + "/LUT_Indoor_Warm.png";
        private const string Stage5PastLutPath = Stage5LutDirectory + "/LUT_TimeWindow_Past.png";
        private const int Stage5LutSize = 32;
        private const int Stage5LutWidth = Stage5LutSize * Stage5LutSize;
        private const int Stage5LutHeight = Stage5LutSize;
        private const string PortalStencilFeatureName = "PortalStencilFeature";
        private const string Hd2dSsaoFeatureName = "FastVS HD2D Soft Contact Occlusion";
        private const string Stage7TiltShiftFeatureName = "FastVS HD2D Stage7 TiltShift";
        private const string Stage7TiltShiftShaderName = "Anemora/FastVS/TiltShiftFullscreen";
        private const string Stage7TiltShiftMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_stage7_tilt_shift.mat";
        private const float Stage7TiltShiftIntensity = 0.68f;
        private const float Stage7TiltShiftRadius = 2.25f;
        private const float Stage7TiltShiftSharpCenter = 0.50f;
        private const float Stage7TiltShiftSharpWidth = 0.18f;
        private const float Stage7TiltShiftFeather = 0.34f;
        private const string Stage7OutlineFeatureName = "FastVS HD2D Stage7 Outline";
        private const string Stage7OutlineShaderName = "Anemora/FastVS/OutlineFullscreen";
        private const string Stage7OutlineMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_stage7_outline.mat";
        private const float Stage7OutlineIntensity = 0.18f;
        private const float Stage7OutlineThreshold = 0.115f;
        private const float Stage7OutlineRadius = 1.25f;
        private const float Stage7OutlineDepthWeight = 0.35f;
        private const float Stage7OutlineColorWeight = 0.65f;
        private const string P0AtmosphericPerspectiveFeatureName = "FastVS HD2D P0 Atmospheric Perspective";
        private const string P0AtmosphericPerspectiveShaderName = "Anemora/FastVS/AtmosphericPerspectiveFullscreen";
        private const string P0AtmosphericPerspectiveMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p0_atmospheric_perspective.mat";
        private const float P0AtmosphericPerspectiveLocalIntensity = 1f;
        private const string ButoRenderFeatureName = "Buto Volumetric Fog";
        private const string FronkonTiltShiftFeatureName = "Fronkon Artistic Tilt Shift";

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
                "shadowDistance=32, mainShadowmap=4096, cascades=0.08/0.24/0.55, ssao=PortalStencilFeature+BlueNoise/DepthNormals, " +
                "volumeProfile=reference Bloom/ColorAdjustments/Vignette/DepthOfField with ACES tonemapping.");
        }

        public static void ApplyFastVsHd2dRenderAssets()
        {
            ApplyShadingFoundationV1();
        }

        private static void ApplyPipelineSettings(UniversalRenderPipelineAsset pipelineAsset)
        {
            pipelineAsset.useSRPBatcher = true;
            pipelineAsset.gpuResidentDrawerMode = GPUResidentDrawerMode.InstancedDrawing;
            TrySetBatchRendererGroupShaderStrippingMode(BatchRendererGroupStrippingMode.KeepAll);

            var serialized = new SerializedObject(pipelineAsset);
            TrySetBool(serialized, "m_UseSRPBatcher", true, "useSRPBatcher");
            TrySetBool(serialized, "m_SupportsDynamicBatching", false, "supportsDynamicBatching");
            TrySetEnumByPreferredNames(serialized, "m_GPUResidentDrawerMode", new[] { "InstancedDrawing" }, "gpuResidentDrawerMode");
            TrySetFloat(serialized, "m_ShadowDistance", 32f, "shadowDistance");
            TrySetBool(serialized, "m_MainLightShadowsSupported", true, "mainLightShadowsSupported");
            TrySetInt(serialized, "m_MainLightShadowmapResolution", 4096, "mainLightShadowmapResolution");
            TrySetInt(serialized, "m_AdditionalLightsRenderingMode", 1, "additionalLightsRenderingMode");
            TrySetInt(serialized, "m_AdditionalLightsPerObjectLimit", 4, "additionalLightsPerObjectLimit");
            TrySetBool(serialized, "m_AdditionalLightShadowsSupported", true, "additionalLightShadowsSupported");
            TrySetBool(serialized, "m_RequireDepthTexture", true, "requireDepthTexture");
            TrySetBool(serialized, "m_RequireOpaqueTexture", true, "requireOpaqueTexture");
            TrySetBool(serialized, "m_SoftShadowsSupported", true, "softShadowsSupported");
            TrySetInt(serialized, "m_SoftShadowQuality", 3, "softShadowQuality");
            TrySetInt(serialized, "m_ShadowCascadeCount", 4, "shadowCascadeCount");
            TrySetVector3(serialized, "m_Cascade4Split", new Vector3(0.08f, 0.24f, 0.55f), "cascade4Split");
            TrySetFloat(serialized, "m_CascadeBorder", 0.15f, "cascadeBorder");
            TrySetFloat(serialized, "m_ShadowDepthBias", 0.8f, "shadowDepthBias");
            TrySetFloat(serialized, "m_ShadowNormalBias", 1f, "shadowNormalBias");
            TrySetEnumByPreferredNames(serialized, "m_LightProbeSystem", new[] { "ProbeVolumes" }, "lightProbeSystem");
            TrySetEnumByPreferredNames(serialized, "m_ProbeVolumeMemoryBudget", new[] { "MemoryBudgetMedium" }, "probeVolumeMemoryBudget");
            TrySetEnumByPreferredNames(serialized, "m_ProbeVolumeBlendingMemoryBudget", new[] { "MemoryBudgetMedium" }, "probeVolumeBlendingMemoryBudget");
            TrySetBool(serialized, "m_SupportProbeVolumeGPUStreaming", false, "supportProbeVolumeGPUStreaming");
            TrySetBool(serialized, "m_SupportProbeVolumeDiskStreaming", false, "supportProbeVolumeDiskStreaming");
            TrySetBool(serialized, "m_SupportProbeVolumeScenarios", false, "supportProbeVolumeScenarios");
            TrySetBool(serialized, "m_SupportProbeVolumeScenarioBlending", false, "supportProbeVolumeScenarioBlending");
            TrySetEnumByPreferredNames(serialized, "m_ProbeVolumeSHBands", new[] { "SphericalHarmonicsL1" }, "probeVolumeSHBands");
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void TrySetBatchRendererGroupShaderStrippingMode(BatchRendererGroupStrippingMode mode)
        {
            var property = typeof(EditorGraphicsSettings).GetProperty(
                "batchRendererGroupShaderStrippingMode",
                BindingFlags.Public | BindingFlags.Static);
            if (property != null && property.CanWrite)
            {
                property.SetValue(null, mode);
            }
        }

        private static void ApplyRendererSettings(UniversalRendererData rendererData)
        {
            rendererData.renderingMode = RenderingMode.ForwardPlus;
            // RecoveryV3 (2026-06-03): DepthPrimingMode.Auto + ForwardPlus skips the early depth
            // pre-pass and conflicts with PortalStencilFeature's stencil/depth sequencing, leaving
            // portal-stencil-dependent opaque geometry (ground/buildings) invisible and the time-window
            // contents stuck in the present world. Disable depth priming so the portal stencil pass is
            // depth-correct.
            rendererData.depthPrimingMode = DepthPrimingMode.Disabled;

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

#if BUTO
            var butoFeature = FindFeature(rendererData, typeof(OccaSoftware.Buto.Runtime.ButoRenderFeature)) as OccaSoftware.Buto.Runtime.ButoRenderFeature;
            if (butoFeature == null)
            {
                butoFeature = ScriptableObject.CreateInstance<OccaSoftware.Buto.Runtime.ButoRenderFeature>();
                butoFeature.name = ButoRenderFeatureName;
                AssetDatabase.AddObjectToAsset(butoFeature, rendererData);
            }

            butoFeature.name = ButoRenderFeatureName;
            butoFeature.SetActive(true);
            butoFeature.Create();
            EditorUtility.SetDirty(butoFeature);
#endif

            var atmosphericPerspectiveFeature = FindNamedFullScreenFeature(rendererData, P0AtmosphericPerspectiveFeatureName);
            if (atmosphericPerspectiveFeature == null)
            {
                atmosphericPerspectiveFeature = ScriptableObject.CreateInstance<FullScreenPassRendererFeature>();
                atmosphericPerspectiveFeature.name = P0AtmosphericPerspectiveFeatureName;
                AssetDatabase.AddObjectToAsset(atmosphericPerspectiveFeature, rendererData);
            }

            atmosphericPerspectiveFeature.name = P0AtmosphericPerspectiveFeatureName;
            atmosphericPerspectiveFeature.SetActive(true);
            atmosphericPerspectiveFeature.injectionPoint = FullScreenPassRendererFeature.InjectionPoint.BeforeRenderingPostProcessing;
            atmosphericPerspectiveFeature.fetchColorBuffer = true;
            atmosphericPerspectiveFeature.requirements = ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth;
            atmosphericPerspectiveFeature.passMaterial = EnsureP0AtmosphericPerspectiveMaterial();
            atmosphericPerspectiveFeature.passIndex = 0;
            atmosphericPerspectiveFeature.bindDepthStencilAttachment = false;
            atmosphericPerspectiveFeature.Create();
            EditorUtility.SetDirty(atmosphericPerspectiveFeature);

            var tiltShiftFeature = FindNamedFullScreenFeature(rendererData, Stage7TiltShiftFeatureName);
            if (tiltShiftFeature == null)
            {
                tiltShiftFeature = ScriptableObject.CreateInstance<FullScreenPassRendererFeature>();
                tiltShiftFeature.name = Stage7TiltShiftFeatureName;
                AssetDatabase.AddObjectToAsset(tiltShiftFeature, rendererData);
            }

            tiltShiftFeature.name = Stage7TiltShiftFeatureName;
#if FRONKON_TILTSHIFT
            tiltShiftFeature.SetActive(false);
#else
            tiltShiftFeature.SetActive(true);
#endif
            tiltShiftFeature.injectionPoint = FullScreenPassRendererFeature.InjectionPoint.AfterRenderingPostProcessing;
            tiltShiftFeature.fetchColorBuffer = true;
            tiltShiftFeature.requirements = ScriptableRenderPassInput.Color;
            tiltShiftFeature.passMaterial = EnsureStage7TiltShiftMaterial();
            tiltShiftFeature.passIndex = 0;
            tiltShiftFeature.bindDepthStencilAttachment = false;
            tiltShiftFeature.Create();
            EditorUtility.SetDirty(tiltShiftFeature);

            var outlineFeature = FindNamedFullScreenFeature(rendererData, Stage7OutlineFeatureName);
            if (outlineFeature == null)
            {
                outlineFeature = ScriptableObject.CreateInstance<FullScreenPassRendererFeature>();
                outlineFeature.name = Stage7OutlineFeatureName;
                AssetDatabase.AddObjectToAsset(outlineFeature, rendererData);
            }

            outlineFeature.name = Stage7OutlineFeatureName;
            outlineFeature.SetActive(true);
            outlineFeature.injectionPoint = FullScreenPassRendererFeature.InjectionPoint.AfterRenderingPostProcessing;
            outlineFeature.fetchColorBuffer = true;
            outlineFeature.requirements = ScriptableRenderPassInput.Color | ScriptableRenderPassInput.Depth;
            outlineFeature.passMaterial = EnsureStage7OutlineMaterial();
            outlineFeature.passIndex = 0;
            outlineFeature.bindDepthStencilAttachment = false;
            outlineFeature.Create();
            EditorUtility.SetDirty(outlineFeature);

#if FRONKON_TILTSHIFT
            var artisticTiltShiftFeature = FindFeature(rendererData, typeof(FronkonGames.Artistic.TiltShift.TiltShift)) as FronkonGames.Artistic.TiltShift.TiltShift;
            if (artisticTiltShiftFeature == null)
            {
                artisticTiltShiftFeature = ScriptableObject.CreateInstance<FronkonGames.Artistic.TiltShift.TiltShift>();
                artisticTiltShiftFeature.name = FronkonTiltShiftFeatureName;
                AssetDatabase.AddObjectToAsset(artisticTiltShiftFeature, rendererData);
            }

            artisticTiltShiftFeature.name = FronkonTiltShiftFeatureName;
            artisticTiltShiftFeature.SetActive(true);
            artisticTiltShiftFeature.Create();
            EditorUtility.SetDirty(artisticTiltShiftFeature);
#endif

            var orderedFeatures = new List<ScriptableRendererFeature>(rendererData.rendererFeatures.Count + 2);
            var portalAdded = false;
            var ssaoAdded = false;
#if BUTO
            var butoAdded = false;
#endif
            var atmosphericPerspectiveAdded = false;
            var tiltShiftAdded = false;
            var outlineAdded = false;
#if FRONKON_TILTSHIFT
            var artisticTiltShiftAdded = false;
#endif

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

#if BUTO
                if (feature == butoFeature || feature is OccaSoftware.Buto.Runtime.ButoRenderFeature)
                {
                    if (!butoAdded)
                    {
                        orderedFeatures.Add(butoFeature);
                        butoAdded = true;
                    }

                    continue;
                }
#endif

                if (feature == atmosphericPerspectiveFeature || feature is FullScreenPassRendererFeature && feature.name == P0AtmosphericPerspectiveFeatureName)
                {
                    if (!atmosphericPerspectiveAdded)
                    {
                        orderedFeatures.Add(atmosphericPerspectiveFeature);
                        atmosphericPerspectiveAdded = true;
                    }

                    continue;
                }

                if (feature == tiltShiftFeature || feature is FullScreenPassRendererFeature && feature.name == Stage7TiltShiftFeatureName)
                {
                    if (!tiltShiftAdded)
                    {
                        orderedFeatures.Add(tiltShiftFeature);
                        tiltShiftAdded = true;
                    }

                    continue;
                }

                if (feature == outlineFeature || feature is FullScreenPassRendererFeature && feature.name == Stage7OutlineFeatureName)
                {
                    if (!outlineAdded)
                    {
                        orderedFeatures.Add(outlineFeature);
                        outlineAdded = true;
                    }

                    continue;
                }

#if FRONKON_TILTSHIFT
                if (feature == artisticTiltShiftFeature || feature is FronkonGames.Artistic.TiltShift.TiltShift)
                {
                    if (!artisticTiltShiftAdded)
                    {
                        orderedFeatures.Add(artisticTiltShiftFeature);
                        artisticTiltShiftAdded = true;
                    }

                    continue;
                }
#endif

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

                ssaoAdded = true;
            }

#if BUTO
            if (!butoAdded)
            {
                var insertIndex = Mathf.Min(orderedFeatures.Count, portalAdded && ssaoAdded ? 2 : orderedFeatures.Count);
                orderedFeatures.Insert(insertIndex, butoFeature);
                butoAdded = true;
            }
#endif

            if (!atmosphericPerspectiveAdded)
            {
                var insertIndex = Mathf.Min(orderedFeatures.Count, portalAdded && ssaoAdded ? 2 : orderedFeatures.Count);
#if BUTO
                if (butoAdded)
                {
                    insertIndex = Mathf.Min(orderedFeatures.Count, orderedFeatures.IndexOf(butoFeature) + 1);
                }
#endif
                orderedFeatures.Insert(insertIndex, atmosphericPerspectiveFeature);
                atmosphericPerspectiveAdded = true;
            }

            if (!tiltShiftAdded)
            {
                var insertIndex = Mathf.Min(orderedFeatures.Count, portalAdded && ssaoAdded ? 2 : orderedFeatures.Count);
#if BUTO
                if (butoAdded)
                {
                    insertIndex = Mathf.Min(orderedFeatures.Count, orderedFeatures.IndexOf(butoFeature) + 1);
                }
#endif
                if (atmosphericPerspectiveAdded)
                {
                    insertIndex = Mathf.Min(orderedFeatures.Count, orderedFeatures.IndexOf(atmosphericPerspectiveFeature) + 1);
                }
                orderedFeatures.Insert(insertIndex, tiltShiftFeature);
                tiltShiftAdded = true;
            }

            if (!outlineAdded)
            {
                var insertIndex = tiltShiftAdded
                    ? Mathf.Min(orderedFeatures.Count, orderedFeatures.IndexOf(tiltShiftFeature) + 1)
                    : orderedFeatures.Count;
                orderedFeatures.Insert(insertIndex, outlineFeature);
            }

#if FRONKON_TILTSHIFT
            if (!artisticTiltShiftAdded)
            {
                orderedFeatures.Add(artisticTiltShiftFeature);
            }
#endif

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

        private static Material EnsureP0AtmosphericPerspectiveMaterial()
        {
            EnsureFolder(MaterialDirectory);

            var shader = Shader.Find(P0AtmosphericPerspectiveShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException($"P0 atmospheric perspective shader not found: {P0AtmosphericPerspectiveShaderName}");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(P0AtmosphericPerspectiveMaterialPath);
            if (material == null)
            {
                material = new Material(shader)
                {
                    name = "FastVS_House_hd2d_p0_atmospheric_perspective"
                };
                AssetDatabase.CreateAsset(material, P0AtmosphericPerspectiveMaterialPath);
            }
            else if (material.shader != shader)
            {
                material.shader = shader;
            }

            material.SetFloat("_LocalIntensity", P0AtmosphericPerspectiveLocalIntensity);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material EnsureStage7TiltShiftMaterial()
        {
            EnsureFolder(MaterialDirectory);

            var shader = Shader.Find(Stage7TiltShiftShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException($"Stage 7 tilt-shift shader not found: {Stage7TiltShiftShaderName}");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(Stage7TiltShiftMaterialPath);
            if (material == null)
            {
                material = new Material(shader)
                {
                    name = "FastVS_House_hd2d_stage7_tilt_shift"
                };
                AssetDatabase.CreateAsset(material, Stage7TiltShiftMaterialPath);
            }
            else if (material.shader != shader)
            {
                material.shader = shader;
            }

            material.SetFloat("_Intensity", Stage7TiltShiftIntensity);
            material.SetFloat("_Radius", Stage7TiltShiftRadius);
            material.SetFloat("_SharpCenter", Stage7TiltShiftSharpCenter);
            material.SetFloat("_SharpWidth", Stage7TiltShiftSharpWidth);
            material.SetFloat("_Feather", Stage7TiltShiftFeather);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Material EnsureStage7OutlineMaterial()
        {
            EnsureFolder(MaterialDirectory);

            var shader = Shader.Find(Stage7OutlineShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException($"Stage 7 outline shader not found: {Stage7OutlineShaderName}");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(Stage7OutlineMaterialPath);
            if (material == null)
            {
                material = new Material(shader)
                {
                    name = "FastVS_House_hd2d_stage7_outline"
                };
                AssetDatabase.CreateAsset(material, Stage7OutlineMaterialPath);
            }
            else if (material.shader != shader)
            {
                material.shader = shader;
            }

            material.SetFloat("_Intensity", Stage7OutlineIntensity);
            material.SetFloat("_Threshold", Stage7OutlineThreshold);
            material.SetFloat("_Radius", Stage7OutlineRadius);
            material.SetFloat("_DepthWeight", Stage7OutlineDepthWeight);
            material.SetFloat("_ColorWeight", Stage7OutlineColorWeight);
            EditorUtility.SetDirty(material);
            return material;
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
            RemoveNullVolumeComponents(volumeProfile);

            var bloom = EnsureVolumeComponent<Bloom>(volumeProfile);
            bloom.active = true;
            bloom.threshold.overrideState = true;
            bloom.threshold.value = 1.05f;
            bloom.intensity.overrideState = true;
            bloom.intensity.value = 0.40f;
            bloom.scatter.overrideState = true;
            bloom.scatter.value = 0.74f;
            bloom.clamp.overrideState = true;
            bloom.clamp.value = 16f;
            bloom.tint.overrideState = true;
            bloom.tint.value = new Color(1.00f, 0.92f, 0.80f, 1f);
            bloom.highQualityFiltering.overrideState = true;
            bloom.highQualityFiltering.value = true;
            bloom.filter.overrideState = true;
            bloom.filter.value = BloomFilterMode.Gaussian;
            bloom.downscale.overrideState = true;
            bloom.downscale.value = BloomDownscaleMode.Half;
            bloom.maxIterations.overrideState = true;
            bloom.maxIterations.value = 6;
            bloom.dirtTexture.overrideState = true;
            bloom.dirtTexture.value = null;
            bloom.dirtIntensity.overrideState = true;
            bloom.dirtIntensity.value = 0f;

            var colorAdjustments = EnsureVolumeComponent<ColorAdjustments>(volumeProfile);
            colorAdjustments.active = true;
            colorAdjustments.postExposure.overrideState = true;
            colorAdjustments.postExposure.value = 0f;
            colorAdjustments.contrast.overrideState = true;
            colorAdjustments.contrast.value = 14f;
            colorAdjustments.saturation.overrideState = true;
            colorAdjustments.saturation.value = 0f;

            var vignette = EnsureVolumeComponent<Vignette>(volumeProfile);
            vignette.active = true;
            vignette.intensity.overrideState = true;
            vignette.intensity.value = 0.30f;
            vignette.smoothness.overrideState = true;
            vignette.smoothness.value = 0.40f;

            var tonemapping = EnsureVolumeComponent<Tonemapping>(volumeProfile);
            tonemapping.active = true;
            tonemapping.mode.overrideState = true;
            tonemapping.mode.value = TonemappingMode.ACES;

            var whiteBalance = EnsureVolumeComponent<WhiteBalance>(volumeProfile);
            whiteBalance.active = true;
            whiteBalance.temperature.overrideState = true;
            whiteBalance.temperature.value = 8f;
            whiteBalance.tint.overrideState = true;
            whiteBalance.tint.value = 0f;

            ApplyStage5ColorLookup(volumeProfile);
            ApplyOptionalColorGrade(volumeProfile);
            ApplyP0AtmosphericPerspectiveVolume(volumeProfile);

            var depthOfField = EnsureVolumeComponent<DepthOfField>(volumeProfile);
            depthOfField.active = true;
            depthOfField.mode.overrideState = true;
            depthOfField.mode.value = DepthOfFieldMode.Bokeh;
            depthOfField.focusDistance.overrideState = true;
            depthOfField.focusDistance.value = 5.4f;
            depthOfField.aperture.overrideState = true;
            depthOfField.aperture.value = 2.4f;
            depthOfField.focalLength.overrideState = true;
            depthOfField.focalLength.value = 85f;
            depthOfField.bladeCount.overrideState = true;
            depthOfField.bladeCount.value = 6;
            depthOfField.bladeCurvature.overrideState = true;
            depthOfField.bladeCurvature.value = 0.85f;
            depthOfField.bladeRotation.overrideState = true;
            depthOfField.bladeRotation.value = 0f;
            depthOfField.highQualitySampling.overrideState = true;
            depthOfField.highQualitySampling.value = true;

#if BUTO
            ApplyHd2dPhaseBBetaButoVolume(volumeProfile);
#endif
#if FRONKON_TILTSHIFT
            ApplyHd2dPhaseCBetaArtisticTiltShiftVolume(volumeProfile);
#endif

            if (volumeProfile.TryGet<FilmGrain>(out var filmGrain))
            {
                DisableVolumeComponentForBaseline(filmGrain);
            }

            EditorUtility.SetDirty(bloom);
            EditorUtility.SetDirty(colorAdjustments);
            EditorUtility.SetDirty(vignette);
            EditorUtility.SetDirty(tonemapping);
            EditorUtility.SetDirty(whiteBalance);
            EditorUtility.SetDirty(depthOfField);
        }

        private static void ApplyP0AtmosphericPerspectiveVolume(VolumeProfile volumeProfile)
        {
            var atmospheric = EnsureVolumeComponent<FastVsHd2dAtmosphericPerspectiveVolume>(volumeProfile);
            atmospheric.strength.overrideState = true;
            atmospheric.strength.value = 0.12f;
            atmospheric.nearColor.overrideState = true;
            atmospheric.nearColor.value = new Color(0.88f, 0.78f, 0.62f, 1f);
            atmospheric.farColor.overrideState = true;
            atmospheric.farColor.value = new Color(0.54f, 0.64f, 0.74f, 1f);
            atmospheric.distanceStart.overrideState = true;
            atmospheric.distanceStart.value = 3.5f;
            atmospheric.distanceEnd.overrideState = true;
            atmospheric.distanceEnd.value = 12f;
            atmospheric.heightBand.overrideState = true;
            atmospheric.heightBand.value = new Vector2(-0.4f, 3.1f);
            atmospheric.heightStrength.overrideState = true;
            atmospheric.heightStrength.value = 0.32f;

            EditorUtility.SetDirty(atmospheric);
        }

#if BUTO
        private static void ApplyHd2dPhaseBBetaButoVolume(VolumeProfile volumeProfile)
        {
            var buto = EnsureVolumeComponent<OccaSoftware.Buto.Runtime.ButoVolumetricFog>(volumeProfile);
            buto.mode.overrideState = true;
            buto.mode.value = OccaSoftware.Buto.Runtime.VolumetricFogMode.On;
            buto.qualityLevel.overrideState = true;
            buto.qualityLevel.value = OccaSoftware.Buto.Runtime.QualityLevel.High;
            buto.anisotropy.overrideState = true;
            buto.anisotropy.value = 0.6f;
            buto.fogDensity.overrideState = true;
            buto.fogDensity.value = 0.02f;
            buto.maxDistanceVolumetric.overrideState = true;
            buto.maxDistanceVolumetric.value = 30f;
            buto.baseHeight.overrideState = true;
            buto.baseHeight.value = 0f;
            buto.attenuationBoundarySize.overrideState = true;
            buto.attenuationBoundarySize.value = 30f;
            buto.litColor.overrideState = true;
            buto.litColor.value = new Color(0.91f, 0.77f, 0.59f, 1f);
            buto.shadowedColor.overrideState = true;
            buto.shadowedColor.value = new Color(0.18f, 0.15f, 0.12f, 1f);
            buto.directionalForward.overrideState = true;
            buto.directionalForward.value = new Color(1.00f, 0.84f, 0.64f, 1f);
            buto.directionalBack.overrideState = true;
            buto.directionalBack.value = new Color(0.42f, 0.48f, 0.58f, 1f);

            EditorUtility.SetDirty(buto);
        }
#endif

#if FRONKON_TILTSHIFT
        private static void ApplyHd2dPhaseCBetaArtisticTiltShiftVolume(VolumeProfile volumeProfile)
        {
            var tiltShift = EnsureVolumeComponent<FronkonGames.Artistic.TiltShift.TiltShiftVolume>(volumeProfile);
            tiltShift.intensity.overrideState = true;
            tiltShift.intensity.value = 1f;
            tiltShift.quality.overrideState = true;
            tiltShift.quality.value = FronkonGames.Artistic.TiltShift.Quality.High;
            tiltShift.angle.overrideState = true;
            tiltShift.angle.value = 0f;
            tiltShift.aperture.overrideState = true;
            tiltShift.aperture.value = 0.7f;
            tiltShift.offset.overrideState = true;
            tiltShift.offset.value = 0f;
            tiltShift.blur.overrideState = true;
            tiltShift.blur.value = 1.2f;
            tiltShift.blurCurve.overrideState = true;
            tiltShift.blurCurve.value = 3f;
            tiltShift.distortion.overrideState = true;
            tiltShift.distortion.value = 0f;
            tiltShift.distortionScale.overrideState = true;
            tiltShift.distortionScale.value = 1f;

            EditorUtility.SetDirty(tiltShift);
        }
#endif

        private static void ApplyStage5ColorLookup(VolumeProfile volumeProfile)
        {
            var colorLookup = EnsureVolumeComponent<ColorLookup>(volumeProfile);
            colorLookup.active = true;
            colorLookup.texture.overrideState = true;
            colorLookup.texture.value = EnsureStage5LutTextures();
            colorLookup.contribution.overrideState = true;
            colorLookup.contribution.value = 0.60f;

            EditorUtility.SetDirty(colorLookup);
        }

        private static T EnsureVolumeComponent<T>(VolumeProfile volumeProfile) where T : VolumeComponent
        {
            var created = false;
            if (!volumeProfile.TryGet(out T component))
            {
                component = volumeProfile.Add<T>(false);
                created = true;
            }
            else
            {
                component.SetAllOverridesTo(false);
            }

            if (created && AssetDatabase.Contains(volumeProfile) && !AssetDatabase.Contains(component))
            {
                AssetDatabase.AddObjectToAsset(component, volumeProfile);
            }

            component.active = true;
            EditorUtility.SetDirty(volumeProfile);
            return component;
        }

        private static void RemoveNullVolumeComponents(VolumeProfile volumeProfile)
        {
            var removed = false;
            for (var i = volumeProfile.components.Count - 1; i >= 0; i--)
            {
                if (volumeProfile.components[i] != null)
                {
                    continue;
                }

                volumeProfile.components.RemoveAt(i);
                removed = true;
            }

            if (removed)
            {
                EditorUtility.SetDirty(volumeProfile);
            }
        }

        private static void DisableVolumeComponentForBaseline(VolumeComponent component)
        {
            if (component == null)
            {
                return;
            }

            component.active = false;
            component.SetAllOverridesTo(false);

            var serialized = new SerializedObject(component);
            if (TryFindProperty(serialized, "active", out var activeProperty, "m_Active"))
            {
                activeProperty.boolValue = false;
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(component);
        }

        private static Texture2D EnsureStage5LutTextures()
        {
            EnsureFolder(Stage5LutDirectory);
            EnsureStage5LutTexture(Stage5DaylightLutPath, ApplyStage5DaylightGrade);
            EnsureStage5LutTexture(Stage5IndoorWarmLutPath, ApplyStage5IndoorWarmGrade);
            EnsureStage5LutTexture(Stage5PastLutPath, ApplyStage5PastGrade);
            return LoadRequiredAsset<Texture2D>(Stage5DaylightLutPath);
        }

        private static Texture2D EnsureStage5LutTexture(string assetPath, Func<Color, Color> gradeFunction)
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            if (!IsStage5LutTextureValid(texture))
            {
                CreateStage5LutPng(assetPath, gradeFunction);
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
                texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            }

            ConfigureStage5LutImporter(assetPath);
            return AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
        }

        private static bool IsStage5LutTextureValid(Texture2D texture)
        {
            return texture != null && texture.width == Stage5LutWidth && texture.height == Stage5LutHeight;
        }

        private static void CreateStage5LutPng(string assetPath, Func<Color, Color> gradeFunction)
        {
            var texture = new Texture2D(Stage5LutWidth, Stage5LutHeight, TextureFormat.RGBA32, false);
            texture.name = Path.GetFileNameWithoutExtension(assetPath);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            var pixels = new Color32[Stage5LutWidth * Stage5LutHeight];
            for (var blue = 0; blue < Stage5LutSize; blue++)
            {
                var blueValue = blue / (Stage5LutSize - 1f);
                for (var green = 0; green < Stage5LutSize; green++)
                {
                    var greenValue = green / (Stage5LutSize - 1f);
                    for (var red = 0; red < Stage5LutSize; red++)
                    {
                        var redValue = red / (Stage5LutSize - 1f);
                        var graded = gradeFunction(new Color(redValue, greenValue, blueValue, 1f));
                        var pixelIndex = (green * Stage5LutWidth) + red + (blue * Stage5LutSize);
                        pixels[pixelIndex] = ToColor32(graded);
                    }
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, false);

            var fullPath = GetAssetFullPath(assetPath);
            File.WriteAllBytes(fullPath, texture.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(texture);
        }

        private static void ConfigureStage5LutImporter(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null)
            {
                throw new InvalidOperationException($"Missing texture importer for {assetPath}.");
            }

            var needsReimport = false;

            if (importer.textureType != TextureImporterType.Default)
            {
                importer.textureType = TextureImporterType.Default;
                needsReimport = true;
            }

            if (importer.sRGBTexture)
            {
                importer.sRGBTexture = false;
                needsReimport = true;
            }

            if (importer.mipmapEnabled)
            {
                importer.mipmapEnabled = false;
                needsReimport = true;
            }

            if (importer.wrapMode != TextureWrapMode.Clamp)
            {
                importer.wrapMode = TextureWrapMode.Clamp;
                needsReimport = true;
            }

            if (importer.filterMode != FilterMode.Bilinear)
            {
                importer.filterMode = FilterMode.Bilinear;
                needsReimport = true;
            }

            if (importer.textureCompression != TextureImporterCompression.Uncompressed)
            {
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                needsReimport = true;
            }

            if (importer.npotScale != TextureImporterNPOTScale.None)
            {
                importer.npotScale = TextureImporterNPOTScale.None;
                needsReimport = true;
            }

            if (importer.isReadable)
            {
                importer.isReadable = false;
                needsReimport = true;
            }

            if (importer.alphaSource != TextureImporterAlphaSource.FromInput)
            {
                importer.alphaSource = TextureImporterAlphaSource.FromInput;
                needsReimport = true;
            }

            if (needsReimport)
            {
                importer.SaveAndReimport();
            }
        }

        private static string GetAssetFullPath(string assetPath)
        {
            var projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            return Path.GetFullPath(Path.Combine(projectRoot, assetPath));
        }

        private static Color32 ToColor32(Color color)
        {
            return new Color32(
                (byte)Mathf.Clamp(Mathf.RoundToInt(Mathf.Clamp01(color.r) * 255f), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(Mathf.Clamp01(color.g) * 255f), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(Mathf.Clamp01(color.b) * 255f), 0, 255),
                (byte)Mathf.Clamp(Mathf.RoundToInt(Mathf.Clamp01(color.a) * 255f), 0, 255));
        }

        private static Color ApplyStage5DaylightGrade(Color color)
        {
            var luma = Stage5Luminance(color);
            color = Stage5ApplyContrast(color, 1.04f);
            color = Stage5ApplySaturation(color, 0.93f);
            color = Stage5Blend(color, new Color(0.90f, 0.96f, 1.06f, 1f), Stage5ShadowMask(luma) * 0.22f);
            color = Stage5Blend(color, new Color(1.08f, 1.01f, 0.90f, 1f), Stage5MidtoneMask(luma) * 0.18f);
            color = Stage5Blend(color, new Color(1.03f, 1.01f, 0.96f, 1f), Stage5HighlightMask(luma) * 0.08f);
            return Stage5Clamp(color);
        }

        private static Color ApplyStage5IndoorWarmGrade(Color color)
        {
            var luma = Stage5Luminance(color);
            color = Stage5ApplyContrast(color, 0.97f);
            color = Stage5ApplySaturation(color, 0.96f);
            color = Stage5Blend(color, new Color(1.10f, 0.98f, 1.04f, 1f), Stage5MidtoneMask(luma) * 0.18f);
            color = Stage5Blend(color, new Color(1.08f, 1.00f, 0.91f, 1f), Stage5MidtoneMask(luma) * 0.20f);
            color = Stage5Blend(color, new Color(0.96f, 0.91f, 1.02f, 1f), Stage5ShadowMask(luma) * 0.12f);
            return Stage5Clamp(color);
        }

        private static Color ApplyStage5PastGrade(Color color)
        {
            var luma = Stage5Luminance(color);
            color = Stage5ApplyContrast(color, 0.99f);
            color = Stage5ApplySaturation(color, 0.84f);
            color = Stage5Blend(color, new Color(0.88f, 0.96f, 1.08f, 1f), Stage5ShadowMask(luma) * 0.18f);
            color = Stage5Blend(color, new Color(1.12f, 1.00f, 0.83f, 1f), Stage5HighlightMask(luma) * 0.28f);
            color = Stage5Blend(color, new Color(1.06f, 0.98f, 0.90f, 1f), Stage5MidtoneMask(luma) * 0.14f);
            return Stage5Clamp(color);
        }

        private static float Stage5Luminance(Color color)
        {
            return (color.r * 0.2126f) + (color.g * 0.7152f) + (color.b * 0.0722f);
        }

        private static float Stage5ShadowMask(float luma)
        {
            return 1f - Mathf.SmoothStep(0.18f, 0.64f, luma);
        }

        private static float Stage5MidtoneMask(float luma)
        {
            var centered = 1f - Mathf.Abs(luma - 0.5f) * 2f;
            return Mathf.Clamp01(centered);
        }

        private static float Stage5HighlightMask(float luma)
        {
            return Mathf.SmoothStep(0.42f, 0.94f, luma);
        }

        private static Color Stage5ApplySaturation(Color color, float saturation)
        {
            var luma = Stage5Luminance(color);
            return new Color(
                luma + ((color.r - luma) * saturation),
                luma + ((color.g - luma) * saturation),
                luma + ((color.b - luma) * saturation),
                color.a);
        }

        private static Color Stage5ApplyContrast(Color color, float contrast)
        {
            const float pivot = 0.5f;
            return new Color(
                ((color.r - pivot) * contrast) + pivot,
                ((color.g - pivot) * contrast) + pivot,
                ((color.b - pivot) * contrast) + pivot,
                color.a);
        }

        private static Color Stage5Blend(Color source, Color target, float amount)
        {
            amount = Mathf.Clamp01(amount);
            return new Color(
                Mathf.Lerp(source.r, target.r, amount),
                Mathf.Lerp(source.g, target.g, amount),
                Mathf.Lerp(source.b, target.b, amount),
                source.a);
        }

        private static Color Stage5Clamp(Color color)
        {
            return new Color(
                Mathf.Clamp01(color.r),
                Mathf.Clamp01(color.g),
                Mathf.Clamp01(color.b),
                Mathf.Clamp01(color.a));
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

        private static FullScreenPassRendererFeature FindNamedFullScreenFeature(UniversalRendererData rendererData, string featureName)
        {
            for (var i = 0; i < rendererData.rendererFeatures.Count; i++)
            {
                var feature = rendererData.rendererFeatures[i];
                if (feature is FullScreenPassRendererFeature fullScreenFeature &&
                    string.Equals(fullScreenFeature.name, featureName, StringComparison.Ordinal))
                {
                    return fullScreenFeature;
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

        private static bool TrySetVector3(SerializedObject serialized, string fieldName, Vector3 value, params string[] fallbackNames)
        {
            if (!TryFindProperty(serialized, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            property.vector3Value = value;
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

        private static bool TrySetEnumByPreferredNames(SerializedObject serialized, string fieldName, string[] preferredEnumNames, params string[] fallbackNames)
        {
            if (preferredEnumNames == null || preferredEnumNames.Length == 0)
            {
                return false;
            }

            if (!TryFindProperty(serialized, fieldName, out var property, fallbackNames))
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
