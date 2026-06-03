using UnityEngine;

namespace Anemora.FastVS
{
    [AddComponentMenu("Anemora/FastVS/HD2D Sun Shaft Dust Mote Field")]
    public sealed class FastVsHd2dSunShaftDustMoteField : MonoBehaviour
    {
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private FastVsDynamicSunShaftField shaftField;
        [SerializeField] private ParticleSystem[] moteSystems;
        [SerializeField] private Renderer[] moteRenderers;
        [SerializeField] private bool centralPlazaOnly = true;
        [SerializeField] private bool motesEnabled = true;
        [SerializeField] private float authoredEmissionRate = 1.15f;
        [SerializeField] private float lowSunEmissionScale = 1.0f;
        [SerializeField] private Color shadedMoteColor = new Color(0.82f, 0.72f, 0.55f, 0.10f);
        [SerializeField] private Color sunlitMoteColor = new Color(1.00f, 0.92f, 0.64f, 0.22f);

        private bool hasReviewOverride;
        private bool reviewEnabled;
        private float reviewEmissionMultiplier = 1f;
        private float appliedEmissionMultiplier;

        public int MoteSystemCountForReview => moteSystems != null ? moteSystems.Length : 0;
        public int MoteRendererCountForReview => moteRenderers != null ? moteRenderers.Length : 0;
        public int TotalMaxParticlesForReview => CountMaxParticles();
        public bool ParticleSystemFallbackForReview => true;
        public bool CentralPlazaOnlyForReview => centralPlazaOnly;
        public bool ActiveForReview => ShouldRenderMotes();
        public float LowSunFactorForReview => ResolveLowSunFactor();
        public float AppliedEmissionMultiplierForReview => appliedEmissionMultiplier;
        public float TotalEmissionRateForReview => EstimateTotalEmissionRate();

        private void Awake()
        {
            ResolveReferences();
            ApplyNowForReview();
        }

        private void OnEnable()
        {
            ResolveReferences();
            ApplyNowForReview();
        }

        private void LateUpdate()
        {
            ApplyNowForReview();
        }

        public void SetReviewOverrideForReview(bool enabled, float emissionMultiplier)
        {
            hasReviewOverride = true;
            reviewEnabled = enabled;
            reviewEmissionMultiplier = Mathf.Max(0f, emissionMultiplier);
            ApplyNowForReview();
        }

        public void ClearReviewOverrideForReview()
        {
            hasReviewOverride = false;
            reviewEmissionMultiplier = 1f;
            ApplyNowForReview();
        }

        public void SimulateForReview(float seconds)
        {
            SimulateForReview(seconds, true);
        }

        public void SimulateForReview(float seconds, bool restart)
        {
            if (moteSystems == null)
            {
                return;
            }

            var duration = Mathf.Max(0f, seconds);
            for (var i = 0; i < moteSystems.Length; i++)
            {
                var system = moteSystems[i];
                if (system == null)
                {
                    continue;
                }

                system.Simulate(duration, true, restart, false);
            }
        }

        public void OffsetMoteSystemsForReview(Vector3 localOffset)
        {
            ResolveReferences();
            if (moteSystems == null)
            {
                return;
            }

            for (var i = 0; i < moteSystems.Length; i++)
            {
                var system = moteSystems[i];
                if (system != null)
                {
                    system.transform.localPosition += localOffset;
                }
            }

            ApplyNowForReview();
        }

        public void ApplyNowForReview()
        {
            ResolveReferences();
            var active = ShouldRenderMotes();
            var lowSunFactor = ResolveLowSunFactor();
            var emissionMultiplier = active ? Mathf.Clamp01(lowSunFactor) * Mathf.Max(0f, lowSunEmissionScale) * reviewEmissionMultiplier : 0f;
            var color = Color.Lerp(shadedMoteColor, sunlitMoteColor, Mathf.Clamp01(lowSunFactor));
            appliedEmissionMultiplier = emissionMultiplier;

            if (moteSystems != null)
            {
                for (var i = 0; i < moteSystems.Length; i++)
                {
                    var system = moteSystems[i];
                    if (system == null)
                    {
                        continue;
                    }

                    var main = system.main;
                    main.startColor = new ParticleSystem.MinMaxGradient(color);

                    var emission = system.emission;
                    emission.enabled = active && emissionMultiplier > 0.001f;
                    emission.rateOverTimeMultiplier = emissionMultiplier;

                    if (active && !system.isPlaying)
                    {
                        system.Play(true);
                    }
                    else if (!active && system.isPlaying)
                    {
                        system.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    }
                }
            }

            if (moteRenderers != null)
            {
                for (var i = 0; i < moteRenderers.Length; i++)
                {
                    var renderer = moteRenderers[i];
                    if (renderer == null)
                    {
                        continue;
                    }

                    renderer.enabled = active && emissionMultiplier > 0.001f;
                    renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    renderer.receiveShadows = false;
                }
            }
        }

        private void ResolveReferences()
        {
            if (areaVisibility == null)
            {
                areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            }

            if (shaftField == null)
            {
                shaftField = FindFirstObjectByType<FastVsDynamicSunShaftField>();
            }

            if (moteSystems == null || moteSystems.Length == 0)
            {
                moteSystems = GetComponentsInChildren<ParticleSystem>(true);
            }

            if (moteRenderers == null || moteRenderers.Length == 0)
            {
                moteRenderers = GetComponentsInChildren<Renderer>(true);
            }
        }

        private bool ShouldRenderMotes()
        {
            if (hasReviewOverride && !reviewEnabled)
            {
                return false;
            }

            if (!hasReviewOverride && !motesEnabled)
            {
                return false;
            }

            if (centralPlazaOnly && areaVisibility != null && areaVisibility.ActiveAreaForReview != FastVsHouseArea.CentralPlaza)
            {
                return false;
            }

            return shaftField == null || shaftField.ActiveForReview;
        }

        private float ResolveLowSunFactor()
        {
            return shaftField != null ? shaftField.LowSunFactorForReview : 0f;
        }

        private int CountMaxParticles()
        {
            var total = 0;
            if (moteSystems == null)
            {
                return total;
            }

            for (var i = 0; i < moteSystems.Length; i++)
            {
                if (moteSystems[i] != null)
                {
                    total += moteSystems[i].main.maxParticles;
                }
            }

            return total;
        }

        private float EstimateTotalEmissionRate()
        {
            var total = 0f;
            if (moteSystems == null)
            {
                return total;
            }

            for (var i = 0; i < moteSystems.Length; i++)
            {
                var system = moteSystems[i];
                if (system == null)
                {
                    continue;
                }

                total += Mathf.Max(0f, authoredEmissionRate) * Mathf.Max(0f, appliedEmissionMultiplier);
            }

            return total;
        }
    }
}
