# feat(hd2d): remove realtime rig painted overlay

Date: 2026-05-28 JST

## Scope

- Phase A Step 3 from `docs/HD2D_SUN_CYCLE_SPEC.md`.
- Removed old Cycle128/Cycle131 painted camera overlay runtime generation and application from `FastVsRealtimeLightShadowRig.cs`.
- Kept renderer shadow policy, 0.35s policy refresh removal, and shader lightening for later Phase A cycles.

## Cycle Worker

SCOPED_PROMPT_ISSUED cycle=169 authored_file=Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAPaintedOverlayRemovalBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseAPaintedOverlayRemovalCycle169ScreenshotsBatch

- Worker `019e6c24-1f60-7a62-ab3e-852856d5dd72` identified the Cycle128/Cycle131 overlay scope but did not edit files.
- Parent applied the implementation and validation helper changes.

## Implementation

- `FastVsRealtimeLightShadowRig.cs` is reduced from 1417 lines to 922 lines.
- Removed runtime camera quad creation, transient materials, generated textures, and activation flow for the old painted overlay path.
- Source grep evidence: `Cycle128`, `Cycle131`, and `Painted` return 0 matches in `FastVsRealtimeLightShadowRig.cs`.
- Preserved realtime renderer shadow policy and the current 0.35s refresh path for the next Phase A cycle.

## Validation

- Runner: `tools/logs/cycle-169-20260528-101724.log`
- Validate: `Logs/cycle-169-20260528-101724-validate.log`
- Capture: `Logs/cycle-169-20260528-101724-capture.log`
- Build: `Logs/cycle-169-20260528-101724-build.log`
- Smoke: `Logs/cycle-169-20260528-101724-smoke.log`
- Smoke scan patterns: `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`
- Smoke pattern count: 0
- Post-commit validate of the final helper source: `Logs/cycle-169-postcommit-validate.log` exited with return code 0.

Two later validate attempts, `cycle-169-20260528-101910` and `cycle-169-20260528-101959`, exited early because the earlier Cycle169 runner still owned the Unity project during build/smoke. They did not roll back or replace the successful runner output.

## Captures

- Devlog captures: `docs/devlog/screenshots/fast_vs_hd2d_phase_a_painted_overlay_removal_cycle169/`
- Public review set: `docs/review/2026-05-28T10-22/`
- This is not the Phase A Tom gate. Phase A still needs the 0.35s refresh removal, shader lightening, 5-area gate screenshots, TimeWindow aperture check, and Tom review before Phase B-alpha.

## Build Artifact For Tom

Build exe path for review:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。
