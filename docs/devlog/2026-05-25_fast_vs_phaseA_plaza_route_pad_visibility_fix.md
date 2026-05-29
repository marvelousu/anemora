# 2026-05-25 Fast VS Phase A Plaza Route Pad Visibility Fix

## Scope
- Keep the runtime camera on the restored shared VS-like follow profile.
- Revert the mistaken plaza-to-library arrival-target change; that route was not the reported issue.
- Fix the exterior-to-plaza review path where the central-plaza map-move pads were present but hidden.

## Changes
- Removed legacy sun-ribbon suppression for `Current_CentralPlaza_ToHouseExterior_MapMoveGlowPad` and `Current_CentralPlaza_ToLibrary_MapMoveGlowPad`.
- Added visible route-pad validation for active current-route maps: active hierarchy, enabled renderer/material, and `FastVsMapMoveGlowPulse`.
- Added a capture helper for exterior-to-plaza arrival, both central-plaza move points, and a TimeWindow aperture image.

## Evidence
- `docs/devlog/screenshots/fast_vs_plaza_route_pads_after_exterior_route_20260525_01/`
- `docs/review/2026-05-25T15-12/`
- Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

## Verification
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaRoutePadsAfterExteriorRouteScreenshotsBatch`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Built-player smoke: 20 seconds, failure-pattern match count 0.

## Remaining Review Notes
- This is a Phase A runtime/camera and route-pad correction, not an HD-2D quality improvement pass.
- Destination-position feedback remains separate from this graphics session unless Tom explicitly re-prioritizes it.
