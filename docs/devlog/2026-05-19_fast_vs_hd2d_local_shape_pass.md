# 2026-05-19 Fast VS HD2D Local Shape Pass

## Scope

This pass resumes HD-2D visual work on `work/post-vs-public-20260518` after the rejected global lighting/volume experiment.

The scope is intentionally local:

- Do not reintroduce the reverted global lighting/URP/Volume pass.
- Do not change the player camera contract or Time Window V24 same-coordinate behavior.
- Add small shape/trim/depth cues that can be reviewed quickly in the current Fast VS build.
- Keep DotGothic16 as the locked Fast VS dialogue font direction.

## Worker Cycle

The requested gpt-5.4-mini worker cycle was used.

- gpt-5.4-mini worker `019e3d22-19a0-7fa2-b330-d73f89f6539a` handled library table/book/shelf-local shape work in `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
- gpt-5.4-mini worker `019e3d22-6031-7902-976c-50b7babae680` handled house, exterior, plaza, and facade trim work in `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
- The parent session reviewed the combined patch, adjusted table-leg depth/material separation, restored the DotGothic16 TMP asset from the previous verified stash, regenerated the scene, captured PNGs, validated, and rebuilt the Windows player.

## Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added interior wall top trim, bed fold/shadow, and small table edge/shadow details.
  - Added exterior house roof/facade trim and porch lip details.
  - Added plaza library facade base/header trim, door/window lips, and stone-square border strips.
  - Replaced current/past library reading-table blocks with assembled tabletop/edge/shadow/leg parts while preserving existing no-step colliders.
  - Added `CaptureHd2dLocalShapeScreenshotsBatch()` so this pass has a separate PNG review directory.

- `Assets/Editor/AnemoraTmpJapaneseAtlasBuilder.cs`
  - Restored the DotGothic16 source font path for the Fast VS Japanese TMP atlas builder.

- `Assets/Scripts/FastVS/FastVsStoryDialoguePresenter.cs`
  - Sets `TMP_Settings.defaultFontAsset` before creating legacy TMP dialogue text objects, matching the runtime HUD path and preventing startup fallback-font warnings.

- `Assets/UI/Localization/Fonts/Anemora_JP.asset`
  - Restored the DotGothic16 TMP font asset.
  - Rebound `m_Material` to `Assets/UI/Localization/Fonts/Anemora_JP_DistanceField.mat` to remove the runtime "Font Atlas Texture missing" warning seen after the first restore.

- `Assets/UI/Localization/Fonts/Anemora_JP_Atlas.asset`
  - Restored the paired DotGothic16 atlas asset.

- `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
  - Regenerated from the updated integrator.

## Verification

- Scene generation:
  - Log: `Logs/fast_vs_hd2d_local_shape_create_scene_20260519.log`
  - Result: `Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity`

- Validation:
  - Final log: `Logs/fast_vs_hd2d_local_shape_validate_final_20260519.log`
  - Result: `Fast VS house slice validation passed.`
  - No remaining `Font Atlas Texture` warning after the material rebind.

- PNG review:
  - Log: `Logs/fast_vs_hd2d_local_shape_capture_20260519.log`
  - Output: `docs/devlog/screenshots/fast_vs_hd2d_local_shape_20260519/`
  - Captured interior, exterior, plaza current/past, library current/past, Timewriter pocket glow, and current-side Time Window cues.

- Windows build:
  - Log: `Logs/fast_vs_hd2d_local_shape_build_20260519.log`
  - Result: `Fast VS house slice player built: Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

- Player smoke:
  - Log: `Logs/fast_vs_hd2d_local_shape_player_smoke_20260519.log`
  - Result: no `error`, `exception`, `failed`, `crash`, `NullReference`, `MissingReference`, `Font Atlas Texture`, `LiberationSans`, or `Unicode` matches after the legacy presenter default-font fix.

- Git hygiene:
  - Removed unrelated Unity-generated ProjectSettings, URP, and Addressables diffs after the build.
  - `git diff --check` passes.

## Review Notes

- The pass makes the build less blockout-like, but it is still a conservative shape pass, not a final art pass.
- The current plaza still has legacy dry-fountain debris that can read as a black vertical object from some angles. It was not changed in this pass to avoid mixing unrelated review items.
- The library mezzanine and side shelves now read better in screenshots, but the next larger visual pass should focus on replacing repeated cube/shelf panels with authored sprites or generated texture sheets.
