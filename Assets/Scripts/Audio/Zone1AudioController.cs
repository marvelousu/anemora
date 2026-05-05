using UnityEngine;

namespace Anemora.Audio
{
    public enum Zone1FootstepSurface
    {
        Stone,
        Wood,
        Grass,
        Sand
    }

    public sealed class Zone1AudioController : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField] private bool autoPlayOnStart = true;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource windAmbienceSource;
        [SerializeField] private AudioSource padAmbienceSource;
        [SerializeField] private AudioSource oneShotSource;

        [Header("Music")]
        [SerializeField] private AudioClip zone1AmbientClip;
        [SerializeField, Range(0f, 1f)] private float musicVolume = 0.45f;

        [Header("Ambience")]
        [SerializeField] private AudioClip windAmbienceClip;
        [SerializeField] private AudioClip silencePadClip;
        [SerializeField] private AudioClip[] environmentOneShotClips;
        [SerializeField] private Vector2 environmentOneShotDelayRange = new Vector2(14f, 32f);
        [SerializeField, Range(0f, 1f)] private float windAmbienceVolume = 0.35f;
        [SerializeField, Range(0f, 1f)] private float silencePadVolume = 0.22f;
        [SerializeField, Range(0f, 1f)] private float environmentOneShotVolume = 0.35f;
        [SerializeField] private bool playEnvironmentOneShots = true;

        [Header("Footsteps")]
        [SerializeField] private AudioClip[] stoneFootstepWalkClips;
        [SerializeField] private AudioClip[] stoneFootstepRunClips;
        [SerializeField] private AudioClip[] stoneFootstepLandClips;
        [SerializeField] private AudioClip[] woodFootstepWalkClips;
        [SerializeField] private AudioClip[] woodFootstepRunClips;
        [SerializeField] private AudioClip[] woodFootstepLandClips;
        [SerializeField] private AudioClip[] grassFootstepWalkClips;
        [SerializeField] private AudioClip[] grassFootstepRunClips;
        [SerializeField] private AudioClip[] grassFootstepLandClips;
        [SerializeField] private AudioClip[] sandFootstepWalkClips;
        [SerializeField] private AudioClip[] sandFootstepRunClips;
        [SerializeField] private AudioClip[] sandFootstepLandClips;
        [SerializeField, Range(0f, 1f)] private float footstepVolume = 0.42f;

        [Header("Time Window")]
        [SerializeField] private AudioClip wheelOpenClip;
        [SerializeField] private AudioClip wheelCloseClip;
        [SerializeField] private AudioClip symbolHoverClip;
        [SerializeField] private AudioClip symbolSelectRedClip;
        [SerializeField] private AudioClip portalOpenClip;
        [SerializeField] private AudioClip portalFlipClip;
        [SerializeField, Range(0f, 1f)] private float timeWindowVolume = 0.7f;

        [Header("NPC")]
        [SerializeField] private AudioClip npcGreetingClip;
        [SerializeField] private AudioClip npcInteractionAckClip;
        [SerializeField] private AudioClip npcDepartureClip;
        [SerializeField, Range(0f, 1f)] private float npcVolume = 0.7f;

        [Header("UI")]
        [SerializeField] private AudioClip uiButtonClickClip;
        [SerializeField] private AudioClip uiMenuOpenClip;
        [SerializeField] private AudioClip uiMenuCloseClip;
        [SerializeField, Range(0f, 1f)] private float uiVolume = 0.65f;

        private float environmentOneShotTimer;
        private bool environmentOneShotsArmed;

        public static Zone1AudioController Instance { get; private set; }

        public AudioClip Zone1AmbientClip => zone1AmbientClip;
        public AudioClip WindAmbienceClip => windAmbienceClip;
        public AudioClip PortalOpenClip => portalOpenClip;
        public AudioClip PortalFlipClip => portalFlipClip;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                enabled = false;
                return;
            }

            Instance = this;
            EnsureSources();
        }

        private void Start()
        {
            if (!autoPlayOnStart)
            {
                return;
            }

            PlayZone1Music();
            PlayAmbienceLoops();
            ScheduleNextEnvironmentOneShot();
            environmentOneShotsArmed = true;
        }

        private void Update()
        {
            if (!environmentOneShotsArmed ||
                !playEnvironmentOneShots ||
                environmentOneShotClips == null ||
                environmentOneShotClips.Length == 0)
            {
                return;
            }

            environmentOneShotTimer -= Time.deltaTime;
            if (environmentOneShotTimer > 0f)
            {
                return;
            }

            PlayRandomClip(environmentOneShotClips, environmentOneShotVolume);
            ScheduleNextEnvironmentOneShot();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        public void PlayZone1Music()
        {
            PlayLoop(musicSource, zone1AmbientClip, musicVolume);
        }

        public void PlayAmbienceLoops()
        {
            PlayLoop(windAmbienceSource, windAmbienceClip, windAmbienceVolume);
            PlayLoop(padAmbienceSource, silencePadClip, silencePadVolume);
        }

        public void PlayFootstep(Zone1FootstepSurface surface, bool running = false)
        {
            PlayRandomClip(ResolveFootstepClips(surface, running, false), footstepVolume);
        }

        public void PlayFootstepLand(Zone1FootstepSurface surface)
        {
            PlayRandomClip(ResolveFootstepClips(surface, false, true), footstepVolume);
        }

        public void PlayTimeWheelOpen()
        {
            PlayOneShot(wheelOpenClip, timeWindowVolume);
        }

        public void PlayTimeWheelClose()
        {
            PlayOneShot(wheelCloseClip, timeWindowVolume);
        }

        public void PlayTimeSymbolHover()
        {
            PlayOneShot(symbolHoverClip, timeWindowVolume * 0.8f);
        }

        public void PlayTimeSymbolSelectRed()
        {
            PlayOneShot(symbolSelectRedClip, timeWindowVolume);
        }

        public void PlayTimePortalOpen()
        {
            PlayOneShot(portalOpenClip, timeWindowVolume);
        }

        public void PlayTimePortalFlip()
        {
            PlayOneShot(portalFlipClip, timeWindowVolume);
        }

        public void PlayNpcGreeting()
        {
            PlayOneShot(npcGreetingClip, npcVolume);
        }

        public void PlayNpcInteractionAck()
        {
            PlayOneShot(npcInteractionAckClip, npcVolume);
        }

        public void PlayNpcDeparture()
        {
            PlayOneShot(npcDepartureClip, npcVolume);
        }

        public void PlayUiButtonClick()
        {
            PlayOneShot(uiButtonClickClip, uiVolume);
        }

        public void PlayUiMenuOpen()
        {
            PlayOneShot(uiMenuOpenClip, uiVolume);
        }

        public void PlayUiMenuClose()
        {
            PlayOneShot(uiMenuCloseClip, uiVolume);
        }

        private void EnsureSources()
        {
            musicSource = EnsureSource(musicSource, "Music_Source", true, musicVolume);
            windAmbienceSource = EnsureSource(windAmbienceSource, "Wind_Ambience_Source", true, windAmbienceVolume);
            padAmbienceSource = EnsureSource(padAmbienceSource, "Pad_Ambience_Source", true, silencePadVolume);
            oneShotSource = EnsureSource(oneShotSource, "OneShot_Source", false, 1f);
        }

        private AudioSource EnsureSource(AudioSource source, string childName, bool loop, float volume)
        {
            if (source == null)
            {
                var child = transform.Find(childName);
                if (child == null)
                {
                    child = new GameObject(childName).transform;
                    child.SetParent(transform, false);
                }

                source = child.GetComponent<AudioSource>();
                if (source == null)
                {
                    source = child.gameObject.AddComponent<AudioSource>();
                }
            }

            source.playOnAwake = false;
            source.loop = loop;
            source.volume = volume;
            source.spatialBlend = 0f;
            return source;
        }

        private static void PlayLoop(AudioSource source, AudioClip clip, float volume)
        {
            if (source == null || clip == null)
            {
                return;
            }

            if (source.clip == clip && source.isPlaying)
            {
                return;
            }

            source.clip = clip;
            source.loop = true;
            source.volume = volume;
            source.Play();
        }

        private void PlayOneShot(AudioClip clip, float volumeScale)
        {
            if (clip == null)
            {
                return;
            }

            if (oneShotSource == null)
            {
                EnsureSources();
            }

            oneShotSource.PlayOneShot(clip, volumeScale);
        }

        private void PlayRandomClip(AudioClip[] clips, float volumeScale)
        {
            if (clips == null || clips.Length == 0)
            {
                return;
            }

            var clip = clips[Random.Range(0, clips.Length)];
            PlayOneShot(clip, volumeScale);
        }

        private AudioClip[] ResolveFootstepClips(Zone1FootstepSurface surface, bool running, bool landing)
        {
            switch (surface)
            {
                case Zone1FootstepSurface.Wood:
                    return landing ? woodFootstepLandClips : running ? woodFootstepRunClips : woodFootstepWalkClips;
                case Zone1FootstepSurface.Grass:
                    return landing ? grassFootstepLandClips : running ? grassFootstepRunClips : grassFootstepWalkClips;
                case Zone1FootstepSurface.Sand:
                    return landing ? sandFootstepLandClips : running ? sandFootstepRunClips : sandFootstepWalkClips;
                default:
                    return landing ? stoneFootstepLandClips : running ? stoneFootstepRunClips : stoneFootstepWalkClips;
            }
        }

        private void ScheduleNextEnvironmentOneShot()
        {
            var minDelay = Mathf.Max(1f, environmentOneShotDelayRange.x);
            var maxDelay = Mathf.Max(minDelay, environmentOneShotDelayRange.y);
            environmentOneShotTimer = Random.Range(minDelay, maxDelay);
        }
    }
}
