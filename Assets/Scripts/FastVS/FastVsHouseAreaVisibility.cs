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
        Chapter1End
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
        [SerializeField] private GameObject currentAriaStreetMap;
        [SerializeField] private GameObject pastAriaStreetMap;
        [SerializeField] private GameObject currentKaiaFarmMap;
        [SerializeField] private GameObject pastKaiaFarmMap;
        [SerializeField] private GameObject currentRuinsMap;
        [SerializeField] private GameObject pastRuinsMap;
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
        public bool AriaStreetActiveForReview => IsActive(currentAriaStreetMap) && IsActive(pastAriaStreetMap);
        public bool KaiaFarmActiveForReview => IsActive(currentKaiaFarmMap) && IsActive(pastKaiaFarmMap);
        public bool RuinsActiveForReview => IsActive(currentRuinsMap) && IsActive(pastRuinsMap);
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
            currentAriaStreetMap != null &&
            pastAriaStreetMap != null &&
            currentKaiaFarmMap != null &&
            pastKaiaFarmMap != null &&
            currentRuinsMap != null &&
            pastRuinsMap != null &&
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
            SetActive(currentAriaStreetMap, activeArea == FastVsHouseArea.AriaStreet);
            SetActive(pastAriaStreetMap, activeArea == FastVsHouseArea.AriaStreet);
            SetActive(currentKaiaFarmMap, activeArea == FastVsHouseArea.KaiaFarm);
            SetActive(pastKaiaFarmMap, activeArea == FastVsHouseArea.KaiaFarm);
            SetActive(currentRuinsMap, activeArea == FastVsHouseArea.Ruins);
            SetActive(pastRuinsMap, activeArea == FastVsHouseArea.Ruins);
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
