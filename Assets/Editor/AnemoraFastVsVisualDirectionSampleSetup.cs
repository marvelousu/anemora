using System;
using System.IO;
using Anemora.FastVS;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsVisualDirectionSampleSetup
    {
        public const string ScenePath = "Assets/Scenes/Anemora_FastVS_VisualDirectionSample.unity";
        public const string BuildDirectory = "Builds/FastVS_VisualDirectionSample";
        public const string BuildExePath = BuildDirectory + "/Anemora_FastVS_VisualDirectionSample.exe";

        private const string MaterialDirectory = "Assets/Art/Materials/FastVS/VisualDirectionSample";
        private const float PortalLocalZ = -2.15f;
        private static readonly Vector2 RegionSize = new Vector2(7.8f, 7.0f);
        private static readonly Vector2 DragStart = new Vector2(380f, 220f);
        private static readonly Vector2 DragEnd = new Vector2(850f, 600f);

        [MenuItem("Anemora/Fast VS/Create Visual Direction Sample")]
        public static void CreateSampleScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EnsureFolder(MaterialDirectory);
            EnsureFolder("Assets/Scenes");
            var materials = EnsureMaterials();

            var currentRoot = new GameObject("FastVS_Current_RuinedLibrary_DirectionSample").transform;
            currentRoot.position = new Vector3(-4.7f, 0f, 0f);

            var otherRoot = new GameObject("FastVS_Past_RestoredLibrary_DirectionSample").transform;
            otherRoot.position = new Vector3(4.7f, 0f, 0f);

            CreateLibraryDirectionSpace(currentRoot, "Current", false, materials);
            CreateLibraryDirectionSpace(otherRoot, "Past", true, materials);

            var camera = CreateCamera(currentRoot);
            CreateLighting();
            var player = CreateNiroPlayer(currentRoot, camera, materials);
            var controller = CreateController(camera, currentRoot, otherRoot, player, materials);
            CreateGuide(camera, controller, player);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Fast VS visual direction sample scene created: {ScenePath}");
        }

        public static void ValidateSampleSceneBatch()
        {
            if (!File.Exists(ScenePath))
            {
                CreateSampleScene();
            }

            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            if (controller == null)
            {
                throw new InvalidOperationException("Fast VS sample validation failed: missing TimeWindow controller.");
            }

            controller.ClosePortal();
            if (!controller.TryOpenPortalForTests(DragStart, DragEnd))
            {
                throw new InvalidOperationException("Fast VS sample validation failed: V24 portal creation rejected.");
            }

            if (!controller.HasPortalPair || !controller.HasLiveApertureViewForReview)
            {
                throw new InvalidOperationException("Fast VS sample validation failed: live aperture portal was not created.");
            }

            var portalLocal = controller.PortalLocalCenterForReview;
            controller.TransferCurrentToOtherForReview(new Vector3(portalLocal.x, 0.72f, portalLocal.z + 0.18f));
            if (!controller.PlayerInOtherTime)
            {
                throw new InvalidOperationException("Fast VS sample validation failed: current-to-past transfer did not occur.");
            }

            controller.TransferOtherToCurrentForReview(new Vector3(portalLocal.x, 0.72f, portalLocal.z - 0.18f));
            if (controller.PlayerInOtherTime)
            {
                throw new InvalidOperationException("Fast VS sample validation failed: return transfer did not occur.");
            }

            Debug.Log("Fast VS visual direction sample validation passed.");
        }

        [MenuItem("Anemora/Fast VS/Build Visual Direction Sample")]
        public static void BuildSamplePlayer()
        {
            if (!File.Exists(ScenePath))
            {
                CreateSampleScene();
            }

            Directory.CreateDirectory(BuildDirectory);
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { ScenePath },
                locationPathName = BuildExePath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            });

            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new InvalidOperationException($"Fast VS visual direction sample build failed: {report.summary.result}");
            }

            Debug.Log($"Fast VS visual direction sample player built: {Path.GetFullPath(BuildExePath)}");
        }

        public static void BuildAndValidateBatch()
        {
            CreateSampleScene();
            ValidateSampleSceneBatch();
            BuildSamplePlayer();
        }

        private static void CreateLibraryDirectionSpace(Transform root, string prefix, bool restored, Materials materials)
        {
            var ground = restored ? materials.PastGround : materials.CurrentGround;
            var path = restored ? materials.PastPath : materials.CurrentPath;
            var wall = restored ? materials.PastWall : materials.CurrentWall;
            var wood = restored ? materials.PastWood : materials.CurrentWood;
            var prop = restored ? materials.PastProp : materials.CurrentProp;

            CreateLandmarkCube($"{prefix}_StoneFloor_CompleteLibraryFootprint", root, new Vector3(0f, -0.05f, 0f), new Vector3(RegionSize.x, 0.10f, RegionSize.y), Quaternion.identity, ground, true, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.floor");
            CreateLandmarkCube($"{prefix}_EntryRunner_ReadableRoute", root, new Vector3(0f, 0.012f, -2.25f), new Vector3(2.10f, 0.035f, 1.85f), Quaternion.identity, path, false, TimeWindowPairedSpaceLandmarkKind.PathOrFloor, $"{prefix}.path.entry");
            CreateLandmarkCube($"{prefix}_LibraryBackWall_BuildingMass", root, new Vector3(0f, 1.05f, 2.95f), new Vector3(7.55f, 2.10f, 0.18f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.wall.back");
            CreateLandmarkCube($"{prefix}_LibraryLeftWall_BuildingMass", root, new Vector3(-3.75f, 0.86f, 0.10f), new Vector3(0.18f, 1.72f, 6.05f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.wall.left");
            CreateLandmarkCube($"{prefix}_LibraryRightWall_BuildingMass", root, new Vector3(3.75f, 0.86f, 0.10f), new Vector3(0.18f, 1.72f, 6.05f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.wall.right");
            CreateLandmarkCube($"{prefix}_FrontLowCurb_WindowSafeThreshold", root, new Vector3(0f, 0.18f, -3.30f), new Vector3(7.55f, 0.36f, 0.14f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.curb.front");

            CreateLandmarkCube($"{prefix}_LeftFacadeColumn_A", root, new Vector3(-2.85f, 1.15f, -2.92f), new Vector3(0.28f, 2.30f, 0.28f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.facade.left_column");
            CreateLandmarkCube($"{prefix}_RightFacadeColumn_A", root, new Vector3(2.85f, 1.15f, -2.92f), new Vector3(0.28f, 2.30f, 0.28f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.facade.right_column");
            CreateLandmarkCube($"{prefix}_ReadableRoofBeam", root, new Vector3(0f, 2.32f, -2.92f), new Vector3(6.05f, 0.28f, 0.34f), Quaternion.identity, wall, true, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.facade.roof_beam");
            CreateLandmarkCube($"{prefix}_PaperSet_BackSign", root, new Vector3(0f, 1.95f, 2.83f), new Vector3(2.15f, 0.42f, 0.08f), Quaternion.identity, prop, false, TimeWindowPairedSpaceLandmarkKind.WallOrLandmark, $"{prefix}.sign.back");

            CreateShelfCluster(root, prefix, restored, materials);
            CreateDeskAndProps(root, prefix, restored, materials);

            if (restored)
            {
                CreateLandmarkCube("Past_WarmLampLine_A", root, new Vector3(-1.85f, 1.38f, 1.92f), new Vector3(0.18f, 0.34f, 0.18f), Quaternion.identity, materials.Lamp, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.lamp.left");
                CreateLandmarkCube("Past_WarmLampLine_B", root, new Vector3(1.85f, 1.38f, 1.92f), new Vector3(0.18f, 0.34f, 0.18f), Quaternion.identity, materials.Lamp, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.lamp.right");
                CreatePaperCharacter(root, "Past Clerk", new Vector3(-1.15f, 0f, -0.32f), 0.88f, null, materials.GenericNpcBody, materials.GenericNpcAccent, materials.CardFace, materials.Label);
                CreateLabel(root, "PAST: restored shelves / warm light / full books", new Vector3(0f, 2.82f, 2.68f), materials.Label, 0.075f);
            }
            else
            {
                CreateLandmarkCube("Current_TimewriterRedLight_ToEmptyShelf", root, new Vector3(-0.92f, 1.08f, 0.68f), new Vector3(0.10f, 0.10f, 3.15f), Quaternion.Euler(0f, 18f, -8f), materials.RedLight, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.timewriter.red_light");
                CreateLandmarkCube("Current_DarkRubbleStrip_AtEmptyStacks", root, new Vector3(1.45f, 0.10f, 1.52f), new Vector3(2.25f, 0.20f, 0.35f), Quaternion.Euler(0f, -9f, 0f), materials.CurrentProp, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.rubble.empty_stack");
                CreatePaperCharacter(root, "Reto", new Vector3(1.18f, 0f, -0.48f), 0.82f, null, materials.RetoBody, materials.RetoAccent, materials.CardFace, materials.Label);
                CreateLabel(root, "CURRENT: ruined library / empty shelves / Reto paper card", new Vector3(0f, 2.82f, 2.68f), materials.Label, 0.075f);
            }
        }

        private static void CreateShelfCluster(Transform root, string prefix, bool restored, Materials materials)
        {
            var shelfMaterial = restored ? materials.PastWood : materials.CurrentWood;
            var propMaterial = restored ? materials.PastProp : materials.CurrentProp;
            var height = restored ? 1.45f : 0.92f;
            var leftRotation = restored ? Quaternion.identity : Quaternion.Euler(0f, 0f, -7f);
            var rightRotation = restored ? Quaternion.identity : Quaternion.Euler(0f, 0f, 5f);

            CreateLandmarkCube($"{prefix}_LeftShelf_ReadableBuildingInterior", root, new Vector3(-2.45f, height * 0.5f, 1.38f), new Vector3(0.52f, height, 1.85f), leftRotation, shelfMaterial, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.shelf.left");
            CreateLandmarkCube($"{prefix}_RightShelf_ReadableBuildingInterior", root, new Vector3(2.45f, height * 0.5f, 1.38f), new Vector3(0.52f, height, 1.85f), rightRotation, shelfMaterial, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.shelf.right");
            CreateLandmarkCube($"{prefix}_BackShelf_LongLibraryIdentity", root, new Vector3(0f, restored ? 0.82f : 0.50f, 2.42f), new Vector3(4.65f, restored ? 1.64f : 1.00f, 0.38f), Quaternion.identity, shelfMaterial, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.shelf.back");

            if (!restored)
            {
                CreateLandmarkCube("Current_EmptyShelfPaleInterior_Left", root, new Vector3(-2.45f, 1.06f, 1.38f), new Vector3(0.55f, 0.16f, 1.65f), Quaternion.identity, materials.EmptyShelf, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.empty.left");
                CreateLandmarkCube("Current_EmptyShelfPaleInterior_Right", root, new Vector3(2.45f, 1.02f, 1.38f), new Vector3(0.55f, 0.16f, 1.65f), Quaternion.identity, materials.EmptyShelf, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Current.empty.right");
                return;
            }

            for (var i = 0; i < 12; i++)
            {
                var side = i % 2 == 0 ? -1f : 1f;
                var z = 0.62f + (i / 2) * 0.31f;
                var y = 0.52f + (i % 3) * 0.23f;
                var material = i % 3 == 0 ? materials.BookRed : i % 3 == 1 ? materials.BookBlue : materials.BookGold;
                CreateLandmarkCube($"Past_BookSpine_{i:00}", root, new Vector3(side * 2.15f, y, z), new Vector3(0.13f, 0.32f, 0.08f), Quaternion.identity, material, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"Past.book.{i:00}");
            }

            CreateLandmarkCube("Past_BackShelf_BookBand_Red", root, new Vector3(-0.95f, 0.94f, 2.18f), new Vector3(1.10f, 0.22f, 0.10f), Quaternion.identity, materials.BookRed, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.book.back.red");
            CreateLandmarkCube("Past_BackShelf_BookBand_Blue", root, new Vector3(0.55f, 1.16f, 2.18f), new Vector3(1.55f, 0.18f, 0.10f), Quaternion.identity, materials.BookBlue, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.book.back.blue");
            CreateLandmarkCube("Past_BackShelf_BookBand_Gold", root, new Vector3(1.52f, 0.70f, 2.18f), new Vector3(0.85f, 0.24f, 0.10f), Quaternion.identity, materials.BookGold, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, "Past.book.back.gold");
        }

        private static void CreateDeskAndProps(Transform root, string prefix, bool restored, Materials materials)
        {
            var wood = restored ? materials.PastWood : materials.CurrentWood;
            CreateLandmarkCube($"{prefix}_RetoDesk_TableSilhouette", root, new Vector3(1.18f, 0.34f, -0.08f), new Vector3(1.18f, 0.16f, 0.72f), Quaternion.identity, wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.desk.top");
            CreateLandmarkCube($"{prefix}_DeskLeftLeg", root, new Vector3(0.72f, 0.16f, -0.34f), new Vector3(0.13f, 0.32f, 0.13f), Quaternion.identity, wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.desk.leg.left");
            CreateLandmarkCube($"{prefix}_DeskRightLeg", root, new Vector3(1.62f, 0.16f, -0.34f), new Vector3(0.13f, 0.32f, 0.13f), Quaternion.identity, wood, true, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.desk.leg.right");
            CreateLandmarkCube($"{prefix}_LedgerBook_OnDesk", root, new Vector3(1.02f, 0.45f, -0.10f), new Vector3(0.44f, 0.045f, 0.30f), Quaternion.Euler(0f, 18f, 0f), restored ? materials.BookGold : materials.EmptyShelf, false, TimeWindowPairedSpaceLandmarkKind.PropOrFeature, $"{prefix}.desk.ledger");
        }

        private static CharacterController CreateNiroPlayer(Transform currentRoot, Camera camera, Materials materials)
        {
            var player = new GameObject("FastVS_Player_NiroPaperController");
            player.tag = "Player";
            player.transform.position = currentRoot.TransformPoint(new Vector3(-0.85f, 0.02f, PortalLocalZ - 0.70f));
            player.AddComponent<FastVsPaperBillboard>();

            var controller = player.AddComponent<CharacterController>();
            controller.height = 1.34f;
            controller.radius = 0.22f;
            controller.center = new Vector3(0f, 0.67f, 0f);

            CreatePaperCardParts(player.transform, "Niro", 0.94f, materials.NiroBody, materials.NiroAccent, materials.CardFace, materials.Label);
            SerializedSet(player.GetComponent<FastVsPaperBillboard>(), "targetCamera", camera);
            return controller;
        }

        private static void CreatePaperCharacter(Transform root, string displayName, Vector3 localPosition, float height, Camera camera, Material body, Material accent, Material face, Material label)
        {
            var character = new GameObject($"FastVS_PaperCharacter_{displayName.Replace(" ", string.Empty)}");
            character.transform.SetParent(root, false);
            character.transform.localPosition = localPosition;
            var billboard = character.AddComponent<FastVsPaperBillboard>();
            if (camera != null)
            {
                SerializedSet(billboard, "targetCamera", camera);
            }

            CreatePaperCardParts(character.transform, displayName, height, body, accent, face, label);
        }

        private static void CreatePaperCardParts(Transform parent, string displayName, float height, Material body, Material accent, Material face, Material label)
        {
            CreateQuad($"{displayName}_PaperBody", parent, new Vector3(0f, height * 0.52f, 0f), new Vector3(height * 0.42f, height * 0.88f, 1f), body);
            CreateQuad($"{displayName}_PaperHead", parent, new Vector3(0f, height * 1.02f, -0.012f), new Vector3(height * 0.33f, height * 0.30f, 1f), face);
            CreateQuad($"{displayName}_PaperHatOrHair", parent, new Vector3(0f, height * 1.17f, -0.024f), new Vector3(height * 0.42f, height * 0.18f, 1f), accent);
            CreateQuad($"{displayName}_PaperFootLine", parent, new Vector3(0f, height * 0.08f, -0.026f), new Vector3(height * 0.48f, height * 0.08f, 1f), accent);
            CreateNameLabel(parent, displayName, new Vector3(0f, height * 1.45f, -0.035f), label);
        }

        private static void CreateQuad(string name, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = name;
            quad.transform.SetParent(parent, false);
            quad.transform.localPosition = localPosition;
            quad.transform.localScale = localScale;
            quad.GetComponent<Renderer>().sharedMaterial = material;
            var collider = quad.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }
        }

        private static void CreateNameLabel(Transform parent, string text, Vector3 localPosition, Material material)
        {
            var label = new GameObject($"{text}_NameLabel");
            label.transform.SetParent(parent, false);
            label.transform.localPosition = localPosition;
            var mesh = label.AddComponent<TextMesh>();
            mesh.text = text;
            mesh.anchor = TextAnchor.MiddleCenter;
            mesh.alignment = TextAlignment.Center;
            mesh.fontSize = 54;
            mesh.characterSize = 0.035f;
            mesh.color = Color.white;
            var renderer = label.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
        }

        private static void CreateLabel(Transform root, string text, Vector3 localPosition, Material material, float characterSize)
        {
            var label = new GameObject(text.Length > 32 ? text.Substring(0, 32) : text);
            label.transform.SetParent(root, false);
            label.transform.localPosition = localPosition;
            label.transform.localRotation = Quaternion.Euler(18f, 0f, 0f);
            var mesh = label.AddComponent<TextMesh>();
            mesh.text = text;
            mesh.anchor = TextAnchor.MiddleCenter;
            mesh.alignment = TextAlignment.Center;
            mesh.fontSize = 48;
            mesh.characterSize = characterSize;
            mesh.color = Color.white;
            label.GetComponent<MeshRenderer>().sharedMaterial = material;
        }

        private static TimeWindowPairedSpacePortalController CreateController(Camera camera, Transform currentRoot, Transform otherRoot, CharacterController player, Materials materials)
        {
            var controllerObject = new GameObject("FastVS_V24_PairedSpacePortalController");
            var controller = controllerObject.AddComponent<TimeWindowPairedSpacePortalController>();
            SerializedSet(controller, "currentSpaceRoot", currentRoot);
            SerializedSet(controller, "otherTimeSpaceRoot", otherRoot);
            SerializedSet(controller, "regionSize", RegionSize);
            SerializedSet(controller, "portalLocalZ", PortalLocalZ);
            SerializedSet(controller, "placePortalFromGroundProjection", true);
            SerializedSet(controller, "anchorPortalBottomToGround", true);
            SerializedSet(controller, "playerController", player);
            SerializedSet(controller, "player", player.transform);
            SerializedSet(controller, "currentPlayerMaterial", materials.NiroBody);
            SerializedSet(controller, "otherTimePlayerMaterial", materials.NiroPastBody);
            SerializedSet(controller, "sceneCamera", camera);
            SerializedSet(controller, "currentFrameMaterial", materials.CurrentFrame);
            SerializedSet(controller, "otherTimeFrameMaterial", materials.PastFrame);
            SerializedSet(controller, "previewFrameMaterial", materials.PreviewFrame);
            SerializedSet(controller, "thresholdMaterial", materials.Threshold);
            SerializedSet(controller, "enablePortalApertureView", true);
            SerializedSet(controller, "apertureTextureSize", 1024);
            SerializedSet(controller, "portalApertureMaterial", materials.Aperture);
            SerializedSet(controller, "enableBackSideBlocking", false);
            SerializedSet(controller, "enableGeneratedOtherTimeWallVolume", true);
            SerializedSet(controller, "wallVolumeDepth", 8.0f);
            SerializedSet(controller, "wallVolumeSideMargin", 0.22f);
            SerializedSet(controller, "wallVolumeThickness", 0.14f);
            SerializedSet(controller, "wallVolumeNearGapDepth", 0.42f);
            SerializedSet(controller, "farBackWallDepthMultiplier", 1.12f);
            SerializedSet(controller, "farBackWallDepthPadding", 0.28f);
            SerializedSet(controller, "farBackWallMinimumDepth", 1.15f);
            SerializedSet(controller, "currentBackSideBlockDepth", 0.38f);
            return controller;
        }

        private static void CreateGuide(Camera camera, TimeWindowPairedSpacePortalController controller, CharacterController player)
        {
            var guideObject = new GameObject("FastVS_VisualDirectionGuide");
            var guide = guideObject.AddComponent<FastVsVisualDirectionGuide>();
            SerializedSet(guide, "portalController", controller);
            SerializedSet(guide, "playerController", player);
            SerializedSet(guide, "player", player.transform);
            SerializedSet(guide, "reviewCamera", camera);
        }

        private static Camera CreateCamera(Transform currentRoot)
        {
            var cameraObject = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener));
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.075f, 0.078f, 0.085f, 1f);
            camera.fieldOfView = 38f;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 90f;
            var position = currentRoot.TransformPoint(new Vector3(-0.85f, 2.75f, PortalLocalZ - 5.25f));
            var lookAt = currentRoot.TransformPoint(new Vector3(-0.45f, 0.70f, PortalLocalZ + 0.20f));
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
            return camera;
        }

        private static void CreateLighting()
        {
            var lightObject = new GameObject("Directional Light", typeof(Light));
            var light = lightObject.GetComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.12f;
            light.shadows = LightShadows.Soft;
            lightObject.transform.rotation = Quaternion.Euler(52f, -35f, 0f);
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.30f, 0.30f, 0.34f);
        }

        private static GameObject CreateLandmarkCube(string objectName, Transform parent, Vector3 localPosition, Vector3 localScale, Quaternion localRotation, Material material, bool keepCollider, TimeWindowPairedSpaceLandmarkKind kind, string landmarkId)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = objectName;
            cube.transform.SetParent(parent, false);
            cube.transform.localPosition = localPosition;
            cube.transform.localRotation = localRotation;
            cube.transform.localScale = localScale;
            cube.GetComponent<Renderer>().sharedMaterial = material;
            if (!keepCollider)
            {
                var collider = cube.GetComponent<Collider>();
                if (collider != null)
                {
                    UnityEngine.Object.DestroyImmediate(collider);
                }
            }

            var landmark = cube.AddComponent<TimeWindowPairedSpaceLandmark>();
            SerializedSet(landmark, "landmarkId", landmarkId);
            SerializedSet(landmark, "kind", kind);
            SerializedSet(landmark, "countsForArrival", true);
            return cube;
        }

        private static Materials EnsureMaterials()
        {
            return new Materials(
                EnsureMaterial("FastVS_CurrentGround.mat", new Color(0.16f, 0.15f, 0.13f, 1f), false),
                EnsureMaterial("FastVS_CurrentPath.mat", new Color(0.31f, 0.24f, 0.19f, 1f), false),
                EnsureMaterial("FastVS_CurrentWall.mat", new Color(0.26f, 0.22f, 0.20f, 1f), false),
                EnsureMaterial("FastVS_CurrentWood.mat", new Color(0.38f, 0.23f, 0.16f, 1f), false),
                EnsureMaterial("FastVS_CurrentProp.mat", new Color(0.48f, 0.28f, 0.19f, 1f), false),
                EnsureMaterial("FastVS_EmptyShelf.mat", new Color(0.20f, 0.18f, 0.16f, 1f), false),
                EnsureMaterial("FastVS_PastGround.mat", new Color(0.22f, 0.34f, 0.36f, 1f), false),
                EnsureMaterial("FastVS_PastPath.mat", new Color(0.36f, 0.56f, 0.54f, 1f), false),
                EnsureMaterial("FastVS_PastWall.mat", new Color(0.33f, 0.46f, 0.52f, 1f), false),
                EnsureMaterial("FastVS_PastWood.mat", new Color(0.54f, 0.40f, 0.24f, 1f), false),
                EnsureMaterial("FastVS_PastProp.mat", new Color(0.74f, 0.63f, 0.32f, 1f), false),
                EnsureMaterial("FastVS_BookRed.mat", new Color(0.68f, 0.16f, 0.16f, 1f), false),
                EnsureMaterial("FastVS_BookBlue.mat", new Color(0.18f, 0.30f, 0.68f, 1f), false),
                EnsureMaterial("FastVS_BookGold.mat", new Color(0.92f, 0.76f, 0.22f, 1f), false),
                EnsureMaterial("FastVS_Lamp.mat", new Color(1.00f, 0.82f, 0.36f, 1f), false),
                EnsureMaterial("FastVS_RedLight.mat", new Color(1.00f, 0.14f, 0.10f, 1f), false),
                EnsureMaterial("FastVS_CurrentFrame.mat", new Color(1.00f, 0.42f, 0.17f, 1f), false),
                EnsureMaterial("FastVS_PastFrame.mat", new Color(0.28f, 0.95f, 1.00f, 1f), false),
                EnsureMaterial("FastVS_PreviewFrame.mat", new Color(0.76f, 0.76f, 0.78f, 1f), false),
                EnsureMaterial("FastVS_Threshold.mat", new Color(0.20f, 0.95f, 0.82f, 1f), false),
                EnsureMaterial("FastVS_NiroBody.mat", new Color(0.26f, 0.42f, 0.78f, 1f), true),
                EnsureMaterial("FastVS_NiroPastBody.mat", new Color(0.46f, 0.72f, 0.96f, 1f), true),
                EnsureMaterial("FastVS_NiroAccent.mat", new Color(0.92f, 0.74f, 0.38f, 1f), true),
                EnsureMaterial("FastVS_RetoBody.mat", new Color(0.58f, 0.34f, 0.25f, 1f), true),
                EnsureMaterial("FastVS_RetoAccent.mat", new Color(0.28f, 0.18f, 0.15f, 1f), true),
                EnsureMaterial("FastVS_GenericNpcBody.mat", new Color(0.52f, 0.50f, 0.62f, 1f), true),
                EnsureMaterial("FastVS_GenericNpcAccent.mat", new Color(0.22f, 0.26f, 0.34f, 1f), true),
                EnsureMaterial("FastVS_CardFace.mat", new Color(0.94f, 0.80f, 0.62f, 1f), true),
                EnsureMaterial("FastVS_Label.mat", Color.white, true),
                EnsureApertureMaterial("FastVS_ApertureOverlay.mat"));
        }

        private static Material EnsureApertureMaterial(string fileName)
        {
            var path = $"{MaterialDirectory}/{fileName}";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            var shader = Shader.Find("Anemora/Review/PortalApertureOverlay");
            if (shader == null)
            {
                shader = Shader.Find("Universal Render Pipeline/Unlit");
            }

            if (shader == null)
            {
                throw new InvalidOperationException("Portal aperture shader not found.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.shader = shader;
            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", Color.white);
            }

            return material;
        }

        private static Material EnsureMaterial(string fileName, Color color, bool unlit)
        {
            var path = $"{MaterialDirectory}/{fileName}";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            var shader = Shader.Find(unlit ? "Universal Render Pipeline/Unlit" : "Universal Render Pipeline/Lit");
            if (shader == null)
            {
                throw new InvalidOperationException($"Required shader not found for {fileName}.");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.shader = shader;
            material.doubleSidedGI = true;
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
                material.SetFloat("_Cull", 0f);
            }

            return material;
        }

        private static void SerializedSet(UnityEngine.Object target, string fieldName, object value)
        {
            var serialized = new SerializedObject(target);
            var property = serialized.FindProperty(fieldName);
            if (property == null)
            {
                throw new InvalidOperationException($"Serialized field not found: {target.GetType().Name}.{fieldName}");
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.ObjectReference:
                    property.objectReferenceValue = value as UnityEngine.Object;
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = (Vector2)value;
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = Convert.ToSingle(value);
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = Convert.ToInt32(value);
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = Convert.ToBoolean(value);
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = value as string;
                    break;
                case SerializedPropertyType.Enum:
                    property.enumValueIndex = Convert.ToInt32(value);
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported property type for {fieldName}: {property.propertyType}");
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            var parts = path.Split('/');
            var current = parts[0];
            for (var index = 1; index < parts.Length; index++)
            {
                var next = $"{current}/{parts[index]}";
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[index]);
                }

                current = next;
            }
        }

        private readonly struct Materials
        {
            public Materials(
                Material currentGround,
                Material currentPath,
                Material currentWall,
                Material currentWood,
                Material currentProp,
                Material emptyShelf,
                Material pastGround,
                Material pastPath,
                Material pastWall,
                Material pastWood,
                Material pastProp,
                Material bookRed,
                Material bookBlue,
                Material bookGold,
                Material lamp,
                Material redLight,
                Material currentFrame,
                Material pastFrame,
                Material previewFrame,
                Material threshold,
                Material niroBody,
                Material niroPastBody,
                Material niroAccent,
                Material retoBody,
                Material retoAccent,
                Material genericNpcBody,
                Material genericNpcAccent,
                Material cardFace,
                Material label,
                Material aperture)
            {
                CurrentGround = currentGround;
                CurrentPath = currentPath;
                CurrentWall = currentWall;
                CurrentWood = currentWood;
                CurrentProp = currentProp;
                EmptyShelf = emptyShelf;
                PastGround = pastGround;
                PastPath = pastPath;
                PastWall = pastWall;
                PastWood = pastWood;
                PastProp = pastProp;
                BookRed = bookRed;
                BookBlue = bookBlue;
                BookGold = bookGold;
                Lamp = lamp;
                RedLight = redLight;
                CurrentFrame = currentFrame;
                PastFrame = pastFrame;
                PreviewFrame = previewFrame;
                Threshold = threshold;
                NiroBody = niroBody;
                NiroPastBody = niroPastBody;
                NiroAccent = niroAccent;
                RetoBody = retoBody;
                RetoAccent = retoAccent;
                GenericNpcBody = genericNpcBody;
                GenericNpcAccent = genericNpcAccent;
                CardFace = cardFace;
                Label = label;
                Aperture = aperture;
            }

            public Material CurrentGround { get; }
            public Material CurrentPath { get; }
            public Material CurrentWall { get; }
            public Material CurrentWood { get; }
            public Material CurrentProp { get; }
            public Material EmptyShelf { get; }
            public Material PastGround { get; }
            public Material PastPath { get; }
            public Material PastWall { get; }
            public Material PastWood { get; }
            public Material PastProp { get; }
            public Material BookRed { get; }
            public Material BookBlue { get; }
            public Material BookGold { get; }
            public Material Lamp { get; }
            public Material RedLight { get; }
            public Material CurrentFrame { get; }
            public Material PastFrame { get; }
            public Material PreviewFrame { get; }
            public Material Threshold { get; }
            public Material NiroBody { get; }
            public Material NiroPastBody { get; }
            public Material NiroAccent { get; }
            public Material RetoBody { get; }
            public Material RetoAccent { get; }
            public Material GenericNpcBody { get; }
            public Material GenericNpcAccent { get; }
            public Material CardFace { get; }
            public Material Label { get; }
            public Material Aperture { get; }
        }
    }
}
