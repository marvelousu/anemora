using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHouseArea
    {
        Interior,
        Exterior,
        CentralPlaza,
        Library
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
        [SerializeField] private FastVsHouseArea activeArea;
        [SerializeField] private Color indoorClearColor = new Color(0.075f, 0.078f, 0.084f, 1f);
        [SerializeField] private Color outdoorSkyClearColor = new Color(0.125f, 0.148f, 0.170f, 1f);

        public FastVsHouseArea ActiveAreaForReview => activeArea;
        public bool InteriorActiveForReview => IsActive(currentInteriorMap) && IsActive(pastInteriorMap);
        public bool ExteriorActiveForReview => IsActive(currentExteriorMap) && IsActive(pastExteriorMap);
        public bool CentralPlazaActiveForReview => IsActive(currentCentralPlazaMap) && IsActive(pastCentralPlazaMap);
        public bool LibraryActiveForReview => IsActive(currentLibraryMap) && IsActive(pastLibraryMap);
        public bool HasAllMapSetsForReview =>
            currentInteriorMap != null &&
            pastInteriorMap != null &&
            currentExteriorMap != null &&
            pastExteriorMap != null &&
            currentCentralPlazaMap != null &&
            pastCentralPlazaMap != null &&
            currentLibraryMap != null &&
            pastLibraryMap != null;

        private void Awake()
        {
            ApplyVisibility();
        }

        public void SetActiveAreaForReview(FastVsHouseArea area)
        {
            activeArea = area;
            ApplyVisibility();
        }

        private void ApplyVisibility()
        {
            SetActive(currentInteriorMap, activeArea == FastVsHouseArea.Interior);
            SetActive(pastInteriorMap, activeArea == FastVsHouseArea.Interior);
            SetActive(currentExteriorMap, activeArea == FastVsHouseArea.Exterior);
            SetActive(pastExteriorMap, activeArea == FastVsHouseArea.Exterior);
            SetActive(currentCentralPlazaMap, activeArea == FastVsHouseArea.CentralPlaza);
            SetActive(pastCentralPlazaMap, activeArea == FastVsHouseArea.CentralPlaza);
            SetActive(currentLibraryMap, activeArea == FastVsHouseArea.Library);
            SetActive(pastLibraryMap, activeArea == FastVsHouseArea.Library);
            ApplyCameraClearColor();
            ApplyLightingProfile();
        }

        private void ApplyCameraClearColor()
        {
            var mainCamera = Camera.main;
            if (mainCamera == null)
            {
                return;
            }

            mainCamera.backgroundColor = activeArea == FastVsHouseArea.Exterior || activeArea == FastVsHouseArea.CentralPlaza
                ? outdoorSkyClearColor
                : indoorClearColor;
        }

        private void ApplyLightingProfile()
        {
            var director = FindFirstObjectByType<FastVsHouseLightingDirector>();
            if (director != null)
            {
                director.ApplyAreaForReview(activeArea);
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
