# 2026-05-05 G3 Partial NPC Dialog Scaffold

## Scope

- Placed the F4 Resident_A and Resident_B prefabs into the Past side of `Anemora_Main.unity`.
- Added the first dialogue interaction scaffold around the A1 `DialogueAsset` ScriptableObject structure.
- Kept localization settings, real dialogue text, lore content, indicator polish, animation, and SFX out of scope.

## Implementation

- Added `Resident_A_Greeting.asset` and `Resident_B_Idle.asset` under `Assets/ScriptableObjects/Dialogues/`.
- Each DialogueAsset uses one variant with two turns.
- Dialogue text is stored as `LocalizedString` references to the `Anemora_Strings` table.
- Turn `speakerId` values use placeholder string-table keys and contain no lore text.
- Added `NpcInteractable`:
  - caches the Player by tag,
  - checks horizontal interaction range,
  - opens `DialogueDisplay` on `E`.
- Added `DialogueDisplay`:
  - active scene singleton,
  - hidden visual panel at startup,
  - resolves turn text through `DialogueTurnSO.GetLocalizedTextOrFallback`,
  - resolves speaker placeholder keys through the same fallback policy,
  - advances with Space, E, or Return,
  - freezes the prototype player while dialogue is visible.
- Added `Anemora.Dialogue.asmdef` because A1 keeps `Anemora.Game.asmdef` non-auto-referenced.
- Added a minimal `DialoguePanel.prefab` with TMP speaker, body, and advance indicator fields.
- Added `PrototypePlayerController.SetMovementFrozen(bool)` and `IsMovementFrozen`.

## Scene Wiring

- `Resident_A_Instance`
  - Parent: `Root_Past`
  - Local position: `(-0.85, 0.02, 1.05)`
  - Layer: 11
  - DialogueAsset: `Resident_A_Greeting.asset`
- `Resident_B_Instance`
  - Parent: `Root_Past`
  - Local position: `(1.25, 0.02, 0.85)`
  - Layer: 11
  - DialogueAsset: `Resident_B_Idle.asset`
- `DialogueCanvas`
  - Screen Space - Camera
  - Main Camera assigned
  - `DialoguePanel.prefab` instance as child.

## Placeholder Keys

- `dialogue.placeholder.resident_a.name`
- `dialogue.placeholder.resident_a.greet`
- `dialogue.placeholder.resident_a.greet_2`
- `dialogue.placeholder.resident_b.name`
- `dialogue.placeholder.resident_b.idle`
- `dialogue.placeholder.resident_b.idle_2`

## Verification Plan

- `NpcDialogueFlowTests` loads `Anemora_Main`, verifies NPC placement and DialogueAsset key wiring, opens Resident_A dialogue by direct interact call, advances through both lines, and confirms player freeze/unfreeze.
- Existing PlayMode tests should remain green, including portal round-trip and G4 action record reflection.
