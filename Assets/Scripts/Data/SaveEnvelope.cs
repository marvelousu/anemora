using System;
using System.Collections.Generic;

namespace Anemora.Data
{
    public enum TimeFrameState
    {
        Normal = 0,
        Crossing = 1
    }

    [Serializable]
    public sealed class SaveMetadata
    {
        public int saveVersion;
        public string slotId;
        public string displayName;
        public string sceneId;
        public string zoneId;
        public int layerIndex;
        public long playTimeSeconds;
        public long savedAtUnixSeconds;
        public bool hasThumbnail;
    }

    [Serializable]
    public sealed class PlayerSaveData
    {
        public float positionX;
        public float positionY;
        public float positionZ;
        public float facingYawDegrees;
        public string sceneSide;
    }

    [Serializable]
    public sealed class ProgressFlagSaveData
    {
        public int currentLayerIndex;
        public List<string> unlockedZoneIds = new List<string>();
        public List<string> completedSideQuestIds = new List<string>();
        public List<string> rawFlags = new List<string>();
    }

    [Serializable]
    public sealed class TimeFrameSaveData
    {
        public TimeFrameState state = TimeFrameState.Normal;
        public string activePortalSide;
    }

    [Serializable]
    public sealed class SaveEnvelope
    {
        public int saveVersion;
        public string buildVersion;
        public string slotId;
        public string sceneId;
        public long savedAtUnixSeconds;
        public SaveMetadata metadata;
        public PlayerSaveData player;
        public ActionRecordStoreSaveData actionRecords;
        public ProgressFlagSaveData progressFlags;
        public TimeFrameSaveData timeFrame;
    }
}
