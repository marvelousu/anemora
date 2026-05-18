# Stage 4 Floor Underlay Material Tier 4 Pass

Date: 2026-05-08

## Summary

This pass separates the main-scene floor underlay planes from portal debug materials. `Current_Floor` and `Past_Floor` now use dedicated URP Lit floor-underlay materials instead of reusing `Debug_Current.mat` and `Debug_Past.mat`, so the tile gaps / base plane no longer inherit portal debug material tuning.

The production scene edit is intentionally minimal: `Assets/Scenes/Anemora_Main.unity` changed only the two material GUID references on `Current_Floor` and `Past_Floor`.

## Files

- `Assets/Art/Materials/Zone1/Stage4_FloorUnderlay_Current.mat`
- `Assets/Art/Materials/Zone1/Stage4_FloorUnderlay_Past.mat`
- `Assets/Editor/Stage4Zone1MaterialSetup.cs`
- `Assets/Scenes/Anemora_Main.unity`
- `Assets/Tests/EditMode/GraphicsFoundationAssetTests.cs`
- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`

## Material Settings

Both floor-underlay materials use `Universal Render Pipeline/Lit`, opaque surface mode, no texture, low smoothness (`0.08`), and disabled specular highlights / environment reflections.

- Current floor underlay base color: `0.55, 0.49, 0.34, 1.0`
- Past floor underlay base color: `0.34, 0.42, 0.39, 1.0`

## Screenshot Artifacts

Updated main-scene review captures:

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
  - SHA256: `3048EE8B5ABF9CC7AE4E8902A92DA3DD2BFA7B0E051987960F254305260A6B62`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
  - SHA256: `44D32CA9B76232EEDDA87C240AADFF8ED801C0FBA127C87B1E6F775912055296`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`
  - SHA256: `E0959962BCB5B769868F485C9E5F955743C78D73E4D71322AF64BAD98A85CA09`

## Verification

- `Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`
  - Exit code: `0`
  - Checked shader error / shader warning / DrawObjectsPass / RecordRenderGraph / RenderGraph matches: `0`
- `GraphicsFoundationAssetTests`
  - Targeted run: `16/16` passed
- Full EditMode
  - Unity Test Runner: `56/56` passed
  - Log caveat: the batchmode editor log still contains Unity licensing handshake `Error` strings and `LogAssemblyErrors` section names; test result is `Passed`.
- `MainSceneStartupLogTests`
  - Targeted run: `3/3` passed
- Windows build smoke
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-floor-underlay/Anemora_Stage4_GraphicsFoundation_FloorUnderlay_Smoke.exe`
  - Build result: success
  - Build folder: `126.363 MiB`, `193` files
  - Build log caveat: the Code Coverage package still prints `System.Numerics.Vector*` reflection resolution messages during build report generation.
- 30 second player smoke
  - Player was intentionally stopped after the smoke window; process exit code after forced stop: `-1`
  - Checked `Error`, `Exception`, `Assert`, `DrawObjectsPass`, `RecordRenderGraph`, `RenderGraph`, `NullReference`, `MissingReference`, `Failed`, and TMP Essential Resources patterns: `0`

## Residual Notes

- This pass is material-only and does not change portal flip ordering, stencil bit design, camera composition, or gameplay logic.
- No new 120 second performance sample was taken for this material-only pass. The latest Tier 4 soft-contact-occlusion performance baseline remains the active performance reference.

## Follow-up

Later on 2026-05-08, the floor material path was extended by `2026-05-08_stage4_floor_surface_palette_tier4_pass.md`. The current underlay colors are now `0.48, 0.41, 0.28, 1.0` for Current and `0.27, 0.38, 0.36, 1.0` for Past, with dedicated `FloorSurfaces` materials normalized for warm stone, dark stone, moss, and wood separation.
