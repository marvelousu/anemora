using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Footstep FX Profile")]
    public sealed class FastVsHd2dFootstepFxProfile : ScriptableObject
    {
        [SerializeField, Min(24)] private int maxParticles = 128;
        [SerializeField, Min(0.05f)] private float stepDistance = 0.58f;
        [SerializeField, Min(0f)] private float minMoveSpeed = 0.22f;
        [SerializeField, Min(0f)] private float footLateralOffset = 0.18f;
        [SerializeField, Min(0f)] private float footForwardOffset = 0.10f;
        [SerializeField, Min(0f)] private float groundRayHeight = 0.65f;
        [SerializeField, Min(0.05f)] private float groundRayDistance = 1.35f;
        [SerializeField, Min(1)] private int dustBurstParticles = 20;
        [SerializeField, Min(1)] private int splashBurstParticles = 24;
        [SerializeField, Min(0.05f)] private float lifetime = 0.48f;
        [SerializeField, Min(0.001f)] private float startSizeMin = 0.075f;
        [SerializeField, Min(0.001f)] private float startSizeMax = 0.180f;
        [SerializeField, Min(0f)] private float horizontalVelocity = 0.44f;
        [SerializeField, Min(0f)] private float upwardVelocity = 0.42f;
        [SerializeField, Min(0f)] private float waterUpwardVelocity = 0.68f;
        [SerializeField, Min(0f)] private float randomSpread = 0.25f;
        [SerializeField] private Color dustColor = new Color(0.86f, 0.70f, 0.46f, 0.82f);
        [SerializeField] private Color grassColor = new Color(0.56f, 0.78f, 0.36f, 0.78f);
        [SerializeField] private Color waterColor = new Color(0.55f, 0.88f, 1.00f, 0.85f);
        [SerializeField] private bool stepCadenceRuntime = true;
        [SerializeField] private bool surfaceRaycastRuntime = true;
        [SerializeField] private bool pooledShurikenRuntime = true;
        [SerializeField] private bool animationEventEntryPoint = true;
        [SerializeField] private bool autoSafeComplete = true;
        [SerializeField] private string sourceNote = "Auto-safe pooled Shuriken footstep dust/scuff puffs. Cadence is distance-based for billboard movement; PlayFootstepFX is available for authored Animation Events.";

        public int MaxParticlesForReview => maxParticles;
        public float StepDistanceForReview => stepDistance;
        public float MinMoveSpeedForReview => minMoveSpeed;
        public float FootLateralOffsetForReview => footLateralOffset;
        public float FootForwardOffsetForReview => footForwardOffset;
        public float GroundRayHeightForReview => groundRayHeight;
        public float GroundRayDistanceForReview => groundRayDistance;
        public int DustBurstParticlesForReview => dustBurstParticles;
        public int SplashBurstParticlesForReview => splashBurstParticles;
        public float LifetimeForReview => lifetime;
        public float StartSizeMinForReview => startSizeMin;
        public float StartSizeMaxForReview => startSizeMax;
        public float HorizontalVelocityForReview => horizontalVelocity;
        public float UpwardVelocityForReview => upwardVelocity;
        public float WaterUpwardVelocityForReview => waterUpwardVelocity;
        public float RandomSpreadForReview => randomSpread;
        public Color DustColorForReview => dustColor;
        public Color GrassColorForReview => grassColor;
        public Color WaterColorForReview => waterColor;
        public bool StepCadenceRuntimeForReview => stepCadenceRuntime;
        public bool SurfaceRaycastRuntimeForReview => surfaceRaycastRuntime;
        public bool PooledShurikenRuntimeForReview => pooledShurikenRuntime;
        public bool AnimationEventEntryPointForReview => animationEventEntryPoint;
        public bool AutoSafeCompleteForReview => autoSafeComplete;
        public string SourceNoteForReview => sourceNote;

        public void ConfigureForReview(
            int configuredMaxParticles,
            float configuredStepDistance,
            float configuredMinMoveSpeed,
            float configuredFootLateralOffset,
            float configuredFootForwardOffset,
            float configuredGroundRayHeight,
            float configuredGroundRayDistance,
            int configuredDustBurstParticles,
            int configuredSplashBurstParticles,
            float configuredLifetime,
            float configuredStartSizeMin,
            float configuredStartSizeMax,
            float configuredHorizontalVelocity,
            float configuredUpwardVelocity,
            float configuredWaterUpwardVelocity,
            float configuredRandomSpread,
            Color configuredDustColor,
            Color configuredGrassColor,
            Color configuredWaterColor,
            bool configuredStepCadenceRuntime,
            bool configuredSurfaceRaycastRuntime,
            bool configuredPooledShurikenRuntime,
            bool configuredAnimationEventEntryPoint,
            bool configuredAutoSafeComplete,
            string configuredSourceNote)
        {
            maxParticles = Mathf.Clamp(configuredMaxParticles, 24, 160);
            stepDistance = Mathf.Max(0.05f, configuredStepDistance);
            minMoveSpeed = Mathf.Max(0f, configuredMinMoveSpeed);
            footLateralOffset = Mathf.Clamp(configuredFootLateralOffset, 0.02f, 0.40f);
            footForwardOffset = Mathf.Clamp(configuredFootForwardOffset, 0f, 0.35f);
            groundRayHeight = Mathf.Max(0.10f, configuredGroundRayHeight);
            groundRayDistance = Mathf.Max(0.10f, configuredGroundRayDistance);
            dustBurstParticles = Mathf.Clamp(configuredDustBurstParticles, 1, 32);
            splashBurstParticles = Mathf.Clamp(configuredSplashBurstParticles, 1, 36);
            lifetime = Mathf.Clamp(configuredLifetime, 0.08f, 0.80f);
            startSizeMin = Mathf.Max(0.001f, Mathf.Min(configuredStartSizeMin, configuredStartSizeMax));
            startSizeMax = Mathf.Max(startSizeMin, configuredStartSizeMax);
            horizontalVelocity = Mathf.Max(0f, configuredHorizontalVelocity);
            upwardVelocity = Mathf.Max(0f, configuredUpwardVelocity);
            waterUpwardVelocity = Mathf.Max(upwardVelocity, configuredWaterUpwardVelocity);
            randomSpread = Mathf.Max(0f, configuredRandomSpread);
            dustColor = ClampVisibleAlpha(configuredDustColor);
            grassColor = ClampVisibleAlpha(configuredGrassColor);
            waterColor = ClampVisibleAlpha(configuredWaterColor);
            stepCadenceRuntime = configuredStepCadenceRuntime;
            surfaceRaycastRuntime = configuredSurfaceRaycastRuntime;
            pooledShurikenRuntime = configuredPooledShurikenRuntime;
            animationEventEntryPoint = configuredAnimationEventEntryPoint;
            autoSafeComplete = configuredAutoSafeComplete;
            sourceNote = configuredSourceNote ?? string.Empty;
        }

        private static Color ClampVisibleAlpha(Color color)
        {
            color.a = Mathf.Clamp(color.a, 0.08f, 0.85f);
            return color;
        }
    }
}
