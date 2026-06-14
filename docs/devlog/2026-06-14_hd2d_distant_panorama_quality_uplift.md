# Improve HD2D distant panorama authored depth

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-14

## Investigation

- Reviewed `docs/STATUS.md`, `AGENTS.md`, and `docs/handoff/2026-06-13_distant_panorama_vista_and_environment_uplift.md` before editing.
- Reviewed the latest all-map contact sheet at `docs/review/2026-06-14T11-09_environment_uplift_phase1_4_apv_rebake/00_contact_sheet.png`.
- The distant vista was no longer void, but the wide frames still read as a large green wall because the panorama used 14 radial segments and each ridge segment had only a sparse vertical slab silhouette.
- The bridge route validation checks clearance and side-bypass blockers, but it still does not prove a built-player crossing route with the intended time-window bridge sequence.

## Change

- Increased the distant panorama radial density from 14 to 28 segments.
- Pushed the authored panorama layers farther from the map edge: foothill forest at 66m, near hills at 72m, mid treeline at 94m, and far peaks at 118m.
- Added a deterministic foothill forest silhouette layer using the existing `Ch1Distant_*MidTreeline` materials, so the cycle stays within the authored-file scope.
- Rebuilt ridge meshes from simple vertical strips into denser low-poly relief with shoulder, peak, thickness, and double-sided quads.
- Lowered the near ridge height and extended linear fog to support the farther ring instead of hiding a close wall with color polish.
- Raised validation thresholds so the vista requires at least 24 radial segments, a far radius beyond 112m, an extra foothill layer, and denser relief meshes.

## Forward Plan

1. Distant vista quality pass
   - Establish the updated look in all wide review frames.
   - If any frame still reads as a wall, adjust geometry distance, mesh profile, and fog range before any color polish.
   - Add parallax evidence from two camera positions once the static wide pass is accepted.

2. Midground and map-edge transition
   - Replace hard edge aprons with authored slope/field/brush shapes per area.
   - Add per-area occluding foreground/midground silhouettes where the ring meets the playable map.
   - Keep every new layer collider-free and on the current/past render layers.

3. Vegetation production pass
   - Replace remaining cube/sphere-like plant reads with reusable low-poly trees, shrubs, grass clumps, and deadwood variants.
   - Preserve existing placement coordinates first, then add deterministic density variation from area/index.
   - Validate no primitive plant colliders are introduced.

4. Surface and building material pass
   - Split ground, road, stone, plaster, roof, and ruin materials into authored Chapter 1 surface families.
   - Break wide-frame tile repetition with mesh chips, overlays, and texel-density checks.
   - Keep new surface materials outside existing cycle material namespaces.

5. Lighting and atmosphere pass
   - Strengthen current/past air-grade difference through Volume, fog, skybox/background, and APV only.
   - Do not change the frozen URP renderer feature list.
   - Capture all maps current/past after APV rebake.

6. Bridge traversal and time-window puzzle pass
   - Add a route audit that proves bridge crossing from F1 to F6, not just route clearance.
   - Separate visual bridge pieces from walkable bridge surfaces and side blockers.
   - Implement the canon two-hop/midpoint-pier bridge flow with built-player evidence before calling traversal accepted.

## Verification

- Validate:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
  - Passed in `Logs/distant_panorama_quality_validate_r2.log`.
- Renderer freeze:
  - `Unity.exe -batchmode -projectPath . -runTests -testPlatform editmode -testResults Logs/distant_panorama_quality_editmode_r2.xml`
  - Passed `36/36`; `RendererFeatureSet_MatchesFrozenBaseline` result is `Passed`.
- Asset validation:
  - `Anemora.EditorTools.AnemoraAssetValidation.ValidateImportedAssetsBatch`
  - Passed in `Logs/distant_panorama_quality_asset_validation_r2.log` with `[AssetValidation] OK`.
- Capture:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureChapter1AllMapsCycle05ScreenshotsBatch`
  - Passed in `Logs/distant_panorama_quality_capture_r2.log`.
  - Review cycle: `docs/review/2026-06-14T17-14_distant_panorama_quality_uplift`
- Visual review:
  - Contact sheet: `docs/review/2026-06-14T17-14_distant_panorama_quality_uplift/00_contact_sheet.png`.
  - The prior wall-like valley apron is reduced to a narrow foothill band; all current/past wide frames keep non-void distant panorama coverage.
  - Remaining quality gap: mountain bands are still broad silhouettes and should be broken into richer midground/terrain forms in the next cycle.
- Shotdiff:
  - Compared against `docs/review/2026-06-14T11-09_environment_uplift_phase1_4_apv_rebake`.
  - `00_contact_sheet.png` is a size mismatch because this cycle uses a compact contact-sheet layout.
  - Image threshold flags are `11_f1_f6_current.png` at `2.7024%` and `12_f1_f6_past.png` at `0.7169%`; other map frames remain under the `0.5%` budget.
- R2/viewer:
  - Uploaded `docs/review/2026-06-14T17-14_distant_panorama_quality_uplift` with `tools/r2/r2-upload-review.ps1`.
  - Manifest HEAD OK: `https://pub-d14764d639a647339a6b0d81de923abf.r2.dev/manifests/wip-hd2d-point15-recovery-20260612.json`
  - Viewer: `https://anemora-viewer.pages.dev/wip-hd2d-point15-recovery-20260612/review`
