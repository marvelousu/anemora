# feat(hd2d): make realtime rig shadow policy event driven

Date: 2026-05-28 JST

## Scope

- Phase A Step 4 from `docs/HD2D_SUN_CYCLE_SPEC.md`.
- Removed the 0.35s renderer shadow policy refresh loop from `FastVsRealtimeLightShadowRig.cs`.
- Added an explicit area-transition notification from `FastVsHouseAreaVisibility` so renderer shadow policy is reapplied without a periodic scan.
- Kept shader lightening for the next Phase A cycle.

## Cycle Worker

SCOPED_PROMPT_ISSUED cycle=170 authored_file=Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAShadowPolicyEventDrivenBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseAShadowPolicyEventDrivenCycle170ScreenshotsBatch

Worker result:

- Authored file: `Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs`
- Side-effect file: `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- Scope widened by parent: `Assets/Scripts/FastVS/FastVsHouseAreaVisibility.cs` was included to satisfy the SPEC area-transition path.
- Validate method: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAShadowPolicyEventDrivenBatch`
- Capture method: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseAShadowPolicyEventDrivenCycle170ScreenshotsBatch`

## Implementation

- Removed `ShadowPolicyRefreshSeconds`, `nextShadowPolicyRefreshTime`, and the `Time.unscaledTime` refresh branch.
- `LateUpdate` now resolves references and applies light/sky only; it no longer calls the renderer shadow policy scan.
- `SceneManager.sceneLoaded` now triggers a one-time renderer shadow policy pass.
- `FastVsHouseAreaVisibility.ApplyVisibility` now notifies `FastVsRealtimeLightShadowRig` once after area activation.

## Source Evidence

`FastVsRealtimeLightShadowRig.cs` grep evidence:

| Token | Count |
|---|---:|
| `ShadowPolicyRefreshSeconds` | 0 |
| `nextShadowPolicyRefreshTime` | 0 |
| `Time.unscaledTime >=` | 0 |
| `Time.unscaledTime +` | 0 |

Required source hooks:

- `SceneManager.sceneLoaded += HandleSceneLoaded;`
- `SceneManager.sceneLoaded -= HandleSceneLoaded;`
- `ApplyRendererShadowPolicyForAreaTransitionForReview(FastVsHouseArea area)`
- `NotifyRealtimeShadowPolicyAreaTransition();`
- `ApplyRendererShadowPolicyForAreaTransitionForReview(activeArea);`

## Validation

Final runner:

- `tools/logs/cycle-170-20260528-104143.log`

Phases:

- Validate: exit 0
- Capture: exit 0
- Build: exit 0
- Smoke: exit 0

Smoke details:

- Built exe launched for 24 seconds with `-batchmode -nographics`.
- Pattern scan: `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`
- Pattern hits: 0
- Note: this smoke path bypasses runtime Update behavior, so dynamic/input behavior still needs real play diagnostics where relevant.

Earlier attempts:

- `cycle-170-20260528-103839` and `cycle-170-20260528-103931` exited early while a Unity batch process was still warming/importing the project. No rollback was performed; the final runner above is the validation record for this cycle.

## Local Review Artifacts

Devlog screenshots:

- `docs/devlog/screenshots/fast_vs_hd2d_cycle170_shadow_policy_event_driven_parent_review_20260528_01/parent_review_01_current_house_interior_sun_cycle_morning.png`
- `docs/devlog/screenshots/fast_vs_hd2d_cycle170_shadow_policy_event_driven_parent_review_20260528_01/parent_review_02_current_house_exterior_sun_cycle_morning.png`
- `docs/devlog/screenshots/fast_vs_hd2d_cycle170_shadow_policy_event_driven_parent_review_20260528_01/parent_review_03_current_central_plaza_sun_cycle_noon.png`
- `docs/devlog/screenshots/fast_vs_hd2d_cycle170_shadow_policy_event_driven_parent_review_20260528_01/parent_review_04_current_library_sun_cycle_evening.png`
- `docs/devlog/screenshots/fast_vs_hd2d_cycle170_shadow_policy_event_driven_parent_review_20260528_01/parent_review_shadow_policy_event_driven_diagnostics.md`
- `docs/devlog/screenshots/fast_vs_hd2d_cycle170_shadow_policy_event_driven_parent_review_20260528_01/parent_review_sun_cycle_scene_wiring_diagnostics.md`

Public curated review set:

- `docs/review/2026-05-28T10-42/01_house_interior_sun_cycle_morning.png`
- `docs/review/2026-05-28T10-42/02_house_exterior_sun_cycle_morning.png`
- `docs/review/2026-05-28T10-42/03_central_plaza_sun_cycle_noon.png`
- `docs/review/2026-05-28T10-42/04_library_sun_cycle_evening.png`
- `docs/review/2026-05-28T10-42/index.md`
- `docs/review/2026-05-28T10-42/devlog.txt`

The public review set contains only project captures. External reference images and comparison boards are intentionally kept out of `docs/review`.

## Build Artifact For Tom

Build exe path:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。

## Gate Status

- 変更を適用しました: 0.35s renderer shadow policy refresh branch is removed, and renderer shadow policy now runs through Awake/review force, sceneLoaded, and area-transition notification paths.
- 参考画像とのギャップは、まだ Phase A 完了前のため大きく残っています。Shader lightening, TimeWindow aperture check, final 5-area Phase A screenshots, and real-play dynamic verification remain pending.
- Tom 判定をお願いする段階ではまだありません。Phase A remaining stepを完了後、最終 Phase A gate として提示します。
