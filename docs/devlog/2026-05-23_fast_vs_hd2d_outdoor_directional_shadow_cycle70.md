# feat(hd2d): add outdoor directional shadow pass

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Primary authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Cycle runner: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\tools\cycle-runner.ps1`

## Direction

Cycle70 continues the HD-2D shadow foundation pass with a broader outdoor composition target. The user clarified that the desired shadows are not strict real-world length; they can be slightly exaggerated when that improves the staged HD-2D look. This cycle therefore adds broad but controlled diagonal cast-shadow cards to the house exterior and central plaza.

## Implementation

- Added `CreateHouseExteriorOutdoorDirectionalShadowCycle70(...)`.
- Added `CreateCentralPlazaOutdoorDirectionalShadowCycle70(...)`.
- Added shared helper `CreateOutdoorExaggeratedDirectionalShadowCycle70(...)`.
- Added `ValidateFastVsHd2dShadowFoundationCycle70OutdoorDirectionalShadow()`.
- Added `CaptureHd2dShadowFoundationCycle70ScreenshotsBatch()`.
- Connected house exterior Cycle70 creation immediately after Cycle69 house grounding.
- Connected central plaza Cycle70 creation immediately after Cycle68 plaza library grounding.

## Objects

House exterior, current and past:

- `Current_HouseExterior_ShadowFoundationCycle70_RoofDiagonalCastA`
- `Current_HouseExterior_ShadowFoundationCycle70_TreeDirectionalCastA`
- `Current_HouseExterior_ShadowFoundationCycle70_SignpostDirectionalCastA`
- `Past_HouseExterior_ShadowFoundationCycle70_RoofDiagonalCastA`
- `Past_HouseExterior_ShadowFoundationCycle70_TreeDirectionalCastA`
- `Past_HouseExterior_ShadowFoundationCycle70_SignpostDirectionalCastA`

Central plaza, current and past:

- `Current_CentralPlaza_ShadowFoundationCycle70_LibraryDiagonalCastA`
- `Current_CentralPlaza_ShadowFoundationCycle70_WestRoadFalloffCastA`
- `Current_CentralPlaza_ShadowFoundationCycle70_NoticeBoardDirectionalCastA`
- `Past_CentralPlaza_ShadowFoundationCycle70_LibraryDiagonalCastA`
- `Past_CentralPlaza_ShadowFoundationCycle70_WestRoadFalloffCastA`
- `Past_CentralPlaza_ShadowFoundationCycle70_NoticeBoardDirectionalCastA`

## Worker Cycle

- Implementation worker: `019e52e5-97f5-7911-b10c-ebfcd5b46e0a`
- Worker added the Cycle70 creation and validation helpers in the authored file.
- Parent added the Cycle70 capture batch method so the new cycle runner can exercise validate/capture/build/smoke from deterministic entry points.

## Validation Plan

The cycle runner is expected to run:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle70ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built Fast VS house slice player

Retained screenshot evidence path:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle70_outdoor_directional_shadow_parent_review_20260523_01`

## Review Notes

- The pass uses `hd2d_outdoor_occlusion_gradient` for broad outdoor shadows and `hd2d_depth_shadow` for smaller prop contact casts.
- All added pieces are non-arrival landmarks and should remain non-colliding.
- The intended visual direction is stronger than physically literal lighting, but still controlled enough to avoid solid black ground slabs.
- The parent visual gate after runner completion should inspect house exterior and central plaza screenshots before continuing to the next cycle.
