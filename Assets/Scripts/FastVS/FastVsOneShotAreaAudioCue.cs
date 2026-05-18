using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsOneShotAreaAudioCue : MonoBehaviour
    {
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;
        [SerializeField] private FastVsHouseArea triggerArea;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private bool hasPlayed;

        private void Update()
        {
            if (hasPlayed)
            {
                return;
            }

            ResolveReferences();
            if (areaVisibility == null || audioSource == null || audioSource.clip == null)
            {
                return;
            }

            if (areaVisibility.ActiveAreaForReview != triggerArea)
            {
                return;
            }

            audioSource.loop = false;
            audioSource.Play();
            hasPlayed = true;
        }

        private void ResolveReferences()
        {
            if (areaVisibility == null)
            {
                areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            }

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }
    }
}
