# Stage 4 Dialogue TMP Capture Investigation (2026-05-06)

Scope: UI/TMP readability capture investigation only. No character sprites, prefabs, scenes, audio, localization tables, font assets, or runtime code were modified.

## 1. Question

Stage 4 needs representative `DialoguePanel` screenshots with actual TextMeshPro glyphs visible before deciding on font, palette, or panel polish. A prior Editor `RenderTexture` capture produced the panel/background but no readable TMP text, so the capture path needs to be changed before visual decisions are made.

## 2. Repo Findings

- `DialogueDisplay` normalizes its parent canvas to `ScreenSpaceOverlay`, forces sorting order >= 1000, then writes TMP speaker/body text during `Show`.
- `DialoguePanel.prefab` contains three `TextMeshProUGUI` children for speaker, body, and advance indicator.
- `AnemoraE1ParallelSetup.CaptureE1Screenshots` uses a camera `RenderTexture` plus `RenderPipeline.SubmitRenderRequest`. That is appropriate for world/camera screenshots, but it is not a reliable representative path for overlay UI.
- Existing PlayMode tests already prove the dialogue UI can be shown and advanced, but they intentionally assert string state and do not perform rendered glyph capture.
- The current readability review already recommends JP/EN screenshots before changing UI assets: `docs/devlog/2026-05-06_stage4_tmp_palette_readability_review.md`.

## 3. Likely Failure Mode

Do not reuse the E1-style camera `RenderTexture` path blindly for dialogue readability capture.

`ScreenSpaceOverlay` canvases are composed by the Game View UI pipeline, not by a normal camera target. Even if a backing panel appears through a temporary setup, TMP can still be missing if the capture runs before canvas/TMP geometry rebuilds, before TMP material references are uploaded, or through a camera-only render request that bypasses overlay composition. That makes a camera `RenderTexture` screenshot a weak proxy for the actual player-visible result.

## 4. Recommended Capture Path

Use a PlayMode/Game View screenshot harness, not a camera-only render request:

1. Load `Anemora_Main` in PlayMode with a graphics device.
2. Set locale explicitly to `ja-JP` or `en` and wait for `LocalizationSettings.SelectedLocaleAsync`.
3. Trigger real dialogue through `NpcInteractable.TryInteract` or `DialogueDisplay.Show` using the existing scene instance and dialogue asset.
4. Advance to the target line when capturing specific Resident_A / Resident_B lines.
5. Wait at least one frame after text assignment.
6. Call `Canvas.ForceUpdateCanvases()`.
7. For every visible `TMP_Text` under `DialogueCanvas`, call `ForceMeshUpdate(true, true)` and assert `textInfo.characterCount > 0`.
8. Yield `WaitForEndOfFrame`.
9. Capture with `ScreenCapture.CaptureScreenshot` or `ScreenCapture.CaptureScreenshotAsTexture`, using Game View resolution presets for 1280x720 and 1920x1080.
10. Add a simple pixel/metadata gate: output file exists, file size is non-trivial, and the lower panel region has non-background high-luminance or edge-density pixels where speaker/body glyphs should be.

This path captures the same overlay composition the player sees and gives a deterministic place to wait for TMP rebuilds.

## 5. Screenshot Set

Minimum review sheet:

| Locale | Target |
|---|---|
| `ja-JP` | Resident_A line 1 |
| `ja-JP` | Resident_B longest current line |
| `en` | Resident_A line 3 |
| `en` | Resident_B line 2 |

Capture each target at 1280x720 and 1920x1080. Keep one additional shot with the brush hint visible and one with dialogue visible to confirm overlay hierarchy.

## 6. Implementation Recommendation

Add a small Editor/test utility only after agreeing where generated screenshots should live. The lowest-risk implementation shape is:

- A PlayMode test or Editor menu command that reuses the existing `NpcDialogueFlowTests` interaction pattern.
- Output under `docs/devlog/screenshots/dialogue_tmp_capture/`.
- No writes to `DialoguePanel.prefab`, localization tables, scene assets, or font assets.
- Optional generated screenshots should be committed only when they are intentional visual evidence for the Stage 4 readability decision.

Do not change `DialoguePanel.prefab` until this harness produces screenshots with visible TMP glyphs and the PlayMode dialogue tests still pass.

## 7. Verification

- Read `DialogueDisplay`, `DialoguePanel.prefab` references, current PlayMode dialogue tests, E1 screenshot capture code, editor automation docs, and the existing TMP/palette readability review.
- Confirmed the active project worktree was clean before investigation and performed this docs-only work in a separate temporary worktree.
- Unity was not run for this docs-only investigation because no production or Editor tool code was changed.

## 8. Decision State

Recommendation for the main orchestrator: schedule a follow-up capture-harness task before any UI prefab polish. The robust path is PlayMode/Game View capture with forced canvas/TMP rebuild and pixel gates; the camera `RenderTexture` path should remain limited to world/camera screenshots.

## 9. Revision History

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-06 | Initial capture-path investigation and recommendation |
