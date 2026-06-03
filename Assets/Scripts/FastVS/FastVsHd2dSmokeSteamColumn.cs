using UnityEngine;
using UnityEngine.Serialization;

namespace Anemora.FastVS
{
    [ExecuteAlways]
    [DefaultExecutionOrder(120)]
    [DisallowMultipleComponent]
    [AddComponentMenu("Anemora/HD2D/Smoke Steam Column")]
    public sealed class FastVsHd2dSmokeSteamColumn : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dSmokeSteamProfile profile;
        [SerializeField] private FastVsHd2dSmokeSteamKind columnKind;
        [SerializeField, FormerlySerializedAs("particleSystem")] private ParticleSystem smokeParticleSystem;
        [SerializeField] private ParticleSystemRenderer particleRenderer;
        [SerializeField] private bool drivenBySharedAmbientVfxDirector = true;
        [SerializeField] private bool distanceCullEnabled = true;
        [SerializeField, Range(0f, 2.5f)] private float reviewRateMultiplier = 1f;
        [SerializeField, Range(0f, 2.5f)] private float reviewAlphaMultiplier = 1f;
        [SerializeField] private bool reviewVisible = true;

        private bool lastDistanceCulled;

        public FastVsHd2dSmokeSteamProfile ProfileForReview => profile;
        public FastVsHd2dSmokeSteamKind ColumnKindForReview => columnKind;
        public ParticleSystem ParticleSystemForReview => smokeParticleSystem;
        public ParticleSystemRenderer ParticleRendererForReview => particleRenderer;
        public bool DrivenBySharedAmbientVfxDirectorForReview => drivenBySharedAmbientVfxDirector;
        public bool DistanceCullEnabledForReview => distanceCullEnabled;
        public float ReviewRateMultiplierForReview => reviewRateMultiplier;
        public float ReviewAlphaMultiplierForReview => reviewAlphaMultiplier;
        public bool ReviewVisibleForReview => reviewVisible;
        public bool LastDistanceCulledForReview => lastDistanceCulled;
        public int LiveParticleCountForReview => smokeParticleSystem != null ? smokeParticleSystem.particleCount : 0;

        private void OnEnable()
        {
            ResolveReferences();
            ApplyForReview();
            if (reviewVisible && smokeParticleSystem != null && !smokeParticleSystem.isPlaying)
            {
                smokeParticleSystem.Play(true);
            }
        }

        private void LateUpdate()
        {
            ApplyDistanceCullForReview();
        }

        private void OnValidate()
        {
            ApplyForReview();
        }

        public void ConfigureForReview(
            FastVsHd2dSmokeSteamProfile configuredProfile,
            FastVsHd2dSmokeSteamKind configuredKind,
            ParticleSystem configuredParticleSystem,
            ParticleSystemRenderer configuredRenderer,
            bool configuredDrivenBySharedAmbientVfxDirector,
            bool configuredDistanceCullEnabled)
        {
            profile = configuredProfile;
            columnKind = configuredKind;
            smokeParticleSystem = configuredParticleSystem;
            particleRenderer = configuredRenderer;
            drivenBySharedAmbientVfxDirector = configuredDrivenBySharedAmbientVfxDirector;
            distanceCullEnabled = configuredDistanceCullEnabled;
            reviewVisible = true;
            reviewRateMultiplier = 1f;
            reviewAlphaMultiplier = 1f;
            ApplyForReview();
        }

        public void SetReviewVisibleForReview(bool visible)
        {
            reviewVisible = visible;
            ResolveReferences();
            if (smokeParticleSystem != null && !visible)
            {
                smokeParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            ApplyForReview();
            if (smokeParticleSystem != null && visible && !smokeParticleSystem.isPlaying)
            {
                smokeParticleSystem.Play(true);
            }
        }

        public void SetReviewRateMultiplierForReview(float multiplier)
        {
            reviewRateMultiplier = Mathf.Clamp(multiplier, 0f, 2.5f);
            ApplyForReview();
        }

        public void SetReviewAlphaMultiplierForReview(float multiplier)
        {
            reviewAlphaMultiplier = Mathf.Clamp(multiplier, 0f, 2.5f);
            ApplyForReview();
        }

        public void ClearForReview()
        {
            ResolveReferences();
            smokeParticleSystem?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        public void SimulateForReview(float seconds, bool restart)
        {
            ResolveReferences();
            if (smokeParticleSystem == null || !reviewVisible)
            {
                return;
            }

            if (restart)
            {
                smokeParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            ApplyForReview();
            smokeParticleSystem.Simulate(Mathf.Max(0f, seconds), true, restart, false);
            if (!smokeParticleSystem.isPlaying)
            {
                smokeParticleSystem.Play(true);
            }
        }

        public void ApplyForReview()
        {
            ResolveReferences();
            if (smokeParticleSystem == null || profile == null)
            {
                SetRendererEnabled(false);
                return;
            }

            var main = smokeParticleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(ResolveReviewColor());

            var emission = smokeParticleSystem.emission;
            emission.enabled = reviewVisible && !lastDistanceCulled;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(profile.ResolveEmissionRateForReview(columnKind) * reviewRateMultiplier);

            SetRendererEnabled(reviewVisible && !lastDistanceCulled);
        }

        private void ApplyDistanceCullForReview()
        {
            if (profile == null || !distanceCullEnabled)
            {
                lastDistanceCulled = false;
                ApplyForReview();
                return;
            }

            var camera = Camera.main;
            var shouldCull = false;
            if (camera != null)
            {
                shouldCull = Vector3.Distance(camera.transform.position, transform.position) > profile.DistanceCullFarMetersForReview;
            }

            if (shouldCull == lastDistanceCulled)
            {
                return;
            }

            lastDistanceCulled = shouldCull;
            ApplyForReview();
        }

        private Color ResolveReviewColor()
        {
            var color = profile.ResolveColorForReview(columnKind);
            color.a = Mathf.Clamp01(color.a * reviewAlphaMultiplier);
            return color;
        }

        private void ResolveReferences()
        {
            if (smokeParticleSystem == null)
            {
                smokeParticleSystem = GetComponent<ParticleSystem>();
            }

            if (particleRenderer == null)
            {
                particleRenderer = GetComponent<ParticleSystemRenderer>();
            }
        }

        private void SetRendererEnabled(bool enabled)
        {
            if (particleRenderer == null)
            {
                return;
            }

            particleRenderer.enabled = enabled;
            particleRenderer.forceRenderingOff = false;
        }
    }
}
