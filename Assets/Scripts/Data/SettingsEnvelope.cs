using System;

namespace Anemora.Data
{
    [Serializable]
    public sealed class AccessibilitySaveData
    {
        public float uiScale = 1.0f;
        public float subtitleSize = 1.0f;
        public bool highContrast;
    }

    [Serializable]
    public sealed class AudioSettingsSaveData
    {
        public float masterVolume = 1.0f;
        public float musicVolume = 1.0f;
        public float sfxVolume = 1.0f;
        public float ambientVolume = 1.0f;
    }

    [Serializable]
    public sealed class DisplaySettingsSaveData
    {
        public int width;
        public int height;
        public bool fullscreen = true;
        public int targetFrameRate = 60;
    }

    [Serializable]
    public sealed class SettingsEnvelope
    {
        public int settingsVersion;
        public string buildVersion;
        public AccessibilitySaveData accessibility;
        public AudioSettingsSaveData audio;
        public DisplaySettingsSaveData display;
    }
}
