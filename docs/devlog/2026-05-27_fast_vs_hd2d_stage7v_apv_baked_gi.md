# Stage7v: APV Baked GI Evidence

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Move the Stage 7 APV work from marker-volume setup into a repeatable baked-GI evidence path with baked APV cell assets and validation.

## Changes

- Added Stage7v APV baking set setup to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
  - Baking set: `Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet.asset`
  - Scene binding: `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
  - Scenario: `Default`
  - Placement: simplification level `3`, minimum probe distance `1.0`, probe offset `(0.5, 0, 0.5)`.
- Changed the current/past Stage 7 APV local volumes from marker-only placement to baked placement by enabling `fillEmptySpaces`.
- Ensured `CreateHouseSliceScene()` reattaches `ProbeVolumePerSceneData` to the Stage7v baking set before save, so the build path keeps APV scene data after its final scene regeneration.
- Added APV batch entry points:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BakeHd2dStage7ApvBakedGiBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7ApvBakedGiBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7ApvBakedGiReferenceScreenshotsBatch`
- Added validation for baked APV evidence:
  - Baking set asset exists and is bound to the house slice scene.
  - `ProbeVolumePerSceneData` is present and points at the baking set.
  - Baked APV cell descriptors are present.
  - Shared APV data and support data are present.

## Baked Assets

- `Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet.asset`
- `Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet-Default.CellData.bytes`
- `Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet-Default.CellOptionalData.bytes`
- `Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet-Default.CellProbeOcclusionData.bytes`
- `Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet.CellBricksData.bytes`
- `Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet.CellSharedData.bytes`
- `Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet.CellSupportData.bytes`

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7ApvBakedGiBatch`
  - Exit 0.
  - Log: `Logs\stage7v_apv_baked_gi_validate_after_pipeline_prepare.log`
  - Evidence line: `Fast VS Stage 7 APV baked GI completed with 8 baked cells in Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet.asset.`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7ApvBakedGiReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage7v_apv_baked_gi`
  - Log: `Logs\stage7v_apv_baked_gi_capture.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - `Fast VS house slice validation passed.`
  - `Build Finished, Result: Success.`
  - Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - Log: `Logs\stage7v_apv_baked_gi_build.log`
- Player smoke:
  - Built exe launched with `-batchmode -nographics`.
  - Stopped after 24 seconds.
  - Log: `Logs\stage7v_apv_baked_gi_smoke.log`
  - No `Exception`, `NullReference`, `MissingReference`, `Assertion`, shader compile error, or C# compile error matches.

## Public Review

- Review directory: `docs/review/2026-05-27T01-25/`
- Public curation:
  - No external game target reference images.
  - No comparison board containing external references.
  - No route-close obstruction diagnostics.
  - Includes build exe path and "フォルダごと起動".
- Published image set:
  - `home.png`
  - `Home_outside.png`
  - `plaza_01.png`
  - `plaza_02_niro_in_shadow.png`
  - `library.png`
  - `tw_current_aperture.png`

## Remaining Gaps

- Stage7v provides baked APV data and a validation path, but this is a technical lighting-data checkpoint rather than visual approval.
- The plaza still reads as broad constructed geometry with mechanical shadow masses and limited painterly breakup.
- The library still needs denser authored prop silhouettes, stronger atmospheric layering, and richer warm/cool separation.
- The TimeWindow aperture remains readable but still presents as a flat technical overlay in several views.
- VFX Graph or richer atmospheric behavior, measured Stage 7 FPS evidence, and additional TimeWindow integration work remain open.
- The target HD-2D quality remains substantially below the reference target.
