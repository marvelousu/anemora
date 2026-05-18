# 2026-05-09 Stage 4 Chapter 1 Map DQ3R Full Atmospheric Pipeline

## Goal

After pass5 atmosphere/variant/camera/promotion landed (separate devlog `2026-05-09_stage4_chapter1_map_dq3r_atmospheric_progress.md`), continue pushing the captures toward DQ3R-class HD-2D quality. This devlog records the post-pass5 DQ3R level-up work: atlas variant tinting at two levels, per-beat semantic character placement, per-beat post-process tuning, atmospheric particle systems (rain / dust / ember), 4K and 8K cinematic captures.

## What landed

### A. Atlas variant per-renderer tint
- `ApplyAtmosphereVariantRendererTint(root, variantId)` adds `MaterialPropertyBlock._BaseColor` per-renderer multiply on top of the existing ambient `Color.Lerp` tint.
- Strength clamped 0-0.30; Character billboards excluded (sprite material name prefix check).
- Atmosphere matrix captures now visibly differ between Day_A / Dawn_A / Dusk_A / Night_A / Overcast_A / Wet_A / GoldenHour_A / Foggy_A intents.
- DQ3R PP / 4K / 8K hero use TOD-mapped variant id via `AtlasVariantIdForTimeOfDay(tod)`.

### B. Per-beat semantic character roster
- 8-character v3 64x96 single-frame stand-sprite cluster: Hero / Niro, Resident_A / Aria, Resident_F / Mia, Resident_D / Dario, Resident_C / Kaia, Resident_L / Luna, Resident_J / Karla, Resident_K / Kairo.
- `BeatCharacterRosters` dict overrides the default cluster per beat:
  - S1 Library: Hero + Resident_B (seated reading sliced 4f, frame 0 via UV scale)
  - S2 Mia House: Mia + Hero + Luna
  - S3 Current Street: Hero + Aria + Karla + Kairo
  - S3 Past Market: Aria + Dario + Mia (younger) + Luna (younger)
  - S4 Current Field: Kaia + Hero
  - S4 Past Field: Dario + Hero + Kaia (younger)
  - S5 North Ruins: Hero + Luna + Karla + Kairo (group)
  - Time Window adjacent: Hero alone
- Billboards are 1m × 1.5m alpha-clipped quads at 43° pitch, URP/Unlit shader, individual transient materials with `_BaseMap` UV scaled to first frame for sliced sheets.

### C. Per-beat post-process tuning
- `ResolveBeatPostProcessTuning(beatKey)` returns `(bloomIntensity, bloomThreshold, vignetteIntensity, vignetteWarmth, contrast, saturation)` per beat category:
  - **Interior (S1 / S2)**: Bloom 0.50, Vignette 0.34, warm vignette tilt, contrast 9, saturation 10
  - **Foggy (S5 / North)**: Bloom 0.40 (reduced — fog already lifts blacks), Vignette 0.36 dark, contrast 14, saturation 6
  - **Time Window**: Bloom 0.75, Vignette 0.40 cool ethereal, contrast 8, saturation 9
  - **Outdoor default (S3 / S4)**: Bloom 0.65, Vignette 0.28, neutral, contrast 10, saturation 8
- Combined with TOD-specific `colorFilter` (Dawn warm tilt / Dusk magenta-warm / Night cool tilt / Day balanced) and `postExposure` adjustment.

### D. Atmospheric particle systems
- `CreateRainParticles(parent, bounds, intensity)` — 9 m/s downward stretched billboards, cool blue tint, 240 emission rate × intensity, simulated forward 2.5 sec.
- `CreateDustDriftParticles(parent, bounds, intensity)` — slow horizontal drift, warm brown tint, 16 emission rate × intensity.
- `CreateEmberParticles(parent, bounds, intensity)` — upward floating warm-orange, additive blend, color-over-lifetime fade, 12 emission rate × intensity.
- All use URP `Particles/Unlit` shader, `simulationSpace = World`, `Simulate(time, true, true)` to fill scene before render.

### E. Particle integration per capture menu
- **Atmosphere matrix**: explicit `ShouldEmitRain/Dust/EmberForMatrixEntry` checks based on `atlas_variant_id`, `intent_id`, and `weather_card_subset` content
  - Wet_A or rain/wet/storm intent → rain particles (intensity scales with intent)
  - Overcast_A / GoldenHour_A or dust/dry/midday intent → dust drift
  - Candle/ember/lamp/hearth intent or Night S1/S2 → ember
- **DQ3R PP / 4K / 8K hero**: `AddDQ3RAtmosphericParticles(parent, bounds, tod, beat)` selects per (tod, beat category):
  - Dawn outdoor → dust (subtle)
  - Day outdoor → dust (medium)
  - Dusk interior → ember; outdoor → dust thin
  - Night interior + Time Window → ember strong; S5 → dust thin; other outdoor → ember weak

### F. Multi-resolution Hero Cinematic
- 4K: 4 hero beats × 4 TOD = 16 captures at 3840 x 2160 (296 KB - 468 KB each)
- 8K: 4 hero beats × 4 TOD = 16 captures at 7680 x 4320 (896 KB - 1.4 MB each)
- All use per-beat semantic character roster + per-TOD post-process + per-TOD variant tint + per-TOD atmospheric particles + Cinematic Dramatic camera + `ConfigureCameraForPostProcess`

### G. Python composite sheets (4)
- `tools/render_dq3r_review_sheets.py` produces:
  - 1920 x 1080 DQ3R quality dashboard (4 x 4 grid)
  - 1920 x 1080 TOD overview sheet (4 x 8 grid)
  - 1920 x 1080 Pass 5 camera rig overview (4 x 2)
  - 1920 x 1080 8K hero overview sheet (4 TOD x 4 beats grid)

## Verification

- Targeted `Anemora.Tests.EditMode.Chapter1MapAssetTests` passes `31/31` (13.1 sec) after every iteration above (atlas variant per-renderer tint, per-beat character roster, per-beat post-process tuning, particle integration in atmosphere matrix and DQ3R PP / 4K / 8K).
- `git diff --check` on the touched code/asset/docs paths: passes.
- `python tools/verify_chapter1_next5_static.py` returns 10/10 pass.
- Unity batchmode side effects (Addressables link.xml / GraphicsSettings.asset / QualitySettings.asset modifications, Windows.meta untracked, tools/__pycache__) are cleaned up after every batch with `git restore` + targeted `rm -f` / `rm -rf`.
- Production scene `Assets/Scenes/Anemora_Main.unity` was not opened or saved.
- Time Window adjacent surface remains a thin world-space visual window; no gate / ring / arch / portal opening was added.
- Character v4 / v10 candidates remain review-only; runtime sprite GUIDs untouched.
- atlas v3 UV rects, 50 tile IDs, UV layer name `Chapter1_Antela_SurfaceAtlas_A` remain stable.

## Capture inventory snapshot

| Category | Count | Resolution | Particles | Characters |
|---|---:|---|---|---|
| close-density | 6 | 1920 x 1080 | – | – |
| next4 material-depth | 8 | 1920 x 1080 | – | – |
| TOD review | 32 | 1920 x 1080 | – | – |
| Cinematic | 12 | 1920 x 1080 | – | – |
| Pass 5 atmosphere matrix | 33 | 1920 x 1080 | conditional rain / dust / ember | – |
| Pass 5 camera rig | 8 | 1920 x 1080 | – | – |
| DQ3R PP review | 16 | 1920 x 1080 | per-TOD | per-beat roster |
| 4K Hero Cinematic | 16 | 3840 x 2160 | per-TOD | per-beat roster |
| 8K Hero Cinematic | 16 | 7680 x 4320 | per-TOD | per-beat roster |
| Composite sheets | 4 | 1920 x 1080 | – | – |
| **計** | **151** | – | – | – |

## What's next (pass6 candidates)

Listed for future sessions; not implemented in this slice:

1. **Custom URP shader** that performs a true atlas variant albedo swap per material (current implementation is `MaterialPropertyBlock._BaseColor` multiply on top of the v3 atlas, which approximates but does not replace the variant authored colors).
2. **Walk-frame variant billboards** — UV-slice walk_front 6f sprites and select frame 0 / 2 / 4 to add motion feel without animating runtime.
3. **Snow particles** — for any `Snow_A` variant if pass6 adds it.
4. **Volumetric sun-shaft slabs** — additive vertical quad bands behind sun direction for stronger dawn / dusk god-ray feel.
5. **Per-character contact shadow oval matched to billboard position** — currently the next5 character-light contact kit is placed at scene center; matching to actual billboard X / Z would tighten the read.
6. **PlayMode capture** — render with one frame of locomotion animation to demonstrate true motion (currently all captures are static stand frame).

## Files touched

- `Assets/Editor/AnemoraChapter1MapAssetSetup.cs` (now ~3900 lines after this slice)
- `Assets/Editor/Stage4CharacterTransferReviewSetup.cs` (V5 sync menu skeleton from earlier)
- `Assets/Tests/EditMode/Chapter1MapAssetTests.cs` (per-path sample-color guard exception list)
- `tools/render_dq3r_review_sheets.py` (extended with `render_pass5_camera_rig_overview` and `render_hero_8k_overview_sheet`)
- `tools/report_pass5_promotion_summary.py` (auto Markdown summary of pass5 promotion manifest)
- `docs/dq3r_visual_rubric.md` (v0.1 → v0.4)
- `docs/VERIFICATION_SUITE.md` (v2.71 → v2.73)
- `docs/ASSET_STRUCTURE.md` (v0.17 → v0.19)
- `docs/legal/asset_ledger.md` (next4 + pass5 rows)
- `docs/devlog/INDEX.md` (v3.38 → v3.39)
- `CHANGELOG.md` (multiple entries)
- This devlog
