using UnityEngine;

namespace Anemora.FastVS
{
    /// <summary>
    /// Review-only HD-2D lighting guidance marker for a specific house-slice area.
    /// It keeps the current-world tuning targets visible in the scene hierarchy.
    /// </summary>
    public sealed class FastVsHd2dAreaLightingProfile : MonoBehaviour
    {
        [SerializeField] private FastVsHouseArea areaId;
        [SerializeField] private string areaName;
        [SerializeField] private bool interior;
        [SerializeField] private Vector2 targetAverageLuminanceBand;
        [SerializeField] private Vector3 keyLightEulerDegrees;
        [SerializeField] private float keyLightIntensity;
        [SerializeField] private Color keyLightTint = Color.white;
        [SerializeField] private float fillIntensity;
        [SerializeField] private Color fillTint = Color.white;
        [SerializeField] private float ambientIntensity;
        [SerializeField] private Color ambientTint = Color.white;

        public FastVsHouseArea AreaIdForReview => areaId;
        public string AreaNameForReview => areaName;
        public bool IsInteriorForReview => interior;
        public Vector2 TargetAverageLuminanceBandForReview => targetAverageLuminanceBand;
        public Vector3 KeyLightEulerDegreesForReview => keyLightEulerDegrees;
        public float KeyLightIntensityForReview => keyLightIntensity;
        public Color KeyLightTintForReview => keyLightTint;
        public float FillIntensityForReview => fillIntensity;
        public Color FillTintForReview => fillTint;
        public float AmbientIntensityForReview => ambientIntensity;
        public Color AmbientTintForReview => ambientTint;
    }
}
