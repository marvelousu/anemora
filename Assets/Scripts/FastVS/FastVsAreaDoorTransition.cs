using System.Collections;
using Anemora.TimeManagement;
using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsAreaDoorTransition : MonoBehaviour
    {
        [SerializeField] private TimeWindowPairedSpacePortalController portalController;
        [SerializeField] private CharacterController playerController;
        [SerializeField] private Transform player;
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private FastVsStoryFlowController storyFlow;
        [SerializeField] private FastVsHouseArea sourceArea;
        [SerializeField] private FastVsHouseArea targetArea;
        [SerializeField] private Vector3 triggerLocalCenter;
        [SerializeField] private Vector3 triggerLocalSize = new Vector3(1.1f, 1.6f, 0.8f);
        [SerializeField] private Vector3 targetLocalPosition;
        [SerializeField] private string transitionLabel;
        [SerializeField] private float cooldownSeconds = 0.55f;
        [SerializeField] private float transitionFadeSeconds = 0.26f;
        [SerializeField] private float transitionHoldSeconds = 0.08f;
        [SerializeField] private Color transitionFadeColor = new Color(0.015f, 0.014f, 0.012f, 0.92f);

        private static float nextGlobalAllowedTime;
        private static int activeTransitionCount;
        private bool isTransitioning;
        private float transitionAlpha;

        public Vector3 TriggerLocalCenterForReview => triggerLocalCenter;
        public Vector3 TriggerLocalSizeForReview => triggerLocalSize;
        public Vector3 TargetLocalPositionForReview => targetLocalPosition;
        public FastVsHouseArea SourceAreaForReview => sourceArea;
        public FastVsHouseArea TargetAreaForReview => targetArea;
        public string TransitionLabelForReview => transitionLabel;
        public float TransitionFadeSecondsForReview => transitionFadeSeconds;
        public float TransitionHoldSecondsForReview => transitionHoldSeconds;
        public bool IsTransitioningForReview => isTransitioning;
        public bool StoryFlowWiredForReview => storyFlow != null;
        public static bool AnyTransitionInProgressForReview => activeTransitionCount > 0;

        public void TriggerForReview()
        {
            ResolveReferences();
            ExecuteTransition();
        }

        public bool TryEvaluateCurrentPlayerForReview()
        {
            ResolveReferences();
            if (!IsPlayerInsideTrigger())
            {
                return false;
            }

            return ExecuteTransition();
        }

        private void Update()
        {
            ResolveReferences();
            if (portalController == null ||
                player == null ||
                isTransitioning ||
                Time.time < nextGlobalAllowedTime ||
                !IsSourceAreaActive())
            {
                return;
            }

            if (storyFlow != null && storyFlow.TryBlockHouseExitForDoorBrushBeat(sourceArea, targetArea))
            {
                return;
            }

            if (!IsPlayerInsideTrigger())
            {
                return;
            }

            if (storyFlow != null && storyFlow.TryBlockHouseExitForDoorBrushBeat(sourceArea, targetArea, true))
            {
                return;
            }

            if (BeginRuntimeTransition())
            {
                var fadeTotal = Mathf.Max(0f, transitionFadeSeconds) * 2f + Mathf.Max(0f, transitionHoldSeconds);
                nextGlobalAllowedTime = Time.time + Mathf.Max(Mathf.Max(0.1f, cooldownSeconds), fadeTotal + 0.12f);
            }
        }

        private void OnGUI()
        {
            if (transitionAlpha <= 0.001f)
            {
                return;
            }

            var previous = GUI.color;
            GUI.color = new Color(
                transitionFadeColor.r,
                transitionFadeColor.g,
                transitionFadeColor.b,
                transitionFadeColor.a * Mathf.Clamp01(transitionAlpha));
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = previous;
        }

        private bool IsPlayerInsideTrigger()
        {
            if (portalController == null || player == null || !IsSourceAreaActive())
            {
                return false;
            }

            var root = portalController.PlayerInOtherTime
                ? portalController.OtherTimeSpaceRootForReview
                : portalController.CurrentSpaceRootForReview;
            if (root == null)
            {
                return false;
            }

            var local = root.InverseTransformPoint(player.position);
            return Contains(local, triggerLocalCenter, triggerLocalSize);
        }

        private bool ExecuteTransition()
        {
            if (!CloseOpenPortalForAreaTransitionIfNeeded())
            {
                return false;
            }

            if (areaVisibility != null)
            {
                if (Application.isPlaying)
                {
                    areaVisibility.SetActiveAreaWithLightingTransitionForReview(targetArea);
                }
                else
                {
                    areaVisibility.SetActiveAreaForReview(targetArea);
                }
            }

            if (portalController != null)
            {
                portalController.WarpPlayerToLocalForReview(targetLocalPosition, transitionLabel);
            }

            return true;
        }

        private bool IsSourceAreaActive()
        {
            return areaVisibility == null || areaVisibility.ActiveAreaForReview == sourceArea;
        }

        private bool BeginRuntimeTransition()
        {
            if (!CloseOpenPortalForAreaTransitionIfNeeded())
            {
                return false;
            }

            if (!Application.isPlaying || transitionFadeSeconds <= 0f && transitionHoldSeconds <= 0f)
            {
                ExecuteTransition();
                return true;
            }

            StartCoroutine(RunTransition());
            return true;
        }

        private IEnumerator RunTransition()
        {
            isTransitioning = true;
            activeTransitionCount++;
            try
            {
                yield return FadeTo(1f, Mathf.Max(0.01f, transitionFadeSeconds));
                ExecuteTransition();
                if (transitionHoldSeconds > 0f)
                {
                    yield return new WaitForSeconds(transitionHoldSeconds);
                }

                yield return FadeTo(0f, Mathf.Max(0.01f, transitionFadeSeconds));
            }
            finally
            {
                transitionAlpha = 0f;
                isTransitioning = false;
                activeTransitionCount = Mathf.Max(0, activeTransitionCount - 1);
            }
        }

        private IEnumerator FadeTo(float targetAlpha, float duration)
        {
            var start = transitionAlpha;
            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                transitionAlpha = Mathf.Lerp(start, targetAlpha, Mathf.Clamp01(elapsed / duration));
                yield return null;
            }

            transitionAlpha = targetAlpha;
        }

        private bool CloseOpenPortalForAreaTransitionIfNeeded()
        {
            if (portalController == null ||
                (!portalController.HasPortalPair && !portalController.HasPreviewPortal))
            {
                return true;
            }

            return portalController.TryClosePortalForAreaTransition();
        }

        private void ResolveReferences()
        {
            if (portalController == null)
            {
                portalController = FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            }

            if (playerController == null)
            {
                playerController = FindFirstObjectByType<CharacterController>();
            }

            if (player == null && playerController != null)
            {
                player = playerController.transform;
            }

            if (areaVisibility == null)
            {
                areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            }

            if (storyFlow == null)
            {
                storyFlow = FindFirstObjectByType<FastVsStoryFlowController>();
            }
        }

        private static bool Contains(Vector3 point, Vector3 center, Vector3 size)
        {
            var half = size * 0.5f;
            return Mathf.Abs(point.x - center.x) <= half.x &&
                   Mathf.Abs(point.y - center.y) <= half.y &&
                   Mathf.Abs(point.z - center.z) <= half.z;
        }
    }
}
