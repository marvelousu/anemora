# HD2D vegetation branching detail

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-15

## Investigation

- Re-read `docs/STATUS.md`, `AGENTS.md`, and the distant panorama/environment uplift handoff before editing. The current implementation line remains `wip/hd2d-point15-recovery-20260612`; `work/chapter1-continuation-*` still exists as Chapter 1 continuation history/content, not the active environment uplift line.
- Checked the live refs: local and remote have `main`, `wip/hd2d-point15-recovery-20260612`, `origin/wip/snapshot-repair-proof-20260603`, and `origin/work/chapter1-continuation-map-vs-20260524`. There is no separate `continuous` implementation branch in this worktree.
- Checked the review publishing mechanism again. Review images stay local under `docs/review/<cycle>/`, are blocked from git by bloat guard, must have `devlog.txt`, and are uploaded by `tools/r2/r2-upload-review.ps1`. The public viewer reads the R2 manifest during its rebuild; if the Anemora push/webhook does not refresh it, `anemora-viewer/public/deploy-refresh.txt` is updated and pushed as an explicit rebuild marker.
- Direct Meshy/Blender MCP tools were not available in this Codex tool context, so this cycle stayed inside the canonical authored setup file and improved the deterministic in-code low-poly vegetation meshes instead of adding external generated model assets.
- The latest contact sheets showed the Phase2 vegetation pass had removed primitive cube/sphere vegetation, but many plants still read as rounded green lumps from the wide camera. This cycle therefore changed structure: add visible branch forks, always-present stems, and leaf-plane fans instead of tint polish.
- Bridge status: `ValidateHouseSliceBatch` still exercises the current/past F1-to-F6 bridge traversal validation path. It passed after this change, so the scaffold is not currently broken. This is not yet a substitute for built-player route evidence.

## Change

- Added `CreateAuthoredVegetationBranchForkMesh`, a deterministic low-poly forked branch mesh built from tapered hexagonal segments.
- Added `CreateAuthoredVegetationLeafFanMesh`, a deterministic set of double-sided angled leaf planes for more directional silhouette.
- Updated every `Phase2VegetationVolume` cluster on all six outdoor maps, current and past, to always include:
  - `Stem`
  - `TwigA`
  - `TwigB`
  - `LeafFanA`
- Updated every `Phase2VegetationGrove` tree form to include:
  - `BranchA`
  - `BranchB`
  - `LeafFanA`
  - `LeafFanB`
- Kept all new pieces visual-only: no colliders, non-arrival landmarks, and no route-layer participation.
- Tightened validation so every outdoor current/past vegetation cluster must now include `LowPolyTrunk`, `BranchFork`, and `LeafFan` authored mesh tokens. A primitive fallback or missing branch detail now fails validation.
- Used only deterministic variation from object names and salt values. No `Random`, `Time`, or `DateTime` placement was introduced.
- Renderer Features were untouched.

## Graphics Plan

Phase 1E, distant vista quality pass: pick one map first, probably House Exterior or Ruins, and make the far panorama read as authored landscape rather than a low-poly wall. Work on real geometry, not sky cards. Add more believable ridge overlap, lower fog/terrain integration, tree-line cutouts, water/valley continuity, and camera-motion parallax proof. Roll out only after the one-map contact and parallax proof are convincing.

Phase 2D, vegetation silhouette pass: continue from this cycle by replacing the remaining overly round plant volumes with map-specific tree/shrub shapes. Add dead branches, saplings, reeds, orchard rows, stumps, fallen limbs, and dry scrub. When external model generation is callable again, swap the in-code meshes for a proper low-poly kit while preserving the current coordinates and validation contracts.

Phase 2E, vegetation composition pass: make each map recognizable from plant ecology alone. House exterior gets domestic yard trees and fence-edge weeds; plaza gets maintained civic greenery plus damaged overgrowth; Mia/Aria get residential shrubs and street saplings; Kaia gets farm rows and orchard rhythm; Ruins gets bridge-edge scrub and reclaimed stone vegetation.

Phase 3A, ground density pass: attack the remaining flat tile read. Add non-repeating grass/path shoulders, dirt lanes, chipped route stones, wet/dry patches, furrows, rubble dust, and edge strips. Keep new materials in `Ch1Ground_*` and `Ch1Surface_*` namespaces.

Phase 3B, architecture/set dressing pass: make buildings and constructed spaces read as authored objects. Add eave thickness, roof trim, wall wear, stone bands, fence contact detail, rubble piles, tools, market/farm props, signboards, and bridge-side construction traces. Avoid adding cards inside cards or purely decorative filler.

Phase 4, lighting and atmosphere pass: after geometry density is readable, strengthen current/past contrast through allowed Volume, APV, fog, skybox, and material response only. Renderer Features remain frozen.

Phase 5, bridge playable proof: convert the bridge from validation scaffold to player-facing evidence. Required proof is F1 to bridge midpoint to F6 traversal in a built player, current-side blocked/collapsed state readability, past-side repair readability, and stone pickup/placement state evidence.

Phase 6, close-camera quality pass: add close review captures per map after the wide maps pass. This should catch bad texel density, low-poly mesh silhouettes, contact shadows, route readability, and player-scale prop quality.

Phase 7, publishing discipline: every visual cycle remains incomplete until Validate, EditMode renderer freeze, asset validation, all-map capture, shotdiff, devlog, review directory, R2 upload, public viewer verification, pathspec commit, and push are done.

## Verification

- Validate: `Logs/vegetation_branching_validate_r1.log` passed with `Fast VS house slice validation passed.` The same run covers the bridge traversal validator.
- Renderer freeze: `Logs/vegetation_branching_editmode_r1.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/vegetation_branching_asset_validation_r1.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/vegetation_branching_capture_r1.log` produced 13 all-map Wide PNGs in `docs/devlog/screenshots/chapter1_all_maps_cycle05`.
- Review packet: `docs/review/2026-06-15T17-49_vegetation_branching/` contains the 13 all-map frames, `00_contact_sheet.png`, and `devlog.txt`.
- Shotdiff: `Logs/shotdiff/vegetation_branching_vs_nearfield_r1/` compared against `docs/review/2026-06-15T17-07_nearfield_dressing`. All 12 Wide frames moved over the 0.05% triage budget: `01_a1_a2_current.png` 0.0640%, `02_a1_a2_past.png` 0.4876%, `03_b1_b3_current.png` 0.2274%, `04_b1_b3_past.png` 0.3005%, `05_c1_c3_current.png` 0.1571%, `06_c1_c3_past.png` 0.2078%, `07_d1_d3_current.png` 0.1089%, `08_d1_d3_past.png` 0.1802%, `09_e1_e3_current.png` 0.1000%, `10_e1_e3_past.png` 0.1756%, `11_f1_f6_current.png` 0.0674%, and `12_f1_f6_past.png` 0.1069%. The contact sheet changed by 6.8925%; the side-view frame remained unchanged.
- Visual review: the all-map contact sheet shows visible branch and leaf-fan silhouettes in every outdoor map. This improves tree read from the wide camera without changing route collision.

## Next

- Run the R2 upload, viewer rebuild/verification, cleanup, pathspec commit, and push for this cycle.
- Next implementation cycle should either make one map's distant vista substantially less low-quality before all-map rollout, or produce built-player bridge traversal evidence if route playability remains the priority.
