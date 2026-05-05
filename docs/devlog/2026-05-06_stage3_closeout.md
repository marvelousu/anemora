# Stage 3 Closeout

Date: 2026-05-06
Commit: `a0bd50b` as latest implementation input; this devlog records the closeout documentation pass.

## Summary

Stage 3 is accepted as complete for the Vertical Slice. The final blocker class was G5 manual confirmation: Windows build launch, playable demo flow, time-window brush feel, UI visibility, and core book-reflection loop. After the `a0bd50b` repair, the user confirmed that the latest drag feel and related demo behavior are acceptable.

The closeout state is:

- Latest build path: `C:\Users\maro6\Documents\Unity\Anemora-demo-repair\Builds\DemoPlayable\Anemora_Demo_Playable.exe`.
- Latest pushed implementation commit: `a0bd50b Repair demo playable time-window brush flow`.
- EditMode: `32/32` passed in the Codex rerun.
- PlayMode: `29/29` passed in the Codex rerun.
- Windows build: success; `demo_build_drag_precision.log` reports `Build Finished, Result: Success`.
- Runtime: previous closeout handover reported exception-free Player.log.

## Final Manual Observation

The final repair changed the time-window brush from a fixed default-size placement into a ground-raycast rectangle flow:

- `Shift` + left-drag shows `TimeWindow_BrushPreview_Runtime`.
- The preview uses a translucent blue fill and cream outline.
- Mouse release creates `TimeWindow_Diorama` with the same center, same size, and matching floor footprint.
- Minimum drag window size is `0.75 x 0.75` Unity units.
- Maximum remains the serialized `maxLocalWindowSize`, currently `9 x 8`.
- Right-click deletion remains available.

The user confirmed the latest drag feel. No remaining Stage 3 blocker was reported.

## Documents Updated

This closeout pass promotes the Stage 3 docs from "G5 pending" to "Stage 3 complete":

- `docs/G5_ACCEPTANCE_MATRIX.md` records manual G5 closeout observations for §H / §I / §L / §M.
- `docs/VS_SCOPE.md` is promoted to v1.0 and marks all mandatory VS completion items as accepted.
- `docs/STAGE3_RETROSPECTIVE.md` is promoted to v1.0 and records Stage 3 closeout numbers.
- `docs/STAGE4_ROADMAP.md` is promoted to v1.0 and becomes the Stage 4 entry roadmap.
- `docs/G5_PREFLIGHT.md` and `docs/VERIFICATION_SUITE.md` are updated to the latest closeout test baseline.

## Stage 4 Carry-Forward

The following are not Stage 3 blockers and should be treated as Stage 4 backlog:

- URP `DrawObjectsPass` / RenderGraph compatibility warning cleanup.
- Niro sprite v2 / hat silhouette review.
- Dialogue v1 polish and broader continuity review.
- Audio loop / mix / SFX replacement pass.
- TMP font, palette, and UI readability review.
- Brush tutorialization and polish now that the core drag precision is fixed.
- Steam Early Access preparation workstreams in `docs/STAGE4_ROADMAP.md`.
