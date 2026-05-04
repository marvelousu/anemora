using System;
using UnityEngine;

namespace Anemora.TimeManagement
{
    /// <summary>
    /// Detects crossing of a portal plane with signed-distance hysteresis.
    /// </summary>
    public sealed class PortalCrossingDetector : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Transform portalPlane;
        [SerializeField] private Vector3 planeOrigin = Vector3.zero;
        [SerializeField] private Vector3 planeNormal = Vector3.forward;
        [SerializeField] private float hysteresisBand = 0.02f;
        [SerializeField] private float minNormalMovement = 0.05f;
        [SerializeField] private bool armed;

        private float lastStableSignedDistance;

        public event Action Crossed;

        public bool IsArmed => armed;
        public float HysteresisBand => hysteresisBand;
        public float MinNormalMovement => minNormalMovement;
        public float LastStableSignedDistance => lastStableSignedDistance;
        public Vector3 PlaneOrigin => portalPlane != null ? portalPlane.position : planeOrigin;
        public Vector3 PlaneNormal => ResolvePlaneNormal();

        public static bool ShouldFlip(
            Vector3 playerPos,
            Vector3 portalPlaneOrigin,
            Vector3 portalPlaneNormal,
            float lastStableSignedDistance,
            float hysteresisBand = 0.02f,
            float minNormalMovement = 0.05f)
        {
            var normal = portalPlaneNormal.sqrMagnitude > Mathf.Epsilon
                ? portalPlaneNormal.normalized
                : Vector3.forward;
            var d = Vector3.Dot(playerPos - portalPlaneOrigin, normal);

            if (Mathf.Abs(d) < hysteresisBand)
            {
                return false;
            }

            var movement = d - lastStableSignedDistance;
            if (Mathf.Abs(movement) < minNormalMovement)
            {
                return false;
            }

            return Mathf.Sign(d) != Mathf.Sign(lastStableSignedDistance);
        }

        private void OnEnable()
        {
            ResetStableDistanceFromPlayer();
        }

        private void Update()
        {
            Tick(Time.unscaledDeltaTime);
        }

        public void Configure(Transform player, Transform portalPlane)
        {
            this.player = player;
            this.portalPlane = portalPlane;
            ResetStableDistanceFromPlayer();
        }

        public void SetArmed(bool armed)
        {
            this.armed = armed;
            if (armed)
            {
                ResetStableDistanceFromPlayer();
            }
        }

        public void SetLastStableSignedDistance(float signedDistance)
        {
            lastStableSignedDistance = signedDistance;
        }

        public void SetLastStableSignedDistance(Vector3 planeNormal, Vector3 playerPosition)
        {
            if (planeNormal.sqrMagnitude > Mathf.Epsilon)
            {
                this.planeNormal = planeNormal.normalized;
            }

            lastStableSignedDistance = GetSignedDistance(playerPosition);
        }

        public float GetSignedDistance(Vector3 position)
        {
            return Vector3.Dot(position - PlaneOrigin, PlaneNormal);
        }

        public void Tick(float unscaledDeltaTime)
        {
            if (!armed || player == null)
            {
                return;
            }

            var currentDistance = GetSignedDistance(player.position);
            if (ShouldFlip(
                    player.position,
                    PlaneOrigin,
                    PlaneNormal,
                    lastStableSignedDistance,
                    hysteresisBand,
                    minNormalMovement))
            {
                armed = false;
                Crossed?.Invoke();
                return;
            }

            if (Mathf.Abs(currentDistance) >= hysteresisBand &&
                Mathf.Sign(currentDistance) == Mathf.Sign(lastStableSignedDistance))
            {
                lastStableSignedDistance = currentDistance;
            }
        }

        private void ResetStableDistanceFromPlayer()
        {
            if (player != null)
            {
                lastStableSignedDistance = GetSignedDistance(player.position);
            }
        }

        private Vector3 ResolvePlaneNormal()
        {
            if (portalPlane != null)
            {
                return portalPlane.forward.sqrMagnitude > Mathf.Epsilon
                    ? portalPlane.forward.normalized
                    : Vector3.forward;
            }

            return planeNormal.sqrMagnitude > Mathf.Epsilon
                ? planeNormal.normalized
                : Vector3.forward;
        }
    }
}
