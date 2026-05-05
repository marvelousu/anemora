using System;
using System.IO;
using System.Linq;
using Anemora.Dialogue;
using Anemora.TimeManagement;
using Anemora.TimeManagement.Reflectors;
using Anemora.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Anemora.EditorTools
{
    public static class AnemoraDemoSceneSetup
    {
        private const string MainScenePath = "Assets/Scenes/Anemora_Main.unity";
        private const string Zone1PrefabRoot = "Assets/Prefabs/Zone1";
        private const string DialoguePanelPrefabPath = "Assets/UI/Prefabs/DialoguePanel.prefab";
        private const string SymbolWheelPrefabPath = "Assets/UI/Prefabs/SymbolWheel.prefab";
        private const string PortalFramePrefabPath = "Assets/Prefabs/Portal/Portal_Frame.prefab";
        private const string TimeWindowDioramaPrefabPath = "Assets/Prefabs/Portal/TimeWindow_Diorama.prefab";
        private const string PortalFrameMaterialPath = "Assets/Art/Materials/Portal/Debug_Past.mat";
        private const string PortalCurrentMaterialPath = "Assets/Art/Materials/Portal/Debug_Current.mat";
        private const string TimeVolumeVeilMaterialPath = "Assets/Art/Materials/Portal/TimeVolume_SpaceVeil.mat";
        private const string TmpSettingsPath = "Assets/TextMesh Pro/Resources/TMP Settings.asset";
        private const string TmpLeadingCharactersPath = "Assets/TextMesh Pro/Resources/LineBreaking Leading Characters.txt";
        private const string TmpFollowingCharactersPath = "Assets/TextMesh Pro/Resources/LineBreaking Following Characters.txt";

        private const int UiLayer = 5;
        private const int CurrentColliderLayer = 8;
        private const int PastColliderLayer = 9;
        private const int CurrentVisualLayer = 10;
        private const int PastVisualLayer = 11;

        [MenuItem("Anemora/Setup/Demo Playable Scene")]
        public static void Apply()
        {
            ConfigureTmpSettings();
            ConfigureTimeWindowDioramaPrefab();
            ConfigureDialoguePanelPrefab();
            ConfigureSymbolWheelPrefab();

            var scene = EditorSceneManager.OpenScene(MainScenePath, OpenSceneMode.Single);
            ConfigureScene(scene);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Anemora demo playable scene setup completed.");
        }

        private static void ConfigureScene(Scene scene)
        {
            var rootCurrent = RequireSceneObject("Root_Current").transform;
            var rootPast = RequireSceneObject("Root_Past").transform;

            DestroyIfExists("DemoZone1_Current");
            DestroyIfExists("DemoZone1_Past");

            var currentDemoRoot = CreateChildRoot(rootCurrent, "DemoZone1_Current", CurrentVisualLayer).transform;
            var pastDemoRoot = CreateChildRoot(rootPast, "DemoZone1_Past", PastVisualLayer).transform;

            BuildCurrentEnvironment(currentDemoRoot);
            BuildPastEnvironment(pastDemoRoot);
            ConfigureCoreSceneObjects();
            ConfigureUi(scene);
            ConfigureNpcPlacement(rootCurrent, rootPast);
            ConfigurePortalSystem();
        }

        private static void BuildCurrentEnvironment(Transform parent)
        {
            SpawnTileGrid(parent, "Floor_Stone", "Current_StoneTile", CurrentVisualLayer, -2, 2, -2, 3, 1.45f, 1.7f);
            SpawnTileGrid(parent, "Floor_Wood", "Current_WoodTile", CurrentVisualLayer, -2, -1, -1, 1, 1.45f, 1.5f);

            SpawnZonePrefab(parent, "House_Player", "Current_House_Player", CurrentVisualLayer, new Vector3(-2.9f, 0f, 2.05f), Quaternion.Euler(0f, 145f, 0f), 2.1f);
            SpawnZonePrefab(parent, "Bed_Player", "Current_Bed_Player", CurrentVisualLayer, new Vector3(-1.2f, 0f, 0.55f), Quaternion.Euler(0f, -12f, 0f), 1.05f);
            SpawnZonePrefab(parent, "Plaza_Fountain_Dry_Broken", "Current_Plaza_Fountain", CurrentVisualLayer, new Vector3(1.15f, 0f, 1.15f), Quaternion.Euler(0f, -15f, 0f), 1.25f);
            SpawnZonePrefab(parent, "StreetLamp", "Current_Lamp_West", CurrentVisualLayer, new Vector3(-1.85f, 0f, -0.55f), Quaternion.identity, 1.1f);
            SpawnZonePrefab(parent, "StreetLamp", "Current_Lamp_East", CurrentVisualLayer, new Vector3(2.05f, 0f, -0.35f), Quaternion.Euler(0f, 20f, 0f), 1.1f);
            SpawnZonePrefab(parent, "Tree_Decay", "Current_DecayTree", CurrentVisualLayer, new Vector3(2.55f, 0f, 2.35f), Quaternion.Euler(0f, -35f, 0f), 1.5f);
            SpawnZonePrefab(parent, "Bookshelf_FamilyBooks", "Current_FamilyBookshelf", CurrentVisualLayer, new Vector3(-1.95f, 0f, 0.95f), Quaternion.Euler(0f, 25f, 0f), 0.95f);
            SpawnZonePrefab(parent, "Table_SmallChair_Wooden", "Current_Table_Chair", CurrentVisualLayer, new Vector3(-0.65f, 0f, 0.95f), Quaternion.Euler(0f, -20f, 0f), 0.9f);
        }

        private static void BuildPastEnvironment(Transform parent)
        {
            SpawnTileGrid(parent, "Floor_Stone", "Past_StoneTile", PastVisualLayer, -2, 2, -2, 3, 1.45f, 1.7f);

            SpawnZonePrefab(parent, "Library_Ruin", "Past_Library_Ruin", PastVisualLayer, new Vector3(2.65f, 0f, 2.25f), Quaternion.Euler(0f, -140f, 0f), 2.1f);
            SpawnZonePrefab(parent, "Plaza_Fountain_Dry_Broken", "Past_Plaza_Fountain", PastVisualLayer, new Vector3(0.95f, 0f, 1.15f), Quaternion.Euler(0f, 10f, 0f), 1.2f);
            SpawnZonePrefab(parent, "Bookshelf_Library_Past", "Past_Library_Bookshelf", PastVisualLayer, new Vector3(1.85f, 0f, 0.8f), Quaternion.Euler(0f, -25f, 0f), 1f);
            SpawnZonePrefab(parent, "Tree_Decay", "Past_DecayTree_A", PastVisualLayer, new Vector3(-2.4f, 0f, 2.35f), Quaternion.Euler(0f, 20f, 0f), 1.55f);
            SpawnZonePrefab(parent, "Tree_Decay", "Past_DecayTree_B", PastVisualLayer, new Vector3(2.6f, 0f, -0.9f), Quaternion.Euler(0f, -55f, 0f), 1.25f);
            SpawnZonePrefab(parent, "StreetLamp", "Past_BrokenLamp", PastVisualLayer, new Vector3(-1.8f, 0f, -0.55f), Quaternion.Euler(0f, 18f, 0f), 1f);
            SpawnZonePrefab(parent, "Door_House", "Past_Door_Remnant", PastVisualLayer, new Vector3(-2.7f, 0f, 0.9f), Quaternion.Euler(0f, 30f, 0f), 0.9f);
        }

        private static void ConfigureCoreSceneObjects()
        {
            var player = RequireSceneObject("Player");
            player.layer = CurrentColliderLayer;
            player.transform.position = new Vector3(0f, 0.62f, -1.35f);
            player.transform.rotation = Quaternion.identity;

            var camera = Camera.main;
            if (camera != null)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(camera.gameObject);
                camera.transform.position = new Vector3(0f, 4.35f, -6.6f);
                LookAt(camera.transform, new Vector3(0f, 0.45f, 1.05f));
                camera.fieldOfView = 48f;
                camera.nearClipPlane = 0.05f;
                camera.farClipPlane = 140f;
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = new Color(0.11f, 0.12f, 0.14f, 1f);
                camera.cullingMask = (1 << CurrentVisualLayer) | (1 << UiLayer);
            }

            var currentFloor = GameObject.Find("Current_Floor");
            if (currentFloor != null)
            {
                SetLayerRecursively(currentFloor, CurrentVisualLayer);
                currentFloor.transform.localScale = new Vector3(7.5f, 0.08f, 8.5f);
            }

            var pastFloor = GameObject.Find("Past_Floor");
            if (pastFloor != null)
            {
                SetLayerRecursively(pastFloor, PastVisualLayer);
                pastFloor.transform.localScale = new Vector3(7.5f, 0.08f, 8.5f);
            }

            var light = GameObject.Find("Directional Light")?.GetComponent<Light>();
            if (light != null)
            {
                light.transform.rotation = Quaternion.Euler(48f, -32f, 0f);
                light.intensity = 1.1f;
                light.shadows = LightShadows.Soft;
            }

            RenderSettings.ambientLight = new Color(0.34f, 0.32f, 0.28f, 1f);
            HideLegacyCurrentBedPrimitive();
        }

        private static void HideLegacyCurrentBedPrimitive()
        {
            var bedPlaceholder = GameObject.Find("Current_BedPlaceholder");
            if (bedPlaceholder == null)
            {
                return;
            }

            RemoveComponent<MeshRenderer>(bedPlaceholder);
            RemoveComponent<MeshFilter>(bedPlaceholder);
            RemoveComponent<BoxCollider>(bedPlaceholder);
            SetLayerRecursively(bedPlaceholder, CurrentVisualLayer);
        }

        private static void ConfigureUi(Scene scene)
        {
            var dialogueCanvas = GameObject.Find("DialogueCanvas");
            if (dialogueCanvas == null)
            {
                dialogueCanvas = new GameObject("DialogueCanvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
                SceneManager.MoveGameObjectToScene(dialogueCanvas, scene);
            }

            var canvas = dialogueCanvas.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 1000;
            canvas.pixelPerfect = false;
            NormalizeCanvasRect(dialogueCanvas);
            SetLayerRecursively(dialogueCanvas, UiLayer);

            var scaler = dialogueCanvas.GetComponent<CanvasScaler>();
            if (scaler != null)
            {
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920f, 1080f);
                scaler.matchWidthOrHeight = 0.5f;
            }

            var dialoguePanel = GameObject.Find("DialoguePanel");
            if (dialoguePanel != null)
            {
                dialoguePanel.transform.SetParent(dialogueCanvas.transform, false);
                SetLayerRecursively(dialoguePanel, UiLayer);
                dialoguePanel.transform.SetAsLastSibling();
            }

            var symbolWheel = FindSceneComponent<SymbolWheelController>();
            if (symbolWheel != null)
            {
                var wheelCanvas = symbolWheel.GetComponentInParent<Canvas>();
                if (wheelCanvas != null)
                {
                    wheelCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    wheelCanvas.overrideSorting = true;
                    wheelCanvas.sortingOrder = 900;
                    NormalizeCanvasRect(wheelCanvas.gameObject);
                    SetLayerRecursively(wheelCanvas.gameObject, UiLayer);
                }

                symbolWheel.SetKeyboardSelectionEnabled(false);
            }
        }

        private static void ConfigureNpcPlacement(Transform rootCurrent, Transform rootPast)
        {
            var residentA = GameObject.Find("Resident_A_Instance");
            if (residentA != null)
            {
                residentA.transform.SetParent(rootPast, false);
                residentA.transform.localPosition = new Vector3(-0.85f, 0.02f, 1.05f);
                residentA.transform.localRotation = Quaternion.identity;
                SetLayerRecursively(residentA, PastVisualLayer);
                ConfigureNpcInteractable(residentA, 2.35f);
            }

            var residentB = GameObject.Find("Resident_B_Instance");
            if (residentB != null)
            {
                residentB.transform.SetParent(rootCurrent, false);
                residentB.transform.localPosition = new Vector3(1.25f, 0.02f, 0.85f);
                residentB.transform.localRotation = Quaternion.identity;
                SetLayerRecursively(residentB, CurrentVisualLayer);
                ConfigureNpcInteractable(residentB, 2.35f);
            }
        }

        private static void ConfigureNpcInteractable(GameObject npc, float range)
        {
            var interactable = npc.GetComponent<NpcInteractable>();
            if (interactable == null)
            {
                return;
            }

            var serialized = new SerializedObject(interactable);
            SetFloat(serialized, "interactionRange", range);
            SetBool(serialized, "requireMainCameraVisibility", true);
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ConfigurePortalSystem()
        {
            var controller = FindSceneComponent<TimeFramePortalController>();
            if (controller != null)
            {
                var windowPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(TimeWindowDioramaPrefabPath);
                if (windowPrefab == null)
                {
                    throw new InvalidOperationException($"Missing time window prefab: {TimeWindowDioramaPrefabPath}");
                }

                var serialized = new SerializedObject(controller);
                SetObject(serialized, "portalPrefab", windowPrefab);
                SetBool(serialized, "enableBrushInput", true);
                SetFloat(serialized, "minBrushDragPixels", 10f);
                SetBool(serialized, "enableQuickPlaceInput", true);
                SetInt(serialized, "quickPlacePortalKey", (int)KeyCode.F);
                SetFloat(serialized, "quickPlaceDistance", 2.2f);
                SetInt(serialized, "cancelMouseButton", 1);
                SetBool(serialized, "enableKeyboardPortalShortcut", false);
                SetBool(serialized, "useLocalDioramaWindow", true);
                SetVector2(serialized, "defaultLocalWindowSize", new Vector2(3.8f, 3.2f));
                SetVector2(serialized, "minLocalWindowSize", new Vector2(2.6f, 2.2f));
                SetVector2(serialized, "maxLocalWindowSize", new Vector2(9f, 8f));
                SetBool(serialized, "showBrushFootprintPreview", true);
                SetInt(serialized, "brushPreviewLayer", CurrentVisualLayer);
                SetFloat(serialized, "brushPreviewLift", 0.075f);
                SetFloat(serialized, "brushPreviewEdgeThickness", 0.045f);
                SetFloat(serialized, "minimumDraggedWindowWorldSize", 0.75f);
                SetColor(serialized, "brushPreviewFillColor", new Color(0.16f, 0.66f, 1f, 0.2f));
                SetColor(serialized, "brushPreviewEdgeColor", new Color(1f, 0.9f, 0.62f, 0.86f));
                serialized.ApplyModifiedPropertiesWithoutUndo();
            }

            var spawnPoint = GameObject.Find("PortalSpawnPoint");
            if (spawnPoint != null)
            {
                spawnPoint.transform.position = new Vector3(0f, 0.98f, -0.08f);
                spawnPoint.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                spawnPoint.transform.localScale = Vector3.one;
            }

            var detector = FindSceneComponent<PortalCrossingDetector>();
            if (detector != null)
            {
                var serialized = new SerializedObject(detector);
                SetFloat(serialized, "hysteresisBand", 0.06f);
                SetFloat(serialized, "minNormalMovement", 0.08f);
                serialized.ApplyModifiedPropertiesWithoutUndo();
                detector.SetArmed(false);
            }
        }

        private static void ConfigureDialoguePanelPrefab()
        {
            var contents = PrefabUtility.LoadPrefabContents(DialoguePanelPrefabPath);
            try
            {
                SetLayerRecursively(contents, UiLayer);
                NormalizeDialoguePanelPrefabLayout(contents);
                PrefabUtility.SaveAsPrefabAsset(contents, DialoguePanelPrefabPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(contents);
            }
        }

        private static void ConfigureSymbolWheelPrefab()
        {
            var contents = PrefabUtility.LoadPrefabContents(SymbolWheelPrefabPath);
            try
            {
                contents.transform.localScale = Vector3.one;
                SetLayerRecursively(contents, UiLayer);

                var canvas = contents.GetComponent<Canvas>();
                if (canvas != null)
                {
                    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.overrideSorting = true;
                    canvas.sortingOrder = 900;
                    canvas.planeDistance = 1f;
                }

                var rectTransform = contents.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.localScale = Vector3.one;
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.offsetMin = Vector2.zero;
                    rectTransform.offsetMax = Vector2.zero;
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                }

                PrefabUtility.SaveAsPrefabAsset(contents, SymbolWheelPrefabPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(contents);
            }
        }

        private static void NormalizeCanvasRect(GameObject canvasObject)
        {
            if (canvasObject.transform is not RectTransform rectTransform)
            {
                canvasObject.transform.localScale = Vector3.one;
                return;
            }

            rectTransform.localScale = Vector3.one;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        private static void NormalizeDialoguePanelPrefabLayout(GameObject contents)
        {
            if (contents.transform is RectTransform rootRect)
            {
                rootRect.localScale = Vector3.one;
                rootRect.anchorMin = Vector2.zero;
                rootRect.anchorMax = Vector2.one;
                rootRect.offsetMin = Vector2.zero;
                rootRect.offsetMax = Vector2.zero;
                rootRect.pivot = new Vector2(0.5f, 0.5f);
            }

            var panel = contents.transform.Find("Panel");
            if (panel is RectTransform panelRect)
            {
                panelRect.localScale = Vector3.one;
                panelRect.anchorMin = new Vector2(0f, 0f);
                panelRect.anchorMax = new Vector2(1f, 0.32f);
                panelRect.offsetMin = new Vector2(64f, 36f);
                panelRect.offsetMax = new Vector2(-64f, -18f);
                panelRect.pivot = new Vector2(0.5f, 0.5f);
            }
        }

        private static void ConfigurePortalFramePrefab()
        {
            var contents = PrefabUtility.LoadPrefabContents(PortalFramePrefabPath);
            try
            {
                RemoveChildIfExists(contents.transform, "Portal_Visible_Frame_Left");
                RemoveChildIfExists(contents.transform, "Portal_Visible_Frame_Right");
                RemoveChildIfExists(contents.transform, "Portal_Visible_Frame_Top");
                RemoveChildIfExists(contents.transform, "Portal_Visible_Frame_Bottom");

                var material = AssetDatabase.LoadAssetAtPath<Material>(PortalFrameMaterialPath);
                if (material == null)
                {
                    throw new InvalidOperationException($"Missing portal frame material: {PortalFrameMaterialPath}");
                }

                CreateFrameBar(contents.transform, "Portal_Visible_Frame_Left", new Vector3(-0.58f, 0f, -0.025f), new Vector3(0.075f, 1.05f, 0.04f), material);
                CreateFrameBar(contents.transform, "Portal_Visible_Frame_Right", new Vector3(0.58f, 0f, -0.025f), new Vector3(0.075f, 1.05f, 0.04f), material);
                CreateFrameBar(contents.transform, "Portal_Visible_Frame_Top", new Vector3(0f, 0.52f, -0.025f), new Vector3(1.18f, 0.075f, 0.04f), material);
                CreateFrameBar(contents.transform, "Portal_Visible_Frame_Bottom", new Vector3(0f, -0.52f, -0.025f), new Vector3(1.18f, 0.075f, 0.04f), material);

                PrefabUtility.SaveAsPrefabAsset(contents, PortalFramePrefabPath);
            }
            finally
            {
                PrefabUtility.UnloadPrefabContents(contents);
            }
        }

        private static void ConfigureTimeWindowDioramaPrefab()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(TimeWindowDioramaPrefabPath));

            var currentMaterial = AssetDatabase.LoadAssetAtPath<Material>(PortalCurrentMaterialPath);
            if (currentMaterial == null)
            {
                throw new InvalidOperationException("Time window frame material is missing.");
            }

            var veilMaterial = EnsureTimeVolumeVeilMaterial();
            var root = new GameObject("TimeWindow_Diorama");
            try
            {
                SetLayerRecursively(root, CurrentVisualLayer);

                var diorama = root.AddComponent<TimeWindowDiorama>();

                var scalableRoot = new GameObject("TimeVolume_FrameRoot").transform;
                scalableRoot.SetParent(root.transform, false);
                scalableRoot.localPosition = Vector3.zero;
                SetLayerRecursively(scalableRoot.gameObject, CurrentVisualLayer);

                CreateTimeVolumeFrame(scalableRoot, currentMaterial);

                var contentRoot = new GameObject("TimeVolume_RuntimeContent").transform;
                contentRoot.SetParent(root.transform, false);
                contentRoot.localPosition = Vector3.zero;
                SetLayerRecursively(contentRoot.gameObject, CurrentVisualLayer);

                var serialized = new SerializedObject(diorama);
                SetObject(serialized, "scalableRoot", scalableRoot);
                SetObject(serialized, "contentRoot", contentRoot);
                SetString(serialized, "sourceRootName", "Root_Past");
                SetString(serialized, "currentRootName", "Root_Current");
                SetInt(serialized, "visibleLayer", CurrentVisualLayer);
                SetBool(serialized, "replaceCurrentSpace", true);
                SetBool(serialized, "showSpaceVeil", true);
                SetObject(serialized, "spaceVeilMaterial", veilMaterial);
                SetColor(serialized, "spaceVeilColor", new Color(0.18f, 0.64f, 1f, 0.16f));
                SetColor(serialized, "pastContentTint", new Color(0.62f, 0.78f, 0.98f, 1f));
                SetFloat(serialized, "pastContentTintStrength", 1f);
                SetColor(serialized, "pastEmissionTint", new Color(0.14f, 0.42f, 0.78f, 1f));
                SetFloat(serialized, "pastEmissionStrength", 0.18f);
                SetFloat(serialized, "openAnimationDuration", 0.18f);
                SetFloat(serialized, "closeAnimationDuration", 0.14f);
                SetFloat(serialized, "animationStartScale", 0.08f);
                serialized.ApplyModifiedPropertiesWithoutUndo();

                PrefabUtility.SaveAsPrefabAsset(root, TimeWindowDioramaPrefabPath);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static Material EnsureTimeVolumeVeilMaterial()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(TimeVolumeVeilMaterialPath));

            var material = AssetDatabase.LoadAssetAtPath<Material>(TimeVolumeVeilMaterialPath);
            if (material == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Unlit/Color");
                if (shader == null)
                {
                    throw new InvalidOperationException("Could not find a shader for TimeVolume_SpaceVeil material.");
                }

                material = new Material(shader)
                {
                    name = "TimeVolume_SpaceVeil",
                    renderQueue = 3000
                };
                AssetDatabase.CreateAsset(material, TimeVolumeVeilMaterialPath);
            }

            var color = new Color(0.18f, 0.64f, 1f, 0.16f);
            material.SetColor("_BaseColor", color);
            material.SetColor("_Color", color);
            material.SetFloat("_Surface", 1f);
            material.SetFloat("_Blend", 0f);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void CreateTimeVolumeFrame(Transform parent, Material currentMaterial)
        {
            const float x = 0.5f;
            const float z = 0.5f;
            const float bottom = 0.06f;
            const float top = 1.55f;
            const float y = 0.805f;
            const float thickness = 0.028f;

            CreateDioramaBorder(parent, "TimeVolume_Front_Left", new Vector3(-x, y, -z), new Vector3(thickness, top - bottom, thickness), currentMaterial);
            CreateDioramaBorder(parent, "TimeVolume_Front_Right", new Vector3(x, y, -z), new Vector3(thickness, top - bottom, thickness), currentMaterial);
            CreateDioramaBorder(parent, "TimeVolume_Back_Left", new Vector3(-x, y, z), new Vector3(thickness, top - bottom, thickness), currentMaterial);
            CreateDioramaBorder(parent, "TimeVolume_Back_Right", new Vector3(x, y, z), new Vector3(thickness, top - bottom, thickness), currentMaterial);

            CreateDioramaBorder(parent, "TimeVolume_Front_Top", new Vector3(0f, top, -z), new Vector3(1f, thickness, thickness), currentMaterial);
            CreateDioramaBorder(parent, "TimeVolume_Back_Top", new Vector3(0f, top, z), new Vector3(1f, thickness, thickness), currentMaterial);
            CreateDioramaBorder(parent, "TimeVolume_Left_Top", new Vector3(-x, top, 0f), new Vector3(thickness, thickness, 1f), currentMaterial);
            CreateDioramaBorder(parent, "TimeVolume_Right_Top", new Vector3(x, top, 0f), new Vector3(thickness, thickness, 1f), currentMaterial);

            CreateDioramaBorder(parent, "TimeVolume_Front_Bottom", new Vector3(0f, bottom, -z), new Vector3(1f, thickness, thickness), currentMaterial);
            CreateDioramaBorder(parent, "TimeVolume_Back_Bottom", new Vector3(0f, bottom, z), new Vector3(1f, thickness, thickness), currentMaterial);
            CreateDioramaBorder(parent, "TimeVolume_Left_Bottom", new Vector3(-x, bottom, 0f), new Vector3(thickness, thickness, 1f), currentMaterial);
            CreateDioramaBorder(parent, "TimeVolume_Right_Bottom", new Vector3(x, bottom, 0f), new Vector3(thickness, thickness, 1f), currentMaterial);
        }

        private static void CreateDioramaBorder(Transform parent, string name, Vector3 localPosition, Vector3 localScale, Material material)
        {
            var border = GameObject.CreatePrimitive(PrimitiveType.Cube);
            border.name = name;
            border.transform.SetParent(parent, false);
            border.transform.localPosition = localPosition;
            border.transform.localRotation = Quaternion.identity;
            border.transform.localScale = localScale;
            ApplyRendererMaterial(border, material);
            RemoveCollider(border);
            SetLayerRecursively(border, CurrentVisualLayer);
        }

        private static GameObject SpawnCharacterPrefab(
            Transform parent,
            string prefabName,
            string instanceName,
            int layer,
            Vector3 position,
            Quaternion rotation,
            float targetMaxDimension)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Prefabs/Characters/{prefabName}.prefab");
            if (prefab == null)
            {
                throw new InvalidOperationException($"Missing character prefab: {prefabName}");
            }

            var instance = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
            if (instance == null)
            {
                throw new InvalidOperationException($"Could not instantiate character prefab: {prefabName}");
            }

            instance.name = instanceName;
            instance.transform.SetPositionAndRotation(position, rotation);
            instance.transform.localScale = Vector3.one;
            FitToMaxDimension(instance, targetMaxDimension);
            MoveBoundsMinToY(instance, position.y);
            SetLayerRecursively(instance, layer);
            return instance;
        }

        private static void ConfigureLocalPastBook(GameObject book)
        {
            var interactable = book.GetComponent<PastBookInteractable>();
            if (interactable == null)
            {
                interactable = book.AddComponent<PastBookInteractable>();
            }

            var serialized = new SerializedObject(interactable);
            SetFloat(serialized, "interactionRange", 1.25f);
            SetBool(serialized, "requirePastSide", false);
            SetBool(serialized, "reflectImmediately", true);
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ConfigureLocalNpc(GameObject npc, string dialogueAssetPath, float range)
        {
            var interactable = npc.GetComponent<NpcInteractable>();
            if (interactable == null)
            {
                interactable = npc.AddComponent<NpcInteractable>();
            }

            var dialogueAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(dialogueAssetPath);
            if (dialogueAsset == null)
            {
                throw new InvalidOperationException($"Missing dialogue asset: {dialogueAssetPath}");
            }

            var serialized = new SerializedObject(interactable);
            SetObject(serialized, "dialogueAsset", dialogueAsset);
            SetFloat(serialized, "interactionRange", range);
            SetBool(serialized, "requireMainCameraVisibility", false);
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ConfigureTmpSettings()
        {
            WriteTextAssetIfMissing(TmpLeadingCharactersPath, "([{");
            WriteTextAssetIfMissing(TmpFollowingCharactersPath, ".,!?)]}");
            AssetDatabase.ImportAsset(TmpLeadingCharactersPath);
            AssetDatabase.ImportAsset(TmpFollowingCharactersPath);

            var settings = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(TmpSettingsPath);
            var leading = AssetDatabase.LoadAssetAtPath<TextAsset>(TmpLeadingCharactersPath);
            var following = AssetDatabase.LoadAssetAtPath<TextAsset>(TmpFollowingCharactersPath);
            if (settings == null || leading == null || following == null)
            {
                throw new InvalidOperationException("TMP Settings or line-breaking TextAssets are missing.");
            }

            var serialized = new SerializedObject(settings);
            SetObject(serialized, "m_leadingCharacters", leading);
            SetObject(serialized, "m_followingCharacters", following);
            SetBool(serialized, "m_ClearDynamicDataOnBuild", false);
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(settings);
        }

        private static void CreateFrameBar(Transform parent, string name, Vector3 localPosition, Vector3 localScale, Material material)
        {
            var bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.name = name;
            bar.transform.SetParent(parent, false);
            bar.transform.localPosition = localPosition;
            bar.transform.localRotation = Quaternion.identity;
            bar.transform.localScale = localScale;
            SetLayerRecursively(bar, CurrentVisualLayer);

            var collider = bar.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = bar.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }
        }

        private static void ApplyRendererMaterial(GameObject gameObject, Material material)
        {
            var renderer = gameObject.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }
        }

        private static void RemoveCollider(GameObject gameObject)
        {
            var collider = gameObject.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }
        }

        private static void RemoveComponent<T>(GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component != null)
            {
                UnityEngine.Object.DestroyImmediate(component);
            }
        }

        private static void RemoveChildIfExists(Transform parent, string name)
        {
            var child = parent.Find(name);
            if (child != null)
            {
                UnityEngine.Object.DestroyImmediate(child.gameObject);
            }
        }

        private static void WriteTextAssetIfMissing(string path, string content)
        {
            if (File.Exists(path))
            {
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path, content);
        }

        private static void SpawnTileGrid(
            Transform parent,
            string prefabName,
            string instancePrefix,
            int layer,
            int minX,
            int maxX,
            int minZ,
            int maxZ,
            float spacing,
            float targetMaxDimension)
        {
            for (var x = minX; x <= maxX; x++)
            {
                for (var z = minZ; z <= maxZ; z++)
                {
                    SpawnZonePrefab(
                        parent,
                        prefabName,
                        $"{instancePrefix}_{x}_{z}",
                        layer,
                        new Vector3(x * spacing, 0f, z * spacing),
                        Quaternion.identity,
                        targetMaxDimension);
                }
            }
        }

        private static GameObject SpawnZonePrefab(
            Transform parent,
            string prefabName,
            string instanceName,
            int layer,
            Vector3 position,
            Quaternion rotation,
            float targetMaxDimension)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{Zone1PrefabRoot}/{prefabName}.prefab");
            if (prefab == null)
            {
                throw new InvalidOperationException($"Missing Zone1 prefab: {prefabName}");
            }

            var instance = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
            if (instance == null)
            {
                throw new InvalidOperationException($"Could not instantiate Zone1 prefab: {prefabName}");
            }

            instance.name = instanceName;
            instance.transform.SetPositionAndRotation(position, rotation);
            instance.transform.localScale = Vector3.one;
            FitToMaxDimension(instance, targetMaxDimension);
            MoveBoundsMinToY(instance, position.y);
            SetLayerRecursively(instance, layer);
            return instance;
        }

        private static void FitToMaxDimension(GameObject instance, float targetMaxDimension)
        {
            var bounds = TryGetRendererBounds(instance);
            if (!bounds.HasValue)
            {
                return;
            }

            var size = bounds.Value.size;
            var maxDimension = Mathf.Max(size.x, size.y, size.z);
            if (maxDimension <= 0.001f)
            {
                return;
            }

            var scale = targetMaxDimension / maxDimension;
            instance.transform.localScale *= scale;
        }

        private static void MoveBoundsMinToY(GameObject instance, float targetY)
        {
            var bounds = TryGetRendererBounds(instance);
            if (!bounds.HasValue)
            {
                return;
            }

            var position = instance.transform.position;
            position.y += targetY - bounds.Value.min.y;
            instance.transform.position = position;
        }

        private static Bounds? TryGetRendererBounds(GameObject root)
        {
            var renderers = root.GetComponentsInChildren<Renderer>(true)
                .Where(renderer => renderer.enabled)
                .ToArray();
            if (renderers.Length == 0)
            {
                return null;
            }

            var bounds = renderers[0].bounds;
            for (var index = 1; index < renderers.Length; index++)
            {
                bounds.Encapsulate(renderers[index].bounds);
            }

            return bounds;
        }

        private static GameObject CreateChildRoot(Transform parent, string name, int layer)
        {
            var root = new GameObject(name);
            root.transform.SetParent(parent, false);
            root.transform.localPosition = Vector3.zero;
            SetLayerRecursively(root, layer);
            return root;
        }

        private static GameObject RequireSceneObject(string name)
        {
            var found = GameObject.Find(name);
            if (found == null)
            {
                throw new InvalidOperationException($"Missing scene object: {name}");
            }

            return found;
        }

        private static T FindSceneComponent<T>() where T : Component
        {
            return UnityEngine.Object.FindFirstObjectByType<T>(FindObjectsInactive.Include);
        }

        private static void DestroyIfExists(string name)
        {
            var found = GameObject.Find(name);
            if (found != null)
            {
                UnityEngine.Object.DestroyImmediate(found);
            }
        }

        private static void LookAt(Transform transform, Vector3 target)
        {
            var direction = target - transform.position;
            if (direction.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            }
        }

        private static void SetLayerRecursively(GameObject root, int layer)
        {
            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        private static void SetBool(SerializedObject serialized, string fieldName, bool value)
        {
            var property = RequiredProperty(serialized, fieldName);
            property.boolValue = value;
        }

        private static void SetFloat(SerializedObject serialized, string fieldName, float value)
        {
            var property = RequiredProperty(serialized, fieldName);
            property.floatValue = value;
        }

        private static void SetInt(SerializedObject serialized, string fieldName, int value)
        {
            var property = RequiredProperty(serialized, fieldName);
            property.intValue = value;
        }

        private static void SetString(SerializedObject serialized, string fieldName, string value)
        {
            var property = RequiredProperty(serialized, fieldName);
            property.stringValue = value;
        }

        private static void SetColor(SerializedObject serialized, string fieldName, Color value)
        {
            var property = RequiredProperty(serialized, fieldName);
            property.colorValue = value;
        }

        private static void SetVector2(SerializedObject serialized, string fieldName, Vector2 value)
        {
            var property = RequiredProperty(serialized, fieldName);
            property.vector2Value = value;
        }

        private static void SetObject(SerializedObject serialized, string fieldName, UnityEngine.Object value)
        {
            var property = RequiredProperty(serialized, fieldName);
            property.objectReferenceValue = value;
        }

        private static SerializedProperty RequiredProperty(SerializedObject serialized, string fieldName)
        {
            var property = serialized.FindProperty(fieldName);
            if (property == null)
            {
                throw new InvalidOperationException(
                    $"Missing serialized field '{fieldName}' on {serialized.targetObject.name}.");
            }

            return property;
        }
    }
}
