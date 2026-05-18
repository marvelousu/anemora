# 2026-05-09 Stage 4 Chapter 1 Map DQ3R Atmospheric Progress

## Goal

Push Chapter 1 map captures toward DQ3R-class HD-2D atmospheric variation while pass5 (atmosphere variant masks, weather cards, camera rigs, promotion gate) is still being authored by the map generation session and v5 (proportion-lock pack) is still being authored by the character generation session.

## What Landed

### A. Time-of-day Unity capture pipeline

- New menu: `Anemora/Assets/Capture Chapter1 Map Time Of Day Review`.
- Four TOD presets (`Dawn`, `Day`, `Dusk`, `Night`) drive ambient color, fog density / color, directional key light angle / warmth, and a rim point light. `Night` adds a warm candle-tinted rim that simulates in-scene lamp glow even when narrative ambient is dim. `S5` beats receive a `1.4×` fog-density boost so the open ruin reads denser haze regardless of TOD.
- Output: 32 captures (`8 beats × 4 TOD`) named `stage4_chapter1_map_unity_tod_<tod>_<beat_key>_review.png` (1920 x 1080).
- After an initial sample-color guard miss on dim S3 Current Night captures, the `Night` ambient was raised to 0.86 intensity, sky color to `(0.32, 0.40, 0.58)`, and rim intensity to 1.20 with range 14m. The result still reads as night while keeping decal / wear detail visible.

### B. Cinematic camera variants

- New menu: `Anemora/Assets/Capture Chapter1 Map Cinematic Review`.
- Three angle variants per beat:
  - `Dramatic` — pitch 30°, ortho `bounds.x / 2.20`, distance multiplier 2.55 (medium hero look).
  - `Overhead` — pitch 60°, ortho `bounds.x / 2.65`, distance multiplier 1.40 (route overview feel).
  - `LowAngle` — pitch 28° (was 22° before reframing pass), ortho `bounds.x / 2.55`, distance multiplier 2.50 (cinematic horizontal read while keeping the scene framed).
- Beats: S3 Current, S3 Past, S4 Past, S5 North.
- Output: 12 captures (`4 beats × 3 variants`) named `stage4_chapter1_map_unity_cinematic_<variant>_<beat_key>_review.png`.

### C. DQ3R post-process review

- New menu: `Anemora/Assets/Capture Chapter1 Map DQ3R Post Process Review`.
- Builds an in-memory URP `Volume` per scene with a transient `VolumeProfile` (HideFlags.DontSave). Effects:
  - `Bloom`: intensity 0.40, threshold 1.05, scatter 0.70.
  - `Vignette`: intensity 0.18, smoothness 0.80, cool tint `(0.05, 0.05, 0.08)`.
  - `ColorAdjustments`: contrast +6, saturation +4, post-exposure +0.05.
- Capture per beat × TOD (4 beats × 4 TOD = 16 PNGs) using the Time-of-day presets and the existing `next4` material-depth assets, then enables `renderPostProcessing` on the orthographic capture camera.
- Output: 16 captures named `stage4_chapter1_map_unity_dq3r_pp_<tod>_<beat_key>_review.png`.

### D. S4 Current darker-than-ideal beat correction

- `ApplyCloseDensityRenderSettings` adds an `isS4Current` branch that uses warmer ambient `(0.62, 0.62, 0.48)`, warmer equator `(0.38, 0.42, 0.30)`, warmer fog `(0.32, 0.36, 0.27)`, and ambient intensity 1.30 (vs the default 1.16) so the open Kaia field reads brighter and warmer than before. Other beats unchanged.
- Refreshed close-density captures: S4 Current PNG grew from 190 KB to 313 KB after the lighting fix, matching its new color / detail variety.

### E. Pass5 receive-side scaffolding (no-op until pass5 lands)

- Path constants for the pass5 manifests: atlas variant manifest, weather/atmosphere card kit manifest, atmosphere beat matrix manifest, camera rig manifest, production promotion manifest, Time Window thin pack manifest, character-light contact manifest.
- `ApplyAtlasV3AndDecalImporterSettings` now also applies consistent texture importer settings (Texture2D / Repeat / Bilinear / mipmaps on / max 1024 / NormalMap importer for normals / no sprite mode) to the seven pass5 atmosphere variant masks (`Day`, `Dawn`, `Dusk`, `Night`, `Overcast`, `Wet`, `GoldenHour`) and the Time Window thin pack PNGs. Each entry checks `File.Exists` first so this is a no-op until pass5 lands.

### F. Character v5 receive-side menu skeleton

- `Anemora/Assets/Sync DQ3R Character V5 Proportion Lock` mirrors the v3 / v4 sync pattern. Copies `model_sheets`, `proportion_gate`, `runtime_candidates_64x96`, `resident_b_idle_only_v5`, and `scene_fit` directories plus 21 v5 manifest / metric / decision / verification files into an isolated review root. Errors loudly if the source pack has not been delivered yet.
- Production runtime sprite GUIDs are not touched. Production import remains blocked until user proportion review.

### G. DQ3R quality dashboard composite (Python / PIL)

- `tools/render_dq3r_review_sheets.py` produces two composite review sheets from the existing Unity captures, no Unity required:
  - `docs/devlog/screenshots/stage4_chapter1_map_unity_dq3r_quality_dashboard.png` — 1920 x 1080 4 x 4 grid (close-density / TOD-Day / TOD-Night / TOD-Dusk / cinematic dramatic / overhead / lowangle / DQ3R PP variants per beat).
  - `docs/devlog/screenshots/stage4_chapter1_map_unity_tod_overview_sheet.png` — 1920 x 1080 4 row × 8 column grid showing all 32 TOD captures at-a-glance (each row a TOD preset, each column a Chapter 1 beat).
- Each thumbnail keeps aspect ratio via `PIL.Image.thumbnail`. Headers (top row) and labels (bottom right) are rendered via `ImageDraw` against semi-transparent strips so dark backgrounds stay readable.

### H. DQ3R visual rubric doc

- `docs/dq3r_visual_rubric.md` v0.1: eight criteria (foreground depth, midground density, background silhouette, contact grounding, atmospheric perspective, light cards / rim, decal authoring, color temperature contrast) graded `pass` / `watch` / `fail` per beat. Cross-cutting pass status table and ordered polish targets for pass5 / pass6 / v5 integration.

## Verification

- Unity batchmode with `executeMethod` on `CaptureChapter1MapTimeOfDayReview`, `CaptureChapter1MapCinematicReview`, `CaptureChapter1MapDQ3RPostProcessReview`, and the close-density refresh all returned exit code `0` (no compile errors, no shader errors, no exceptions / asserts / null refs / missing methods, after excluding known Unity licensing / socket startup noise and the existing `TextureImporter.spritesheet` obsolete API warning).
- Initial sample-color guard caught three under-framed / dim captures (TOD Night S3 Current, DQ3R PP Night S3 Current, Cinematic LowAngle S3 Current). After raising Night ambient / rim intensity and reframing the LowAngle camera (pitch 22°→28°, ortho `2.10` → `2.55`, distance `2.85` → `2.50`), all three captures pass the 16-unique-color guard. The guard remains intact at its original strict threshold.
- The Python composite script ran with `missing 0` against both the dashboard and TOD overview sheet inputs, producing 360 KB and 216 KB output PNGs respectively.

## Tests

EditMode test additions in `Chapter1MapAssetTests`:

- `Chapter1MapTimeOfDayReviewCapturesExist` — 32 PNGs with `1920 x 1080` + sample-color guard.
- `Chapter1MapCinematicReviewCapturesExist` — 12 PNGs with `1920 x 1080` + sample-color guard.
- `Chapter1MapDQ3RPostProcessReviewCapturesExist` — 16 PNGs with `1920 x 1080` + sample-color guard.
- `Chapter1MapPass5ManifestPathConstantsAreReachableViaReflection` — verifies the seven pass5 manifest path constants exist on `AnemoraChapter1MapAssetSetup` via reflection.
- `Chapter1MapDQ3RQualityDashboardCompositeExists` — 1920 x 1080 + sample-color guard for the Python-composited dashboard.
- `Chapter1MapTODOverviewSheetCompositeExists` — 1920 x 1080 + sample-color guard for the Python-composited TOD overview.

The existing 12 base tests + 5 next4 tests (`Chapter1MapAssetTests`) remain green; total grew from `17/17` to `23/23` after this pass.

## What's Next

Ordered by user-facing impact:

1. Map session pass5 atmosphere variant masks — when they land, `ApplyAtlasV3AndDecalImporterSettings` already wires them in. Build atmosphere beat matrix capture menu.
2. Map session pass5 camera rigs — wire into a cinematic capture menu that uses the manifest-defined rigs instead of the three Dramatic / Overhead / LowAngle defaults.
3. Map session pass5 production promotion manifest — build a Unity-side reader that surfaces `production_ready` vs `production_pending_user_review` vs `production_blocked` for each beat / asset and emits a `production_promotion_overview.png` review sheet.
4. Character v5 proportion-lock pack — wire `SyncCharacterTransferV5ProportionLock` then add a v5 scene-fit capture menu mirroring the v3 / v4 capture pattern (no runtime sprite replacement).
5. S5 north fog/depth deepening — multi-band fog cards + distant ruin silhouette FBX (pass5 deliverable).
6. Time Window thin sub-band variants — keep thin; add inner-rim warm/cool variants and distance-fade scuff (pass5 deliverable).
