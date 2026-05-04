using System.IO;
using UnityEditor;
using UnityEngine;

namespace Anemora.Editor
{
    public static class AnemoraZone1BuildingAssetSetup
    {
        private const string ModelRoot = "Assets/Art/Models/Zone1";
        private const string MaterialDir = ModelRoot + "/Materials";
        private const string AtlasPath = ModelRoot + "/Anemora_Zone1_Atlas_512.png";
        private const string MaterialPath = MaterialDir + "/Anemora_Zone1_Atlas_URP.mat";
        private const string PrefabRoot = "Assets/Prefabs/Zone1";

        [MenuItem("Anemora/Assets/Apply Zone1 Building Import")]
        public static void ApplyZone1BuildingImport()
        {
            EnsureFolder("Assets/Art", "Models");
            EnsureFolder("Assets/Art/Models", "Zone1");
            EnsureFolder(ModelRoot, "Materials");
            EnsureFolder("Assets/Prefabs", "Zone1");

            AssetDatabase.ImportAsset(ModelRoot, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);

            var fallbackMaterial = LoadOrCreateFallbackMaterial();
            ApplyModelImporterSettings();
            CreatePrefabs(fallbackMaterial);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static Material LoadOrCreateFallbackMaterial()
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(AtlasPath);
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            var material = AssetDatabase.LoadAssetAtPath<Material>(MaterialPath);
            if (material == null)
            {
                material = new Material(shader) { name = "Anemora_Zone1_Atlas_URP_Fallback" };
                AssetDatabase.CreateAsset(material, MaterialPath);
            }

            material.shader = shader;
            SetTexture(material, "_BaseMap", texture);
            SetTexture(material, "_MainTex", texture);
            SetColor(material, "_BaseColor", Color.white);
            SetColor(material, "_Color", Color.white);
            SetFloat(material, "_Metallic", 0f);
            SetFloat(material, "_Glossiness", 0f);
            SetFloat(material, "_Smoothness", 0.15f);
            SetFloat(material, "_SpecularHighlights", 0f);
            SetFloat(material, "_EnvironmentReflections", 0f);
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void ApplyModelImporterSettings()
        {
            foreach (var path in FindFbxPaths())
            {
                if (AssetImporter.GetAtPath(path) is not ModelImporter importer)
                {
                    continue;
                }

                importer.globalScale = 1f;
                importer.useFileScale = false;
                importer.importCameras = false;
                importer.importLights = false;
                importer.importAnimation = false;
                importer.animationType = ModelImporterAnimationType.None;
                importer.importBlendShapes = false;
                importer.meshCompression = ModelImporterMeshCompression.Off;
                importer.isReadable = false;
                importer.optimizeMeshPolygons = true;
                importer.optimizeMeshVertices = true;
                importer.weldVertices = true;
                importer.importNormals = ModelImporterNormals.Import;
                importer.importTangents = ModelImporterTangents.CalculateMikk;
                importer.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
                importer.SaveAndReimport();
            }
        }

        private static void CreatePrefabs(Material fallbackMaterial)
        {
            foreach (var path in FindFbxPaths())
            {
                var model = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (model == null)
                {
                    continue;
                }

                if (PrefabUtility.InstantiatePrefab(model) is not GameObject instance)
                {
                    continue;
                }

                var prefabName = Path.GetFileNameWithoutExtension(path);
                var root = new GameObject(prefabName);
                instance.name = prefabName + "_Model";
                instance.transform.SetParent(root.transform, false);

                foreach (var renderer in instance.GetComponentsInChildren<Renderer>(true))
                {
                    var slots = renderer.sharedMaterials;
                    if (slots.Length == 0)
                    {
                        slots = new Material[1];
                    }

                    for (var i = 0; i < slots.Length; i++)
                    {
                        if (slots[i] == null)
                        {
                            slots[i] = fallbackMaterial;
                        }

                        ConfigureUrpMaterial(slots[i]);
                    }

                    renderer.sharedMaterials = slots;
                }

                NormalizeInstanceBounds(instance);

                var prefabPath = $"{PrefabRoot}/{prefabName}.prefab";
                PrefabUtility.SaveAsPrefabAsset(root, prefabPath);
                Object.DestroyImmediate(root);
            }
        }

        private static string[] FindFbxPaths()
        {
            var guids = AssetDatabase.FindAssets("t:Model", new[] { ModelRoot });
            var paths = new string[guids.Length];
            for (var i = 0; i < guids.Length; i++)
            {
                paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
            }

            return System.Array.FindAll(paths, path => path.EndsWith(".fbx", System.StringComparison.OrdinalIgnoreCase));
        }

        private static void NormalizeInstanceBounds(GameObject instance)
        {
            var renderers = instance.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0)
            {
                return;
            }

            var bounds = renderers[0].bounds;
            for (var i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            var offset = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            instance.transform.position -= offset;
        }

        private static void ConfigureUrpMaterial(Material material)
        {
            if (material == null)
            {
                return;
            }

            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader != null)
            {
                material.shader = shader;
            }

            SetFloat(material, "_Metallic", 0f);
            SetFloat(material, "_Glossiness", 0f);
            SetFloat(material, "_Smoothness", 0.15f);
            SetFloat(material, "_SpecularHighlights", 0f);
            SetFloat(material, "_EnvironmentReflections", 0f);
            EditorUtility.SetDirty(material);
        }

        private static void EnsureFolder(string parent, string child)
        {
            var path = $"{parent}/{child}";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, child);
            }
        }

        private static void SetTexture(Material material, string property, Texture texture)
        {
            if (material.HasProperty(property))
            {
                material.SetTexture(property, texture);
            }
        }

        private static void SetColor(Material material, string property, Color color)
        {
            if (material.HasProperty(property))
            {
                material.SetColor(property, color);
            }
        }

        private static void SetFloat(Material material, string property, float value)
        {
            if (material.HasProperty(property))
            {
                material.SetFloat(property, value);
            }
        }
    }
}
