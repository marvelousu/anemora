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
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP1AmbientDustPollenRootName = "FastVS_HD2D_P1_AmbientDustPollenLayer";
        private const string Hd2dAutonomousP1AmbientDustPollenSystemName = "FastVS_HD2D_P1_AmbientDustPollen_CPUShurikenWorldBox";
        private const string Hd2dAutonomousP1AmbientDustPollenProfilePath = "Assets/Settings/FastVS_HD2D_P1_AmbientDustPollenProfile.asset";

        public static void CaptureHd2dAutonomousP1Item45AmbientDustPollenBatch()
        {
            CreateHouseSliceScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var realtimeRig = UnityEngine.Object.FindFirstObjectByType<FastVsRealtimeLightShadowRig>();
            var camera = Camera.main;
            var sunCycleRoot = FindSceneObjectIncludingInactive(Hd2dPhaseASunCycleRootName);
            var sunDriver = sunCycleRoot != null ? sunCycleRoot.GetComponent<AnemoraSunCycleDriver>() : null;
            var layerRoot = FindSceneObjectIncludingInactive(Hd2dAutonomousP1AmbientDustPollenRootName);
            var layer = layerRoot != null ? layerRoot.GetComponent<FastVsHd2dAmbientDustPollenLayer>() : null;
            if (controller == null || visibility == null || guide == null || realtimeRig == null || camera == null || sunDriver == null || layer == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P1-45 ambient dust/pollen capture failed: scene review components are missing.");
            }

            ValidateHd2dAutonomousP1AmbientDustPollenLayer();
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dAmbientDustPollenProfile>(Hd2dAutonomousP1AmbientDustPollenProfilePath);
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("ambient_dust_pollen_layer");
            Directory.CreateDirectory(outputDirectory);
            var screenshotFiles = new[]
            {
                "01_dust_off_shaded_baseline.png",
                "02_dust_on_shaded_pollen_layer.png",
                "03_drift_frame_a_seeded.png",
                "04_drift_frame_b_after_one_second.png",
                "05_evening_warm_pollen_tint.png",
                "06_library_shadow_no_shafts.png"
            };
            var shotRows = new List<string>();
            var captureEmissionMultiplier = profile != null ? profile.ReviewCaptureEmissionMultiplierForReview : 2.15f;
            var warmupSeconds = profile != null ? profile.LifetimeForReview : 10.5f;
            try
            {
                guide.SetMovementFrozen(true);
                sunDriver.ApplyPreset(SunPreset.Noon, true);
                realtimeRig.ApplyNowForReview();

                CaptureHd2dAutonomousP1AmbientDustPollenReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHouseArea.AriaStreet,
                    Chapter1AriaStreetMapCenter + new Vector3(-1.3f, 0.02f, 5.1f),
                    new Vector3(0.35f, 8.2f, -10.8f),
                    new Vector3(0.02f, 1.18f, 0.32f),
                    38f,
                    false,
                    0f,
                    0.28f,
                    0f,
                    true,
                    outputDirectory,
                    screenshotFiles[0],
                    "shaded Aria Street baseline with ambient dust disabled",
                    shotRows);

                CaptureHd2dAutonomousP1AmbientDustPollenReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHouseArea.AriaStreet,
                    Chapter1AriaStreetMapCenter + new Vector3(-1.3f, 0.02f, 5.1f),
                    new Vector3(0.35f, 8.2f, -10.8f),
                    new Vector3(0.02f, 1.18f, 0.32f),
                    38f,
                    true,
                    captureEmissionMultiplier,
                    0.28f,
                    warmupSeconds,
                    true,
                    outputDirectory,
                    screenshotFiles[1],
                    "same shaded framing with camera-attached dust/pollen enabled",
                    shotRows);

                CaptureHd2dAutonomousP1AmbientDustPollenReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHouseArea.AriaStreet,
                    Chapter1AriaStreetMapCenter + new Vector3(-1.3f, 0.02f, 5.1f),
                    new Vector3(0.35f, 8.2f, -10.8f),
                    new Vector3(0.02f, 1.18f, 0.32f),
                    38f,
                    true,
                    captureEmissionMultiplier,
                    0.28f,
                    0f,
                    false,
                    outputDirectory,
                    screenshotFiles[2],
                    "drift frame A after seeded prewarm",
                    shotRows);

                CaptureHd2dAutonomousP1AmbientDustPollenReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHouseArea.AriaStreet,
                    Chapter1AriaStreetMapCenter + new Vector3(-1.3f, 0.02f, 5.1f),
                    new Vector3(0.35f, 8.2f, -10.8f),
                    new Vector3(0.02f, 1.18f, 0.32f),
                    38f,
                    true,
                    captureEmissionMultiplier,
                    0.28f,
                    1.05f,
                    false,
                    outputDirectory,
                    screenshotFiles[3],
                    "drift frame B after 1.05 seconds with no restart",
                    shotRows);

                sunDriver.ApplyPreset(SunPreset.Evening, true);
                realtimeRig.ApplyNowForReview();
                CaptureHd2dAutonomousP1AmbientDustPollenReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHouseArea.CentralPlaza,
                    CentralPlazaVsCenter + new Vector3(0.3f, 0.02f, 3.25f),
                    new Vector3(0.2f, 11.8f, -13.2f),
                    new Vector3(0.00f, 1.35f, 0.32f),
                    43f,
                    true,
                    captureEmissionMultiplier,
                    0.78f,
                    warmupSeconds,
                    true,
                    outputDirectory,
                    screenshotFiles[4],
                    "evening warm tint check across the broader scene",
                    shotRows);

                sunDriver.ApplyPreset(SunPreset.Noon, true);
                realtimeRig.ApplyNowForReview();
                CaptureHd2dAutonomousP1AmbientDustPollenReviewShot(
                    controller,
                    visibility,
                    guide,
                    realtimeRig,
                    camera,
                    layer,
                    FastVsHouseArea.Library,
                    LibraryVsCenter + new Vector3(-0.92f, 0.02f, -0.62f),
                    new Vector3(0.10f, 4.4f, -5.8f),
                    new Vector3(0.10f, 0.92f, -0.26f),
                    35f,
                    true,
                    captureEmissionMultiplier,
                    0.16f,
                    warmupSeconds,
                    true,
                    outputDirectory,
                    screenshotFiles[5],
                    "library shadow check proves the layer is not shaft-gated",
                    shotRows);

                var offOnMetrics = ValidateHd2dAutonomousP1AmbientDustPollenReviewPairDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1], "dust-off-vs-dust-on");
                var driftMetrics = ValidateHd2dAutonomousP1AmbientDustPollenReviewPairDiff(outputDirectory, screenshotFiles[2], screenshotFiles[3], "one-second-drift");
                WriteHd2dAutonomousP1AmbientDustPollenReviewReport(outputDirectory, screenshotFiles, shotRows, offOnMetrics, driftMetrics);
            }
            finally
            {
                layer.ClearReviewOverrideForReview();
                if (profile != null)
                {
                    layer.SimulateForReview(profile.LifetimeForReview, true);
                }
            }

            Debug.Log($"Fast VS autonomous P1-45 ambient dust/pollen review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP1AmbientDustPollenLayer(Transform sceneRoot, Camera camera)
        {
            _ = sceneRoot;
            if (camera == null)
            {
                return;
            }

            var profile = EnsureHd2dAutonomousP1AmbientDustPollenProfileAsset();
            var material = EnsureHd2dAtmosphereParticleMaterial();
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP1AmbientDustPollenRootName);
            if (root == null)
            {
                root = new GameObject(Hd2dAutonomousP1AmbientDustPollenRootName, typeof(FastVsHd2dAmbientDustPollenLayer));
            }
            else if (root.GetComponent<FastVsHd2dAmbientDustPollenLayer>() == null)
            {
                root.AddComponent<FastVsHd2dAmbientDustPollenLayer>();
            }

            root.transform.SetParent(camera.transform, false);
            root.transform.localPosition = profile.CameraLocalOffsetForReview;
            root.transform.localRotation = Quaternion.identity;
            root.transform.localScale = Vector3.one;
            root.SetActive(true);

            var systemTransform = root.transform.Find(Hd2dAutonomousP1AmbientDustPollenSystemName);
            var systemObject = systemTransform != null ? systemTransform.gameObject : null;
            if (systemObject == null)
            {
                systemObject = new GameObject(Hd2dAutonomousP1AmbientDustPollenSystemName, typeof(ParticleSystem), typeof(ParticleSystemRenderer));
                systemObject.transform.SetParent(root.transform, false);
            }

            systemObject.transform.localPosition = Vector3.zero;
            systemObject.transform.localRotation = Quaternion.identity;
            systemObject.transform.localScale = Vector3.one;
            systemObject.SetActive(true);

            var system = systemObject.GetComponent<ParticleSystem>();
            if (system == null)
            {
                system = systemObject.AddComponent<ParticleSystem>();
            }

            var renderer = systemObject.GetComponent<ParticleSystemRenderer>();
            if (renderer == null)
            {
                renderer = systemObject.AddComponent<ParticleSystemRenderer>();
            }

            ConfigureHd2dAutonomousP1AmbientDustPollenParticleSystem(system, renderer, profile, material);
            var layer = root.GetComponent<FastVsHd2dAmbientDustPollenLayer>();
            layer.ConfigureForReview(
                profile,
                camera,
                system,
                renderer,
                true,
                true,
                profile.ConservativeReviewModeForReview,
                profile.RequiresTomArtApprovalForReview);
            layer.SimulateForReview(profile.LifetimeForReview, true);

            EditorUtility.SetDirty(systemObject);
            EditorUtility.SetDirty(root);
        }

        private static void ValidateHd2dAutonomousP1AmbientDustPollenLayer()
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dAmbientDustPollenProfile>(Hd2dAutonomousP1AmbientDustPollenProfilePath);
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP1AmbientDustPollenRootName);
            var camera = Camera.main;
            if (profile == null || root == null || camera == null)
            {
                throw new InvalidOperationException("House slice validation failed: P1-45 needs an ambient dust/pollen profile, camera child root, and main camera.");
            }

            var layer = root.GetComponent<FastVsHd2dAmbientDustPollenLayer>();
            var systems = root.GetComponentsInChildren<ParticleSystem>(true);
            var renderers = root.GetComponentsInChildren<ParticleSystemRenderer>(true);
            var system = systems.Length > 0 ? systems[0] : null;
            var renderer = renderers.Length > 0 ? renderers[0] : null;
            if (layer == null ||
                systems.Length != 1 ||
                renderer == null ||
                system == null ||
                !root.transform.IsChildOf(camera.transform) ||
                !layer.CameraAttachedForReview ||
                !layer.IndependentOfSunShaftsForReview ||
                !layer.UsesCpuShurikenForReview ||
                !profile.CpuShurikenForReview ||
                !profile.IndependentOfSunShaftsForReview ||
                !profile.CameraAttachedForReview ||
                !profile.SimulationSpaceWorldForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P1-45 must be one camera-attached CPU Shuriken layer independent from sun shafts.");
            }

            var main = system.main;
            var shape = system.shape;
            var emission = system.emission;
            var noise = system.noise;
            var velocity = system.velocityOverLifetime;
            if (main.simulationSpace != ParticleSystemSimulationSpace.World ||
                main.maxParticles < 80 ||
                main.maxParticles > 200 ||
                !shape.enabled ||
                shape.shapeType != ParticleSystemShapeType.Box ||
                shape.scale.x < 12f ||
                shape.scale.z < 14f ||
                !emission.enabled ||
                profile.EmissionRateForReview <= 0f ||
                profile.AlphaCeilingForReview > 0.12f ||
                !noise.enabled ||
                profile.NoiseStrengthForReview <= 0f ||
                !velocity.enabled ||
                renderer.sharedMaterial == null ||
                renderer.shadowCastingMode != ShadowCastingMode.Off ||
                renderer.receiveShadows ||
                !profile.ConservativeReviewModeForReview ||
                !profile.RequiresTomArtApprovalForReview)
            {
                throw new InvalidOperationException("House slice validation failed: P1-45 dust/pollen layer must use world-space box emission, low alpha, noise drift, no shadows, and conservative TOM-facing data.");
            }

            var editorSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.P1AmbientDustPollen.cs");
            var runtimeProfilePath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dAmbientDustPollenProfile.cs");
            var runtimeLayerPath = Path.Combine("Assets", "Scripts", "FastVS", "FastVsHd2dAmbientDustPollenLayer.cs");
            var mainSourcePath = Path.Combine("Assets", "Editor", "AnemoraFastVsHouseSliceSetup.cs");
            foreach (var token in new[]
            {
                "CaptureHd2dAutonomousP1Item45AmbientDustPollenBatch",
                "ambient_dust_pollen_layer",
                "ParticleSystemSimulationSpace.World",
                "ValidateHd2dAutonomousP1AmbientDustPollenLayer"
            })
            {
                ValidateSourceToken(File.ReadAllText(editorSourcePath), token, editorSourcePath);
            }

            ValidateSourceToken(File.ReadAllText(runtimeProfilePath), "AlphaCeilingForReview", runtimeProfilePath);
            ValidateSourceToken(File.ReadAllText(runtimeLayerPath), "IndependentOfSunShaftsForReview", runtimeLayerPath);
            ValidateSourceToken(File.ReadAllText(mainSourcePath), "CreateHd2dAutonomousP1AmbientDustPollenLayer", mainSourcePath);
            ValidateSourceToken(File.ReadAllText(mainSourcePath), "ValidateHd2dAutonomousP1AmbientDustPollenLayer", mainSourcePath);
        }

        private static FastVsHd2dAmbientDustPollenProfile EnsureHd2dAutonomousP1AmbientDustPollenProfileAsset()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dAmbientDustPollenProfile>(Hd2dAutonomousP1AmbientDustPollenProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dAmbientDustPollenProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP1AmbientDustPollenProfilePath);
            }

            profile.ConfigureForReview(
                144,
                12f,
                10.5f,
                13.5f,
                0.012f,
                0.042f,
                0.024f,
                0.062f,
                new Vector3(0f, 0.18f, 6.0f),
                new Vector3(15.5f, 4.2f, 18.0f),
                new Vector3(0.018f, 0.014f, -0.010f),
                0.16f,
                0.034f,
                0.18f,
                0.38f,
                new Color(0.76f, 0.84f, 0.90f, 0.082f),
                new Color(1.00f, 0.82f, 0.54f, 0.104f),
                2.15f,
                true,
                true,
                true,
                true,
                true,
                true,
                "Procedural CPU Shuriken camera-attached ambient dust/pollen review layer; keep alpha below snow/noise thresholds and request Tom to tune final density, tint, and grade.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static void ConfigureHd2dAutonomousP1AmbientDustPollenParticleSystem(
            ParticleSystem system,
            ParticleSystemRenderer renderer,
            FastVsHd2dAmbientDustPollenProfile profile,
            Material material)
        {
            var main = system.main;
            main.loop = true;
            main.prewarm = true;
            main.playOnAwake = true;
            main.duration = profile.DurationForReview;
            main.startLifetime = new ParticleSystem.MinMaxCurve(profile.LifetimeForReview * 0.82f, profile.LifetimeForReview);
            main.startSpeed = new ParticleSystem.MinMaxCurve(profile.StartSpeedMinForReview, profile.StartSpeedMaxForReview);
            main.startSize = new ParticleSystem.MinMaxCurve(profile.StartSizeMinForReview, profile.StartSizeMaxForReview);
            main.startColor = new ParticleSystem.MinMaxGradient(Color.Lerp(profile.CoolShadeTintForReview, profile.WarmPollenTintForReview, profile.DefaultTintWarmthForReview));
            main.maxParticles = profile.MaxParticlesForReview;
            main.gravityModifier = 0f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.stopAction = ParticleSystemStopAction.None;

            var emission = system.emission;
            emission.enabled = true;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(profile.EmissionRateForReview);

            var shape = system.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = profile.BoxSizeForReview;
            shape.position = Vector3.zero;
            shape.rotation = Vector3.zero;
            shape.randomDirectionAmount = profile.RandomDirectionAmountForReview;

            var velocity = system.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.World;
            velocity.x = new ParticleSystem.MinMaxCurve(profile.WorldWindVelocityForReview.x);
            velocity.y = new ParticleSystem.MinMaxCurve(profile.WorldWindVelocityForReview.y);
            velocity.z = new ParticleSystem.MinMaxCurve(profile.WorldWindVelocityForReview.z);

            var noise = system.noise;
            noise.enabled = true;
            noise.strength = new ParticleSystem.MinMaxCurve(profile.NoiseStrengthForReview);
            noise.frequency = profile.NoiseFrequencyForReview;
            noise.damping = true;
            noise.quality = ParticleSystemNoiseQuality.Medium;

            renderer.sharedMaterial = material;
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.alignment = ParticleSystemRenderSpace.View;
            renderer.sortingOrder = 1;
            EditorUtility.SetDirty(system);
            EditorUtility.SetDirty(renderer);
        }

        private static void CaptureHd2dAutonomousP1AmbientDustPollenReviewShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            FastVsRealtimeLightShadowRig realtimeRig,
            Camera camera,
            FastVsHd2dAmbientDustPollenLayer layer,
            FastVsHouseArea activeArea,
            Vector3 anchorLocalPosition,
            Vector3 cameraOffset,
            Vector3 lookOffset,
            float fieldOfView,
            bool layerEnabled,
            float emissionMultiplier,
            float tintWarmth,
            float simulateSeconds,
            bool restart,
            string outputDirectory,
            string fileName,
            string label,
            ICollection<string> rows)
        {
            visibility.SetActiveAreaForReview(activeArea);
            controller.ForcePlayerCurrentLocalForReview(anchorLocalPosition + new Vector3(0f, 0.02f, -1.0f));
            guide.ApplyActiveTimeIsolationForReview();
            realtimeRig.ApplyNowForReview();
            camera.orthographic = false;
            camera.fieldOfView = fieldOfView;
            camera.nearClipPlane = 0.03f;
            camera.farClipPlane = 170f;
            PositionChapter1AllMapsCamera(camera, controller.CurrentSpaceRootForReview.TransformPoint(anchorLocalPosition), cameraOffset, lookOffset);
            ApplyStage7BokehFocusForReview(camera);
            layer.SetReviewOverrideForReview(layerEnabled, emissionMultiplier);
            layer.SetReviewTintWarmthForReview(tintWarmth);
            layer.SimulateForReview(Mathf.Max(0f, simulateSeconds), restart);
            WarmUpCameraRender(camera);
            SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            ValidateCloseReviewOutputExists(outputDirectory, fileName);
            rows?.Add($"| `{fileName}` | {label} | {activeArea} | {FormatVector3ForReport(anchorLocalPosition)} | {FormatVector3ForReport(cameraOffset)} | {fieldOfView:0.#} | {emissionMultiplier:0.##} | {tintWarmth:0.##} |");
        }

        private static void WriteHd2dAutonomousP1AmbientDustPollenReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            Hd2dAutonomousP1AmbientDustPollenDiffMetrics offOnMetrics,
            Hd2dAutonomousP1AmbientDustPollenDiffMetrics driftMetrics)
        {
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dAmbientDustPollenProfile>(Hd2dAutonomousP1AmbientDustPollenProfilePath);
            var root = FindSceneObjectIncludingInactive(Hd2dAutonomousP1AmbientDustPollenRootName);
            var layer = root != null ? root.GetComponent<FastVsHd2dAmbientDustPollenLayer>() : null;
            var lines = new List<string>
            {
                "# P1-45 Scene-Wide Ambient Dust/Pollen Layer Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative whole-scene CPU Shuriken dust/pollen layer, camera-attached with World simulation so shaded non-shaft areas receive subtle moving depth cues.",
                "- Recommendation: keep the camera-attached World simulation/profile contract and ask Tom to tune final density, tint warmth, and grade; the review pass intentionally keeps alpha below snow/noise readings.",
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP1AmbientDustPollenProfilePath}` |",
                $"| Root | `{Hd2dAutonomousP1AmbientDustPollenRootName}` |",
                $"| Particle systems / max particles | {layer?.ParticleSystemCountForReview ?? 0} / {layer?.TotalMaxParticlesForReview ?? 0} |",
                $"| Runtime emission / capture multiplier | {profile?.EmissionRateForReview ?? 0f:0.###} / {profile?.ReviewCaptureEmissionMultiplierForReview ?? 0f:0.###} |",
                $"| Alpha ceiling | {profile?.AlphaCeilingForReview ?? 0f:0.###} |",
                $"| Box size | {FormatVector3ForReport(profile != null ? profile.BoxSizeForReview : Vector3.zero)} |",
                $"| World wind velocity | {FormatVector3ForReport(profile != null ? profile.WorldWindVelocityForReview : Vector3.zero)} |",
                $"| Noise strength / frequency | {profile?.NoiseStrengthForReview ?? 0f:0.###} / {profile?.NoiseFrequencyForReview ?? 0f:0.###} |",
                $"| Camera attached / World sim / independent of shafts | {FormatBool(layer != null && layer.CameraAttachedForReview)} / {FormatBool(layer != null && layer.SimulationSpaceWorldForReview)} / {FormatBool(layer != null && layer.IndependentOfSunShaftsForReview)} |",
                $"| Conservative review mode | {FormatBool(profile != null && profile.ConservativeReviewModeForReview)} |",
                $"| Requires TOM art approval | {FormatBool(profile != null && profile.RequiresTomArtApprovalForReview)} |",
                $"| Source note | {profile?.SourceNoteForReview ?? "missing"} |",
                string.Empty,
                "| Comparison | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                offOnMetrics.ToReportRow("dust-off vs dust-on"),
                driftMetrics.ToReportRow("drift frame A vs B"),
                string.Empty,
                "| Screenshot | Label | Area | Anchor | Offset | FOV | Emission x | Tint warmth |",
                "|---|---|---|---|---|---:|---:|---:|"
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|",
                $"| `{screenshotFiles[0]}` | Baseline with P1-45 renderer and emission disabled |",
                $"| `{screenshotFiles[1]}` | Same shaded Aria Street framing with dust/pollen enabled |",
                $"| `{screenshotFiles[2]}` | Drift frame A after prewarm, no spawn pop |",
                $"| `{screenshotFiles[3]}` | Drift frame B after 1.05 seconds without restart |",
                $"| `{screenshotFiles[4]}` | Evening warm pollen tint in a broader scene view |",
                $"| `{screenshotFiles[5]}` | Library shadowed/no-shaft check using the same camera-attached layer |"
            });

            foreach (var screenshotFile in screenshotFiles)
            {
                ValidateScreenshotOutputExists(outputDirectory, screenshotFile);
            }

            File.WriteAllText(Path.Combine(outputDirectory, "ambient_dust_pollen_layer_review.md"), string.Join(Environment.NewLine, lines), Encoding.UTF8);
        }

        private static Hd2dAutonomousP1AmbientDustPollenDiffMetrics ValidateHd2dAutonomousP1AmbientDustPollenReviewPairDiff(string outputDirectory, string firstFile, string secondFile, string label)
        {
            var metrics = MeasureHd2dAutonomousP1AmbientDustPollenDiff(Path.Combine(outputDirectory, firstFile), Path.Combine(outputDirectory, secondFile), 4);
            if (metrics.SampleCount <= 0 || metrics.ChangedPixels <= 0)
            {
                throw new InvalidOperationException($"Fast VS autonomous P1-45 ambient dust/pollen capture failed: {label} images have no measurable particle/drift difference.");
            }

            return metrics;
        }

        private static Hd2dAutonomousP1AmbientDustPollenDiffMetrics MeasureHd2dAutonomousP1AmbientDustPollenDiff(string firstPath, string secondPath, int threshold)
        {
            var firstTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            var secondTexture = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);
            try
            {
                if (!firstTexture.LoadImage(File.ReadAllBytes(firstPath)) || !secondTexture.LoadImage(File.ReadAllBytes(secondPath)))
                {
                    return new Hd2dAutonomousP1AmbientDustPollenDiffMetrics(0, 0, 0f, 0f);
                }

                var firstPixels = firstTexture.GetPixels32();
                var secondPixels = secondTexture.GetPixels32();
                var sampleCount = Mathf.Min(firstPixels.Length, secondPixels.Length);
                var changedPixels = 0;
                var totalDelta = 0f;
                for (var i = 0; i < sampleCount; i++)
                {
                    var delta =
                        Mathf.Abs(firstPixels[i].r - secondPixels[i].r) +
                        Mathf.Abs(firstPixels[i].g - secondPixels[i].g) +
                        Mathf.Abs(firstPixels[i].b - secondPixels[i].b);
                    totalDelta += delta / 3f;
                    if (delta > threshold)
                    {
                        changedPixels++;
                    }
                }

                var changedPercent = sampleCount > 0 ? changedPixels * 100f / sampleCount : 0f;
                var meanDelta = sampleCount > 0 ? totalDelta / sampleCount : 0f;
                return new Hd2dAutonomousP1AmbientDustPollenDiffMetrics(sampleCount, changedPixels, changedPercent, meanDelta);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(firstTexture);
                UnityEngine.Object.DestroyImmediate(secondTexture);
            }
        }

        private readonly struct Hd2dAutonomousP1AmbientDustPollenDiffMetrics
        {
            public readonly int SampleCount;
            public readonly int ChangedPixels;
            public readonly float ChangedPercent;
            public readonly float MeanRgbDelta;

            public Hd2dAutonomousP1AmbientDustPollenDiffMetrics(int sampleCount, int changedPixels, float changedPercent, float meanRgbDelta)
            {
                SampleCount = sampleCount;
                ChangedPixels = changedPixels;
                ChangedPercent = changedPercent;
                MeanRgbDelta = meanRgbDelta;
            }

            public string ToReportRow(string label)
            {
                return $"| {label} | {SampleCount} | {ChangedPixels} | {ChangedPercent:0.###} | {MeanRgbDelta:0.###} |";
            }
        }
    }
}
