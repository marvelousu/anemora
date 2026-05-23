# 2026-05-23 Fast VS HD2D Shadow Response Foundation Cycle 65

## Scope

- Project: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Setup source: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Sprite audit: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dSpriteCardLightingAudit.cs`
- Worker: `gpt-5.4-mini` subagent `019e5236-f63d-74d1-b73d-7ca8aaeb94b0`

Cycle65 starts the broad shadow/shading foundation pass requested after Cycle64. The goal is not to add more props, but to make the existing HD-2D material stack respond more decisively to shadow while keeping the previous close-review stability.

## Implementation

Retuned the shared generated material values:

- `SpriteCardWorldShadowReceiveStrength`: `0.07` to `0.11`
- `SurfaceRampShadowReceiveStrength`: `0.26` to `0.30`
- `SurfaceRampSideShade`: `(0.95, 0.98, 1.03)` to `(0.91, 0.94, 1.00)`
- `SurfaceRampFloorShade`: `(0.92, 0.94, 0.97)` to `(0.86, 0.89, 0.94)`
- `hd2d_depth_shadow` alpha: `0.12` to `0.17`
- `hd2d_outdoor_occlusion_gradient` alpha: `0.24` to `0.28`
- `FastVS_House_surface_directional_shade_overlay_soft.png` alpha was strengthened and capped at `0.30`

Updated validation bands so the stronger shadow response is now the expected baseline:

- Surface ramp `_ShadowReceiveStrength`: `0.29-0.31`
- Sprite card `_WorldShadowReceiveStrength`: `0.10-0.12`
- Surface directional shade overlay center alpha: `0.17-0.21`
- Surface directional shade overlay max alpha: `0.27-0.31`
- Sprite card audit max for `_WorldShadowReceiveStrength`: `0.13`

## Verification

Structural validation:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle65_shadow_response_validate_parent_20260523.log`
- Result: `Fast VS house slice validation passed.`
- Confirmed pass lines for material role, sprite card lighting, shading foundation, area lighting profile, overlay profile, surface profile, surface texture metric, and lighting transition audits.

Visual snapshot audit:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle65_shadow_response_visual_snapshot_parent_20260523.log`
- Result: `Fast VS HD2D visual snapshot audit passed: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_visual_snapshot_audit_cycle10_20260522`

Close-review capture:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle65_shadow_response_close_review_parent_20260523.log`
- Result: `Fast VS close-review screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_close_review_20260520`

Cycle65 evidence copies:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle65_shadow_response_parent_review_20260523_01\visual_snapshot`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle65_shadow_response_parent_review_20260523_01\close_review`

MCP note:

- No Unity MCP resource or callable Unity MCP tool was exposed in this Codex session, so verification used Unity batchmode validation and deterministic screenshot capture.

## Result

The current/past surfaces, sprite cards, depth-shadow overlays, and directional shade overlays now share a stronger shadow response baseline. Parent review of the close-review screenshots shows clearer library floor shade, door recess darkness, and eave/occlusion separation without reintroducing the old black-card contact-shadow artifact.

Next cycles should apply this stronger foundation to usage-level problems: area-specific key/fill contrast, library interior shadow hierarchy, outdoor facade depth, and character-to-ground contact in motion.
