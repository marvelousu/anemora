# 2026-05-21 Fast VS HD2D House Exterior Eave Contact Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_eave_contact_20260521`

This cycle adds a narrow visual-only HD-2D pass around Niro house exterior eaves, porch posts, door threshold, and foundation contact lines. The goal is to make the house sit more believably on the ground without changing player movement, map transition pads, Time Window behavior, story, UI/font, character animation, colliders, or map bounds.

No API token, paid asset purchase, or external download was used.

## Changes

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Wired `CreateHouseExteriorEaveContactPolish(...)` immediately after `CreateHouseExteriorFacadeMicrodepthPolish(...)`.
- Added six current non-arrival landmark cubes:
  - `Current_HouseExterior_EaveContact_FrontUnderEaveDustShadowA`
  - `Current_HouseExterior_EaveContact_RoofSideBreakShadowA`
  - `Current_HouseExterior_EaveContact_DoorThresholdDustA`
  - `Current_HouseExterior_EaveContact_PorchPostFootShadowLeftA`
  - `Current_HouseExterior_EaveContact_PorchPostFootShadowRightA`
  - `Current_HouseExterior_EaveContact_FoundationScuffLineA`
- Added six past non-arrival landmark cubes:
  - `Past_HouseExterior_EaveContact_FrontUnderEaveWarmShadowA`
  - `Past_HouseExterior_EaveContact_RoofSideCleanHighlightA`
  - `Past_HouseExterior_EaveContact_DoorThresholdWarmA`
  - `Past_HouseExterior_EaveContact_PorchPostFootWarmLeftA`
  - `Past_HouseExterior_EaveContact_PorchPostFootWarmRightA`
  - `Past_HouseExterior_EaveContact_FoundationCleanLineA`
- Added `ValidateFastVsHd2dEightyThirdCycleHouseExteriorEaveContact()`.
- Added `CaptureHd2dEightyThirdCycleScreenshotsBatch()` and `CaptureHd2dEightyThirdCycleScreenshotsToDirectory(...)`.
- Wired the new validation into `ValidateHouseSliceBatch()`.

## Validation

Worker handoff:

- Worker `019e48bf-be4c-77d3-ba0e-b191e1705c0e` produced the core code patch but did not complete validation or devlog before shutdown.
- Parent session reviewed and finished validation, capture, build, smoke, and repository hygiene.

Parent validation log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle83_house_exterior_eave_contact_parent_validate_20260521.log`
- Result: passed with `Fast VS house slice validation passed.`

Parent screenshot capture log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle83_house_exterior_eave_contact_parent_capture_20260521.log`
- Result: passed with `Fast VS eighty-third-cycle screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_eave_contact_20260521`

Parent build log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle83_house_exterior_eave_contact_parent_build_20260521.log`
- Result: passed with `Build Finished, Result: Success.`
- Note: Unity emitted unrelated startup/license/import noise, but the batch completed successfully.

Parent smoke log:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle83_house_exterior_eave_contact_parent_smoke_20260521.log`
- Result: 20-second startup smoke completed with no matches for `Exception`, `Error`, `Failed`, `NullReference`, `MissingMethod`, or `Crash`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_eave_contact_20260521\01_current_house_exterior_eave_contact_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_eave_contact_20260521\02_past_house_exterior_eave_contact_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_eave_contact_20260521\03_current_house_exterior_eave_contact_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_eave_contact_20260521\04_past_house_exterior_eave_contact_close.png`

## Notes

- The pass stayed deterministic and non-colliding.
- All new objects are thin `PropOrFeature` landmarks built from `CreateNonArrivalLandmarkCube(...)`.
- Existing map movement glow pads and house door landmarks are asserted by the new validation to avoid regressing the transition contract.
- The visual read in close screenshots is intentionally subtle: current space receives dust/shadow grounding, while past space receives warmer contact accents.
