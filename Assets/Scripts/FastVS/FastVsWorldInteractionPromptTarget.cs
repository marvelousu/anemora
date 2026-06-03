using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsWorldInteractionPromptKind
    {
        Examine,
        Talk,
        Enter,
    }

    public enum FastVsWorldInteractionPromptInputScheme
    {
        KeyboardMouse,
        Gamepad,
    }

    public sealed class FastVsWorldInteractionPromptTarget : MonoBehaviour
    {
        [SerializeField] private string promptId = "interaction";
        [SerializeField] private FastVsWorldInteractionPromptKind promptKind = FastVsWorldInteractionPromptKind.Examine;
        [SerializeField] private string promptLabel = "Examine";
        [SerializeField] private FastVsHouseArea activeArea = FastVsHouseArea.Interior;
        [SerializeField] private bool requireActiveArea = true;
        [SerializeField] private float interactionRange = 1.35f;
        [SerializeField] private Vector3 anchorLocalOffset = new Vector3(0f, 1.2f, 0f);

        public string PromptIdForReview => promptId;
        public FastVsWorldInteractionPromptKind PromptKindForReview => promptKind;
        public string PromptLabelForReview => promptLabel;
        public FastVsHouseArea ActiveAreaForReview => activeArea;
        public bool RequireActiveAreaForReview => requireActiveArea;
        public float InteractionRangeForReview => interactionRange;
        public Vector3 AnchorLocalOffsetForReview => anchorLocalOffset;
        public Vector3 AnchorWorldPositionForReview => transform.TransformPoint(anchorLocalOffset);

        public bool IsEligibleForReview(Vector3 playerWorldPosition, FastVsHouseArea currentArea)
        {
            if (!isActiveAndEnabled)
            {
                return false;
            }

            if (requireActiveArea && currentArea != activeArea)
            {
                return false;
            }

            var delta = playerWorldPosition - transform.position;
            delta.y = 0f;
            return delta.sqrMagnitude <= interactionRange * interactionRange;
        }
    }
}
