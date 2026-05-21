# 2026-05-22 Fast VS HD2D Surface Ramp Shader Cycle 09

## Scope

Foundation pass for deterministic opaque ramp shading on Fast VS generated floor, wall, furniture, bookshelf, stone, and other SurfaceLit environment materials. This is an environment material shader foundation, not final asset polish.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Intent

Cycle 08 improved sprite-card lighting, but the HD-2D read still risks feeling like a shadow overlay layered on top of unchanged environment materials. This cycle moves the grading into the generated opaque SurfaceLit materials themselves so floors, walls, furniture, shelves, books, stone, and similar meshes pick up a deterministic top-light / side-shade response from their mesh normals while keeping the procedural texture plates, material roles, and opaque queues intact. Parent review narrowed the shader application away from unlit glow/probe materials and added conservative main-light shadow receiving so the shader is a real environment lighting foundation rather than a flat replacement for URP/Lit.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Shaders\FastVS\FastVS_SurfaceRampLit.shader`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Shaders\FastVS\FastVS_SurfaceRampLit.shader.meta`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\` surface material assets for generated floor, wall, furniture, bookshelf, book, stone, path, fence, dust, rope, and sign materials
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-22_fast_vs_hd2d_surface_ramp_shader_cycle09.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

## Validation Performed

- `git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'`
- Unity batch validation via `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`
- Log target:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_ramp_shader_cycle09_validate_worker_20260522.log`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_surface_ramp_shader_cycle09_validate_parent_20260522.log`

## Result

- Pass. `Fast VS house slice validation passed.`
- The material role audit and the full house slice batch validation both completed successfully.
- The new surface-ramp assertions confirmed the key current/past interior floor, wall, exterior wall, roof, furniture, and book materials use `Anemora/FastVS/SurfaceRampLit` with opaque render settings.
- Parent review kept route glow/probe style unlit materials out of the surface-ramp shader scope, retained the bookshelf front texture panels as environment panels, and added `_DirectionalLightStrength` / `_ShadowReceiveStrength` validation for the key surface-ramp materials.

## Cleanup

- Removed Unity-generated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\AddressableAssetsData\Windows.meta`.
- No intentional scene, story, UI, font, dialogue, Time Window, control, map, or camera changes were made for this cycle.

## Risks

- The ramp values are intentionally conservative. Final visual quality still depends on in-editor review of the generated materials under the real scene lighting.
- This shader foundation covers the key SurfaceLit environment materials, but it is still a foundation pass rather than final polish for every material in the project.

## Next Steps

- Review the updated generated environment materials in-scene and tune only if the surface read still feels flat or too cool/warm in the real composition.
- Expand the same opaque ramp pattern only if additional SurfaceLit materials need the same treatment after visual signoff.
