using System;
using Anemora.FastVS;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Anemora.EditorTools
{
    public static class AnemoraFastVsHd2dLightingTransitionAudit
    {
        [MenuItem("Tools/Anemora/Verify HD2D Lighting Transition V1")]
        public static void VerifyLightingTransitionV1()
        {
            EnsureHouseSliceSceneLoaded();

            var visibility = FindVisibility();
            var directorObject = GameObject.Find("FastVS_HD2D_LightingDirector");
            if (directorObject == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing FastVS_HD2D_LightingDirector.");
            }

            var director = directorObject.GetComponent<FastVsHouseLightingDirector>();
            if (director == null || !director.HasRequiredLightsForReview)
            {
                throw new InvalidOperationException("House slice validation failed: FastVS_HD2D_LightingDirector must reference the main, fill, rim, and library window lights.");
            }

            if (director.TransitionDurationForReview < 0.20f || director.TransitionDurationForReview > 1.20f)
            {
                throw new InvalidOperationException("House slice validation failed: lighting transition duration must stay within the review clamp range.");
            }

            var mainLight = GetLight("Directional Light");
            var warmFill = GetLight("FastVS_HD2D_WarmFillLight");
            var coolRim = GetLight("FastVS_HD2D_CoolRimLight");
            var libraryWindow = GetLight("FastVS_HD2D_LibraryWindowLight");
            if (mainLight == null || warmFill == null || coolRim == null || libraryWindow == null)
            {
                throw new InvalidOperationException("House slice validation failed: one or more HD-2D lighting foundation lights are missing.");
            }

            director.ApplyAreaForReview(FastVsHouseArea.Interior);
            if (director.LastAppliedAreaForReview != FastVsHouseArea.Interior ||
                director.TargetAreaForReview != FastVsHouseArea.Interior ||
                director.TransitionActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: interior apply must stay immediate for review validation.");
            }

            if (warmFill.type != LightType.Point || warmFill.shadows != LightShadows.None)
            {
                throw new InvalidOperationException("House slice validation failed: warm fill must remain a non-shadowing point light.");
            }

            if (coolRim.type != LightType.Directional || coolRim.shadows != LightShadows.None)
            {
                throw new InvalidOperationException("House slice validation failed: cool rim must remain a non-shadowing directional light.");
            }

            var interiorMainIntensity = mainLight.intensity;

            director.ApplyAreaForReview(FastVsHouseArea.Library);
            if (director.LastAppliedAreaForReview != FastVsHouseArea.Library ||
                director.TargetAreaForReview != FastVsHouseArea.Library ||
                director.TransitionActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: library apply must stay immediate for review validation.");
            }

            var libraryMainIntensity = mainLight.intensity;

            director.ApplyAreaForReview(FastVsHouseArea.Interior);
            director.BeginAreaTransitionForReview(FastVsHouseArea.Library);
            if (director.TargetAreaForReview != FastVsHouseArea.Library || !director.TransitionActiveForReview)
            {
                throw new InvalidOperationException("House slice validation failed: transition API must immediately expose the library as the active target.");
            }

            if (director.CurrentBlendForReview < 0f || director.CurrentBlendForReview > 1f)
            {
                throw new InvalidOperationException("House slice validation failed: transition blend must remain normalized.");
            }

            director.SampleTransitionForReview(FastVsHouseArea.Interior, FastVsHouseArea.Library, 0.5f);
            var midpointIntensity = mainLight.intensity;
            var lower = Mathf.Min(interiorMainIntensity, libraryMainIntensity);
            var upper = Mathf.Max(interiorMainIntensity, libraryMainIntensity);
            if (midpointIntensity <= lower || midpointIntensity >= upper)
            {
                throw new InvalidOperationException("House slice validation failed: sampled midpoint intensity must land between the interior and library profiles.");
            }

            if (!libraryWindow.enabled || libraryWindow.type != LightType.Spot)
            {
                throw new InvalidOperationException("House slice validation failed: the library window light must remain a spot light in the transition profile.");
            }

            if (libraryWindow.shadows != LightShadows.None)
            {
                throw new InvalidOperationException("House slice validation failed: the library window light must remain non-shadowing during transitions.");
            }

            director.ApplyAreaForReview(visibility.ActiveAreaForReview);

            Debug.Log("HD2D lighting transition audit passed.");
        }

        private static void EnsureHouseSliceSceneLoaded()
        {
            var activeScene = EditorSceneManager.GetActiveScene();
            if (!string.Equals(activeScene.path, AnemoraFastVsHouseSliceSetup.ScenePath, StringComparison.Ordinal))
            {
                EditorSceneManager.OpenScene(AnemoraFastVsHouseSliceSetup.ScenePath, OpenSceneMode.Single);
            }
        }

        private static FastVsHouseAreaVisibility FindVisibility()
        {
            var visibility = UnityEngine.Object.FindFirstObjectByType<FastVsHouseAreaVisibility>(FindObjectsInactive.Include);
            if (visibility == null)
            {
                throw new InvalidOperationException("House slice validation failed: missing FastVsHouseAreaVisibility.");
            }

            return visibility;
        }

        private static Light GetLight(string objectName)
        {
            var target = GameObject.Find(objectName);
            return target != null ? target.GetComponent<Light>() : null;
        }
    }
}
