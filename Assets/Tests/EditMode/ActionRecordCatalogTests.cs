using System;
using System.Collections;
using System.Reflection;
using Anemora.Data;
using NUnit.Framework;
using UnityEngine;

namespace Anemora.Tests.EditMode
{
    public sealed class ActionRecordCatalogTests
    {
        private static readonly Type CatalogType = Type.GetType(
            "Anemora.TimeManagement.ActionRecordCatalog, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type CatalogEntryType = Type.GetType(
            "Anemora.TimeManagement.ActionRecordCatalog+CatalogEntry, Assembly-CSharp",
            throwOnError: true);

        [Test]
        public void AddEntry_AppendsEntryAndFindsByActionId()
        {
            var catalog = ScriptableObject.CreateInstance(CatalogType);
            try
            {
                AddEntry(catalog, CreateEntry("take_book_001", ActionType.Take, "SpawnBookOnBed"));

                Assert.That(CountEntries(catalog), Is.EqualTo(1));
                Assert.That(TryGetEntry(catalog, "take_book_001", out var found), Is.True);
                Assert.That(GetField<string>(found, "actionId"), Is.EqualTo("take_book_001"));
                Assert.That(GetField<ActionType>(found, "type"), Is.EqualTo(ActionType.Take));
                Assert.That(GetField<string>(found, "currentSideEffect"), Is.EqualTo("SpawnBookOnBed"));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(catalog);
            }
        }

        [Test]
        public void TryGetEntry_MissingKey_ReturnsFalse()
        {
            var catalog = ScriptableObject.CreateInstance(CatalogType);
            try
            {
                AddEntry(catalog, CreateEntry("take_book_001", ActionType.Take, "SpawnBookOnBed"));

                Assert.That(TryGetEntry(catalog, "missing", out var found), Is.False);
                Assert.That(found, Is.Null);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(catalog);
            }
        }

        [Test]
        public void AddEntry_EmptyActionId_Throws()
        {
            var catalog = ScriptableObject.CreateInstance(CatalogType);
            try
            {
                var exception = Assert.Throws<TargetInvocationException>(
                    () => AddEntry(catalog, CreateEntry(string.Empty, ActionType.Take, "SpawnBookOnBed")));
                Assert.That(exception.InnerException, Is.TypeOf<ArgumentException>());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(catalog);
            }
        }

        private static object CreateEntry(string actionId, ActionType type, string currentSideEffect)
        {
            var entry = Activator.CreateInstance(CatalogEntryType);
            SetField(entry, "actionId", actionId);
            SetField(entry, "type", type);
            SetField(entry, "currentSideEffect", currentSideEffect);
            return entry;
        }

        private static void AddEntry(UnityEngine.Object catalog, object entry)
        {
            CatalogType.GetMethod("AddEntry").Invoke(catalog, new[] { entry });
        }

        private static bool TryGetEntry(UnityEngine.Object catalog, string actionId, out object entry)
        {
            var parameters = new object[] { actionId, null };
            var found = (bool)CatalogType.GetMethod("TryGetEntry").Invoke(catalog, parameters);
            entry = parameters[1];
            return found;
        }

        private static int CountEntries(UnityEngine.Object catalog)
        {
            var entries = (IEnumerable)CatalogType.GetProperty("Entries").GetValue(catalog);
            var count = 0;
            foreach (var _ in entries)
            {
                count++;
            }

            return count;
        }

        private static void SetField(object target, string fieldName, object value)
        {
            target.GetType()
                .GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(target, value);
        }

        private static T GetField<T>(object target, string fieldName)
        {
            return (T)target.GetType()
                .GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(target);
        }
    }
}
