# 2026-05-22 Fast VS HD2D Static Directional Shadow Cycle 06

Scope: Fast VS / HD2D static directional cast shadow foundation.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Goal

Cycle 05 added the character directional cast shadow layer. This cycle extends the same lighting idea to static, tall objects so later polish work can reuse one thin directional shadow foundation instead of dropping in one-off dark blocks for each building or bookshelf.

## Implementation

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` now creates and validates a shared static directional cast shadow asset pair:

- Material: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_static_directional_cast_shadow.mat`
- Texture: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_static_directional_cast_shadow_soft.png`

The new helper keeps the shadow in the transparent unlit contact-shadow role, disables DepthOnly and SHADOWCASTER, and uses a low render queue in the 2992-2998 range. The generated PNG is a soft, thin, horizontally stretched falloff rather than a black square.

Scene object placements added under the existing map roots:

- `Current_HouseExterior_StaticDirectionalCastShadow_HouseFacade`
- `Past_HouseExterior_StaticDirectionalCastShadow_HouseFacade`
- `Current_CentralPlaza_StaticDirectionalCastShadow_LibraryFacade`
- `Past_CentralPlaza_StaticDirectionalCastShadow_LibraryFacade`
- `Current_Library_StaticDirectionalCastShadow_BackShelf`
- `Past_Library_StaticDirectionalCastShadow_BackShelf`

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs` now includes `static_directional_cast_shadow` in the contact-shadow role audit.

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md` was updated to `v6.77` with the new dated record and updated counts.

## Validation

Commands run:

```powershell
git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
```

Result:

- Pass

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_cycle06_validate_worker_20260522.log'
```

Result:

- Pass: `Fast VS house slice validation passed.`
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_static_directional_shadow_cycle06_validate_worker_20260522.log`

## Cleanup

Unity side effects on the existing scene, project settings, addressables, and tree sprite metadata were reverted after validation. The new static directional shadow material, PNG texture, and matching metadata remain as the intended outputs for this cycle.
