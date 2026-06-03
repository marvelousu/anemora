using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public sealed class FastVsHd2dGradientSkyDriver : MonoBehaviour
    {
        private static readonly int DayHorizonId = Shader.PropertyToID("_DayHorizon");
        private static readonly int DayZenithId = Shader.PropertyToID("_DayZenith");
        private static readonly int SunsetHorizonId = Shader.PropertyToID("_SunsetHorizon");
        private static readonly int SunsetZenithId = Shader.PropertyToID("_SunsetZenith");
        private static readonly int NightHorizonId = Shader.PropertyToID("_NightHorizon");
        private static readonly int NightZenithId = Shader.PropertyToID("_NightZenith");
        private static readonly int SunDiscColorId = Shader.PropertyToID("_SunDiscColor");
        private static readonly int SunHaloColorId = Shader.PropertyToID("_SunHaloColor");
        private static readonly int MoonColorId = Shader.PropertyToID("_MoonColor");
        private static readonly int SunDiscSizeId = Shader.PropertyToID("_SunDiscSize");
        private static readonly int SunHaloSizeId = Shader.PropertyToID("_SunHaloSize");
        private static readonly int MoonSizeId = Shader.PropertyToID("_MoonSize");
        private static readonly int MoonPhaseId = Shader.PropertyToID("_MoonPhase");
        private static readonly int BandCountId = Shader.PropertyToID("_BandCount");
        private static readonly int BandStrengthId = Shader.PropertyToID("_BandStrength");
        private static readonly int GradientExposureId = Shader.PropertyToID("_GradientExposure");
        private static readonly int SkySunDirectionId = Shader.PropertyToID("_AnemoraHd2dSkySunDirection");
        private static readonly int SkyMoonDirectionId = Shader.PropertyToID("_AnemoraHd2dSkyMoonDirection");
        private static readonly int CloudTintId = Shader.PropertyToID("_AnemoraHd2dCloudTint");
        private static readonly int StarColorId = Shader.PropertyToID("_StarColor");
        private static readonly int StarDensityId = Shader.PropertyToID("_StarDensity");
        private static readonly int StarThresholdId = Shader.PropertyToID("_StarThreshold");
        private static readonly int StarPointSizeId = Shader.PropertyToID("_StarPointSize");
        private static readonly int StarIntensityId = Shader.PropertyToID("_StarIntensity");
        private static readonly int StarTwinkleStrengthId = Shader.PropertyToID("_StarTwinkleStrength");
        private static readonly int StarTwinkleSpeedId = Shader.PropertyToID("_StarTwinkleSpeed");
        private static readonly int StarHorizonFadeStartId = Shader.PropertyToID("_StarHorizonFadeStart");
        private static readonly int StarHorizonFadeEndId = Shader.PropertyToID("_StarHorizonFadeEnd");
        private static readonly int StarNightOpacityId = Shader.PropertyToID("_StarNightOpacity");
        private static readonly int StarMilkyWayIntensityId = Shader.PropertyToID("_StarMilkyWayIntensity");
        private static readonly int StarReviewTimeId = Shader.PropertyToID("_StarReviewTime");

        [SerializeField] private FastVsHd2dGradientSkyProfile profile;
        [SerializeField] private FastVsHd2dTwinklingStarFieldProfile starFieldProfile;
        [SerializeField] private Material skyboxMaterial;
        [SerializeField] private Material cloudBandMaterial;
        [SerializeField] private Light directionalSun;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Renderer[] cloudBandRenderers = System.Array.Empty<Renderer>();
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private float reviewCloudDriftOffset;
        [SerializeField] private float reviewStarTimeSeconds;

        public bool IsReadyForReview => profile != null && skyboxMaterial != null && cloudBandMaterial != null && directionalSun != null;
        public FastVsHd2dGradientSkyProfile ProfileForReview => profile;
        public FastVsHd2dTwinklingStarFieldProfile StarFieldProfileForReview => starFieldProfile;
        public Material SkyboxMaterialForReview => skyboxMaterial;
        public Material CloudBandMaterialForReview => cloudBandMaterial;
        public int CloudBandCountForReview => cloudBandRenderers != null ? cloudBandRenderers.Length : 0;
        public float LastSunViewHeightForReview { get; private set; }
        public Color LastHorizonColorForReview { get; private set; } = Color.black;
        public Color LastZenithColorForReview { get; private set; } = Color.black;
        public Color LastCloudTintForReview { get; private set; } = Color.white;
        public float LastFogDensityForReview { get; private set; }
        public float LastStarNightVisibilityForReview { get; private set; }
        public float ReviewCloudDriftOffsetForReview => reviewCloudDriftOffset;
        public float ReviewStarTimeSecondsForReview => reviewStarTimeSeconds;
        public bool PublishEveryFrameForReview => publishEveryFrame;
        public bool AmbientUsesSkyboxForReview => RenderSettings.ambientMode == AmbientMode.Skybox;
        public bool CameraUsesSkyboxClearForReview => sceneCamera != null && sceneCamera.clearFlags == CameraClearFlags.Skybox;

        private void OnEnable()
        {
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
            FastVsHd2dGradientSkyProfile skyProfile,
            Material authoredSkyboxMaterial,
            Material authoredCloudBandMaterial,
            Light sunLight,
            Camera camera,
            Renderer[] cloudRenderers,
            FastVsHd2dTwinklingStarFieldProfile configuredStarFieldProfile = null)
        {
            profile = skyProfile;
            starFieldProfile = configuredStarFieldProfile;
            skyboxMaterial = authoredSkyboxMaterial;
            cloudBandMaterial = authoredCloudBandMaterial;
            directionalSun = sunLight;
            sceneCamera = camera;
            cloudBandRenderers = cloudRenderers ?? System.Array.Empty<Renderer>();
            publishEveryFrame = true;
            PublishForReview();
        }

        public void ConfigureStarFieldForReview(FastVsHd2dTwinklingStarFieldProfile configuredStarFieldProfile)
        {
            starFieldProfile = configuredStarFieldProfile;
            PublishForReview();
        }

        public void ApplyReviewSunDirectionForReview(float elevationDegrees, float azimuthDegrees, float cloudDriftOffset)
        {
            if (directionalSun != null)
            {
                directionalSun.transform.rotation = Quaternion.Euler(elevationDegrees, azimuthDegrees, 0f);
                RenderSettings.sun = directionalSun;
            }

            reviewCloudDriftOffset = cloudDriftOffset;
            PublishForReview();
        }

        public void ApplyReviewStarTimeForReview(float starTimeSeconds)
        {
            reviewStarTimeSeconds = Mathf.Max(0f, starTimeSeconds);
            PublishForReview();
        }

        public void ApplyAmbientVfxCloudDriftForReview(float cloudDriftOffset)
        {
            reviewCloudDriftOffset = cloudDriftOffset;
            PublishForReview();
        }

        public void PublishForReview()
        {
            if (profile == null || skyboxMaterial == null)
            {
                return;
            }

            if (sceneCamera == null)
            {
                sceneCamera = Camera.main;
            }

            if (directionalSun == null)
            {
                directionalSun = RenderSettings.sun;
            }

            var lightDirection = directionalSun != null
                ? (directionalSun.transform.rotation * Vector3.forward).normalized
                : new Vector3(0f, -0.65f, -0.75f).normalized;
            var sunViewDirection = (-lightDirection).normalized;
            var moonViewDirection = lightDirection.normalized;
            LastSunViewHeightForReview = sunViewDirection.y;
            var state = profile.EvaluateForReview(LastSunViewHeightForReview);
            LastHorizonColorForReview = state.HorizonColor;
            LastZenithColorForReview = state.ZenithColor;
            LastCloudTintForReview = state.CloudTint;
            LastFogDensityForReview = state.FogDensity;
            LastStarNightVisibilityForReview = starFieldProfile != null
                ? starFieldProfile.EvaluateNightVisibilityForReview(LastSunViewHeightForReview)
                : 0f;

            ApplySkyboxMaterial(sunViewDirection, moonViewDirection);
            ApplyRenderSettings(state);
            ApplyCloudBands(state);
        }

        private void ApplySkyboxMaterial(Vector3 sunViewDirection, Vector3 moonViewDirection)
        {
            skyboxMaterial.SetColor(DayHorizonId, profile.DayHorizonForReview);
            skyboxMaterial.SetColor(DayZenithId, profile.DayZenithForReview);
            skyboxMaterial.SetColor(SunsetHorizonId, profile.SunsetHorizonForReview);
            skyboxMaterial.SetColor(SunsetZenithId, profile.SunsetZenithForReview);
            skyboxMaterial.SetColor(NightHorizonId, profile.NightHorizonForReview);
            skyboxMaterial.SetColor(NightZenithId, profile.NightZenithForReview);
            skyboxMaterial.SetColor(SunDiscColorId, profile.SunDiscColorForReview);
            skyboxMaterial.SetColor(SunHaloColorId, profile.SunHaloColorForReview);
            skyboxMaterial.SetColor(MoonColorId, profile.MoonColorForReview);
            skyboxMaterial.SetFloat(SunDiscSizeId, profile.SunDiscSizeForReview);
            skyboxMaterial.SetFloat(SunHaloSizeId, profile.SunHaloSizeForReview);
            skyboxMaterial.SetFloat(MoonSizeId, profile.MoonSizeForReview);
            skyboxMaterial.SetFloat(MoonPhaseId, profile.MoonPhaseForReview);
            skyboxMaterial.SetFloat(BandCountId, profile.BandCountForReview);
            skyboxMaterial.SetFloat(BandStrengthId, profile.BandStrengthForReview);
            skyboxMaterial.SetFloat(GradientExposureId, profile.GradientExposureForReview);
            ApplyStarFieldMaterial();
            skyboxMaterial.SetVector(SkySunDirectionId, new Vector4(sunViewDirection.x, sunViewDirection.y, sunViewDirection.z, 0f));
            skyboxMaterial.SetVector(SkyMoonDirectionId, new Vector4(moonViewDirection.x, moonViewDirection.y, moonViewDirection.z, 0f));
            Shader.SetGlobalVector(SkySunDirectionId, new Vector4(sunViewDirection.x, sunViewDirection.y, sunViewDirection.z, 0f));
            Shader.SetGlobalVector(SkyMoonDirectionId, new Vector4(moonViewDirection.x, moonViewDirection.y, moonViewDirection.z, 0f));
            RenderSettings.skybox = skyboxMaterial;
        }

        private void ApplyStarFieldMaterial()
        {
            if (starFieldProfile == null)
            {
                skyboxMaterial.SetFloat(StarIntensityId, 0f);
                skyboxMaterial.SetFloat(StarNightOpacityId, 0f);
                return;
            }

            skyboxMaterial.SetColor(StarColorId, starFieldProfile.StarColorForReview);
            skyboxMaterial.SetFloat(StarDensityId, starFieldProfile.StarDensityForReview);
            skyboxMaterial.SetFloat(StarThresholdId, starFieldProfile.StarThresholdForReview);
            skyboxMaterial.SetFloat(StarPointSizeId, starFieldProfile.StarPointSizeForReview);
            skyboxMaterial.SetFloat(StarIntensityId, starFieldProfile.StarIntensityForReview);
            skyboxMaterial.SetFloat(StarTwinkleStrengthId, starFieldProfile.TwinkleStrengthForReview);
            skyboxMaterial.SetFloat(StarTwinkleSpeedId, starFieldProfile.TwinkleSpeedForReview);
            skyboxMaterial.SetFloat(StarHorizonFadeStartId, starFieldProfile.HorizonFadeStartForReview);
            skyboxMaterial.SetFloat(StarHorizonFadeEndId, starFieldProfile.HorizonFadeEndForReview);
            skyboxMaterial.SetFloat(StarNightOpacityId, starFieldProfile.MaxNightOpacityForReview);
            skyboxMaterial.SetFloat(StarMilkyWayIntensityId, starFieldProfile.MilkyWayIntensityForReview);
            skyboxMaterial.SetFloat(StarReviewTimeId, reviewStarTimeSeconds);
        }

        private void ApplyRenderSettings(FastVsHd2dGradientSkyProfile.SkyState state)
        {
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientIntensity = state.AmbientIntensity;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = state.HorizonColor;
            RenderSettings.fogDensity = state.FogDensity;
            if (sceneCamera != null)
            {
                sceneCamera.clearFlags = CameraClearFlags.Skybox;
            }

            DynamicGI.UpdateEnvironment();
        }

        private void ApplyCloudBands(FastVsHd2dGradientSkyProfile.SkyState state)
        {
            Shader.SetGlobalColor(CloudTintId, state.CloudTint);
            if (cloudBandMaterial != null)
            {
                if (cloudBandMaterial.HasProperty("_BaseColor"))
                {
                    cloudBandMaterial.SetColor("_BaseColor", state.CloudTint);
                }

                if (cloudBandMaterial.HasProperty("_Color"))
                {
                    cloudBandMaterial.SetColor("_Color", state.CloudTint);
                }

                cloudBandMaterial.SetTextureOffset("_BaseMap", new Vector2(reviewCloudDriftOffset, 0f));
                cloudBandMaterial.SetTextureOffset("_MainTex", new Vector2(reviewCloudDriftOffset, 0f));
            }

            if (cloudBandRenderers == null)
            {
                return;
            }

            for (var index = 0; index < cloudBandRenderers.Length; index++)
            {
                var cloudRenderer = cloudBandRenderers[index];
                if (cloudRenderer == null)
                {
                    continue;
                }

                cloudRenderer.sharedMaterial = cloudBandMaterial;
                if (sceneCamera != null)
                {
                    var toCamera = cloudRenderer.transform.position - sceneCamera.transform.position;
                    if (toCamera.sqrMagnitude > 0.01f)
                    {
                        cloudRenderer.transform.rotation = Quaternion.LookRotation(toCamera.normalized, Vector3.up);
                    }
                }
            }
        }
    }
}
