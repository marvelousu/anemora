# 2026-05-06 Stage 4 Brush Tutorial Hint

## Summary

Stage 4 Phase 0 adds a small runtime brush-control hint for the accepted local time-window flow.

The Stage 3 closeout confirmed that `Shift` + left-drag now creates a window matching the preview footprint, but the action still depended on developer explanation. This pass keeps the core brush behavior unchanged and adds a lightweight overlay that makes the existing input affordance visible in the demo build.

## Implementation

- `TimeFramePortalController` now creates `BrushTutorialHintCanvas_Runtime` at runtime when local diorama brush input is enabled.
- The hint renders as a screen-space overlay below the dialogue canvas sorting order.
- The text changes with controller state:
  - Normal: `[Shift] + drag: draw time window`
  - Drag preview: `Release: place time window`
  - Open window: `Right-click: close time window`
- The hint hides while dialogue is visible and does not create scene YAML changes.
- Existing brush preview, quick placement, right-click close, portal generation, and dialogue guards are unchanged.

## Validation Added

`DemoPlayableSmokeTests` now checks:

- The runtime hint canvas exists in `Anemora_Main`.
- The hint is below `DialogueCanvas` sorting order.
- The initial hint exposes `Shift`.
- The open-window hint exposes `Right-click`.
- The drag preview hint exposes `Release`.

## Verification

- Targeted PlayMode `DemoPlayableSmokeTests`: `2/2` passed.
- Full EditMode: `32/32` passed.
- Full PlayMode: `29/29` passed.
- Windows Standalone build: success.
- Build output: `C:\Users\maro6\Documents\Unity\Anemora-stage4-brushux\Builds\Stage4BrushUX\Anemora_Stage4_BrushUX.exe`.

The first implementation used runtime TMP text and produced a TMP missing-font warning during hint creation. The final implementation uses legacy uGUI `Text` with a runtime font fallback, and the targeted PlayMode rerun no longer reports `BrushTutorialHintText_Runtime` / `LiberationSans` warnings.

## Scope Notes

This is an initial affordance pass, not a final HUD design. Stage 4 UI review can later replace the temporary text overlay with a more polished icon-based or localized tutorial treatment.

No scene, prefab, localization table, or asset-ledger changes were made in this pass.
