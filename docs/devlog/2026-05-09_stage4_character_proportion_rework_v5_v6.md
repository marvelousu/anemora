# Stage 4 Character Proportion Rework v5/v6

Date: 2026-05-09

User review flagged the v4 character transfer pack as visually unsafe: the nominal `64x96` cell contract was consistent, but head/body proportions and face scale did not read as one cast. This pass keeps v4 intact as source material and creates review-only proportion candidates. Unity was not launched.

## v5 Mechanical Rework

- Root: `docs/review_gallery/imports/stage4_dq3r_character_proportion_rework_v5_2026-05-09/`
- Manifest: `docs/review_gallery/imports/stage4_dq3r_character_proportion_rework_v5_2026-05-09/character_proportion_rework_manifest_v5.json`
- Asset manifest mirror: `docs/asset_manifests/stage4_character_proportion_rework_manifest_v5_2026-05-09.json`
- Main review sheet: `docs/review_gallery/imports/stage4_dq3r_character_proportion_rework_v5_2026-05-09/review_sheets/character_proportion_rework_lineup_v5.png`

v5 tried three deterministic variants:

- `A_scale_only`: whole-sprite scaling.
- `B_head_reduce`: reduce upper-head mass.
- `C_unified_body`: reduce head mass and slightly extend body read.

Result: v5 is useful as a failure/diagnostic pass, but not a production solution. Scaling and split-body transforms do not fix the underlying art-direction mismatch cleanly.

## v6 Source Extract

- Root: `docs/review_gallery/imports/stage4_dq3r_character_source_extract_v6_2026-05-09/`
- Manifest: `docs/review_gallery/imports/stage4_dq3r_character_source_extract_v6_2026-05-09/source_extract_manifest_v6.json`
- Asset manifest mirror: `docs/asset_manifests/stage4_character_source_extract_manifest_v6_2026-05-09.json`
- Assessment: `docs/review_gallery/imports/stage4_dq3r_character_source_extract_v6_2026-05-09/source_extract_assessment_v6.md`
- Main review sheet: `docs/review_gallery/imports/stage4_dq3r_character_source_extract_v6_2026-05-09/review_sheets/source_extract_vs_v4_v5_lineup_v6.png`
- Scene floor preview: `docs/review_gallery/imports/stage4_dq3r_character_source_extract_v6_2026-05-09/review_sheets/source_extract_scene_floor_preview_v6.png`

v6 extracts stand-only candidates from `niro_post_cast_master_01.png`, while keeping Hero / Niro and Resident_B / Reto as v4 anchors. This produces a better proportion target than v4/v5 for the non-anchor cast, but it is not animation-ready.

## Current Decision

- v4 remains blocked for production replacement because cast proportion is not accepted.
- v5 is diagnostic only.
- v6 is a stand-only redraw target candidate.
- No v5/v6 asset is approved for production import.
- Next useful art step is to re-author full animation sheets against the v6 proportion target, or generate a new unified full-cast animation sheet using Hero / Reto as fixed anchors.

## Verification

Ran:

```powershell
python tools\build_stage4_dq3r_character_proportion_rework_v5.py
python tools\build_stage4_dq3r_source_front_proportion_candidates_v6.py
```

Results:

- v5 PNG decode: 263 files checked.
- v6 PNG decode: 11 files checked.
- v5/v6 dimension checks: pass.
- v5/v6 alpha checks: pass.
- Manifest path checks: pass.
- Review gallery rebuild: `python tools\build_review_gallery.py`, 2264 images indexed.
- Unity launch: not performed in this session.
