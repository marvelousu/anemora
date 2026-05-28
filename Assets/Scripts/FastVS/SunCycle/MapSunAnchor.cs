using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Anemora.FastVS.SunCycle
{
    [AddComponentMenu("Anemora/HD2D/Map Sun Anchor")]
    [DisallowMultipleComponent]
    public sealed class MapSunAnchor : MonoBehaviour
    {
        [Header("Preset")]
        [SerializeField] private SunPreset sunPreset = SunPreset.Noon;

        [Header("Priority")]
        [SerializeField] private int priority;

        [Header("Transition")]
        [SerializeField] private bool transitionFromPrevious;

        [Header("Debug")]
        [SerializeField] private bool drawGizmo = true;

        public SunPreset SunPreset => sunPreset;
        public int Priority => priority;
        public bool TransitionFromPrevious => transitionFromPrevious;

        public void SetPresetAtRuntime(SunPreset newPreset, bool instant = false)
        {
            sunPreset = newPreset;
            var driver = ResolveDriver();
            if (driver != null)
            {
                driver.ApplyPreset(sunPreset, instant);
            }
        }

        private void OnEnable()
        {
            var driver = ResolveDriver();
            if (driver != null)
            {
                driver.RegisterMapSunAnchor(this);
            }
            else
            {
                Debug.LogWarning("[MapSunAnchor] AnemoraSunCycleDriver was not found. The driver default preset will be used.", this);
            }
        }

        private static AnemoraSunCycleDriver ResolveDriver()
        {
            if (AnemoraSunCycleDriver.Instance != null)
            {
                return AnemoraSunCycleDriver.Instance;
            }

            return FindFirstObjectByType<AnemoraSunCycleDriver>(FindObjectsInactive.Exclude);
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmo)
            {
                return;
            }

            var sunDirection = Quaternion.Euler(AnemoraSunCycleDriver.GetReferenceDirectionEuler(sunPreset)) * Vector3.forward;
            var start = transform.position;
            var end = start + (sunDirection.normalized * 5f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(start, end);
            Gizmos.DrawSphere(end, 0.2f);

#if UNITY_EDITOR
            Handles.color = Color.yellow;
            Handles.Label(end, sunPreset.ToString());
#endif
        }
    }
}
