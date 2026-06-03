using System;
using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsHd2dFootstepSurfaceKind
    {
        Dust,
        Grass,
        Water
    }

    public sealed class FastVsHd2dFootstepFxEmitter : MonoBehaviour
    {
        private const float FootstepParticleContactLift = 0.055f;

        [SerializeField] private FastVsHd2dFootstepFxProfile profile;
        [SerializeField] private Transform player;
        [SerializeField] private CharacterController playerController;
        [SerializeField] private ParticleSystem footstepSystem;
        [SerializeField] private LayerMask surfaceMask = ~0;
        [SerializeField] private bool enableRuntimeCadence = true;

        private bool hasPreviousPosition;
        private Vector3 previousPosition;
        private Vector3 lastMoveDirection = Vector3.forward;
        private float accumulatedDistance;
        private bool nextLeftFoot = true;
        private int totalEmittedFootsteps;
        private Vector3 lastFootWorldPosition;
        private FastVsHd2dFootstepSurfaceKind lastSurfaceKind = FastVsHd2dFootstepSurfaceKind.Dust;

        public FastVsHd2dFootstepFxProfile ProfileForReview => profile;
        public ParticleSystem ParticleSystemForReview => footstepSystem;
        public bool RuntimeCadenceEnabledForReview => enableRuntimeCadence;
        public bool AnimationEventEntryPointAvailableForReview => profile != null && profile.AnimationEventEntryPointForReview;
        public bool PooledShurikenForReview => profile != null && profile.PooledShurikenRuntimeForReview && footstepSystem != null;
        public int TotalEmittedFootstepsForReview => totalEmittedFootsteps;
        public int LiveParticleCountForReview => footstepSystem != null ? footstepSystem.particleCount : 0;
        public Vector3 LastFootWorldPositionForReview => lastFootWorldPosition;
        public FastVsHd2dFootstepSurfaceKind LastSurfaceKindForReview => lastSurfaceKind;
        public float AccumulatedDistanceForReview => accumulatedDistance;

        private void Awake()
        {
            ResolveReferences();
            PrepareParticleSystem();
        }

        private void OnEnable()
        {
            hasPreviousPosition = false;
            accumulatedDistance = 0f;
            PrepareParticleSystem();
        }

        private void Update()
        {
            if (!Application.isPlaying || !enableRuntimeCadence || profile == null)
            {
                return;
            }

            ResolveReferences();
            if (player == null)
            {
                return;
            }

            var position = player.position;
            if (!hasPreviousPosition)
            {
                previousPosition = position;
                hasPreviousPosition = true;
                return;
            }

            var delta = position - previousPosition;
            previousPosition = position;
            delta.y = 0f;
            var distance = delta.magnitude;
            if (Time.deltaTime <= 0f || distance <= 0.0001f)
            {
                return;
            }

            var speed = distance / Time.deltaTime;
            if (speed < profile.MinMoveSpeedForReview)
            {
                accumulatedDistance = Mathf.Min(accumulatedDistance, profile.StepDistanceForReview * 0.45f);
                return;
            }

            lastMoveDirection = delta.normalized;
            accumulatedDistance += distance;
            var guard = 0;
            while (accumulatedDistance >= profile.StepDistanceForReview && guard < 3)
            {
                accumulatedDistance -= profile.StepDistanceForReview;
                EmitCadenceFootstep();
                guard++;
            }
        }

        public void ConfigureForReview(
            FastVsHd2dFootstepFxProfile configuredProfile,
            Transform configuredPlayer,
            CharacterController configuredPlayerController,
            ParticleSystem configuredFootstepSystem)
        {
            profile = configuredProfile;
            player = configuredPlayer;
            playerController = configuredPlayerController;
            footstepSystem = configuredFootstepSystem;
            enableRuntimeCadence = true;
            PrepareParticleSystem();
        }

        public void SetRuntimeCadenceEnabledForReview(bool enabled)
        {
            enableRuntimeCadence = enabled;
        }

        public void PlayFootstepFX(Transform footTransform)
        {
            if (footTransform == null)
            {
                return;
            }

            PlayFootstepFXAt(footTransform.position);
        }

        public void PlayFootstepFXAt(Vector3 worldPosition)
        {
            EmitAt(worldPosition, ResolveSurfaceAt(worldPosition), lastMoveDirection, nextLeftFoot);
            nextLeftFoot = !nextLeftFoot;
        }

        public void EmitFootstepForReview(
            FastVsHd2dFootstepSurfaceKind surfaceKind,
            Vector3 worldPosition,
            Vector3 movementDirection,
            bool leftFoot)
        {
            EmitAt(worldPosition, surfaceKind, movementDirection.sqrMagnitude > 0.0001f ? movementDirection.normalized : Vector3.forward, leftFoot);
            nextLeftFoot = !leftFoot;
        }

        public Vector3 ResolveFootWorldForReview(Vector3 movementDirection, bool leftFoot)
        {
            ResolveReferences();
            return ResolveFootWorld(movementDirection.sqrMagnitude > 0.0001f ? movementDirection.normalized : Vector3.forward, leftFoot).Position;
        }

        public void ClearForReview()
        {
            totalEmittedFootsteps = 0;
            accumulatedDistance = 0f;
            if (footstepSystem != null)
            {
                footstepSystem.Clear(true);
                footstepSystem.Play(true);
            }
        }

        public void SimulateForReview(float seconds, bool restart)
        {
            if (footstepSystem == null)
            {
                return;
            }

            if (restart)
            {
                footstepSystem.Simulate(0f, true, true, false);
            }

            footstepSystem.Simulate(Mathf.Max(0f, seconds), true, false, false);
        }

        private void EmitCadenceFootstep()
        {
            var resolved = ResolveFootWorld(lastMoveDirection, nextLeftFoot);
            EmitAt(resolved.Position, resolved.SurfaceKind, lastMoveDirection, nextLeftFoot);
            nextLeftFoot = !nextLeftFoot;
        }

        private ResolvedFootstep ResolveFootWorld(Vector3 movementDirection, bool leftFoot)
        {
            ResolveReferences();
            var anchor = player != null ? player.position : transform.position;
            var direction = movementDirection.sqrMagnitude > 0.0001f ? movementDirection.normalized : Vector3.forward;
            var right = Vector3.Cross(Vector3.up, direction);
            if (right.sqrMagnitude <= 0.0001f)
            {
                right = Vector3.right;
            }

            right.Normalize();
            var side = leftFoot ? -1f : 1f;
            var candidate = anchor +
                            direction * (profile != null ? profile.FootForwardOffsetForReview : 0.10f) +
                            right * (side * (profile != null ? profile.FootLateralOffsetForReview : 0.18f));
            var surface = ResolveSurfaceAt(candidate, out var groundPoint);
            return new ResolvedFootstep(groundPoint, surface);
        }

        private FastVsHd2dFootstepSurfaceKind ResolveSurfaceAt(Vector3 candidate)
        {
            return ResolveSurfaceAt(candidate, out _);
        }

        private FastVsHd2dFootstepSurfaceKind ResolveSurfaceAt(Vector3 candidate, out Vector3 groundPoint)
        {
            groundPoint = candidate;
            if (profile == null || !profile.SurfaceRaycastRuntimeForReview)
            {
                groundPoint.y += FootstepParticleContactLift;
                return FastVsHd2dFootstepSurfaceKind.Dust;
            }

            var origin = candidate + Vector3.up * profile.GroundRayHeightForReview;
            if (Physics.Raycast(origin, Vector3.down, out var hit, profile.GroundRayHeightForReview + profile.GroundRayDistanceForReview, surfaceMask, QueryTriggerInteraction.Ignore))
            {
                groundPoint = hit.point + hit.normal * FootstepParticleContactLift;
                return ClassifySurface(hit.collider);
            }

            groundPoint.y += FootstepParticleContactLift;
            return FastVsHd2dFootstepSurfaceKind.Dust;
        }

        private static FastVsHd2dFootstepSurfaceKind ClassifySurface(Collider collider)
        {
            if (collider == null)
            {
                return FastVsHd2dFootstepSurfaceKind.Dust;
            }

            var token = collider.name + " " + collider.gameObject.name;
            var renderer = collider.GetComponent<Renderer>();
            if (renderer != null && renderer.sharedMaterial != null)
            {
                token += " " + renderer.sharedMaterial.name;
            }

            if (token.IndexOf("water", StringComparison.OrdinalIgnoreCase) >= 0 ||
                token.IndexOf("splash", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return FastVsHd2dFootstepSurfaceKind.Water;
            }

            if (token.IndexOf("grass", StringComparison.OrdinalIgnoreCase) >= 0 ||
                token.IndexOf("leaf", StringComparison.OrdinalIgnoreCase) >= 0 ||
                token.IndexOf("foliage", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return FastVsHd2dFootstepSurfaceKind.Grass;
            }

            return FastVsHd2dFootstepSurfaceKind.Dust;
        }

        private void EmitAt(Vector3 position, FastVsHd2dFootstepSurfaceKind surfaceKind, Vector3 movementDirection, bool leftFoot)
        {
            if (profile == null || footstepSystem == null)
            {
                return;
            }

            PrepareParticleSystem();
            var color = ResolveColor(surfaceKind);
            var count = surfaceKind == FastVsHd2dFootstepSurfaceKind.Water ? profile.SplashBurstParticlesForReview : profile.DustBurstParticlesForReview;
            var baseUp = surfaceKind == FastVsHd2dFootstepSurfaceKind.Water ? profile.WaterUpwardVelocityForReview : profile.UpwardVelocityForReview;
            var direction = movementDirection.sqrMagnitude > 0.0001f ? movementDirection.normalized : Vector3.forward;
            var right = Vector3.Cross(Vector3.up, direction);
            if (right.sqrMagnitude <= 0.0001f)
            {
                right = Vector3.right;
            }

            right.Normalize();
            var footSign = leftFoot ? -1f : 1f;
            for (var index = 0; index < count; index++)
            {
                var t = count <= 1 ? 0f : index / (float)(count - 1);
                var angle = (t - 0.5f) * 2f;
                var spread = Mathf.Sin((totalEmittedFootsteps + index + 1) * 12.9898f) * profile.RandomSpreadForReview;
                var outward = right * (footSign * (0.10f + Mathf.Abs(angle) * profile.HorizontalVelocityForReview * 0.32f)) +
                              direction * (profile.HorizontalVelocityForReview * (0.35f + t * 0.45f)) +
                              new Vector3(spread, 0f, -spread * 0.35f);
                var emit = new ParticleSystem.EmitParams
                {
                    position = position + right * (footSign * 0.015f * index / Mathf.Max(1, count)),
                    velocity = outward + Vector3.up * (baseUp * (0.75f + 0.35f * t)),
                    startLifetime = profile.LifetimeForReview,
                    startSize = Mathf.Lerp(profile.StartSizeMinForReview, profile.StartSizeMaxForReview, Mathf.Repeat(t * 1.7f, 1f)),
                    startColor = color,
                    randomSeed = (uint)(1009 + totalEmittedFootsteps * 37 + index)
                };
                footstepSystem.Emit(emit, 1);
            }

            totalEmittedFootsteps++;
            lastFootWorldPosition = position;
            lastSurfaceKind = surfaceKind;
        }

        private Color ResolveColor(FastVsHd2dFootstepSurfaceKind surfaceKind)
        {
            switch (surfaceKind)
            {
                case FastVsHd2dFootstepSurfaceKind.Grass:
                    return profile.GrassColorForReview;
                case FastVsHd2dFootstepSurfaceKind.Water:
                    return profile.WaterColorForReview;
                default:
                    return profile.DustColorForReview;
            }
        }

        private void ResolveReferences()
        {
            if (player == null)
            {
                var controller = playerController != null ? playerController : FindFirstObjectByType<CharacterController>();
                player = controller != null ? controller.transform : transform;
            }

            if (playerController == null && player != null)
            {
                playerController = player.GetComponent<CharacterController>();
            }

            if (footstepSystem == null)
            {
                footstepSystem = GetComponentInChildren<ParticleSystem>(true);
            }
        }

        private void PrepareParticleSystem()
        {
            if (footstepSystem == null)
            {
                return;
            }

            if (!footstepSystem.isPlaying)
            {
                footstepSystem.Play(true);
            }

            var renderer = footstepSystem.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                renderer.forceRenderingOff = false;
                renderer.enabled = true;
            }
        }

        private readonly struct ResolvedFootstep
        {
            public readonly Vector3 Position;
            public readonly FastVsHd2dFootstepSurfaceKind SurfaceKind;

            public ResolvedFootstep(Vector3 position, FastVsHd2dFootstepSurfaceKind surfaceKind)
            {
                Position = position;
                SurfaceKind = surfaceKind;
            }
        }
    }
}
