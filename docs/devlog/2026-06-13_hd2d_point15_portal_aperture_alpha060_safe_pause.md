# 2026-06-13 HD2D point15 portal aperture alpha 0.60 safe pause

## Scope

- Safely pause after the user requested a stop at a good point in the fog/visual-recovery line.
- The active thread had just moved from fog tuning into the time-window open visual-change reprobe.
- Built-player evidence remains mandatory for acceptance.
- This record is a pause/handoff, not an accepted visual-fix cycle.

## Baseline reprobe before the pending aperture alpha change

- Review folder:
  - `docs\review\2026-06-13T10-20_window_aperture_reprobe_after_cycle125_alpha060`
- Player log:
  - `Logs\point15_window_aperture_reprobe_after_cycle125_alpha060_20260613T1020.log`
- Captures:
  - `PNG_COUNT=23`
  - `PNG_TOTAL_BYTES=18324472`
- Completion:
  - `ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: end count=23`

Lighting state was unchanged across closed/open/closed-after:

```text
LIGHT label=windowDoorReview.closed.ready expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning transitioning=False main=1.500 warm=0.300 cool=0.160 ambientMax=0.074 fog=False
LIGHT label=windowDoorReview.open.ready expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning transitioning=False main=1.500 warm=0.300 cool=0.160 ambientMax=0.074 fog=False
LIGHT label=windowDoorReview.closedAfter.ready expectedArea=CentralPlaza activeArea=CentralPlaza sunCurrent=Morning sunTarget=Morning transitioning=False main=1.500 warm=0.300 cool=0.160 ambientMax=0.074 fog=False
```

Open-window deltas before the pending alpha change:

```text
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_open meanAbsRgb=6.409 changedSamplePct=20.271 changed=11676 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=currentApertureOff matched=1 disabled=1 logged=[FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V25_Current_LivePortalAperture_ClippedToFrame]
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta open_vs_currentApertureOff meanAbsRgb=5.666 changedSamplePct=19.450 changed=11203 samples=57600
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_currentApertureOff meanAbsRgb=0.964 changedSamplePct=2.363 changed=1361 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=currentPortalFrameOnlyOff matched=5 disabled=5 logged=[FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V21_Current_PortalThresholdLine_NotPicture | FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V21_Current_Frame_Bottom | FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V21_Current_Frame_Top | FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V21_Current_Frame_Right | FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V21_Current_Frame_Left]
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta open_vs_currentPortalFrameOnlyOff meanAbsRgb=0.936 changedSamplePct=2.366 changed=1363 samples=57600
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_currentPortalFrameOnlyOff meanAbsRgb=5.690 changedSamplePct=19.483 changed=11222 samples=57600
ANEMORA_HOUSE_SLICE_RENDERER_ISOLATION: variant=currentPortalAllOff matched=6 disabled=6 logged=[FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V25_Current_LivePortalAperture_ClippedToFrame | FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V21_Current_PortalThresholdLine_NotPicture | FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V21_Current_Frame_Bottom | FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V21_Current_Frame_Top | FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V21_Current_Frame_Right | FastVS_Current_NiroHouseInteriorExterior/TW_V21_CurrentPortal_GeneratedThreshold/TW_V21_Current_Frame_Left]
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta open_vs_currentPortalAllOff meanAbsRgb=6.395 changedSamplePct=20.245 changed=11661 samples=57600
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_currentPortalAllOff meanAbsRgb=0.020 changedSamplePct=0.080 changed=46 samples=57600
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedBefore_vs_closedAfter meanAbsRgb=0.004 changedSamplePct=0.035 changed=20 samples=57600
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: imageDelta closedDoorBaseline_vs_closedBeforePortal meanAbsRgb=0.465 changedSamplePct=1.842 changed=1061 samples=57600
ANEMORA_HOUSE_SLICE_WINDOW_DOOR_REVIEW: end count=23
```

Aperture material state before the pending alpha change:

```text
TW_V25_Current_LivePortalAperture_ClippedToFrame" active=True enabled=True visible=True layer=26 sortingLayer=0 sortingOrder=0 bounds=center=(21.594,1.239,19.419),size=(2.937,2.347,0.000),min=(20.125,0.065,19.419),max=(23.062,2.412,19.419) materials=[name=Current_LivePortalApertureMaterial,shader=Anemora/Review/PortalApertureOverlay,queue=2990,tagQueue=Transparent-10,tagRenderType=Transparent,_Surface=<missing>,_Cull=<missing>,_ZWrite=<missing>,_ZTest=<missing>,_AlphaClip=<missing>,_SrcBlend=<missing>,_DstBlend=<missing>,_Color=(1,1,1,0.72),_BaseColor=<missing>
```

Interpretation: the open-window visual change is not caused by a lighting preset switch in this built-player probe. The measured driver is still the current-side live portal aperture composite over the scene.

## Pending code change

- File:
  - `Assets\Scripts\TimeManagement\TimeWindowPairedSpacePortalController.cs`
- Change made before pausing:
  - `PortalApertureCompositeAlpha: 0.72f -> 0.60f`
- Intent:
  - Reduce the current-side portal aperture overlay influence without changing SunCycle, GradientSky, shadows, or Cycle125 fog/air objects.
- Scope warning:
  - This is not yet accepted. It has a successful build only; the post-change built-player window-door capture still needs to be run.

## Build evidence after pending code change

- Build log:
  - `Logs\unity_build_validate_portal_aperture_alpha060_20260613T1030.log`
- Key lines:

```text
Fast VS house slice validation passed.
Build Finished, Result: Success.
Fast VS house slice player built: C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260612\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe
Exiting batchmode successfully now!
```

- Built player:
  - `Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
  - `Length=667648`
  - `LastWriteTime=2026-06-13 10:37:08`
- Unity process state at pause:
  - `UNITY_RUNNING=False`

## Stop state

- No post-change built-player capture has been run for `PortalApertureCompositeAlpha = 0.60f`.
- No R2 upload has been run for this pending aperture-alpha change.
- `anemora-viewer` has not been updated for this pending aperture-alpha change.
- Do not treat this build as accepted until the next built-player review exists.

## Required next step

1. Run built-player window-door review against the latest player:
   - output suggestion: `docs\review\2026-06-13T10-50_window_aperture_alpha060_after_build`
   - log suggestion: `Logs\point15_window_aperture_alpha060_after_build_20260613T1050.log`
   - argument: `--anemora-house-slice-window-door-review-dir`
2. Confirm material state changed to `_Color=(1,1,1,0.6)`.
3. Compare against the pre-change values above:
   - `closedBefore_vs_open`
   - `open_vs_currentApertureOff`
   - `closedBefore_vs_currentApertureOff`
4. If accepted by measurement and visual inspection, then create the full cycle evidence:
   - built-player captures
   - devlog finalization
   - review `devlog.txt`
   - R2 upload
   - `anemora-viewer` refresh/build/push/public 200 checks

