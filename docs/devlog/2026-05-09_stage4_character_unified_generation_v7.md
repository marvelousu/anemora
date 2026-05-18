# Stage 4 Character Unified Generation v7

Date: 2026-05-09

User review flagged that the v4/v5 character candidates still did not share a convincing body scale. This pass creates a unified full-cast generation set and a stricter proportion gate. Unity was not launched.

## Outputs

- Root: `docs/review_gallery/imports/stage4_dq3r_character_unified_generation_v7_2026-05-09/`
- Manifest: `docs/review_gallery/imports/stage4_dq3r_character_unified_generation_v7_2026-05-09/unified_generation_manifest_v7.json`
- Asset manifest mirror: `docs/asset_manifests/stage4_character_unified_generation_manifest_v7_2026-05-09.json`
- Assessment: `docs/review_gallery/imports/stage4_dq3r_character_unified_generation_v7_2026-05-09/unified_generation_assessment_v7.md`
- Quick review HTML: `docs/review_gallery/imports/stage4_dq3r_character_unified_generation_v7_2026-05-09/quick_review_index_v7.html`

## Candidate Images

- `source/generated_unified_lineup_v7a.png`: first unified cast candidate.
- `source/generated_unified_lineup_v7b_scale_locked.png`: scale-locked prompt candidate.
- `source/generated_unified_lineup_v7c_height_calibrated.png`: height-calibrated prompt candidate.
- `alpha/*_alpha.png`: chroma-key removed versions for measurement and review.

## Review Sheets

- `review_sheets/unified_generation_source_contact_v7.png`
- `review_sheets/unified_generation_proportion_gate_v7.png`
- `review_sheets/unified_generation_floor_readability_v7.png`
- `review_sheets/unified_generation_v7c_stand_cell_targets.png`

## Findings

- v7a and v7b are useful style references but have component-segmentation and scale issues.
- v7c is the strongest redraw target. It clears the intended scale chain: Niro > Aria > Luna, with adults kept near a shared baseline.
- v7c has coherent costume density and better cast-wide style consistency than the previous v4/v5 material.
- v7 is still not runtime-ready. It is front-facing only and does not provide idle or locomotion animation strips.

## Fixed-Cell Targets

`stand_targets_64x96_review_only/` and `stand_targets_96x144_review_only/` contain v7c stand references cut into fixed cells. These are review-only proportion locks for the next animation redraw pass; they must not replace runtime locomotion assets.

## Verification

Ran:

```powershell
python tools\build_stage4_dq3r_unified_generation_v7.py
```

Results:

- v7 source images copied into the repo.
- Chroma-key alpha images created with the imagegen helper.
- Proportion metrics CSV generated.
- v7c 64x96 / 96x144 stand target PNGs generated.
- Review HTML generated.
- Unity launch: not performed in this session.
