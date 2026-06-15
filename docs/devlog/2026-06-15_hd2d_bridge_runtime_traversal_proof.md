# HD2D bridge runtime traversal proof

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- The earlier bridge cycles proved static support and editor-side `CharacterController` traversal, but not a built-player run. That left a real acceptance gap: the bridge could still have passed scene validation while failing in the executable.
- The player object in the generated house-slice scene has a `CharacterController`, and the paired-space controller already exposes `MovePlayerLocalForReview(..., useCharacterController: true)`. That gave this cycle a narrow runtime proof path without changing route geometry, renderer features, or scene-generation layout.
- A first built-player proof run with `-nographics` reached F6 in both current and past, but produced blank gray PNGs because the graphics device capped render textures at 512. The accepted review packet therefore uses the second built-player run without `-nographics`, while retaining the same traversal command and pass criteria.
- Visual review of the new bridge proof frames also confirms the next graphics weakness: the bridge is functional and readable, but it still reads as box-authored planks and rails rather than a production bridge asset. That should be a near-term authored art pass after the traversal proof is protected.

## Change

- Added a built-player runtime proof mode to `FastVsHouseRuntimeSmokeProbe`:
  - command-line argument: `--anemora-house-slice-bridge-proof-dir`,
  - log marker: `ANEMORA_HOUSE_SLICE_BRIDGE_TRAVERSAL`,
  - output: current and past start, midspan, and F6-exit PNGs,
  - failure behavior: exits non-zero if support is missing, movement is blocked, the active time side changes, the player leaves the playable height band, or F6 is not reached.
- Reused the editor validator's F1-to-F6 route, PathOrFloor raycast support checks, and small deterministic `CharacterController` movement steps.
- Kept this cycle focused on proof infrastructure: no renderer feature changes, no route geometry changes, no procedural backdrop path, and no random placement.
- Refreshed the all-map Wide capture packet so the cycle remains visible in the review pipeline even though the scene visuals are intentionally unchanged.

## Updated Graphics Plan

Phase 5A, bridge authored asset pass: keep the proven traversal lane, then replace the bridge's box read with authored mesh detail. Add uneven planks, rail breaks, side beams, diagonal braces, pier caps, approach ramps, river-bank contact shadows, and current/past repair-state differences. Acceptance requires the runtime bridge proof to continue passing after every art iteration.

Phase 5B, bridge route readability: make the crossing read as the intended path in wide and player-height shots. Add approach dressing, broken-side blockers, safer visual thresholds, and route-state cues without adding blocking colliders above the walk lane. Acceptance is both visual readability and built-player traversal from F1 to F6.

Phase 1G follow-up, distant vista authored mesh kit: the all-map panorama is no longer void, but the mountain and shore silhouettes still need authored low-poly kit replacement. Build one approved kit first, then roll to all outdoor maps. Do not tune only colors if the silhouette or texel scale reads wrong.

Phase 2 follow-up, vegetation authored kit: the proof frames still show primitive-looking tree masses. Replace the remaining cube/sphere-like vegetation with a species kit: broadleaf, sapling, reed, hedge, stump, dead scrub, flower/seed head, and ruin overgrowth. Reuse existing coordinates before densifying.

Phase 3 follow-up, ground and building surfaces: the bridge proof frames expose repeated ground planes and box buildings around the route. Continue the `Ch1Ground_*` / `Ch1Surface_*` material separation, then add edge breakup, chipped stone, dirt shoulders, plaster trim, roof fascia, door/window returns, and under-eave shadow planes.

Phase 4 follow-up, atmosphere and lighting: after geometry and material passes read, strengthen current/past contrast with allowed fog, skybox, Volume overrides, and APV. Renderer features remain frozen.

Phase 7 operations: every visual or traversal cycle must publish Validate, EditMode renderer freeze, AssetValidation, all-map Wide captures, review packet, devlog, R2 upload, viewer refresh, pathspec commit, and push. A built-player pass without review images is not accepted; a review image packet without a devlog is also not accepted.

## Verification

- Validate: `Logs/bridge_runtime_proof_validate_r1.log` passed with `Fast VS house slice validation passed.`
- Build: `Logs/bridge_runtime_proof_build_r1.log` passed and built `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.
- Built-player proof r1: `Logs/bridge_runtime_proof_player_r1.log` passed traversal in both current and past, but `-nographics` produced blank PNGs. This run is log evidence only.
- Built-player proof r2: `Logs/bridge_runtime_proof_player_r2.log` passed traversal in both current and past with rendered PNGs. Current final local was `(88.79, 0.12, 15.95)` with delta `0.115`; past final local was `(88.79, 0.12, 15.95)` with delta `0.115`.
- Renderer freeze: `Logs/bridge_runtime_proof_editmode_r1.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/bridge_runtime_proof_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- All-map capture: `Logs/bridge_runtime_proof_allmap_capture_r1.log` produced 13 all-map Wide PNGs in `docs/devlog/screenshots/chapter1_all_maps_cycle05`.
- Shotdiff: `Logs/shotdiff/bridge_runtime_proof_vs_distant_vista_rollout_r1/summary.txt` compared against `docs/review/2026-06-15T20-17_distant_vista_all_map_rollout`. The 13 all-map frames were unchanged at `0.0000%`; only the previous cycle's contact sheet was missing from the candidate comparison set.
- Review packet: `docs/review/2026-06-15T22-13_bridge_runtime_traversal_proof/` contains bridge proof frames, all-map frames, `00_contact_sheet.png`, `01_all_maps_contact_sheet.png`, and `devlog.txt`.

## Next

- Upload the review packet to R2 and refresh anemora-viewer so the new bridge proof is visible from the public review route.
- Start the next graphics cycle on bridge authored art or vegetation authored kit. The bridge proof should be rerun after any bridge art/collider work.
