using System.Collections.Generic;
using System.Linq;
using Anemora.Data;
using Anemora.TimeManagement.Reflectors;
using UnityEngine;

namespace Anemora.TimeManagement
{
    public sealed class ActionRecordRuntime : MonoBehaviour
    {
        [SerializeField] private ActionRecordCatalog catalog;
        [SerializeField] private TimeFramePortalController portalController;
        [SerializeField] private MonoBehaviour[] reflectorBehaviours;

        private readonly ActionRecordStore store = new ActionRecordStore();
        private bool subscribedToPortalController;

        public static ActionRecordRuntime Instance { get; private set; }
        public ActionRecordStore Store => store;
        public IReadOnlyList<ActionRecordEntry> Entries => store.Entries;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Multiple ActionRecordRuntime instances are active in this scene.", this);
            }

            Instance = this;
        }

        private void OnEnable()
        {
            SubscribeToPortalController();
        }

        private void Start()
        {
            SubscribeToPortalController();
        }

        private void OnDisable()
        {
            UnsubscribeFromPortalController();
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void AddEntry(ActionRecordEntry entry)
        {
            store.Add(entry);
        }

        public IEnumerable<ActionRecordEntry> GetUnreflected()
        {
            return store.GetUnreflected();
        }

        public IEnumerable<ActionRecordEntry> GetReflected()
        {
            return store.GetReflected();
        }

        public bool MarkReflected(string actionId)
        {
            return store.MarkReflected(actionId);
        }

        public void LoadFromSaveData(ActionRecordStoreSaveData saveData)
        {
            store.LoadFromSaveData(saveData);
            RestoreReflectedState();
        }

        public ActionRecordStoreSaveData ToSaveData()
        {
            return store.ToSaveData();
        }

        public int ReflectUnreflected()
        {
            var reflectedCount = 0;
            foreach (var entry in store.GetUnreflected().ToList())
            {
                if (TryDispatch(entry))
                {
                    store.MarkReflected(entry.actionId);
                    reflectedCount++;
                }
            }

            return reflectedCount;
        }

        private void SubscribeToPortalController()
        {
            if (subscribedToPortalController)
            {
                return;
            }

            if (portalController == null)
            {
                portalController = FindFirstObjectByType<TimeFramePortalController>();
            }

            if (portalController == null)
            {
                return;
            }

            portalController.CrossingCompleted += HandleCrossingCompleted;
            subscribedToPortalController = true;
        }

        private void UnsubscribeFromPortalController()
        {
            if (!subscribedToPortalController || portalController == null)
            {
                return;
            }

            portalController.CrossingCompleted -= HandleCrossingCompleted;
            subscribedToPortalController = false;
        }

        private void HandleCrossingCompleted(SceneSide targetSide)
        {
            if (targetSide == SceneSide.Current)
            {
                ReflectUnreflected();
            }
        }

        private bool TryDispatch(ActionRecordEntry entry)
        {
            foreach (var reflector in ResolveReflectors())
            {
                if (reflector.TryReflect(entry, catalog))
                {
                    return true;
                }
            }

            return false;
        }

        private int RestoreReflectedState()
        {
            var reflectedEntries = store.GetReflected().ToList();
            if (reflectedEntries.Count == 0)
            {
                return 0;
            }

            var restoredCount = 0;
            foreach (var restorer in ResolveReflectedStateRestorers())
            {
                restoredCount += restorer.RestoreReflected(reflectedEntries, catalog);
            }

            return restoredCount;
        }

        private IEnumerable<IReflector> ResolveReflectors()
        {
            if (reflectorBehaviours == null || reflectorBehaviours.Length == 0)
            {
                reflectorBehaviours = GetComponentsInChildren<MonoBehaviour>(true)
                    .Where(behaviour => behaviour is IReflector)
                    .ToArray();
            }

            foreach (var behaviour in reflectorBehaviours)
            {
                if (behaviour is IReflector reflector)
                {
                    yield return reflector;
                }
            }
        }

        private IEnumerable<IReflectedStateRestorer> ResolveReflectedStateRestorers()
        {
            if (reflectorBehaviours == null || reflectorBehaviours.Length == 0)
            {
                reflectorBehaviours = GetComponentsInChildren<MonoBehaviour>(true)
                    .Where(behaviour => behaviour is IReflector || behaviour is IReflectedStateRestorer)
                    .ToArray();
            }

            foreach (var behaviour in reflectorBehaviours)
            {
                if (behaviour is IReflectedStateRestorer restorer)
                {
                    yield return restorer;
                }
            }
        }
    }
}
