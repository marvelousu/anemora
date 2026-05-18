# Stage 4 DQ3R Character Unity Transfer Pack v2

Date: 2026-05-09

Built a second, non-destructive character transfer pack for production-candidate review. Unity was not launched, and the first transfer pack remains preserved.

## Transfer Pack

- Root: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v2_2026-05-09/`
- Manifest: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v2_2026-05-09/graphics_foundation_transfer_manifest_v2.json`
- Asset manifest mirror: `docs/asset_manifests/stage4_character_unity_transfer_manifest_v2_2026-05-09.json`
- Import decisions: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v2_2026-05-09/import_decision_table_v2.md`
- Scene matrix: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v2_2026-05-09/scene_compatibility_matrix_v2.md`
- Unity checklist: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v2_2026-05-09/unity_import_checklist_v2.md`
- Redraw list: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v2_2026-05-09/redraw_request_list_v2.md`
- Verification: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v2_2026-05-09/transfer_pack_verification_v2.json`

## Contents

- `runtime_locomotion/32x48/`: preserved prototype copies for the high-priority Chapter 1 set.
- `runtime_locomotion/64x96/`: 64x96 production candidates for Hero, Resident_A, Mia, Kaia, Dario, Karla, Kairo, and Luna.
- `runtime_idle_pose_candidates/`: Resident_B idle-only 64x96 candidate.
- `review_failed_or_redraw_needed/`: Resident_B walk placeholders plus copied review/redraw strips for known risky Karla, Kairo, and Luna animations.
- `composition_compatibility/`: 1x / 2x / 4x composition gates for neutral checker, Current stone, Past market, S1 library, S2 Mia house, S3 Current/Past, S4 Current/Past, S5 fog, and the graphics foundation Unity placement screenshot when available.
- `crowd_64x96/`: Past market, Current ruin, Field shared, and Robot_B1 background groups with individual transparent PNGs.
- `review_only_role_pose/`, `portrait_only/`, `expression_only/`: intentionally separated non-locomotion review assets.

## Import Guidance

Ready for 64x96 candidate review/import:

- Hero / Niro: scaled from approved 32x48 anchor, review before replacing runtime.
- Resident_A / Aria: scaled from approved 32x48 anchor, review before replacing runtime.
- Resident_F / Mia: 64x96 candidate, 6f walk plus 8f review option.
- Resident_C / Kaia: 64x96 candidate, 6f walk plus 8f review option.
- Resident_D / Dario: 64x96 candidate, 6f walk plus 8f review option.
- Resident_J / Karla: 64x96 candidate, review walk-back stability first.
- Resident_K / Kairo: 64x96 candidate, 6f walk plus 8f review option, review walk-right stability.
- Resident_L / Luna: 64x96 candidate, 6f walk plus 8f review option, review walk-front foot contact.

Remain 32x48-only / idle-only:

- Resident_B / Reto: seated idle-only; directional walk needs redraw and must not be imported from placeholders.

## Verification

Ran:

```powershell
python tools\build_stage4_dq3r_unity_transfer_pack_v2.py
```

Result:

- PNG decode: 306 files checked.
- Exact dimension / strip frame-size check: pass.
- Alpha foreground check: pass.
- Manifest path check: pass.
- Portrait/runtime folder mixing check: pass.
- Review gallery rebuild: `python tools\build_review_gallery.py`, 897 images indexed.
- Unity launch: not performed in this session.
