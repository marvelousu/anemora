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

        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Light mainLight;
        [SerializeField] private bool enforceRendererShadowPolicy = true;
        [SerializeField] private Color exteriorSkyColor = new Color(0.48f, 0.50f, 0.46f, 1f);
        [SerializeField] private Color centralPlazaSkyColor = new Color(0.48f, 0.43f, 0.33f, 1f);

        private float nextShadowPolicyRefreshTime;
        private GameObject cycle128GradeRoot;
        private MeshRenderer cycle128GradeRenderer;
        private MeshRenderer cycle128BeamRenderer;
        private Material cycle128GradeMaterial;
        private Material cycle128BeamMaterial;
        private Texture2D cycle128GradeTexture;
        private Texture2D cycle128BeamTexture;

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
                mainLight.shadowStrength = Mathf.Max(mainLight.shadowStrength, 0.98f);
                mainLight.shadowBias = Mathf.Min(mainLight.shadowBias, 0.025f);
                mainLight.shadowNormalBias = Mathf.Min(mainLight.shadowNormalBias, 0.18f);
                mainLight.shadowNearPlane = Mathf.Min(Mathf.Max(mainLight.shadowNearPlane, 0.05f), 0.12f);

                if (isCentralPlaza)
                {
                    mainLight.intensity = Mathf.Max(mainLight.intensity, 2.16f);
                    mainLight.color = new Color(1.00f, 0.88f, 0.58f, 1f);
                    mainLight.transform.rotation = Quaternion.Euler(31f, -42f, 0f);
                }
            }

            if (sceneCamera != null && (area == FastVsHouseArea.Exterior || area == FastVsHouseArea.CentralPlaza))
            {
                sceneCamera.clearFlags = CameraClearFlags.SolidColor;
                sceneCamera.backgroundColor = area == FastVsHouseArea.CentralPlaza ? centralPlazaSkyColor : exteriorSkyColor;
            }

            if (area == FastVsHouseArea.Exterior || area == FastVsHouseArea.CentralPlaza)
            {
                RenderSettings.fog = false;
                RenderSettings.ambientMode = AmbientMode.Flat;
            }

            if (isCentralPlaza)
            {
                RenderSettings.ambientLight = new Color(0.040f, 0.036f, 0.030f, 1f);
                RenderSettings.reflectionIntensity = 0f;
                EnsureCycle128CameraGrade();
                SetCycle128CameraGradeActive(true);
                UpdateCycle128CameraGradeScale();
            }
            else
            {
                RenderSettings.reflectionIntensity = 1f;
                SetCycle128CameraGradeActive(false);
            }
        }

        private void ApplyRendererShadowPolicy()
        {
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
                    continue;
                }

                var role = GetMaterialRole(renderer.sharedMaterial);
                if (role == SurfaceLitRole)
                {
                    ApplySurfaceShadowPolicy(renderer);
                }
                else if (role == SpriteCardRole || role == PaperCardRole)
                {
                    renderer.shadowCastingMode = ShadowCastingMode.On;
                    renderer.receiveShadows = true;
                }
                else if (role == PortalWindowRole)
                {
                    renderer.shadowCastingMode = ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                }
            }
        }

        private static void ApplySurfaceShadowPolicy(Renderer renderer)
        {
            renderer.enabled = true;
            if (ShouldReceiveRealtimeSurfaceShadow(renderer))
            {
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = true;
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

        private static bool ShouldReceiveRealtimeSurfaceShadow(Renderer renderer)
        {
            var surface = renderer.GetComponent<FastVsHd2dSurfaceProfile>();
            if (surface != null &&
                (surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Floor ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Ground ||
                 surface.SurfaceKindForReview == FastVsHd2dSurfaceKind.Road))
            {
                return true;
            }

            var name = renderer.gameObject.name;
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

            cycle128GradeTexture = new Texture2D(256, 144, TextureFormat.RGBA32, false)
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
                    var radius = Mathf.Sqrt(dx * dx * 0.82f + dy * dy * 1.38f);
                    var edge = Mathf.SmoothStep(0.48f, 1.18f, radius);
                    var topWarm = Mathf.SmoothStep(0.52f, 1.00f, v);
                    var baseAlpha = 0.018f + edge * 0.14f + topWarm * 0.024f;
                    var warm = Color.Lerp(new Color(0.82f, 0.58f, 0.30f, baseAlpha), new Color(0.11f, 0.070f, 0.035f, baseAlpha), edge);
                    cycle128GradeTexture.SetPixel(x, y, warm);
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
                    var beam = Mathf.SmoothStep(0.11f, 0.010f, rayA) * 0.10f + Mathf.SmoothStep(0.09f, 0.010f, rayB) * 0.055f;
                    beam *= Mathf.SmoothStep(0.04f, 0.42f, v) * Mathf.SmoothStep(1.00f, 0.62f, v);
                    beam *= Mathf.SmoothStep(1.04f, 0.58f, u);
                    cycle128BeamTexture.SetPixel(x, y, new Color(1.00f, 0.83f, 0.48f, beam));
                }
            }

            cycle128BeamTexture.Apply(false, true);
            return cycle128BeamTexture;
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
        }
    }
}
