# Stage7x: Public Review Shadow Legibility

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Respond to the public-review complaint that the review images still contain black/dark regions that make the scene hard to inspect.

## Changes

- Lowered realtime review shadow strength for CentralPlaza, Exterior, and Library in `FastVsRealtimeLightShadowRig`.
- Reduced runtime receiver shadow strength, shadow texture strength, and sprite-card world shadow receive for outdoor review areas.
- Kept CentralPlaza ambient within the existing validation band and lifted legibility through shadow/receiver/overlay changes; raised review ambient for Exterior and Library, which remain internal diagnostics for this cycle.
- Reduced the dark camera paint overlays used by the cycle128/cycle131 review pass.
- Added batch entry points:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7PublicReviewShadowLegibilityBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7PublicReviewShadowLegibilityReferenceScreenshotsBatch`
- Wired `ValidateHd2dStage7PublicReviewShadowLegibility()` into the full house-slice validation chain.
- Updated older Stage142/143/148/149/150/161/162 validation thresholds so full validation now rejects over-dark review shadow values instead of requiring the previous heavy black-shadow settings.

## Public Review Cleanup

- Removed older tracked public review entries/images whose central dark coverage or overall black coverage made them poor viewer-facing evidence, then stripped the corresponding image links from their `index.md` files.
- Removed the Stage7x `home.png`, `Home_outside.png`, and `library.png` candidates from the public review set because they still contain dark masses or weak composition evidence and should remain internal diagnostics until improved further.
- The public review copy intentionally excludes external target reference images, comparison boards, and route-close obstruction diagnostics.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7PublicReviewShadowLegibilityBatch`
  - Exit 0.
  - Log: `Logs\stage7x_public_review_shadow_legibility_validate_r3.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7PublicReviewShadowLegibilityReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage7x_public_review_shadow_legibility`
  - Log: `Logs\stage7x_public_review_shadow_legibility_capture_r4.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - Build finished successfully.
  - Log: `Logs\stage7x_public_review_shadow_legibility_build_r6.log`
- Built player smoke:
  - Log: `Logs\stage7x_public_review_shadow_legibility_smoke.log`
  - No `Exception`, `NullReference`, `MissingReference`, assertion, shader error, or C# compile-error matches in the 24-second null-graphics smoke.

## Dark-Region Measurement

Compared with `docs/review/2026-05-27T01-42/`:

- `plaza_01.png`: central dark `<45` reduced from `0.215` to `0.081`.
- `plaza_02_niro_in_shadow.png`: central dark `<45` reduced from `0.277` to `0.099`.
- `tw_current_aperture.png`: central dark `<45` stayed low at `0.003`.
- `home.png`, `Home_outside.png`, and `library.png` remain too dark or compositionally weak for public review and were not copied into the new public set.
- After cleanup, no tracked `docs/review/**/*.png` exceeded the scan thresholds used here (`overall dark <45 >= 0.45` or `central dark <45 >= 0.30`).

## Build

- Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Review/build note: フォルダごと起動

## Remaining Gaps

- This cycle improves review legibility for the curated public set, but the Exterior and Library still need more authored light, material, and composition work before they should be public review evidence again.
- The current HD-2D quality remains substantially below the reference target.
