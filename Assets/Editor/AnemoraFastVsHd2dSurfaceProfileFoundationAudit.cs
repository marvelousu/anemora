using System;
using System.Collections.Generic;
using Anemora.FastVS;
using UnityEditor;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dSurfaceProfileFoundationAudit
    {
        private const string MaterialRoleTagName = "AnemoraFastVsHd2dRole";
        private static readonly string[] AllowedMaterialRoles =
        {
            AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.SurfaceLit.ToString(),
            AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.PaperCard.ToString(),
            AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.PortalWindow.ToString()
        };

        private static readonly RequiredSurface[] RequiredSurfaces =
        {
            new RequiredSurface("Current.HouseInterior.Floor.Main", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Floor, true),
            new RequiredSurface("Current.HouseInterior.Wall.Back", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Wall, true),
            new RequiredSurface("Current.HouseInterior.Wall.Left", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Wall, true),
            new RequiredSurface("Current.HouseInterior.Wall.Right", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Wall, true),
            new RequiredSurface("Current.HouseInterior.Bed.Main", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Furniture, true),
            new RequiredSurface("Current.HouseInterior.Table.Main", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Furniture, true),
            new RequiredSurface("Past.HouseInterior.Floor.Main", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Floor, false),
            new RequiredSurface("Past.HouseInterior.Wall.Back", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Wall, false),
            new RequiredSurface("Past.HouseInterior.Wall.Left", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Wall, false),
            new RequiredSurface("Past.HouseInterior.Wall.Right", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Wall, false),
            new RequiredSurface("Past.HouseInterior.Bed.Main", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Furniture, false),
            new RequiredSurface("Past.HouseInterior.Table.Main", FastVsHouseArea.Interior, FastVsHd2dSurfaceKind.Furniture, false),
            new RequiredSurface("Current.HouseExterior.Ground.Yard", FastVsHouseArea.Exterior, FastVsHd2dSurfaceKind.Ground, true),
            new RequiredSurface("Current.HouseExterior.Path.ToInterior", FastVsHouseArea.Exterior, FastVsHd2dSurfaceKind.Road, true),
            new RequiredSurface("Past.HouseExterior.Ground.Yard", FastVsHouseArea.Exterior, FastVsHd2dSurfaceKind.Ground, false),
            new RequiredSurface("Past.HouseExterior.Path.ToInterior", FastVsHouseArea.Exterior, FastVsHd2dSurfaceKind.Road, false),
            new RequiredSurface("Current.CentralPlaza.Ground.Paving", FastVsHouseArea.CentralPlaza, FastVsHd2dSurfaceKind.Road, true),
            new RequiredSurface("Past.CentralPlaza.Ground.Paving", FastVsHouseArea.CentralPlaza, FastVsHd2dSurfaceKind.Road, false),
            new RequiredSurface("Current.CentralPlaza.Wall.LibraryFacade", FastVsHouseArea.CentralPlaza, FastVsHd2dSurfaceKind.Wall, true),
            new RequiredSurface("Past.CentralPlaza.Wall.LibraryFacade", FastVsHouseArea.CentralPlaza, FastVsHd2dSurfaceKind.Wall, false),
            new RequiredSurface("Current.Library.Floor.Main", FastVsHouseArea.Library, FastVsHd2dSurfaceKind.Floor, true),
            new RequiredSurface("Current.Library.Wall.Back", FastVsHouseArea.Library, FastVsHd2dSurfaceKind.Wall, true),
            new RequiredSurface("Current.Library.Desk.Main", FastVsHouseArea.Library, FastVsHd2dSurfaceKind.Furniture, true),
            new RequiredSurface("Current.Library.Bookshelf.Back", FastVsHouseArea.Library, FastVsHd2dSurfaceKind.Bookshelf, true),
            new RequiredSurface("Past.Library.Floor.Main", FastVsHouseArea.Library, FastVsHd2dSurfaceKind.Floor, false),
            new RequiredSurface("Past.Library.Wall.Back", FastVsHouseArea.Library, FastVsHd2dSurfaceKind.Wall, false),
            new RequiredSurface("Past.Library.Desk.Main", FastVsHouseArea.Library, FastVsHd2dSurfaceKind.Furniture, false),
            new RequiredSurface("Past.Library.Bookshelf.Back", FastVsHouseArea.Library, FastVsHd2dSurfaceKind.Bookshelf, false)
        };

        [MenuItem("Tools/Anemora/Verify HD2D Surface Profiles V1")]
        public static void VerifySurfaceProfilesV1()
        {
            var issues = new List<string>();
            var surfaceProfiles = Resources.FindObjectsOfTypeAll<FastVsHd2dSurfaceProfile>();
            var seenSurfaceIds = new HashSet<string>(StringComparer.Ordinal);
            var requiredById = BuildRequiredSurfaceLookup();

            foreach (var profile in surfaceProfiles)
            {
                if (profile == null || profile.gameObject == null)
                {
                    continue;
                }

                if (!profile.gameObject.scene.IsValid() ||
                    !string.Equals(profile.gameObject.scene.path, AnemoraFastVsHouseSliceSetup.ScenePath, StringComparison.Ordinal))
                {
                    continue;
                }

                ValidateProfile(issues, profile);

                var surfaceId = profile.SurfaceIdForReview;
                if (string.IsNullOrWhiteSpace(surfaceId))
                {
                    continue;
                }

                if (!seenSurfaceIds.Add(surfaceId))
                {
                    issues.Add($"Duplicate HD2D surface profile surfaceId detected: {surfaceId}.");
                }

                if (requiredById.TryGetValue(surfaceId, out var requiredSurface))
                {
                    ValidateRequiredProfile(issues, profile, requiredSurface);
                }
            }

            foreach (var requiredSurface in RequiredSurfaces)
            {
                if (!seenSurfaceIds.Contains(requiredSurface.SurfaceId))
                {
                    issues.Add($"Missing HD2D surface profile surfaceId: {requiredSurface.SurfaceId}.");
                }
            }

            if (issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D surface profile audit failed:\n- " + string.Join("\n- ", issues));
            }

            Debug.Log("HD2D surface profile audit passed");
        }

        private static void ValidateProfile(List<string> issues, FastVsHd2dSurfaceProfile profile)
        {
            var gameObject = profile.gameObject;
            var surfaceId = profile.SurfaceIdForReview;
            if (string.IsNullOrWhiteSpace(surfaceId))
            {
                issues.Add($"Surface profile on {gameObject.name} must keep a non-empty surfaceId.");
            }

            ValidateBand(
                issues,
                gameObject.name,
                "targetLuminanceBand",
                profile.TargetLuminanceBandForReview,
                0.05f,
                0.95f);

            ValidateBand(
                issues,
                gameObject.name,
                "targetContrastBand",
                profile.TargetContrastBandForReview,
                0.00f,
                0.60f);

            if (profile.TextureDensityHintForReview.x <= 0f || profile.TextureDensityHintForReview.y <= 0f)
            {
                issues.Add($"Surface profile {gameObject.name} must keep a positive textureDensityHint.");
            }

            if (string.IsNullOrWhiteSpace(profile.IntendedMaterialTokenForReview))
            {
                issues.Add($"Surface profile {gameObject.name} must keep a non-empty intendedMaterialToken.");
            }

            var renderer = gameObject.GetComponent<MeshRenderer>();
            if (renderer == null || renderer.sharedMaterial == null)
            {
                issues.Add($"Surface profile {gameObject.name} must keep a MeshRenderer with a material.");
                return;
            }

            var material = renderer.sharedMaterial;
            var materialName = material.name ?? string.Empty;
            var token = profile.IntendedMaterialTokenForReview ?? string.Empty;
            var tokenMatch = !string.IsNullOrWhiteSpace(token) &&
                             materialName.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0;
            var role = material.GetTag(MaterialRoleTagName, false, string.Empty);
            var roleAllowed = Array.IndexOf(AllowedMaterialRoles, role) >= 0;
            if (!tokenMatch && !roleAllowed)
            {
                issues.Add($"Surface profile {gameObject.name} must keep material token '{token}' in the material name or use an allowed {MaterialRoleTagName} tag, but was '{materialName}' / '{role}'.");
            }
        }

        private static void ValidateRequiredProfile(List<string> issues, FastVsHd2dSurfaceProfile profile, RequiredSurface requiredSurface)
        {
            if (profile.AreaIdForReview != requiredSurface.AreaId)
            {
                issues.Add($"Surface profile {requiredSurface.SurfaceId} must keep area {requiredSurface.AreaId}, but was {profile.AreaIdForReview}.");
            }

            if (profile.SurfaceKindForReview != requiredSurface.SurfaceKind)
            {
                issues.Add($"Surface profile {requiredSurface.SurfaceId} must keep kind {requiredSurface.SurfaceKind}, but was {profile.SurfaceKindForReview}.");
            }

            if (profile.IsCurrentWorldForReview != requiredSurface.CurrentWorld)
            {
                issues.Add($"Surface profile {requiredSurface.SurfaceId} currentWorld must be {requiredSurface.CurrentWorld}.");
            }
        }

        private static void ValidateBand(List<string> issues, string objectName, string fieldName, Vector2 band, float minInclusive, float maxInclusive)
        {
            if (band.x >= band.y)
            {
                issues.Add($"Surface profile {objectName} must keep {fieldName}.x < {fieldName}.y, but was {band}.");
            }

            if (band.x < minInclusive || band.x > maxInclusive || band.y < minInclusive || band.y > maxInclusive)
            {
                issues.Add($"Surface profile {objectName} must keep {fieldName} within {minInclusive:0.00}-{maxInclusive:0.00}, but was {band}.");
            }
        }

        private static Dictionary<string, RequiredSurface> BuildRequiredSurfaceLookup()
        {
            var result = new Dictionary<string, RequiredSurface>(StringComparer.Ordinal);
            foreach (var requiredSurface in RequiredSurfaces)
            {
                result.Add(requiredSurface.SurfaceId, requiredSurface);
            }

            return result;
        }

        private readonly struct RequiredSurface
        {
            public RequiredSurface(string surfaceId, FastVsHouseArea areaId, FastVsHd2dSurfaceKind surfaceKind, bool currentWorld)
            {
                SurfaceId = surfaceId;
                AreaId = areaId;
                SurfaceKind = surfaceKind;
                CurrentWorld = currentWorld;
            }

            public string SurfaceId { get; }
            public FastVsHouseArea AreaId { get; }
            public FastVsHd2dSurfaceKind SurfaceKind { get; }
            public bool CurrentWorld { get; }
        }
    }
}
