# Stage 4 Aria Locomotion Pilot v10

Date: 2026-05-09

This pass extends the v9 locomotion pipeline to Aria (Resident_A), starting with the front-walk source generated under the v8c child-read direction. Unity was not launched. Codex generated the v10a front source PNG before stopping; this Claude session built the extraction pipeline, ran it, and added a Niro v9 vs Aria v10 scale compare sheet.

## Outputs

- Root: `docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/`
- Builder: `tools/build_stage4_dq3r_aria_locomotion_pilot_v10.py`
- Scale comparator: `tools/build_stage4_dq3r_aria_v10_vs_niro_v9_scale_compare.py`
- Manifest: `docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/aria_locomotion_pilot_manifest_v10.json`
- Asset manifest mirror: `docs/asset_manifests/stage4_character_aria_locomotion_pilot_manifest_v10_2026-05-09.json`
- Assessment: `docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/aria_locomotion_pilot_assessment_v10.md`
- Review sheet: `docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/review_sheets/resident_a_aria_locomotion_direction_candidates_6f_v10_review.png`
- Scale compare sheet (vs Niro v9a): `docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/review_sheets/aria_v10_vs_niro_v9_walk_front_scale_compare.png`
- Source PNG (2172x724 RGB, magenta chroma key): `docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source/resident_a_aria_walk_front_6f_v10a.png`
- Alpha PNG: `docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/alpha/resident_a_aria_walk_front_6f_v10a_alpha.png`
- Front strip (64x96 6f review-only): `docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/runtime_candidate_64x96_review_only/resident_a_aria_walk_front_v10a_64x96_6f_review_only.png`
- Back / left / right source prompt brief (next image-gen pass): `docs/asset_prompts/stage4_aria_v10_back_left_right_locomotion_prompts.md`

## Metrics (walk_front v10a)

- Frame count: 6, cell size 64x96, target_height 65 (Aria v8c child-read; Niro v9 used 72).
- Head-top drift: 0 px.
- Bottom drift: 0 px.
- Width per frame (post-fit): 25 px (well under the 62 px width-pressure trigger).
- Mean frame delta: 714.2 pixels, motion read `ok` (>180 threshold).
- Status: `watch_not_import_first`.
- Aria silhouette height in cell: 66 px (~92% of Niro 72; height-chain target 0.796/0.854 ~= 93%, on target).
- Color: chroma key via `chroma_key_preserve_rgb` (mirrors `tools/build_stage4_dq3r_true64_redraw_v8.py`); Aria mustard / cream warm tones preserved, no green/dark despill shift.

## Decision

Front-only at this stop point. Aria v10a front passes drift, width and color checks, holds the v8c child-read scale, and slots cleanly under Niro v9a in the scale compare sheet. Not import-first. Back / left / right have not been generated yet; the prompt brief is staged in `docs/asset_prompts/stage4_aria_v10_back_left_right_locomotion_prompts.md` for the next image-generation pass. Once those source PNGs land in `source/`, append entries to `SOURCES` in `tools/build_stage4_dq3r_aria_locomotion_pilot_v10.py` and rerun the builder; the metrics, review sheet, manifest and assessment refresh in place.

## Verification

Ran:

```powershell
python -m py_compile tools\build_stage4_dq3r_aria_locomotion_pilot_v10.py
python tools\build_stage4_dq3r_aria_locomotion_pilot_v10.py
python -m py_compile tools\build_stage4_dq3r_aria_v10_vs_niro_v9_scale_compare.py
python tools\build_stage4_dq3r_aria_v10_vs_niro_v9_scale_compare.py
python -m py_compile tools\build_stage4_dq3r_unified_generation_v7.py tools\build_stage4_dq3r_true64_redraw_v8.py tools\build_stage4_dq3r_locomotion_pilot_v9.py tools\build_stage4_dq3r_aria_locomotion_pilot_v10.py tools\build_stage4_dq3r_aria_v10_vs_niro_v9_scale_compare.py
python tools\build_review_gallery.py
git diff --check -- tools docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09 docs/asset_manifests docs/devlog
```

Results:

- Chroma-key alpha extracted via `chroma_key_preserve_rgb` (RGB-preserving, despill avoided per the v8c green/dark incident in v8).
- 6 frame components detected, cell-fit to 64x96 with target_height 65.
- Front strip + metrics CSV + review sheet + manifest + asset manifest mirror + assessment all written.
- Niro v9a vs Aria v10a scale compare PNG generated.
- All v7/v8/v9/v10 scripts byte-compile clean.
- Review gallery rebuilt.
- `git diff --check` for the v10 paths reported no whitespace issues.
- `tools/__pycache__` cleaned after verification.
- Unity launch: not performed in this session.

## Carry-forward

- See `docs/asset_prompts/stage4_aria_v10_back_left_right_locomotion_prompts.md` for the back / left / right source prompts.
- After back / side sources land, append to `SOURCES` in the v10 builder and rerun.
- `remove_chroma_key.py --despill` must remain off the table for Aria (causes green/dark shift; v8c regression risk).
- Do not import these strips into Unity; they remain `runtime_ready=false`, `unity_import_allowed=false` until accepted in a separate pass.
- See `notes/_handover/anemora-character-generation-claude-aria-v10-front-complete-2026-05-09.md` for the next-session handover.
