using System;
using System.Collections.Generic;
using Anemora.FastVS;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dOverlayProfileFoundationAudit
    {
        private const string MaterialRoleTagName = "AnemoraFastVsHd2dRole";
        private const float VectorTolerance = 0.005f;
        private const float ColorTolerance = 0.02f;

        [MenuItem("Tools/Anemora/Verify HD2D Overlay Profiles V1")]
        public static void VerifyOverlayProfilesV1()
        {
            var issues = new List<string>();

            ValidateProfile(
                issues,
                "FastVS_PlayerContactShadow_Niro",
                FastVsHouseArea.Interior,
                FastVsHd2dOverlayKind.CharacterContactShadow,
                currentWorld: true,
                dynamicSubject: true,
                new Vector2(0.18f, 0.42f),
                new Vector2(0.66f, 0.24f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "FastVS_PlayerFootContact_Niro",
                FastVsHouseArea.Interior,
                FastVsHd2dOverlayKind.CharacterFootContact,
                currentWorld: true,
                dynamicSubject: true,
                new Vector2(0.20f, 0.48f),
                new Vector2(0.34f, 0.075f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "FastVS_PlayerDirectionalCastShadow_Niro",
                FastVsHouseArea.Interior,
                FastVsHd2dOverlayKind.CharacterDirectionalCastShadow,
                currentWorld: true,
                dynamicSubject: true,
                new Vector2(0.10f, 0.34f),
                new Vector2(0.72f, 0.18f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Current_Library_Reto_ContactShadow",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterContactShadow,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.18f, 0.42f),
                new Vector2(0.66f, 0.24f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Current_Library_Reto_FootContact",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterFootContact,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.20f, 0.48f),
                new Vector2(0.30f, 0.070f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Current_Library_Reto_DirectionalCastShadow",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterDirectionalCastShadow,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(0.60f, 0.16f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Past_Library_Aria_ContactShadow",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterContactShadow,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.18f, 0.42f),
                new Vector2(0.70f, 0.24f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Past_Library_Aria_FootContact",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterFootContact,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.20f, 0.48f),
                new Vector2(0.31f, 0.070f),
                new Color(0.20f, 0.19f, 0.18f, 0.96f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Past_Library_Aria_DirectionalCastShadow",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.CharacterDirectionalCastShadow,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(0.60f, 0.16f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Current_HouseExterior_StaticDirectionalCastShadow_HouseFacade",
                FastVsHouseArea.Exterior,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(2.04f, 0.18f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Past_HouseExterior_StaticDirectionalCastShadow_HouseFacade",
                FastVsHouseArea.Exterior,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(2.04f, 0.18f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Current_CentralPlaza_StaticDirectionalCastShadow_LibraryFacade",
                FastVsHouseArea.CentralPlaza,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(3.12f, 0.18f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Past_CentralPlaza_StaticDirectionalCastShadow_LibraryFacade",
                FastVsHouseArea.CentralPlaza,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(3.12f, 0.18f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Current_Library_StaticDirectionalCastShadow_BackShelf",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(4.98f, 0.16f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Past_Library_StaticDirectionalCastShadow_BackShelf",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.StaticDirectionalCastShadow,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.10f, 0.34f),
                new Vector2(4.98f, 0.16f),
                new Color(0.20f, 0.19f, 0.18f, 0.90f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Current_HouseExterior_SurfaceDirectionalShade_FacadeLeft",
                FastVsHouseArea.Exterior,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(1.30f, 2.10f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Past_HouseExterior_SurfaceDirectionalShade_FacadeLeft",
                FastVsHouseArea.Exterior,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(1.24f, 2.04f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Current_CentralPlaza_SurfaceDirectionalShade_LibraryFacade",
                FastVsHouseArea.CentralPlaza,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(8.96f, 2.62f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Past_CentralPlaza_SurfaceDirectionalShade_LibraryFacade",
                FastVsHouseArea.CentralPlaza,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(8.72f, 2.56f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Current_Library_SurfaceDirectionalShade_BackShelf",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: true,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(8.82f, 2.02f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                requireContactShadowMaterialRole: true);

            ValidateProfile(
                issues,
                "Past_Library_SurfaceDirectionalShade_BackShelf",
                FastVsHouseArea.Library,
                FastVsHd2dOverlayKind.SurfaceDirectionalShade,
                currentWorld: false,
                dynamicSubject: false,
                new Vector2(0.04f, 0.11f),
                new Vector2(8.66f, 1.96f),
                new Color(0.20f, 0.19f, 0.18f, 0.84f),
                requireContactShadowMaterialRole: true);

            if (issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D overlay profile audit failed:\n- " + string.Join("\n- ", issues));
            }

            Debug.Log("HD2D overlay profile audit passed.");
        }

        private static void ValidateProfile(
            List<string> issues,
            string objectName,
            FastVsHouseArea expectedArea,
            FastVsHd2dOverlayKind expectedKind,
            bool currentWorld,
            bool dynamicSubject,
            Vector2 expectedOpacityBand,
            Vector2 expectedFootprintWorldSize,
            Color expectedTint,
            bool requireContactShadowMaterialRole)
        {
            var sceneObject = FindSceneObjectIncludingInactive(objectName);
            if (sceneObject == null)
            {
                issues.Add($"Missing HD2D overlay profile object: {objectName}");
                return;
            }

            if (sceneObject.scene.path != AnemoraFastVsHouseSliceSetup.ScenePath)
            {
                issues.Add($"HD2D overlay profile {objectName} must live in the house slice scene.");
            }

            var profile = sceneObject.GetComponent<FastVsHd2dOverlayProfile>();
            if (profile == null)
            {
                issues.Add($"HD2D overlay profile {objectName} is missing FastVsHd2dOverlayProfile.");
                return;
            }

            if (!string.Equals(profile.OverlayIdForReview, objectName, StringComparison.Ordinal))
            {
                issues.Add($"HD2D overlay profile {objectName} must keep overlayId '{objectName}', but was '{profile.OverlayIdForReview}'.");
            }

            if (profile.AreaIdForReview != expectedArea)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep area {expectedArea}, but was {profile.AreaIdForReview}.");
            }

            if (profile.OverlayKindForReview != expectedKind)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep overlay kind {expectedKind}, but was {profile.OverlayKindForReview}.");
            }

            if (profile.IsCurrentWorldForReview != currentWorld)
            {
                issues.Add($"HD2D overlay profile {objectName} currentWorld must be {currentWorld}.");
            }

            if (profile.IsDynamicSubjectForReview != dynamicSubject)
            {
                issues.Add($"HD2D overlay profile {objectName} dynamicSubject must be {dynamicSubject}.");
            }

            ValidateVector2(issues, objectName, "opacityBand", profile.OpacityBandForReview, expectedOpacityBand);
            ValidateVector2(issues, objectName, "footprintWorldSize", profile.FootprintWorldSizeForReview, expectedFootprintWorldSize);
            ValidateColor(issues, objectName, "intendedTint", profile.IntendedTintForReview, expectedTint);

            var renderer = sceneObject.GetComponent<MeshRenderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep a MeshRenderer with a material.");
                return;
            }

            if (!renderer.enabled)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep its MeshRenderer enabled.");
            }

            if (renderer.shadowCastingMode != ShadowCastingMode.Off || renderer.receiveShadows)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep shadow casting disabled.");
            }

            if (requireContactShadowMaterialRole)
            {
                var materialRole = renderer.sharedMaterial.GetTag(MaterialRoleTagName, false, string.Empty);
                if (!string.Equals(materialRole, AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow.ToString(), StringComparison.Ordinal))
                {
                    issues.Add($"HD2D overlay profile {objectName} must keep material tag {MaterialRoleTagName}=ContactShadow, but was '{materialRole}'.");
                }
            }
        }

        private static void ValidateVector2(List<string> issues, string objectName, string fieldName, Vector2 actual, Vector2 expected)
        {
            if (Vector2.Distance(actual, expected) > VectorTolerance)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep {fieldName} near {expected}, but was {actual}.");
            }
        }

        private static void ValidateColor(List<string> issues, string objectName, string fieldName, Color actual, Color expected)
        {
            if (Mathf.Abs(actual.r - expected.r) > ColorTolerance ||
                Mathf.Abs(actual.g - expected.g) > ColorTolerance ||
                Mathf.Abs(actual.b - expected.b) > ColorTolerance ||
                Mathf.Abs(actual.a - expected.a) > ColorTolerance)
            {
                issues.Add($"HD2D overlay profile {objectName} must keep {fieldName} near {expected}, but was {actual}.");
            }
        }

        private static GameObject FindSceneObjectIncludingInactive(string objectName)
        {
            var active = GameObject.Find(objectName);
            if (active != null)
            {
                return active;
            }

            foreach (var gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (gameObject == null ||
                    !string.Equals(gameObject.name, objectName, StringComparison.Ordinal) ||
                    !gameObject.scene.IsValid() ||
                    gameObject.scene.path != AnemoraFastVsHouseSliceSetup.ScenePath)
                {
                    continue;
                }

                return gameObject;
            }

            return null;
        }
    }
}
