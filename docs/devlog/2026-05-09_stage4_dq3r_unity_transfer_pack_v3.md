# Stage 4 DQ3R Character Unity Transfer Pack v3

Date: 2026-05-09

Built a third, non-destructive character transfer pack focused on production tightening rather than broad gallery expansion. Unity was not launched, and the v1/v2 transfer packs remain preserved.

## Transfer Pack

- Root: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v3_2026-05-09/`
- Manifest: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v3_2026-05-09/graphics_foundation_transfer_manifest_v3.json`
- Asset manifest mirror: `docs/asset_manifests/stage4_character_unity_transfer_manifest_v3_2026-05-09.json`
- Import table: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v3_2026-05-09/import_decision_table_v3.md`
- Priority import list: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v3_2026-05-09/graphics_foundation_priority_import_list_v3.md`
- Redraw queue: `docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v3_2026-05-09/redraw_request_list_v3.md`

## v3 Focus

- Stabilized v2 watch strips for Resident_J walk_back, Resident_K walk_right, and Resident_L walk_front; v2 originals are copied into `review_failed_or_redraw_needed/` for comparison.
- Kept Resident_B as idle-only with stronger rationale and added seated/turning/read/breath idle candidates instead of fabricating invalid locomotion.
- Preserved or derived `96x144` master sources for every `64x96` export, plus master contact sheets and 96 -> 64 -> 32 downscale comparison sheets.
- Added contact shadow, soft foot shadow, rim mask, and lower-body occlusion helper assets as separate transparent PNGs.
- Added animation timing metrics, recommendations, and per-character foot-contact marker sheets.
- Added role-pose, portrait, expression, and crowd bank assets in separate non-locomotion folders.
- Used graphics foundation review screenshots where available for compatibility gates.

## Import Guidance

Import-priority 64x96 candidates:

- Resident_F / Mia
- Resident_C / Kaia
- Resident_D / Dario
- Resident_K / Kairo
- Resident_L / Luna
- Resident_J / Karla after walk_back comparison

Review-only scale anchors:

- Hero / Niro
- Resident_A / Aria

Do not import as locomotion:

- Resident_B directional movement; idle-only candidates only.
- `portrait_only/`, `expression_only/`, `review_only_role_pose/`, helper masks, and v2 originals in `review_failed_or_redraw_needed/`.

## Verification

Ran:

```powershell
python tools\build_stage4_dq3r_unity_transfer_pack_v3.py
```

Result:

- PNG decode: 545 files checked.
- Exact dimension / strip frame-size check: pass.
- Alpha foreground check: pass.
- Transparent-corner check: pass.
- Manifest path check: pass.
- Duplicate output path collision check: pass.
- Runtime / review-only folder separation check: pass.
- Review gallery rebuild: `python tools\build_review_gallery.py`, 1442 images indexed.
- Unity launch: not performed in this session.
