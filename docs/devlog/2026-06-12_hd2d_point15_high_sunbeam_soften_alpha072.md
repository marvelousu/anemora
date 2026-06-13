# 2026-06-12 HD2D point15 HighSunbeam soften alpha 0.72

## Scope

- Continue the point15 fog / haze line after the window aperture alpha fix.
- Address the remaining white haze in front of the library without deleting the sunlight cue entirely.
- Keep built-player evidence as the acceptance source.

## Code change

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Adjusted `Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_HighSunbeamColumnA` review metadata from `opacityBand=(0.03,0.20), intendedTint.a=0.18` to `opacityBand=(0.03,0.14), intendedTint.a=0.12`.
  - Confirmed this metadata is review-only and does not itself drive the rendered image.
  - Changed `EnsureHd2dPlazaReferenceDioramaAirCycle125Material()` tint from `Color.white` to `new Color(1f, 1f, 1f, 0.72f)`.

## Regenerated scene/material state

- Scene object:
  - `Current_CentralPlaza_Cycle125_ReferenceDioramaShadow_HighSunbeamColumnA`
  - `opacityBand: {x: 0.03, y: 0.14}`
  - `intendedTint: {r: 0.96, g: 0.9, b: 0.76, a: 0.12}`
- Material:
  - `Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_plaza_reference_diorama_air_cycle125.mat`
  - `m_CustomRenderQueue: 3124`
  - `RenderType: Transparent`
  - `_Surface: 1`
  - `_ZWrite: 0`
  - `_Cull: 0`
  - `_BaseColor: {r: 1, g: 1, b: 1, a: 0.72}`
  - `_Color: {r: 1, g: 1, b: 1, a: 0.72}`

## Build evidence

- Metadata-only probe build:
  - `Logs\point15_renderer_high_sunbeam_soften_build_validate_20260612T172718.log`
  - Exit: `0`
- Final alpha 0.72 build:
  - `Logs\point15_renderer_high_sunbeam_soften_alpha072_build_validate_20260612T173629.log`
  - Exit: `0`
- Final build key lines:
  - `DisplayProgressNotification: Build Successful`
  - `Build Finished, Result: Success.`
  - `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Built-player evidence

- Metadata-only player log:
  - `Logs\point15_renderer_high_sunbeam_soften_player_20260612T173320.log`
- Metadata-only review folder:
  - `docs\review\2026-06-12T17-33_renderer_high_sunbeam_soften`
- Final alpha 0.72 player log:
  - `Logs\point15_renderer_high_sunbeam_soften_alpha072_player_20260612T174246.log`
- Final alpha 0.72 review folder:
  - `docs\review\2026-06-12T17-42_renderer_high_sunbeam_soften_alpha072`
- Captures:
  - 50 PNGs
- Main comparison captures:
  - `02_baseline_current_library_facade_close.png`
  - `21_no_cycle125_reference_diorama_shadow_current_plaza_library_facade.png`
  - `22_no_cycle125_reference_diorama_shadow_current_library_facade_close.png`
  - `37_no_cycle125_high_sunbeam_column_current_plaza_library_facade.png`
  - `38_no_cycle125_high_sunbeam_column_current_library_facade_close.png`

## Renderer contract

- Final built-player contract line:
  - `ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>`

## Metadata-only result

- The first attempt changed only the overlay profile metadata.
- Built-player deltas did not move from the prior Cycle125 object split:
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=transparentCycle125Off view=wide baselineDelta meanAbsRgb=0.420 changedSamplePct=1.460 changed=841 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=transparentCycle125Off view=close baselineDelta meanAbsRgb=3.002 changedSamplePct=10.580 changed=6094 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=cycle125HighSunbeamColumnOff view=wide baselineDelta meanAbsRgb=0.240 changedSamplePct=0.887 changed=511 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=cycle125HighSunbeamColumnOff view=close baselineDelta meanAbsRgb=2.070 changedSamplePct=7.675 changed=4421 samples=57600`
- Conclusion:
  - `FastVsHd2dOverlayProfile` is review/audit metadata.
  - The rendered haze strength for this object is controlled by the shared air material/texture path, not by the review-only profile values.

## Final alpha 0.72 result

- Cycle125 aggregate transparent overlay contribution:
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=transparentCycle125Off view=wide baselineDelta meanAbsRgb=0.353 changedSamplePct=1.408 changed=811 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=transparentCycle125Off view=close baselineDelta meanAbsRgb=2.421 changedSamplePct=10.002 changed=5761 samples=57600`
- HighSunbeam contribution:
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=cycle125HighSunbeamColumnOff view=wide baselineDelta meanAbsRgb=0.173 changedSamplePct=0.835 changed=481 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=cycle125HighSunbeamColumnOff view=close baselineDelta meanAbsRgb=1.489 changedSamplePct=7.097 changed=4088 samples=57600`
- CenterChalk stayed unchanged:
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=cycle125CenterChalkSunCatchOff view=wide baselineDelta meanAbsRgb=0.152 changedSamplePct=0.342 changed=197 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=cycle125CenterChalkSunCatchOff view=close baselineDelta meanAbsRgb=0.756 changedSamplePct=1.674 changed=964 samples=57600`
- BackDepthHaze remained disabled/no-op in built-player:
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=cycle125BackDepthHazeOff view=wide baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - `ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=cycle125BackDepthHazeOff view=close baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- Previously suspicious Cycle125 pale wash names remained inactive/no-op:
  - `cycle125ReferenceReceiverLiftOff view=close baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - `cycle125StoneSunMatteFieldOff view=close baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - `cycle125CloseSeamSunMuteOff view=close baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - `cycle125BackStepPaleSunOff view=close baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
  - `cycle125FacadeReferenceSunPatchOff view=close baselineDelta meanAbsRgb=0.000 changedSamplePct=0.000 changed=0 samples=57600`
- Final capture count:
  - 50 PNGs

## Delta summary

- `cycle125HighSunbeamColumnOff` close meanAbsRgb:
  - before: `2.070`
  - final: `1.489`
- `transparentCycle125Off` close meanAbsRgb:
  - before: `3.002`
  - final: `2.421`
- The library-front white veil is reduced while the floor sunbeam remains visible.

## Interpretation

- The remaining library-front white haze was primarily the active Cycle125 `HighSunbeamColumnA`, not the already-disabled `BackDepthHazeA`.
- The safe next step was not deletion: deleting `HighSunbeamColumnA` removes the intended light cue.
- Alpha 0.72 reduces the visible haze contribution while preserving the readable sunbeam.
- Do not use profile metadata-only tweaks as visual fixes unless a runtime path is added to consume them.

## Next slice

- Upload this review cycle to anemora-viewer for visual judgment.
- If Tom wants the haze still lower, test a separate `air material alpha=0.60` slice with the same built-player isolation probe.
- Continue the broader flicker / object pop diagnostic line after viewer propagation.
