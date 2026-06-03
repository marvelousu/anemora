using Anemora.TimeManagement;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Splines;

namespace Anemora.FastVS
{
    public sealed class FastVsHd2dCutsceneCameraDirector : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dCutsceneCameraProfile profile;
        [SerializeField] private Camera reviewCamera;
        [SerializeField] private TimeWindowPairedSpacePortalController portalController;
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private FastVsVisualDirectionGuide visualGuide;
        [SerializeField] private FastVsStoryRuntimeHud storyHud;
        [SerializeField] private CinemachineCamera cutsceneCamera;
        [SerializeField] private CinemachineSplineDolly splineDolly;
        [SerializeField] private SplineContainer splineContainer;
        [SerializeField] private PlayableDirector playableDirector;

        public bool IsReadyForReview =>
            profile != null &&
            cutsceneCamera != null &&
            splineDolly != null &&
            splineContainer != null &&
            playableDirector != null;

        public FastVsHd2dCutsceneCameraProfile ProfileForReview => profile;
        public CinemachineCamera CutsceneCameraForReview => cutsceneCamera;
        public CinemachineSplineDolly SplineDollyForReview => splineDolly;
        public SplineContainer SplineContainerForReview => splineContainer;
        public PlayableDirector PlayableDirectorForReview => playableDirector;
        public FastVsHd2dCutsceneCameraProfile.CutsceneFrame LastFrameForReview { get; private set; }
        public bool LastLiveForReview { get; private set; }
        public bool AutomaticDollyDisabledForReview => splineDolly != null && !splineDolly.AutomaticDolly.Enabled;
        public bool DampingDisabledForReview => splineDolly != null && !splineDolly.Damping.Enabled;
        public float DollyCameraPositionForReview => splineDolly != null ? splineDolly.CameraPosition : -1f;
        public int CutsceneCameraPriorityForReview => cutsceneCamera != null ? cutsceneCamera.Priority.Value : int.MinValue;
        public double TimelineDurationForReview => playableDirector != null && playableDirector.playableAsset != null ? playableDirector.playableAsset.duration : 0.0;
        public bool MovementFrozenForReview => visualGuide != null && visualGuide.MovementFrozenForReview;
        public bool HudSuppressedForReview => storyHud != null && !storyHud.gameObject.activeSelf;

        public void ConfigureForReview(
            FastVsHd2dCutsceneCameraProfile cutsceneProfile,
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsStoryRuntimeHud hud,
            CinemachineCamera authoredCutsceneCamera,
            CinemachineSplineDolly authoredSplineDolly,
            SplineContainer authoredSplineContainer,
            PlayableDirector director)
        {
            profile = cutsceneProfile;
            reviewCamera = camera;
            portalController = controller;
            areaVisibility = visibility;
            visualGuide = guide;
            storyHud = hud;
            cutsceneCamera = authoredCutsceneCamera;
            splineDolly = authoredSplineDolly;
            splineContainer = authoredSplineContainer;
            playableDirector = director;
            ApplyCutscenePriorityForReview(false);
            SetHudSuppressed(false);
        }

        public void RaiseCutscenePriorityForTimelineSignal()
        {
            ApplyCutscenePriorityForReview(true);
            SetCutsceneControlState(true);
        }

        public void LowerCutscenePriorityForTimelineSignal()
        {
            RestoreGameplayForReview();
        }

        public void ApplyCutscenePriorityForReview(bool live)
        {
            if (cutsceneCamera == null || profile == null)
            {
                return;
            }

            cutsceneCamera.Priority.Value = live ? profile.LivePriorityForReview : profile.InactivePriorityForReview;
        }

        public bool EvaluateSideViewForReview(
            float normalizedTime,
            bool live,
            out FastVsHd2dCutsceneCameraProfile.CutsceneFrame frame)
        {
            frame = default;
            if (!IsReadyForReview || !profile.TryEvaluateFrameForReview(normalizedTime, out frame))
            {
                return false;
            }

            LastFrameForReview = frame;
            if (areaVisibility != null)
            {
                areaVisibility.SetActiveAreaForReview(profile.AreaForReview);
            }

            ApplySplineDollyState(frame.NormalizedTime);

            if (playableDirector != null)
            {
                playableDirector.time = frame.NormalizedTime * profile.DurationSecondsForReview;
                playableDirector.Evaluate();
            }

            LastLiveForReview = live;
            SetCutsceneControlState(live);
            ApplyCutscenePriorityForReview(live);
            ApplyCameraState(frame);
            return true;
        }

        public void RestoreGameplayForReview()
        {
            ApplyCutscenePriorityForReview(false);
            SetCutsceneControlState(false);
            LastLiveForReview = false;
        }

        private void ApplySplineDollyState(float normalizedTime)
        {
            if (splineDolly == null)
            {
                return;
            }

            splineDolly.AutomaticDolly.Enabled = false;
            splineDolly.Damping.Enabled = false;
            splineDolly.PositionUnits = PathIndexUnit.Normalized;
            splineDolly.CameraPosition = Mathf.Clamp01(normalizedTime);
        }

        private void ApplyCameraState(FastVsHd2dCutsceneCameraProfile.CutsceneFrame frame)
        {
            var root = portalController != null ? portalController.CurrentSpaceRootForReview : null;
            var position = root != null ? root.TransformPoint(frame.CameraLocalPosition) : frame.CameraLocalPosition;
            var lookAt = root != null ? root.TransformPoint(frame.LookLocalPosition) : frame.LookLocalPosition;
            var rotation = Quaternion.LookRotation(lookAt - position, Vector3.up);
            if (cutsceneCamera != null)
            {
                cutsceneCamera.transform.SetPositionAndRotation(position, rotation);
                cutsceneCamera.Follow = null;
                cutsceneCamera.LookAt = null;
                var lens = cutsceneCamera.Lens;
                lens.FieldOfView = frame.FieldOfView;
                lens.NearClipPlane = 0.03f;
                lens.FarClipPlane = reviewCamera != null ? reviewCamera.farClipPlane : 140f;
                cutsceneCamera.Lens = lens;
            }

            if (reviewCamera != null)
            {
                reviewCamera.orthographic = false;
                reviewCamera.fieldOfView = frame.FieldOfView;
                reviewCamera.nearClipPlane = 0.03f;
                reviewCamera.transform.SetPositionAndRotation(position, rotation);
            }
        }

        private void SetCutsceneControlState(bool cutsceneLive)
        {
            if (visualGuide != null)
            {
                visualGuide.SetMovementFrozen(cutsceneLive);
                visualGuide.ApplyActiveTimeIsolationForReview();
            }

            SetHudSuppressed(cutsceneLive);
        }

        private void SetHudSuppressed(bool suppressed)
        {
            if (storyHud == null)
            {
                return;
            }

            storyHud.gameObject.SetActive(!suppressed);
        }
    }
}
