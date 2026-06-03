using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/FastVS/HD2D Panini Projection Profile")]
    public sealed class FastVsHd2dPaniniProjectionProfile : ScriptableObject
    {
        [Header("URP Panini Projection")]
        [Range(0f, 1f)] public float distance = 0.55f;
        [Range(0f, 1f)] public float cropToFit = 0.85f;

        [Header("Review Camera")]
        [Range(22f, 36f)] public float reviewFieldOfView = 32f;

        [Header("Review State")]
        public bool needsTomApproval = true;
        public bool finalProjectionApproved;
        [TextArea(2, 4)] public string reviewNotes =
            "Conservative P2-54 Panini baseline on the existing shared Global Volume. Tom should tune distance/crop against edge buildings before final approval.";
    }
}
