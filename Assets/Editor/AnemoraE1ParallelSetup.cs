using System.Collections.Generic;
using System.IO;
using Anemora.TimeManagement;
using Anemora.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Anemora.EditorTools
{
    public static class AnemoraE1ParallelSetup
    {
        private const string PortalDirectory = "Assets/Art/Materials/Portal";
        private const string UiPrefabDirectory = "Assets/UI/Prefabs";
        private const string UiSpriteDirectory = "Assets/UI/Sprites";
        private const string ScenesDirectory = "Assets/Scenes";

        private const string PortalMaskMaterialPath = PortalDirectory + "/PortalMask.mat";
        private const string InsideOnlyMaterialPath = PortalDirectory + "/InsideOnly.mat";
        private const string CurrentDebugMaterialPath = PortalDirectory + "/Debug_Current.mat";
        private const string PastDebugMaterialPath = PortalDirectory + "/Debug_Past.mat";

        private const string SandboxScenePath = ScenesDirectory + "/Sandbox_E1_Stencil.unity";
        private const string MainScenePath = ScenesDirectory + "/Anemora_Main.unity";
        private const string SymbolWheelPrefabPath = UiPrefabDirectory + "/SymbolWheel.prefab";

        private const int LayerCurrentCollider = 8;
        private const int LayerPastCollider = 9;
        private const int LayerCurrentVisual = 10;
        private const int LayerPastVisual = 11;

        [MenuItem("Anemora/Setup/E1 Parallel Assets")]
        public static void Run()
        {
            EnsureFolder(PortalDirectory);
            EnsureFolder(UiPrefabDirectory);
            EnsureFolder(UiSpriteDirectory);
            EnsureFolder(ScenesDirectory);

            EnsureLayers();

            var portalMask = EnsureMaterial(PortalMaskMaterialPath, "Anemora/Portal/PortalMask", Color.clear);
            var insideOnly = EnsureMaterial(InsideOnlyMaterialPath, "Anemora/Portal/InsideOnly", new Color(0.16f, 0.54f, 1f, 1f));
            var currentDebug = EnsureMaterial(CurrentDebugMaterialPath, "Universal Render Pipeline/Lit", new Color(0.83f, 0.72f, 0.52f, 1f));
            var pastDebug = EnsureMaterial(PastDebugMaterialPath, "Universal Render Pipeline/Lit", new Color(0.46f, 0.64f, 0.88f, 1f));

            var redSprite = EnsureSymbolSprite(UiSpriteDirectory + "/symbol_red.png", new Color(0.92f, 0.18f, 0.16f, 1f));
            var whiteSprite = EnsureSymbolSprite(UiSpriteDirectory + "/symbol_white_disabled.png", new Color(0.92f, 0.92f, 0.86f, 1f));
            var blueSprite = EnsureSymbolSprite(UiSpriteDirectory + "/symbol_blue_disabled.png", new Color(0.25f, 0.48f, 0.95f, 1f));

            CreateSandboxScene(portalMask, insideOnly, currentDebug);
            CreateMainScene(currentDebug, pastDebug);
            CreateSymbolWheelPrefab(redSprite, whiteSprite, blueSprite);
            EnsureBuildSettingsScenes();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Anemora E1 parallel setup complete.");
        }

        [MenuItem("Anemora/Setup/Capture E1 Screenshots")]
        public static void CaptureE1Screenshots()
        {
            EditorSceneManager.OpenScene(SandboxScenePath, OpenSceneMode.Single);

            var camera = Camera.main;
            if (camera == null)
            {
                throw new System.InvalidOperationException("Sandbox_E1_Stencil has no Main Camera.");
            }

            var outputDirectory = "docs/devlog/screenshots";
            Directory.CreateDirectory(outputDirectory);

            CaptureCamera(camera, new Vector3(0f, 1.25f, -4.2f), new Vector3(0f, 1.05f, 0.75f), outputDirectory + "/e1_portal_front.png");
            CaptureCamera(camera, new Vector3(2.7f, 1.25f, -0.2f), new Vector3(0f, 1f, 0.3f), outputDirectory + "/e1_portal_side.png");
            CaptureCamera(camera, new Vector3(0f, 1.25f, 3.4f), new Vector3(0f, 1f, 0f), outputDirectory + "/e1_portal_back.png");

            AssetDatabase.Refresh();
            Debug.Log("Anemora E1 screenshots captured.");
        }

        private static void EnsureLayers()
        {
            var tagManagerAssets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            var tagManager = new SerializedObject(tagManagerAssets[0]);
            var layers = tagManager.FindProperty("layers");

            SetLayerName(layers, LayerCurrentCollider, "Layer_Current_Collider");
            SetLayerName(layers, LayerPastCollider, "Layer_Past_Collider");
            SetLayerName(layers, LayerCurrentVisual, "Layer_Current_Visual");
            SetLayerName(layers, LayerPastVisual, "Layer_Past_Visual");

            tagManager.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void SetLayerName(SerializedProperty layers, int index, string name)
        {
            var layer = layers.GetArrayElementAtIndex(index);
            if (string.IsNullOrEmpty(layer.stringValue) || layer.stringValue == name)
            {
                layer.stringValue = name;
            }
            else
            {
                Debug.LogWarning($"Layer {index} is already '{layer.stringValue}', expected '{name}'.");
            }
        }

        private static Material EnsureMaterial(string path, string shaderName, Color color)
        {
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            var shader = Shader.Find(shaderName) ?? Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (shader == null)
            {
                throw new System.InvalidOperationException($"Shader not found: {shaderName}");
            }

            if (material == null)
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }

            material.shader = shader;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }
            else if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", color);
            }

            EditorUtility.SetDirty(material);
            return material;
        }

        private static Sprite EnsureSymbolSprite(string path, Color fill)
        {
            if (!File.Exists(path))
            {
                var texture = new Texture2D(64, 64, TextureFormat.RGBA32, false);
                var transparent = new Color(0f, 0f, 0f, 0f);
                var border = new Color(0.08f, 0.07f, 0.06f, 1f);
                var center = new Vector2(31.5f, 31.5f);

                for (var y = 0; y < 64; y++)
                {
                    for (var x = 0; x < 64; x++)
                    {
                        var distance = Vector2.Distance(new Vector2(x, y), center);
                        texture.SetPixel(x, y, distance <= 30f ? (distance >= 25f ? border : fill) : transparent);
                    }
                }

                texture.Apply();
                File.WriteAllBytes(path, texture.EncodeToPNG());
                Object.DestroyImmediate(texture);
            }

            AssetDatabase.ImportAsset(path);
            if (AssetImporter.GetAtPath(path) is TextureImporter importer)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spritePixelsPerUnit = 64;
                importer.mipmapEnabled = false;
                importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SaveAndReimport();
            }

            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        private static void CreateSandboxScene(Material portalMask, Material insideOnly, Material currentDebug)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var camera = CreateMainCamera(new Vector3(0f, 1.25f, -4.2f), new Vector3(0f, 1.05f, 0.75f));
            camera.backgroundColor = new Color(0.08f, 0.1f, 0.13f, 1f);

            CreateDirectionalLight(new Vector3(40f, -30f, 0f));

            var floor = CreateCube("Reference_Floor", new Vector3(0f, -0.05f, 0.75f), new Vector3(4f, 0.1f, 4f), currentDebug, LayerCurrentVisual);
            floor.transform.SetParent(null);

            var portal = GameObject.CreatePrimitive(PrimitiveType.Quad);
            portal.name = "PortalMask_Quad";
            portal.transform.position = new Vector3(0f, 1f, 0f);
            portal.transform.localScale = new Vector3(1.2f, 1.8f, 1f);
            portal.GetComponent<MeshRenderer>().sharedMaterial = portalMask;

            var insideCube = CreateCube("InsideOnly_Cube_VisibleThroughPortal", new Vector3(0f, 1f, 1.45f), Vector3.one * 0.72f, insideOnly, LayerPastVisual);
            insideCube.transform.rotation = Quaternion.Euler(0f, 35f, 0f);

            var sideMarker = CreateCube("Reference_Current_Cube_OutsidePortal", new Vector3(-1.45f, 0.45f, 0.65f), Vector3.one * 0.45f, currentDebug, LayerCurrentVisual);
            sideMarker.transform.rotation = Quaternion.Euler(0f, -25f, 0f);

            EditorSceneManager.SaveScene(scene, SandboxScenePath);
        }

        private static void CreateMainScene(Material currentDebug, Material pastDebug)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var mainCamera = CreateMainCamera(new Vector3(0f, 2.35f, -5.5f), new Vector3(0f, 0.8f, 0.45f));
            mainCamera.cullingMask = (1 << LayerCurrentVisual) | (1 << LayerPastVisual) | 1;

            CreateDirectionalLight(new Vector3(42f, -28f, 0f));

            var rootCurrent = new GameObject("Root_Current");
            CreateCube("Current_Floor", new Vector3(0f, -0.05f, 0f), new Vector3(5f, 0.1f, 5f), currentDebug, LayerCurrentVisual).transform.SetParent(rootCurrent.transform);
            CreateCube("Current_BedPlaceholder", new Vector3(-1.2f, 0.25f, 0.55f), new Vector3(1.1f, 0.35f, 0.55f), currentDebug, LayerCurrentVisual).transform.SetParent(rootCurrent.transform);

            var rootPast = new GameObject("Root_Past");
            CreateCube("Past_Floor", new Vector3(0f, 0.02f, 0f), new Vector3(5f, 0.08f, 5f), pastDebug, LayerPastVisual).transform.SetParent(rootPast.transform);
            CreateCube("Past_Table", new Vector3(0.9f, 0.35f, 0.65f), new Vector3(0.9f, 0.18f, 0.55f), pastDebug, LayerPastVisual).transform.SetParent(rootPast.transform);
            CreateCube("Past_BookPlaceholder", new Vector3(0.9f, 0.53f, 0.65f), new Vector3(0.3f, 0.06f, 0.22f), pastDebug, LayerPastVisual).transform.SetParent(rootPast.transform);

            var npc = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            npc.name = "Past_NPC_Placeholder";
            npc.layer = LayerPastVisual;
            npc.transform.SetParent(rootPast.transform);
            npc.transform.position = new Vector3(-0.75f, 0.9f, 1f);
            npc.transform.localScale = new Vector3(0.35f, 0.7f, 0.35f);
            npc.GetComponent<MeshRenderer>().sharedMaterial = pastDebug;

            var pastCameraObject = new GameObject("Camera_Past");
            var pastCamera = pastCameraObject.AddComponent<Camera>();
            pastCamera.enabled = false;
            pastCamera.cullingMask = 1 << LayerPastVisual;
            pastCamera.clearFlags = CameraClearFlags.Depth;

            var sync = pastCameraObject.AddComponent<PastCameraSync>();
            var syncObject = new SerializedObject(sync);
            syncObject.FindProperty("sourceCamera").objectReferenceValue = mainCamera;
            syncObject.FindProperty("targetCamera").objectReferenceValue = pastCamera;
            syncObject.ApplyModifiedPropertiesWithoutUndo();

            var registryObject = new GameObject("SceneRootRegistry");
            var registry = registryObject.AddComponent<SceneRootRegistry>();
            var serializedRegistry = new SerializedObject(registry);
            serializedRegistry.FindProperty("rootCurrent").objectReferenceValue = rootCurrent;
            serializedRegistry.FindProperty("rootPast").objectReferenceValue = rootPast;
            serializedRegistry.FindProperty("mainCamera").objectReferenceValue = mainCamera;
            serializedRegistry.FindProperty("pastCamera").objectReferenceValue = pastCamera;
            serializedRegistry.FindProperty("pastRootVisibleOnStart").boolValue = true;
            serializedRegistry.ApplyModifiedPropertiesWithoutUndo();

            EditorSceneManager.SaveScene(scene, MainScenePath);
        }

        private static Camera CreateMainCamera(Vector3 position, Vector3 lookAt)
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = position;
            cameraObject.transform.rotation = Quaternion.LookRotation(lookAt - position, Vector3.up);

            var camera = cameraObject.AddComponent<Camera>();
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 100f;
            camera.fieldOfView = 45f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.12f, 0.13f, 0.15f, 1f);

            cameraObject.AddComponent<AudioListener>();
            cameraObject.AddComponent<UniversalAdditionalCameraData>();
            return camera;
        }

        private static void CreateDirectionalLight(Vector3 eulerAngles)
        {
            var lightObject = new GameObject("Directional Light");
            lightObject.transform.rotation = Quaternion.Euler(eulerAngles);
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.4f;
            light.shadows = LightShadows.Soft;
        }

        private static GameObject CreateCube(string name, Vector3 position, Vector3 scale, Material material, int layer)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.layer = layer;
            cube.transform.position = position;
            cube.transform.localScale = scale;
            cube.GetComponent<MeshRenderer>().sharedMaterial = material;
            return cube;
        }

        private static void CreateSymbolWheelPrefab(Sprite redSprite, Sprite whiteSprite, Sprite blueSprite)
        {
            var root = new GameObject("SymbolWheel", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.sortingOrder = 20;

            var scaler = root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            var red = CreateSymbolImage("RedSymbol", root.transform, redSprite, new Vector2(0f, 0f), Color.white, true);
            var white = CreateSymbolImage("WhiteSymbol_Disabled", root.transform, whiteSprite, new Vector2(0f, 92f), new Color(1f, 1f, 1f, 0.4f), false);
            var blue = CreateSymbolImage("BlueSymbol_Disabled", root.transform, blueSprite, new Vector2(0f, -92f), new Color(1f, 1f, 1f, 0.4f), false);

            var controller = root.AddComponent<SymbolWheelController>();
            var serializedController = new SerializedObject(controller);
            serializedController.FindProperty("rootCanvas").objectReferenceValue = canvas;
            serializedController.FindProperty("redSymbol").objectReferenceValue = red;
            serializedController.FindProperty("whiteSymbol").objectReferenceValue = white;
            serializedController.FindProperty("blueSymbol").objectReferenceValue = blue;
            serializedController.ApplyModifiedPropertiesWithoutUndo();

            PrefabUtility.SaveAsPrefabAsset(root, SymbolWheelPrefabPath);
            Object.DestroyImmediate(root);
        }

        private static Image CreateSymbolImage(string name, Transform parent, Sprite sprite, Vector2 anchoredPosition, Color color, bool raycastTarget)
        {
            var symbol = new GameObject(name, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            symbol.transform.SetParent(parent, false);

            var rectTransform = symbol.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(72f, 72f);

            var image = symbol.GetComponent<Image>();
            image.sprite = sprite;
            image.color = color;
            image.raycastTarget = raycastTarget;
            return image;
        }

        private static void EnsureBuildSettingsScenes()
        {
            var scenePaths = new[] { SandboxScenePath, MainScenePath };
            var scenes = new List<EditorBuildSettingsScene>();

            foreach (var existing in EditorBuildSettings.scenes)
            {
                if (existing == null || string.IsNullOrEmpty(existing.path))
                {
                    continue;
                }

                var duplicate = false;
                foreach (var scenePath in scenePaths)
                {
                    duplicate |= existing.path == scenePath;
                }

                if (!duplicate)
                {
                    scenes.Add(existing);
                }
            }

            foreach (var scenePath in scenePaths)
            {
                scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            }

            EditorBuildSettings.scenes = scenes.ToArray();
        }

        private static void CaptureCamera(Camera camera, Vector3 position, Vector3 lookAt, string path)
        {
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
            var descriptor = new RenderTextureDescriptor(1280, 720)
            {
                graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm,
                depthStencilFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.D24_UNorm_S8_UInt,
                msaaSamples = 1
            };
            camera.targetTexture = new RenderTexture(descriptor);

            var previousActive = RenderTexture.active;
            try
            {
                var request = new RenderPipeline.StandardRequest
                {
                    destination = camera.targetTexture,
                    mipLevel = 0,
                    slice = 0,
                    face = CubemapFace.Unknown
                };

                RenderPipeline.SubmitRenderRequest(camera, request);
                RenderTexture.active = camera.targetTexture;

                var texture = new Texture2D(camera.targetTexture.width, camera.targetTexture.height, TextureFormat.RGBA32, false);
                texture.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
                texture.Apply();
                File.WriteAllBytes(path, texture.EncodeToPNG());
                Object.DestroyImmediate(texture);
            }
            finally
            {
                RenderTexture.active = previousActive;
                Object.DestroyImmediate(camera.targetTexture);
                camera.targetTexture = null;
            }
        }

        private static void EnsureFolder(string path)
        {
            var parts = path.Split('/');
            var current = parts[0];

            for (var index = 1; index < parts.Length; index++)
            {
                var next = current + "/" + parts[index];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[index]);
                }

                current = next;
            }
        }
    }
}
