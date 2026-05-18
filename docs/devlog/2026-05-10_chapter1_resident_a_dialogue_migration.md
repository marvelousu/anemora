# Chapter 1 Resident_A Dialogue Migration (2026-05-10)

## Summary

graphics foundation orchestrator review の #6 に沿って、Stage 3 の `Resident_A_Greeting.asset` を削除し、Chapter 1 のシーン 1 [1.E] / シーン 3 [3.D] 用 DialogueAsset に分割した。

## Changed

- Deleted `Assets/ScriptableObjects/Dialogues/Resident_A_Greeting.asset`.
- Added `Resident_A_Past_Library_Glimpse.asset`.
  - `variantId`: `scene1_e_library_glimpse`
  - key: `dialogue.scene1.past_aria.glimpse_1`
  - speaker: `dialogue.speaker.niro`
- Added `Resident_A_Past_AriaHouse_Lesson.asset`.
  - `variantId`: `scene3_d_aria_house_lesson`
  - keys: `dialogue.scene3.aria_house.*`
  - speakers: `dialogue.speaker.resident_j` / `dialogue.speaker.resident_a`
- Migrated Resident_A localization keys away from `dialogue.encounter.past_resident_a.*`.
- Added `dialogue.speaker.resident_j` for Karla.
- Updated `Anemora_Main.unity` Resident_A reference to the scene 1 library glimpse asset so the Stage 3 reference scene does not hold a missing GUID.
- Added `Chapter1DialogueAssetTests` for the new Resident_A assets.
- Updated dialogue docs that describe current assets and current StringTable keys.

## Verification

- Unity batchmode compile/import smoke completed:
  - `Tundra build success`
  - `Application will terminate with return code 0`
- Targeted EditMode test command was attempted, but Unity 6000.3.14f1 exited after AssetDatabase refresh without producing a Test Runner XML. No compile errors remained after adding `Unity.Localization` / `Unity.ResourceManager` references to the EditMode test asmdef.
- Later hardening pass: `Chapter1DialogueAssetTests` now runs `7/7` and verifies the removed Stage 3 Resident_A / Resident_B assets are absent, the legacy `dialogue.encounter.*` prefixes are absent from shared table data, and all Chapter 1 dialogue keys have shared/en/ja table entries.
  - XML: `<temp>\anemora_ch1_impl_chapter1_dialogue_asset_tests_after_key_migration.xml`

## Notes

- Historical Stage 3 devlogs / G5 preflight documents still mention `Resident_A_Greeting.asset`; those were left unchanged as historical records.
- `Anemora_Main` remains a Stage 3 reference scene. The actual Chapter 1 production scene should be `Anemora_Chapter1.unity`.

## Next

- Continue #7: split `Resident_B_Idle.asset` into scene 1 [1.B] / [1.C] / [1.D] / [1.F] / [1.G] assets and migrate keys to `dialogue.scene1.reto.*`.
