using System;
using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Parallax Backdrop Profile")]
    public sealed class FastVsHd2dParallaxBackdropProfile : ScriptableObject
    {
        [Serializable]
        public sealed class LayerSettings
        {
            [SerializeField] private string layerId = "far_mountains";
            [SerializeField] private Vector3 localPosition;
            [SerializeField] private Vector3 localScale = Vector3.one;
            [SerializeField, Range(0f, 0.45f)] private float parallaxFactor = 0.05f;
            [SerializeField, Range(0f, 1f)] private float hazeWeight = 0.5f;
            [SerializeField] private Color tint = Color.white;

            public string LayerIdForReview => layerId;
            public Vector3 LocalPositionForReview => localPosition;
            public Vector3 LocalScaleForReview => localScale;
            public float ParallaxFactorForReview => parallaxFactor;
            public float HazeWeightForReview => hazeWeight;
            public Color TintForReview => tint;

            public void ConfigureForReview(
                string configuredLayerId,
                Vector3 configuredLocalPosition,
                Vector3 configuredLocalScale,
                float configuredParallaxFactor,
                float configuredHazeWeight,
                Color configuredTint)
            {
                layerId = configuredLayerId;
                localPosition = configuredLocalPosition;
                localScale = configuredLocalScale;
                parallaxFactor = Mathf.Clamp(configuredParallaxFactor, 0f, 0.45f);
                hazeWeight = Mathf.Clamp01(configuredHazeWeight);
                tint = configuredTint;
            }
        }

        [SerializeField] private LayerSettings[] layers = Array.Empty<LayerSettings>();
        [SerializeField, Range(24f, 120f)] private float farMountainsDistance = 58f;
        [SerializeField, Range(18f, 96f)] private float midHillsDistance = 43f;
        [SerializeField, Range(14f, 80f)] private float lowHazeDistance = 34f;
        [SerializeField, Range(0f, 0.35f)] private float farMountainsParallaxFactor = 0.045f;
        [SerializeField, Range(0f, 0.45f)] private float midHillsParallaxFactor = 0.155f;
        [SerializeField, Range(0f, 0.40f)] private float lowHazeParallaxFactor = 0.095f;
        [SerializeField, Range(0f, 1f)] private float farMountainsHorizonBlend = 0.72f;
        [SerializeField, Range(0f, 1f)] private float midHillsHorizonBlend = 0.46f;
        [SerializeField, Range(0f, 1f)] private float lowHazeHorizonBlend = 0.86f;
        [SerializeField] private Color farMountainsTint = new Color(0.36f, 0.48f, 0.64f, 0.48f);
        [SerializeField] private Color midHillsTint = new Color(0.34f, 0.43f, 0.36f, 0.62f);
        [SerializeField] private Color lowHazeTint = new Color(0.56f, 0.63f, 0.66f, 0.34f);
        [SerializeField, Range(8f, 40f)] private float ringWingAngleDegrees = 22f;
        [SerializeField] private bool followCameraXz = true;

        public float FarMountainsDistanceForReview => farMountainsDistance;
        public float MidHillsDistanceForReview => midHillsDistance;
        public float LowHazeDistanceForReview => lowHazeDistance;
        public float FarMountainsParallaxFactorForReview => farMountainsParallaxFactor;
        public float MidHillsParallaxFactorForReview => midHillsParallaxFactor;
        public float LowHazeParallaxFactorForReview => lowHazeParallaxFactor;
        public float FarMountainsHorizonBlendForReview => farMountainsHorizonBlend;
        public float MidHillsHorizonBlendForReview => midHillsHorizonBlend;
        public float LowHazeHorizonBlendForReview => lowHazeHorizonBlend;
        public Color FarMountainsTintForReview => farMountainsTint;
        public Color MidHillsTintForReview => midHillsTint;
        public Color LowHazeTintForReview => lowHazeTint;
        public float RingWingAngleDegreesForReview => ringWingAngleDegrees;
        public bool FollowCameraXzForReview => followCameraXz;
        public int LayerCountForReview => layers != null ? layers.Length : 0;

        public LayerSettings GetLayerForReview(int index)
        {
            if (layers == null || index < 0 || index >= layers.Length)
            {
                return null;
            }

            return layers[index];
        }

        public void ConfigureForReview(
            float configuredFarDistance,
            float configuredMidDistance,
            float configuredHazeDistance,
            float configuredFarParallax,
            float configuredMidParallax,
            float configuredHazeParallax,
            float configuredFarHorizonBlend,
            float configuredMidHorizonBlend,
            float configuredHazeHorizonBlend,
            Color configuredFarTint,
            Color configuredMidTint,
            Color configuredHazeTint,
            float configuredWingAngleDegrees,
            bool configuredFollowCameraXz)
        {
            farMountainsDistance = Mathf.Clamp(configuredFarDistance, 24f, 120f);
            midHillsDistance = Mathf.Clamp(configuredMidDistance, 18f, 96f);
            lowHazeDistance = Mathf.Clamp(configuredHazeDistance, 14f, 80f);
            farMountainsParallaxFactor = Mathf.Clamp(configuredFarParallax, 0f, 0.35f);
            midHillsParallaxFactor = Mathf.Clamp(configuredMidParallax, 0f, 0.45f);
            lowHazeParallaxFactor = Mathf.Clamp(configuredHazeParallax, 0f, 0.40f);
            farMountainsHorizonBlend = Mathf.Clamp01(configuredFarHorizonBlend);
            midHillsHorizonBlend = Mathf.Clamp01(configuredMidHorizonBlend);
            lowHazeHorizonBlend = Mathf.Clamp01(configuredHazeHorizonBlend);
            farMountainsTint = configuredFarTint;
            midHillsTint = configuredMidTint;
            lowHazeTint = configuredHazeTint;
            ringWingAngleDegrees = Mathf.Clamp(configuredWingAngleDegrees, 8f, 40f);
            followCameraXz = configuredFollowCameraXz;
        }

        public void ConfigureLayersForReview(params LayerSettings[] configuredLayers)
        {
            layers = configuredLayers ?? Array.Empty<LayerSettings>();
        }

        public static LayerSettings CreateLayerForReview(
            string layerId,
            Vector3 localPosition,
            Vector3 localScale,
            float parallaxFactor,
            float hazeWeight,
            Color tint)
        {
            var layer = new LayerSettings();
            layer.ConfigureForReview(layerId, localPosition, localScale, parallaxFactor, hazeWeight, tint);
            return layer;
        }

        public LayerStyle EvaluateLayerForReview(int depthRank, Color horizonColor)
        {
            if (depthRank <= 0)
            {
                return new LayerStyle(
                    Color.Lerp(farMountainsTint, horizonColor, farMountainsHorizonBlend),
                    farMountainsTint.a,
                    farMountainsHorizonBlend,
                    farMountainsParallaxFactor);
            }

            if (depthRank == 1)
            {
                return new LayerStyle(
                    Color.Lerp(midHillsTint, horizonColor, midHillsHorizonBlend),
                    midHillsTint.a,
                    midHillsHorizonBlend,
                    midHillsParallaxFactor);
            }

            return new LayerStyle(
                Color.Lerp(lowHazeTint, horizonColor, lowHazeHorizonBlend),
                lowHazeTint.a,
                lowHazeHorizonBlend,
                lowHazeParallaxFactor);
        }

        public readonly struct LayerStyle
        {
            public readonly Color Tint;
            public readonly float Alpha;
            public readonly float HorizonBlend;
            public readonly float ParallaxFactor;

            public LayerStyle(Color tint, float alpha, float horizonBlend, float parallaxFactor)
            {
                Tint = tint;
                Alpha = Mathf.Clamp01(alpha);
                HorizonBlend = Mathf.Clamp01(horizonBlend);
                ParallaxFactor = parallaxFactor;
            }
        }
    }
}
