# point15 library front marker removal

Date: 2026-06-10
Branch: `wip/hd2d-point15-recovery-20260609`
Worktree: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260609`
Baseline: point15 `e7277f0a` plus the current point15 recovery renderer contract.

## Context

The user reported that the object circled in `C:\Users\maro6\Pictures\Screenshots\Screenshot 2026-06-09 222113.png` was still visible in front of the library door. The previous window-door note identified `Current_CentralPlaza_LibraryDoorCenterPlank` as the likely object, but that finding was wrong for the circled thick pillar. Built-player-only evidence is required for visual acceptance.

## Correction

The actual object isolated by the built-player probe was:

- `Current_CentralPlaza_Chapter1_B2_LibraryFrontMarker`
- Bounds in the marker probe: `center=(20.800,0.680,23.100),size=(0.180,0.560,0.180),min=(20.710,0.400,23.010),max=(20.890,0.960,23.190)`
- Material in the marker probe: `FastVS_House_current_fence`

The earlier `Current_CentralPlaza_LibraryDoorCenterPlank` removal remains recorded as a separate door-detail change, but it was not the screenshot-circled pillar.

## Probe Evidence

Built-player marker isolation probe:

- Build/validation log: `Logs\point15_window_door_marker_probe_build_validate_20260610T010956.log`
- Player log: `Logs\point15_window_door_marker_probe_player_20260610T012032.log`
- Review directory: `docs\review\2026-06-10T01-21_window_door_marker_probe`
- Key capture: `docs\review\2026-06-10T01-21_window_door_marker_probe\02_no_library_front_marker.png`

The `02_no_library_front_marker.png` isolation capture removed the thick door-front pillar while keeping the door and surrounding facade, confirming the target object.

## Fix

Updated `Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Removed generation of `${prefix}_CentralPlaza_Chapter1_B2_LibraryFrontMarker`.
- Removed `Current_CentralPlaza_Chapter1_B2_LibraryFrontMarker` and `Past_CentralPlaza_Chapter1_B2_LibraryFrontMarker` from required Chapter 1 continuation baseline markers.
- Added validation that both marker objects are absent:
  - `ValidateSceneObjectRemoved("Current_CentralPlaza_Chapter1_B2_LibraryFrontMarker")`
  - `ValidateSceneObjectRemoved("Past_CentralPlaza_Chapter1_B2_LibraryFrontMarker")`

Updated `Assets\Scripts\FastVS\FastVsHouseRuntimeSmokeProbe.cs`:

- Added the marker to the door-candidate status list.
- Added a marker-specific isolation capture named `02_no_library_front_marker.png`.
- Kept the ROI candidate capture set for future door-front object identification.

## Final Build

Final build/validation log:

- `Logs\point15_window_door_marker_removed_build_validate_20260610T012327.log`

Measured build evidence:

- `Fast VS house slice validation passed.`
- `DisplayProgressNotification: Build Successful`
- `Build Finished, Result: Success.`
- `PlayerBuildInfo duration=81956`

Build output:

- Player exe: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260609\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Data directory updated at `2026-06-10 01:32:14`
- Managed directory updated at `2026-06-10 01:32:15`
- The exe stub timestamp stayed at `2026-06-09 22:55:38`, so the exe mtime alone is not valid evidence for whether this fix is included.

## Final Built-Player Evidence

Final built-player capture:

- Player log: `Logs\point15_window_door_marker_removed_player_20260610T013251.log`
- Review directory: `docs\review\2026-06-10T01-32_window_door_marker_removed`

Renderer contract log:

- `pipeline=UniversalRenderPipeline`
- `renderer=UniversalRenderPipeline_Renderer`
- `RenderingMode=2`
- `DepthPrimingMode=0`
- `CopyDepthMode=0`
- `PortalStencilFeatureActive=True`
- Active features include `PortalStencilFeature`, `FastVS HD2D Soft Contact Occlusion`, `FastVS HD2D Stage7 TiltShift`, and `FastVS HD2D Stage7 Outline`
- `error=<none>`

Door-front marker status:

- `Current_CentralPlaza_Chapter1_B2_LibraryFrontMarker=missing`
- `Current_CentralPlaza_LibraryDoorCenterPlank=missing`

Image deltas:

- `closedBefore_vs_open meanAbsRgb=8.327 changedSamplePct=22.141 changed=12753 samples=57600`
- `closedBefore_vs_closedAfter meanAbsRgb=0.001 changedSamplePct=0.007 changed=4 samples=57600`
- `closedDoorBaseline_vs_closedBeforePortal meanAbsRgb=0.480 changedSamplePct=1.908 changed=1099 samples=57600`
- Final probe count: `end count=20`

Final review captures:

- `01_closed_door_light_baseline.png` 799013
- `02_no_library_front_marker.png` 797283
- `03_no_center_plank.png` 797640
- `04_no_door_panels.png` 797695
- `05_roi_candidate_01_Current_CentralPlaza_RuinLandmarkPolish_FountainBasinDus.png` 797276
- `05_roi_candidate_02_Current_CentralPlaza_FountainDryBasinInnerFloor.png` 797278
- `05_roi_candidate_03_Current_CentralPlaza_FountainWater.png` 799771
- `05_roi_candidate_04_Current_CentralPlaza_DryFountainWoodPlankB.png` 798141
- `05_roi_candidate_05_Current_CentralPlaza_DryFountainCrack.png` 797133
- `05_roi_candidate_06_Current_CentralPlaza_Chapter1_Cycle79_FountainNorthRing.png` 797038
- `05_roi_candidate_07_Current_CentralPlaza_Chapter1_Cycle88_FountainNorthPaver.png` 797284
- `05_roi_candidate_08_Current_CentralPlaza_FocalPropReadability_FountainWoodSp.png` 796240
- `05_roi_candidate_09_Current_CentralPlaza_PavingReadability_CentralSeamAccent.png` 797306
- `05_roi_candidate_10_Current_CentralPlaza_RuinLandmarkPolish_FountainRimShado.png` 797241
- `05_roi_candidate_11_Current_CentralPlaza_FountainDryBasinCrackA.png` 797234
- `05_roi_candidate_12_Current_CentralPlaza_Chapter1_Cycle88_NorthBrokenTileB.png` 797332
- `06_no_door_relief_depth.png` 794798
- `10_window_closed_before_open.png` 797213
- `11_window_open.png` 829333
- `12_window_closed_after_open.png` 797242

## Result

The screenshot-circled thick door-front pillar is removed in the final built-player baseline capture. The time-window close/open/close check remains stable after returning closed, with `meanAbsRgb=0.001` and `changed=4/57600` for `closedBefore_vs_closedAfter`.

## Viewer Propagation

R2 upload:

- Command uploaded `22 files for chapter1-continuation-map-vs-20260524/2026-06-10T01-32_window_door_marker_removed`.
- R2 manifest now lists `157 paths`.
- Manifest probe: `status=200 count=157 hasMarkerRemoved=True`.
- Original PNG probe: `status=200 bytes=799013 contentType=image/png` for `01_closed_door_light_baseline.png`.

anemora-viewer:

- Viewer repo: `C:\Users\maro6\projects\anemora-viewer`
- Commit pushed: `4dc7825 chore: refresh review devlog 2026-06-10 marker-removed`
- Public gallery probe: `status=200 bytes=39372 containsMarker=True containsImage=True`
- Public gallery URL: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-10T01-32_window_door_marker_removed/`
- Local viewer generation proof: `collect-content` processed `files=5559, docs=964, images=2894, unsupported=600` and wrote `1 branches, 964 docs, 2705 images`; generated the marker-removed album and WebP thumbnails.

No Anemora push or PR was performed.
