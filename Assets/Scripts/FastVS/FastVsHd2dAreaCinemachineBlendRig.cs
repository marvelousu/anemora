using System;
using Anemora.TimeManagement;
using Unity.Cinemachine;
using UnityEngine;

namespace Anemora.FastVS
{
    [DisallowMultipleComponent]
    public sealed class FastVsHd2dAreaCinemachineBlendRig : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private TimeWindowPairedSpacePortalController portalController;
        [SerializeField] private CharacterController playerController;
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private FastVsHd2dAreaCameraBlendProfile profile;
        [SerializeField] private FastVsHd2dAreaCameraVolume[] volumes = Array.Empty<FastVsHd2dAreaCameraVolume>();
        [SerializeField] private FastVsHd2dAreaCameraVolume activeVolume;

        private FastVsHd2dAreaCameraBlendProfile.Entry activeEntry;
        private Vector3 currentPositionOffset;
        private Vector3 currentLookOffset;
        private float currentFieldOfView = 32f;
        private float currentDistance = 5.15f;
        private Vector3 blendStartPositionOffset;
        private Vector3 blendStartLookOffset;
        private float blendStartFieldOfView = 32f;
        private float blendStartDistance = 5.15f;
        private float blendElapsedSeconds;
        private bool hasBlendState;
        private bool hasForcedAreaForReview;
        private FastVsHouseArea forcedAreaForReview;

        public FastVsHd2dAreaCameraBlendProfile ProfileForReview => profile;
        public bool IsReadyForReview => profile != null && profile.EntryCountForReview > 0;
        public bool HasBlendStateForReview => hasBlendState;
        public string ActiveProfileEntryIdForReview => activeEntry != null ? activeEntry.EntryIdForReview : string.Empty;
        public FastVsHouseArea ActiveAreaForReview => activeEntry != null ? activeEntry.AreaForReview : FastVsHouseArea.Interior;
        public float PitchDegreesForReview => profile != null ? profile.PitchDegreesForReview : 29f;
        public float DefaultBlendSecondsForReview => profile != null ? profile.DefaultBlendSecondsForReview : 0.72f;
        public float CurrentFieldOfViewForReview => currentFieldOfView;
        public float CurrentDistanceForReview => currentDistance;
        public float TargetFieldOfViewForReview => activeEntry != null ? activeEntry.FieldOfViewForReview : currentFieldOfView;
        public float TargetDistanceForReview => activeEntry != null ? activeEntry.DistanceForReview : currentDistance;
        public int CameraCountForReview => volumes != null ? volumes.Length : 0;
        public FastVsHd2dAreaCameraVolume ActiveVolumeForReview => activeVolume;
        public FastVsHd2dAreaCameraVolume[] VolumesForReview => volumes ?? Array.Empty<FastVsHd2dAreaCameraVolume>();

        private void Awake()
        {
            ResolveReferences();
        }

        private void LateUpdate()
        {
            if (!IsReadyForReview)
            {
                return;
            }

            var root = ResolveActiveRoot();
            var anchor = root != null && portalController != null
                ? root.TransformPoint(portalController.GetPlayerLocalCoordinateForReview())
                : playerController != null
                    ? playerController.transform.position
                    : Vector3.zero;
            var area = areaVisibility != null ? areaVisibility.ActiveAreaForReview : FastVsHouseArea.CentralPlaza;
            UpdateRigForReview(anchor, root, area, Time.deltaTime, false);
        }

        public void ConfigureForReview(
            Camera camera,
            TimeWindowPairedSpacePortalController controller,
            CharacterController player,
            FastVsHouseAreaVisibility visibility,
            FastVsHd2dAreaCameraBlendProfile blendProfile,
            FastVsHd2dAreaCameraVolume[] cameraVolumes)
        {
            mainCamera = camera;
            portalController = controller;
            playerController = player;
            areaVisibility = visibility;
            profile = blendProfile;
            volumes = cameraVolumes ?? Array.Empty<FastVsHd2dAreaCameraVolume>();
            hasForcedAreaForReview = false;
            hasBlendState = false;
        }

        public void ForceAreaForReview(FastVsHouseArea area, bool immediate)
        {
            hasForcedAreaForReview = true;
            forcedAreaForReview = area;
            if (profile != null && profile.TryGetEntryForArea(area, out var entry))
            {
                SetTargetEntry(entry, immediate || !hasBlendState);
            }
        }

        public void ClearForcedAreaForReview()
        {
            hasForcedAreaForReview = false;
        }

        public bool UpdateRigForReview(Vector3 anchorWorld, Transform activeRoot, FastVsHouseArea currentArea, float deltaTime, bool immediate)
        {
            if (!IsReadyForReview)
            {
                return false;
            }

            ResolveReferences();
            var localPosition = ResolveLocalPlayerPosition(activeRoot);
            var targetEntry = ResolveTargetEntry(localPosition, currentArea);
            if (targetEntry == null)
            {
                return false;
            }

            SetTargetEntry(targetEntry, immediate || !hasBlendState);
            AdvanceBlend(deltaTime, immediate);
            ApplyVolumePriorities();
            ApplyCinemachineTransforms(anchorWorld);
            return true;
        }

        public bool TryGetBlendedFollowProfileForReview(out Vector3 positionOffset, out Vector3 lookOffset, out float fieldOfView)
        {
            positionOffset = currentPositionOffset;
            lookOffset = currentLookOffset;
            fieldOfView = currentFieldOfView;
            return hasBlendState;
        }

        private void ResolveReferences()
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            if (portalController == null)
            {
                portalController = FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            }

            if (playerController == null)
            {
                playerController = FindFirstObjectByType<CharacterController>();
            }

            if (areaVisibility == null)
            {
                areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            }

            if ((volumes == null || volumes.Length == 0) && profile != null)
            {
                volumes = GetComponentsInChildren<FastVsHd2dAreaCameraVolume>(true);
            }
        }

        private Transform ResolveActiveRoot()
        {
            if (portalController == null)
            {
                return null;
            }

            return portalController.PlayerInOtherTime
                ? portalController.OtherTimeSpaceRootForReview
                : portalController.CurrentSpaceRootForReview;
        }

        private Vector3 ResolveLocalPlayerPosition(Transform activeRoot)
        {
            if (portalController != null)
            {
                return portalController.GetPlayerLocalCoordinateForReview();
            }

            if (activeRoot != null && playerController != null)
            {
                return activeRoot.InverseTransformPoint(playerController.transform.position);
            }

            return Vector3.zero;
        }

        private FastVsHd2dAreaCameraBlendProfile.Entry ResolveTargetEntry(Vector3 localPosition, FastVsHouseArea currentArea)
        {
            var requestedArea = hasForcedAreaForReview ? forcedAreaForReview : currentArea;
            var exactEntry = profile.FindEntryForArea(requestedArea);
            if (exactEntry != null)
            {
                return exactEntry;
            }

            if (volumes != null)
            {
                for (var index = 0; index < volumes.Length; index++)
                {
                    var volume = volumes[index];
                    if (volume == null || !volume.ContainsLocalPositionForReview(localPosition))
                    {
                        continue;
                    }

                    var entry = profile.FindEntryById(volume.ProfileEntryIdForReview);
                    if (entry != null)
                    {
                        return entry;
                    }
                }
            }

            return profile.TryGetEntryForArea(FastVsHouseArea.CentralPlaza, out var fallback)
                ? fallback
                : null;
        }

        private void SetTargetEntry(FastVsHd2dAreaCameraBlendProfile.Entry targetEntry, bool immediate)
        {
            if (targetEntry == null)
            {
                return;
            }

            if (activeEntry == targetEntry && hasBlendState)
            {
                return;
            }

            var targetPositionOffset = targetEntry.PositionOffsetForPitch(PitchDegreesForReview);
            var targetLookOffset = targetEntry.LookOffsetForReview;

            if (!hasBlendState || immediate)
            {
                currentPositionOffset = targetPositionOffset;
                currentLookOffset = targetLookOffset;
                currentFieldOfView = targetEntry.FieldOfViewForReview;
                currentDistance = targetEntry.DistanceForReview;
                blendStartPositionOffset = currentPositionOffset;
                blendStartLookOffset = currentLookOffset;
                blendStartFieldOfView = currentFieldOfView;
                blendStartDistance = currentDistance;
                blendElapsedSeconds = DefaultBlendSecondsForReview;
                activeEntry = targetEntry;
                hasBlendState = true;
                return;
            }

            blendStartPositionOffset = currentPositionOffset;
            blendStartLookOffset = currentLookOffset;
            blendStartFieldOfView = currentFieldOfView;
            blendStartDistance = currentDistance;
            blendElapsedSeconds = 0f;
            activeEntry = targetEntry;
        }

        private void AdvanceBlend(float deltaTime, bool immediate)
        {
            if (activeEntry == null)
            {
                return;
            }

            var targetPositionOffset = activeEntry.PositionOffsetForPitch(PitchDegreesForReview);
            var targetLookOffset = activeEntry.LookOffsetForReview;
            if (immediate)
            {
                currentPositionOffset = targetPositionOffset;
                currentLookOffset = targetLookOffset;
                currentFieldOfView = activeEntry.FieldOfViewForReview;
                currentDistance = activeEntry.DistanceForReview;
                blendElapsedSeconds = DefaultBlendSecondsForReview;
                return;
            }

            var blendSeconds = Mathf.Max(0.01f, DefaultBlendSecondsForReview);
            blendElapsedSeconds = Mathf.Min(blendSeconds, blendElapsedSeconds + Mathf.Max(0f, deltaTime));
            var t = Mathf.SmoothStep(0f, 1f, blendElapsedSeconds / blendSeconds);
            currentPositionOffset = Vector3.Lerp(blendStartPositionOffset, targetPositionOffset, t);
            currentLookOffset = Vector3.Lerp(blendStartLookOffset, targetLookOffset, t);
            currentFieldOfView = Mathf.Lerp(blendStartFieldOfView, activeEntry.FieldOfViewForReview, t);
            currentDistance = Mathf.Lerp(blendStartDistance, activeEntry.DistanceForReview, t);
        }

        private void ApplyVolumePriorities()
        {
            activeVolume = null;
            if (volumes == null)
            {
                return;
            }

            for (var index = 0; index < volumes.Length; index++)
            {
                var volume = volumes[index];
                if (volume == null)
                {
                    continue;
                }

                var live = activeEntry != null && volume.ProfileEntryIdForReview == activeEntry.EntryIdForReview;
                volume.ApplyPriorityForReview(live);
                var appliedPriority = volume.CinemachineCameraForReview != null
                    ? volume.CinemachineCameraForReview.Priority.Value
                    : int.MinValue;
                if (live && appliedPriority >= 0)
                {
                    activeVolume = volume;
                }
            }
        }

        private void ApplyCinemachineTransforms(Vector3 anchorWorld)
        {
            if (volumes == null || profile == null)
            {
                return;
            }

            for (var index = 0; index < volumes.Length; index++)
            {
                var volume = volumes[index];
                if (volume == null)
                {
                    continue;
                }

                var entry = profile.FindEntryById(volume.ProfileEntryIdForReview);
                var camera = volume.CinemachineCameraForReview;
                if (entry == null || camera == null)
                {
                    continue;
                }

                var position = anchorWorld + entry.PositionOffsetForPitch(PitchDegreesForReview);
                var lookAt = anchorWorld + entry.LookOffsetForReview;
                camera.transform.SetPositionAndRotation(position, Quaternion.LookRotation(lookAt - position, Vector3.up));
                camera.Follow = null;
                camera.LookAt = null;
                var lens = camera.Lens;
                lens.FieldOfView = entry.FieldOfViewForReview;
                lens.NearClipPlane = 0.30f;
                lens.FarClipPlane = mainCamera != null ? mainCamera.farClipPlane : 140f;
                camera.Lens = lens;
            }
        }
    }
}
