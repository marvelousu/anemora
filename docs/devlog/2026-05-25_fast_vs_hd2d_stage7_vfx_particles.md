# Fast VS HD-2D Stage 7c VFX Particles

Date: 2026-05-25
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

## Scope

- Stage 7 VFX foundation for plaza/library only.
- `com.unity.visualeffectgraph` is not present in `Packages/manifest.json`; this pass uses the existing Unity `ParticleSystem` module instead of adding a package and lockfile churn.
- Added low-count Stage 7 particle systems:
  - `FastVS_HD2D_Stage7_CurrentLibrary_Fireflies`
  - `FastVS_HD2D_Stage7_CurrentPlaza_SunMotes`
  - `FastVS_HD2D_Stage7_PastLibrary_WarmFireflies`
  - `FastVS_HD2D_Stage7_PastPlaza_AmberMotes`
- Added small deterministic billboard mote cards for current plaza/library so batch screenshots and runtime rendering have visible VFX pixels.
- `FastVsRealtimeLightShadowRig` now exempts objects named `FastVS_HD2D_Stage7_*` from the old broad particle/OverlayGlow suppression path, while keeping shadows disabled.

## Review Images

- Review directory: `docs/review/2026-05-25T19-52/`
- Capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7_vfx`
- Comparison board: `docs/review/2026-05-25T19-52/stage7_vfx_reference_comparison.png`

Hash comparison against Stage 7 outline:

| Image | Diff vs Stage 7 outline |
| --- | --- |
| `home.png` | No |
| `Home_outside.png` | No |
| `plaza_01.png` | Yes |
| `plaza_02_niro_in_shadow.png` | Yes |
| `library.png` | Yes |
| `tw_current_aperture.png` | Yes |

## Validation

- Single Stage 7c validation: `Logs/stage7-vfx-particles-validate-single.log`, exit 0.
- Full house-slice validation: `Logs/stage7-vfx-particles-validate-full-gfx.log`, exit 0, `Fast VS house slice validation passed`.
- Capture: `Logs/stage7-vfx-particles-capture-gfx.log`, exit 0.
- Build: `Logs/stage7-vfx-particles-build-gfx.log`, exit 0.
- Smoke: `Logs/stage7-vfx-particles-smoke.log`, killed after 20s by the harness, target error match count 0.
- TimeWindow aperture PNG was read visually; it is not black.
- `PortalStencilFeature` remains active in `Assets/Settings/UniversalRenderPipeline_Renderer.asset`.
- Paired-space serialized names were found in `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`.
- `Assets/Scenes/Anemora_Chapter1.unity` is absent in this worktree; no Chapter1 APPLY/INTEGRATOR/REFRESH path was touched.

## Build

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Launch the whole `Builds\FastVS_HouseSlice` folder, not the exe copied alone.

## Gap Evaluation

- The VFX change is visible only as sparse motes in plaza/library; it does not approach Octopath-level atmospheric density.
- There is still no VFX Graph, no volumetric shafts, no fire/camp-night effect, and no art-directed area-specific particle behavior.
- The plaza still reads as broad hard-shadow slabs over simple geometry.
- The library gains small glints but remains materially flat compared with the reference night camp image.
- The effect is additive and narrow; it does not solve asset density, baked GI, depth hierarchy, or painterly material response.

## Tom Review Hook

- Stage 7c: ParticleSystem fallback plus visible plaza/library mote cards.
- Build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe` (folder launch required).
- Tom capture request: 5 area screenshots to `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7_vfx`.
- Status: 判定待ち.
