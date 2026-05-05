using System;
using System.Collections;
using System.Reflection;
using Anemora.Game.Dialogue;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class NpcDialogueFlowTests
    {
        private const string SceneName = "Anemora_Main";
        private const string TableName = "Anemora_Strings";
        private const string ResidentAGreetKey = "dialogue.encounter.past_resident_a.line_1";
        private const string ResidentAGreetJapanese = "あれ、見えてる？　向こうの大きな建物。";
        private const string ResidentAGreetEnglish = "Can you see it? The big building over there.";
        private const int CurrentVisualLayer = 10;
        private const int PastVisualLayer = 11;

        private static readonly Type NpcInteractableType = ResolveRuntimeType("Anemora.Dialogue.NpcInteractable");
        private static readonly Type DialogueDisplayType = ResolveRuntimeType("Anemora.Dialogue.DialogueDisplay");
        private static readonly Type PrototypePlayerControllerType = Type.GetType(
            "Anemora.Player.PrototypePlayerController, Assembly-CSharp",
            throwOnError: true);

        [UnityTest]
        public IEnumerator SceneContainsResidentNpcInstancesWithFinalDialogueAssets()
        {
            yield return LoadMainScene();

            var rootPast = GameObject.Find("Root_Past");
            var rootCurrent = GameObject.Find("Root_Current");
            var residentA = GameObject.Find("Resident_A_Instance");
            var residentB = GameObject.Find("Resident_B_Instance");

            Assert.That(rootPast, Is.Not.Null);
            Assert.That(rootCurrent, Is.Not.Null);
            AssertPlacedNpc(residentA, rootPast.transform, PastVisualLayer, new Vector3(-0.85f, 0.02f, 1.05f));
            AssertPlacedNpc(residentB, rootCurrent.transform, CurrentVisualLayer, new Vector3(1.25f, 0.02f, 0.85f));

            var residentAAsset = GetDialogueAsset(residentA);
            AssertDialogueAsset(
                residentAAsset,
                "resident_a",
                "greeting",
                "dialogue.speaker.resident_a",
                "dialogue.encounter.past_resident_a.line_1",
                "dialogue.encounter.past_resident_a.line_2",
                "dialogue.encounter.past_resident_a.line_3");

            var residentBAsset = GetDialogueAsset(residentB);
            AssertDialogueAsset(
                residentBAsset,
                "resident_b",
                "idle",
                "dialogue.speaker.resident_b",
                "dialogue.encounter.present_resident_b.line_1",
                "dialogue.encounter.present_resident_b.line_2",
                "dialogue.encounter.present_resident_b.line_3",
                "dialogue.encounter.present_resident_b.line_4");

            var niroMonologue = GameObject.Find("TimeFramePortalSystem")?.GetComponent(
                ResolveRuntimeType("Anemora.TimeManagement.NiroMonologueController"));
            Assert.That(niroMonologue, Is.Not.Null);
        }

        [UnityTest]
        public IEnumerator ResidentAInteractionShowsAdvancesAndClosesDialoguePanel()
        {
            yield return LoadMainScene();

            var player = GameObject.FindWithTag("Player");
            var residentA = GameObject.Find("Resident_A_Instance");
            var display = UnityEngine.Object.FindObjectOfType(DialogueDisplayType);
            var controller = player.GetComponent(PrototypePlayerControllerType);

            Assert.That(player, Is.Not.Null);
            Assert.That(residentA, Is.Not.Null);
            Assert.That(display, Is.Not.Null);
            Assert.That(controller, Is.Not.Null);

            Invoke(display, "Close");
            player.transform.position = residentA.transform.position + new Vector3(0.25f, 0f, 0f);
            yield return null;

            var interactable = residentA.GetComponent(NpcInteractableType);
            Assert.That((bool)Invoke(interactable, "TryInteract"), Is.True);

            Assert.That((bool)GetProperty(display, "IsVisible"), Is.True);
            Assert.That(GetProperty(display, "CurrentSpeaker"), Is.EqualTo("dialogue.speaker.resident_a"));
            Assert.That(GetProperty(display, "CurrentText"), Is.EqualTo("dialogue.encounter.past_resident_a.line_1"));
            Assert.That((bool)GetProperty(controller, "IsMovementFrozen"), Is.True);

            Invoke(display, "AdvanceLine");
            Assert.That((bool)GetProperty(display, "IsVisible"), Is.True);
            Assert.That(GetProperty(display, "CurrentText"), Is.EqualTo("dialogue.encounter.past_resident_a.line_2"));
            Assert.That((bool)GetProperty(controller, "IsMovementFrozen"), Is.True);

            Invoke(display, "AdvanceLine");
            Assert.That((bool)GetProperty(display, "IsVisible"), Is.True);
            Assert.That(GetProperty(display, "CurrentText"), Is.EqualTo("dialogue.encounter.past_resident_a.line_3"));
            Assert.That((bool)GetProperty(controller, "IsMovementFrozen"), Is.True);

            Invoke(display, "AdvanceLine");
            Assert.That((bool)GetProperty(display, "IsVisible"), Is.False);
            Assert.That((bool)GetProperty(controller, "IsMovementFrozen"), Is.False);
        }

        [UnityTest]
        public IEnumerator ResidentADialogueResolvesFinalTextAfterLocaleSwitch()
        {
            yield return LocalizationSettings.InitializationOperation;

            var ja = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("ja-JP"));
            var en = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("en"));
            Assert.That(ja, Is.Not.Null);
            Assert.That(en, Is.Not.Null);

            yield return LoadMainScene();

            var player = GameObject.FindWithTag("Player");
            var residentA = GameObject.Find("Resident_A_Instance");
            var display = UnityEngine.Object.FindObjectOfType(DialogueDisplayType);

            Assert.That(player, Is.Not.Null);
            Assert.That(residentA, Is.Not.Null);
            Assert.That(display, Is.Not.Null);

            LocalizationSettings.SelectedLocale = ja;
            yield return LocalizationSettings.SelectedLocaleAsync;

            var jaOperation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
                TableName,
                ResidentAGreetKey,
                ja,
                FallbackBehavior.UseFallback);
            yield return jaOperation;
            Assert.That(jaOperation.Result, Is.EqualTo(ResidentAGreetJapanese));

            yield return ShowResidentADialogue(player, residentA, display);
            Assert.That(
                GetProperty(display, "CurrentText"),
                Is.EqualTo(ExpectedDialogueDisplayText(ResidentAGreetKey, ResidentAGreetJapanese)));

            Invoke(display, "Close");

            LocalizationSettings.SelectedLocale = en;
            yield return LocalizationSettings.SelectedLocaleAsync;

            var enOperation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
                TableName,
                ResidentAGreetKey,
                en,
                FallbackBehavior.UseFallback);
            yield return enOperation;
            Assert.That(enOperation.Result, Is.EqualTo(ResidentAGreetEnglish));

            yield return ShowResidentADialogue(player, residentA, display);
            Assert.That(
                GetProperty(display, "CurrentText"),
                Is.EqualTo(ExpectedDialogueDisplayText(ResidentAGreetKey, ResidentAGreetEnglish)));

            Invoke(display, "Close");
            LocalizationSettings.SelectedLocale = ja;
            yield return LocalizationSettings.SelectedLocaleAsync;
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

        private static IEnumerator ShowResidentADialogue(GameObject player, GameObject residentA, object display)
        {
            Invoke(display, "Close");
            player.transform.position = residentA.transform.position + new Vector3(0.25f, 0f, 0f);
            yield return null;

            var interactable = residentA.GetComponent(NpcInteractableType);
            Assert.That((bool)Invoke(interactable, "TryInteract"), Is.True);
            Assert.That((bool)GetProperty(display, "IsVisible"), Is.True);
        }

        private static string ExpectedDialogueDisplayText(string key, string localizedText)
        {
            // DialogueLocalization intentionally returns key fallback in batchmode.
            return Application.isBatchMode ? key : localizedText;
        }

        private static void AssertPlacedNpc(
            GameObject npc,
            Transform expectedParent,
            int expectedLayer,
            Vector3 expectedLocalPosition)
        {
            Assert.That(npc, Is.Not.Null);
            Assert.That(npc.layer, Is.EqualTo(expectedLayer));
            Assert.That(npc.transform.parent, Is.EqualTo(expectedParent));
            Assert.That(Vector3.Distance(npc.transform.localPosition, expectedLocalPosition), Is.LessThan(0.001f));
            Assert.That(npc.GetComponent<SpriteRenderer>(), Is.Not.Null);
            Assert.That(npc.GetComponent<Animator>(), Is.Not.Null);
            Assert.That(npc.GetComponent(NpcInteractableType), Is.Not.Null);
        }

        private static DialogueAsset GetDialogueAsset(GameObject npc)
        {
            var interactable = npc.GetComponent(NpcInteractableType);
            Assert.That(interactable, Is.Not.Null);
            return (DialogueAsset)GetField(interactable, "dialogueAsset");
        }

        private static void AssertDialogueAsset(
            DialogueAsset asset,
            string npcId,
            string variantId,
            string speakerKey,
            params string[] lineKeys)
        {
            Assert.That(asset, Is.Not.Null);
            Assert.That(asset.npcId, Is.EqualTo(npcId));
            Assert.That(asset.variants, Has.Count.EqualTo(1));
            Assert.That(asset.variants[0].variantId, Is.EqualTo(variantId));
            Assert.That(asset.variants[0].turns, Has.Count.EqualTo(lineKeys.Length));

            for (var index = 0; index < lineKeys.Length; index++)
            {
                Assert.That(asset.variants[0].turns[index].speakerId, Is.EqualTo(speakerKey));
                Assert.That(
                    asset.variants[0].turns[index].GetLocalizedTextOrFallback(lineKeys[index]),
                    Is.EqualTo(lineKeys[index]));
            }
        }

        private static Type ResolveRuntimeType(string fullName)
        {
            return Type.GetType($"{fullName}, Anemora.Dialogue", throwOnError: false) ??
                   Type.GetType($"{fullName}, Assembly-CSharp", throwOnError: true);
        }

        private static object GetField(object target, string fieldName)
        {
            return target.GetType()
                .GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(target);
        }

        private static object GetProperty(object target, string propertyName)
        {
            return target.GetType()
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
