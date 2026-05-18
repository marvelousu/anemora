# Stage 4 DQ3R Character Unity Transfer Pack v4

Date: 2026-05-09

Built a fourth, non-Unity character transfer pack as the production-lock candidate pass requested by `anemora-character-generation-session-dq3r-next-instructions-4-2026-05-09.md`. Unity was not launched in this session.

## Transfer Pack

- Root: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v4_2026-05-09/`
- Manifest: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v4_2026-05-09/graphics_foundation_transfer_manifest_v4.json`
- Asset manifest mirror: `docs/asset_manifests/stage4_character_unity_transfer_manifest_v4_2026-05-09.json`
- Import table: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v4_2026-05-09/import_decision_table_v4.md`
- Priority import list: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v4_2026-05-09/graphics_foundation_priority_import_list_v4.md`
- Redraw queue: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v4_2026-05-09/redraw_request_list_v4.md`
- Verification log: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v4_2026-05-09/transfer_pack_verification_v4.json`
- Unity import path table: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v4_2026-05-09/unity_import_path_table_v4.csv`
- Quick review page: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v4_2026-05-09/quick_review_index_v4.html`

## v4 Focus

- Added true 64x96 redraw candidates for Hero / Niro and Resident_A / Aria by preserving the accepted silhouette while adding 64px-scale clothing, hair, face, hand, hem, and foot detail. The approved 32x48 fallback strips are kept alongside the true64 candidates for identity review.
- Kept Resident_B / Reto on an idle-only production track. The pack includes seated neutral, reading, turn-left, turn-right, breath, attention, far-background silhouette, and optional stand pose variants without fabricating directional locomotion.
- Created watch repair competitions for Resident_J walk_back, Resident_K walk_right, and Resident_L walk_front. v3 references are explicitly copied to `review_failed_or_redraw_needed/`, while v4 accepted-best strips are placed in runtime candidate paths.
- Mirrored those three rejected references into `rejected_animation_strips_v4.md` so the do-not-import list is explicit without cross-reading the redraw queue.
- Added contact shadow, rim, and lower-body occlusion helper assets plus manifests. These are separate helper PNGs and must not be imported as base locomotion sprites.
- Added animation metrics, frame-hold table, rejected-strip notes, scene-fit matrix, scene-fit failure list, role-pose micro-acting bank, portrait/expression manifest, and crowd-density bank.
- Used graphics-foundation screenshots where present to generate scene-fit sheets and flag possible background-merge risks before Unity import.
- Added `production_lock_coverage_matrix_v4.md`, `production_lock_acceptance_risks_v4.md`, and `true64_delta_audit_v4.json` so graphics foundation can check A-J coverage and true64 deltas without reverse-engineering the folder tree.
- Added `unity_import_path_table_v4.csv` / `.md` with 84 runtime candidate rows, frame counts, recommended PPU/FPS, pivot rule, helper mask paths, and import status.
- Added `quick_review_index_v4.html`, a v4-only visual entry point for true64, master contacts, watch repairs, scene-fit sheets, helper overview, and crowd density sheets.
- Added a proportion gate after visual review found that the character set does not yet share a convincing head/body scale. `import_first` is withdrawn; all non-Resident_B production replacements now require proportion review against Hero / Niro and Resident_B / Reto.
- Added `proportion_gate/character_proportion_gate_v4.md`, current lineup, scale-only trials, and bbox metrics. The scale-only trials are review aids only and do not solve the art-direction mismatch by themselves.

## Import Guidance

Proportion-review order before any production replacement:

- Resident_F / Mia
- Resident_C / Kaia
- Resident_D / Dario
- Resident_K / Kairo v4 accepted repair
- Resident_L / Luna v4 accepted repair

Review after comparison:

- Resident_J / Karla, after checking the v4 accepted walk_back repair against the rejected reference.

Identity review before replacement:

- Hero / Niro true64 candidate
- Resident_A / Aria true64 candidate

Idle-only:

- Resident_B / Reto. Use only seated/turning idle candidates unless design changes require a separate locomotion batch.

Do not import as locomotion:

- `portrait_only/`
- `expression_only/`
- `review_only_role_pose/`
- helper mask folders
- rejected references in `review_failed_or_redraw_needed/`

## Verification

Ran:

```powershell
python tools\build_stage4_dq3r_unity_transfer_pack_v4.py
```

Result:

- PNG decode: 548 files checked.
- Exact dimension / strip frame-size check: pass.
- Alpha foreground check: pass.
- Transparent-corner check: pass.
- Helper mask dimension check: pass.
- Manifest path check: pass.
- Duplicate output path collision check: pass.
- Runtime / review-only folder separation check: pass.
- Review gallery rebuild: `python tools\build_review_gallery.py`, 1990 images indexed.
- Quick review page smoke test: served over local `http.server` and opened at `http://127.0.0.1:8791/quick_review_index_v4.html`; browser console reported 0 errors / 0 warnings.
- Unity launch: not performed in this session.
