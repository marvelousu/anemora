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
    public sealed class G4ActionRecordReflectionE2ETests
    {
        private const string SceneName = "Anemora_Main";
        private const string ReflectionsRootName = "ActionRecordReflections_Current";

        private static readonly Type ControllerType = Type.GetType(
            "Anemora.TimeManagement.TimeFramePortalController, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type DetectorType = Type.GetType(
            "Anemora.TimeManagement.PortalCrossingDetector, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type PolarityType = Type.GetType(
            "Anemora.TimeManagement.SceneSidePolarity, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type SymbolType = Type.GetType(
            "Anemora.UI.SymbolType, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type ActionRecordRuntimeType = Type.GetType(
            "Anemora.TimeManagement.ActionRecordRuntime, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type BookReflectorType = Type.GetType(
            "Anemora.TimeManagement.Reflectors.BookReflector, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type PastBookInteractableType = Type.GetType(
            "Anemora.TimeManagement.Reflectors.PastBookInteractable, Assembly-CSharp",
            throwOnError: true);

        [UnityTest]
        public IEnumerator PastBookInteractionReflectsOneCurrentBookOnReturn()
        {
            Time.timeScale = 1f;
            yield return LoadMainScene();

            var player = GameObject.FindWithTag("Player");
            var controller = FindSceneComponent(ControllerType);
            var detector = FindSceneComponent(DetectorType);
            var polarity = FindSceneComponent(PolarityType);
            var runtime = FindSceneComponent(ActionRecordRuntimeType);
            var reflector = FindSceneComponent(BookReflectorType);
            var pastBook = FindSceneComponent(PastBookInteractableType);
            var reflectionsRoot = GameObject.Find(ReflectionsRootName);

            Assert.That(player, Is.Not.Null);
            Assert.That(controller, Is.Not.Null);
            Assert.That(detector, Is.Not.Null);
            Assert.That(polarity, Is.Not.Null);
            Assert.That(runtime, Is.Not.Null);
            Assert.That(reflector, Is.Not.Null);
            Assert.That(pastBook, Is.Not.Null);
            Assert.That(reflectionsRoot, Is.Not.Null);

            SetDurationsForTests(controller, 0f, 0f, 0f);
            SelectRed(controller);
            yield return WaitForPortalOpen(controller);

            yield return CrossTo(player.transform, detector, polarity, "Past");

            player.transform.position = pastBook.transform.position;
            var interacted = (bool)PastBookInteractableType
                .GetMethod("TryInteract", BindingFlags.Public | BindingFlags.Instance)
                .Invoke(pastBook, new object[] { player.transform });
            Assert.That(interacted, Is.True);
            Assert.That(pastBook.gameObject.activeSelf, Is.False);

            yield return CrossTo(player.transform, detector, polarity, "Current");

            Assert.That(GetProperty<int>(reflector, "ReflectionCount"), Is.EqualTo(1));
            Assert.That(CountReflectedBooks(reflectionsRoot), Is.EqualTo(1));

            yield return CrossTo(player.transform, detector, polarity, "Past");
            yield return CrossTo(player.transform, detector, polarity, "Current");

            Assert.That(GetProperty<int>(reflector, "ReflectionCount"), Is.EqualTo(1));
            Assert.That(CountReflectedBooks(reflectionsRoot), Is.EqualTo(1));
            Assert.That(Time.timeScale, Is.EqualTo(1f).Within(0.0001f));
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

        private static IEnumerator WaitForPortalOpen(Component controller)
        {
            var deadline = Time.realtimeSinceStartup + 2f;
            while (Time.realtimeSinceStartup < deadline &&
                   GetProperty<object>(controller, "State").ToString() != "Open")
            {
                yield return null;
            }

            Assert.That(GetProperty<object>(controller, "State").ToString(), Is.EqualTo("Open"));
        }

        private static IEnumerator CrossTo(
            Transform player,
            Component detector,
            Component polarity,
            string targetSide)
        {
            var targetDistance = targetSide == "Past" ? -0.18f : 0.18f;
            MovePlayerToSignedDistance(player, detector, targetDistance);
            yield return null;
            yield return new WaitForSecondsRealtime(0.08f);
            Assert.That(GetProperty<object>(polarity, "CurrentSide").ToString(), Is.EqualTo(targetSide));
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

        private static void SetDurationsForTests(
            Component controller,
            float generationDuration,
            float flipCooldown,
            float flashDuration)
        {
            ControllerType.GetMethod("SetDurationsForTests", BindingFlags.Public | BindingFlags.Instance)
                .Invoke(controller, new object[] { generationDuration, flipCooldown, flashDuration });
        }

        private static void MovePlayerToSignedDistance(
            Transform player,
            Component detector,
            float targetSignedDistance)
        {
            var normal = GetProperty<Vector3>(detector, "PlaneNormal");
            var currentDistance = (float)DetectorType
                .GetMethod("GetSignedDistance", BindingFlags.Public | BindingFlags.Instance)
                .Invoke(detector, new object[] { player.position });
            player.position += normal * (targetSignedDistance - currentDistance);
        }

        private static int CountReflectedBooks(GameObject root)
        {
            return root.transform
                .Cast<Transform>()
                .Count(child => child.name.StartsWith("Book_Family_Current_take_book_001_Reflected", StringComparison.Ordinal));
        }

        private static T GetProperty<T>(object target, string propertyName)
        {
            return (T)target.GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
                .GetValue(target);
        }
    }
}
