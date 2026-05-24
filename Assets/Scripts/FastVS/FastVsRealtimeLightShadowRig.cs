using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    [DefaultExecutionOrder(1000)]
    public sealed class FastVsRealtimeLightShadowRig : MonoBehaviour
    {
        private const string MaterialRoleTagName = "AnemoraFastVsHd2dRole";
        private const string SurfaceLitRole = "SurfaceLit";
        private const string SpriteCardRole = "SpriteCard";
        private const string PaperCardRole = "PaperCard";
        private const string PortalWindowRole = "PortalWindow";
        private const string OverlayGlowRole = "OverlayGlow";
        private const string ContactShadowRole = "ContactShadow";
        private const float ShadowPolicyRefreshSeconds = 0.35f;
        private const int CentralPlazaSunCookieSize = 128;
        private const float CentralPlazaSunCookieWorldSize = 9.25f;
        private const string CentralPlazaSunCookieName = "FastVS_CentralPlazaRealtimeSunCookieCycle147";
        private static readonly int SurfaceRampStrengthId = Shader.PropertyToID("_SurfaceRampStrength");
        private static readonly int DirectionalLightStrengthId = Shader.PropertyToID("_DirectionalLightStrength");
        private static readonly int ShadowReceiveStrengthId = Shader.PropertyToID("_ShadowReceiveStrength");
        private static readonly int ShadowTextureStrengthId = Shader.PropertyToID("_ShadowTextureStrength");
        private static readonly int TopLightId = Shader.PropertyToID("_TopLight");
        private static readonly int SideShadeId = Shader.PropertyToID("_SideShade");
        private static readonly int FloorShadeId = Shader.PropertyToID("_FloorShade");
        private static readonly int RampStrengthId = Shader.PropertyToID("_RampStrength");
        private static readonly int WorldLightStrengthId = Shader.PropertyToID("_WorldLightStrength");
        private static readonly int WorldShadowReceiveStrengthId = Shader.PropertyToID("_WorldShadowReceiveStrength");
        private static readonly Color CentralPlazaTopLight = new Color(1.20f, 1.10f, 0.88f, 1f);
        private static readonly Color CentralPlazaSideShade = new Color(0.50f, 0.50f, 0.47f, 1f);
        private static readonly Color CentralPlazaFloorShade = new Color(0.44f, 0.45f, 0.42f, 1f);

        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Light mainLight;
        [SerializeField] private bool enforceRendererShadowPolicy = true;
        [SerializeField] private Color exteriorSkyColor = new Color(0.48f, 0.50f, 0.46f, 1f);
        [SerializeField] private Color centralPlazaSkyColor = new Color(0.220f, 0.286f, 0.340f, 1f);

        private float nextShadowPolicyRefreshTime;
        private Material centralPlazaSkyboxMaterial;
        private GameObject cycle128GradeRoot;
        private MeshRenderer cycle128GradeRenderer;
        private MeshRenderer cycle128BeamRenderer;
        private MeshRenderer cycle131ShadowPaintRenderer;
        private MeshRenderer cycle131SunPaintRenderer;
        private Material cycle128GradeMaterial;
        private Material cycle128BeamMaterial;
        private Material cycle131ShadowPaintMaterial;
        private Material cycle131SunPaintMaterial;
        private Texture2D cycle128GradeTexture;
        private Texture2D cycle128BeamTexture;
        private Texture2D cycle131ShadowPaintTexture;
        private Texture2D cycle131SunPaintTexture;
        private Texture2D centralPlazaSunCookieTexture;

        private void Awake()
        {
            ResolveReferences();
            ApplyNowForReview();
        }

        private void LateUpdate()
        {
            ResolveReferences();
            ApplyLightAndSky();

            if (enforceRendererShadowPolicy && Time.unscaledTime >= nextShadowPolicyRefreshTime)
            {
                ApplyRendererShadowPolicy();
                nextShadowPolicyRefreshTime = Time.unscaledTime + ShadowPolicyRefreshSeconds;
            }
        }

        public void ApplyNowForReview()
        {
            ResolveReferences();
            ApplyLightAndSky();
            ApplyRendererShadowPolicy();
            nextShadowPolicyRefreshTime = Time.unscaledTime + ShadowPolicyRefreshSeconds;
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
                var lightObject = GameObject.Find("Directional Light");
                mainLight = lightObject != null ? lightObject.GetComponent<Light>() : null;
            }
        }

        private void ApplyLightAndSky()
        {
            var area = areaVisibility != null ? areaVisibility.ActiveAreaForReview : FastVsHouseArea.Interior;
            var isCentralPlaza = area == FastVsHouseArea.CentralPlaza;

            if (mainLight != null)
            {
                mainLight.enabled = true;
                mainLight.type = LightType.Directional;
                mainLight.shadows = LightShadows.Soft;
                mainLight.shadowResolution = isCentralPlaza ? LightShadowResolution.VeryHigh : mainLight.shadowResolution;
                mainLight.shadowStrength = isCentralPlaza ? 0.92f : Mathf.Max(mainLight.shadowStrength, 0.82f);
                mainLight.shadowBias = isCentralPlaza ? 0.012f : Mathf.Min(mainLight.shadowBias, 0.025f);
                mainLight.shadowNormalBias = isCentralPlaza ? 0.10f : Mathf.Min(mainLight.shadowNormalBias, 0.18f);
                mainLight.shadowNearPlane = Mathf.Min(Mathf.Max(mainLight.shadowNearPlane, 0.05f), 0.12f);

                if (isCentralPlaza)
                {
                    mainLight.intensity = 2.45f;
                    mainLight.color = new Color(1.00f, 0.93f, 0.78f, 1f);
                    mainLight.transform.rotation = Quaternion.Euler(43f, -38f, 0f);
                    mainLight.cookie = EnsureCentralPlazaSunCookieTexture();
                    mainLight.cookieSize = CentralPlazaSunCookieWorldSize;
                }
                else if (mainLight.cookie != null && mainLight.cookie.name == CentralPlazaSunCookieName)
                {
                    mainLight.cookie = null;
                }
            }

            if (sceneCamera != null && (area == FastVsHouseArea.Exterior || area == FastVsHouseArea.CentralPlaza))
            {
                if (area == FastVsHouseArea.CentralPlaza && TryApplyCentralPlazaSkybox())
                {
                    sceneCamera.clearFlags = CameraClearFlags.Skybox;
                }
                else
                {
                    sceneCamera.clearFlags = CameraClearFlags.SolidColor;
                    sceneCamera.backgroundColor = area == FastVsHouseArea.CentralPlaza ? centralPlazaSkyColor : exteriorSkyColor;
                }
            }

            if (area == FastVsHouseArea.Exterior || area == FastVsHouseArea.CentralPlaza)
            {
                RenderSettings.fog = false;
                RenderSettings.ambientMode = AmbientMode.Flat;
            }

            if (isCentralPlaza)
            {
                RenderSettings.ambientLight = new Color(0.055f, 0.050f, 0.043f, 1f);
                RenderSettings.reflectionIntensity = 0f;
                SetCycle128CameraGradeActive(false);
            }
            else
            {
                RenderSettings.reflectionIntensity = 1f;
                SetCycle128CameraGradeActive(false);
            }
        }

        private bool TryApplyCentralPlazaSkybox()
        {
            var skybox = EnsureCentralPlazaSkyboxMaterial();
            if (skybox == null)
            {
                return false;
            }

            RenderSettings.skybox = skybox;
            sceneCamera.backgroundColor = centralPlazaSkyColor;
            return true;
        }

        private Material EnsureCentralPlazaSkyboxMaterial()
        {
            if (centralPlazaSkyboxMaterial != null)
            {
                return centralPlazaSkyboxMaterial;
            }

            var shader = Shader.Find("Skybox/Procedural");
            if (shader == null)
            {
                return null;
            }

            centralPlazaSkyboxMaterial = new Material(shader)
            {
                name = "FastVS_CentralPlazaRuntimeSkyboxCycle156",
                hideFlags = HideFlags.DontSave
            };

            if (centralPlazaSkyboxMaterial.HasProperty("_SkyTint"))
            {
                centralPlazaSkyboxMaterial.SetColor("_SkyTint", new Color(0.42f, 0.50f, 0.56f, 1f));
            }

            if (centralPlazaSkyboxMaterial.HasProperty("_GroundColor"))
            {
                centralPlazaSkyboxMaterial.SetColor("_GroundColor", new Color(0.18f, 0.20f, 0.20f, 1f));
            }

            if (centralPlazaSkyboxMaterial.HasProperty("_AtmosphereThickness"))
            {
                centralPlazaSkyboxMaterial.SetFloat("_AtmosphereThickness", 0.62f);
            }

            if (centralPlazaSkyboxMaterial.HasProperty("_Exposure"))
            {
                centralPlazaSkyboxMaterial.SetFloat("_Exposure", 0.56f);
            }

            if (centralPlazaSkyboxMaterial.HasProperty("_SunSize"))
            {
                centralPlazaSkyboxMaterial.SetFloat("_SunSize", 0.018f);
            }

            if (centralPlazaSkyboxMaterial.HasProperty("_SunSizeConvergence"))
            {
                centralPlazaSkyboxMaterial.SetFloat("_SunSizeConvergence", 4.0f);
            }

            return centralPlazaSkyboxMaterial;
        }

        private Texture2D EnsureCentralPlazaSunCookieTexture()
        {
            if (centralPlazaSunCookieTexture != null)
            {
                return centralPlazaSunCookieTexture;
            }

            centralPlazaSunCookieTexture = new Texture2D(
                CentralPlazaSunCookieSize,
                CentralPlazaSunCookieSize,
                TextureFormat.RGBA32,
                false,
                true)
            {
                name = CentralPlazaSunCookieName,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
                hideFlags = HideFlags.DontSave
            };

            var pixels = new Color[CentralPlazaSunCookieSize * CentralPlazaSunCookieSize];
            for (var y = 0; y < CentralPlazaSunCookieSize; y++)
            {
                var v = y / (float)(CentralPlazaSunCookieSize - 1);
                for (var x = 0; x < CentralPlazaSunCookieSize; x++)
                {
                    var u = x / (float)(CentralPlazaSunCookieSize - 1);
                    var luma = SampleCentralPlazaSunCookieLuma(u, v);
                    pixels[(y * CentralPlazaSunCookieSize) + x] = new Color(luma, luma, luma, 1f);
                }
            }

            centralPlazaSunCookieTexture.SetPixels(pixels);
            centralPlazaSunCookieTexture.Apply(false, true);
            return centralPlazaSunCookieTexture;
        }

        private static float SampleCentralPlazaSunCookieLuma(float u, float v)
        {
            var leafNoiseA = Hash01(Mathf.FloorToInt(u * 20f), Mathf.FloorToInt(v * 20f), 1471);
            var leafNoiseB = Hash01(Mathf.FloorToInt((u + v) * 29f), Mathf.FloorToInt((v - u) * 29f), 1472);
            var leafNoiseC = Hash01(Mathf.FloorToInt(u * 9f), Mathf.FloorToInt(v * 9f), 1473);
            var dapple = Mathf.SmoothStep(0.36f, 0.84f, (leafNoiseA * 0.54f) + (leafNoiseB * 0.34f) + (leafNoiseC * 0.12f));
            var vignette = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((u * (1f - u) * v * (1f - v)) * 15.5f));
            var softSunField = Mathf.Lerp(0.72f, 1.0f, dapple);
            var broadVariation = Mathf.Lerp(0.92f, 1.03f, leafNoiseC);
            return Mathf.Clamp01(softSunField * broadVariation * Mathf.Lerp(0.94f, 1.02f, vignette));
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

        private void ApplyRendererShadowPolicy()
        {
            var isCentralPlaza = areaVisibility != null && areaVisibility.ActiveAreaForReview == FastVsHouseArea.CentralPlaza;
            foreach (var renderer in FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (renderer == null || !renderer.gameObject.scene.IsValid())
                {
                    continue;
                }

                if (ShouldForceDisableRenderer(renderer))
                {
                    renderer.enabled = false;
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                    continue;
                }

                if (renderer.gameObject.name.Contains("RealtimeShadowCasterCycle"))
                {
                    renderer.enabled = true;
                    renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                    renderer.receiveShadows = false;
                    continue;
                }

                if (ShouldReceiveRealtimeSurfaceShadow(renderer))
                {
                    renderer.enabled = true;
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.receiveShadows = true;
                    if (isCentralPlaza)
                    {
                        ApplyCentralPlazaSurfaceRealtimeGrade(renderer);
                    }
                    continue;
                }

                var role = GetMaterialRole(renderer.sharedMaterial);
                if (role == SurfaceLitRole)
                {
                    ApplySurfaceShadowPolicy(renderer, isCentralPlaza);
                    if (isCentralPlaza)
                    {
                        ApplyCentralPlazaSurfaceRealtimeGrade(renderer);
                    }
                }
                else if (role == SpriteCardRole || role == PaperCardRole)
                {
                    renderer.shadowCastingMode = ShadowCastingMode.On;
                    renderer.receiveShadows = true;
                    ApplySpriteRealtimeGrade(renderer, isCentralPlaza);
                }
                else if (role == PortalWindowRole)
                {
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                }
            }
        }

        private static void ApplySurfaceShadowPolicy(Renderer renderer, bool isCentralPlaza)
        {
            renderer.enabled = true;
            if (ShouldReceiveRealtimeSurfaceShadow(renderer))
            {
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = true;
                return;
            }

            if (isCentralPlaza && ShouldCastVisibleCentralPlazaRealtimeShadow(renderer))
            {
                renderer.shadowCastingMode = ShadowCastingMode.On;
                renderer.receiveShadows = true;
                ApplyCentralPlazaSurfaceRealtimeGrade(renderer);
                return;
            }

            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        private static bool ShouldForceDisableRenderer(Renderer renderer)
        {
            if (renderer is ParticleSystemRenderer)
            {
                return true;
            }

            var name = renderer.gameObject.name;
            if (name.Contains("MapMoveGlowPad"))
            {
                return false;
            }

            if (ShouldSuppressCentralPlazaReferenceOverlay(name))
            {
                return true;
            }

            if (renderer.GetComponentInParent<FastVsHd2dOverlayProfile>(true) != null)
            {
                return true;
            }

            var role = GetMaterialRole(renderer.sharedMaterial);
            if (role == OverlayGlowRole || role == ContactShadowRole)
            {
                return true;
            }

            return name.Contains("Sky") ||
                   name.Contains("Backdrop") ||
                   name.Contains("SunDisc") ||
                   name.Contains("SkyVeil") ||
                   name.Contains("HouseSkyBarMask") ||
                   name.Contains("OutdoorVoidBackground") ||
                   name.Contains("ScenicBackdrop") ||
                   name.Contains("Current_CentralPlaza_Cycle103_") ||
                   name.Contains("Current_CentralPlaza_Cycle104_") ||
                   name.Contains("Current_CentralPlaza_Cycle106_") ||
                   name.Contains("Current_CentralPlaza_Cycle107_") ||
                   name.Contains("Current_CentralPlaza_Cycle108_") ||
                   name.Contains("Current_CentralPlaza_Cycle109_") ||
                   name.Contains("Current_CentralPlaza_Cycle111_") ||
                   name.Contains("Current_CentralPlaza_Cycle112_") ||
                   name.Contains("Current_CentralPlaza_Cycle113_") ||
                   name.Contains("Current_CentralPlaza_Cycle114_") ||
                   name.Contains("Current_CentralPlaza_Cycle116_") ||
                   name.Contains("Current_CentralPlaza_Cycle117_") ||
                   name.Contains("Current_CentralPlaza_Cycle118_") ||
                   name.Contains("Current_CentralPlaza_Cycle119_") ||
                   name.Contains("Current_CentralPlaza_Cycle120_") ||
                   name.Contains("Current_CentralPlaza_Cycle121_") ||
                   name.Contains("Current_CentralPlaza_Cycle122_") ||
                   name.Contains("Current_CentralPlaza_Cycle123_") ||
                   name.Contains("Current_CentralPlaza_Cycle124_") ||
                   name.Contains("Current_CentralPlaza_Cycle125_") ||
                   name.Contains("Current_CentralPlaza_Cycle126_") ||
                   name.Contains("Current_CentralPlaza_Cycle129Painted") ||
                   name.Contains("Current_CentralPlaza_Cycle130Reference") ||
                   name.Contains("FastVS_Cycle128GradePlate") ||
                   name.Contains("FastVS_Cycle128RayPlate") ||
                   name.Contains("FastVS_Cycle131ShadowPaintPlate") ||
                   name.Contains("FastVS_Cycle131SunPaintPlate") ||
                   name.Contains("PaintedSoftShadow") ||
                   name.Contains("PaintedLightFleck") ||
                   name.Contains("Sunbeam") ||
                   name.Contains("Sunbreak") ||
                   name.Contains("Sunlit") ||
                   name.Contains("SunSlash") ||
                   name.Contains("SolarReset") ||
                   name.Contains("LightColumn") ||
                   name.Contains("LightComposition") ||
                   name.Contains("FramedLight") ||
                   name.Contains("ShadowReceiverField") ||
                   name.Contains("ShadowPenumbra") ||
                   name.Contains("ShadowMidtoneLift") ||
                   name.Contains("CastShadowContrast") ||
                   name.Contains("ReferenceComposite") ||
                   name.Contains("ReferenceSurfaceRemap") ||
                   name.Contains("ReferenceFocusShadow") ||
                   name.Contains("ReferenceDioramaShadow") ||
                   name.Contains("CloseShadowBarMute") ||
                   name.Contains("AerialHaze") ||
                   name.Contains("AirVeil") ||
                   name.Contains("AirFacade") ||
                   name.Contains("AirForeground") ||
                   name.Contains("SkyWash");
        }

        private static bool ShouldSuppressCentralPlazaReferenceOverlay(string name)
        {
            return name.Contains("Current_CentralPlaza_Cycle130ReferenceSunPatchA") ||
                   name.Contains("Current_CentralPlaza_Cycle129PaintedLightFlecksA") ||
                   name.Contains("Current_CentralPlaza_Cycle121");
        }

        private static bool ShouldReceiveRealtimeSurfaceShadow(Renderer renderer)
        {
            var surface = renderer.GetComponent<FastVsHd2dSurfaceProfile>();
            if (surface != null &&
                (surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Floor ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Ground ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Road ||
                 (surface.AreaIdForReview == FastVsHouseArea.CentralPlaza &&
                  surface.IsCurrentWorldForReview &&
                  (surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Wall ||
                   surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Door ||
                   surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Roof))))
            {
                return true;
            }

            var name = renderer.gameObject.name;
            if (IsCurrentCentralPlazaRealtimeFacadeReceiverName(name))
            {
                return true;
            }

            if (name.Contains("Shadow") ||
                name.Contains("Light") ||
                name.Contains("Sun") ||
                name.Contains("Air") ||
                name.Contains("Haze") ||
                name.Contains("Veil") ||
                name.Contains("Wash") ||
                name.Contains("Sky") ||
                name.Contains("Backdrop") ||
                name.Contains("Wall") ||
                name.Contains("Roof") ||
                name.Contains("Facade") ||
                name.Contains("Wing") ||
                name.Contains("Volume") ||
                name.Contains("Door") ||
                name.Contains("Window") ||
                name.Contains("Mullion") ||
                name.Contains("Pane") ||
                name.Contains("Post") ||
                name.Contains("Board") ||
                name.Contains("Tree") ||
                name.Contains("Silhouette") ||
                name.Contains("Contact") ||
                name.Contains("Seam") ||
                name.Contains("Skirt") ||
                name.Contains("Street") ||
                name.Contains("Field") ||
                name.Contains("Shelf") ||
                name.Contains("Shoulder") ||
                name.Contains("Horizon") ||
                name.Contains("Continuation") ||
                name.Contains("Strip"))
            {
                return false;
            }

            return name.Contains("Ground") ||
                   name.Contains("StoneSquare") ||
                   name.Contains("Road") ||
                   name.Contains("Path") ||
                   name.Contains("Paving") ||
                   name.Contains("FloorJoint") ||
                   name.Contains("Approach") ||
                   name.Contains("Step") ||
                   name.Contains("Paver") ||
                   name.Contains("Curb") ||
                   name.Contains("Apron") ||
                   name.Contains("Pebble") ||
                   name.Contains("DustScuff") ||
                   name.Contains("FountainDryBasinInnerFloor");
        }

        private static bool ShouldCastVisibleCentralPlazaRealtimeShadow(Renderer renderer)
        {
            var name = renderer.gameObject.name;
            var surface = renderer.GetComponent<FastVsHd2dSurfaceProfile>();
            if (surface != null &&
                surface.AreaIdForReview == FastVsHouseArea.CentralPlaza &&
                surface.IsCurrentWorldForReview &&
                (surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Furniture ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Prop ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Window ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Bookshelf))
            {
                return !IsSuppressedCentralPlazaVisibleCasterName(name);
            }

            return IsCurrentCentralPlazaVisibleCasterName(name);
        }

        private static bool IsCurrentCentralPlazaVisibleCasterName(string name)
        {
            if (string.IsNullOrEmpty(name) ||
                !name.Contains("Current_CentralPlaza") ||
                IsSuppressedCentralPlazaVisibleCasterName(name))
            {
                return false;
            }

            return name.Contains("Fountain") ||
                   name.Contains("Market") ||
                   name.Contains("Plank") ||
                   name.Contains("RimChip") ||
                   name.Contains("StoneChip") ||
                   name.Contains("ThresholdChip") ||
                   name.Contains("Crack") ||
                   name.Contains("Shard") ||
                   name.Contains("Pebble") ||
                   name.Contains("Board") ||
                   name.Contains("Paper") ||
                   name.Contains("Post") ||
                   name.Contains("Crate") ||
                   name.Contains("Rubble") ||
                   name.Contains("Brick") ||
                   name.Contains("Frame") ||
                   name.Contains("Trim") ||
                   name.Contains("Mullion") ||
                   name.Contains("Pane") ||
                   name.Contains("Window") ||
                   name.Contains("KickPlate") ||
                   name.Contains("Hinge") ||
                   name.Contains("DoorNail");
        }

        private static bool IsSuppressedCentralPlazaVisibleCasterName(string name)
        {
            return string.IsNullOrEmpty(name) ||
                   name.Contains("Shadow") ||
                   name.Contains("Light") ||
                   name.Contains("Sun") ||
                   name.Contains("Air") ||
                   name.Contains("Haze") ||
                   name.Contains("Veil") ||
                   name.Contains("Wash") ||
                   name.Contains("Sky") ||
                   name.Contains("Backdrop") ||
                   name.Contains("Glow") ||
                   name.Contains("Occlusion") ||
                   name.Contains("Water") ||
                   name.Contains("Horizon") ||
                   name.Contains("Silhouette") ||
                   name.Contains("Field") ||
                   name.Contains("WorldEnvelope") ||
                   name.Contains("RealtimeShadowCasterCycle") ||
                   name.Contains("Painted") ||
                   name.Contains("Reference") ||
                   name.Contains("FramedLight") ||
                   name.Contains("LightComposition");
        }

        private static void ApplyCentralPlazaSurfaceRealtimeGrade(Renderer renderer)
        {
            var surface = renderer.GetComponent<FastVsHd2dSurfaceProfile>();
            var objectName = renderer.gameObject.name;
            var isCentralSurface =
                surface != null &&
                surface.AreaIdForReview == FastVsHouseArea.CentralPlaza &&
                surface.IsCurrentWorldForReview;
            var isNamedFacadeReceiver = IsCurrentCentralPlazaRealtimeFacadeReceiverName(objectName);
            var isNamedVisibleCaster = IsCurrentCentralPlazaVisibleCasterName(objectName);
            if (!isCentralSurface && !isNamedFacadeReceiver && !isNamedVisibleCaster)
            {
                return;
            }

            var isFacadeReceiver =
                isNamedFacadeReceiver ||
                surface != null &&
                (surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Wall ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Door ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Roof);

            var material = renderer.sharedMaterial;
            if (material == null)
            {
                return;
            }

            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            if (material.HasProperty(SurfaceRampStrengthId))
            {
                block.SetFloat(SurfaceRampStrengthId, 0.45f);
            }

            if (material.HasProperty(DirectionalLightStrengthId))
            {
                block.SetFloat(DirectionalLightStrengthId, 0.70f);
            }

            if (material.HasProperty(ShadowReceiveStrengthId))
            {
                block.SetFloat(ShadowReceiveStrengthId, 0.66f);
            }

            if (material.HasProperty(ShadowTextureStrengthId))
            {
                block.SetFloat(ShadowTextureStrengthId, isFacadeReceiver ? 0.48f : 0.36f);
            }

            if (material.HasProperty(TopLightId))
            {
                block.SetColor(TopLightId, CentralPlazaTopLight);
            }

            if (material.HasProperty(SideShadeId))
            {
                block.SetColor(SideShadeId, CentralPlazaSideShade);
            }

            if (material.HasProperty(FloorShadeId))
            {
                block.SetColor(FloorShadeId, CentralPlazaFloorShade);
            }

            renderer.SetPropertyBlock(block);
        }

        private static void ApplySpriteRealtimeGrade(Renderer renderer, bool isCentralPlaza)
        {
            var material = renderer.sharedMaterial;
            if (material == null)
            {
                return;
            }

            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);

            if (material.HasProperty(RampStrengthId))
            {
                block.SetFloat(RampStrengthId, isCentralPlaza ? 0.24f : 0.18f);
            }

            if (material.HasProperty(WorldLightStrengthId))
            {
                block.SetFloat(WorldLightStrengthId, isCentralPlaza ? 0.18f : 0.10f);
            }

            if (material.HasProperty(WorldShadowReceiveStrengthId))
            {
                block.SetFloat(WorldShadowReceiveStrengthId, isCentralPlaza ? 0.17f : 0.11f);
            }

            renderer.SetPropertyBlock(block);
        }

        private static bool IsCurrentCentralPlazaRealtimeFacadeReceiverName(string name)
        {
            if (string.IsNullOrEmpty(name) || !name.Contains("Current_CentralPlaza"))
            {
                return false;
            }

            if (name.Contains("Shadow") ||
                name.Contains("Light") ||
                name.Contains("Sun") ||
                name.Contains("Sky") ||
                name.Contains("Backdrop") ||
                name.Contains("Haze") ||
                name.Contains("Veil") ||
                name.Contains("Wash") ||
                name.Contains("Window") ||
                name.Contains("Mullion") ||
                name.Contains("Pane"))
            {
                return false;
            }

            return name.Contains("Wall") ||
                   name.Contains("Facade") ||
                   name.Contains("Door") ||
                   name.Contains("Roof") ||
                   name.Contains("Lintel") ||
                   name.Contains("Eave") ||
                   name.Contains("Post") ||
                   name.Contains("Board");
        }

        private static string GetMaterialRole(Material material)
        {
            return material != null ? material.GetTag(MaterialRoleTagName, false, string.Empty) : string.Empty;
        }

        private void EnsureCycle128CameraGrade()
        {
            if (sceneCamera == null)
            {
                return;
            }

            if (cycle128GradeRoot == null)
            {
                cycle128GradeRoot = new GameObject("FastVS_Cycle128GradeRoot");
                cycle128GradeRoot.hideFlags = HideFlags.DontSave;
                cycle128GradeRoot.transform.SetParent(sceneCamera.transform, false);
                cycle128GradeRoot.transform.localPosition = Vector3.zero;
                cycle128GradeRoot.transform.localRotation = Quaternion.identity;
                cycle128GradeRoot.transform.localScale = Vector3.one;
            }

            if (cycle128GradeRenderer == null)
            {
                cycle128GradeRenderer = CreateCycle128CameraQuad("FastVS_Cycle128GradePlate", 0.46f, EnsureCycle128GradeMaterial());
            }

            if (cycle128BeamRenderer == null)
            {
                cycle128BeamRenderer = CreateCycle128CameraQuad("FastVS_Cycle128RayPlate", 0.455f, EnsureCycle128BeamMaterial());
            }
        }

        private void EnsureCycle131CameraPaint()
        {
            EnsureCycle128CameraGrade();

            if (cycle131ShadowPaintRenderer == null)
            {
                cycle131ShadowPaintRenderer = CreateCycle128CameraQuad("FastVS_Cycle131ShadowPaintPlate", 0.450f, EnsureCycle131ShadowPaintMaterial());
            }

            if (cycle131SunPaintRenderer == null)
            {
                cycle131SunPaintRenderer = CreateCycle128CameraQuad("FastVS_Cycle131SunPaintPlate", 0.445f, EnsureCycle131SunPaintMaterial());
            }
        }

        private MeshRenderer CreateCycle128CameraQuad(string objectName, float localZ, Material material)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = objectName;
            quad.hideFlags = HideFlags.DontSave;
            quad.transform.SetParent(cycle128GradeRoot.transform, false);
            quad.transform.localPosition = new Vector3(0f, 0f, localZ);
            quad.transform.localRotation = Quaternion.identity;
            quad.transform.localScale = Vector3.one;

            var collider = quad.GetComponent<Collider>();
            if (collider != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(collider);
                }
                else
                {
                    DestroyImmediate(collider);
                }
            }

            var renderer = quad.GetComponent<MeshRenderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.enabled = true;
            return renderer;
        }

        private Material EnsureCycle128GradeMaterial()
        {
            if (cycle128GradeMaterial == null)
            {
                cycle128GradeMaterial = CreateCycle128TransparentMaterial("FastVS_Cycle128GradeMaterial", EnsureCycle128GradeTexture(), 5000);
            }

            return cycle128GradeMaterial;
        }

        private Material EnsureCycle128BeamMaterial()
        {
            if (cycle128BeamMaterial == null)
            {
                cycle128BeamMaterial = CreateCycle128TransparentMaterial("FastVS_Cycle128RayMaterial", EnsureCycle128BeamTexture(), 5010);
            }

            return cycle128BeamMaterial;
        }

        private Material EnsureCycle131ShadowPaintMaterial()
        {
            if (cycle131ShadowPaintMaterial == null)
            {
                cycle131ShadowPaintMaterial = CreateCycle128TransparentMaterial("FastVS_Cycle131ShadowPaintMaterial", EnsureCycle131ShadowPaintTexture(), 5020);
            }

            return cycle131ShadowPaintMaterial;
        }

        private Material EnsureCycle131SunPaintMaterial()
        {
            if (cycle131SunPaintMaterial == null)
            {
                cycle131SunPaintMaterial = CreateCycle128TransparentMaterial("FastVS_Cycle131SunPaintMaterial", EnsureCycle131SunPaintTexture(), 5030);
            }

            return cycle131SunPaintMaterial;
        }

        private static Material CreateCycle128TransparentMaterial(string materialName, Texture2D texture, int renderQueue)
        {
            var shader = Shader.Find("Universal Render Pipeline/Unlit");
            if (shader == null)
            {
                shader = Shader.Find("Unlit/Transparent");
            }

            if (shader == null)
            {
                shader = Shader.Find("Sprites/Default");
            }

            var material = new Material(shader)
            {
                name = materialName,
                hideFlags = HideFlags.DontSave,
                renderQueue = renderQueue,
                doubleSidedGI = false
            };

            if (material.HasProperty("_BaseMap"))
            {
                material.SetTexture("_BaseMap", texture);
            }

            if (material.HasProperty("_MainTex"))
            {
                material.SetTexture("_MainTex", texture);
            }

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", Color.white);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", Color.white);
            }

            if (material.HasProperty("_Surface"))
            {
                material.SetFloat("_Surface", 1f);
            }

            if (material.HasProperty("_Blend"))
            {
                material.SetFloat("_Blend", 0f);
            }

            if (material.HasProperty("_SrcBlend"))
            {
                material.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
            }

            if (material.HasProperty("_DstBlend"))
            {
                material.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
            }

            if (material.HasProperty("_ZWrite"))
            {
                material.SetFloat("_ZWrite", 0f);
            }

            if (material.HasProperty("_ZTest"))
            {
                material.SetFloat("_ZTest", (float)CompareFunction.Always);
            }

            if (material.HasProperty("_Cull"))
            {
                material.SetFloat("_Cull", 0f);
            }

            material.SetOverrideTag("RenderType", "Transparent");
            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", false);
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            return material;
        }

        private Texture2D EnsureCycle128GradeTexture()
        {
            if (cycle128GradeTexture != null)
            {
                return cycle128GradeTexture;
            }

            cycle128GradeTexture = new Texture2D(512, 288, TextureFormat.RGBA32, false)
            {
                name = "FastVS_Cycle128GradeTexture",
                hideFlags = HideFlags.DontSave,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            for (var y = 0; y < cycle128GradeTexture.height; y++)
            {
                for (var x = 0; x < cycle128GradeTexture.width; x++)
                {
                    var u = (x + 0.5f) / cycle128GradeTexture.width;
                    var v = (y + 0.5f) / cycle128GradeTexture.height;
                    var dx = (u - 0.50f) * 2f;
                    var dy = (v - 0.48f) * 2f;
                    var radius = Mathf.Sqrt(dx * dx * 0.90f + dy * dy * 1.18f);
                    var edge = Mathf.SmoothStep(0.42f, 1.05f, radius) * 0.16f;
                    var lowerShade = Mathf.SmoothStep(0.38f, 0.92f, v) * 0.13f;
                    var topOcclusion = Mathf.SmoothStep(0.86f, 0.18f, v) * 0.06f;
                    var diagonalA = Mathf.SmoothStep(0.22f, 0.012f, Mathf.Abs((v - 0.35f) - (u - 0.50f) * -0.28f)) * 0.18f;
                    var diagonalB = Mathf.SmoothStep(0.18f, 0.014f, Mathf.Abs((v - 0.61f) - (u - 0.42f) * -0.38f)) * 0.14f;
                    var canopyNoise = Mathf.PerlinNoise((u * 8.8f) + 14.2f, (v * 7.1f) + 3.4f);
                    var fineNoise = Mathf.PerlinNoise((u * 29.0f) + 1.2f, (v * 24.0f) + 9.7f);
                    var dapple = Mathf.SmoothStep(0.58f, 0.86f, (canopyNoise * 0.72f) + (fineNoise * 0.28f)) * 0.055f;
                    var playerClear = SoftEllipseCycle131(u, v, 0.50f, 0.42f, 0.15f, 0.16f, 0f, 0.13f);
                    var alpha = Mathf.Clamp01(edge + lowerShade + topOcclusion + diagonalA + diagonalB + dapple - playerClear);
                    cycle128GradeTexture.SetPixel(x, y, new Color(0.012f, 0.014f, 0.012f, Mathf.Min(alpha, 0.38f)));
                }
            }

            cycle128GradeTexture.Apply(false, true);
            return cycle128GradeTexture;
        }

        private Texture2D EnsureCycle128BeamTexture()
        {
            if (cycle128BeamTexture != null)
            {
                return cycle128BeamTexture;
            }

            cycle128BeamTexture = new Texture2D(256, 144, TextureFormat.RGBA32, false)
            {
                name = "FastVS_Cycle128RayTexture",
                hideFlags = HideFlags.DontSave,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            for (var y = 0; y < cycle128BeamTexture.height; y++)
            {
                for (var x = 0; x < cycle128BeamTexture.width; x++)
                {
                    var u = (x + 0.5f) / cycle128BeamTexture.width;
                    var v = (y + 0.5f) / cycle128BeamTexture.height;
                    var rayA = Mathf.Abs((u - 0.05f) - (1f - v) * 0.58f);
                    var rayB = Mathf.Abs((u - 0.34f) - (1f - v) * 0.46f);
                    var beam = Mathf.SmoothStep(0.080f, 0.010f, rayA) * 0.11f + Mathf.SmoothStep(0.070f, 0.010f, rayB) * 0.08f;
                    beam *= Mathf.SmoothStep(0.08f, 0.48f, v) * Mathf.SmoothStep(1.00f, 0.68f, v);
                    beam *= Mathf.SmoothStep(1.04f, 0.58f, u);
                    cycle128BeamTexture.SetPixel(x, y, new Color(1.00f, 0.96f, 0.78f, beam));
                }
            }

            cycle128BeamTexture.Apply(false, true);
            return cycle128BeamTexture;
        }

        private Texture2D EnsureCycle131ShadowPaintTexture()
        {
            if (cycle131ShadowPaintTexture != null)
            {
                return cycle131ShadowPaintTexture;
            }

            cycle131ShadowPaintTexture = new Texture2D(320, 180, TextureFormat.RGBA32, false)
            {
                name = "FastVS_Cycle131ShadowPaintTexture",
                hideFlags = HideFlags.DontSave,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            for (var y = 0; y < cycle131ShadowPaintTexture.height; y++)
            {
                for (var x = 0; x < cycle131ShadowPaintTexture.width; x++)
                {
                    var u = (x + 0.5f) / cycle131ShadowPaintTexture.width;
                    var v = (y + 0.5f) / cycle131ShadowPaintTexture.height;
                    var diagonalA = Mathf.SmoothStep(0.24f, 0.012f, Mathf.Abs((v - 0.35f) - (u - 0.50f) * -0.30f)) * 0.24f;
                    var diagonalB = Mathf.SmoothStep(0.19f, 0.014f, Mathf.Abs((v - 0.60f) - (u - 0.45f) * -0.42f)) * 0.20f;
                    var lowerContact = Mathf.SmoothStep(0.50f, 0.10f, v) * 0.16f;
                    var sideVignette = Mathf.SmoothStep(0.58f, 1.06f, Mathf.Abs(u - 0.50f) * 2f) * 0.12f;
                    var dappleNoise = Mathf.PerlinNoise((u * 17.0f) + 5.1f, (v * 14.0f) + 8.7f);
                    var dapple = Mathf.SmoothStep(0.58f, 0.86f, dappleNoise) * 0.060f;
                    var alpha = Mathf.Clamp01(diagonalA + diagonalB + lowerContact + sideVignette + dapple);
                    cycle131ShadowPaintTexture.SetPixel(x, y, new Color(0.010f, 0.012f, 0.010f, Mathf.Min(alpha, 0.44f)));
                }
            }

            cycle131ShadowPaintTexture.Apply(false, true);
            return cycle131ShadowPaintTexture;
        }

        private Texture2D EnsureCycle131SunPaintTexture()
        {
            if (cycle131SunPaintTexture != null)
            {
                return cycle131SunPaintTexture;
            }

            cycle131SunPaintTexture = new Texture2D(320, 180, TextureFormat.RGBA32, false)
            {
                name = "FastVS_Cycle131SunPaintTexture",
                hideFlags = HideFlags.DontSave,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            for (var y = 0; y < cycle131SunPaintTexture.height; y++)
            {
                for (var x = 0; x < cycle131SunPaintTexture.width; x++)
                {
                    var u = (x + 0.5f) / cycle131SunPaintTexture.width;
                    var v = (y + 0.5f) / cycle131SunPaintTexture.height;
                    var patchA = SoftEllipseCycle131(u, v, 0.46f, 0.42f, 0.14f, 0.040f, -12f, 0.070f);
                    var patchB = SoftEllipseCycle131(u, v, 0.64f, 0.31f, 0.10f, 0.036f, -18f, 0.050f);
                    var rayA = Mathf.SmoothStep(0.050f, 0.006f, Mathf.Abs((u - 0.03f) - (1f - v) * 0.55f)) * 0.075f;
                    var rayB = Mathf.SmoothStep(0.055f, 0.007f, Mathf.Abs((u - 0.28f) - (1f - v) * 0.42f)) * 0.055f;
                    var topToMid = Mathf.SmoothStep(0.12f, 0.52f, v) * Mathf.SmoothStep(1.00f, 0.58f, v);
                    var grain = (Mathf.PerlinNoise((u * 34.0f) + 2.2f, (v * 26.0f) + 6.3f) - 0.5f) * 0.018f;
                    var alpha = Mathf.Clamp01(patchA + patchB + (rayA + rayB) * topToMid + grain);
                    cycle131SunPaintTexture.SetPixel(x, y, new Color(1.00f, 0.94f, 0.72f, Mathf.Min(alpha, 0.13f)));
                }
            }

            cycle131SunPaintTexture.Apply(false, true);
            return cycle131SunPaintTexture;
        }

        private static float SoftEllipseCycle131(float u, float v, float centerU, float centerV, float radiusU, float radiusV, float rotationDegrees, float strength)
        {
            var radians = rotationDegrees * Mathf.Deg2Rad;
            var cos = Mathf.Cos(radians);
            var sin = Mathf.Sin(radians);
            var dx = u - centerU;
            var dy = v - centerV;
            var rotatedU = dx * cos + dy * sin;
            var rotatedV = -dx * sin + dy * cos;
            var distance = (rotatedU * rotatedU) / (radiusU * radiusU) + (rotatedV * rotatedV) / (radiusV * radiusV);
            return Mathf.SmoothStep(1.12f, 0.08f, distance) * strength;
        }

        private static float Hash01Cycle131(int x, int y)
        {
            unchecked
            {
                var n = x * 1103515245 ^ y * 12345 ^ 0x45d9f3b;
                n = (n << 13) ^ n;
                return ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 2147483647f;
            }
        }

        private void SetCycle128CameraGradeActive(bool active)
        {
            if (cycle128GradeRoot != null && cycle128GradeRoot.activeSelf != active)
            {
                cycle128GradeRoot.SetActive(active);
            }
        }

        private void UpdateCycle128CameraGradeScale()
        {
            if (sceneCamera == null || cycle128GradeRoot == null)
            {
                return;
            }

            var distance = 0.46f;
            var height = 2f * distance * Mathf.Tan(sceneCamera.fieldOfView * Mathf.Deg2Rad * 0.5f);
            var width = height * sceneCamera.aspect;
            cycle128GradeRoot.transform.localPosition = Vector3.zero;
            cycle128GradeRoot.transform.localRotation = Quaternion.identity;

            if (cycle128GradeRenderer != null)
            {
                cycle128GradeRenderer.transform.localScale = new Vector3(width * 1.06f, height * 1.06f, 1f);
            }

            if (cycle128BeamRenderer != null)
            {
                cycle128BeamRenderer.transform.localScale = new Vector3(width * 1.10f, height * 1.10f, 1f);
            }

            if (cycle131ShadowPaintRenderer != null)
            {
                cycle131ShadowPaintRenderer.transform.localScale = new Vector3(width * 1.12f, height * 1.12f, 1f);
            }

            if (cycle131SunPaintRenderer != null)
            {
                cycle131SunPaintRenderer.transform.localScale = new Vector3(width * 1.12f, height * 1.12f, 1f);
            }
        }
    }
}
