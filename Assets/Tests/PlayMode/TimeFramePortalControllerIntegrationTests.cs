using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class TimeFramePortalControllerIntegrationTests
    {
        private const int CurrentCameraMask = (1 << 10) | (1 << 5);
        private const int PastCameraMask = (1 << 11) | (1 << 5);
        private const int CurrentPlayerLayer = 8;
        private const int PastPlayerLayer = 9;
        private const int CurrentVisualMask = 1 << 10;
        private const int PastVisualMask = 1 << 11;

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

        private static readonly Type VolumeType = Type.GetType(
            "UnityEngine.Rendering.Volume, Unity.RenderPipelines.Core.Runtime",
            throwOnError: true);

        private static readonly Type StencilFeatureType = Type.GetType(
            "Anemora.TimeManagement.Portal.PortalStencilFeature, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type SymbolType = Type.GetType(
            "Anemora.UI.SymbolType, Assembly-CSharp",
            throwOnError: true);

        [UnityTest]
        public IEnumerator SymbolWheelBurstCreatesOnlyOnePortalAndRestoresTimeScale()
        {
            var harness = CreateHarness();
            var states = new List<string>();
            var timeScales = new List<float>();
            var stateEvent = ControllerType.GetEvent("StateChanged");
            var stateRecorder = CreateStateRecorder(stateEvent.EventHandlerType, states, timeScales);
            stateEvent.AddEventHandler(harness.Controller, stateRecorder);

            try
            {
                for (var i = 0; i < 5; i++)
                {
                    SelectRed(harness.Controller);
                }

                yield return null;
                yield return null;

                Assert.That(GetProperty<int>(harness.Controller, "PortalGenerationCount"), Is.EqualTo(1));
                Assert.That(GetProperty<object>(harness.Controller, "State").ToString(), Is.EqualTo("Open"));
                Assert.That(Time.timeScale, Is.EqualTo(1f).Within(0.0001f));

                SelectRed(harness.Controller);
                yield return null;

                Assert.That(GetProperty<int>(harness.Controller, "PortalGenerationCount"), Is.EqualTo(1));
                Assert.That(states, Does.Contain("Selecting"));
                Assert.That(states, Does.Contain("Generating"));
                Assert.That(states, Does.Contain("Open"));
                Assert.That(timeScales[states.IndexOf("Selecting")], Is.EqualTo(0f).Within(0.0001f));
                Assert.That(timeScales[states.IndexOf("Generating")], Is.EqualTo(0f).Within(0.0001f));
                Assert.That(timeScales[states.IndexOf("Open")], Is.EqualTo(1f).Within(0.0001f));
            }
            finally
            {
                stateEvent.RemoveEventHandler(harness.Controller, stateRecorder);
                harness.Destroy();
                Time.timeScale = 1f;
            }
        }

        [UnityTest]
        public IEnumerator AtomicFlipAppliesCameraPlayerAndStencilBeforeSideEvent()
        {
            var harness = CreateHarness();
            var observations = new List<string>();
            var sideEvent = PolarityType.GetEvent("SideChanged");
            var atomicRecorder = CreateAtomicRecorder(
                sideEvent.EventHandlerType,
                observations,
                harness.Camera,
                harness.Player,
                harness.StencilFeature);
            sideEvent.AddEventHandler(harness.Polarity, atomicRecorder);

            try
            {
                SelectRed(harness.Controller);
                yield return null;
                yield return null;

                ControllerType.GetMethod("TriggerCrossingForTests").Invoke(harness.Controller, Array.Empty<object>());
                Assert.That(observations, Has.Count.EqualTo(1));
                Assert.That(
                    observations[0],
                    Is.EqualTo($"Past|{PastCameraMask}|{PastPlayerLayer}|{PastVisualMask}|{CurrentVisualMask}"));

                yield return null;
                Assert.That(GetProperty<object>(harness.Polarity, "CurrentSide").ToString(), Is.EqualTo("Past"));
                Assert.That(GetProperty<object>(harness.Controller, "State").ToString(), Is.EqualTo("Open"));

                ControllerType.GetMethod("TriggerCrossingForTests").Invoke(harness.Controller, Array.Empty<object>());
                Assert.That(observations, Has.Count.EqualTo(2));
                Assert.That(
                    observations[1],
                    Is.EqualTo($"Current|{CurrentCameraMask}|{CurrentPlayerLayer}|{CurrentVisualMask}|{PastVisualMask}"));

                yield return null;
                Assert.That(GetProperty<object>(harness.Polarity, "CurrentSide").ToString(), Is.EqualTo("Current"));
                Assert.That(Time.timeScale, Is.EqualTo(1f).Within(0.0001f));
            }
            finally
            {
                sideEvent.RemoveEventHandler(harness.Polarity, atomicRecorder);
                harness.Destroy();
                Time.timeScale = 1f;
            }
        }

        [UnityTest]
        public IEnumerator CrossingRunsThroughCrossingAndFlippingStates()
        {
            var harness = CreateHarness();
            var states = new List<string>();
            var timeScales = new List<float>();
            var stateEvent = ControllerType.GetEvent("StateChanged");
            var stateRecorder = CreateStateRecorder(stateEvent.EventHandlerType, states, timeScales);
            stateEvent.AddEventHandler(harness.Controller, stateRecorder);

            try
            {
                SelectRed(harness.Controller);
                yield return null;
                yield return null;

                ControllerType.GetMethod("TriggerCrossingForTests").Invoke(harness.Controller, Array.Empty<object>());
                yield return null;

                Assert.That(states, Does.Contain("Crossing"));
                Assert.That(states, Does.Contain("Flipping"));
                Assert.That(GetProperty<object>(harness.Controller, "State").ToString(), Is.EqualTo("Open"));
                Assert.That(timeScales[states.IndexOf("Crossing")], Is.EqualTo(1f).Within(0.0001f));
                Assert.That(timeScales[states.IndexOf("Flipping")], Is.EqualTo(1f).Within(0.0001f));
            }
            finally
            {
                stateEvent.RemoveEventHandler(harness.Controller, stateRecorder);
                harness.Destroy();
                Time.timeScale = 1f;
            }
        }

        private static Harness CreateHarness()
        {
            Time.timeScale = 1f;

            var cameraObject = new GameObject("Main Camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.cullingMask = CurrentCameraMask;

            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.layer = CurrentPlayerLayer;
            player.transform.position = new Vector3(0f, 0f, 0.1f);

            var portalPrefab = GameObject.CreatePrimitive(PrimitiveType.Quad);
            portalPrefab.name = "Portal_Frame_TestPrefab";
            portalPrefab.transform.rotation = Quaternion.identity;
            var portalCollider = portalPrefab.GetComponent<Collider>();
            if (portalCollider != null)
            {
                UnityEngine.Object.DestroyImmediate(portalCollider);
            }

            var spawnPoint = new GameObject("PortalSpawnPoint");
            spawnPoint.transform.position = Vector3.zero;
            spawnPoint.transform.rotation = Quaternion.identity;

            var root = new GameObject("TimeFramePortalControllerHarness");
            var detector = root.AddComponent(DetectorType);
            var polarity = root.AddComponent(PolarityType);
            var switcher = root.AddComponent(SwitcherType);
            root.AddComponent(VolumeType);
            var flash = root.AddComponent(FlashType);
            var controller = root.AddComponent(ControllerType);

            var stencilFeature = ScriptableObject.CreateInstance(StencilFeatureType);
            var featureArray = Array.CreateInstance(StencilFeatureType, 1);
            featureArray.SetValue(stencilFeature, 0);

            SetField(switcher, "mainCamera", camera);
            SetField(switcher, "stencilFeatures", featureArray);
            SetField(controller, "player", player.transform);
            SetField(controller, "portalPrefab", portalPrefab);
            SetField(controller, "portalSpawnPoint", spawnPoint.transform);
            SetField(controller, "crossingDetector", detector);
            SetField(controller, "sidePolarity", polarity);
            SetField(controller, "visualSwitcher", switcher);
            SetField(controller, "flashPlayer", flash);
            ControllerType.GetMethod("SetDurationsForTests")
                .Invoke(controller, new object[] { 0f, 0f, 0.05f });

            return new Harness(
                root,
                cameraObject,
                player,
                portalPrefab,
                spawnPoint,
                controller,
                polarity,
                camera,
                stencilFeature);
        }

        private static void SelectRed(object controller)
        {
            var red = Enum.Parse(SymbolType, "Red");
            ControllerType.GetMethod("HandleSymbolSelected").Invoke(controller, new[] { red });
        }

        private static T GetProperty<T>(object target, string propertyName)
        {
            return (T)target.GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
                .GetValue(target);
        }

        private static void SetField(object target, string fieldName, object value)
        {
            target.GetType()
                .GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(target, value);
        }

        private static Delegate CreateStateRecorder(
            Type handlerType,
            List<string> states,
            List<float> timeScales)
        {
            var parameters = handlerType.GetMethod("Invoke").GetParameters();
            var stateParameter = Expression.Parameter(parameters[0].ParameterType, "state");
            var scaleParameter = Expression.Parameter(parameters[1].ParameterType, "timeScale");
            var record = typeof(TimeFramePortalControllerIntegrationTests)
                .GetMethod(nameof(RecordState), BindingFlags.NonPublic | BindingFlags.Static);

            var body = Expression.Call(
                record,
                Expression.Constant(states),
                Expression.Constant(timeScales),
                Expression.Call(stateParameter, parameters[0].ParameterType.GetMethod("ToString", Type.EmptyTypes)),
                Expression.Convert(scaleParameter, typeof(float)));

            return Expression.Lambda(handlerType, body, stateParameter, scaleParameter).Compile();
        }

        private static Delegate CreateAtomicRecorder(
            Type handlerType,
            List<string> observations,
            Camera camera,
            GameObject player,
            object stencilFeature)
        {
            var parameterType = handlerType.GetMethod("Invoke").GetParameters()[0].ParameterType;
            var sideParameter = Expression.Parameter(parameterType, "side");
            var record = typeof(TimeFramePortalControllerIntegrationTests)
                .GetMethod(nameof(RecordAtomicObservation), BindingFlags.NonPublic | BindingFlags.Static);

            var body = Expression.Call(
                record,
                Expression.Constant(observations),
                Expression.Constant(camera),
                Expression.Constant(player),
                Expression.Constant(stencilFeature),
                Expression.Call(sideParameter, parameterType.GetMethod("ToString", Type.EmptyTypes)));

            return Expression.Lambda(handlerType, body, sideParameter).Compile();
        }

        private static void RecordState(
            List<string> states,
            List<float> timeScales,
            string state,
            float timeScale)
        {
            states.Add(state);
            timeScales.Add(timeScale);
        }

        private static void RecordAtomicObservation(
            List<string> observations,
            Camera camera,
            GameObject player,
            object stencilFeature,
            string side)
        {
            var featureType = stencilFeature.GetType();
            var portalMask = (LayerMask)featureType.GetProperty("PortalMaskLayers").GetValue(stencilFeature);
            var insideMask = (LayerMask)featureType.GetProperty("InsidePortalLayers").GetValue(stencilFeature);
            observations.Add($"{side}|{camera.cullingMask}|{player.layer}|{portalMask.value}|{insideMask.value}");
        }

        private sealed class Harness
        {
            private readonly GameObject root;
            private readonly GameObject cameraObject;
            private readonly GameObject portalPrefab;
            private readonly GameObject spawnPoint;

            public Harness(
                GameObject root,
                GameObject cameraObject,
                GameObject player,
                GameObject portalPrefab,
                GameObject spawnPoint,
                object controller,
                object polarity,
                Camera camera,
                object stencilFeature)
            {
                this.root = root;
                this.cameraObject = cameraObject;
                this.portalPrefab = portalPrefab;
                this.spawnPoint = spawnPoint;
                Player = player;
                Controller = controller;
                Polarity = polarity;
                Camera = camera;
                StencilFeature = stencilFeature;
            }

            public GameObject Player { get; }
            public object Controller { get; }
            public object Polarity { get; }
            public Camera Camera { get; }
            public object StencilFeature { get; }

            public void Destroy()
            {
                var portalInstance = GetProperty<GameObject>(Controller, "PortalInstance");
                DestroyObject(portalInstance);
                DestroyObject(root);
                DestroyObject(cameraObject);
                DestroyObject(Player);
                DestroyObject(portalPrefab);
                DestroyObject(spawnPoint);
                DestroyObject((UnityEngine.Object)StencilFeature);
            }

            private static void DestroyObject(UnityEngine.Object value)
            {
                if (value != null)
                {
                    UnityEngine.Object.DestroyImmediate(value);
                }
            }
        }
    }
}
