using Anemora.TimeManagement.Portal;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Anemora.EditorTools
{
    public static class AnemoraE0Setup
    {
        private const string SettingsDirectory = "Assets/Settings";
        private const string PipelineAssetPath = SettingsDirectory + "/UniversalRenderPipeline.asset";
        private const string RendererDataPath = SettingsDirectory + "/UniversalRenderPipeline_Renderer.asset";
        private const string PortalStencilFeatureName = "PortalStencilFeature";

        public static void Run()
        {
            AssetDatabase.StartAssetEditing();
            try
            {
                EnsureFolder(SettingsDirectory);
                MoveAssetIfPresent("Assets/UniversalRenderPipelineGlobalSettings.asset", SettingsDirectory + "/UniversalRenderPipelineGlobalSettings.asset");
                MoveAssetIfPresent("Assets/DefaultVolumeProfile.asset", SettingsDirectory + "/DefaultVolumeProfile.asset");
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }

            var rendererData = EnsureRendererData();
            var pipelineAsset = EnsurePipelineAsset(rendererData);

            EnsurePortalStencilFeature(rendererData);
            AssignPipeline(pipelineAsset);

            EditorUtility.SetDirty(rendererData);
            EditorUtility.SetDirty(pipelineAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Anemora E0 URP setup complete.");
        }

        private static UniversalRendererData EnsureRendererData()
        {
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(RendererDataPath);
            if (rendererData == null)
            {
                rendererData = ScriptableObject.CreateInstance<UniversalRendererData>();
                rendererData.name = "UniversalRenderPipeline_Renderer";
                rendererData.renderingMode = RenderingMode.ForwardPlus;
                ResourceReloader.ReloadAllNullIn(rendererData, UniversalRenderPipelineAsset.packagePath);
                AssetDatabase.CreateAsset(rendererData, RendererDataPath);
            }

            rendererData.renderingMode = RenderingMode.ForwardPlus;
            EditorUtility.SetDirty(rendererData);
            return rendererData;
        }

        private static UniversalRenderPipelineAsset EnsurePipelineAsset(UniversalRendererData rendererData)
        {
            var pipelineAsset = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(PipelineAssetPath);
            if (pipelineAsset == null)
            {
                pipelineAsset = UniversalRenderPipelineAsset.Create(rendererData);
                pipelineAsset.name = "UniversalRenderPipeline";
                AssetDatabase.CreateAsset(pipelineAsset, PipelineAssetPath);
            }

            var serialized = new SerializedObject(pipelineAsset);
            var rendererList = serialized.FindProperty("m_RendererDataList");
            rendererList.arraySize = 1;
            rendererList.GetArrayElementAtIndex(0).objectReferenceValue = rendererData;
            serialized.FindProperty("m_DefaultRendererIndex").intValue = 0;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            return pipelineAsset;
        }

        private static void EnsurePortalStencilFeature(UniversalRendererData rendererData)
        {
            foreach (var feature in rendererData.rendererFeatures)
            {
                if (feature is PortalStencilFeature)
                {
                    return;
                }
            }

            var portalFeature = ScriptableObject.CreateInstance<PortalStencilFeature>();
            portalFeature.name = PortalStencilFeatureName;
            AssetDatabase.AddObjectToAsset(portalFeature, rendererData);
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(portalFeature, out _, out long localId);

            var serialized = new SerializedObject(rendererData);
            var features = serialized.FindProperty("m_RendererFeatures");
            var featureMap = serialized.FindProperty("m_RendererFeatureMap");

            features.arraySize++;
            features.GetArrayElementAtIndex(features.arraySize - 1).objectReferenceValue = portalFeature;

            featureMap.arraySize++;
            featureMap.GetArrayElementAtIndex(featureMap.arraySize - 1).longValue = localId;

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(portalFeature);
        }

        private static void AssignPipeline(UniversalRenderPipelineAsset pipelineAsset)
        {
            GraphicsSettings.defaultRenderPipeline = pipelineAsset;

            var originalQualityLevel = QualitySettings.GetQualityLevel();
            for (var index = 0; index < QualitySettings.names.Length; index++)
            {
                QualitySettings.SetQualityLevel(index, false);
                QualitySettings.renderPipeline = pipelineAsset;
            }

            QualitySettings.SetQualityLevel(originalQualityLevel, false);
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            AssetDatabase.CreateFolder("Assets", "Settings");
        }

        private static void MoveAssetIfPresent(string sourcePath, string destinationPath)
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(sourcePath) == null || AssetDatabase.LoadAssetAtPath<Object>(destinationPath) != null)
            {
                return;
            }

            var error = AssetDatabase.MoveAsset(sourcePath, destinationPath);
            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogWarning($"Could not move {sourcePath} to {destinationPath}: {error}");
            }
        }
    }
}
