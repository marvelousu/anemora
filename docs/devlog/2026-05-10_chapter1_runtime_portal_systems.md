# 2026-05-10 Chapter 1 Runtime / Portal Systems

## Scope

担当範囲は Chapter 1 runtime systems の #10 / #12 / #13 / #14。

- TimeFramePortalSystem v3.2 local-window mode
- Symbol Wheel 1 周目 Red-only / Blue progression unlock
- scene 1 [1.F] book reflection hook
- scene 4 [4.D] [4.G] story auto-trigger hook

DialogueAsset / StringTable / map prefab integration / scene assembly / graphics assets / character generation は触っていない。

## Implemented

### Progression / Symbol Wheel

- `PlayerProgressionFlag.BlueTimeSymbolUnlocked` を追加。
- `SaveEnvelope.progressFlags.rawFlags` に typed progression flag helper を追加。
- `PlayerProgressionRuntime` を追加し、save DTO との load / export と runtime notification を担当。
- `SymbolWheelController` は White を hidden、Blue を preview/disabled、flag unlock 後だけ selectable に変更。
- `Assets/UI/Prefabs/SymbolWheel.prefab` は White symbol を inactive、Blue preview enabled / locked に更新。

### Time Window v3.2

- `TimeWindowBoundaryContext` (`Outdoor` / `Interior` / `Ruin`) を追加。
- `PortalCrossingDetector.IsInsideBoundary()` を追加し、local window footprint 判定を detector 系へ集約。
- `TimeFramePortalController.RequestAutoTrigger(TimeFrameAutoTriggerRequest)` を追加。
- `TimeFrameStoryAutoTrigger` を追加し、player position / required flag / once-only / request window bounds を controller に送れるようにした。
- `TimeWindowDiorama` は boundary context を保持し、local-window clone 内の `PastActionInteractable` を active action として扱う。
- past NPC は既定で local window interaction から除外。

### ActionRecord reflection

- `ActionType.Touch` / `ActionType.Push` を追加。
- `PastActionInteractable` を追加し、touch / take / push を `ActionRecordEntry` として記録可能にした。
- `BookReflector` は既存 prefab spawn に加え、`Book_Family_Current` のような serialized existing object activation をサポート。
- `GameObjectVisibilityReflector` を追加し、scene 4 用に複数 GameObject の active state を ActionRecord から切替可能にした。

### Auto-trigger monologue hook

- `NiroMonologueController` に `storyAutoTriggerDialogue` serialized hook と `TryShowStoryAutoTriggerReaction()` を追加。
- controller の `AutoTriggerRequested` event から hook できる。
- placeholder/localize key の確定は Dialogue / StringTable セッションへ引き継ぎ。

## Tests

- Unity batchmode compile/import smoke: success.
- EditMode: `36/36` passed.
- PlayMode: `32/32` passed.

Logs:

- `Logs/codex_compile_smoke.log`
- `Logs/codex_editmode_results.xml`
- `Logs/codex_playmode_results.xml`

## Scene Assembly Hook Points

- `SymbolWheelController.progressionRuntime`: 任意。未設定なら `PlayerProgressionRuntime.Instance` を解決。
- `PlayerProgressionRuntime`: scene root へ配置し、save load 時に `LoadFromSaveData()` / `ToSaveData()` を呼ぶ。
- `TimeFramePortalController.defaultLocalWindowContext`: scene の既定 boundary context。
- `TimeFrameStoryAutoTrigger`: scene 4 [4.D] [4.G] の trigger volume GameObject に配置。
- `TimeFrameStoryAutoTrigger.requestId`: once-only の識別子。
- `TimeFrameStoryAutoTrigger.requiredRawFlag` / `completedRawFlag`: state flag gating / consume persistence 用。
- `TimeFrameStoryAutoTrigger.windowCenterOverride` / `windowCenterOffset` / `windowSize` / `boundaryContext`: 自動発動する窓の bounds。
- `NiroMonologueController.storyAutoTriggerDialogue`: `(...筆が、反応している)` 用 dialogue hook。localize key は別セッション。
- `BookReflector.reflectedBookObject`: scene 1 [1.F] で `Book_Family_Current` を直接 active 化する場合に設定。
- `GameObjectVisibilityReflector.visibilityEntries`: scene 4 [4.F] など複数痕跡の current-side active state。
- `PastActionInteractable.actionId` / `targetObjectId` / `actionType`: scene 4 touch / take / push 用 ActionRecord hook。

## Notes

- `docs/SPEC.md` は runtime worktree に存在しなかったため未更新。
- 新 ADR は既存ファイル名形式に合わせて `docs/adr/0010-time-window-mode-v3-2.md` として追加した。
- `Anemora_Main.unity` は編集していない。
- commit / push / PR は未実施。
