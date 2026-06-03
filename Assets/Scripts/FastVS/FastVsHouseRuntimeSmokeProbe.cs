using System;
using System.Collections;
using Anemora.TimeManagement;
using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsHouseRuntimeSmokeProbe : MonoBehaviour
    {
        private const string EnableArgument = "--anemora-house-slice-smoke";
        private const string PassMarker = "ANEMORA_HOUSE_SLICE_SMOKE_PASS";
        private const string FailMarker = "ANEMORA_HOUSE_SLICE_SMOKE_FAIL";

        private IEnumerator Start()
        {
            if (!ShouldRun())
            {
                yield break;
            }

            yield return null;
            yield return new WaitForSeconds(3.0f);

            try
            {
                RunChecks();
                Debug.Log($"{PassMarker}: stable startup framing, MiaInterior and AriaInterior door travel, and indoor character activation verified.");
                Application.Quit(0);
            }
            catch (Exception exception)
            {
                Debug.LogError($"{FailMarker}: {exception}");
                Application.Quit(31);
            }
        }

        private static bool ShouldRun()
        {
            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length; i++)
            {
                if (string.Equals(args[i], EnableArgument, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static void RunChecks()
        {
            var controller = RequireObject<TimeWindowPairedSpacePortalController>("paired space controller");
            var visibility = RequireObject<FastVsHouseAreaVisibility>("area visibility");

            VerifyStartupFraming(controller, visibility);

            VerifyTravel(
                controller,
                visibility,
                RequireTransition("FastVS_DoorTransition_MiaHouse_To_MiaInterior"),
                FastVsHouseArea.MiaHouse,
                FastVsHouseArea.MiaInterior,
                "Mia house to Mia interior");
            RequireActiveRenderer("FastVS_SpriteCharacter_Mia");

            VerifyTravel(
                controller,
                visibility,
                RequireTransition("FastVS_DoorTransition_MiaInterior_To_MiaHouse"),
                FastVsHouseArea.MiaInterior,
                FastVsHouseArea.MiaHouse,
                "Mia interior to Mia house");

            VerifyTravel(
                controller,
                visibility,
                RequireTransition("FastVS_DoorTransition_AriaStreet_To_AriaInterior"),
                FastVsHouseArea.AriaStreet,
                FastVsHouseArea.AriaInterior,
                "Aria street to Aria interior");
            RequireActiveRenderer("FastVS_SpriteCharacter_Karla");
            RequireActiveRenderer("FastVS_SpriteCharacter_Aria");

            VerifyTravel(
                controller,
                visibility,
                RequireTransition("FastVS_DoorTransition_AriaInterior_To_AriaStreet"),
                FastVsHouseArea.AriaInterior,
                FastVsHouseArea.AriaStreet,
                "Aria interior to Aria street");
        }

        private static void VerifyStartupFraming(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility)
        {
            if (visibility.ActiveAreaForReview != FastVsHouseArea.CentralPlaza)
            {
                throw new InvalidOperationException($"Startup active area was {visibility.ActiveAreaForReview}, expected CentralPlaza.");
            }

            if (controller.PlayerInOtherTime)
            {
                throw new InvalidOperationException("Startup player must remain in current time.");
            }

            var camera = Camera.main;
            if (camera == null)
            {
                throw new InvalidOperationException("Missing main camera.");
            }

            var player = RequireObject<CharacterController>("player controller");
            RequireActiveRenderer("Current_CentralPlazaMap_SeparateSpace", "central plaza stage");
            var playerRenderer = RequireActiveRenderer("FastVS_PlayerVisual_NiroPaper", "player visual");

            var playerBit = 1 << Mathf.Clamp(controller.PlayerVisibleRenderLayerForReview, 0, 31);
            if ((camera.cullingMask & playerBit) == 0)
            {
                throw new InvalidOperationException("Startup camera culling mask does not include the visible player layer.");
            }

            var viewport = camera.WorldToViewportPoint(player.transform.position + Vector3.up * 0.75f);
            if (viewport.z <= 0f ||
                viewport.x < 0.12f ||
                viewport.x > 0.88f ||
                viewport.y < 0.10f ||
                viewport.y > 0.90f)
            {
                throw new InvalidOperationException($"Startup player framing is out of range: viewport={viewport}.");
            }

            var playerLocal = controller.GetPlayerLocalCoordinateForReview();
            if (playerLocal.y < -0.05f || playerLocal.y > 0.45f)
            {
                throw new InvalidOperationException($"Startup player local height drifted out of range after warmup: local={playerLocal}.");
            }

            VerifyRendererFraming(camera, playerRenderer, "player visual");
        }

        private static void VerifyRendererFraming(Camera camera, Renderer renderer, string label)
        {
            if (renderer == null)
            {
                throw new InvalidOperationException($"Missing {label} renderer.");
            }

            var bounds = renderer.bounds;
            if (bounds.size.y < 0.25f || bounds.size.x < 0.10f)
            {
                throw new InvalidOperationException($"{label} renderer bounds are too small to confirm visibility: size={bounds.size}.");
            }

            var center = camera.WorldToViewportPoint(bounds.center);
            var top = camera.WorldToViewportPoint(bounds.center + Vector3.up * bounds.extents.y);
            var bottom = camera.WorldToViewportPoint(bounds.center - Vector3.up * bounds.extents.y);
            var height = Mathf.Abs(top.y - bottom.y);
            if (center.z <= 0f ||
                center.x < 0.08f ||
                center.x > 0.92f ||
                center.y < 0.08f ||
                center.y > 0.92f ||
                height < 0.045f)
            {
                throw new InvalidOperationException($"{label} renderer is not framed clearly enough: center={center}, viewportHeight={height:0.000}.");
            }
        }

        private static void VerifyTravel(
            TimeWindowPairedSpacePortalController controller,
            FastVsHouseAreaVisibility visibility,
            FastVsAreaDoorTransition transition,
            FastVsHouseArea sourceArea,
            FastVsHouseArea targetArea,
            string label)
        {
            visibility.SetActiveAreaForReview(sourceArea);
            controller.ForcePlayerCurrentLocalForReview(transition.TriggerLocalCenterForReview);
            if (!transition.TryEvaluateCurrentPlayerForReview())
            {
                throw new InvalidOperationException($"{label} did not trigger.");
            }

            if (visibility.ActiveAreaForReview != targetArea)
            {
                throw new InvalidOperationException($"{label} activated {visibility.ActiveAreaForReview}, expected {targetArea}.");
            }

            var actualLocal = controller.GetPlayerLocalCoordinateForReview();
            var expectedLocal = transition.TargetLocalPositionForReview;
            if ((actualLocal - expectedLocal).sqrMagnitude > 0.01f)
            {
                throw new InvalidOperationException($"{label} placed player at {actualLocal}, expected {expectedLocal}.");
            }
        }

        private static T RequireObject<T>(string label)
            where T : UnityEngine.Object
        {
            var found = FindFirstObjectByType<T>();
            if (found == null)
            {
                throw new InvalidOperationException($"Missing {label}.");
            }

            return found;
        }

        private static FastVsAreaDoorTransition RequireTransition(string objectName)
        {
            var gameObject = GameObject.Find(objectName);
            var transition = gameObject != null ? gameObject.GetComponent<FastVsAreaDoorTransition>() : null;
            if (transition == null)
            {
                throw new InvalidOperationException($"Missing door transition {objectName}.");
            }

            return transition;
        }

        private static Renderer RequireActiveRenderer(string objectName)
        {
            return RequireActiveRenderer(objectName, objectName);
        }

        private static Renderer RequireActiveRenderer(string objectName, string label)
        {
            var gameObject = GameObject.Find(objectName);
            if (gameObject == null)
            {
                throw new InvalidOperationException($"Missing {label} {objectName}.");
            }

            var renderers = gameObject.GetComponentsInChildren<Renderer>(true);
            for (var i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && renderers[i].enabled && renderers[i].gameObject.activeInHierarchy)
                {
                    return renderers[i];
                }
            }

            throw new InvalidOperationException($"{label} {objectName} has no active renderer.");
        }
    }
}
