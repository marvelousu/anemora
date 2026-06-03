using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dAerialRampTintProfile", menuName = "Anemora/HD2D/Aerial Ramp Tint Profile")]
    public sealed class FastVsHd2dAerialRampTintProfile : ScriptableObject
    {
        [Range(0f, 0.28f)] public float strength = 0.16f;
        public float distanceStartOffset = 1.0f;
        public float distanceEndOffset = 5.0f;
        public Color currentTint = new Color(0.54f, 0.64f, 0.74f, 1f);
        public Color pastTint = new Color(0.74f, 0.56f, 0.42f, 1f);
        public bool enableGroundAndBuildingMaterials = true;
        public bool needsTomApproval = true;
        public bool finalAerialTintApproved;
        [TextArea(2, 4)] public string reviewNotes;
    }
}
