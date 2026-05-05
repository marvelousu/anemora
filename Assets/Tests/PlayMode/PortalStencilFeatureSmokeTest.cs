using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class PortalStencilFeatureSmokeTest
    {
        private const string SceneName = "Sandbox_E1_Stencil";

        [UnityTest]
        public IEnumerator SandboxSceneEnqueuesPortalStencilPasses()
        {
            var renderGraphWarningCount = 0;
            void CountRenderGraphWarning(string condition, string stackTrace, LogType type)
            {
                if (condition.Contains("DrawObjectsPass does not have an implementation of the RecordRenderGraph method"))
                {
                    renderGraphWarningCount++;
                }
            }

            Application.logMessageReceived += CountRenderGraphWarning;

            var featureType = Type.GetType(
                "Anemora.TimeManagement.Portal.PortalStencilFeature, Assembly-CSharp",
                throwOnError: true);

            try
            {
                featureType.GetMethod("ResetDiagnosticsForTests", BindingFlags.Public | BindingFlags.Static)
                    ?.Invoke(null, Array.Empty<object>());

                var operation = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
                while (!operation.isDone)
                {
                    yield return null;
                }

                var startFrame = Time.frameCount;
                var camera = Camera.main;
                Assert.That(camera, Is.Not.Null);

                camera.enabled = true;

                RenderShot(camera, new Vector3(0f, 1.25f, -4.2f), new Vector3(0f, 1.05f, 0.75f));
                yield return null;

                RenderShot(camera, new Vector3(2.7f, 1.25f, -0.2f), new Vector3(0f, 1f, 0.3f));
                yield return null;

                RenderShot(camera, new Vector3(0f, 1.25f, 3.4f), new Vector3(0f, 1f, 0f));
                yield return null;

                var lastFrame = (int)featureType.GetProperty("LastEnqueueFrame", BindingFlags.Public | BindingFlags.Static)
                    .GetValue(null);
                var passCount = (int)featureType.GetProperty("LastEnqueuedPassCount", BindingFlags.Public | BindingFlags.Static)
                    .GetValue(null);
                var cameraName = (string)featureType.GetProperty("LastCameraName", BindingFlags.Public | BindingFlags.Static)
                    .GetValue(null);

                Assert.That(SceneManager.GetActiveScene().name, Is.EqualTo(SceneName));
                Assert.That(lastFrame, Is.GreaterThanOrEqualTo(startFrame));
                Assert.That(passCount, Is.EqualTo(2));
                Assert.That(cameraName, Is.Not.Empty);
                Assert.That(renderGraphWarningCount, Is.Zero);
            }
            finally
            {
                Application.logMessageReceived -= CountRenderGraphWarning;
            }
        }

        private static void RenderShot(Camera camera, Vector3 position, Vector3 lookAt)
        {
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));

            const int width = 1280;
            const int height = 720;
            var descriptor = new RenderTextureDescriptor(width, height)
            {
                graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm,
                depthStencilFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.D24_UNorm_S8_UInt,
                msaaSamples = 1
            };
            var renderTexture = new RenderTexture(descriptor);
            var previousTarget = camera.targetTexture;

            try
            {
                camera.targetTexture = renderTexture;
                var request = new RenderPipeline.StandardRequest
                {
                    destination = renderTexture,
                    mipLevel = 0,
                    slice = 0,
                    face = CubemapFace.Unknown
                };

                RenderPipeline.SubmitRenderRequest(camera, request);
            }
            finally
            {
                camera.targetTexture = previousTarget;
                UnityEngine.Object.Destroy(renderTexture);
            }
        }
    }
}
