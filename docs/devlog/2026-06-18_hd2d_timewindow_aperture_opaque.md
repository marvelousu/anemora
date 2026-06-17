# HD2D timewindow aperture opaque

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-18

## Context

- The latest built-player review showed the TimeWindow aperture still reading as transparent: the current side and the old white haze could remain visible inside the window.
- The renderer feature set remains frozen; this cycle does not add, remove, or reorder URP renderer features.
- The fix stays scoped to the TimeWindow aperture path, its review shader, and the authored validation guard.

## Change

- Pinned the portal aperture composite alpha to `1.0` so the aperture no longer blends the current side through the target-time render.
- Changed `PortalApertureOverlay` to render as a late opaque portal surface with `Blend One Zero`, `ZWrite On`, and `ZTest Always`.
- Raised the runtime aperture material queue to `3040` so later current-side transparent overlays do not sit on top of the aperture image.
- Removed the aperture visual-overlay exemption path so `Veil`, `Wash`, `AirDepth`, `Glow`, and similar intersecting current-side overlays are suppressed instead of surviving inside the window.
- Added validation for opaque alpha, late render queue, no visual-overlay exemption, and the shader blend/depth contract.

## Visual Review

- Accepted packet: `docs/review/2026-06-18T04-47_timewindow_aperture_opaque_r2/`.
- Contact sheet: `docs/review/2026-06-18T04-47_timewindow_aperture_opaque_r2/contact_sheet.png`.
- Direct proof image: `docs/review/2026-06-18T04-47_timewindow_aperture_opaque_r2/01_tw_current_aperture_opaque.png`.
- Capture source: `C:/Users/maro6/OneDrive/work/projects/anemora_reference/reference/20260525_stage7_portal_facade_brightness/tw_current_aperture.png`.

## Verification

- Validate: `Logs/aperture_opaque_validate_r5.log` passed with `Fast VS house slice validation passed.`, `Exiting batchmode successfully now!`, and `exemptedVisualOverlay=0` in the aperture suppression logs.
- Renderer freeze: `Logs/aperture_opaque_editmode_r3.xml` passed all 36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/aperture_opaque_asset_validation_r3.log` passed with `[AssetValidation] OK`.
- Capture: `Logs/aperture_opaque_capture_r4.log` updated the portal facade reference screenshots and logged `exemptedVisualOverlay=0`.
- Build: `Logs/aperture_opaque_build_r2.log` passed with `Fast VS house slice validation passed.`, `Build Finished, Result: Success.`, and rebuilt `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.
- Latest build timestamp: `2026-06-18 04:45:47 JST`.

## Next

- Continue the environment graphics uplift after this correctness fix: distant panorama and realistic natural assets remain the next visual priority.
- If the target-time side itself still reads too thin, treat that as a target-surface/material authoring pass rather than another current-side transparency fix.
