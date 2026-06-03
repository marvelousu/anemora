using System;
using System.Collections.Generic;
using System.IO;
using Anemora.FastVS;
using Anemora.TimeManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace Anemora.EditorTools
{
    public static partial class AnemoraFastVsHouseSliceSetup
    {
        private const string Hd2dAutonomousP2LocalVolumetricFogInteriorRootName = "Current_HouseInterior_P2_LocalVolumetricFogReview";
        private const string Hd2dAutonomousP2LocalVolumetricFogCurrentPlazaRootName = "Current_CentralPlaza_P2_LocalVolumetricFogReview";
        private const string Hd2dAutonomousP2LocalVolumetricFogPastPlazaRootName = "Past_CentralPlaza_P2_LocalVolumetricFogReview";
        private const string Hd2dAutonomousP2LocalVolumetricFogProfilePath = "Assets/Settings/FastVS_HD2D_P2_LocalVolumetricFogProfile.asset";
        private const string Hd2dAutonomousP2LocalVolumetricFogProfileRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dLocalVolumetricFogProfile.cs";
        private const string Hd2dAutonomousP2LocalVolumetricFogVolumeRuntimePath = "Assets/Scripts/FastVS/FastVsHd2dLocalVolumetricFogVolume.cs";
        private const string Hd2dAutonomousP2LocalVolumetricFogShaderPath = "Assets/Art/Shaders/FastVS/FastVS_LocalVolumetricFogSlice.shader";
        private const string Hd2dAutonomousP2LocalVolumetricFogMaterialPath = MaterialDirectory + "/FastVS_House_hd2d_p2_local_volumetric_fog_slice.mat";
        private const string Hd2dAutonomousP2LocalVolumetricFogMaterialId = "hd2d_p2_local_volumetric_fog_slice";

        public static void CaptureHd2dAutonomousP2Item65LocalVolumetricFogBatch()
        {
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            if (FindSceneObjectIncludingInactive(Hd2dAutonomousP2LocalVolumetricFogInteriorRootName) == null ||
                FindSceneObjectIncludingInactive(Hd2dAutonomousP2LocalVolumetricFogCurrentPlazaRootName) == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-65 local volumetric fog capture failed: review roots are missing. Run BuildAndValidateBatch before capture.");
            }

            var controller = UnityEngine.Object.FindFirstObjectByType<TimeWindowPairedSpacePortalController>();
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>();
            var guide = UnityEngine.Object.FindFirstObjectByType<FastVsVisualDirectionGuide>();
            var camera = Camera.main;
            var profile = EnsureHd2dAutonomousP2LocalVolumetricFogProfile();
            if (controller == null || visibility == null || guide == null || camera == null || profile == null)
            {
                throw new InvalidOperationException("Fast VS autonomous P2-65 local volumetric fog capture failed: required scene review components are missing.");
            }

            ValidateHd2dAutonomousP2LocalVolumetricFog();
            var outputDirectory = ResolveAutonomousReviewOutputDirectory("localized_volumetric_fog_volumes");
            Directory.CreateDirectory(outputDirectory);

            var screenshotFiles = new[]
            {
                "01_interior_fog_disabled_control.png",
                "02_interior_bounded_mist_enabled.png",
                "03_portal_closed_no_haze_control.png",
                "04_portal_open_luminous_threshold_haze.png",
                "05_low_valley_conservative_mist_context.png",
                "06_portal_stronger_option_for_tom.png"
            };
            var shotRows = new List<string>();

            try
            {
                guide.SetMovementFrozen(true);
                controller.ForcePlayerCurrentLocalForReview(HouseInteriorPlayerStart);
                controller.ClosePortal();

                SetHd2dAutonomousP2LocalVolumetricFogVisible(false);
                CaptureHd2dAutonomousP2LocalVolumetricFogInteriorShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[0],
                    "interior local fog disabled control",
                    0f,
                    shotRows);

                SetHd2dAutonomousP2LocalVolumetricFogVisible(false);
                SetHd2dAutonomousP2LocalVolumetricFogKindVisible(FastVsHd2dLocalVolumetricFogKind.InteriorRoom, true);
                SetHd2dAutonomousP2LocalVolumetricFogKindMultiplier(FastVsHd2dLocalVolumetricFogKind.InteriorRoom, 1f);
                CaptureHd2dAutonomousP2LocalVolumetricFogInteriorShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[1],
                    "bounded room mist enabled: confined inside the room footprint",
                    profile.InteriorDensityForReview,
                    shotRows);

                SetHd2dAutonomousP2LocalVolumetricFogVisible(false);
                SetHd2dAutonomousP2LocalVolumetricFogKindVisible(FastVsHd2dLocalVolumetricFogKind.PortalThreshold, true);
                SetHd2dAutonomousP2LocalVolumetricFogKindMultiplier(FastVsHd2dLocalVolumetricFogKind.PortalThreshold, 1f);
                CaptureHd2dAutonomousP2LocalVolumetricFogPortalShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[2],
                    false,
                    "portal closed: local threshold haze density is absent",
                    profile.PortalClosedDensityForReview,
                    shotRows);

                SetHd2dAutonomousP2LocalVolumetricFogVisible(false);
                SetHd2dAutonomousP2LocalVolumetricFogKindVisible(FastVsHd2dLocalVolumetricFogKind.PortalThreshold, true);
                SetHd2dAutonomousP2LocalVolumetricFogKindMultiplier(FastVsHd2dLocalVolumetricFogKind.PortalThreshold, 1f);
                CaptureHd2dAutonomousP2LocalVolumetricFogPortalShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[3],
                    true,
                    "portal open: luminous local haze hugs the threshold plane",
                    profile.PortalOpenDensityForReview,
                    shotRows);

                SetHd2dAutonomousP2LocalVolumetricFogVisible(false);
                SetHd2dAutonomousP2LocalVolumetricFogKindVisible(FastVsHd2dLocalVolumetricFogKind.LowValley, true);
                SetHd2dAutonomousP2LocalVolumetricFogKindMultiplier(FastVsHd2dLocalVolumetricFogKind.LowValley, 1f);
                CaptureHd2dAutonomousP2LocalVolumetricFogValleyShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[4],
                    "low valley/route pocket: conservative ground-hugging mist",
                    profile.LowValleyDensityForReview,
                    shotRows);

                SetHd2dAutonomousP2LocalVolumetricFogVisible(false);
                SetHd2dAutonomousP2LocalVolumetricFogKindVisible(FastVsHd2dLocalVolumetricFogKind.PortalThreshold, true);
                SetHd2dAutonomousP2LocalVolumetricFogKindMultiplier(FastVsHd2dLocalVolumetricFogKind.PortalThreshold, profile.PortalStrongOptionMultiplierForReview);
                CaptureHd2dAutonomousP2LocalVolumetricFogPortalShot(
                    controller,
                    visibility,
                    guide,
                    camera,
                    outputDirectory,
                    screenshotFiles[5],
                    true,
                    "stronger portal haze option for Tom",
                    profile.PortalOpenDensityForReview * profile.PortalStrongOptionMultiplierForReview,
                    shotRows);
            }
            finally
            {
                controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.34f, 0.02f, 3.26f));
                controller.ClosePortal();
                SetHd2dAutonomousP2LocalVolumetricFogVisible(true);
                SetHd2dAutonomousP2LocalVolumetricFogAllMultipliers(1f);
                SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(true);
                SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
                SetHd2dAutonomousP2ToonWaterMotionReviewVisible(true);
                SetHd2dAutonomousP2CausticsReviewVisible(true);
                SetHd2dAutonomousP2WaterReflectionReviewVisible(true);
                AssetDatabase.SaveAssets();
            }

            var interiorDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[0], screenshotFiles[1]);
            var portalDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[2], screenshotFiles[3]);
            var portalStrongDiff = MeasureHd2dAutonomousP2DirectionalWaterFlowDiff(outputDirectory, screenshotFiles[3], screenshotFiles[5]);
            WriteHd2dAutonomousP2LocalVolumetricFogReviewReport(
                outputDirectory,
                screenshotFiles,
                shotRows,
                profile,
                interiorDiff,
                portalDiff,
                portalStrongDiff);

            AssetDatabase.Refresh();
            Debug.Log($"Fast VS autonomous P2-65 local volumetric fog review captured: {Path.GetFullPath(outputDirectory)}");
        }

        private static void CreateHd2dAutonomousP2LocalVolumetricFog(
            Transform currentInteriorRoot,
            Transform currentCentralPlazaRoot,
            Transform pastCentralPlazaRoot)
        {
            var profile = EnsureHd2dAutonomousP2LocalVolumetricFogProfile();
            var material = EnsureHd2dAutonomousP2LocalVolumetricFogMaterial();
            CreateHd2dAutonomousP2LocalVolumetricFogReviewSet(currentInteriorRoot, currentCentralPlazaRoot, pastCentralPlazaRoot, material, profile);
            AssetDatabase.SaveAssets();
        }

        private static void ValidateHd2dAutonomousP2LocalVolumetricFog()
        {
            var profile = EnsureHd2dAutonomousP2LocalVolumetricFogProfile();
            var material = EnsureHd2dAutonomousP2LocalVolumetricFogMaterial();
            if (profile == null ||
                material == null ||
                !profile.NeedsTomApprovalForReview ||
                !profile.RenderGraphNativePackageDeferredForReview ||
                !profile.LegitimateVolumetricAssetDecisionRequiredForReview ||
                profile.FinalLocalFogApprovedForReview ||
                profile.InteriorDensityForReview < 0.12f ||
                profile.PortalOpenDensityForReview <= profile.PortalClosedDensityForReview + 0.18f ||
                profile.PortalStrongOptionMultiplierForReview <= 1f)
            {
                throw new InvalidOperationException("House slice validation failed: P2-65 needs conservative non-final local fog profile data with the legitimate volumetric package decision deferred to Tom.");
            }

            foreach (var propertyName in new[]
            {
                "_FogColor",
                "_FogDensity",
                "_EdgeFeather",
                "_NoiseStrength",
                "_NoiseScale",
                "_HeightFade",
                "_PortalGlow",
                "_TimeOffset"
            })
            {
                if (!material.HasProperty(propertyName))
                {
                    throw new InvalidOperationException($"House slice validation failed: P2-65 local fog material missing property {propertyName}.");
                }
            }

            var shaderSource = File.ReadAllText(Hd2dAutonomousP2LocalVolumetricFogShaderPath);
            foreach (var token in new[]
            {
                "Anemora/FastVS/LocalVolumetricFogSlice",
                "_FogDensity",
                "_PortalGlow",
                "Blend SrcAlpha OneMinusSrcAlpha",
                "ZWrite Off"
            })
            {
                ValidateSourceToken(shaderSource, token, Hd2dAutonomousP2LocalVolumetricFogShaderPath);
            }

            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2LocalVolumetricFogProfileRuntimePath), "legitimateVolumetricAssetDecisionRequired", Hd2dAutonomousP2LocalVolumetricFogProfileRuntimePath);
            ValidateSourceToken(File.ReadAllText(Hd2dAutonomousP2LocalVolumetricFogVolumeRuntimePath), "HasPortalPair", Hd2dAutonomousP2LocalVolumetricFogVolumeRuntimePath);
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "CreateHd2dAutonomousP2LocalVolumetricFog", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");
            ValidateSourceToken(File.ReadAllText("Assets/Editor/AnemoraFastVsHouseSliceSetup.cs"), "ValidateHd2dAutonomousP2LocalVolumetricFog", "Assets/Editor/AnemoraFastVsHouseSliceSetup.cs");

            var interiorCount = CountHd2dAutonomousP2LocalVolumetricFogVolumes(FastVsHd2dLocalVolumetricFogKind.InteriorRoom);
            var valleyCount = CountHd2dAutonomousP2LocalVolumetricFogVolumes(FastVsHd2dLocalVolumetricFogKind.LowValley);
            var portalCount = CountHd2dAutonomousP2LocalVolumetricFogVolumes(FastVsHd2dLocalVolumetricFogKind.PortalThreshold);
            if (interiorCount < 1 || valleyCount < 1 || portalCount < 2)
            {
                throw new InvalidOperationException($"House slice validation failed: P2-65 review set requires interior, low-valley, and paired portal threshold local fog volumes. Counts interior/valley/portal={interiorCount}/{valleyCount}/{portalCount}.");
            }

            foreach (var volume in FindHd2dAutonomousP2LocalVolumetricFogVolumes())
            {
                if (volume == null ||
                    volume.ProfileForReview != profile ||
                    volume.RendererCountForReview < 3)
                {
                    throw new InvalidOperationException("House slice validation failed: P2-65 local fog volume is missing profile wiring or slice renderers.");
                }

                if (volume.FogKindForReview == FastVsHd2dLocalVolumetricFogKind.PortalThreshold &&
                    (!volume.ReactsToPortalOpenForReview || !volume.SyncsToPortalPlaneForReview))
                {
                    throw new InvalidOperationException("House slice validation failed: P2-65 portal local fog must react to portal open state and sync to the threshold plane.");
                }
            }
        }

        private static FastVsHd2dLocalVolumetricFogProfile EnsureHd2dAutonomousP2LocalVolumetricFogProfile()
        {
            EnsureFolder("Assets/Settings");
            var profile = AssetDatabase.LoadAssetAtPath<FastVsHd2dLocalVolumetricFogProfile>(Hd2dAutonomousP2LocalVolumetricFogProfilePath);
            if (profile == null)
            {
                profile = ScriptableObject.CreateInstance<FastVsHd2dLocalVolumetricFogProfile>();
                AssetDatabase.CreateAsset(profile, Hd2dAutonomousP2LocalVolumetricFogProfilePath);
            }

            profile.ConfigureForReview(
                0.17f,
                0.12f,
                0.00f,
                0.24f,
                1.35f,
                0.42f,
                0.18f,
                4.2f,
                0.34f,
                0.20f,
                new Color(0.66f, 0.76f, 0.84f, 0.30f),
                new Color(0.58f, 0.72f, 0.78f, 0.28f),
                new Color(0.70f, 0.92f, 1.00f, 0.38f),
                new Vector3(4.90f, 1.35f, 3.20f),
                new Vector3(4.40f, 0.58f, 1.45f),
                new Vector3(2.15f, 1.65f, 0.32f),
                true,
                true,
                true,
                true,
                false,
                "Conservative P2-65 local mist data prep using lightweight transparent slice volumes. Tom should decide whether to buy a legitimate Render Graph-native volumetric solution for final interior/portal fog, then tune density, portal glow, and doorway falloff against the approved camera and grade.");
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static Material EnsureHd2dAutonomousP2LocalVolumetricFogMaterial()
        {
            EnsureFolder(MaterialDirectory);
            var material = AssetDatabase.LoadAssetAtPath<Material>(Hd2dAutonomousP2LocalVolumetricFogMaterialPath);
            var shader = Shader.Find("Anemora/FastVS/LocalVolumetricFogSlice");
            if (shader == null)
            {
                throw new InvalidOperationException("P2-65 local volumetric fog shader is missing.");
            }

            if (material == null)
            {
                material = new Material(shader)
                {
                    name = "FastVS_House_hd2d_p2_local_volumetric_fog_slice",
                    renderQueue = 3035
                };
                AssetDatabase.CreateAsset(material, Hd2dAutonomousP2LocalVolumetricFogMaterialPath);
            }

            material.shader = shader;
            material.SetColor("_FogColor", new Color(0.70f, 0.92f, 1.00f, 0.48f));
            material.SetFloat("_FogDensity", 0.25f);
            material.SetFloat("_EdgeFeather", 0.42f);
            material.SetFloat("_NoiseStrength", 0.18f);
            material.SetFloat("_NoiseScale", 4.2f);
            material.SetFloat("_HeightFade", 0.34f);
            material.SetFloat("_PortalGlow", 0.0f);
            material.SetFloat("_TimeOffset", 0.0f);
            material.renderQueue = 3035;
            material.enableInstancing = true;
            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", false);
            material.SetShaderPassEnabled("ShadowCaster", false);
            ApplyMaterialRole(material, Hd2dAutonomousP2LocalVolumetricFogMaterialId, FastVsHd2dMaterialRole.OverlayGlow);
            EditorUtility.SetDirty(material);
            AssetDatabase.SaveAssets();
            return material;
        }

        private static void CreateHd2dAutonomousP2LocalVolumetricFogReviewSet(
            Transform currentInteriorRoot,
            Transform currentCentralPlazaRoot,
            Transform pastCentralPlazaRoot,
            Material material,
            FastVsHd2dLocalVolumetricFogProfile profile)
        {
            DestroyHd2dAutonomousP2LocalFogRoot(Hd2dAutonomousP2LocalVolumetricFogInteriorRootName);
            DestroyHd2dAutonomousP2LocalFogRoot(Hd2dAutonomousP2LocalVolumetricFogCurrentPlazaRootName);
            DestroyHd2dAutonomousP2LocalFogRoot(Hd2dAutonomousP2LocalVolumetricFogPastPlazaRootName);

            if (currentInteriorRoot != null)
            {
                var root = new GameObject(Hd2dAutonomousP2LocalVolumetricFogInteriorRootName);
                root.transform.SetParent(currentInteriorRoot, false);
                root.transform.localPosition = Vector3.zero;
                CreateHd2dAutonomousP2LocalFogVolume(
                    root.transform,
                    "P2_65_CurrentInterior_BoundedRoomMist",
                    FastVsHd2dLocalVolumetricFogKind.InteriorRoom,
                    profile,
                    material,
                    HouseInteriorCenter + new Vector3(-0.26f, 0.82f, -0.52f),
                    profile.InteriorBoundsForReview,
                    Quaternion.Euler(0f, -3f, 0f),
                    false,
                    false,
                    Vector3.zero);
            }

            if (currentCentralPlazaRoot != null)
            {
                var root = new GameObject(Hd2dAutonomousP2LocalVolumetricFogCurrentPlazaRootName);
                root.transform.SetParent(currentCentralPlazaRoot, false);
                root.transform.localPosition = Vector3.zero;
                CreateHd2dAutonomousP2LocalFogVolume(
                    root.transform,
                    "P2_65_CurrentPlaza_LowValleyMist",
                    FastVsHd2dLocalVolumetricFogKind.LowValley,
                    profile,
                    material,
                    CentralPlazaVsCenter + new Vector3(0.18f, 0.40f, 4.34f),
                    profile.LowValleyBoundsForReview,
                    Quaternion.Euler(0f, -4f, 0f),
                    false,
                    false,
                    Vector3.zero);
                CreateHd2dAutonomousP2LocalFogVolume(
                    root.transform,
                    "P2_65_CurrentPlaza_PortalThresholdMist",
                    FastVsHd2dLocalVolumetricFogKind.PortalThreshold,
                    profile,
                    material,
                    CentralPlazaVsCenter + new Vector3(-0.28f, 1.05f, 3.82f),
                    profile.PortalBoundsForReview,
                    Quaternion.identity,
                    true,
                    true,
                    new Vector3(0f, 0.08f, 0f));
            }

            if (pastCentralPlazaRoot != null)
            {
                var root = new GameObject(Hd2dAutonomousP2LocalVolumetricFogPastPlazaRootName);
                root.transform.SetParent(pastCentralPlazaRoot, false);
                root.transform.localPosition = Vector3.zero;
                CreateHd2dAutonomousP2LocalFogVolume(
                    root.transform,
                    "P2_65_PastPlaza_PortalThresholdMist",
                    FastVsHd2dLocalVolumetricFogKind.PortalThreshold,
                    profile,
                    material,
                    CentralPlazaVsCenter + new Vector3(-0.28f, 1.05f, 3.82f),
                    profile.PortalBoundsForReview,
                    Quaternion.identity,
                    true,
                    true,
                    new Vector3(0f, 0.08f, 0f));
            }
        }

        private static FastVsHd2dLocalVolumetricFogVolume CreateHd2dAutonomousP2LocalFogVolume(
            Transform parent,
            string name,
            FastVsHd2dLocalVolumetricFogKind kind,
            FastVsHd2dLocalVolumetricFogProfile profile,
            Material material,
            Vector3 localCenter,
            Vector3 bounds,
            Quaternion localRotation,
            bool reactToPortalOpen,
            bool syncToPortalPlane,
            Vector3 portalLocalOffset)
        {
            var root = new GameObject(name);
            root.transform.SetParent(parent, false);
            root.transform.localPosition = localCenter;
            root.transform.localRotation = localRotation;
            root.transform.localScale = Vector3.one;

            var renderers = new List<Renderer>();
            if (kind == FastVsHd2dLocalVolumetricFogKind.PortalThreshold)
            {
                for (var i = 0; i < 5; i++)
                {
                    var t = i / 4f;
                    var z = Mathf.Lerp(-bounds.z * 0.5f, bounds.z * 0.5f, t);
                    renderers.Add(CreateHd2dAutonomousP2LocalFogSlice(
                        root.transform,
                        $"{name}_PlaneZ{i + 1}",
                        material,
                        new Vector3(0f, 0f, z),
                        Quaternion.identity,
                        new Vector2(bounds.x, bounds.y)));
                }
            }
            else
            {
                for (var i = 0; i < 3; i++)
                {
                    var t = i / 2f;
                    var y = Mathf.Lerp(-bounds.y * 0.22f, bounds.y * 0.22f, t);
                    renderers.Add(CreateHd2dAutonomousP2LocalFogSlice(
                        root.transform,
                        $"{name}_Horizontal{i + 1}",
                        material,
                        new Vector3(0f, y, 0f),
                        Quaternion.Euler(90f, 0f, 0f),
                        new Vector2(bounds.x, bounds.z)));
                }

                renderers.Add(CreateHd2dAutonomousP2LocalFogSlice(
                    root.transform,
                    $"{name}_VerticalZ",
                    material,
                    Vector3.zero,
                    Quaternion.identity,
                    new Vector2(bounds.x, bounds.y)));
                renderers.Add(CreateHd2dAutonomousP2LocalFogSlice(
                    root.transform,
                    $"{name}_VerticalX",
                    material,
                    Vector3.zero,
                    Quaternion.Euler(0f, 90f, 0f),
                    new Vector2(bounds.z, bounds.y)));
            }

            var driver = root.AddComponent<FastVsHd2dLocalVolumetricFogVolume>();
            driver.ConfigureForReview(
                profile,
                kind,
                renderers.ToArray(),
                reactToPortalOpen,
                syncToPortalPlane,
                localCenter,
                portalLocalOffset,
                1f);
            EditorUtility.SetDirty(root);
            EditorUtility.SetDirty(driver);
            return driver;
        }

        private static Renderer CreateHd2dAutonomousP2LocalFogSlice(
            Transform parent,
            string name,
            Material material,
            Vector3 localPosition,
            Quaternion localRotation,
            Vector2 size)
        {
            var slice = GameObject.CreatePrimitive(PrimitiveType.Quad);
            slice.name = name;
            slice.transform.SetParent(parent, false);
            slice.transform.localPosition = localPosition;
            slice.transform.localRotation = localRotation;
            slice.transform.localScale = new Vector3(size.x, size.y, 1f);
            var collider = slice.GetComponent<Collider>();
            if (collider != null)
            {
                UnityEngine.Object.DestroyImmediate(collider);
            }

            var renderer = slice.GetComponent<Renderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.lightProbeUsage = LightProbeUsage.Off;
            renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            EditorUtility.SetDirty(slice);
            EditorUtility.SetDirty(renderer);
            return renderer;
        }

        private static void DestroyHd2dAutonomousP2LocalFogRoot(string rootName)
        {
            var root = FindSceneObjectIncludingInactive(rootName);
            if (root != null)
            {
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static void CaptureHd2dAutonomousP2LocalVolumetricFogInteriorShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName,
            string label,
            float density,
            ICollection<string> shotRows)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.Interior);
            controller.ForcePlayerCurrentLocalForReview(HouseInteriorCenter + new Vector3(-1.55f, 0.02f, -1.26f));
            controller.ClosePortal();
            guide.ApplyActiveTimeIsolationForReview();
            ApplyHd2dAutonomousP2LocalVolumetricFogNow();

            var previousMask = camera.cullingMask;
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            try
            {
                camera.cullingMask = currentBit | playerBit;
                HideHd2dAutonomousP2WaterReviewSetsForFogCapture();
                PositionCloseReviewCamera(
                    camera,
                    controller.CurrentSpaceRootForReview.TransformPoint(HouseInteriorCenter + new Vector3(-0.24f, 0.76f, -0.58f)),
                    new Vector3(0.94f, 1.16f, -2.18f),
                    new Vector3(0.02f, 0.12f, 0.10f));
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            }
            finally
            {
                camera.cullingMask = previousMask;
                RestoreHd2dAutonomousP2WaterReviewSetsAfterFogCapture();
                guide.ApplyActiveTimeIsolationForReview();
            }

            ValidateScreenshotOutputExists(outputDirectory, fileName);
            shotRows.Add($"| `{fileName}` | {label} | InteriorRoom | {density:0.###} |");
        }

        private static void CaptureHd2dAutonomousP2LocalVolumetricFogPortalShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName,
            bool openPortal,
            string label,
            float density,
            ICollection<string> shotRows)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.34f, 0.02f, 3.26f));
            guide.ApplyActiveTimeIsolationForReview();

            var previousMask = camera.cullingMask;
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var otherBit = 1 << Mathf.Clamp(controller.OtherTimeSpaceRenderLayerForReview, 0, 31);
            var portalBit = 1 << Mathf.Clamp(controller.PortalFrameRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            var fallbackPortalLocal = CentralPlazaVsCenter + new Vector3(-0.28f, 1.05f, 3.82f);
            try
            {
                camera.cullingMask = currentBit | portalBit | playerBit | (openPortal ? otherBit : 0);
                HideHd2dAutonomousP2WaterReviewSetsForFogCapture();
                PositionCloseReviewCamera(
                    camera,
                    controller.CurrentSpaceRootForReview.TransformPoint(fallbackPortalLocal),
                    new Vector3(1.08f, 1.72f, -3.28f),
                    new Vector3(0f, 0.14f, 0.12f));

                controller.ClosePortal();
                if (openPortal && !controller.TryOpenPortalForTests(DragStart, DragEnd))
                {
                    throw new InvalidOperationException("Fast VS autonomous P2-65 local fog capture failed: review portal could not be opened.");
                }

                controller.RenderPortalAperturesForReview();
                ApplyHd2dAutonomousP2LocalVolumetricFogNow();

                var portalLocal = controller.HasPortalPair ? controller.PortalLocalCenterForReview + new Vector3(0f, 0.80f, 0f) : fallbackPortalLocal;
                PositionCloseReviewCamera(
                    camera,
                    controller.CurrentSpaceRootForReview.TransformPoint(portalLocal),
                    new Vector3(1.08f, 1.72f, -3.28f),
                    new Vector3(0f, 0.14f, 0.12f));
                controller.RenderPortalAperturesForReview();
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            }
            finally
            {
                camera.cullingMask = previousMask;
                RestoreHd2dAutonomousP2WaterReviewSetsAfterFogCapture();
                guide.ApplyActiveTimeIsolationForReview();
            }

            ValidateScreenshotOutputExists(outputDirectory, fileName);
            shotRows.Add($"| `{fileName}` | {label} | PortalThreshold | {density:0.###} |");
        }

        private static void CaptureHd2dAutonomousP2LocalVolumetricFogValleyShot(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsVisualDirectionGuide guide,
            Camera camera,
            string outputDirectory,
            string fileName,
            string label,
            float density,
            ICollection<string> shotRows)
        {
            visibility.SetActiveAreaForReview(FastVsHouseArea.CentralPlaza);
            controller.ForcePlayerCurrentLocalForReview(CentralPlazaVsCenter + new Vector3(-0.20f, 0.02f, 3.42f));
            controller.ClosePortal();
            guide.ApplyActiveTimeIsolationForReview();
            ApplyHd2dAutonomousP2LocalVolumetricFogNow();

            var previousMask = camera.cullingMask;
            var currentBit = 1 << Mathf.Clamp(controller.CurrentSpaceRenderLayerForReview, 0, 31);
            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            try
            {
                camera.cullingMask = currentBit | playerBit;
                HideHd2dAutonomousP2WaterReviewSetsForFogCapture();
                PositionCloseReviewCamera(
                    camera,
                    controller.CurrentSpaceRootForReview.TransformPoint(CentralPlazaVsCenter + new Vector3(0.18f, 0.48f, 4.34f)),
                    new Vector3(0.72f, 1.36f, -2.62f),
                    new Vector3(0.00f, 0.10f, 0.12f));
                WarmUpCameraRender(camera);
                SaveCameraPng(camera, Path.Combine(outputDirectory, fileName));
            }
            finally
            {
                camera.cullingMask = previousMask;
                RestoreHd2dAutonomousP2WaterReviewSetsAfterFogCapture();
                guide.ApplyActiveTimeIsolationForReview();
            }

            ValidateScreenshotOutputExists(outputDirectory, fileName);
            shotRows.Add($"| `{fileName}` | {label} | LowValley | {density:0.###} |");
        }

        private static void HideHd2dAutonomousP2WaterReviewSetsForFogCapture()
        {
            SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(false);
            SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(false);
            SetHd2dAutonomousP2ToonWaterMotionReviewVisible(false);
            SetHd2dAutonomousP2CausticsReviewVisible(false);
            SetHd2dAutonomousP2WaterReflectionReviewVisible(false);
        }

        private static void RestoreHd2dAutonomousP2WaterReviewSetsAfterFogCapture()
        {
            SetHd2dAutonomousP2DirectionalWaterFlowReviewTroughVisible(true);
            SetHd2dAutonomousP2FakeRefractionWaterSurfacesVisible(true);
            SetHd2dAutonomousP2ToonWaterMotionReviewVisible(true);
            SetHd2dAutonomousP2CausticsReviewVisible(true);
            SetHd2dAutonomousP2WaterReflectionReviewVisible(true);
        }

        private static void SetHd2dAutonomousP2LocalVolumetricFogVisible(bool visible)
        {
            foreach (var volume in FindHd2dAutonomousP2LocalVolumetricFogVolumes())
            {
                if (volume == null)
                {
                    continue;
                }

                volume.SetReviewVisibleForReview(visible);
                EditorUtility.SetDirty(volume);
            }
        }

        private static void SetHd2dAutonomousP2LocalVolumetricFogKindVisible(FastVsHd2dLocalVolumetricFogKind kind, bool visible)
        {
            foreach (var volume in FindHd2dAutonomousP2LocalVolumetricFogVolumes())
            {
                if (volume == null || volume.FogKindForReview != kind)
                {
                    continue;
                }

                volume.SetReviewVisibleForReview(visible);
                EditorUtility.SetDirty(volume);
            }
        }

        private static void SetHd2dAutonomousP2LocalVolumetricFogKindMultiplier(FastVsHd2dLocalVolumetricFogKind kind, float multiplier)
        {
            foreach (var volume in FindHd2dAutonomousP2LocalVolumetricFogVolumes())
            {
                if (volume == null || volume.FogKindForReview != kind)
                {
                    continue;
                }

                volume.SetReviewAlphaMultiplierForReview(multiplier);
                EditorUtility.SetDirty(volume);
            }
        }

        private static void SetHd2dAutonomousP2LocalVolumetricFogAllMultipliers(float multiplier)
        {
            foreach (var volume in FindHd2dAutonomousP2LocalVolumetricFogVolumes())
            {
                if (volume == null)
                {
                    continue;
                }

                volume.SetReviewAlphaMultiplierForReview(multiplier);
                EditorUtility.SetDirty(volume);
            }
        }

        private static void ApplyHd2dAutonomousP2LocalVolumetricFogNow()
        {
            foreach (var volume in FindHd2dAutonomousP2LocalVolumetricFogVolumes())
            {
                volume?.ApplyNowForReview();
            }
        }

        private static int CountHd2dAutonomousP2LocalVolumetricFogVolumes(FastVsHd2dLocalVolumetricFogKind kind)
        {
            var count = 0;
            foreach (var volume in FindHd2dAutonomousP2LocalVolumetricFogVolumes())
            {
                if (volume != null && volume.FogKindForReview == kind)
                {
                    count++;
                }
            }

            return count;
        }

        private static FastVsHd2dLocalVolumetricFogVolume[] FindHd2dAutonomousP2LocalVolumetricFogVolumes()
        {
            return UnityEngine.Object.FindObjectsByType<FastVsHd2dLocalVolumetricFogVolume>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        }

        private static void WriteHd2dAutonomousP2LocalVolumetricFogReviewReport(
            string outputDirectory,
            IReadOnlyList<string> screenshotFiles,
            IReadOnlyList<string> shotRows,
            FastVsHd2dLocalVolumetricFogProfile profile,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics interiorDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics portalDiff,
            Hd2dAutonomousP2DirectionalWaterFlowDiffMetrics portalStrongDiff)
        {
            var lines = new List<string>
            {
                "# P2-65 Localized Volumetric Fog Volumes Review",
                string.Empty,
                "- Scope: NEEDS-TOM conservative local fog/mist control plane for interiors, a low route pocket, and the Time Window portal threshold.",
                "- Implementation note: this pass does not import or purchase a third-party volumetric asset. It uses bounded transparent local-slice volumes as A/B data and leaves the legitimate Render Graph-native volumetric package decision to Tom.",
                "- Recommendation: " + profile.RecommendationForReview,
                string.Empty,
                "| Setting | Value |",
                "|---|---|",
                $"| Profile | `{Hd2dAutonomousP2LocalVolumetricFogProfilePath}` |",
                $"| Material | `{Hd2dAutonomousP2LocalVolumetricFogMaterialPath}` |",
                $"| Shader | `{Hd2dAutonomousP2LocalVolumetricFogShaderPath}` |",
                $"| Needs Tom approval | {FormatBool(profile.NeedsTomApprovalForReview)} |",
                $"| Final approved | {FormatBool(profile.FinalLocalFogApprovedForReview)} |",
                $"| Render Graph-native package deferred | {FormatBool(profile.RenderGraphNativePackageDeferredForReview)} |",
                $"| Legitimate volumetric asset decision required | {FormatBool(profile.LegitimateVolumetricAssetDecisionRequiredForReview)} |",
                $"| Interior / low valley / portal densities | {profile.InteriorDensityForReview:0.###} / {profile.LowValleyDensityForReview:0.###} / {profile.PortalClosedDensityForReview:0.###}->{profile.PortalOpenDensityForReview:0.###} |",
                $"| Edge feather / noise / height fade / portal glow | {profile.EdgeFeatherForReview:0.###} / {profile.NoiseStrengthForReview:0.###}@{profile.NoiseScaleForReview:0.###} / {profile.HeightFadeForReview:0.###} / {profile.PortalGlowForReview:0.###} |",
                $"| Bounds interior / valley / portal | {profile.InteriorBoundsForReview} / {profile.LowValleyBoundsForReview} / {profile.PortalBoundsForReview} |",
                $"| Volume counts interior / valley / portal | {CountHd2dAutonomousP2LocalVolumetricFogVolumes(FastVsHd2dLocalVolumetricFogKind.InteriorRoom)} / {CountHd2dAutonomousP2LocalVolumetricFogVolumes(FastVsHd2dLocalVolumetricFogKind.LowValley)} / {CountHd2dAutonomousP2LocalVolumetricFogVolumes(FastVsHd2dLocalVolumetricFogKind.PortalThreshold)} |",
                string.Empty,
                "| A/B Evidence | Samples | Changed px > 4 RGB | Changed % | Mean RGB delta |",
                "|---|---:|---:|---:|---:|",
                interiorDiff.ToReportRow("Interior fog disabled vs bounded mist"),
                portalDiff.ToReportRow("Portal closed vs open luminous haze"),
                portalStrongDiff.ToReportRow("Portal conservative vs stronger option"),
                string.Empty,
                "| Screenshot | Label | Fog kind | Density |",
                "|---|---|---|---:|"
            };
            lines.AddRange(shotRows);
            lines.AddRange(new[]
            {
                string.Empty,
                "| Screenshot | Purpose |",
                "|---|---|"
            });
            for (var i = 0; i < screenshotFiles.Count; i++)
            {
                var file = screenshotFiles[i];
                ValidateScreenshotOutputExists(outputDirectory, file);
                lines.Add($"| `{file}` | P2-65 local volumetric fog capture {i + 1}. |");
            }

            File.WriteAllText(Path.Combine(outputDirectory, "localized_volumetric_fog_volumes_review.md"), string.Join(Environment.NewLine, lines));
        }
    }
}
