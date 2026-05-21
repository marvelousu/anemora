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

        private FastVsHouseArea lastArea = (FastVsHouseArea)(-1);

        public FastVsHouseArea LastAppliedAreaForReview => lastArea;
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
        }

        public void ApplyAreaForReview(FastVsHouseArea area)
        {
            ApplyProfile(area, true);
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

            ApplyProfile(areaVisibility.ActiveAreaForReview, force);
        }

        private void ApplyProfile(FastVsHouseArea area, bool force)
        {
            if (!force && area == lastArea)
            {
                return;
            }

            switch (area)
            {
                case FastVsHouseArea.Exterior:
                    ApplyExteriorProfile();
                    break;
                case FastVsHouseArea.CentralPlaza:
                    ApplyCentralPlazaProfile();
                    break;
                case FastVsHouseArea.Library:
                    ApplyLibraryProfile();
                    break;
                default:
                    ApplyInteriorProfile();
                    break;
            }

            lastArea = area;
        }

        private void ApplyInteriorProfile()
        {
            ApplyMainLight(0.86f, 0.48f, new Color(1.00f, 0.88f, 0.72f), new Vector3(46f, -42f, 0f));
            ApplyAmbient(new Color(0.205f, 0.195f, 0.190f));
            ApplyFog(false, new Color(0.080f, 0.074f, 0.070f), 10f, 42f);
            ApplyCameraBackground(new Color(0.064f, 0.060f, 0.060f, 1f));
            ApplyWarmFill(new Vector3(-7.25f, 1.65f, -9.10f), 0.30f, 6.0f, new Color(1.00f, 0.72f, 0.46f));
            ApplyCoolRim(new Vector3(25f, 132f, 0f), 0.045f, new Color(0.58f, 0.70f, 1.00f));
            ApplyLibraryWindow(false);
        }

        private void ApplyExteriorProfile()
        {
            ApplyMainLight(1.04f, 0.54f, new Color(1.00f, 0.94f, 0.82f), new Vector3(50f, -36f, 0f));
            ApplyAmbient(new Color(0.245f, 0.255f, 0.265f));
            ApplyFog(true, new Color(0.130f, 0.150f, 0.170f), 20f, 82f);
            ApplyCameraBackground(new Color(0.118f, 0.142f, 0.166f, 1f));
            ApplyWarmFill(new Vector3(7.10f, 1.50f, 6.75f), 0.18f, 8.0f, new Color(1.00f, 0.76f, 0.48f));
            ApplyCoolRim(new Vector3(32f, 146f, 0f), 0.070f, new Color(0.58f, 0.72f, 1.00f));
            ApplyLibraryWindow(false);
        }

        private void ApplyCentralPlazaProfile()
        {
            ApplyMainLight(1.08f, 0.56f, new Color(1.00f, 0.92f, 0.78f), new Vector3(49f, -31f, 0f));
            ApplyAmbient(new Color(0.235f, 0.245f, 0.260f));
            ApplyFog(true, new Color(0.118f, 0.138f, 0.160f), 18f, 90f);
            ApplyCameraBackground(new Color(0.112f, 0.138f, 0.164f, 1f));
            ApplyWarmFill(new Vector3(20.80f, 1.25f, 17.30f), 0.14f, 10.0f, new Color(1.00f, 0.72f, 0.42f));
            ApplyCoolRim(new Vector3(28f, 152f, 0f), 0.075f, new Color(0.56f, 0.72f, 1.00f));
            ApplyLibraryWindow(false);
        }

        private void ApplyLibraryProfile()
        {
            ApplyMainLight(0.74f, 0.50f, new Color(1.00f, 0.86f, 0.64f), new Vector3(54f, -28f, 0f));
            ApplyAmbient(new Color(0.155f, 0.145f, 0.135f));
            ApplyFog(true, new Color(0.075f, 0.068f, 0.060f), 8f, 38f);
            ApplyCameraBackground(new Color(0.050f, 0.046f, 0.044f, 1f));
            ApplyWarmFill(new Vector3(31.00f, 1.45f, 18.65f), 0.24f, 7.5f, new Color(1.00f, 0.70f, 0.42f));
            ApplyCoolRim(new Vector3(22f, 148f, 0f), 0.035f, new Color(0.52f, 0.62f, 0.96f));
            ApplyLibraryWindow(true);
        }

        private void ApplyMainLight(float intensity, float shadowStrength, Color color, Vector3 euler)
        {
            if (mainLight == null)
            {
                return;
            }

            mainLight.type = LightType.Directional;
            mainLight.intensity = intensity;
            mainLight.shadowStrength = shadowStrength;
            mainLight.color = color;
            mainLight.shadows = LightShadows.Soft;
            mainLight.transform.rotation = Quaternion.Euler(euler);
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

        private void ApplyCoolRim(Vector3 euler, float intensity, Color color)
        {
            if (coolRimLight == null)
            {
                return;
            }

            coolRimLight.enabled = true;
            coolRimLight.type = LightType.Directional;
            coolRimLight.transform.rotation = Quaternion.Euler(euler);
            coolRimLight.intensity = intensity;
            coolRimLight.color = color;
            coolRimLight.shadows = LightShadows.None;
        }

        private void ApplyLibraryWindow(bool enabled)
        {
            if (libraryWindowLight == null)
            {
                return;
            }

            libraryWindowLight.enabled = enabled;
            libraryWindowLight.type = LightType.Spot;
            libraryWindowLight.transform.SetPositionAndRotation(
                new Vector3(28.55f, 3.05f, 23.15f),
                Quaternion.Euler(58f, 36f, 0f));
            libraryWindowLight.intensity = enabled ? 0.48f : 0f;
            libraryWindowLight.range = 8.5f;
            libraryWindowLight.spotAngle = 48f;
            libraryWindowLight.color = new Color(1.00f, 0.76f, 0.48f);
            libraryWindowLight.shadows = LightShadows.None;
        }

        private static Light FindNamedLight(string objectName)
        {
            var target = GameObject.Find(objectName);
            return target != null ? target.GetComponent<Light>() : null;
        }
    }
}
