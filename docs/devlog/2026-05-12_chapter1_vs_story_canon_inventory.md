# Chapter 1 VS Story Canon Inventory

Date: 2026-05-12

Scope: current playable VS path from Niro house interior through central plaza to the Scene 1 library/Reto event. Production TimeWindow remains frozen, so TimeWindow-dependent beats are tracked as blocked rather than treated as complete.

Canonical source priority for this pass:

- `docs/STAGE3_G_PLAN.md`
- `docs/STAGE3_TBD_RESOLUTION.md`
- `docs/localization/glossary.md`
- `docs/devlog/2026-05-09_chapter1_scene1_v3_final.md`
- `<notes>\games\anemora\docs\draft\chapter1_s1_s2_handover_2026-05-08.md`
- `Assets/ScriptableObjects/Dialogues/Resident_B_Scene1_*.asset`
- `Assets/Localization/StringTables/Anemora_Strings Shared Data.asset`
- `Assets/Localization/StringTables/Anemora_Strings_ja-JP.asset`

Conflict note: `chapter1_s1_s2_handover_2026-05-08.md` contains later revision history that reopens some Scene 1 ideas, while the current user request explicitly names `2026-05-09_chapter1_scene1_v3_final.md`. This inventory treats the v3 final document as the current VS story reference until the user explicitly selects a later revision.

| Beat | Canon source | Required line/key/content | Current implementation | Status | Required action |
|---|---|---|---|---|---|
| Opening D-3 | `docs/localization/glossary.md` 7.1, `docs/STAGE3_G_PLAN.md` 4.1 | `夢を見ていたような、夢を見ていなかったような。` | `Chapter1UiText.OpeningD3` shows the exact line. | Implemented | Keep. Log key/id in smoke evidence. |
| Opening D-7 | `docs/localization/glossary.md` 7.3, user follow-up | Visual-only hand/self-check; no text. User rejected forced placeholder. | `Chapter1OpeningFlowController.showD7Placeholder` is false in the current flow. | Disabled intentionally | Keep disabled until final art/animation exists. Do not replace with text. |
| Opening D-6 weak | `docs/localization/glossary.md` 7.2 | `(なんとなく、重い)` and removable/configurable. | `Chapter1UiText.OpeningD6Weak`; `showD6Weak` toggle exists. | Implemented but optional | Keep configurable. |
| Door Timewriter/pen beat | `docs/localization/glossary.md` 7.4, `docs/STAGE3_G_PLAN.md` G1, `docs/STAGE3_TBD_RESOLUTION.md` STORY-02 | `(ポケットに、何か入っている)`; should read as Niro noticing an artifact he already has, not discovering it for the first time. Movement locks until dismissed. | `Chapter1OpeningFlowController.TimewriterDoorBeatRoutine` calls `Chapter1PlayableFlowController.ShowFeedback(...)` with the canonical text, but presentation is a bottom prompt/log panel and user reports it feels like a freeze. | Text implemented, presentation mismatched | Move this beat to a styled story/narration UI path and log `opening.timewriter_pocket_beat`. Keep text exact. |
| House objective text | Current VS implementation; no final story line source found | Japanese objective to exit house. | `Chapter1UiText.ObjectiveHouseExit` etc. | Temporary navigation copy | Keep utilitarian but do not treat as story canon. Avoid English/debug style. |
| Scene 1 [1.B] Reto initial | `2026-05-09_chapter1_scene1_v3_final.md`; `Resident_B_Scene1_B_Initial.asset` | `dialogue.scene1.reto.b_initial.line_1` = `...見ない顔ですね`; line_2 = `私はレト。元、教師でした`; line_3 = `今は、ここで街の記録を残しています` | Asset exists, but current library event does not show this asset in the player path. | Missing from current VS event path | Wire Reto event to begin with `Resident_B_Scene1_B_Initial`. |
| Scene 1 [1.C] library history | `2026-05-09_chapter1_scene1_v3_final.md`; `Resident_B_Scene1_C_LibraryHistory.asset` | Six keys `dialogue.scene1.reto.c_library_history.line_1` through `line_6`; includes record gathering, names/families/decisions, nobody remembers, scraps of stories, and invitation about when books existed. | Asset exists, but current library event does not show this asset in the player path. | Missing from current VS event path | Wire after [1.B]. |
| Scene 1 [1.C] Niro inner thought | `2026-05-09_chapter1_scene1_v3_final.md` | `(...誰も)` and `(...からっぽ)` are canonical Niro thoughts around the empty shelf. | No dedicated current asset/key found. Library endpoint uses `Chapter1UiText.LibraryEndpointCompleted = "(...あった)"`, which is not the v3-final shelf thought. | Mismatched/temporary | Replace endpoint feedback with canonical inner thought if a key exists; otherwise mark as temporary missing key and avoid claiming story-complete. |
| Scene 1 [1.D] Timewriter activation | `2026-05-09_chapter1_scene1_v3_final.md`; TimeWindow runtime recovery lane | Niro takes out Timewriter; Reto sees only `...?`; red light toward empty shelf; tutorial follows. | Production TimeWindow is frozen; `Resident_B_Scene1_D_BrushReaction.asset` exists with key `dialogue.scene1.reto.d_brush_reaction.line_1`, but current path does not present the full beat. | Blocked by TimeWindow freeze | Keep out of production replacement until TimeWindow approval. Do not use current volume cue as final. |
| Scene 1 [1.E] past library observation | `2026-05-09_chapter1_scene1_v3_final.md` | Past library/shelves/Aria observation; Niro enters TimeWindow. | Not available in current production because TimeWindow is frozen. | Blocked | Track as missing for full Scene 1; not required for house/plaza camera patch. |
| Scene 1 [1.F] return to present | `2026-05-09_chapter1_scene1_v3_final.md` | No book appears; Reto weak lines: `...どうかしましたか?`, `...そうですか`. | `Resident_B_Scene1_F_BookAppears.asset` currently has four keys whose ja-JP text includes `...?`, `...本物だ`, `...そうですか`, `...あなたのような方が、来てくれるとは`; this conflicts with v3 final. | Mismatched | Do not use this asset as final v3 line set. Replace or mark blocked/mismatched in tests. |
| Scene 1 [1.G] Mia hint | `2026-05-09_chapter1_scene1_v3_final.md`; `Resident_B_Scene1_G_MiaHint.asset` | `...そういえば`; `中央集落のミアさんが、今朝、困っていました`; v3 final asks `もし手があるなら、少し、助けてやってください`. | Asset exists. Current ja-JP line_3 is `あなたなら、力になれるかもしれません`, matching an older/later variant rather than v3 final. Current event uses this asset only. | Partially implemented, line_3 mismatch | Update line_3 or document conflict before user review. |
| Reto visual state mapping | User correction, current Reto graphics pass | Normal/after dialogue = raised/writing loop; during dialogue = lowered hands. | `RetoLibraryEventVisualController` implements `raised_writing_loop`, `lower_arms`, `lowered_dialogue_loop`, `raise_arms`. | Implemented | Preserve. Add smoke lines for before/during/after state. |
| Reto movement lock | User blocker fixed baseline | Movement must remain blocked after one Enter advance while dialogue visible. | `DialogueDisplay` holds interaction lock; smoke driver has real-input probe. | Implemented | Preserve and keep multi-frame proof in smoke. |
| VS end condition | User requirement | VS ends after the library event, not mere endpoint inspection. | `VsLibraryEventCompleteFlag` exists; objective uses it. Current story content is incomplete because Reto event only shows Mia hint. | Mechanics implemented, story incomplete | Keep end flag tied to event completion, but do not mark story-complete until [1.B]/[1.C]/[1.G] are shown or missing beats documented. |
