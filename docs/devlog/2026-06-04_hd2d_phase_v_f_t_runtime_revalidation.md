# 2026-06-04 HD-2D Phase V/F/T Runtime Revalidation

Continuation branch: `wip/snapshot-repair-proof-20260603`.

After Phase R runtime repair commit `306a944b`, the Tom hard-stop was explicitly waived by the user, so the remaining handoff sequence continued through Phase V, F, and T preparation.

## Phase V

- Reused the Phase R post-repair built-player gate as the current runtime baseline: 25s player run, no `NullReferenceException`, no `Exception`, and no `error` hits in `Player.log`.
- Reused the Phase R 5-area review folder `docs/review/2026-06-04T08-08_phase_r_playtest_regression` as current-area evidence for home, outside, plaza, past plaza, and library.
- Triaged backlog item 64 through 85 as conservative data or parked NEEDS-TOM baselines rather than final art approval.
- Added the missing NEEDS-TOM rows for P2-64, P3-84, and P3-85 to `docs/HD2D_TOM_DECISION_SHEET.md`.

## Phase F

- Re-ran `CaptureHd2dAutonomousFlatGroundRealFixBatch` after Phase R.
- New folder `docs/review/2026-06-04T08-37_flat_ground_real_fix` shows non-zero A/B evidence: current `40.186%`, past `39.953%`.
- The older failure mode was that fixed ground materials reached renderer assignments but not visible pixels. After R-1 narrowed runtime wash suppression, the existing renderer-level material route now reaches the captured ground pixels.

## Phase T

- Decision sheet remains a preparation artifact only. No NEEDS-TOM item was blindly finalized.
- P2-66 and P2-73 are treated as auto-safe runtime-clean items, not Tom taste rows.
- Final `BuildAndValidateBatch` log `build_phase_vft.log` reached `Fast VS house slice validation passed.` and `Build Finished, Result: Success.` with no `error CS` hits.
- Final built-player 25s run copied to `docs/review/2026-06-04T08-39_phase_v_revalidation/Player_phase_vft.log` had zero `NullReferenceException|Exception|error` hits.
- Push and PR were not performed.
