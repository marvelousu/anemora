using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Anemora.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class SaveLoadLocaleIntegrationTests
    {
        private const string SceneName = "Anemora_Main";
        private const string TableName = "Anemora_Strings";
        private const string ResidentAGreetKey = "dialogue.encounter.past_resident_a.line_1";
        private const string ResidentAGreetEnglish = "Can you see it? The big building over there.";

        private static readonly Type NpcInteractableType = ResolveRuntimeType("Anemora.Dialogue.NpcInteractable");
        private static readonly Type DialogueDisplayType = ResolveRuntimeType("Anemora.Dialogue.DialogueDisplay");
        private static readonly Type ActionRecordRuntimeType = Type.GetType(
            "Anemora.TimeManagement.ActionRecordRuntime, Assembly-CSharp",
            throwOnError: true);

        [UnityTest]
        public IEnumerator ResidentADialogueUsesCurrentLocaleAfterSaveEnvelopeRoundTripAndSceneReload()
        {
            Time.timeScale = 1f;
            yield return LocalizationSettings.InitializationOperation;

            var ja = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("ja-JP"));
            var en = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("en"));
            Assert.That(ja, Is.Not.Null);
            Assert.That(en, Is.Not.Null);

            yield return LoadMainScene();

            var player = GameObject.FindWithTag("Player");
            var residentA = GameObject.Find("Resident_A_Instance");
            var display = FindSceneComponent(DialogueDisplayType);
            var runtime = FindSceneComponent(ActionRecordRuntimeType);

            Assert.That(player, Is.Not.Null);
            Assert.That(residentA, Is.Not.Null);
            Assert.That(display, Is.Not.Null);
            Assert.That(runtime, Is.Not.Null);

            LocalizationSettings.SelectedLocale = ja;
            yield return LocalizationSettings.SelectedLocaleAsync;

            var jaOperation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
                TableName,
                ResidentAGreetKey,
                ja,
                FallbackBehavior.UseFallback);
            yield return jaOperation;
            var jaText = jaOperation.Result;
            Assert.That(jaText, Is.Not.Empty);
            Assert.That(jaText, Is.Not.EqualTo(ResidentAGreetKey));

            yield return ShowResidentADialogue(player, residentA, display);
            Assert.That(
                GetProperty<string>(display, "CurrentText"),
                Is.EqualTo(ExpectedDialogueDisplayText(ResidentAGreetKey, jaText)));
            Invoke(display, "Close");

            LocalizationSettings.SelectedLocale = en;
            yield return LocalizationSettings.SelectedLocaleAsync;

            var enOperation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
                TableName,
                ResidentAGreetKey,
                en,
                FallbackBehavior.UseFallback);
            yield return enOperation;
            var enText = enOperation.Result;
            Assert.That(enText, Is.EqualTo(ResidentAGreetEnglish));

            yield return ShowResidentADialogue(player, residentA, display);
            Assert.That(
                GetProperty<string>(display, "CurrentText"),
                Is.EqualTo(ExpectedDialogueDisplayText(ResidentAGreetKey, enText)));

            var savedEnvelope = BuildSaveEnvelope(runtime, player.transform);
            Assert.That(
                typeof(SaveEnvelope).GetField(
                    "locale",
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase),
                Is.Null,
                "Locale is intentionally not persisted in SaveEnvelope; player settings own the selected locale.");

            var json = JsonConvert.SerializeObject(savedEnvelope);
            var restoredEnvelope = JsonConvert.DeserializeObject<SaveEnvelope>(json);
            Assert.That(restoredEnvelope, Is.Not.Null);
            Assert.That(restoredEnvelope.actionRecords, Is.Not.Null);
            Assert.That(LocalizationSettings.SelectedLocale.Identifier.Code, Is.EqualTo("en"));
            Invoke(display, "Close");

            yield return LoadMainScene();
            Assert.That(LocalizationSettings.SelectedLocale.Identifier.Code, Is.EqualTo("en"));

            var reloadedRuntime = FindSceneComponent(ActionRecordRuntimeType);
            var reloadedPlayer = GameObject.FindWithTag("Player");
            var reloadedResidentA = GameObject.Find("Resident_A_Instance");
            var reloadedDisplay = FindSceneComponent(DialogueDisplayType);

            Assert.That(reloadedRuntime, Is.Not.Null);
            Assert.That(reloadedPlayer, Is.Not.Null);
            Assert.That(reloadedResidentA, Is.Not.Null);
            Assert.That(reloadedDisplay, Is.Not.Null);

            ActionRecordRuntimeType.GetMethod("LoadFromSaveData", BindingFlags.Public | BindingFlags.Instance)
                .Invoke(reloadedRuntime, new object[] { restoredEnvelope.actionRecords });
            yield return null;

            Assert.That(
                LocalizationSettings.SelectedLocale.Identifier.Code,
                Is.EqualTo("en"),
                "Save restore must preserve the already selected runtime locale.");

            yield return ShowResidentADialogue(reloadedPlayer, reloadedResidentA, reloadedDisplay);
            Assert.That(
                GetProperty<string>(reloadedDisplay, "CurrentText"),
                Is.EqualTo(ExpectedDialogueDisplayText(ResidentAGreetKey, enText)));
            Invoke(reloadedDisplay, "Close");

            LocalizationSettings.SelectedLocale = ja;
            yield return LocalizationSettings.SelectedLocaleAsync;
        }

        private static SaveEnvelope BuildSaveEnvelope(Component runtime, Transform player)
        {
            return new SaveEnvelope
            {
                saveVersion = 1,
                buildVersion = Application.version,
                slotId = "playmode_locale_roundtrip",
                sceneId = SceneName,
                savedAtUnixSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                metadata = new SaveMetadata
                {
                    saveVersion = 1,
                    slotId = "playmode_locale_roundtrip",
                    displayName = "PlayMode Locale Roundtrip",
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
                    sceneSide = "Past"
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

        private static IEnumerator ShowResidentADialogue(GameObject player, GameObject residentA, Component display)
        {
            Invoke(display, "Close");
            player.transform.position = residentA.transform.position + new Vector3(0.25f, 0f, 0f);
            yield return null;

            var interactable = residentA.GetComponent(NpcInteractableType);
            Assert.That(interactable, Is.Not.Null);
            Assert.That((bool)Invoke(interactable, "TryInteract"), Is.True);
            Assert.That((bool)GetProperty<bool>(display, "IsVisible"), Is.True);
        }

        private static Component FindSceneComponent(Type type)
        {
            var activeScene = SceneManager.GetActiveScene();
            return Resources.FindObjectsOfTypeAll(type)
                .OfType<Component>()
                .FirstOrDefault(component => component.gameObject.scene == activeScene);
        }

        private static string ExpectedDialogueDisplayText(string key, string localizedText)
        {
            return Application.isBatchMode ? key : localizedText;
        }

        private static Type ResolveRuntimeType(string fullName)
        {
            return Type.GetType($"{fullName}, Anemora.Dialogue", throwOnError: false) ??
                   Type.GetType($"{fullName}, Assembly-CSharp", throwOnError: true);
        }

        private static T GetProperty<T>(object target, string propertyName)
        {
            return (T)target.GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
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
