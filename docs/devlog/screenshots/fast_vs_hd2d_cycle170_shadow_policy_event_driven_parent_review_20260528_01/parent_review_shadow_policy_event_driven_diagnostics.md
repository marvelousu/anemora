# HD2D Phase A Shadow Policy Event-Driven Diagnostics

- Source: `Assets\Scripts\FastVS\FastVsRealtimeLightShadowRig.cs`
- Area transition source: `Assets\Scripts\FastVS\FastVsHouseAreaVisibility.cs`
- Validate entry: `ValidateHd2dPhaseAShadowPolicyEventDrivenBatch`
- Capture entry: `CaptureHd2dPhaseAShadowPolicyEventDrivenCycle170ScreenshotsBatch`
- Grep: Rig source contains sceneLoaded subscription/unsubscription, area transition shadow policy method, and force-path review pass.
- Grep: Area visibility source calls the rig's area transition shadow policy method from `ApplyVisibility`.
- Grep: Rig source no longer contains `ShadowPolicyRefreshSeconds`, `nextShadowPolicyRefreshTime`, or `Time.unscaledTime >=` refresh-loop tokens.
- Remaining ownership: `LateUpdate` still resolves references and applies light/sky; renderer shadow policy now runs on scene load, area transition, or explicit review force.
