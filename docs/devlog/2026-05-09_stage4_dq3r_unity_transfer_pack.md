# Stage 4 DQ3R Character Unity Transfer Pack

Date: 2026-05-09

Built a self-contained character transfer pack for the graphics foundation session. Unity was not launched, and no scene, prefab, Animator, AnimationClip, or `.meta` file was intentionally changed.

## Transfer Pack

- Root: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_2026-05-09/`
- Manifest: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_2026-05-09/graphics_foundation_transfer_manifest.json`
- Import decision table: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_2026-05-09/import_decision_table.md`
- Graphics foundation checklist: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_2026-05-09/graphics_foundation_import_checklist.md`
- Asset manifest mirror: `docs/asset_manifests/stage4_character_unity_transfer_manifest_2026-05-09.json`
- Verification: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_2026-05-09/transfer_pack_verification.json`
- Asset ledger addendum: `docs/legal/asset_ledger_stage4_character_addendum_2026-05-08.md`

## Contents

- `prototype_runtime_32x48/`: prototype-ready copies for Hero, Resident_A, Resident_B, and Chapter 1 priority NPCs.
- `production_candidate_64x96/`: runtime-ready candidate strips for Dario, Karla, Kairo, and Luna.
- `master_review_96x144/`: larger review sheets for the same high-priority named characters.
- `framecount_review_64x96/`: 4f / 6f / 8f comparison strips and analysis sheets for Dario, Kairo, and Luna.
- `scene_compatibility/`: lighting/readability gate sheets for neutral checker, Stage 4 Current, Stage 4 Past, S1 library, S2 Mia house, S3 market, S4 Kaia field, and S5 north-road fog.
- `crowd_groups/`: grouped background-character sheets for Past market, Current ruin, Future review, and Robot_B1.
- `review_only_expression_portrait/`: expression, portrait, role-pose, diagonal, and costume-detail materials copied into a non-runtime boundary folder.

## Import Guidance

Ready for 32x48 Unity prototype import:

- Hero / Niro
- Resident_A / Aria
- Resident_B / Reto
- Resident_F / Mia
- Resident_C / Kaia
- Resident_D / Dario
- Resident_J / Karla
- Resident_K / Kairo
- Resident_L / Luna

Ready for 64x96 production-candidate import:

- Resident_D / Dario
- Resident_J / Karla
- Resident_K / Kairo
- Resident_L / Luna

Keep review-only:

- 96x144 master sheets
- portrait-only assets
- expression-only assets
- scene compatibility sheets

Recommended framecount:

- Mia, Kaia: keep 4f for now.
- Dario, Kairo, Luna: use 6f as the first production candidate; keep 8f as a review option.
- Karla: 4f/6f candidate, but verify the prior walk-back stability flag before production lock.

## Verification

Ran:

```powershell
python tools\build_stage4_dq3r_unity_transfer_pack.py
```

Result:

- PNG decode: 199 files checked.
- Alpha foreground check: pass.
- Exact dimension check: pass.
- Required 64x96 strip names: 36/36 present, no bad dimensions.
- Manifest path check: pass.
- Jitter/metrics output: `animation_quality_metrics.csv` and `.json` generated.
- Review gallery rebuild: `python tools\build_review_gallery.py`, 591 images indexed.
- Unity launch: not performed in this session.
