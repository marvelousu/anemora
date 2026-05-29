# Stage7t: Plan Conformance Settings Closure

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Close the explicit HD-2D implementation-plan drift in the shared postprocess and URP shadow settings before moving to the remaining long-term Stage 7 items.

## Changes

- Updated `Assets/Settings/DefaultVolumeProfile.asset` to match the Stage 1 numeric grade from the HD-2D plan:
  - `ColorAdjustments.postExposure`: `0`
  - `Bloom.threshold`: `0.85`
  - `Bloom.highQualityFiltering`: `true`
  - `Vignette.intensity`: `0.30`
  - `Vignette.smoothness`: `0.40`
  - `FilmGrain.active`: `false`
- Updated `Assets/Settings/UniversalRenderPipeline.asset` to match the Stage 3 cascade direction:
  - `m_ShadowCascadeCount`: `4`
  - `m_Cascade4Split`: `(0.10, 0.30, 0.60)`
  - `m_ShadowNormalBias`: `1.5`
  - `m_ShadowDepthBias`: kept at `1`
- Updated the render asset setup and shading foundation audit so future re-apply and validation paths preserve those values.
- Updated existing HouseSlice volume validation gates in cycle 92 / cycle 115 / cycle 145 blocks to expect the plan-conformant grade.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit.VerifyShadingFoundationV1`
  - First run failed because `FilmGrain.active` was still enabled.
  - Second run passed after disabling FilmGrain.
  - Log: `Logs\stage7t_plan_conformance_shading_audit_r2.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
  - Exit 0.
  - `Shading Foundation v1 audit passed.`
  - `Fast VS house slice validation passed.`
  - Log: `Logs\stage7t_plan_conformance_validate_house.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7ApertureFrameBlendReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7s_aperture_frame_blend`
  - Log: `Logs\stage7t_plan_conformance_capture.log`
- Candidate public images were checked for central black obstruction:
  - `home.png`: center average luminance `0.228`, dark pixel ratio `0.00%`
  - `Home_outside.png`: center average luminance `0.215`, dark pixel ratio `0.91%`
  - `plaza_01.png`: center average luminance `0.286`, dark pixel ratio `2.78%`
  - `plaza_02_niro_in_shadow.png`: center average luminance `0.273`, dark pixel ratio `1.74%`
  - `library.png`: center average luminance `0.227`, dark pixel ratio `8.98%`
  - `tw_current_aperture.png`: center average luminance `0.418`, dark pixel ratio `0.00%`
- `tw_current_aperture.png`, `plaza_01.png`, and `library.png` were visually inspected; they are not route-close obstruction diagnostics.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - `Build Finished, Result: Success.`
  - Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - Log: `Logs\stage7t_plan_conformance_build.log`
- Player smoke:
  - Built exe launched with `-batchmode -nographics`.
  - Killed after 22 seconds.
  - Log: `Logs\stage7t_plan_conformance_smoke.log`
  - No `Exception`, `Error`, `Failed`, `NullReference`, `MissingReference`, or `Assertion` matches.

## Public Review

- Review directory: `docs/review/2026-05-26T23-56/`
- Public curation:
  - No external game target reference images.
  - No comparison board containing external references.
  - No route-close obstruction diagnostics.
  - Includes build exe path and "フォルダごと起動".

## Remaining Gaps

- The plan-conformant grade is now enforced, but the scene still reads as constructed geometry with large shadow cards in several views.
- The library remains sparse in authored prop density and warm/cool atmospheric layering.
- The TimeWindow aperture frame is visible and brighter after the grade shift, but still reads as a flat technical overlay rather than integrated HD-2D staging.
- The target HD-2D quality remains substantially below the reference target; this cycle is a settings closure checkpoint, not a final visual approval.

Next cycle candidates:
- Implement the missing `FastVS_SpriteCardRampLit.shader` Alpha Clip + ZWrite On variant from the long-term Stage 7 plan.
- Add or validate an APV bake/probe-volume scene artifact rather than only URP APV configuration.
- Continue toward VFX Graph or equivalent particle atmosphere if the package/runtime support is present.
