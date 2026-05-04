# ADR-0004: Project Directory Structure

## Status
Accepted

## Date
2026-05-05

## Context
Anemora is developed from a Windows notebook that can access both native Windows paths and WSL2 paths. Unity Editor is a Windows GUI application and creates many small files under `Library/`, `Temp/`, `Logs/`, and package caches. Opening a Unity project through `\\wsl$` or another cross-filesystem path can increase import time and file-locking risk.

The Stage 3 handover asked B track to decide where the Unity project should live and to define the initial Unity directory layout before the vertical-slice prototype starts.

## Decision
Keep the Unity project in a Windows-native checkout:

- `C:\Users\maro6\Documents\Unity\Anemora`

Use Git as the synchronization boundary between machines and environments. Do not treat WSL2 as the primary Unity working tree. WSL2 may still be used for documentation, CLI review, or automation after cloning the same Git remote.

Use this initial Unity layout. Subdirectories may be created lazily as Stage 3 tracks need them; the listing below records the agreed-on placements so other ADRs can reference stable paths.

- `Assets/Scripts/` for gameplay and tool scripts
  - `Assets/Scripts/TimeManagement/` for Time Frame portal, ActionRecord runtime, scene-flip logic (ADR-0005)
  - `Assets/Scripts/Save/` for save/load services and migration chain (ADR-0006)
  - `Assets/Scripts/Data/` for plain DTOs (`SaveEnvelope`, `ActionRecordEntry`, etc., ADR-0005 / 0006)
  - `Assets/Scripts/UI/` for HUD, dialog, symbol wheel, ESC menu controllers (ADR-0007)
- `Assets/Art/` for visual source assets
  - `Assets/Art/Sprites/` for sprite and HD-2D visual source exports
  - `Assets/Art/Materials/` for materials and visual test assets
  - `Assets/Art/Models/` for low-poly 3D background models (ADR-0003)
- `Assets/Audio/` for audio assets
  - `Assets/Audio/Music/` for BGM tracks (ADR-0003: AIVA / Suno / Stable Audio)
  - `Assets/Audio/SFX/` for sound effects (ADR-0003: ElevenLabs SFX v2)
- `Assets/UI/` for UI-specific resources (ADR-0007)
  - `Assets/UI/Prefabs/` for HUD, Dialog, SymbolWheel, Menu prefabs
  - `Assets/UI/Sprites/` for 9-slice frames, icons, pixel UI sprites
  - `Assets/UI/Scripts/` for UI control scripts not shared with gameplay
  - `Assets/UI/Localization/` for Unity Localization String Tables and TMP font assets
- `Assets/ScriptableObjects/` for static configuration assets
  - `Assets/ScriptableObjects/ActionRecords/` for `ActionRecordCatalog` static definitions (ADR-0005)
- `Assets/Prefabs/` for reusable scene objects (non-UI)
- `Assets/Scenes/` for Unity scenes
- `Assets/Settings/` for URP, input, and project assets
- `Assets/Tests/` for Unity test assets
- `Packages/` for Unity Package Manager manifests
- `ProjectSettings/` for Unity project settings

The project targets Unity `6000.3.14f1`, a Unity 6.3 LTS build, with Universal Render Pipeline enabled through `com.unity.render-pipelines.universal`.

Asmdef boundaries are not yet imposed at Stage 3 Day 1. They will be introduced when Newtonsoft.Json (ADR-0006) or test assemblies require explicit references; the directory layout above is shaped to make later asmdef splits straightforward (`Save`, `Data`, `TimeManagement`, `UI` each form natural assembly seams).

## Consequences
Unity import and editor file access stay on the fastest local filesystem for this machine. WSL2 remains useful for text-oriented work but is not on the critical path for Unity asset import or editor iteration.

Git must exclude Unity-generated folders and solution files. `Library/`, `Temp/`, `Logs/`, `UserSettings/`, `obj/`, `*.csproj`, and `*.sln` are not versioned.

The initial scaffold is intentionally sparse. Unity will generate `.meta` files, `ProjectSettings` details, and package lock details when the project is opened after Unity Hub license activation.

## Alternatives
Use the WSL2 checkout directly through `\\wsl$`. Rejected for the initial Unity project because the Unity Editor performs many small-file operations and the handover explicitly identified file-lock and I/O delay as risks to validate.

Maintain two separate repositories, one for docs in WSL2 and one for Unity on Windows. Rejected because it would split Stage 3 decisions and implementation history before the public GitHub repository is established.

## References
- `docs/STAGE3_PLAN.md` section 10.6
- `docs/adr/0001-engine-unity6.3-lts.md` — Unity 6.3 LTS + URP target
- `docs/adr/0003-asset-pipeline.md` — informs `Assets/Art/`, `Assets/Audio/` placement and asset ledger flow
- `docs/adr/0005-time-management-scene-switching.md` — informs `Assets/Scripts/TimeManagement/` and `Assets/ScriptableObjects/ActionRecords/`
- `docs/adr/0006-save-system.md` — informs `Assets/Scripts/Save/` and `Assets/Scripts/Data/`
- `docs/adr/0007-ui-framework-ugui.md` — informs `Assets/UI/{Prefabs,Sprites,Scripts,Localization}/`

## Revision History
| Rev | Date | Change |
|---|---|---|
| v1 | 2026-05-05 | Initial draft (Windows Codex, B track) |
| v1.1 | 2026-05-04 | Linux Claude follow-up: expanded `Assets/` layout for ADR-0003 / 0005 / 0006 / 0007 alignment, added asmdef note, added cross-ADR references |
