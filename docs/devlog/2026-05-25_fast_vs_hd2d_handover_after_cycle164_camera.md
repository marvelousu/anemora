# Anemora Fast VS HD-2D Handover After Cycle164

## Stop State

- User requested this cycle stop before any camera-behavior implementation.
- Worktree was clean before this handover file was written.
- No Unity Editor / build player process was left running; only Unity Hub / licensing processes were observed.
- Current branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Latest pushed commit: `2fc829b2 fix(fast-vs): keep exterior exit camera off tree`
- Remote is up to date at `origin/work/fast-vs-hd2d-shading-foundation-20260522`.

## Last Completed Work

Cycle164 fixed the latest-build bug where leaving the house framed the tree area and felt unusable.

- Changed Exterior follow camera minimum anchor so house exit stays playable.
- Added validation that runs the actual Interior -> Exterior door transition.
- Added checks that the exit anchor stays on the player, stays away from the tree, and allows movement after the warp.
- Validate / Capture / Build / Smoke passed.
- Review screenshots:
  `docs/devlog/screenshots/fast_vs_hd2d_cycle164_exterior_exit_tree_clearance_parent_review_20260525_01`
- Latest build:
  `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

## User's Latest Direction

User says camera follow behavior is still wrong and should behave like VS.

This should be treated as a gameplay/camera parity issue, not a lighting polish issue.

## Camera Investigation Already Started

The VS continuation branch was checked:

- `origin/work/chapter1-continuation-map-vs-20260524:Assets/Scripts/FastVS/FastVsVisualDirectionGuide.cs`
- `origin/work/post-vs-public-20260518:Assets/Scripts/FastVS/FastVsVisualDirectionGuide.cs`
- `work/fast-vs-hd2d-polish-20260520:Assets/Scripts/FastVS/FastVsVisualDirectionGuide.cs`

Those VS-side versions use plain player-local follow:

- `ResolveActiveSideCameraAnchor()` returns `root.TransformPoint(portalController.GetPlayerLocalCoordinateForReview())`.
- No Exterior min-X/min-Z clamp.
- No CentralPlaza max-Z clamp.
- Follow camera uses the same base offset/look behavior:
  - position offset: `(0, 2.75, -4.55)`
  - look offset: `(0, 0.72, 0.45)`
- Older VS-side code did not use area-specific follow profiles for Exterior/CentralPlaza/Library.

Current HD-2D branch differs materially:

- `Assets/Scripts/FastVS/FastVsVisualDirectionGuide.cs`
  - `CentralPlazaVsCameraMaxAnchorZ = 14.70f`
  - `ExteriorVsCameraMinAnchorX = 7.05f`
  - `ExteriorVsCameraMinAnchorZ = 5.00f`
  - `ResolveActiveSideCameraAnchor()` clamps CentralPlaza and Exterior anchors.
  - `GetFollowCameraProfile()` uses area-specific offsets/FOV:
    - Exterior: `(0.70, 2.85, -5.25)`, look `(0.25, 0.78, 0.90)`, FOV `39`
    - CentralPlaza: `(0, 3.55, -6.50)`, look `(0, 1.18, 1.35)`, FOV `40`
    - Library: `(0.25, 2.95, -5.05)`, look `(0.10, 0.84, 0.74)`, FOV `39`

These differences were introduced while trying to make visual review captures readable, but they are likely the reason runtime camera behavior no longer feels like VS.

## Validation Impact If Restoring VS Camera

Several validation gates currently hard-code the non-VS camera profiles or depth clamps. If the next session restores VS-like camera behavior, these tests must be updated deliberately instead of patched around.

Known affected areas in `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`:

- Cycle127 central plaza profile check around `ValidateHd2dPlazaRealtimeLightShadowCycle127`.
- Cycle134 / Cycle135 / Cycle147 central plaza profile checks.
- Cycle151 / Cycle156 central plaza anchor depth clamp checks.
- Cycle162 Exterior and Library profile checks.
- Cycle163 Exterior anchor/profile checks.
- Cycle164 exit clearance validation should remain conceptually useful, but should validate VS-style player-follow instead of a clamp.
- Later exterior readability validation near the `exterior follow camera position offset` and `exterior snapshot camera offset` checks.

Also affected:

- `Assets/Editor/AnemoraFastVsHd2dVisualSnapshotAudit.cs`
  - House exterior snapshot profile still uses the HD-2D-specific exterior offset/FOV.

## Recommended Next Session Approach

1. Read the new HD-2D procedure document first and follow it over the older cycle assumptions.
2. Decide whether runtime camera should exactly match VS and only capture tools should use special review camera positions.
3. If yes, restore runtime `FastVsVisualDirectionGuide` to VS-like player-local follow:
   - remove area anchor clamps
   - use shared follow offset/look/FOV for runtime mode
   - keep any HD-2D-only framing in capture helpers, not runtime follow
4. Update validation gates so they protect VS-like runtime behavior and allow special review/capture-only camera framing separately.
5. Run full cycle runner with Validate / Capture / Build / Smoke before commit/push.

## Do Not Carry Forward

- Do not continue Cycle165 from this aborted turn.
- No camera behavior implementation was committed after Cycle164.
- Do not assume the current area-specific camera profiles are correct just because older lighting validations require them.
