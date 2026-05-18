# 2026-05-18 Fast VS library story / scale / shadow cleanup

## Scope

- Target: `Anemora_FastVS_HouseSlice`
- Canon source: `2026-05-09_chapter1_scene1_v3_final.md` via the 2026-05-12 canon inventory.

## Changes

- Replaced the temporary/mojibake Reto script with the v3-final sequence:
  - [1.B] first Reto encounter.
  - [1.C] library history and empty-shelf inner thoughts.
  - [1.D] Timewriter activation and Time Window prompt.
  - [1.E] past-library observation after the player enters the Time Window.
  - [1.F] return to present with no book appearance.
  - [1.G] Mia hint.
  - VS clear only after [1.G].
- Removed the temporary past-library `Archivist Memory` paper character and its white name label.
- Added validation to reject the temporary past-library character/label if it reappears.
- Reduced library desk/table/book prop scale so it sits closer to Niro/Reto scale.
- Strengthened Niro's contact shadow without reintroducing the rejected dark rectangle overlay.

## Evidence

- Unity validation:
  - `Fast VS house slice validation passed.`
  - Log: `%TEMP%\anemora_fastvs_library_story_scale_validate2_20260518.log`
- Windows build:
  - `Build Finished, Result: Success.`
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
  - Log: `%TEMP%\anemora_fastvs_library_story_scale_build_20260518.log`
- Runtime smoke:
  - 18-second `-batchmode -nographics` launch produced no `error|exception|failed|crash|NullReference` hits.
  - Log: `%TEMP%\anemora_fastvs_library_story_scale_smoke_20260518.log`
- Screenshot updates:
  - `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/01_interior_niro_shadow.png`
  - `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/03_library_reto_desk.png`
  - `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/05_library_past_no_temp_people.png`

## Notes

- The v3-final route does not implement "past book handoff" or "book appears in the present"; those were removed from the VS path.
- `RenderTexture.Create failed` appears in `-nographics` validation because the V24 aperture cameras try to allocate review render textures in null graphics mode. Validation still completes and passes.
