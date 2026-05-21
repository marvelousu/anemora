using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dMaterialRoleFoundationAudit
    {
        private const string MaterialDirectory = "Assets/Art/Materials/FastVS/HouseSlice";
        private const string RoleTagName = "AnemoraFastVsHd2dRole";
        private const string SpriteCardRampShaderName = "Anemora/FastVS/SpriteCardRampUnlit";
        private const string SurfaceRampLitShaderName = "Anemora/FastVS/SurfaceRampLit";

        [MenuItem("Tools/Anemora/Verify HD2D Material Roles V1")]
        public static void VerifyMaterialRolesV1()
        {
            var issues = new List<string>();

            ValidateSurfaceMaterials(issues);
            ValidateSurfaceRampMaterials(issues);
            ValidatePaperCardMaterials(issues);
            ValidateSpriteCardMaterials(issues);
            ValidateOverlayGlowMaterials(issues);
            ValidateContactShadowMaterials(issues);
            ValidatePortalFrameMaterials(issues);
            ValidatePortalWindowMaterials(issues);

            if (issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D material role audit failed:\n- " + string.Join("\n- ", issues));
            }

            Debug.Log("HD2D material role audit passed.");
        }

        private static void ValidateSurfaceMaterials(List<string> issues)
        {
            foreach (var materialId in new[]
            {
                "doorway_dark",
                "current_ground",
                "current_grass",
                "current_path",
                "current_interior_floor",
                "current_interior_wall",
                "current_exterior_wall",
                "current_roof",
                "current_furniture",
                "current_fence",
                "current_house_door_detail",
                "current_library_door_detail",
                "current_stone",
                "current_bed",
                "current_leaf",
                "past_grass",
                "past_path",
                "past_wood_floor",
                "past_interior_wall",
                "past_exterior_wall",
                "past_roof",
                "past_furniture",
                "past_fence",
                "past_house_door_detail",
                "past_library_door_detail",
                "past_stone",
                "past_bed",
                "leaf",
                "pillow",
                "dust",
                "current_rubble_detail",
                "book",
                "water",
                "rope",
                "flower_red",
                "flower_yellow",
                "flower_blue",
                "laundry_bright",
                "laundry_accent",
                "sign_paint"
            })
            {
                ValidateMaterialAsset(issues, materialId, AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.SurfaceLit, requireOpaque: true);
            }
        }

        private static void ValidateSurfaceRampMaterials(List<string> issues)
        {
            foreach (var materialId in new[]
            {
                "current_exterior_wall",
                "current_ground",
                "current_path",
                "current_roof",
                "past_wood_floor",
                "past_interior_wall",
                "past_exterior_wall",
                "past_furniture",
                "book"
            })
            {
                var path = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
                ValidateMaterialAsset(issues, materialId, AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.SurfaceLit, requireOpaque: true);

                var material = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (material == null)
                {
                    continue;
                }

                if (material.shader == null || !string.Equals(material.shader.name, SurfaceRampLitShaderName, StringComparison.Ordinal))
                {
                    issues.Add($"Material {path} must use shader {SurfaceRampLitShaderName}.");
                }

                if (material.renderQueue >= 2990)
                {
                    issues.Add($"Material {path} must keep an opaque renderQueue, but was {material.renderQueue}.");
                }

                var renderType = material.GetTag("RenderType", false, string.Empty);
                if (!string.Equals(renderType, "Opaque", StringComparison.Ordinal))
                {
                    issues.Add($"Material {path} must keep RenderType Opaque, but was '{renderType}'.");
                }

                if (!material.HasProperty("_SurfaceRampStrength"))
                {
                    issues.Add($"Material {path} must keep _SurfaceRampStrength.");
                    continue;
                }

                var surfaceRampStrength = material.GetFloat("_SurfaceRampStrength");
                if (surfaceRampStrength < 0.10f || surfaceRampStrength > 0.30f)
                {
                    issues.Add($"Material {path} must keep _SurfaceRampStrength in the 0.10-0.30 range, but was {surfaceRampStrength:0.000}.");
                }

                ValidateOptionalFloatBand(issues, material, path, "_DirectionalLightStrength", 0.04f, 0.24f);
                ValidateOptionalFloatBand(issues, material, path, "_ShadowReceiveStrength", 0.05f, 0.30f);
            }
        }

        private static void ValidateOptionalFloatBand(List<string> issues, Material material, string path, string propertyName, float min, float max)
        {
            if (!material.HasProperty(propertyName))
            {
                issues.Add($"Material {path} must keep {propertyName}.");
                return;
            }

            var value = material.GetFloat(propertyName);
            if (value < min || value > max)
            {
                issues.Add($"Material {path} must keep {propertyName} in the {min:0.00}-{max:0.00} range, but was {value:0.000}.");
            }
        }

        private static void ValidatePaperCardMaterials(List<string> issues)
        {
            foreach (var materialId in new[]
            {
                "niro_body",
                "niro_past_body",
                "niro_accent",
                "memory_body",
                "memory_accent",
                "card_face",
                "label"
            })
            {
                ValidateMaterialAsset(issues, materialId, AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.PaperCard, requireUnlitShader: true, requireOpaque: true);
            }
        }

        private static void ValidateSpriteCardMaterials(List<string> issues)
        {
            foreach (var materialId in new[]
            {
                "niro_left_sprite",
                "niro_right_sprite",
                "niro_back_sprite",
                "niro_front_sprite",
                "niro_walk_front_sprite",
                "niro_walk_back_sprite",
                "niro_walk_left_sprite",
                "niro_walk_right_sprite",
                "niro_past_front_sprite",
                "niro_past_back_sprite",
                "niro_past_left_sprite",
                "niro_past_right_sprite",
                "niro_past_walk_front_sprite",
                "niro_past_walk_back_sprite",
                "niro_past_walk_left_sprite",
                "niro_past_walk_right_sprite",
                "reto_v02_writing_loop_sprite",
                "reto_v02_lower_arms_sprite",
                "reto_v02_talk_loop_sprite",
                "reto_v02_raise_arms_sprite",
                "aria_v46_normal_loop_breath_sprite",
                "current_house_exterior_tree3_sprite_cc0",
                "past_house_exterior_tree3_sprite_cc0",
                "current_house_exterior_north_hedge_sprite_a_cc0",
                "current_house_exterior_north_hedge_sprite_b_cc0",
                "past_house_exterior_north_hedge_sprite_a_cc0",
                "past_house_exterior_north_hedge_sprite_b_cc0",
                "current_central_plaza_north_tree_line_sprite_a_cc0",
                "current_central_plaza_north_tree_line_sprite_b_cc0",
                "past_central_plaza_north_tree_line_sprite_a_cc0",
                "past_central_plaza_north_tree_line_sprite_b_cc0"
            })
            {
                ValidateMaterialAsset(issues, materialId, AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.SpriteCard, requireUnlitShader: true, requireTransparent: true, minRenderQueue: 3000, maxRenderQueue: 3015);
            }
        }

        private static void ValidateOverlayGlowMaterials(List<string> issues)
        {
            foreach (var materialId in new[]
            {
                "lamp",
                "timewindow_cue_yellow_light",
                "timewindow_marker_yellow",
                "hd2d_warm_light_pool",
                "hd2d_character_ground_bounce",
                "timewriter_pocket_yellow_glow",
                "hd2d_atmosphere_particle"
            })
            {
                ValidateMaterialAsset(issues, materialId, AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.OverlayGlow, requireUnlitShader: true);
            }
        }

        private static void ValidateContactShadowMaterials(List<string> issues)
        {
            foreach (var materialId in new[]
            {
                "shadow",
                "niro_contact_shadow",
                "reto_contact_shadow",
                "aria_contact_shadow",
                "character_directional_cast_shadow",
                "static_directional_cast_shadow",
                "surface_directional_shade_overlay",
                "hd2d_depth_shadow"
            })
            {
                var requireTransparent = materialId != "shadow";
                ValidateMaterialAsset(
                    issues,
                    materialId,
                    AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.ContactShadow,
                    requireUnlitShader: true,
                    requireTransparent: requireTransparent,
                    minRenderQueue: requireTransparent ? 2988 : (int?)null,
                    maxRenderQueue: requireTransparent ? 3010 : (int?)null,
                    requireShadowAlpha: requireTransparent);
            }
        }

        private static void ValidatePortalFrameMaterials(List<string> issues)
        {
            foreach (var materialId in new[]
            {
                "current_frame",
                "past_frame",
                "preview_frame",
                "threshold"
            })
            {
                ValidateMaterialAsset(issues, materialId, AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.PortalWindow, requireOpaque: true);
            }
        }

        private static void ValidatePortalWindowMaterials(List<string> issues)
        {
            foreach (var materialId in new[]
            {
                "window_light",
                "empty_window",
                "hd2d_library_window_light"
            })
            {
                ValidateMaterialAsset(issues, materialId, AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.PortalWindow, requireUnlitShader: true, allowOpaqueOrTransparent: true);
            }

            foreach (var materialId in new[]
            {
                "house_aperture"
            })
            {
                ValidateMaterialAsset(issues, materialId, AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole.PortalWindow, allowOpaqueOrTransparent: true);
            }
        }

        private static void ValidateMaterialAsset(
            List<string> issues,
            string materialId,
            AnemoraFastVsHouseSliceSetup.FastVsHd2dMaterialRole role,
            bool requireUnlitShader = false,
            bool requireTransparent = false,
            bool requireOpaque = false,
            bool requireShadowAlpha = false,
            int? expectedRenderQueue = null,
            int? minRenderQueue = null,
            int? maxRenderQueue = null,
            bool allowOpaqueOrTransparent = false)
        {
            var path = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                issues.Add($"Missing material asset: {path}");
                return;
            }

            var roleToken = role.ToString();
            if (material.name == null ||
                material.name.IndexOf(materialId, StringComparison.OrdinalIgnoreCase) < 0)
            {
                issues.Add($"Material {path} must be named with id '{materialId}', but was '{material.name}'.");
            }

            var tag = material.GetTag(RoleTagName, false, string.Empty);
            if (!string.Equals(tag, roleToken, StringComparison.Ordinal))
            {
                issues.Add($"Material {path} must carry role tag '{roleToken}', but had '{tag}'.");
            }

            if (requireUnlitShader && !IsSupportedUnlitShader(material.shader))
            {
                issues.Add($"Material {path} must use an unlit shader for role {roleToken}.");
            }

            if (expectedRenderQueue.HasValue && material.renderQueue != expectedRenderQueue.Value)
            {
                issues.Add($"Material {path} must use renderQueue {expectedRenderQueue.Value}, but was {material.renderQueue}.");
            }

            if (minRenderQueue.HasValue && material.renderQueue < minRenderQueue.Value)
            {
                issues.Add($"Material {path} must keep renderQueue at or above {minRenderQueue.Value}, but was {material.renderQueue}.");
            }

            if (maxRenderQueue.HasValue && material.renderQueue > maxRenderQueue.Value)
            {
                issues.Add($"Material {path} must keep renderQueue at or below {maxRenderQueue.Value}, but was {material.renderQueue}.");
            }

            if (requireTransparent && material.renderQueue < 2990 && !allowOpaqueOrTransparent)
            {
                issues.Add($"Material {path} must use a transparent overlay renderQueue for role {roleToken}, but was {material.renderQueue}.");
            }

            if (requireTransparent && !allowOpaqueOrTransparent)
            {
                var renderType = material.GetTag("RenderType", false, string.Empty);
                if (!string.Equals(renderType, "Transparent", StringComparison.Ordinal))
                {
                    issues.Add($"Material {path} must keep RenderType Transparent for role {roleToken}, but was '{renderType}'.");
                }

                if (material.GetShaderPassEnabled("DepthOnly"))
                {
                    issues.Add($"Material {path} must keep DepthOnly disabled for transparent role {roleToken}.");
                }

                if (material.GetShaderPassEnabled("SHADOWCASTER"))
                {
                    issues.Add($"Material {path} must keep SHADOWCASTER disabled for transparent role {roleToken}.");
                }
            }

            if (requireOpaque && material.renderQueue >= 2990)
            {
                issues.Add($"Material {path} must stay in the opaque queue family for role {roleToken}, but was {material.renderQueue}.");
            }

            if (material.HasProperty("_Surface"))
            {
                var surface = material.GetFloat("_Surface");
                if (requireTransparent && surface != 1f)
                {
                    issues.Add($"Material {path} must keep transparent surface mode for role {roleToken}.");
                }
            }

            if (material.HasProperty("_ZWrite") && (requireTransparent || requireShadowAlpha))
            {
                if (Mathf.Abs(material.GetFloat("_ZWrite")) > 0.001f)
                {
                    issues.Add($"Material {path} must disable ZWrite for role {roleToken}.");
                }
            }

            if (requireShadowAlpha && TryGetMaterialAlpha(material, out var alpha) && alpha >= 0.98f)
            {
                issues.Add($"Material {path} must remain a low-alpha shadow overlay for role {roleToken}, but alpha was {alpha:0.000}.");
            }
        }

        private static bool TryGetMaterialAlpha(Material material, out float alpha)
        {
            if (material.HasProperty("_BaseColor"))
            {
                alpha = material.GetColor("_BaseColor").a;
                return true;
            }

            if (material.HasProperty("_Color"))
            {
                alpha = material.GetColor("_Color").a;
                return true;
            }

            alpha = 1f;
            return false;
        }

        private static bool IsSupportedUnlitShader(Shader shader)
        {
            if (shader == null)
            {
                return false;
            }

            if (string.Equals(shader.name, SpriteCardRampShaderName, StringComparison.Ordinal))
            {
                return true;
            }

            return shader.name.IndexOf("Unlit", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
