# 2026-05-22 Fast VS HD2D Surface Directional Shade Cycle 07

Scope: Fast VS / HD2D vertical surface shade overlay foundation.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Goal

Cycle 06 established static directional cast shadows as thin horizontal grounding cues. This cycle adds the next layer the user asked for: a shared, vertical surface shade overlay that reads as face shading on walls, facades, and shelf backs instead of another floor shadow. The intent is to give the house, plaza library front, and library back wall a subtle HD-2D surface gradient without turning them into black rectangles.

## Implementation

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` now creates and validates a shared surface directional shade overlay asset pair:

- Material: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_surface_directional_shade_overlay.mat`
- Texture: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_surface_directional_shade_overlay_soft.png`

The new helper uses `CreateQuad`, keeps the quads vertical, disables real shadow casting and receiving, and assigns a transparent unlit contact-shadow material in the 2997-3002 render queue range. The PNG is a low-alpha blue-black/purple-black falloff with soft edge fade so it reads as surface shading rather than a floor shadow block.

Scene object placements added under the existing map roots:

- `Current_HouseExterior_SurfaceDirectionalShade_FacadeLeft`
- `Past_HouseExterior_SurfaceDirectionalShade_FacadeLeft`
- `Current_CentralPlaza_SurfaceDirectionalShade_LibraryFacade`
- `Past_CentralPlaza_SurfaceDirectionalShade_LibraryFacade`
- `Current_Library_SurfaceDirectionalShade_BackShelf`
- `Past_Library_SurfaceDirectionalShade_BackShelf`

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs` now includes `surface_directional_shade_overlay` in the contact-shadow role audit.

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md` was updated to `v6.78` with the new dated record and updated counts.

## Validation

Commands to run:

```powershell
git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
```

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_cycle07_validate_worker_20260522.log'
```

Results:

- Pass: `Fast VS house slice validation passed.`
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_directional_shade_cycle07_validate_worker_20260522.log`

## Cleanup

Unity side effects on the existing scene, project settings, addressables, and unrelated metadata were reverted after validation. The new surface directional shade material, PNG texture, and matching metadata remain as the intended generated outputs for this cycle.
