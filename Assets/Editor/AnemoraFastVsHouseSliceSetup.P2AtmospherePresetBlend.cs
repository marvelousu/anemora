using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.FastVS.SunCycle;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2AtmospherePresetBlendRootName = "FastVS_HD2D_P2_AtmospherePresetBlender";
        private const string Hd2dAutonomousP2AtmospherePresetBlendProfilePath = "Assets/Settings/FastVS_HD2D_P2_AtmospherePresetBlendProfile.asset";
        private const string Hd2dAutonomousP2AtmospherePresetBlendRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dAtmospherePresetBlender.cs";
        private const string Hd2dAutonomousP2AtmospherePresetBlendProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dAtmospherePresetBlendProfile.cs";

        public static void CaptureHd2dAutonomousP2Item57TimeOfDayAtmospherePresetBlendBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var sunDriver = sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : null;
            var atmosphericDriver = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAtmosphericPerspectiveDriver>(FindObjectsInactive.Include);
            var shaftField = FindSceneObjectIncludingInactive(Hd2dCycle180DynamicSunShaftFieldName)?.GetComponent<FastVsDynamicSunShaftField>();
            var dustField = FindSceneObjectIncludingInactive(Hd2dAutonomousP0SunShaftDustMoteFieldName)?.GetComponent<FastVsHd2dSunShaftDustMoteField>();
            var ambientDirector = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAmbientVfxDirector>(FindObjectsInactive.Include);
            var ambientDust = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAmbientDustPollenLayer>(FindObjectsInactive.Include);
            var blender = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAtmospherePresetBlender>(FindObjectsInactive.Include);
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || realtimeRig == null || sunDriver == null ||
                atmosphericDriver == null || shaftField == null || dustField == null || ambientDirector == null ||
                ambientDust == null || blender == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-57 ToD atmosphere preset capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2AtmospherePresetBlend();
            var profile = EnsureHd2dAutonomousP2AtmospherePresetBlendProfile();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("tod_atmosphere_preset_blend");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_morning_static_atmosphere_baseline.png",
                "02_morning_preset_blend_enabled.png",
                "03_noon_static_atmosphere_baseline.png",
                "04_noon_preset_blend_enabled.png",
                "05_evening_static_atmosphere_baseline.png",
                "06_evening_preset_blend_enabled.png",
                "07_night_static_atmosphere_baseline.png",
                "08_night_preset_blend_enabled.png",
                "09_morning_to_noon_half_blend_diagnostic.png"
            };
            var shotRows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                CaptureHd2dAutonomousP2AtmospherePresetBlendShot(
                    controller, visibility, guide, realtimeRig, sunDriver, atmosphericDriver, shaftField, dustField,
                    ambientDirector, blender, camera, SunPreset.Morning, false, false, 0f, outputDirectory, screenshotFiles[0],
                    "Morning static P0 atmosphere baseline", shotRows);
                CaptureHd2dAutonomousP2AtmospherePresetBlendShot(
                    controller, visibility, guide, realtimeRig, sunDriver, atmosphericDriver, shaftField, dustField,
                    ambientDirector, blender, camera, SunPreset.Morning, true, false, 0f, outputDirectory, screenshotFiles[1],
                    "Morning warm fog, high low-sun shaft and warmer pollen preset", shotRows);
                CaptureHd2dAutonomousP2AtmospherePresetBlendShot(
                    controller, visibility, guide, realtimeRig, sunDriver, atmosphericDriver, shaftField, dustField,
                    ambientDirector, blender, camera, SunPreset.Noon, false, false, 0f, outputDirectory, screenshotFiles[2],
                    "Noon static P0 atmosphere baseline", shotRows);
                CaptureHd2dAutonomousP2AtmospherePresetBlendShot(
                    controller, visibility, guide, realtimeRig, sunDriver, atmosphericDriver, shaftField, dustField,
                    ambientDirector, blender, camera, SunPreset.Noon, true, false, 0f, outputDirectory, screenshotFiles[3],
                    "Noon cooler, thinner fog and reduced dust preset", shotRows);
                CaptureHd2dAutonomousP2AtmospherePresetBlendShot(
                    controller, visibility, guide, realtimeRig, sunDriver, atmosphericDriver, shaftField, dustField,
                    ambientDirector, blender, camera, SunPreset.Evening, false, false, 0f, outputDirectory, screenshotFiles[4],
                    "Evening static P0 atmosphere baseline", shotRows);
                CaptureHd2dAutonomousP2AtmospherePresetBlendShot(
                    controller, visibility, guide, realtimeRig, sunDriver, atmosphericDriver, shaftField, dustField,
                    ambientDirector, blender, camera, SunPreset.Evening, true, false, 0f, outputDirectory, screenshotFiles[5],
                    "Evening amber fog, strongest low-sun shaft and pollen preset", shotRows);
                CaptureHd2dAutonomousP2AtmospherePresetBlendShot(
                    controller, visibility, guide, realtimeRig, sunDriver, atmosphericDriver, shaftField, dustField,
                    ambientDirector, blender, camera, SunPreset.Night, false, false, 0f, outputDirectory, screenshotFiles[6],
                    "Night static P0 atmosphere baseline", shotRows);
                CaptureHd2dAutonomousP2AtmospherePresetBlendShot(
                    controller, visibility, guide, realtimeRig, sunDriver, atmosphericDriver, shaftField, dustField,
                    ambientDirector, blender, camera, SunPreset.Night, true, false, 0f, outputDirectory, screenshotFiles[7],
                    "Night dense cool fog with shafts and motes suppressed", shotRows);
                CaptureHd2dAutonomousP2AtmospherePresetBlendShot(
                    controller, visibility, guide, realtimeRig, sunDriver, atmosphericDriver, shaftField, dustField,
                    ambientDirector, blender, camera, SunPreset.Noon, true, true, 0.5f, outputDirectory, screenshotFiles[8],
                    "Morning-to-noon half blend diagnostic for smooth interpolation", shotRows);
            }
            finally
            {
                blender.SetReviewActiveForReview(true);
                blender.ClearReviewStateForReview();
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                atmosphericDriver.PublishCurrentForReview();
                AssetDatabase.SaveAssets();
            }

            var morningDiff = MeasureHd2dAutonomousP2AtmospherePresetBlendDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var noonDiff = MeasureHd2dAutonomousP2AtmospherePresetBlendDiff(outputDirectory, screenshotFiles[2], screenshotFiles[3]);
            var eveningDiff = MeasureHd2dAutonomousP2AtmospherePresetBlendDiff(outputDirectory, screenshotFiles[4], screenshotFiles[5]);
            var nightDiff = MeasureHd2dAutonomousP2AtmospherePresetBlendDiff(outputDirectory, screenshotFiles[6], screenshotFiles[7]);
            var morningNoonDiff = MeasureHd2dAutonomousP2AtmospherePresetBlendDiff(outputDirectory, screenshotFiles[1], screenshotFiles[3]);
            var noonEveningDiff = MeasureHd2dAutonomousP2AtmospherePresetBlendDiff(outputDirectory, screenshotFiles[3], screenshotFiles[5]);
            var eveningNightDiff = MeasureHd2dAutonomousP2AtmospherePresetBlendDiff(outputDirectory, screenshotFiles[5], screenshotFiles[7]);
            WriteHd2dAutonomousP2AtmospherePresetBlendReviewReport(
                outputDirectory,
                screenshotFiles,
                shotRows,
                profile,
                morningDiff,
                noonDiff,
                eveningDiff,
                nightDiff,
                morningNoonDiff,
                noonEveningDiff,
                eveningNightDiff);

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-57 ToD atmosphere preset blend review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2AtmospherePresetBlend()
        {
            var profile = EnsureHd2dAutonomousP2AtmospherePresetBlendProfile();
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2AtmospherePresetBlendRootName);
            if (root == null)
            {
                root = new GameObject(Hd2dAutonomousP2AtmospherePresetBlendRootName, typeof(FastVsHd2dAtmospherePresetBlender));
            }
            else if (root.GetComponent<FastVsHd2dAtmospherePresetBlender>() == null)
            {
                root.AddComponent<FastVsHd2dAtmospherePresetBlender>();
            }

            root.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            root.transform.localScale = Vector3.one;
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var blender = root.GetComponent<FastVsHd2dAtmospherePresetBlender>();
            blender.ConfigureForReview(
                profile,
                sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : UnityEngine.Object.FindFirstObjectByType<AnemoraSunCycleDriver>(FindObjectsInactive.Include),
                FindHd2dAutonomousP0AtmosphericPerspectiveDriver(),
                FindSceneObjectIncludingInactive(Hd2dCycle180DynamicSunShaftFieldName)?.GetComponent<FastVsDynamicSunShaftField>(),
                FindSceneObjectIncludingInactive(Hd2dAutonomousP0SunShaftDustMoteFieldName)?.GetComponent<FastVsHd2dSunShaftDustMoteField>(),
                UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAmbientDustPollenLayer>(FindObjectsInactive.Include),
                UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAmbientVfxDirector>(FindObjectsInactive.Include));
            blender.SetReviewActiveForReview(true);

            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(blender);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2AtmospherePresetBlend()
        {
            var profile = EnsureHd2dAutonomousP2AtmospherePresetBlendProfile();
            var blender = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dAtmospherePresetBlender>(FindObjectsInactive.Include);
            if (profile == null ||
                blender == null ||
                blender.ProfileForReview != profile ||
                blender.SunCycleDriverForReview == null ||
                blender.AtmosphericDriverForReview == null ||
                blender.ShaftFieldForReview == null ||
                blender.SunShaftDustMoteFieldForReview == null ||
                blender.AmbientDustLayerForReview == null ||
                !blender.PublishEveryFrameForReview ||
                !blender.ApplyToParticlesForReview ||
                !blender.ConservativeNeedsTomApprovalForReview ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalAtmosphereApprovedForReview ||
                profile.PresetCountForReview != 4)
            {
                throw new InvalidOperationException("House slice validation failed: P2-57 needs one conservative NEEDS-TOM atmosphere preset blender wired to ToD, fog, shafts, sun motes, and ambient dust.");
            }

            foreach (SunPreset preset in Enum.GetValues(typeof(SunPreset)))
            {
                if (!profile.TryResolvePresetForReview(preset, out var values) ||
                    values.FogGradient == null ||
                    values.FogStrength < 0.06f ||
                    values.FogStrength > 0.22f ||
                    values.DistanceEnd <= values.DistanceStart + 4f ||
                    values.HeightStrength <= 0.15f ||
                    values.AerialTintStrength <= 0f ||
                    values.AerialTintStrength > 0.22f)
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-57 preset is missing or outside conservative bounds: {preset}.");
                }
            }

            var morning = profile.ResolvePresetForReview(SunPreset.Morning);
            var noon = profile.ResolvePresetForReview(SunPreset.Noon);
            var evening = profile.ResolvePresetForReview(SunPreset.Evening);
            var night = profile.ResolvePresetForReview(SunPreset.Night);
            if (morning.FogStrength <= noon.FogStrength ||
                evening.FogStrength <= noon.FogStrength ||
                night.FogStrength <= evening.FogStrength ||
                ColorDistance(morning.FogFarColor, noon.FogFarColor) < 0.12f ||
                ColorDistance(evening.FogFarColor, noon.FogFarColor) < 0.18f ||
                ColorDistance(night.FogFarColor, evening.FogFarColor) < 0.25f ||
                morning.ShaftIntensityMultiplier <= noon.ShaftIntensityMultiplier ||
                evening.ShaftIntensityMultiplier <= noon.ShaftIntensityMultiplier ||
                night.ShaftIntensityMultiplier > 0.05f ||
                morning.AmbientDustEmissionMultiplier <= noon.AmbientDustEmissionMultiplier ||
                evening.AmbientDustEmissionMultiplier <= morning.AmbientDustEmissionMultiplier ||
                night.AmbientDustEmissionMultiplier >= noon.AmbientDustEmissionMultiplier)
            {
                throw new InvalidOperationException("House slice validation failed: P2-57 dawn/noon/dusk/night presets must have distinct fog hue/density, shaft strength, and dust rates.");
            }

            blender.ApplyPresetForReview(SunPreset.Evening);
            blender.SimulateForReview(0.15f, true);
            if (!blender.AtmosphericDriverForReview.HasAtmospherePresetOverrideForReview ||
                Mathf.Abs(blender.AtmosphericDriverForReview.OverrideStrengthForReview - evening.FogStrength) > 0.001f ||
                Mathf.Abs(blender.AtmosphericDriverForReview.OverrideAerialRampTintStrengthForReview - evening.AerialTintStrength) > 0.001f ||
                blender.ShaftFieldForReview.AtmospherePresetAlphaMultiplierForReview < 1f ||
                blender.AmbientDustLayerForReview.AppliedEmissionRateForReview <= 0f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-57 blender did not publish the evening preset to fog, aerial tint, shafts, and dust.");
            }

            blender.ApplyBlendForReview(SunPreset.Morning, SunPreset.Noon, 0.5f);
            var halfway = profile.EvaluateBlendForReview(SunPreset.Morning, SunPreset.Noon, 0.5f);
            if (Mathf.Abs(blender.LastFogStrengthForReview - halfway.FogStrength) > 0.01f ||
                Mathf.Abs(blender.LastAmbientDustMultiplierForReview - halfway.AmbientDustEmissionMultiplier) > 0.01f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-57 half-blend diagnostic did not interpolate profile values.");
            }

            blender.ClearReviewStateForReview();

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2AtmospherePresetBlendRuntimePath), "ApplyBlendForReview", Hd2dAutonomousP2AtmospherePresetBlendRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2AtmospherePresetBlendRuntimePath), "SetAtmospherePresetOverrideForReview", Hd2dAutonomousP2AtmospherePresetBlendRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2AtmospherePresetBlendProfileRuntimePath), "AtmospherePreset", Hd2dAutonomousP2AtmospherePresetBlendProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText("Assets/Scripts/FastVS/FastVsHd2dAtmosphericPerspectiveDriver.cs"), "SetAtmospherePresetOverrideForReview", "Assets/Scripts/FastVS/FastVsHd2dAtmosphericPerspectiveDriver.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Scripts/FastVS/FastVsDynamicSunShaftField.cs"), "AtmospherePresetAlphaMultiplierForReview", "Assets/Scripts/FastVS/FastVsDynamicSunShaftField.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2AtmospherePresetBlend", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2AtmospherePresetBlend", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dAtmospherePresetBlendProfile EnsureHd2dAutonomousP2AtmospherePresetBlendProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dAtmospherePresetBlendProfile>(Hd2dAutonomousP2AtmospherePresetBlendProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dAtmospherePresetBlendProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2AtmospherePresetBlendProfilePath);
            }

            var morningNear = new Color(0.98f, 0.82f, 0.60f, 1f);
            var morningFar = new Color(0.78f, 0.58f, 0.46f, 1f);
            var noonNear = new Color(0.80f, 0.86f, 0.88f, 1f);
            var noonFar = new Color(0.54f, 0.66f, 0.78f, 1f);
            var eveningNear = new Color(1.00f, 0.66f, 0.44f, 1f);
            var eveningFar = new Color(0.82f, 0.46f, 0.36f, 1f);
            var nightNear = new Color(0.42f, 0.50f, 0.66f, 1f);
            var nightFar = new Color(0.18f, 0.24f, 0.36f, 1f);

            profile.ConfigureForReview(
                new[]
                {
                    new FastVsHd2dAtmospherePresetBlendProfile.AtmospherePreset(
                        SunPreset.Morning,
                        ResolveHd2dAutonomousP2AtmospherePresetSunElevation(SunPreset.Morning),
                        0.145f,
                        morningNear,
                        morningFar,
                        EnsureHd2dAutonomousP2AtmospherePresetGradientTexture(SunPreset.Morning, morningNear, morningFar),
                        2.8f,
                        12.5f,
                        new Vector2(-0.35f, 3.4f),
                        0.40f,
                        0.17f,
                        new Vector2(0.8f, 5.4f),
                        1.05f,
                        new Color(1.00f, 0.86f, 0.64f, 1f),
                        1.15f,
                        0.68f,
                        1.20f),
                    new FastVsHd2dAtmospherePresetBlendProfile.AtmospherePreset(
                        SunPreset.Noon,
                        ResolveHd2dAutonomousP2AtmospherePresetSunElevation(SunPreset.Noon),
                        0.075f,
                        noonNear,
                        noonFar,
                        EnsureHd2dAutonomousP2AtmospherePresetGradientTexture(SunPreset.Noon, noonNear, noonFar),
                        5.0f,
                        15.0f,
                        new Vector2(-0.25f, 3.0f),
                        0.22f,
                        0.10f,
                        new Vector2(1.4f, 5.8f),
                        0.38f,
                        new Color(0.86f, 0.90f, 0.96f, 1f),
                        0.70f,
                        0.32f,
                        0.28f),
                    new FastVsHd2dAtmospherePresetBlendProfile.AtmospherePreset(
                        SunPreset.Evening,
                        ResolveHd2dAutonomousP2AtmospherePresetSunElevation(SunPreset.Evening),
                        0.170f,
                        eveningNear,
                        eveningFar,
                        EnsureHd2dAutonomousP2AtmospherePresetGradientTexture(SunPreset.Evening, eveningNear, eveningFar),
                        2.6f,
                        12.0f,
                        new Vector2(-0.45f, 3.6f),
                        0.46f,
                        0.18f,
                        new Vector2(0.7f, 5.2f),
                        1.18f,
                        new Color(1.00f, 0.74f, 0.48f, 1f),
                        1.32f,
                        0.82f,
                        1.42f),
                    new FastVsHd2dAtmospherePresetBlendProfile.AtmospherePreset(
                        SunPreset.Night,
                        ResolveHd2dAutonomousP2AtmospherePresetSunElevation(SunPreset.Night),
                        0.205f,
                        nightNear,
                        nightFar,
                        EnsureHd2dAutonomousP2AtmospherePresetGradientTexture(SunPreset.Night, nightNear, nightFar),
                        2.4f,
                        10.2f,
                        new Vector2(-0.60f, 3.8f),
                        0.52f,
                        0.15f,
                        new Vector2(0.6f, 4.6f),
                        0.00f,
                        new Color(0.48f, 0.58f, 0.82f, 1f),
                        0.42f,
                        0.12f,
                        0.00f)
                },
                1.8f,
                true,
                true,
                false,
                "Keep the P2-57 profile as conservative data prep. Recommendation: use the authored dawn/noon/dusk/night deltas as the starting point, but Tom should tune final fog hue, shaft alpha, and dust density after camera/grade sign-off.");

            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static Texture2D EnsureHd2dAutonomousP2AtmospherePresetGradientTexture(SunPreset preset, Color nearColor, Color farColor)
        {
            EnsureFolder(TextureDirectory);
            var texturePath = $"{TextureDirectory}/FastVS_House_hd2d_p2_atmosphere_preset_{preset.ToString().ToLowerInvariant()}_gradient.asset";
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            if (texture == null)
            {
                texture = new Texture2D(64, 1, TextureFormat.RGBA32, false, true)
                {
                    name = Path.GetFileNameWithoutExtension(texturePath),
                    filterMode = FilterMode.Bilinear,
                    wrapMode = TextureWrapMode.Clamp
                };
                AssetDatabase.CreateAsset(texture, texturePath);
            }
            else if (texture.width != 64 || texture.height != 1)
            {
                texture.Reinitialize(64, 1, TextureFormat.RGBA32, false);
            }

            for (var x = 0; x < 64; x++)
            {
                var t = x / 63f;
                var color = Color.Lerp(nearColor, farColor, Mathf.SmoothStep(0f, 1f, t));
                var luma = CalculateLuminance(color);
                color = Color.Lerp(color, new Color(luma, luma, luma, 1f), t * 0.10f);
                color.a = 1f;
                texture.SetPixel(x, 0, color);
            }

            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply(false, false);
            EditorUtility.SetDirty(texture);
            return texture;
        }

        private static float ResolveHd2dAutonomousP2AtmospherePresetSunElevation(SunPreset preset)
        {
            var asset = AssetDatabase.LoadAssetAtPath<SunPresetData>(GetHd2dPhaseASunPresetAssetPath(preset));
            return asset != null ? asset.directionEuler.x : AnemoraSunCycleDriver.GetReferenceDirectionEuler(preset).x;
        }

        private static void CaptureHd2dAutonomousP2AtmospherePresetBlendShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            AnemoraSunCycleDriver sunDriver,
            FastVsHd2dAtmosphericPerspectiveDriver atmosphericDriver,
            FastVsDynamicSunShaftField shaftField,
            FastVsHd2dSunShaftDustMoteField dustField,
            FastVsHd2dAmbientVfxDirector ambientDirector,
            FastVsHd2dAtmospherePresetBlender blender,
            Camera camera,
            SunPreset preset,
            bool presetBlendEnabled,
            bool halfBlendDiagnostic,
            float halfBlendT,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(0.10f, 0.02f, 2.82f));
            guide.ApplyActiveTimeIsolationForReview();
            sunDriver.ApplyPreset(preset, true);
            realtimeRig.ApplyNowForReview();
            ambientDirector.ApplyReviewStateForReview(
                Hd2dAutonomousP1AmbientVfxCentralZoneId,
                preset,
                new Vector3(0.86f, 0f, 0.50f),
                0.58f,
                0.18f,
                0.24f);
            ambientDirector.SimulateForReview(7.5f, true);

            if (presetBlendEnabled)
            {
                blender.SetReviewActiveForReview(true);
                if (halfBlendDiagnostic)
                {
                    blender.ApplyBlendForReview(SunPreset.Morning, SunPreset.Noon, halfBlendT);
                }
                else
                {
                    blender.ApplyPresetForReview(preset);
                }

                blender.SimulateForReview(7.5f, true);
            }
            else
            {
                blender.SetReviewActiveForReview(false);
                atmosphericDriver.ClearAtmospherePresetOverrideForReview();
                atmosphericDriver.PublishCurrentForReview();
                shaftField.ClearAtmospherePresetOverrideForReview();
                dustField.ClearReviewOverrideForReview();
            }

            shaftField.ApplyNowForReview();
            dustField.SimulateForReview(7.5f, true);
            camera.orthographic = false;
            camera.fieldOfView = 42f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 180f;
            PositionChapter1AllMapsCamera(
                camera,
                controller.CurrentSpaceRootForReview.TransformPoint(CentralPlazaVsCenter + new Vector3(0.00f, 0.02f, 3.22f)),
                new Vector3(0.16f, 11.2f, -13.0f),
                new Vector3(0.00f, 1.90f, 0.18f));
            ApplyStage7BokehFocusForReview(camera);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);

            var atmosphere = EnsureHd2dAutonomousP2AtmospherePresetBlendProfile().ResolvePresetForReview(preset);
            if (halfBlendDiagnostic)
            {
                atmosphere = EnsureHd2dAutonomousP2AtmospherePresetBlendProfile().EvaluateBlendForReview(SunPreset.Morning, SunPreset.Noon, halfBlendT);
            }

            rows?.Add(
                $"| `{fileName}` | {label} | {preset} | {FormatBool(presetBlendEnabled)} | {atmosphere.FogStrength:0.###} | {FormatColor(atmosphere.FogFarColor)} | {atmosphere.ShaftIntensityMultiplier:0.###} | {atmosphere.AmbientDustEmissionMultiplier:0.###} | {atmosphere.SunMoteEmissionMultiplier:0.###} | {blender.LastBlendTForReview:0.###} |");
        }

        private static void WriteHd2dAutonomousP2AtmospherePresetBlendReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dAtmospherePresetBlendProfile profile,
            Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics morningDiff,
            Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics noonDiff,
            Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics eveningDiff,
            Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics nightDiff,
            Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics morningNoonDiff,
            Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics noonEveningDiff,
            Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics eveningNightDiff)
        {
            var lines = new List<string>
            {
                "# P2-57 Time-of-Day Atmosphere Preset Blend Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative data prep for dawn/noon/dusk/night fog hue/density, aerial tint, shaft intensity, ambient dust, and sun-mote spawn from the existing sun cycle.",
                $"- Recommendation: {profile.RecommendationForReview}",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2AtmospherePresetBlendProfilePath}` |",
                $"| Runtime manager | `{Hd2dAutonomousP2AtmospherePresetBlendRootName}` |",
                $"| Publish every frame / transition seconds | {FormatBool(profile.PublishEveryFrameForReview)} / {profile.TransitionSecondsForReview:0.###} |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalAtmosphereApprovedForReview)} |",
                string.Empty,
                "| Preset | Sun elev | Fog strength | Fog far | Distance | Height strength | Aerial | Shaft x | Ambient dust x | Sun motes x | Gradient |",
                "|---|---:|---:|---|---|---:|---:|---:|---:|---:|---|"
            };

            foreach (SunPreset preset in Enum.GetValues(typeof(SunPreset)))
            {
                var values = profile.ResolvePresetForReview(preset);
                lines.Add(
                    $"| {preset} | {values.SunElevationDegrees:0.###} | {values.FogStrength:0.###} | {FormatColor(values.FogFarColor)} | {values.DistanceStart:0.##}-{values.DistanceEnd:0.##} | {values.HeightStrength:0.###} | {values.AerialTintStrength:0.###} | {values.ShaftIntensityMultiplier:0.###} | {values.AmbientDustEmissionMultiplier:0.###} | {values.SunMoteEmissionMultiplier:0.###} | `{AssetDatabase.GetAssetPath(values.FogGradient)}` |");
            }

            lines.AddRange(new[]
            {
                string.Empty,
                "| A/B or Preset Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                morningDiff.ToReportRow("Morning static baseline vs preset blend"),
                noonDiff.ToReportRow("Noon static baseline vs preset blend"),
                eveningDiff.ToReportRow("Evening static baseline vs preset blend"),
                nightDiff.ToReportRow("Night static baseline vs preset blend"),
                morningNoonDiff.ToReportRow("Morning preset vs noon preset"),
                noonEveningDiff.ToReportRow("Noon preset vs evening preset"),
                eveningNightDiff.ToReportRow("Evening preset vs night preset"),
                string.Empty,
                "| Screenshot | Label | Preset | Preset Blend | Fog | Far Color | Shaft x | Dust x | Motes x | Blend t |",
                "|---|---|---|---|---:|---|---:|---:|---:|---:|"
            });
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|"
            });

            for (var i = 0; i < screenshotFiles.Count; i++)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[i]);
                lines.Add($"| `{screenshotFiles[i]}` | P2-57 review capture {i + 1} |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "tod_atmosphere_preset_blend_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics MeasureHd2dAutonomousP2AtmospherePresetBlendDiff(string outputDirectory, string firstFile, string secondFile)
        {
            var firstPath = Path.Combine(outputDirectory, firstFile);
            var secondPath = Path.Combine(outputDirectory, secondFile);
            var firstTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            var secondTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            try
            {
                if (!firstTexture.LoadImage(File.ReadAllBytes(firstPath)) || !secondTexture.LoadImage(File.ReadAllBytes(secondPath)))
                {
                    return new Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics(0, 0, 0f, 0f);
                }

                var firstPixels = firstTexture.GetPixels32();
                var secondPixels = secondTexture.GetPixels32();
                var sampleCount = Mathf.Min(firstPixels.Length, secondPixels.Length);
                var changedPixels = 0;
                var totalDelta = 0f;
                for (var i = 0; i < sampleCount; i++)
                {
                    var delta =
                        Mathf.Abs(firstPixels[i].r - secondPixels[i].r) +
                        Mathf.Abs(firstPixels[i].g - secondPixels[i].g) +
                        Mathf.Abs(firstPixels[i].b - secondPixels[i].b);
                    totalDelta += delta / 3f;
                    if (delta > 4)
                    {
                        changedPixels++;
                    }
                }

                return new Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics(
                    sampleCount,
                    changedPixels,
                    sampleCount > 0 ? changedPixels * 100f / sampleCount : 0f,
                    sampleCount > 0 ? totalDelta / sampleCount : 0f);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(firstTexture);
                UnityEngine.Object.DestroyImmediate(secondTexture);
            }
        }

        private readonly struct Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics
        {
            public readonly int SampleCount;
            public readonly int ChangedPixels;
            public readonly float ChangedPercent;
            public readonly float MeanRgbDelta;

            public Hd2dAutonomousP2AtmospherePresetBlendDiffMetrics(int sampleCount, int changedPixels, float changedPercent, float meanRgbDelta)
            {
                SampleCount = sampleCount;
                ChangedPixels = changedPixels;
                ChangedPercent = changedPercent;
                MeanRgbDelta = meanRgbDelta;
            }

            public string ToReportRow(string label)
            {
                return $"| {label} | {SampleCount} | {ChangedPixels} | {ChangedPercent:0.###} | {MeanRgbDelta:0.###} |";
            }
        }
    }
}
