# 2026-05-22 Fast VS HD2D Shading Foundation Cycle 01

Scope: Fast VS / HD2D shading foundation.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Goal

The user feedback after the previous visual cycles was that the map had improved in asset count and local details, but still did not read as a product-quality HD-2D image. The specific failure mode was that the image often looked like brighter/darker flat assets rather than a coherent lit scene.

This cycle therefore stops adding more object dressing and establishes a reusable shading foundation:

- URP settings that can support soft contact shadows, depth/opaque texture dependent effects, and additional lights.
- A renderer feature audit that keeps `PortalStencilFeature` and SSAO order stable.
- Area-aware lighting profiles for house interior, house exterior, central plaza, and library.
- Verification hooks that fail if later scene-generation work removes the lighting director or weakens the render baseline.

## Worker Cycle

The implementation followed the current HD2D workflow: parent session planned the scope and a `gpt-5.4-mini` worker implemented the render asset foundation first.

Worker output:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dRenderAssetSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dShadingFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\UniversalRenderPipeline.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\UniversalRenderPipeline_Renderer.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`

Parent integration then added the area-aware runtime lighting director and scene-generation wiring:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scripts\FastVS\FastVsHouseAreaVisibility.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`

## Render Foundation

`AnemoraFastVsHd2dRenderAssetSetup.ApplyShadingFoundationV1()` now applies the baseline render setup:

- Main light shadow map: `4096`
- Shadow distance: `35`
- Shadow cascades: `2`, split near `0.35`
- Additional lights: enabled, up to `4` per object
- Additional light shadows: enabled for future use
- Depth texture: enabled
- Opaque texture: enabled
- Soft shadow quality: high
- SSAO: one feature, ordered after `PortalStencilFeature`
- SSAO source/method: DepthNormals + BlueNoise
- SSAO intensity/radius: product-safe contact bias, not a heavy global darkener
- Volume profile: Neutral tonemapping, mild contrast/saturation, very low bloom/vignette, DepthOfField and FilmGrain disabled

The separate audit tool is:

- `Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit.VerifyShadingFoundationV1`

It is intended to catch future regressions where a renderer change silently removes SSAO, reorders the portal stencil feature, disables depth textures, or re-enables broad blur/grain.

## Area Lighting

`FastVsHouseLightingDirector` now applies different lighting profiles by active VS area.

Required scene lights:

- `Directional Light`
- `FastVS_HD2D_WarmFillLight`
- `FastVS_HD2D_CoolRimLight`
- `FastVS_HD2D_LibraryWindowLight`

Lighting intent:

- House interior: warm, no fog, lower background value, readable character contact.
- House exterior: warm sun with cool rim, outdoor fog active.
- Central plaza: slightly stronger sun contrast and longer fog depth.
- Library: darker warm interior, lower ambient, enabled spot-like window light.

`FastVsHouseAreaVisibility.ApplyVisibility()` now asks the lighting director to apply the profile immediately after map visibility changes. This prevents camera/map transitions from keeping the previous area's lighting until the next runtime poll.

## Validation

Commands run:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit.VerifyShadingFoundationV1 -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shading_foundation_cycle01_audit_final_20260522.log'
```

Result:

- Pass: `Shading Foundation v1 audit passed.`
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shading_foundation_cycle01_audit_final_20260522.log`

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shading_foundation_cycle01_validate_final_20260522.log'
```

Result:

- Pass: `Fast VS house slice validation passed.`
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shading_foundation_cycle01_validate_final_20260522.log`

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dShadingFoundationCycle01ScreenshotsBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shading_foundation_cycle01_capture_20260522.log'
```

Result:

- Pass: 4 review screenshots produced.
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shading_foundation_cycle01_capture_20260522.log`
- Output directory: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shading_foundation_cycle01_20260522`

Screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shading_foundation_cycle01_20260522\01_current_house_exterior_lighting_balance.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shading_foundation_cycle01_20260522\02_current_central_plaza_lighting_balance.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shading_foundation_cycle01_20260522\03_current_library_lighting_balance.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shading_foundation_cycle01_20260522\04_past_central_plaza_lighting_balance.png`

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shading_foundation_cycle01_build_20260522.log'
```

Result:

- Pass: `Fast VS house slice validation passed.`
- Pass: `Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shading_foundation_cycle01_build_20260522.log`

Player smoke:

```powershell
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe -batchmode -nographics -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shading_foundation_cycle01_player_smoke_20260522.log
```

Result:

- Ran for 30 seconds and was stopped intentionally.
- Checked patterns: `Error`, `Exception`, `Assert`, `NullReference`, `MissingReference`, `Failed`, `DrawObjectsPass`, `RecordRenderGraph`, `RenderGraph`, `ScreenSpaceAmbientOcclusion`.
- Matches: 0.
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_shading_foundation_cycle01_player_smoke_20260522.log`

## Notes

- No paid asset purchase or new API token was needed for this cycle.
- The visible quality gain from this cycle is intentionally foundational rather than asset-heavy. It should make subsequent material/sprite/shader work more predictable because the render path, SSAO, post-processing, and area lighting are now explicit and audited.
- Unity batch operations repeatedly touched `Assets/AddressableAssetsData/link.xml`, some generated material YAML, and ProjectSettings side-effect files. Those are not part of this cycle's authored scope and were restored before commit.

## Next Work

Next cycles should build on this base rather than adding unrelated props:

- Shader/material role pass: classify sprite cards, floor/wall volumes, props, and emissive guide markers into lit/unlit/overlay material families.
- Sprite-card self-shading pass: add a controlled character/prop shading overlay that does not become a flat dark rectangle.
- Area-specific screenshot QA: add pixel/statistical checks for over-darkening, blank sky/background, and excessive bloom/SSAO.
- Library and plaza material relighting: tune surfaces against the new lighting profiles instead of manually darkening textures.
