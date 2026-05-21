using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dSpriteCardLightingAudit
    {
        private const string MaterialDirectory = "Assets/Art/Materials/FastVS/HouseSlice";
        private const string TextureDirectory = "Assets/Art/Textures/FastVS/HouseSlice";
        private const string SpriteCardRampShaderName = "Anemora/FastVS/SpriteCardRampUnlit";
        private const float TransparentThreshold = 0.01f;
        private const float OpaqueThreshold = 0.12f;
        private const float SpriteCardRampStrengthMin = 0.14f;
        private const float SpriteCardRampStrengthMax = 0.22f;
        private const int SpriteCardRenderQueueMin = 3000;
        private const int SpriteCardRenderQueueMax = 3015;

        [MenuItem("Tools/Anemora/Verify HD2D Sprite Card Lighting V1")]
        public static void VerifySpriteCardLightingV1()
        {
            var issues = new List<string>();

            ValidateSpriteTexture(
                issues,
                "niro_front_sprite",
                "Assets/Art/Characters/FastVS/Niro/hero_niro_idle_front_v45_review_only.png");
            ValidateSpriteTexture(
                issues,
                "niro_walk_front_sprite",
                "Assets/Art/Characters/FastVS/Niro/hero_niro_walk_front_v45_review_only.png");
            ValidateSpriteTexture(
                issues,
                "reto_v02_writing_loop_sprite",
                "Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png");
            ValidateSpriteTexture(
                issues,
                "aria_v46_normal_loop_breath_sprite",
                "Assets/Art/Characters/FastVS/Aria/resident_a_aria_normal_loop_breath_v01_4f_64x96_review_only.png");
            ValidateSpriteCardMaterial(issues, "niro_front_sprite");
            ValidateSpriteCardMaterial(issues, "niro_back_sprite");
            ValidateSpriteCardMaterial(issues, "niro_left_sprite");
            ValidateSpriteCardMaterial(issues, "niro_right_sprite");
            ValidateSpriteCardMaterial(issues, "niro_walk_front_sprite");
            ValidateSpriteCardMaterial(issues, "niro_walk_back_sprite");
            ValidateSpriteCardMaterial(issues, "niro_walk_left_sprite");
            ValidateSpriteCardMaterial(issues, "niro_walk_right_sprite");
            ValidateSpriteCardMaterial(issues, "niro_past_front_sprite");
            ValidateSpriteCardMaterial(issues, "niro_past_back_sprite");
            ValidateSpriteCardMaterial(issues, "niro_past_left_sprite");
            ValidateSpriteCardMaterial(issues, "niro_past_right_sprite");
            ValidateSpriteCardMaterial(issues, "niro_past_walk_front_sprite");
            ValidateSpriteCardMaterial(issues, "niro_past_walk_back_sprite");
            ValidateSpriteCardMaterial(issues, "niro_past_walk_left_sprite");
            ValidateSpriteCardMaterial(issues, "niro_past_walk_right_sprite");
            ValidateSpriteCardMaterial(issues, "reto_v02_writing_loop_sprite");
            ValidateSpriteCardMaterial(issues, "reto_v02_lower_arms_sprite");
            ValidateSpriteCardMaterial(issues, "reto_v02_talk_loop_sprite");
            ValidateSpriteCardMaterial(issues, "reto_v02_raise_arms_sprite");
            ValidateSpriteCardMaterial(issues, "aria_v46_normal_loop_breath_sprite");
            ValidateSpriteCardMaterial(issues, "current_house_exterior_tree3_sprite_cc0");
            ValidateSpriteCardMaterial(issues, "past_house_exterior_tree3_sprite_cc0");
            ValidateSpriteCardMaterial(issues, "current_house_exterior_north_hedge_sprite_a_cc0");
            ValidateSpriteCardMaterial(issues, "current_house_exterior_north_hedge_sprite_b_cc0");
            ValidateSpriteCardMaterial(issues, "past_house_exterior_north_hedge_sprite_a_cc0");
            ValidateSpriteCardMaterial(issues, "past_house_exterior_north_hedge_sprite_b_cc0");
            ValidateSpriteCardMaterial(issues, "current_central_plaza_north_tree_line_sprite_a_cc0");
            ValidateSpriteCardMaterial(issues, "current_central_plaza_north_tree_line_sprite_b_cc0");
            ValidateSpriteCardMaterial(issues, "past_central_plaza_north_tree_line_sprite_a_cc0");
            ValidateSpriteCardMaterial(issues, "past_central_plaza_north_tree_line_sprite_b_cc0");

            if (issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D sprite card lighting audit failed:\n- " + string.Join("\n- ", issues));
            }

            Debug.Log("HD2D sprite card lighting audit passed.");
        }

        private static void ValidateSpriteTexture(List<string> issues, string spriteId, string sourcePath)
        {
            var shadedPath = $"{TextureDirectory}/FastVS_House_{spriteId}_shaded.asset";
            var shaded = AssetDatabase.LoadAssetAtPath<Texture2D>(shadedPath);
            if (shaded == null)
            {
                issues.Add($"Missing shaded sprite texture: {shadedPath}");
                return;
            }

            if (shaded.width <= 0 || shaded.height <= 0)
            {
                issues.Add($"Shaded sprite texture has invalid dimensions: {shadedPath}");
                return;
            }

            if (!TryReadPixel(shaded, 0, 0, out _))
            {
                issues.Add($"Shaded sprite texture is not readable: {shadedPath}");
                return;
            }

            var source = AssetDatabase.LoadAssetAtPath<Texture2D>(sourcePath);
            if (source == null)
            {
                issues.Add($"Missing source sprite texture: {sourcePath}");
                return;
            }

            if (shaded.width != source.width || shaded.height != source.height)
            {
                issues.Add($"Shaded sprite texture dimensions must match source: {shadedPath} ({shaded.width}x{shaded.height}) vs {sourcePath} ({source.width}x{source.height}).");
            }

            var sourceStats = CollectStats(source);
            var stats = CollectStats(shaded);
            if (stats.opaqueCount == 0)
            {
                issues.Add($"Shaded sprite texture has no opaque pixels: {shadedPath}");
                return;
            }

            if (stats.luminanceMax - stats.luminanceMin < 0.05f)
            {
                issues.Add($"Shaded sprite texture luminance range is too flat: {shadedPath}");
            }

            if (stats.averageLuminance < 0.08f || stats.averageLuminance > 0.88f)
            {
                issues.Add($"Shaded sprite texture average luminance is out of band: {shadedPath} ({stats.averageLuminance:0.000}).");
            }

            if (sourceStats.opaqueCount > 0)
            {
                var lowerRelativeBound = sourceStats.averageLuminance * 0.58f;
                var upperRelativeBound = Mathf.Max(0.28f, sourceStats.averageLuminance * 1.55f);
                if (stats.averageLuminance < lowerRelativeBound)
                {
                    issues.Add($"Shaded sprite texture became too dark relative to source: {shadedPath} ({stats.averageLuminance:0.000} vs source {sourceStats.averageLuminance:0.000}).");
                }

                if (stats.averageLuminance > upperRelativeBound)
                {
                    issues.Add($"Shaded sprite texture became too bright relative to source: {shadedPath} ({stats.averageLuminance:0.000} vs source {sourceStats.averageLuminance:0.000}).");
                }
            }

            ValidateTransparentCornersIfApplicable(issues, source, shaded, shadedPath);
            ValidateDirectionalBias(issues, shaded, shadedPath);
        }

        private static void ValidateTransparentCornersIfApplicable(List<string> issues, Texture2D source, Texture2D shaded, string shadedPath)
        {
            var corners = new[]
            {
                new Vector2Int(0, 0),
                new Vector2Int(0, source.height - 1),
                new Vector2Int(source.width - 1, 0),
                new Vector2Int(source.width - 1, source.height - 1)
            };

            for (var i = 0; i < corners.Length; i++)
            {
                var corner = corners[i];
                if (!TryReadPixel(source, corner.x, corner.y, out var sourcePixel))
                {
                    continue;
                }

                if (sourcePixel.a > TransparentThreshold)
                {
                    continue;
                }

                if (!TryReadPixel(shaded, corner.x, corner.y, out var shadedPixel))
                {
                    issues.Add($"Could not sample shaded texture corner at {shadedPath} ({corner.x},{corner.y}).");
                    continue;
                }

                if (shadedPixel.a > TransparentThreshold)
                {
                    issues.Add($"Transparent source corner became visible in shaded texture: {shadedPath} ({corner.x},{corner.y}).");
                }
            }
        }

        private static void ValidateDirectionalBias(List<string> issues, Texture2D shaded, string shadedPath)
        {
            var frameWidth = GetSpriteFrameWidth(shaded.width, shaded.height);
            if (!TryFindOpaqueSample(shaded, frameWidth, true, out var topLeft) || !TryFindOpaqueSample(shaded, frameWidth, false, out var lowerRight))
            {
                return;
            }

            var topLeftLum = GetLuminance(topLeft);
            var lowerRightLum = GetLuminance(lowerRight);
            if (lowerRightLum - topLeftLum > 0.10f)
            {
                issues.Add($"Directional shading is inverted or too weak in {shadedPath}: lower-right sample ({lowerRightLum:0.000}) is much brighter than upper-left ({topLeftLum:0.000}).");
            }
        }

        private static void ValidateSpriteCardMaterial(List<string> issues, string materialId)
        {
            var path = $"{MaterialDirectory}/FastVS_House_{materialId}.mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                issues.Add($"Missing sprite card material: {path}");
                return;
            }

            if (material.shader == null || !string.Equals(material.shader.name, SpriteCardRampShaderName, StringComparison.Ordinal))
            {
                issues.Add($"Sprite card material must use {SpriteCardRampShaderName}: {path}");
            }

            if (material.renderQueue < SpriteCardRenderQueueMin || material.renderQueue > SpriteCardRenderQueueMax)
            {
                issues.Add($"Sprite card material renderQueue is out of range: {path} ({material.renderQueue}).");
            }

            var texture = material.GetTexture("_BaseMap") as Texture2D ?? material.GetTexture("_MainTex") as Texture2D;
            if (texture == null)
            {
                issues.Add($"Sprite card material is missing a texture assignment: {path}");
            }

            if (!material.HasProperty("_RampStrength"))
            {
                issues.Add($"Sprite card material is missing _RampStrength: {path}");
            }
            else
            {
                var rampStrength = material.GetFloat("_RampStrength");
                if (rampStrength < SpriteCardRampStrengthMin || rampStrength > SpriteCardRampStrengthMax)
                {
                    issues.Add($"Sprite card material ramp strength is out of band: {path} ({rampStrength:0.000}).");
                }
            }

            ValidateSpriteCardColorBand(issues, material, path, "_TopLight", 0.98f, 1.12f, 0.96f, 1.08f, 0.90f, 1.05f);
            ValidateSpriteCardColorBand(issues, material, path, "_SideShade", 0.90f, 1.00f, 0.94f, 1.03f, 0.98f, 1.08f);
            ValidateSpriteCardColorBand(issues, material, path, "_FloorShade", 0.84f, 0.94f, 0.88f, 0.97f, 0.92f, 1.00f);
        }

        private static void ValidateSpriteCardColorBand(
            List<string> issues,
            Material material,
            string path,
            string propertyName,
            float minR,
            float maxR,
            float minG,
            float maxG,
            float minB,
            float maxB)
        {
            if (!material.HasProperty(propertyName))
            {
                issues.Add($"Sprite card material is missing {propertyName}: {path}");
                return;
            }

            var color = material.GetColor(propertyName);
            if (color.r < minR || color.r > maxR ||
                color.g < minG || color.g > maxG ||
                color.b < minB || color.b > maxB)
            {
                issues.Add($"Sprite card material {propertyName} is out of range: {path} ({color.r:0.000}, {color.g:0.000}, {color.b:0.000}).");
            }
        }

        private static bool TryFindOpaqueSample(Texture2D texture, int frameWidth, bool topLeftSearch, out Color pixel)
        {
            var height = texture.height;
            var xStart = topLeftSearch ? 0 : Mathf.Max(0, frameWidth - Mathf.Max(8, frameWidth / 3));
            var xEnd = topLeftSearch ? Mathf.Max(1, Mathf.Min(frameWidth, Mathf.Max(8, frameWidth / 3))) : frameWidth;
            var yStart = topLeftSearch ? Mathf.Max(0, height - Mathf.Max(8, height / 3)) : 0;
            var yEnd = topLeftSearch ? height : Mathf.Max(1, Mathf.Min(height, Mathf.Max(8, height / 3)));

            for (var y = yStart; y < yEnd; y++)
            {
                for (var x = xStart; x < xEnd; x++)
                {
                    if (!TryReadPixel(texture, x, y, out pixel))
                    {
                        continue;
                    }

                    if (pixel.a > OpaqueThreshold)
                    {
                        return true;
                    }
                }
            }

            pixel = default;
            return false;
        }

        private static int GetSpriteFrameWidth(int width, int height)
        {
            if (height == 96 && width > 64 && width % 64 == 0)
            {
                return 64;
            }

            return Mathf.Max(1, width);
        }

        private static TextureStats CollectStats(Texture2D texture)
        {
            var stats = new TextureStats
            {
                luminanceMin = float.PositiveInfinity,
                luminanceMax = float.NegativeInfinity
            };

            for (var y = 0; y < texture.height; y++)
            {
                for (var x = 0; x < texture.width; x++)
                {
                    if (!TryReadPixel(texture, x, y, out var pixel))
                    {
                        continue;
                    }

                    if (pixel.a <= TransparentThreshold)
                    {
                        continue;
                    }

                    var luminance = GetLuminance(pixel);
                    stats.opaqueCount++;
                    stats.averageLuminance += luminance;
                    stats.luminanceMin = Mathf.Min(stats.luminanceMin, luminance);
                    stats.luminanceMax = Mathf.Max(stats.luminanceMax, luminance);
                }
            }

            if (stats.opaqueCount > 0)
            {
                stats.averageLuminance /= stats.opaqueCount;
            }

            if (float.IsPositiveInfinity(stats.luminanceMin))
            {
                stats.luminanceMin = 0f;
            }

            if (float.IsNegativeInfinity(stats.luminanceMax))
            {
                stats.luminanceMax = 0f;
            }

            return stats;
        }

        private static bool TryReadPixel(Texture2D texture, int x, int y, out Color pixel)
        {
            try
            {
                pixel = texture.GetPixel(x, y);
                return true;
            }
            catch (Exception)
            {
                pixel = default;
                return false;
            }
        }

        private static float GetLuminance(Color pixel)
        {
            return (pixel.r * 0.2126f) + (pixel.g * 0.7152f) + (pixel.b * 0.0722f);
        }

        private struct TextureStats
        {
            public int opaqueCount;
            public float averageLuminance;
            public float luminanceMin;
            public float luminanceMax;
        }
    }
}
