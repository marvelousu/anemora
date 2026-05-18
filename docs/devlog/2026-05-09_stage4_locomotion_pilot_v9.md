# Stage 4 Locomotion Pilot v9

Date: 2026-05-09

This pass tests whether direct image generation can produce usable 64x96 locomotion frames from the Niro v8b readability direction. Unity was not launched.

## Outputs

- Root: `docs/review_gallery/imports/stage4_dq3r_character_locomotion_pilot_v9_2026-05-09/`
- Manifest: `docs/review_gallery/imports/stage4_dq3r_character_locomotion_pilot_v9_2026-05-09/locomotion_pilot_manifest_v9.json`
- Asset manifest mirror: `docs/asset_manifests/stage4_character_locomotion_pilot_manifest_v9_2026-05-09.json`
- Assessment: `docs/review_gallery/imports/stage4_dq3r_character_locomotion_pilot_v9_2026-05-09/locomotion_pilot_assessment_v9.md`
- Review sheet: `docs/review_gallery/imports/stage4_dq3r_character_locomotion_pilot_v9_2026-05-09/review_sheets/hero_niro_locomotion_direction_candidates_6f_v9_review.png`
- Strips:
  - `docs/review_gallery/imports/stage4_dq3r_character_locomotion_pilot_v9_2026-05-09/runtime_candidate_64x96_review_only/hero_walk_front_v9a_64x96_6f_review_only.png`
  - `docs/review_gallery/imports/stage4_dq3r_character_locomotion_pilot_v9_2026-05-09/runtime_candidate_64x96_review_only/hero_walk_back_v9a_64x96_6f_review_only.png`
  - `docs/review_gallery/imports/stage4_dq3r_character_locomotion_pilot_v9_2026-05-09/runtime_candidate_64x96_review_only/hero_walk_left_v9a_64x96_6f_review_only.png`
  - `docs/review_gallery/imports/stage4_dq3r_character_locomotion_pilot_v9_2026-05-09/runtime_candidate_64x96_review_only/hero_walk_right_v9a_64x96_6f_review_only.png`
  - `docs/review_gallery/imports/stage4_dq3r_character_locomotion_pilot_v9_2026-05-09/runtime_candidate_64x96_review_only/hero_walk_left_v9b_64x96_6f_review_only.png`
  - `docs/review_gallery/imports/stage4_dq3r_character_locomotion_pilot_v9_2026-05-09/runtime_candidate_64x96_review_only/hero_walk_right_v9b_64x96_6f_review_only.png`

## Metrics

- Directions: walk_front, walk_back, walk_left, walk_right, plus v9b side stride-reduction candidates.
- Frame count: 6 per direction.
- Cell size: 64x96.
- Head-top drift: 0 px for all four directions.
- Bottom drift: 0 px for all four directions.
- Motion read: ok.
- Status: `watch_not_import_first`.

## Decision

The v9 pilot is useful evidence that direct generation can produce coherent 64x96 Niro locomotion pilots after fixed-cell extraction. It is not an import-first asset. Front/back are stronger than side-view frames. v9b improves side-view width pressure and should be preferred over v9a if this path continues.

## Verification

Ran:

```powershell
python tools\build_stage4_dq3r_locomotion_pilot_v9.py
```

Results:

- Chroma-key alpha source processed.
- 6 frame components detected for each of four directions.
- 64x96 strips generated for front/back/left/right and v9b side stride-reduction candidates.
- Metrics CSV and review sheet generated.
- Unity launch: not performed in this session.
