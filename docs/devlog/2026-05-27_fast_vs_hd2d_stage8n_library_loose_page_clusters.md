# Fast VS HD-2D Stage8n Library Loose Page Clusters

Date: 2026-05-27 JST

## Scope

- Continued the Library-focused HD-2D pass after Stage8m.
- Added current-side authored loose page/book clusters:
  - long-table bookmark slip, loose page pair, and page shadow
  - floor loose page and closed book with contact shadows
  - right-desk page and book stack accents
- Kept all Stage8n additions current-space-only, non-colliding, and non-arrival for TimeWindow pairing.
- Target HD-2D quality remains substantially below reference; this cycle adds small readable micro accents for review.

## Implementation

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Added `CreateCurrentLibraryLoosePageClustersStage8n`.
  - Added Stage8n validate and capture batch methods:
    - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage8NLibraryLoosePageClustersBatch`
    - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage8NLibraryLoosePageClustersReferenceScreenshotsBatch`
  - Extended Stage8i library review camera composition to Stage8n captures.

## Validation

- Unity validate:
  - `Logs/stage8n_library_loose_page_clusters_validate.log`
  - exit code: 0
  - log contains `Batchmode quit successfully` and `Exiting batchmode successfully`.
- Unity capture:
  - `Logs/stage8n_library_loose_page_clusters_capture.log`
  - exit code: 0
  - output: `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260527_stage8n_library_loose_page_clusters`
- Unity build:
  - `Logs/stage8n_library_loose_page_clusters_build.log`
  - exit code: 0
  - log contains `Build Finished, Result: Success.`
- Built exe smoke:
  - `Logs/stage8n_library_loose_page_clusters_smoke.log`
  - launched for 24 seconds with `-batchmode -nographics`
  - scanned patterns: `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`
  - pattern count: 0

## Local Visual Metrics

Metrics use simple RGB mean luminance and dark45 ratio.

| File | mean | dark45 | centralDark45 | alpha |
|---|---:|---:|---:|---|
| plaza_01.png | 89.5 | 0.067 | 0.090 | 255..255 |
| library.png | 73.0 | 0.149 | 0.122 | 255..255 |
| tw_current_aperture.png | 82.3 | 0.114 | 0.007 | 255..255 |
| home.png | 61.8 | 0.190 | 0.266 | 255..255 |
| Home_outside.png | 66.6 | 0.197 | 0.137 | 255..255 |
| plaza_02_niro_in_shadow.png | 82.6 | 0.088 | 0.131 | 255..255 |

Stage8m vs Stage8n library delta:

- `meanAbs`: 1.693
- `changedRatio_gt3`: 0.0348

## Public Review Set

- `docs/review/2026-05-27T11-00/`
  - `01_plaza_overview.png`
  - `02_library_loose_page_clusters.png`
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
