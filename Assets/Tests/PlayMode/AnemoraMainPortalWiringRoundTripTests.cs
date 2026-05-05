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
    public sealed class AnemoraMainPortalWiringRoundTripTests
    {
        private const string SceneName = "Anemora_Main";
        private const int CurrentCameraMask = (1 << 10) | (1 << 5);
        private const int PastCameraMask = (1 << 11) | (1 << 5);
        private const int CurrentPlayerLayer = 8;
        private const int PastPlayerLayer = 9;

        private static readonly Type ControllerType = Type.GetType(
            "Anemora.TimeManagement.TimeFramePortalController, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type DetectorType = Type.GetType(
            "Anemora.TimeManagement.PortalCrossingDetector, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type PolarityType = Type.GetType(
            "Anemora.TimeManagement.SceneSidePolarity, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type SwitcherType = Type.GetType(
            "Anemora.TimeManagement.PortalVisualSwitcher, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type FlashType = Type.GetType(
            "Anemora.TimeManagement.PortalFlashPlayer, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type SymbolWheelType = Type.GetType(
            "Anemora.UI.SymbolWheelController, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type SymbolType = Type.GetType(
            "Anemora.UI.SymbolType, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type PlayerControllerType = Type.GetType(
            "Anemora.Player.PrototypePlayerController, Assembly-CSharp",
            throwOnError: true);

        [UnityTest]
        public IEnumerator MainScenePortalWiringSupportsBoundaryRoundTrip()
        {
            Time.timeScale = 1f;

            var operation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
            while (!operation.isDone)
            {
                yield return null;
            }

            var player = GameObject.FindWithTag("Player");
            var controller = FindSceneComponent(ControllerType);
            var detector = FindSceneComponent(DetectorType);
            var polarity = FindSceneComponent(PolarityType);
            var switcher = FindSceneComponent(SwitcherType);
            var flash = FindSceneComponent(FlashType);
            var wheel = FindSceneComponent(SymbolWheelType);
            var mainCamera = Camera.main;

            Assert.That(player, Is.Not.Null);
            Assert.That(player.GetComponent(PlayerControllerType), Is.Not.Null);
            Assert.That(player.layer, Is.EqualTo(CurrentPlayerLayer));
            Assert.That(controller, Is.Not.Null);
            Assert.That(detector, Is.Not.Null);
            Assert.That(polarity, Is.Not.Null);
            Assert.That(switcher, Is.Not.Null);
            Assert.That(flash, Is.Not.Null);
            Assert.That(wheel, Is.Not.Null);
            Assert.That(mainCamera, Is.Not.Null);
            Assert.That(mainCamera.cullingMask, Is.EqualTo(CurrentCameraMask));

            ControllerType.GetMethod("SetLocalDioramaWindowForTests", BindingFlags.Public | BindingFlags.Instance)
                .Invoke(controller, new object[] { false });
            SelectRed(controller);
            yield return new WaitForSecondsRealtime(0.15f);

            Assert.That(GetProperty<object>(controller, "PortalInstance"), Is.Not.Null);
            Assert.That(GetProperty<object>(controller, "State").ToString(), Is.EqualTo("Open"));
            Assert.That(GetProperty<bool>(detector, "IsArmed"), Is.True);

            MovePlayerToSignedDistance(player.transform, detector, -0.18f);
            yield return null;
            yield return new WaitForSecondsRealtime(0.18f);

            Assert.That(GetProperty<object>(polarity, "CurrentSide").ToString(), Is.EqualTo("Past"));
            Assert.That(mainCamera.cullingMask, Is.EqualTo(PastCameraMask));
            Assert.That(player.layer, Is.EqualTo(PastPlayerLayer));
            Assert.That(GetProperty<object>(controller, "State").ToString(), Is.EqualTo("Open"));

            MovePlayerToSignedDistance(player.transform, detector, 0.18f);
            yield return null;
            yield return new WaitForSecondsRealtime(0.18f);

            Assert.That(GetProperty<object>(polarity, "CurrentSide").ToString(), Is.EqualTo("Current"));
            Assert.That(mainCamera.cullingMask, Is.EqualTo(CurrentCameraMask));
            Assert.That(player.layer, Is.EqualTo(CurrentPlayerLayer));
            Assert.That(Time.timeScale, Is.EqualTo(1f).Within(0.0001f));
        }

        private static Component FindSceneComponent(Type type)
        {
            var activeScene = SceneManager.GetActiveScene();
            return Resources.FindObjectsOfTypeAll(type)
                .OfType<Component>()
                .FirstOrDefault(component => component.gameObject.scene == activeScene);
        }

        private static void SelectRed(Component controller)
        {
            var red = Enum.Parse(SymbolType, "Red");
            ControllerType.GetMethod("HandleSymbolSelected", BindingFlags.Public | BindingFlags.Instance)
                .Invoke(controller, new[] { red });
        }

        private static void MovePlayerToSignedDistance(Transform player, Component detector, float targetSignedDistance)
        {
            var normal = GetProperty<Vector3>(detector, "PlaneNormal");
            var currentDistance = (float)DetectorType
                .GetMethod("GetSignedDistance", BindingFlags.Public | BindingFlags.Instance)
                .Invoke(detector, new object[] { player.position });
            player.position += normal * (targetSignedDistance - currentDistance);
        }

        private static T GetProperty<T>(Component target, string propertyName)
        {
            return (T)target.GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
                .GetValue(target);
        }
    }
}
