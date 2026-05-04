using System;
using UnityEngine;

namespace Anemora.TimeManagement
{
    public enum SceneSide
    {
        Current,
        Past
    }

    /// <summary>
    /// Holds the dominant scene side and notifies after visual/collision state is already applied.
    /// </summary>
    public sealed class SceneSidePolarity : MonoBehaviour
    {
        [SerializeField] private SceneSide currentSide = SceneSide.Current;

        public event Action<SceneSide> SideChanged;

        public SceneSide CurrentSide => currentSide;

        public bool FlipTo(SceneSide targetSide)
        {
            if (currentSide == targetSide)
            {
                return false;
            }

            currentSide = targetSide;
            SideChanged?.Invoke(currentSide);
            return true;
        }

        public static SceneSide OppositeOf(SceneSide side)
        {
            return side == SceneSide.Current ? SceneSide.Past : SceneSide.Current;
        }
    }
}
