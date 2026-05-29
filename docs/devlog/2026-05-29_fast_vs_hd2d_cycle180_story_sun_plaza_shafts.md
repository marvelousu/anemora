# Fast VS HD-2D Cycle 180 Story Sun and Plaza Shafts

Date: 2026-05-29
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

## Cycle Worker

- Worker: `019e72e3-c99b-7dc2-8f59-82d9fe699611` (`Rawls`)
- Authored file requested: `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- Validate entry: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dCycle180StorySunAndPlazaShaftsBatch`
- Capture entry: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dCycle180StorySunAndPlazaShaftsScreenshotsBatch`

## Changes Applied

- Kept the pre-library-exit story sun state on `Morning` for Interior / Exterior / CentralPlaza / Library map anchors.
- Changed `AnemoraSunCycleDriver` fallback `defaultPreset` to `Morning` so the default scene state does not jump to Noon before the library exit beat.
- Added `FastVsAreaDoorTransition` story state so `Library -> CentralPlaza` switches to `Noon` during the transition fade, not after the player is visibly outside.
- Added indoor sun suppression in `AnemoraSunCycleDriver` for Interior and Library: direct sun intensity, sun disk, screen-space flare, lens flare, and volumetric fog are reduced/disabled while the active area is indoor.
- Added `FastVsDynamicSunShaftField` and five Plaza-wide dynamic shaft quads that update alpha, rotation, and parallax from camera/sun each frame instead of being a single static decoration near the library door.
- Extended Cycle180 validate/capture diagnostics to include serialized scene evidence for the Morning anchors, dynamic sunshaft field, and each dynamic shaft renderer.

## Serialized Scene Evidence

`Assets/Scenes/Anemora_FastVS_HouseSlice.unity` after Cycle180 batch regeneration:

- `defaultPreset: 0` at line `234502`.
- `FastVS_HD2D_MapSunAnchor_Library_Morning` at line `523437`.
- `transitionFromPrevious: 1` at lines `111223`, `499287`, and `523472`.
- `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaftField` at line `257504`.
- `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaft_WestWide` at line `315932`.
- `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaft_Center` at line `403487`.
- `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaft_EastWide` at line `110654`.

## Evidence

- Validate: `Logs/cycle-180-20260529-story-sun-plaza-shafts-validate-4.log`
  - Exit code: `0`
  - Severe scan matched only the Unity licensing token: `[Licensing::Module] Error: Access token is unavailable; failed to update`
- Capture: `Logs/cycle-180-20260529-story-sun-plaza-shafts-capture-2.log`
  - Exit code: `0`
  - Output: `docs/devlog/screenshots/fast_vs_hd2d_cycle180_story_sun_and_plaza_shafts_parent_review_20260529_01/`
  - Residual warning: `Light.shadowResolution is compatible only with the Built-In Render Pipeline.` from the existing realtime light rig during capture.
- Build: `Logs/cycle-180-20260529-story-sun-plaza-shafts-build.log`
  - `Build Finished, Result: Success.`
- Built exe smoke: `Logs/cycle-180-20260529-story-sun-plaza-shafts-smoke.log`
  - Launch duration: 24 seconds with `-batchmode -nographics`
  - Pattern scan `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`: `0`

## Review Set

Public curated review directory:

- `docs/review/2026-05-29T18-13/01_house_interior_story_morning.png`
- `docs/review/2026-05-29T18-13/02_house_exterior_story_morning.png`
- `docs/review/2026-05-29T18-13/03_plaza_west_dynamic_sunshaft.png`
- `docs/review/2026-05-29T18-13/04_plaza_east_dynamic_sunshaft.png`
- `docs/review/2026-05-29T18-13/05_library_story_morning_exit.png`

PNG hash prefixes:

- `3943DDFA7F647296` `01_house_interior_story_morning.png`
- `BF9868E48BED0D3A` `02_house_exterior_story_morning.png`
- `E99C6CA999987E5D` `03_plaza_west_dynamic_sunshaft.png`
- `596955EF926669F7` `04_plaza_east_dynamic_sunshaft.png`
- `14CDE216137E759E` `05_library_story_morning_exit.png`

## Reference Gap / Next Cycle

The reference-quality gap remains substantial. The Plaza now has wider dynamic shaft coverage than the previous library-front-only haze, but the screenshots still read partly as bright ground wash and atmospheric bands rather than fully convincing volumetric sunlight cutting through the whole map. The Library still needs a separate cleanup pass for the remaining suspicious texture/overbright artifacts. Cycle181 should focus on volumetric shaft shape/readability and Library artifact removal.

## Build Path

`Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

Run note: start the whole `Builds/FastVS_HouseSlice/` folder, not only a copied exe.
