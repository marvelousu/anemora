using System;
using System.Collections.Generic;
using System.Linq;

namespace Anemora.Data
{
    [Serializable]
    public sealed class ActionRecordEntry
    {
        public string actionId;
        public string targetObjectId;
        public ActionType type;
        public long gameTimeTicks;
        public bool reflected;
    }

    [Serializable]
    public sealed class ActionRecordStoreSaveData
    {
        public List<ActionRecordEntry> entries = new List<ActionRecordEntry>();
    }

    public sealed class ActionRecordStore
    {
        private readonly List<ActionRecordEntry> entries = new List<ActionRecordEntry>();

        public IReadOnlyList<ActionRecordEntry> Entries => entries;

        public void Add(ActionRecordEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            if (string.IsNullOrEmpty(entry.actionId))
            {
                throw new ArgumentException("ActionRecordEntry.actionId must be non-empty", nameof(entry));
            }
            entries.Add(entry);
        }

        public IEnumerable<ActionRecordEntry> GetUnreflected()
        {
            return entries.Where(e => !e.reflected);
        }

        public IEnumerable<ActionRecordEntry> GetReflected()
        {
            return entries.Where(e => e.reflected);
        }

        public bool MarkReflected(string actionId)
        {
            if (string.IsNullOrEmpty(actionId))
            {
                return false;
            }
            var entry = entries.FirstOrDefault(e => e.actionId == actionId && !e.reflected);
            if (entry == null)
            {
                return false;
            }
            entry.reflected = true;
            return true;
        }

        public ActionRecordStoreSaveData ToSaveData()
        {
            return new ActionRecordStoreSaveData
            {
                entries = entries
                    .Select(e => new ActionRecordEntry
                    {
                        actionId = e.actionId,
                        targetObjectId = e.targetObjectId,
                        type = e.type,
                        gameTimeTicks = e.gameTimeTicks,
                        reflected = e.reflected
                    })
                    .ToList()
            };
        }

        public void LoadFromSaveData(ActionRecordStoreSaveData saveData)
        {
            entries.Clear();
            if (saveData?.entries == null)
            {
                return;
            }
            entries.AddRange(saveData.entries);
        }

        public void Clear()
        {
            entries.Clear();
        }
    }
}
