using System.Collections.Generic;
using Anemora.Data;
using UnityEngine;

namespace Anemora.TimeManagement.Reflectors
{
    public sealed class BookReflector : MonoBehaviour, IReflector
    {
        private const string DefaultSpawnBookSideEffect = "SpawnBookOnBed";

        [SerializeField] private ActionRecordCatalog catalog;
        [SerializeField] private string spawnBookSideEffect = DefaultSpawnBookSideEffect;
        [SerializeField] private GameObject bookPrefab;
        [SerializeField] private Transform bedSpawnPoint;
        [SerializeField] private Transform spawnParent;

        private readonly HashSet<string> reflectedActionIds = new HashSet<string>();

        public GameObject LastSpawnedBook { get; private set; }
        public int ReflectionCount { get; private set; }

        public bool TryReflect(ActionRecordEntry entry, ActionRecordCatalog catalogOverride)
        {
            if (entry == null || entry.reflected || string.IsNullOrEmpty(entry.actionId))
            {
                return false;
            }

            if (reflectedActionIds.Contains(entry.actionId))
            {
                return false;
            }

            var resolvedCatalog = catalogOverride != null ? catalogOverride : catalog;
            if (resolvedCatalog == null ||
                !resolvedCatalog.TryGetEntry(entry.actionId, out var catalogEntry) ||
                !MatchesBookSideEffect(entry, catalogEntry))
            {
                return false;
            }

            if (bookPrefab == null)
            {
                return false;
            }

            var spawnTransform = bedSpawnPoint != null ? bedSpawnPoint : transform;
            var parent = spawnParent != null ? spawnParent : null;
            LastSpawnedBook = Instantiate(
                bookPrefab,
                spawnTransform.position,
                spawnTransform.rotation,
                parent);
            LastSpawnedBook.name = $"{bookPrefab.name}_{entry.actionId}_Reflected";
            reflectedActionIds.Add(entry.actionId);
            ReflectionCount++;
            return true;
        }

        private bool MatchesBookSideEffect(
            ActionRecordEntry entry,
            ActionRecordCatalog.CatalogEntry catalogEntry)
        {
            if (catalogEntry == null)
            {
                return false;
            }

            if (catalogEntry.type != ActionType.Unknown && catalogEntry.type != entry.type)
            {
                return false;
            }

            var expectedSideEffect = string.IsNullOrEmpty(spawnBookSideEffect)
                ? DefaultSpawnBookSideEffect
                : spawnBookSideEffect;
            return catalogEntry.currentSideEffect == expectedSideEffect;
        }
    }
}
