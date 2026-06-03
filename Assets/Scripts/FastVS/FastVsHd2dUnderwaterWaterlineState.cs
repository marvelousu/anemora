using UnityEngine;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Underwater Waterline State")]
    public sealed class FastVsHd2dUnderwaterWaterlineState : MonoBehaviour
    {
        private static readonly int UnderwaterBlendId = Shader.PropertyToID("_AnemoraHd2dUnderwaterBlend");
        private static readonly int UnderwaterTintId = Shader.PropertyToID("_AnemoraHd2dUnderwaterTint");
        private static readonly int FogDistortionId = Shader.PropertyToID("_AnemoraHd2dUnderwaterFogDistortion");
        private static readonly int WaterlineId = Shader.PropertyToID("_AnemoraHd2dUnderwaterWaterline");
        private static readonly int CausticsId = Shader.PropertyToID("_AnemoraHd2dUnderwaterCaustics");

        [SerializeField] private FastVsHd2dUnderwaterWaterlineProfile profile;
        [SerializeField] private Camera targetCamera;
        [SerializeField] private Transform referenceWaterPlane;
        [SerializeField] private bool runtimeDefaultDisabled = true;
        [SerializeField] private bool effectLockedUntilTomApproval = true;
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField, Range(0f, 1f)] private float activeBlend;
        [SerializeField] private float lastCameraWaterDelta;
        [SerializeField] private bool lastSubmerged;

        public bool IsReadyForReview => profile != null && targetCamera != null;
        public FastVsHd2dUnderwaterWaterlineProfile ProfileForReview => profile;
        public Camera TargetCameraForReview => targetCamera;
        public Transform ReferenceWaterPlaneForReview => referenceWaterPlane;
        public bool RuntimeDefaultDisabledForReview => runtimeDefaultDisabled;
        public bool EffectLockedUntilTomApprovalForReview => effectLockedUntilTomApproval;
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public float ActiveBlendForReview => activeBlend;
        public float LastCameraWaterDeltaForReview => lastCameraWaterDelta;
        public bool LastSubmergedForReview => lastSubmerged;
        public float WaterPlaneHeightForReview => referenceWaterPlane != null
            ? referenceWaterPlane.position.y
            : profile != null
                ? profile.ReferenceWaterPlaneHeightForReview
                : 0f;

        private void OnEnable()
        {
            PublishForReview();
        }

        private void OnValidate()
        {
            activeBlend = Mathf.Clamp01(activeBlend);
            PublishForReview();
        }

        private void LateUpdate()
        {
            if (publishEveryFrame)
            {
                PublishForReview();
            }
        }

        public void ConfigureForReview(
            FastVsHd2dUnderwaterWaterlineProfile underwaterProfile,
            Camera camera,
            Transform waterPlane)
        {
            profile = underwaterProfile;
            targetCamera = camera;
            referenceWaterPlane = waterPlane;
            runtimeDefaultDisabled = profile == null || profile.RuntimeDefaultDisabledForReview;
            effectLockedUntilTomApproval = profile == null || !profile.FinalUnderwaterWaterlineApprovedForReview;
            publishEveryFrame = true;
            activeBlend = 0f;
            lastCameraWaterDelta = 0f;
            lastSubmerged = false;
            PublishForReview();
        }

        public float CalculateBlendForReview(float cameraHeight, float waterPlaneHeight)
        {
            if (profile == null)
            {
                return 0f;
            }

            var threshold = profile.SubmergedActivationThresholdForReview;
            var band = Mathf.Max(0.001f, profile.TransitionBandForReview);
            var depthBelow = waterPlaneHeight - cameraHeight - threshold;
            return Mathf.Clamp01(depthBelow / band);
        }

        public void PublishForReview()
        {
            if (profile == null || targetCamera == null)
            {
                ClearGlobalsForReview();
                return;
            }

            var waterHeight = WaterPlaneHeightForReview;
            lastCameraWaterDelta = targetCamera.transform.position.y - waterHeight;
            lastSubmerged = CalculateBlendForReview(targetCamera.transform.position.y, waterHeight) > 0.001f;

            if (runtimeDefaultDisabled || effectLockedUntilTomApproval || !profile.FinalUnderwaterWaterlineApprovedForReview)
            {
                activeBlend = 0f;
                ClearGlobalsForReview();
                return;
            }

            activeBlend = CalculateBlendForReview(targetCamera.transform.position.y, waterHeight);
            PublishGlobalsForReview(
                activeBlend,
                profile.UnderwaterTintForReview,
                profile.ConservativeDepthFogDensityForReview,
                profile.ConservativeTintBlendForReview,
                profile.ConservativeDistortionPixelsForReview,
                profile.ConservativeFarDesaturationForReview,
                profile.NormalizedWaterlineYForReview,
                profile.WaterlineFeatherForReview,
                profile.SurfaceLineStrengthForReview,
                profile.EdgeVignetteStrengthForReview,
                profile.GodRayStrengthForReview,
                Time.realtimeSinceStartup * profile.DistortionScrollSpeedForReview);
        }

        public static void PublishGlobalsForReview(
            float blend,
            Color underwaterTint,
            float fogDensity,
            float tintBlend,
            float distortionPixels,
            float farDesaturation,
            float normalizedWaterlineY,
            float waterlineFeather,
            float surfaceLineStrength,
            float edgeVignetteStrength,
            float godRayStrength,
            float timeOffset)
        {
            Shader.SetGlobalFloat(UnderwaterBlendId, Mathf.Clamp01(blend));
            underwaterTint.a = 1f;
            Shader.SetGlobalColor(UnderwaterTintId, underwaterTint);
            Shader.SetGlobalVector(
                FogDistortionId,
                new Vector4(
                    Mathf.Clamp(fogDensity, 0f, 1f),
                    Mathf.Clamp01(tintBlend),
                    Mathf.Clamp(distortionPixels, 0f, 6f),
                    Mathf.Clamp01(farDesaturation)));
            Shader.SetGlobalVector(
                WaterlineId,
                new Vector4(
                    Mathf.Clamp01(normalizedWaterlineY),
                    Mathf.Clamp(waterlineFeather, 0.001f, 0.25f),
                    Mathf.Clamp01(surfaceLineStrength),
                    Mathf.Clamp01(edgeVignetteStrength)));
            Shader.SetGlobalVector(
                CausticsId,
                new Vector4(
                    Mathf.Clamp01(godRayStrength),
                    timeOffset,
                    0f,
                    0f));
        }

        public static void ClearGlobalsForReview()
        {
            Shader.SetGlobalFloat(UnderwaterBlendId, 0f);
            Shader.SetGlobalVector(FogDistortionId, Vector4.zero);
            Shader.SetGlobalVector(WaterlineId, Vector4.zero);
            Shader.SetGlobalVector(CausticsId, Vector4.zero);
        }
    }
}
