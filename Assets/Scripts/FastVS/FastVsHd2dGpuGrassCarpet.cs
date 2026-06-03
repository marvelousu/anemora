using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/GPU Grass Carpet")]
    public sealed class FastVsHd2dGpuGrassCarpet : MonoBehaviour
    {
        private const int ThreadGroupSize = 128;
        private const int BladeStride = 32;
        private const int FrustumPlaneCount = 6;

        private static readonly int BladeInputId = Shader.PropertyToID("_BladeInput");
        private static readonly int VisibleBladeOutputId = Shader.PropertyToID("_VisibleBladeOutput");
        private static readonly int VisibleBladeDataId = Shader.PropertyToID("_AnemoraHd2dGrassVisibleBladeData");
        private static readonly int LocalToWorldId = Shader.PropertyToID("_AnemoraHd2dGrassLocalToWorld");
        private static readonly int CullPlanesId = Shader.PropertyToID("_GrassCullPlanes");
        private static readonly int CullDistanceId = Shader.PropertyToID("_GrassCullDistance");
        private static readonly int CameraPositionId = Shader.PropertyToID("_CameraPositionWS");
        private static readonly int BladeCountId = Shader.PropertyToID("_BladeCount");
        private static readonly int GroundTintId = Shader.PropertyToID("_GroundTint");
        private static readonly int TipTintId = Shader.PropertyToID("_TipTint");
        private static readonly int ShadowTintId = Shader.PropertyToID("_ShadowTint");
        private static readonly int WindStrengthId = Shader.PropertyToID("_WindStrength");
        private static readonly int WindSpeedId = Shader.PropertyToID("_WindSpeed");
        private static readonly int FallbackWindDirectionId = Shader.PropertyToID("_FallbackWindDirection");
        private static readonly int PixelSnapStrengthId = Shader.PropertyToID("_PixelSnapStrength");

        [SerializeField] private Mesh bladeMesh;
        [SerializeField] private Material bladeMaterial;
        [SerializeField] private ComputeShader cullCompute;
        [SerializeField, Min(1024)] private int bladeCount = 50000;
        [SerializeField] private Vector2 areaSize = new Vector2(6.6f, 4.5f);
        [SerializeField] private Vector2 bladeHeightRange = new Vector2(0.16f, 0.34f);
        [SerializeField] private Vector2 bladeWidthRange = new Vector2(0.028f, 0.060f);
        [SerializeField] private float groundYOffset = 0.024f;
        [SerializeField] private float cullDistance = 18.0f;
        [SerializeField] private int randomSeed = 14014;
        [SerializeField] private bool renderGrass = true;
        [SerializeField] private bool useGpuCull = true;
        [SerializeField] private Color groundTint = new Color(0.36f, 0.48f, 0.23f, 1f);
        [SerializeField] private Color tipTint = new Color(0.70f, 0.80f, 0.42f, 1f);
        [SerializeField] private Color shadowTint = new Color(0.20f, 0.27f, 0.18f, 1f);
        [SerializeField] private Vector3 fallbackWindDirection = new Vector3(0.86f, 0f, 0.50f);
        [SerializeField] private float windStrength = 0.075f;
        [SerializeField] private float windSpeed = 1.25f;
        [SerializeField] private float pixelSnapStrength = 0.28f;
        [SerializeField] private bool useArtDirectedScatter;
        [SerializeField] private float artDirectedBaseDensity = 0.42f;
        [SerializeField] private float artDirectedMaxDensity = 2.15f;
        [SerializeField] private Vector2 wornPathStart = new Vector2(-2.82f, -1.28f);
        [SerializeField] private Vector2 wornPathEnd = new Vector2(2.82f, 1.18f);
        [SerializeField] private float wornPathHalfWidth = 0.46f;
        [SerializeField] private float wornPathFeather = 0.34f;
        [SerializeField] private float wornPathDensity = 0.025f;
        [SerializeField] private Vector4 treeBaseClusterA = new Vector4(-2.24f, 1.38f, 1.02f, 1.72f);
        [SerializeField] private Vector4 rockEdgeClusterB = new Vector4(1.74f, -1.34f, 0.86f, 1.42f);
        [SerializeField] private Vector4 waterEdgeClusterC = new Vector4(2.46f, 1.26f, 0.72f, 1.18f);
        [SerializeField] private Vector2 steepSuppressionCenter = new Vector2(2.28f, -1.34f);
        [SerializeField] private Vector2 steepSuppressionSize = new Vector2(1.22f, 0.88f);
        [SerializeField] private float steepSuppressionFeather = 0.20f;
        [SerializeField] private float steepSuppressionDensity = 0.0f;

        private readonly Vector4[] cullPlanes = new Vector4[FrustumPlaneCount];
        private readonly GraphicsBuffer.IndirectDrawIndexedArgs[] commandData = new GraphicsBuffer.IndirectDrawIndexedArgs[1];

        private GraphicsBuffer bladeDataBuffer;
        private GraphicsBuffer visibleBladeBuffer;
        private GraphicsBuffer commandBuffer;
        private MaterialPropertyBlock materialProperties;
        private int cullKernel = -1;
        private int allocatedBladeCount;
        private bool resourcesDirty = true;
        private bool subscribed;

        [StructLayout(LayoutKind.Sequential)]
        private struct BladeData
        {
            public Vector4 positionHeight;
            public Vector4 angleWidthPhaseVariant;
        }

        public int BladeCountForReview => Mathf.Max(0, bladeCount);
        public bool RenderMeshIndirectEnabledForReview => bladeMesh != null && bladeMaterial != null;
        public bool GpuCullEnabledForReview => useGpuCull && cullCompute != null;
        public Mesh BladeMeshForReview => bladeMesh;
        public Material BladeMaterialForReview => bladeMaterial;
        public ComputeShader CullComputeForReview => cullCompute;
        public Vector2 AreaSizeForReview => areaSize;
        public Vector2 BladeHeightRangeForReview => bladeHeightRange;
        public Vector2 BladeWidthRangeForReview => bladeWidthRange;
        public float CullDistanceForReview => cullDistance;
        public Color GroundTintForReview => groundTint;
        public Color TipTintForReview => tipTint;
        public Color ShadowTintForReview => shadowTint;
        public bool UseArtDirectedScatterForReview => useArtDirectedScatter;
        public float ArtDirectedBaseDensityForReview => artDirectedBaseDensity;
        public float ArtDirectedMaxDensityForReview => artDirectedMaxDensity;
        public Vector2 WornPathStartForReview => wornPathStart;
        public Vector2 WornPathEndForReview => wornPathEnd;
        public float WornPathHalfWidthForReview => wornPathHalfWidth;
        public float WornPathFeatherForReview => wornPathFeather;
        public float WornPathDensityForReview => wornPathDensity;
        public Vector4 TreeBaseClusterAForReview => treeBaseClusterA;
        public Vector4 RockEdgeClusterBForReview => rockEdgeClusterB;
        public Vector4 WaterEdgeClusterCForReview => waterEdgeClusterC;
        public Vector2 SteepSuppressionCenterForReview => steepSuppressionCenter;
        public Vector2 SteepSuppressionSizeForReview => steepSuppressionSize;
        public float SteepSuppressionFeatherForReview => steepSuppressionFeather;
        public float SteepSuppressionDensityForReview => steepSuppressionDensity;

        public readonly struct ArtDirectedScatterStats
        {
            public ArtDirectedScatterStats(int bladeCount, int wornPathBladeCount, int steepSuppressionBladeCount, int clusterBladeCount, int openFieldBladeCount)
            {
                BladeCount = bladeCount;
                WornPathBladeCount = wornPathBladeCount;
                SteepSuppressionBladeCount = steepSuppressionBladeCount;
                ClusterBladeCount = clusterBladeCount;
                OpenFieldBladeCount = openFieldBladeCount;
            }

            public int BladeCount { get; }
            public int WornPathBladeCount { get; }
            public int SteepSuppressionBladeCount { get; }
            public int ClusterBladeCount { get; }
            public int OpenFieldBladeCount { get; }
            public float WornPathRatio => BladeCount <= 0 ? 0f : WornPathBladeCount / (float)BladeCount;
            public float SteepSuppressionRatio => BladeCount <= 0 ? 0f : SteepSuppressionBladeCount / (float)BladeCount;
            public float ClusterRatio => BladeCount <= 0 ? 0f : ClusterBladeCount / (float)BladeCount;
        }

        private void OnEnable()
        {
            Subscribe();
            MarkResourcesDirty();
        }

        private void OnDisable()
        {
            Unsubscribe();
            ReleaseResources();
        }

        private void OnDestroy()
        {
            Unsubscribe();
            ReleaseResources();
        }

        private void OnValidate()
        {
            bladeCount = Mathf.Max(1024, bladeCount);
            areaSize = new Vector2(Mathf.Max(0.5f, areaSize.x), Mathf.Max(0.5f, areaSize.y));
            bladeHeightRange = SortPositiveRange(bladeHeightRange, 0.02f);
            bladeWidthRange = SortPositiveRange(bladeWidthRange, 0.004f);
            groundYOffset = Mathf.Max(0f, groundYOffset);
            cullDistance = Mathf.Max(1f, cullDistance);
            windStrength = Mathf.Max(0f, windStrength);
            windSpeed = Mathf.Max(0.01f, windSpeed);
            pixelSnapStrength = Mathf.Clamp01(pixelSnapStrength);
            artDirectedBaseDensity = Mathf.Clamp(artDirectedBaseDensity, 0.02f, 1.5f);
            artDirectedMaxDensity = Mathf.Max(artDirectedBaseDensity + 0.05f, artDirectedMaxDensity);
            wornPathHalfWidth = Mathf.Max(0.02f, wornPathHalfWidth);
            wornPathFeather = Mathf.Max(0.01f, wornPathFeather);
            wornPathDensity = Mathf.Clamp(wornPathDensity, 0f, artDirectedMaxDensity);
            treeBaseClusterA = SanitizeCluster(treeBaseClusterA);
            rockEdgeClusterB = SanitizeCluster(rockEdgeClusterB);
            waterEdgeClusterC = SanitizeCluster(waterEdgeClusterC);
            steepSuppressionSize = new Vector2(Mathf.Max(0.05f, steepSuppressionSize.x), Mathf.Max(0.05f, steepSuppressionSize.y));
            steepSuppressionFeather = Mathf.Max(0.01f, steepSuppressionFeather);
            steepSuppressionDensity = Mathf.Clamp(steepSuppressionDensity, 0f, artDirectedMaxDensity);
            MarkResourcesDirty();
        }

        public void RenderNowForReview(Camera camera)
        {
            RenderForCamera(camera);
        }

        public void SetArtDirectedScatterForReview(bool enabled)
        {
            if (useArtDirectedScatter == enabled)
            {
                return;
            }

            useArtDirectedScatter = enabled;
            MarkResourcesDirty();
        }

        public ArtDirectedScatterStats AnalyzeArtDirectedScatterForReview()
        {
            var data = GenerateBladeData(Mathf.Max(1, bladeCount));
            var wornPath = 0;
            var steepSuppression = 0;
            var cluster = 0;
            var openField = 0;

            for (var i = 0; i < data.Length; i++)
            {
                var position = new Vector2(data[i].positionHeight.x, data[i].positionHeight.z);
                var pathMask = ResolveWornPathMask(position);
                var steepMask = ResolveSteepSuppressionMask(position);
                var clusterMask = Mathf.Max(
                    ResolveClusterMask(position, treeBaseClusterA),
                    Mathf.Max(ResolveClusterMask(position, rockEdgeClusterB), ResolveClusterMask(position, waterEdgeClusterC)));

                if (pathMask >= 0.82f)
                {
                    wornPath++;
                }

                if (steepMask >= 0.96f)
                {
                    steepSuppression++;
                }

                if (clusterMask >= 0.35f)
                {
                    cluster++;
                }

                if (pathMask < 0.10f && steepMask < 0.10f && clusterMask < 0.85f)
                {
                    openField++;
                }
            }

            return new ArtDirectedScatterStats(data.Length, wornPath, steepSuppression, cluster, openField);
        }

        private static Vector2 SortPositiveRange(Vector2 value, float minimum)
        {
            var low = Mathf.Max(minimum, Mathf.Min(value.x, value.y));
            var high = Mathf.Max(low + minimum, Mathf.Max(value.x, value.y));
            return new Vector2(low, high);
        }

        private void Subscribe()
        {
            if (subscribed)
            {
                return;
            }

            RenderPipelineManager.beginCameraRendering += HandleBeginCameraRendering;
            subscribed = true;
        }

        private void Unsubscribe()
        {
            if (!subscribed)
            {
                return;
            }

            RenderPipelineManager.beginCameraRendering -= HandleBeginCameraRendering;
            subscribed = false;
        }

        private void HandleBeginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            RenderForCamera(camera);
        }

        private void RenderForCamera(Camera camera)
        {
            if (!renderGrass || camera == null || !isActiveAndEnabled || bladeMesh == null || bladeMaterial == null)
            {
                return;
            }

            EnsureResources();
            if (bladeDataBuffer == null || commandBuffer == null)
            {
                return;
            }

            var visibleBuffer = bladeDataBuffer;
            var instanceCount = (uint)Mathf.Max(0, bladeCount);
            InitializeCommandBuffer(instanceCount);
            if (useGpuCull && cullCompute != null && visibleBladeBuffer != null && cullKernel >= 0)
            {
                visibleBuffer = DispatchGpuCull(camera);
            }

            materialProperties ??= new MaterialPropertyBlock();
            materialProperties.Clear();
            materialProperties.SetBuffer(VisibleBladeDataId, visibleBuffer);
            materialProperties.SetMatrix(LocalToWorldId, transform.localToWorldMatrix);
            materialProperties.SetColor(GroundTintId, groundTint);
            materialProperties.SetColor(TipTintId, tipTint);
            materialProperties.SetColor(ShadowTintId, shadowTint);
            materialProperties.SetFloat(WindStrengthId, windStrength);
            materialProperties.SetFloat(WindSpeedId, windSpeed);
            materialProperties.SetVector(FallbackWindDirectionId, ResolveFallbackWindDirection());
            materialProperties.SetFloat(PixelSnapStrengthId, pixelSnapStrength);

            var renderParams = new RenderParams(bladeMaterial)
            {
                camera = camera,
                layer = gameObject.layer,
                matProps = materialProperties,
                receiveShadows = true,
                shadowCastingMode = ShadowCastingMode.Off,
                worldBounds = ResolveWorldBounds()
            };

            Graphics.RenderMeshIndirect(renderParams, bladeMesh, commandBuffer, 1);
        }

        private GraphicsBuffer DispatchGpuCull(Camera camera)
        {
            visibleBladeBuffer.SetCounterValue(0);
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            for (var i = 0; i < FrustumPlaneCount; i++)
            {
                var plane = planes[i];
                var normal = plane.normal;
                cullPlanes[i] = new Vector4(normal.x, normal.y, normal.z, plane.distance);
            }

            cullCompute.SetInt(BladeCountId, Mathf.Max(0, bladeCount));
            cullCompute.SetFloat(CullDistanceId, cullDistance);
            cullCompute.SetVector(CameraPositionId, camera.transform.position);
            cullCompute.SetMatrix(LocalToWorldId, transform.localToWorldMatrix);
            cullCompute.SetVectorArray(CullPlanesId, cullPlanes);
            cullCompute.SetBuffer(cullKernel, BladeInputId, bladeDataBuffer);
            cullCompute.SetBuffer(cullKernel, VisibleBladeOutputId, visibleBladeBuffer);
            cullCompute.Dispatch(cullKernel, Mathf.CeilToInt(Mathf.Max(1, bladeCount) / (float)ThreadGroupSize), 1, 1);
            GraphicsBuffer.CopyCount(visibleBladeBuffer, commandBuffer, 4);
            return visibleBladeBuffer;
        }

        private Bounds ResolveWorldBounds()
        {
            var center = transform.TransformPoint(new Vector3(0f, groundYOffset + (bladeHeightRange.y * 0.5f), 0f));
            var size = new Vector3(areaSize.x + bladeHeightRange.y, bladeHeightRange.y + 0.24f, areaSize.y + bladeHeightRange.y);
            return new Bounds(center, size);
        }

        private Vector4 ResolveFallbackWindDirection()
        {
            var direction = fallbackWindDirection;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
            {
                direction = Vector3.right;
            }

            direction.Normalize();
            return new Vector4(direction.x, 0f, direction.z, 0f);
        }

        private void EnsureResources()
        {
            var targetCount = Mathf.Max(1, bladeCount);
            if (!resourcesDirty && allocatedBladeCount == targetCount && bladeDataBuffer != null && commandBuffer != null)
            {
                return;
            }

            ReleaseResources();
            allocatedBladeCount = targetCount;
            bladeDataBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, targetCount, BladeStride);
            bladeDataBuffer.SetData(GenerateBladeData(targetCount));
            visibleBladeBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Append, targetCount, BladeStride);
            commandBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, 1, GraphicsBuffer.IndirectDrawIndexedArgs.size);
            cullKernel = ResolveCullKernel();
            resourcesDirty = false;
        }

        private int ResolveCullKernel()
        {
            if (cullCompute == null)
            {
                return -1;
            }

            try
            {
                return cullCompute.FindKernel("CullGrass");
            }
            catch (ArgumentException)
            {
                return -1;
            }
        }

        private void InitializeCommandBuffer(uint instanceCount)
        {
            commandData[0].indexCountPerInstance = bladeMesh.GetIndexCount(0);
            commandData[0].instanceCount = instanceCount;
            commandData[0].startIndex = bladeMesh.GetIndexStart(0);
            commandData[0].baseVertexIndex = bladeMesh.GetBaseVertex(0);
            commandData[0].startInstance = 0;
            commandBuffer.SetData(commandData);
        }

        private BladeData[] GenerateBladeData(int count)
        {
            var data = new BladeData[count];
            var state = (uint)Mathf.Max(1, randomSeed);
            var minHeight = Mathf.Min(bladeHeightRange.x, bladeHeightRange.y);
            var maxHeight = Mathf.Max(bladeHeightRange.x, bladeHeightRange.y);
            var minWidth = Mathf.Min(bladeWidthRange.x, bladeWidthRange.y);
            var maxWidth = Mathf.Max(bladeWidthRange.x, bladeWidthRange.y);

            for (var i = 0; i < count; i++)
            {
                var position = SelectBladePosition(ref state);
                var x = position.x;
                var z = position.y;
                var edgeFade = Mathf.Clamp01(Mathf.Min((areaSize.x * 0.5f) - Mathf.Abs(x), (areaSize.y * 0.5f) - Mathf.Abs(z)) * 2.4f);
                var height = Mathf.Lerp(minHeight, maxHeight, Next01(ref state)) * Mathf.Lerp(0.78f, 1.08f, edgeFade);
                var width = Mathf.Lerp(minWidth, maxWidth, Next01(ref state));
                var angle = Next01(ref state) * Mathf.PI * 2f;
                var phase = Next01(ref state) * Mathf.PI * 2f;
                var variant = Next01(ref state);
                var y = groundYOffset + ((Next01(ref state) - 0.5f) * 0.010f);

                data[i] = new BladeData
                {
                    positionHeight = new Vector4(x, y, z, height),
                    angleWidthPhaseVariant = new Vector4(angle, width, phase, variant)
                };
            }

            return data;
        }

        private Vector2 SelectBladePosition(ref uint state)
        {
            if (!useArtDirectedScatter)
            {
                return SampleUniformPosition(ref state);
            }

            var maxDensity = Mathf.Max(0.05f, artDirectedMaxDensity);
            const int maxAttempts = 16;

            for (var attempt = 0; attempt < maxAttempts; attempt++)
            {
                var candidate = SampleUniformPosition(ref state);
                if (ResolveSteepSuppressionMask(candidate) >= 0.96f)
                {
                    continue;
                }

                var density = ResolveArtDirectedDensity(candidate);
                if (Next01(ref state) * maxDensity <= density)
                {
                    return candidate;
                }
            }

            var fallback = SampleUniformPosition(ref state);
            for (var attempt = 0; attempt < 8 && ResolveSteepSuppressionMask(fallback) >= 0.96f; attempt++)
            {
                fallback = SampleUniformPosition(ref state);
            }

            return fallback;
        }

        private Vector2 SampleUniformPosition(ref uint state)
        {
            return new Vector2(
                (Next01(ref state) - 0.5f) * areaSize.x,
                (Next01(ref state) - 0.5f) * areaSize.y);
        }

        private float ResolveArtDirectedDensity(Vector2 position)
        {
            if (!useArtDirectedScatter)
            {
                return 1f;
            }

            var density = Mathf.Max(0.01f, artDirectedBaseDensity);
            density += ResolveClusterContribution(position, treeBaseClusterA);
            density += ResolveClusterContribution(position, rockEdgeClusterB);
            density += ResolveClusterContribution(position, waterEdgeClusterC);

            var pathMask = ResolveWornPathMask(position);
            density = Mathf.Lerp(density, wornPathDensity, pathMask);

            var steepMask = ResolveSteepSuppressionMask(position);
            density = Mathf.Lerp(density, steepSuppressionDensity, steepMask);
            return Mathf.Clamp(density, 0f, Mathf.Max(0.05f, artDirectedMaxDensity));
        }

        private float ResolveWornPathMask(Vector2 position)
        {
            var distance = DistanceToSegment(position, wornPathStart, wornPathEnd);
            return 1f - Smooth01(wornPathHalfWidth, wornPathHalfWidth + wornPathFeather, distance);
        }

        private float ResolveSteepSuppressionMask(Vector2 position)
        {
            var halfSize = steepSuppressionSize * 0.5f;
            var dx = Mathf.Abs(position.x - steepSuppressionCenter.x) - halfSize.x;
            var dz = Mathf.Abs(position.y - steepSuppressionCenter.y) - halfSize.y;
            var outside = Mathf.Max(dx, dz);
            return 1f - Smooth01(0f, steepSuppressionFeather, outside);
        }

        private static float ResolveClusterContribution(Vector2 position, Vector4 cluster)
        {
            return cluster.w * ResolveClusterMask(position, cluster);
        }

        private static float ResolveClusterMask(Vector2 position, Vector4 cluster)
        {
            var radius = Mathf.Max(0.05f, cluster.z);
            var distance = Vector2.Distance(position, new Vector2(cluster.x, cluster.y));
            return 1f - Smooth01(radius * 0.18f, radius, distance);
        }

        private static float Smooth01(float edge0, float edge1, float value)
        {
            var range = Mathf.Max(0.0001f, edge1 - edge0);
            var t = Mathf.Clamp01((value - edge0) / range);
            return t * t * (3f - (2f * t));
        }

        private static float DistanceToSegment(Vector2 point, Vector2 a, Vector2 b)
        {
            var ab = b - a;
            var lengthSq = Vector2.Dot(ab, ab);
            if (lengthSq <= 0.0001f)
            {
                return Vector2.Distance(point, a);
            }

            var t = Mathf.Clamp01(Vector2.Dot(point - a, ab) / lengthSq);
            return Vector2.Distance(point, a + ab * t);
        }

        private static Vector4 SanitizeCluster(Vector4 cluster)
        {
            cluster.z = Mathf.Max(0.05f, cluster.z);
            cluster.w = Mathf.Max(0f, cluster.w);
            return cluster;
        }

        private static float Next01(ref uint state)
        {
            state = (state * 1664525u) + 1013904223u;
            return (state & 0x00FFFFFF) / 16777215f;
        }

        private void MarkResourcesDirty()
        {
            resourcesDirty = true;
        }

        private void ReleaseResources()
        {
            bladeDataBuffer?.Release();
            visibleBladeBuffer?.Release();
            commandBuffer?.Release();
            bladeDataBuffer = null;
            visibleBladeBuffer = null;
            commandBuffer = null;
            allocatedBladeCount = 0;
        }
    }
}
