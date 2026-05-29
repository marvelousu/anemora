# Fast VS HD-2D Stage8k Library Chair/Shelf Depth

Date: 2026-05-27 JST

## Scope

- Continued the Library-focused HD-2D pass after Stage8j.
- Added current-side authored chair and shelf depth accents around the library long table:
  - cool floor spill behind the near chair
  - warm floor kick under the chair
  - chair seat lip, back cut, legs, and contact shadow
  - back shelf warm rim, pocket shadow, and page glint
- Kept all Stage8k additions current-space-only, non-colliding, and non-arrival for TimeWindow pairing.
- Target HD-2D quality remains substantially below reference; this cycle only adds a small authored depth layer for review.

## Implementation

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `CreateCurrentLibraryChairShelfDepthStage8k`.
  - Added Stage8k validate and capture batch methods:
    - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8KLibraryChairShelfDepthBatch`
    - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8KLibraryChairShelfDepthReferenceScreenshotsBatch`
  - Extended Stage8i library review camera composition to Stage8k captures.

## Validation

- Unity validate:
  - `Logs/stage8k_library_chair_shelf_depth_validate.log`
  - exit code: 0
  - log contains `Batchmode quit successfully` and `Exiting batchmode successfully`.
- Unity capture:
  - `Logs/stage8k_library_chair_shelf_depth_capture.log`
  - exit code: 0
  - output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8k_library_chair_shelf_depth`
- Unity build:
  - `Logs/stage8k_library_chair_shelf_depth_build.log`
  - exit code: 0
  - log contains `Build Finished, Result: Success.`
- Built exe smoke:
  - `Logs/stage8k_library_chair_shelf_depth_smoke.log`
  - launched for 24 seconds with `-batchmode -nographics`
  - scanned patterns: `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`
  - pattern count: 0

## Local Visual Metrics

Metrics use simple RGB mean luminance and dark45 ratio.

| File | mean | dark45 | centralDark45 | alpha |
|---|---:|---:|---:|---|
| plaza_01.png | 89.5 | 0.067 | 0.090 | 255..255 |
| library.png | 71.6 | 0.147 | 0.122 | 255..255 |
| tw_current_aperture.png | 82.3 | 0.114 | 0.007 | 255..255 |
| home.png | 61.8 | 0.190 | 0.266 | 255..255 |
| Home_outside.png | 66.6 | 0.197 | 0.137 | 255..255 |
| plaza_02_niro_in_shadow.png | 82.6 | 0.088 | 0.131 | 255..255 |

Stage8j vs Stage8k library delta:

- `meanAbs`: 0.636
- `changedRatio_gt3`: 0.0222

## Public Review Set

- `docs/review/2026-05-27T09-31/`
  - `01_plaza_overview.png`
  - `02_library_chair_shelf_depth.png`
  - `03_timewindow_aperture.png`
  - `04_home_interior.png`
  - `05_house_exterior.png`
  - `06_plaza_shadow_route.png`
  - `devlog.txt`
  - `index.md`

## Build

- Build exe:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- 起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。
