using System;
using System.Collections.Generic;
using System.Linq;
using Anemora.Data;
using UnityEngine;

namespace Anemora.TimeManagement
{
    [CreateAssetMenu(menuName = "Anemora/ActionRecordCatalog")]
    public sealed class ActionRecordCatalog : ScriptableObject
    {
        [SerializeField] private List<CatalogEntry> entries = new List<CatalogEntry>();

        public IReadOnlyList<CatalogEntry> Entries => entries;

        public void AddEntry(CatalogEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (string.IsNullOrEmpty(entry.actionId))
            {
                throw new ArgumentException("CatalogEntry.actionId must be non-empty", nameof(entry));
            }

            entries.Add(entry);
        }

        public bool TryGetEntry(string actionId, out CatalogEntry entry)
        {
            entry = null;
            if (string.IsNullOrEmpty(actionId))
            {
                return false;
            }

            entry = entries.FirstOrDefault(candidate => candidate.actionId == actionId);
            return entry != null;
        }

        [Serializable]
        public sealed class CatalogEntry
        {
            public string actionId;
            public ActionType type = ActionType.Unknown;
            public string currentSideEffect;
            public List<CatalogParameter> parameters = new List<CatalogParameter>();

            public bool TryGetParameter(string key, out string value)
            {
                value = null;
                if (string.IsNullOrEmpty(key) || parameters == null)
                {
                    return false;
                }

                var parameter = parameters.FirstOrDefault(candidate => candidate.key == key);
                if (parameter == null)
                {
                    return false;
                }

                value = parameter.value;
                return true;
            }
        }

        [Serializable]
        public sealed class CatalogParameter
        {
            public string key;
            public string value;
        }
    }
}
