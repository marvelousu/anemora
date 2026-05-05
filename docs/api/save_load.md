# Save / Load API Surface

Status: Draft for Stage 4 onboarding

Last source scan: 2026-05-05

## 1. Scope

This document records the current implemented save/load API surface for new contributors. It describes only code that exists in the repository at the time of the source scan.

Current state:

- `Anemora.Save` exists as an asmdef and currently contains the migration interface only.
- Serializable save DTOs live in `Anemora.Data`.
- Runtime ActionRecord persistence is exposed through `ActionRecordStore` and `ActionRecordRuntime`.
- A full filesystem `SaveService` / slot repository is defined by ADR-0006 but is not implemented in code yet.

## 2. Assembly Responsibilities

| Assembly | Files scanned | Current responsibility |
|---|---|---|
| `Anemora.Save` | `Assets/Scripts/Save/Anemora.Save.asmdef`, `Assets/Scripts/Save/Migration/ISaveMigration.cs` | Save migration contract. The asmdef references `Anemora.Data` and `Newtonsoft.Json`. |
| `Anemora.Data` | `Assets/Scripts/Data/*.cs` | Engine-free POCO DTOs for save envelopes, settings, ActionRecord state, and dialogue data. `Anemora.Data.asmdef` has `noEngineReferences: true`. |
| `Anemora.TimeManagement` scripts | `Assets/Scripts/TimeManagement/ActionRecordRuntime.cs`, ActionRecord reflector scripts | Runtime holder and reflection dispatch for ActionRecord state. It converts runtime state to/from `ActionRecordStoreSaveData`. |
| `Anemora.Game` | `Assets/Scripts/Game/Dialogue/DialogueAsset.cs` | Unity-dependent `DialogueAsset` ScriptableObject layer. It shares string keys with `DialogueAssetData` but is not serialized into `SaveEnvelope` currently. |

`Anemora.Save.asmdef` currently has `noEngineReferences: false`, but the only implemented source file under `Assets/Scripts/Save/` does not use Unity APIs.

## 3. Implemented API Surface

### 3.1 `Anemora.Save`

| Type | File | Public members |
|---|---|---|
| `ISaveMigration` | `Assets/Scripts/Save/Migration/ISaveMigration.cs` | `int FromVersion { get; }`; `int ToVersion { get; }`; `JObject Migrate(JObject source)` |

There is no implemented `SaveService`, `SaveRepository`, `SaveManager`, slot writer, or filesystem API in `Anemora.Save` at this scan point.

### 3.2 Save DTOs in `Anemora.Data`

| Type | File | Purpose |
|---|---|---|
| `SaveEnvelope` | `Assets/Scripts/Data/SaveEnvelope.cs` | Slot-level save root DTO. |
| `SaveMetadata` | `Assets/Scripts/Data/SaveEnvelope.cs` | Load-screen metadata DTO. |
| `PlayerSaveData` | `Assets/Scripts/Data/SaveEnvelope.cs` | Player position, yaw, and scene side. |
| `ProgressFlagSaveData` | `Assets/Scripts/Data/SaveEnvelope.cs` | Layer index, unlocked zones, completed side quests, raw flags. |
| `TimeFrameSaveData` | `Assets/Scripts/Data/SaveEnvelope.cs` | Time-frame crossing state and active portal side. |
| `TimeFrameState` | `Assets/Scripts/Data/SaveEnvelope.cs` | `Normal`, `Crossing`. |
| `SettingsEnvelope` | `Assets/Scripts/Data/SettingsEnvelope.cs` | Slot-independent settings root DTO. |
| `AccessibilitySaveData` | `Assets/Scripts/Data/SettingsEnvelope.cs` | UI scale, subtitle size, high contrast. |
| `AudioSettingsSaveData` | `Assets/Scripts/Data/SettingsEnvelope.cs` | Master, music, SFX, ambient volume. |
| `DisplaySettingsSaveData` | `Assets/Scripts/Data/SettingsEnvelope.cs` | Width, height, fullscreen, target frame rate. |
| `ActionRecordEntry` | `Assets/Scripts/Data/ActionRecord.cs` | One persisted player action record. |
| `ActionRecordStoreSaveData` | `Assets/Scripts/Data/ActionRecord.cs` | Serializable list wrapper for ActionRecord entries. |
| `ActionRecordStore` | `Assets/Scripts/Data/ActionRecord.cs` | Runtime collection with save-data conversion methods. |
| `ActionType` | `Assets/Scripts/Data/ActionType.cs` | `Unknown`, `Take`, `Tell`, `Move`. |
| `DialogueAssetData` | `Assets/Scripts/Data/Dialogue.cs` | Engine-free dialogue data root. Not currently a field on `SaveEnvelope`. |
| `DialogueVariantData` | `Assets/Scripts/Data/Dialogue.cs` | Dialogue variant with required/excluded flags. |
| `DialogueTurnData` | `Assets/Scripts/Data/Dialogue.cs` | Speaker and text key. |
| `DialogueChoiceData` | `Assets/Scripts/Data/Dialogue.cs` | Emotion, label key, next turn id. |

### 3.3 ActionRecord Runtime Persistence

| Type | File | Public save-related members |
|---|---|---|
| `ActionRecordStore` | `Assets/Scripts/Data/ActionRecord.cs` | `IReadOnlyList<ActionRecordEntry> Entries`; `void Add(ActionRecordEntry entry)`; `IEnumerable<ActionRecordEntry> GetUnreflected()`; `IEnumerable<ActionRecordEntry> GetReflected()`; `bool MarkReflected(string actionId)`; `ActionRecordStoreSaveData ToSaveData()`; `void LoadFromSaveData(ActionRecordStoreSaveData saveData)`; `void Clear()` |
| `ActionRecordRuntime` | `Assets/Scripts/TimeManagement/ActionRecordRuntime.cs` | `ActionRecordStore Store`; `IReadOnlyList<ActionRecordEntry> Entries`; `void AddEntry(ActionRecordEntry entry)`; `IEnumerable<ActionRecordEntry> GetUnreflected()`; `IEnumerable<ActionRecordEntry> GetReflected()`; `bool MarkReflected(string actionId)`; `void LoadFromSaveData(ActionRecordStoreSaveData saveData)`; `ActionRecordStoreSaveData ToSaveData()`; `int ReflectUnreflected()` |

`ActionRecordRuntime` reflects unreflected entries when `TimeFramePortalController.CrossingCompleted` reports `SceneSide.Current`. Successful reflection marks the entry as reflected, so `reflected` is part of persisted state.

## 4. Current Save / Load Flow

There is no implemented file-level save/load service yet. The current implemented flow is DTO conversion plus JSON round-trip verification in EditMode tests.

### 4.1 ActionRecord capture into `SaveEnvelope`

Actual callable surface:

```csharp
ActionRecordStoreSaveData ActionRecordStore.ToSaveData();
void ActionRecordStore.LoadFromSaveData(ActionRecordStoreSaveData saveData);
ActionRecordStoreSaveData ActionRecordRuntime.ToSaveData();
void ActionRecordRuntime.LoadFromSaveData(ActionRecordStoreSaveData saveData);
```

Example:

```csharp
using Anemora.Data;
using Anemora.TimeManagement;

var envelope = new SaveEnvelope
{
    saveVersion = 1,
    buildVersion = "0.1.0",
    slotId = "autosave",
    sceneId = "Anemora_Main",
    actionRecords = actionRecordRuntime.ToSaveData()
};
```

### 4.2 ActionRecord restore from `SaveEnvelope`

Example:

```csharp
using Anemora.Data;
using Anemora.TimeManagement;
using Newtonsoft.Json;

SaveEnvelope envelope = JsonConvert.DeserializeObject<SaveEnvelope>(json);
actionRecordRuntime.LoadFromSaveData(envelope.actionRecords);
```

`LoadFromSaveData(null)` is implemented to clear the store. `ToSaveData()` returns copied entries; changing the returned DTO does not mutate the original store.

### 4.3 JSON serialization currently covered by tests

The existing EditMode tests serialize and deserialize DTOs with Newtonsoft.Json:

```csharp
using Anemora.Data;
using Newtonsoft.Json;

var json = JsonConvert.SerializeObject(envelope);
var restored = JsonConvert.DeserializeObject<SaveEnvelope>(json);
```

The same pattern is covered for `SettingsEnvelope`.

### 4.4 Migration contract

Actual callable surface:

```csharp
using Newtonsoft.Json.Linq;

public interface ISaveMigration
{
    int FromVersion { get; }
    int ToVersion { get; }
    JObject Migrate(JObject source);
}
```

`SaveMigrationTests` currently verifies a sample `fromVersion -> fromVersion + 1` migration implementation. A production `SaveMigrator` chain is not implemented yet.

## 5. ActionRecord Persistence

E5 + G4 established the current ActionRecord persistence boundary:

1. Past-side interaction calls `ActionRecordRuntime.AddEntry(...)`.
2. The entry stores `actionId`, `targetObjectId`, `ActionType`, `gameTimeTicks`, and `reflected`.
3. `ActionRecordRuntime.ToSaveData()` exposes `ActionRecordStoreSaveData` for inclusion in `SaveEnvelope.actionRecords`.
4. On restore, `ActionRecordRuntime.LoadFromSaveData(envelope.actionRecords)` repopulates the runtime store.
5. On return to Current, `ReflectUnreflected()` dispatches entries to reflectors and marks successfully reflected entries with `reflected = true`.

`ActionRecordCatalog` is a `ScriptableObject` static catalog and is not persisted in `SaveEnvelope`. The save file persists action records, not the catalog definitions.

## 6. DialogueAsset Relationship

Current implemented relationship:

- `DialogueAssetData` is an engine-free POCO type in `Anemora.Data`.
- `DialogueAsset` is a Unity `ScriptableObject` type in `Anemora.Game`.
- Both layers use string-key based dialogue content and variant flags.
- `SaveEnvelope` currently has no `DialogueAssetData`, dialogue history, locale, or selected dialogue state field.
- `DisplaySettingsSaveData` currently has no `localeCode` field.

ADR-0008 records `DisplaySettingsSaveData.localeCode` as a Stage 4 addition candidate. Until that is implemented, locale selection and dialogue progression are not part of the actual save DTO surface.

## 7. Batchmode / Non-Batchmode Differences

The save DTO and migration code has no batchmode-specific branch.

Dialogue localization has runtime differences outside the save layer:

- `DialogueAsset.DialogueLocalization.ResolveOrFallback(...)` returns the fallback string when `Application.isBatchMode` is true.
- `DialogueDisplay.ResolveLocalizedStringOrFallback(...)` also returns fallback when `Application.isBatchMode` is true or localization settings are unavailable.

This affects dialogue rendering tests and placeholder key display, not `SaveEnvelope` serialization.

## 8. Format, Paths, and Extensions

Actual code state:

- DTO JSON round-trip is covered with `Newtonsoft.Json.JsonConvert`.
- No implemented code currently writes `save.json`, `settings.json`, or any other save file to disk.
- No implemented code currently calls `Application.persistentDataPath`.
- No implemented code currently defines slot directory names or extensions.

ADR-0006 defines the intended Stage 4 filesystem layout under `<persistentDataPath>/Anemora/`, with `settings.json`, `saves/<slot>/save.json`, `meta.json`, and backup / temp JSON files. Treat that as the accepted design direction, not as an implemented API surface.

## 9. Verification Coverage

Current automated coverage:

- `SaveEnvelopeRoundTripTests`: `SaveEnvelope` and `SettingsEnvelope` JSON round-trip.
- `SaveMigrationTests`: `ISaveMigration` sample migration behavior.
- `ActionRecordStoreTests`: ActionRecord add/query/reflection flag/save-data round-trip behavior.

`docs/VERIFICATION_SUITE.md` records that PlayMode has no save/load-specific test class currently; standalone save/load round-trip remains a G5 manual verification item.

## 10. ADR Cross-References

- `docs/adr/0004-project-directory-structure.md`: places POCO DTOs under `Assets/Scripts/Data/` and keeps ScriptableObject catalogs separate.
- `docs/adr/0005-time-management-scene-switching.md`: defines ActionRecord persistence and reflection ownership.
- `docs/adr/0006-save-system.md`: accepted save system direction: JSON, `Application.persistentDataPath`, autosave/manual slots, metadata, backups, migrations.
- `docs/adr/0008-localization.md`: DialogueAsset / DialogueAssetData split and Stage 4 locale persistence caveat.

## 11. Current Caveats

- `Anemora.Save` is not yet the full save service layer; it currently exposes only `ISaveMigration`.
- File IO, slot enumeration, atomic writes, backup recovery, metadata regeneration, settings fallback, and Steam Cloud sync are ADR-level design, not implemented API.
- A1 LocalizationSettings / StringTable work may still change settings and dialogue-related DTO needs. Any future `localeCode` or dialogue-progress field should update this document when added.
