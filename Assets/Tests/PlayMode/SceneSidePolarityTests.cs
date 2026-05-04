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
    public sealed class SceneSidePolarityTests
    {
        private static readonly Type PolarityType = Type.GetType(
            "Anemora.TimeManagement.SceneSidePolarity, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type SceneSideType = Type.GetType(
            "Anemora.TimeManagement.SceneSide, Assembly-CSharp",
            throwOnError: true);

        [UnityTest]
        public IEnumerator FlipToOppositeRaisesOneEventAndSameSideIsNoop()
        {
            var gameObject = new GameObject("SceneSidePolarityTest");
            var polarity = gameObject.AddComponent(PolarityType);
            var received = new List<string>();
            var eventInfo = PolarityType.GetEvent("SideChanged");
            var recorder = CreateSingleArgumentRecorder(eventInfo.EventHandlerType, received);
            eventInfo.AddEventHandler(polarity, recorder);

            try
            {
                var past = Enum.Parse(SceneSideType, "Past");
                var flipTo = PolarityType.GetMethod("FlipTo", BindingFlags.Public | BindingFlags.Instance);

                Assert.That((bool)flipTo.Invoke(polarity, new[] { past }), Is.True);
                Assert.That((bool)flipTo.Invoke(polarity, new[] { past }), Is.False);
                Assert.That(received, Is.EqualTo(new[] { "Past" }));

                yield return null;
            }
            finally
            {
                eventInfo.RemoveEventHandler(polarity, recorder);
                UnityEngine.Object.DestroyImmediate(gameObject);
            }
        }

        private static Delegate CreateSingleArgumentRecorder(Type handlerType, List<string> destination)
        {
            var parameterType = handlerType.GetMethod("Invoke").GetParameters()[0].ParameterType;
            var parameter = Expression.Parameter(parameterType, "value");
            var add = typeof(List<string>).GetMethod("Add", new[] { typeof(string) });
            var toString = parameterType.GetMethod("ToString", Type.EmptyTypes);
            var body = Expression.Call(Expression.Constant(destination), add, Expression.Call(parameter, toString));
            return Expression.Lambda(handlerType, body, parameter).Compile();
        }
    }
}
