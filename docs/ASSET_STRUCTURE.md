# Anemora Asset Structure

> Status: v0.3 draft (2026-05-06). This file records the directory layout observed in the working tree after Stage 4 character v2 import.

## 1. 概要

本 doc は Anemora repo の canonical asset layout reference です。複数セッションで asset を生成・配置するときの path 認識を揃え、Stage 4 GitHub Public 公開時の onboarding 資料として使うことを目的にします。

ADR-0009 `docs/adr/0009-asset-pipeline.md` は asset pipeline workflow を扱います。本 doc は directory layout と実 file の所在に限定します。

想定 reader は新規 contributor、Stage 4 onboarding、並列セッションで作業する agent です。作業前には本 doc を確認し、大きな構造変更がある場合は本 doc も更新してください。

## 2. リポジトリ root 構成

Scan basis: clean temporary worktree based on `origin/main`, 2026-05-05. Canonical project root remains `C:\Users\maro6\Documents\Unity\Anemora`; `Library/`, `Logs/`, `Temp/`, `.git/` は scan 表示から除外しました。

| Path | 用途 | 状態 |
|---|---|---|
| `Assets/` | Unity project asset root | tracked + 一部 untracked |
| `Packages/` | Unity package manifest / lock | tracked |
| `ProjectSettings/` | Unity project settings | tracked + 一部 dirty |
| `docs/` | ADR, plan, devlog, legal, prompt docs | tracked |
| `tools/` | Python helper scripts | tracked + 一部 untracked |
| `art/_intermediate/` | PixelLab / Aseprite / Meshy などの中間ファイル | `.gitignore` 対象 |
| `audio/_intermediate/` | DAW song, stem WAV, audio raw などの中間ファイル | `.gitignore` 対象 |
| `Builds/` | local build output | `.gitignore` 対象 |
| `UserSettings/` | Unity user-local settings | `.gitignore` 対象 |

## 3. Unity project (`Assets/`) 階層

### 3.1 Art

| Path / file | 用途 |
|---|---|
| `Assets/Art/Sprites/` | character sprite root |
| `Assets/Art/Sprites/Hero/v1/` | Hero v1 sprites: stand, idle, walk, D-7 hands |
| `Assets/Art/Sprites/Hero/v1/_draft/` | Hero F1 draft comparison files |
| `Assets/Art/Sprites/Hero/v2/` | Hero v2 Stage 4 sprites: stand, idle, walk, D-7 hands |
| `Assets/Art/Sprites/NPC/Resident_A/v1/` | Resident_A v1 idle / walk sprites |
| `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/` | Resident_A F1 draft comparison files |
| `Assets/Art/Sprites/NPC/Resident_A/v2/` | Resident_A v2 Stage 4 idle / walk sprites |
| `Assets/Art/Sprites/NPC/Resident_B/v1/` | Resident_B v1 seated idle sprite |
| `Assets/Art/Sprites/NPC/Resident_B/v1/_draft/` | Resident_B F1 draft comparison file |
| `Assets/Art/Sprites/NPC/Resident_B/v2/` | Resident_B v2 Stage 4 seated idle sprite |
| `Assets/Art/Materials/` | art material root |
| `Assets/Art/Materials/Portal/` | portal stencil / inside-only shader materials |
| `Assets/Art/Models/Zone1/` | Zone1 FBX, atlas, manifest, material root |
| `Assets/Art/Models/Zone1/HousePlayer/` | house/player-home FBX assets |
| `Assets/Art/Models/Zone1/LibraryRuin/` | library ruin / book / bookshelf FBX assets |
| `Assets/Art/Models/Zone1/Plaza/` | plaza / floor / fountain / tree / lamp FBX assets |
| `Assets/Art/Models/Zone1/Materials/` | Zone1 atlas material |
| `Assets/Art/Models/Zone1/Textures/` | Zone1 source textures imported with models |
| `Assets/Art/anemora_palette_v0.aseprite-palette` | Aseprite native palette source |
| `Assets/Art/anemora_palette_v0.gpl` | GIMP / Aseprite import palette |
| `Assets/Art/anemora_palette_v0.png` | palette swatch sheet |

Major observed files:

- Character sprite PNGs: 33 non-meta `.png` files under `Assets/Art/Sprites/`.
- Zone1 FBX files: 14 non-meta `.fbx` files under `Assets/Art/Models/Zone1/`.
- Portal shader files: `PortalMask.shader`, `InsideOnly.shader`.
- Portal material files: `PortalMask.mat`, `InsideOnly.mat`, `Debug_Current.mat`, `Debug_Past.mat`.
- Zone1 atlas files: `zone1_buildings_manifest.json`, `Anemora_Zone1_Atlas_512.png`, `Anemora_Zone1_Atlas_URP.mat`.

### 3.2 Animators

Animator assets currently live in `Assets/Animators/`, not under `Assets/Art/`.

| Path / file | 用途 |
|---|---|
| `Assets/Animators/HeroLocomotion.controller` | Hero locomotion AnimatorController |
| `Assets/Animators/ResidentALocomotion.controller` | Resident_A locomotion AnimatorController |
| `Assets/Animators/ResidentBIdle.controller` | Resident_B idle AnimatorController |
| `Assets/Animators/Clips/` | Hero / Resident animation clips |

Observed animation clips: `Hero_Idle.anim`, `Hero_Walk.anim`, `Resident_A_Idle.anim`, `Resident_A_Walk.anim`, `Resident_B_Idle.anim`.

### 3.3 Audio

`Assets/Audio/` is now tracked as the A4 Zone1 audio layout. The canonical runtime SFX set is under `Assets/Audio/SFX/Zone1/**`; flat category directories remain as compatibility copies.

| Path / file | 用途 | 状態 |
|---|---|---|
| `Assets/Audio/Music/` | BGM root | tracked |
| `Assets/Audio/Music/Zone1_Ambient.ogg` | Zone1 ambient BGM | tracked |
| `Assets/Audio/SFX/Zone1/` | canonical Zone1 SFX root | tracked |
| `Assets/Audio/SFX/Zone1/environment/` | environment SFX: birds, water, dry leaves, silence pad, wind loop, wood creak | tracked |
| `Assets/Audio/SFX/Zone1/footsteps/` | footstep SFX: stone / wood / grass / sand, walk / run / land | tracked |
| `Assets/Audio/SFX/Zone1/time_window/` | Time Frame Portal SFX: wheel open/close, symbol hover/select, portal open/flip | tracked |
| `Assets/Audio/SFX/Zone1/npc/` | NPC SFX: greeting, interaction ack, departure | tracked |
| `Assets/Audio/SFX/Zone1/ui/` | UI SFX: button click, menu open, menu close | tracked |
| `Assets/Audio/SFX/env/` | compatibility copy of environment SFX | tracked |
| `Assets/Audio/SFX/footstep/` | compatibility copy of footstep SFX | tracked |
| `Assets/Audio/SFX/timeframe/` | compatibility copy of time-window SFX | tracked |
| `Assets/Audio/SFX/npc/` | compatibility copy of NPC SFX | tracked |
| `Assets/Audio/SFX/ui/` | compatibility copy of UI SFX | tracked |

Observed audio files: 61 non-meta `.ogg` files: 1 music file, 30 canonical Zone1 SFX files, and 30 compatibility-copy SFX files. No `Assets/Audio/Mixers/` or AudioMixer asset was observed in the clean scan.

### 3.4 Localization

Project-level Unity Localization assets are now present under `Assets/Localization/`.

| Path / file | 用途 |
|---|---|
| `Assets/Localization/LocalizationSettings.asset` | Unity Localization settings asset |
| `Assets/Localization/Locales/Locale_ja-JP.asset` | Japanese locale |
| `Assets/Localization/Locales/Locale_en.asset` | English locale |
| `Assets/Localization/StringTables/` | StringTableCollection and locale tables |
| `Assets/Localization/StringTables/Anemora_Strings.asset` | `Anemora_Strings` StringTableCollection |
| `Assets/Localization/StringTables/Anemora_Strings Shared Data.asset` | shared table keys |
| `Assets/Localization/StringTables/Anemora_Strings_ja-JP.asset` | Japanese string table |
| `Assets/Localization/StringTables/Anemora_Strings_en.asset` | English string table |

Observed `Anemora_Strings` keys include menu keys, `dialogue.speaker.niro`, `dialogue.speaker.resident_a`, `dialogue.speaker.resident_b`, Niro intro / past-portal lines, and Resident_A / Resident_B encounter lines.

Font / TMP localization assets remain under UI:

- `Assets/UI/Localization/Fonts/`
- `Assets/TextMesh Pro/Resources/TMP Settings.asset`

### 3.5 Prefabs

| Path / file | 用途 |
|---|---|
| `Assets/Prefabs/Zone1/` | Zone1 building / prop / ActionRecord-related prefabs |
| `Assets/Prefabs/Characters/` | Hero / Resident_A / Resident_B character prefabs |
| `Assets/Prefabs/Portal/` | portal frame prefab |

Observed Zone1 prefab files: `Bed_Player.prefab`, `Book_Family_Current.prefab`, `Book_Family_Past.prefab`, `Bookshelf_Empty.prefab`, `Bookshelf_FamilyBooks.prefab`, `Bookshelf_Library_Past.prefab`, `Door_House.prefab`, `Floor_Stone.prefab`, `Floor_Wood.prefab`, `House_Player.prefab`, `Library_Ruin.prefab`, `Plaza_Fountain_Dry_Broken.prefab`, `StreetLamp.prefab`, `Table_SmallChair_Wooden.prefab`, `Tree_Decay.prefab`.

Observed character prefab files: `Hero.prefab`, `Resident_A.prefab`, `Resident_B.prefab`.

Observed portal prefab file: `Portal_Frame.prefab`.

### 3.6 Scenes

| Path / file | 用途 |
|---|---|
| `Assets/Scenes/Anemora_Main.unity` | Vertical Slice main scene |
| `Assets/Scenes/Sandbox_E1_Stencil.unity` | E1 portal stencil sandbox scene |

### 3.7 ScriptableObjects

| Path / file | 用途 |
|---|---|
| `Assets/ScriptableObjects/ActionRecords/ActionRecordCatalog.asset` | E5 / G4 ActionRecord catalog |
| `Assets/ScriptableObjects/Dialogues/Niro_Intro.asset` | Niro intro monologue DialogueAsset |
| `Assets/ScriptableObjects/Dialogues/Niro_PastPortal.asset` | Niro past-portal monologue DialogueAsset |
| `Assets/ScriptableObjects/Dialogues/Resident_A_Greeting.asset` | Resident_A encounter DialogueAsset |
| `Assets/ScriptableObjects/Dialogues/Resident_B_Idle.asset` | Resident_B encounter DialogueAsset |

All observed dialogue ScriptableObjects reference the `Anemora_Strings` table. `Resident_A_Greeting.asset` uses `dialogue.encounter.past_resident_a.*`; `Resident_B_Idle.asset` uses `dialogue.encounter.present_resident_b.*`; Niro assets use `dialogue.niro.*` keys.

### 3.8 Scripts

| Path | 用途 |
|---|---|
| `Assets/Scripts/Audio/` | Zone1 runtime audio controller |
| `Assets/Scripts/Data/` | `Anemora.Data` asmdef and POCO data types |
| `Assets/Scripts/Save/` | `Anemora.Save` asmdef and save migration code |
| `Assets/Scripts/Save/Migration/` | save migration interface |
| `Assets/Scripts/Game/` | `Anemora.Game` asmdef |
| `Assets/Scripts/Game/Dialogue/` | DialogueAsset ScriptableObject layer |
| `Assets/Scripts/Dialogue/` | NPC interaction and dialogue display components |
| `Assets/Scripts/PerformanceHarness/` | Stage 4 stress sample harness skeleton |
| `Assets/Scripts/Player/` | prototype player controller and animator binder |
| `Assets/Scripts/TimeManagement/` | portal, scene side, camera sync, ActionRecord runtime, Niro monologue hook |
| `Assets/Scripts/TimeManagement/Portal/` | URP portal stencil renderer feature |
| `Assets/Scripts/TimeManagement/Reflectors/` | reflector interfaces and book reflection components |

Key observed files added since v0.1:

- `Assets/Scripts/Audio/Zone1AudioController.cs`
- `Assets/Scripts/TimeManagement/NiroMonologueController.cs`
- `Assets/Scripts/PerformanceHarness/StressSampleRunner.cs`

Observed non-meta C# files under `Assets/Scripts/`: 27.

### 3.9 Tests

| Path | 用途 |
|---|---|
| `Assets/Tests/EditMode/` | EditMode tests and asmdef |
| `Assets/Tests/PlayMode/` | PlayMode tests and asmdef |

Observed test files at scan time:

- EditMode: 7 non-meta `.cs` files.
- PlayMode: 15 non-meta `.cs` files.

PlayMode files added / confirmed since v0.1 include:

- `Assets/Tests/PlayMode/Zone1AudioWiringTests.cs`
- `Assets/Tests/PlayMode/SaveLoadLocaleIntegrationTests.cs`
- `Assets/Tests/PlayMode/StressSampleRunnerSmokeTests.cs`

### 3.10 UI

| Path / file | 用途 |
|---|---|
| `Assets/UI/Localization/Fonts/` | TMP font assets and atlas assets |
| `Assets/UI/Localization/Fonts/ThirdParty/` | third-party font files and license files |
| `Assets/UI/Prefabs/SymbolWheel.prefab` | Time Frame Portal symbol wheel UI prefab |
| `Assets/UI/Prefabs/DialoguePanel.prefab` | G3 partial dialogue UI prefab |
| `Assets/UI/Scripts/SymbolWheelController.cs` | symbol wheel UI controller |
| `Assets/UI/Sprites/` | symbol wheel icon sprites |

Observed font / atlas files: `Anemora_JP.asset`, `Anemora_JP_Atlas.asset`, `Anemora_EN.asset`, `Anemora_EN_Atlas.asset`, `misaki_gothic.ttf`, `PressStart2P-Regular.ttf`, `PressStart2P_LICENSE.txt`. String tables are not under `Assets/UI/Localization/`; they live under `Assets/Localization/StringTables/`.

Observed UI sprite files: `symbol_red.png`, `symbol_blue_disabled.png`, `symbol_white_disabled.png`.

### 3.11 Settings

| Path / file | 用途 |
|---|---|
| `Assets/Settings/UniversalRenderPipeline.asset` | URP pipeline asset |
| `Assets/Settings/UniversalRenderPipeline_Renderer.asset` | URP renderer asset |
| `Assets/Settings/UniversalRenderPipelineGlobalSettings.asset` | URP global settings |
| `Assets/Settings/DefaultVolumeProfile.asset` | default volume profile |
| `Assets/Settings/Portal/PortalFlash_VolumeProfile.asset` | portal flash volume profile |

Some `Assets/Settings/` files are dirty in the current working tree. This doc records their path role only and does not modify them.

### 3.12 Editor

| Path / file | 用途 |
|---|---|
| `Assets/Editor/AnemoraE0Setup.cs` | E0 URP setup automation |
| `Assets/Editor/AnemoraE1ParallelSetup.cs` | E1 setup automation |
| `Assets/Editor/AnemoraTmpJapaneseAtlasBuilder.cs` | TMP JP atlas builder helper |
| `Assets/Editor/AnemoraZone1BuildingAssetSetup.cs` | Zone1 building asset setup helper |
| `Assets/Editor/Zone1AudioSceneSetup.cs` | Zone1 audio scene wiring helper |

Observed non-meta C# files under `Assets/Editor/`: 5.

## 4. docs/ 階層

| Path | 用途 |
|---|---|
| `docs/adr/` | ADR-0001 through ADR-0009 plus ADR README |
| `docs/asset_prompts/` | prompt specs for BGM, SFX, sprite, NPC, Zone1 buildings |
| `docs/devlog/` | milestone devlogs and `INDEX.md` |
| `docs/devlog/screenshots/` | visual evidence screenshots |
| `docs/draft/` | draft text assets such as opening / NPC dialogue |
| `docs/legal/` | asset ledger, Steam AI disclosure, code license options |
| `docs/localization/` | glossary |

Root-level docs observed under `docs/`: `VS_SCOPE.md`, `STAGE3_PLAN.md`, `STAGE3_E_PLAN.md`, `STAGE3_F_PLAN.md`, `STAGE3_G_PLAN.md`, `STAGE3_REVIEW_AIDS.md`, `STAGE3_TBD_RESOLUTION.md`, `G5_ACCEPTANCE_MATRIX.md`, `G5_PREFLIGHT.md`, `VERIFICATION_SUITE.md`.

## 5. tools/ 階層

| Path / file | 用途 | 状態 |
|---|---|---|
| `tools/meshy_zone1_buildings.py` | Meshy Zone1 generation helper | tracked |
| `tools/postprocess_meshy_zone1_buildings.py` | Blender postprocess helper | tracked |
| `tools/generate_zone1_buildings_blender.py` | Blender-side Zone1 generation helper | untracked at scan time |
| `tools/generate_zone1_sfx_elevenlabs.ps1` | ElevenLabs Zone1 SFX generation helper | untracked at scan time |

## 6. 中間ファイル (`.gitignore`)

ADR-0009 の中間ファイル方針と整合し、公開 repo には最終 import assets と検証ログを残します。

| Path | 用途 |
|---|---|
| `art/_intermediate/` | PixelLab raw, Aseprite working files, Meshy export raw, comparison screenshots |
| `audio/_intermediate/` | DAW song files, stem WAV, raw generated audio, comparison exports |
| `Library/` | Unity generated cache |
| `Logs/` | Unity logs |
| `Builds/` | local build output |
| `UserSettings/` | Unity local user settings |

`.gitignore` also excludes Unity / IDE generated `Temp/` and `obj/` style paths, but those directories were not present in the working tree at scan time.

## 7. Naming conventions

Naming conventions are a local reference extracted from ADR-0009 section 4 and current observed files. If ADR-0009 changes, update this section rather than inventing a parallel rule.

| Asset type | Convention | Example observed |
|---|---|---|
| Character sprite PNG | `<actor>_<state_or_direction>.png` or `<actor>_<action>_<direction>.png` | `hero_walk_front.png`, `resident_a_idle.png` |
| Draft sprite PNG | `<direction>_v<version>.png` | `front_v1.png`, `seated_v1.png` |
| Palette | `anemora_palette_<version>.<ext>` | `anemora_palette_v0.gpl` |
| 3D model | `<Name>.fbx` with PascalCase / underscore naming matching source asset | `House_Player.fbx`, `Book_Family_Past.fbx` |
| Audio | `<category_or_area>_<id>.ogg` | `Zone1_Ambient.ogg`, `sfx_ui_button_click_01.ogg` |
| Prefab | `<Name>.prefab` | `Hero.prefab`, `Book_Family_Current.prefab` |
| Material | `<Name>.mat` | `PortalMask.mat`, `Anemora_Zone1_Atlas_URP.mat` |
| Scene | `<Name>.unity` | `Anemora_Main.unity` |
| ScriptableObject | `<Name>.asset` | `ActionRecordCatalog.asset`, `Resident_A_Greeting.asset` |
| C# script | `<TypeName>.cs` | `TimeFramePortalController.cs` |

## 8. セッション間 path 認識の注意

- Character sprite の canonical root は `Assets/Art/Sprites/` です。`Assets/Art/Characters/` は v0.2 scan 時点で存在しません。
- AnimatorController / `.anim` files の canonical root は `Assets/Animators/` です。
- Zone1 SFX の primary layout は `Assets/Audio/SFX/Zone1/` 配下の `environment/`, `footsteps/`, `time_window/`, `npc/`, `ui/` です。`Assets/Audio/SFX/env/` などの flat category directories は compatibility copy として残っています。
- Project-level Locale / StringTable assets は `Assets/Localization/` 配下です。TMP font assets は `Assets/UI/Localization/Fonts/` 配下です。
- Niro monologue runtime hook の実在パスは `Assets/Scripts/TimeManagement/NiroMonologueController.cs` です。Dialogue UI / interactable scaffolding は `Assets/Scripts/Dialogue/` 配下です。
- Performance stress harness skeleton は `Assets/Scripts/PerformanceHarness/StressSampleRunner.cs` です。Stage 4 perf v1.0 で scene wiring / deterministic execution を追加予定です。
- 本 doc は source of truth として扱いますが、Unity import や他 session push により実態と乖離することがあります。作業前に `rg --files` または directory scan で対象 path を再確認してください。
- 大幅な構造変更があれば v0.x として本 doc を改訂してください。

## 9. 更新履歴

| Version | Date | Notes |
|---|---|---|
| v0.2 | 2026-05-05 | Niro lore DialogueAsset / localization, Zone1 audio, audio scene setup, PlayMode additions, and performance harness skeleton を実 scan ベースで反映 |
| v0.1 | 2026-05-05 | 初版起草。Stage 3 Day 1 時点の root / Assets / docs / tools / intermediate layout を実 scan ベースで整理 |
