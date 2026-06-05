using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHouseArea
    {
        Interior,
        Exterior,
        CentralPlaza,
        Library,
        MiaHouse,
        AriaStreet,
        KaiaFarm,
        Ruins,
        Chapter1End,
        MiaInterior,
        AriaInterior,
        KaiaInterior,
        RuinsF4Interior,
        RuinsF2Interior,
        RuinsF3Interior,
        RuinsF5Interior
    }

    public sealed class FastVsHouseAreaVisibility : MonoBehaviour
    {
        [SerializeField] private GameObject currentInteriorMap;
        [SerializeField] private GameObject pastInteriorMap;
        [SerializeField] private GameObject currentExteriorMap;
        [SerializeField] private GameObject pastExteriorMap;
        [SerializeField] private GameObject currentCentralPlazaMap;
        [SerializeField] private GameObject pastCentralPlazaMap;
        [SerializeField] private GameObject currentLibraryMap;
        [SerializeField] private GameObject pastLibraryMap;
        [SerializeField] private GameObject currentMiaHouseMap;
        [SerializeField] private GameObject pastMiaHouseMap;
        [SerializeField] private GameObject currentMiaInteriorMap;
        [SerializeField] private GameObject pastMiaInteriorMap;
        [SerializeField] private GameObject currentAriaStreetMap;
        [SerializeField] private GameObject pastAriaStreetMap;
        [SerializeField] private GameObject currentAriaInteriorMap;
        [SerializeField] private GameObject pastAriaInteriorMap;
        [SerializeField] private GameObject currentKaiaFarmMap;
        [SerializeField] private GameObject pastKaiaFarmMap;
        [SerializeField] private GameObject currentKaiaInteriorMap;
        [SerializeField] private GameObject pastKaiaInteriorMap;
        [SerializeField] private GameObject currentRuinsMap;
        [SerializeField] private GameObject pastRuinsMap;
        [SerializeField] private GameObject currentRuinsF4InteriorMap;
        [SerializeField] private GameObject pastRuinsF4InteriorMap;
        [SerializeField] private GameObject currentRuinsF2InteriorMap;
        [SerializeField] private GameObject pastRuinsF2InteriorMap;
        [SerializeField] private GameObject currentRuinsF3InteriorMap;
        [SerializeField] private GameObject pastRuinsF3InteriorMap;
        [SerializeField] private GameObject currentRuinsF5InteriorMap;
        [SerializeField] private GameObject pastRuinsF5InteriorMap;
        [SerializeField] private GameObject currentChapter1EndMap;
        [SerializeField] private GameObject pastChapter1EndMap;
        [SerializeField] private FastVsHouseArea activeArea;
        [SerializeField] private Color indoorClearColor = new Color(0.064f, 0.060f, 0.060f, 1f);
        [SerializeField] private Color exteriorSkyClearColor = new Color(0.118f, 0.142f, 0.166f, 1f);
        [SerializeField] private Color centralPlazaSkyClearColor = new Color(0.220f, 0.286f, 0.340f, 1f);

        private bool hasRuntimeTimeSetActiveIsolation;
        private bool runtimeTimeSetActiveOtherTime;
        private bool runtimeTimeSetActiveKeepBothTimes;
        private bool runtimeTimeSetActiveForceKeepBothTimes;

        public FastVsHouseArea ActiveAreaForReview => activeArea;
        public bool InteriorActiveForReview => IsActive(currentInteriorMap) && IsActive(pastInteriorMap);
        public bool ExteriorActiveForReview => IsActive(currentExteriorMap) && IsActive(pastExteriorMap);
        public bool CentralPlazaActiveForReview => IsActive(currentCentralPlazaMap) && IsActive(pastCentralPlazaMap);
        public bool LibraryActiveForReview => IsActive(currentLibraryMap) && IsActive(pastLibraryMap);
        public bool MiaHouseActiveForReview => IsActive(currentMiaHouseMap) && IsActive(pastMiaHouseMap);
        public bool MiaInteriorActiveForReview => IsActive(currentMiaInteriorMap) && IsActive(pastMiaInteriorMap);
        public bool AriaStreetActiveForReview => IsActive(currentAriaStreetMap) && IsActive(pastAriaStreetMap);
        public bool AriaInteriorActiveForReview => IsActive(currentAriaInteriorMap) && IsActive(pastAriaInteriorMap);
        public bool KaiaFarmActiveForReview => IsActive(currentKaiaFarmMap) && IsActive(pastKaiaFarmMap);
        public bool KaiaInteriorActiveForReview => IsActive(currentKaiaInteriorMap) && IsActive(pastKaiaInteriorMap);
        public bool RuinsActiveForReview => IsActive(currentRuinsMap) && IsActive(pastRuinsMap);
        public bool RuinsF4InteriorActiveForReview => IsActive(currentRuinsF4InteriorMap) && IsActive(pastRuinsF4InteriorMap);
        public bool RuinsF2InteriorActiveForReview => IsActive(currentRuinsF2InteriorMap) && IsActive(pastRuinsF2InteriorMap);
        public bool RuinsF3InteriorActiveForReview => IsActive(currentRuinsF3InteriorMap) && IsActive(pastRuinsF3InteriorMap);
        public bool RuinsF5InteriorActiveForReview => IsActive(currentRuinsF5InteriorMap) && IsActive(pastRuinsF5InteriorMap);
        public bool Chapter1EndActiveForReview => IsActive(currentChapter1EndMap) && IsActive(pastChapter1EndMap);
        public bool HasAllMapSetsForReview =>
            currentInteriorMap != null &&
            pastInteriorMap != null &&
            currentExteriorMap != null &&
            pastExteriorMap != null &&
            currentCentralPlazaMap != null &&
            pastCentralPlazaMap != null &&
            currentLibraryMap != null &&
            pastLibraryMap != null &&
            currentMiaHouseMap != null &&
            pastMiaHouseMap != null &&
            currentMiaInteriorMap != null &&
            pastMiaInteriorMap != null &&
            currentAriaStreetMap != null &&
            pastAriaStreetMap != null &&
            currentAriaInteriorMap != null &&
            pastAriaInteriorMap != null &&
            currentKaiaFarmMap != null &&
            pastKaiaFarmMap != null &&
            currentKaiaInteriorMap != null &&
            pastKaiaInteriorMap != null &&
            currentRuinsMap != null &&
            pastRuinsMap != null &&
            currentRuinsF4InteriorMap != null &&
            pastRuinsF4InteriorMap != null &&
            currentRuinsF2InteriorMap != null &&
            pastRuinsF2InteriorMap != null &&
            currentRuinsF3InteriorMap != null &&
            pastRuinsF3InteriorMap != null &&
            currentRuinsF5InteriorMap != null &&
            pastRuinsF5InteriorMap != null &&
            currentChapter1EndMap != null &&
            pastChapter1EndMap != null;
        public bool RuntimeTimeSetActiveIsolationForReview => hasRuntimeTimeSetActiveIsolation;
        public bool RuntimeTimeSetActiveOtherTimeForReview => runtimeTimeSetActiveOtherTime;
        public bool RuntimeTimeSetActiveKeepBothTimesForReview => runtimeTimeSetActiveKeepBothTimes;
        public bool RuntimeTimeSetActiveForceKeepBothTimesForReview => runtimeTimeSetActiveForceKeepBothTimes;
        public int RuntimeActiveAreaRootCountForReview => CountRuntimeActiveAreaRoots();

        private void Awake()
        {
            ApplyVisibility(false);
        }

        public void SetActiveAreaForReview(FastVsHouseArea area)
        {
            activeArea = area;
            ApplyVisibility(false);
        }

        public void SetActiveAreaWithLightingTransitionForReview(FastVsHouseArea area)
        {
            activeArea = area;
            ApplyVisibility(true);
        }

        public void ApplyRuntimeTimeSetActiveIsolationForReview(bool playerInOtherTime, bool keepBothTimesActive)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (hasRuntimeTimeSetActiveIsolation &&
                runtimeTimeSetActiveOtherTime == playerInOtherTime &&
                runtimeTimeSetActiveKeepBothTimes == keepBothTimesActive)
            {
                return;
            }

            hasRuntimeTimeSetActiveIsolation = true;
            runtimeTimeSetActiveOtherTime = playerInOtherTime;
            runtimeTimeSetActiveKeepBothTimes = keepBothTimesActive;
            ApplyAreaMapVisibility(ShouldKeepCurrentTimeActive(), ShouldKeepPastTimeActive());
        }

        public void SetRuntimeTimeSetActiveForceKeepBothTimesForReview(bool forceKeepBothTimes)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            runtimeTimeSetActiveForceKeepBothTimes = forceKeepBothTimes;
            if (hasRuntimeTimeSetActiveIsolation)
            {
                ApplyAreaMapVisibility(ShouldKeepCurrentTimeActive(), ShouldKeepPastTimeActive());
            }
        }

        private void ApplyVisibility(bool transitionLighting)
        {
            ApplyAreaMapVisibility(ShouldKeepCurrentTimeActive(), ShouldKeepPastTimeActive());
            if (!ApplyLightingProfile(transitionLighting))
            {
                ApplyCameraClearColor();
            }

            NotifyRealtimeShadowPolicyAreaTransition();
        }

        private void ApplyAreaMapVisibility(bool keepCurrentTimeActive, bool keepPastTimeActive)
        {
            SetAreaPairActive(currentInteriorMap, pastInteriorMap, FastVsHouseArea.Interior, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentExteriorMap, pastExteriorMap, FastVsHouseArea.Exterior, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentCentralPlazaMap, pastCentralPlazaMap, FastVsHouseArea.CentralPlaza, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentLibraryMap, pastLibraryMap, FastVsHouseArea.Library, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentMiaHouseMap, pastMiaHouseMap, FastVsHouseArea.MiaHouse, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentMiaInteriorMap, pastMiaInteriorMap, FastVsHouseArea.MiaInterior, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentAriaStreetMap, pastAriaStreetMap, FastVsHouseArea.AriaStreet, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentAriaInteriorMap, pastAriaInteriorMap, FastVsHouseArea.AriaInterior, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentKaiaFarmMap, pastKaiaFarmMap, FastVsHouseArea.KaiaFarm, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentKaiaInteriorMap, pastKaiaInteriorMap, FastVsHouseArea.KaiaInterior, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentRuinsMap, pastRuinsMap, FastVsHouseArea.Ruins, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentRuinsF4InteriorMap, pastRuinsF4InteriorMap, FastVsHouseArea.RuinsF4Interior, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentRuinsF2InteriorMap, pastRuinsF2InteriorMap, FastVsHouseArea.RuinsF2Interior, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentRuinsF3InteriorMap, pastRuinsF3InteriorMap, FastVsHouseArea.RuinsF3Interior, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentRuinsF5InteriorMap, pastRuinsF5InteriorMap, FastVsHouseArea.RuinsF5Interior, keepCurrentTimeActive, keepPastTimeActive);
            SetAreaPairActive(currentChapter1EndMap, pastChapter1EndMap, FastVsHouseArea.Chapter1End, keepCurrentTimeActive, keepPastTimeActive);
        }

        private void SetAreaPairActive(
            GameObject currentTimeMap,
            GameObject pastTimeMap,
            FastVsHouseArea area,
            bool keepCurrentTimeActive,
            bool keepPastTimeActive)
        {
            var areaActive = activeArea == area;
            SetActive(currentTimeMap, areaActive && keepCurrentTimeActive);
            SetActive(pastTimeMap, areaActive && keepPastTimeActive);
        }

        private bool ShouldKeepCurrentTimeActive()
        {
            return !hasRuntimeTimeSetActiveIsolation ||
                   runtimeTimeSetActiveForceKeepBothTimes ||
                   runtimeTimeSetActiveKeepBothTimes ||
                   !runtimeTimeSetActiveOtherTime;
        }

        private bool ShouldKeepPastTimeActive()
        {
            return !hasRuntimeTimeSetActiveIsolation ||
                   runtimeTimeSetActiveForceKeepBothTimes ||
                   runtimeTimeSetActiveKeepBothTimes ||
                   runtimeTimeSetActiveOtherTime;
        }

        private int CountRuntimeActiveAreaRoots()
        {
            var count = 0;
            CountIfActive(currentInteriorMap, ref count);
            CountIfActive(pastInteriorMap, ref count);
            CountIfActive(currentExteriorMap, ref count);
            CountIfActive(pastExteriorMap, ref count);
            CountIfActive(currentCentralPlazaMap, ref count);
            CountIfActive(pastCentralPlazaMap, ref count);
            CountIfActive(currentLibraryMap, ref count);
            CountIfActive(pastLibraryMap, ref count);
            CountIfActive(currentMiaHouseMap, ref count);
            CountIfActive(pastMiaHouseMap, ref count);
            CountIfActive(currentMiaInteriorMap, ref count);
            CountIfActive(pastMiaInteriorMap, ref count);
            CountIfActive(currentAriaStreetMap, ref count);
            CountIfActive(pastAriaStreetMap, ref count);
            CountIfActive(currentAriaInteriorMap, ref count);
            CountIfActive(pastAriaInteriorMap, ref count);
            CountIfActive(currentKaiaFarmMap, ref count);
            CountIfActive(pastKaiaFarmMap, ref count);
            CountIfActive(currentKaiaInteriorMap, ref count);
            CountIfActive(pastKaiaInteriorMap, ref count);
            CountIfActive(currentRuinsMap, ref count);
            CountIfActive(pastRuinsMap, ref count);
            CountIfActive(currentRuinsF4InteriorMap, ref count);
            CountIfActive(pastRuinsF4InteriorMap, ref count);
            CountIfActive(currentRuinsF2InteriorMap, ref count);
            CountIfActive(pastRuinsF2InteriorMap, ref count);
            CountIfActive(currentRuinsF3InteriorMap, ref count);
            CountIfActive(pastRuinsF3InteriorMap, ref count);
            CountIfActive(currentRuinsF5InteriorMap, ref count);
            CountIfActive(pastRuinsF5InteriorMap, ref count);
            CountIfActive(currentChapter1EndMap, ref count);
            CountIfActive(pastChapter1EndMap, ref count);
            return count;
        }

        private static void CountIfActive(GameObject target, ref int count)
        {
            if (IsActive(target))
            {
                count++;
            }
        }

        private void ApplyCameraClearColor()
        {
            var mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }

            mainCamera.backgroundColor = activeArea == FastVsHouseArea.Exterior || activeArea == FastVsHouseArea.CentralPlaza
                ? (activeArea == FastVsHouseArea.Exterior ? exteriorSkyClearColor : centralPlazaSkyClearColor)
                : indoorClearColor;
        }

        private bool ApplyLightingProfile(bool transitionLighting)
        {
            var director = FindFirstObjectByType<FastVsHouseLightingDirector>();
            if (director != null)
            {
                if (transitionLighting)
                {
                    director.BeginAreaTransitionForReview(activeArea);
                }
                else
                {
                    director.ApplyAreaForReview(activeArea);
                }

                return true;
            }

            return false;
        }

        private void NotifyRealtimeShadowPolicyAreaTransition()
        {
            var realtimeRig = FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            if (realtimeRig != null)
            {
                realtimeRig.ApplyRendererShadowPolicyForAreaTransitionForReview(activeArea);
            }
        }

        private static void SetActive(GameObject target, bool active)
        {
            if (target != null && target.activeSelf != active)
            {
                target.SetActive(active);
            }
        }

        private static bool IsActive(GameObject target)
        {
            return target != null && target.activeSelf;
        }
    }
}
