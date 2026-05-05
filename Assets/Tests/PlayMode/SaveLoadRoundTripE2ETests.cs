using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Anemora.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class SaveLoadRoundTripE2ETests
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
        public IEnumerator BookReflectionSurvivesSaveEnvelopeJsonRoundTripAndSceneReload()
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

            yield return CrossTo(player.transform, detector, polarity, "Current");

            Assert.That(GetProperty<int>(reflector, "ReflectionCount"), Is.EqualTo(1));
            Assert.That(CountReflectedBooks(reflectionsRoot), Is.EqualTo(1));

            var savedEnvelope = BuildSaveEnvelope(runtime, player.transform, "Current");
            var json = JsonConvert.SerializeObject(savedEnvelope);
            var restoredEnvelope = JsonConvert.DeserializeObject<SaveEnvelope>(json);

            Assert.That(restoredEnvelope, Is.Not.Null);
            Assert.That(restoredEnvelope.actionRecords, Is.Not.Null);
            Assert.That(restoredEnvelope.actionRecords.entries, Has.Count.EqualTo(1));
            Assert.That(restoredEnvelope.actionRecords.entries[0].actionId, Is.EqualTo("take_book_001"));
            Assert.That(restoredEnvelope.actionRecords.entries[0].reflected, Is.True);

            yield return LoadMainScene();

            var reloadedRuntime = FindSceneComponent(ActionRecordRuntimeType);
            var reloadedReflector = FindSceneComponent(BookReflectorType);
            var reloadedReflectionsRoot = GameObject.Find(ReflectionsRootName);

            Assert.That(reloadedRuntime, Is.Not.Null);
            Assert.That(reloadedReflector, Is.Not.Null);
            Assert.That(reloadedReflectionsRoot, Is.Not.Null);
            Assert.That(CountReflectedBooks(reloadedReflectionsRoot), Is.EqualTo(0));

            ActionRecordRuntimeType.GetMethod("LoadFromSaveData", BindingFlags.Public | BindingFlags.Instance)
                .Invoke(reloadedRuntime, new object[] { restoredEnvelope.actionRecords });
            yield return null;

            var reloadedEntries = GetProperty<IReadOnlyList<ActionRecordEntry>>(reloadedRuntime, "Entries");
            Assert.That(reloadedEntries, Has.Count.EqualTo(1));
            Assert.That(reloadedEntries[0].actionId, Is.EqualTo("take_book_001"));
            Assert.That(reloadedEntries[0].reflected, Is.True);
            Assert.That(CountReflectedBooks(reloadedReflectionsRoot), Is.EqualTo(1));
            Assert.That(GetProperty<int>(reloadedReflector, "ReflectionCount"), Is.EqualTo(1));

            var reflectedAgain = (int)ActionRecordRuntimeType
                .GetMethod("ReflectUnreflected", BindingFlags.Public | BindingFlags.Instance)
                .Invoke(reloadedRuntime, Array.Empty<object>());
            Assert.That(reflectedAgain, Is.EqualTo(0));
            Assert.That(CountReflectedBooks(reloadedReflectionsRoot), Is.EqualTo(1));
            Assert.That(Time.timeScale, Is.EqualTo(1f).Within(0.0001f));
        }

        private static SaveEnvelope BuildSaveEnvelope(Component runtime, Transform player, string sceneSide)
        {
            return new SaveEnvelope
            {
                saveVersion = 1,
                buildVersion = Application.version,
                slotId = "playmode_roundtrip",
                sceneId = SceneName,
                savedAtUnixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                metadata = new SaveMetadata
                {
                    saveVersion = 1,
                    slotId = "playmode_roundtrip",
                    displayName = "PlayMode Roundtrip",
                    sceneId = SceneName,
                    zoneId = "zone1",
                    layerIndex = 1,
                    playTimeSeconds = 0,
                    savedAtUnixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    hasThumbnail = false
                },
                player = new PlayerSaveData
                {
                    positionX = player.position.x,
                    positionY = player.position.y,
                    positionZ = player.position.z,
                    facingYawDegrees = player.eulerAngles.y,
                    sceneSide = sceneSide
                },
                actionRecords = (ActionRecordStoreSaveData)ActionRecordRuntimeType
                    .GetMethod("ToSaveData", BindingFlags.Public | BindingFlags.Instance)
                    .Invoke(runtime, Array.Empty<object>()),
                progressFlags = new ProgressFlagSaveData
                {
                    currentLayerIndex = 1
                },
                timeFrame = new TimeFrameSaveData
                {
                    state = TimeFrameState.Normal,
                    activePortalSide = null
                }
            };
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
