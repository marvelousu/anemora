using System;
using System.Collections;
using System.Reflection;
using Anemora.Data;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class BookReflectorIntegrationTests
    {
        private static readonly Type CatalogType = Type.GetType(
            "Anemora.TimeManagement.ActionRecordCatalog, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type CatalogEntryType = Type.GetType(
            "Anemora.TimeManagement.ActionRecordCatalog+CatalogEntry, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type BookReflectorType = Type.GetType(
            "Anemora.TimeManagement.Reflectors.BookReflector, Assembly-CSharp",
            throwOnError: true);

        private static readonly Type ActionRecordRuntimeType = Type.GetType(
            "Anemora.TimeManagement.ActionRecordRuntime, Assembly-CSharp",
            throwOnError: true);

        [UnityTest]
        public IEnumerator ReflectKnownAction_SpawnsBookAtBed()
        {
            var harness = CreateHarness(withPrefab: true);
            try
            {
                AddCatalogEntry(harness.Catalog, "take_book_001", ActionType.Take, "SpawnBookOnBed");

                var reflected = TryReflect(
                    harness.Reflector,
                    new ActionRecordEntry { actionId = "take_book_001", type = ActionType.Take },
                    harness.Catalog);

                Assert.That(reflected, Is.True);
                Assert.That(GetProperty<int>(harness.Reflector, "ReflectionCount"), Is.EqualTo(1));
                var spawned = GetProperty<GameObject>(harness.Reflector, "LastSpawnedBook");
                Assert.That(spawned, Is.Not.Null);
                Assert.That(spawned.transform.position, Is.EqualTo(harness.Bed.transform.position));
                Assert.That(spawned.transform.parent, Is.EqualTo(harness.SpawnParent.transform));
                yield return null;
            }
            finally
            {
                harness.Destroy();
            }
        }

        [UnityTest]
        public IEnumerator MissingActionId_IsIgnored()
        {
            var harness = CreateHarness(withPrefab: true);
            try
            {
                AddCatalogEntry(harness.Catalog, "take_book_001", ActionType.Take, "SpawnBookOnBed");

                var reflected = TryReflect(
                    harness.Reflector,
                    new ActionRecordEntry { actionId = "unknown_action", type = ActionType.Take },
                    harness.Catalog);

                Assert.That(reflected, Is.False);
                Assert.That(GetProperty<int>(harness.Reflector, "ReflectionCount"), Is.EqualTo(0));
                Assert.That(GetProperty<GameObject>(harness.Reflector, "LastSpawnedBook"), Is.Null);
                yield return null;
            }
            finally
            {
                harness.Destroy();
            }
        }

        [UnityTest]
        public IEnumerator NullPrefab_IsNoOp()
        {
            var harness = CreateHarness(withPrefab: false);
            try
            {
                AddCatalogEntry(harness.Catalog, "take_book_001", ActionType.Take, "SpawnBookOnBed");

                var reflected = TryReflect(
                    harness.Reflector,
                    new ActionRecordEntry { actionId = "take_book_001", type = ActionType.Take },
                    harness.Catalog);

                Assert.That(reflected, Is.False);
                Assert.That(GetProperty<int>(harness.Reflector, "ReflectionCount"), Is.EqualTo(0));
                Assert.That(GetProperty<GameObject>(harness.Reflector, "LastSpawnedBook"), Is.Null);
                yield return null;
            }
            finally
            {
                harness.Destroy();
            }
        }

        [UnityTest]
        public IEnumerator RuntimeDispatchesUnreflectedRecordsOnceAndMarksReflected()
        {
            var harness = CreateHarness(withPrefab: true);
            var runtimeObject = new GameObject("ActionRecordRuntimeTest");
            var runtime = runtimeObject.AddComponent(ActionRecordRuntimeType);
            try
            {
                AddCatalogEntry(harness.Catalog, "take_book_001", ActionType.Take, "SpawnBookOnBed");
                SetField(runtime, "catalog", harness.Catalog);
                SetField(runtime, "reflectorBehaviours", new[] { (MonoBehaviour)harness.Reflector });

                var validEntry = new ActionRecordEntry { actionId = "take_book_001", type = ActionType.Take };
                var missingEntry = new ActionRecordEntry { actionId = "missing", type = ActionType.Take };
                ActionRecordRuntimeType.GetMethod("AddEntry").Invoke(runtime, new object[] { validEntry });
                ActionRecordRuntimeType.GetMethod("AddEntry").Invoke(runtime, new object[] { missingEntry });

                Assert.That(CallReflectUnreflected(runtime), Is.EqualTo(1));
                Assert.That(validEntry.reflected, Is.True);
                Assert.That(missingEntry.reflected, Is.False);
                Assert.That(CallReflectUnreflected(runtime), Is.EqualTo(0));
                Assert.That(GetProperty<int>(harness.Reflector, "ReflectionCount"), Is.EqualTo(1));
                yield return null;
            }
            finally
            {
                DestroyObject(runtimeObject);
                harness.Destroy();
            }
        }

        private static Harness CreateHarness(bool withPrefab)
        {
            var catalog = ScriptableObject.CreateInstance(CatalogType);
            var root = new GameObject("BookReflectorTest");
            var reflector = root.AddComponent(BookReflectorType);
            var bed = new GameObject("BedSpawnPoint");
            bed.transform.position = new Vector3(1f, 2f, 3f);
            bed.transform.rotation = Quaternion.Euler(0f, 45f, 0f);
            var spawnParent = new GameObject("BookSpawnParent");
            var prefab = withPrefab ? GameObject.CreatePrimitive(PrimitiveType.Cube) : null;
            if (prefab != null)
            {
                prefab.name = "CurrentBookPlaceholder_Test";
            }

            SetField(reflector, "catalog", catalog);
            SetField(reflector, "bookPrefab", prefab);
            SetField(reflector, "bedSpawnPoint", bed.transform);
            SetField(reflector, "spawnParent", spawnParent.transform);

            return new Harness(catalog, root, reflector, bed, spawnParent, prefab);
        }

        private static object CreateCatalogEntry(string actionId, ActionType type, string currentSideEffect)
        {
            var entry = Activator.CreateInstance(CatalogEntryType);
            SetField(entry, "actionId", actionId);
            SetField(entry, "type", type);
            SetField(entry, "currentSideEffect", currentSideEffect);
            return entry;
        }

        private static void AddCatalogEntry(UnityEngine.Object catalog, string actionId, ActionType type, string effect)
        {
            CatalogType.GetMethod("AddEntry")
                .Invoke(catalog, new[] { CreateCatalogEntry(actionId, type, effect) });
        }

        private static bool TryReflect(object reflector, ActionRecordEntry entry, UnityEngine.Object catalog)
        {
            return (bool)BookReflectorType.GetMethod("TryReflect")
                .Invoke(reflector, new object[] { entry, catalog });
        }

        private static int CallReflectUnreflected(UnityEngine.Object runtime)
        {
            return (int)ActionRecordRuntimeType.GetMethod("ReflectUnreflected")
                .Invoke(runtime, Array.Empty<object>());
        }

        private static void SetField(object target, string fieldName, object value)
        {
            target.GetType()
                .GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(target, value);
        }

        private static T GetProperty<T>(object target, string propertyName)
        {
            return (T)target.GetType()
                .GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)
                .GetValue(target);
        }

        private static void DestroyObject(UnityEngine.Object value)
        {
            if (value != null)
            {
                UnityEngine.Object.DestroyImmediate(value);
            }
        }

        private sealed class Harness
        {
            public Harness(
                UnityEngine.Object catalog,
                GameObject root,
                object reflector,
                GameObject bed,
                GameObject spawnParent,
                GameObject prefab)
            {
                Catalog = catalog;
                Root = root;
                Reflector = reflector;
                Bed = bed;
                SpawnParent = spawnParent;
                Prefab = prefab;
            }

            public UnityEngine.Object Catalog { get; }
            public GameObject Root { get; }
            public object Reflector { get; }
            public GameObject Bed { get; }
            public GameObject SpawnParent { get; }
            public GameObject Prefab { get; }

            public void Destroy()
            {
                DestroyObject(GetProperty<GameObject>(Reflector, "LastSpawnedBook"));
                DestroyObject(Prefab);
                DestroyObject(SpawnParent);
                DestroyObject(Bed);
                DestroyObject(Root);
                DestroyObject(Catalog);
            }
        }
    }
}
