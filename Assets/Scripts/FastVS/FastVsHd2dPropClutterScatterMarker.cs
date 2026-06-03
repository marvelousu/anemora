using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dPropClutterScatterType
    {
        Pot = 0,
        Sack = 1,
        Firewood = 2,
        HangingLaundry = 3,
        Weed = 4,
        Puddle = 5,
        FallenLeaf = 6,
        CornerCrate = 7,
        Barrel = 8,
    }

    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Prop Clutter Scatter Marker")]
    public sealed class FastVsHd2dPropClutterScatterMarker : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dPropClutterScatterProfile profile;
        [SerializeField] private FastVsHd2dPropClutterScatterType clutterType;
        [SerializeField] private string placementGroup = string.Empty;
        [SerializeField, Min(0f)] private float seamBreakMeters;
        [SerializeField] private bool inFocalBand = true;
        [SerializeField] private bool bareCornerFill;
        [SerializeField] private bool staticBatchingReady;
        [SerializeField] private bool gpuInstancingReady;
        [SerializeField, Min(0f)] private float cullDistanceMeters = 36f;
        [SerializeField] private bool conservativeNeedsTomApproval = true;

        public FastVsHd2dPropClutterScatterProfile ProfileForReview => profile;
        public FastVsHd2dPropClutterScatterType ClutterTypeForReview => clutterType;
        public string PlacementGroupForReview => placementGroup ?? string.Empty;
        public float SeamBreakMetersForReview => Mathf.Max(0f, seamBreakMeters);
        public bool InFocalBandForReview => inFocalBand;
        public bool BareCornerFillForReview => bareCornerFill;
        public bool StaticBatchingReadyForReview => staticBatchingReady;
        public bool GpuInstancingReadyForReview => gpuInstancingReady;
        public float CullDistanceMetersForReview => Mathf.Max(0f, cullDistanceMeters);
        public bool ConservativeNeedsTomApprovalForReview => conservativeNeedsTomApproval;

        public void ConfigureForReview(
            FastVsHd2dPropClutterScatterProfile configuredProfile,
            FastVsHd2dPropClutterScatterType configuredClutterType,
            string configuredPlacementGroup,
            float configuredSeamBreakMeters,
            bool configuredInFocalBand,
            bool configuredBareCornerFill,
            bool configuredStaticBatchingReady,
            bool configuredGpuInstancingReady,
            float configuredCullDistanceMeters,
            bool configuredConservativeNeedsTomApproval)
        {
            profile = configuredProfile;
            clutterType = configuredClutterType;
            placementGroup = configuredPlacementGroup ?? string.Empty;
            seamBreakMeters = Mathf.Max(0f, configuredSeamBreakMeters);
            inFocalBand = configuredInFocalBand;
            bareCornerFill = configuredBareCornerFill;
            staticBatchingReady = configuredStaticBatchingReady;
            gpuInstancingReady = configuredGpuInstancingReady;
            cullDistanceMeters = Mathf.Max(0f, configuredCullDistanceMeters);
            conservativeNeedsTomApproval = configuredConservativeNeedsTomApproval;
        }
    }
}
