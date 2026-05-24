# feat(hd2d): push plaza reference grade

## Intent

Cycle 127 established actual realtime cast shadows, but the image still read flat and too empty versus the references. Cycle 128 prioritizes a fast visible jump: stronger central-plaza sun, lower ambient, a closer playable camera profile, camera-space grade, and more realtime caster breakup.

## Scope

- Tighten the central-plaza follow camera from the broad review framing to a closer HD-2D framing.
- Override central-plaza runtime review lighting to a stronger low-angle warm sun, very low ambient, and a darker muted clear color.
- Add runtime camera-grade plates for central plaza only:
  - warm/sepia edge grade,
  - diagonal warm ray plate.
- Add 12 extra `ShadowsOnly` realtime casters over the current central plaza for eave, lintel, dapple, rafter, and mid-break shadow layers.
- Keep the runtime renderer policy on realtime casters and shadow receivers, without reviving the old static overlay stack.
- Add Cycle 128 validation and parent-review screenshot capture.

## Validation

Planned runner:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 128 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceGradeCycle128Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceGradeCycle128ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-25_fast_vs_hd2d_plaza_reference_grade_cycle128.md' `
  -Audience parent_review `
  -NoRollback
```

Expected screenshots:

`docs/devlog/screenshots/fast_vs_hd2d_cycle128_plaza_reference_grade_parent_review_20260525_01`

- `parent_review_01_current_central_plaza_reference_grade_hero.png`
- `parent_review_02_current_central_plaza_reference_grade_floor_shadow.png`
- `parent_review_03_current_central_plaza_reference_grade_follow.png`
- `parent_review_04_current_library_reference_grade_guard.png`

## Visual Gate

- Central plaza should no longer read as a flat khaki stage.
- The first and follow shots should show immediate warm sun, deeper shade, edge grade, and visible cast-shadow direction.
- Floor shadow detail should show several overlapping shadow layers rather than one flat band.
- Library guard capture should remain stable with the Cycle 128 central-plaza grade disabled.

## Cycle 128 failure (validate) -- 20260525-003715

```
[00:37:15] Cycle runner starting
[00:37:15]   CycleNumber    : 128
[00:37:15]   ProjectPath    : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
[00:37:15]   BatchTool      : C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
[00:37:15]   ValidateMethod : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceGradeCycle128Batch
[00:37:15]   CaptureMethod  : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaReferenceGradeCycle128ScreenshotsBatch
[00:37:15]   BuildMethod    : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch
[00:37:15]   Audience       : parent_review
[00:37:15]   CaptureOutDir  : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots
[00:37:15]   DevlogPath     : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-25_fast_vs_hd2d_plaza_reference_grade_cycle128.md
[00:37:15]   SmokeSeconds   : 20
[00:37:15]   SmokePatterns  : Error|Exception|Assert|NullReference|Font Atlas Texture|DrawObjectsPass|RenderGraph
[00:37:15]   CommitPath     : Assets/Editor/AnemoraFastVsHouseSliceSetup.cs; Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs; Assets/Scripts/FastVS/FastVsVisualDirectionGuide.cs; docs/devlog/2026-05-25_fast_vs_hd2d_plaza_reference_grade_cycle128.md; docs/devlog/INDEX.md; docs/devlog/screenshots/fast_vs_hd2d_cycle128_plaza_reference_grade_parent_review_20260525_01
[00:37:15]   NoRollback     : True
[00:37:15]   RunLog         : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\tools\logs\cycle-128-20260525-003715.log
[00:37:15] Phase 'validate' begin: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceGradeCycle128Batch

===== validate batch log (C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-128-20260525-003715-validate.log) =====
[Licensing::Module] Trying to connect to existing licensing client channel...
Built from '6000.3/staging' branch; Version is '6000.3.14f1 (d68c3f99a318) revision 14060607'; Using compiler version '194234433'; Build Type 'Release'
OS: 'Windows 11  (10.0.26200) Core' Language: 'en' Physical Memory: 14177 MB
[Licensing::IpcConnector] Successfully connected to: "LicenseClient-maro6" at "2026-05-24T15:37:15.3691401Z"
BatchMode: 1, IsHumanControllingUs: 0, StartBugReporterOnCrash: 0, Is64bit: 1
System  architecture: x64
Process architecture: x64
Date: 2026-05-24T15:37:15Z

COMMAND LINE ARGUMENTS:
C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
-batchmode
-quit
-projectPath
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
-executeMethod
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaReferenceGradeCycle128Batch
-logFile
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-128-20260525-003715-validate.log
Successfully changed project path to: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
C:/Users/maro6/Documents/Unity/Anemora-fast-vs-v24-hd2d-work
Exiting without the bug reporter. Application will terminate with return code 1
[00:37:18] Phase 'validate' FAILED with exit 1
[00:37:18] NoRollback set; preserving worktree after validate failure
```
