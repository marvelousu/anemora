# Stage 4 graphics automation doc refresh

Date: 2026-05-08
Scope: GFX-0 / GFX-3 documentation and verification hygiene
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Refreshed the editor automation documentation after adding the portal flash VolumeProfile maintenance path to `Stage4GraphicsBaselineCapture`.

## Changes

- `docs/EDITOR_AUTOMATION.md`
  - Adds the `Anemora/Review/Create Portal Flash Volume Profile` menu entry.
  - Adds the batchmode invocation for `Stage4GraphicsBaselineCapture.CreateOrUpdatePortalFlashVolumeProfileAsset`.
  - Lists `Assets/Settings/Portal/PortalFlash_VolumeProfile.asset` as an output of the graphics baseline/profile maintenance tool.
  - Clarifies that the tool now maintains review/profile artifacts, not only screenshot outputs.

## Verification

- Documentation-only change.
- `git diff --check` passed before staging.

## Caveats

- No runtime asset or scene changes in this pass.
