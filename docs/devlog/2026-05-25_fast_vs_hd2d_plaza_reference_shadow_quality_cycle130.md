# feat(hd2d): push plaza reference shadow quality

## Intent

Cycle 129 removed the worst hard-caster silhouettes, but the image still read as a flat yellow-brown wash instead of HD-2D sun and shadow. Cycle 130 takes the faster direct-art route: reduce the full-screen brown grade, raise the central-plaza camera back toward a diorama angle, and paint separate white sun patches, ink shadows, contact shadows, and atmospheric shafts.

## Scope

- Raise the current central-plaza follow camera to a more reference-like high angle.
- Push central-plaza runtime sun brighter and warmer while keeping ambient low/cool.
- Rework the runtime camera grade away from full yellow-brown coverage toward subtle edge darkening and stronger white shafts.
- Add current-plaza Cycle 130 transparent reference quads:
  - two broad dark ink-shadow floor bands,
  - one bright sun-bleached floor patch,
  - three near-black contact shadows,
  - two pale vertical sun shafts.
- Add generated bilinear Cycle 130 ink-shadow, sun-patch, contact-shadow, and sun-shaft textures/materials.
- Capture selected review images into both `docs/devlog/screenshots/` and `docs/review/`.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 130 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceShadowQualityCycle130Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceShadowQualityCycle130ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_reference_shadow_quality_cycle130.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle130_plaza_reference_shadow_quality_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_reference_shadow_quality_follow.png`
- `parent_review_02_current_central_plaza_reference_shadow_quality_floor.png`
- `parent_review_03_current_central_plaza_reference_shadow_quality_facade.png`
- `parent_review_04_current_library_reference_shadow_quality_guard.png`

## Visual Gate

- Central plaza should no longer read as a uniform yellow-brown wash.
- Sunlit floor patches should visibly lift toward pale reference highlights.
- Contact shadows and broad shade should read closer to inked HD-2D occlusion.
- Follow and floor shots should show the character, floor, and facade together from a higher diorama angle.
- Library guard should remain unaffected.
