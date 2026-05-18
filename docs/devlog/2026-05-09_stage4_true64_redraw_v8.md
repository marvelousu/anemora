# Stage 4 True64 Redraw v8

Date: 2026-05-09

This pass follows the v7 proportion lock with review-only true64 turnaround candidates for Hero / Niro and Resident_A / Aria. Unity was not launched.

## Outputs

- Root: `docs/review_gallery/imports/stage4_dq3r_character_true64_redraw_v8_2026-05-09/`
- Manifest: `docs/review_gallery/imports/stage4_dq3r_character_true64_redraw_v8_2026-05-09/true64_redraw_manifest_v8.json`
- Asset manifest mirror: `docs/asset_manifests/stage4_character_true64_redraw_manifest_v8_2026-05-09.json`
- Assessment: `docs/review_gallery/imports/stage4_dq3r_character_true64_redraw_v8_2026-05-09/true64_redraw_assessment_v8.md`
- Review sheet: `docs/review_gallery/imports/stage4_dq3r_character_true64_redraw_v8_2026-05-09/review_sheets/true64_redraw_v8_niro_aria_turnaround_review.png`

## Candidates

- Niro v8a: usable direction consistency, but too dark after 64x96 extraction.
- Niro v8b: stronger readability candidate with better face/hand contrast.
- Aria v8a: clean model sheet, but reads older than the target.
- Aria v8b: slightly better young-read candidate, still review-only until accepted.
- Aria v8c: stricter child-read candidate. This is the best Aria reference in v8, but still review-only until accepted against Niro and Luna.

## Fixed-Cell Exports

- `model_pose_64x96_review_only/`
- `model_pose_96x144_review_only/`

These exports are direction model references only. They are not locomotion strips and must not replace runtime assets.

## Decision

- Prefer Niro v8b over v8a for the next true64 locomotion prompt or manual sprite pass.
- Keep Aria v8c as the best Aria reference, but continue to monitor age-read against Niro and Luna.
- Do not import v8 as gameplay sprites yet.

## Verification

Ran:

```powershell
python tools\build_stage4_dq3r_true64_redraw_v8.py
```

Results:

- Niro / Aria v8 source images copied into the repo.
- Chroma-key alpha images created with the imagegen helper; fixed-cell extraction preserves source RGB to avoid magenta-despill color shifts.
- 64x96 / 96x144 model-pose PNGs generated.
- Component count: 8 poses per candidate.
- Unity launch: not performed in this session.
