using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Anemora.FastVS;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2CharacterSpriteScaleRootName = "FastVS_HD2D_P2_73_CharacterSpriteScaleStandard";
        private const string Hd2dAutonomousP2CharacterSpriteScaleProfilePath = "Assets/Settings/FastVS_HD2D_P2_CharacterSpriteScaleProfile.asset";
        private const string Hd2dAutonomousP2CharacterSpriteScaleProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dCharacterSpriteScaleProfile.cs";
        private const string Hd2dAutonomousP2CharacterSpriteScaleMarkerRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dCharacterSpriteScaleMarker.cs";
        private const string Hd2dAutonomousP2CharacterSpriteScaleEditorPath = "Assets/Editor/AnemoraFastVsHouseSliceSetup.P2CharacterSpriteScaleStandard.cs";
        private const string Hd2dAutonomousP2CharacterSpriteScaleReviewMaterialPrefix = "hd2d_p2_73_character_scale_review_";

        public static void CaptureHd2dAutonomousP2Item73CharacterSpriteScaleStandardBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-73 character sprite scale capture failed: review scene components are missing.");
            }

            ValidateHd2dAutonomousP2CharacterSpriteScaleStandard();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("character_sprite_scale_standard");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_character_scale_doorway_lineup.png",
                "02_character_texel_density_2x_crop.png",
                "03_character_foot_pivot_floor_alignment.png",
            };

            var profile = EnsureHd2dAutonomousP2CharacterSpriteScaleProfile();
            var lineupRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP2CharacterSpriteScaleRootName);
            if (lineupRoot == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-73 character sprite scale capture failed: lineup root is missing.");
            }

            var wasLineupActive = lineupRoot.activeSelf;
            lineupRoot.SetActive(true);
            var focus = lineupRoot.transform.TransformPoint(new Vector3(0f, profile.StandardAdultCardWorldHeightForReview * 0.54f, 0.04f));
            try
            {
                guide.SetMovementFrozen(true);
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                controller.ForcePlayerCurrentLocalForReview(GetHd2dAutonomousP2CharacterSpriteScaleLineupLocalPosition() + new Vector3(-1.20f, 0.02f, -1.10f));
                guide.ApplyActiveTimeIsolationForReview();
                Physics.SyncTransforms();
                realtimeRig.ApplyNowForReview();

                camera.fieldOfView = profile.FixedReviewCameraFieldOfViewForReview;
                PositionCloseReviewCamera(camera, focus, new Vector3(3.05f, 1.82f, -3.95f), new Vector3(0.04f, 0.18f, 0.02f));
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, screenshotFiles[0]));
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[0]);

                CreateHd2dAutonomousP2CharacterSpriteScaleTwoXCrop(
                    Path.Combine(outputDirectory, screenshotFiles[0]),
                    Path.Combine(outputDirectory, screenshotFiles[1]));
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[1]);

                camera.fieldOfView = 24f;
                PositionCloseReviewCamera(camera, lineupRoot.transform.TransformPoint(new Vector3(-0.30f, 0.18f, 0.02f)), new Vector3(1.70f, 0.78f, -2.18f), new Vector3(0.02f, 0.02f, 0.02f));
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, screenshotFiles[2]));
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[2]);
            }
            finally
            {
                lineupRoot.SetActive(wasLineupActive);
                guide.SetMovementFrozen(false);
            }

            var stats = MeasureHd2dAutonomousP2CharacterSpriteScaleStats();
            WriteHd2dAutonomousP2CharacterSpriteScaleReviewReport(outputDirectory, screenshotFiles, profile, stats);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-73 character sprite scale standard review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2CharacterSpriteScaleStandard(Transform currentCentralPlazaRoot, Materials materials, Camera camera)
        {
            if (currentCentralPlazaRoot == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP2CharacterSpriteScaleProfile();
            RefreshHd2dAutonomousP2CharacterSpriteScaleMarkers(profile);

            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP2CharacterSpriteScaleRootName);
            if (root != null)
            {
                UnityEngine.Object.DestroyImmediate(root);
            }

            root = new GameObject(Hd2dAutonomousP2CharacterSpriteScaleRootName);
            root.transform.SetParent(currentCentralPlazaRoot, false);
            root.transform.localPosition = GetHd2dAutonomousP2CharacterSpriteScaleLineupLocalPosition();
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.layer = CurrentSpaceRenderLayer;

            CreateHd2dAutonomousP2CharacterSpriteScaleReferenceDoorway(root.transform, materials, profile);
            CreateHd2dAutonomousP2CharacterSpriteScaleLineupCharacter(root.transform, "Scale_Niro", "Niro", -1.42f, profile.StandardAdultCardWorldHeightForReview, EnsureHd2dAutonomousP2CharacterSpriteScaleReviewMaterial("niro", NiroFrontStripPath, Color.white, NiroAnimatedFrameCount), materials, camera);
            CreateHd2dAutonomousP2CharacterSpriteScaleLineupCharacter(root.transform, "Scale_Mia", "Mia", -0.48f, profile.StandardAdultCardWorldHeightForReview, EnsureHd2dAutonomousP2CharacterSpriteScaleReviewMaterial("mia", MiaNormalLoopStripPath, Color.white, NiroAnimatedFrameCount), materials, camera);
            CreateHd2dAutonomousP2CharacterSpriteScaleLineupCharacter(root.transform, "Scale_Kaia", "Kaia", 0.46f, profile.StandardAdultCardWorldHeightForReview, EnsureHd2dAutonomousP2CharacterSpriteScaleReviewMaterial("kaia", KaiaNormalLoopStripPath, Color.white, NiroAnimatedFrameCount), materials, camera);
            CreateHd2dAutonomousP2CharacterSpriteScaleLineupCharacter(root.transform, "Scale_Dario", "Dario", 1.40f, profile.StandardAdultCardWorldHeightForReview, EnsureHd2dAutonomousP2CharacterSpriteScaleReviewMaterial("dario", DarioNormalLoopStripPath, Color.white, NiroAnimatedFrameCount), materials, camera);

            var landmark = root.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", "current.central_plaza.hd2d_p2_73.character_sprite_scale_standard");
            SerializedSet(landmark, "kind", TimeWindowPairedSpaceLandmarkKind.PropOrFeature);
            SerializedSet(landmark, "countsForArrival", false);
            SetHd2dAutonomousP2CharacterSpriteScaleLayerRecursively(root.transform, CurrentSpaceRenderLayer);
            root.SetActive(false);
            EditorUtility.SetDirty(root);
        }

        private static void ValidateHd2dAutonomousP2CharacterSpriteScaleStandard()
        {
            var profile = EnsureHd2dAutonomousP2CharacterSpriteScaleProfile();
            if (Mathf.Abs(profile.WorldUnitMetersForReview - 1f) > 0.001f ||
                profile.FramePixelWidthForReview != NiroExpectedTextureWidth ||
                profile.FramePixelHeightForReview != NiroExpectedTextureHeight ||
                profile.LoopFrameCountForReview != NiroAnimatedFrameCount ||
                profile.StandardAdultCardWorldHeightForReview < 1.12f ||
                profile.StandardAdultCardWorldHeightForReview > 1.24f ||
                profile.MinimumReviewLineupCountForReview < 3 ||
                !profile.PointFilteringRequiredForReview ||
                !profile.MipMapsDisabledRequiredForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P2-73 character sprite scale profile must define 1m units, 64x96 frames, standard adult height, Point filtering, and no mipmaps.");
            }

            RefreshHd2dAutonomousP2CharacterSpriteScaleMarkers(profile);
            var markers = UnityEngine.Object.FindObjectsByType<FastVsHd2dCharacterSpriteScaleMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(marker => marker != null && marker.gameObject.scene.IsValid())
                .ToArray();
            if (markers.Length < 10)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-73 needs character sprite scale markers on the active character cards, but only found {markers.Length}.");
            }

            var lineupMarkers = markers.Where(marker => marker.ReviewLineupCardForReview).ToArray();
            if (lineupMarkers.Length < profile.MinimumReviewLineupCountForReview)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-73 needs at least {profile.MinimumReviewLineupCountForReview} review lineup cards.");
            }

            var maxHeightDeviation = 0f;
            var maxWidthDeviation = 0f;
            var maxFootDeviation = 0f;
            var maxLineupTexelDensityDeviation = 0f;
            for (var i = 0; i < markers.Length; i++)
            {
                ValidateHd2dAutonomousP2CharacterSpriteScaleMarker(markers[i], profile, ref maxHeightDeviation, ref maxWidthDeviation, ref maxFootDeviation);
            }

            var expectedLineupTexels = profile.StandardTexelsPerWorldUnitForReview;
            for (var i = 0; i < lineupMarkers.Length; i++)
            {
                var marker = lineupMarkers[i];
                if (marker.ScaleClassForReview != FastVsHd2dCharacterSpriteScaleClass.ReviewStandard)
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-73 lineup marker {marker.CharacterIdForReview} must use ReviewStandard scale class.");
                }

                var densityDeviation = Mathf.Abs(marker.ActualTexelsPerWorldUnitForReview - expectedLineupTexels);
                maxLineupTexelDensityDeviation = Mathf.Max(maxLineupTexelDensityDeviation, densityDeviation);
                if (densityDeviation > profile.TexelsPerWorldUnitToleranceForReview)
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-73 lineup texel density drifted by {densityDeviation:0.###} px/unit for {marker.CharacterIdForReview}.");
                }
            }

            if (maxHeightDeviation > profile.HeightToleranceForReview ||
                maxWidthDeviation > profile.WidthToleranceForReview ||
                maxFootDeviation > profile.VisualFootPivotToleranceForReview)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: P2-73 marker deviations exceeded tolerance. height={maxHeightDeviation:0.####}, width={maxWidthDeviation:0.####}, foot={maxFootDeviation:0.####}.");
            }

            if (FindSceneObjectIncludingInactive(Hd2dAutonomousP2CharacterSpriteScaleRootName) == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-73 review lineup root is missing.");
            }

            if (FindSceneObjectIncludingInactive(Hd2dAutonomousP2CharacterSpriteScaleRootName).activeSelf)
            {
                throw new InvalidOperationException("House slice validation failed: P2-73 review lineup root must remain hidden outside the capture batch.");
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2CharacterSpriteScaleProfileRuntimePath), "worldUnitMeters", Hd2dAutonomousP2CharacterSpriteScaleProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2CharacterSpriteScaleMarkerRuntimePath), "VisualFootLocalYForReview", Hd2dAutonomousP2CharacterSpriteScaleMarkerRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2CharacterSpriteScaleEditorPath), "CreateHd2dAutonomousP2CharacterSpriteScaleTwoXCrop", Hd2dAutonomousP2CharacterSpriteScaleEditorPath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2CharacterSpriteScaleStandard", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2CharacterSpriteScaleStandard", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dCharacterSpriteScaleProfile EnsureHd2dAutonomousP2CharacterSpriteScaleProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dCharacterSpriteScaleProfile>(Hd2dAutonomousP2CharacterSpriteScaleProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dCharacterSpriteScaleProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2CharacterSpriteScaleProfilePath);
            }

            profile.ConfigureForReview(
                1f,
                NiroExpectedTextureWidth,
                NiroExpectedTextureHeight,
                NiroAnimatedFrameCount,
                1.18f,
                0.96f,
                1.12f,
                0.78f,
                NiroTransparentFootPixels,
                0.035f,
                0.030f,
                0.018f,
                0.75f,
                1.80f,
                4,
                30f,
                true,
                true,
                "World unit is one meter. Character cards use bottom-center visual-foot pivot after the transparent 2px foot pad, 64x96 authored frames, Point filtering, no mips, and fixed review camera distance so texel density remains comparable.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static void ConfigureHd2dAutonomousP2CharacterSpriteScaleMarker(Renderer renderer, string displayName, float height, bool reviewLineupCard)
        {
            if (renderer == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP2CharacterSpriteScaleProfile();
            var marker = renderer.GetComponent<FastVsHd2dCharacterSpriteScaleMarker>();
            if (marker == null)
            {
                marker = renderer.gameObject.AddComponent<FastVsHd2dCharacterSpriteScaleMarker>();
            }

            var texture = renderer.sharedMaterial == null ? null : renderer.sharedMaterial.mainTexture as Texture2D;
            var frameCount = texture == null || profile.FramePixelWidthForReview <= 0
                ? 1
                : Mathf.Max(1, texture.width / profile.FramePixelWidthForReview);
            marker.ConfigureForReview(
                BuildHd2dAutonomousP2CharacterSpriteScaleId(renderer, displayName),
                ResolveHd2dAutonomousP2CharacterSpriteScaleClass(displayName, renderer, height, reviewLineupCard, profile),
                profile.FramePixelWidthForReview,
                profile.FramePixelHeightForReview,
                frameCount,
                height,
                profile.TransparentFootPixelsForReview,
                true,
                reviewLineupCard);
            EditorUtility.SetDirty(marker);
            EditorUtility.SetDirty(renderer.gameObject);
        }

        private static void RefreshHd2dAutonomousP2CharacterSpriteScaleMarkers(FastVsHd2dCharacterSpriteScaleProfile profile)
        {
            var renderers = UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (var i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer == null ||
                    renderer.gameObject == null ||
                    !renderer.gameObject.scene.IsValid() ||
                    renderer.GetComponent<FastVsHd2dCharacterSpriteScaleMarker>() != null ||
                    !IsHd2dAutonomousP2CharacterSpriteCardRenderer(renderer))
                {
                    continue;
                }

                ConfigureHd2dAutonomousP2CharacterSpriteScaleMarker(renderer, ResolveHd2dAutonomousP2CharacterDisplayName(renderer), Mathf.Abs(renderer.transform.localScale.y), false);
            }
        }

        private static void ValidateHd2dAutonomousP2CharacterSpriteScaleMarker(
            FastVsHd2dCharacterSpriteScaleMarker marker,
            FastVsHd2dCharacterSpriteScaleProfile profile,
            ref float maxHeightDeviation,
            ref float maxWidthDeviation,
            ref float maxFootDeviation)
        {
            if (marker == null || marker.RendererForReview == null || marker.RendererForReview.sharedMaterial == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-73 marker has no renderer/material.");
            }

            var texture = marker.TextureForReview;
            if (texture == null)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-73 marker {marker.CharacterIdForReview} has no texture.");
            }

            if (texture.height != profile.FramePixelHeightForReview ||
                texture.width < profile.FramePixelWidthForReview ||
                texture.width % profile.FramePixelWidthForReview != 0)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-73 marker {marker.CharacterIdForReview} texture must be a 64x96 or 4x64x96 strip, got {texture.width}x{texture.height}.");
            }

            if (profile.PointFilteringRequiredForReview && texture.filterMode != FilterMode.Point)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-73 marker {marker.CharacterIdForReview} texture must use Point filtering.");
            }

            if (profile.MipMapsDisabledRequiredForReview && texture.mipmapCount != 1)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-73 marker {marker.CharacterIdForReview} texture must have controlled no-mip sampling.");
            }

            var heightDeviation = Mathf.Abs(marker.AuthoredWorldHeightForReview - marker.ExpectedWorldHeightForReview);
            var widthDeviation = Mathf.Abs(marker.AuthoredWorldWidthForReview - marker.ExpectedWorldWidthForReview);
            var footDeviation = Mathf.Abs(marker.VisualFootLocalYForReview);
            maxHeightDeviation = Mathf.Max(maxHeightDeviation, heightDeviation);
            maxWidthDeviation = Mathf.Max(maxWidthDeviation, widthDeviation);
            maxFootDeviation = Mathf.Max(maxFootDeviation, footDeviation);

            ValidateHd2dAutonomousP2CharacterSpriteScaleClass(marker, profile);
            if (heightDeviation > profile.HeightToleranceForReview ||
                widthDeviation > profile.WidthToleranceForReview ||
                footDeviation > profile.VisualFootPivotToleranceForReview)
            {
                throw new InvalidOperationException(
                    $"House slice validation failed: P2-73 marker {marker.CharacterIdForReview} deviates from its authored contract. height={heightDeviation:0.####}, width={widthDeviation:0.####}, foot={footDeviation:0.####}.");
            }
        }

        private static void ValidateHd2dAutonomousP2CharacterSpriteScaleClass(FastVsHd2dCharacterSpriteScaleMarker marker, FastVsHd2dCharacterSpriteScaleProfile profile)
        {
            var height = marker.AuthoredWorldHeightForReview;
            switch (marker.ScaleClassForReview)
            {
                case FastVsHd2dCharacterSpriteScaleClass.ReviewStandard:
                case FastVsHd2dCharacterSpriteScaleClass.StandardAdult:
                    if (Mathf.Abs(height - profile.StandardAdultCardWorldHeightForReview) > 0.085f)
                    {
                        throw new InvalidOperationException($"House slice validation failed: P2-73 standard adult marker {marker.CharacterIdForReview} height {height:0.###} drifted from the profile.");
                    }
                    break;
                case FastVsHd2dCharacterSpriteScaleClass.SmallAdult:
                case FastVsHd2dCharacterSpriteScaleClass.SeatedOrDesk:
                    if (height < profile.SmallAdultMinimumWorldHeightForReview - profile.HeightToleranceForReview ||
                        height > profile.SmallAdultMaximumWorldHeightForReview + profile.HeightToleranceForReview)
                    {
                        throw new InvalidOperationException($"House slice validation failed: P2-73 small adult marker {marker.CharacterIdForReview} height {height:0.###} is outside the allowed scale ladder.");
                    }
                    break;
                case FastVsHd2dCharacterSpriteScaleClass.Child:
                    if (Mathf.Abs(height - profile.ChildCardWorldHeightForReview) > 0.06f)
                    {
                        throw new InvalidOperationException($"House slice validation failed: P2-73 child marker {marker.CharacterIdForReview} height {height:0.###} drifted from the child scale ladder.");
                    }
                    break;
                default:
                    throw new InvalidOperationException($"House slice validation failed: P2-73 marker {marker.CharacterIdForReview} has an unsupported scale class.");
            }
        }

        private static void CreateHd2dAutonomousP2CharacterSpriteScaleReferenceDoorway(Transform root, Materials materials, FastVsHd2dCharacterSpriteScaleProfile profile)
        {
            var frame = materials.CurrentFrame != null ? materials.CurrentFrame : materials.CurrentFurniture;
            var dark = materials.DoorwayDark != null ? materials.DoorwayDark : materials.Shadow;
            var threshold = materials.Threshold != null ? materials.Threshold : materials.CurrentPath;
            var meterGauge = materials.PreviewFrame != null ? materials.PreviewFrame : threshold;
            var adultGauge = materials.RedMarker != null ? materials.RedMarker : frame;
            CreateLandmarkCube("P2_73_ReferenceDoor_InteriorDark", root, new Vector3(2.28f, profile.DoorwayReferenceWorldHeightForReview * 0.5f, 0.18f), new Vector3(0.84f, profile.DoorwayReferenceWorldHeightForReview, 0.06f), Quaternion.identity, dark, false, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, "p2_73.reference_door.dark");
            CreateLandmarkCube("P2_73_ReferenceDoor_LeftJamb", root, new Vector3(1.80f, profile.DoorwayReferenceWorldHeightForReview * 0.5f, 0.13f), new Vector3(0.08f, profile.DoorwayReferenceWorldHeightForReview, 0.11f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, "p2_73.reference_door.left_jamb");
            CreateLandmarkCube("P2_73_ReferenceDoor_RightJamb", root, new Vector3(2.76f, profile.DoorwayReferenceWorldHeightForReview * 0.5f, 0.13f), new Vector3(0.08f, profile.DoorwayReferenceWorldHeightForReview, 0.11f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, "p2_73.reference_door.right_jamb");
            CreateLandmarkCube("P2_73_ReferenceDoor_Lintel", root, new Vector3(2.28f, profile.DoorwayReferenceWorldHeightForReview + 0.05f, 0.13f), new Vector3(1.10f, 0.10f, 0.12f), Quaternion.identity, frame, false, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, "p2_73.reference_door.lintel");
            CreateLandmarkCube("P2_73_FootBaseline", root, new Vector3(-0.06f, 0.006f, -0.02f), new Vector3(4.82f, 0.012f, 0.08f), Quaternion.identity, threshold, false, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, "p2_73.foot_baseline");
            CreateLandmarkCube("P2_73_OneMeterGauge", root, new Vector3(-2.15f, 0.50f, 0.11f), new Vector3(0.045f, 1.00f, 0.06f), Quaternion.identity, meterGauge, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "p2_73.one_meter_gauge");
            CreateLandmarkCube("P2_73_AdultHeightGauge", root, new Vector3(-2.00f, profile.StandardAdultCardWorldHeightForReview * 0.5f, 0.11f), new Vector3(0.045f, profile.StandardAdultCardWorldHeightForReview, 0.06f), Quaternion.identity, adultGauge, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "p2_73.adult_height_gauge");
        }

        private static void CreateHd2dAutonomousP2CharacterSpriteScaleLineupCharacter(
            Transform root,
            string objectName,
            string displayName,
            float localX,
            float height,
            Material spriteMaterial,
            Materials materials,
            Camera camera)
        {
            var character = new GameObject($"FastVS_HD2D_P2_73_Lineup_{objectName}");
            character.transform.SetParent(root, false);
            character.transform.localPosition = new Vector3(localX, 0f, 0f);
            character.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            character.transform.localScale = Vector3.one;
            var billboard = character.AddComponent<FastVsPaperBillboard>();
            SerializedSet(billboard, "targetCamera", camera);

            var renderer = CreateSpriteCardParts(character.transform, $"P2_73_{displayName}", height, spriteMaterial, false, materials.Label);
            ConfigureHd2dAutonomousP2CharacterSpriteScaleMarker(renderer, displayName, height, true);
            var shadow = CreateCharacterContactShadow(
                $"FastVS_HD2D_P2_73_Lineup_{objectName}_ContactShadow",
                root,
                new Vector3(localX, 0.034f, 0.035f),
                new Vector3(height * 0.62f, 0.22f, 1f),
                EnsureCharacterContactShadowMaterial("chapter1_runtime_character_contact_shadow"));
            var shadowRenderer = shadow.GetComponent<MeshRenderer>();
            if (shadowRenderer != null)
            {
                shadowRenderer.sortingOrder = -2;
            }
        }

        private static Material EnsureHd2dAutonomousP2CharacterSpriteScaleReviewMaterial(string materialToken, string texturePath, Color tint, int frameCount)
        {
            EnsureTextureImporter(texturePath);
            var texture = EnsureShadedSpriteTexture(Hd2dAutonomousP2CharacterSpriteScaleReviewMaterialPrefix + materialToken, texturePath);
            if (texture == null)
            {
                throw new InvalidOperationException($"Fast VS autonomous P2-73 character scale review material failed: missing texture {texturePath}.");
            }

            var materialId = Hd2dAutonomousP2CharacterSpriteScaleReviewMaterialPrefix + materialToken;
            var materialPath = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(materialPath);
            var shader = Shader.Find(SpriteCardRampShaderName) ?? Shader.Find(URPUnlitShaderName);
            if (shader == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-73 character scale review material failed: no sprite-card/unlit shader found.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, materialPath);
            }

            material.shader = shader;
            ConfigureTransparentMaterial(material, 3018, SpriteCardRampShaderName, URPUnlitShaderName);
            AssignMaterialTexture(material, texture, frameCount > 1 ? new Vector2(1f / frameCount, 1f) : Vector2.one);
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", tint);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", tint);
            }

            ApplySpriteCardRampProfile(material);
            ApplyMaterialRole(material, materialId, FastVsHd2dMaterialRole.SpriteCard);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static Hd2dAutonomousP2CharacterSpriteScaleStats MeasureHd2dAutonomousP2CharacterSpriteScaleStats()
        {
            var profile = EnsureHd2dAutonomousP2CharacterSpriteScaleProfile();
            var stats = new Hd2dAutonomousP2CharacterSpriteScaleStats();
            var markers = UnityEngine.Object.FindObjectsByType<FastVsHd2dCharacterSpriteScaleMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (var i = 0; i < markers.Length; i++)
            {
                var marker = markers[i];
                if (marker == null || marker.RendererForReview == null || !marker.gameObject.scene.IsValid())
                {
                    continue;
                }

                stats.MarkerCount++;
                if (marker.ReviewLineupCardForReview)
                {
                    stats.ReviewLineupCount++;
                    stats.MaxLineupTexelDensityDeviation = Mathf.Max(
                        stats.MaxLineupTexelDensityDeviation,
                        Mathf.Abs(marker.ActualTexelsPerWorldUnitForReview - profile.StandardTexelsPerWorldUnitForReview));
                }

                stats.MaxHeightDeviation = Mathf.Max(stats.MaxHeightDeviation, Mathf.Abs(marker.AuthoredWorldHeightForReview - marker.ExpectedWorldHeightForReview));
                stats.MaxWidthDeviation = Mathf.Max(stats.MaxWidthDeviation, Mathf.Abs(marker.AuthoredWorldWidthForReview - marker.ExpectedWorldWidthForReview));
                stats.MaxVisualFootPivotDeviation = Mathf.Max(stats.MaxVisualFootPivotDeviation, Mathf.Abs(marker.VisualFootLocalYForReview));
                stats.MinTexelsPerWorldUnit = stats.MinTexelsPerWorldUnit <= 0f
                    ? marker.ActualTexelsPerWorldUnitForReview
                    : Mathf.Min(stats.MinTexelsPerWorldUnit, marker.ActualTexelsPerWorldUnitForReview);
                stats.MaxTexelsPerWorldUnit = Mathf.Max(stats.MaxTexelsPerWorldUnit, marker.ActualTexelsPerWorldUnitForReview);
            }

            return stats;
        }

        private static void WriteHd2dAutonomousP2CharacterSpriteScaleReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            FastVsHd2dCharacterSpriteScaleProfile profile,
            Hd2dAutonomousP2CharacterSpriteScaleStats stats)
        {
            var builder = new StringBuilder();
            builder.AppendLine("# P2-73 Character Sprite Scale Standard Review");
            builder.AppendLine();
            builder.AppendLine("- Status: auto-safe implemented");
            builder.AppendLine("- World unit: 1 unit = 1 meter");
            builder.AppendLine($"- Frame contract: {profile.FramePixelWidthForReview}x{profile.FramePixelHeightForReview}px, {profile.LoopFrameCountForReview}f loops");
            builder.AppendLine($"- Standard review adult card: {profile.StandardAdultCardWorldHeightForReview:0.###}m high x {profile.StandardAdultCardWorldWidthForReview:0.###}m wide");
            builder.AppendLine($"- Standard texel density: {profile.StandardTexelsPerWorldUnitForReview:0.###} vertical px/world unit");
            builder.AppendLine($"- Visual-foot pivot: bottom-center after {profile.TransparentFootPixelsForReview:0.###} transparent source pixels");
            builder.AppendLine($"- Doorway reference height: {profile.DoorwayReferenceWorldHeightForReview:0.###}m");
            builder.AppendLine();
            builder.AppendLine("## Captures");
            for (var i = 0; i < screenshotFiles.Count; i++)
            {
                builder.AppendLine($"- `{screenshotFiles[i]}`");
            }

            builder.AppendLine();
            builder.AppendLine("## Validation Metrics");
            builder.AppendLine($"- Marked character cards: {stats.MarkerCount}");
            builder.AppendLine($"- Review lineup cards: {stats.ReviewLineupCount}");
            builder.AppendLine($"- Max authored height deviation: {stats.MaxHeightDeviation:0.####}m");
            builder.AppendLine($"- Max authored width deviation: {stats.MaxWidthDeviation:0.####}m");
            builder.AppendLine($"- Max visual-foot pivot deviation: {stats.MaxVisualFootPivotDeviation:0.####}m");
            builder.AppendLine($"- Review lineup texel-density max deviation: {stats.MaxLineupTexelDensityDeviation:0.####} px/unit");
            builder.AppendLine($"- Scene texel-density range: {stats.MinTexelsPerWorldUnit:0.###} to {stats.MaxTexelsPerWorldUnit:0.###} px/unit");
            builder.AppendLine();
            builder.AppendLine("## Self Review");
            builder.AppendLine("- PASS: Four review characters share the same 64x96 frame contract, card height, and fixed-camera texel density.");
            builder.AppendLine("- PASS: The doorway and adult-height gauge provide a known-size scale reference without changing existing gameplay character placements.");
            builder.AppendLine("- PASS: Runtime characters keep their existing small-adult/child scale ladder; Luna remains a child-scale exception.");
            builder.AppendLine("- PASS: Character source and shaded textures are Point-filtered with no mipmaps through the existing importer/shaded texture path.");
            File.WriteAllText(Path.Combine(outputDirectory, "p2_73_character_sprite_scale_standard_review.md"), builder.ToString());
        }

        private static void CreateHd2dAutonomousP2CharacterSpriteScaleTwoXCrop(string sourcePath, string destinationPath)
        {
            var source = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            if (!source.LoadImage(File.ReadAllBytes(sourcePath), false))
            {
                throw new InvalidOperationException($"Fast VS autonomous P2-73 crop failed: could not load {sourcePath}.");
            }

            var cropWidth = Mathf.Max(64, source.width / 2);
            var cropHeight = Mathf.Max(64, source.height / 2);
            var cropX = Mathf.Clamp((source.width - cropWidth) / 2, 0, Mathf.Max(0, source.width - cropWidth));
            var cropY = Mathf.Clamp(source.height / 5, 0, Mathf.Max(0, source.height - cropHeight));
            var scaled = new Texture2D(cropWidth * 2, cropHeight * 2, TextureFormat.RGBA32, false);
            scaled.filterMode = FilterMode.Point;
            for (var y = 0; y < scaled.height; y++)
            {
                var sourceY = cropY + (y / 2);
                for (var x = 0; x < scaled.width; x++)
                {
                    var sourceX = cropX + (x / 2);
                    scaled.SetPixel(x, y, source.GetPixel(sourceX, sourceY));
                }
            }

            scaled.Apply(false, false);
            File.WriteAllBytes(destinationPath, scaled.EncodeToPNG());
            UnityEngine.Object.DestroyImmediate(source);
            UnityEngine.Object.DestroyImmediate(scaled);
        }

        private static bool IsHd2dAutonomousP2CharacterSpriteCardRenderer(Renderer renderer)
        {
            if (renderer == null || renderer.sharedMaterial == null || renderer.gameObject == null)
            {
                return false;
            }

            if (renderer.gameObject.name.EndsWith("_Sprite64x96", StringComparison.Ordinal))
            {
                return true;
            }

            var materialRole = renderer.sharedMaterial.GetTag(MaterialRoleTagName, false, string.Empty);
            if (!string.IsNullOrEmpty(materialRole) &&
                !string.Equals(materialRole, FastVsHd2dMaterialRole.SpriteCard.ToString(), StringComparison.Ordinal))
            {
                return false;
            }

            var materialName = renderer.sharedMaterial.name;
            return materialName.IndexOf("_sprite", StringComparison.OrdinalIgnoreCase) >= 0 &&
                renderer.transform.localScale.y >= 0.72f &&
                renderer.transform.localScale.y <= 1.24f &&
                Mathf.Abs(renderer.transform.localScale.x - renderer.transform.localScale.y * (NiroExpectedTextureWidth / (float)NiroExpectedTextureHeight)) < 0.05f;
        }

        private static string ResolveHd2dAutonomousP2CharacterDisplayName(Renderer renderer)
        {
            if (renderer == null)
            {
                return "Unknown";
            }

            var objectName = renderer.gameObject.name;
            var suffix = "_Sprite64x96";
            if (objectName.EndsWith(suffix, StringComparison.Ordinal))
            {
                objectName = objectName.Substring(0, objectName.Length - suffix.Length);
            }

            if (objectName.StartsWith("P2_73_", StringComparison.Ordinal))
            {
                objectName = objectName.Substring("P2_73_".Length);
            }

            return string.IsNullOrWhiteSpace(objectName) ? "Unknown" : objectName;
        }

        private static FastVsHd2dCharacterSpriteScaleClass ResolveHd2dAutonomousP2CharacterSpriteScaleClass(
            string displayName,
            Renderer renderer,
            float height,
            bool reviewLineupCard,
            FastVsHd2dCharacterSpriteScaleProfile profile)
        {
            if (reviewLineupCard)
            {
                return FastVsHd2dCharacterSpriteScaleClass.ReviewStandard;
            }

            var token = $"{displayName} {renderer?.transform.parent?.name ?? string.Empty} {renderer?.gameObject.name ?? string.Empty}";
            if (token.IndexOf("Luna", StringComparison.OrdinalIgnoreCase) >= 0 || height <= profile.ChildCardWorldHeightForReview + 0.06f)
            {
                return FastVsHd2dCharacterSpriteScaleClass.Child;
            }

            if (token.IndexOf("Reto", StringComparison.OrdinalIgnoreCase) >= 0 && token.IndexOf("Desk", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return FastVsHd2dCharacterSpriteScaleClass.SeatedOrDesk;
            }

            if (height >= profile.StandardAdultCardWorldHeightForReview - 0.05f)
            {
                return FastVsHd2dCharacterSpriteScaleClass.StandardAdult;
            }

            return FastVsHd2dCharacterSpriteScaleClass.SmallAdult;
        }

        private static string BuildHd2dAutonomousP2CharacterSpriteScaleId(Renderer renderer, string displayName)
        {
            var parentName = renderer == null || renderer.transform.parent == null ? "scene" : renderer.transform.parent.name;
            var cardName = renderer == null ? "card" : renderer.gameObject.name;
            return $"{parentName}.{displayName}.{cardName}";
        }

        private static Vector3 GetHd2dAutonomousP2CharacterSpriteScaleLineupLocalPosition()
        {
            return CentralPlazaVsCenter + new Vector3(0.55f, 0.02f, -3.55f);
        }

        private static void SetHd2dAutonomousP2CharacterSpriteScaleLayerRecursively(Transform root, int layer)
        {
            if (root == null)
            {
                return;
            }

            root.gameObject.layer = layer;
            for (var i = 0; i < root.childCount; i++)
            {
                SetHd2dAutonomousP2CharacterSpriteScaleLayerRecursively(root.GetChild(i), layer);
            }
        }

        private struct Hd2dAutonomousP2CharacterSpriteScaleStats
        {
            public int MarkerCount;
            public int ReviewLineupCount;
            public float MaxHeightDeviation;
            public float MaxWidthDeviation;
            public float MaxVisualFootPivotDeviation;
            public float MaxLineupTexelDensityDeviation;
            public float MinTexelsPerWorldUnit;
            public float MaxTexelsPerWorldUnit;
        }
    }
}
