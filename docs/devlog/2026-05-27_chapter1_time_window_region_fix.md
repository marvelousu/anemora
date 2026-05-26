# Chapter 1 Time Window Region Fix

## Scope

- Branch: `work/chapter1-continuation-map-vs-20260524`
- Authored files:
  - `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - `Assets/Scripts/TimeManagement/TimeWindowPairedSpacePortalController.cs`
  - `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`

## Changes

- Split the compact visual coordinate field size from the Time Window placement region.
- Kept the visible paired field at `78 x 58` so validation/build does not add a wider background slab.
- Serialized the Time Window controller with a wider `196 x 58` placement region so drag-created windows can reach the right-side Chapter 1 continuation route after the street corner.
- Added validation that the controller's serialized region matches the wider Time Window region and that the region covers D3/E3/F5/F6 route positions with portal width margin.
- Added a review accessor for the controller's region size.

## Validation

- `ValidateChapter1AllMapsBatch`: passed (`Logs/chapter1_timewindow_region_validate_r2.log`)
- `BuildAndValidateBatch`: passed (`Logs/chapter1_timewindow_region_build.log`)
- Player smoke: passed (`Logs/chapter1_timewindow_region_player_smoke.log`, fatal match count 0)
- Review artifact for user review: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
