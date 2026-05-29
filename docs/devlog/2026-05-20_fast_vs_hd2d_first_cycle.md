# 2026-05-20 Fast VS HD2D First Cycle

## Summary

Implemented the first HD-2D polish cycle on the active HD-2D work branch:

- Branch: `work/fast-vs-hd2d-polish-20260520`
- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Public baseline preserved: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample` on `main` was not edited.

This cycle intentionally stayed inside global rendering and light-response setup. It does not change story flow, route contracts, Time Window behavior, HUD copy, transition logic, or event flags.

## Planning And Delegation

The user requested the continuing cycle of detailed parent planning, gpt-5.4-mini worker implementation, parent review, validation, and new planning. This pass followed that process:

- Planning agent: `019e4121-c4f6-75b2-99fb-cdff3c32d88e` (`Herschel`, gpt-5.5 xhigh) produced the Cycle 1 plan.
- Worker A: `019e4128-2625-7440-a932-7ec508bd50be` (`Hilbert`, gpt-5.4-mini) owned render assets only.
- Worker B: `019e4128-9c3c-71c2-9ced-34e62d0c2837` (`Nietzsche`, gpt-5.4-mini) owned `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs` only.
- Parent review added the guard that keeps Time Window / portal frame materials out of the matte material rewrite.

## Implemented Scope

Render asset setup:

- Added `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dRenderAssetSetup.cs`.
- Populated the previously empty `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\DefaultVolumeProfile.asset`.
- Enabled URP soft shadows on `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\UniversalRenderPipeline.asset`.
- Added low-cost SSAO to `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\UniversalRenderPipeline_Renderer.asset`.
- Kept Unity's regenerated URP runtime resources in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Settings\UniversalRenderPipelineGlobalSettings.asset` because the build and player smoke test need the SSAO renderer resources resolved.

Scene generation setup:

- Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`.
- Added post-processing/depth setup to the generated camera.
- Added `FastVS_HD2D_GlobalVolume` to the generated scene.
- Shifted the main directional light toward a warmer, directional HD-2D look.
- Rebalanced fog and ambient light.
- Converted non-portal Lit environment materials to a more matte, less glossy response.
- Slightly strengthened baked sprite directional shading.
- Added first-cycle screenshot and visual validation batch entry points.

Explicitly not implemented in this cycle:

- No DOF.
- No pixelization pass.
- No per-character 3D-lit sprite shader.
- No story, dialogue, route, save, or Time Window behavior changes.
- No return of the rejected `FastVS_PlayerSpriteShadingOverlay_Niro` black-rectangle style.

## Verification

Unity batch apply:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_apply_render_assets_20260520.log`
- Result: success.
- Key log line: `Fast VS HD2D render assets applied: shadowDistance=30, softShadows=on, rendererFeature=PortalStencilFeature+FastVS HD2D Soft Contact Occlusion, volumeProfile=Bloom/ColorAdjustments/Vignette/Tonemapping.`

Scene regeneration:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_create_scene_20260520_retry1.log`
- Result: success.
- Output scene: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`

Batch validation:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_validate_20260520.log`
- Result: success.
- Key log line: `Fast VS house slice validation passed.`
- Note: the log also contains Unity's licensing refresh message `Error: Access token is unavailable; failed to update`; it did not fail validation.

Screenshot batch:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_capture_20260520.log`
- Output directory: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_first_cycle_20260520`
- Captures:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_first_cycle_20260520\01_interior_niro_shadow.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_first_cycle_20260520\02_exterior_niro_shadow.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_first_cycle_20260520\03_library_reto_desk.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_first_cycle_20260520\04_library_reto_talk_loop.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_first_cycle_20260520\05_library_past_no_temp_people.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_first_cycle_20260520\06_library_dialogue_tmp_font.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_first_cycle_20260520\07_plaza_library_facade_current.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_first_cycle_20260520\08_plaza_library_facade_past.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_first_cycle_20260520\09_library_timewriter_pocket_glow.png`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_first_cycle_20260520\10_library_current_yellow_timewindow_cues.png`

Windows build:

- Command log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_build_20260520.log`
- Result: success.
- Build output: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Player smoke:

- Player log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_player_smoke_20260520.log`
- Result: launched and rendered for 20 seconds, then was intentionally stopped.
- Checked patterns: `Error`, `Exception`, `NullReference`, `MissingReference`, `Failed`, `Crash`, `Font Atlas Texture`, `LiberationSans`, `ScreenSpaceAmbientOcclusion`, `DrawObjectsPass`, `RenderGraph`.
- Match count: 0.

Diff hygiene:

- `git diff --check -- Assets/Editor docs` passed.
- Full `git diff --check` reports Unity-generated empty YAML fields such as `m_Name: ` inside `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity` and generated settings assets. These are generated Unity serialization lines and were not mechanically rewritten.

MCP boundary:

- `functions.list_mcp_resources` returned no live resources in this Codex session.
- Unity Editor live MCP inspection was therefore unavailable for this pass.
- Verification used Unity batch methods, generated scene assertions, screenshot capture, build, and player smoke instead.

## Review Notes

The reviewed screenshots show stronger perspective-readable light direction and contact shadowing without the earlier black rectangle overlay failure. The pass is still intentionally conservative: it improves the lighting/material foundation first, then leaves more opinionated pixelization and per-area grade work for the next cycle.

The known next-cycle candidates are:

- Add a controlled pixelization pass only if it preserves the selected dialogue font and UI readability.
- Split interior/exterior grade if the global grade makes the house or library too uniformly dark.
- Add per-character sprite-lighting only after validating it does not reintroduce rectangular overlays.
