using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.FastVS
{
    public sealed class FastVsHd2dDepthOfFieldFocusController : MonoBehaviour
    {
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Transform focusTarget;
        [SerializeField] private Volume globalVolume;
        [SerializeField] private Vector3 focusTargetLocalOffset = new Vector3(0f, 0.72f, 0f);
        [SerializeField] private float minFocusDistance = 2.5f;
        [SerializeField] private float maxFocusDistance = 12f;
        [SerializeField] private float focusBias = 0f;
        [SerializeField] private float aperture = 2.4f;
        [SerializeField] private float focalLength = 85f;
        [SerializeField] private int bladeCount = 6;
        [SerializeField] private float bladeCurvature = 0.85f;
        [SerializeField] private float bladeRotation = 0f;

        private float focusDistanceForReview;

        public float FocusDistanceForReview => focusDistanceForReview;
        public float ApertureForReview => aperture;
        public float FocalLengthForReview => focalLength;
        public Transform FocusTargetForReview => focusTarget;
        public Volume VolumeForReview => globalVolume;

        private void LateUpdate()
        {
            ApplyNowForReview();
        }

        public void ConfigureForReview(Camera camera, Transform target, Volume volume)
        {
            sceneCamera = camera;
            focusTarget = target;
            globalVolume = volume;
            ApplyNowForReview();
        }

        public void ApplyNowForReview()
        {
            ResolveReferences();

            if (sceneCamera == null || focusTarget == null || globalVolume == null)
            {
                return;
            }

            var profile = globalVolume.profile;
            if (profile == null)
            {
                return;
            }

            if (!profile.TryGet<DepthOfField>(out var depthOfField) || depthOfField == null)
            {
                return;
            }

            var focusPoint = focusTarget.TransformPoint(focusTargetLocalOffset);
            var focusDistance = Mathf.Clamp(
                Vector3.Distance(sceneCamera.transform.position, focusPoint) + focusBias,
                Mathf.Min(minFocusDistance, maxFocusDistance),
                Mathf.Max(minFocusDistance, maxFocusDistance));

            depthOfField.active = true;
            depthOfField.mode.overrideState = true;
            depthOfField.mode.value = DepthOfFieldMode.Bokeh;
            depthOfField.focusDistance.overrideState = true;
            depthOfField.focusDistance.value = focusDistance;
            depthOfField.aperture.overrideState = true;
            depthOfField.aperture.value = aperture;
            depthOfField.focalLength.overrideState = true;
            depthOfField.focalLength.value = focalLength;
            depthOfField.bladeCount.overrideState = true;
            depthOfField.bladeCount.value = bladeCount;
            depthOfField.bladeCurvature.overrideState = true;
            depthOfField.bladeCurvature.value = bladeCurvature;
            depthOfField.bladeRotation.overrideState = true;
            depthOfField.bladeRotation.value = bladeRotation;

            focusDistanceForReview = focusDistance;
        }

        private void ResolveReferences()
        {
            if (sceneCamera == null)
            {
                sceneCamera = GetComponent<Camera>();
            }

            if (sceneCamera == null)
            {
                sceneCamera = Camera.main;
            }

            if (globalVolume == null)
            {
                globalVolume = FindGlobalVolume();
            }
        }

        private static Volume FindGlobalVolume()
        {
            var namedVolumeObject = GameObject.Find("FastVS_HD2D_GlobalVolume");
            if (namedVolumeObject != null && namedVolumeObject.TryGetComponent(out Volume namedVolume))
            {
                return namedVolume;
            }

            var volumes = FindObjectsByType<Volume>(FindObjectsSortMode.None);
            for (var i = 0; i < volumes.Length; i++)
            {
                var volume = volumes[i];
                if (volume != null && volume.isGlobal)
                {
                    return volume;
                }
            }

            return null;
        }
    }
}
