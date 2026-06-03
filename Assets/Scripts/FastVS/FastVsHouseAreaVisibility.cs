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

        private void ApplyVisibility(bool transitionLighting)
        {
            SetActive(currentInteriorMap, activeArea == FastVsHouseArea.Interior);
            SetActive(pastInteriorMap, activeArea == FastVsHouseArea.Interior);
            SetActive(currentExteriorMap, activeArea == FastVsHouseArea.Exterior);
            SetActive(pastExteriorMap, activeArea == FastVsHouseArea.Exterior);
            SetActive(currentCentralPlazaMap, activeArea == FastVsHouseArea.CentralPlaza);
            SetActive(pastCentralPlazaMap, activeArea == FastVsHouseArea.CentralPlaza);
            SetActive(currentLibraryMap, activeArea == FastVsHouseArea.Library);
            SetActive(pastLibraryMap, activeArea == FastVsHouseArea.Library);
            SetActive(currentMiaHouseMap, activeArea == FastVsHouseArea.MiaHouse);
            SetActive(pastMiaHouseMap, activeArea == FastVsHouseArea.MiaHouse);
            SetActive(currentMiaInteriorMap, activeArea == FastVsHouseArea.MiaInterior);
            SetActive(pastMiaInteriorMap, activeArea == FastVsHouseArea.MiaInterior);
            SetActive(currentAriaStreetMap, activeArea == FastVsHouseArea.AriaStreet);
            SetActive(pastAriaStreetMap, activeArea == FastVsHouseArea.AriaStreet);
            SetActive(currentAriaInteriorMap, activeArea == FastVsHouseArea.AriaInterior);
            SetActive(pastAriaInteriorMap, activeArea == FastVsHouseArea.AriaInterior);
            SetActive(currentKaiaFarmMap, activeArea == FastVsHouseArea.KaiaFarm);
            SetActive(pastKaiaFarmMap, activeArea == FastVsHouseArea.KaiaFarm);
            SetActive(currentKaiaInteriorMap, activeArea == FastVsHouseArea.KaiaInterior);
            SetActive(pastKaiaInteriorMap, activeArea == FastVsHouseArea.KaiaInterior);
            SetActive(currentRuinsMap, activeArea == FastVsHouseArea.Ruins);
            SetActive(pastRuinsMap, activeArea == FastVsHouseArea.Ruins);
            SetActive(currentRuinsF4InteriorMap, activeArea == FastVsHouseArea.RuinsF4Interior);
            SetActive(pastRuinsF4InteriorMap, activeArea == FastVsHouseArea.RuinsF4Interior);
            SetActive(currentRuinsF2InteriorMap, activeArea == FastVsHouseArea.RuinsF2Interior);
            SetActive(pastRuinsF2InteriorMap, activeArea == FastVsHouseArea.RuinsF2Interior);
            SetActive(currentRuinsF3InteriorMap, activeArea == FastVsHouseArea.RuinsF3Interior);
            SetActive(pastRuinsF3InteriorMap, activeArea == FastVsHouseArea.RuinsF3Interior);
            SetActive(currentRuinsF5InteriorMap, activeArea == FastVsHouseArea.RuinsF5Interior);
            SetActive(pastRuinsF5InteriorMap, activeArea == FastVsHouseArea.RuinsF5Interior);
            SetActive(currentChapter1EndMap, activeArea == FastVsHouseArea.Chapter1End);
            SetActive(pastChapter1EndMap, activeArea == FastVsHouseArea.Chapter1End);
            if (!ApplyLightingProfile(transitionLighting))
            {
                ApplyCameraClearColor();
            }

            NotifyRealtimeShadowPolicyAreaTransition();
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
