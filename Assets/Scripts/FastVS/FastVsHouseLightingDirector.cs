using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsHouseLightingDirector : MonoBehaviour
    {
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Light mainLight;
        [SerializeField] private Light warmFillLight;
        [SerializeField] private Light coolRimLight;
        [SerializeField] private Light libraryWindowLight;
        [SerializeField] private float transitionDuration = 0.55f;

        private const int LibraryWindowCookieSize = 128;
        private const string LibraryWindowCookieName = "FastVS_LibraryWindowCookieStage3";

        private FastVsHouseArea lastArea = (FastVsHouseArea)(-1);
        private FastVsHouseArea targetArea = (FastVsHouseArea)(-1);
        private bool transitionActive;
        private float transitionElapsed;
        private bool hasCurrentProfile;
        private LightingProfile currentProfile;
        private LightingProfile transitionStartProfile;
        private LightingProfile transitionTargetProfile;
        private Texture2D libraryWindowCookieTexture;

        public FastVsHouseArea LastAppliedAreaForReview => lastArea;
        public FastVsHouseArea TargetAreaForReview => targetArea;
        public bool TransitionActiveForReview => transitionActive;
        public float TransitionDurationForReview => Mathf.Clamp(transitionDuration, 0.20f, 1.20f);
        public float CurrentBlendForReview => transitionActive ? SmoothStep(GetTransitionProgress()) : 1f;
        public bool HasRequiredLightsForReview =>
            mainLight != null &&
            warmFillLight != null &&
            coolRimLight != null &&
            libraryWindowLight != null;

        private void Awake()
        {
            ResolveReferences();
            ApplyCurrentArea(true);
        }

        private void LateUpdate()
        {
            ResolveReferences();
            ApplyCurrentArea(false);
            UpdateTransition();
        }

        private void OnValidate()
        {
            transitionDuration = Mathf.Clamp(transitionDuration, 0.20f, 1.20f);
        }

        public void ApplyAreaForReview(FastVsHouseArea area)
        {
            ResolveReferences();
            ApplyProfileImmediately(area, CreateProfile(area));
        }

        public void BeginAreaTransitionForReview(FastVsHouseArea area)
        {
            ResolveReferences();

            if (!hasCurrentProfile)
            {
                ApplyProfileImmediately(area, CreateProfile(area));
                return;
            }

            if (transitionActive && area == targetArea)
            {
                return;
            }

            if (!transitionActive && area == lastArea)
            {
                return;
            }

            transitionStartProfile = currentProfile;
            transitionTargetProfile = CreateProfile(area);
            targetArea = area;
            transitionElapsed = 0f;
            transitionActive = true;
        }

        public void SampleTransitionForReview(FastVsHouseArea from, FastVsHouseArea to, float normalized)
        {
            ResolveReferences();

            var fromProfile = CreateProfile(from);
            var toProfile = CreateProfile(to);
            var blend = SmoothStep(Mathf.Clamp01(normalized));
            var sampledProfile = LerpProfiles(fromProfile, toProfile, blend);

            ApplyLightingSnapshot(sampledProfile, fromProfile, toProfile, blend, false);
            currentProfile = sampledProfile;
            lastArea = to;
            targetArea = to;
            hasCurrentProfile = true;
            transitionActive = false;
            transitionElapsed = 0f;
        }

        private void ResolveReferences()
        {
            if (areaVisibility == null)
            {
                areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            }

            if (sceneCamera == null)
            {
                sceneCamera = Camera.main;
            }

            if (mainLight == null)
            {
                mainLight = FindNamedLight("Directional Light");
            }

            if (warmFillLight == null)
            {
                warmFillLight = FindNamedLight("FastVS_HD2D_WarmFillLight");
            }

            if (coolRimLight == null)
            {
                coolRimLight = FindNamedLight("FastVS_HD2D_CoolRimLight");
            }

            if (libraryWindowLight == null)
            {
                libraryWindowLight = FindNamedLight("FastVS_HD2D_LibraryWindowLight");
            }
        }

        private void ApplyCurrentArea(bool force)
        {
            if (areaVisibility == null)
            {
                return;
            }

            var area = areaVisibility.ActiveAreaForReview;
            if (force)
            {
                ApplyProfileImmediately(area, CreateProfile(area));
                return;
            }

            if (!transitionActive && area == lastArea)
            {
                return;
            }

            if (transitionActive && area == targetArea)
            {
                return;
            }

            BeginAreaTransitionForReview(area);
        }

        private void UpdateTransition()
        {
            if (!transitionActive)
            {
                return;
            }

            transitionElapsed += Time.unscaledDeltaTime;
            var normalized = GetTransitionProgress();
            if (normalized >= 1f)
            {
                currentProfile = transitionTargetProfile;
                ApplyLightingSnapshot(transitionTargetProfile, transitionStartProfile, transitionTargetProfile, 1f, false);
                lastArea = targetArea;
                hasCurrentProfile = true;
                transitionActive = false;
                transitionElapsed = 0f;
                return;
            }

            var eased = SmoothStep(normalized);
            currentProfile = LerpProfiles(transitionStartProfile, transitionTargetProfile, eased);
            ApplyLightingSnapshot(currentProfile, transitionStartProfile, transitionTargetProfile, eased, true);
            hasCurrentProfile = true;
        }

        private float GetTransitionProgress()
        {
            var duration = TransitionDurationForReview;
            if (duration <= 0f)
            {
                return 1f;
            }

            return Mathf.Clamp01(transitionElapsed / duration);
        }

        private void ApplyProfileImmediately(FastVsHouseArea area, in LightingProfile profile)
        {
            currentProfile = profile;
            lastArea = area;
            targetArea = area;
            hasCurrentProfile = true;
            transitionActive = false;
            transitionElapsed = 0f;
            ApplyLightingSnapshot(profile, profile, profile, 1f, false);
        }

        private void ApplyLightingSnapshot(
            in LightingProfile profile,
            in LightingProfile fromProfile,
            in LightingProfile toProfile,
            float blend,
            bool isTransition)
        {
            ApplyAmbient(profile.ambientLight);
            ApplyFog(
                isTransition ? (fromProfile.fogEnabled || toProfile.fogEnabled) : profile.fogEnabled,
                profile.fogColor,
                profile.fogStartDistance,
                profile.fogEndDistance);
            ApplyCameraBackground(profile.cameraBackgroundColor);
            ApplyWarmFill(profile.warmFillPosition, profile.warmFillIntensity, profile.warmFillRange, profile.warmFillColor);
            ApplyCoolRim(profile.coolRimRotation, profile.coolRimIntensity, profile.coolRimColor);
            ApplyLibraryWindow(
                isTransition ? (fromProfile.libraryWindowEnabled || toProfile.libraryWindowEnabled) : profile.libraryWindowEnabled,
                profile.libraryWindowPosition,
                profile.libraryWindowRotation,
                profile.libraryWindowIntensity,
                profile.libraryWindowRange,
                profile.libraryWindowSpotAngle,
                profile.libraryWindowColor);
        }

        private static void ApplyAmbient(Color ambient)
        {
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = ambient;
        }

        private static void ApplyFog(bool enabled, Color color, float startDistance, float endDistance)
        {
            RenderSettings.fog = enabled;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = color;
            RenderSettings.fogStartDistance = startDistance;
            RenderSettings.fogEndDistance = endDistance;
        }

        private void ApplyCameraBackground(Color color)
        {
            if (sceneCamera != null)
            {
                sceneCamera.backgroundColor = color;
            }
        }

        private void ApplyWarmFill(Vector3 position, float intensity, float range, Color color)
        {
            if (warmFillLight == null)
            {
                return;
            }

            warmFillLight.enabled = true;
            warmFillLight.type = LightType.Point;
            warmFillLight.transform.position = position;
            warmFillLight.intensity = intensity;
            warmFillLight.range = range;
            warmFillLight.color = color;
            warmFillLight.shadows = LightShadows.None;
        }

        private void ApplyCoolRim(Quaternion rotation, float intensity, Color color)
        {
            if (coolRimLight == null)
            {
                return;
            }

            coolRimLight.enabled = true;
            coolRimLight.type = LightType.Directional;
            coolRimLight.transform.rotation = rotation;
            coolRimLight.intensity = intensity;
            coolRimLight.color = color;
            coolRimLight.shadows = LightShadows.None;
        }

        private void ApplyLibraryWindow(
            bool enabled,
            Vector3 position,
            Quaternion rotation,
            float intensity,
            float range,
            float spotAngle,
            Color color)
        {
            if (libraryWindowLight == null)
            {
                return;
            }

            libraryWindowLight.enabled = enabled;
            libraryWindowLight.type = LightType.Spot;
            libraryWindowLight.transform.SetPositionAndRotation(position, rotation);
            libraryWindowLight.intensity = intensity;
            libraryWindowLight.range = range;
            libraryWindowLight.spotAngle = spotAngle;
            libraryWindowLight.color = color;
            libraryWindowLight.shadows = LightShadows.None;
            libraryWindowLight.cookie = enabled ? EnsureLibraryWindowCookieTexture() : null;
        }

        private Texture2D EnsureLibraryWindowCookieTexture()
        {
            if (libraryWindowCookieTexture != null)
            {
                return libraryWindowCookieTexture;
            }

            libraryWindowCookieTexture = new Texture2D(
                LibraryWindowCookieSize,
                LibraryWindowCookieSize,
                TextureFormat.RGBA32,
                false,
                true)
            {
                name = LibraryWindowCookieName,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.DontSave
            };

            var pixels = new Color[LibraryWindowCookieSize * LibraryWindowCookieSize];
            for (var y = 0; y < LibraryWindowCookieSize; y++)
            {
                var v = y / (float)(LibraryWindowCookieSize - 1);
                for (var x = 0; x < LibraryWindowCookieSize; x++)
                {
                    var u = x / (float)(LibraryWindowCookieSize - 1);
                    var luma = SampleLibraryWindowCookieLuma(u, v);
                    pixels[(y * LibraryWindowCookieSize) + x] = new Color(luma, luma, luma, 1f);
                }
            }

            libraryWindowCookieTexture.SetPixels(pixels);
            libraryWindowCookieTexture.Apply(false, false);
            return libraryWindowCookieTexture;
        }

        private static float SampleLibraryWindowCookieLuma(float u, float v)
        {
            var mullion = Mathf.Max(
                SmoothBand(0.012f, 0.040f, Mathf.Abs(u - 0.50f)),
                Mathf.Max(
                    SmoothBand(0.010f, 0.032f, Mathf.Abs(v - 0.50f)),
                    Mathf.Max(
                        SmoothBand(0.008f, 0.030f, Mathf.Abs(v - 0.25f)),
                        SmoothBand(0.008f, 0.030f, Mathf.Abs(v - 0.75f)))));
            var edgeSoftness = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((u * (1f - u) * v * (1f - v)) * 20f));
            var glassNoise = Hash01(Mathf.FloorToInt(u * 16f), Mathf.FloorToInt(v * 16f), 3301);
            var paneLuma = Mathf.Lerp(0.86f, 1.0f, glassNoise);
            var luma = Mathf.Lerp(paneLuma, 0.34f, mullion);
            return Mathf.Clamp01(Mathf.Lerp(0.50f, luma, edgeSoftness));
        }

        private static float SmoothBand(float innerRadius, float outerRadius, float distance)
        {
            return 1f - Mathf.SmoothStep(0f, 1f, Mathf.InverseLerp(innerRadius, outerRadius, distance));
        }

        private static float Hash01(int x, int y, int seed)
        {
            unchecked
            {
                var hash = seed;
                hash ^= x * 374761393;
                hash = (hash << 13) ^ hash;
                hash ^= y * 668265263;
                hash *= 1274126177;
                hash ^= hash >> 16;
                return (hash & 0x7fffffff) / (float)int.MaxValue;
            }
        }

        private static LightingProfile CreateProfile(FastVsHouseArea area)
        {
            switch (area)
            {
                case FastVsHouseArea.Exterior:
                    return new LightingProfile
                    {
                        ambientLight = new Color(0.152f, 0.158f, 0.164f, 1f),
                        fogEnabled = true,
                        fogColor = new Color(0.226f, 0.221f, 0.204f, 1f),
                        fogStartDistance = 20f,
                        fogEndDistance = 82f,
                        cameraBackgroundColor = new Color(0.234f, 0.221f, 0.198f, 1f),
                        warmFillPosition = new Vector3(7.10f, 1.50f, 6.75f),
                        warmFillIntensity = 0.40f,
                        warmFillRange = 7.8f,
                        warmFillColor = new Color(1.00f, 0.72f, 0.46f, 1f),
                        coolRimRotation = Quaternion.Euler(32f, 146f, 0f),
                        coolRimIntensity = 0.16f,
                        coolRimColor = new Color(0.58f, 0.72f, 1.00f, 1f),
                        libraryWindowEnabled = false,
                        libraryWindowPosition = new Vector3(28.55f, 3.05f, 23.15f),
                        libraryWindowRotation = Quaternion.Euler(58f, 36f, 0f),
                        libraryWindowIntensity = 0f,
                        libraryWindowRange = 8.5f,
                        libraryWindowSpotAngle = 48f,
                        libraryWindowColor = new Color(1.00f, 0.76f, 0.48f, 1f)
                    };
                case FastVsHouseArea.CentralPlaza:
                    return new LightingProfile
                    {
                        ambientLight = new Color(0.074f, 0.068f, 0.058f, 1f),
                        fogEnabled = false,
                        fogColor = new Color(0.235f, 0.215f, 0.178f, 1f),
                        fogStartDistance = 8f,
                        fogEndDistance = 40f,
                        cameraBackgroundColor = new Color(0.220f, 0.286f, 0.340f, 1f),
                        warmFillPosition = new Vector3(20.80f, 1.25f, 17.30f),
                        warmFillIntensity = 0.30f,
                        warmFillRange = 9.8f,
                        warmFillColor = new Color(1.00f, 0.64f, 0.34f, 1f),
                        coolRimRotation = Quaternion.Euler(28f, 152f, 0f),
                        coolRimIntensity = 0.16f,
                        coolRimColor = new Color(0.46f, 0.58f, 0.92f, 1f),
                        libraryWindowEnabled = false,
                        libraryWindowPosition = new Vector3(28.55f, 3.05f, 23.15f),
                        libraryWindowRotation = Quaternion.Euler(58f, 36f, 0f),
                        libraryWindowIntensity = 0f,
                        libraryWindowRange = 8.5f,
                        libraryWindowSpotAngle = 48f,
                        libraryWindowColor = new Color(1.00f, 0.76f, 0.48f, 1f)
                    };
                case FastVsHouseArea.Library:
                    return new LightingProfile
                    {
                        ambientLight = new Color(0.110f, 0.102f, 0.096f, 1f),
                        fogEnabled = true,
                        fogColor = new Color(0.075f, 0.068f, 0.060f, 1f),
                        fogStartDistance = 8f,
                        fogEndDistance = 38f,
                        cameraBackgroundColor = new Color(0.046f, 0.042f, 0.040f, 1f),
                        warmFillPosition = new Vector3(31.00f, 1.45f, 18.65f),
                        warmFillIntensity = 0.50f,
                        warmFillRange = 7.3f,
                        warmFillColor = new Color(1.00f, 0.68f, 0.42f, 1f),
                        coolRimRotation = Quaternion.Euler(22f, 148f, 0f),
                        coolRimIntensity = 0.16f,
                        coolRimColor = new Color(0.52f, 0.62f, 0.96f, 1f),
                        libraryWindowEnabled = true,
                        libraryWindowPosition = new Vector3(28.55f, 3.05f, 23.15f),
                        libraryWindowRotation = Quaternion.Euler(58f, 36f, 0f),
                        libraryWindowIntensity = 1.50f,
                        libraryWindowRange = 8.5f,
                        libraryWindowSpotAngle = 48f,
                        libraryWindowColor = new Color(1.00f, 0.74f, 0.44f, 1f)
                    };
                case FastVsHouseArea.MiaInterior:
                    return CreateSmallHouseInteriorProfile(new Vector3(-21.20f, 1.65f, -8.90f));
                case FastVsHouseArea.AriaInterior:
                    return CreateSmallHouseInteriorProfile(new Vector3(-32.95f, 1.65f, -8.80f));
                default:
                    return new LightingProfile
                    {
                        ambientLight = new Color(0.155f, 0.145f, 0.138f, 1f),
                        fogEnabled = false,
                        fogColor = new Color(0.080f, 0.074f, 0.070f, 1f),
                        fogStartDistance = 10f,
                        fogEndDistance = 42f,
                        cameraBackgroundColor = new Color(0.060f, 0.056f, 0.055f, 1f),
                        warmFillPosition = new Vector3(-7.25f, 1.65f, -9.10f),
                        warmFillIntensity = 0.60f,
                        warmFillRange = 5.8f,
                        warmFillColor = new Color(1.00f, 0.70f, 0.44f, 1f),
                        coolRimRotation = Quaternion.Euler(25f, 132f, 0f),
                        coolRimIntensity = 0.15f,
                        coolRimColor = new Color(0.58f, 0.70f, 1.00f, 1f),
                        libraryWindowEnabled = false,
                        libraryWindowPosition = new Vector3(28.55f, 3.05f, 23.15f),
                        libraryWindowRotation = Quaternion.Euler(58f, 36f, 0f),
                        libraryWindowIntensity = 0f,
                        libraryWindowRange = 8.5f,
                        libraryWindowSpotAngle = 48f,
                        libraryWindowColor = new Color(1.00f, 0.76f, 0.48f, 1f)
                    };
            }
        }

        private static LightingProfile CreateSmallHouseInteriorProfile(Vector3 warmFillPosition)
        {
            return new LightingProfile
            {
                ambientLight = new Color(0.155f, 0.145f, 0.138f, 1f),
                fogEnabled = false,
                fogColor = new Color(0.080f, 0.074f, 0.070f, 1f),
                fogStartDistance = 10f,
                fogEndDistance = 42f,
                cameraBackgroundColor = new Color(0.060f, 0.056f, 0.055f, 1f),
                warmFillPosition = warmFillPosition,
                warmFillIntensity = 0.60f,
                warmFillRange = 5.8f,
                warmFillColor = new Color(1.00f, 0.70f, 0.44f, 1f),
                coolRimRotation = Quaternion.Euler(25f, 132f, 0f),
                coolRimIntensity = 0.15f,
                coolRimColor = new Color(0.58f, 0.70f, 1.00f, 1f),
                libraryWindowEnabled = false,
                libraryWindowPosition = new Vector3(28.55f, 3.05f, 23.15f),
                libraryWindowRotation = Quaternion.Euler(58f, 36f, 0f),
                libraryWindowIntensity = 0f,
                libraryWindowRange = 8.5f,
                libraryWindowSpotAngle = 48f,
                libraryWindowColor = new Color(1.00f, 0.76f, 0.48f, 1f)
            };
        }

        private static LightingProfile LerpProfiles(in LightingProfile from, in LightingProfile to, float blend)
        {
            return new LightingProfile
            {
                ambientLight = Color.Lerp(from.ambientLight, to.ambientLight, blend),
                fogEnabled = blend < 1f ? (from.fogEnabled || to.fogEnabled) : to.fogEnabled,
                fogColor = Color.Lerp(from.fogColor, to.fogColor, blend),
                fogStartDistance = Mathf.Lerp(from.fogStartDistance, to.fogStartDistance, blend),
                fogEndDistance = Mathf.Lerp(from.fogEndDistance, to.fogEndDistance, blend),
                cameraBackgroundColor = Color.Lerp(from.cameraBackgroundColor, to.cameraBackgroundColor, blend),
                warmFillPosition = Vector3.Lerp(from.warmFillPosition, to.warmFillPosition, blend),
                warmFillIntensity = Mathf.Lerp(from.warmFillIntensity, to.warmFillIntensity, blend),
                warmFillRange = Mathf.Lerp(from.warmFillRange, to.warmFillRange, blend),
                warmFillColor = Color.Lerp(from.warmFillColor, to.warmFillColor, blend),
                coolRimRotation = Quaternion.Slerp(from.coolRimRotation, to.coolRimRotation, blend),
                coolRimIntensity = Mathf.Lerp(from.coolRimIntensity, to.coolRimIntensity, blend),
                coolRimColor = Color.Lerp(from.coolRimColor, to.coolRimColor, blend),
                libraryWindowEnabled = blend < 1f ? (from.libraryWindowEnabled || to.libraryWindowEnabled) : to.libraryWindowEnabled,
                libraryWindowPosition = Vector3.Lerp(from.libraryWindowPosition, to.libraryWindowPosition, blend),
                libraryWindowRotation = Quaternion.Slerp(from.libraryWindowRotation, to.libraryWindowRotation, blend),
                libraryWindowIntensity = Mathf.Lerp(from.libraryWindowIntensity, to.libraryWindowIntensity, blend),
                libraryWindowRange = Mathf.Lerp(from.libraryWindowRange, to.libraryWindowRange, blend),
                libraryWindowSpotAngle = Mathf.Lerp(from.libraryWindowSpotAngle, to.libraryWindowSpotAngle, blend),
                libraryWindowColor = Color.Lerp(from.libraryWindowColor, to.libraryWindowColor, blend)
            };
        }

        private static float SmoothStep(float t)
        {
            t = Mathf.Clamp01(t);
            return t * t * (3f - 2f * t);
        }

        private static Light FindNamedLight(string objectName)
        {
            var target = GameObject.Find(objectName);
            return target != null ? target.GetComponent<Light>() : null;
        }

        private struct LightingProfile
        {
            public Color ambientLight;
            public bool fogEnabled;
            public Color fogColor;
            public float fogStartDistance;
            public float fogEndDistance;
            public Color cameraBackgroundColor;
            public Vector3 warmFillPosition;
            public float warmFillIntensity;
            public float warmFillRange;
            public Color warmFillColor;
            public Quaternion coolRimRotation;
            public float coolRimIntensity;
            public Color coolRimColor;
            public bool libraryWindowEnabled;
            public Vector3 libraryWindowPosition;
            public Quaternion libraryWindowRotation;
            public float libraryWindowIntensity;
            public float libraryWindowRange;
            public float libraryWindowSpotAngle;
            public Color libraryWindowColor;
        }
    }
}
