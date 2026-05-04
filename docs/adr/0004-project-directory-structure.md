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

Use this initial Unity layout:

- `Assets/Scripts/` for gameplay and tool scripts
- `Assets/Art/Sprites/` for sprite and HD-2D visual source exports
- `Assets/Art/Materials/` for materials and visual test assets
- `Assets/Prefabs/` for reusable scene objects
- `Assets/Scenes/` for Unity scenes
- `Assets/Settings/` for URP, input, and project assets
- `Assets/Tests/` for Unity test assets
- `Packages/` for Unity Package Manager manifests
- `ProjectSettings/` for Unity project settings

The project targets Unity `6000.3.14f1`, a Unity 6.3 LTS build, with Universal Render Pipeline enabled through `com.unity.render-pipelines.universal`.

## Consequences
Unity import and editor file access stay on the fastest local filesystem for this machine. WSL2 remains useful for text-oriented work but is not on the critical path for Unity asset import or editor iteration.

Git must exclude Unity-generated folders and solution files. `Library/`, `Temp/`, `Logs/`, `UserSettings/`, `obj/`, `*.csproj`, and `*.sln` are not versioned.

The initial scaffold is intentionally sparse. Unity will generate `.meta` files, `ProjectSettings` details, and package lock details when the project is opened after Unity Hub license activation.

## Alternatives
Use the WSL2 checkout directly through `\\wsl$`. Rejected for the initial Unity project because the Unity Editor performs many small-file operations and the handover explicitly identified file-lock and I/O delay as risks to validate.

Maintain two separate repositories, one for docs in WSL2 and one for Unity on Windows. Rejected because it would split Stage 3 decisions and implementation history before the public GitHub repository is established.

## References
- `docs/STAGE3_PLAN.md` section 10.6
- `docs/adr/0001-engine-unity6.3-lts.md`
