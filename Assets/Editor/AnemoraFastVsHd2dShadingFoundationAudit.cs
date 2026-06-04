using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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
        private const string TextureDirectory = "Assets/Art/Textures/FastVS/HouseSlice";
        private const string Stage5LutDaylightPlazaTexturePath = TextureDirectory + "/LUT_Daylight_Plaza.png";
        private const string Stage5LutIndoorWarmTexturePath = TextureDirectory + "/LUT_Indoor_Warm.png";
        private const string Stage5LutTimeWindowPastTexturePath = TextureDirectory + "/LUT_TimeWindow_Past.png";
        private const float Stage5LutContribution = 0.60f;
        private const string Cycle25OutputDirectory = "docs/devlog/screenshots/fast_vs_hd2d_postprocess_grade_cycle25_20260522";
        private const string Cycle25ReportFileName = "postprocess_grade_cycle25_20260522.md";
        private const string Cycle25ReportRelativePath = Cycle25OutputDirectory + "/" + Cycle25ReportFileName;

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

        [MenuItem("Tools/Anemora/Write HD2D Postprocess Grade Cycle 25 Report")]
        public static void WritePostprocessGradeCycle25ReportBatch()
        {
            AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene();
            VerifyShadingFoundationV1();

            Directory.CreateDirectory(GetAbsoluteProjectPath(Cycle25OutputDirectory));
            File.WriteAllText(GetAbsoluteProjectPath(Cycle25ReportRelativePath), BuildPostprocessGradeCycle25Markdown(), Encoding.UTF8);
            AssetDatabase.Refresh();

            Debug.Log($"HD2D postprocess grade cycle 25 report written: {GetAbsoluteProjectPath(Cycle25ReportRelativePath)}");
        }

        private static void ValidatePipeline(UniversalRenderPipelineAsset pipelineAsset, List<string> issues)
        {
            var serialized = new SerializedObject(pipelineAsset);

            RequireFloat(serialized, "m_ShadowDistance", 24f, 0.01f, issues, "Pipeline shadow distance must be 24 for the Tier-2 performance review profile.", "shadowDistance");
            RequireBool(serialized, "m_MainLightShadowsSupported", true, issues, "Main light shadows must be supported.", "mainLightShadowsSupported");
            RequireInt(serialized, "m_MainLightShadowmapResolution", 2048, issues, "Main light shadowmap resolution must be 2048 for the Tier-2 performance profile.", "mainLightShadowmapResolution");
            RequireInt(serialized, "m_AdditionalLightsRenderingMode", 1, issues, "Additional lights rendering mode must be enabled.", "additionalLightsRenderingMode");
            RequireInt(serialized, "m_AdditionalLightsPerObjectLimit", 2, issues, "Additional lights per-object limit must be 2 for the Tier-2 performance profile.", "additionalLightsPerObjectLimit");
            RequireBool(serialized, "m_AdditionalLightShadowsSupported", true, issues, "Additional light shadows must be supported.", "additionalLightShadowsSupported");
            RequireBool(serialized, "m_RequireDepthTexture", true, issues, "Depth texture must be required.", "requireDepthTexture");
            RequireBool(serialized, "m_RequireOpaqueTexture", true, issues, "Opaque texture must be required.", "requireOpaqueTexture");
            RequireBool(serialized, "m_SoftShadowsSupported", true, issues, "Soft shadows must be enabled.", "softShadowsSupported");
            RequireInt(serialized, "m_SoftShadowQuality", 2, issues, "Soft shadow quality must be medium for the Tier-2 performance profile.", "softShadowQuality");
            RequireInt(serialized, "m_ShadowCascadeCount", 3, issues, "Shadow cascade count must be 3 for the Tier-2 performance profile.", "shadowCascadeCount");
            RequireVector2(serialized, "m_Cascade3Split", new Vector2(0.12f, 0.38f), 0.01f, issues, "Cascade 3 split must be near 0.12 / 0.38 for the Tier-2 performance profile.", "cascade3Split");
            RequireFloat(serialized, "m_CascadeBorder", 0.15f, 0.01f, issues, "Cascade border must stay at 0.15 for the P1 contact-hardening review profile.", "cascadeBorder");
            RequireFloat(serialized, "m_ShadowDepthBias", 0.8f, 0.01f, issues, "Shadow depth bias must stay at 0.8 for the P1 contact-hardening review profile.", "shadowDepthBias");
            RequireFloat(serialized, "m_ShadowNormalBias", 1f, 0.01f, issues, "Shadow normal bias must stay at 1 for the P1 contact-hardening review profile.", "shadowNormalBias");
            RequireBool(serialized, "m_SupportsHDR", true, issues, "HDR must remain enabled.", "supportsHDR");
            RequireInt(serialized, "m_LightProbeSystem", 1, issues, "Adaptive Probe Volumes must be selected as the light probe system.", "lightProbeSystem");
            RequireInt(serialized, "m_ProbeVolumeMemoryBudget", 1024, issues, "APV memory budget must remain medium.", "probeVolumeMemoryBudget");
            RequireInt(serialized, "m_ProbeVolumeBlendingMemoryBudget", 256, issues, "APV blending memory budget must remain medium.", "probeVolumeBlendingMemoryBudget");
            RequireBool(serialized, "m_SupportProbeVolumeGPUStreaming", false, issues, "APV GPU streaming must remain disabled for the house-slice build.", "supportProbeVolumeGPUStreaming");
            RequireBool(serialized, "m_SupportProbeVolumeDiskStreaming", false, issues, "APV disk streaming must remain disabled for the house-slice build.", "supportProbeVolumeDiskStreaming");
            RequireBool(serialized, "m_SupportProbeVolumeScenarios", false, issues, "APV lighting scenarios must remain disabled for this pass.", "supportProbeVolumeScenarios");
            RequireBool(serialized, "m_SupportProbeVolumeScenarioBlending", false, issues, "APV scenario blending must remain disabled for this pass.", "supportProbeVolumeScenarioBlending");
            RequireInt(serialized, "m_ProbeVolumeSHBands", 1, issues, "APV must use L1 SH bands for this bounded pass.", "probeVolumeSHBands");
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
                RequireFloat(bloom.threshold.value, 1.05f, 0.03f, issues, "Bloom threshold must use the P1-26 threshold-gated HD-2D grade.");
                RequireBool(bloom.intensity.overrideState, true, issues, "Bloom intensity override must be enabled.");
                RequireFloat(bloom.intensity.value, 0.40f, 0.04f, issues, "Bloom intensity must stay low enough to avoid global haze.");
                RequireBool(bloom.scatter.overrideState, true, issues, "Bloom scatter override must be enabled.");
                RequireFloat(bloom.scatter.value, 0.74f, 0.06f, issues, "Bloom scatter must stay controlled for threshold-gated highlights.");
                RequireBool(bloom.clamp.overrideState, true, issues, "Bloom clamp override must be enabled.");
                RequireFloat(bloom.clamp.value, 16f, 0.05f, issues, "Bloom clamp must stay low enough to suppress emissive/firefly specks.");
                RequireBool(bloom.highQualityFiltering.overrideState, true, issues, "Bloom high quality filtering override must be enabled.");
                RequireBool(bloom.highQualityFiltering.value, true, issues, "Bloom high quality filtering must be enabled.");
            }

            if (!volumeProfile.TryGet<ColorAdjustments>(out var colorAdjustments))
            {
                issues.Add("ColorAdjustments component is missing.");
            }
            else
            {
                RequireBool(colorAdjustments.active, true, issues, "ColorAdjustments must be active.");
                RequireBool(colorAdjustments.postExposure.overrideState, true, issues, "ColorAdjustments post exposure override must be enabled.");
                RequireFloat(colorAdjustments.postExposure.value, 0f, 0.02f, issues, "ColorAdjustments post exposure must return to the Stage 1 neutral exposure.");
                RequireBool(colorAdjustments.contrast.overrideState, true, issues, "ColorAdjustments contrast override must be enabled.");
                RequireFloat(colorAdjustments.contrast.value, 14f, 1.0f, issues, "ColorAdjustments contrast must support the stronger sun-shadow grade while keeping readability.");
                RequireBool(colorAdjustments.saturation.overrideState, true, issues, "ColorAdjustments saturation override must be enabled.");
                RequireFloat(colorAdjustments.saturation.value, 0f, 0.5f, issues, "ColorAdjustments saturation must use the Stage 1 neutral saturation reset.");
            }

            if (!volumeProfile.TryGet<Vignette>(out var vignette))
            {
                issues.Add("Vignette component is missing.");
            }
            else
            {
                RequireBool(vignette.active, true, issues, "Vignette must be active.");
                RequireBool(vignette.intensity.overrideState, true, issues, "Vignette intensity override must be enabled.");
                RequireFloat(vignette.intensity.value, 0.30f, 0.02f, issues, "Vignette intensity must use the Stage 1 shadow frame.");
                RequireBool(vignette.smoothness.overrideState, true, issues, "Vignette smoothness override must be enabled.");
                RequireFloat(vignette.smoothness.value, 0.40f, 0.02f, issues, "Vignette smoothness must use the Stage 1 tighter frame.");
            }

            if (!volumeProfile.TryGet<Tonemapping>(out var tonemapping))
            {
                issues.Add("Tonemapping component is missing.");
            }
            else
            {
                RequireBool(tonemapping.active, true, issues, "Tonemapping must be active.");
                RequireBool(tonemapping.mode.overrideState, true, issues, "Tonemapping mode override must be enabled.");
                RequireEnumValue(tonemapping.mode.value, TonemappingMode.ACES, issues, "Tonemapping must use Stage 1 ACES.");
            }

            if (!volumeProfile.TryGet<WhiteBalance>(out var whiteBalance))
            {
                issues.Add("WhiteBalance component is missing.");
            }
            else
            {
                RequireBool(whiteBalance.active, true, issues, "WhiteBalance must be active.");
                RequireBool(whiteBalance.temperature.overrideState, true, issues, "WhiteBalance temperature override must be enabled.");
                RequireFloat(whiteBalance.temperature.value, 8f, 0.5f, issues, "WhiteBalance temperature must use the Stage 1 +8 warm lift.");
            }

            if (!volumeProfile.TryGet<ColorLookup>(out var colorLookup))
            {
                issues.Add("ColorLookup component is missing.");
            }
            else
            {
                ValidateStage5ColorLookup(colorLookup, issues);
            }

            if (!volumeProfile.TryGet<DepthOfField>(out var depthOfField))
            {
                issues.Add("DepthOfField component is missing.");
            }
            else
            {
                RequireBool(depthOfField.active, true, issues, "DepthOfField must be active for the HD-2D depth grade.");
                RequireBool(depthOfField.mode.overrideState, true, issues, "DepthOfField mode override must be enabled.");
                RequireEnumValue(depthOfField.mode.value, DepthOfFieldMode.Bokeh, issues, "DepthOfField mode must use the Stage 7 Bokeh profile.");
                RequireBool(depthOfField.focusDistance.overrideState, true, issues, "DepthOfField focus distance override must be enabled.");
                RequireFloat(depthOfField.focusDistance.value, 5.4f, 0.8f, issues, "DepthOfField focus distance must stay near the Stage 7 review baseline.");
                RequireBool(depthOfField.aperture.overrideState, true, issues, "DepthOfField aperture override must be enabled.");
                RequireFloat(depthOfField.aperture.value, 2.4f, 0.2f, issues, "DepthOfField aperture must stay near the Stage 7 shallow-focus baseline.");
                RequireBool(depthOfField.focalLength.overrideState, true, issues, "DepthOfField focal length override must be enabled.");
                RequireFloat(depthOfField.focalLength.value, 85f, 4.0f, issues, "DepthOfField focal length must stay near the Stage 7 tilt-shift baseline.");
                RequireBool(depthOfField.bladeCount.overrideState, true, issues, "DepthOfField blade count override must be enabled.");
                RequireFloat(depthOfField.bladeCount.value, 6f, 0.1f, issues, "DepthOfField blade count must stay at 6.");
                RequireBool(depthOfField.bladeCurvature.overrideState, true, issues, "DepthOfField blade curvature override must be enabled.");
                RequireFloat(depthOfField.bladeCurvature.value, 0.85f, 0.08f, issues, "DepthOfField blade curvature must stay near 0.85.");
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

        private static void ValidateStage5ColorLookup(ColorLookup colorLookup, List<string> issues)
        {
            ValidateStage5LutTexture(Stage5LutDaylightPlazaTexturePath, "daylight plaza LUT", issues);
            ValidateStage5LutTexture(Stage5LutIndoorWarmTexturePath, "indoor warm LUT", issues);
            ValidateStage5LutTexture(Stage5LutTimeWindowPastTexturePath, "TimeWindow past LUT", issues);

            RequireBool(colorLookup.active, true, issues, "ColorLookup must be active.");
            RequireBool(colorLookup.texture.overrideState, true, issues, "ColorLookup texture override must be enabled.");
            RequireBool(colorLookup.contribution.overrideState, true, issues, "ColorLookup contribution override must be enabled.");
            RequireFloat(colorLookup.contribution.value, Stage5LutContribution, 0.001f, issues, "ColorLookup contribution must use the Stage 5 LUT blend.");

            var texture = colorLookup.texture.value as Texture2D;
            if (texture == null)
            {
                issues.Add("ColorLookup texture must reference a Texture2D LUT.");
                return;
            }

            var texturePath = AssetDatabase.GetAssetPath(texture);
            if (!string.Equals(texturePath, Stage5LutDaylightPlazaTexturePath, StringComparison.Ordinal))
            {
                issues.Add($"ColorLookup texture must use {Stage5LutDaylightPlazaTexturePath}. (found {texturePath})");
            }
        }

        private static void ValidateStage5LutTexture(string path, string label, List<string> issues)
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            if (texture == null)
            {
                issues.Add($"Stage 5 {label} is missing at {path}.");
                return;
            }

            if (texture.width != 1024 || texture.height != 32)
            {
                issues.Add($"Stage 5 {label} must be 1024x32 for a 32^3 LUT. (found {texture.width}x{texture.height})");
            }

            if (texture.filterMode != FilterMode.Bilinear || texture.wrapMode != TextureWrapMode.Clamp)
            {
                issues.Add($"Stage 5 {label} must use bilinear filtering and clamp wrapping.");
            }

            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null)
            {
                issues.Add($"Stage 5 {label} must have TextureImporter settings.");
                return;
            }

            if (importer.mipmapEnabled)
            {
                issues.Add($"Stage 5 {label} must disable mipmaps.");
            }

            if (importer.sRGBTexture)
            {
                issues.Add($"Stage 5 {label} must disable sRGB import.");
            }

            if (importer.textureCompression != TextureImporterCompression.Uncompressed)
            {
                issues.Add($"Stage 5 {label} must use uncompressed import.");
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

        private static string BuildPostprocessGradeCycle25Markdown()
        {
            var pipeline = new SerializedObject(LoadRequiredAsset<UniversalRenderPipelineAsset>(PipelineAssetPath));
            var rendererData = LoadRequiredAsset<UniversalRendererData>(RendererDataPath);
            var volumeProfile = LoadRequiredAsset<VolumeProfile>(VolumeProfilePath);

            var builder = new StringBuilder();
            builder.AppendLine("# Fast VS HD2D Postprocess Grade Cycle 25 Report");
            builder.AppendLine();
            builder.AppendLine("Foundation audit report for the already-applied URP, renderer, and volume HD-2D shading setup. It records the faded camera grade contract, keeps FilmGrain disabled, and verifies the Stage 7 Bokeh DepthOfField baseline used by the current Fast VS HD-2D grade.");
            builder.AppendLine();
            builder.AppendLine($"- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`");
            builder.AppendLine($"- Worktree: `{GetAbsoluteProjectPath("")}`");
            builder.AppendLine($"- Report file: `{GetAbsoluteProjectPath(Cycle25ReportRelativePath)}`");
            builder.AppendLine($"- Pipeline asset: `{GetAbsoluteProjectPath(PipelineAssetPath)}`");
            builder.AppendLine($"- Renderer asset: `{GetAbsoluteProjectPath(RendererDataPath)}`");
            builder.AppendLine($"- Volume profile: `{GetAbsoluteProjectPath(VolumeProfilePath)}`");
            builder.AppendLine();
            builder.AppendLine("## Pipeline");
            builder.AppendLine();
            builder.AppendLine("| Field | Value |");
            builder.AppendLine("|---|---|");
            builder.AppendLine($"| Shadow distance | {FormatFloat(FindFloat(pipeline, "m_ShadowDistance", "shadowDistance"))} |");
            builder.AppendLine($"| Main light shadows supported | {FormatBool(FindBool(pipeline, "m_MainLightShadowsSupported", "mainLightShadowsSupported"))} |");
            builder.AppendLine($"| Main shadowmap resolution | {FormatInt(FindInt(pipeline, "m_MainLightShadowmapResolution", "mainLightShadowmapResolution"))} |");
            builder.AppendLine($"| Additional lights mode | {FormatInt(FindInt(pipeline, "m_AdditionalLightsRenderingMode", "additionalLightsRenderingMode"))} |");
            builder.AppendLine($"| Additional lights per object | {FormatInt(FindInt(pipeline, "m_AdditionalLightsPerObjectLimit", "additionalLightsPerObjectLimit"))} |");
            builder.AppendLine($"| Additional light shadows | {FormatBool(FindBool(pipeline, "m_AdditionalLightShadowsSupported", "additionalLightShadowsSupported"))} |");
            builder.AppendLine($"| Depth texture | {FormatBool(FindBool(pipeline, "m_RequireDepthTexture", "requireDepthTexture"))} |");
            builder.AppendLine($"| Opaque texture | {FormatBool(FindBool(pipeline, "m_RequireOpaqueTexture", "requireOpaqueTexture"))} |");
            builder.AppendLine($"| Soft shadows | {FormatBool(FindBool(pipeline, "m_SoftShadowsSupported", "softShadowsSupported"))} |");
            builder.AppendLine($"| Soft shadow quality | {FormatInt(FindInt(pipeline, "m_SoftShadowQuality", "softShadowQuality"))} |");
            builder.AppendLine($"| Cascade count | {FormatInt(FindInt(pipeline, "m_ShadowCascadeCount", "shadowCascadeCount"))} |");
            builder.AppendLine($"| Cascade 3 split | {FormatVector2(FindVector2(pipeline, "m_Cascade3Split", "cascade3Split"))} |");
            builder.AppendLine($"| Cascade border | {FormatFloat(FindFloat(pipeline, "m_CascadeBorder", "cascadeBorder"))} |");
            builder.AppendLine($"| Shadow depth bias | {FormatFloat(FindFloat(pipeline, "m_ShadowDepthBias", "shadowDepthBias"))} |");
            builder.AppendLine($"| Shadow normal bias | {FormatFloat(FindFloat(pipeline, "m_ShadowNormalBias", "shadowNormalBias"))} |");
            builder.AppendLine($"| HDR | {FormatBool(FindBool(pipeline, "m_SupportsHDR", "supportsHDR"))} |");
            builder.AppendLine();
            builder.AppendLine("## Renderer");
            builder.AppendLine();
            builder.AppendLine("| Feature | Order | Active | Asset Name |");
            builder.AppendLine("|---|---:|---|---|");
            AppendRendererFeatureRow(builder, rendererData, typeof(PortalStencilFeature), "PortalStencilFeature");
            AppendRendererFeatureRow(builder, rendererData, typeof(ScreenSpaceAmbientOcclusion), "ScreenSpaceAmbientOcclusion");
            builder.AppendLine();
            builder.AppendLine("### ScreenSpaceAmbientOcclusion Settings");
            builder.AppendLine();
            builder.AppendLine("| Field | Value |");
            builder.AppendLine("|---|---|");
            AppendSsaoSettingRows(builder, rendererData);
            builder.AppendLine();
            builder.AppendLine("## Volume");
            builder.AppendLine();
            builder.AppendLine("| Field | Value |");
            builder.AppendLine("|---|---|");
            AppendVolumeRows(builder, volumeProfile);

            return builder.ToString();
        }

        private static void AppendRendererFeatureRow(StringBuilder builder, UniversalRendererData rendererData, Type featureType, string displayName)
        {
            var feature = FindRendererFeature(rendererData, featureType, out var index);
            var assetName = feature != null ? feature.name : null;
            var orderText = feature == null ? "n/a" : FormatInt(index);
            var activeText = feature == null ? "n/a" : FormatBool(feature is PortalStencilFeature portalFeature ? (bool?)portalFeature.isActive : feature is ScreenSpaceAmbientOcclusion ssaoFeature ? (bool?)ssaoFeature.isActive : null);
            builder.AppendLine($"| {displayName} | {orderText} | {activeText} | {FormatText(assetName)} |");
        }

        private static void AppendSsaoSettingRows(StringBuilder builder, UniversalRendererData rendererData)
        {
            var feature = FindRendererFeature(rendererData, typeof(ScreenSpaceAmbientOcclusion), out _ ) as ScreenSpaceAmbientOcclusion;
            if (feature == null)
            {
                builder.AppendLine("| AOMethod | n/a |");
                builder.AppendLine("| Source | n/a |");
                builder.AppendLine("| Samples | n/a |");
                builder.AppendLine("| BlurQuality | n/a |");
                builder.AppendLine("| Intensity | n/a |");
                builder.AppendLine("| Radius | n/a |");
                builder.AppendLine("| DirectLightingStrength | n/a |");
                builder.AppendLine("| Falloff | n/a |");
                return;
            }

            var serialized = new SerializedObject(feature);
            if (!TryFindProperty(serialized, "m_Settings", out var settings, "settings"))
            {
                builder.AppendLine("| AOMethod | n/a |");
                builder.AppendLine("| Source | n/a |");
                builder.AppendLine("| Samples | n/a |");
                builder.AppendLine("| BlurQuality | n/a |");
                builder.AppendLine("| Intensity | n/a |");
                builder.AppendLine("| Radius | n/a |");
                builder.AppendLine("| DirectLightingStrength | n/a |");
                builder.AppendLine("| Falloff | n/a |");
                return;
            }

            builder.AppendLine($"| AOMethod | {FormatEnumName(settings, "AOMethod")} |");
            builder.AppendLine($"| Source | {FormatEnumName(settings, "Source")} |");
            builder.AppendLine($"| Samples | {FormatEnumName(settings, "Samples")} |");
            builder.AppendLine($"| BlurQuality | {FormatEnumName(settings, "BlurQuality")} |");
            builder.AppendLine($"| Intensity | {FormatFloat(FindFloat(settings, "Intensity"))} |");
            builder.AppendLine($"| Radius | {FormatFloat(FindFloat(settings, "Radius"))} |");
            builder.AppendLine($"| DirectLightingStrength | {FormatFloat(FindFloat(settings, "DirectLightingStrength"))} |");
            builder.AppendLine($"| Falloff | {FormatFloat(FindFloat(settings, "Falloff"))} |");
        }

        private static void AppendVolumeRows(StringBuilder builder, VolumeProfile volumeProfile)
        {
            if (volumeProfile.TryGet<Bloom>(out var bloom))
            {
                builder.AppendLine($"| Bloom active | {FormatBool(bloom.active)} |");
                builder.AppendLine($"| Bloom threshold | {FormatVolumeFloat(bloom.threshold)} |");
                builder.AppendLine($"| Bloom intensity | {FormatVolumeFloat(bloom.intensity)} |");
                builder.AppendLine($"| Bloom scatter | {FormatVolumeFloat(bloom.scatter)} |");
                builder.AppendLine($"| Bloom clamp | {FormatVolumeFloat(bloom.clamp)} |");
                builder.AppendLine($"| Bloom high quality filtering | {FormatVolumeBool(bloom.highQualityFiltering)} |");
            }
            else
            {
                builder.AppendLine("| Bloom active | n/a |");
                builder.AppendLine("| Bloom threshold | n/a |");
                builder.AppendLine("| Bloom intensity | n/a |");
                builder.AppendLine("| Bloom scatter | n/a |");
                builder.AppendLine("| Bloom clamp | n/a |");
                builder.AppendLine("| Bloom high quality filtering | n/a |");
            }

            if (volumeProfile.TryGet<ColorAdjustments>(out var colorAdjustments))
            {
                builder.AppendLine($"| ColorAdjustments active | {FormatBool(colorAdjustments.active)} |");
                builder.AppendLine($"| ColorAdjustments postExposure | {FormatVolumeFloat(colorAdjustments.postExposure)} |");
                builder.AppendLine($"| ColorAdjustments contrast | {FormatVolumeFloat(colorAdjustments.contrast)} |");
                builder.AppendLine($"| ColorAdjustments saturation | {FormatVolumeFloat(colorAdjustments.saturation)} |");
            }
            else
            {
                builder.AppendLine("| ColorAdjustments active | n/a |");
                builder.AppendLine("| ColorAdjustments postExposure | n/a |");
                builder.AppendLine("| ColorAdjustments contrast | n/a |");
                builder.AppendLine("| ColorAdjustments saturation | n/a |");
            }

            if (volumeProfile.TryGet<Vignette>(out var vignette))
            {
                builder.AppendLine($"| Vignette active | {FormatBool(vignette.active)} |");
                builder.AppendLine($"| Vignette intensity | {FormatVolumeFloat(vignette.intensity)} |");
                builder.AppendLine($"| Vignette smoothness | {FormatVolumeFloat(vignette.smoothness)} |");
            }
            else
            {
                builder.AppendLine("| Vignette active | n/a |");
                builder.AppendLine("| Vignette intensity | n/a |");
                builder.AppendLine("| Vignette smoothness | n/a |");
            }

            if (volumeProfile.TryGet<Tonemapping>(out var tonemapping))
            {
                builder.AppendLine($"| Tonemapping active | {FormatBool(tonemapping.active)} |");
                builder.AppendLine($"| Tonemapping mode | {FormatEnumValue(tonemapping.mode.value)} |");
            }
            else
            {
                builder.AppendLine("| Tonemapping active | n/a |");
                builder.AppendLine("| Tonemapping mode | n/a |");
            }

            if (volumeProfile.TryGet<ColorLookup>(out var colorLookup))
            {
                builder.AppendLine($"| ColorLookup active | {FormatBool(colorLookup.active)} |");
                builder.AppendLine($"| ColorLookup texture | {FormatVolumeTexture(colorLookup.texture)} |");
                builder.AppendLine($"| ColorLookup contribution | {FormatVolumeFloat(colorLookup.contribution)} |");
            }
            else
            {
                builder.AppendLine("| ColorLookup active | n/a |");
                builder.AppendLine("| ColorLookup texture | n/a |");
                builder.AppendLine("| ColorLookup contribution | n/a |");
            }

            if (volumeProfile.TryGet<ShadowsMidtonesHighlights>(out var shadowsMidtonesHighlights))
            {
                builder.AppendLine($"| ShadowsMidtonesHighlights active | {FormatBool(shadowsMidtonesHighlights.active)} |");
                builder.AppendLine($"| ShadowsMidtonesHighlights shadows | {FormatVector4(shadowsMidtonesHighlights.shadows.value)} |");
                builder.AppendLine($"| ShadowsMidtonesHighlights midtones | {FormatVector4(shadowsMidtonesHighlights.midtones.value)} |");
                builder.AppendLine($"| ShadowsMidtonesHighlights highlights | {FormatVector4(shadowsMidtonesHighlights.highlights.value)} |");
                builder.AppendLine($"| ShadowsMidtonesHighlights shadowsStart | {FormatVolumeFloat(shadowsMidtonesHighlights.shadowsStart)} |");
                builder.AppendLine($"| ShadowsMidtonesHighlights shadowsEnd | {FormatVolumeFloat(shadowsMidtonesHighlights.shadowsEnd)} |");
                builder.AppendLine($"| ShadowsMidtonesHighlights highlightsStart | {FormatVolumeFloat(shadowsMidtonesHighlights.highlightsStart)} |");
                builder.AppendLine($"| ShadowsMidtonesHighlights highlightsEnd | {FormatVolumeFloat(shadowsMidtonesHighlights.highlightsEnd)} |");
            }
            else
            {
                builder.AppendLine("| ShadowsMidtonesHighlights active | n/a |");
                builder.AppendLine("| ShadowsMidtonesHighlights shadows | n/a |");
                builder.AppendLine("| ShadowsMidtonesHighlights midtones | n/a |");
                builder.AppendLine("| ShadowsMidtonesHighlights highlights | n/a |");
                builder.AppendLine("| ShadowsMidtonesHighlights shadowsStart | n/a |");
                builder.AppendLine("| ShadowsMidtonesHighlights shadowsEnd | n/a |");
                builder.AppendLine("| ShadowsMidtonesHighlights highlightsStart | n/a |");
                builder.AppendLine("| ShadowsMidtonesHighlights highlightsEnd | n/a |");
            }

            if (volumeProfile.TryGet<LiftGammaGain>(out var liftGammaGain))
            {
                builder.AppendLine($"| LiftGammaGain active | {FormatBool(liftGammaGain.active)} |");
                builder.AppendLine($"| LiftGammaGain lift | {FormatVector4(liftGammaGain.lift.value)} |");
                builder.AppendLine($"| LiftGammaGain gamma | {FormatVector4(liftGammaGain.gamma.value)} |");
                builder.AppendLine($"| LiftGammaGain gain | {FormatVector4(liftGammaGain.gain.value)} |");
            }
            else
            {
                builder.AppendLine("| LiftGammaGain active | n/a |");
                builder.AppendLine("| LiftGammaGain lift | n/a |");
                builder.AppendLine("| LiftGammaGain gamma | n/a |");
                builder.AppendLine("| LiftGammaGain gain | n/a |");
            }

            if (volumeProfile.TryGet<DepthOfField>(out var depthOfField))
            {
                builder.AppendLine($"| DepthOfField active | {FormatBool(depthOfField.active)} |");
            }
            else
            {
                builder.AppendLine("| DepthOfField active | n/a |");
            }

            if (volumeProfile.TryGet<FilmGrain>(out var filmGrain))
            {
                builder.AppendLine($"| FilmGrain active | {FormatBool(filmGrain.active)} |");
            }
            else
            {
                builder.AppendLine("| FilmGrain active | n/a |");
            }
        }

        private static ScriptableRendererFeature FindRendererFeature(UniversalRendererData rendererData, Type featureType, out int index)
        {
            index = -1;
            if (rendererData == null || rendererData.rendererFeatures == null)
            {
                return null;
            }

            for (var i = 0; i < rendererData.rendererFeatures.Count; i++)
            {
                var feature = rendererData.rendererFeatures[i];
                if (feature != null && featureType.IsInstanceOfType(feature))
                {
                    index = i;
                    return feature;
                }
            }

            return null;
        }

        private static string FormatEnumName(SerializedProperty parent, string fieldName, params string[] fallbackNames)
        {
            if (!TryGetEnumName(parent, fieldName, out var value, fallbackNames))
            {
                return "n/a";
            }

            return FormatText(value);
        }

        private static string FormatEnumValue<T>(T value) where T : Enum
        {
            return value.ToString();
        }

        private static string FormatVolumeFloat(VolumeParameter<float> parameter)
        {
            if (parameter == null || !parameter.overrideState)
            {
                return "n/a";
            }

            return FormatFloat(parameter.value);
        }

        private static string FormatVolumeBool(VolumeParameter<bool> parameter)
        {
            if (parameter == null || !parameter.overrideState)
            {
                return "n/a";
            }

            return FormatBool(parameter.value);
        }

        private static string FormatVolumeTexture(VolumeParameter<Texture> parameter)
        {
            if (parameter == null || !parameter.overrideState || parameter.value == null)
            {
                return "n/a";
            }

            var path = AssetDatabase.GetAssetPath(parameter.value);
            return string.IsNullOrEmpty(path) ? parameter.value.name : path;
        }

        private static string FormatVector4(Vector4 value)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "({0:0.###}, {1:0.###}, {2:0.###}, {3:0.###})",
                value.x,
                value.y,
                value.z,
                value.w);
        }

        private static string FormatVector3(Vector3? value)
        {
            if (!value.HasValue)
            {
                return "n/a";
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "({0:0.###}, {1:0.###}, {2:0.###})",
                value.Value.x,
                value.Value.y,
                value.Value.z);
        }

        private static string FormatVector2(Vector2? value)
        {
            if (!value.HasValue)
            {
                return "n/a";
            }

            return string.Format(
                CultureInfo.InvariantCulture,
                "({0:0.###}, {1:0.###})",
                value.Value.x,
                value.Value.y);
        }

        private static string FormatFloat(float? value)
        {
            return value.HasValue ? value.Value.ToString("0.###", CultureInfo.InvariantCulture) : "n/a";
        }

        private static string FormatInt(int? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.InvariantCulture) : "n/a";
        }

        private static string FormatBool(bool? value)
        {
            return value.HasValue ? (value.Value ? "true" : "false") : "n/a";
        }

        private static string FormatText(string value)
        {
            return string.IsNullOrEmpty(value) ? "n/a" : value;
        }

        private static float? FindFloat(SerializedObject serialized, string fieldName, params string[] fallbackNames)
        {
            if (!TryGetFloat(serialized, fieldName, out var value, fallbackNames))
            {
                return null;
            }

            return value;
        }

        private static float? FindFloat(SerializedProperty parent, string fieldName, params string[] fallbackNames)
        {
            if (!TryGetFloat(parent, fieldName, out var value, fallbackNames))
            {
                return null;
            }

            return value;
        }

        private static Vector3? FindVector3(SerializedObject serialized, string fieldName, params string[] fallbackNames)
        {
            if (!TryGetVector3(serialized, fieldName, out var value, fallbackNames))
            {
                return null;
            }

            return value;
        }

        private static Vector2? FindVector2(SerializedObject serialized, string fieldName, params string[] fallbackNames)
        {
            if (!TryGetVector2(serialized, fieldName, out var value, fallbackNames))
            {
                return null;
            }

            return value;
        }

        private static int? FindInt(SerializedObject serialized, string fieldName, params string[] fallbackNames)
        {
            if (!TryGetInt(serialized, fieldName, out var value, fallbackNames))
            {
                return null;
            }

            return value;
        }

        private static bool? FindBool(SerializedObject serialized, string fieldName, params string[] fallbackNames)
        {
            if (!TryGetBool(serialized, fieldName, out var value, fallbackNames))
            {
                return null;
            }

            return value;
        }

        private static string GetAbsoluteProjectPath(string relativePath)
        {
            return Path.GetFullPath(Path.Combine(ProjectRoot, relativePath));
        }

        private static string ProjectRoot => Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

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

        private static void RequireVector3(SerializedObject serialized, string fieldName, Vector3 expected, float tolerance, List<string> issues, string message, params string[] fallbackNames)
        {
            if (!TryGetVector3(serialized, fieldName, out var actual, fallbackNames))
            {
                issues.Add(message + $" (missing property '{fieldName}').");
                return;
            }

            if (Mathf.Abs(actual.x - expected.x) > tolerance ||
                Mathf.Abs(actual.y - expected.y) > tolerance ||
                Mathf.Abs(actual.z - expected.z) > tolerance)
            {
                issues.Add(message + $" (found {actual} on '{fieldName}').");
            }
        }

        private static void RequireVector2(SerializedObject serialized, string fieldName, Vector2 expected, float tolerance, List<string> issues, string message, params string[] fallbackNames)
        {
            if (!TryGetVector2(serialized, fieldName, out var actual, fallbackNames))
            {
                issues.Add(message + $" (missing property '{fieldName}').");
                return;
            }

            if (Mathf.Abs(actual.x - expected.x) > tolerance ||
                Mathf.Abs(actual.y - expected.y) > tolerance)
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

        private static bool TryGetVector3(SerializedObject serialized, string fieldName, out Vector3 value, params string[] fallbackNames)
        {
            value = default;
            if (!TryFindProperty(serialized, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            value = property.vector3Value;
            return true;
        }

        private static bool TryGetVector2(SerializedObject serialized, string fieldName, out Vector2 value, params string[] fallbackNames)
        {
            value = default;
            if (!TryFindProperty(serialized, fieldName, out var property, fallbackNames))
            {
                return false;
            }

            value = property.vector2Value;
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
