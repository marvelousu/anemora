using UnityEngine;

namespace Anemora.TimeManagement
{
    /// <summary>
    /// Central access point for current/past scene roots used by the time-frame prototype.
    /// </summary>
    public sealed class SceneRootRegistry : MonoBehaviour
    {
        [SerializeField] private GameObject rootCurrent;
        [SerializeField] private GameObject rootPast;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera pastCamera;
        [SerializeField] private bool pastRootVisibleOnStart = true;

        public static SceneRootRegistry Instance { get; private set; }

        public GameObject RootCurrent => rootCurrent;
        public GameObject RootPast => rootPast;
        public Camera MainCamera => mainCamera;
        public Camera PastCamera => pastCamera;

        private void Awake()
        {
            Instance = this;

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            SetPastRootVisible(pastRootVisibleOnStart);
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void SetPastRootVisible(bool visible)
        {
            if (rootPast != null && rootPast.activeSelf != visible)
            {
                rootPast.SetActive(visible);
            }
        }

        public void TogglePastRoot()
        {
            if (rootPast != null)
            {
                rootPast.SetActive(!rootPast.activeSelf);
            }
        }
    }
}
