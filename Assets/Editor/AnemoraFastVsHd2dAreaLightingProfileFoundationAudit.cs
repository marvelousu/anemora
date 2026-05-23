using System;
using System.Collections.Generic;
using Anemora.FastVS;
using UnityEditor;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dAreaLightingProfileFoundationAudit
    {
        private const float PositionTolerance = 0.01f;
        private const float FloatTolerance = 0.001f;
        private const float RotationToleranceDegrees = 0.25f;
        private static readonly Vector2 InteriorLuminanceBand = new Vector2(0.15f, 0.23f);
        private static readonly Vector2 ExteriorLuminanceBand = new Vector2(0.19f, 0.27f);
        private static readonly Vector2 CentralPlazaLuminanceBand = new Vector2(0.18f, 0.26f);
        private static readonly Vector2 LibraryLuminanceBand = new Vector2(0.11f, 0.18f);

        [MenuItem("Tools/Anemora/Verify HD2D Area Lighting Profiles V1")]
        public static void VerifyAreaLightingProfilesV1()
        {
            var issues = new List<string>();
            var director = UnityEngine.Object.FindFirstObjectByType<FastVsHouseLightingDirector>(FindObjectsInactive.Include);
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>(FindObjectsInactive.Include);
            var mainLight = GetLight("Directional Light");
            var warmFill = GetLight("FastVS_HD2D_WarmFillLight");

            if (director == null)
            {
                issues.Add("Missing FastVS_HD2D_LightingDirector for area lighting profile sync validation.");
            }

            if (mainLight == null)
            {
                issues.Add("Missing Directional Light for area lighting profile sync validation.");
            }

            if (warmFill == null)
            {
                issues.Add("Missing FastVS_HD2D_WarmFillLight for area lighting profile sync validation.");
            }

            ValidateProfile(
                issues,
                director,
                mainLight,
                warmFill,
                "FastVS_HD2D_HouseInteriorLightingProfile",
                "FastVS_Current_NiroHouseInteriorExterior",
                "Current_HouseInteriorMap_SeparateSpace",
                FastVsHouseArea.Interior,
                "house interior",
                true,
                new Vector3(-8.35f, 0f, -8.35f),
                InteriorLuminanceBand,
                new Vector3(46f, -42f, 0f),
                0.85f,
                new Color(1.00f, 0.84f, 0.62f, 1f),
                0.27f,
                new Color(1.00f, 0.68f, 0.42f, 1f),
                0.163f,
                new Color(0.170f, 0.160f, 0.160f, 1f));

            ValidateProfile(
                issues,
                director,
                mainLight,
                warmFill,
                "FastVS_HD2D_HouseExteriorLightingProfile",
                "FastVS_Current_NiroHouseInteriorExterior",
                "Current_HouseExteriorMap_SeparateSpace",
                FastVsHouseArea.Exterior,
                "house exterior",
                false,
                new Vector3(8.20f, 0f, 8.20f),
                ExteriorLuminanceBand,
                new Vector3(50f, -36f, 0f),
                1.06f,
                new Color(1.00f, 0.90f, 0.72f, 1f),
                0.13f,
                new Color(1.00f, 0.70f, 0.42f, 1f),
                0.209f,
                new Color(0.200f, 0.210f, 0.220f, 1f));

            ValidateProfile(
                issues,
                director,
                mainLight,
                warmFill,
                "FastVS_HD2D_CentralPlazaLightingProfile",
                "FastVS_Current_NiroHouseInteriorExterior",
                "Current_CentralPlazaMap_SeparateSpace",
                FastVsHouseArea.CentralPlaza,
                "central plaza",
                false,
                new Vector3(20.80f, 0f, 15.80f),
                CentralPlazaLuminanceBand,
                new Vector3(49f, -31f, 0f),
                1.08f,
                new Color(1.00f, 0.88f, 0.68f, 1f),
                0.10f,
                new Color(1.00f, 0.68f, 0.38f, 1f),
                0.199f,
                new Color(0.190f, 0.200f, 0.210f, 1f));

            ValidateProfile(
                issues,
                director,
                mainLight,
                warmFill,
                "FastVS_HD2D_LibraryLightingProfile",
                "FastVS_Current_NiroHouseInteriorExterior",
                "Current_LibraryMap_SeparateSpace",
                FastVsHouseArea.Library,
                "library",
                true,
                new Vector3(31.00f, 0f, 20.00f),
                LibraryLuminanceBand,
                new Vector3(54f, -28f, 0f),
                0.75f,
                new Color(1.00f, 0.82f, 0.58f, 1f),
                0.19f,
                new Color(1.00f, 0.66f, 0.38f, 1f),
                0.112f,
                new Color(0.120f, 0.110f, 0.100f, 1f));

            if (director != null && visibility != null)
            {
                director.ApplyAreaForReview(visibility.ActiveAreaForReview);
            }

            if (issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D area lighting profile audit failed:\n- " + string.Join("\n- ", issues));
            }

            Debug.Log("HD2D area lighting profile audit passed.");
        }

        private static void ValidateProfile(
            List<string> issues,
            FastVsHouseLightingDirector director,
            Light mainLight,
            Light warmFill,
            string objectName,
            string expectedRootName,
            string expectedParentName,
            FastVsHouseArea expectedArea,
            string expectedAreaName,
            bool expectedInterior,
            Vector3 expectedLocalPosition,
            Vector2 expectedLuminanceBand,
            Vector3 expectedKeyLightEulerDegrees,
            float expectedKeyLightIntensity,
            Color expectedKeyLightTint,
            float expectedFillIntensity,
            Color expectedFillTint,
            float expectedAmbientIntensity,
            Color expectedAmbientTint)
        {
            var profileObject = FindSceneObjectIncludingInactive(objectName);
            if (profileObject == null)
            {
                issues.Add($"Missing area lighting profile object: {objectName}");
                return;
            }

            if (profileObject.scene.path != AnemoraFastVsHouseSliceSetup.ScenePath)
            {
                issues.Add($"Area lighting profile {objectName} must live in the house slice scene.");
            }

            var parent = profileObject.transform.parent;
            if (parent == null || !string.Equals(parent.name, expectedParentName, StringComparison.Ordinal))
            {
                issues.Add($"Area lighting profile {objectName} must be parented under {expectedParentName}.");
            }

            if (parent != null)
            {
                var root = parent.root != null ? parent.root : parent;
                if (!string.Equals(root.name, expectedRootName, StringComparison.Ordinal))
                {
                    issues.Add($"Area lighting profile {objectName} must stay under root {expectedRootName}, but root was {root.name}.");
                }
            }

            var profile = profileObject.GetComponent<FastVsHd2dAreaLightingProfile>();
            if (profile == null)
            {
                issues.Add($"Area lighting profile {objectName} is missing FastVsHd2dAreaLightingProfile.");
                return;
            }

            if (profile.AreaIdForReview != expectedArea)
            {
                issues.Add($"Area lighting profile {objectName} must use area {expectedArea}, but was {profile.AreaIdForReview}.");
            }

            if (!string.Equals(profile.AreaNameForReview, expectedAreaName, StringComparison.Ordinal))
            {
                issues.Add($"Area lighting profile {objectName} must keep area name '{expectedAreaName}', but was '{profile.AreaNameForReview}'.");
            }

            if (profile.IsInteriorForReview != expectedInterior)
            {
                issues.Add($"Area lighting profile {objectName} interior flag must be {expectedInterior}.");
            }

            if (Vector3.Distance(profileObject.transform.localPosition, expectedLocalPosition) > PositionTolerance)
            {
                issues.Add($"Area lighting profile {objectName} local position must stay near {expectedLocalPosition}, but was {profileObject.transform.localPosition}.");
            }

            ValidateVector2(issues, objectName, "targetAverageLuminanceBand", profile.TargetAverageLuminanceBandForReview, expectedLuminanceBand);
            ValidateVector3(issues, objectName, "keyLightEulerDegrees", profile.KeyLightEulerDegreesForReview, expectedKeyLightEulerDegrees);
            ValidateFloat(issues, objectName, "keyLightIntensity", profile.KeyLightIntensityForReview, expectedKeyLightIntensity);
            ValidateColor(issues, objectName, "keyLightTint", profile.KeyLightTintForReview, expectedKeyLightTint);
            ValidateFloat(issues, objectName, "fillIntensity", profile.FillIntensityForReview, expectedFillIntensity);
            ValidateColor(issues, objectName, "fillTint", profile.FillTintForReview, expectedFillTint);
            ValidateFloat(issues, objectName, "ambientIntensity", profile.AmbientIntensityForReview, expectedAmbientIntensity);
            ValidateColor(issues, objectName, "ambientTint", profile.AmbientTintForReview, expectedAmbientTint);

            if (profile.TargetAverageLuminanceBandForReview.x <= 0f || profile.TargetAverageLuminanceBandForReview.x >= profile.TargetAverageLuminanceBandForReview.y)
            {
                issues.Add($"Area lighting profile {objectName} must keep a positive luminance band with min < max.");
            }

            ValidateRuntimeLightingSync(issues, objectName, director, mainLight, warmFill, profile);
        }

        private static void ValidateVector2(List<string> issues, string objectName, string fieldName, Vector2 actual, Vector2 expected)
        {
            if (Vector2.Distance(actual, expected) > FloatTolerance)
            {
                issues.Add($"Area lighting profile {objectName} must keep {fieldName} near {expected}, but was {actual}.");
            }
        }

        private static void ValidateFloat(List<string> issues, string objectName, string fieldName, float actual, float expected)
        {
            if (Mathf.Abs(actual - expected) > FloatTolerance)
            {
                issues.Add($"Area lighting profile {objectName} must keep {fieldName} near {expected:0.000}, but was {actual:0.000}.");
            }
        }

        private static void ValidateVector3(List<string> issues, string objectName, string fieldName, Vector3 actual, Vector3 expected)
        {
            if (Vector3.Distance(actual, expected) > FloatTolerance)
            {
                issues.Add($"Area lighting profile {objectName} must keep {fieldName} near {expected}, but was {actual}.");
            }
        }

        private static void ValidateColor(List<string> issues, string objectName, string fieldName, Color actual, Color expected)
        {
            if (Mathf.Abs(actual.r - expected.r) > FloatTolerance ||
                Mathf.Abs(actual.g - expected.g) > FloatTolerance ||
                Mathf.Abs(actual.b - expected.b) > FloatTolerance ||
                Mathf.Abs(actual.a - expected.a) > FloatTolerance)
            {
                issues.Add($"Area lighting profile {objectName} must keep {fieldName} near {expected}, but was {actual}.");
            }
        }

        private static void ValidateRuntimeLightingSync(
            List<string> issues,
            string objectName,
            FastVsHouseLightingDirector director,
            Light mainLight,
            Light warmFill,
            FastVsHd2dAreaLightingProfile profile)
        {
            if (director == null || mainLight == null || warmFill == null)
            {
                return;
            }

            director.ApplyAreaForReview(profile.AreaIdForReview);
            ValidateFloat(issues, objectName, "runtime mainLight.intensity", mainLight.intensity, profile.KeyLightIntensityForReview);
            ValidateColor(issues, objectName, "runtime mainLight.color", mainLight.color, profile.KeyLightTintForReview);
            ValidateFloat(issues, objectName, "runtime warmFill.intensity", warmFill.intensity, profile.FillIntensityForReview);
            ValidateColor(issues, objectName, "runtime warmFill.color", warmFill.color, profile.FillTintForReview);
            ValidateColor(issues, objectName, "runtime ambientLight", RenderSettings.ambientLight, profile.AmbientTintForReview);
            ValidateFloat(issues, objectName, "runtime ambient luminance", GetLuminance(RenderSettings.ambientLight), profile.AmbientIntensityForReview);

            var expectedRotation = Quaternion.Euler(profile.KeyLightEulerDegreesForReview);
            var angle = Quaternion.Angle(mainLight.transform.rotation, expectedRotation);
            if (angle > RotationToleranceDegrees)
            {
                issues.Add($"Area lighting profile {objectName} runtime main light rotation must match {profile.KeyLightEulerDegreesForReview}, but angle delta was {angle:0.000} degrees.");
            }
        }

        private static float GetLuminance(Color color)
        {
            return (color.r * 0.2126f) + (color.g * 0.7152f) + (color.b * 0.0722f);
        }

        private static Light GetLight(string objectName)
        {
            var target = GameObject.Find(objectName);
            return target != null ? target.GetComponent<Light>() : null;
        }

        private static GameObject FindSceneObjectIncludingInactive(string objectName)
        {
            foreach (var candidate in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (candidate == null || !string.Equals(candidate.name, objectName, StringComparison.Ordinal))
                {
                    continue;
                }

                if (!candidate.scene.IsValid() || !string.Equals(candidate.scene.path, AnemoraFastVsHouseSliceSetup.ScenePath, StringComparison.Ordinal))
                {
                    continue;
                }

                return candidate;
            }

            return null;
        }
    }
}
