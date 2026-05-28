# feat(hd2d): wire phase a sun cycle into house slice scene

Date: 2026-05-28 JST

## Scope

- Phase A Step 1/2 scene wiring from `docs/HD2D_SUN_CYCLE_SPEC.md`.
- Added generated HouseSlice scene objects for `AnemoraSunCycleDriver` and same-scene `MapSunAnchor` area switching.
- Split `SunPresetData` and `MapSunAnchor` into their own script files so Unity MonoScript GUID serialization resolves preset assets and scene components.
- Kept existing lighting handoff, Painted Overlay removal, 0.35s policy removal, and shader changes for later Phase A cycles.

## Cycle Worker

SCOPED_PROMPT_ISSUED cycle=166 authored_file=Assets/Editor/AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseASunCycleSceneWiringBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseASunCycleSceneWiringCycle166ScreenshotsBatch

The cycle-worker explored the insertion point, then parent implementation completed the patch because scene wiring required deterministic integration with the existing HouseSlice generator and Unity asset GUID repair.

## Validation

Final parent runner execution:

- Runner log: `tools/logs/cycle-166-20260528-093044.log`
- Validate log: `Logs/cycle-166-20260528-093044-validate.log`
- Capture log: `Logs/cycle-166-20260528-093044-capture.log`
- Build log: `Logs/cycle-166-20260528-093044-build.log`
- Smoke log: `Logs/cycle-166-20260528-093044-smoke.log`
- Validate result: exit 0.
- Capture result: exit 0.
- Build result: exit 0.
- Smoke result: built exe launched for 24 seconds with `-batchmode -nographics`; scanned `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`; pattern count 0.

Earlier attempt:

- `cycle-166-20260528-090110`: first run stalled at the outer shell timeout after Unity reported missing `SunPreset_Morning.asset` as a typed `SunPresetData`. Root cause was broken MonoScript serialization: cycle165 preset assets had `m_Script: {fileID: 0}` until `SunPresetData.cs` was split out and committed with the GUID those assets reference.
- Unity also reported low disk space. `C:\Users\maro6\AppData\Local\Temp` entries older than one day were removed, freeing about 3.35GB. No repo tracked files or build outputs were deleted for that cleanup.

## Scene Wiring Evidence

- Generator call: `CreateHd2dPhaseASunCycleSceneWiring(currentAreas)` runs immediately after `CreateHd2dGlobalVolume()`.
- Driver object: `FastVS_HD2D_SunCycle`
- Driver references: `Directional Light`, `FastVS_HD2D_GlobalVolume`, four `Assets/Settings/SunCycle/SunPreset_*.asset`.
- Same-scene anchors:
  - `FastVS_HD2D_MapSunAnchor_Interior_Morning`
  - `FastVS_HD2D_MapSunAnchor_Exterior_Morning`
  - `FastVS_HD2D_MapSunAnchor_CentralPlaza_Noon`
  - `FastVS_HD2D_MapSunAnchor_Library_Evening`
- The validate batch reads `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` and greps serialized YAML for the SunCycle root, anchor names, driver references, preset GUIDs, `defaultPreset: 1`, and `transitionDuration: 1.8`.
- The scene file is intentionally not staged in this commit because it had a large pre-existing unrelated dirty diff before cycle166. The generator code and local YAML grep provide the reproducible scene asset evidence without committing unrelated scene churn.

## Review Artifacts

Local diagnostic output:

- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_sun_cycle_scene_wiring_cycle166/parent_review_01_current_house_interior_sun_cycle_morning.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_sun_cycle_scene_wiring_cycle166/parent_review_02_current_house_exterior_sun_cycle_morning.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_sun_cycle_scene_wiring_cycle166/parent_review_03_current_central_plaza_sun_cycle_noon.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_sun_cycle_scene_wiring_cycle166/parent_review_04_current_library_sun_cycle_evening.png`
- `docs/devlog/screenshots/fast_vs_hd2d_phase_a_sun_cycle_scene_wiring_cycle166/sun_cycle_scene_wiring_diagnostics.md`

Public curated review set:

- `docs/review/2026-05-28T09-30/01_house_interior_sun_cycle_morning.png`
- `docs/review/2026-05-28T09-30/02_house_exterior_sun_cycle_morning.png`
- `docs/review/2026-05-28T09-30/03_central_plaza_sun_cycle_noon.png`
- `docs/review/2026-05-28T09-30/04_library_sun_cycle_evening.png`
- `docs/review/2026-05-28T09-30/index.md`
- `docs/review/2026-05-28T09-30/devlog.txt`

This diagnostic capture is not a Tom visual sign-off gate for Phase A. The Phase A gate remains the later 5-area build screenshots plus TimeWindow aperture check after existing light handoff, Painted Overlay removal, renderer-policy change, and shader lightening.

## Build Artifact For Tom

Build exe path for review:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。

## Gate Note

変更を適用しました。参考画像とのギャップは、まだ scene wiring 段階のため、god rays / 太陽盤 / 暖寒対比 / エミッシブ表現が未実装である点です。Tom 判定は Phase A の 5 エリア capture 完了時にお願いします。
