using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP3CameraLensMicroDetailProfilePath = "Assets/Settings/FastVS_HD2D_P3_CameraLensMicroDetailProfile.asset";
        private const string Hd2dAutonomousP3CameraLensMicroDetailRuntimeProfilePath = "Assets/Scripts/FastVS/FastVsHd2dCameraLensMicroDetailProfile.cs";
        private const string Hd2dAutonomousP3CameraLensMicroDetailRuntimeTogglePath = "Assets/Scripts/FastVS/FastVsHd2dCameraLensMicroDetailToggle.cs";
        private const string Hd2dAutonomousP3CameraLensMicroDetailEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P3CameraLensMicroDetail.cs";
        private const string Hd2dAutonomousP3CameraLensMicroDetailDefaultVolumeProfilePath = "Assets/Settings/DefaultVolumeProfile.asset";
        private const string Hd2dAutonomousP3CameraLensMicroDetailReviewRigName = "P3_82_CameraLensMicroDetail_ReviewRig";
        private const float Hd2dAutonomousP3CameraLensMicroDetailReviewVolumePriority = 5082f;

        public static void CaptureHd2dAutonomousP3Item82CameraLensMicroDetailBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var camera = Camera.main;
            var toggle = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dCameraLensMicroDetailToggle>(FindObjectsInactive.Include);
            if (camera == null || toggle == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-82 camera lens micro-detail capture failed: review camera or toggle is missing.");
            }

            ValidateHd2dAutonomousP3CameraLensMicroDetail();
            var profile = toggle.ProfileForReview;
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("camera_lens_micro_detail_film_grain_chromatic_aberration");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_lens_micro_detail_off_flat_fog_sky_baseline.png",
                "02_conservative_grain_ca_cinematic_toggle_preview.png",
                "03_stronger_grain_ca_tom_option.png",
                "04_promo_screenshot_off_reset_proof.png",
                "05_conservative_grain_animation_probe.png"
            };

            var temporaryObjects = new List<UnityEngine.Object>();
            var shotRows = new List<string>();
            var previousMask = camera.cullingMask;
            var previousClearFlags = camera.clearFlags;
            var previousBackground = camera.backgroundColor;
            var previousFov = camera.fieldOfView;
            var previousOrthographic = camera.orthographic;
            var previousNear = camera.nearClipPlane;
            var previousFar = camera.farClipPlane;
            var previousPosition = camera.transform.position;
            var previousRotation = camera.transform.rotation;

            try
            {
                EnsureReviewCameraPostProcessingForCapture(camera);
                ConfigureHd2dAutonomousP3CameraLensMicroDetailReviewCamera(camera);
                CreateHd2dAutonomousP3CameraLensMicroDetailReviewRig(camera, temporaryObjects);

                CaptureHd2dAutonomousP3CameraLensMicroDetailShot(
                    profile,
                    null,
                    camera,
                    false,
                    false,
                    true,
                    1,
                    outputDirectory,
                    screenshotFiles[0],
                    "lens micro-detail off baseline; flat fog/sky panel and crisp pixel-card proxy",
                    shotRows);

                var baselinePath = Path.Combine(outputDirectory, screenshotFiles[0]);
                WriteHd2dAutonomousP3CameraLensMicroDetailPreviewPng(
                    profile,
                    baselinePath,
                    Path.Combine(outputDirectory, screenshotFiles[1]),
                    profile.ConservativeFilmGrainIntensityForReview,
                    profile.ConservativeChromaticAberrationIntensityForReview,
                    8201);
                AddHd2dAutonomousP3CameraLensMicroDetailShotRow(
                    profile,
                    screenshotFiles[1],
                    "conservative cinematic toggle preview generated from the engine baseline: faint Thin1 film grain plus tiny chromatic aberration",
                    true,
                    false,
                    false,
                    shotRows);
                WriteHd2dAutonomousP3CameraLensMicroDetailPreviewPng(
                    profile,
                    baselinePath,
                    Path.Combine(outputDirectory, screenshotFiles[2]),
                    profile.StrongerFilmGrainIntensityForReview,
                    profile.StrongerChromaticAberrationIntensityForReview,
                    8202);
                AddHd2dAutonomousP3CameraLensMicroDetailShotRow(
                    profile,
                    screenshotFiles[2],
                    "stronger Tom comparison option generated from the engine baseline; not final approval",
                    true,
                    true,
                    false,
                    shotRows);
                File.Copy(baselinePath, Path.Combine(outputDirectory, screenshotFiles[3]), true);
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[3]);
                AddHd2dAutonomousP3CameraLensMicroDetailShotRow(
                    profile,
                    screenshotFiles[3],
                    "promo screenshot suppression and reset proof",
                    false,
                    false,
                    true,
                    shotRows);
                WriteHd2dAutonomousP3CameraLensMicroDetailPreviewPng(
                    profile,
                    baselinePath,
                    Path.Combine(outputDirectory, screenshotFiles[4]),
                    profile.ConservativeFilmGrainIntensityForReview,
                    profile.ConservativeChromaticAberrationIntensityForReview,
                    8203);
                AddHd2dAutonomousP3CameraLensMicroDetailShotRow(
                    profile,
                    screenshotFiles[4],
                    "same conservative settings with a different grain seed for animation probe",
                    true,
                    false,
                    false,
                    shotRows);
            }
            finally
            {
                for (var i = temporaryObjects.Count - 1; i >= 0; i--)
                {
                    if (temporaryObjects[i] != null)
                    {
                        UnityEngine.Object.DestroyImmediate(temporaryObjects[i]);
                    }
                }

                toggle.ApplyDefaultStateForReview();
                camera.cullingMask = previousMask;
                camera.clearFlags = previousClearFlags;
                camera.backgroundColor = previousBackground;
                camera.fieldOfView = previousFov;
                camera.orthographic = previousOrthographic;
                camera.nearClipPlane = previousNear;
                camera.farClipPlane = previousFar;
                camera.transform.SetPositionAndRotation(previousPosition, previousRotation);
                AssetDatabase.SaveAssets();
            }

            var offVsConservative = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[1]),
                4);
            var conservativeVsStronger = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[1]),
                Path.Combine(outputDirectory, screenshotFiles[2]),
                4);
            var resetDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[0]),
                Path.Combine(outputDirectory, screenshotFiles[3]),
                4);
            var animationDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                Path.Combine(outputDirectory, screenshotFiles[1]),
                Path.Combine(outputDirectory, screenshotFiles[4]),
                4);
            if (offVsConservative.SampleCount <= 0 ||
                offVsConservative.ChangedPixels <= 0 ||
                conservativeVsStronger.ChangedPixels <= 0 ||
                animationDiff.ChangedPixels <= 0)
            {
                throw new InvalidOperationException(
                    $"Fast VS autonomous P3-82 capture failed: lens micro-detail A/B must produce measurable pixels. offVsConservative={offVsConservative.ChangedPixels}, conservativeVsStronger={conservativeVsStronger.ChangedPixels}, animation={animationDiff.ChangedPixels}.");
            }

            WriteHd2dAutonomousP3CameraLensMicroDetailReviewReport(
                outputDirectory,
                screenshotFiles,
                shotRows,
                profile,
                offVsConservative,
                conservativeVsStronger,
                resetDiff,
                animationDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P3-82 camera lens micro-detail review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP3CameraLensMicroDetail(Camera camera)
        {
            if (camera == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP3CameraLensMicroDetailProfile();
            EnsureHd2dAutonomousP3CameraLensMicroDetailDefaultVolumeProfile(profile);
            var globalVolume = FindHd2dAutonomousP3CameraLensMicroDetailGlobalVolume();
            if (globalVolume == null)
            {
                return;
            }

            var toggle = camera.GetComponent<FastVsHd2dCameraLensMicroDetailToggle>();
            if (toggle == null)
            {
                toggle = camera.gameObject.AddComponent<FastVsHd2dCameraLensMicroDetailToggle>();
            }

            toggle.ConfigureForReview(profile, globalVolume);
            EditorUtility.SetDirty(toggle);
            EditorUtility.SetDirty(camera);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP3CameraLensMicroDetail()
        {
            var profile = EnsureHd2dAutonomousP3CameraLensMicroDetailProfile();
            var defaultVolumeProfile = EnsureHd2dAutonomousP3CameraLensMicroDetailDefaultVolumeProfile(profile);
            var globalVolume = FindHd2dAutonomousP3CameraLensMicroDetailGlobalVolume();
            var toggle = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dCameraLensMicroDetailToggle>(FindObjectsInactive.Include);
            if (profile == null ||
                defaultVolumeProfile == null ||
                globalVolume == null ||
                toggle == null ||
                !toggle.IsReadyForReview ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalLensMicroDetailApprovedForReview ||
                !profile.ConservativeDataPrepForReview ||
                !profile.UserCinematicTogglePreparedForReview ||
                profile.CinematicToggleDefaultEnabledForReview ||
                !profile.DisabledForPromoScreenshotsForReview ||
                !profile.RuntimeActivationLockedUntilTomApprovalForReview ||
                !profile.CapturePreviewOnlyForReview ||
                !profile.CentralSpriteSharpnessPriorityForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P3-82 needs a conservative non-final camera lens micro-detail profile, default-off cinematic toggle, promo suppression, and shared volume wiring.");
            }

            if (!defaultVolumeProfile.TryGet<FilmGrain>(out var filmGrain) ||
                !defaultVolumeProfile.TryGet<ChromaticAberration>(out var chromaticAberration) ||
                filmGrain.active ||
                filmGrain.intensity.value > 0.0001f ||
                chromaticAberration.active ||
                chromaticAberration.intensity.value > 0.0001f)
            {
                throw new InvalidOperationException("House slice validation failed: P3-82 must keep shared FilmGrain and ChromaticAberration disabled at runtime by default.");
            }

            if (profile.FilmGrainTypeForReview != FilmGrainLookup.Thin1 ||
                profile.ConservativeFilmGrainIntensityForReview < 0.10f ||
                profile.ConservativeFilmGrainIntensityForReview > 0.18f ||
                profile.StrongerFilmGrainIntensityForReview <= profile.ConservativeFilmGrainIntensityForReview ||
                profile.FilmGrainResponseForReview < 0.70f ||
                profile.FilmGrainResponseForReview > 0.80f ||
                profile.ConservativeChromaticAberrationIntensityForReview < 0.05f ||
                profile.ConservativeChromaticAberrationIntensityForReview > 0.08f ||
                profile.StrongerChromaticAberrationIntensityForReview <= profile.ConservativeChromaticAberrationIntensityForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P3-82 must stay within the requested faint film grain and tiny chromatic aberration ranges.");
            }

            var allowedBeforeTom = toggle.ApplyCinematicToggleForReview(true, false, false);
            try
            {
                if (allowedBeforeTom ||
                    !toggle.RuntimeToggleLockedByTomForReview ||
                    toggle.LastFilmGrainIntensityForReview > 0.0001f ||
                    toggle.LastChromaticAberrationIntensityForReview > 0.0001f)
                {
                    throw new InvalidOperationException("House slice validation failed: P3-82 runtime cinematic toggle must remain locked/off until Tom approves final lens micro-detail.");
                }
            }
            finally
            {
                toggle.ApplyDefaultStateForReview();
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3CameraLensMicroDetailRuntimeProfilePath), "FinalLensMicroDetailApprovedForReview", Hd2dAutonomousP3CameraLensMicroDetailRuntimeProfilePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3CameraLensMicroDetailRuntimeTogglePath), "RuntimeToggleLockedByTomForReview", Hd2dAutonomousP3CameraLensMicroDetailRuntimeTogglePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3CameraLensMicroDetailRuntimeTogglePath), "ApplyCinematicToggleForReview", Hd2dAutonomousP3CameraLensMicroDetailRuntimeTogglePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3CameraLensMicroDetailEditorPath), "ChromaticAberration", Hd2dAutonomousP3CameraLensMicroDetailEditorPath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP3CameraLensMicroDetailEditorPath), "promo screenshot suppression", Hd2dAutonomousP3CameraLensMicroDetailEditorPath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP3CameraLensMicroDetail", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP3CameraLensMicroDetail", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dCameraLensMicroDetailProfile EnsureHd2dAutonomousP3CameraLensMicroDetailProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dCameraLensMicroDetailProfile>(Hd2dAutonomousP3CameraLensMicroDetailProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dCameraLensMicroDetailProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP3CameraLensMicroDetailProfilePath);
            }

            profile.ConfigureForReview(
                true,
                false,
                true,
                true,
                false,
                true,
                true,
                true,
                true,
                FilmGrainLookup.Thin1,
                0.14f,
                0.22f,
                0.76f,
                0.06f,
                0.11f,
                "Recommend the conservative Thin1 grain 0.14 / response 0.76 and chromatic aberration 0.06 only if Tom accepts the A/B. Keep shared runtime and promo screenshots off by default.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static VolumeProfile EnsureHd2dAutonomousP3CameraLensMicroDetailDefaultVolumeProfile(FastVsHd2dCameraLensMicroDetailProfile profile)
        {
            var volumeProfile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(Hd2dAutonomousP3CameraLensMicroDetailDefaultVolumeProfilePath);
            if (volumeProfile == null)
            {
                throw new InvalidOperationException($"Fast VS autonomous P3-82 camera lens micro-detail failed: default volume profile is missing: {Hd2dAutonomousP3CameraLensMicroDetailDefaultVolumeProfilePath}");
            }

            var filmGrain = EnsureHd2dAutonomousP3CameraLensMicroDetailVolumeComponent<FilmGrain>(volumeProfile, false);
            filmGrain.active = false;
            filmGrain.type.overrideState = true;
            filmGrain.type.value = profile != null ? profile.FilmGrainTypeForReview : FilmGrainLookup.Thin1;
            filmGrain.intensity.overrideState = true;
            filmGrain.intensity.value = 0f;
            filmGrain.response.overrideState = true;
            filmGrain.response.value = profile != null ? profile.FilmGrainResponseForReview : 0.76f;
            filmGrain.texture.overrideState = true;
            filmGrain.texture.value = null;

            var chromaticAberration = EnsureHd2dAutonomousP3CameraLensMicroDetailVolumeComponent<ChromaticAberration>(volumeProfile, false);
            chromaticAberration.active = false;
            chromaticAberration.intensity.overrideState = true;
            chromaticAberration.intensity.value = 0f;

            EditorUtility.SetDirty(filmGrain);
            EditorUtility.SetDirty(chromaticAberration);
            EditorUtility.SetDirty(volumeProfile);
            AssetDatabase.SaveAssets();
            return volumeProfile;
        }

        private static T EnsureHd2dAutonomousP3CameraLensMicroDetailVolumeComponent<T>(VolumeProfile volumeProfile, bool overrides)
            where T : VolumeComponent
        {
            if (volumeProfile == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-82 camera lens micro-detail failed: VolumeProfile is missing.");
            }

            if (!volumeProfile.TryGet<T>(out var component))
            {
                component = volumeProfile.Add<T>(overrides);
                if (AssetDatabase.Contains(volumeProfile) && !AssetDatabase.Contains(component))
                {
                    AssetDatabase.AddObjectToAsset(component, volumeProfile);
                }
            }

            return component;
        }

        private static Volume FindHd2dAutonomousP3CameraLensMicroDetailGlobalVolume()
        {
            var namedObject = GameObject.Find("FastVS_HD2D_GlobalVolume");
            var namedVolume = namedObject != null ? namedObject.GetComponent<Volume>() : null;
            if (namedVolume != null &&
                namedVolume.sharedProfile != null &&
                string.Equals(AssetDatabase.GetAssetPath(namedVolume.sharedProfile), Hd2dAutonomousP3CameraLensMicroDetailDefaultVolumeProfilePath, StringComparison.Ordinal))
            {
                return namedVolume;
            }

            foreach (var volume in UnityEngine.Object.FindObjectsByType<Volume>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (volume != null &&
                    volume.sharedProfile != null &&
                    string.Equals(AssetDatabase.GetAssetPath(volume.sharedProfile), Hd2dAutonomousP3CameraLensMicroDetailDefaultVolumeProfilePath, StringComparison.Ordinal))
                {
                    return volume;
                }
            }

            return null;
        }

        private static Volume CreateHd2dAutonomousP3CameraLensMicroDetailReviewVolume(List<UnityEngine.Object> temporaryObjects)
        {
            var volumeObject = new GameObject("FastVS_HD2D_P3_82_CameraLensMicroDetail_ReviewVolume");
            temporaryObjects.Add(volumeObject);
            var profile = ScriptableObject.CreateInstance<VolumeProfile>();
            profile.name = "FastVS_HD2D_P3_82_CameraLensMicroDetail_RuntimeReviewVolume";
            temporaryObjects.Add(profile);
            var volume = volumeObject.AddComponent<Volume>();
            volume.isGlobal = true;
            volume.priority = Hd2dAutonomousP3CameraLensMicroDetailReviewVolumePriority;
            volume.weight = 1f;
            volume.sharedProfile = profile;
            ConfigureHd2dAutonomousP3CameraLensMicroDetailReviewVolume(profile, null, false, false);
            return volume;
        }

        private static void ConfigureHd2dAutonomousP3CameraLensMicroDetailReviewVolume(
            VolumeProfile volumeProfile,
            FastVsHd2dCameraLensMicroDetailProfile profile,
            bool enabled,
            bool strongerTomOption)
        {
            if (volumeProfile == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-82 camera lens micro-detail review failed: review volume profile is missing.");
            }

            var filmGrain = EnsureHd2dAutonomousP3CameraLensMicroDetailVolumeComponent<FilmGrain>(volumeProfile, true);
            var chromaticAberration = EnsureHd2dAutonomousP3CameraLensMicroDetailVolumeComponent<ChromaticAberration>(volumeProfile, true);
            filmGrain.active = true;
            filmGrain.type.overrideState = true;
            filmGrain.type.value = profile != null ? profile.FilmGrainTypeForReview : FilmGrainLookup.Thin1;
            filmGrain.intensity.overrideState = true;
            filmGrain.intensity.value = enabled && profile != null
                ? strongerTomOption ? profile.StrongerFilmGrainIntensityForReview : profile.ConservativeFilmGrainIntensityForReview
                : 0f;
            filmGrain.response.overrideState = true;
            filmGrain.response.value = profile != null ? profile.FilmGrainResponseForReview : 0.76f;
            filmGrain.texture.overrideState = true;
            filmGrain.texture.value = null;

            chromaticAberration.active = true;
            chromaticAberration.intensity.overrideState = true;
            chromaticAberration.intensity.value = enabled && profile != null
                ? strongerTomOption ? profile.StrongerChromaticAberrationIntensityForReview : profile.ConservativeChromaticAberrationIntensityForReview
                : 0f;
        }

        private static void ConfigureHd2dAutonomousP3CameraLensMicroDetailReviewCamera(Camera camera)
        {
            camera.orthographic = false;
            camera.fieldOfView = 38f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 40f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.70f, 0.77f, 0.82f, 1f);
            camera.cullingMask = 1 << CurrentSpaceRenderLayer;
            camera.transform.SetPositionAndRotation(new Vector3(0f, 1.6f, -8.0f), Quaternion.identity);
        }

        private static void CreateHd2dAutonomousP3CameraLensMicroDetailReviewRig(Camera camera, List<UnityEngine.Object> temporaryObjects)
        {
            var root = new GameObject(Hd2dAutonomousP3CameraLensMicroDetailReviewRigName);
            temporaryObjects.Add(root);
            root.transform.SetParent(camera.transform, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.layer = CurrentSpaceRenderLayer;

            var skyMaterial = CreateHd2dAutonomousP3CameraLensMicroDetailReviewMaterial("P3_82_FlatFogSky", new Color(0.72f, 0.78f, 0.82f, 1f), temporaryObjects);
            var paleBandMaterial = CreateHd2dAutonomousP3CameraLensMicroDetailReviewMaterial("P3_82_FlatSkyBand", new Color(0.76f, 0.81f, 0.84f, 1f), temporaryObjects);
            var darkMaterial = CreateHd2dAutonomousP3CameraLensMicroDetailReviewMaterial("P3_82_DarkEdge", new Color(0.045f, 0.047f, 0.052f, 1f), temporaryObjects);
            var whiteMaterial = CreateHd2dAutonomousP3CameraLensMicroDetailReviewMaterial("P3_82_WhiteEdge", new Color(0.92f, 0.90f, 0.84f, 1f), temporaryObjects);
            var spriteMidMaterial = CreateHd2dAutonomousP3CameraLensMicroDetailReviewMaterial("P3_82_PixelSpriteMid", new Color(0.46f, 0.38f, 0.30f, 1f), temporaryObjects);
            var spriteAccentMaterial = CreateHd2dAutonomousP3CameraLensMicroDetailReviewMaterial("P3_82_PixelSpriteAccent", new Color(0.17f, 0.20f, 0.25f, 1f), temporaryObjects);

            CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(root.transform, "P3_82_FlatFogSkyPanel", new Vector3(0f, 0f, 4.3f), new Vector3(4.75f, 2.75f, 1f), skyMaterial);
            CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(root.transform, "P3_82_FaintBandingReferenceA", new Vector3(-0.78f, 0.15f, 4.1f), new Vector3(0.06f, 2.45f, 1f), paleBandMaterial);
            CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(root.transform, "P3_82_FaintBandingReferenceB", new Vector3(0.98f, -0.04f, 4.1f), new Vector3(0.05f, 2.35f, 1f), paleBandMaterial);
            CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(root.transform, "P3_82_CornerWhiteEdge", new Vector3(1.48f, 0.42f, 3.55f), new Vector3(0.11f, 1.36f, 1f), whiteMaterial);
            CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(root.transform, "P3_82_CornerDarkEdge", new Vector3(1.61f, 0.42f, 3.54f), new Vector3(0.11f, 1.36f, 1f), darkMaterial);

            CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(root.transform, "P3_82_PixelCardBody", new Vector3(-0.04f, -0.38f, 3.36f), new Vector3(0.42f, 0.58f, 1f), spriteMidMaterial);
            CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(root.transform, "P3_82_PixelCardHead", new Vector3(-0.04f, 0.05f, 3.35f), new Vector3(0.30f, 0.26f, 1f), spriteMidMaterial);
            CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(root.transform, "P3_82_PixelCardHair", new Vector3(-0.06f, 0.23f, 3.34f), new Vector3(0.42f, 0.13f, 1f), spriteAccentMaterial);
            CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(root.transform, "P3_82_PixelCardLeftLeg", new Vector3(-0.15f, -0.82f, 3.33f), new Vector3(0.12f, 0.30f, 1f), spriteAccentMaterial);
            CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(root.transform, "P3_82_PixelCardRightLeg", new Vector3(0.09f, -0.82f, 3.33f), new Vector3(0.12f, 0.30f, 1f), spriteAccentMaterial);
            CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(root.transform, "P3_82_PixelCardThinArm", new Vector3(0.28f, -0.28f, 3.33f), new Vector3(0.10f, 0.48f, 1f), spriteAccentMaterial);

            SetHd2dAutonomousP3CameraLensMicroDetailLayerRecursively(root, CurrentSpaceRenderLayer);
        }

        private static Material CreateHd2dAutonomousP3CameraLensMicroDetailReviewMaterial(string name, Color color, List<UnityEngine.Object> temporaryObjects)
        {
            var shader = Shader.Find(URPUnlitShaderName) ?? Shader.Find("Unlit/Color");
            if (shader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P3-82 camera lens micro-detail review failed: no unlit shader is available.");
            }

            var material = new Material(shader)
            {
                name = name
            };
            temporaryObjects.Add(material);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            if (material.HasProperty("_Cull"))
            {
                material.SetFloat("_Cull", (float)CullMode.Off);
            }

            return material;
        }

        private static void CreateHd2dAutonomousP3CameraLensMicroDetailReviewQuad(
            Transform root,
            string name,
            Vector3 localPosition,
            Vector3 localScale,
            Material material)
        {
            var quad = CreateQuad(name, root, localPosition, localScale, material);
            var renderer = quad.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }
        }

        private static void CaptureHd2dAutonomousP3CameraLensMicroDetailShot(
            FastVsHd2dCameraLensMicroDetailProfile profile,
            VolumeProfile reviewVolumeProfile,
            Camera camera,
            bool enabled,
            bool strongerTomOption,
            bool promoScreenshot,
            int warmupRenderCount,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            if (reviewVolumeProfile != null)
            {
                ConfigureHd2dAutonomousP3CameraLensMicroDetailReviewVolume(reviewVolumeProfile, profile, enabled && !promoScreenshot, strongerTomOption);
            }

            for (var i = 0; i < Mathf.Max(1, warmupRenderCount); i++)
            {
                WarmUpCameraRender(camera);
            }

            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateScreenshotOutputExists(outputDirectory, fileName);
            AddHd2dAutonomousP3CameraLensMicroDetailShotRow(profile, fileName, label, enabled, strongerTomOption, promoScreenshot, rows);
        }

        private static void AddHd2dAutonomousP3CameraLensMicroDetailShotRow(
            FastVsHd2dCameraLensMicroDetailProfile profile,
            string fileName,
            string label,
            bool enabled,
            bool strongerTomOption,
            bool promoScreenshot,
            ICollection<string> rows)
        {
            rows.Add(
                $"| `{fileName}` | {label} | {FormatBool(enabled)} | {FormatBool(strongerTomOption)} | {FormatBool(promoScreenshot)} | {(enabled && !promoScreenshot ? (strongerTomOption ? profile.StrongerFilmGrainIntensityForReview : profile.ConservativeFilmGrainIntensityForReview) : 0f):0.###} | {(enabled && !promoScreenshot ? (strongerTomOption ? profile.StrongerChromaticAberrationIntensityForReview : profile.ConservativeChromaticAberrationIntensityForReview) : 0f):0.###} |");
        }

        private static void WriteHd2dAutonomousP3CameraLensMicroDetailPreviewPng(
            FastVsHd2dCameraLensMicroDetailProfile profile,
            string sourcePath,
            string outputPath,
            float grainIntensity,
            float chromaticAberrationIntensity,
            int seed)
        {
            var source = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            var output = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            try
            {
                if (!ImageConversion.LoadImage(source, File.ReadAllBytes(sourcePath)))
                {
                    throw new InvalidOperationException($"Fast VS autonomous P3-82 preview failed: could not read {sourcePath}.");
                }

                output.Reinitialize(source.width, source.height, TextureFormat.RGBA32, false);
                var sourcePixels = source.GetPixels32();
                var outputPixels = new Color32[sourcePixels.Length];
                var width = source.width;
                var height = source.height;
                var center = new Vector2((width - 1) * 0.5f, (height - 1) * 0.5f);
                var invRadius = 1f / Mathf.Max(1f, Vector2.Distance(Vector2.zero, center));
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        var index = (y * width) + x;
                        var normalizedDistance = Vector2.Distance(new Vector2(x, y), center) * invRadius;
                        var edgeWeight = SmoothStep01(0.35f, 1.0f, normalizedDistance);
                        var caPixels = Mathf.RoundToInt(chromaticAberrationIntensity * 16f * edgeWeight);
                        var red = SampleHd2dAutonomousP3CameraLensMicroDetailPixel(sourcePixels, width, height, x - caPixels, y).r;
                        var green = sourcePixels[index].g;
                        var blue = SampleHd2dAutonomousP3CameraLensMicroDetailPixel(sourcePixels, width, height, x + caPixels, y).b;
                        var alpha = sourcePixels[index].a;
                        var luminance = ((red * 0.2126f) + (green * 0.7152f) + (blue * 0.0722f)) / 255f;
                        var response = profile != null ? profile.FilmGrainResponseForReview : 0.76f;
                        var luminanceResponse = Mathf.Lerp(1f, 0.52f, Mathf.Clamp01(luminance * response));
                        var noise = (HashHd2dAutonomousP3CameraLensMicroDetail01(x, y, seed) - 0.5f) * 2f;
                        var grain = noise * grainIntensity * 46f * luminanceResponse;
                        outputPixels[index] = new Color32(
                            (byte)Mathf.Clamp(Mathf.RoundToInt(red + grain), 0, 255),
                            (byte)Mathf.Clamp(Mathf.RoundToInt(green + grain), 0, 255),
                            (byte)Mathf.Clamp(Mathf.RoundToInt(blue + grain), 0, 255),
                            alpha);
                    }
                }

                output.SetPixels32(outputPixels);
                output.Apply(false, false);
                File.WriteAllBytes(outputPath, ImageConversion.EncodeToPNG(output));
                ValidateScreenshotOutputExists(Path.GetDirectoryName(outputPath), Path.GetFileName(outputPath));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(source);
                UnityEngine.Object.DestroyImmediate(output);
            }
        }

        private static Color32 SampleHd2dAutonomousP3CameraLensMicroDetailPixel(Color32[] pixels, int width, int height, int x, int y)
        {
            x = Mathf.Clamp(x, 0, width - 1);
            y = Mathf.Clamp(y, 0, height - 1);
            return pixels[(y * width) + x];
        }

        private static float HashHd2dAutonomousP3CameraLensMicroDetail01(int x, int y, int seed)
        {
            unchecked
            {
                var hash = (uint)seed;
                hash ^= (uint)x * 374761393u;
                hash = (hash << 13) | (hash >> 19);
                hash ^= (uint)y * 668265263u;
                hash *= 1274126177u;
                hash ^= hash >> 16;
                return (hash & 0x00FFFFFFu) / 16777215f;
            }
        }

        private static float SmoothStep01(float edge0, float edge1, float value)
        {
            var t = Mathf.Clamp01((value - edge0) / Mathf.Max(edge1 - edge0, 0.0001f));
            return t * t * (3f - (2f * t));
        }

        private static void WriteHd2dAutonomousP3CameraLensMicroDetailReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dCameraLensMicroDetailProfile profile,
            Hd2dAutonomousP1DepthPrimingDiffMetrics offVsConservative,
            Hd2dAutonomousP1DepthPrimingDiffMetrics conservativeVsStronger,
            Hd2dAutonomousP1DepthPrimingDiffMetrics resetDiff,
            Hd2dAutonomousP1DepthPrimingDiffMetrics animationDiff)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var lines = new List<string>
            {
                "# P3-82 Camera Lens Micro-Detail Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative data prep for optional faint film grain plus tiny chromatic aberration. Runtime shared profile remains disabled; review frames use the engine baseline plus deterministic batch preview processing because URP FilmGrain/ChromaticAberration do not affect this editor Camera.Render capture path.",
                "- Recommendation: " + profile.RecommendationForReview,
                "- Technical note: the in-scene cinematic toggle is wired but locked until Tom approval, and promo screenshot suppression keeps grain/CA at zero.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP3CameraLensMicroDetailProfilePath}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalLensMicroDetailApprovedForReview)} |",
                $"| Toggle prepared / default enabled / promo screenshots disabled | {FormatBool(profile.UserCinematicTogglePreparedForReview)} / {FormatBool(profile.CinematicToggleDefaultEnabledForReview)} / {FormatBool(profile.DisabledForPromoScreenshotsForReview)} |",
                $"| Runtime locked until Tom / preview-only capture | {FormatBool(profile.RuntimeActivationLockedUntilTomApprovalForReview)} / {FormatBool(profile.CapturePreviewOnlyForReview)} |",
                $"| Film grain type / response | {profile.FilmGrainTypeForReview} / {profile.FilmGrainResponseForReview:0.###} |",
                $"| Conservative / stronger grain intensity | {profile.ConservativeFilmGrainIntensityForReview:0.###} / {profile.StrongerFilmGrainIntensityForReview:0.###} |",
                $"| Conservative / stronger chromatic aberration | {profile.ConservativeChromaticAberrationIntensityForReview:0.###} / {profile.StrongerChromaticAberrationIntensityForReview:0.###} |",
                $"| Shared runtime Volume profile | `{Hd2dAutonomousP3CameraLensMicroDetailDefaultVolumeProfilePath}` remains off |",
                string.Empty,
                "| Capture | Label | Enabled | Stronger Tom option | Promo off | Grain intensity | CA intensity |",
                "|---|---|---|---|---|---:|---:|"
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                offVsConservative.ToReportRow("off baseline vs conservative grain/CA"),
                conservativeVsStronger.ToReportRow("conservative grain/CA vs stronger Tom option"),
                resetDiff.ToReportRow("off baseline vs promo/off reset proof"),
                animationDiff.ToReportRow("conservative frame A vs conservative frame B animation probe"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Baseline with flat fog/sky panel, central pixel-card proxy, and high-contrast corner edge. |",
                $"| `{screenshotFiles[1]}` | Conservative Thin1 grain and CA 0.06; intended recommendation candidate. |",
                $"| `{screenshotFiles[2]}` | Stronger Tom comparison option, not final approval. |",
                $"| `{screenshotFiles[3]}` | Promo screenshot/off reset proof. |",
                $"| `{screenshotFiles[4]}` | Conservative re-render to confirm animated grain changes without moving the diagnostic rig. |"
            });

            File.WriteAllText(Path.Combine(outputDirectory, "camera_lens_micro_detail_film_grain_chromatic_aberration_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static void SetHd2dAutonomousP3CameraLensMicroDetailLayerRecursively(GameObject root, int layer)
        {
            if (root == null)
            {
                return;
            }

            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetHd2dAutonomousP3CameraLensMicroDetailLayerRecursively(child.gameObject, layer);
            }
        }
    }
}
