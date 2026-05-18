# Fast VS HD-2D 80/20 Lighting and Volume Pass

Date: 2026-05-18

## Scope

Implemented the first HD-2D 80/20 slice from `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\HD2D_IMPLEMENTATION_PROPOSAL.md`, following the §3 rule that generated scene changes must be fixed in the integrator/generator path.

Implemented:

- Step 1(A): lower, cooler flat ambient and warm main directional light in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`.
- Step 1(D): URP soft shadows enabled and shadow distance reduced from 50 to 30 in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Settings\UniversalRenderPipeline.asset`.
- Step 3(A/B): generated a Fast VS-specific global post volume from the scene generator instead of editing the empty default profile. The persistent profile is `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Settings\FastVS_HD2D_80_20_VolumeProfile.asset`.
- Step 5(A): lit Fast VS environment materials are regenerated with `_Smoothness: 0.1`.
- Step 5(D): linear fog is generated with the scene.

Deferred:

- Step 1(C) SSAO. The gpt-5.4-mini worker confirmed URP 17.3.0 contains `ScreenSpaceAmbientOcclusion.cs` and this project can add it through `UniversalRenderPipeline_Renderer.asset`, but recommended deferring it for the first slice.
- Sprite lit shader / full pixelization / DOF. These remain next-pass polish candidates after visual review of this base.

## Worker Cycle

Used the requested plan -> gpt-5.4-mini worker -> review cycle.

- Worker: `019e3a6a-1f65-76a2-9008-09bfad02eb91` (`Parfit`)
- Assignment: read `HD2D_IMPLEMENTATION_PROPOSAL.md`, select the safe first 80/20 slice, identify validation commands and SSAO risk.
- Result: worker agreed with the smaller initial slice: generator-owned lighting/fog/volume, URP soft-shadow setting, lit-material smoothness, and SSAO deferred.

## Implementation Notes

The proposal text described `DefaultVolumeProfile.asset` as already populated, but the repo state had `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Settings\DefaultVolumeProfile.asset` empty. To avoid hand-editing the wrong global path, the Fast VS generator now creates `FastVS_HD2D_GlobalVolume` and assigns the dedicated `FastVS_HD2D_80_20_VolumeProfile.asset`.

The first generated volume profile attempt serialized null component references. This was fixed by adding Bloom, ColorAdjustments, and Vignette as real sub-assets of the profile. Grep now confirms the post components and values are persisted in the asset.

Unity build/capture generated unrelated Addressables, DefaultVolumeProfile, URP global settings, project settings, and old screenshot churn. Those were excluded from this implementation set.

## Verification

Scene generation passed:

```text
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_create_scene_20260518_hd2d_80_20_volume_fix.log
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
```

Build passed:

```text
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_build_player_20260518_hd2d_80_20_volume_fix.log
Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
```

Updated EXE:

```text
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
LastWriteTime: 2026-05-18 18:39:35
Length: 667648
```

Grep checks passed for:

- `m_Fog: 1`
- `m_AmbientSkyColor: {r: 0.14, g: 0.15, b: 0.18, a: 1}`
- `FastVS_HD2D_GlobalVolume`
- `m_RenderPostProcessing: 1`
- `m_ShadowDistance: 30`
- `m_SoftShadowsSupported: 1`
- `Bloom`, `ColorAdjustments`, `Vignette`, and their first-pass values in `FastVS_HD2D_80_20_VolumeProfile.asset`

Review screenshots were generated and copied for this pass:

```text
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_hd2d_80_20_20260518\01_interior_hd2d_80_20.png
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_hd2d_80_20_20260518\02_plaza_hd2d_80_20.png
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_hd2d_80_20_20260518\03_library_reto_hd2d_80_20.png
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_hd2d_80_20_20260518\04_library_pocket_glow_hd2d_80_20.png
```

Manual image review notes:

- Library and plaza now show stronger light direction, contact/cast shadows, darker recesses, and a more diorama-like contrast.
- The upper library becomes quite dark; this should be reviewed in playable context before pushing the next polish pass.
- This is a base lighting/post pass, not the final HD-2D look.

`ValidateHouseSliceBatch` was attempted and failed on the pre-existing font gate:

```text
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_validate_20260518_hd2d_80_20.log
House slice validation failed: Anemora_JP must be regenerated from DotGothic16, but familyName was 'MisakiGothic'.
```

The failure is outside this HD-2D pass and was already visible in earlier validation work.
