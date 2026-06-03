using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Anemora.FastVS;
using Anemora.FastVS.SunCycle;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2DioramaEdgeTreatmentRootSuffix = "_P2_75_DioramaEdgeTreatment";
        private const string Hd2dAutonomousP2DioramaEdgeTreatmentProfilePath = "Assets/Settings/FastVS_HD2D_P2_DioramaEdgeTreatmentProfile.asset";
        private const string Hd2dAutonomousP2DioramaEdgeTreatmentProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dDioramaEdgeTreatmentProfile.cs";
        private const string Hd2dAutonomousP2DioramaEdgeTreatmentMarkerRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dDioramaEdgeTreatmentMarker.cs";
        private const string Hd2dAutonomousP2DioramaEdgeTreatmentEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P2DioramaEdgeTreatment.cs";

        public static void CaptureHd2dAutonomousP2Item75DioramaEdgeTreatmentBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var sunDriver = sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : null;
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || sunDriver == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-75 diorama edge treatment capture failed: required review components are missing.");
            }

            ValidateHd2dAutonomousP2DioramaEdgeTreatment();
            var profile = EnsureHd2dAutonomousP2DioramaEdgeTreatmentProfile();
            var roots = GetHd2dAutonomousP2DioramaEdgeTreatmentRoots();
            if (roots.Length < profile.MinimumDioramaRootCountForReview)
            {
                throw new InvalidOperationException($"Fast VS autonomous P2-75 capture failed: expected edge treatment roots are missing. roots={roots.Length}");
            }

            var outputDirectory = ResolveAutonomousReviewOutputDirectory("diorama_edge_cliff_lip_foliage_fog");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_aria_front_edge_treatment_off.png",
                "02_aria_front_edge_treatment_on.png",
                "03_aria_left_edge_foliage_skirt_close.png",
                "04_past_ruins_edge_value_drop_check.png",
                "05_topdown_edge_coverage_diagnostic.png",
            };
            var shotRows = new List<string>();
            var previousCullingMask = camera.cullingMask;
            var previousRootActive = roots.ToDictionary(root => root, root => root.activeSelf);

            try
            {
                guide.SetMovementFrozen(true);
                sunDriver.ApplyPreset(SunPreset.Noon, true);
                realtimeRig.ApplyNowForReview();

                SetHd2dAutonomousP2DioramaEdgeTreatmentRootsActive(roots, false);
                CaptureHd2dAutonomousP2DioramaEdgeTreatmentShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.AriaStreet,
                    false,
                    Chapter1AriaStreetMapCenter + new Vector3(-7.0f, 0.02f, -9.85f),
                    new Vector3(0.35f, 8.65f, -11.35f),
                    new Vector3(0.00f, 0.62f, 1.42f),
                    35f,
                    previousCullingMask,
                    outputDirectory,
                    screenshotFiles[0],
                    "Aria Street front diorama edge with P2-75 roots disabled",
                    false,
                    shotRows);

                SetHd2dAutonomousP2DioramaEdgeTreatmentRootsActive(roots, true);
                CaptureHd2dAutonomousP2DioramaEdgeTreatmentShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.AriaStreet,
                    false,
                    Chapter1AriaStreetMapCenter + new Vector3(-7.0f, 0.02f, -9.85f),
                    new Vector3(0.35f, 8.65f, -11.35f),
                    new Vector3(0.00f, 0.62f, 1.42f),
                    35f,
                    previousCullingMask,
                    outputDirectory,
                    screenshotFiles[1],
                    "same Aria Street edge with cliff lip, foliage skirt, AO band, and value drop enabled",
                    true,
                    shotRows);

                CaptureHd2dAutonomousP2DioramaEdgeTreatmentShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.AriaStreet,
                    false,
                    Chapter1AriaStreetMapCenter + new Vector3(-23.1f, 0.02f, 0.3f),
                    new Vector3(-6.25f, 6.95f, -8.45f),
                    new Vector3(1.12f, 0.64f, 0.42f),
                    33f,
                    previousCullingMask,
                    outputDirectory,
                    screenshotFiles[2],
                    "Aria Street left edge close-up: alpha-clip foliage skirt breaks the rim",
                    true,
                    shotRows);

                CaptureHd2dAutonomousP2DioramaEdgeTreatmentShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.Ruins,
                    true,
                    Chapter1RuinsMapCenter + new Vector3(8.8f, 0.02f, 10.15f),
                    new Vector3(0.25f, 13.80f, -18.80f),
                    new Vector3(0.00f, 0.82f, 1.40f),
                    42f,
                    previousCullingMask,
                    outputDirectory,
                    screenshotFiles[3],
                    "past Ruins back edge: value band and rock lip sink the boundary",
                    true,
                    shotRows);

                CaptureHd2dAutonomousP2DioramaEdgeTreatmentShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.AriaStreet,
                    false,
                    Chapter1AriaStreetMapCenter + new Vector3(0f, 0.02f, -0.2f),
                    new Vector3(0.45f, 30.50f, -27.40f),
                    new Vector3(0.00f, 1.00f, 2.15f),
                    53f,
                    previousCullingMask,
                    outputDirectory,
                    screenshotFiles[4],
                    "top-down diagnostic: rim treatment wraps the readable diorama boundary",
                    true,
                    shotRows);
            }
            finally
            {
                foreach (var pair in previousRootActive)
                {
                    if (pair.Key != null)
                    {
                        pair.Key.SetActive(pair.Value || profile.ConservativeTreatmentEnabledForReview);
                    }
                }

                camera.cullingMask = previousCullingMask;
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                guide.SetMovementFrozen(false);
                AssetDatabase.SaveAssets();
            }

            var beforeAfterDiff = MeasureHd2dAutonomousP2DioramaEdgeTreatmentDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var leftCloseDiff = MeasureHd2dAutonomousP2DioramaEdgeTreatmentDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            WriteHd2dAutonomousP2DioramaEdgeTreatmentReviewReport(outputDirectory, screenshotFiles, shotRows, profile, beforeAfterDiff, leftCloseDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-75 diorama edge treatment review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2DioramaEdgeTreatment(Transform parent, string prefix, string mapToken, Vector3 center, float width, float depth)
        {
            if (parent == null || string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(mapToken) || width <= 0f || depth <= 0f)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP2DioramaEdgeTreatmentProfile();
            var rootName = GetHd2dAutonomousP2DioramaEdgeTreatmentRootName(prefix, mapToken);
            var existing = FindSceneObjectIncludingInactive(rootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(rootName);
            root.transform.SetParent(parent, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            var materials = EnsureHd2dAutonomousP2DioramaEdgeTreatmentMaterials();
            CreateHd2dAutonomousP2DioramaEdgeTreatmentSide(root.transform, profile, materials, prefix, mapToken, "front", center, width, depth);
            CreateHd2dAutonomousP2DioramaEdgeTreatmentSide(root.transform, profile, materials, prefix, mapToken, "back", center, width, depth);
            CreateHd2dAutonomousP2DioramaEdgeTreatmentSide(root.transform, profile, materials, prefix, mapToken, "left", center, width, depth);
            CreateHd2dAutonomousP2DioramaEdgeTreatmentSide(root.transform, profile, materials, prefix, mapToken, "right", center, width, depth);

            ApplyHd2dAutonomousP0StaticFlags(root);
            SetHd2dAutonomousP2DioramaEdgeTreatmentLayerRecursively(root.transform, string.Equals(prefix, "Past", StringComparison.Ordinal) ? OtherTimeSpaceRenderLayer : CurrentSpaceRenderLayer);
            root.SetActive(profile.ConservativeTreatmentEnabledForReview);
            EditorUtility.SetDirty(root);
        }

        private static void ValidateHd2dAutonomousP2DioramaEdgeTreatment()
        {
            var profile = EnsureHd2dAutonomousP2DioramaEdgeTreatmentProfile();
            var markers = UnityEngine.Object.FindObjectsByType<FastVsHd2dDioramaEdgeTreatmentMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var roots = GetHd2dAutonomousP2DioramaEdgeTreatmentRoots();
            var renderers = markers
                .Select(marker => marker != null ? marker.GetComponent<Renderer>() : null)
                .Where(renderer => renderer != null)
                .ToArray();
            var mapCount = markers.Select(marker => marker.MapTokenForReview).Distinct().Count();
            var typeCount = markers.Select(marker => marker.TreatmentTypeForReview).Distinct().Count();
            var currentWorldCount = markers.Count(marker => marker.CurrentWorldForReview);
            var pastWorldCount = markers.Length - currentWorldCount;
            var hiddenEdgeCount = markers.Count(marker => marker.HidesFlatSlabEdgeForReview);
            var alphaCards = markers.Count(marker => marker.AlphaClipFoliageCardForReview);
            var cliffCoverage = markers.Where(marker => marker.TreatmentTypeForReview == FastVsHd2dDioramaEdgeTreatmentType.CliffLip).Sum(marker => marker.CoverageMetersForReview);
            var foliageCoverage = markers.Where(marker => marker.TreatmentTypeForReview == FastVsHd2dDioramaEdgeTreatmentType.FoliageSkirt).Sum(marker => marker.CoverageMetersForReview);
            var valueDropCoverage = markers.Where(marker => marker.TreatmentTypeForReview == FastVsHd2dDioramaEdgeTreatmentType.ValueDropOff).Sum(marker => marker.CoverageMetersForReview);
            var colliderCount = markers.Sum(marker => marker.GetComponentsInChildren<Collider>(true).Length);
            var staticReady = markers.Count(marker =>
            {
                var flags = GameObjectUtility.GetStaticEditorFlags(marker.gameObject);
                return (flags & StaticEditorFlags.BatchingStatic) != 0 && (flags & StaticEditorFlags.ContributeGI) != 0;
            });
            var instancedRenderers = renderers.Count(renderer => renderer.sharedMaterial != null && renderer.sharedMaterial.enableInstancing);

            if (!profile.NeedsTomApprovalForReview ||
                profile.FinalDioramaEdgeTreatmentApprovedForReview ||
                !profile.ConservativeTreatmentEnabledForReview ||
                roots.Length < profile.MinimumDioramaRootCountForReview ||
                markers.Length < profile.MinimumMarkerCountForReview ||
                mapCount < profile.MinimumMapCountForReview ||
                typeCount < profile.MinimumTreatmentTypeCountForReview ||
                currentWorldCount == 0 ||
                pastWorldCount == 0 ||
                hiddenEdgeCount < markers.Length ||
                alphaCards < 80 ||
                cliffCoverage < profile.TargetVisibleEdgeCoverageMetersForReview ||
                foliageCoverage < profile.TargetFoliageSkirtCoverageMetersForReview ||
                valueDropCoverage < profile.TargetVisibleEdgeCoverageMetersForReview ||
                colliderCount != 0 ||
                staticReady < markers.Length ||
                instancedRenderers < renderers.Length)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: P2-75 diorama edge treatment metrics are short. roots={roots.Length}, markers={markers.Length}, maps={mapCount}, types={typeCount}, current/past={currentWorldCount}/{pastWorldCount}, alphaCards={alphaCards}, cliff={cliffCoverage:0.###}, foliage={foliageCoverage:0.###}, valueDrop={valueDropCoverage:0.###}, colliders={colliderCount}, static={staticReady}/{markers.Length}, instanced={instancedRenderers}/{renderers.Length}.");
            }

            if (profile.CliffLipDropMetersForReview < 0.45f ||
                profile.ValueDropBandMetersForReview < 0.65f ||
                profile.EdgeAoBandWidthMetersForReview < 0.20f ||
                profile.FoliageSkirtSpacingMetersForReview > 4.2f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-75 profile must keep a visible cliff lip, value drop-off band, AO band, and conservative foliage skirt spacing.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2DioramaEdgeTreatmentProfileRuntimePath), "finalDioramaEdgeTreatmentApproved", Hd2dAutonomousP2DioramaEdgeTreatmentProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2DioramaEdgeTreatmentMarkerRuntimePath), "FastVsHd2dDioramaEdgeTreatmentType", Hd2dAutonomousP2DioramaEdgeTreatmentMarkerRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2DioramaEdgeTreatmentEditorPath), "CreateHd2dAutonomousP2DioramaEdgeTreatmentSide", Hd2dAutonomousP2DioramaEdgeTreatmentEditorPath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2DioramaEdgeTreatment", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dDioramaEdgeTreatmentProfile EnsureHd2dAutonomousP2DioramaEdgeTreatmentProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dDioramaEdgeTreatmentProfile>(Hd2dAutonomousP2DioramaEdgeTreatmentProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dDioramaEdgeTreatmentProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2DioramaEdgeTreatmentProfilePath);
            }

            profile.ConfigureForReview(
                10,
                260,
                5,
                4,
                520f,
                190f,
                0.78f,
                1.18f,
                0.42f,
                3.45f,
                true,
                true,
                true,
                true,
                false,
                "P2-75 composes existing static cliff/stone pixels, alpha-clip foliage cards, and dark value bands around current/past Chapter 1 outdoor diorama rims; no new render pipeline feature is introduced.",
                "Keep the conservative rim treatment as data only. Tom should tune final cliff silhouettes, fog value, foliage density, approved rock/grass meshes, and per-map edge language.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static void CreateHd2dAutonomousP2DioramaEdgeTreatmentSide(
            Transform root,
            FastVsHd2dDioramaEdgeTreatmentProfile profile,
            Hd2dAutonomousP2DioramaEdgeTreatmentMaterials materials,
            string prefix,
            string mapToken,
            string side,
            Vector3 center,
            float width,
            float depth)
        {
            var horizontal = string.Equals(side, "front", StringComparison.Ordinal) || string.Equals(side, "back", StringComparison.Ordinal);
            var negativeSide = string.Equals(side, "front", StringComparison.Ordinal) || string.Equals(side, "left", StringComparison.Ordinal);
            var sideLength = horizontal ? width : depth;
            var halfLength = sideLength * 0.5f;
            var sign = negativeSide ? -1f : 1f;
            var edgeCoordinate = horizontal ? center.z + sign * depth * 0.5f : center.x + sign * width * 0.5f;
            var sideYaw = GetHd2dAutonomousP2DioramaEdgeTreatmentYaw(side);
            var currentWorld = string.Equals(prefix, "Current", StringComparison.Ordinal);
            var token = $"{prefix}_{mapToken}_P2_75_{side}";

            CreateHd2dAutonomousP2DioramaEdgeTreatmentCliffLipSegments(root, profile, materials.CliffLip, prefix, mapToken, side, center, sideLength, edgeCoordinate, horizontal, sign, sideYaw, currentWorld);

            var valuePosition = horizontal
                ? new Vector3(center.x, -0.86f, edgeCoordinate + sign * 0.72f)
                : new Vector3(edgeCoordinate + sign * 0.72f, -0.86f, center.z);
            var valueScale = horizontal
                ? new Vector3(sideLength + 1.08f, 0.20f, profile.ValueDropBandMetersForReview)
                : new Vector3(profile.ValueDropBandMetersForReview, 0.20f, sideLength + 1.08f);
            var valueBand = CreateHd2dAutonomousP2DioramaEdgeTreatmentCube(
                $"{token}_ValueDropFogBand",
                root,
                valuePosition,
                valueScale,
                Quaternion.Euler(0f, sideYaw, 0f),
                materials.ValueDrop,
                false,
                false);
            ConfigureHd2dAutonomousP2DioramaEdgeTreatmentMarker(valueBand, profile, FastVsHd2dDioramaEdgeTreatmentType.ValueDropOff, mapToken, side, currentWorld, sideLength, true, false);

            var aoPosition = horizontal
                ? new Vector3(center.x, 0.025f, edgeCoordinate - sign * 0.18f)
                : new Vector3(edgeCoordinate - sign * 0.18f, 0.025f, center.z);
            var aoScale = horizontal
                ? new Vector3(sideLength, 0.028f, profile.EdgeAoBandWidthMetersForReview)
                : new Vector3(profile.EdgeAoBandWidthMetersForReview, 0.028f, sideLength);
            var aoBand = CreateHd2dAutonomousP2DioramaEdgeTreatmentCube(
                $"{token}_EdgeAoBand",
                root,
                aoPosition,
                aoScale,
                Quaternion.Euler(0f, sideYaw, 0f),
                materials.EdgeAo,
                false,
                false);
            ConfigureHd2dAutonomousP2DioramaEdgeTreatmentMarker(aoBand, profile, FastVsHd2dDioramaEdgeTreatmentType.EdgeAoBand, mapToken, side, currentWorld, sideLength, true, false);

            CreateHd2dAutonomousP2DioramaEdgeTreatmentRockBreakups(root, profile, materials.RockBreakup, prefix, mapToken, side, center, sideLength, edgeCoordinate, horizontal, sign, sideYaw, currentWorld);
            CreateHd2dAutonomousP2DioramaEdgeTreatmentFoliageSkirt(root, profile, materials.FoliageSkirt, prefix, mapToken, side, center, sideLength, halfLength, edgeCoordinate, horizontal, sign, sideYaw, currentWorld);
        }

        private static void CreateHd2dAutonomousP2DioramaEdgeTreatmentCliffLipSegments(
            Transform root,
            FastVsHd2dDioramaEdgeTreatmentProfile profile,
            Material material,
            string prefix,
            string mapToken,
            string side,
            Vector3 center,
            float sideLength,
            float edgeCoordinate,
            bool horizontal,
            float sign,
            float sideYaw,
            bool currentWorld)
        {
            var count = Mathf.Clamp(Mathf.CeilToInt(sideLength / 6.2f), 4, 9);
            var segmentStride = (sideLength + 0.72f) / count;
            for (var i = 0; i < count; i++)
            {
                var t = (i + 0.5f) / count;
                var along = Mathf.Lerp(-sideLength * 0.5f + 0.34f, sideLength * 0.5f - 0.34f, t);
                var jitter = Mathf.Sin((i + 2) * 1.439f + sideLength * 0.09f) * 0.18f;
                var lipDrop = profile.CliffLipDropMetersForReview * (0.66f + PositiveModulo(i * 3, 5) * 0.045f);
                var segmentLength = segmentStride * (0.76f + PositiveModulo(i + 1, 4) * 0.04f);
                var cross = 0.15f + PositiveModulo(i * 2, 5) * 0.045f;
                var position = horizontal
                    ? new Vector3(center.x + along + jitter, -lipDrop * 0.5f + 0.035f, edgeCoordinate + sign * cross)
                    : new Vector3(edgeCoordinate + sign * cross, -lipDrop * 0.5f + 0.035f, center.z + along + jitter);
                var scale = horizontal
                    ? new Vector3(segmentLength, lipDrop, 0.46f + PositiveModulo(i, 3) * 0.045f)
                    : new Vector3(0.46f + PositiveModulo(i, 3) * 0.045f, lipDrop, segmentLength);
                var cliff = CreateHd2dAutonomousP2DioramaEdgeTreatmentCube(
                    $"{prefix}_{mapToken}_P2_75_{side}_CliffLipSegment_{i:00}",
                    root,
                    position,
                    scale,
                    Quaternion.Euler(0f, sideYaw + Mathf.Sin(i * 1.9f) * 3.2f + (currentWorld ? 0.6f : -0.6f), 0f),
                    material,
                    true,
                    true);
                ConfigureHd2dAutonomousP2DioramaEdgeTreatmentMarker(cliff, profile, FastVsHd2dDioramaEdgeTreatmentType.CliffLip, mapToken, side, currentWorld, sideLength / count, true, false);
            }
        }

        private static void CreateHd2dAutonomousP2DioramaEdgeTreatmentRockBreakups(
            Transform root,
            FastVsHd2dDioramaEdgeTreatmentProfile profile,
            Material material,
            string prefix,
            string mapToken,
            string side,
            Vector3 center,
            float sideLength,
            float edgeCoordinate,
            bool horizontal,
            float sign,
            float sideYaw,
            bool currentWorld)
        {
            var count = Mathf.Clamp(Mathf.RoundToInt(sideLength / 9f), 2, 7);
            for (var i = 0; i < count; i++)
            {
                var t = (i + 0.5f) / count;
                var along = Mathf.Lerp(-sideLength * 0.5f + 0.78f, sideLength * 0.5f - 0.78f, t);
                var jitter = Mathf.Sin((i + 1) * 1.713f + sideLength * 0.11f) * 0.28f;
                var height = 0.28f + PositiveModulo(i * 5, 7) * 0.052f;
                var position = horizontal
                    ? new Vector3(center.x + along + jitter, height * 0.5f + 0.02f, edgeCoordinate - sign * (0.08f + PositiveModulo(i, 3) * 0.055f))
                    : new Vector3(edgeCoordinate - sign * (0.08f + PositiveModulo(i, 3) * 0.055f), height * 0.5f + 0.02f, center.z + along + jitter);
                var scale = horizontal
                    ? new Vector3(1.04f + PositiveModulo(i, 3) * 0.22f, height, 0.32f)
                    : new Vector3(0.32f, height, 1.04f + PositiveModulo(i, 3) * 0.22f);
                var rock = CreateHd2dAutonomousP2DioramaEdgeTreatmentCube(
                    $"{prefix}_{mapToken}_P2_75_{side}_RockBreakup_{i:00}",
                    root,
                    position,
                    scale,
                    Quaternion.Euler(0f, sideYaw + Mathf.Sin(i * 2.1f) * 7f, 0f),
                    material,
                    true,
                    true);
                ConfigureHd2dAutonomousP2DioramaEdgeTreatmentMarker(rock, profile, FastVsHd2dDioramaEdgeTreatmentType.RockBreakup, mapToken, side, currentWorld, scale.x + scale.z, true, false);
            }
        }

        private static void CreateHd2dAutonomousP2DioramaEdgeTreatmentFoliageSkirt(
            Transform root,
            FastVsHd2dDioramaEdgeTreatmentProfile profile,
            Material material,
            string prefix,
            string mapToken,
            string side,
            Vector3 center,
            float sideLength,
            float halfLength,
            float edgeCoordinate,
            bool horizontal,
            float sign,
            float sideYaw,
            bool currentWorld)
        {
            var count = Mathf.Clamp(Mathf.RoundToInt(sideLength / profile.FoliageSkirtSpacingMetersForReview), 3, 18);
            for (var i = 0; i < count; i++)
            {
                var t = count <= 1 ? 0.5f : i / (count - 1f);
                var along = Mathf.Lerp(-halfLength + 0.68f, halfLength - 0.68f, t);
                var jitter = Mathf.Sin((i + 3) * 1.247f + sideLength * 0.07f) * 0.20f;
                var edgeInset = 0.16f + PositiveModulo(i * 3, 5) * 0.055f;
                var position = horizontal
                    ? new Vector3(center.x + along + jitter, 0.54f + PositiveModulo(i, 4) * 0.018f, edgeCoordinate - sign * edgeInset)
                    : new Vector3(edgeCoordinate - sign * edgeInset, 0.54f + PositiveModulo(i, 4) * 0.018f, center.z + along + jitter);
                var scale = new Vector3(0.70f + PositiveModulo(i, 3) * 0.09f, 1.02f + PositiveModulo(i * 2, 5) * 0.055f, 1f);
                var card = CreateQuad(
                    $"{prefix}_{mapToken}_P2_75_{side}_FoliageSkirtCard_{i:00}",
                    root,
                    position,
                    scale,
                    material);
                card.transform.localRotation = Quaternion.Euler(0f, sideYaw + (i % 2 == 0 ? -4f : 4f), i % 2 == 0 ? -5f : 5f);
                ApplyHd2dAutonomousP0FoliageTightMesh(card, material);
                ConfigureHd2dAutonomousP2DioramaEdgeTreatmentRenderer(card.GetComponent<Renderer>(), material, false, true);
                ConfigureHd2dAutonomousP2DioramaEdgeTreatmentMarker(card, profile, FastVsHd2dDioramaEdgeTreatmentType.FoliageSkirt, mapToken, side, currentWorld, profile.FoliageSkirtSpacingMetersForReview, true, true);
            }
        }

        private static Hd2dAutonomousP2DioramaEdgeTreatmentMaterials EnsureHd2dAutonomousP2DioramaEdgeTreatmentMaterials()
        {
            var cliff = EnsureHd2dAutonomousP2DioramaEdgeTreatmentPixelMaterial("hd2d_p2_75_diorama_cliff_lip_rock", new Color32(48, 45, 39, 255), new Color32(88, 78, 62, 255), new Color32(126, 112, 82, 255), PixelPattern.Stone, new Vector2(2.6f, 1.7f));
            var rock = EnsureHd2dAutonomousP2DioramaEdgeTreatmentPixelMaterial("hd2d_p2_75_diorama_cliff_rock_breakup", new Color32(46, 42, 36, 255), new Color32(98, 86, 64, 255), new Color32(143, 124, 88, 255), PixelPattern.Stone, new Vector2(2.0f, 1.4f));
            var value = EnsureHd2dAutonomousP2DioramaEdgeTreatmentPixelMaterial("hd2d_p2_75_diorama_value_drop_fog_band", new Color32(30, 38, 40, 255), new Color32(50, 61, 62, 255), new Color32(72, 82, 78, 255), PixelPattern.Noise, new Vector2(1.2f, 1.2f));
            var ao = FlatMaterial("hd2d_p2_75_diorama_edge_ao_band", new Color(0.095f, 0.105f, 0.091f, 1f), true);
            ao.enableInstancing = true;
            ApplyMaterialRole(ao, "hd2d_p2_75_diorama_edge_ao_band", FastVsHd2dMaterialRole.SurfaceLit);
            EditorUtility.SetDirty(ao);
            var foliage = EnsureHd2dAutonomousP2FoliageVarietyCardMaterial(
                "hd2d_p2_75_diorama_edge_foliage_skirt",
                EnsureFoliageCardTexture(FoliageGrassCardBTexturePath),
                new Color(0.34f, 0.56f, 0.28f, 1f),
                0.012f,
                1f);
            foliage.enableInstancing = true;
            EditorUtility.SetDirty(foliage);
            return new Hd2dAutonomousP2DioramaEdgeTreatmentMaterials(cliff, rock, value, ao, foliage);
        }

        private static Material EnsureHd2dAutonomousP2DioramaEdgeTreatmentPixelMaterial(string materialId, Color32 dark, Color32 mid, Color32 high, PixelPattern pattern, Vector2 tiling)
        {
            var material = PixelMaterial(materialId, mid, high, dark, pattern, false, tiling);
            material.enableInstancing = true;
            ApplyMaterialRole(material, materialId, FastVsHd2dMaterialRole.SurfaceLit);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static GameObject CreateHd2dAutonomousP2DioramaEdgeTreatmentCube(
            string objectName,
            Transform parent,
            Vector3 localPosition,
            Vector3 localScale,
            Quaternion localRotation,
            Material material,
            bool castsShadows,
            bool receivesShadows)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = objectName;
            cube.transform.SetParent(parent, false);
            cube.transform.localPosition = localPosition;
            cube.transform.localRotation = localRotation;
            cube.transform.localScale = localScale;
            var collider = cube.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            ConfigureHd2dAutonomousP2DioramaEdgeTreatmentRenderer(cube.GetComponent<Renderer>(), material, castsShadows, receivesShadows);
            return cube;
        }

        private static void ConfigureHd2dAutonomousP2DioramaEdgeTreatmentRenderer(Renderer renderer, Material material, bool castsShadows, bool receivesShadows)
        {
            if (renderer == null)
            {
                return;
            }

            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = castsShadows ? ShadowCastingMode.On : ShadowCastingMode.Off;
            renderer.receiveShadows = receivesShadows;
            renderer.lightProbeUsage = LightProbeUsage.BlendProbes;
            renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            EditorUtility.SetDirty(renderer);
        }

        private static void ConfigureHd2dAutonomousP2DioramaEdgeTreatmentMarker(
            GameObject target,
            FastVsHd2dDioramaEdgeTreatmentProfile profile,
            FastVsHd2dDioramaEdgeTreatmentType type,
            string mapToken,
            string edgeSide,
            bool currentWorld,
            float coverageMeters,
            bool hidesFlatSlabEdge,
            bool alphaClipFoliageCard)
        {
            var marker = target.GetComponent<FastVsHd2dDioramaEdgeTreatmentMarker>();
            if (marker == null)
            {
                marker = target.AddComponent<FastVsHd2dDioramaEdgeTreatmentMarker>();
            }

            var renderer = target.GetComponent<Renderer>();
            marker.ConfigureForReview(
                profile,
                type,
                mapToken,
                edgeSide,
                currentWorld,
                coverageMeters,
                hidesFlatSlabEdge,
                alphaClipFoliageCard,
                true,
                renderer != null && renderer.sharedMaterial != null && renderer.sharedMaterial.enableInstancing,
                true);
            EditorUtility.SetDirty(marker);
        }

        private static void CaptureHd2dAutonomousP2DioramaEdgeTreatmentShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            FastVsHouseArea area,
            bool pastTimeline,
            Vector3 anchorLocalPosition,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView,
            int previousCullingMask,
            string outputDirectory,
            string fileName,
            string label,
            bool edgeTreatmentActive,
            ICollection<string> rows)
        {
            visibility.SetActiveAreaForReview(area);
            if (pastTimeline)
            {
                controller.ForcePlayerOtherTimeLocalForReview(anchorLocalPosition + new Vector3(0f, 0.02f, -1.2f));
                var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
                var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
                var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
                camera.cullingMask = (previousCullingMask & ~currentBit) | otherBit | playerBit;
                PositionChapter1AllMapsCamera(camera, controller.OtherTimeSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            }
            else
            {
                controller.ForcePlayerCurrentLocalForReview(anchorLocalPosition + new Vector3(0f, 0.02f, -1.2f));
                camera.cullingMask = previousCullingMask;
                PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            }

            guide.ApplyActiveTimeIsolationForReview();
            realtimeRig.ApplyNowForReview();
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 170f;
            ApplyStage7BokehFocusForReview(camera);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
            rows?.Add($"| `{fileName}` | {label} | {area} | {(pastTimeline ? "past" : "current")} | {FormatBool(edgeTreatmentActive)} | {FormatVector3ForReport(anchorLocalPosition)} | {FormatVector3ForReport(cameraOffset)} | {fieldOfView:0.#} |");
        }

        private static void WriteHd2dAutonomousP2DioramaEdgeTreatmentReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dDioramaEdgeTreatmentProfile profile,
            Hd2dAutonomousP2DioramaEdgeTreatmentDiffMetrics beforeAfterDiff,
            Hd2dAutonomousP2DioramaEdgeTreatmentDiffMetrics closeupDiff)
        {
            var markers = UnityEngine.Object.FindObjectsByType<FastVsHd2dDioramaEdgeTreatmentMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            var roots = GetHd2dAutonomousP2DioramaEdgeTreatmentRoots();
            var cliffCoverage = markers.Where(marker => marker.TreatmentTypeForReview == FastVsHd2dDioramaEdgeTreatmentType.CliffLip).Sum(marker => marker.CoverageMetersForReview);
            var foliageCoverage = markers.Where(marker => marker.TreatmentTypeForReview == FastVsHd2dDioramaEdgeTreatmentType.FoliageSkirt).Sum(marker => marker.CoverageMetersForReview);
            var valueDropCoverage = markers.Where(marker => marker.TreatmentTypeForReview == FastVsHd2dDioramaEdgeTreatmentType.ValueDropOff).Sum(marker => marker.CoverageMetersForReview);
            var alphaCards = markers.Count(marker => marker.AlphaClipFoliageCardForReview);

            var lines = new List<string>
            {
                "# P2-75 Diorama Edge Treatment Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative data prep for edge-of-diorama treatment: cliff lip, foliage skirt, AO band, and dark value drop-off into fog.",
                "- Final art approval remains false; this pass records static rim coverage and representative A/B evidence for Tom.",
                $"- Recommendation: {profile.RecommendationForReview}",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2DioramaEdgeTreatmentProfilePath}` |",
                $"| Roots | {roots.Length} |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalDioramaEdgeTreatmentApprovedForReview)} |",
                $"| Conservative treatment enabled | {FormatBool(profile.ConservativeTreatmentEnabledForReview)} |",
                $"| Cliff lip drop / value drop band / AO band / foliage spacing | {profile.CliffLipDropMetersForReview:0.###} / {profile.ValueDropBandMetersForReview:0.###} / {profile.EdgeAoBandWidthMetersForReview:0.###} / {profile.FoliageSkirtSpacingMetersForReview:0.###} |",
                $"| Source note | {profile.SourceNoteForReview} |",
                string.Empty,
                "| Metric | Value |",
                "|---|---:|",
                $"| Markers | {markers.Length} |",
                $"| Maps | {markers.Select(marker => marker.MapTokenForReview).Distinct().Count()} |",
                $"| Current / past markers | {markers.Count(marker => marker.CurrentWorldForReview)} / {markers.Count(marker => !marker.CurrentWorldForReview)} |",
                $"| Cliff coverage meters | {cliffCoverage:0.###} |",
                $"| Foliage skirt coverage meters | {foliageCoverage:0.###} |",
                $"| Value drop coverage meters | {valueDropCoverage:0.###} |",
                $"| Alpha-clip skirt cards | {alphaCards} |",
                string.Empty,
                "| Treatment Type | Count | Coverage Meters |",
                "|---|---:|---:|"
            };

            foreach (var group in markers.GroupBy(marker => marker.TreatmentTypeForReview).OrderBy(group => group.Key.ToString()))
            {
                lines.Add($"| {group.Key} | {group.Count()} | {group.Sum(marker => marker.CoverageMetersForReview):0.###} |");
            }

            lines.AddRange(new[]
            {
                string.Empty,
                "| Map | Current Markers | Past Markers |",
                "|---|---:|---:|"
            });
            foreach (var group in markers.GroupBy(marker => marker.MapTokenForReview).OrderBy(group => group.Key))
            {
                lines.Add($"| {group.Key} | {group.Count(marker => marker.CurrentWorldForReview)} | {group.Count(marker => !marker.CurrentWorldForReview)} |");
            }

            lines.AddRange(new[]
            {
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                beforeAfterDiff.ToReportRow("Aria front edge off vs conservative on"),
                closeupDiff.ToReportRow("overview after vs left-edge close-up framing"),
                string.Empty,
                "| Screenshot | Label | Area | Timeline | Edge treatment active | Anchor | Camera offset | FOV |",
                "|---|---|---|---|---|---|---|---|"
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
                lines.Add($"| `{screenshotFiles[i]}` | P2-75 review capture {i + 1} |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "diorama_edge_cliff_lip_foliage_fog_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Hd2dAutonomousP2DioramaEdgeTreatmentDiffMetrics MeasureHd2dAutonomousP2DioramaEdgeTreatmentDiff(string outputDirectory, string firstFile, string secondFile)
        {
            var firstPath = Path.Combine(outputDirectory, firstFile);
            var secondPath = Path.Combine(outputDirectory, secondFile);
            var firstTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            var secondTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            try
            {
                if (!firstTexture.LoadImage(File.ReadAllBytes(firstPath)) || !secondTexture.LoadImage(File.ReadAllBytes(secondPath)))
                {
                    return new Hd2dAutonomousP2DioramaEdgeTreatmentDiffMetrics(0, 0, 0f, 0f);
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

                return new Hd2dAutonomousP2DioramaEdgeTreatmentDiffMetrics(
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

        private static void SetHd2dAutonomousP2DioramaEdgeTreatmentRootsActive(IReadOnlyList<GameObject> roots, bool active)
        {
            for (var i = 0; i < roots.Count; i++)
            {
                if (roots[i] != null)
                {
                    roots[i].SetActive(active);
                }
            }
        }

        private static GameObject[] GetHd2dAutonomousP2DioramaEdgeTreatmentRoots()
        {
            return UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(transform => transform != null && transform.name.EndsWith(Hd2dAutonomousP2DioramaEdgeTreatmentRootSuffix, StringComparison.Ordinal))
                .Select(transform => transform.gameObject)
                .Distinct()
                .OrderBy(gameObject => gameObject.name)
                .ToArray();
        }

        private static string GetHd2dAutonomousP2DioramaEdgeTreatmentRootName(string prefix, string mapToken)
        {
            return $"{prefix}_{mapToken}{Hd2dAutonomousP2DioramaEdgeTreatmentRootSuffix}";
        }

        private static float GetHd2dAutonomousP2DioramaEdgeTreatmentYaw(string side)
        {
            if (string.Equals(side, "back", StringComparison.Ordinal))
            {
                return 180f;
            }

            if (string.Equals(side, "left", StringComparison.Ordinal))
            {
                return 90f;
            }

            if (string.Equals(side, "right", StringComparison.Ordinal))
            {
                return -90f;
            }

            return 0f;
        }

        private static void SetHd2dAutonomousP2DioramaEdgeTreatmentLayerRecursively(Transform root, int layer)
        {
            if (root == null)
            {
                return;
            }

            foreach (var child in root.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.layer = layer;
            }
        }

        private readonly struct Hd2dAutonomousP2DioramaEdgeTreatmentMaterials
        {
            public readonly Material CliffLip;
            public readonly Material RockBreakup;
            public readonly Material ValueDrop;
            public readonly Material EdgeAo;
            public readonly Material FoliageSkirt;

            public Hd2dAutonomousP2DioramaEdgeTreatmentMaterials(Material cliffLip, Material rockBreakup, Material valueDrop, Material edgeAo, Material foliageSkirt)
            {
                CliffLip = cliffLip;
                RockBreakup = rockBreakup;
                ValueDrop = valueDrop;
                EdgeAo = edgeAo;
                FoliageSkirt = foliageSkirt;
            }
        }

        private readonly struct Hd2dAutonomousP2DioramaEdgeTreatmentDiffMetrics
        {
            private readonly int sampleCount;
            private readonly int changedPixels;
            private readonly float changedPercent;
            private readonly float meanRgbDelta;

            public Hd2dAutonomousP2DioramaEdgeTreatmentDiffMetrics(int sampleCount, int changedPixels, float changedPercent, float meanRgbDelta)
            {
                this.sampleCount = sampleCount;
                this.changedPixels = changedPixels;
                this.changedPercent = changedPercent;
                this.meanRgbDelta = meanRgbDelta;
            }

            public string ToReportRow(string label)
            {
                return $"| {label} | {sampleCount} | {changedPixels} | {changedPercent:0.###} | {meanRgbDelta:0.###} |";
            }
        }
    }
}
