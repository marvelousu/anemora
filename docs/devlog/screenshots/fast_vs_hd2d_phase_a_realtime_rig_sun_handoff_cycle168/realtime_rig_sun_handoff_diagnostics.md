# HD2D Phase A Realtime Rig Sun Handoff Diagnostics

- Source: `Assets\Scripts\FastVS\FastVsRealtimeLightShadowRig.cs`
- Validate entry: `ValidateHd2dPhaseARealtimeRigSunHandoffBatch`
- Capture entry: `CaptureHd2dPhaseARealtimeRigSunHandoffCycle168ScreenshotsBatch`
- Grep: Rig source has no `mainLight.intensity`, `mainLight.shadowStrength`, `mainLight.color`, `mainLight.transform.rotation`, `mainLight.cookie`, `mainLight.cookieSize`, `mainLight.cookieSize2D`, `EnsureCentralPlazaSunCookieTexture`, `EnsureExteriorSunCookieTexture`, `IsRuntimeDirectionalCookie`, `RenderSettings.ambientMode`, `RenderSettings.ambientLight`, or `RenderSettings.fog` token.
- Remaining ownership: Rig keeps realtime shadow bias/resolution setup; `AnemoraSunCycleDriver` owns sun appearance and ambient/fog state.
