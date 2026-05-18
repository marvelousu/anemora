# 2026-05-18 Fast VS public README playability pass

## Scope

- Project: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- README: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\README.md`
- Local Windows build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Local release zip: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\Anemora_FastVS_HouseSlice_20260518.zip`

## User Review Items

- Confirm whether the repo is set up so someone can easily play the VS.
- Confirm whether the README exposes the technical basics clearly enough.

## Worker Cycle

- Plan: inspect README, build packaging, ignored paths, and existing technical docs before editing.
- Worker instruction: gpt-5.4-mini worker `019e3a4d-f06f-7e81-8015-6a9961ddc24a` inspected README and relevant docs without editing.
- Worker result: the worker confirmed that the README still said no playable build existed, and recommended replacing the status/getting-started text with a play-first Windows build section, controls, and technical entry points.
- Integrator review: the final patch keeps build artifacts out of Git, adds README guidance for release zip attachment, and links the Fast VS technical entry points.

## Changes

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\README.md`
  - Replaced the old work-in-progress playable-state text.
  - Added `Play the Fast VS` with the executable path, required neighboring Unity build files, and release-zip note.
  - Added controls for movement, interaction, map transitions, Time Window creation, and Time Window close.
  - Added `Technical Basics` with the scene, generator, Time Window V24 controller, route/map scripts, story flow, devlog index, public baseline record, and validation log.
  - Updated Getting Started to point at `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`.

## Verification

- Confirmed all README-referenced local paths exist:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\Anemora_FastVS_HouseSlice_20260518.zip`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\TimeManagement\TimeWindowPairedSpacePortalController.cs`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_build_validate_20260518_skip_opening_wake_line.log`
- Confirmed `Builds/` remains ignored by Git, so the zip must be attached as a release asset rather than committed.
- Confirmed the old README phrases `通しプレイ可能な build はまだ存在しません` and `現在 Stage 3 Vertical Slice を制作中` are no longer present.

## Notes

- This pass does not change runtime code or rebuild the player. It only updates public onboarding and creates a local release zip from the already validated build.
