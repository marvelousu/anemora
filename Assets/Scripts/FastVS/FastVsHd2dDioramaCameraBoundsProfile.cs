using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dDioramaCameraBoundsProfile", menuName = "Anemora/Fast VS/HD2D Diorama Camera Bounds Profile")]
    public sealed class FastVsHd2dDioramaCameraBoundsProfile : ScriptableObject
    {
        [SerializeField, Min(0f)] private float viewportSafetyPadding = 0.38f;
        [SerializeField] private Entry[] entries = Array.Empty<Entry>();

        public float ViewportSafetyPaddingForReview => Mathf.Max(0f, viewportSafetyPadding);
        public IReadOnlyList<Entry> EntriesForReview => entries ?? Array.Empty<Entry>();
        public int EntryCountForReview => entries != null ? entries.Length : 0;

        public void ConfigureForReview(float safetyPadding, Entry[] configuredEntries)
        {
            viewportSafetyPadding = Mathf.Max(0f, safetyPadding);
            ConfigureForReview(configuredEntries);
        }

        public void ConfigureForReview(Entry[] configuredEntries)
        {
            entries = configuredEntries ?? Array.Empty<Entry>();
        }

        public Entry FindEntryForArea(FastVsHouseArea area)
        {
            if (entries == null)
            {
                return null;
            }

            for (var index = 0; index < entries.Length; index++)
            {
                var entry = entries[index];
                if (entry != null && entry.AreaForReview == area)
                {
                    return entry;
                }
            }

            return null;
        }

        public bool TryClampAnchorForReview(
            FastVsHouseArea area,
            Vector3 rawWorldAnchor,
            Transform activeRoot,
            Vector3 positionOffset,
            Vector3 lookOffset,
            float fieldOfView,
            float aspect,
            out Vector3 clampedWorldAnchor,
            out ClampResult result)
        {
            clampedWorldAnchor = rawWorldAnchor;
            result = default;

            var entry = FindEntryForArea(area);
            if (entry == null)
            {
                return false;
            }

            var rawLocalAnchor = activeRoot != null ? activeRoot.InverseTransformPoint(rawWorldAnchor) : rawWorldAnchor;
            var viewMargin = entry.CalculateViewMarginForReview(positionOffset, lookOffset, fieldOfView, aspect) + Vector2.one * ViewportSafetyPaddingForReview;
            var clampedLocalAnchor = rawLocalAnchor;
            clampedLocalAnchor.x = ClampAxis(rawLocalAnchor.x, entry.BoundsMinForReview.x, entry.BoundsMaxForReview.x, viewMargin.x);
            clampedLocalAnchor.z = ClampAxis(rawLocalAnchor.z, entry.BoundsMinForReview.y, entry.BoundsMaxForReview.y, viewMargin.y);
            clampedWorldAnchor = activeRoot != null ? activeRoot.TransformPoint(clampedLocalAnchor) : clampedLocalAnchor;
            result = new ClampResult(entry, rawLocalAnchor, clampedLocalAnchor, viewMargin);
            return true;
        }

        public static float ClampAxis(float value, float boundsMin, float boundsMax, float viewMargin)
        {
            var min = Mathf.Min(boundsMin, boundsMax);
            var max = Mathf.Max(boundsMin, boundsMax);
            var half = Mathf.Max(0f, (max - min) * 0.5f);
            var margin = Mathf.Min(Mathf.Max(0f, viewMargin), Mathf.Max(0f, half - 0.01f));
            var safeMin = min + margin;
            var safeMax = max - margin;
            if (safeMin > safeMax)
            {
                return (min + max) * 0.5f;
            }

            return Mathf.Clamp(value, safeMin, safeMax);
        }

        public static Vector2 CalculateGroundPlaneViewHalfExtentsForReview(
            Vector3 positionOffset,
            Vector3 lookOffset,
            float fieldOfView,
            float aspect)
        {
            var forward = lookOffset - positionOffset;
            if (forward.sqrMagnitude < 0.0001f)
            {
                forward = Vector3.forward;
            }

            var rotation = Quaternion.LookRotation(forward.normalized, Vector3.up);
            var clampedFieldOfView = Mathf.Clamp(fieldOfView, 1f, 120f);
            var verticalTan = Mathf.Tan(clampedFieldOfView * 0.5f * Mathf.Deg2Rad);
            var horizontalTan = verticalTan * Mathf.Max(0.01f, aspect);
            var maxAbsX = Mathf.Abs(lookOffset.x);
            var maxAbsZ = Mathf.Abs(lookOffset.z);
            var hitCount = 0;

            for (var y = -1; y <= 1; y += 2)
            {
                for (var x = -1; x <= 1; x += 2)
                {
                    var localRay = new Vector3(x * horizontalTan, y * verticalTan, 1f).normalized;
                    var ray = rotation * localRay;
                    if (ray.y >= -0.001f)
                    {
                        continue;
                    }

                    var t = -positionOffset.y / ray.y;
                    if (t < 0f || t > 200f)
                    {
                        continue;
                    }

                    var hit = positionOffset + ray * t;
                    maxAbsX = Mathf.Max(maxAbsX, Mathf.Abs(hit.x));
                    maxAbsZ = Mathf.Max(maxAbsZ, Mathf.Abs(hit.z));
                    hitCount++;
                }
            }

            if (hitCount == 0)
            {
                var focusDistance = Mathf.Max(0.1f, Vector3.Distance(positionOffset, lookOffset));
                maxAbsX = Mathf.Max(maxAbsX, horizontalTan * focusDistance);
                maxAbsZ = Mathf.Max(maxAbsZ, verticalTan * focusDistance + Mathf.Abs(positionOffset.z - lookOffset.z));
            }

            return new Vector2(Mathf.Max(0.01f, maxAbsX), Mathf.Max(0.01f, maxAbsZ));
        }

        public readonly struct ClampResult
        {
            public readonly string EntryIdForReview;
            public readonly FastVsHouseArea AreaForReview;
            public readonly Vector3 RawLocalAnchor;
            public readonly Vector3 ClampedLocalAnchor;
            public readonly Vector2 BoundsMin;
            public readonly Vector2 BoundsMax;
            public readonly Vector2 ViewMargin;
            public readonly bool WasClampedForReview;

            public ClampResult(Entry entry, Vector3 rawLocalAnchor, Vector3 clampedLocalAnchor, Vector2 viewMargin)
            {
                EntryIdForReview = entry != null ? entry.EntryIdForReview : string.Empty;
                AreaForReview = entry != null ? entry.AreaForReview : FastVsHouseArea.CentralPlaza;
                RawLocalAnchor = rawLocalAnchor;
                ClampedLocalAnchor = clampedLocalAnchor;
                BoundsMin = entry != null ? entry.BoundsMinForReview : Vector2.zero;
                BoundsMax = entry != null ? entry.BoundsMaxForReview : Vector2.zero;
                ViewMargin = viewMargin;
                WasClampedForReview =
                    Mathf.Abs(rawLocalAnchor.x - clampedLocalAnchor.x) > 0.001f ||
                    Mathf.Abs(rawLocalAnchor.z - clampedLocalAnchor.z) > 0.001f;
            }
        }

        [Serializable]
        public sealed class Entry
        {
            [SerializeField] private string entryId = "central_plaza_route_safe_rect";
            [SerializeField] private string reviewRole = "wide route safe frame";
            [SerializeField] private FastVsHouseArea area = FastVsHouseArea.CentralPlaza;
            [SerializeField] private Vector2 boundsMin = new Vector2(-10f, -8f);
            [SerializeField] private Vector2 boundsMax = new Vector2(10f, 8f);
            [SerializeField] private Vector2 minimumViewMargin = new Vector2(2f, 2f);
            [SerializeField] private Vector2 viewMarginScale = new Vector2(0.66f, 0.78f);

            public string EntryIdForReview => entryId;
            public string ReviewRoleForReview => reviewRole;
            public FastVsHouseArea AreaForReview => area;
            public Vector2 BoundsMinForReview => new Vector2(Mathf.Min(boundsMin.x, boundsMax.x), Mathf.Min(boundsMin.y, boundsMax.y));
            public Vector2 BoundsMaxForReview => new Vector2(Mathf.Max(boundsMin.x, boundsMax.x), Mathf.Max(boundsMin.y, boundsMax.y));
            public Vector2 MinimumViewMarginForReview => new Vector2(Mathf.Max(0f, minimumViewMargin.x), Mathf.Max(0f, minimumViewMargin.y));
            public Vector2 ViewMarginScaleForReview => new Vector2(Mathf.Max(0f, viewMarginScale.x), Mathf.Max(0f, viewMarginScale.y));

            public static Entry CreateForReview(
                string id,
                FastVsHouseArea cameraArea,
                Vector2 min,
                Vector2 max,
                Vector2 minViewMargin,
                Vector2 marginScale)
            {
                var entry = new Entry();
                entry.ConfigureForReview(id, cameraArea, min, max, minViewMargin, marginScale);
                return entry;
            }

            public static Entry CreateForReview(
                string id,
                string role,
                FastVsHouseArea cameraArea,
                Vector3 center,
                Vector2 sizeXZ,
                float extraSafetyPadding)
            {
                var halfSize = new Vector2(Mathf.Abs(sizeXZ.x), Mathf.Abs(sizeXZ.y)) * 0.5f;
                var min = new Vector2(center.x - halfSize.x, center.z - halfSize.y);
                var max = new Vector2(center.x + halfSize.x, center.z + halfSize.y);
                var entry = CreateForReview(
                    id,
                    cameraArea,
                    min,
                    max,
                    Vector2.one * Mathf.Max(0f, extraSafetyPadding),
                    new Vector2(0.66f, 0.78f));
                entry.reviewRole = string.IsNullOrWhiteSpace(role) ? cameraArea.ToString() : role;
                return entry;
            }

            public void ConfigureForReview(
                string id,
                FastVsHouseArea cameraArea,
                Vector2 min,
                Vector2 max,
                Vector2 minViewMargin,
                Vector2 marginScale)
            {
                entryId = string.IsNullOrWhiteSpace(id) ? cameraArea.ToString() : id;
                reviewRole = cameraArea.ToString();
                area = cameraArea;
                boundsMin = new Vector2(Mathf.Min(min.x, max.x), Mathf.Min(min.y, max.y));
                boundsMax = new Vector2(Mathf.Max(min.x, max.x), Mathf.Max(min.y, max.y));
                minimumViewMargin = new Vector2(Mathf.Max(0f, minViewMargin.x), Mathf.Max(0f, minViewMargin.y));
                viewMarginScale = new Vector2(Mathf.Max(0f, marginScale.x), Mathf.Max(0f, marginScale.y));
            }

            public Vector2 CalculateViewMarginForReview(
                Vector3 positionOffset,
                Vector3 lookOffset,
                float fieldOfView,
                float aspect)
            {
                var groundHalfExtents = CalculateGroundPlaneViewHalfExtentsForReview(positionOffset, lookOffset, fieldOfView, aspect);
                var margin = new Vector2(
                    Mathf.Max(MinimumViewMarginForReview.x, groundHalfExtents.x * ViewMarginScaleForReview.x),
                    Mathf.Max(MinimumViewMarginForReview.y, groundHalfExtents.y * ViewMarginScaleForReview.y));

                var halfBounds = (BoundsMaxForReview - BoundsMinForReview) * 0.5f;
                margin.x = Mathf.Min(margin.x, Mathf.Max(0f, halfBounds.x - 0.01f));
                margin.y = Mathf.Min(margin.y, Mathf.Max(0f, halfBounds.y - 0.01f));
                return margin;
            }
        }
    }
}
