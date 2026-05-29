# Stage7u: Sprite Card Cutout Depth

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Close the Stage 7 plan gap where sprite cards still used the transparent unlit ramp path instead of a cutout lit/depth-writing path.

## Changes

- Added `Assets/Art/Shaders/FastVS/FastVS_SpriteCardRampLit.shader`.
  - Queue/tag path: `AlphaTest` / `TransparentCutout`
  - Alpha clipping: `clip(baseSample.a - _Cutoff)`
  - Depth path: `ZWrite [_ZWrite]`
  - Main-light path keeps URP cookie/shadow participation with `_LIGHT_COOKIES` and `GetMainLight(...)`
  - ShadowCaster pass applies the same cutoff.
- Updated sprite card material setup so generated Niro, Reto, Aria, hedge, and tree card materials use `Anemora/FastVS/SpriteCardRampLit`.
  - `_Cutoff`: `0.15`
  - `_AlphaClip`: `1`
  - `_ZWrite`: `1`
  - Render queue: `2450`
  - `RenderType`: `TransparentCutout`
  - `ShadowCaster`: enabled
- Added `ValidateHd2dStage7SpriteCardCutoutDepth()` and wired it into `ValidateHouseSliceBatch()`.
- Added batch entry points:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7SpriteCardCutoutDepthBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7SpriteCardCutoutDepthReferenceScreenshotsBatch`
- Updated sprite-card material and lighting audits to require the cutout/depth-writing shader path.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7SpriteCardCutoutDepthBatch`
  - Exit 0.
  - Log: `Logs\stage7u_sprite_card_cutout_validate.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
  - Exit 0.
  - `HD2D material role audit passed.`
  - `HD2D sprite card lighting audit passed.`
  - `Fast VS house slice validation passed.`
  - Log: `Logs\stage7u_sprite_card_cutout_validate_house.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7SpriteCardCutoutDepthReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7u_sprite_card_cutout_depth`
  - Log: `Logs\stage7u_sprite_card_cutout_capture.log`
- Candidate public images were checked for central black obstruction:
  - `home.png`: center average luminance `0.241`, dark pixel ratio `0.00%`
  - `Home_outside.png`: center average luminance `0.214`, dark pixel ratio `0.01%`
  - `plaza_01.png`: center average luminance `0.290`, dark pixel ratio `0.58%`
  - `plaza_02_niro_in_shadow.png`: center average luminance `0.268`, dark pixel ratio `0.21%`
  - `library.png`: center average luminance `0.228`, dark pixel ratio `6.42%`
  - `tw_current_aperture.png`: center average luminance `0.408`, dark pixel ratio `0.00%`
- Route-close diagnostic captures were not copied into `docs/review`:
  - `01_current_plaza_to_library_route_glow_close.png`
  - `02_past_plaza_to_library_route_glow_close.png`
- `plaza_02_niro_in_shadow.png`, `tw_current_aperture.png`, and `library.png` were visually inspected as review candidates.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - `Build Finished, Result: Success.`
  - Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - Log: `Logs\stage7u_sprite_card_cutout_build.log`
- Player smoke:
  - Built exe launched with `-batchmode -nographics`.
  - Stopped after 24 seconds.
  - Log: `Logs\stage7u_sprite_card_cutout_smoke.log`
  - No `Exception`, `NullReference`, `MissingReference`, `Assertion`, shader compile error, or C# compile error matches.

## Public Review

- Review directory: `docs/review/2026-05-27T00-33/`
- Public curation:
  - No external game target reference images.
  - No comparison board containing external references.
  - No route-close obstruction diagnostics.
  - Includes build exe path and "フォルダごと起動".

## Remaining Gaps

- Sprite cards now participate in cutout depth and shadow paths, but this is a technical rendering closure rather than visual approval.
- The plaza still reads as constructed geometry with broad mechanical shadow shapes and limited dense authored surface language.
- The library remains sparse in authored prop silhouettes, atmospheric layering, and warm/cool staging.
- The TimeWindow aperture remains visibly flat and technical rather than integrated into the scene lighting/material language.
- APV bake/probe-volume artifacts, richer VFX/atmospheric motion, and measured Stage 7 FPS evidence remain open next-cycle work.
- The target HD-2D quality remains substantially below the reference target.
