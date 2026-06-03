using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Anemora.FastVS;
using Anemora.FastVS.SunCycle;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2LightCookieGoboProfilePath = "Assets/Settings/FastVS_HD2D_P2_LightCookieGoboProfile.asset";
        private const string Hd2dAutonomousP2CanopyDappleCookieId = "hd2d_p2_canopy_dapple_cookie";
        private const string Hd2dAutonomousP2WindowGoboCookieId = "hd2d_p2_window_gobo_cookie";
        private const int Hd2dAutonomousP2CookieSize = 128;
        private const string Hd2dAutonomousP2SunCycleDriverPath = "Assets/Scripts/FastVS/SunCycle/AnemoraSunCycleDriver.cs";
        private const string Hd2dAutonomousP2LightingDirectorPath = "Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs";
        private static readonly string[] Hd2dAutonomousP2WindowProxyRendererNames =
        {
            "Current_Library_WindowLightShaft_Left",
            "Current_Library_WindowLightShaft_Right",
            "Current_Library_WindowLightPool_LeftFloor",
            "Current_Library_WindowLightPool_RightFloor"
        };

        public static void CaptureHd2dAutonomousP2Item53LightCookieGoboBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var director = UnityEngine.Object.FindFirstObjectByType<FastVsHouseLightingDirector>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var mainLight = GameObject.Find("Directional Light")?.GetComponent<Light>();
            var libraryWindow = FindSceneObjectIncludingInactive("FastVS_HD2D_LibraryWindowLight")?.GetComponent<Light>();
            if (controller == null || visibility == null || guide == null || director == null || realtimeRig == null ||
                camera == null || mainLight == null || libraryWindow == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-53 light cookie/gobo capture failed: required review components are missing.");
            }

            ValidateHd2dAutonomousP2LightCookieGobos();
            var profile = EnsureHd2dAutonomousP2LightCookieGoboProfile();
            var dappleStats = AnalyzeHd2dAutonomousP2CookieTexture(profile.directionalCanopyCookie);
            var windowStats = AnalyzeHd2dAutonomousP2CookieTexture(profile.windowGoboCookie);
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("light_cookie_gobo");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_canopy_cookie_off_flat_ground.png",
                "02_canopy_cookie_on_dappled_ground.png",
                "03_window_light_no_gobo_pool.png",
                "04_window_light_gobo_pattern_pool.png"
            };

            var mainLightState = CaptureHd2dAutonomousP2LightCookieState(mainLight);
            var libraryWindowState = CaptureHd2dAutonomousP2LightCookieState(libraryWindow);
            var windowProxyStates = CaptureHd2dAutonomousP2WindowProxyRendererStates();
            try
            {
                PrepareHd2dAutonomousP2LightCookieShot(
                    controller,
                    visibility,
                    guide,
                    director,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(-2.35f, 0.02f, 1.30f),
                    new Vector3(2.35f, 2.05f, -4.18f),
                    new Vector3(0.0f, 0.08f, 0.0f),
                    31f);
                ApplyHd2dAutonomousP2DirectionalCookie(mainLight, null, profile.directionalCookieSize);
                SaveHd2dAutonomousP2LightCookieShot(camera, outputDirectory, screenshotFiles[0]);

                PrepareHd2dAutonomousP2LightCookieShot(
                    controller,
                    visibility,
                    guide,
                    director,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(-2.35f, 0.02f, 1.30f),
                    new Vector3(2.35f, 2.05f, -4.18f),
                    new Vector3(0.0f, 0.08f, 0.0f),
                    31f);
                ApplyHd2dAutonomousP2DirectionalCookie(mainLight, profile.directionalCanopyCookie, profile.directionalCookieSize);
                SaveHd2dAutonomousP2LightCookieShot(camera, outputDirectory, screenshotFiles[1]);

                PrepareHd2dAutonomousP2LightCookieShot(
                    controller,
                    visibility,
                    guide,
                    director,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.Library,
                    LibraryVsCenter + new Vector3(1.45f, 0.04f, -0.85f),
                    new Vector3(0.58f, 2.02f, -3.82f),
                    new Vector3(0.0f, 0.32f, 0.02f),
                    32f);
                SetHd2dAutonomousP2WindowProxyRenderersEnabled(windowProxyStates, false);
                ApplyHd2dAutonomousP2WindowGobo(libraryWindow, null, profile);
                SaveHd2dAutonomousP2LightCookieShot(camera, outputDirectory, screenshotFiles[2]);

                PrepareHd2dAutonomousP2LightCookieShot(
                    controller,
                    visibility,
                    guide,
                    director,
                    realtimeRig,
                    camera,
                    FastVsHouseArea.Library,
                    LibraryVsCenter + new Vector3(1.45f, 0.04f, -0.85f),
                    new Vector3(0.58f, 2.02f, -3.82f),
                    new Vector3(0.0f, 0.32f, 0.02f),
                    32f);
                ApplyHd2dAutonomousP2WindowGobo(libraryWindow, profile.windowGoboCookie, profile);
                SaveHd2dAutonomousP2LightCookieShot(camera, outputDirectory, screenshotFiles[3]);

                var dappleDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                    Path.Combine(outputDirectory, screenshotFiles[0]),
                    Path.Combine(outputDirectory, screenshotFiles[1]),
                    4);
                var windowDiff = MeasureHd2dAutonomousP1DepthPrimingDiff(
                    Path.Combine(outputDirectory, screenshotFiles[2]),
                    Path.Combine(outputDirectory, screenshotFiles[3]),
                    4);
                WriteHd2dAutonomousP2LightCookieGoboReviewReport(outputDirectory, screenshotFiles, profile, dappleStats, windowStats, dappleDiff, windowDiff, windowProxyStates.Count);
            }
            finally
            {
                RestoreHd2dAutonomousP2WindowProxyRendererStates(windowProxyStates);
                RestoreHd2dAutonomousP2LightCookieState(mainLightState);
                RestoreHd2dAutonomousP2LightCookieState(libraryWindowState);
                AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-53 light cookie/gobo review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2LightCookieGobos()
        {
            EnsureHd2dAutonomousP2LightCookieGoboProfile();
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2LightCookieGobos()
        {
            var profile = EnsureHd2dAutonomousP2LightCookieGoboProfile();
            if (profile.directionalCanopyCookie == null || profile.windowGoboCookie == null)
            {
                throw new InvalidOperationException("House slice validation failed: P2-53 light cookie/gobo profile must reference both generated cookies.");
            }

            if (!profile.needsTomApproval || profile.runtimePresetIntegrationApproved)
            {
                throw new InvalidOperationException("House slice validation failed: P2-53 light cookie/gobo profile must remain NEEDS-TOM and must not mark runtime preset integration approved.");
            }

            var dappleStats = AnalyzeHd2dAutonomousP2CookieTexture(profile.directionalCanopyCookie);
            var windowStats = AnalyzeHd2dAutonomousP2CookieTexture(profile.windowGoboCookie);
            if (!dappleStats.IsValidDirectionalDapple || !windowStats.IsValidWindowGobo)
            {
                throw new InvalidOperationException(
                    "House slice validation failed: P2-53 generated cookie metrics are outside conservative review bounds. " +
                    $"dapple(avg={dappleStats.Average:0.###}, min={dappleStats.Min:0.###}, max={dappleStats.Max:0.###}, std={dappleStats.StdDev:0.###}), " +
                    $"window(avg={windowStats.Average:0.###}, min={windowStats.Min:0.###}, max={windowStats.Max:0.###}, std={windowStats.StdDev:0.###}).");
            }

            var sunCycleSource = File.Exists(Hd2dAutonomousP2SunCycleDriverPath) ? File.ReadAllText(Hd2dAutonomousP2SunCycleDriverPath) : string.Empty;
            ValidateSourceToken(sunCycleSource, "directionalSunLight.cookie = effectiveValues.cookieTexture", Hd2dAutonomousP2SunCycleDriverPath);
            ValidateSourceToken(sunCycleSource, "directionalSunLight.cookieSize2D", Hd2dAutonomousP2SunCycleDriverPath);

            var lightingSource = File.Exists(Hd2dAutonomousP2LightingDirectorPath) ? File.ReadAllText(Hd2dAutonomousP2LightingDirectorPath) : string.Empty;
            ValidateSourceToken(lightingSource, "libraryWindowLight.cookie = enabled ? EnsureLibraryWindowCookieTexture() : null", Hd2dAutonomousP2LightingDirectorPath);

            var presetGuids = AssetDatabase.FindAssets("t:SunPresetData", new[] { "Assets/Settings/SunCycle" });
            if (presetGuids.Length < 4)
            {
                throw new InvalidOperationException("House slice validation failed: P2-53 expected all four SunPresetData assets with cookie fields.");
            }

            for (var i = 0; i < presetGuids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(presetGuids[i]);
                var preset = AssetDatabase.LoadAssetAtPath<SunPresetData>(path);
                if (preset == null || preset.cookieTexture == null || preset.cookieSize < 1f)
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-53 sun preset cookie data is missing or invalid at {path}.");
                }
            }
        }

        private static FastVsHd2dLightCookieGoboProfile EnsureHd2dAutonomousP2LightCookieGoboProfile()
        {
            var dappleCookie = EnsureHd2dAutonomousP2CanopyDappleCookieTexture();
            var windowCookie = EnsureHd2dAutonomousP2WindowGoboCookieTexture();
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dLightCookieGoboProfile>(Hd2dAutonomousP2LightCookieGoboProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dLightCookieGoboProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2LightCookieGoboProfilePath);
            }

            profile.directionalCanopyCookie = dappleCookie;
            profile.directionalCookieSize = 8.5f;
            profile.directionalCookieContrast = 0.32f;
            profile.directionalCookieDriftPreview = 0.04f;
            profile.windowGoboCookie = windowCookie;
            profile.windowGoboIntensity = 2.4f;
            profile.windowGoboRange = 8.5f;
            profile.windowGoboSpotAngle = 46f;
            profile.needsTomApproval = true;
            profile.runtimePresetIntegrationApproved = false;
            profile.reviewNotes =
                "Conservative P2-53 data staging: generated dapple and window-gobo cookies are review baselines only. Tom should approve final cookie art, intensity, scale, and runtime preset integration.";
            EditorUtility.SetDirty(profile);
            return profile;
        }

        private static Texture2D EnsureHd2dAutonomousP2CanopyDappleCookieTexture()
        {
            var texture = EnsureGeneratedTexture(
                Hd2dAutonomousP2CanopyDappleCookieId,
                Hd2dAutonomousP2CookieSize,
                Hd2dAutonomousP2CookieSize,
                FilterMode.Bilinear,
                SampleHd2dAutonomousP2CanopyDappleCookiePixel);
            texture.wrapMode = TextureWrapMode.Repeat;
            EditorUtility.SetDirty(texture);
            return texture;
        }

        private static Texture2D EnsureHd2dAutonomousP2WindowGoboCookieTexture()
        {
            return EnsureGeneratedTexture(
                Hd2dAutonomousP2WindowGoboCookieId,
                Hd2dAutonomousP2CookieSize,
                Hd2dAutonomousP2CookieSize,
                FilterMode.Bilinear,
                SampleHd2dAutonomousP2WindowGoboCookiePixel);
        }

        private static Color SampleHd2dAutonomousP2CanopyDappleCookiePixel(int x, int y)
        {
            var u = x / 127f;
            var v = y / 127f;
            var luma = Mathf.Lerp(0.64f, 0.98f, Mathf.PerlinNoise((u * 5.8f) + 1.7f, (v * 5.2f) + 3.1f));
            luma += (Mathf.PerlinNoise((u * 14.0f) + 0.3f, (v * 12.0f) + 0.9f) - 0.5f) * 0.14f;

            for (var i = 0; i < 11; i++)
            {
                var cx = Hd2dAutonomousP2Hash01(i, 37, 731);
                var cy = Hd2dAutonomousP2Hash01(i, 83, 947);
                var rx = Mathf.Lerp(0.035f, 0.085f, Hd2dAutonomousP2Hash01(i, 19, 433));
                var ry = Mathf.Lerp(0.025f, 0.075f, Hd2dAutonomousP2Hash01(i, 53, 617));
                var dx = (u - cx) / rx;
                var dy = (v - cy) / ry;
                var leaf = 1f - Mathf.SmoothStep(0.72f, 1.08f, (dx * dx) + (dy * dy));
                luma -= leaf * 0.27f;
            }

            var branchA = 1f - Mathf.SmoothStep(0.018f, 0.075f, Mathf.Abs(((u * 0.88f) + (v * 0.36f)) - 0.58f));
            var branchB = 1f - Mathf.SmoothStep(0.012f, 0.060f, Mathf.Abs(((u * -0.42f) + (v * 0.92f)) - 0.21f));
            luma -= (branchA * 0.16f) + (branchB * 0.11f);
            luma = Mathf.Clamp(luma, 0.34f, 1.0f);
            return new Color(luma, luma, luma, 1f);
        }

        private static Color SampleHd2dAutonomousP2WindowGoboCookiePixel(int x, int y)
        {
            var u = x / 127f;
            var v = y / 127f;
            var edge = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((u * (1f - u) * v * (1f - v)) * 18f));
            var luma = Mathf.Lerp(0.58f, 1.20f, edge);
            var verticalA = Hd2dAutonomousP2SoftBand01(u, 0.34f, 0.026f);
            var verticalB = Hd2dAutonomousP2SoftBand01(u, 0.66f, 0.026f);
            var horizontalA = Hd2dAutonomousP2SoftBand01(v, 0.33f, 0.024f);
            var horizontalB = Hd2dAutonomousP2SoftBand01(v, 0.67f, 0.024f);
            var diagonal = 1f - Mathf.SmoothStep(0.012f, 0.050f, Mathf.Abs(((u * 0.72f) + (v * 0.48f)) - 0.62f));
            luma -= Mathf.Max(Mathf.Max(verticalA, verticalB), Mathf.Max(horizontalA, horizontalB)) * 0.34f;
            luma -= diagonal * 0.08f;
            luma += (Mathf.PerlinNoise((u * 18f) + 4.1f, (v * 18f) + 2.4f) - 0.5f) * 0.045f;
            luma = Mathf.Clamp(luma, 0.32f, 0.99f);
            return new Color(luma, luma, luma, 1f);
        }

        private static void PrepareHd2dAutonomousP2LightCookieShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsHouseLightingDirector director,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            FastVsHouseArea area,
            Vector3 playerLocal,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView)
        {
            visibility.SetActiveAreaForReview(area);
            controller.ForcePlayerCurrentLocalForReview(playerLocal);
            guide.ApplyActiveTimeIsolationForReview();
            director.ApplyAreaForReview(area);
            realtimeRig.ApplyNowForReview();
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 170f;
            PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(playerLocal), cameraOffset, lookOffset);
            ApplyStage7BokehFocusForReview(camera);
        }

        private static void ApplyHd2dAutonomousP2DirectionalCookie(Light mainLight, Texture cookie, float cookieSize)
        {
            mainLight.enabled = true;
            mainLight.type = LightType.Directional;
            mainLight.cookie = cookie;
            mainLight.cookieSize2D = new Vector2(cookieSize, cookieSize);
        }

        private static void ApplyHd2dAutonomousP2WindowGobo(Light windowLight, Texture cookie, FastVsHd2dLightCookieGoboProfile profile)
        {
            windowLight.enabled = true;
            windowLight.type = LightType.Spot;
            windowLight.intensity = profile.windowGoboIntensity;
            windowLight.range = profile.windowGoboRange;
            windowLight.spotAngle = profile.windowGoboSpotAngle;
            windowLight.shadows = LightShadows.None;
            windowLight.color = new Color(1.00f, 0.78f, 0.52f, 1f);
            windowLight.transform.SetPositionAndRotation(new Vector3(28.55f, 3.05f, 23.15f), Quaternion.Euler(58f, 36f, 0f));
            windowLight.cookie = cookie;
        }

        private static List<Hd2dAutonomousP2RendererEnabledState> CaptureHd2dAutonomousP2WindowProxyRendererStates()
        {
            var states = new List<Hd2dAutonomousP2RendererEnabledState>();
            for (var i = 0; i < Hd2dAutonomousP2WindowProxyRendererNames.Length; i++)
            {
                var target = FindSceneObjectIncludingInactive(Hd2dAutonomousP2WindowProxyRendererNames[i]);
                if (target == null)
                {
                    continue;
                }

                var renderers = target.GetComponentsInChildren<Renderer>(true);
                for (var rendererIndex = 0; rendererIndex < renderers.Length; rendererIndex++)
                {
                    var renderer = renderers[rendererIndex];
                    if (renderer == null)
                    {
                        continue;
                    }

                    states.Add(new Hd2dAutonomousP2RendererEnabledState
                    {
                        Renderer = renderer,
                        Enabled = renderer.enabled
                    });
                }
            }

            return states;
        }

        private static void SetHd2dAutonomousP2WindowProxyRenderersEnabled(
            IReadOnlyList<Hd2dAutonomousP2RendererEnabledState> states,
            bool enabled)
        {
            for (var i = 0; i < states.Count; i++)
            {
                if (states[i].Renderer == null)
                {
                    continue;
                }

                states[i].Renderer.enabled = enabled;
                EditorUtility.SetDirty(states[i].Renderer);
            }
        }

        private static void RestoreHd2dAutonomousP2WindowProxyRendererStates(
            IReadOnlyList<Hd2dAutonomousP2RendererEnabledState> states)
        {
            for (var i = 0; i < states.Count; i++)
            {
                if (states[i].Renderer == null)
                {
                    continue;
                }

                states[i].Renderer.enabled = states[i].Enabled;
                EditorUtility.SetDirty(states[i].Renderer);
            }
        }

        private static void SaveHd2dAutonomousP2LightCookieShot(Camera camera, string outputDirectory, string fileName)
        {
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
        }

        private static Hd2dAutonomousP2LightCookieState CaptureHd2dAutonomousP2LightCookieState(Light light)
        {
            return new Hd2dAutonomousP2LightCookieState
            {
                Light = light,
                Enabled = light.enabled,
                Type = light.type,
                Cookie = light.cookie,
                CookieSize2D = light.cookieSize2D,
                Intensity = light.intensity,
                Range = light.range,
                SpotAngle = light.spotAngle,
                Color = light.color,
                Shadows = light.shadows,
                Position = light.transform.position,
                Rotation = light.transform.rotation
            };
        }

        private static void RestoreHd2dAutonomousP2LightCookieState(Hd2dAutonomousP2LightCookieState state)
        {
            if (state.Light == null)
            {
                return;
            }

            state.Light.enabled = state.Enabled;
            state.Light.type = state.Type;
            state.Light.cookie = state.Cookie;
            state.Light.cookieSize2D = state.CookieSize2D;
            state.Light.intensity = state.Intensity;
            state.Light.range = state.Range;
            state.Light.spotAngle = state.SpotAngle;
            state.Light.color = state.Color;
            state.Light.shadows = state.Shadows;
            state.Light.transform.SetPositionAndRotation(state.Position, state.Rotation);
        }

        private static Hd2dAutonomousP2CookieTextureStats AnalyzeHd2dAutonomousP2CookieTexture(Texture2D texture)
        {
            if (texture == null)
            {
                return default;
            }

            var pixels = texture.GetPixels();
            var sum = 0.0;
            var sumSq = 0.0;
            var min = 1f;
            var max = 0f;
            for (var i = 0; i < pixels.Length; i++)
            {
                var luma = (pixels[i].r * 0.2126f) + (pixels[i].g * 0.7152f) + (pixels[i].b * 0.0722f);
                sum += luma;
                sumSq += luma * luma;
                min = Mathf.Min(min, luma);
                max = Mathf.Max(max, luma);
            }

            var average = pixels.Length > 0 ? (float)(sum / pixels.Length) : 0f;
            var variance = pixels.Length > 0 ? Mathf.Max(0f, (float)(sumSq / pixels.Length) - (average * average)) : 0f;
            return new Hd2dAutonomousP2CookieTextureStats
            {
                Width = texture.width,
                Height = texture.height,
                Average = average,
                Min = min,
                Max = max,
                StdDev = Mathf.Sqrt(variance),
                AssetPath = AssetDatabase.GetAssetPath(texture)
            };
        }

        private static void WriteHd2dAutonomousP2LightCookieGoboReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            FastVsHd2dLightCookieGoboProfile profile,
            Hd2dAutonomousP2CookieTextureStats dappleStats,
            Hd2dAutonomousP2CookieTextureStats windowStats,
            Hd2dAutonomousP1DepthPrimingDiffMetrics dappleDiff,
            Hd2dAutonomousP1DepthPrimingDiffMetrics windowDiff,
            int isolatedWindowProxyRendererCount)
        {
            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            var lines = new List<string>
            {
                "# P2-53 Light Cookie / Gobo Review",
                string.Empty,
                "- Scope: stage conservative generated light-cookie/gobo data for canopy dapple and shaped window light; keep final art/intensity as NEEDS-TOM.",
                "- Runtime ownership: SunCycle owns Directional Light cookies; LightingDirector owns the library spot cookie. This capture applies the P2 profile as a review override only.",
                string.Empty,
                "| Profile | Value |",
                "|---|---:|",
                $"| Directional cookie size | {profile.directionalCookieSize:0.###} |",
                $"| Directional cookie contrast | {profile.directionalCookieContrast:0.###} |",
                $"| Window gobo intensity / range / angle | {profile.windowGoboIntensity:0.###} / {profile.windowGoboRange:0.###} / {profile.windowGoboSpotAngle:0.###} |",
                $"| Needs Tom approval | {FormatBool(profile.needsTomApproval)} |",
                $"| Runtime preset integration approved | {FormatBool(profile.runtimePresetIntegrationApproved)} |",
                $"| Window proxy renderers hidden for isolated spot-cookie A/B | {isolatedWindowProxyRendererCount} |",
                string.Empty,
                "| Cookie texture | Size | Avg | Min | Max | StdDev | Asset |",
                "|---|---:|---:|---:|---:|---:|---|",
                dappleStats.ToReportRow("Canopy dapple"),
                windowStats.ToReportRow("Window gobo"),
                string.Empty,
                "| A/B evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                dappleDiff.ToReportRow("Canopy cookie off vs on"),
                windowDiff.ToReportRow("Window spot no gobo vs gobo, emissive proxy hidden"),
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Central Plaza canopy/ground baseline with directional cookie disabled |",
                $"| `{screenshotFiles[1]}` | Same view with generated directional canopy dapple cookie enabled |",
                $"| `{screenshotFiles[2]}` | Current Library spot light with generated window proxy renderers hidden and spot cookie disabled |",
                $"| `{screenshotFiles[3]}` | Same isolated receiver view with generated window/gobo cookie enabled |",
                string.Empty,
                "Recommendation: keep the profile and generated-cookie metrics as data prep only. Tom should replace or approve cookie art, choose final scale/intensity/drift, and then decide whether to integrate the P2 dapple texture into the SunPresetData assets."
            };

            File.WriteAllLines(Path.Combine(outputDirectory, "light_cookie_gobo_review.md"), lines, Encoding.UTF8);
        }

        private static float Hd2dAutonomousP2SoftBand01(float value, float center, float halfWidth)
        {
            return 1f - Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(Mathf.Abs(value - center) / Mathf.Max(0.0001f, halfWidth)));
        }

        private static float Hd2dAutonomousP2Hash01(int x, int y, int seed)
        {
            unchecked
            {
                var hash = seed;
                hash ^= x * 374761393;
                hash = (hash << 13) ^ hash;
                hash ^= y * 668265263;
                hash *= 1274126177;
                hash ^= hash >> 16;
                return (hash & 0x7fffffff) / (float)int.MaxValue;
            }
        }

        private struct Hd2dAutonomousP2LightCookieState
        {
            public Light Light;
            public bool Enabled;
            public LightType Type;
            public Texture Cookie;
            public Vector2 CookieSize2D;
            public float Intensity;
            public float Range;
            public float SpotAngle;
            public Color Color;
            public LightShadows Shadows;
            public Vector3 Position;
            public Quaternion Rotation;
        }

        private struct Hd2dAutonomousP2RendererEnabledState
        {
            public Renderer Renderer;
            public bool Enabled;
        }

        private struct Hd2dAutonomousP2CookieTextureStats
        {
            public int Width;
            public int Height;
            public float Average;
            public float Min;
            public float Max;
            public float StdDev;
            public string AssetPath;

            public bool IsValidDirectionalDapple =>
                Width == Hd2dAutonomousP2CookieSize &&
                Height == Hd2dAutonomousP2CookieSize &&
                Average >= 0.52f &&
                Average <= 0.86f &&
                Min <= 0.56f &&
                Max >= 0.875f &&
                StdDev >= 0.055f;

            public bool IsValidWindowGobo =>
                Width == Hd2dAutonomousP2CookieSize &&
                Height == Hd2dAutonomousP2CookieSize &&
                Average >= 0.46f &&
                Average <= 0.86f &&
                Min <= 0.42f &&
                Max >= 0.88f &&
                StdDev >= 0.060f;

            public string ToReportRow(string label)
            {
                return $"| {label} | {Width} x {Height} | {Average:0.###} | {Min:0.###} | {Max:0.###} | {StdDev:0.###} | `{AssetPath}` |";
            }
        }
    }
}
