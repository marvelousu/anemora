using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class DemoPlayableSmokeTests
    {
        private const string SceneName = "Anemora_Main";
        private const int UiLayer = 5;
        private const int CurrentCameraMask = (1 << 10) | (1 << 5);
        private const int CurrentVisualLayer = 10;
        private const int PastVisualLayer = 11;

        private static readonly Type SymbolWheelType = ResolveRuntimeType("Anemora.UI.SymbolWheelController");
        private static readonly Type PortalControllerType = ResolveRuntimeType("Anemora.TimeManagement.TimeFramePortalController");
        private static readonly Type PortalStateType = ResolveRuntimeType("Anemora.TimeManagement.PortalState");
        private static readonly Type SceneSidePolarityType = ResolveRuntimeType("Anemora.TimeManagement.SceneSidePolarity");
        private static readonly Type TimeWindowDioramaType = ResolveRuntimeType("Anemora.TimeManagement.TimeWindowDiorama");
        private static readonly Type DialogueDisplayType = ResolveRuntimeType("Anemora.Dialogue.DialogueDisplay");
        private static readonly Type NpcInteractableType = ResolveRuntimeType("Anemora.Dialogue.NpcInteractable");

        [UnityTest]
        public IEnumerator MainSceneHasVisibleDemoEnvironmentAndTopmostUi()
        {
            yield return LoadMainScene();

            var currentRoot = GameObject.Find("DemoZone1_Current");
            var pastRoot = GameObject.Find("DemoZone1_Past");
            var dialogueCanvas = GameObject.Find("DialogueCanvas")?.GetComponent<Canvas>();
            var symbolWheel = FindSceneComponent(SymbolWheelType);

            Assert.That(currentRoot, Is.Not.Null);
            Assert.That(pastRoot, Is.Not.Null);
            Assert.That(CountEnabledRenderersOnLayer(currentRoot, CurrentVisualLayer), Is.GreaterThanOrEqualTo(20));
            Assert.That(CountEnabledRenderersOnLayer(pastRoot, PastVisualLayer), Is.GreaterThanOrEqualTo(20));

            Assert.That(dialogueCanvas, Is.Not.Null);
            Assert.That(dialogueCanvas.renderMode, Is.EqualTo(RenderMode.ScreenSpaceOverlay));
            Assert.That(dialogueCanvas.sortingOrder, Is.GreaterThanOrEqualTo(1000));
            Assert.That(dialogueCanvas.GetComponentsInChildren<Transform>(true).All(child => child.gameObject.layer == UiLayer), Is.True);

            Assert.That(symbolWheel, Is.Not.Null);
            Assert.That(symbolWheel.GetComponentInParent<Canvas>().renderMode, Is.EqualTo(RenderMode.ScreenSpaceOverlay));
            Assert.That((bool)GetProperty(symbolWheel, "IsVisible"), Is.False);
        }

        [UnityTest]
        public IEnumerator BrushPortalAndNpcDialogueAreUsableInMainScene()
        {
            yield return LoadMainScene();

            var player = GameObject.FindWithTag("Player");
            var portal = FindSceneComponent(PortalControllerType);
            var polarity = FindSceneComponent(SceneSidePolarityType);
            var dialogue = FindSceneComponent(DialogueDisplayType);
            var mainCamera = Camera.main;

            Assert.That(player, Is.Not.Null);
            Assert.That(portal, Is.Not.Null);
            Assert.That(polarity, Is.Not.Null);
            Assert.That(dialogue, Is.Not.Null);
            Assert.That(mainCamera, Is.Not.Null);
            Assert.That((bool)GetProperty(portal, "UsesLocalDioramaWindow"), Is.True);

            Assert.That((bool)Invoke(portal, "TryPlaceQuickLocalWindowForTests"), Is.True);
            yield return new WaitForSecondsRealtime(0.25f);

            Assert.That(GetProperty(portal, "State"), Is.EqualTo(Enum.Parse(PortalStateType, "Open")));
            Assert.That(GetProperty(polarity, "CurrentSide").ToString(), Is.EqualTo("Current"));
            var quickPortalInstance = (GameObject)GetProperty(portal, "PortalInstance");
            Assert.That(quickPortalInstance, Is.Not.Null);
            Assert.That(quickPortalInstance.transform.rotation.eulerAngles.y, Is.EqualTo(0f).Within(0.1f));
            var quickWindow = quickPortalInstance.GetComponent(TimeWindowDioramaType);
            Assert.That(quickWindow, Is.Not.Null);
            Assert.That((int)GetProperty(quickWindow, "RuntimeContentCount"), Is.GreaterThan(0));
            Assert.That((int)GetProperty(quickWindow, "RuntimeVeilCount"), Is.GreaterThanOrEqualTo(8));

            var sourceBook = GameObject.Find("Past_BookPlaceholder");
            var clonedBook = FindChildByName(quickPortalInstance.transform, "TimeVolume_Past_BookPlaceholder");
            var footprint = FindChildByName(quickPortalInstance.transform, "TimeVolume_SpaceVeil_Footprint");
            var spaceVeilBack = FindChildByName(quickPortalInstance.transform, "TimeVolume_SpaceVeil_Back");
            var currentTable = GameObject.Find("Current_Table_Chair");
            Assert.That(sourceBook, Is.Not.Null);
            Assert.That(clonedBook, Is.Not.Null);
            Assert.That(footprint, Is.Not.Null);
            Assert.That(spaceVeilBack, Is.Not.Null);
            Assert.That(currentTable, Is.Not.Null);
            Assert.That(CountEnabledRenderers(currentTable), Is.EqualTo(0));
            var quickSize = (Vector2)GetProperty(quickWindow, "Size");
            Assert.That(footprint.localScale.x, Is.EqualTo(quickSize.x).Within(0.05f));
            Assert.That(footprint.localScale.z, Is.EqualTo(quickSize.y).Within(0.05f));
            Assert.That(Vector2.Distance(
                    new Vector2(sourceBook.transform.position.x, sourceBook.transform.position.z),
                    new Vector2(clonedBook.position.x, clonedBook.position.z)),
                Is.LessThan(0.04f));

            Invoke(portal, "ClosePortal");
            yield return new WaitForSecondsRealtime(0.25f);
            Assert.That(GetProperty(portal, "State"), Is.EqualTo(Enum.Parse(PortalStateType, "Normal")));
            Assert.That(CountEnabledRenderers(currentTable), Is.GreaterThan(0));

            var dragStartWorld = new Vector3(-0.8f, 0f, 0.15f);
            var dragEndWorld = new Vector3(1.25f, 0f, 1.65f);
            var dragStart = (Vector2)mainCamera.WorldToScreenPoint(dragStartWorld);
            var dragEnd = (Vector2)mainCamera.WorldToScreenPoint(dragEndWorld);
            ResolveExpectedDragBounds(portal, mainCamera, dragStart, dragEnd, out var expectedDragCenter, out var expectedDragSize);

            Assert.That((bool)Invoke(portal, "TryUpdateBrushPreviewForTests", dragStart, dragEnd), Is.True);
            var preview = GameObject.Find("TimeWindow_BrushPreview_Runtime");
            var previewFill = preview != null ? FindChildByName(preview.transform, "TimeWindow_BrushPreview_Fill") : null;
            Assert.That(preview, Is.Not.Null);
            Assert.That(preview.transform.position.x, Is.EqualTo(expectedDragCenter.x).Within(0.04f));
            Assert.That(preview.transform.position.z, Is.EqualTo(expectedDragCenter.z).Within(0.04f));
            Assert.That(previewFill, Is.Not.Null);
            Assert.That(previewFill.localScale.x, Is.EqualTo(expectedDragSize.x).Within(0.04f));
            Assert.That(previewFill.localScale.z, Is.EqualTo(expectedDragSize.y).Within(0.04f));
            Invoke(portal, "ClearBrushPreviewForTests");
            yield return null;
            Assert.That(GameObject.Find("TimeWindow_BrushPreview_Runtime"), Is.Null);

            Assert.That(
                (bool)Invoke(portal, "TryCompleteBrushStrokeForTests", dragStart, dragEnd, true),
                Is.True);
            yield return new WaitForSecondsRealtime(0.25f);

            Assert.That(GetProperty(portal, "State"), Is.EqualTo(Enum.Parse(PortalStateType, "Open")));
            Assert.That(GetProperty(polarity, "CurrentSide").ToString(), Is.EqualTo("Current"));
            Assert.That(mainCamera.cullingMask, Is.EqualTo(CurrentCameraMask));

            var portalInstance = (GameObject)GetProperty(portal, "PortalInstance");
            Assert.That(portalInstance, Is.Not.Null);
            Assert.That(portalInstance.transform.rotation.eulerAngles.y, Is.EqualTo(0f).Within(0.1f));
            Assert.That(portalInstance.transform.position.x, Is.EqualTo(expectedDragCenter.x).Within(0.04f));
            Assert.That(portalInstance.transform.position.z, Is.EqualTo(expectedDragCenter.z).Within(0.04f));
            var timeWindow = portalInstance.GetComponent(TimeWindowDioramaType);
            Assert.That(timeWindow, Is.Not.Null);
            var dragWindowSize = (Vector2)GetProperty(timeWindow, "Size");
            Assert.That(dragWindowSize.x, Is.EqualTo(expectedDragSize.x).Within(0.04f));
            Assert.That(dragWindowSize.y, Is.EqualTo(expectedDragSize.y).Within(0.04f));
            var dragFootprint = FindChildByName(portalInstance.transform, "TimeVolume_SpaceVeil_Footprint");
            Assert.That(dragFootprint, Is.Not.Null);
            Assert.That(dragFootprint.localScale.x, Is.EqualTo(expectedDragSize.x).Within(0.04f));
            Assert.That(dragFootprint.localScale.z, Is.EqualTo(expectedDragSize.y).Within(0.04f));
            Assert.That(CountEnabledRenderersOnLayer(portalInstance, CurrentVisualLayer), Is.GreaterThanOrEqualTo(8));
            Assert.That(CountEnabledRenderersOnLayer(portalInstance, PastVisualLayer), Is.EqualTo(0));

            player.transform.position = portalInstance.transform.position + new Vector3(0.08f, 0.62f, 0.08f);
            yield return null;
            Assert.That((bool)GetProperty(timeWindow, "IsPlayerInside"), Is.True);

            var interactable = portalInstance
                .GetComponentsInChildren(NpcInteractableType, true)
                .OfType<Component>()
                .FirstOrDefault();
            Assert.That(interactable, Is.Not.Null);

            player.transform.position = interactable.transform.position + new Vector3(0.2f, 0f, 0f);
            yield return null;

            Assert.That((bool)Invoke(interactable, "TryInteract"), Is.True);
            Assert.That((bool)GetProperty(dialogue, "IsVisible"), Is.True);
            Assert.That((string)GetProperty(dialogue, "CurrentText"), Is.Not.Empty);

            Invoke(dialogue, "Close");
            Assert.That((bool)GetProperty(dialogue, "IsVisible"), Is.False);
        }

        private static IEnumerator LoadMainScene()
        {
            var operation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
            while (!operation.isDone)
            {
                yield return null;
            }

            yield return null;
        }

        private static int CountEnabledRenderersOnLayer(GameObject root, int layer)
        {
            return root.GetComponentsInChildren<Renderer>(true)
                .Count(renderer => renderer.enabled && renderer.gameObject.layer == layer);
        }

        private static int CountEnabledRenderers(GameObject root)
        {
            return root.GetComponentsInChildren<Renderer>(true)
                .Count(renderer => renderer.enabled);
        }

        private static Type ResolveRuntimeType(string fullName)
        {
            return Type.GetType($"{fullName}, Anemora.Dialogue", throwOnError: false) ??
                   Type.GetType($"{fullName}, Assembly-CSharp", throwOnError: true);
        }

        private static Component FindSceneComponent(Type type)
        {
            var activeScene = SceneManager.GetActiveScene();
            return Resources.FindObjectsOfTypeAll(type)
                .OfType<Component>()
                .FirstOrDefault(component => component.gameObject.scene == activeScene);
        }

        private static Transform FindChildByName(Transform root, string childName)
        {
            return root.GetComponentsInChildren<Transform>(true)
                .FirstOrDefault(child => child.name == childName);
        }

        private static void ResolveExpectedDragBounds(object portal, Camera camera, Vector2 start, Vector2 end, out Vector3 center, out Vector2 size)
        {
            Assert.That(TryScreenPointToGround(camera, start, out var startWorld), Is.True);
            Assert.That(TryScreenPointToGround(camera, end, out var endWorld), Is.True);

            var rawSize = new Vector2(Mathf.Abs(endWorld.x - startWorld.x), Mathf.Abs(endWorld.z - startWorld.z));
            var maxSize = (Vector2)GetField(portal, "maxLocalWindowSize");
            var minimum = Mathf.Max(0.05f, (float)GetField(portal, "minimumDraggedWindowWorldSize"));
            size = new Vector2(
                Mathf.Clamp(rawSize.x, minimum, Mathf.Max(minimum, maxSize.x)),
                Mathf.Clamp(rawSize.y, minimum, Mathf.Max(minimum, maxSize.y)));
            center = new Vector3(
                (startWorld.x + endWorld.x) * 0.5f,
                0.035f,
                (startWorld.z + endWorld.z) * 0.5f);
        }

        private static bool TryScreenPointToGround(Camera camera, Vector2 screenPosition, out Vector3 worldPosition)
        {
            if (camera == null)
            {
                worldPosition = default;
                return false;
            }

            var ray = camera.ScreenPointToRay(screenPosition);
            var plane = new Plane(Vector3.up, Vector3.zero);
            if (!plane.Raycast(ray, out var distance))
            {
                worldPosition = default;
                return false;
            }

            worldPosition = ray.GetPoint(distance);
            return true;
        }

        private static object GetProperty(object target, string propertyName)
        {
            return target.GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(target);
        }

        private static object GetField(object target, string fieldName)
        {
            return target.GetType()
                .GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(target);
        }

        private static object Invoke(object target, string methodName, params object[] parameters)
        {
            return target.GetType()
                .GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(target, parameters);
        }
    }
}
