# 2026-05-22 Fast VS HD2D House Exterior Architectural Closure Cycle 30

## Scope

Cycle 30 closes the visible exterior shell on the house review camera angles so the outside reads like a solid building instead of exposing interior, backside, or open-shell gaps.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The user complaint was a structural readability problem, not a shadow-density problem. This cycle adds an architectural closure layer over the existing house exterior, keeps the outdoor sky/background work intact, and leaves gameplay, triggers, transitions, and time-window behavior unchanged.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_house_exterior_architectural_closure_cycle30.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Implementation

`CreateExterior(...)` now calls `CreateHouseExteriorArchitecturalClosurePolish(...)` immediately after the existing facade composition polish pass.

The new closure pass adds current/past non-arrival, shadow-safe landmark cubes for:

- front wall continuous panel
- left corner return
- right corner return
- rear roof depth cap
- left roof rake cap
- right roof rake cap
- under-eave ambient occlusion strip
- door vestibule side shadow
- back leak mask left
- back leak mask right

Parent review tightened the pass after visual inspection:

- moved the continuous front wall panel back so it no longer covered the visible door/window plane
- added left/right/top door reveal fills
- widened the exterior closed-door panel so the close review angle no longer exposed the old interior gap
- reduced the old doorway-dark mask to a narrow seam
- changed the new left-side vestibule element from a black shadow strip to a door-detail trim so it reads as a wooden jamb instead of another hole

The current world uses `CurrentExteriorWall`, `CurrentRoof`, `CurrentFence`, `CurrentStone`, and `Shadow`. The past world uses the corresponding past materials plus `Shadow`.

The main validation sequence now includes `ValidateFastVsHd2dThirtiethCycleHouseExteriorArchitecturalClosure()`, and the new helper checks:

- current and past objects exist
- correct parent space
- renderer with a matching material token
- `TimeWindowPairedSpaceLandmark`
- `countsForArrival = false`
- no colliders
- tight local placement around `HouseExteriorCenter`
- reasonable scale and shadow-safe rendering

## Validation Performed

- `git diff --check`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_architectural_closure_cycle30_validate_worker_20260522.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_architectural_closure_cycle30_validate_parent_door_trim_20260522.log`
- `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dVisualSnapshotAudit.CaptureAndVerifyFastVsHd2dVisualSnapshotAuditBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_architectural_closure_cycle30_capture_parent_door_trim_20260522.log`

## Results

- `git diff --check` completed with the existing CRLF warning for `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
- Worker Unity batch validation did not complete because the editor log reported compile errors around the story-validation text block.
- Parent review traced those compile errors to a Cycle 29 text-encoding regression in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`, restored the C# file from the last clean base, and reapplied only the intended Cycle 29 and Cycle 30 additions.
- Parent validation was rerun after the encoding repair and passed in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_architectural_closure_cycle30_validate_parent_door_trim_20260522.log`.
- The visual snapshot audit passed in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_house_exterior_architectural_closure_cycle30_capture_parent_door_trim_20260522.log`.
- The reviewed house exterior snapshot is `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522\02_current_house_exterior_visual_snapshot.png`.

## Residual Risk

- The new architectural-closure placements are constrained and additive, but the result still needs rendered review against the close house-exterior camera, because a valid shell can still read as too flat if the closure panels dominate the shot.
- This cycle closes the obvious see-through shell defects. It does not yet solve the larger art-direction issue: the exterior still has an oversized roof/wall read and needs a dedicated facade, roof-depth, sky/background, and map-edge composition pass.
