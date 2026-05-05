# Anemora Asset Structure

> Status: v0.1 draft (2026-05-05). This file records the directory layout observed in the working tree at Stage 3 Day 1.

## 1. 概要

本 doc は Anemora repo の canonical asset layout reference です。複数セッションで asset を生成・配置するときの path 認識を揃え、Stage 4 GitHub Public 公開時の onboarding 資料として使うことを目的にします。

ADR-0009 `docs/adr/0009-asset-pipeline.md` は asset pipeline workflow を扱います。本 doc は directory layout と実 file の所在に限定します。

想定 reader は新規 contributor、Stage 4 onboarding、並列セッションで作業する agent です。作業前には本 doc を確認し、大きな構造変更がある場合は本 doc も更新してください。

## 2. リポジトリ root 構成

Scan basis: repo root `C:\Users\maro6\Documents\Unity\Anemora`, 2026-05-05. `Library/`, `Logs/`, `Temp/`, `.git/` は scan 表示から除外しました。

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
| `Assets/Art/Sprites/NPC/Resident_A/v1/` | Resident_A v1 idle / walk sprites |
| `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/` | Resident_A F1 draft comparison files |
| `Assets/Art/Sprites/NPC/Resident_B/v1/` | Resident_B v1 seated idle sprite |
| `Assets/Art/Sprites/NPC/Resident_B/v1/_draft/` | Resident_B F1 draft comparison file |
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

- Character sprite PNGs: 20 non-meta `.png` files under `Assets/Art/Sprites/`.
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

`Assets/Audio/` exists in the current working tree and is untracked at scan time. Treat it as the current A4 audio layout until that session's commit lands.

| Path / file | 用途 | 状態 |
|---|---|---|
| `Assets/Audio/Music/` | BGM root | untracked |
| `Assets/Audio/Music/Zone1_Ambient.ogg` | Zone1 ambient BGM | untracked |
| `Assets/Audio/SFX/` | SFX root | untracked |
| `Assets/Audio/SFX/env/` | environment SFX category | untracked |
| `Assets/Audio/SFX/footstep/` | footstep SFX category | untracked |
| `Assets/Audio/SFX/npc/` | NPC SFX category | untracked |
| `Assets/Audio/SFX/timeframe/` | Time Frame Portal SFX category | untracked |
| `Assets/Audio/SFX/ui/` | UI SFX category | untracked |
| `Assets/Audio/SFX/ui/sfx_ui_button_click_01.ogg` | UI button click SFX | untracked |

No AudioMixer asset directory was observed at scan time.

### 3.4 Localization

Project-level Locale / StringTable assets are not present as standalone Unity assets at scan time. A1 localization completion should update this section after those assets are imported.

Existing localization-related assets are currently font/TMP assets under UI:

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
| `Assets/ScriptableObjects/Dialogues/Resident_A_Greeting.asset` | G3 partial dialogue asset |
| `Assets/ScriptableObjects/Dialogues/Resident_B_Idle.asset` | G3 partial dialogue asset |

### 3.8 Scripts

| Path | 用途 |
|---|---|
| `Assets/Scripts/Data/` | `Anemora.Data` asmdef and POCO data types |
| `Assets/Scripts/Save/` | `Anemora.Save` asmdef and save migration code |
| `Assets/Scripts/Save/Migration/` | save migration interface |
| `Assets/Scripts/Game/` | `Anemora.Game` asmdef |
| `Assets/Scripts/Game/Dialogue/` | DialogueAsset ScriptableObject layer |
| `Assets/Scripts/Dialogue/` | NPC interaction and dialogue display components |
| `Assets/Scripts/Player/` | prototype player controller and animator binder |
| `Assets/Scripts/TimeManagement/` | portal, scene side, camera sync, ActionRecord runtime |
| `Assets/Scripts/TimeManagement/Portal/` | URP portal stencil renderer feature |
| `Assets/Scripts/TimeManagement/Reflectors/` | reflector interfaces and book reflection components |

Observed non-meta C# files under `Assets/Scripts/`: 24.

### 3.9 Tests

| Path | 用途 |
|---|---|
| `Assets/Tests/EditMode/` | EditMode tests and asmdef |
| `Assets/Tests/PlayMode/` | PlayMode tests and asmdef |

Observed test files at scan time:

- EditMode: 7 non-meta `.cs` files.
- PlayMode: 10 non-meta `.cs` files.

### 3.10 UI

| Path / file | 用途 |
|---|---|
| `Assets/UI/Localization/Fonts/` | TMP font assets and atlas assets |
| `Assets/UI/Localization/Fonts/ThirdParty/` | third-party font files and license files |
| `Assets/UI/Prefabs/SymbolWheel.prefab` | Time Frame Portal symbol wheel UI prefab |
| `Assets/UI/Prefabs/DialoguePanel.prefab` | G3 partial dialogue UI prefab |
| `Assets/UI/Scripts/SymbolWheelController.cs` | symbol wheel UI controller |
| `Assets/UI/Sprites/` | symbol wheel icon sprites |

Observed font / atlas files: `Anemora_JP.asset`, `Anemora_JP_Atlas.asset`, `Anemora_EN.asset`, `Anemora_EN_Atlas.asset`, `misaki_gothic.ttf`, `PressStart2P-Regular.ttf`, `PressStart2P_LICENSE.txt`.

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

- Character sprite の canonical root は `Assets/Art/Sprites/` です。
- AnimatorController / `.anim` files の canonical root は `Assets/Animators/` です。
- 現状の SFX category layout は `Assets/Audio/SFX/` 直下の category directory です。ただし `Assets/Audio/` は scan 時点で untracked のため、A4 commit 後に再確認してください。
- Project-level Locale / StringTable assets は scan 時点で未配置です。A1 完了後に本 doc を更新してください。
- 本 doc は source of truth として扱いますが、Unity import や他 session push により実態と乖離することがあります。作業前に `rg --files` または directory scan で対象 path を再確認してください。
- 大幅な構造変更があれば v0.x として本 doc を改訂してください。

## 9. 更新履歴

| Version | Date | Notes |
|---|---|---|
| v0.1 | 2026-05-05 | 初版起草。Stage 3 Day 1 時点の root / Assets / docs / tools / intermediate layout を実 scan ベースで整理 |
