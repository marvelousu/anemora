# 2026-05-18 Fast VS Plaza Rebalance and Font Preview Tool

## Request

- Expand the central plaza floor a little more.
- Reduce the wasted space on the front/south side of the plaza.
- Keep the fountain ruin around the middle of the floor-covered area so the back/north side does not feel cramped.
- Fix the remaining issue where the player can still stand on the fountain ruin.
- Show the font candidates through a preview tool.

## Implementation

- Expanded `CentralPlaza_PixelGround` from `16.2 x 21.4` to `17.8 x 21.8`.
- Back-shifted the plaza ground by `+2.35` on local Z to move usable visual space toward the library side.
- Moved the front invisible boundary inward from local Z `-9.54` to `-7.45`.
- Moved the back invisible boundary outward to local Z `+13.35`.
- Recentered the left/right invisible boundaries at local Z `+2.95` so their coverage matches the new front/back span.
- Moved the fountain ruin to local Z `+2.70`, closer to the middle of the revised floor instead of leaving it near the old center.
- Removed the visible fountain base collider by setting `keepCollider = false`.
- Enlarged the invisible `CentralPlaza_FountainNoStepCollider` to `2.25 x 1.60 x 2.25`, positioned through the player body instead of only near the fountain top.
- Tightened validation so the fountain no-step collider must be invisible and large/tall enough to block traversal.

## Font Preview

- Added local preview tool:
  `docs/font_preview/anemora_font_preview.html`
- Preview compares:
  - `BIZ UDPGothic`
  - `Noto Sans JP`
  - `M PLUS 2`
  - `DotGothic16`
- The preview uses a dialogue-window sample, location label, choices, and side-by-side candidate cards.
- A temporary local server was started for browser preview:
  `http://127.0.0.1:8765/docs/font_preview/anemora_font_preview.html`

Current recommendation remains:

- Main dialogue/UI: `BIZ UDPGothic`
- Fallback/general coverage: `Noto Sans JP`
- Accent/title only: `DotGothic16`

## Verification

- Worker implemented the first plaza adjustment pass.
- Main review fixed a side-boundary coverage gap after the front/back rebalance.
- `git diff --check` passed for the edited setup file and preview HTML.
- Unity batch generation and validation passed via `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`.
- Player build succeeded:
  `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 18-second `-batchmode -nographics` smoke run produced no error, exception, crash, or `NullReference` log entries.
- Browser preview loaded via local HTTP server; only `favicon.ico` produced a harmless 404.

Known non-fatal batch warnings:

- Unity licensing warning during batchmode startup.
- `RenderTexture.Create failed` warnings under `-nographics`.
