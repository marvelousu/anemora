# 2026-05-18 Fast VS Library Expansion, Debug Label Cleanup, and Dot Font Candidates

## Request

- Expand the library map a little more.
- Remove the mysterious white object visible above the plaza map.
- Keep `DotGothic16` as the current best font direction, but present several more dot-like Japanese font options.
- Exclude the previous normal-looking candidates:
  - `BIZ UDPGothic`
  - `Noto Sans JP`
  - `M PLUS 2`
- Treat `PixelMplus12` as too hard to read for the current dialogue UI.

## Implementation Cycle

- Planned the work locally.
- Sent a narrow implementation instruction to a `gpt-5.4-mini` worker:
  - Own only `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
  - Remove debug world labels from the route maps.
  - Expand the library map footprint and validation.
- Reviewed the worker result in the main session.
- Added a main-session correction to remove the remaining house interior/exterior debug world labels as well, because they used the same white overhead `TextMesh` pattern.

## Map Changes

Updated:

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`

Library:

- Expanded `Library_PixelFloor` to `16.6 x 11.8`.
- Moved/expanded the back wall and side wall hints to match the larger room.
- Adjusted library boundary guards around the new footprint.
- Kept `LibraryVsCenter` unchanged so the route and V24 same-coordinate contract remain stable.
- Added validation that current/past library floors are at least `16.0 x 11.0` and that `Library_BackWall` exists.

White object cleanup:

- Removed `CreateWorldLabel` calls from route map generation.
- Removed the remaining `PAST/CURRENT HOUSE INTERIOR` and `PAST/CURRENT HOUSE EXTERIOR` labels.
- Added validation that the old debug label GameObjects do not remain.

## Font Preview Update

Updated:

- `docs/font_preview/anemora_font_preview.html`

Added local preview fonts:

- `KH Dot Dougenzaka 16`
- `KH Dot Akihabara 16`
- `KH Dot Kodenmachou 16`
- `Fusion Pixel 12 JA`
- `k8x12`

Kept:

- `DotGothic16` as the baseline and default preview font.
- `PixelMplus12` only as a rejected reference.

Removed from the candidate set:

- `BIZ UDPGothic`
- `Noto Sans JP`
- `M PLUS 2`

Current direction:

- Best next candidate to compare against `DotGothic16`: `KH Dot Dougenzaka 16`.
- `Fusion Pixel 12 JA` is more strongly pixelated, but likely needs larger dialogue text.
- `k8x12` is included as an extreme low-resolution reference, not as a likely default dialogue font.

Font sources:

- DotGothic16:
  https://github.com/fontworks-fonts/DotGothic16
- KH Dot Font:
  http://jikasei.me/font/kh-dotfont/
- KH Dot WebFontSearch license cross-check:
  https://web-font-search.net/kh%E3%83%89%E3%83%83%E3%83%88%E3%83%95%E3%82%A9%E3%83%B3%E3%83%88-16%E9%81%93%E7%8E%84%E5%9D%82/
- Fusion Pixel Font:
  https://github.com/TakWolf/fusion-pixel-font
- k8x12:
  https://littlelimit.net/k8x12.htm

## Verification

- `git diff --check` passed for the edited setup file and font preview HTML.
- Browser preview loaded at:
  `http://127.0.0.1:8765/docs/font_preview/anemora_font_preview.html`
- Browser font loading check confirmed local candidates loaded:
  - `KH Dot Dougenzaka 16`
  - `KH Dot Akihabara 16`
  - `KH Dot Kodenmachou 16`
  - `Fusion Pixel 12 JA`
  - `k8x12`
  - `PixelMplus12`
- Browser console had no relevant errors or warnings; only missing `favicon.ico`.
- Unity batch generation and validation passed via `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`.
- Player build succeeded:
  `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 18-second `-batchmode -nographics` smoke run produced no error, exception, crash, or `NullReference` log entries.

Known non-fatal batch warnings:

- Unity licensing access-token update warning.
- `RenderTexture.Create failed` warnings under `-nographics`.
