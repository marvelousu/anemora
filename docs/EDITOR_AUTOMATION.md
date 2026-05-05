# Editor Automation

> Status: v0.1 draft (2026-05-05). This file documents editor automation and helper scripts observed in the working tree.

## 1. Overview

This document is an onboarding reference for Unity editor automation and repository helper scripts. It covers how each script is invoked, what it expects before running, where it writes output, and whether it has a dry-run or retry-safe mode.

ADR-0009 section 2 defines the tool split: AI/external tools generate candidates, Blender / Aseprite / Studio One / Unity handle intermediate processing, and only final import assets are committed under `Assets/`.

Before running any automation, check the working tree and stage only the intended outputs:

```powershell
git status -sb
git diff --stat
```

## 2. Unity Editor Automation

### 2.1 `Assets/Editor/AnemoraE0Setup.cs`

- Purpose: creates or updates the E0 URP baseline.
- Invocation:
  - No `MenuItem` attribute is present in the source at scan time.
  - Batchmode example:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" `
  -batchmode `
  -projectPath "C:\Users\maro6\Documents\Unity\Anemora" `
  -executeMethod Anemora.EditorTools.AnemoraE0Setup.Run `
  -quit
```

- Prerequisites: Unity Editor, URP package, and compiled `Anemora.TimeManagement.Portal.PortalStencilFeature`.
- Expected behavior: ensures `Assets/Settings/`, creates or updates `UniversalRenderPipeline.asset` and `UniversalRenderPipeline_Renderer.asset`, adds `PortalStencilFeature` if missing, moves root-level global settings / default volume profile into `Assets/Settings/` when present, and assigns the pipeline to Graphics and all Quality levels.
- Output paths:
  - `Assets/Settings/UniversalRenderPipeline.asset`
  - `Assets/Settings/UniversalRenderPipeline_Renderer.asset`
  - `Assets/Settings/UniversalRenderPipelineGlobalSettings.asset`
  - `Assets/Settings/DefaultVolumeProfile.asset`
- Dry-run: none.
- Retry behavior: mostly idempotent; it loads existing assets before creating them and skips adding another `PortalStencilFeature` when one already exists.

### 2.2 `Assets/Editor/AnemoraE1ParallelSetup.cs`

- Purpose: creates the E1 portal stencil sandbox assets, minimal main scene skeleton, symbol wheel prefab, and E1 screenshots.
- Invocation:
  - Unity menu: `Anemora/Setup/E1 Parallel Assets`
  - Unity menu: `Anemora/Setup/Capture E1 Screenshots`
  - Batchmode execute methods:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -projectPath "C:\Users\maro6\Documents\Unity\Anemora" -executeMethod Anemora.EditorTools.AnemoraE1ParallelSetup.Run -quit
& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -projectPath "C:\Users\maro6\Documents\Unity\Anemora" -executeMethod Anemora.EditorTools.AnemoraE1ParallelSetup.CaptureE1Screenshots -quit
```

- Prerequisites: Unity Editor, URP, portal shaders, `SymbolWheelController`, and TimeManagement scripts compiled.
- Expected behavior: ensures layers 8-11, creates portal materials, creates symbol sprites, writes `Sandbox_E1_Stencil.unity`, writes / rewrites `Anemora_Main.unity`, creates `SymbolWheel.prefab`, and updates build settings scenes.
- Output paths:
  - `Assets/Art/Materials/Portal/`
  - `Assets/UI/Sprites/`
  - `Assets/UI/Prefabs/SymbolWheel.prefab`
  - `Assets/Scenes/Sandbox_E1_Stencil.unity`
  - `Assets/Scenes/Anemora_Main.unity`
  - `docs/devlog/screenshots/e1_portal_front.png`
  - `docs/devlog/screenshots/e1_portal_side.png`
  - `docs/devlog/screenshots/e1_portal_back.png`
- Dry-run: none.
- Retry behavior: folder / material / sprite creation is guarded, but scene and prefab creation overwrite target assets.

### 2.3 `Assets/Editor/AnemoraTmpJapaneseAtlasBuilder.cs`

- Purpose: builds the TMP Japanese font asset and atlas from Misaki Gothic.
- Invocation:
  - Unity menu: `Anemora/Build TMP Japanese Atlas v0`
  - Batchmode execute method:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -projectPath "C:\Users\maro6\Documents\Unity\Anemora" -executeMethod AnemoraTmpJapaneseAtlasBuilder.Build -quit
```

- Prerequisites: `Assets/UI/Localization/Fonts/ThirdParty/misaki_gothic.ttf`, TextMeshPro, and TMP essential resources or package cache access.
- Expected behavior: ensures TMP settings / shader references, deletes existing JP font asset and atlas, builds a 4096x4096 SDF atlas from the JIS-oriented character set, logs missing character count, and saves the new assets.
- Output paths:
  - `Assets/UI/Localization/Fonts/Anemora_JP.asset`
  - `Assets/UI/Localization/Fonts/Anemora_JP_Atlas.asset`
  - `Assets/TextMesh Pro/Resources/TMP Settings.asset` when missing
- Dry-run: none.
- Retry behavior: destructive for the two JP output assets because it deletes and recreates them.

### 2.4 `Assets/Editor/AnemoraZone1BuildingAssetSetup.cs`

- Purpose: applies Unity import settings to Zone1 FBX files and creates Zone1 prefabs.
- Invocation:
  - Unity menu: `Anemora/Assets/Apply Zone1 Building Import`
  - Batchmode execute method:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -projectPath "C:\Users\maro6\Documents\Unity\Anemora" -executeMethod Anemora.Editor.AnemoraZone1BuildingAssetSetup.ApplyZone1BuildingImport -quit
```

- Prerequisites: FBX files under `Assets/Art/Models/Zone1/` and the Zone1 atlas PNG if the shared material should receive a texture.
- Expected behavior: imports Zone1 models recursively, creates / updates `Anemora_Zone1_Atlas_URP.mat`, disables animation / cameras / lights on model importers, configures mesh import settings, normalizes model bounds, and writes prefabs.
- Output paths:
  - `Assets/Art/Models/Zone1/Materials/Anemora_Zone1_Atlas_URP.mat`
  - `Assets/Prefabs/Zone1/*.prefab`
- Dry-run: none.
- Retry behavior: rerunnable, but prefabs and model importer settings are overwritten to match the script.

## 3. Repository Helper Scripts

### 3.1 `tools/meshy_zone1_buildings.py`

- Purpose: generates Zone1 building and prop candidates through the Meshy Text to 3D API.
- Invocation:

```powershell
$env:MESHY_API_KEY = "<key>"
python tools/meshy_zone1_buildings.py --balance-only
python tools/meshy_zone1_buildings.py --only House_Player_Candidate_03 Bed_Player
python tools/meshy_zone1_buildings.py --sequential
```

- Prerequisites: Python 3, network access, and `MESHY_API_KEY`. The script uses Python standard library modules only.
- Authentication: `MESHY_API_KEY` process environment variable.
- Credit behavior: each `AssetSpec` has `expected_credits=30` by default. The full observed list contains 18 asset specs, so the script prints an expected maximum of 540 credits before generation. `--balance-only` checks balance and does not create generation tasks.
- Retry-safe behavior: task IDs are written to `art/_intermediate/zone1_meshy/meshy_zone1_state.json` before polling and downloading, so reruns resume existing preview / refine tasks rather than creating replacements.
- Output paths:
  - `art/_intermediate/zone1_meshy/meshy_zone1_state.json`
  - `art/_intermediate/zone1_meshy/<group>/<asset_id>/preview_payload.json`
  - `art/_intermediate/zone1_meshy/<group>/<asset_id>/refine_payload.json`
  - `art/_intermediate/zone1_meshy/<group>/<asset_id>/preview_*`
  - `art/_intermediate/zone1_meshy/<group>/<asset_id>/refine_*`
- Dry-run: none. `--balance-only` is a non-generating API check, not a full dry-run.
- Notes: use `--only` for a small subset and `--sequential` when avoiding parallel preview / refine task creation.

### 3.2 `tools/postprocess_meshy_zone1_buildings.py`

- Purpose: post-processes Meshy GLB outputs in Blender and exports final Zone1 FBX assets / manifest.
- Invocation:

```powershell
blender --background --python tools/postprocess_meshy_zone1_buildings.py
```

- Prerequisites: Blender 4.5.5 LTS or compatible Blender Python with `bpy`, Meshy intermediate state at `art/_intermediate/zone1_meshy/meshy_zone1_state.json`, and source `refine_model.glb` files for Meshy-backed assets.
- Authentication: none at this stage; it reads already downloaded Meshy outputs.
- Expected behavior: imports GLB files or rebuilds selected assets procedurally, normalizes scale / origin, cleans geometry, desaturates textures, writes FBX, and writes a manifest with Meshy credit totals from the state file.
- Output paths:
  - `Assets/Art/Models/Zone1/<category>/<asset_id>.fbx`
  - `Assets/Art/Models/Zone1/Textures/*.png`
  - `Assets/Art/Models/Zone1/zone1_buildings_manifest.json`
- Dry-run: none.
- Retry behavior: rerunnable after Meshy outputs exist; output FBX / texture / manifest files are overwritten.

### 3.3 `tools/generate_zone1_buildings_blender.py`

- Scan status: present in the working tree but untracked at scan time.
- Purpose: generates procedural Blender fallback drafts for Zone1 buildings / props when Meshy API execution is blocked or unavailable.
- Invocation:

```powershell
blender --background --python tools/generate_zone1_buildings_blender.py
```

- Prerequisites: Blender Python with `bpy`, `bmesh`, and `mathutils`.
- Authentication: none.
- Expected behavior: creates a 512px Zone1 color atlas, exports procedural FBX assets, writes prompt-status JSON files documenting Meshy API blockage, and writes a Zone1 manifest.
- Output paths:
  - `Assets/Art/Models/Zone1/Anemora_Zone1_Atlas_512.png`
  - `Assets/Art/Models/Zone1/<category>/<asset_id>.fbx`
  - `Assets/Art/Models/Zone1/zone1_buildings_manifest.json`
  - `art/_intermediate/zone1_meshy/*_prompt_status.json`
  - `art/_intermediate/zone1_meshy/plaza_center/option_*/*.fbx`
- Dry-run: none.
- Retry behavior: rerunnable, but it rewrites generated FBX / atlas / manifest outputs.

### 3.4 `tools/generate_zone1_sfx_elevenlabs.ps1`

- Scan status: present in the working tree but untracked at scan time.
- Purpose: generates Zone1 SFX through ElevenLabs, converts generated MP3 files to OGG Vorbis q6, and writes a generation manifest.
- Invocation:

```powershell
$env:ELEVENLABS_API_KEY = "<key>"
powershell -ExecutionPolicy Bypass -File tools/generate_zone1_sfx_elevenlabs.ps1 -Only sfx_ui_button_click_01
powershell -ExecutionPolicy Bypass -File tools/generate_zone1_sfx_elevenlabs.ps1 -Force
```

- Prerequisites: PowerShell, network access, `ELEVENLABS_API_KEY`, and the ffmpeg-static executable currently referenced at `C:\Users\maro6\.codex\tools\ffmpeg-static\node_modules\ffmpeg-static\ffmpeg.exe`.
- Authentication: `ELEVENLABS_API_KEY` read from Process, User, then Machine environment scopes.
- Expected behavior: calls ElevenLabs sound generation, stores MP3 intermediates, converts to mono 44.1 kHz OGG Vorbis q6 with HPF / trim / fade / loudnorm, writes final Zone1 paths and compatibility paths, and updates the manifest.
- Output paths:
  - `audio/_intermediate/sfx_zone1/<category>/<id>.mp3`
  - `audio/_intermediate/sfx_zone1/elevenlabs_generation_manifest.json`
  - `Assets/Audio/SFX/Zone1/<zoneSubdir>/<id>.ogg`
  - `Assets/Audio/SFX/<category>/<id>.ogg`
- Dry-run: none.
- Retry behavior: skips API generation when the intermediate MP3 already exists unless `-Force` is provided. OGG conversion and manifest update still run.

## 4. Quick Reference

| Script | Counted in repo scan | Main entry point | Auth / env | Primary output | Dry-run |
|---|---:|---|---|---|---|
| `Assets/Editor/AnemoraE0Setup.cs` | yes | `Anemora.EditorTools.AnemoraE0Setup.Run` | none | `Assets/Settings/` URP assets | no |
| `Assets/Editor/AnemoraE1ParallelSetup.cs` | yes | Unity menu / `Anemora.EditorTools.AnemoraE1ParallelSetup.Run` | none | portal assets, scenes, symbol wheel | no |
| `Assets/Editor/AnemoraTmpJapaneseAtlasBuilder.cs` | yes | Unity menu / `AnemoraTmpJapaneseAtlasBuilder.Build` | none | TMP JP font asset / atlas | no |
| `Assets/Editor/AnemoraZone1BuildingAssetSetup.cs` | yes | Unity menu / `Anemora.Editor.AnemoraZone1BuildingAssetSetup.ApplyZone1BuildingImport` | none | Zone1 material / prefabs | no |
| `tools/meshy_zone1_buildings.py` | yes | `python tools/meshy_zone1_buildings.py` | `MESHY_API_KEY` | `art/_intermediate/zone1_meshy/` | no; `--balance-only` check exists |
| `tools/postprocess_meshy_zone1_buildings.py` | yes | `blender --background --python ...` | none | `Assets/Art/Models/Zone1/` | no |
| `tools/generate_zone1_buildings_blender.py` | yes, untracked | `blender --background --python ...` | none | `Assets/Art/Models/Zone1/` | no |
| `tools/generate_zone1_sfx_elevenlabs.ps1` | yes, untracked | `powershell -File ...` | `ELEVENLABS_API_KEY` | `Assets/Audio/SFX/Zone1/` | no |

## 5. Operational Notes

- Unity editor scripts can generate side-effect assets. Review `git status -sb` and `git diff --stat` before staging.
- API scripts can consume paid credits or quota. Use `--balance-only`, `--only`, or script-specific filters before broad generation.
- Intermediate roots `art/_intermediate/` and `audio/_intermediate/` are ignored by Git. Commit final imported assets and docs, not raw generation files, unless a later policy explicitly changes that.
- For public onboarding, revisit hard-coded local paths such as the ffmpeg path in `tools/generate_zone1_sfx_elevenlabs.ps1` before relying on the script outside this Windows machine.

## 6. Change History

| Version | Date | Notes |
|---|---|---|
| v0.1 | 2026-05-05 | 初版起草。Stage 3 Day 1 時点の Unity editor automation 4 件と `tools/` helper 4 件を実 scan ベースで整理 |
