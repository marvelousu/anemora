using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsHd2dTownDressingMarker : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dTownDressingProfile profile;
        [SerializeField] private string dressingKind = string.Empty;
        [SerializeField] private string materialId = string.Empty;
        [SerializeField] private bool emissiveLantern;
        [SerializeField] private bool pointLightReady;
        [SerializeField] private bool spriteCardWindReady;
        [SerializeField] private bool inAcceptedReviewFrame;

        public FastVsHd2dTownDressingProfile ProfileForReview => profile;
        public string DressingKindForReview => dressingKind;
        public string MaterialIdForReview => materialId;
        public bool EmissiveLanternForReview => emissiveLantern;
        public bool PointLightReadyForReview => pointLightReady;
        public bool SpriteCardWindReadyForReview => spriteCardWindReady;
        public bool InAcceptedReviewFrameForReview => inAcceptedReviewFrame;

        public void ConfigureForReview(
            FastVsHd2dTownDressingProfile configuredProfile,
            string configuredDressingKind,
            string configuredMaterialId,
            bool configuredEmissiveLantern,
            bool configuredPointLightReady,
            bool configuredSpriteCardWindReady,
            bool configuredInAcceptedReviewFrame)
        {
            profile = configuredProfile;
            dressingKind = configuredDressingKind ?? string.Empty;
            materialId = configuredMaterialId ?? string.Empty;
            emissiveLantern = configuredEmissiveLantern;
            pointLightReady = configuredPointLightReady;
            spriteCardWindReady = configuredSpriteCardWindReady;
            inAcceptedReviewFrame = configuredInAcceptedReviewFrame;
        }
    }
}
