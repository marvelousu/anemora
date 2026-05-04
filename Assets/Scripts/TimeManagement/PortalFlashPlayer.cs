using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.TimeManagement
{
    /// <summary>
    /// Plays a brief unscaled-time flash through a runtime VolumeProfile instance.
    /// </summary>
    [RequireComponent(typeof(Volume))]
    public sealed class PortalFlashPlayer : MonoBehaviour
    {
        [SerializeField] private Volume volume;
        [SerializeField] private VolumeProfile templateProfile;
        [SerializeField] private AnimationCurve flashCurve = new AnimationCurve(
            new Keyframe(0f, 0f),
            new Keyframe(0.5f, 1f),
            new Keyframe(1f, 0f));
        [SerializeField] private float defaultDuration = 0.05f;
        [SerializeField] private float maxPostExposure = 2.5f;

        private VolumeProfile runtimeProfile;
        private float elapsed;
        private float duration;
        private bool playing;

        public bool IsPlaying => playing;
        public float CurrentWeight => volume != null ? volume.weight : 0f;
        public VolumeProfile RuntimeProfile => runtimeProfile;

        private void Awake()
        {
            if (volume == null)
            {
                volume = GetComponent<Volume>();
            }

            EnsureRuntimeProfile();
        }

        private void Update()
        {
            if (!playing || volume == null)
            {
                return;
            }

            elapsed += Time.unscaledDeltaTime;
            var t = duration > Mathf.Epsilon ? Mathf.Clamp01(elapsed / duration) : 1f;
            volume.weight = Mathf.Clamp01(flashCurve.Evaluate(t));

            if (t >= 1f)
            {
                playing = false;
                volume.weight = 0f;
            }
        }

        private void OnDestroy()
        {
            if (runtimeProfile != null)
            {
                Destroy(runtimeProfile);
            }
        }

        public void PlayOnce()
        {
            PlayOnce(defaultDuration);
        }

        public void PlayOnce(float durationSeconds)
        {
            EnsureRuntimeProfile();
            duration = Mathf.Max(0.001f, durationSeconds);
            elapsed = 0f;
            playing = true;
            if (volume != null)
            {
                volume.weight = 0f;
            }
        }

        private void EnsureRuntimeProfile()
        {
            if (volume == null)
            {
                return;
            }

            if (runtimeProfile != null)
            {
                return;
            }

            runtimeProfile = templateProfile != null
                ? Instantiate(templateProfile)
                : ScriptableObject.CreateInstance<VolumeProfile>();
            runtimeProfile.name = "PortalFlash_RuntimeProfile";

            if (!runtimeProfile.TryGet<ColorAdjustments>(out var colorAdjustments))
            {
                colorAdjustments = runtimeProfile.Add<ColorAdjustments>(true);
            }

            colorAdjustments.postExposure.overrideState = true;
            colorAdjustments.postExposure.value = maxPostExposure;
            colorAdjustments.colorFilter.overrideState = true;
            colorAdjustments.colorFilter.value = Color.white;

            volume.isGlobal = true;
            volume.priority = 100f;
            volume.weight = 0f;
            volume.profile = runtimeProfile;
        }
    }
}
