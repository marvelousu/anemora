using NUnit.Framework;
using Newtonsoft.Json;
using Anemora.Data;

namespace Anemora.Tests.EditMode
{
    [TestFixture]
    public class SaveEnvelopeRoundTripTests
    {
        [Test]
        public void SaveEnvelope_RoundTrip_PreservesAllFields()
        {
            var envelope = BuildSampleEnvelope();

            var json = JsonConvert.SerializeObject(envelope);
            var restored = JsonConvert.DeserializeObject<SaveEnvelope>(json);

            Assert.IsNotNull(restored);
            Assert.AreEqual(envelope.saveVersion, restored.saveVersion);
            Assert.AreEqual(envelope.buildVersion, restored.buildVersion);
            Assert.AreEqual(envelope.slotId, restored.slotId);
            Assert.AreEqual(envelope.sceneId, restored.sceneId);
            Assert.AreEqual(envelope.savedAtUnixSeconds, restored.savedAtUnixSeconds);

            Assert.IsNotNull(restored.metadata);
            Assert.AreEqual(envelope.metadata.zoneId, restored.metadata.zoneId);
            Assert.AreEqual(envelope.metadata.layerIndex, restored.metadata.layerIndex);
            Assert.AreEqual(envelope.metadata.playTimeSeconds, restored.metadata.playTimeSeconds);

            Assert.IsNotNull(restored.player);
            Assert.AreEqual(envelope.player.positionX, restored.player.positionX);
            Assert.AreEqual(envelope.player.positionZ, restored.player.positionZ);
            Assert.AreEqual(envelope.player.sceneSide, restored.player.sceneSide);

            Assert.IsNotNull(restored.actionRecords);
            Assert.AreEqual(1, restored.actionRecords.entries.Count);
            Assert.AreEqual("take_book_001", restored.actionRecords.entries[0].actionId);
            Assert.AreEqual(ActionType.Take, restored.actionRecords.entries[0].type);

            Assert.IsNotNull(restored.progressFlags);
            Assert.AreEqual(1, restored.progressFlags.currentLayerIndex);
            CollectionAssert.AreEqual(new[] { "zone1" }, restored.progressFlags.unlockedZoneIds);

            Assert.IsNotNull(restored.timeFrame);
            Assert.AreEqual(TimeFrameState.Normal, restored.timeFrame.state);
            Assert.IsNull(restored.timeFrame.activePortalSide);
        }

        [Test]
        public void SaveEnvelope_TimeFrameCrossingState_RoundTrips()
        {
            var envelope = BuildSampleEnvelope();
            envelope.timeFrame.state = TimeFrameState.Crossing;
            envelope.timeFrame.activePortalSide = "Past";

            var json = JsonConvert.SerializeObject(envelope);
            var restored = JsonConvert.DeserializeObject<SaveEnvelope>(json);

            Assert.AreEqual(TimeFrameState.Crossing, restored.timeFrame.state);
            Assert.AreEqual("Past", restored.timeFrame.activePortalSide);
        }

        [Test]
        public void SettingsEnvelope_RoundTrip_PreservesAllFields()
        {
            var settings = new SettingsEnvelope
            {
                settingsVersion = 1,
                buildVersion = "0.1.0",
                accessibility = new AccessibilitySaveData
                {
                    uiScale = 1.25f,
                    subtitleSize = 1.5f,
                    highContrast = true
                },
                audio = new AudioSettingsSaveData
                {
                    masterVolume = 0.8f,
                    musicVolume = 0.5f,
                    sfxVolume = 0.7f,
                    ambientVolume = 0.6f
                },
                display = new DisplaySettingsSaveData
                {
                    width = 1920,
                    height = 1080,
                    fullscreen = true,
                    targetFrameRate = 60
                }
            };

            var json = JsonConvert.SerializeObject(settings);
            var restored = JsonConvert.DeserializeObject<SettingsEnvelope>(json);

            Assert.IsNotNull(restored);
            Assert.AreEqual(settings.settingsVersion, restored.settingsVersion);
            Assert.AreEqual(1.25f, restored.accessibility.uiScale);
            Assert.IsTrue(restored.accessibility.highContrast);
            Assert.AreEqual(0.8f, restored.audio.masterVolume);
            Assert.AreEqual(1920, restored.display.width);
            Assert.AreEqual(60, restored.display.targetFrameRate);
        }

        private static SaveEnvelope BuildSampleEnvelope()
        {
            var envelope = new SaveEnvelope
            {
                saveVersion = 1,
                buildVersion = "0.1.0",
                slotId = "autosave",
                sceneId = "Anemora_Main",
                savedAtUnixSeconds = 1735689600,
                metadata = new SaveMetadata
                {
                    saveVersion = 1,
                    slotId = "autosave",
                    displayName = "Auto Save",
                    sceneId = "Anemora_Main",
                    zoneId = "zone1",
                    layerIndex = 1,
                    playTimeSeconds = 600,
                    savedAtUnixSeconds = 1735689600,
                    hasThumbnail = false
                },
                player = new PlayerSaveData
                {
                    positionX = 1.0f,
                    positionY = 0.0f,
                    positionZ = 2.0f,
                    facingYawDegrees = 90.0f,
                    sceneSide = "Current"
                },
                actionRecords = new ActionRecordStoreSaveData(),
                progressFlags = new ProgressFlagSaveData
                {
                    currentLayerIndex = 1
                },
                timeFrame = new TimeFrameSaveData
                {
                    state = TimeFrameState.Normal,
                    activePortalSide = null
                }
            };

            envelope.actionRecords.entries.Add(new ActionRecordEntry
            {
                actionId = "take_book_001",
                targetObjectId = "Book_Family_Past_001",
                type = ActionType.Take,
                gameTimeTicks = 12345,
                reflected = false
            });

            envelope.progressFlags.unlockedZoneIds.Add("zone1");

            return envelope;
        }
    }
}
