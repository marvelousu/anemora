# 2026-05-22 Fast VS HD2D House Exterior Hero Readability Cycle 34

## Scope

Cycle 34 is a stronger house-exterior readability pass for the Fast VS HD-2D house slice. It is focused on the exterior runtime/snapshot view only: roof mass, facade read, shell closure, camera composition, and the exterior wall plate texture. It does not touch story, dialogue, time-window behavior, transitions, player controller, UI, or the main branch.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

Cycle 33 was a small proportion cleanup and still left the exterior reading as a roof-heavy board with weak facade presence. Cycle 34 pushes farther:

- the exterior camera now frames more facade, porch, ground, sky, and right-side context;
- the roof silhouette is smaller and less screen-dominant;
- the wall plate sampler reads more like weathered plaster/masonry with broader lighting and subtler seams;
- a new hero-readability polish layer closes the shell more intentionally around the facade and porch;
- the validation gates now reflect the new composition and texture language.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsVisualDirectionGuide.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dVisualSnapshotAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_house_exterior_hero_readability_cycle34.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle34_house_exterior_hero_readability_parent_review_20260522_01\`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle34_house_exterior_hero_readability_parent_review_20260522_02_mask_removed\`

## What Changed

- Exterior follow camera in `FastVsVisualDirectionGuide` was shifted to a wider, slightly farther, right-biased framing.
- Matching house-exterior snapshot camera constants in `AnemoraFastVsHd2dVisualSnapshotAudit` were updated to the same composition.
- `CreateExteriorHouseArchitecture` now uses a smaller roof body, narrower eaves, a shorter ridge cap, and a reduced chimney mass.
- `CreateHouseExteriorOcclusionShell` was tightened so the roof depth cap and under-eave mask are less board-like.
- `SampleCurrentExteriorWallHd2dPixel`, `SamplePastExteriorWallHd2dPixel`, and `SampleWeatheredWallPlatePixel` were reworked toward a more plaster/masonry-like read with broad lighting and much subtler seam behavior.
- `CreateHouseExteriorHeroReadabilityPolish(...)` was added and called after `CreateHouseExteriorProportionCleanupPolish(...)` to ground the facade, porch, eaves, and shell closure with non-colliding hero-readability pieces.
- `ValidateFastVsHd2dThirtyFourthCycleHouseExteriorHeroReadability()` was added to `ValidateHouseSliceBatch()`.
- Cycle 33 roof cleanup validation bounds were widened to match the new roof proportions, and the exterior wall texture contrast gate was updated to a vertical lighting contrast probe instead of a stripe-oriented seam probe.
- Parent review found black rectangular backdrop masks still visible behind the roof/tree. Those were traced to `HouseExterior_FacadeComposition_BackdropSideMaskLeftA/RightA`; the objects were removed, and the stronger composition sky panel now handles the background without reading as holes.
- The screenshot workflow now preserves cycle-specific review copies instead of relying only on the overwritten broad audit folder.

## Validation Performed

- Unity batch house validation:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_hero_readability_cycle34_validate_worker_20260522.log'`
  - Result: `Fast VS house slice validation passed.`
- Unity batch visual snapshot audit:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_hero_readability_cycle34_capture_worker_20260522.log'`
  - Result: `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Parent Unity batch house validation:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_hero_readability_cycle34_validate_parent_20260522.log'`
  - Result: `Fast VS house slice validation passed.`
- Parent Unity batch visual snapshot audit:
  - `& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_hero_readability_cycle34_capture_parent_20260522.log'`
  - Result: `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`

## Output Evidence

- Screenshot directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`
- Exterior screenshot to inspect:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\02_current_house_exterior_visual_snapshot.png`
- Metrics file:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\visual_snapshot_metrics_cycle10_20260522.md`
- Parent review screenshot copy before mask removal:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle34_house_exterior_hero_readability_parent_review_20260522_01\02_current_house_exterior_visual_snapshot.png`
- Parent review screenshot copy after mask removal:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle34_house_exterior_hero_readability_parent_review_20260522_02_mask_removed\02_current_house_exterior_visual_snapshot.png`

## Visual Review Notes

The exterior snapshot now reads as a house first, not a roof plate first. Parent review caught that the first pass still contained black rectangular backdrop masks behind the roof/tree; the final pass removes those masks and keeps the exterior closed through the sky/backdrop foundation instead. The image still leaves room for follow-up polish on the facade silhouette and ground-plane rhythm, but the facade is more legible, the roof is less overwhelming, and the shell closure no longer reads as open or hollow.

Snapshot metrics for the exterior shot are:

- average luminance: `0.286`
- luminance range: `0.588`
- local contrast: `0.0323`

## Residual Risk

- The wall texture gate was relaxed to match the new texture language, so future wall-art changes should watch that probe rather than assuming the old seam check still means the same thing.
- The exterior snapshot is stronger, but the composition still depends on the current camera framing and may need another pass if later facade work changes the silhouette again.
- The snapshot audit is still a broad gate, not a substitute for manual review of the exterior PNG.
- Follow-up direction from review: outdoor areas still need a stronger ground/terrain continuation outside the playable tile so they do not read as floating sky islands, and the plaza/library pass needs to reconcile the library exterior volume with the tall, multi-level interior read.
