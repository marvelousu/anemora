using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/FastVS/HD2D Ground Painterly Value Profile")]
    public sealed class FastVsHd2dGroundPainterlyValueProfile : ScriptableObject
    {
        [Header("World-Space Value")]
        public Vector2 centerXZ = new Vector2(20.8f, 15.8f);
        [Range(1f, 40f)] public float radius = 12f;
        [Range(0f, 0.35f)] public float valueStrength = 0.12f;
        [Range(0f, 0.4f)] public float aoStrength = 0.18f;
        public Color warmCenterTint = new Color(1.04f, 1.02f, 0.94f, 1f);
        public Color coolRecessTint = new Color(0.86f, 0.91f, 1.00f, 1f);

        [Header("Review State")]
        public bool needsTomApproval = true;
        public bool finalGroundArtApproved;
        [TextArea(2, 4)] public string reviewNotes =
            "Conservative P2-55 ground value/AO baseline. Tom should tune center/radius/tints and replace procedural recess AO with painted vertex/texture masks before final approval.";
    }
}
