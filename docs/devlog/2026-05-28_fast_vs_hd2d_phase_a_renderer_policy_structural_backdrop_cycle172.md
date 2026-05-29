# fix(hd2d): preserve structural backdrop renderers

Cycle: 172
Date: 2026-05-28
Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

## Scope

- Phase A follow-up for Cycle170 event-driven renderer shadow policy.
- Keep structural backdrop / sky composition renderers enabled when the area-transition path applies realtime shadow policy.
- Keep Cycle detail / closure objects out of realtime shadow receiving/casting unless they are the dedicated `RealtimeShadowCasterCycle` objects.
- Preserve Phase A removals: no Cycle128 / Cycle131 painted overlay path and no 0.35s periodic renderer policy refresh.

## Implementation

- `FastVsRealtimeLightShadowRig` now treats structural backdrop names (`OutdoorVoidBackground`, `ScenicBackdrop`, `SkyBarMask`, backdrop foundation/readability/occlusion objects) as renderers that must not be disabled by realtime shadow policy.
- `IsRealtimeShadowSafeDetailName` now excludes facade leak/opaque/naturalization and non-caster Cycle detail objects from realtime shadow receive/cast decisions.
- Added Phase A Cycle172 source/render validation and capture entries in `AnemoraFastVsHouseSliceSetup`.

## Verification Plan

- Validate:
  `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseARendererPolicyStructuralBackdropBatch`
- Capture:
  `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseARendererPolicyStructuralBackdropCycle172ScreenshotsBatch`
- Build:
  `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer`
- Smoke:
  built player for 24 seconds with failure-pattern scan.

## Review Artifacts

- Curated review set will be copied to `docs/review/<timestamp>/` after successful capture/build/smoke.
- Build exe path:
  `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Launch note:
  start from the whole `Builds/FastVS_HouseSlice/` folder, not by moving the exe alone.

## Cycle 172 failure (validate) -- 20260528-122237

```
[12:22:37] Cycle runner starting
[12:22:37]   CycleNumber    : 172
[12:22:37]   ProjectPath    : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
[12:22:37]   BatchTool      : C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
[12:22:37]   ValidateMethod : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseARendererPolicyStructuralBackdropBatch
[12:22:37]   CaptureMethod  : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseARendererPolicyStructuralBackdropCycle172ScreenshotsBatch
[12:22:37]   BuildMethod    : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer
[12:22:37]   Audience       : parent_review
[12:22:37]   CaptureOutDir  : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots
[12:22:37]   DevlogPath     : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-28_fast_vs_hd2d_phase_a_renderer_policy_structural_backdrop_cycle172.md
[12:22:37]   SmokeSeconds   : 24
[12:22:37]   SmokePatterns  : Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed
[12:22:37]   CommitPath     : Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs; Assets/Editor/AnemoraFastVsHouseSliceSetup.cs; docs/devlog/2026-05-28_fast_vs_hd2d_phase_a_renderer_policy_structural_backdrop_cycle172.md; docs/devlog/screenshots/fast_vs_hd2d_cycle172_structural_backdrop_renderer_policy_parent_review_20260528_01
[12:22:37]   NoRollback     : True
[12:22:37]   RunLog         : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\tools\logs\cycle-172-20260528-122237.log
[12:22:37] Phase 'validate' begin: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseARendererPolicyStructuralBackdropBatch

===== validate batch log (C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-172-20260528-122237-validate.log) =====
[Licensing::Module] Trying to connect to existing licensing client channel...
Built from '6000.3/staging' branch; Version is '6000.3.14f1 (d68c3f99a318) revision 14060607'; Using compiler version '194234433'; Build Type 'Release'
OS: 'Windows 11  (10.0.26200) Core' Language: 'en' Physical Memory: 14177 MB
[Licensing::IpcConnector] Successfully connected to: "LicenseClient-maro6" at "2026-05-28T03:22:37.9439026Z"
BatchMode: 1, IsHumanControllingUs: 0, StartBugReporterOnCrash: 0, Is64bit: 1
System  architecture: x64
Process architecture: x64
Date: 2026-05-28T03:22:37Z

COMMAND LINE ARGUMENTS:
C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
-batchmode
-quit
-projectPath
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
-executeMethod
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseARendererPolicyStructuralBackdropBatch
-logFile
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-172-20260528-122237-validate.log
Successfully changed project path to: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
C:/Users/maro6/Documents/Unity/Anemora-fast-vs-v24-hd2d-work
Exiting without the bug reporter. Application will terminate with return code 1
[12:23:17] Phase 'validate' FAILED with exit 1
[12:23:17] NoRollback set; preserving worktree after validate failure
```
