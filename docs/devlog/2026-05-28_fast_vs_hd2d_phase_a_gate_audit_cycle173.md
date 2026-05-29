# docs(hd2d): add phase a gate audit evidence

Date: 2026-05-28 JST

## Scope

- Phase A gate evidence hardening while Tom judgement is pending.
- This cycle does not start Phase B-alpha/B-beta/C-alpha/C-beta.
- Add a consolidated Phase A gate audit validate/capture entry in `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
- The audit should cover SunCycle wiring, runtime `MapSunAnchor.SetPresetAtRuntime()` handoff, 1.8s transition source path, main directional handoff, painted overlay removal, event-driven renderer shadow policy, surface shader lightening, and the corrected five-area public review set.

## Cycle Worker

SCOPED_PROMPT_ISSUED cycle=173 authored_file=Assets/Editor/AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAGateAuditBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseAGateAuditCycle173ScreenshotsBatch

## Verification Plan

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAGateAuditBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseAGateAuditCycle173ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built exe launch with `-batchmode -nographics`, scanning configured failure patterns.

## Build Artifact For Tom

Build exe path:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `Builds\FastVS_HouseSlice\` フォルダごと起動対象として扱う。

## Gate Wording

変更を適用しました。

参考画像とのギャップは、Phase A 時点では Buto の volumetric god rays、Tilt Shift、HDR emissive/VFX が未導入であり、target HD-2D quality remains substantially below reference です。

Tom 判定をお願いします。
