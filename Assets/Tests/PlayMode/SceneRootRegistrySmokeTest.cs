using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class SceneRootRegistrySmokeTest
    {
        private const int PastVisualLayer = 11;
        private const string SceneName = "Anemora_Main";

        [UnityTest]
        public IEnumerator MainSceneContainsSceneRootsAndSyncedPastCamera()
        {
            var operation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
            while (!operation.isDone)
            {
                yield return null;
            }

            var rootCurrent = GameObject.Find("Root_Current");
            var rootPast = GameObject.Find("Root_Past");
            var mainCamera = Camera.main;
            var pastCameraObject = GameObject.Find("Camera_Past");

            Assert.That(rootCurrent, Is.Not.Null);
            Assert.That(rootPast, Is.Not.Null);
            Assert.That(mainCamera, Is.Not.Null);
            Assert.That(pastCameraObject, Is.Not.Null);

            var pastCamera = pastCameraObject.GetComponent<Camera>();
            Assert.That(pastCamera, Is.Not.Null);
            Assert.That(pastCamera.cullingMask, Is.EqualTo(1 << PastVisualLayer));

            mainCamera.transform.SetPositionAndRotation(
                new Vector3(1.5f, 2.25f, -4f),
                Quaternion.Euler(12f, 24f, 0f));
            mainCamera.fieldOfView = 43f;

            yield return null;

            Assert.That(Vector3.Distance(pastCamera.transform.position, mainCamera.transform.position), Is.LessThan(0.001f));
            Assert.That(Quaternion.Angle(pastCamera.transform.rotation, mainCamera.transform.rotation), Is.LessThan(0.01f));
            Assert.That(pastCamera.fieldOfView, Is.EqualTo(mainCamera.fieldOfView).Within(0.001f));
        }
    }
}
