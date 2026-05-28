# HD-2D Phase A / B-alpha / B-beta / C-alpha / C-beta Completion

Date: 2026-05-28

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Build exe path: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`

Launch note: start the build from the full `Builds/FastVS_HouseSlice/` folder.

## Phase Review Sets

- Phase A gate review: `docs/review/2026-05-28T15-20/`
- Phase B-alpha lens flare review: `docs/review/2026-05-28T17-17/`
- Phase B-beta Buto adoption review: `docs/review/2026-05-28T18-07/`
- Phase C-alpha emissive VFX review: `docs/review/2026-05-28T18-30/`
- Phase C-beta Artistic Tilt Shift adoption review: `docs/review/2026-05-28T18-50/`

Each public review set contains only current project captures and review text. External reference images and comparison boards were not placed in `docs/review`.

## Non-public Review Artifacts

- TimeWindow phase comparison: `docs/devlog/screenshots/fast_vs_hd2d_phase_full_completion_20260528_01/timewindow_phase_comparison.png`
- Artifact manifest: `docs/devlog/screenshots/fast_vs_hd2d_phase_full_completion_20260528_01/completion_artifact_manifest.txt`
- Local reference comparison boards for Tom review, outside `docs/review`: `OneDrive/work/projects/anemora_reference/hd2d_phase_full_completion_20260528/`

## Commit List

- `c0a0b116` docs(hd2d): record sun cycle area decision
- `3159fb3f` feat(hd2d): add phase a sun cycle runtime api
- `8b56da83` feat(hd2d): wire phase a sun cycle into house slice scene
- `97f7e4a9` docs(hd2d): publish phase a sun cycle scene review
- `8d6e6cf7` feat(hd2d): hand off director sun control
- `624a5dd1` feat(hd2d): hand off realtime rig sun control
- `ff850a77` feat(hd2d): remove realtime rig painted overlay
- `5cacf372` feat(hd2d): make realtime rig shadow policy event driven
- `a6db7f4e` feat(hd2d): lighten surface ramp shadow path
- `7f82b40e` docs(hd2d): publish phase a five-area review
- `bc4fcf03` docs(hd2d): add phase a gate audit evidence
- `05795035` docs(hd2d): publish phase a audit review
- `5cdcddac` docs(hd2d): package phase a gate submission
- `63af5d58` docs(hd2d): clarify phase a generated scene evidence
- `c91acf75` feat(hd2d): add phase b alpha sun runtime controls
- `667ace12` docs(hd2d): publish phase b alpha runtime review
- `4525b7ee` feat(hd2d): add phase b alpha scene lens flare setup
- `2d88df13` docs(hd2d): publish phase b alpha lens flare review
- `af59796f` feat(hd2d): add phase b beta buto adoption fallback
- `5abd7540` docs(hd2d): publish phase b beta buto adoption review
- `ae5dd1bc` feat(hd2d): add phase c alpha emissive vfx fallback
- `e3a119ac` docs(hd2d): publish phase c alpha emissive vfx review
- `857e59d8` feat(hd2d): add phase c beta artistic tilt shift adoption fallback
- `11d6a44d` docs(hd2d): publish phase c beta artistic tilt shift review

## Acceptance Evidence and Gaps

Phase A:
- Sun cycle runtime API, scene wiring, `MapSunAnchor`, main directional handoff, painted overlay removal, event-driven renderer shadow policy, and shader lightening were implemented and batch-validated.
- Serialized scene evidence after regeneration includes `AnemoraSunCycleDriver`, `MapSunAnchor`, `LensFlareComponentSRP`, and Phase C-alpha emitter/light names.
- Grep evidence: `Cycle128|Cycle131|Painted` in `FastVsRealtimeLightShadowRig.cs` returns 0 matches.
- `FastVS_SurfaceRampLit.shader` uses `Cull Back`; local grep for `Cull Off` / custom PCF tokens returns 0 matches.
- Tom visual judgement is still required.

Phase B-alpha:
- Sun preset runtime controls now drive sky sun size, lens flare intensity, and directional light volumetric scattering where the local API exposes it.
- Directional Sun lens flare and ScreenSpaceLensFlare volume settings were added and batch-validated.
- Local URP 17 package scan did not expose the requested `VolumetricFog` override/property path; the report records `URP serialized volumetric fog property exposed locally: False`.
- APV density/rebake was not completed in this cycle set, so APV dark-area color improvement is not claimed.

Phase B-beta:
- Buto was not detected in this workspace: no asset path matches, no loaded assembly/type matches, and no Buto-specific renderer/volume candidate.
- B-alpha fallback remains active.
- True B-alpha vs B-beta Buto quality comparison could not be produced until the package is imported.
- Adoption decision devlog: `docs/devlog/2026-05-28_fast_vs_hd2d_buto_adoption_decision.md`

Phase C-alpha:
- Window/library emission values, warm point lights, and four low-count ParticleSystem fallback emitters were added and batch-validated.
- VFX Graph package was not present, so `.vfx` assets were not created; fallback state is recorded in diagnostics.
- Bloom/warm-cool contrast evidence is available for Tom PNG review, but no final visual acceptance is claimed.

Phase C-beta:
- Artistic: Tilt Shift was not detected in this workspace: no manifest/package-lock matches, no asset path matches, no loaded assembly/type matches, and no Artistic renderer/volume hints.
- Existing `FastVS HD2D Stage7 TiltShift` Full Screen Pass renderer feature remains active.
- True C-alpha vs C-beta Artistic comparison could not be produced until the package is imported.
- Adoption decision devlog: `docs/devlog/2026-05-28_fast_vs_hd2d_artistic_tiltshift_adoption_decision.md`

## Validation Summary

- Cycle 175 B-alpha scene lens flare: validate/capture/build/smoke passed.
- Cycle 176 B-beta Buto fallback: validate/capture/build/smoke passed after disk-space recovery; smoke pattern count 0.
- Cycle 177 C-alpha emissive VFX fallback: validate/capture/build/smoke passed; smoke pattern count 0.
- Cycle 178 C-beta Artistic fallback: validate/capture/build/smoke passed; smoke pattern count 0.
- Built exe smoke is `-batchmode -nographics`; it does not replace real play input/dynamic checks or formal profiler screenshots.

## Residual Work

- Import Buto and Artistic: Tilt Shift in this workspace, then rerun B-beta/C-beta true comparison cycles.
- Capture formal Profiler screenshots for Migration-before / Phase A / B-alpha / B-beta / C-alpha / C-beta GPU and CPU timings. The current evidence set has batch smoke logs and review captures, not those profiler screenshots.
- Perform real play diagnostics for dynamic/input behavior if Tom needs runtime transition evidence beyond batch validation.
- Revisit APV density/rebake after package/import state is stable.

## Tom Gate Notes

- Changes applied: Phase A through C-beta implementation and fallback diagnostics were applied, review sets were pushed per cycle, and local non-public comparison boards were generated outside `docs/review`.
- Gap to reference images: the target HD-2D quality remains substantially below the reference target, especially because Buto, Artistic: Tilt Shift, VFX Graph `.vfx`, APV rebake, and formal profiler evidence are not present in the current workspace.
- Tom decision requested: review the five Phase review sets, decide whether to import Buto/Artistic for true comparison cycles, and judge whether the current fallback state is sufficient to proceed temporarily.
