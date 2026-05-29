# 2026-05-22 Fast VS HD2D Sprite Card Ramp Shader Cycle 08

## Scope

Foundation pass for deterministic sprite-card directional grading on Fast VS billboard characters and object sprites. This is a shader-and-validation slice, not final art polish.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

The current HD-2D result still reads too much like external shadow overlays. This cycle adds a custom transparent sprite-card ramp shader so Niro, Reto, Aria, and the other Fast VS billboard sprites can receive subtle UV-based warm-top-left / cool-lower-right grading directly from their own texture coordinates. Character strips keep the generated shaded sprite textures from the previous lighting cycle as their base; this shader is a second, controllable grading layer rather than a rollback to raw source sprites.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Shaders\FastVS\FastVS_SpriteCardRampUnlit.shader`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Shaders\FastVS\FastVS_SpriteCardRampUnlit.shader.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSpriteCardLightingAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Validation Performed

- `git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'`
- Unity batch validation via `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Log target:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_sprite_card_ramp_shader_cycle08_validate_worker_20260522.log`

## Result

- Pass. `Fast VS house slice validation passed.`
- The batch log shows the material role audit, sprite card lighting audit, and full house slice validation all completed successfully.
- Parent review changed the character strip texture path back to the generated shaded sprite textures before final validation, so Cycle 03 sprite-texture lighting remains part of the rendered result.

## Risks

- This is a foundation pass. The ramp colors and strength are intentionally conservative, but final visual tuning still depends on in-editor review and screenshots.
- The custom shader path assumes the sprite-card materials keep their current texture assignment pattern and transparent render-queue setup.

## Next Steps

- Review the updated Niro/Reto/Aria sprite cards under the scene lighting with the new ramp shader active.
- Tighten the ramp balance only if the result still reads as flat or over-darkened after the scene-level shadow layers are evaluated together.
