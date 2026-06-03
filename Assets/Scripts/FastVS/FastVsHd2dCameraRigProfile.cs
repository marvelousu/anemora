using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dCameraRigProfile", menuName = "Anemora/Fast VS/HD2D Camera Rig Profile")]
    public sealed class FastVsHd2dCameraRigProfile : ScriptableObject
    {
        [SerializeField, Range(22f, 32f)] private float fieldOfView = 32f;
        [SerializeField, Range(28f, 35f)] private float pitchDegrees = 29f;
        [SerializeField, Min(1f)] private float distance = 5.15f;
        [SerializeField] private float targetHeight = 0.72f;
        [SerializeField] private float lookAhead = 0.45f;
        [SerializeField] private float lateralOffset;

        public float FieldOfView => Mathf.Clamp(fieldOfView, 22f, 32f);
        public float PitchDegrees => Mathf.Clamp(pitchDegrees, 28f, 35f);
        public float Distance => Mathf.Max(1f, distance);
        public float TargetHeight => targetHeight;
        public float LookAhead => lookAhead;
        public float LateralOffset => lateralOffset;

        public Vector3 PositionOffset
        {
            get
            {
                var forwardDistance = Mathf.Max(0.1f, Distance + LookAhead);
                var height = TargetHeight + Mathf.Tan(PitchDegrees * Mathf.Deg2Rad) * forwardDistance;
                return new Vector3(LateralOffset, height, -Distance);
            }
        }

        public Vector3 LookOffset => new Vector3(0f, TargetHeight, LookAhead);
    }
}
