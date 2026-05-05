# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

Stage 3 Vertical Slice closeout notes. Stage 3 is complete as of 2026-05-06 after G5 manual confirmation and the latest demo brush repair. Stage 3 /spec resolution selected Niro and Antela as provisional names, kept the code license as All Rights Reserved by default, and locked the planned public release direction to Steam Early Access.

Milestone groups covered in this section:

| Group | Scope |
|---|---|
| Core loop / G5 | Automated G5, audio rebuild, performance baseline, manual review, closeout docs |
| Audio | Zone1 BGM, SFX 30, controller wiring, audio tests, SFX ledger consolidation |
| Dialogue / localization | G3 dialogue, LocalizationSettings, locale switching, DialogueAsset authoring |
| Lore / public materials | Niro / Antela reflection, public pitch, trailer script, README polish |
| Verification / QA | Test-count reconciliation, save/load coverage, stress harness, URP warning investigation |
| Documentation / onboarding | API docs, scene tour, asset structure, notices, contributing, devlog index |
| Stage 4 entry | Stage 3 retrospective, Stage 4 roadmap, Phase 0 triage, code license options |

### Added

| Commit | Entry |
|---|---|
| This task | Added a runtime brush tutorial hint that switches between create, release, and close affordances for the local time-window flow. |
| This task | Added the user-approved Stage 4 character v2 sprite sets for Niro / Hero, Resident_A, and Resident_B. |
| `4029cc0` | Added G3 partial NPC placement and dialogue UI scaffold in `Anemora_Main`, including Resident_A / Resident_B instances and `NpcDialogueFlowTests`. |
| `2f3197b` | Added LocalizationSettings, ja-JP / en locale assets, StringTable seed, Addressables registration, and localization resolution PlayMode tests. |
| `d2ae62a` | Added Save/Load round-trip E2E PlayMode coverage for ActionRecord reflection persistence. |
| `ec1bbb0` | Added locale switch dialog E2E PlayMode coverage. |
| `6809c4b` | Added Zone1 audio implementation: `Zone1_Ambient.ogg`, 30 Zone1 SFX assets, and `Zone1AudioController` wiring. |
| `b9daccb` | Added Zone1 audio PlayMode wiring tests. |
| `5f45a29` | Added combined Save/Load + Locale switch integration PlayMode coverage. |
| `19e992e` | Added stress sample harness design and skeleton for Stage 4 performance / repeat-interaction verification. |
| `da6040f` | Added G3 final dialogue draft for the Vertical Slice. |

### Changed

| Commit | Entry |
|---|---|
| This task | Switched the Hero, Resident_A, and Resident_B prefabs and animation clips from provisional v1 sprites to the Stage 4 v2 sprite sets. |
| `f486c28` | Revised `docs/VS_SCOPE.md` to v0.2 with E0-E5 / A1-A3 / F1-F4 / G4 completion state and core-loop minimum achievement. |
| `2cf0dfa` | Revised ADR-0008 to v0.3 with LocalizationSettings completion, StringTable seed, Addressables setup, and batchmode fallback details. |
| `9f1d5c7` | Revised `docs/VS_SCOPE.md` to v0.3 with Audio completion, G3 Localization completion, and G5 as the remaining completion gate. |
| `50ab8c0` | Consolidated asset ledger review for Zone1 SFX 30 entries. |
| `c0cb631` | Reflected Stage 3 /spec resolution interview decisions across planning docs: Niro / ニロ (provisional), Antela / アンテラ (provisional), gender-neutral protagonist expression, 15-19 age range, no family/acquaintance setup, Resident_A / Resident_B VS roles, Stage 3 provisional art/font/palette adoption, ADR-0009 Accepted, All Rights Reserved default, and Steam Early Access release direction. |
| `63de141` | Recorded Stage 3 TBD resolution closure and moved remaining re-evaluation items toward Stage 4. |
| `127da7d` | Updated CONCEPT v1.4 to mark internal design vocabulary as planning-only and keep public-facing text focused on what the player sees and changes. |
| `e6e3c61` | Rebuilt the G5 Windows build with audio and refreshed `docs/G5_ACCEPTANCE_MATRIX.md` section K metrics. |
| `df19870` | Recorded audio-loaded performance baseline v0.2 with build size, FPS, memory, and known URP warning counts. |
| `3d11e4b` | Updated `docs/scene_tour_anemora_main.md` to v0.2 with Niro lore and audio scene wiring. |
| `7ab3499`, `1d5b078` | Polished README after lore reflection and Stage 4 entry updates. |
| `a0bd50b` | Repaired the demo playable time-window brush flow: drag preview now matches generated time-window center / size / floor footprint, with right-click deletion preserved. |

### Fixed

| Commit | Entry |
|---|---|
| `73d4808` | Applied ADR-0001 through ADR-0009 review pass fixes and documented remaining ADR consistency candidates. |
| `5a1a39b` | Applied audio prompts integration check fixes across audio prompt / ADR / notice references. |
| `1c7ac12` | Reconciled the EditMode 31 vs 32 test-count baseline across verification docs. |
| `c72a79c` | Applied docs cross-link audit fixes for path drift and documented remaining larger sync candidates. |
| This task | Replaced `PortalStencilFeature`'s URP internal `DrawObjectsPass` usage with public `RenderObjectsPass`, added a PlayMode warning-count assertion, and verified player log warning count `0`. |

### Documentation

| Commit | Entry |
|---|---|
| This task | Promoted Stage 4 character art scope so Niro / Hero v2 is a full redraw, with Resident_A / Resident_B review decisions tracked separately. |
| This task | Recorded the character v2 import, asset ledger rows, and user-approved pixel granularity direction for Hero / Resident_A / Resident_B. |
| This task | Recorded the post-runtime Resident_A v2 follow-up: Hero and Resident_B are the accepted reference; Resident_A needs reduced pixel harshness and face/head scale adjustment. |
| This task | Added a Resident_A follow-up review sheet with A/B/C concept options; no runtime asset replacement yet. |
| This task | Recorded the Stage 4 TMP / palette readability review and next screenshot gate before font or UI asset replacement. |
| `33a2507`, `8bd0d01` | Added and updated `docs/devlog/INDEX.md` for devlog navigation and CHANGELOG cross-reference. |
| `34112eb` | Added Stage 3 review aids for high-impact visual review items. |
| `4417663` | Added `docs/G5_PREFLIGHT.md` for G5 Go / No-Go preparation. |
| `8311815` | Added `NOTICES.md` for third-party license aggregation and AI asset disclosure summary. |
| `b6b60db` | Added `docs/VERIFICATION_SUITE.md` with automated test catalog and G5 matrix cross-reference. |
| `bb72f12` | Added `docs/legal/code_license_options.md` for Stage 4 code license decision support. |
| `ac8879e` | Added `docs/ASSET_STRUCTURE.md` as the canonical asset layout reference. |
| `9aa1828` | Added `docs/EDITOR_AUTOMATION.md` for editor automation usage. |
| `a40fd28` | Added `docs/api/save_load.md` for Save/Load architecture onboarding. |
| `256b231` | Added `CONTRIBUTING.md` draft for Stage 4 entry preparation. |
| `2e3569f` | Recorded Stage 3 Day 1 performance baseline v0.1 for G5 reference. |
| `cc72aa7` | Added `docs/api/dialogue_localization.md` for dialogue localization API onboarding. |
| `e111da8` | Added initial `docs/scene_tour_anemora_main.md` walkthrough. |
| `c4b1fd4` | Added DialogueAsset authoring guide. |
| `c17d62f` | Recorded G5 automated sections, test/build results, and matrix updates. |
| `6c25875` | Added `docs/G5_MANUAL_RUBRIC.md` for user manual G5 review. |
| `ecc4656` | Added `docs/PITCH_PUBLIC.md` v0 as Steam Early Access / press-kit source text. |
| `1b15880` | Added `docs/TRAILER_SCRIPT.md` v0 for 30s / 60s / 180s trailer planning. |
| `026bf1f` | Investigated the URP `DrawObjectsPass` RenderGraph warning and documented Stage 4 fix options. |
| `34585e3` | Added `docs/STAGE3_RETROSPECTIVE.md` v0.1 preliminary draft. |
| `a49ee52` | Added `docs/STAGE4_ROADMAP.md` v0.1 preliminary draft. |
| This task | Promoted Stage 3 closeout docs: `docs/G5_ACCEPTANCE_MATRIX.md`, `docs/VS_SCOPE.md`, `docs/STAGE3_RETROSPECTIVE.md`, `docs/STAGE4_ROADMAP.md`, `docs/G5_PREFLIGHT.md`, `docs/VERIFICATION_SUITE.md`, and `docs/devlog/2026-05-06_stage3_closeout.md`. |
| This task | Added `docs/STAGE4_PHASE0_TRIAGE.md` and `docs/devlog/2026-05-06_stage4_phase0_triage.md` to classify Stage 4 immediate fixes, backlog items, and no-action items. |

### Removed

- No entries.

## [0.1.0-alpha.1] - 2026-05-05

Stage 3 Day 1 milestone navigation draft. This is not a public release tag yet; it summarizes pushed work that will become the source for later GitHub Public release notes.

### Added

#### Engine / Pipeline

- E0: Added URP pipeline setup and `AnemoraE0Setup.cs` editor automation for Stage 3E bootstrap. (commit f854466)
- E1: Added `PortalStencilFeature`, `PortalMask` / `InsideOnly` shaders, stencil bit 3 (`Mask=8`), and dual-pass portal rendering defense. (commit 773d35f)
- E2: Added persistent Current / Past hierarchy skeleton, `SceneRootRegistry`, and `Camera_Past` scaffolding. (commit 773d35f)
- E3: Added `SymbolWheel` prefab and controller with three symbols and red-only activation for the initial prototype. (commit 773d35f)
- E4: Added `PortalCrossingDetector`, `SceneSidePolarity`, `PortalVisualSwitcher`, `PortalFlashPlayer`, `TimeFramePortalController`, six-state portal state machine, and atomic Current / Past flip. (commit 11c9590)
- E5: Added ActionRecord integration with `IReflector`, `BookReflector`, `ActionRecordCatalog`, and `ActionRecordRuntime`. (commit 61edb4e)

#### Scene / Wiring

- A2: Wired the real `Anemora_Main` scene with Player, `PortalSpawnPoint`, `SymbolWheel`, `TimeFramePortalSystem`, `PrototypePlayerController`, and boundary round-trip PlayMode coverage. (commit cb2b6ed)
- G4: Added the `take_book_001` ActionRecord trigger path with catalog entry, `Book_Family_Current.prefab`, `PastBookInteractable`, `Anemora_Main` wiring, and E2E PlayMode coverage; this established the minimum VS_SCOPE §3.1 core loop. (commit 0644822)

#### Assets

- F1: Added PixelLab Hero / Resident_A / Resident_B draft sprite generation outputs, including intermediate PNG review material and selected draft sprites. (commit 4a420a5)
- F2: Added palette-finished Hero and Resident_A/B v1 sprite sheets, then re-exported with the Steam version of Aseprite for the final indexed PNG pass. (commit 08f61b8; related commit 4d2092a)
- F4: Added Hero / Resident_A / Resident_B prefabs, animator controllers, animation clips, `HeroAnimatorBinder`, and prefab / PlayMode tests for basic locomotion state machines. (commit d2c95c2)
- A3: Added Zone1 building asset set from Meshy v6 through Blender repair and Unity import, including atlas, manifest, and helper scripts. (commit a547e96)

#### Localization / UI

- A1: Added the DialogueAsset two-layer structure with `Anemora.Data` POCOs, `Anemora.Game` asmdef boundary, DialogueAsset ScriptableObject workflow, and `com.unity.localization@1.5.9`. (commit 523c048)
- UI foundation v0: Added Anemora palette v0 and Japanese TMP atlas draft using Misaki Gothic, including palette sheet and atlas measurement devlogs. (commit a8f2710)
- A5: Added TMP English atlas draft using Press Start 2P and configured JP / EN fallback chain for mixed-language text. (commit f5b4685)

#### Documentation

- Added Accepted ADR coverage for ADR-0001 through ADR-0008 as the Stage 3 Day 1 implementation baseline. (commit 8a3505c)
- Added ADR-0009 draft for asset pipeline formalization with `Status: Proposed`. (commit cbb6ac1)
- Added README draft for GitHub Public release preparation while leaving public release date, code license, protagonist name, and formal Zone 1 name as TBD. (commit eb70049)
- Added G5 acceptance matrix draft with 36 verification items for VS final review. (commit 7c4a258)
- Added SFX 30-entry prompt detail in `docs/asset_prompts/sfx_zone1.md` v1.0 draft for ElevenLabs SFX / Stable Audio / Studio One production. (commit ed7833c)

### Changed

- Revised ADR-0002 to v1.1 with confirmed E1 stencil values, stencil bit 3, dual-pass defense, and URP StencilLight conflict history. (commit 02f5c22)
- Revised ADR-0005 to v1.1 with E4 confirmed values: hysteresis `0.02m`, minimum movement `0.05m`, cooldown `0.1s`, and flash `0.05s`. (commit 3a29757)
- Revised ADR-0008 localization boundaries to clarify the `DialogueAssetData` POCO vs `DialogueAsset` ScriptableObject two-layer split and asmdef dependencies. (commit 8a3505c)
- Corrected audio-production documentation from Reaper to Studio One across the asset pipeline notes. (commit 3a29757)
- Documented the TMP Japanese atlas v0 capacity measurement and 70 missing glyphs for later Stage 4 localization follow-up. (commit a8f2710)
- Updated `docs/STAGE3_TBD_RESOLUTION.md` as a Stage 3 tracking sheet while keeping unresolved lore-sensitive items as TBD. (commit 18271bc)

### Fixed

- Fixed EditMode data test assembly references during the Stage 3 data / save foundation work. (commit 8714af9)

## Roadmap

- Stage 3 Vertical Slice: complete as of 2026-05-06 after G5 manual confirmation and latest demo brush repair.
- Stage 4: G5 result reflection, quality reinforcement, zone expansion planning, localization/content polish, art/font revision as needed, and Steam Early Access submission preparation.
- Stage 5+: Steam Early Access feedback / full release planning.

[Unreleased]: https://github.com/marvelousu/anemora/compare/v0.1.0-alpha.1...HEAD
[0.1.0-alpha.1]: https://github.com/marvelousu/anemora/releases/tag/v0.1.0-alpha.1
