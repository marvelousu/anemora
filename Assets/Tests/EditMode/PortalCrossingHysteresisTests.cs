using System;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace Anemora.Tests.EditMode
{
    public sealed class PortalCrossingHysteresisTests
    {
        private static readonly Type DetectorType = Type.GetType(
            "Anemora.TimeManagement.PortalCrossingDetector, Assembly-CSharp",
            throwOnError: true);

        private static readonly MethodInfo ShouldFlipMethod = DetectorType.GetMethod(
            "ShouldFlip",
            BindingFlags.Public | BindingFlags.Static);

        [Test]
        public void LateralMovementWithUnchangedSignedDistanceDoesNotFlip()
        {
            Assert.That(
                ShouldFlip(new Vector3(12f, 0f, 0.08f), 0.08f),
                Is.False);
        }

        [Test]
        public void HysteresisBandSuppressesNearPlaneSignChange()
        {
            Assert.That(
                ShouldFlip(new Vector3(0f, 0f, -0.01f), 0.03f),
                Is.False);
        }

        [Test]
        public void NormalMovementPastMinimumDistanceFlips()
        {
            Assert.That(
                ShouldFlip(new Vector3(0f, 0f, -0.03f), 0.03f),
                Is.True);
        }

        [Test]
        public void LastStableSignedDistanceCanBeResetAfterFlip()
        {
            var player = new GameObject("Player");
            var detectorObject = new GameObject("Detector");
            var detector = detectorObject.AddComponent(DetectorType);

            try
            {
                var method = DetectorType.GetMethod(
                    "SetLastStableSignedDistance",
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    new[] { typeof(Vector3), typeof(Vector3) },
                    null);

                method.Invoke(detector, new object[] { Vector3.forward, new Vector3(0f, 0f, -0.04f) });

                var lastStable = (float)DetectorType
                    .GetProperty("LastStableSignedDistance", BindingFlags.Public | BindingFlags.Instance)
                    .GetValue(detector);

                Assert.That(lastStable, Is.EqualTo(-0.04f).Within(0.0001f));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(player);
                UnityEngine.Object.DestroyImmediate(detectorObject);
            }
        }

        private static bool ShouldFlip(Vector3 playerPosition, float lastStableSignedDistance)
        {
            return (bool)ShouldFlipMethod.Invoke(
                null,
                new object[]
                {
                    playerPosition,
                    Vector3.zero,
                    Vector3.forward,
                    lastStableSignedDistance,
                    0.02f,
                    0.05f
                });
        }
    }
}
