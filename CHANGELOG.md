# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

Stage 3 is still in progress. Public release, code license, protagonist name, and the formal Zone 1 name are TBD.

### Added

### Changed

### Fixed

### Removed

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

- Stage 3 Vertical Slice completion: G3 review / integration, G5 verification, and Audio finalization (BGM + SFX) remain. F4 prefab / animator work is already pushed in this milestone.
- Stage 4: Zone 2 expansion, localization completion, Steam submission preparation, and GitHub Public release decision.
- Stage 5+: TBD.

[Unreleased]: https://github.com/marvelousu/anemora/compare/v0.1.0-alpha.1...HEAD
[0.1.0-alpha.1]: https://github.com/marvelousu/anemora/releases/tag/v0.1.0-alpha.1
