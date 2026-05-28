# HD2D Phase A Painted Overlay Removal Diagnostics

- Source: `Assets\Scripts\FastVS\FastVsRealtimeLightShadowRig.cs`
- Validate entry: `ValidateHd2dPhaseAPaintedOverlayRemovalBatch`
- Capture entry: `CaptureHd2dPhaseAPaintedOverlayRemovalCycle169ScreenshotsBatch`
- Grep: Rig source has no Cycle128/Cycle131 camera overlay helper, activation, or legacy painted overlay suppression token.
- Remaining ownership: Rig keeps realtime shadow policy and sun handoff behavior; the old painted camera overlay runtime block is removed.
