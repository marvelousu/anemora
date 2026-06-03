using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const float Hd2dAutonomousP2SoftBlobLowSunPitch = 18f;
        private const float Hd2dAutonomousP2SoftBlobLowSunYaw = 142f;
        private const string Hd2dAutonomousP2SoftBlobRuntimeRigPath = "Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs";

        public static void CaptureHd2dAutonomousP2Item52SoftBlobContactShadowBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var mainLight = GameObject.Find("Directional Light")?.GetComponent<Light>();
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || mainLight == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-52 soft blob contact shadow capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP2SoftBlobContactShadows();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("soft_blob_contact_shadow");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_low_sun_directional_only_no_blob.png",
                "02_low_sun_blob_on_directional_raked.png",
                "03_blob_only_directional_off_grounding.png",
                "04_library_reto_static_blob_guard.png"
            };

            var contactStates = CaptureHd2dAutonomousP1DirectionalShadowReviewStates(FastVsHd2dOverlayKind.CharacterContactShadow);
            var footStates = CaptureHd2dAutonomousP1DirectionalShadowReviewStates(FastVsHd2dOverlayKind.CharacterFootContact);
            var directionalStates = CaptureHd2dAutonomousP1DirectionalShadowReviewStates(FastVsHd2dOverlayKind.CharacterDirectionalCastShadow);
            var originalLightRotation = mainLight.transform.rotation;

            try
            {
                guide.SetMovementFrozen(true);
                mainLight.transform.rotation = Quaternion.Euler(Hd2dAutonomousP2SoftBlobLowSunPitch, Hd2dAutonomousP2SoftBlobLowSunYaw, 0f);

                PrepareHd2dAutonomousP2SoftBlobContactShadowShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(-0.22f, 0.02f, 3.46f),
                    CentralPlazaVsCenter + new Vector3(-0.14f, 0.58f, 3.82f),
                    new Vector3(0.48f, 1.05f, -2.04f),
                    new Vector3(0.02f, 0.12f, 0.12f),
                    30f);
                SetHd2dAutonomousP2SoftBlobOverlayEnabled(contactStates, false);
                SetHd2dAutonomousP2SoftBlobOverlayEnabled(footStates, false);
                SetHd2dAutonomousP2OverlayEnabled(directionalStates, true);
                SaveHd2dAutonomousP2SoftBlobContactShadowShot(camera, outputDirectory, screenshotFiles[0]);

                PrepareHd2dAutonomousP2SoftBlobContactShadowShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(-0.22f, 0.02f, 3.46f),
                    CentralPlazaVsCenter + new Vector3(-0.14f, 0.58f, 3.82f),
                    new Vector3(0.48f, 1.05f, -2.04f),
                    new Vector3(0.02f, 0.12f, 0.12f),
                    30f);
                SetHd2dAutonomousP2SoftBlobOverlayEnabled(contactStates, true);
                SetHd2dAutonomousP2SoftBlobOverlayEnabled(footStates, true);
                SetHd2dAutonomousP2OverlayEnabled(directionalStates, true);
                SaveHd2dAutonomousP2SoftBlobContactShadowShot(camera, outputDirectory, screenshotFiles[1]);

                PrepareHd2dAutonomousP2SoftBlobContactShadowShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(-0.22f, 0.02f, 3.46f),
                    CentralPlazaVsCenter + new Vector3(-0.14f, 0.58f, 3.82f),
                    new Vector3(0.48f, 1.05f, -2.04f),
                    new Vector3(0.02f, 0.12f, 0.12f),
                    30f);
                SetHd2dAutonomousP2SoftBlobOverlayEnabled(contactStates, true);
                SetHd2dAutonomousP2SoftBlobOverlayEnabled(footStates, true);
                SetHd2dAutonomousP2OverlayEnabled(directionalStates, false);
                SaveHd2dAutonomousP2SoftBlobContactShadowShot(camera, outputDirectory, screenshotFiles[2]);

                PrepareHd2dAutonomousP2SoftBlobContactShadowShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.Library,
                    RetoLibraryDeskLocalPosition + new Vector3(-0.72f, 0.02f, -1.16f),
                    RetoLibraryDeskLocalPosition + new Vector3(0.08f, 0.48f, 0.02f),
                    new Vector3(0.50f, 1.16f, -2.22f),
                    new Vector3(0.00f, 0.08f, 0.12f),
                    28f);
                SetHd2dAutonomousP2SoftBlobOverlayEnabled(contactStates, true);
                SetHd2dAutonomousP2SoftBlobOverlayEnabled(footStates, true);
                SetHd2dAutonomousP2OverlayEnabled(directionalStates, false);
                SaveHd2dAutonomousP2SoftBlobContactShadowShot(camera, outputDirectory, screenshotFiles[3]);

                var stats = AnalyzeHd2dAutonomousP2SoftBlobContactShadowStats();
                var blobDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                    Path.Combine(outputDirectory, screenshotFiles[0]),
                    Path.Combine(outputDirectory, screenshotFiles[1]),
                    4);
                var directionalOffDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                    Path.Combine(outputDirectory, screenshotFiles[1]),
                    Path.Combine(outputDirectory, screenshotFiles[2]),
                    4);
                WriteHd2dAutonomousP2SoftBlobContactShadowReviewReport(outputDirectory, screenshotFiles, stats, blobDiff, directionalOffDiff);
            }
            finally
            {
                mainLight.transform.rotation = originalLightRotation;
                RestoreHd2dAutonomousP1DirectionalShadowReviewStates(contactStates);
                RestoreHd2dAutonomousP1DirectionalShadowReviewStates(footStates);
                RestoreHd2dAutonomousP1DirectionalShadowReviewStates(directionalStates);
                realtimeRig.ApplyNowForReview();
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-52 soft blob contact shadow review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2SoftBlobContactShadows()
        {
            EnsureNiroContactShadowMaterial();
            EnsureRetoContactShadowMaterial();
            EnsureAriaContactShadowMaterial();
            EnsureCharacterContactShadowMaterial("chapter1_runtime_character_contact_shadow");
            EnsureCharacterContactShadowTexture();
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2SoftBlobContactShadows()
        {
            ValidateCharacterContactShadowTextureMetrics("P2-52 independent soft-blob contact shadows");
            ValidateCharacterContactShadowObject("FastVS_PlayerContactShadow_Niro", "FastVS_Player_NiroHouseSlice");
            ValidateCharacterFootContactShadowObject(
                "FastVS_PlayerFootContact_Niro",
                "FastVS_Player_NiroHouseSlice",
                new Vector3(0f, 0.027f, -0.055f),
                new Vector3(0.36f, 0.080f, 1f));
            ValidateCharacterContactShadowObject("Current_Library_Reto_ContactShadow", "Current_LibraryMap_SeparateSpace");
            ValidateCharacterFootContactShadowObject(
                "Current_Library_Reto_FootContact",
                "Current_LibraryMap_SeparateSpace",
                RetoLibraryDeskLocalPosition + new Vector3(0.02f, 0.041f, -0.045f),
                new Vector3(0.34f, 0.078f, 1f));
            ValidateCharacterContactShadowObject("Past_Library_Aria_ContactShadow", "Past_LibraryMap_SeparateSpace");
            ValidateCharacterFootContactShadowObject(
                "Past_Library_Aria_FootContact",
                "Past_LibraryMap_SeparateSpace",
                PastLibraryPersonCueLocalPosition + new Vector3(-0.02f, 0.041f, -0.040f),
                new Vector3(0.34f, 0.078f, 1f));

            ValidateSourceToken(
                File.Exists(Hd2dAutonomousP2SoftBlobRuntimeRigPath) ? File.ReadAllText(Hd2dAutonomousP2SoftBlobRuntimeRigPath) : string.Empty,
                "ApplyIndependentCharacterContactOverlayTransform",
                Hd2dAutonomousP2SoftBlobRuntimeRigPath);

            var stats = AnalyzeHd2dAutonomousP2SoftBlobContactShadowStats();
            if (stats.ContactOverlayCount < 3 ||
                stats.FootOverlayCount < 3 ||
                stats.ContactMaterialRoleCount != stats.ContactOverlayCount + stats.FootOverlayCount ||
                !stats.NiroContactAnchored ||
                !stats.NiroFootAnchored ||
                !stats.ContactRotationIndependent ||
                !stats.ContactScaleIndependent ||
                !stats.DirectionalRotationFollowsLowSun ||
                !stats.DirectionalScaleFollowsLowSun ||
                stats.DirectionalOverlayCount < 3)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P2-52 independent soft-blob contact shadows are not ready. " +
                    $"contacts={stats.ContactOverlayCount}, feet={stats.FootOverlayCount}, contactRole={stats.ContactMaterialRoleCount}, " +
                    $"directionals={stats.DirectionalOverlayCount}, niroContactAnchored={stats.NiroContactAnchored}, niroFootAnchored={stats.NiroFootAnchored}, " +
                    $"rotationIndependent={stats.ContactRotationIndependent}, scaleIndependent={stats.ContactScaleIndependent}, " +
                    $"directionalRotationDelta={stats.NiroDirectionalRotationDelta:0.###}, expectedDirectionalYaw={stats.ExpectedDirectionalYaw:0.#}, " +
                    $"directionalLength={stats.NiroDirectionalLength:0.###}, baseLength={stats.NiroDirectionalBaseLength:0.###}.");
            }
        }

        private static Hd2dAutonomousP2SoftBlobContactShadowStats AnalyzeHd2dAutonomousP2SoftBlobContactShadowStats()
        {
            var stats = new Hd2dAutonomousP2SoftBlobContactShadowStats
            {
                ContactRotationIndependent = true,
                ContactScaleIndependent = true
            };

            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var mainLight = GameObject.Find("Directional Light")?.GetComponent<Light>();
            var originalLightRotation = mainLight != null ? mainLight.transform.rotation : Quaternion.identity;
            var contactStates = CaptureHd2dAutonomousP1DirectionalShadowReviewStates(FastVsHd2dOverlayKind.CharacterContactShadow);
            var footStates = CaptureHd2dAutonomousP1DirectionalShadowReviewStates(FastVsHd2dOverlayKind.CharacterFootContact);
            var directionalStates = CaptureHd2dAutonomousP1DirectionalShadowReviewStates(FastVsHd2dOverlayKind.CharacterDirectionalCastShadow);
            try
            {
                if (visibility != null)
                {
                    visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                }

                if (mainLight != null)
                {
                    mainLight.transform.rotation = Quaternion.Euler(Hd2dAutonomousP2SoftBlobLowSunPitch, Hd2dAutonomousP2SoftBlobLowSunYaw, 0f);
                }

                realtimeRig?.ApplyNowForReview();
                ApplyHd2dAutonomousP2SoftBlobLowSunOverlaysForReview(mainLight);

                var overlays = UnityEngine.Object.FindObjectsByType<FastVsHd2dOverlayProfile>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                for (var i = 0; i < overlays.Length; i++)
                {
                    var overlay = overlays[i];
                    if (overlay == null)
                    {
                        continue;
                    }

                    if (overlay.OverlayKindForReview == FastVsHd2dOverlayKind.CharacterDirectionalCastShadow)
                    {
                        stats.DirectionalOverlayCount++;
                        continue;
                    }

                    if (overlay.OverlayKindForReview != FastVsHd2dOverlayKind.CharacterContactShadow &&
                        overlay.OverlayKindForReview != FastVsHd2dOverlayKind.CharacterFootContact)
                    {
                        continue;
                    }

                    if (overlay.OverlayKindForReview == FastVsHd2dOverlayKind.CharacterContactShadow)
                    {
                        stats.ContactOverlayCount++;
                    }
                    else
                    {
                        stats.FootOverlayCount++;
                    }

                    var renderer = overlay.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        if (renderer.enabled)
                        {
                            stats.EnabledOverlayCount++;
                        }

                        if (renderer.shadowCastingMode == ShadowCastingMode.Off && !renderer.receiveShadows)
                        {
                            stats.NonCastingOverlayCount++;
                        }

                        var materialRole = renderer.sharedMaterial != null
                            ? renderer.sharedMaterial.GetTag(MaterialRoleTagName, false, string.Empty)
                            : string.Empty;
                        if (string.Equals(materialRole, FastVsHd2dMaterialRole.ContactShadow.ToString(), StringComparison.Ordinal))
                        {
                            stats.ContactMaterialRoleCount++;
                        }
                    }

                    if (overlay.IsDynamicSubjectForReview)
                    {
                        stats.DynamicOverlayCount++;
                    }

                    if (Quaternion.Angle(overlay.transform.localRotation, Quaternion.Euler(90f, 0f, 0f)) > 1.5f)
                    {
                        stats.ContactRotationIndependent = false;
                    }

                    var footprint = overlay.FootprintWorldSizeForReview;
                    if (footprint.x > 0.001f && footprint.y > 0.001f)
                    {
                        if (Mathf.Abs(Mathf.Abs(overlay.transform.localScale.x) - footprint.x) > 0.035f ||
                            Mathf.Abs(Mathf.Abs(overlay.transform.localScale.y) - footprint.y) > 0.035f)
                        {
                            stats.ContactScaleIndependent = false;
                        }
                    }
                }

                var niroContact = FindSceneObjectIncludingInactive("FastVS_PlayerContactShadow_Niro");
                var niroFoot = FindSceneObjectIncludingInactive("FastVS_PlayerFootContact_Niro");
                stats.NiroContactAnchored = IsHd2dAutonomousP2SoftBlobAnchoredNearFeet(niroContact, 0.075f);
                stats.NiroFootAnchored = IsHd2dAutonomousP2SoftBlobAnchoredNearFeet(niroFoot, 0.085f);
                stats.NiroContactYaw = niroContact != null ? niroContact.transform.localEulerAngles.z : -1f;
                var niroDirectional = FindSceneObjectIncludingInactive("FastVS_PlayerDirectionalCastShadow_Niro");
                stats.NiroDirectionalYaw = niroDirectional != null ? niroDirectional.transform.localEulerAngles.z : -1f;
                stats.ExpectedDirectionalYaw = mainLight != null
                    ? FastVsRealtimeLightShadowRig.GetP1ContactHardeningShadowYawDegreesForReview(mainLight)
                    : -1f;
                if (niroDirectional != null)
                {
                    var directionalProfile = niroDirectional.GetComponent<FastVsHd2dOverlayProfile>();
                    stats.NiroDirectionalLength = Mathf.Abs(niroDirectional.transform.localScale.x);
                    stats.NiroDirectionalBaseLength = directionalProfile != null
                        ? directionalProfile.FootprintWorldSizeForReview.x
                        : 0f;
                    stats.NiroDirectionalRotationDelta = Quaternion.Angle(
                        niroDirectional.transform.localRotation,
                        Quaternion.Euler(90f, 0f, stats.ExpectedDirectionalYaw));
                    stats.DirectionalRotationFollowsLowSun =
                        stats.NiroDirectionalRotationDelta <= 1.5f;
                    stats.DirectionalScaleFollowsLowSun =
                        stats.NiroDirectionalBaseLength > 0.001f &&
                        stats.NiroDirectionalLength > stats.NiroDirectionalBaseLength + 0.05f;
                }

                stats.LowSunPitch = Hd2dAutonomousP2SoftBlobLowSunPitch;
                stats.LowSunYaw = Hd2dAutonomousP2SoftBlobLowSunYaw;
            }
            finally
            {
                if (mainLight != null)
                {
                    mainLight.transform.rotation = originalLightRotation;
                }

                realtimeRig?.ApplyNowForReview();
                RestoreHd2dAutonomousP1DirectionalShadowReviewStates(contactStates);
                RestoreHd2dAutonomousP1DirectionalShadowReviewStates(footStates);
                RestoreHd2dAutonomousP1DirectionalShadowReviewStates(directionalStates);
            }

            return stats;
        }

        private static bool IsHd2dAutonomousP2SoftBlobAnchoredNearFeet(GameObject shadow, float maxPlanarOffset)
        {
            if (shadow == null || shadow.transform.parent == null)
            {
                return false;
            }

            var p = shadow.transform.localPosition;
            return Mathf.Abs(p.x) <= maxPlanarOffset && Mathf.Abs(p.z) <= maxPlanarOffset;
        }

        private static void PrepareHd2dAutonomousP2SoftBlobContactShadowShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            FastVsHouseArea area,
            Vector3 playerLocal,
            Vector3 anchorLocal,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView)
        {
            visibility.SetActiveAreaForReview(area);
            controller.ForcePlayerCurrentLocalForReview(playerLocal);
            guide.ApplyActiveTimeIsolationForReview();
            realtimeRig.ApplyNowForReview();
            ApplyHd2dAutonomousP2SoftBlobLowSunOverlaysForReview(GameObject.Find("Directional Light")?.GetComponent<Light>());
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 170f;
            PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocal), cameraOffset, lookOffset);
            ApplyStage7BokehFocusForReview(camera);
        }

        private static void SaveHd2dAutonomousP2SoftBlobContactShadowShot(Camera camera, string outputDirectory, string fileName)
        {
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
        }

        private static void ApplyHd2dAutonomousP2SoftBlobLowSunOverlaysForReview(Light mainLight)
        {
            if (mainLight == null)
            {
                return;
            }

            var overlays = UnityEngine.Object.FindObjectsByType<FastVsHd2dOverlayProfile>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (var i = 0; i < overlays.Length; i++)
            {
                var overlay = overlays[i];
                if (overlay == null ||
                    (overlay.OverlayKindForReview != FastVsHd2dOverlayKind.CharacterDirectionalCastShadow &&
                     overlay.OverlayKindForReview != FastVsHd2dOverlayKind.CharacterContactShadow &&
                     overlay.OverlayKindForReview != FastVsHd2dOverlayKind.CharacterFootContact))
                {
                    continue;
                }

                FastVsRealtimeLightShadowRig.ApplyP1ContactHardeningOverlayTransformForReview(overlay, mainLight);
            }
        }

        private static void SetHd2dAutonomousP2OverlayEnabled(
            IReadOnlyList<Hd2dAutonomousP1DirectionalShadowReviewState> states,
            bool enabled)
        {
            for (var i = 0; i < states.Count; i++)
            {
                var state = states[i];
                if (state == null || state.Renderer == null)
                {
                    continue;
                }

                state.Renderer.enabled = enabled;
                state.Renderer.shadowCastingMode = ShadowCastingMode.Off;
                state.Renderer.receiveShadows = false;
            }
        }

        private static void SetHd2dAutonomousP2SoftBlobOverlayEnabled(
            IReadOnlyList<Hd2dAutonomousP1DirectionalShadowReviewState> states,
            bool enabled)
        {
            for (var i = 0; i < states.Count; i++)
            {
                var state = states[i];
                if (state == null || state.Transform == null || state.Renderer == null)
                {
                    continue;
                }

                state.Renderer.enabled = enabled;
                state.Renderer.shadowCastingMode = ShadowCastingMode.Off;
                state.Renderer.receiveShadows = false;
                var baseLength = state.FootprintWorldSize.x > 0.001f ? state.FootprintWorldSize.x : Mathf.Abs(state.OriginalLocalScale.x);
                var baseWidth = state.FootprintWorldSize.y > 0.001f ? state.FootprintWorldSize.y : Mathf.Abs(state.OriginalLocalScale.y);
                var zScale = Mathf.Abs(state.OriginalLocalScale.z) > 0.001f ? state.OriginalLocalScale.z : 1f;
                state.Transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                state.Transform.localScale = new Vector3(baseLength, baseWidth, zScale);
            }
        }

        private static void WriteHd2dAutonomousP2SoftBlobContactShadowReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            Hd2dAutonomousP2SoftBlobContactShadowStats stats,
            Hd2dAutonomousP1DepthPrimingDiffMetrics blobDiff,
            Hd2dAutonomousP1DepthPrimingDiffMetrics directionalOffDiff)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var lines = new List<string>
            {
                "# P2-52 Independent Soft Blob Contact Shadow Review",
                string.Empty,
                "- Scope: keep a small soft grounding blob directly under each character, independent of the low-sun directional cast shadow overlay.",
                "- Implementation: existing contact/foot overlay assets are reused; runtime light-shadow rig now resets contact/foot overlays to horizontal yaw 0 while only directional overlays rotate/lengthen with sun direction.",
                string.Empty,
                "| Audit | Value |",
                "|---|---:|",
                $"| Character contact overlays | {stats.ContactOverlayCount} |",
                $"| Character foot-contact overlays | {stats.FootOverlayCount} |",
                $"| Dynamic contact overlays | {stats.DynamicOverlayCount} |",
                $"| Directional character overlays | {stats.DirectionalOverlayCount} |",
                $"| Enabled contact/foot renderers | {stats.EnabledOverlayCount} |",
                $"| Non-casting/non-receiving contact/foot renderers | {stats.NonCastingOverlayCount} |",
                $"| ContactShadow material-role matches | {stats.ContactMaterialRoleCount} |",
                $"| Low-sun pitch / yaw | {stats.LowSunPitch:0.#} / {stats.LowSunYaw:0.#} |",
                $"| Niro contact yaw / directional yaw / expected directional yaw | {stats.NiroContactYaw:0.#} / {stats.NiroDirectionalYaw:0.#} / {stats.ExpectedDirectionalYaw:0.#} |",
                $"| Niro directional rotation delta | {stats.NiroDirectionalRotationDelta:0.###} |",
                $"| Niro directional length / base length | {stats.NiroDirectionalLength:0.###} / {stats.NiroDirectionalBaseLength:0.###} |",
                string.Empty,
                "| Acceptance guard | Value |",
                "|---|---|",
                $"| Niro contact blob remains under feet | {FormatBool(stats.NiroContactAnchored)} |",
                $"| Niro foot-contact blob remains under feet | {FormatBool(stats.NiroFootAnchored)} |",
                $"| Contact/foot rotation independent of sun yaw | {FormatBool(stats.ContactRotationIndependent)} |",
                $"| Contact/foot scale independent of sun lengthening | {FormatBool(stats.ContactScaleIndependent)} |",
                $"| Directional shadow rotates with low sun | {FormatBool(stats.DirectionalRotationFollowsLowSun)} |",
                $"| Directional shadow lengthens with low sun | {FormatBool(stats.DirectionalScaleFollowsLowSun)} |",
                string.Empty,
                "| A/B evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                blobDiff.ToReportRow("Low-sun directional only vs contact blob restored"),
                directionalOffDiff.ToReportRow("Contact blob restored vs directional shadow disabled"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Low sun with raked directional overlay only; contact/foot blobs hidden for baseline |",
                $"| `{screenshotFiles[1]}` | Same low-sun view with independent contact/foot blobs restored under Niro's feet |",
                $"| `{screenshotFiles[2]}` | Directional character shadow disabled; contact/foot blobs alone should still ground the character |",
                $"| `{screenshotFiles[3]}` | Current-library Reto static-character guard with directional character shadow disabled |"
            };

            File.WriteAllLines(Path.Combine(outputDirectory, "soft_blob_contact_shadow_review.md"), lines, Encoding.UTF8);
        }

        private struct Hd2dAutonomousP2SoftBlobContactShadowStats
        {
            public int ContactOverlayCount;
            public int FootOverlayCount;
            public int DynamicOverlayCount;
            public int DirectionalOverlayCount;
            public int EnabledOverlayCount;
            public int NonCastingOverlayCount;
            public int ContactMaterialRoleCount;
            public bool NiroContactAnchored;
            public bool NiroFootAnchored;
            public bool ContactRotationIndependent;
            public bool ContactScaleIndependent;
            public bool DirectionalRotationFollowsLowSun;
            public bool DirectionalScaleFollowsLowSun;
            public float NiroContactYaw;
            public float NiroDirectionalYaw;
            public float ExpectedDirectionalYaw;
            public float NiroDirectionalRotationDelta;
            public float NiroDirectionalLength;
            public float NiroDirectionalBaseLength;
            public float LowSunPitch;
            public float LowSunYaw;
        }
    }
}
