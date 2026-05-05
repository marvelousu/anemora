using System.Collections;
using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class StressSampleRunnerSmokeTests
    {
        private static readonly Type StressSampleRunnerType = Type.GetType(
            "Anemora.PerformanceHarness.StressSampleRunner, Assembly-CSharp",
            throwOnError: true);

        [UnityTest]
        public IEnumerator StressSampleRunnerStartsAndStopsWithoutSceneWiring()
        {
            var root = new GameObject("StressSampleRunnerSmoke");
            var runner = root.AddComponent(StressSampleRunnerType);

            try
            {
                Invoke(runner, "StartSample");
                yield return null;

                Assert.That(GetProperty<bool>(runner, "IsRunning"), Is.True);

                Invoke(runner, "StopSample");
                yield return null;

                Assert.That(GetProperty<bool>(runner, "IsRunning"), Is.False);

                Invoke(runner, "RunSingleStepForSmoke");
                var result = GetProperty<object>(runner, "LastResult");
                Assert.That(result, Is.Not.Null);
                Assert.That(GetField<float>(result, "durationSeconds"), Is.GreaterThan(0f));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(root);
                Time.timeScale = 1f;
            }
        }

        private static void Invoke(object target, string methodName)
        {
            target.GetType()
                .GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance)
                .Invoke(target, Array.Empty<object>());
        }

        private static T GetProperty<T>(object target, string propertyName)
        {
            return (T)target.GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
                .GetValue(target);
        }

        private static T GetField<T>(object target, string fieldName)
        {
            return (T)target.GetType()
                .GetField(fieldName, BindingFlags.Public | BindingFlags.Instance)
                .GetValue(target);
        }
    }
}
