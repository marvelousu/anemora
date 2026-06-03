using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anemora.FastVS
{
    [CreateAssetMenu(fileName = "FastVsHd2dAreaCameraBlendProfile", menuName = "Anemora/Fast VS/HD2D Area Camera Blend Profile")]
    public sealed class FastVsHd2dAreaCameraBlendProfile : ScriptableObject
    {
        [SerializeField, Range(28f, 35f)] private float pitchDegrees = 29f;
        [SerializeField, Range(0.6f, 1.0f)] private float defaultBlendSeconds = 0.72f;
        [SerializeField] private int inactivePriority = 10;
        [SerializeField] private int livePriority = 120;
        [SerializeField] private Entry[] entries = Array.Empty<Entry>();

        public float PitchDegreesForReview => Mathf.Clamp(pitchDegrees, 28f, 35f);
        public float DefaultBlendSecondsForReview => Mathf.Clamp(defaultBlendSeconds, 0.6f, 1.0f);
        public int InactivePriorityForReview => inactivePriority;
        public int LivePriorityForReview => livePriority;
        public IReadOnlyList<Entry> EntriesForReview => entries ?? Array.Empty<Entry>();
        public int EntryCountForReview => entries != null ? entries.Length : 0;

        public void ConfigureForReview(float pitch, float blendSeconds, int inactive, int live, Entry[] configuredEntries)
        {
            pitchDegrees = Mathf.Clamp(pitch, 28f, 35f);
            defaultBlendSeconds = Mathf.Clamp(blendSeconds, 0.6f, 1.0f);
            inactivePriority = inactive;
            livePriority = Mathf.Max(live, inactive + 1);
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

        public bool TryGetEntryForArea(FastVsHouseArea area, out Entry entry)
        {
            entry = FindEntryForArea(area);
            if (entry != null)
            {
                return true;
            }

            entry = entries != null && entries.Length > 0 ? entries[0] : null;
            return entry != null;
        }

        public Entry FindEntryById(string entryId)
        {
            if (entries == null || string.IsNullOrEmpty(entryId))
            {
                return null;
            }

            for (var index = 0; index < entries.Length; index++)
            {
                var entry = entries[index];
                if (entry != null && string.Equals(entry.EntryIdForReview, entryId, StringComparison.Ordinal))
                {
                    return entry;
                }
            }

            return null;
        }

        [Serializable]
        public sealed class Entry
        {
            [SerializeField] private string entryId = "central_plaza_wide";
            [SerializeField] private string reviewRole = "wide town";
            [SerializeField] private FastVsHouseArea area = FastVsHouseArea.CentralPlaza;
            [SerializeField, Range(22f, 40f)] private float fieldOfView = 32f;
            [SerializeField, Min(1f)] private float distance = 5.15f;
            [SerializeField] private float targetHeight = 0.72f;
            [SerializeField] private float lookAhead = 0.45f;
            [SerializeField] private float lateralOffset;
            [SerializeField] private Vector3 triggerLocalCenter;
            [SerializeField] private Vector3 triggerLocalSize = new Vector3(18f, 3f, 16f);

            public string EntryIdForReview => entryId;
            public string ReviewRoleForReview => reviewRole;
            public FastVsHouseArea AreaForReview => area;
            public float FieldOfViewForReview => Mathf.Clamp(fieldOfView, 22f, 40f);
            public float DistanceForReview => Mathf.Max(1f, distance);
            public float TargetHeightForReview => targetHeight;
            public float LookAheadForReview => lookAhead;
            public float LateralOffsetForReview => lateralOffset;
            public Vector3 TriggerLocalCenterForReview => triggerLocalCenter;
            public Vector3 TriggerLocalSizeForReview => new Vector3(
                Mathf.Max(0.01f, Mathf.Abs(triggerLocalSize.x)),
                Mathf.Max(0.01f, Mathf.Abs(triggerLocalSize.y)),
                Mathf.Max(0.01f, Mathf.Abs(triggerLocalSize.z)));

            public static Entry CreateForReview(
                string id,
                string role,
                FastVsHouseArea cameraArea,
                float fov,
                float cameraDistance,
                float height,
                float ahead,
                float lateral,
                Vector3 triggerCenter,
                Vector3 triggerSize)
            {
                var entry = new Entry();
                entry.ConfigureForReview(id, role, cameraArea, fov, cameraDistance, height, ahead, lateral, triggerCenter, triggerSize);
                return entry;
            }

            public void ConfigureForReview(
                string id,
                string role,
                FastVsHouseArea cameraArea,
                float fov,
                float cameraDistance,
                float height,
                float ahead,
                float lateral,
                Vector3 triggerCenter,
                Vector3 triggerSize)
            {
                entryId = string.IsNullOrWhiteSpace(id) ? cameraArea.ToString() : id;
                reviewRole = string.IsNullOrWhiteSpace(role) ? cameraArea.ToString() : role;
                area = cameraArea;
                fieldOfView = Mathf.Clamp(fov, 22f, 40f);
                distance = Mathf.Max(1f, cameraDistance);
                targetHeight = height;
                lookAhead = ahead;
                lateralOffset = lateral;
                triggerLocalCenter = triggerCenter;
                triggerLocalSize = triggerSize;
            }

            public Vector3 PositionOffsetForPitch(float pitchDegrees)
            {
                var forwardDistance = Mathf.Max(0.1f, DistanceForReview + LookAheadForReview);
                var height = TargetHeightForReview + Mathf.Tan(Mathf.Clamp(pitchDegrees, 28f, 35f) * Mathf.Deg2Rad) * forwardDistance;
                return new Vector3(LateralOffsetForReview, height, -DistanceForReview);
            }

            public Vector3 LookOffsetForReview => new Vector3(0f, TargetHeightForReview, LookAheadForReview);

            public bool ContainsLocalPositionForReview(Vector3 localPosition)
            {
                var halfSize = TriggerLocalSizeForReview * 0.5f;
                var delta = localPosition - TriggerLocalCenterForReview;
                return Mathf.Abs(delta.x) <= halfSize.x &&
                       Mathf.Abs(delta.y) <= halfSize.y &&
                       Mathf.Abs(delta.z) <= halfSize.z;
            }
        }
    }
}
