using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.FastVS.SunCycle;
using Anemora.TimeManagement;
using Unity.Cinemachine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2GroupTargetFramingRootName = "Current_CentralPlaza_P2_70_GroupTargetFramingReview";
        private const string Hd2dAutonomousP2GroupTargetFramingPreviewName = "P2_70_GroupTargetFramingPreview";
        private const string Hd2dAutonomousP2GroupTargetGroupName = "P2_70_CinemachineTargetGroup";
        private const string Hd2dAutonomousP2GroupCameraName = "P2_70_GroupFramingCinemachineCamera_InactivePreview";
        private const string Hd2dAutonomousP2GroupTargetFramingProfilePath = "Assets/Settings/FastVS_HD2D_P2_GroupTargetFramingProfile.asset";
        private const string Hd2dAutonomousP2GroupTargetFramingProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dGroupTargetFramingProfile.cs";
        private const string Hd2dAutonomousP2GroupTargetFramingPreviewRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dGroupTargetFramingPreview.cs";
        private const string Hd2dAutonomousP2GroupTargetFramingAllyMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p2_group_framing_actor_ally.mat";
        private const string Hd2dAutonomousP2GroupTargetFramingEnemyMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p2_group_framing_actor_enemy.mat";
        private const string Hd2dAutonomousP2GroupTargetFramingSpeakerMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p2_group_framing_actor_speaker.mat";

        private static readonly Vector3[] Hd2dAutonomousP2GroupTargetFramingBattle3v3LocalPositions =
        {
            CentralPlazaVsCenter + new Vector3(-1.80f, 0.72f, 2.05f),
            CentralPlazaVsCenter + new Vector3(-0.58f, 0.72f, 1.82f),
            CentralPlazaVsCenter + new Vector3(0.62f, 0.72f, 2.08f),
            CentralPlazaVsCenter + new Vector3(1.78f, 0.72f, 3.28f),
            CentralPlazaVsCenter + new Vector3(2.80f, 0.72f, 3.03f),
            CentralPlazaVsCenter + new Vector3(3.65f, 0.72f, 3.46f)
        };

        private static readonly Vector3[] Hd2dAutonomousP2GroupTargetFramingBattleWideLocalPositions =
        {
            CentralPlazaVsCenter + new Vector3(-2.70f, 0.72f, 2.00f),
            CentralPlazaVsCenter + new Vector3(-1.18f, 0.72f, 1.76f),
            CentralPlazaVsCenter + new Vector3(0.44f, 0.72f, 2.12f),
            CentralPlazaVsCenter + new Vector3(1.92f, 0.72f, 3.24f),
            CentralPlazaVsCenter + new Vector3(3.30f, 0.72f, 3.02f),
            CentralPlazaVsCenter + new Vector3(4.58f, 0.72f, 3.46f),
            CentralPlazaVsCenter + new Vector3(6.35f, 0.78f, 3.18f)
        };

        private static readonly Vector3[] Hd2dAutonomousP2GroupTargetFramingDialogueLocalPositions =
        {
            CentralPlazaVsCenter + new Vector3(-1.10f, 0.72f, 2.22f),
            CentralPlazaVsCenter + new Vector3(1.18f, 0.72f, 2.28f)
        };

        private static readonly bool[] Hd2dAutonomousP2GroupTargetFramingBattle3v3EnemyFlags =
        {
            false,
            false,
            false,
            true,
            true,
            true
        };

        private static readonly bool[] Hd2dAutonomousP2GroupTargetFramingBattleWideEnemyFlags =
        {
            false,
            false,
            false,
            true,
            true,
            true,
            true
        };

        public static void CaptureHd2dAutonomousP2Item70GroupTargetFramingBatch()
        {
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            if (FindSceneObjectIncludingInactive(Hd2dAutonomousP2GroupTargetFramingRootName) == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-70 group target framing capture failed: review root is missing. Run BuildAndValidateBatch before capture.");
            }

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var preview = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dGroupTargetFramingPreview>(FindObjectsInactive.Include);
            var profile = EnsureHd2dAutonomousP2GroupTargetFramingProfile();
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var sunDriver = sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : null;
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || preview == null || profile == null || sunDriver == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-70 group target framing capture failed: required review components are missing.");
            }

            ValidateHd2dAutonomousP2GroupTargetFraming();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("group_target_framing_combat_dialogue");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_manual_follow_baseline_unframed_group.png",
                "02_battle_3v3_group_framed_safe_margin.png",
                "03_battle_3v4_auto_pullback_wide_enemy.png",
                "04_dialogue_speaker_left_thirds_headroom.png",
                "05_dialogue_speaker_right_option_for_tom.png"
            };
            var frameRows = new List<string>();
            FastVsHd2dGroupTargetFramingPreview.FrameResult battle3v3Result = default;
            FastVsHd2dGroupTargetFramingPreview.FrameResult battleWideResult = default;
            FastVsHd2dGroupTargetFramingPreview.FrameResult dialogueLeftResult = default;
            FastVsHd2dGroupTargetFramingPreview.FrameResult dialogueRightResult = default;

            var previousMask = camera.cullingMask;
            try
            {
                guide.SetMovementFrozen(true);
                controller.ClosePortal();
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.30f, 0.02f, 2.42f));
                visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
                guide.ApplyActiveTimeIsolationForReview();
                sunDriver.ApplyPreset(SunPreset.Noon, true);
                realtimeRig.ApplyNowForReview();
                camera.cullingMask = ResolveCurrentTimeReviewCullingMask(controller, previousMask);

                if (!preview.ApplyScenarioForReview(
                        "manual baseline target placement",
                        controller.CurrentSpaceRootForReview,
                        Hd2dAutonomousP2GroupTargetFramingBattle3v3LocalPositions,
                        Hd2dAutonomousP2GroupTargetFramingBattle3v3EnemyFlags,
                        -1,
                        false,
                        false,
                        camera,
                        out _))
                {
                    throw new InvalidOperationException("Fast VS autonomous P2-70 capture failed: baseline target placement did not apply.");
                }

                ApplyHd2dAutonomousP2GroupTargetFramingActorMaterials(
                    Hd2dAutonomousP2GroupTargetFramingBattle3v3LocalPositions.Length,
                    Hd2dAutonomousP2GroupTargetFramingBattle3v3EnemyFlags,
                    -1);
                PositionReviewCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(CentralPlazaVsCenter + new Vector3(0.82f, 0.02f, 2.58f)));
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, screenshotFiles[0]));
                ValidateScreenshotOutputExists(outputDirectory, screenshotFiles[0]);

                frameRows.Add(CaptureHd2dAutonomousP2GroupTargetFramingShot(
                    preview,
                    controller.CurrentSpaceRootForReview,
                    camera,
                    outputDirectory,
                    screenshotFiles[1],
                    "battle 3v3 group-framed: all allies/enemies inside safe margin",
                    Hd2dAutonomousP2GroupTargetFramingBattle3v3LocalPositions,
                    Hd2dAutonomousP2GroupTargetFramingBattle3v3EnemyFlags,
                    -1,
                    false,
                    false,
                    out battle3v3Result));

                frameRows.Add(CaptureHd2dAutonomousP2GroupTargetFramingShot(
                    preview,
                    controller.CurrentSpaceRootForReview,
                    camera,
                    outputDirectory,
                    screenshotFiles[2],
                    "battle 3v4 wide enemy: seventh participant forces conservative pullback",
                    Hd2dAutonomousP2GroupTargetFramingBattleWideLocalPositions,
                    Hd2dAutonomousP2GroupTargetFramingBattleWideEnemyFlags,
                    -1,
                    false,
                    false,
                    out battleWideResult));

                frameRows.Add(CaptureHd2dAutonomousP2GroupTargetFramingShot(
                    preview,
                    controller.CurrentSpaceRootForReview,
                    camera,
                    outputDirectory,
                    screenshotFiles[3],
                    "dialogue option A: left speaker held near thirds with extra headroom",
                    Hd2dAutonomousP2GroupTargetFramingDialogueLocalPositions,
                    new[] { false, false },
                    0,
                    false,
                    true,
                    out dialogueLeftResult));

                frameRows.Add(CaptureHd2dAutonomousP2GroupTargetFramingShot(
                    preview,
                    controller.CurrentSpaceRootForReview,
                    camera,
                    outputDirectory,
                    screenshotFiles[4],
                    "dialogue option B for Tom: right speaker mirrored near thirds",
                    Hd2dAutonomousP2GroupTargetFramingDialogueLocalPositions,
                    new[] { false, false },
                    1,
                    true,
                    true,
                    out dialogueRightResult));
            }
            finally
            {
                camera.cullingMask = previousMask;
                preview.SetAllParticipantsVisibleForReview(false);
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.34f, 0.02f, 3.26f));
                controller.ClosePortal();
                guide.SetMovementFrozen(false);
                guide.ApplyActiveTimeIsolationForReview();
                sunDriver.ApplyPreset(SunPreset.Morning, true);
                realtimeRig.ApplyNowForReview();
                AssetDatabase.SaveAssets();
            }

            var baselineDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var pullbackDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[1], screenshotFiles[2]);
            var dialogueDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[3], screenshotFiles[4]);
            WriteHd2dAutonomousP2GroupTargetFramingReviewReport(
                outputDirectory,
                screenshotFiles,
                frameRows,
                profile,
                preview,
                battle3v3Result,
                battleWideResult,
                dialogueLeftResult,
                dialogueRightResult,
                baselineDiff,
                pullbackDiff,
                dialogueDiff);
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-70 group target framing review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2GroupTargetFraming(Transform currentCentralPlazaRoot, Camera camera)
        {
            var profile = EnsureHd2dAutonomousP2GroupTargetFramingProfile();
            var allyMaterial = EnsureHd2dAutonomousP2GroupFramingActorMaterial(Hd2dAutonomousP2GroupTargetFramingAllyMaterialPath, new Color(0.36f, 0.76f, 1.0f, 0.96f));
            var enemyMaterial = EnsureHd2dAutonomousP2GroupFramingActorMaterial(Hd2dAutonomousP2GroupTargetFramingEnemyMaterialPath, new Color(1.0f, 0.46f, 0.36f, 0.96f));
            var speakerMaterial = EnsureHd2dAutonomousP2GroupFramingActorMaterial(Hd2dAutonomousP2GroupTargetFramingSpeakerMaterialPath, new Color(1.0f, 0.82f, 0.42f, 0.98f));
            if (currentCentralPlazaRoot == null || camera == null || profile == null || allyMaterial == null || enemyMaterial == null || speakerMaterial == null)
            {
                return;
            }

            var existing = FindSceneObjectIncludingInactive(Hd2dAutonomousP2GroupTargetFramingRootName);
            if (existing != null)
            {
                UnityEngine.Object.DestroyImmediate(existing);
            }

            var root = new GameObject(Hd2dAutonomousP2GroupTargetFramingRootName);
            root.transform.SetParent(currentCentralPlazaRoot, false);
            root.transform.localPosition = Vector3.zero;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;

            var previewObject = new GameObject(Hd2dAutonomousP2GroupTargetFramingPreviewName);
            previewObject.transform.SetParent(root.transform, false);
            var preview = previewObject.AddComponent<FastVsHd2dGroupTargetFramingPreview>();

            var targetGroupObject = new GameObject(Hd2dAutonomousP2GroupTargetGroupName);
            targetGroupObject.transform.SetParent(root.transform, false);
            var targetGroup = targetGroupObject.AddComponent<CinemachineTargetGroup>();
            targetGroup.PositionMode = CinemachineTargetGroup.PositionModes.GroupCenter;
            targetGroup.RotationMode = CinemachineTargetGroup.RotationModes.Manual;
            targetGroup.UpdateMethod = CinemachineTargetGroup.UpdateMethods.LateUpdate;

            var cameraObject = new GameObject(Hd2dAutonomousP2GroupCameraName);
            cameraObject.transform.SetParent(root.transform, false);
            var groupCamera = cameraObject.AddComponent<CinemachineCamera>();
            var groupFraming = cameraObject.AddComponent<CinemachineGroupFraming>();
            var recomposer = cameraObject.AddComponent<CinemachineRecomposer>();
            groupCamera.Priority.Value = profile.InactivePriorityForReview;
            groupCamera.Follow = targetGroup.transform;
            groupCamera.LookAt = targetGroup.transform;
            var lens = groupCamera.Lens;
            lens.FieldOfView = profile.BaseFieldOfViewForReview;
            lens.NearClipPlane = 0.03f;
            lens.FarClipPlane = camera.farClipPlane;
            groupCamera.Lens = lens;

            var participants = new Transform[7];
            for (var index = 0; index < participants.Length; index++)
            {
                var material = index < 3 ? allyMaterial : enemyMaterial;
                participants[index] = CreateHd2dAutonomousP2GroupTargetFramingActor(
                    root.transform,
                    camera,
                    $"P2_70_GroupActor_{index + 1:00}_{(index < 3 ? "Ally" : "Enemy")}",
                    material,
                    index == 6 ? new Vector3(0.80f, 1.75f, 0.12f) : new Vector3(0.64f, 1.55f, 0.10f));
            }

            preview.ConfigureForReview(profile, camera, groupCamera, targetGroup, groupFraming, recomposer, participants);
            preview.SetAllParticipantsVisibleForReview(false);
            SetHd2dAutonomousP2GroupTargetFramingLayerRecursively(root, CurrentSpaceRenderLayer);
            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(preview);
            EditorUtility.SetDirty(targetGroup);
            EditorUtility.SetDirty(groupCamera);
            EditorUtility.SetDirty(groupFraming);
            EditorUtility.SetDirty(recomposer);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2GroupTargetFraming()
        {
            var profile = EnsureHd2dAutonomousP2GroupTargetFramingProfile();
            var preview = UnityEngine.Object.FindFirstObjectByType<FastVsHd2dGroupTargetFramingPreview>(FindObjectsInactive.Include);
            var camera = Camera.main;
            if (profile == null ||
                !profile.NeedsTomApprovalForReview ||
                profile.FinalGroupFramingApprovedForReview ||
                !profile.TargetGroupConfiguredForReview ||
                !profile.GroupFramingConfiguredForReview ||
                !profile.RecomposerConfiguredForReview ||
                !profile.DirectRuntimeCameraAuthorityDisabledForReview ||
                !profile.ConservativeDataPrepForReview ||
                profile.BaseFieldOfViewForReview < 22f ||
                profile.MaxFieldOfViewForReview > 34f ||
                profile.ViewportSafeMarginForReview < 0.06f ||
                preview == null ||
                !preview.IsReadyForReview ||
                !preview.DirectRuntimeCameraAuthorityDisabledForReview ||
                camera == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-70 needs conservative non-final group target framing profile, inactive CinemachineTargetGroup preview rig, safe-margin config, and runtime authority disabled.");
            }

            if (preview.GroupCameraForReview == null ||
                preview.GroupCameraForReview.Priority.Value != profile.InactivePriorityForReview ||
                preview.TargetGroupForReview == null ||
                preview.GroupFramingForReview == null ||
                preview.RecomposerForReview == null ||
                preview.ParticipantCapacityForReview < 7)
            {
                throw new InvalidOperationException("House slice validation failed: P2-70 preview rig must keep an inactive CinemachineCamera with TargetGroup, GroupFraming, Recomposer, and seven participant anchors.");
            }

            if (!preview.ApplyScenarioForReview(
                    "validation battle 3v3",
                    UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>()?.CurrentSpaceRootForReview,
                    Hd2dAutonomousP2GroupTargetFramingBattle3v3LocalPositions,
                    Hd2dAutonomousP2GroupTargetFramingBattle3v3EnemyFlags,
                    -1,
                    false,
                    false,
                    camera,
                    out var battle3v3) ||
                !battle3v3.AllActorsInsideSafeMargin)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-70 battle 3v3 framing must keep all six participants inside the configured safe margin (viewport min=({battle3v3.ViewportMin.x:0.###},{battle3v3.ViewportMin.y:0.###}), max=({battle3v3.ViewportMax.x:0.###},{battle3v3.ViewportMax.y:0.###}), distance={battle3v3.CameraDistance:0.###}, fov={battle3v3.FieldOfView:0.###}).");
            }

            if (!preview.ApplyScenarioForReview(
                    "validation battle 3v4",
                    UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>()?.CurrentSpaceRootForReview,
                    Hd2dAutonomousP2GroupTargetFramingBattleWideLocalPositions,
                    Hd2dAutonomousP2GroupTargetFramingBattleWideEnemyFlags,
                    -1,
                    false,
                    false,
                    camera,
                    out var battleWide) ||
                !battleWide.AllActorsInsideSafeMargin ||
                battleWide.ActiveParticipantCount != 7 ||
                battleWide.CameraDistance <= battle3v3.CameraDistance + 0.10f)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-70 wider 7-actor combat framing must stay safe and pull back vs 3v3 (3v3={battle3v3.CameraDistance:0.###} min=({battle3v3.ViewportMin.x:0.###},{battle3v3.ViewportMin.y:0.###}) max=({battle3v3.ViewportMax.x:0.###},{battle3v3.ViewportMax.y:0.###}), wide={battleWide.CameraDistance:0.###} min=({battleWide.ViewportMin.x:0.###},{battleWide.ViewportMin.y:0.###}) max=({battleWide.ViewportMax.x:0.###},{battleWide.ViewportMax.y:0.###})).");
            }

            if (!preview.ApplyScenarioForReview(
                    "validation dialogue left speaker",
                    UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>()?.CurrentSpaceRootForReview,
                    Hd2dAutonomousP2GroupTargetFramingDialogueLocalPositions,
                    new[] { false, false },
                    0,
                    false,
                    true,
                    camera,
                    out var dialogueLeft) ||
                !dialogueLeft.AllActorsInsideSafeMargin ||
                dialogueLeft.SpeakerViewportX >= 0.46f)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-70 dialogue left-speaker framing must keep the speaker near the left third with headroom (speaker x={dialogueLeft.SpeakerViewportX:0.###}).");
            }

            if (!preview.ApplyScenarioForReview(
                    "validation dialogue right speaker",
                    UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>()?.CurrentSpaceRootForReview,
                    Hd2dAutonomousP2GroupTargetFramingDialogueLocalPositions,
                    new[] { false, false },
                    1,
                    true,
                    true,
                    camera,
                    out var dialogueRight) ||
                !dialogueRight.AllActorsInsideSafeMargin ||
                dialogueRight.SpeakerViewportX <= 0.54f)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-70 dialogue right-speaker framing must mirror near the right third with headroom (speaker x={dialogueRight.SpeakerViewportX:0.###}).");
            }

            preview.SetAllParticipantsVisibleForReview(false);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2GroupTargetFramingProfileRuntimePath), "directRuntimeCameraAuthorityDisabled", Hd2dAutonomousP2GroupTargetFramingProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2GroupTargetFramingProfileRuntimePath), "finalGroupFramingApproved", Hd2dAutonomousP2GroupTargetFramingProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2GroupTargetFramingPreviewRuntimePath), "CinemachineTargetGroup", Hd2dAutonomousP2GroupTargetFramingPreviewRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2GroupTargetFramingPreviewRuntimePath), "CinemachineGroupFraming", Hd2dAutonomousP2GroupTargetFramingPreviewRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2GroupTargetFramingPreviewRuntimePath), "CinemachineRecomposer", Hd2dAutonomousP2GroupTargetFramingPreviewRuntimePath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.P2GroupTargetFraming.cs"), "CaptureHd2dAutonomousP2Item70GroupTargetFramingBatch", "Assets/Editor/AnemoraFastVsHouseSliceSetup.P2GroupTargetFraming.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2GroupTargetFraming", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2GroupTargetFraming", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
        }

        private static FastVsHd2dGroupTargetFramingProfile EnsureHd2dAutonomousP2GroupTargetFramingProfile()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dGroupTargetFramingProfile>(Hd2dAutonomousP2GroupTargetFramingProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dGroupTargetFramingProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2GroupTargetFramingProfilePath);
            }

            profile.ConfigureForReview(
                32f,
                32f,
                29f,
                5.45f,
                15.50f,
                0.72f,
                0.08f,
                0.54f,
                1.18f,
                0.46f,
                0.42f,
                1.0f,
                1.0f,
                1.85f,
                0.85f,
                0.25f,
                0.24f,
                0f,
                6,
                170,
                true,
                true,
                true,
                true,
                true,
                true,
                false,
                "Keep this as P2-70 camera data prep only. Tom should tune combat padding, dialogue thirds/headroom, and blend timing before this controls the live gameplay camera.");
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static Transform CreateHd2dAutonomousP2GroupTargetFramingActor(
            Transform parent,
            Camera camera,
            string objectName,
            Material material,
            Vector3 cardScale)
        {
            var actor = new GameObject(objectName);
            actor.transform.SetParent(parent, false);
            actor.transform.localPosition = Vector3.zero;
            actor.transform.localRotation = Quaternion.identity;
            actor.transform.localScale = Vector3.one;
            var billboard = actor.AddComponent<FastVsPaperBillboard>();
            SerializedSet(billboard, "targetCamera", camera);
            SerializedSet(billboard, "lockY", true);
            SerializedSet(billboard, "useCameraForward", true);

            var card = GameObject.CreatePrimitive(PrimitiveType.Cube);
            card.name = objectName + "_SpriteCard";
            card.transform.SetParent(actor.transform, false);
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;
            card.transform.localScale = cardScale;
            var collider = card.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = card.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }

            return actor.transform;
        }

        private static Material EnsureHd2dAutonomousP2GroupFramingActorMaterial(string path, Color color)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                var shader = Shader.Find(URPUnlitShaderName);
                if (shader == null)
                {
                    return null;
                }

                material = new Material(shader)
                {
                    name = Path.GetFileNameWithoutExtension(path),
                    renderQueue = (int)RenderQueue.AlphaTest
                };
                AssetDatabase.CreateAsset(material, path);
            }

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            if (material.HasProperty("_Surface"))
            {
                material.SetFloat("_Surface", 0f);
            }

            if (material.HasProperty("_AlphaClip"))
            {
                material.SetFloat("_AlphaClip", 0f);
            }

            if (material.HasProperty("_Cull"))
            {
                material.SetFloat("_Cull", 0f);
            }

            material.SetOverrideTag("RenderType", "Opaque");
            material.renderQueue = (int)RenderQueue.Geometry;
            material.enableInstancing = true;
            EditorUtility.SetDirty(material);
            return material;
        }

        private static string CaptureHd2dAutonomousP2GroupTargetFramingShot(
            FastVsHd2dGroupTargetFramingPreview preview,
            Transform activeRoot,
            Camera camera,
            string outputDirectory,
            string fileName,
            string label,
            Vector3[] localPositions,
            bool[] enemyFlags,
            int speakerIndex,
            bool speakerOnRight,
            bool dialogue,
            out FastVsHd2dGroupTargetFramingPreview.FrameResult result)
        {
            if (!preview.ApplyScenarioForReview(label, activeRoot, localPositions, enemyFlags, speakerIndex, speakerOnRight, dialogue, camera, out result))
            {
                throw new InvalidOperationException($"Fast VS autonomous P2-70 group target framing capture failed: scenario `{label}` did not apply.");
            }

            if (!result.AllActorsInsideSafeMargin)
            {
                throw new InvalidOperationException($"Fast VS autonomous P2-70 group target framing capture failed: scenario `{label}` left actors outside the safe margin.");
            }

            ApplyHd2dAutonomousP2GroupTargetFramingActorMaterials(localPositions.Length, enemyFlags, speakerIndex);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateScreenshotOutputExists(outputDirectory, fileName);
            return result.ToReportRow(fileName);
        }

        private static void ApplyHd2dAutonomousP2GroupTargetFramingActorMaterials(int activeCount, bool[] enemyFlags, int speakerIndex)
        {
            var allyMaterial = EnsureHd2dAutonomousP2GroupFramingActorMaterial(Hd2dAutonomousP2GroupTargetFramingAllyMaterialPath, new Color(0.36f, 0.76f, 1.0f, 0.96f));
            var enemyMaterial = EnsureHd2dAutonomousP2GroupFramingActorMaterial(Hd2dAutonomousP2GroupTargetFramingEnemyMaterialPath, new Color(1.0f, 0.46f, 0.36f, 0.96f));
            var speakerMaterial = EnsureHd2dAutonomousP2GroupFramingActorMaterial(Hd2dAutonomousP2GroupTargetFramingSpeakerMaterialPath, new Color(1.0f, 0.82f, 0.42f, 0.98f));
            if (allyMaterial == null || enemyMaterial == null || speakerMaterial == null)
            {
                return;
            }

            for (var index = 0; index < 7; index++)
            {
                var actor = FindSceneObjectIncludingInactive($"P2_70_GroupActor_{index + 1:00}_{(index < 3 ? "Ally" : "Enemy")}");
                var renderer = actor != null ? actor.GetComponentInChildren<MeshRenderer>(true) : null;
                if (renderer == null)
                {
                    continue;
                }

                var isEnemy = enemyFlags != null && index < enemyFlags.Length ? enemyFlags[index] : index >= 3;
                renderer.sharedMaterial = index < activeCount && index == speakerIndex ? speakerMaterial : isEnemy ? enemyMaterial : allyMaterial;
                EditorUtility.SetDirty(renderer);
            }
        }

        private static int ResolveCurrentTimeReviewCullingMask(TimeWindowPairedSpacePortalController controller, int previousMask)
        {
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            return ((previousMask & ~otherBit) | currentBit | playerBit);
        }

        private static void SetHd2dAutonomousP2GroupTargetFramingLayerRecursively(GameObject root, int layer)
        {
            if (root == null)
            {
                return;
            }

            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetHd2dAutonomousP2GroupTargetFramingLayerRecursively(child.gameObject, layer);
            }
        }

        private static void WriteHd2dAutonomousP2GroupTargetFramingReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> frameRows,
            FastVsHd2dGroupTargetFramingProfile profile,
            FastVsHd2dGroupTargetFramingPreview preview,
            FastVsHd2dGroupTargetFramingPreview.FrameResult battle3v3,
            FastVsHd2dGroupTargetFramingPreview.FrameResult battleWide,
            FastVsHd2dGroupTargetFramingPreview.FrameResult dialogueLeft,
            FastVsHd2dGroupTargetFramingPreview.FrameResult dialogueRight,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics baselineDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics pullbackDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics dialogueDiff)
        {
            var lines = new List<string>
            {
                "# P2-70 Group / Target Framing Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative camera data prep for combat/dialogue group framing. The CinemachineTargetGroup, CinemachineGroupFraming, and Recomposer contract is present, but the preview camera remains inactive and does not take runtime authority.",
                "- Review note: the actor cards are diagnostic placeholders for framing only. Final combat composition, blend timing, dialogue thirds, and camera feel remain Tom-facing decisions.",
                "- Recommendation: " + profile.RecommendationForReview,
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2GroupTargetFramingProfilePath}` |",
                $"| Runtime preview | `{Hd2dAutonomousP2GroupTargetFramingPreviewRuntimePath}` |",
                $"| Needs Tom approval / final approved | {FormatBool(profile.NeedsTomApprovalForReview)} / {FormatBool(profile.FinalGroupFramingApprovedForReview)} |",
                $"| Runtime camera authority disabled | {FormatBool(profile.DirectRuntimeCameraAuthorityDisabledForReview && preview.DirectRuntimeCameraAuthorityDisabledForReview)} |",
                $"| TargetGroup / GroupFraming / Recomposer | {FormatBool(preview.TargetGroupForReview != null)} / {FormatBool(preview.GroupFramingForReview != null)} / {FormatBool(preview.RecomposerForReview != null)} |",
                $"| Inactive preview priority / planned live priority | {profile.InactivePriorityForReview} / {profile.PreviewPriorityForReview} |",
                $"| FOV base/max / pitch | {profile.BaseFieldOfViewForReview:0.#} / {profile.MaxFieldOfViewForReview:0.#} / {profile.PitchDegreesForReview:0.#} |",
                $"| Distance base/max | {profile.BaseDistanceForReview:0.###} / {profile.MaxDistanceForReview:0.###} |",
                $"| Group framing size / viewport safe margin | {profile.GroupFramingSizeForReview:0.###} / {profile.ViewportSafeMarginForReview:0.###} |",
                $"| Dialogue offset / headroom | {profile.DialogueScreenOffsetForReview:0.###} / {profile.DialogueHeadroomForReview:0.###} |",
                $"| 3v3 vs 3v4 pullback distance | {battle3v3.CameraDistance:0.###} -> {battleWide.CameraDistance:0.###} |",
                $"| Dialogue speaker viewport X left/right | {dialogueLeft.SpeakerViewportX:0.###} / {dialogueRight.SpeakerViewportX:0.###} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                baselineDiff.ToReportRow("Manual follow baseline vs 3v3 group framed"),
                pullbackDiff.ToReportRow("3v3 framed vs 3v4 wider pullback"),
                dialogueDiff.ToReportRow("Dialogue left-speaker vs right-speaker option"),
                string.Empty,
                "| Screenshot | Scenario | Actors | Safe margin | Distance | FOV | Viewport min | Viewport max | Speaker viewport X |",
                "|---|---|---:|---|---:|---:|---|---|---:|"
            };
            lines.AddRange(frameRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|"
            });

            for (var i = 0; i < screenshotFiles.Count; i++)
            {
                var file = screenshotFiles[i];
                ValidateScreenshotOutputExists(outputDirectory, file);
                lines.Add($"| `{file}` | P2-70 group target framing capture {i + 1}. |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "group_target_framing_combat_dialogue_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }
    }
}
