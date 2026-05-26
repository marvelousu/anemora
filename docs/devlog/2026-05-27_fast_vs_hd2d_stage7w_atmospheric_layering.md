# Stage7w: Atmospheric Layering Fallback

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Cycle goal:
- Add a richer atmospheric-behavior pass for plaza/library without adding `com.unity.visualeffectgraph`, which is still absent from `Packages/manifest.json`.

## Changes

- Added Stage7w ParticleSystem fallback layering to `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
  - Current library:
    - `FastVS_HD2D_Stage7_CurrentLibrary_ShelfDustRibbon`
    - `FastVS_HD2D_Stage7_CurrentLibrary_DeskAmberFloaters`
    - `FastVS_HD2D_Stage7_CurrentLibrary_ShelfDustVeil`
    - `FastVS_HD2D_Stage7_CurrentLibrary_TableFloatVeil`
  - Current plaza:
    - `FastVS_HD2D_Stage7_CurrentPlaza_FacadeDustRibbon`
    - `FastVS_HD2D_Stage7_CurrentPlaza_SunbreakFloaters`
    - `FastVS_HD2D_Stage7_CurrentPlaza_FacadeAirVeil`
    - `FastVS_HD2D_Stage7_CurrentPlaza_SunbreakAirVeil`
  - Past library/plaza:
    - `FastVS_HD2D_Stage7_PastLibrary_MemoryDustRibbon`
    - `FastVS_HD2D_Stage7_PastLibrary_MemoryVeil`
    - `FastVS_HD2D_Stage7_PastPlaza_AmberMemoryRibbon`
    - `FastVS_HD2D_Stage7_PastPlaza_AmberAirVeil`
- Kept particle counts low (`10` to `12`) and reused the existing transparent atmosphere material.
- Added deterministic review simulation for the new particle systems through `PrepareStage7VfxParticlesForReview(...)`.
- Added batch entry points:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7AtmosphericLayeringBatch`
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7AtmosphericLayeringReferenceScreenshotsBatch`
- Wired `ValidateHd2dStage7AtmosphericLayering()` into the full house-slice validation chain.

## Validation

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7AtmosphericLayeringBatch`
  - Exit 0.
  - Log: `Logs\stage7w_atmospheric_layering_validate.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7AtmosphericLayeringReferenceScreenshotsBatch`
  - Exit 0 with graphics enabled.
  - Internal capture output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage7w_atmospheric_layering`
  - Log: `Logs\stage7w_atmospheric_layering_capture.log`
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
  - `Fast VS house slice validation passed.`
  - `Build Finished, Result: Success.`
  - Build exe: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - Log: `Logs\stage7w_atmospheric_layering_build.log`
- Player smoke:
  - Built exe launched with `-batchmode -nographics`.
  - Stopped after 24 seconds.
  - Log: `Logs\stage7w_atmospheric_layering_smoke.log`
  - No `Exception`, `NullReference`, `MissingReference`, assertion, shader compile error, or C# compile error matches.

## Public Review

- Review directory: `docs/review/2026-05-27T01-42/`
- Public curation:
  - No external game target reference images.
  - No comparison board containing external references.
  - No route-close obstruction diagnostics.
  - Includes build exe path and "フォルダごと起動".
- Published image set:
  - `home.png`
  - `Home_outside.png`
  - `plaza_01.png`
  - `plaza_02_niro_in_shadow.png`
  - `library.png`
  - `tw_current_aperture.png`

## Remaining Gaps

- Stage7w increases visible mote/air-layer density in plaza/library, but it is still a ParticleSystem fallback rather than a true VFX Graph implementation.
- The plaza still reads as broad constructed geometry with mechanical shadow masses and limited painterly breakup.
- The library still needs denser authored prop silhouettes, stronger material response, and stronger warm/cool separation.
- The TimeWindow aperture remains readable, but still presents as a flat technical overlay in the public review capture.
- Measured Stage 7 FPS evidence and additional TimeWindow integration work remain open.
- The target HD-2D quality remains substantially below the reference target.
