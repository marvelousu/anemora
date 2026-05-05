# Stress Sample Harness Design for Stage 4 Perf v1.0

Date: 2026-05-05

## 1. Purpose

Stage 3 performance baseline v0.2 recorded an audio-loaded 120s idle sample, but skipped stress sampling because there was no deterministic auto-input harness. This document and the accompanying skeleton define the Stage 4 harness shape for repeatable portal, symbol, and dialogue stress runs.

Primary goals:

- Repeat portal open / close / crossing actions deterministically.
- Repeat red symbol selection through the existing portal controller API.
- Trigger NPC dialogue through the existing interactable API without manual keyboard input.
- Collect FPS, frame-time histogram values, GC / heap samples, and URP warning counts in one result artifact.

Non-goals for this Stage 3 skeleton:

- No scene wiring in `Anemora_Main`.
- No production controller redesign.
- No committed stress baseline numbers.
- No replacement for the existing G5 performance v0.2 idle baseline.

## 2. Inputs

The Stage 4 activated runner should accept these inputs:

| Input | Current skeleton field | Notes |
| --- | --- | --- |
| Scene | `Anemora_Main` by scene placement | Skeleton is a `MonoBehaviour`; Stage 4 can place it in scene or a temporary bootstrap scene. |
| Duration | `durationSeconds` | Default 30s in skeleton; Stage 4 perf v1.0 should use 120s or 300s depending on matrix target. |
| Frequency / pacing | `stepDelaySeconds` | Skeleton waits a fixed delay between actions. Stage 4 can reinterpret this as `frequencyHz = 1 / stepDelaySeconds`. |
| Portal target | `TimeFramePortalController portalController` | Auto-resolves with `FindFirstObjectByType` if not assigned. |
| Player target | `Transform player` | Auto-resolves `GameObject.FindWithTag("Player")`. Used to move near dialogue interactables. |
| Dialogue targets | `MonoBehaviour[] dialogueInteractables` | Auto-resolves components whose type name is `Anemora.Dialogue.NpcInteractable`. Reflection avoids adding asmdef coupling. |
| Output path | `outputFileName` | Relative paths write under `Application.persistentDataPath`; absolute paths are supported. |

## 3. Action Sequence

The skeleton action cycle is:

1. Open portal by calling `TimeFramePortalController.HandleSymbolSelected(SymbolType.Red)`.
2. Wait `stepDelaySeconds`.
3. Trigger portal crossing by calling `TimeFramePortalController.TriggerCrossingForTests()` when enabled.
4. Wait `stepDelaySeconds`.
5. Move player near the first resolved NPC interactable and invoke `TryInteract()` by reflection when enabled.
6. Wait `stepDelaySeconds`.
7. Close portal by calling `TimeFramePortalController.ClosePortal()` when enabled.
8. Wait `stepDelaySeconds`.
9. Repeat until `durationSeconds` is reached.

This uses existing public APIs only. The only "test API" currently used is `TriggerCrossingForTests()`, which already exists in `TimeFramePortalController` and was used by earlier PlayMode tests.

## 4. Outputs

The skeleton writes a JSON-like result file with:

| Output | Source |
| --- | --- |
| `durationSeconds` | Runner wall-clock sample duration. |
| `frameCount` | Number of sampled frames during wait windows. |
| `averageFps` | `frameCount / durationSeconds`. |
| `averageFrameMs` | Average `Time.unscaledDeltaTime`. |
| `p95FrameMs` / `p99FrameMs` / `maxFrameMs` | Sorted frame-time samples. |
| `gcUsedMemoryStartMiB` / `gcUsedMemoryEndMiB` / `gcUsedMemoryPeakMiB` | Unity `ProfilerRecorder` memory counter. |
| `totalUsedMemoryPeakMiB` | Unity `ProfilerRecorder` memory counter. |
| `monoHeapPeakMiB` | Unity `ProfilerRecorder` `Mono Used Memory` counter if available. |
| `urpDrawObjectsWarningCount` | Counted through `Application.logMessageReceived` for the known `DrawObjectsPass` warning string. |
| `portalOpenCount` / `portalCloseCount` / `portalCrossingCount` | Runner action counters. |
| `dialogueTriggerCount` / `dialogueTriggerSuccessCount` | Runner action counters. |

External Stage 4 sampling should still capture process-level working set, private bytes, GPU dedicated/shared memory, and player log size, as v0.2 did.

## 5. Skeleton Files

| File | Purpose |
| --- | --- |
| `Assets/Scripts/PerformanceHarness/StressSampleRunner.cs` | Compileable Stage 4 runner scaffold. |
| `Assets/Tests/PlayMode/StressSampleRunnerSmokeTests.cs` | Smoke test that verifies the runner starts, stops, and can build a minimal result without scene wiring. |

The skeleton deliberately avoids direct references to `NpcInteractable` and `DialogueDisplay` to reduce assembly-definition coupling. It detects NPC interactables by runtime type name and invokes `TryInteract()` by reflection.

## 6. Stage 4 Activation Plan

1. Create a temporary perf worktree from the Stage 4 target commit.
2. Add a temporary bootstrap scene or place `StressSampleRunner` under a disabled/Editor-only performance harness root in `Anemora_Main`.
3. Assign `portalController`, `player`, and the intended `Resident_A/B` interactables explicitly.
4. Set `durationSeconds = 120` for parity with v0.2, or `300` for the full Stage 4 perf v1.0 pass.
5. Set `stepDelaySeconds` based on the desired action frequency. Example: `1.0` = one action group roughly every four seconds.
6. Run a Windows Standalone player with `-logFile` and a known output path.
7. Pair the runner JSON with external process/GPU counter sampling.
8. Record result rows in the Stage 4 perf devlog and G5/Stage 4 acceptance matrix.

## 7. Risks / Follow-ups

- `TriggerCrossingForTests()` is acceptable for deterministic stress measurement, but Stage 4 should decide whether to rename or wrap it as a formal harness API.
- Dialogue success depends on dialogue panel state; if the panel remains open, repeated `TryInteract()` calls can return false until the dialogue is advanced or closed. Stage 4 should add an explicit `DialogueDisplay.Close()` hook or an auto-advance action.
- Portal close immediately after crossing may hide bugs in longer open-window traversal. Stage 4 should add scenario presets: idle open, rapid open/close, rapid crossing, dialogue repeat, and mixed flow.
- The current runtime warning count catches the known `DrawObjectsPass` warning, but Stage 4 should also parse the player log to detect any new warning classes.
- `ProfilerRecorder` counter availability differs between Editor and Player. Missing counters should be recorded as `0` or `unavailable` with tool notes in the Stage 4 devlog.

## 8. Acceptance for This Skeleton

- `StressSampleRunner` compiles without modifying existing production APIs.
- Smoke PlayMode test confirms the component can start and stop without scene wiring.
- No stress numbers are claimed in Stage 3.
- Stage 4 has a concrete activation path for perf v1.0.
