# 2026-06-14 HD2D environment uplift Phase 1-4 and APV closeout

## Scope

- Close out the authored environment uplift for `wip/hd2d-point15-recovery-20260612`.
- Record the final review-image and devlog evidence after Phase 1 through Phase 4.
- Keep review imagery out of git per the R2 image workflow.

## Implementation Summary

The environment uplift was completed as a sequence of one-map prototype cycles followed by all-map expansion cycles:

| Commit | Cycle | Result |
|---|---|---|
| `c33abea6` | Phase 1 prototype | Added the first distant panorama vista. |
| `1e60b7db` | Phase 1 all maps | Expanded distant panorama vistas to all outdoor maps. |
| `5feac77f` | Phase 2 prototype | Added the first authored vegetation prototype. |
| `a7578849` | Phase 2 all maps | Expanded authored vegetation to all outdoor maps. |
| `312c30d4` | Phase 3 prototype | Added HouseExterior production ground and surface materials. |
| `f3fa104a` | Phase 3 all maps | Expanded production surfaces to all outdoor maps. |
| `f412c9ee` | Phase 4 prototype | Added HouseExterior current/past lighting grade. |
| `9a5e2467` | Phase 4 all maps | Expanded current/past lighting grade to all outdoor maps. |
| `d552daae` | Phase 4 APV | Rebuilt APV baking-set source data after the lighting uplift. |

No URP Renderer Feature was added, removed, or reordered. The work stayed on authored environment geometry, materials, Volume profiles, RenderSettings fog, APV setup, and generated first-party assets.

## Final Review Images

Local review directory:

```text
docs/review/2026-06-14T11-09_environment_uplift_phase1_4_apv_rebake
```

Contents:

```text
00_contact_sheet.png
01_a1_a2_current.png
02_a1_a2_past.png
03_b1_b3_current.png
04_b1_b3_past.png
05_c1_c3_current.png
06_c1_c3_past.png
07_d1_d3_current.png
08_d1_d3_past.png
09_e1_e3_current.png
10_e1_e3_past.png
11_f1_f6_current.png
12_f1_f6_past.png
13_scene6_sideview_auto.png
devlog.txt
```

The source capture was `docs/devlog/screenshots/chapter1_all_maps_cycle05/phase4_apv_rebake_*.png`. Those files and the review directory are intentionally not committed; `docs/review/` and `docs/devlog/screenshots/` are R2-managed evidence paths.

Visual read:

- `01` through `12` preserve the Phase 1-4 acceptance shape across current/past: no exposed map-edge void in the reviewed wide frames, a circular distant panorama, authored vegetation, broken-up ground/building surfaces, and clear current/past air-grade difference.
- `13_scene6_sideview_auto.png` remains a dark sideview automation aid and is not the primary acceptance target.

## Verification

APV rebake:

```text
Logs/phase4_apv_rebake_r1.log
Fast VS Stage 7 APV baked GI completed with 8 baked cells in Assets/Settings/FastVS_HouseSlice_Stage7_APV_BakingSet.asset.
```

Validate:

```text
Logs/validate_phase4_apv_rebake_r1.log
HD2D surface texture metric audit passed
Fast VS house slice validation passed.
```

Renderer freeze:

```text
Logs/editmode_phase4_apv_rebake_r2.xml
testcasecount="36" result="Passed" total="36" passed="36" failed="0"
RendererFeatureSet_MatchesFrozenBaseline result="Passed"
```

Asset validation:

```text
Logs/asset_validation_phase4_apv_rebake_r1.log
[AssetValidation] OK - no missing references, no review-only leaks, no oversized meshes.
```

Capture:

```text
Logs/capture_phase4_apv_rebake_r1.log
Fast VS chapter 1 all maps screenshots captured: docs/devlog/screenshots/chapter1_all_maps_cycle05
```

## R2 / Git Hygiene

- The review directory has a `devlog.txt` whose first non-empty line points back to this devlog.
- Review images are not staged or committed.
- R2 upload succeeded with 16 manifest paths for slug `wip-hd2d-point15-recovery-20260612`.
- Public HEAD checks returned 200 for:
  - `https://pub-d14764d639a647339a6b0d81de923abf.r2.dev/manifests/wip-hd2d-point15-recovery-20260612.json`
  - `https://pub-d14764d639a647339a6b0d81de923abf.r2.dev/tree/wip-hd2d-point15-recovery-20260612/docs/review/2026-06-14T11-09_environment_uplift_phase1_4_apv_rebake/00_contact_sheet.png`
  - `https://pub-d14764d639a647339a6b0d81de923abf.r2.dev/tree/wip-hd2d-point15-recovery-20260612/docs/devlog/2026-06-14_hd2d_environment_uplift_phase1_4_apv_rebake_closeout.md`
- APV `*.Cell*.bytes` payloads remain ignored per `.gitignore`; the committed APV source change is the `FastVS_HouseSlice_Stage7_APV_BakingSet.asset` update from `d552daae`.
- Unity batch side effects such as `Assets/AddressableAssetsData/link.xml`, material whitespace reserialization, texture meta whitespace, tracked screenshot churn, and `DefaultVolumeProfile.asset` were restored before commits.

## Closeout

The environment uplift reached authored production quality for the current review target across all outdoor maps and both time spaces. The final committed branch head before this devlog cycle was:

```text
d552daae Rebake environment APV lighting data
```

This documentation/evidence pass exists because the implementation cycles had produced local screenshots but had not yet created the required R2 review directory or devlog record.
