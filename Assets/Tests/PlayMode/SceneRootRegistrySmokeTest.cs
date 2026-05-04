using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class SceneRootRegistrySmokeTest
    {
        private const int PastVisualLayer = 11;
        private const int CurrentVisualLayer = 10;
        private const int UiLayer = 5;
        private const string SceneName = "Anemora_Main";

        [UnityTest]
        public IEnumerator MainSceneContainsSceneRootsAndDisabledPastCamera()
        {
            var operation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
            while (!operation.isDone)
            {
                yield return null;
            }

            var rootCurrent = GameObject.Find("Root_Current");
            var rootPast = GameObject.Find("Root_Past");
            var mainCamera = Camera.main;
            var pastCamera = Resources.FindObjectsOfTypeAll<Camera>()
                .FirstOrDefault(camera =>
                    camera.name == "Camera_Past" &&
                    camera.gameObject.scene == SceneManager.GetActiveScene());

            Assert.That(rootCurrent, Is.Not.Null);
            Assert.That(rootPast, Is.Not.Null);
            Assert.That(mainCamera, Is.Not.Null);
            Assert.That(pastCamera, Is.Not.Null);

            Assert.That(mainCamera.cullingMask, Is.EqualTo((1 << CurrentVisualLayer) | (1 << UiLayer)));
            Assert.That(pastCamera.gameObject.activeSelf, Is.False);
            Assert.That(pastCamera.enabled, Is.False);
            Assert.That(pastCamera.cullingMask, Is.EqualTo(1 << PastVisualLayer));

            yield return null;
        }
    }
}
