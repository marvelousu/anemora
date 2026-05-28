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
        private static readonly int SurfaceRampStrengthId = Shader.PropertyToID("_SurfaceRampStrength");
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int DirectionalLightStrengthId = Shader.PropertyToID("_DirectionalLightStrength");
        private static readonly int ShadowReceiveStrengthId = Shader.PropertyToID("_ShadowReceiveStrength");
        private static readonly int ShadowTextureStrengthId = Shader.PropertyToID("_ShadowTextureStrength");
        private static readonly int TopLightId = Shader.PropertyToID("_TopLight");
        private static readonly int SideShadeId = Shader.PropertyToID("_SideShade");
        private static readonly int FloorShadeId = Shader.PropertyToID("_FloorShade");
        private static readonly int RampStrengthId = Shader.PropertyToID("_RampStrength");
        private static readonly int WorldLightStrengthId = Shader.PropertyToID("_WorldLightStrength");
        private static readonly int WorldShadowReceiveStrengthId = Shader.PropertyToID("_WorldShadowReceiveStrength");
        private static readonly int EmissionMapId = Shader.PropertyToID("_EmissionMap");
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");
        private static readonly int EmissionIntensityId = Shader.PropertyToID("_EmissionIntensity");
        private static readonly Color CentralPlazaTopLight = new Color(1.06f, 1.04f, 0.94f, 1f);
        private static readonly Color RealtimeOutdoorSideShade = new Color(0.68f, 0.69f, 0.66f, 1f);
        private static readonly Color RealtimeOutdoorFloorShade = new Color(0.64f, 0.65f, 0.61f, 1f);
        private const float CentralPlazaStage7jShadowReceiveStrength = 0.44f;
        private const float CentralPlazaStage7jFacadeShadowTextureStrength = 0.20f;
        private const float CentralPlazaStage7jFloorShadowTextureStrength = 0.18f;
        private const float RealtimeOutdoorShadowReceiveStrength = 0.30f;
        private const float RealtimeOutdoorFacadeShadowTextureStrength = 0.12f;
        private const float RealtimeOutdoorFloorShadowTextureStrength = 0.10f;
        private static readonly Color CentralPlazaStage7jSideShade = new Color(0.62f, 0.63f, 0.60f, 1f);
        private static readonly Color CentralPlazaStage7jFloorShade = new Color(0.58f, 0.59f, 0.55f, 1f);

        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Light mainLight;
        [SerializeField] private bool enforceRendererShadowPolicy = true;
        [SerializeField] private Color exteriorSkyColor = new Color(0.30f, 0.38f, 0.43f, 1f);
        [SerializeField] private Color centralPlazaSkyColor = new Color(0.220f, 0.286f, 0.340f, 1f);

        private float nextShadowPolicyRefreshTime;
        private Material centralPlazaSkyboxMaterial;
        private Material exteriorSkyboxMaterial;

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
            var isRealtimeOutdoor = IsRealtimeOutdoorArea(area);

            if (mainLight != null)
            {
                mainLight.enabled = true;
                mainLight.type = LightType.Directional;
                mainLight.shadows = LightShadows.Soft;
                mainLight.shadowResolution = isRealtimeOutdoor ? LightShadowResolution.VeryHigh : mainLight.shadowResolution;
                mainLight.shadowBias = isRealtimeOutdoor ? 0.012f : Mathf.Min(mainLight.shadowBias, 0.025f);
                mainLight.shadowNormalBias = isRealtimeOutdoor ? 0.10f : Mathf.Min(mainLight.shadowNormalBias, 0.18f);
                mainLight.shadowNearPlane = Mathf.Min(Mathf.Max(mainLight.shadowNearPlane, 0.05f), 0.12f);
            }

            if (sceneCamera != null && (area == FastVsHouseArea.Exterior || area == FastVsHouseArea.CentralPlaza))
            {
                if (TryApplyRuntimeOutdoorSkybox(area))
                {
                    sceneCamera.clearFlags = CameraClearFlags.Skybox;
                }
                else
                {
                    sceneCamera.clearFlags = CameraClearFlags.SolidColor;
                    sceneCamera.backgroundColor = area == FastVsHouseArea.CentralPlaza ? centralPlazaSkyColor : exteriorSkyColor;
                }
            }

            if (isRealtimeOutdoor)
            {
                RenderSettings.reflectionIntensity = 0f;
            }
            else
            {
                RenderSettings.reflectionIntensity = 1f;
            }
        }

        private static bool IsRealtimeOutdoorArea(FastVsHouseArea area)
        {
            return area == FastVsHouseArea.Exterior ||
                   area == FastVsHouseArea.CentralPlaza ||
                   area == FastVsHouseArea.Library;
        }

        private bool TryApplyRuntimeOutdoorSkybox(FastVsHouseArea area)
        {
            if (area == FastVsHouseArea.CentralPlaza)
            {
                return TryApplyCentralPlazaSkybox();
            }

            if (area != FastVsHouseArea.Exterior)
            {
                return false;
            }

            var skybox = EnsureExteriorSkyboxMaterial();
            if (skybox == null)
            {
                return false;
            }

            RenderSettings.skybox = skybox;
            sceneCamera.backgroundColor = exteriorSkyColor;
            return true;
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

        private Material EnsureExteriorSkyboxMaterial()
        {
            if (exteriorSkyboxMaterial != null)
            {
                return exteriorSkyboxMaterial;
            }

            var shader = Shader.Find("Skybox/Procedural");
            if (shader == null)
            {
                return null;
            }

            exteriorSkyboxMaterial = new Material(shader)
            {
                name = "FastVS_ExteriorRuntimeSkyboxCycle162",
                hideFlags = HideFlags.DontSave
            };

            if (exteriorSkyboxMaterial.HasProperty("_SkyTint"))
            {
                exteriorSkyboxMaterial.SetColor("_SkyTint", new Color(0.54f, 0.58f, 0.57f, 1f));
            }

            if (exteriorSkyboxMaterial.HasProperty("_GroundColor"))
            {
                exteriorSkyboxMaterial.SetColor("_GroundColor", new Color(0.40f, 0.43f, 0.39f, 1f));
            }

            if (exteriorSkyboxMaterial.HasProperty("_AtmosphereThickness"))
            {
                exteriorSkyboxMaterial.SetFloat("_AtmosphereThickness", 0.42f);
            }

            if (exteriorSkyboxMaterial.HasProperty("_Exposure"))
            {
                exteriorSkyboxMaterial.SetFloat("_Exposure", 0.68f);
            }

            if (exteriorSkyboxMaterial.HasProperty("_SunSize"))
            {
                exteriorSkyboxMaterial.SetFloat("_SunSize", 0.014f);
            }

            if (exteriorSkyboxMaterial.HasProperty("_SunSizeConvergence"))
            {
                exteriorSkyboxMaterial.SetFloat("_SunSizeConvergence", 4.8f);
            }

            return exteriorSkyboxMaterial;
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
                name = "FastVS_CentralPlazaRuntimeSkyboxCycle157",
                hideFlags = HideFlags.DontSave
            };

            if (centralPlazaSkyboxMaterial.HasProperty("_SkyTint"))
            {
                centralPlazaSkyboxMaterial.SetColor("_SkyTint", new Color(0.50f, 0.56f, 0.61f, 1f));
            }

            if (centralPlazaSkyboxMaterial.HasProperty("_GroundColor"))
            {
                centralPlazaSkyboxMaterial.SetColor("_GroundColor", new Color(0.31f, 0.37f, 0.41f, 1f));
            }

            if (centralPlazaSkyboxMaterial.HasProperty("_AtmosphereThickness"))
            {
                centralPlazaSkyboxMaterial.SetFloat("_AtmosphereThickness", 0.48f);
            }

            if (centralPlazaSkyboxMaterial.HasProperty("_Exposure"))
            {
                centralPlazaSkyboxMaterial.SetFloat("_Exposure", 0.62f);
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

        private void ApplyRendererShadowPolicy()
        {
            var activeArea = areaVisibility != null ? areaVisibility.ActiveAreaForReview : FastVsHouseArea.Interior;
            var isRealtimeOutdoor = IsRealtimeOutdoorArea(activeArea);
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

                if (IsStage7VfxRenderer(renderer))
                {
                    renderer.enabled = true;
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
                    if (isRealtimeOutdoor)
                    {
                        ApplyRealtimeSurfaceGrade(renderer, activeArea);
                    }
                    continue;
                }

                var role = GetMaterialRole(renderer.sharedMaterial);
                if (role == SurfaceLitRole)
                {
                    ApplySurfaceShadowPolicy(renderer, isRealtimeOutdoor, activeArea);
                    if (isRealtimeOutdoor)
                    {
                        ApplyRealtimeSurfaceGrade(renderer, activeArea);
                    }
                }
                else if (role == SpriteCardRole || role == PaperCardRole)
                {
                    renderer.shadowCastingMode = ShadowCastingMode.On;
                    renderer.receiveShadows = true;
                    ApplySpriteRealtimeGrade(renderer, isRealtimeOutdoor);
                }
                else if (role == PortalWindowRole)
                {
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                }
            }
        }

        private static void ApplySurfaceShadowPolicy(Renderer renderer, bool isRealtimeOutdoor, FastVsHouseArea activeArea)
        {
            renderer.enabled = true;
            if (ShouldReceiveRealtimeSurfaceShadow(renderer))
            {
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = true;
                return;
            }

            if (isRealtimeOutdoor && ShouldCastVisibleRealtimeShadow(renderer, activeArea))
            {
                renderer.shadowCastingMode = ShadowCastingMode.On;
                renderer.receiveShadows = true;
                ApplyRealtimeSurfaceGrade(renderer, activeArea);
                return;
            }

            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        private static bool ShouldForceDisableRenderer(Renderer renderer)
        {
            if (IsStage7VfxRenderer(renderer))
            {
                return false;
            }

            if (renderer is ParticleSystemRenderer)
            {
                return true;
            }

            var name = renderer.gameObject.name;
            if (name.Contains("MapMoveGlowPad"))
            {
                return false;
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

        private static bool IsStage7VfxRenderer(Renderer renderer)
        {
            return renderer != null && renderer.gameObject.name.Contains("FastVS_HD2D_Stage7_");
        }

        private static bool ShouldReceiveRealtimeSurfaceShadow(Renderer renderer)
        {
            var surface = renderer.GetComponent<FastVsHd2dSurfaceProfile>();
            if (surface != null &&
                surface.IsCurrentWorldForReview &&
                IsRealtimeOutdoorArea(surface.AreaIdForReview) &&
                (surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Floor ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Ground ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Road ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Wall ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Door ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Roof))
            {
                return true;
            }

            var name = renderer.gameObject.name;
            if (IsCurrentRealtimeFacadeReceiverName(name))
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

        private static bool ShouldCastVisibleRealtimeShadow(Renderer renderer, FastVsHouseArea activeArea)
        {
            var name = renderer.gameObject.name;
            var surface = renderer.GetComponent<FastVsHd2dSurfaceProfile>();
            if (surface != null &&
                surface.AreaIdForReview == activeArea &&
                IsRealtimeOutdoorArea(surface.AreaIdForReview) &&
                surface.IsCurrentWorldForReview &&
                (surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Furniture ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Prop ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Window ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Bookshelf))
            {
                return !IsSuppressedCentralPlazaVisibleCasterName(name);
            }

            return IsCurrentRealtimeVisibleCasterName(name);
        }

        private static bool IsCurrentRealtimeVisibleCasterName(string name)
        {
            if (string.IsNullOrEmpty(name) ||
                !name.Contains("Current_") ||
                IsSuppressedCentralPlazaVisibleCasterName(name))
            {
                return false;
            }

            if (!name.Contains("CentralPlaza") &&
                !name.Contains("HouseExterior") &&
                !name.Contains("Library"))
            {
                return false;
            }

            return IsCurrentCentralPlazaVisibleCasterName(name) ||
                   name.Contains("Tree") ||
                   name.Contains("Crate") ||
                   name.Contains("Board") ||
                   name.Contains("Post") ||
                   name.Contains("Barrel") ||
                   name.Contains("Fence") ||
                   name.Contains("Shelf") ||
                   name.Contains("Book") ||
                   name.Contains("Table") ||
                   name.Contains("Chair") ||
                   name.Contains("Door") ||
                   name.Contains("Window") ||
                   name.Contains("Trim") ||
                   name.Contains("Frame");
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
                   name.Contains("Reference") ||
                   name.Contains("FramedLight") ||
                   name.Contains("LightComposition");
        }

        private static void ApplyRealtimeSurfaceGrade(Renderer renderer, FastVsHouseArea activeArea)
        {
            var surface = renderer.GetComponent<FastVsHd2dSurfaceProfile>();
            var objectName = renderer.gameObject.name;
            var isCentralPlaza = activeArea == FastVsHouseArea.CentralPlaza;
            var isRealtimeSurface =
                surface != null &&
                surface.AreaIdForReview == activeArea &&
                IsRealtimeOutdoorArea(surface.AreaIdForReview) &&
                surface.IsCurrentWorldForReview;
            var isNamedFacadeReceiver = IsCurrentRealtimeFacadeReceiverName(objectName);
            var isNamedFloorReceiver = IsRealtimeFloorReceiverName(objectName);
            var isNamedVisibleCaster = IsCurrentRealtimeVisibleCasterName(objectName);
            if (!isRealtimeSurface && !isNamedFacadeReceiver && !isNamedFloorReceiver && !isNamedVisibleCaster)
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
            if (material.HasProperty(BaseColorId))
            {
                var baseColor = ResolveRealtimeBaseColor(renderer, surface, isFacadeReceiver);
                if (baseColor.a > 0f)
                {
                    block.SetColor(BaseColorId, baseColor);
                }
            }

            if (material.HasProperty(SurfaceRampStrengthId))
            {
                block.SetFloat(SurfaceRampStrengthId, 0.50f);
            }

            if (material.HasProperty(DirectionalLightStrengthId))
            {
                block.SetFloat(DirectionalLightStrengthId, 0.92f);
            }

            if (material.HasProperty(ShadowReceiveStrengthId))
            {
                block.SetFloat(ShadowReceiveStrengthId, isCentralPlaza ? CentralPlazaStage7jShadowReceiveStrength : RealtimeOutdoorShadowReceiveStrength);
            }

            if (material.HasProperty(ShadowTextureStrengthId))
            {
                block.SetFloat(
                    ShadowTextureStrengthId,
                    isCentralPlaza
                        ? (isFacadeReceiver ? CentralPlazaStage7jFacadeShadowTextureStrength : CentralPlazaStage7jFloorShadowTextureStrength)
                        : isFacadeReceiver ? RealtimeOutdoorFacadeShadowTextureStrength : RealtimeOutdoorFloorShadowTextureStrength);
            }

            if (material.HasProperty(TopLightId))
            {
                block.SetColor(TopLightId, CentralPlazaTopLight);
            }

            if (material.HasProperty(SideShadeId))
            {
                block.SetColor(SideShadeId, isCentralPlaza ? CentralPlazaStage7jSideShade : RealtimeOutdoorSideShade);
            }

            if (material.HasProperty(FloorShadeId))
            {
                block.SetColor(FloorShadeId, isCentralPlaza ? CentralPlazaStage7jFloorShade : RealtimeOutdoorFloorShade);
            }

            CopyMaterialEmissionToPropertyBlock(material, block);
            renderer.SetPropertyBlock(block);
        }

        private static Color ResolveRealtimeBaseColor(Renderer renderer, FastVsHd2dSurfaceProfile surface, bool isFacadeReceiver)
        {
            if (surface != null)
            {
                switch (surface.SurfaceKindForReview)
                {
                    case FastVsHd2dSurfaceKind.Floor:
                    case FastVsHd2dSurfaceKind.Ground:
                    case FastVsHd2dSurfaceKind.Road:
                        return new Color(0.66f, 0.68f, 0.74f, 1f);
                    case FastVsHd2dSurfaceKind.Wall:
                    case FastVsHd2dSurfaceKind.Door:
                        return new Color(0.70f, 0.70f, 0.68f, 1f);
                    case FastVsHd2dSurfaceKind.Roof:
                        return new Color(0.66f, 0.64f, 0.60f, 1f);
                    case FastVsHd2dSurfaceKind.Bookshelf when surface.AreaIdForReview == FastVsHouseArea.Library:
                        return new Color(0.48f, 0.42f, 0.35f, 1f);
                    case FastVsHd2dSurfaceKind.Furniture when surface.AreaIdForReview == FastVsHouseArea.Library:
                    case FastVsHd2dSurfaceKind.Prop when surface.AreaIdForReview == FastVsHouseArea.Library:
                        return new Color(0.58f, 0.51f, 0.42f, 1f);
                    case FastVsHd2dSurfaceKind.Window when surface.AreaIdForReview == FastVsHouseArea.Library:
                        return new Color(0.40f, 0.45f, 0.46f, 1f);
                    case FastVsHd2dSurfaceKind.Furniture when surface.AreaIdForReview == FastVsHouseArea.Exterior:
                    case FastVsHd2dSurfaceKind.Prop when surface.AreaIdForReview == FastVsHouseArea.Exterior:
                    case FastVsHd2dSurfaceKind.Window when surface.AreaIdForReview == FastVsHouseArea.Exterior:
                        return new Color(0.56f, 0.52f, 0.45f, 1f);
                }
            }

            var name = renderer != null ? renderer.gameObject.name : string.Empty;
            if (IsRealtimeFloorReceiverName(name))
            {
                return new Color(0.66f, 0.68f, 0.74f, 1f);
            }

            if (isFacadeReceiver ||
                name.Contains("Wall") ||
                name.Contains("Facade") ||
                name.Contains("Door") ||
                name.Contains("Lintel") ||
                name.Contains("Eave"))
            {
                return new Color(0.70f, 0.70f, 0.68f, 1f);
            }

            return new Color(0f, 0f, 0f, 0f);
        }

        private static bool IsRealtimeFloorReceiverName(string name)
        {
            if (string.IsNullOrEmpty(name) || !name.Contains("Current_"))
            {
                return false;
            }

            if (!name.Contains("CentralPlaza") &&
                !name.Contains("HouseExterior") &&
                !name.Contains("Library"))
            {
                return false;
            }

            return name.Contains("Floor") ||
                   name.Contains("Ground") ||
                   name.Contains("Road") ||
                   name.Contains("Path") ||
                   name.Contains("Stone") ||
                   name.Contains("Cobble") ||
                   name.Contains("Pavement") ||
                   name.Contains("Tile") ||
                   name.Contains("Step") ||
                   name.Contains("Terrace") ||
                   name.Contains("Lane") ||
                   name.Contains("Square");
        }

        private static void ApplySpriteRealtimeGrade(Renderer renderer, bool isRealtimeOutdoor)
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
                block.SetFloat(RampStrengthId, isRealtimeOutdoor ? 0.24f : 0.18f);
            }

            if (material.HasProperty(WorldLightStrengthId))
            {
                block.SetFloat(WorldLightStrengthId, isRealtimeOutdoor ? 0.42f : 0.10f);
            }

            if (material.HasProperty(WorldShadowReceiveStrengthId))
            {
                block.SetFloat(WorldShadowReceiveStrengthId, isRealtimeOutdoor ? 0.03f : 0.11f);
            }

            CopyMaterialEmissionToPropertyBlock(material, block);
            renderer.SetPropertyBlock(block);
        }

        private static void CopyMaterialEmissionToPropertyBlock(Material material, MaterialPropertyBlock block)
        {
            if (material == null || block == null)
            {
                return;
            }

            if (material.HasProperty(EmissionMapId))
            {
                var emissionMap = material.GetTexture(EmissionMapId);
                if (emissionMap != null)
                {
                    block.SetTexture(EmissionMapId, emissionMap);
                }
            }

            if (material.HasProperty(EmissionColorId))
            {
                block.SetColor(EmissionColorId, material.GetColor(EmissionColorId));
            }

            if (material.HasProperty(EmissionIntensityId))
            {
                block.SetFloat(EmissionIntensityId, material.GetFloat(EmissionIntensityId));
            }
        }

        private static bool IsCurrentRealtimeFacadeReceiverName(string name)
        {
            if (string.IsNullOrEmpty(name) || !name.Contains("Current_"))
            {
                return false;
            }

            if (!name.Contains("CentralPlaza") &&
                !name.Contains("HouseExterior") &&
                !name.Contains("Library"))
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

    }
}
