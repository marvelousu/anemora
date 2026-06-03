using Anemora.TimeManagement;
using UnityEngine;

namespace Anemora.FastVS
{
    public sealed class FastVsHd2dDioramaCameraBoundsClamp : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dDioramaCameraBoundsProfile profile;
        [SerializeField] private TimeWindowPairedSpacePortalController controller;
        [SerializeField] private FastVsHouseAreaVisibility areaVisibility;

        public bool IsReadyForReview => profile != null && profile.EntryCountForReview > 0;
        public Vector3 LastRequestedLocalAnchorForReview { get; private set; }
        public Vector3 LastClampedLocalAnchorForReview { get; private set; }
        public Vector2 LastSafeMinForReview { get; private set; }
        public Vector2 LastSafeMaxForReview { get; private set; }
        public Vector2 LastDynamicMarginForReview { get; private set; }
        public FastVsHouseArea LastAreaForReview { get; private set; }
        public bool LastClampAppliedForReview { get; private set; }
        public string LastEntryIdForReview { get; private set; } = string.Empty;
        public FastVsHd2dDioramaCameraBoundsProfile ProfileForReview => profile;

        public void ConfigureForReview(
            FastVsHd2dDioramaCameraBoundsProfile boundsProfile,
            TimeWindowPairedSpacePortalController pairedSpaceController,
            FastVsHouseAreaVisibility visibility)
        {
            profile = boundsProfile;
            controller = pairedSpaceController;
            areaVisibility = visibility;
        }

        public bool TryClampAnchorForReview(
            Vector3 requestedAnchorWorld,
            Transform activeRoot,
            FastVsHouseArea area,
            Vector3 positionOffset,
            Vector3 lookOffset,
            float fieldOfView,
            float aspect,
            out Vector3 clampedAnchorWorld)
        {
            clampedAnchorWorld = requestedAnchorWorld;
            if (profile == null)
            {
                ClearLastResult(area);
                return false;
            }

            if (!profile.TryClampAnchorForReview(
                    ResolveArea(area),
                    requestedAnchorWorld,
                    activeRoot,
                    positionOffset,
                    lookOffset,
                    fieldOfView,
                    aspect,
                    out clampedAnchorWorld,
                    out var result))
            {
                ClearLastResult(area);
                return false;
            }

            StoreLastResult(result);
            return true;
        }

        public bool TryClampLocalAnchorForReview(
            FastVsHouseArea area,
            Vector3 requestedLocalAnchor,
            Vector3 positionOffset,
            Vector3 lookOffset,
            float fieldOfView,
            float aspect,
            out Vector3 clampedLocalAnchor,
            out Vector2 safeMin,
            out Vector2 safeMax,
            out Vector2 dynamicMargin)
        {
            clampedLocalAnchor = requestedLocalAnchor;
            safeMin = Vector2.zero;
            safeMax = Vector2.zero;
            dynamicMargin = Vector2.zero;

            var root = controller != null ? controller.CurrentSpaceRootForReview : null;
            var requestedWorldAnchor = root != null ? root.TransformPoint(requestedLocalAnchor) : requestedLocalAnchor;
            if (!TryClampAnchorForReview(
                    requestedWorldAnchor,
                    root,
                    area,
                    positionOffset,
                    lookOffset,
                    fieldOfView,
                    aspect,
                    out var clampedWorldAnchor))
            {
                return false;
            }

            clampedLocalAnchor = root != null ? root.InverseTransformPoint(clampedWorldAnchor) : clampedWorldAnchor;
            safeMin = LastSafeMinForReview;
            safeMax = LastSafeMaxForReview;
            dynamicMargin = LastDynamicMarginForReview;
            return true;
        }

        private FastVsHouseArea ResolveArea(FastVsHouseArea requestedArea)
        {
            if (areaVisibility == null)
            {
                return requestedArea;
            }

            return requestedArea == FastVsHouseArea.Interior ? areaVisibility.ActiveAreaForReview : requestedArea;
        }

        private void StoreLastResult(FastVsHd2dDioramaCameraBoundsProfile.ClampResult result)
        {
            LastEntryIdForReview = result.EntryIdForReview;
            LastAreaForReview = result.AreaForReview;
            LastRequestedLocalAnchorForReview = result.RawLocalAnchor;
            LastClampedLocalAnchorForReview = result.ClampedLocalAnchor;
            LastDynamicMarginForReview = result.ViewMargin;
            LastSafeMinForReview = result.BoundsMin + result.ViewMargin;
            LastSafeMaxForReview = result.BoundsMax - result.ViewMargin;
            LastClampAppliedForReview = result.WasClampedForReview;
        }

        private void ClearLastResult(FastVsHouseArea area)
        {
            LastEntryIdForReview = string.Empty;
            LastAreaForReview = area;
            LastRequestedLocalAnchorForReview = Vector3.zero;
            LastClampedLocalAnchorForReview = Vector3.zero;
            LastDynamicMarginForReview = Vector2.zero;
            LastSafeMinForReview = Vector2.zero;
            LastSafeMaxForReview = Vector2.zero;
            LastClampAppliedForReview = false;
        }
    }
}
