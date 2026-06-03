# HD-2D Pixel Font Pipeline

P0-19 establishes two TMP pixel-font roles for Chapter 1 review builds.

| Role | TMP asset | Source font | Rule |
|---|---|---|---|
| Body/dialogue | `Assets/UI/Localization/Fonts/Anemora_JP.asset` | DotGothic16 | native 16 px, runtime body text at 32 px |
| Display/title | `Assets/UI/Localization/Fonts/Anemora_EN.asset` | Press Start 2P | native 16 px, title/cue text at 48 px |

Asset rules:
- TMP creation metadata is stamped as RASTER_HINTED, native 16 px, padding 4.
- Atlas textures must be 512 px or larger, Filter Mode Point, and mip count 1.
- Runtime-readable materials keep outline, softness, and face dilation at 0.

Runtime scaling rules:
- Dialogue/menu canvases must be Screen Space Overlay or Screen Space Camera. World Space canvases are not allowed for pixel dialogue text.
- Canvas scaling uses Constant Pixel Size only.
- 1080p: 1x.
- 4K: 2x.
- On-screen text sizes must stay integer multiples of native 16 px: 16, 32, or 48.

Review rule:
- Each P0-19 review capture includes a 1080p dialogue frame, a 4K dialogue frame, and nearest-neighbor 400 percent crops from both.
