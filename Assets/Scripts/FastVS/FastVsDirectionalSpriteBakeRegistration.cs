using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsDirectionalSpriteBakeRegistration : MonoBehaviour
    {
        [SerializeField] private FastVsDirectionalSpriteBakeSet bakeSet;

        public FastVsDirectionalSpriteBakeSet BakeSetForReview => bakeSet;
        public bool HasValidBakeSetForReview =>
            bakeSet != null && bakeSet.HasRegisteredDiffuseAndNormalSheetsForReview;
    }
}
