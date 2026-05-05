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
    public sealed class Zone1AudioWiringTests
    {
        private const string SceneName = "Anemora_Main";

        private static readonly Type AudioControllerType = Type.GetType(
            "Anemora.Audio.Zone1AudioController, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type NpcInteractableType =
            Type.GetType("Anemora.Dialogue.NpcInteractable, Anemora.Dialogue", throwOnError: true);

        private static readonly Type DialogueDisplayType =
            Type.GetType("Anemora.Dialogue.DialogueDisplay, Anemora.Dialogue", throwOnError: true);

        [UnityTest]
        public IEnumerator MainSceneHasZone1AudioControllerWithCoreClips()
        {
            yield return LoadMainScene();

            var controller = FindSceneComponent(AudioControllerType);
            Assert.That(controller, Is.Not.Null);

            AssertClip(controller, "zone1AmbientClip");
            AssertClip(controller, "windAmbienceClip");
            AssertClip(controller, "silencePadClip");
            AssertClip(controller, "portalOpenClip");
            AssertClip(controller, "portalFlipClip");
            AssertClipArray(controller, "stoneFootstepWalkClips");
            AssertClipArray(controller, "grassFootstepWalkClips");
            AssertClipArray(controller, "environmentOneShotClips", 4);
        }

        [UnityTest]
        public IEnumerator MainSceneHasNpcDialogueAudioClips()
        {
            yield return LoadMainScene();

            var residentA = GameObject.Find("Resident_A_Instance");
            var npc = residentA != null ? residentA.GetComponent(NpcInteractableType) : null;
            var display = FindSceneComponent(DialogueDisplayType);

            Assert.That(npc, Is.Not.Null);
            Assert.That(display, Is.Not.Null);
            AssertClip(npc, "interactionClip");
            AssertClip(display, "advanceClip");
            AssertClip(display, "closeClip");
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

        private static Component FindSceneComponent(Type type)
        {
            var activeScene = SceneManager.GetActiveScene();
            return Resources.FindObjectsOfTypeAll(type)
                .OfType<Component>()
                .FirstOrDefault(component => component.gameObject.scene == activeScene);
        }

        private static void AssertClip(object target, string fieldName)
        {
            Assert.That(GetField<AudioClip>(target, fieldName), Is.Not.Null, fieldName);
        }

        private static void AssertClipArray(object target, string fieldName, int minCount = 1)
        {
            var clips = GetField<AudioClip[]>(target, fieldName);
            Assert.That(clips, Is.Not.Null, fieldName);
            Assert.That(clips.Length, Is.GreaterThanOrEqualTo(minCount), fieldName);
            Assert.That(clips.All(clip => clip != null), Is.True, fieldName);
        }

        private static T GetField<T>(object target, string fieldName)
        {
            return (T)target.GetType()
                .GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(target);
        }
    }
}
