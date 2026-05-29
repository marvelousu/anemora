# Fast VS HD2D Postprocess Grade Cycle 25 Report

Foundation discovery report for the already-applied URP, renderer, and volume HD-2D shading setup. This cycle does not introduce new grade target values; it records the current contract and confirms DepthOfField and FilmGrain are disabled for the Fast VS baseline.

- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Report file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_postprocess_grade_cycle25_20260522\postprocess_grade_cycle25_20260522.md`
- Pipeline asset: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\UniversalRenderPipeline.asset`
- Renderer asset: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\UniversalRenderPipeline_Renderer.asset`
- Volume profile: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`

## Pipeline

| Field | Value |
|---|---|
| Shadow distance | 35 |
| Main light shadows supported | true |
| Main shadowmap resolution | 4096 |
| Additional lights mode | 1 |
| Additional lights per object | 4 |
| Additional light shadows | true |
| Depth texture | true |
| Opaque texture | true |
| Soft shadows | true |
| Soft shadow quality | 3 |
| Cascade count | 2 |
| Cascade split | 0.35 |
| HDR | true |

## Renderer

| Feature | Order | Active | Asset Name |
|---|---:|---|---|
| PortalStencilFeature | 0 | true | PortalStencilFeature |
| ScreenSpaceAmbientOcclusion | 1 | true | FastVS HD2D Soft Contact Occlusion |

### ScreenSpaceAmbientOcclusion Settings

| Field | Value |
|---|---|
| AOMethod | BlueNoise |
| Source | DepthNormals |
| Samples | High |
| BlurQuality | High |
| Intensity | 0.58 |
| Radius | 0.085 |
| DirectLightingStrength | 0.08 |
| Falloff | 80 |

## Volume

| Field | Value |
|---|---|
| Bloom active | true |
| Bloom threshold | 0.8 |
| Bloom intensity | 0.07 |
| Bloom scatter | 0.45 |
| ColorAdjustments active | true |
| ColorAdjustments postExposure | 0 |
| ColorAdjustments contrast | 8 |
| ColorAdjustments saturation | 2 |
| Vignette active | true |
| Vignette intensity | 0.04 |
| Tonemapping active | true |
| Tonemapping mode | Neutral |
| ShadowsMidtonesHighlights active | true |
| ShadowsMidtonesHighlights shadows | (0.985, 0.995, 1.015, 0) |
| ShadowsMidtonesHighlights midtones | (1, 1, 1, 0) |
| ShadowsMidtonesHighlights highlights | (1.012, 1.006, 0.992, 0) |
| ShadowsMidtonesHighlights shadowsStart | 0 |
| ShadowsMidtonesHighlights shadowsEnd | 0.3 |
| ShadowsMidtonesHighlights highlightsStart | 0.58 |
| ShadowsMidtonesHighlights highlightsEnd | 1 |
| LiftGammaGain active | true |
| LiftGammaGain lift | (1, 1, 1, 0) |
| LiftGammaGain gamma | (1, 1, 1, 0) |
| LiftGammaGain gain | (1, 1, 1, 0) |
| DepthOfField active | false |
| FilmGrain active | false |
