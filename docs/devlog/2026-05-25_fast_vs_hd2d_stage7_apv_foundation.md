# Fast VS HD-2D Stage 7d APV Foundation

Date: 2026-05-25
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

## Scope

- Enabled Adaptive Probe Volumes in the URP asset:
  - `Assets/Settings/UniversalRenderPipeline.asset`
  - `m_LightProbeSystem: 1`
  - medium APV texture budgets
  - L1 SH bands
  - APV GPU/disk streaming and lighting scenarios disabled for this bounded pass
- Added generated APV marker volumes to the Fast VS house-slice setup:
  - `FastVS_HD2D_Stage7_APV_CurrentLocalVolume`
  - `FastVS_HD2D_Stage7_APV_PastLocalVolume`
- Added validation/capture hooks:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7ApvFoundationBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7ApvReferenceScreenshotsBatch`

This is not an APV bake. No `ProbeVolumeBakingSet`, baked APV cells, LightingDataAsset bake, or baked GI output was created in this pass.

## Review Images

- Review directory: `docs/review/2026-05-25T20-25/`
- Capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7_apv`
- Comparison board: `docs/review/2026-05-25T20-25/stage7_apv_reference_comparison.png`

Hash comparison against Stage 7 VFX is not a clean APV visual delta because the capture includes the cumulative Stage 7c particle simulation, which is not deterministic frame-to-frame.

## Validation

- APV single validation: `Logs/stage7-apv-foundation-validate-single.log`, exit 0.
- Full house-slice validation: `Logs/stage7-apv-foundation-validate-full-gfx.log`, exit 0, `Fast VS house slice validation passed`.
- Capture: `Logs/stage7-apv-foundation-capture-gfx.log`, exit 0.
- Build: `Logs/stage7-apv-foundation-build-gfx.log`, exit 0.
- Smoke: `Logs/stage7-apv-foundation-smoke.log`, killed after 20s by the harness, target error match count 0.
- TimeWindow aperture PNG was read visually; it is not black.
- `PortalStencilFeature` remains active in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`.
- Paired-space serialized names and Stage 7 APV volume names were found in `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` before scene cleanup.
- `Assets/Scenes/Anemora_Chapter1.unity` is absent in this worktree; no Chapter1 APPLY/INTEGRATOR/REFRESH path was touched.

## Build

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Launch the whole `Builds\FastVS_HouseSlice` folder, not the exe copied alone.

## Gap Evaluation

- This pass does not produce baked GI. It only makes the project/scene APV-ready.
- The screenshots do not show a trustworthy APV lighting improvement.
- The current plaza still reads as hard, broad shadow shapes over sparse geometry.
- The library still lacks reference-level bounce-light shaping, material richness, and warm volumetric depth.
- The reference images rely on dense art direction, baked/local GI, volumetric shafts, asset density, and painterly material control; this pass does not solve those gaps.

## Tom Review Hook

- Stage 7d: APV foundation only; no APV bake or baked GI result.
- Build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe` (folder launch required).
- Tom capture request: 5 area screenshots to `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7_apv`.
- Status: 判定待ち.
