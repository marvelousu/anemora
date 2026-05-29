# FastVS HD2D Feedback Lighting Polish - Cycle 179

Date: 2026-05-29
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

## Scope

Cycle 179 addresses Tom's feedback that the latest presented build looked like an older Phase A build, that the sun direction changed too abruptly after exiting the house, that the visible light/god-ray particles read as scattered decoration rather than an intentional lighting pass, and that a suspicious floor texture remained in the library.

This cycle is a bounded feedback polish pass, not a final visual sign-off.

## Changes Applied

- Updated the runtime SunPreset table and regenerated the four SunPreset assets with less abrupt directional changes and lower Noon harshness.
- Enabled smoothing on `FastVS_HD2D_MapSunAnchor_Exterior_Morning` so the exterior anchor uses a transition instead of a hard switch when leaving the house.
- Reduced library/plaza dust and light-mote density, alpha, and particle fallback intensity so they no longer dominate the lighting read.
- Reduced the C-alpha fallback VFX/light settings for fire sparks, fireflies, smoke, water sparkle, library warm emissive light, and plaza water sparkle light.
- Disabled broad generated library floor overlays that were contributing to the suspicious floor-texture read while leaving the source objects present for future review.
- Added `ValidateHd2dFeedbackLightingPolishBatch` and `CaptureHd2dFeedbackLightingPolishCycle179ScreenshotsBatch` for this cycle's repeatable validation and capture.
- Wrote a diagnostics report into the capture directory with MCP, material alpha, particle, light, and library floor cleanup evidence.

## Serialized Scene Evidence

Scene asset verification used direct serialized grep rather than exe hash or marker checks.

- `FastVS_HD2D_MapSunAnchor_Exterior_Morning`
- `transitionFromPrevious: 1`
- `Current_Library_Stage8c_WarmFloorPool_LongTableA` with `m_IsActive: 0`
- `Current_Library_Stage8l_MidFloorShadowBreakA` with `m_IsActive: 0`
- `Current_Library_RuinFloorDetail_DustMatCenterA` with `m_IsActive: 0`
- `Current_Library_FloorDecay_ScuffBandCenter` with `m_IsActive: 0`

## MCP / RealPlayDiag Check

- `com.gamelovers.mcp-unity` is present in `Packages/manifest.json`.
- `ProjectSettings/McpUnitySettings.json` is present locally.
- This Codex session exposed no MCP resources or resource templates.
- `rg "RealPlayDiag|RealPlay|PlayDiag" Assets docs -g "!docs/review/**"` returned no project hook matches.

Because no callable MCP or RealPlayDiag hook was exposed to this session, verification used Unity batchmode validation/capture, serialized scene grep, a fresh build, and a built-exe smoke run.

## Validation

- `Logs/cycle-179-20260529-sun-cycle-runtime-validate-3.log`: exit 0.
- `Logs/cycle-179-20260529-feedback-polish-validate-5.log`: exit 0.
- `Logs/cycle-179-20260529-feedback-polish-capture-4.log`: exit 0.
- `Logs/cycle-179-20260529-feedback-polish-build.log`: exit 0, `Build Finished, Result: Success.`
- `Logs/cycle-179-20260529-feedback-polish-smoke.log`: launched the built exe for 24 seconds with `-batchmode -nographics`; scanned `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`; pattern count 0.

## Captures

Capture directory:

`docs/devlog/screenshots/fast_vs_hd2d_feedback_lighting_polish_cycle179_parent_review_20260529_01/`

Files:

- `01_current_house_interior_sun_cycle_morning.png`
- `02_current_house_exterior_sun_cycle_morning.png`
- `03_current_central_plaza_sun_cycle_noon.png`
- `04_current_library_sun_cycle_evening.png`
- `05_current_timewindow_aperture.png`
- `feedback_lighting_polish_diagnostics.md`
- `sun_cycle_scene_wiring_diagnostics.md`

Curated public review set:

`docs/review/2026-05-29T17-08/`

## Build

Build exe:

`Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

Launch note: start the whole `Builds/FastVS_HouseSlice` folder, not only a copied exe.

The exe was rebuilt after this cycle's changes on 2026-05-29 at 17:05:59 local time.

## Remaining Reference Gap

The target HD-2D quality remains substantially below the reference. The plaza still reads broad and washed compared with the reference image composition, and the library still needs a stronger authored light hierarchy around the table/books instead of relying on many small atmospheric elements. This cycle only reduces the specific abrupt sun transition, scattered-light, and floor-overlay issues for Tom review.

Tom review requested before treating this as accepted.
