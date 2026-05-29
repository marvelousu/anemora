# HD2D Phase A Main Directional Handoff Diagnostics

- Source: `Assets\Scripts\FastVS\FastVsHouseLightingDirector.cs`
- Validate entry: `ValidateHd2dPhaseAMainDirectionalHandoffBatch`
- Capture entry: `CaptureHd2dPhaseAMainDirectionalHandoffCycle167ScreenshotsBatch`
- Grep: Director source has no `ApplyMainLight(`, `mainLight.intensity`, `mainLight.shadowStrength`, `mainLight.color`, `mainLight.transform.rotation`, `mainLight.cookie`, or `mainLight.cookieSize` token.
- Remaining main-light ownership: `AnemoraSunCycleDriver` controls Directional Light rotation/color/intensity/cookie.
