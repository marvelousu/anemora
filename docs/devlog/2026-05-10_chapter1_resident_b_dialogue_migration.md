# Chapter 1 Resident_B Dialogue Migration (2026-05-10)

## Summary

graphics foundation orchestrator review の #7 に沿って、Stage 3 の `Resident_B_Idle.asset` を Chapter 1 scene 1 v4 の Reto dialogue に差し替え、section 別 DialogueAsset と `dialogue.scene1.reto.*` keys へ移行した。

## Changed

- Renamed `Resident_B_Idle.asset` to `Resident_B_Scene1_B_Initial.asset` while preserving the original GUID.
  - `variantId`: `scene1_b_initial`
  - keys: `dialogue.scene1.reto.b_initial.line_1` through `line_3`
- Added section DialogueAssets:
  - `Resident_B_Scene1_C_LibraryHistory.asset`
  - `Resident_B_Scene1_D_BrushReaction.asset`
  - `Resident_B_Scene1_F_BookAppears.asset`
  - `Resident_B_Scene1_G_MiaHint.asset`
- Migrated Resident_B localization keys away from `dialogue.encounter.present_resident_b.*`.
- Added 17 `dialogue.scene1.reto.*` StringTable entries across shared / ja-JP / en tables.
- Kept `Anemora_Main.unity` compatible by preserving the Resident_B DialogueAsset GUID; the scene reference now resolves to `Resident_B_Scene1_B_Initial.asset`.
- Updated `NpcDialogueFlowTests`, `LocalizationSettingsResolutionTests`, and `Chapter1DialogueAssetTests`.
- Updated current dialogue / asset docs to point to the new Resident_B assets and keys.

## Verification

- Unity batchmode import/compile smoke completed:
  - `Tundra build success`
  - Unity process exited with return code `0`.
- Known warning remains in `NpcDialogueFlowTests`: `Object.FindObjectOfType(Type)` is obsolete in Unity 6000.3.14f1. This is existing test API usage and not a dialogue migration compile failure.
- Later hardening pass: `Chapter1DialogueAssetTests` now runs `7/7` and verifies the removed Stage 3 Resident_A / Resident_B assets are absent, the legacy `dialogue.encounter.*` prefixes are absent from shared table data, and all Chapter 1 dialogue keys have shared/en/ja table entries.
  - XML: `<temp>\anemora_ch1_impl_chapter1_dialogue_asset_tests_after_key_migration.xml`

## Notes

- Reto v8 expression / motion data is not represented in the current `DialogueAsset` schema. This pass wires text assets and localization keys only; character visual / animation carry-forward remains with the character generation session.
- Section C / D / F / G assets are available for Chapter 1 scene assembly. Current `NpcInteractable` still carries one `dialogueAsset`; non-initial section selection needs scene triggers or a dialogue runner in the later scene wiring pass.
- Historical Stage 3 devlogs and G5 preflight docs still mention `Resident_B_Idle.asset`; those were left unchanged as historical records.

## Next

- #8 / #9 can use `Resident_B_Scene1_B_Initial.asset` for the default scene 1 Reto interaction and wire C / D / F / G through section-specific triggers.
