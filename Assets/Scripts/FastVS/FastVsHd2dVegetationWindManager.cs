using Anemora.TimeManagement;
using UnityEngine;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Vegetation Wind Control Plane")]
    public sealed class FastVsHd2dVegetationWindManager : MonoBehaviour
    {
        private const float InteractiveBendSidewaysWeight = 0.16f;

        private static readonly int WindDirectionId = Shader.PropertyToID("_WindDirection");
        private static readonly int WindMainId = Shader.PropertyToID("_WindMain");
        private static readonly int WindTurbulenceId = Shader.PropertyToID("_WindTurbulence");
        private static readonly int WindPulseFrequencyId = Shader.PropertyToID("_WindPulseFrequency");
        private static readonly int SeasonTintId = Shader.PropertyToID("_SeasonTint");
        private static readonly int TimeOfDayTintId = Shader.PropertyToID("_TimeOfDayTint");
        private static readonly int WitherednessId = Shader.PropertyToID("_Witheredness");
        private static readonly int BendMapId = Shader.PropertyToID("_BendMap");
        private static readonly int BendMapWorldRectId = Shader.PropertyToID("_BendMapWorldRect");
        private static readonly int BendMapStrengthId = Shader.PropertyToID("_BendMapStrength");

        [SerializeField] private WindZone windZone;
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private Vector3 fallbackDirection = new Vector3(0.86f, 0f, 0.50f);
        [SerializeField, Min(0f)] private float mainStrength = 0.44f;
        [SerializeField, Min(0f)] private float turbulence = 0.22f;
        [SerializeField, Min(0.01f)] private float pulseFrequency = 0.95f;
        [SerializeField] private Color seasonTint = Color.white;
        [SerializeField] private Color timeOfDayTint = Color.white;
        [SerializeField, Range(0f, 1f)] private float witheredness;
        [SerializeField] private Texture bendMap;
        [SerializeField] private bool useInteractiveBendMap = true;
        [SerializeField, Range(64, 512)] private int bendMapResolution = 128;
        [SerializeField] private Vector2 bendMapWorldSize = new Vector2(6.4f, 5.8f);
        [SerializeField] private Vector3 bendMapWorldCenter = new Vector3(0f, 0f, 0f);
        [SerializeField] private bool trackPrimaryTrampler = true;
        [SerializeField, Min(0f)] private float bendMapDecayPerSecond = 1.65f;
        [SerializeField, Range(0f, 2f)] private float bendMapStrength = 1.0f;
        [SerializeField, Min(0.05f)] private float playerTramplerRadius = 0.58f;
        [SerializeField, Min(0.05f)] private float portalTramplerRadius = 1.12f;
        [SerializeField] private bool useReviewTramplers;
        [SerializeField] private Vector4 reviewTramplerA;
        [SerializeField] private Vector4 reviewTramplerB;
        [SerializeField] private Vector4 reviewTramplerC;
        [SerializeField] private bool publishEveryFrame = true;
        [SerializeField] private bool readWindZoneStrength = true;

        private RenderTexture interactiveBendMap;
        private Texture2D bendMapCpuTexture;
        private Color32[] bendMapPixels;
        private int allocatedBendMapResolution;
        private int bendMapActivePixels;
        private float bendMapPeakValue;
        private CharacterController cachedPrimaryTrampler;
        private TimeWindowPairedSpacePortalController cachedPortalController;

        private readonly Trampler[] tramplers = new Trampler[4];

        private struct Trampler
        {
            public Vector3 Position;
            public float Radius;
            public float Strength;
        }

        public WindZone WindZoneForReview => windZone;
        public FastVsHouseAreaVisibility AreaVisibilityForReview => areaVisibility;
        public Vector3 WindDirectionForReview => ResolveWindDirection();
        public float WindMainForReview => ResolveWindMain();
        public float WindTurbulenceForReview => ResolveWindTurbulence();
        public float WindPulseFrequencyForReview => ResolveWindPulseFrequency();
        public Color SeasonTintForReview => ResolveTint(seasonTint);
        public Color TimeOfDayTintForReview => ResolveTint(timeOfDayTint);
        public float WitherednessForReview => Mathf.Clamp01(witheredness);
        public Texture BendMapForReview => bendMap;
        public bool UseInteractiveBendMapForReview => useInteractiveBendMap;
        public RenderTexture InteractiveBendMapForReview => interactiveBendMap;
        public int BendMapResolutionForReview => Mathf.Clamp(bendMapResolution, 64, 512);
        public Vector2 BendMapWorldSizeForReview => bendMapWorldSize;
        public Vector3 BendMapWorldCenterForReview => bendMapWorldCenter;
        public float BendMapDecayPerSecondForReview => bendMapDecayPerSecond;
        public float BendMapStrengthForReview => bendMapStrength;
        public int BendMapActivePixelsForReview => bendMapActivePixels;
        public float BendMapPeakValueForReview => bendMapPeakValue;
        public int ReviewTramplerCountForReview => CountReviewTramplers();
        public bool PublishEveryFrameForReview => publishEveryFrame;

        private void OnEnable()
        {
            PublishNowForReview();
        }

        private void OnValidate()
        {
            mainStrength = Mathf.Max(0f, mainStrength);
            turbulence = Mathf.Max(0f, turbulence);
            pulseFrequency = Mathf.Max(0.01f, pulseFrequency);
            witheredness = Mathf.Clamp01(witheredness);
            fallbackDirection = SanitizeDirection(fallbackDirection);
            bendMapResolution = Mathf.Clamp(bendMapResolution, 64, 512);
            bendMapWorldSize = new Vector2(Mathf.Max(0.5f, bendMapWorldSize.x), Mathf.Max(0.5f, bendMapWorldSize.y));
            bendMapDecayPerSecond = Mathf.Max(0f, bendMapDecayPerSecond);
            bendMapStrength = Mathf.Clamp(bendMapStrength, 0f, 2f);
            playerTramplerRadius = Mathf.Max(0.05f, playerTramplerRadius);
            portalTramplerRadius = Mathf.Max(0.05f, portalTramplerRadius);
            PublishNowForReview();
        }

        private void OnDisable()
        {
            ReleaseInteractiveBendMap();
        }

        private void OnDestroy()
        {
            ReleaseInteractiveBendMap();
        }

        private void Update()
        {
            if (publishEveryFrame)
            {
                PublishNowForReview();
            }
        }

        public void PublishNowForReview()
        {
            var direction = ResolveWindDirection();
            Shader.SetGlobalVector(WindDirectionId, new Vector4(direction.x, 0f, direction.z, 0f));
            Shader.SetGlobalFloat(WindMainId, ResolveWindMain());
            Shader.SetGlobalFloat(WindTurbulenceId, ResolveWindTurbulence());
            Shader.SetGlobalFloat(WindPulseFrequencyId, ResolveWindPulseFrequency());
            Shader.SetGlobalColor(SeasonTintId, ResolveTint(seasonTint));
            Shader.SetGlobalColor(TimeOfDayTintId, ResolveTint(timeOfDayTint));
            Shader.SetGlobalFloat(WitherednessId, Mathf.Clamp01(witheredness));
            PublishBendMapGlobals(ResolveBendMapTextureForPublish(ResolveReviewDeltaTime()));
        }

        public void ApplyInteractiveBendMapReviewStateForReview(
            bool enabled,
            Vector3 worldCenter,
            Vector4 tramplerA,
            Vector4 tramplerB,
            Vector4 tramplerC,
            bool clearExisting)
        {
            useInteractiveBendMap = enabled;
            useReviewTramplers = enabled;
            trackPrimaryTrampler = false;
            bendMapWorldCenter = worldCenter;
            reviewTramplerA = tramplerA;
            reviewTramplerB = tramplerB;
            reviewTramplerC = tramplerC;

            if (clearExisting)
            {
                ClearInteractiveBendMapData();
            }

            SimulateInteractiveBendMapForReview(enabled ? 0.18f : 0f);
        }

        public void SimulateInteractiveBendMapForReview(float seconds)
        {
            if (!useInteractiveBendMap)
            {
                PublishBendMapGlobals(bendMap != null ? bendMap : Texture2D.blackTexture);
                return;
            }

            var steps = Mathf.Max(1, Mathf.CeilToInt(Mathf.Max(0.01f, seconds) * 30f));
            for (var i = 0; i < steps; i++)
            {
                UpdateInteractiveBendMap(1f / 30f);
            }

            PublishBendMapGlobals(interactiveBendMap != null ? interactiveBendMap : Texture2D.blackTexture);
        }

        public void ApplyDefaultInteractiveBendMapForReview()
        {
            useInteractiveBendMap = true;
            useReviewTramplers = false;
            trackPrimaryTrampler = true;
            ClearInteractiveBendMapData();
            PublishNowForReview();
        }

        public void ApplyDefaultReviewStateForReview()
        {
            ApplyReviewStateForReview(new Vector3(0.86f, 0f, 0.50f), 0.44f, 0.22f, 0.95f, Color.white, Color.white, 0f);
        }

        public void ApplyVegetationTintReviewStateForReview(Color reviewSeasonTint, Color reviewTimeOfDayTint, float reviewWitheredness)
        {
            seasonTint = ResolveTint(reviewSeasonTint);
            timeOfDayTint = ResolveTint(reviewTimeOfDayTint);
            witheredness = Mathf.Clamp01(reviewWitheredness);
            PublishNowForReview();
        }

        public void ApplyReviewStateForReview(
            Vector3 direction,
            float main,
            float windTurbulence,
            float windPulseFrequency,
            Color reviewSeasonTint,
            Color reviewTimeOfDayTint,
            float reviewWitheredness)
        {
            fallbackDirection = SanitizeDirection(direction);
            mainStrength = Mathf.Max(0f, main);
            turbulence = Mathf.Max(0f, windTurbulence);
            pulseFrequency = Mathf.Max(0.01f, windPulseFrequency);
            seasonTint = ResolveTint(reviewSeasonTint);
            timeOfDayTint = ResolveTint(reviewTimeOfDayTint);
            witheredness = Mathf.Clamp01(reviewWitheredness);

            if (windZone != null)
            {
                windZone.transform.rotation = Quaternion.LookRotation(fallbackDirection, Vector3.up);
                windZone.windMain = mainStrength;
                windZone.windTurbulence = turbulence;
                windZone.windPulseFrequency = pulseFrequency;
            }

            PublishNowForReview();
        }

        private Vector3 ResolveWindDirection()
        {
            if (windZone != null)
            {
                return SanitizeDirection(Vector3.ProjectOnPlane(windZone.transform.forward, Vector3.up));
            }

            return SanitizeDirection(fallbackDirection);
        }

        private Texture ResolveBendMapTextureForPublish(float deltaTime)
        {
            if (!useInteractiveBendMap)
            {
                Shader.SetGlobalVector(BendMapWorldRectId, new Vector4(bendMapWorldCenter.x, bendMapWorldCenter.z, bendMapWorldSize.x, bendMapWorldSize.y));
                Shader.SetGlobalFloat(BendMapStrengthId, 0f);
                return bendMap != null ? bendMap : Texture2D.blackTexture;
            }

            UpdateInteractiveBendMap(deltaTime);
            return interactiveBendMap != null ? interactiveBendMap : Texture2D.blackTexture;
        }

        private void PublishBendMapGlobals(Texture texture)
        {
            Shader.SetGlobalVector(BendMapWorldRectId, new Vector4(bendMapWorldCenter.x, bendMapWorldCenter.z, bendMapWorldSize.x, bendMapWorldSize.y));
            Shader.SetGlobalFloat(BendMapStrengthId, useInteractiveBendMap ? Mathf.Clamp(bendMapStrength, 0f, 2f) : 0f);
            Shader.SetGlobalTexture(BendMapId, texture != null ? texture : Texture2D.blackTexture);
        }

        private void UpdateInteractiveBendMap(float deltaTime)
        {
            EnsureInteractiveBendMapResources();
            if (bendMapPixels == null || bendMapCpuTexture == null || interactiveBendMap == null)
            {
                return;
            }

            var tramplerCount = ResolveTramplers();
            if (trackPrimaryTrampler && tramplerCount > 0)
            {
                bendMapWorldCenter = tramplers[0].Position;
            }

            var decay = Mathf.Exp(-Mathf.Max(0f, bendMapDecayPerSecond) * Mathf.Max(0f, deltaTime));
            for (var i = 0; i < bendMapPixels.Length; i++)
            {
                var pixel = bendMapPixels[i];
                var intensity = (pixel.b / 255f) * decay;
                if (intensity <= 0.002f)
                {
                    bendMapPixels[i] = new Color32(128, 128, 0, 255);
                    continue;
                }

                pixel.b = (byte)Mathf.Clamp(Mathf.RoundToInt(intensity * 255f), 0, 255);
                pixel.a = 255;
                bendMapPixels[i] = pixel;
            }

            for (var i = 0; i < tramplerCount; i++)
            {
                DrawTrampler(tramplers[i]);
            }

            RefreshBendMapStats();
            bendMapCpuTexture.SetPixels32(bendMapPixels);
            bendMapCpuTexture.Apply(false, false);
            Graphics.Blit(bendMapCpuTexture, interactiveBendMap);
        }

        private int ResolveTramplers()
        {
            var count = 0;
            if (useReviewTramplers)
            {
                AddReviewTrampler(reviewTramplerA, ref count);
                AddReviewTrampler(reviewTramplerB, ref count);
                AddReviewTrampler(reviewTramplerC, ref count);
                return count;
            }

            if (cachedPrimaryTrampler == null)
            {
                cachedPrimaryTrampler = FindFirstObjectByType<CharacterController>();
            }

            var player = cachedPrimaryTrampler;
            if (player != null)
            {
                tramplers[count++] = new Trampler
                {
                    Position = player.transform.position,
                    Radius = playerTramplerRadius,
                    Strength = 1f
                };
            }

            if (cachedPortalController == null)
            {
                cachedPortalController = FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            }

            var portal = cachedPortalController;
            if (portal != null && portal.HasPortalPair)
            {
                AddPortalTrampler(portal.CurrentPortalRootForReview, ref count);
                AddPortalTrampler(portal.OtherTimePortalRootForReview, ref count);
            }

            return count;
        }

        private void AddReviewTrampler(Vector4 reviewTrampler, ref int count)
        {
            if (count >= tramplers.Length || reviewTrampler.z <= 0.001f || reviewTrampler.w <= 0.001f)
            {
                return;
            }

            tramplers[count++] = new Trampler
            {
                Position = new Vector3(reviewTrampler.x, bendMapWorldCenter.y, reviewTrampler.y),
                Radius = reviewTrampler.z,
                Strength = reviewTrampler.w
            };
        }

        private void AddPortalTrampler(GameObject portalRoot, ref int count)
        {
            if (count >= tramplers.Length || portalRoot == null)
            {
                return;
            }

            tramplers[count++] = new Trampler
            {
                Position = portalRoot.transform.position,
                Radius = portalTramplerRadius,
                Strength = 0.78f
            };
        }

        private void DrawTrampler(Trampler trampler)
        {
            var resolution = BendMapResolutionForReview;
            var halfSize = bendMapWorldSize * 0.5f;
            var minX = bendMapWorldCenter.x - halfSize.x;
            var minZ = bendMapWorldCenter.z - halfSize.y;
            var pixelSizeX = bendMapWorldSize.x / resolution;
            var pixelSizeZ = bendMapWorldSize.y / resolution;
            var radius = Mathf.Max(0.05f, trampler.Radius);
            var minPixelX = Mathf.Clamp(Mathf.FloorToInt((trampler.Position.x - radius - minX) / pixelSizeX), 0, resolution - 1);
            var maxPixelX = Mathf.Clamp(Mathf.CeilToInt((trampler.Position.x + radius - minX) / pixelSizeX), 0, resolution - 1);
            var minPixelY = Mathf.Clamp(Mathf.FloorToInt((trampler.Position.z - radius - minZ) / pixelSizeZ), 0, resolution - 1);
            var maxPixelY = Mathf.Clamp(Mathf.CeilToInt((trampler.Position.z + radius - minZ) / pixelSizeZ), 0, resolution - 1);

            for (var y = minPixelY; y <= maxPixelY; y++)
            {
                var worldZ = minZ + ((y + 0.5f) * pixelSizeZ);
                for (var x = minPixelX; x <= maxPixelX; x++)
                {
                    var worldX = minX + ((x + 0.5f) * pixelSizeX);
                    var delta = new Vector2(worldX - trampler.Position.x, worldZ - trampler.Position.z);
                    var distance = delta.magnitude;
                    if (distance > radius)
                    {
                        continue;
                    }

                    var falloff = 1f - Smooth01(0f, radius, distance);
                    var intensity = Mathf.Clamp01(falloff * trampler.Strength);
                    var index = (y * resolution) + x;
                    var existing = bendMapPixels[index].b / 255f;
                    if (intensity <= existing)
                    {
                        continue;
                    }

                    var direction = distance > 0.0001f ? delta / distance : new Vector2(ResolveWindDirection().x, ResolveWindDirection().z);
                    direction *= InteractiveBendSidewaysWeight;
                    bendMapPixels[index] = new Color32(
                        (byte)Mathf.Clamp(Mathf.RoundToInt((direction.x * 0.5f + 0.5f) * 255f), 0, 255),
                        (byte)Mathf.Clamp(Mathf.RoundToInt((direction.y * 0.5f + 0.5f) * 255f), 0, 255),
                        (byte)Mathf.Clamp(Mathf.RoundToInt(intensity * 255f), 0, 255),
                        255);
                }
            }
        }

        private void EnsureInteractiveBendMapResources()
        {
            var resolution = BendMapResolutionForReview;
            if (interactiveBendMap != null && bendMapCpuTexture != null && bendMapPixels != null && allocatedBendMapResolution == resolution)
            {
                return;
            }

            ReleaseInteractiveBendMap();
            allocatedBendMapResolution = resolution;
            interactiveBendMap = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.ARGB32)
            {
                name = "FastVS_HD2D_InteractiveBendMap",
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                useMipMap = false,
                autoGenerateMips = false
            };
            interactiveBendMap.Create();
            bendMapCpuTexture = new Texture2D(resolution, resolution, TextureFormat.RGBA32, false)
            {
                name = "FastVS_HD2D_InteractiveBendMap_CPU",
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear
            };
            bendMapPixels = new Color32[resolution * resolution];
            ClearInteractiveBendMapData();
        }

        private void ClearInteractiveBendMapData()
        {
            if (bendMapPixels == null)
            {
                bendMapActivePixels = 0;
                bendMapPeakValue = 0f;
                return;
            }

            for (var i = 0; i < bendMapPixels.Length; i++)
            {
                bendMapPixels[i] = new Color32(128, 128, 0, 255);
            }

            bendMapActivePixels = 0;
            bendMapPeakValue = 0f;
            if (bendMapCpuTexture != null)
            {
                bendMapCpuTexture.SetPixels32(bendMapPixels);
                bendMapCpuTexture.Apply(false, false);
            }

            if (interactiveBendMap != null && bendMapCpuTexture != null)
            {
                Graphics.Blit(bendMapCpuTexture, interactiveBendMap);
            }
        }

        private void RefreshBendMapStats()
        {
            bendMapActivePixels = 0;
            bendMapPeakValue = 0f;
            if (bendMapPixels == null)
            {
                return;
            }

            for (var i = 0; i < bendMapPixels.Length; i++)
            {
                var value = bendMapPixels[i].b / 255f;
                if (value > 0.015f)
                {
                    bendMapActivePixels++;
                    bendMapPeakValue = Mathf.Max(bendMapPeakValue, value);
                }
            }
        }

        private void ReleaseInteractiveBendMap()
        {
            if (interactiveBendMap != null)
            {
                interactiveBendMap.Release();
                DestroyObject(interactiveBendMap);
            }

            if (bendMapCpuTexture != null)
            {
                DestroyObject(bendMapCpuTexture);
            }

            interactiveBendMap = null;
            bendMapCpuTexture = null;
            bendMapPixels = null;
            allocatedBendMapResolution = 0;
            bendMapActivePixels = 0;
            bendMapPeakValue = 0f;
        }

        private int CountReviewTramplers()
        {
            if (!useReviewTramplers)
            {
                return 0;
            }

            var count = 0;
            if (reviewTramplerA.z > 0.001f && reviewTramplerA.w > 0.001f) count++;
            if (reviewTramplerB.z > 0.001f && reviewTramplerB.w > 0.001f) count++;
            if (reviewTramplerC.z > 0.001f && reviewTramplerC.w > 0.001f) count++;
            return count;
        }

        private static void DestroyObject(Object target)
        {
            if (target == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Destroy(target);
            }
            else
            {
                DestroyImmediate(target);
            }
        }

        private static float ResolveReviewDeltaTime()
        {
            return Application.isPlaying ? Mathf.Max(0.001f, Time.deltaTime) : 1f / 30f;
        }

        private static float Smooth01(float edge0, float edge1, float value)
        {
            var range = Mathf.Max(0.0001f, edge1 - edge0);
            var t = Mathf.Clamp01((value - edge0) / range);
            return t * t * (3f - (2f * t));
        }

        private float ResolveWindMain()
        {
            return readWindZoneStrength && windZone != null ? Mathf.Max(0f, windZone.windMain) : mainStrength;
        }

        private float ResolveWindTurbulence()
        {
            return readWindZoneStrength && windZone != null ? Mathf.Max(0f, windZone.windTurbulence) : turbulence;
        }

        private float ResolveWindPulseFrequency()
        {
            return readWindZoneStrength && windZone != null ? Mathf.Max(0.01f, windZone.windPulseFrequency) : pulseFrequency;
        }

        private static Vector3 SanitizeDirection(Vector3 direction)
        {
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                direction = Vector3.right;
            }

            return direction.normalized;
        }

        private static Color ResolveTint(Color tint)
        {
            var luma = (tint.r * 0.2126f) + (tint.g * 0.7152f) + (tint.b * 0.0722f);
            if (luma <= 0.001f)
            {
                return Color.white;
            }

            tint.a = 1f;
            return tint;
        }
    }
}
