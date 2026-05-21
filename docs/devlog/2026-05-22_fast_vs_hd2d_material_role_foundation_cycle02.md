# 2026-05-22 Fast VS HD2D Material Role Foundation Cycle 02

Scope: Fast VS / HD2D material-role metadata, transparent queue hardening, and validation.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Goal

Cycle 02 adds a machine-readable material role contract on top of the Cycle 01 lighting foundation. The intent is to make later HD-2D shader and asset work less ad hoc: surfaces, sprite cards, paper cards, glow overlays, contact shadows, and portal/window materials now have explicit roles and an audit.

No new shader was added in this cycle. Geometry and scene layout were not intentionally changed.

## Parent Review Corrections

The first worker pass overreached in two places:

- it tried to rename generated material object names with role tokens
- it made broad `shadow` / `doorway_dark` / window-panel transparency changes

The parent pass kept the useful role foundation but corrected those risks:

- material object names remain stable as `FastVS_House_{id}` so existing validators and scene references keep their expected names
- the role source of truth is `SetOverrideTag("AnemoraFastVsHd2dRole", role)`
- `shadow` remains an opaque dark material even though its role is `ContactShadow`
- `doorway_dark` remains an opaque surface material
- `window_light` and `empty_window` remain opaque/unlit panel materials, while true light overlays keep transparent queues

## Material Role Foundation

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` now defines the local role enum:

- `SurfaceLit`
- `SpriteCard`
- `PaperCard`
- `OverlayGlow`
- `ContactShadow`
- `PortalWindow`

The setup now applies `AnemoraFastVsHd2dRole` tags through the material factories and specialized helpers:

- `FlatMaterial`, `PixelMaterial`, and `PaintedSurfaceMaterial`
- `SpriteStripMaterial` and `ExternalSpriteMaterial`
- character contact shadows and depth shadows
- warm light pools, guide glows, atmosphere particles, and pocket glow
- portal aperture, window panel, and time-window frame materials

The generated `.mat` assets under `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\` were updated intentionally so the role metadata is persisted, not only regenerated during validation.

## Transparent Queue Hardening

Transparent overlay setup now also sets:

- `RenderType = Transparent`
- `DepthOnly` and `SHADOWCASTER` disabled
- `_QueueOffset = renderQueue - 3000`

The `_QueueOffset` addition is required because URP reported some custom transparent materials, especially `FastVS_House_hd2d_warm_light_pool.mat`, as queue `3000` unless the queue offset matched the custom queue. The existing validator expects warm pools in the `3005-3015` range, and this now passes.

Opaque setup now resets:

- `RenderType = Opaque`
- `_QueueOffset = 0`
- transparent keyword disabled
- ZWrite enabled

## Audit

New validator:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs`

It is called from:

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch()`

The audit checks representative materials across:

- Niro directional and walk sprite cards
- Reto writing / lower arms / talk / raise arms sprite cards
- Aria idle sprite card
- external tree and hedge sprite cards
- surface materials for floors, walls, paths, stones, furniture, beds, books, and props
- paper-card materials
- contact shadow overlays
- portal/window/frame materials
- glow overlays and atmosphere materials

## Validation

Command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_material_role_foundation_cycle02_validate_parent_retry3_20260522.log'
```

Result:

- Pass: `HD2D material role audit passed.`
- Pass: `Fast VS house slice validation passed.`
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_material_role_foundation_cycle02_validate_parent_retry3_20260522.log`

Expected non-blocking log noise:

- Unity licensing module reported an unavailable access token while batch validation still completed successfully.

## Cleanup

After validation, Unity side effects unrelated to this cycle were restored or removed:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings\GraphicsSettings.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\ProjectSettings\QualitySettings.asset`
- Addressables-generated link and Windows side files
- external tree texture importer side effect

## Next

Cycle 03 should start from this role contract and add a shader-facing or asset-facing improvement in one role family at a time. The next practical target is sprite-card lighting response, because it can improve Niro/Reto/Aria readability without touching map topology.
