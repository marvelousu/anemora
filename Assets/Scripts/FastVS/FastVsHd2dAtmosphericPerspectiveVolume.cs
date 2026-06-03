using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.FastVS
{
    [System.Serializable]
    [VolumeComponentMenu("Anemora/HD2D/Atmospheric Perspective")]
    public sealed class FastVsHd2dAtmosphericPerspectiveVolume : VolumeComponent
    {
        public ClampedFloatParameter strength = new ClampedFloatParameter(0.12f, 0f, 0.35f, true);
        public ColorParameter nearColor = new ColorParameter(new Color(0.88f, 0.78f, 0.62f, 1f), false, false, true, true);
        public ColorParameter farColor = new ColorParameter(new Color(0.54f, 0.64f, 0.74f, 1f), false, false, true, true);
        public TextureParameter colorGradient = new TextureParameter(null, true);
        public MinFloatParameter distanceStart = new MinFloatParameter(3.5f, 0f, true);
        public MinFloatParameter distanceEnd = new MinFloatParameter(12f, 0.1f, true);
        public Vector2Parameter heightBand = new Vector2Parameter(new Vector2(-0.4f, 3.1f), true);
        public ClampedFloatParameter heightStrength = new ClampedFloatParameter(0.32f, 0f, 1f, true);

        public bool HasUsableDistanceBand => distanceEnd.value > distanceStart.value + 0.25f;
        public bool IsUsable => active && strength.value > 0.001f && HasUsableDistanceBand;
    }
}
