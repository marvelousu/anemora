using Anemora.TimeManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

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
        // RecoveryV3 diagnostic toggle: false = capture the native snapshot look (fake-look washes left ON)
        // so we can compare against the wash-stripped build and decide exactly what to keep vs remove.
        private const bool EnableFakeLookWashSuppression = true;
        private static readonly int CharacterBillboardShadowFixId = Shader.PropertyToID("_CharacterBillboardShadowFix");
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
        private const float OutdoorContactHardeningShadowBias = 0.010f;
        private const float OutdoorContactHardeningNormalBias = 0.08f;
        private const float OutdoorContactHardeningNearPlane = 0.06f;
        private const float P1ContactHardeningFallbackShadowYawDegrees = 142f;
        private const float P1ContactHardeningLowPitchDegrees = 18f;
        private const float P1ContactHardeningHighPitchDegrees = 55f;
        private static readonly Color CentralPlazaStage7jSideShade = new Color(0.62f, 0.63f, 0.60f, 1f);
        private static readonly Color CentralPlazaStage7jFloorShade = new Color(0.58f, 0.59f, 0.55f, 1f);

        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Light mainLight;
        [SerializeField] private bool enforceRendererShadowPolicy = true;
        [SerializeField] private Color exteriorSkyColor = new Color(0.30f, 0.38f, 0.43f, 1f);
        [SerializeField] private Color centralPlazaSkyColor = new Color(0.220f, 0.286f, 0.340f, 1f);

        private Material centralPlazaSkyboxMaterial;
        private Material exteriorSkyboxMaterial;

        // RecoveryV3 (2026-06-03): one-shot runtime visibility probe. The ground/buildings were reported
        // rendererVisible=false from the main camera (a render-pipeline issue, not culling). After a few
        // rendered frames, log whether the key opaque geometry is actually visible so we can confirm the
        // depth-priming fix flips them to visible.
        private int _visDiagFrameCounter;
        private bool _visDiagLogged;
        private bool _washSuppressionLogged;

        private void Awake()
        {
            ResolveReferences();
            ApplyNowForReview();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void LateUpdate()
        {
            ResolveReferences();
            ApplyLightAndSky();

            if (Application.isPlaying && !_visDiagLogged)
            {
                _visDiagFrameCounter++;
                if (_visDiagFrameCounter >= 120)
                {
                    _visDiagLogged = true;
                    LogVisibilityDiagnostic();
                }
            }
        }

        private void LogVisibilityDiagnostic()
        {
            var total = 0;
            var visible = 0;
            var key = new System.Text.StringBuilder();
            var keyCount = 0;
            foreach (var r in FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            {
                if (r == null || !r.gameObject.scene.IsValid())
                {
                    continue;
                }

                total++;
                if (r.isVisible)
                {
                    visible++;
                }

                var n = r.gameObject.name;
                var isKey = n.Contains("PixelGround") ||
                            n.Contains("Facade") ||
                            n.Contains("Library") ||
                            n.Contains("BuildingFacade") ||
                            (n.Contains("CentralPlaza") && (n.Contains("Ground") || n.Contains("Wall") || n.Contains("Building")));
                if (isKey && keyCount < 24)
                {
                    keyCount++;
                    key.Append($"{n}[vis={r.isVisible},en={r.enabled},layer={r.gameObject.layer}] ");
                }
            }

            var cam = Camera.main;
            var camInfo = cam != null
                ? $"cam pos={cam.transform.position} fov={cam.fieldOfView} mask={cam.cullingMask}"
                : "cam=null";
            Debug.Log($"[RecoveryV3Vis] frame={_visDiagFrameCounter} activeRenderers={total} isVisibleTrue={visible} | {camInfo} | KEY: {key}");
        }

        public void ApplyNowForReview()
        {
            ResolveReferences();
            ApplyLightAndSky();
            ApplyRendererShadowPolicyForCurrentArea();
        }

        public static float P1ContactHardeningOutdoorShadowBiasForReview => OutdoorContactHardeningShadowBias;
        public static float P1ContactHardeningOutdoorShadowNormalBiasForReview => OutdoorContactHardeningNormalBias;
        public static float P1ContactHardeningOutdoorShadowNearPlaneForReview => OutdoorContactHardeningNearPlane;

        private void HandleSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            ResolveReferences();
            ApplyRendererShadowPolicyForCurrentArea();
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
                if (!Application.isPlaying)
                {
                    mainLight.shadowResolution = isRealtimeOutdoor ? LightShadowResolution.VeryHigh : mainLight.shadowResolution;
                }

                mainLight.shadowBias = isRealtimeOutdoor ? OutdoorContactHardeningShadowBias : Mathf.Min(mainLight.shadowBias, 0.025f);
                mainLight.shadowNormalBias = isRealtimeOutdoor ? OutdoorContactHardeningNormalBias : Mathf.Min(mainLight.shadowNormalBias, 0.18f);
                mainLight.shadowNearPlane = isRealtimeOutdoor ? OutdoorContactHardeningNearPlane : Mathf.Min(Mathf.Max(mainLight.shadowNearPlane, 0.05f), 0.12f);
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

            if (Application.isPlaying && isRealtimeOutdoor)
            {
                ApplyP1ContactHardeningShadowOverlaysForReview();
            }
        }

        public static float GetP1ContactHardeningShadowYawDegreesForReview(Light keyLight)
        {
            if (keyLight == null)
            {
                return P1ContactHardeningFallbackShadowYawDegrees;
            }

            return Mathf.Repeat(keyLight.transform.eulerAngles.y + 270f, 360f);
        }

        public static float GetP1ContactHardeningLowSunFactorForReview(Light keyLight)
        {
            if (keyLight == null)
            {
                return 0.70f;
            }

            var signedPitch = Mathf.Abs(Mathf.DeltaAngle(0f, keyLight.transform.eulerAngles.x));
            return 1f - Mathf.InverseLerp(P1ContactHardeningLowPitchDegrees, P1ContactHardeningHighPitchDegrees, signedPitch);
        }

        public static float GetP1ContactHardeningDirectionalLengthMultiplierForReview(Light keyLight)
        {
            return Mathf.Lerp(0.58f, 1.75f, GetP1ContactHardeningLowSunFactorForReview(keyLight));
        }

        public static float GetP1ContactHardeningStaticLengthMultiplierForReview(Light keyLight)
        {
            return Mathf.Lerp(0.78f, 1.42f, GetP1ContactHardeningLowSunFactorForReview(keyLight));
        }

        public static float GetP1ContactHardeningContactLengthMultiplierForReview(Light keyLight)
        {
            return Mathf.Lerp(0.96f, 1.18f, GetP1ContactHardeningLowSunFactorForReview(keyLight));
        }

        public static bool ApplyP1ContactHardeningOverlayTransformForReview(FastVsHd2dOverlayProfile profile, Light keyLight)
        {
            if (profile == null || keyLight == null)
            {
                return false;
            }

            var yawDegrees = GetP1ContactHardeningShadowYawDegreesForReview(keyLight);
            var lowSunFactor = GetP1ContactHardeningLowSunFactorForReview(keyLight);
            switch (profile.OverlayKindForReview)
            {
                case FastVsHd2dOverlayKind.CharacterDirectionalCastShadow:
                    ApplyP1ContactHardeningOverlayTransform(
                        profile,
                        yawDegrees,
                        GetP1ContactHardeningDirectionalLengthMultiplierForReview(keyLight),
                        Mathf.Lerp(0.90f, 1.08f, lowSunFactor));
                    return true;
                case FastVsHd2dOverlayKind.StaticDirectionalCastShadow:
                    ApplyP1ContactHardeningOverlayTransform(
                        profile,
                        yawDegrees,
                        GetP1ContactHardeningStaticLengthMultiplierForReview(keyLight),
                        Mathf.Lerp(0.92f, 1.12f, lowSunFactor));
                    return true;
                case FastVsHd2dOverlayKind.CharacterContactShadow:
                case FastVsHd2dOverlayKind.CharacterFootContact:
                    ApplyIndependentCharacterContactOverlayTransform(profile);
                    return true;
                default:
                    return false;
            }
        }

        private void ApplyP1ContactHardeningShadowOverlaysForReview()
        {
            if (mainLight == null)
            {
                return;
            }

            var overlays = FindObjectsByType<FastVsHd2dOverlayProfile>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (var i = 0; i < overlays.Length; i++)
            {
                ApplyP1ContactHardeningOverlayTransformForReview(overlays[i], mainLight);
            }
        }

        private static void ApplyP1ContactHardeningOverlayTransform(
            FastVsHd2dOverlayProfile profile,
            float yawDegrees,
            float lengthMultiplier,
            float widthMultiplier)
        {
            var target = profile.transform;
            var baseFootprint = profile.FootprintWorldSizeForReview;
            var baseLength = baseFootprint.x > 0.001f ? baseFootprint.x : Mathf.Abs(target.localScale.x);
            var baseWidth = baseFootprint.y > 0.001f ? baseFootprint.y : Mathf.Abs(target.localScale.y);
            var zScale = Mathf.Abs(target.localScale.z) > 0.001f ? target.localScale.z : 1f;

            target.localRotation = Quaternion.Euler(90f, 0f, yawDegrees);
            target.localScale = new Vector3(baseLength * lengthMultiplier, baseWidth * widthMultiplier, zScale);
        }

        private static void ApplyIndependentCharacterContactOverlayTransform(FastVsHd2dOverlayProfile profile)
        {
            var target = profile.transform;
            var baseFootprint = profile.FootprintWorldSizeForReview;
            var baseLength = baseFootprint.x > 0.001f ? baseFootprint.x : Mathf.Abs(target.localScale.x);
            var baseWidth = baseFootprint.y > 0.001f ? baseFootprint.y : Mathf.Abs(target.localScale.y);
            var zScale = Mathf.Abs(target.localScale.z) > 0.001f ? target.localScale.z : 1f;

            target.localRotation = Quaternion.Euler(90f, 0f, 0f);
            target.localScale = new Vector3(baseLength, baseWidth, zScale);
        }

        private FastVsHouseArea GetActiveAreaForRendererShadowPolicy()
        {
            return areaVisibility != null ? areaVisibility.ActiveAreaForReview : FastVsHouseArea.Interior;
        }

        public void ApplyRendererShadowPolicyForAreaTransitionForReview(FastVsHouseArea area)
        {
            if (!enforceRendererShadowPolicy)
            {
                return;
            }

            ResolveReferences();
            ApplyRendererShadowPolicy(area);
        }

        private void ApplyRendererShadowPolicyForCurrentArea()
        {
            ApplyRendererShadowPolicy(GetActiveAreaForRendererShadowPolicy());
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

        private void ApplyRendererShadowPolicy(FastVsHouseArea activeArea)
        {
            var isRealtimeOutdoor = IsRealtimeOutdoorArea(activeArea);
            var fakeLookWashDisabledCount = 0;
            var totalRenderers = 0;
            var washDisabledNames = new System.Collections.Generic.HashSet<string>();
            foreach (var renderer in FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (renderer == null || !renderer.gameObject.scene.IsValid())
                {
                    continue;
                }

                totalRenderers++;

                // RecoveryV3 (2026-06-03): strip only the annotated pale surface coating over the plaza
                // floor/library facade. Gated on isPlaying so editor bake keeps the lifted receivers and
                // validators still pass; sky, dust, shafts, water, contact shadows, portal and sprite
                // layers are intentionally left enabled.
                if (EnableFakeLookWashSuppression && Application.isPlaying && IsPaleFakeLookSurfaceWashRenderer(renderer))
                {
                    renderer.enabled = false;
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                    fakeLookWashDisabledCount++;
                    if (washDisabledNames.Count < 80)
                    {
                        washDisabledNames.Add(renderer.gameObject.name);
                    }
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

                if (ShouldSuppressLegacyCameraPlate(renderer))
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

                if (IsHd2dOverlayProfileRenderer(renderer))
                {
                    renderer.enabled = true;
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                    continue;
                }

                if (IsRealtimeShadowSafeDetailName(renderer.gameObject.name))
                {
                    renderer.enabled = true;
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
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
                    if (UsesCharacterBillboardShadowFix(renderer))
                    {
                        renderer.shadowCastingMode = ShadowCastingMode.TwoSided;
                        renderer.receiveShadows = false;
                    }
                    else
                    {
                        renderer.shadowCastingMode = ShadowCastingMode.On;
                        renderer.receiveShadows = true;
                    }

                    ApplySpriteRealtimeGrade(renderer, isRealtimeOutdoor);
                }
                else if (role == PortalWindowRole)
                {
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                }
            }

            if (Application.isPlaying && fakeLookWashDisabledCount > 0 && !_washSuppressionLogged)
            {
                _washSuppressionLogged = true;
                Debug.Log($"[RecoveryV3] ApplyRendererShadowPolicy area={activeArea}: total renderers={totalRenderers}, disabled {fakeLookWashDisabledCount} pale fake-look wash renderer(s). Disabled names: {string.Join(" | ", washDisabledNames)}");
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

            if (IsStructuralBackdropRenderer(renderer))
            {
                return false;
            }

            if (renderer is ParticleSystemRenderer)
            {
                return true;
            }

            var name = renderer.gameObject.name;
            if (name.Contains("TreeBillboardLikeTrunk") ||
                name.Contains("TreePixelCrown"))
            {
                return true;
            }

            if (name.Contains("DirectionalCastShadow"))
            {
                return false;
            }

            if (name.Contains("ContactShadow") ||
                name.Contains("FootContact") ||
                name.Contains("GroundBounce"))
            {
                return false;
            }

            if (name.Contains("MapMoveGlowPad"))
            {
                return false;
            }

            return false;
        }

        private static bool IsHd2dOverlayProfileRenderer(Renderer renderer)
        {
            return renderer != null &&
                   renderer.GetComponentInParent<FastVsHd2dOverlayProfile>(true) != null;
        }

        private static bool IsStage7VfxRenderer(Renderer renderer)
        {
            return renderer != null && renderer.gameObject.name.Contains("FastVS_HD2D_Stage7_");
        }

        private static bool ShouldSuppressLegacyCameraPlate(Renderer renderer)
        {
            var landmark = renderer != null ? renderer.GetComponent<TimeWindowPairedSpaceLandmark>() : null;
            var landmarkId = landmark != null ? landmark.LandmarkId : string.Empty;
            if (!string.IsNullOrEmpty(landmarkId) &&
                (landmarkId.Contains(".cycle129.") ||
                 landmarkId.Contains(".cycle130.reference_")))
            {
                return true;
            }

            var name = renderer != null ? renderer.gameObject.name : string.Empty;
            return name.Contains("Current_CentralPlaza") &&
                   (name.Contains("SkyWash") ||
                    name.Contains("SkyVeil") ||
                    name.Contains("Backdrop") ||
                    name.Contains("Haze") ||
                    name.Contains("Veil") ||
                    name.Contains("AirForeground") ||
                    name.Contains("AirFacade"));
        }

        private static bool IsStructuralBackdropRenderer(Renderer renderer)
        {
            if (renderer == null)
            {
                return false;
            }

            var name = renderer.gameObject.name;
            return name.Contains("OutdoorVoidBackground") ||
                   name.Contains("ScenicBackdrop") ||
                   name.Contains("HouseSkyBarMask") ||
                   name.Contains("SkyBarMask") ||
                   name.Contains("BackdropFoundation") ||
                   name.Contains("CompositionSkyBackdrop") ||
                   name.Contains("FacadeBackdropReadability") ||
                   name.Contains("BackdropOcclusionFoundation");
        }

        private static bool ShouldReceiveRealtimeSurfaceShadow(Renderer renderer)
        {
            var name = renderer.gameObject.name;
            if (IsRealtimeShadowSafeDetailName(name))
            {
                return false;
            }

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
            if (IsRealtimeShadowSafeDetailName(name))
            {
                return false;
            }

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
                IsRealtimeShadowSafeDetailName(name) ||
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
                var sideShade = isCentralPlaza ? CentralPlazaStage7jSideShade : RealtimeOutdoorSideShade;
                block.SetColor(SideShadeId, sideShade);
            }

            if (material.HasProperty(FloorShadeId))
            {
                var floorShade = isCentralPlaza ? CentralPlazaStage7jFloorShade : RealtimeOutdoorFloorShade;
                block.SetColor(FloorShadeId, floorShade);
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

            if (IsRealtimeShadowSafeDetailName(name))
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

        private static bool IsRealtimeShadowSafeDetailName(string name)
        {
            return !string.IsNullOrEmpty(name) &&
                   (name.Contains("FacadeLeakClosure") ||
                    name.Contains("FacadeOpaqueClosure") ||
                    name.Contains("FacadeNaturalization") ||
                    name.Contains("FacadeArtifactConsolidation") ||
                    name.Contains("FacadeComposition") ||
                    name.Contains("ArchitecturalClosure") ||
                    name.Contains("FacadeBackdropReadability") ||
                    name.Contains("FacadeMicrodepth") ||
                    name.Contains("OcclusionReadability") ||
                    name.Contains("OcclusionShell") ||
                    name.Contains("HeroReadability") ||
                    name.Contains("PathPorch") ||
                    name.Contains("PorchDoorGrounding") ||
                    name.Contains("ProportionCleanup") ||
                    name.Contains("ArchitectureSurfaceDepth") ||
                    name.Contains("HorizonDepthCleanup") ||
                    name.Contains("OutdoorSkyWash") ||
                    name.Contains("OutdoorSkyDetail") ||
                    name.Contains("OutdoorSkyBackdrop") ||
                    name.Contains("OutdoorBackgroundSkyDepth") ||
                    name.Contains("OutdoorWorldEnvelope") ||
                    name.Contains("ScenicHorizonGrounding") ||
                    name.Contains("LibraryFacadeCloseDetail") ||
                    name.Contains("LibraryFacadeArchitecture") ||
                    name.Contains("LibraryFacadeLandmark") ||
                    name.Contains("LibraryFacadeSurfaceBreakup") ||
                    name.Contains("LibraryExteriorDepth") ||
                    name.Contains("LibraryEntryDepth") ||
                    name.Contains("LibraryFrontDepth") ||
                    name.Contains("LibraryOcclusionReadability") ||
                    name.Contains("LibraryRearVolume") ||
                    name.Contains("LibraryRearRoofConnection") ||
                    name.Contains("LibraryBackwardVolume") ||
                    name.Contains("LibraryRoofSideDepth") ||
                    name.Contains("LibrarySideSurfaceBreakup") ||
                    name.Contains("LibrarySideWallMasonryRelief") ||
                    name.Contains("LibrarySideWallSurface") ||
                    name.Contains("LibrarySideRecess") ||
                    name.Contains("LibraryDeepExteriorVolume") ||
                    name.Contains("LibraryBrightAccentCleanup") ||
                    name.Contains("LibraryLowerFacade") ||
                    name.Contains("LibraryWindowReveal") ||
                    name.Contains("LibraryDoorRelief") ||
                    name.Contains("DepthPool") ||
                    IsLegacyCycleVisualDetailName(name));
        }

        private static bool IsLegacyCycleVisualDetailName(string name)
        {
            return !string.IsNullOrEmpty(name) &&
                   name.Contains("Cycle") &&
                   !name.Contains("RealtimeShadowCasterCycle");
        }

        private static string GetMaterialRole(Material material)
        {
            return material != null ? material.GetTag(MaterialRoleTagName, false, string.Empty) : string.Empty;
        }

        private static bool UsesCharacterBillboardShadowFix(Renderer renderer)
        {
            var material = renderer != null ? renderer.sharedMaterial : null;
            if (material == null)
            {
                return false;
            }

            // RecoveryV3 (2026-06-03): default to the TwoSided billboard-shadow path UNLESS a material
            // explicitly opts out (property present and <= 0.5). New chapter1 character/NPC materials
            // never set _CharacterBillboardShadowFix, so the old strict check returned false and routed
            // them to ShadowCastingMode.On, which the GPU Resident Drawer mis-batches and culls (NPCs
            // vanish). Absent property now means "on".
            if (!material.HasProperty(CharacterBillboardShadowFixId))
            {
                return true;
            }

            return material.GetFloat(CharacterBillboardShadowFixId) > 0.5f;
        }

        private static bool IsPaleFakeLookSurfaceWashRenderer(Renderer renderer)
        {
            if (renderer == null)
            {
                return false;
            }

            // Never touch the time-window portal, gameplay nav pads / interaction "open" cues, or the
            // character-grounding family (ground bounce, foot/contact/directional cast shadows).
            var name = renderer.gameObject.name;
            if (name.Contains("MapMoveGlowPad") ||
                name.Contains("GlowCue") ||
                name.Contains("OpenCue") ||
                name.Contains("GroundBounce") ||
                name.Contains("FootContact") ||
                name.Contains("ContactShadow") ||
                name.Contains("DirectionalCastShadow"))
            {
                return false;
            }

            if (!IsTargetedWhiteSurfaceWashRendererName(name))
            {
                return false;
            }

            var material = renderer.sharedMaterial;
            if (material == null)
            {
                return false;
            }

            var role = GetMaterialRole(material);
            if (role == PortalWindowRole || role == SpriteCardRole || role == PaperCardRole)
            {
                return false;
            }

            if (!IsPaleWashColor(material))
            {
                return false;
            }

            var profile = renderer.GetComponentInParent<FastVsHd2dOverlayProfile>(true);
            if (profile != null)
            {
                var kind = profile.OverlayKindForReview;
                return kind == FastVsHd2dOverlayKind.LightPool ||
                       kind == FastVsHd2dOverlayKind.Atmosphere;
            }

            if (material.renderQueue < 2950)
            {
                return false;
            }

            return true;
        }

        private static bool IsTargetedWhiteSurfaceWashRendererName(string name)
        {
            return IsTargetedPlazaFloorWhiteWashName(name) ||
                   IsTargetedLibraryExteriorWhiteWashName(name);
        }

        private static bool IsTargetedPlazaFloorWhiteWashName(string name)
        {
            return name.Contains("Current_CentralPlaza_Cycle122_ReferenceSurfaceRemap_StoneSquareSunMass") ||
                   name.Contains("Current_CentralPlaza_Cycle109_SunlitFloorIsland_");
        }

        private static bool IsTargetedLibraryExteriorWhiteWashName(string name)
        {
            return name.Contains("Current_CentralPlaza_Cycle122_ReferenceSurfaceRemap_FacadeBroadRake") ||
                   name.Contains("Current_CentralPlaza_LibraryFacade") &&
                   (name.Contains("FacadeReadability") || name.Contains("SurfaceRemap"));
        }

        private static bool IsPaleWashColor(Material material)
        {
            var color = Color.white;
            if (material.HasProperty(BaseColorId))
            {
                color = material.GetColor(BaseColorId);
            }
            else if (material.HasProperty("_Color"))
            {
                color = material.GetColor("_Color");
            }

            var maxChannel = Mathf.Max(color.r, Mathf.Max(color.g, color.b));
            if (maxChannel < 0.70f)
            {
                return false; // dark overlay (shadow / void / occlusion) -> not a whitening wash
            }

            var minChannel = Mathf.Min(color.r, Mathf.Min(color.g, color.b));
            var saturation = maxChannel > 0.0001f ? (maxChannel - minChannel) / maxChannel : 0f;
            return saturation <= 0.45f; // near-white / cream / pale -> whitening wash
        }

    }
}
