using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Cutscene Camera Profile")]
    public sealed class FastVsHd2dCutsceneCameraProfile : ScriptableObject
    {
        [SerializeField] private string entryId = "side_view_auto_pan";
        [SerializeField] private string reviewRole = "ending side-view auto-pan";
        [SerializeField] private FastVsHouseArea area = FastVsHouseArea.Ruins;
        [SerializeField] private float durationSeconds = 6f;
        [SerializeField] private float fieldOfView = 26f;
        [SerializeField] private int inactivePriority = 5;
        [SerializeField] private int livePriority = 180;
        [SerializeField] private Vector3 startCameraLocal;
        [SerializeField] private Vector3 endCameraLocal;
        [SerializeField] private Vector3 startLookLocal;
        [SerializeField] private Vector3 endLookLocal;

        public string EntryIdForReview => entryId;
        public string ReviewRoleForReview => reviewRole;
        public FastVsHouseArea AreaForReview => area;
        public float DurationSecondsForReview => durationSeconds;
        public float FieldOfViewForReview => fieldOfView;
        public int InactivePriorityForReview => inactivePriority;
        public int LivePriorityForReview => livePriority;
        public Vector3 StartCameraLocalForReview => startCameraLocal;
        public Vector3 EndCameraLocalForReview => endCameraLocal;
        public Vector3 StartLookLocalForReview => startLookLocal;
        public Vector3 EndLookLocalForReview => endLookLocal;
        public float LateralDistanceForReview => Mathf.Abs(endCameraLocal.x - startCameraLocal.x);
        public float ConstantHeightForReview => startCameraLocal.y;

        public void ConfigureForReview(
            string configuredEntryId,
            string configuredReviewRole,
            FastVsHouseArea configuredArea,
            float configuredDurationSeconds,
            float configuredFieldOfView,
            int configuredInactivePriority,
            int configuredLivePriority,
            Vector3 configuredStartCameraLocal,
            Vector3 configuredEndCameraLocal,
            Vector3 configuredStartLookLocal,
            Vector3 configuredEndLookLocal)
        {
            entryId = configuredEntryId;
            reviewRole = configuredReviewRole;
            area = configuredArea;
            durationSeconds = Mathf.Max(0.1f, configuredDurationSeconds);
            fieldOfView = Mathf.Clamp(configuredFieldOfView, 10f, 75f);
            inactivePriority = configuredInactivePriority;
            livePriority = Mathf.Max(configuredLivePriority, configuredInactivePriority + 1);
            startCameraLocal = configuredStartCameraLocal;
            endCameraLocal = configuredEndCameraLocal;
            startLookLocal = configuredStartLookLocal;
            endLookLocal = configuredEndLookLocal;
        }

        public bool TryEvaluateFrameForReview(float normalizedTime, out CutsceneFrame frame)
        {
            frame = default;
            if (durationSeconds <= 0f)
            {
                return false;
            }

            var t = Mathf.Clamp01(normalizedTime);
            var cameraLocal = Vector3.Lerp(startCameraLocal, endCameraLocal, t);
            var lookLocal = Vector3.Lerp(startLookLocal, endLookLocal, t);
            var forward = lookLocal - cameraLocal;
            if (forward.sqrMagnitude < 0.0001f)
            {
                return false;
            }

            var normalizedForward = forward.normalized;
            var pitchDegrees = Mathf.Asin(Mathf.Clamp(normalizedForward.y, -1f, 1f)) * Mathf.Rad2Deg;
            var yawDegrees = Mathf.Atan2(normalizedForward.x, normalizedForward.z) * Mathf.Rad2Deg;
            var lateralSpeed = LateralDistanceForReview / Mathf.Max(0.1f, durationSeconds);
            frame = new CutsceneFrame(t, cameraLocal, lookLocal, fieldOfView, pitchDegrees, yawDegrees, lateralSpeed);
            return true;
        }

        public readonly struct CutsceneFrame
        {
            public readonly float NormalizedTime;
            public readonly Vector3 CameraLocalPosition;
            public readonly Vector3 LookLocalPosition;
            public readonly float FieldOfView;
            public readonly float PitchDegrees;
            public readonly float YawDegrees;
            public readonly float LateralSpeedUnitsPerSecond;

            public CutsceneFrame(
                float normalizedTime,
                Vector3 cameraLocalPosition,
                Vector3 lookLocalPosition,
                float fieldOfView,
                float pitchDegrees,
                float yawDegrees,
                float lateralSpeedUnitsPerSecond)
            {
                NormalizedTime = normalizedTime;
                CameraLocalPosition = cameraLocalPosition;
                LookLocalPosition = lookLocalPosition;
                FieldOfView = fieldOfView;
                PitchDegrees = pitchDegrees;
                YawDegrees = yawDegrees;
                LateralSpeedUnitsPerSecond = lateralSpeedUnitsPerSecond;
            }
        }
    }
}
