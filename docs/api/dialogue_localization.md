# Dialogue Localization API Surface

Status: Draft for Stage 3 contributor onboarding

Last source scan: 2026-05-05

## 1. Scope

This document records the currently implemented dialogue localization API surface. It describes only code and assets that exist in the repository at the time of the source scan.

Files scanned:

- `Assets/Scripts/Data/Dialogue.cs`
- `Assets/Scripts/Data/Anemora.Data.asmdef`
- `Assets/Scripts/Game/Dialogue/DialogueAsset.cs`
- `Assets/Scripts/Game/Anemora.Game.asmdef`
- `Assets/Localization/LocalizationSettings.asset`
- `Assets/Localization/Locales/*.asset`
- `Assets/Localization/StringTables/Anemora_Strings*.asset`
- `Assets/AddressableAssetsData/AssetGroups/Localization-*.asset`
- `ProjectSettings/EditorBuildSettings.asset`

## 2. Assembly Responsibilities

| Assembly | Files | Current responsibility |
|---|---|---|
| `Anemora.Data` | `Assets/Scripts/Data/Dialogue.cs` | Engine-free dialogue DTOs. Stores string keys only. |
| `Anemora.Game` | `Assets/Scripts/Game/Dialogue/DialogueAsset.cs` | Unity-dependent dialogue `ScriptableObject` layer. Stores `LocalizedString` references and fallback helper methods. |
| Unity Localization / Addressables assets | `Assets/Localization/`, `Assets/AddressableAssetsData/` | Project locales, active `LocalizationSettings`, `Anemora_Strings` collection, and Addressables entries for locale/table loading. |

`Anemora.Data.asmdef` has `noEngineReferences: true` and no assembly references. `Anemora.Game.asmdef` references `Anemora.Data`, `Unity.Localization`, and `Unity.ResourceManager`; it has `noEngineReferences: false` and `autoReferenced: false`.

## 3. `Anemora.Data` POCO Layer

Namespace: `Anemora.Data`

The POCO layer is the engine-free representation. It uses `[Serializable]` classes and public fields. Dialogue text and choice labels are stored as string keys, not as Unity Localization types.

| Type | Public fields | Responsibility |
|---|---|---|
| `DialogueAssetData` | `string npcId`; `List<DialogueVariantData> variants` | Root DTO for one NPC dialogue asset. |
| `DialogueVariantData` | `string variantId`; `List<DialogueTurnData> turns`; `List<string> requiredFlags`; `List<string> excludedFlags` | Variant DTO with required/excluded flag lists. |
| `DialogueTurnData` | `string speakerId`; `string textKey`; `List<DialogueChoiceData> choices` | Turn DTO with speaker id, localized text key, and choices. |
| `DialogueChoiceData` | `string emotion`; `string labelKey`; `string nextTurnId` | Choice DTO with emotion, localized label key, and next turn id. |

Current tests serialize these DTOs with Newtonsoft.Json and assert that the serialized form contains `textKey` / `labelKey` and does not contain `LocalizedString` or `tableReference`.

## 4. `Anemora.Game` DialogueAsset Layer

Namespace: `Anemora.Game.Dialogue`

The Unity-facing layer is `Assets/Scripts/Game/Dialogue/DialogueAsset.cs`. It is intended for Inspector/runtime dialogue assets and stores Unity Localization `LocalizedString` fields.

| Type | Unity/API details | Public fields / methods |
|---|---|---|
| `DialogueAsset` | `ScriptableObject`; `[CreateAssetMenu(menuName = "Anemora/Dialogue", fileName = "Dialogue")]` | `string npcId`; `List<DialogueVariantSO> variants` |
| `DialogueVariantSO` | `[Serializable]` | `string variantId`; `List<DialogueTurnSO> turns`; `List<string> requiredFlags`; `List<string> excludedFlags` |
| `DialogueTurnSO` | `[Serializable]` | `string speakerId`; `LocalizedString text`; `List<DialogueChoiceSO> choices`; `string GetLocalizedTextOrFallback(string fallback)` |
| `DialogueChoiceSO` | `[Serializable]` | `string emotion`; `LocalizedString label`; `string nextTurnId`; `string GetLocalizedLabelOrFallback(string fallback)` |

`DialogueTurnSO.GetLocalizedTextOrFallback(...)` delegates to `DialogueLocalization.ResolveOrFallback(text, fallback)`. `DialogueChoiceSO.GetLocalizedLabelOrFallback(...)` delegates to `DialogueLocalization.ResolveOrFallback(label, fallback)`.

## 5. `DialogueLocalization.ResolveOrFallback`

Declaring type:

```csharp
internal static class DialogueLocalization
```

Public method signature:

```csharp
public static string ResolveOrFallback(LocalizedString localizedString, string fallback)
```

The method is `public`, but its declaring class is `internal`, so this helper is available inside the `Anemora.Game` assembly only. The supported public entry points for normal callers are currently:

```csharp
string DialogueTurnSO.GetLocalizedTextOrFallback(string fallback);
string DialogueChoiceSO.GetLocalizedLabelOrFallback(string fallback);
```

Current behavior:

| Condition | Result |
|---|---|
| `localizedString == null` or `localizedString.IsEmpty` | Returns `fallback ?? string.Empty`. |
| `LocalizationSettings.HasSettings == false` | Returns `fallback ?? string.Empty`. |
| `Application.isBatchMode == true` | Returns `fallback ?? string.Empty`. |
| Non-batchmode runtime | Waits for `LocalizationSettings.SelectedLocaleAsync` if needed, chooses `localizedString.LocaleOverride` when set, otherwise the selected locale, then resolves through `LocalizationSettings.StringDatabase.GetTableEntry(...)`. |
| `localizedString.FallbackState == FallbackBehavior.DontUseFallback` | Calls `GetTableEntry(...)` with `FallbackBehavior.DontUseFallback`. |
| Any other fallback state | Calls `GetTableEntry(...)` with `FallbackBehavior.UseFallback`. |
| Resolved entry is missing or localized value is null/empty | Returns `fallback ?? string.Empty`. |
| Exception while resolving | Returns `fallback ?? string.Empty`. |

The method intentionally keeps the batchmode fallback path. Current PlayMode tests cover both the fallback helpers and the non-batch-style `LocalizationSettings.StringDatabase.GetLocalizedStringAsync(...)` resolution path.

## 6. Localization Asset Structure

### 6.1 Active settings

`ProjectSettings/EditorBuildSettings.asset` registers:

- `com.unity.localization.settings` -> `Assets/Localization/LocalizationSettings.asset`
- `com.unity.addressableassets` -> `Assets/AddressableAssetsData/AddressableAssetSettings.asset`

`Assets/Localization/LocalizationSettings.asset` currently has:

- Project locale identifier: `ja-JP`
- Startup selectors:
  - `CommandLineLocaleSelector` with `-language=`
  - `SpecificLocaleSelector` with `ja-JP`
- Asset database fallback: enabled
- String database fallback: enabled
- Preload behavior: `1`
- Initialize synchronously: `0`

### 6.2 Locales

| Asset | Locale code | Locale name | Fallback metadata |
|---|---|---|---|
| `Assets/Localization/Locales/Locale_ja-JP.asset` | `ja-JP` | `Japanese` | Fallback locale points to `Locale_en.asset`. |
| `Assets/Localization/Locales/Locale_en.asset` | `en` | `English` | No fallback metadata entries. |

### 6.3 `Anemora_Strings` collection

| Asset | Role |
|---|---|
| `Assets/Localization/StringTables/Anemora_Strings.asset` | `StringTableCollection`; collection name `Anemora_Strings`. |
| `Assets/Localization/StringTables/Anemora_Strings Shared Data.asset` | Shared keys and numeric ids for the collection. |
| `Assets/Localization/StringTables/Anemora_Strings_ja-JP.asset` | `StringTable` for locale `ja-JP`. |
| `Assets/Localization/StringTables/Anemora_Strings_en.asset` | `StringTable` for locale `en`. |

Current shared data entries:

| Key | Shared id | ja-JP value | en value |
|---|---:|---|---|
| `dialogue.placeholder.resident_a.greet` | `1349509120` | `[TBD: Resident_A greet line]` | `[TBD: Resident_A greet line]` |
| `dialogue.placeholder.resident_b.idle` | `1534058496` | `[TBD: Resident_B idle line]` | `[TBD: Resident_B idle line]` |
| `ui.menu.start` | `1534058497` | `はじめる` | `Start` |
| `ui.menu.continue` | `1534058498` | `つづきから` | `Continue` |
| `ui.menu.options` | `1538252800` | `せってい` | `Options` |
| `ui.menu.quit` | `1538252801` | `おわる` | `Quit` |
| `system.autosave_indicator` | `1538252802` | `じどうほぞん中` | `Autosaving...` |

### 6.4 Addressables groups

The localization assets are registered in read-only localization Addressables groups:

| Group | Entries / addresses |
|---|---|
| `Localization-Locales` | `English`, `Japanese` |
| `Localization-Assets-Shared` | `Assets/Localization/StringTables/Anemora_Strings Shared Data.asset` |
| `Localization-String-Tables-Japanese` | `Anemora_Strings_ja-JP` |
| `Localization-String-Tables-English` | `Anemora_Strings_en` |

## 7. Key Naming

Current categories:

| Prefix | Current use |
|---|---|
| `dialogue.*` | Dialogue-facing text keys. Placeholder dialogue content uses `[TBD: ...]` until final content is approved. |
| `dialogue.placeholder.*` | Current seed pattern for placeholder NPC lines, for example `dialogue.placeholder.resident_a.greet`. |
| `ui.*` | UI labels. Current menu entries live under `ui.menu.*`. |
| `system.*` | System-facing UI/status text, for example `system.autosave_indicator`. |

For `DialogueAsset` SO fields, set `LocalizedString` table references to `Anemora_Strings` and entry references to the exact string key. Code/tests may also construct a reference directly:

```csharp
var text = new LocalizedString("Anemora_Strings", "dialogue.placeholder.resident_a.greet");
```

## 8. Adding a New StringTable Key

Use the current collection structure as the source of truth:

1. Add the key to `Anemora_Strings Shared Data.asset` under `m_Entries`.
2. Ensure the new shared entry has one stable numeric `m_Id`.
3. Add a matching `m_TableData` row with the same `m_Id` to both:
   - `Anemora_Strings_ja-JP.asset`
   - `Anemora_Strings_en.asset`
4. For unfinished dialogue content, keep localized values as `[TBD: <description>]`.
5. For finalized UI/system text, add the actual ja-JP and en values.
6. In `DialogueAsset` SOs, point `LocalizedString` to table `Anemora_Strings` and the new key.
7. Verify runtime resolution with `LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Anemora_Strings", key, locale, FallbackBehavior.UseFallback)` or through `DialogueTurnSO.GetLocalizedTextOrFallback(...)` / `DialogueChoiceSO.GetLocalizedLabelOrFallback(...)`.

The Unity Localization editor should be preferred for normal key creation because it keeps shared ids and locale table rows aligned. When reviewing YAML, the invariant is that the shared key id and each locale table row id match.

## 9. Known Caveats

- Do not remove the `Application.isBatchMode` fallback in `DialogueLocalization.ResolveOrFallback(...)`. It is the implemented compatibility path for batchmode tests/builds.
- `DialogueLocalization.ResolveOrFallback(...)` catches resolution exceptions and returns fallback text, so callers should pass the intended key string as fallback when they want missing localization to display the key.
- `LocalizationSettings.asset` contains Unity `SerializeReference` YAML lines where trailing spaces are significant, including `data: ` and `- ` empty values. Blanket trimming those spaces has previously broken localization initialization and caused PlayMode resolution tests to time out.
- The seed `Anemora_Strings` table currently contains the seven entries listed above. Additional scene or dialogue assets may still rely on fallback behavior until their keys are added to the table.
