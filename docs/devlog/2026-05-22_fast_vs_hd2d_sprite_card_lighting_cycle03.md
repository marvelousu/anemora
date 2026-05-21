# 2026-05-22 Fast VS HD2D Sprite Card Lighting Cycle 03

## Scope

Implemented a deterministic sprite-card lighting pass for the Fast VS house slice pipeline. The change keeps alpha handling, point filtering, and source texture paths intact, while giving shaded character sprite outputs a more directional HD-2D style ramp for Niro, Reto, and Aria.

## Code Changes

- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - Added a strip-aware `ShadeSpritePixel` ramp with warm top-left key light, lower-body/contact darkening, cool right/lower shadowing, rim highlight, and safety luminance clamping that preserves the directional lighting change.
  - Added frame-width detection for 64x96 sprite strips so Reto and Aria strips shade per frame without assuming Niro-only layout.
  - Fixed the sprite-strip material generation path to call the same transparent unlit configuration used by other sprite cards, preventing future batch regeneration from reverting sprite materials to opaque RenderType/pass settings.
  - Added `AnemoraFastVsHd2dSpriteCardLightingAudit.VerifySpriteCardLightingV1()` to the batch validation flow after material-role validation.
- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs`
  - Extended transparent role checks to assert `RenderType=Transparent` and disabled `DepthOnly`/`SHADOWCASTER` passes, so generator-side regressions are caught by batch validation.
- Added `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSpriteCardLightingAudit.cs`
  - Validates shaded textures for `niro_front_sprite`, `niro_walk_front_sprite`, `reto_v02_writing_loop_sprite`, and `aria_v46_normal_loop_breath_sprite`.
  - Checks source dimension consistency, visibility, alpha preservation on transparent corners, luminance spread, source-relative luminance bounds, and a gentle upper-left to lower-right directional bias.
- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`
  - Added this cycle entry to the 2026-05-22 section.

## Validation

Parent validation command:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_lighting_cycle03_validate_parent_retry3_20260522.log'
```

Result:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_lighting_cycle03_validate_parent_retry3_20260522.log`
  - `HD2D material role audit passed.`
  - `HD2D sprite card lighting audit passed.`
  - `Fast VS house slice validation passed.`

## Notes

No gameplay topology or scene layout changes were made. The intended output change is deterministic regeneration of the shaded sprite texture assets under `Assets/Art/Textures/FastVS/HouseSlice/`.
