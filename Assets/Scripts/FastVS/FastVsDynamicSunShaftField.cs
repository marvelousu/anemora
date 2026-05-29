using Anemora.FastVS.SunCycle;
using UnityEngine;

namespace Anemora.FastVS
{
    [AddComponentMenu("Anemora/FastVS/HD2D Dynamic Sun Shaft Field")]
    public sealed class FastVsDynamicSunShaftField : MonoBehaviour
    {
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private Light directionalSun;
        [SerializeField] private Renderer[] shaftRenderers;
        [SerializeField] private float baseAlpha = 0.24f;
        [SerializeField] private float viewReactiveAlpha = 0.12f;
        [SerializeField] private float pulseAmplitude = 0.035f;
        [SerializeField] private float pulseSpeed = 0.28f;
        [SerializeField] private float cameraParallax = 0.12f;
        [SerializeField] private float sunYawInfluence = 10f;
        [SerializeField] private bool centralPlazaOnly = true;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");

        private MaterialPropertyBlock propertyBlock;
        private Transform[] shaftTransforms;
        private Vector3[] baseLocalPositions;
        private Quaternion[] baseLocalRotations;

        public int ShaftRendererCountForReview => shaftRenderers != null ? shaftRenderers.Length : 0;
        public bool ActiveForReview => ShouldRenderShafts();

        private void Awake()
        {
            EnsurePropertyBlock();
            ResolveReferences();
            CacheShafts();
        }

        private void OnEnable()
        {
            EnsurePropertyBlock();
            ResolveReferences();
            CacheShafts();
            ApplyNowForReview();
        }

        private void LateUpdate()
        {
            ApplyNowForReview();
        }

        public void ApplyNowForReview()
        {
            EnsurePropertyBlock();
            ResolveReferences();
            CacheShafts();

            if (shaftRenderers == null)
            {
                return;
            }

            var visible = ShouldRenderShafts();
            var sunYaw = ResolveSunYawDegrees();
            var viewFactor = ResolveViewAlignmentFactor();
            var pulse = 1f + Mathf.Sin(Time.time * Mathf.Max(0.01f, pulseSpeed)) * pulseAmplitude;
            var alpha = visible ? Mathf.Clamp01((baseAlpha + viewReactiveAlpha * viewFactor) * pulse) : 0f;
            var parallaxOffset = ResolveCameraParallaxOffset();

            for (var i = 0; i < shaftRenderers.Length; i++)
            {
                var renderer = shaftRenderers[i];
                if (renderer == null)
                {
                    continue;
                }

                renderer.enabled = visible;
                ApplyRendererAlpha(renderer, alpha);

                if (shaftTransforms == null || i >= shaftTransforms.Length || shaftTransforms[i] == null)
                {
                    continue;
                }

                var phase = (i - ((shaftRenderers.Length - 1) * 0.5f)) * 0.45f;
                shaftTransforms[i].localPosition = baseLocalPositions[i] + parallaxOffset * phase;
                shaftTransforms[i].localRotation = baseLocalRotations[i] * Quaternion.Euler(0f, 0f, sunYaw * sunYawInfluence * 0.01f);
            }
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

            if (directionalSun == null)
            {
                directionalSun = RenderSettings.sun;
            }

            if (directionalSun == null && AnemoraSunCycleDriver.Instance != null)
            {
                directionalSun = RenderSettings.sun;
            }
        }

        private void CacheShafts()
        {
            if (shaftRenderers == null || shaftRenderers.Length == 0)
            {
                shaftRenderers = GetComponentsInChildren<Renderer>(true);
            }

            if (shaftRenderers == null)
            {
                return;
            }

            if (shaftTransforms != null && shaftTransforms.Length == shaftRenderers.Length)
            {
                return;
            }

            shaftTransforms = new Transform[shaftRenderers.Length];
            baseLocalPositions = new Vector3[shaftRenderers.Length];
            baseLocalRotations = new Quaternion[shaftRenderers.Length];

            for (var i = 0; i < shaftRenderers.Length; i++)
            {
                var renderer = shaftRenderers[i];
                var shaftTransform = renderer != null ? renderer.transform : null;
                shaftTransforms[i] = shaftTransform;
                baseLocalPositions[i] = shaftTransform != null ? shaftTransform.localPosition : Vector3.zero;
                baseLocalRotations[i] = shaftTransform != null ? shaftTransform.localRotation : Quaternion.identity;
            }
        }

        private bool ShouldRenderShafts()
        {
            if (!centralPlazaOnly)
            {
                return true;
            }

            return areaVisibility != null && areaVisibility.ActiveAreaForReview == FastVsHouseArea.CentralPlaza;
        }

        private float ResolveSunYawDegrees()
        {
            if (directionalSun == null)
            {
                return 0f;
            }

            var forward = Vector3.ProjectOnPlane(directionalSun.transform.forward, Vector3.up);
            if (forward.sqrMagnitude < 0.0001f)
            {
                return 0f;
            }

            return Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        }

        private float ResolveViewAlignmentFactor()
        {
            if (sceneCamera == null || directionalSun == null)
            {
                return 0.5f;
            }

            var view = Vector3.ProjectOnPlane(sceneCamera.transform.forward, Vector3.up).normalized;
            var sun = Vector3.ProjectOnPlane(-directionalSun.transform.forward, Vector3.up).normalized;
            if (view.sqrMagnitude < 0.0001f || sun.sqrMagnitude < 0.0001f)
            {
                return 0.5f;
            }

            return Mathf.Clamp01((Vector3.Dot(view, sun) + 1f) * 0.5f);
        }

        private Vector3 ResolveCameraParallaxOffset()
        {
            if (sceneCamera == null || cameraParallax <= 0f)
            {
                return Vector3.zero;
            }

            var local = transform.InverseTransformPoint(sceneCamera.transform.position);
            return new Vector3(
                Mathf.Clamp(local.x * -cameraParallax, -0.28f, 0.28f),
                Mathf.Clamp(local.y * -cameraParallax * 0.10f, -0.04f, 0.04f),
                Mathf.Clamp(local.z * -cameraParallax, -0.28f, 0.28f));
        }

        private void ApplyRendererAlpha(Renderer renderer, float alpha)
        {
            EnsurePropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);
            var sourceColor = Color.white;
            var material = renderer.sharedMaterial;
            if (material != null && material.HasProperty(BaseColorId))
            {
                sourceColor = material.GetColor(BaseColorId);
            }
            else if (material != null && material.HasProperty(ColorId))
            {
                sourceColor = material.GetColor(ColorId);
            }

            sourceColor.a = alpha;
            propertyBlock.SetColor(BaseColorId, sourceColor);
            propertyBlock.SetColor(ColorId, sourceColor);
            renderer.SetPropertyBlock(propertyBlock);
        }

        private void EnsurePropertyBlock()
        {
            if (propertyBlock == null)
            {
                propertyBlock = new MaterialPropertyBlock();
            }
        }
    }
}
