using UnityEngine;

namespace Anemora.TimeManagement
{
    public enum TimeWindowPairedSpaceLandmarkKind
    {
        WallOrLandmark,
        PropOrFeature,
        PathOrFloor
    }

    /// <summary>
    /// Review-only semantic marker used by TimeWindow paired-space prototypes.
    /// It distinguishes complete map content from landing-pad props.
    /// </summary>
    public sealed class TimeWindowPairedSpaceLandmark : MonoBehaviour
    {
        [SerializeField] private string landmarkId;
        [SerializeField] private TimeWindowPairedSpaceLandmarkKind kind;
        [SerializeField] private bool countsForArrival = true;

        public string LandmarkId => string.IsNullOrEmpty(landmarkId) ? name : landmarkId;
        public TimeWindowPairedSpaceLandmarkKind Kind => kind;
        public bool CountsForArrival => countsForArrival;

        public float DistanceTo(Vector3 worldPosition)
        {
            var collider = GetComponent<Collider>();
            if (collider != null)
            {
                return Vector3.Distance(worldPosition, collider.bounds.ClosestPoint(worldPosition));
            }

            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                return Vector3.Distance(worldPosition, renderer.bounds.ClosestPoint(worldPosition));
            }

            return Vector3.Distance(worldPosition, transform.position);
        }
    }
}
