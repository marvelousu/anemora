using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsMapMoveGlowPulse : MonoBehaviour
    {
        [SerializeField] private float bobAmplitude = 0.018f;
        [SerializeField] private float scaleAmplitude = 0.075f;
        [SerializeField] private float speed = 1.65f;

        private Vector3 baseLocalPosition;
        private Vector3 baseLocalScale;

        private void Awake()
        {
            baseLocalPosition = transform.localPosition;
            baseLocalScale = transform.localScale;
        }

        private void Update()
        {
            var pulse = Mathf.Sin(Time.time * speed);
            transform.localPosition = baseLocalPosition + Vector3.up * (pulse * bobAmplitude);
            transform.localScale = baseLocalScale * (1f + pulse * scaleAmplitude);
        }
    }
}
