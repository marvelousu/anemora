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
