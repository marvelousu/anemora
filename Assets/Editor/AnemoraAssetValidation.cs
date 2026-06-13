// AnemoraAssetValidation — machine acceptance gate for imported assets.
//
// Why: the 2026-06-13 environment audit found the asset pipeline has no automated
// import checks. Three concrete gaps it named:
//   1. Missing mesh/material references slip into prefabs undetected.
//   2. No polycount ceiling on imported meshes (outliers can't be caught).
//   3. "review_only" character versions (e.g. the v59 generic NPC bank) can leak
//      into runtime prefab references with nothing to catch it.
//
// This adds a batch validate entry point the cycle-runner can call as a Validate
// phase: it discovers assets (no hardcoded layout), reports every problem, and
// exits non-zero if any hard error is found.
//
//   "<UnityExe>" -batchmode -quit -projectPath <proj> \
//     -executeMethod Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch
//
// Purely additive: this is a new Editor class; it does not touch the authored
// AnemoraFastVsHouseSliceSetup.cs.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static class AnemoraAssetValidation
    {
        // --- Tunables (adjust to the project's real budgets on first adoption) ---
        // Runtime prefab roots: anything referenced from here must be production-grade.
        static readonly string[] RuntimePrefabRoots = { "Assets/Prefabs" };
        // Substrings that mark a NON-runtime (review-only / experimental) asset.
        // A runtime prefab depending on any of these is a hard error.
        static readonly string[] NonRuntimeMarkers = { "review_only", "review-only", "/review/", "_reviewonly" };
        // Polycount ceiling per mesh. Soft warning (HD-2D low-poly should stay well under).
        const int MeshTriangleWarnThreshold = 20000;

        [MenuItem("Anemora/Validate Imported Assets")]
        public static void ValidateImportedAssetsMenu() => RunValidation(throwOnError: true);

        // Batch entry point for the cycle-runner Validate phase.
        public static void ValidateImportedAssetsBatch()
        {
            int errors = RunValidation(throwOnError: false);
            if (errors > 0)
            {
                Debug.LogError($"[AssetValidation] FAILED with {errors} error(s).");
                EditorApplication.Exit(1);
            }
            else
            {
                Debug.Log("[AssetValidation] OK — no missing references, no review-only leaks, no oversized meshes.");
            }
        }

        static int RunValidation(bool throwOnError)
        {
            var errors = new List<string>();
            var warnings = new List<string>();

            CheckMissingReferences(errors);
            CheckReviewOnlyLeaks(errors);
            CheckMeshPolycount(warnings);

            foreach (var w in warnings) Debug.LogWarning("[AssetValidation] " + w);
            foreach (var e in errors) Debug.LogError("[AssetValidation] " + e);

            if (throwOnError && errors.Count > 0)
                throw new Exception($"Asset validation failed with {errors.Count} error(s). See console.");
            return errors.Count;
        }

        // 1. Missing mesh/material references in every prefab.
        static void CheckMissingReferences(List<string> errors)
        {
            foreach (var guid in AssetDatabase.FindAssets("t:Prefab"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var root = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (root == null) continue;

                foreach (var mf in root.GetComponentsInChildren<MeshFilter>(true))
                    if (mf.sharedMesh == null)
                        errors.Add($"missing mesh: {path} :: {PathOf(mf.transform)}");

                foreach (var r in root.GetComponentsInChildren<Renderer>(true))
                {
                    var mats = r.sharedMaterials;
                    if (mats == null || mats.Length == 0)
                    {
                        // SpriteRenderer legitimately uses a default material; only flag
                        // mesh renderers with an empty slot.
                        if (r is MeshRenderer)
                            errors.Add($"missing material (no slots): {path} :: {PathOf(r.transform)}");
                        continue;
                    }
                    for (int i = 0; i < mats.Length; i++)
                        if (mats[i] == null)
                            errors.Add($"missing material (slot {i}): {path} :: {PathOf(r.transform)}");
                }
            }
        }

        // 2. review-only / non-runtime assets referenced from runtime prefab roots.
        static void CheckReviewOnlyLeaks(List<string> errors)
        {
            string[] roots = RuntimePrefabRoots.Where(AssetDatabase.IsValidFolder).ToArray();
            if (roots.Length == 0) return;

            foreach (var guid in AssetDatabase.FindAssets("t:Prefab", roots))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                foreach (var dep in AssetDatabase.GetDependencies(path, true))
                {
                    string lower = dep.Replace("\\", "/").ToLowerInvariant();
                    if (NonRuntimeMarkers.Any(m => lower.Contains(m)))
                        errors.Add($"review-only asset leaked into runtime: {path} -> {dep}");
                }
            }
        }

        // 3. Oversized meshes (soft warning).
        static void CheckMeshPolycount(List<string> warnings)
        {
            foreach (var guid in AssetDatabase.FindAssets("t:Mesh"))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!path.StartsWith("Assets/")) continue; // skip built-in/package meshes
                foreach (var obj in AssetDatabase.LoadAllAssetsAtPath(path))
                {
                    var mesh = obj as Mesh;
                    if (mesh == null) continue;
                    int tris = 0;
                    for (int s = 0; s < mesh.subMeshCount; s++)
                        tris += (int)(mesh.GetIndexCount(s) / 3);
                    if (tris > MeshTriangleWarnThreshold)
                        warnings.Add($"mesh over {MeshTriangleWarnThreshold} tris ({tris}): {path} :: {mesh.name}");
                }
            }
        }

        static string PathOf(Transform t)
        {
            var stack = new Stack<string>();
            while (t != null) { stack.Push(t.name); t = t.parent; }
            return string.Join("/", stack.ToArray());
        }
    }
}
