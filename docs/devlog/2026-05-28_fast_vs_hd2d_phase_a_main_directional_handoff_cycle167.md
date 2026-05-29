# feat(hd2d): hand off director sun control

Date: 2026-05-28 JST

## Scope

- Phase A Step 2 from `docs/HD2D_SUN_CYCLE_SPEC.md`.
- `FastVsHouseLightingDirector` no longer writes Main Directional rotation/color/intensity/shadowStrength/cookie/cookieSize values.
- `FastVsHouseLightingDirector` keeps the same area transition review APIs and continues to drive ambient, fog, camera background, warm fill, cool rim, and library window lights.
- `AnemoraSunCycleDriver` remains the owner for Directional Light rotation/color/intensity/cookie.
- `FastVsRealtimeLightShadowRig` handoff remains for the next Phase A cycle.

## Cycle Worker

SCOPED_PROMPT_ISSUED cycle=167 authored_file=Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAMainDirectionalHandoffBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseAMainDirectionalHandoffCycle167ScreenshotsBatch

The cycle-worker inspected the local dirty diff and reported that it already satisfied the scoped handoff, so it made no additional edits.

## Validation

Final parent runner execution:

- Runner log: `tools/logs/cycle-167-20260528-094550.log`
- Validate log: `Logs/cycle-167-20260528-094550-validate.log`
- Capture log: `Logs/cycle-167-20260528-094550-capture.log`
- Build log: `Logs/cycle-167-20260528-094550-build.log`
- Smoke log: `Logs/cycle-167-20260528-094550-smoke.log`
- Validate result: exit 0.
- Capture result: exit 0.
- Build result: exit 0.
- Smoke result: built exe launched for 24 seconds with `-batchmode -nographics`; scanned `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`; pattern count 0.

## Source Evidence

Exact source grep for `Assets/Scripts/FastVS/FastVsHouseLightingDirector.cs`:

- `ApplyMainLight(`: 0
- `mainLight.intensity`: 0
- `mainLight.shadowStrength`: 0
- `mainLight.color`: 0
- `mainLight.transform.rotation`: 0
- `mainLight.cookie`: 0
- `mainLight.cookieSize`: 0

## Review Artifacts

Local diagnostic output:

- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_main_directional_handoff_cycle167/parent_review_01_current_house_interior_sun_cycle_morning.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_main_directional_handoff_cycle167/parent_review_02_current_house_exterior_sun_cycle_morning.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_main_directional_handoff_cycle167/parent_review_03_current_central_plaza_sun_cycle_noon.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_main_directional_handoff_cycle167/parent_review_04_current_library_sun_cycle_evening.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_main_directional_handoff_cycle167/main_directional_handoff_diagnostics.md`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_main_directional_handoff_cycle167/sun_cycle_scene_wiring_diagnostics.md`

Public curated review set:

- `docs/review/2026-05-28T09-50/01_house_interior_sun_cycle_morning.png`
- `docs/review/2026-05-28T09-50/02_house_exterior_sun_cycle_morning.png`
- `docs/review/2026-05-28T09-50/03_central_plaza_sun_cycle_noon.png`
- `docs/review/2026-05-28T09-50/04_library_sun_cycle_evening.png`
- `docs/review/2026-05-28T09-50/index.md`
- `docs/review/2026-05-28T09-50/devlog.txt`

This diagnostic capture is not a Tom visual sign-off gate for Phase A. The Phase A gate remains the later 5-area build screenshots plus TimeWindow aperture check after light-shadow rig handoff, Painted Overlay removal, renderer-policy change, and shader lightening.

## Build Artifact For Tom

Build exe path for review:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。

## Gate Note

変更を適用しました。参考画像とのギャップは、まだ Main Directional の所有権移管段階のため、god rays / 太陽盤 / Painted Overlay 削除後の実画面 / 暖寒対比 / エミッシブ表現が未実装である点です。Tom 判定は Phase A の 5 エリア capture 完了時にお願いします。
