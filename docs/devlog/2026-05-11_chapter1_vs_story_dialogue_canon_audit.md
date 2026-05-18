# Chapter 1 VS Story / Dialogue Canon Audit - 2026-05-11

Scope: current connected VS path through the library event. TimeWindow production files were not touched.

## Sources Checked

- `docs/STORY_BIBLE_v1.md`
- `docs/devlog/2026-05-09_chapter1_scene1_v3_final.md`
- `<notes>\_handover\anemora-chapter1-a-dialogue-details-2026-05-10.md`
- `<notes>\games\anemora\docs\draft\chapter1_s1_s2_handover_2026-05-08.md`
- `Assets/ScriptableObjects/Dialogues/Resident_B_Scene1_*.asset`
- `Assets/Localization/StringTables/Anemora_Strings_*.asset`
- `docs/api/dialogue_localization.md`

## Current VS Strings

| Runtime string | Status | Action |
| --- | --- | --- |
| `LibraryEndpointPrompt` / `RetoLibraryPrompt` / `RetoLibraryMissing` | Temporary VS interaction UI, not story dialogue. | Kept as prompt/feedback scaffolding. |
| `LibraryEndpointCompleted` | Temporary VS investigation log. Previous text was ad hoc. | Replaced with the documented Scene 1 [1.F] Niro inner text `(...あった)`. No localization key exists yet for this Niro line in the current tables. |
| `RetoLibraryCompleted` | Fallback only when DialogueDisplay is unavailable. Previous text was ad hoc. | Replaced with the existing localized text from `dialogue.scene1.reto.g_mia_hint.line_3`; the Reto event uses the canonical `Resident_B_Scene1_G_MiaHint.asset` DialogueAsset directly. |
| `ObjectiveLibraryInspect` / `ObjectiveLibraryTalkToReto` / `ObjectiveLibraryVsComplete` | Temporary VS objective UI. | Kept as non-diegetic objective text; not treated as final story dialogue. |

## Guards

- `LibraryRoute_Reto_EventPlaceholder` has `completedDialogueAsset = Resident_B_Scene1_G_MiaHint.asset`.
- PlayMode guard asserts the scene Reto event references that DialogueAsset and that interaction opens the canonical dialogue key path while locking player movement.
- EditMode guard `ResidentBMiaHintLineThree_UsesLatestSceneOneV4RestoredWording` asserts `dialogue.scene1.reto.g_mia_hint.line_3` remains the current v4-compatible line.

## Reto Line 3 Resolution

Known conflict: `docs/devlog/2026-05-09_chapter1_scene1_v3_final.md` records the v3-final [1.G] wording `もし手があるなら、少し、助けてやってください`, while the current localization table and `Resident_B_Scene1_G_MiaHint.asset` use `あなたなら、力になれるかもしれません`.

Resolution for this VS pass: keep `あなたなら、力になれるかもしれません`, because `<notes>\games\anemora\docs\draft\chapter1_s1_s2_handover_2026-05-08.md` later records the v4 restoration for [1.G] after the book-result beat was restored. No new final key or ad hoc replacement text was authored.
