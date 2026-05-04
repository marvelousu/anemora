using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class HeroAnimatorBinderTests
    {
        private const string SceneName = "Anemora_Main";
        private const int CurrentVisualLayer = 10;
        private const int PastVisualLayer = 11;

        private static readonly Type PrototypePlayerControllerType = Type.GetType(
            "Anemora.Player.PrototypePlayerController, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type HeroAnimatorBinderType = Type.GetType(
            "Anemora.Player.HeroAnimatorBinder, Assembly-CSharp",
            throwOnError: true);

        [UnityTest]
        public IEnumerator BinderUpdatesMovingAndFacingFromObservedMovement()
        {
            var operation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
            while (!operation.isDone)
            {
                yield return null;
            }

            var source = GameObject.Find("Player_Visual_Current");
            Assert.That(source, Is.Not.Null);

            var player = new GameObject("HeroAnimatorBinderTestPlayer");
            var controller = player.AddComponent(PrototypePlayerControllerType);
            var instance = UnityEngine.Object.Instantiate(source);
            try
            {
                var binder = instance.GetComponent(HeroAnimatorBinderType);
                var animator = instance.GetComponent<Animator>();
                var renderer = instance.GetComponent<SpriteRenderer>();
                Assert.That(binder, Is.Not.Null);
                Assert.That(animator, Is.Not.Null);
                Assert.That(renderer, Is.Not.Null);

                SetField(binder, "playerController", controller);
                SetField(binder, "observedTransform", player.transform);
                Invoke(binder, "ResetTrackingForTests");

                player.transform.position += Vector3.right * 0.25f;
                Invoke(binder, "TickForTests", 0.1f);
                Assert.That(animator.GetBool("isMoving"), Is.True);
                Assert.That(animator.GetInteger("facing"), Is.EqualTo(1));
                Assert.That(renderer.flipX, Is.False);

                player.transform.position += Vector3.left * 0.35f;
                Invoke(binder, "TickForTests", 0.1f);
                Assert.That(animator.GetInteger("facing"), Is.EqualTo(1));
                Assert.That(renderer.flipX, Is.True);

                player.transform.position += Vector3.forward * 0.35f;
                Invoke(binder, "TickForTests", 0.1f);
                Assert.That(animator.GetInteger("facing"), Is.EqualTo(2));
                Assert.That(renderer.flipX, Is.False);

                Invoke(binder, "TickForTests", 0.1f);
                Assert.That(animator.GetBool("isMoving"), Is.False);
                yield return null;
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(instance);
                UnityEngine.Object.DestroyImmediate(player);
            }
        }

        [UnityTest]
        public IEnumerator MainSceneUsesHeroPrefabInstancesForCurrentAndPastVisuals()
        {
            var operation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
            while (!operation.isDone)
            {
                yield return null;
            }

            var player = GameObject.FindWithTag("Player");
            var current = GameObject.Find("Player_Visual_Current");
            var past = GameObject.Find("Player_Visual_Past");

            Assert.That(player, Is.Not.Null);
            AssertHeroVisual(current, CurrentVisualLayer);
            AssertHeroVisual(past, PastVisualLayer);
            Assert.That(current.transform.parent, Is.EqualTo(player.transform));
            Assert.That(past.transform.parent, Is.EqualTo(player.transform));
        }

        private static void AssertHeroVisual(GameObject visual, int expectedLayer)
        {
            Assert.That(visual, Is.Not.Null);
            Assert.That(visual.layer, Is.EqualTo(expectedLayer));
            Assert.That(visual.GetComponent<SpriteRenderer>(), Is.Not.Null);
            Assert.That(visual.GetComponent<Animator>(), Is.Not.Null);
            Assert.That(visual.GetComponent(HeroAnimatorBinderType), Is.Not.Null);
        }

        private static void SetField(object target, string fieldName, object value)
        {
            target.GetType()
                .GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(target, value);
        }

        private static void Invoke(object target, string methodName, params object[] parameters)
        {
            target.GetType()
                .GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(target, parameters);
        }
    }
}
