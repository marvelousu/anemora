using System;
using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(menuName = "Anemora/Fast VS/HD-2D Modular Building Kit Profile")]
    public sealed class FastVsHd2dModularBuildingKitProfile : ScriptableObject
    {
        [Serializable]
        public sealed class ModuleDefinition
        {
            [SerializeField] private string moduleId = "wall_interior";
            [SerializeField] private string kind = "wall";
            [SerializeField] private Vector3 gridSize = Vector3.one;
            [SerializeField] private bool interiorIncluded;
            [SerializeField] private string prefabPath = string.Empty;

            public string ModuleIdForReview => moduleId;
            public string KindForReview => kind;
            public Vector3 GridSizeForReview => gridSize;
            public bool InteriorIncludedForReview => interiorIncluded;
            public string PrefabPathForReview => prefabPath;

            public ModuleDefinition(
                string configuredModuleId,
                string configuredKind,
                Vector3 configuredGridSize,
                bool configuredInteriorIncluded,
                string configuredPrefabPath)
            {
                moduleId = configuredModuleId;
                kind = configuredKind;
                gridSize = configuredGridSize;
                interiorIncluded = configuredInteriorIncluded;
                prefabPath = configuredPrefabPath;
            }
        }

        [Serializable]
        public sealed class BuildingRecipe
        {
            [SerializeField] private string buildingId = "narrow_shop";
            [SerializeField] private Vector2Int footprint = new Vector2Int(3, 2);
            [SerializeField] private int floors = 1;
            [SerializeField] private string roofStyle = "gable";
            [SerializeField] private int doorCount = 1;
            [SerializeField] private int windowCount = 2;
            [SerializeField] private string facadeVariant = "plaster_a";
            [SerializeField] private bool interiorWalls = true;

            public string BuildingIdForReview => buildingId;
            public Vector2Int FootprintForReview => footprint;
            public int FloorsForReview => floors;
            public string RoofStyleForReview => roofStyle;
            public int DoorCountForReview => doorCount;
            public int WindowCountForReview => windowCount;
            public string FacadeVariantForReview => facadeVariant;
            public bool InteriorWallsForReview => interiorWalls;
            public string SignatureForReview => $"{footprint.x}x{footprint.y}-{floors}-{roofStyle}-{doorCount}-{windowCount}-{facadeVariant}";

            public BuildingRecipe(
                string configuredBuildingId,
                Vector2Int configuredFootprint,
                int configuredFloors,
                string configuredRoofStyle,
                int configuredDoorCount,
                int configuredWindowCount,
                string configuredFacadeVariant,
                bool configuredInteriorWalls)
            {
                buildingId = configuredBuildingId;
                footprint = configuredFootprint;
                floors = Mathf.Max(1, configuredFloors);
                roofStyle = configuredRoofStyle;
                doorCount = Mathf.Max(0, configuredDoorCount);
                windowCount = Mathf.Max(0, configuredWindowCount);
                facadeVariant = configuredFacadeVariant;
                interiorWalls = configuredInteriorWalls;
            }
        }

        [SerializeField, Range(0.5f, 2f)] private float gridUnit = 1f;
        [SerializeField] private string sourceKitName = "Quaternius Medieval Village MegaKit";
        [SerializeField] private string sourceKitRoot = "Assets/Art/External/QuaterniusP0PropKit/MedievalVillageMegaKit";
        [SerializeField] private bool prefabVariantsEnabled = true;
        [SerializeField] private bool sourceBuildingFbxPresent;
        [SerializeField] private ModuleDefinition[] modules = Array.Empty<ModuleDefinition>();
        [SerializeField] private BuildingRecipe[] recipes = Array.Empty<BuildingRecipe>();

        public float GridUnitForReview => gridUnit;
        public string SourceKitNameForReview => sourceKitName;
        public string SourceKitRootForReview => sourceKitRoot;
        public bool PrefabVariantsEnabledForReview => prefabVariantsEnabled;
        public bool SourceBuildingFbxPresentForReview => sourceBuildingFbxPresent;
        public int ModuleCountForReview => modules != null ? modules.Length : 0;
        public int BuildingRecipeCountForReview => recipes != null ? recipes.Length : 0;

        public bool HasInteriorIncludedWallForReview
        {
            get
            {
                if (modules == null)
                {
                    return false;
                }

                for (var i = 0; i < modules.Length; i++)
                {
                    var module = modules[i];
                    if (module != null &&
                        module.InteriorIncludedForReview &&
                        string.Equals(module.KindForReview, "wall", StringComparison.Ordinal))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public ModuleDefinition GetModuleForReview(int index)
        {
            if (modules == null || index < 0 || index >= modules.Length)
            {
                return null;
            }

            return modules[index];
        }

        public BuildingRecipe GetRecipeForReview(int index)
        {
            if (recipes == null || index < 0 || index >= recipes.Length)
            {
                return null;
            }

            return recipes[index];
        }

        public void ConfigureForReview(
            float configuredGridUnit,
            string configuredSourceKitName,
            string configuredSourceKitRoot,
            bool configuredPrefabVariantsEnabled,
            bool configuredSourceBuildingFbxPresent,
            ModuleDefinition[] configuredModules,
            BuildingRecipe[] configuredRecipes)
        {
            gridUnit = Mathf.Clamp(configuredGridUnit, 0.5f, 2f);
            sourceKitName = configuredSourceKitName;
            sourceKitRoot = configuredSourceKitRoot;
            prefabVariantsEnabled = configuredPrefabVariantsEnabled;
            sourceBuildingFbxPresent = configuredSourceBuildingFbxPresent;
            modules = configuredModules ?? Array.Empty<ModuleDefinition>();
            recipes = configuredRecipes ?? Array.Empty<BuildingRecipe>();
        }
    }
}
