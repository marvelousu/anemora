# 2026-05-05 G3 Resident A/B Aseprite Finish Draft

## Summary

- Input drafts:
  - `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/front_v1.png`
  - `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/back_v1.png`
  - `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/left_v1.png`
  - `Assets/Art/Sprites/NPC/Resident_B/v1/_draft/seated_v1.png`
- Palette: `Assets/Art/anemora_palette_v0.gpl`
- Aseprite executable was not available on PATH, so this pass used deterministic local palette compression and simple 4-frame sheet assembly.
- Status: draft/v1 review stop. Resident age contrast and Resident_B darkness remain user-review items.

## Outputs

| Asset | Size | Frames | Palette colors | Outside palette |
|---|---:|---:|---:|---:|
| `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_idle.png` | 128x48 | 4 | 12 | 0 |
| `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_walk_front.png` | 128x48 | 4 | 12 | 0 |
| `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_walk_back.png` | 128x48 | 4 | 11 | 0 |
| `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_walk_left.png` | 128x48 | 4 | 13 | 0 |
| `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_walk_right.png` | 128x48 | 4 | 13 | 0 |
| `Assets/Art/Sprites/NPC/Resident_B/v1/resident_b_idle.png` | 128x48 | 4 | 7 | 0 |

## Finish Notes

- Resident_A uses the front/back/left drafts as source; right walk was generated as a mirrored derivative of left for F4 convenience.
- Resident_A idle has a small tired upper-body sway. Walk frames use restrained 1-pixel offsets and should be considered motion-blocking, not final hand animation.
- Resident_B's PixFlux draft had 68 original colors and a darker rendering style. It was compressed to moss grey / dark trouser / weathered stone colors from palette v0, leaving 7 opaque palette colors.
- Resident_B idle is almost still, with a small seated breathing shift.

## Unity Import

- Main project batchmode import could not run because another Unity instance had the project open.
- Import settings were generated in a temporary Unity project at `%TEMP%/AnemoraF2ImportProject` and copied back for the output PNG metas.
- Meta settings: Texture Type Sprite, Sprite Mode Single, PPU 32, Point filter, no mipmaps, uncompressed texture compression, alpha transparency enabled.
- The 4-frame sheets are currently imported as single sheet textures. F4/G wiring should slice them as 32x48 horizontal frames when creating clips.

## Review Holds

- Resident_A age contrast against the protagonist needs user judgment.
- Resident_A front/back/left clothing continuity should be reviewed before animator integration.
- Resident_B's dark, inward-looking silhouette reads clearly after palette compression, but may still be too visually separate from Resident_A. User review decides whether to lighten or keep.
- Palette v0 was sufficient for this pass; no v0.1 palette addition was required.
