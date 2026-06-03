using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Anemora.FastVS
{
    [DisallowMultipleComponent]
    public sealed class FastVsHd2dGroupTargetFramingPreview : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dGroupTargetFramingProfile profile;
        [SerializeField] private Camera reviewCamera;
        [SerializeField] private CinemachineCamera groupCamera;
        [SerializeField] private CinemachineTargetGroup targetGroup;
        [SerializeField] private CinemachineGroupFraming groupFraming;
        [SerializeField] private CinemachineRecomposer recomposer;
        [SerializeField] private Transform[] participantTargets = Array.Empty<Transform>();

        private readonly List<CinemachineTargetGroup.Target> groupTargets = new List<CinemachineTargetGroup.Target>(8);

        public bool IsReadyForReview =>
            profile != null &&
            groupCamera != null &&
            targetGroup != null &&
            groupFraming != null &&
            recomposer != null &&
            participantTargets != null &&
            participantTargets.Length >= 7;

        public FastVsHd2dGroupTargetFramingProfile ProfileForReview => profile;
        public CinemachineCamera GroupCameraForReview => groupCamera;
        public CinemachineTargetGroup TargetGroupForReview => targetGroup;
        public CinemachineGroupFraming GroupFramingForReview => groupFraming;
        public CinemachineRecomposer RecomposerForReview => recomposer;
        public int ParticipantCapacityForReview => participantTargets != null ? participantTargets.Length : 0;
        public int TargetGroupMemberCountForReview => targetGroup != null && targetGroup.Targets != null ? targetGroup.Targets.Count : 0;
        public bool DirectRuntimeCameraAuthorityDisabledForReview => profile != null && profile.DirectRuntimeCameraAuthorityDisabledForReview;

        public void ConfigureForReview(
            FastVsHd2dGroupTargetFramingProfile framingProfile,
            Camera camera,
            CinemachineCamera cinemachineCamera,
            CinemachineTargetGroup cinemachineTargetGroup,
            CinemachineGroupFraming cinemachineGroupFraming,
            CinemachineRecomposer cinemachineRecomposer,
            Transform[] participants)
        {
            profile = framingProfile;
            reviewCamera = camera;
            groupCamera = cinemachineCamera;
            targetGroup = cinemachineTargetGroup;
            groupFraming = cinemachineGroupFraming;
            recomposer = cinemachineRecomposer;
            participantTargets = participants ?? Array.Empty<Transform>();
            ApplyInactiveCinemachineContractForReview();
        }

        public bool ApplyScenarioForReview(
            string scenarioId,
            Transform activeRoot,
            Vector3[] localBodyCenters,
            bool[] enemyFlags,
            int speakerIndex,
            bool speakerOnRight,
            bool dialogue,
            Camera cameraOverride,
            out FrameResult result)
        {
            result = default;
            if (!IsReadyForReview || localBodyCenters == null || localBodyCenters.Length == 0)
            {
                return false;
            }

            var camera = cameraOverride != null ? cameraOverride : reviewCamera;
            if (camera == null)
            {
                return false;
            }

            var count = Mathf.Min(localBodyCenters.Length, participantTargets.Length);
            ConfigureTargetsForReview(activeRoot, localBodyCenters, enemyFlags, speakerIndex, dialogue, count);
            ApplyCinemachineModulesForReview(dialogue, speakerOnRight);

            var fieldOfView = profile.BaseFieldOfViewForReview;
            var distance = CalculateInitialDistance(camera.aspect, localBodyCenters, count, fieldOfView);
            var localBounds = CalculateLocalBounds(localBodyCenters, count);
            var lookLocal = CalculateLookLocal(localBodyCenters, count, localBounds, speakerIndex, speakerOnRight, dialogue);
            ViewportBounds viewport = default;

            for (var attempt = 0; attempt < 8; attempt++)
            {
                ApplyCameraStateForReview(activeRoot, camera, lookLocal, distance, fieldOfView);
                OrientParticipantCardsForReview(camera);
                viewport = MeasureViewportBounds(camera, activeRoot, localBodyCenters, count);
                if (viewport.AllInsideSafeMargin(profile.ViewportSafeMarginForReview))
                {
                    break;
                }

                if (distance < profile.MaxDistanceForReview - 0.01f)
                {
                    distance = Mathf.Min(profile.MaxDistanceForReview, distance * 1.10f + 0.16f);
                }
                else
                {
                    fieldOfView = Mathf.Min(profile.MaxFieldOfViewForReview, fieldOfView + 1.25f);
                }
            }

            ApplyGroupCameraStateForReview(activeRoot, lookLocal, distance, fieldOfView);
            var speakerViewportX = speakerIndex >= 0 && speakerIndex < count
                ? camera.WorldToViewportPoint(activeRoot != null ? activeRoot.TransformPoint(localBodyCenters[speakerIndex]) : localBodyCenters[speakerIndex]).x
                : -1f;

            result = new FrameResult(
                scenarioId,
                count,
                dialogue,
                speakerIndex,
                speakerViewportX,
                distance,
                fieldOfView,
                localBounds.center,
                localBounds.size,
                viewport.Min,
                viewport.Max,
                viewport.AllInsideSafeMargin(profile.ViewportSafeMarginForReview));
            return true;
        }

        public void SetAllParticipantsVisibleForReview(bool visible)
        {
            if (participantTargets == null)
            {
                return;
            }

            for (var index = 0; index < participantTargets.Length; index++)
            {
                if (participantTargets[index] != null)
                {
                    participantTargets[index].gameObject.SetActive(visible);
                }
            }
        }

        private void ApplyInactiveCinemachineContractForReview()
        {
            if (profile == null || groupCamera == null)
            {
                return;
            }

            groupCamera.Follow = targetGroup != null ? targetGroup.transform : null;
            groupCamera.LookAt = targetGroup != null ? targetGroup.transform : null;
            groupCamera.Priority.Value = profile.InactivePriorityForReview;
            var lens = groupCamera.Lens;
            lens.FieldOfView = profile.BaseFieldOfViewForReview;
            lens.NearClipPlane = 0.03f;
            lens.FarClipPlane = reviewCamera != null ? reviewCamera.farClipPlane : 140f;
            groupCamera.Lens = lens;
        }

        private void ApplyCinemachineModulesForReview(bool dialogue, bool speakerOnRight)
        {
            if (profile == null)
            {
                return;
            }

            if (targetGroup != null)
            {
                targetGroup.PositionMode = CinemachineTargetGroup.PositionModes.GroupCenter;
                targetGroup.RotationMode = CinemachineTargetGroup.RotationModes.Manual;
                targetGroup.UpdateMethod = CinemachineTargetGroup.UpdateMethods.LateUpdate;
            }

            if (groupFraming != null)
            {
                groupFraming.FramingMode = CinemachineGroupFraming.FramingModes.HorizontalAndVertical;
                groupFraming.FramingSize = profile.GroupFramingSizeForReview;
                groupFraming.CenterOffset = dialogue
                    ? new Vector2(speakerOnRight ? profile.DialogueScreenOffsetForReview : -profile.DialogueScreenOffsetForReview, profile.DialogueHeadroomForReview * 0.15f)
                    : Vector2.zero;
                groupFraming.Damping = profile.GroupDampingSecondsForReview;
                groupFraming.SizeAdjustment = CinemachineGroupFraming.SizeAdjustmentModes.DollyThenZoom;
                groupFraming.LateralAdjustment = CinemachineGroupFraming.LateralAdjustmentModes.ChangePosition;
                groupFraming.FovRange = new Vector2(profile.BaseFieldOfViewForReview, profile.MaxFieldOfViewForReview);
                groupFraming.DollyRange = new Vector2(-0.25f, profile.MaxDistanceForReview - profile.BaseDistanceForReview);
                groupFraming.OrthoSizeRange = new Vector2(1f, 12f);
            }

            if (recomposer != null)
            {
                recomposer.ApplyAfter = CinemachineCore.Stage.Finalize;
                recomposer.Tilt = dialogue ? -profile.DialogueHeadroomForReview * 2.4f : 0f;
                recomposer.Pan = dialogue ? (speakerOnRight ? -1f : 1f) * profile.DialogueScreenOffsetForReview * 5.5f : 0f;
                recomposer.Dutch = 0f;
                recomposer.ZoomScale = 1f;
                recomposer.FollowAttachment = 1f;
                recomposer.LookAtAttachment = 1f;
            }
        }

        private void ConfigureTargetsForReview(
            Transform activeRoot,
            Vector3[] localBodyCenters,
            bool[] enemyFlags,
            int speakerIndex,
            bool dialogue,
            int count)
        {
            groupTargets.Clear();
            for (var index = 0; index < participantTargets.Length; index++)
            {
                var target = participantTargets[index];
                if (target == null)
                {
                    continue;
                }

                var active = index < count;
                target.gameObject.SetActive(active);
                if (!active)
                {
                    continue;
                }

                target.position = activeRoot != null ? activeRoot.TransformPoint(localBodyCenters[index]) : localBodyCenters[index];
                var weight = ResolveWeightForReview(index, enemyFlags, speakerIndex, dialogue);
                groupTargets.Add(new CinemachineTargetGroup.Target
                {
                    Object = target,
                    Weight = weight,
                    Radius = profile.ActorRadiusForReview
                });
            }

            if (targetGroup != null)
            {
                targetGroup.Targets.Clear();
                targetGroup.Targets.AddRange(groupTargets);
                targetGroup.transform.position = CalculateWorldCenter(activeRoot, localBodyCenters, count);
            }
        }

        private float ResolveWeightForReview(int index, bool[] enemyFlags, int speakerIndex, bool dialogue)
        {
            if (dialogue)
            {
                return index == speakerIndex ? profile.SpeakerWeightForReview : profile.ListenerWeightForReview;
            }

            var enemy = enemyFlags != null && index < enemyFlags.Length && enemyFlags[index];
            return enemy ? profile.EnemyWeightForReview : profile.AllyWeightForReview;
        }

        private Vector3 CalculateWorldCenter(Transform activeRoot, Vector3[] localBodyCenters, int count)
        {
            var center = Vector3.zero;
            for (var index = 0; index < count; index++)
            {
                center += localBodyCenters[index];
            }

            center /= Mathf.Max(1, count);
            return activeRoot != null ? activeRoot.TransformPoint(center) : center;
        }

        private float CalculateInitialDistance(float aspect, Vector3[] localBodyCenters, int count, float fieldOfView)
        {
            var localBounds = CalculateLocalBounds(localBodyCenters, count);
            var verticalTan = Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad);
            var horizontalTan = verticalTan * Mathf.Max(0.01f, aspect);
            var widthDemand = Mathf.Max(0.01f, localBounds.size.x + profile.ActorRadiusForReview * 2f);
            var verticalDemand = Mathf.Max(
                profile.ActorVisualHeightForReview,
                profile.ActorVisualHeightForReview + localBounds.size.z * profile.DepthFitWeightForReview);
            var framing = Mathf.Max(0.1f, profile.GroupFramingSizeForReview);
            var distanceByWidth = widthDemand * 0.5f / Mathf.Max(0.01f, horizontalTan * framing);
            var distanceByHeight = verticalDemand * 0.5f / Mathf.Max(0.01f, verticalTan * framing);
            return Mathf.Clamp(
                Mathf.Max(profile.BaseDistanceForReview, distanceByWidth, distanceByHeight) + profile.DistancePaddingForReview,
                profile.BaseDistanceForReview,
                profile.MaxDistanceForReview);
        }

        private Bounds CalculateLocalBounds(Vector3[] localBodyCenters, int count)
        {
            var halfHeight = profile.ActorVisualHeightForReview * 0.5f;
            var bounds = new Bounds(localBodyCenters[0], new Vector3(profile.ActorRadiusForReview * 2f, profile.ActorVisualHeightForReview, profile.ActorRadiusForReview * 2f));
            for (var index = 0; index < count; index++)
            {
                var center = localBodyCenters[index];
                bounds.Encapsulate(center + new Vector3(profile.ActorRadiusForReview, halfHeight, profile.ActorRadiusForReview));
                bounds.Encapsulate(center + new Vector3(-profile.ActorRadiusForReview, -halfHeight, -profile.ActorRadiusForReview));
            }

            return bounds;
        }

        private Vector3 CalculateLookLocal(
            Vector3[] localBodyCenters,
            int count,
            Bounds localBounds,
            int speakerIndex,
            bool speakerOnRight,
            bool dialogue)
        {
            var lookLocal = localBounds.center;
            if (dialogue && speakerIndex >= 0 && speakerIndex < count)
            {
                var speaker = localBodyCenters[speakerIndex];
                lookLocal = Vector3.Lerp(localBounds.center, speaker, 0.35f);
                lookLocal.x += (speakerOnRight ? -1f : 1f) * profile.DialogueScreenOffsetForReview * Mathf.Max(1f, localBounds.size.x);
                lookLocal.y += profile.DialogueHeadroomForReview;
            }

            lookLocal.y = Mathf.Max(0.72f, lookLocal.y);
            return lookLocal;
        }

        private void ApplyCameraStateForReview(Transform activeRoot, Camera camera, Vector3 lookLocal, float distance, float fieldOfView)
        {
            var pitchRadians = profile.PitchDegreesForReview * Mathf.Deg2Rad;
            var localOffset = new Vector3(0f, Mathf.Tan(pitchRadians) * (distance + 0.45f), -distance);
            var position = activeRoot != null ? activeRoot.TransformPoint(lookLocal + localOffset) : lookLocal + localOffset;
            var lookAt = activeRoot != null ? activeRoot.TransformPoint(lookLocal) : lookLocal;
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
        }

        private void ApplyGroupCameraStateForReview(Transform activeRoot, Vector3 lookLocal, float distance, float fieldOfView)
        {
            if (groupCamera == null)
            {
                return;
            }

            var pitchRadians = profile.PitchDegreesForReview * Mathf.Deg2Rad;
            var localOffset = new Vector3(0f, Mathf.Tan(pitchRadians) * (distance + 0.45f), -distance);
            var position = activeRoot != null ? activeRoot.TransformPoint(lookLocal + localOffset) : lookLocal + localOffset;
            var lookAt = activeRoot != null ? activeRoot.TransformPoint(lookLocal) : lookLocal;
            groupCamera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
            var lens = groupCamera.Lens;
            lens.FieldOfView = fieldOfView;
            lens.NearClipPlane = 0.03f;
            lens.FarClipPlane = reviewCamera != null ? reviewCamera.farClipPlane : 140f;
            groupCamera.Lens = lens;
            groupCamera.Priority.Value = profile.InactivePriorityForReview;
        }

        private void OrientParticipantCardsForReview(Camera camera)
        {
            if (participantTargets == null || camera == null)
            {
                return;
            }

            var direction = -camera.transform.forward;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                return;
            }

            var rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            for (var index = 0; index < participantTargets.Length; index++)
            {
                var participant = participantTargets[index];
                if (participant != null && participant.gameObject.activeSelf)
                {
                    participant.rotation = rotation;
                }
            }
        }

        private ViewportBounds MeasureViewportBounds(Camera camera, Transform activeRoot, Vector3[] localBodyCenters, int count)
        {
            var min = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
            var max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);
            var allInFront = true;
            var halfWidth = profile.ActorRadiusForReview;
            var halfHeight = profile.ActorVisualHeightForReview * 0.5f;
            for (var index = 0; index < count; index++)
            {
                var center = activeRoot != null ? activeRoot.TransformPoint(localBodyCenters[index]) : localBodyCenters[index];
                var right = camera.transform.right * halfWidth;
                var up = Vector3.up * halfHeight;
                MeasurePoint(camera, center, ref min, ref max, ref allInFront);
                MeasurePoint(camera, center + right, ref min, ref max, ref allInFront);
                MeasurePoint(camera, center - right, ref min, ref max, ref allInFront);
                MeasurePoint(camera, center + up, ref min, ref max, ref allInFront);
                MeasurePoint(camera, center - up, ref min, ref max, ref allInFront);
            }

            if (float.IsInfinity(min.x))
            {
                min = Vector2.zero;
                max = Vector2.zero;
                allInFront = false;
            }

            return new ViewportBounds(min, max, allInFront);
        }

        private static void MeasurePoint(Camera camera, Vector3 world, ref Vector2 min, ref Vector2 max, ref bool allInFront)
        {
            var viewport = camera.WorldToViewportPoint(world);
            if (viewport.z <= 0f)
            {
                allInFront = false;
            }

            min.x = Mathf.Min(min.x, viewport.x);
            min.y = Mathf.Min(min.y, viewport.y);
            max.x = Mathf.Max(max.x, viewport.x);
            max.y = Mathf.Max(max.y, viewport.y);
        }

        private readonly struct ViewportBounds
        {
            public readonly Vector2 Min;
            public readonly Vector2 Max;
            private readonly bool allInFront;

            public ViewportBounds(Vector2 min, Vector2 max, bool allInFront)
            {
                Min = min;
                Max = max;
                this.allInFront = allInFront;
            }

            public bool AllInsideSafeMargin(float margin)
            {
                return allInFront &&
                       Min.x >= margin &&
                       Min.y >= margin &&
                       Max.x <= 1f - margin &&
                       Max.y <= 1f - margin;
            }
        }

        public readonly struct FrameResult
        {
            public readonly string ScenarioId;
            public readonly int ActiveParticipantCount;
            public readonly bool Dialogue;
            public readonly int SpeakerIndex;
            public readonly float SpeakerViewportX;
            public readonly float CameraDistance;
            public readonly float FieldOfView;
            public readonly Vector3 GroupCenterLocal;
            public readonly Vector3 GroupSizeLocal;
            public readonly Vector2 ViewportMin;
            public readonly Vector2 ViewportMax;
            public readonly bool AllActorsInsideSafeMargin;

            public FrameResult(
                string scenarioId,
                int activeParticipantCount,
                bool dialogue,
                int speakerIndex,
                float speakerViewportX,
                float cameraDistance,
                float fieldOfView,
                Vector3 groupCenterLocal,
                Vector3 groupSizeLocal,
                Vector2 viewportMin,
                Vector2 viewportMax,
                bool allActorsInsideSafeMargin)
            {
                ScenarioId = scenarioId ?? string.Empty;
                ActiveParticipantCount = activeParticipantCount;
                Dialogue = dialogue;
                SpeakerIndex = speakerIndex;
                SpeakerViewportX = speakerViewportX;
                CameraDistance = cameraDistance;
                FieldOfView = fieldOfView;
                GroupCenterLocal = groupCenterLocal;
                GroupSizeLocal = groupSizeLocal;
                ViewportMin = viewportMin;
                ViewportMax = viewportMax;
                AllActorsInsideSafeMargin = allActorsInsideSafeMargin;
            }

            public string ToReportRow(string screenshot)
            {
                return $"| `{screenshot}` | {ScenarioId} | {ActiveParticipantCount} | {FormatBool(AllActorsInsideSafeMargin)} | {CameraDistance:0.###} | {FieldOfView:0.##} | ({ViewportMin.x:0.###},{ViewportMin.y:0.###}) | ({ViewportMax.x:0.###},{ViewportMax.y:0.###}) | {SpeakerViewportX:0.###} |";
            }

            private static string FormatBool(bool value)
            {
                return value ? "yes" : "no";
            }
        }
    }
}
