# 2026-05-25 Fast VS HD-2D Stage 1 Volume Profile

## Scope
- Apply the implementation-plan Stage 1 volume-profile values only.
- Keep runtime camera behavior on the shared VS-like follow profile restored in Phase A.
- Keep Stage 1 capture output in the external reference folder and selected review output in `docs/review/`.

## Changes
- `ColorAdjustments.saturation`: `-12` to `0`.
- `Bloom.intensity`: `0.30` to `0.80`.
- `Tonemapping.mode`: `Neutral` to `ACES`.
- `WhiteBalance.temperature`: `0` to `8`.

## Evidence
- Reference screenshots: `C:/Users/maro6/OneDrive/work/projects/anemora_reference/reference/20260525_stage1/`
- Side-by-side comparison: `C:/Users/maro6/OneDrive/work/projects/anemora_reference/reference/20260525_stage1/stage1_reference_comparison.png`
- Review screenshots: `docs/review/2026-05-25T15-50/`
- TimeWindow aperture review after Stage 1: `docs/review/2026-05-25T15-50/tw_current_aperture.png`
- Build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe` (`2026-05-25 15:54` local)

## Verification
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch` (`Logs/stage1-evidence-validate-gfx.log`)
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage1ReferenceScreenshotsBatch` (`Logs/stage1-evidence-capture-gfx.log`)
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch` (`Logs/stage1-evidence-build-gfx.log`)
- Built-player smoke: `Logs/stage1-evidence-smoke.log`, `MatchCount=0` for `Error|Exception|Assert|NullReference|Font Atlas Texture|DrawObjectsPass|RenderGraph`.
- Renderer feature check: `PortalStencilFeature` remains active (`m_Active: 1`) in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`.
- `CaptureHd2dStage1ReferenceScreenshotsBatch` now emits `tw_current_aperture.png` directly into the Stage 1 reference output and `docs/review/` selection; separate manual copy is no longer required.

## Gap Notes
- This stage is limited to global post-process values; it does not add emissive response, light cookies, real volumetrics, tilt-shift, outlines, APV GI, or VFX Graph.
- HD-2D quality remains far below the Octopath reference target after this isolated value change.
- Automated Stage 1 captures are not final art-signoff framing: `home.png` and `plaza_02_niro_in_shadow.png` crop Niro at the lower edge, and `Home_outside.png` is roof-dominant.
