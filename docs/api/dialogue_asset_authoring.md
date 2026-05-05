# DialogueAsset Authoring Guide

Status: Draft for Stage 3 contributor onboarding

Last source scan: 2026-05-05

## 1. Scope

This guide is a practical walkthrough for adding a new NPC dialogue asset. It complements `docs/api/dialogue_localization.md`, which remains the source of truth for the implemented API surface and localization asset structure.

This document only describes files and APIs that exist in the repository at the source scan date. It intentionally uses placeholder keys and placeholder table values. Do not add final lore, NPC backstory, protagonist names, or finalized dialogue text through this guide unless those decisions have already been approved elsewhere.

Files scanned:

- `docs/api/dialogue_localization.md`
- `Assets/Scripts/Game/Dialogue/DialogueAsset.cs`
- `Assets/Scripts/Dialogue/DialogueDisplay.cs`
- `Assets/Scripts/Dialogue/NpcInteractable.cs`
- `Assets/ScriptableObjects/Dialogues/Resident_A_Greeting.asset`
- `Assets/ScriptableObjects/Dialogues/Resident_B_Idle.asset`
- `Assets/Localization/StringTables/Anemora_Strings*.asset`
- `Assets/Tests/PlayMode/DialogueAssetIntegrationTests.cs`
- `Assets/Tests/PlayMode/NpcDialogueFlowTests.cs`
- `Assets/Tests/PlayMode/LocalizationSettingsResolutionTests.cs`
- `Assets/Tests/EditMode/DialogueAssetDataTests.cs`

## 2. Current Authoring Surface

| Area | Path / API | Authoring role |
|---|---|---|
| Dialogue SO type | `Anemora.Game.Dialogue.DialogueAsset` | Root `ScriptableObject` for one NPC dialogue asset. |
| Create menu | `Create > Anemora > Dialogue` | Unity Editor menu from `[CreateAssetMenu(menuName = "Anemora/Dialogue")]`. |
| Dialogue variant | `DialogueVariantSO` | Holds one variant id, turns, required flags, and excluded flags. |
| Dialogue turn | `DialogueTurnSO` | Holds `speakerId`, localized text, choices, and fallback helper. |
| Dialogue choice | `DialogueChoiceSO` | Holds optional choice label, emotion id, and next turn id. |
| Text reference | `UnityEngine.Localization.LocalizedString` | Points to a table collection and entry key. |
| Text fallback | `DialogueTurnSO.GetLocalizedTextOrFallback(string fallback)` | Returns localized value outside batchmode, or the supplied fallback key when unresolved. |
| Choice fallback | `DialogueChoiceSO.GetLocalizedLabelOrFallback(string fallback)` | Same fallback pattern for choice labels. |
| NPC component | `Anemora.Dialogue.NpcInteractable` | Scene or prefab component that triggers a `DialogueAsset`. |
| NPC assignment field | `NpcInteractable.dialogueAsset` | Inspector field for the asset reference. |
| Display entry point | `DialogueDisplay.Show(DialogueAsset asset)` | Runtime display path used by `NpcInteractable.TryInteract()`. |
| Localization check | `LocalizationSettings.StringDatabase.GetLocalizedStringAsync(...)` | Test-facing way to verify StringTable entries by locale. |

Current live collection name:

```text
Anemora_Strings
```

Current placeholder pattern:

```text
dialogue.placeholder.<npc>.<topic>
```

Use exact, lowercase, stable ids for `<npc>` and `<topic>`, for example `<npc_id>` and `<topic_id>`. Existing scene assets currently include placeholder examples such as `dialogue.placeholder.resident_a.greet`; keep new authored content at the same placeholder level until text is approved.

## 3. Walkthrough

### Step 1: Create the DialogueAsset SO instance

In the Unity Editor:

1. Open the Project window.
2. Navigate to `Assets/ScriptableObjects/Dialogues/`.
3. Use `Create > Anemora > Dialogue`.
4. Name the asset:

```text
Assets/ScriptableObjects/Dialogues/<NPC_Name>_<Topic>.asset
```

Use PascalCase for the asset filename and stable snake_case ids inside the asset. Example structure:

| Field | Value pattern | Notes |
|---|---|---|
| `npcId` | `<npc_id>` | Runtime id, not display text. |
| `variants[0].variantId` | `<topic_id>` | One initial variant is enough for a placeholder pass. |
| `variants[0].requiredFlags` | Empty or approved action record ids | Leave empty unless a gated line is already designed. |
| `variants[0].excludedFlags` | Empty or approved flags | Leave empty unless exclusion behavior is already designed. |
| `turns[n].speakerId` | `dialogue.placeholder.<npc>.name` | `DialogueDisplay` resolves this through `Anemora_Strings`. |
| `turns[n].text` | `dialogue.placeholder.<npc>.<topic>` | `LocalizedString` reference, configured in Step 2. |
| `choices[n].label` | `dialogue.placeholder.<npc>.<topic>.choice.<choice_id>` | Optional. Only add choices when flow behavior is required. |
| `choices[n].nextTurnId` | `<npc_id>.<topic_id>.<turn_id>` | Optional. Current display path advances sequentially and does not branch yet. |

Keep placeholder values descriptive, not final prose. Use `[TBD: <short neutral description>]` in StringTables for unfinished content.

### Step 2: Set LocalizedString fields to Anemora_Strings keys

For each `DialogueTurnSO.text` and optional `DialogueChoiceSO.label` in the Inspector:

1. Set the table reference to `Anemora_Strings`.
2. Set the entry reference to the exact key.
3. Pass the same key as the fallback value in tests when verifying fallback behavior.

Code and tests can construct the same reference directly:

```csharp
var text = new LocalizedString(
    "Anemora_Strings",
    "dialogue.placeholder.<npc>.<topic>");
```

Current runtime behavior:

- In non-batchmode, `DialogueLocalization` resolves the `LocalizedString` through Unity Localization.
- In batchmode, no localization settings, or missing table entry cases, `GetLocalizedTextOrFallback(...)` returns the supplied fallback string.
- `DialogueDisplay` supplies the `LocalizedString` key itself as fallback, so unresolved dialogue displays the key rather than empty text.

### Step 3: Add StringTable entries for ja-JP and en

Preferred Editor path:

1. Open the Unity Localization Tables window.
2. Open the `Anemora_Strings` String Table Collection.
3. Add one shared key for each new dialogue or speaker key.
4. Fill both locale rows:
   - `ja-JP`
   - `en`
5. Use placeholder values until final text is approved:

```text
[TBD: <short neutral description>]
```

Direct asset editing is only for careful review or mechanical fixes. If editing YAML directly, keep this invariant:

1. Add the key to `Assets/Localization/StringTables/Anemora_Strings Shared Data.asset`.
2. Preserve one stable numeric shared `m_Id`.
3. Add matching `m_TableData` rows with the same `m_Id` to:
   - `Assets/Localization/StringTables/Anemora_Strings_ja-JP.asset`
   - `Assets/Localization/StringTables/Anemora_Strings_en.asset`
4. Do not blanket-trim Unity Localization YAML whitespace. `docs/api/dialogue_localization.md` records existing trailing-space caveats for `LocalizationSettings.asset`.

Some current scene dialogue keys intentionally rely on fallback until their table rows are added. For new authoring, add both locale rows in the same change when the key is expected to resolve to a placeholder value.

### Step 4: Assign the DialogueAsset to an NPC GameObject

For a scene object or prefab that should start dialogue:

1. Add or find `Anemora.Dialogue.NpcInteractable`.
2. Assign the new asset to the `dialogueAsset` field.
3. Keep `interactionRange` at the local design value, or document any changed value in the relevant scene task.
4. Confirm the scene has one active `Anemora.Dialogue.DialogueDisplay`.
5. Confirm the player object has tag `Player`, because `NpcInteractable` uses `GameObject.FindWithTag("Player")`.

Runtime path:

```text
NpcInteractable.TryInteract()
  -> DialogueDisplay.Show(dialogueAsset)
  -> first DialogueVariantSO that has turns
  -> DialogueTurnSO.GetLocalizedTextOrFallback(key)
```

The current `DialogueDisplay` advances sequentially through turns. It does not yet branch on `DialogueChoiceSO.nextTurnId`.

### Step 5: Verify with PlayMode tests

Use existing PlayMode tests as the template:

| Test file | What it currently verifies |
|---|---|
| `Assets/Tests/PlayMode/NpcDialogueFlowTests.cs` | Scene NPC instances, assigned `DialogueAsset`, interaction, displayed fallback keys, and close/advance behavior. |
| `Assets/Tests/PlayMode/DialogueAssetIntegrationTests.cs` | `DialogueAsset` can hold a `LocalizedString` dialogue tree and fallback key values. |
| `Assets/Tests/PlayMode/LocalizationSettingsResolutionTests.cs` | `Anemora_Strings` resolves placeholder keys for `ja-JP` and `en`, and missing keys fall back to the provided key string. |

For a new NPC or topic, add a narrow PlayMode assertion based on `NpcDialogueFlowTests`:

1. Load `Anemora_Main`.
2. Find the NPC GameObject by stable scene name.
3. Get `NpcInteractable`.
4. Assert `dialogueAsset.npcId`.
5. Assert variant id and turn count.
6. Assert each `LocalizedString` key or fallback string.
7. Place the player in range and call `TryInteract()`.
8. Assert `DialogueDisplay.CurrentSpeaker` and `DialogueDisplay.CurrentText`.

Do not assert final narrative text while content remains TBD. Assert keys or `[TBD: ...]` placeholder values only.

### Step 6: Verify batchmode fallback behavior

Use the standard Unity Test Runner batchmode path from `docs/VERIFICATION_SUITE.md`:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" `
  -batchmode `
  -projectPath "C:\Users\maro6\Documents\Unity\Anemora" `
  -runTests `
  -testPlatform PlayMode `
  -testResults dialogue_playmode_results.xml `
  -quit
```

Expected batchmode behavior:

- `DialogueTurnSO.GetLocalizedTextOrFallback(key)` returns `key`.
- `DialogueChoiceSO.GetLocalizedLabelOrFallback(key)` returns `key`.
- `DialogueDisplay.CurrentText` can show the unresolved key when running the current fallback path.
- `LocalizationSettingsResolutionTests` still validates non-batch-style StringDatabase resolution by awaiting `LocalizationSettings.InitializationOperation` and `GetLocalizedStringAsync(...)`.

When debugging a missing key, check both outcomes:

1. The PlayMode/batchmode fallback path displays the exact key and does not return empty text.
2. The StringTable resolution path returns the `[TBD: ...]` locale row for `ja-JP` and `en` once table rows exist.

## 4. Authoring Checklist

- New SO lives under `Assets/ScriptableObjects/Dialogues/`.
- Asset filename follows `<NPC_Name>_<Topic>.asset`.
- `npcId` is a stable snake_case id.
- At least one `DialogueVariantSO` has at least one `DialogueTurnSO`.
- Every `LocalizedString` table reference is `Anemora_Strings`.
- Every dialogue key follows `dialogue.placeholder.<npc>.<topic>` or an approved final key pattern.
- `ja-JP` and `en` table rows are added together when localized placeholder values are expected.
- `NpcInteractable.dialogueAsset` is assigned.
- No final lore text is introduced unless approved by a separate content decision.
- PlayMode or batchmode verification covers the new asset path.

## 5. References

- `docs/api/dialogue_localization.md`
- `docs/VERIFICATION_SUITE.md`
- `Assets/Scripts/Game/Dialogue/DialogueAsset.cs`
- `Assets/Scripts/Dialogue/DialogueDisplay.cs`
- `Assets/Scripts/Dialogue/NpcInteractable.cs`
- `Assets/ScriptableObjects/Dialogues/Resident_A_Greeting.asset`
- `Assets/ScriptableObjects/Dialogues/Resident_B_Idle.asset`
- `Assets/Tests/PlayMode/NpcDialogueFlowTests.cs`
- `Assets/Tests/PlayMode/DialogueAssetIntegrationTests.cs`
- `Assets/Tests/PlayMode/LocalizationSettingsResolutionTests.cs`

## 6. Change History

| Version | Date | Notes |
|---|---|---|
| v0.1 | 2026-05-05 | Initial draft for DialogueAsset authoring walkthrough. |
