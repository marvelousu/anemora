# 2026-05-22 Fast VS HD2D Outdoor Framed Light Planes Cycle 39

## Scope
- Expanded the outdoor HD-2D lighting composition for the house exterior and central plaza so the audit camera reads larger, softer, and more deliberate light planes at first glance.
- Kept gameplay collision, transitions, story, dialogue, UI, player control, and time-window behavior unchanged.

## Intent
- Replace the too-small cycle 38 contact accents with a second-level plane pass that reads clearly at audit distance.
- Keep the image composed rather than simply darker.

## Files Changed
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_occlusion_gradient.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_occlusion_gradient.mat.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_warm_stage_light.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_warm_stage_light.mat.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_warm_stage_light_soft.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_warm_stage_light_soft.asset.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_outdoor_framed_light_planes_cycle39.md`

## Implementation
- Added a new non-arrival framed-light plane pass after the outdoor contact grounding pass.
- House exterior:
  - soft under-eave occlusion gradient plane
  - cool side-light plane on the porch/right wall side
  - cool leading porch-plane highlight toward the door
- Central plaza:
  - broad facade occlusion gradient across the library face
  - visible door and approach light planes
  - left/right return occlusion planes and low plinth light planes to deepen the facade edges
- Added deterministic local assets:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_occlusion_gradient.mat`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_warm_stage_light.mat`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_outdoor_warm_stage_light_soft.asset`

## Validation
- House validation command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_framed_light_planes_cycle39_validate_worker_20260522.log`
- Visual snapshot audit command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_outdoor_framed_light_planes_cycle39_capture_worker_20260522.log`

## Snapshot Output
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\02_current_house_exterior_visual_snapshot.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\03_current_central_plaza_visual_snapshot.png`
- Parent review snapshot directory:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle39_outdoor_framed_light_planes_parent_review_20260522_01`

## Parent Review Notes
- The parent review copied the refreshed audit screenshots into the cycle-specific parent review directory above.
- The new framed planes are visible in the audit captures without introducing black masks, hard UI-like rectangles, or door/player obstruction.
- The pass is accepted as a visible lighting-composition step; the next cycle should tune opacity and placement rather than adding more unrelated geometry.

## Residual Risk
- The broad planes should read clearly at audit distance, but the exact balance between the new framed planes and the pre-existing cycle 111 contact grounding still needs visual confirmation in the batch capture.
- If the facade feels slightly over-layered, the next adjustment should trim opacity before trimming geometry.
