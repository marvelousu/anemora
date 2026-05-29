# feat(hd2d): soften outdoor directional shadows

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Primary authored file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Cycle runner: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\tools\cycle-runner.ps1`

## Direction

Cycle70 passed structural validation, build, smoke, and push, but the visual gate caught a problem: the house exterior overview read as black slab-like boards instead of soft directional shadows. Cycle71 is a corrective pass. The goal is still a slightly exaggerated HD-2D shadow, but it must remain a ground shadow that blends into the scene rather than a visible black plate.

## Implementation

- Re-tuned the existing Cycle70 outdoor directional shadow objects to lower Y positions and much thinner Y scale.
- Shortened broad house, tree, library, and road casts.
- Switched small prop casts from `hd2d_depth_shadow` to `hd2d_outdoor_occlusion_gradient` so sign/notice shadows do not read as hard black marks.
- Updated `ValidateFastVsHd2dShadowFoundationCycle70OutdoorDirectionalShadow()` to enforce the softened ranges.
- Added `CaptureHd2dShadowFoundationCycle71ScreenshotsBatch()`.
- Added `CaptureHd2dShadowFoundationCycle71ScreenshotsToDirectory(...)` with better house exterior framing than the Cycle70 capture, avoiding the roof-clipped camera angle that made the issue harder to read correctly.

## Worker Cycle

- Scoped prompt issued using the cycle-start protocol for Cycle71.
- Implementation worker: `019e5333-4c49-7030-9b6f-533858dad6c7`
- Worker stayed within the authored file scope.
- Parent review found two house exterior creation values still at the old Cycle70 scale while the validator had already been lowered; parent corrected those values before runner execution.

## Validation Plan

The cycle runner is expected to run:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadowFoundationCycle71ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built Fast VS house slice player

Retained screenshot evidence path:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle71_outdoor_shadow_softening_parent_review_20260523_01`

## Review Notes

- This cycle intentionally prioritizes avoiding black slabs over maximizing shadow drama.
- If the result becomes too subtle, the next cycle should increase opacity/coverage through material or layered soft cards rather than returning to thick hard planes.
- The visual gate must inspect both house exterior and central plaza screenshots before continuing.
