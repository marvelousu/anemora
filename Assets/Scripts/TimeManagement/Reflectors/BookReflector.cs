using System.Collections.Generic;
using Anemora.Data;
using UnityEngine;

namespace Anemora.TimeManagement.Reflectors
{
    public sealed class BookReflector : MonoBehaviour, IReflector, IReflectedStateRestorer
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
            if (entry == null || entry.reflected || !CanSpawnBook(entry, catalogOverride))
            {
                return false;
            }

            SpawnBook(entry);
            return true;
        }

        public int RestoreReflected(IEnumerable<ActionRecordEntry> entries, ActionRecordCatalog catalogOverride)
        {
            if (entries == null)
            {
                return 0;
            }

            var restoredCount = 0;
            foreach (var entry in entries)
            {
                if (entry == null || !entry.reflected || !CanSpawnBook(entry, catalogOverride))
                {
                    continue;
                }

                SpawnBook(entry);
                restoredCount++;
            }

            return restoredCount;
        }

        private bool CanSpawnBook(ActionRecordEntry entry, ActionRecordCatalog catalogOverride)
        {
            if (entry == null || string.IsNullOrEmpty(entry.actionId) || reflectedActionIds.Contains(entry.actionId))
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

            return true;
        }

        private void SpawnBook(ActionRecordEntry entry)
        {
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
