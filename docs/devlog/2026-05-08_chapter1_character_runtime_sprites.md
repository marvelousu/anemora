# 2026-05-08 Chapter 1 Character Runtime Sprites

## Summary

Prepared runtime-ready sprite PNGs for the Chapter 1 priority characters without opening Unity.

The work keeps the existing runtime contract:

- 32x48 px cell
- 128x48 px four-frame animation strips
- RGBA transparent PNG
- intended PPU 32 / Point filter / no mipmaps / prefab scale 1

## Chapter 1 Priority Characters

Generated runtime folders:

- `Assets/Art/Sprites/NPC/Resident_F/v1/` — Mia / ミア
- `Assets/Art/Sprites/NPC/Resident_C/v1/` — Kaia / カイア
- `Assets/Art/Sprites/NPC/Resident_D/v1/` — Dario / ダリオ
- `Assets/Art/Sprites/NPC/Resident_J/v1/` — Karla / カーラ
- `Assets/Art/Sprites/NPC/Resident_K/v1/` — Kairo / カイロ
- `Assets/Art/Sprites/NPC/Resident_L/v1/` — Luna / ルナ

Each folder contains:

- `*_stand.png`
- `*_idle.png`
- `*_walk_front.png`
- `*_walk_back.png`
- `*_walk_left.png`
- `*_walk_right.png`

## Additional Named Characters

Also generated the same runtime strip contract for remaining named Stage 4 characters:

- `Resident_E`
- `Resident_G`
- `Resident_H`
- `Resident_I`
- `Resident_R`
- `Robot_X`

## Reusable Background Characters

Generated idle-only runtime placeholders:

- `Robot_B1_01` through `Robot_B1_04`
- `Crowd_Past_01` through `Crowd_Past_08`
- `Crowd_Current_01` through `Crowd_Current_06`
- `Crowd_Future_01` through `Crowd_Future_05`

These include `stand` and four-frame repeated `idle` strips only.

## Review Assets

Review folders:

- `docs/review_gallery/imports/chapter1_runtime_sprites_2026-05-08/`
- `docs/review_gallery/imports/stage4_remaining_named_runtime_sprites_2026-05-08/`
- `docs/review_gallery/imports/stage4_background_character_runtime_sprites_2026-05-08/`

Review gallery was rebuilt with:

```powershell
python tools\build_review_gallery.py
```

Indexed image count after rebuild: 155.

## Reproduction Tools

Extraction tools:

- `tools/extract_chapter1_character_runtime_sprites.py`
- `tools/extract_stage4_remaining_named_runtime_sprites.py`
- `tools/extract_stage4_background_character_runtime_sprites.py`

## Validation

Performed local PNG validation:

- Chapter 1 priority: 36 PNGs generated, all expected sizes and RGBA.
- Remaining named: 36 PNGs generated, all expected sizes and RGBA.
- Background/reusable: 46 PNGs generated, all expected sizes and RGBA.
- Strong magenta chroma-key remnants were checked by pixel scan after reprocessing.

Unity was not opened in this session to avoid conflict with the graphics improvement session.

## Next Unity Step

The Unity-owning session should import these sprites, configure texture import settings, slice the 128x48 strips, duplicate existing biped Animator patterns, and create prefabs after user review of the contact sheets.

## 2026-05-09 DQ3R-Class Review Addendum

Added non-Unity review material for deciding whether Chapter 1 production should remain on the existing `32x48` runtime contract or move toward a higher-resolution HD-2D character baseline.

New review folders:

- `docs/review_gallery/imports/stage4_dq3r_character_review_2026-05-09/`
- `docs/review_gallery/imports/stage4_dq3r_master_sheets_2026-05-09/`
- `docs/review_gallery/imports/stage4_animation_framecount_review_2026-05-09/`

Generated review material:

- `32x48` readability sheets at `1x / 3x / 6x`
- transparent, Antela Current floor, Antela Past warm floor, and dark interior floor comparisons
- static walk/idle jitter metrics and annotated analysis sheets
- `64x96` and `96x144` master contacts for Chapter 1 priority characters
- runtime vs master comparison sheets at `1x / 2x / 4x / 6x`
- representative `4f / 6f / 8f` frame-count comparison sheets for Mia, Dario, and Luna
- crowd density previews for past/current/future placeholder characters
- diagonal-direction review source for Chapter 1 priority characters
- costume/detail polish candidate source for Chapter 1 priority characters

Static jitter findings:

- Karla / Resident_J: `resident_j_walk_back.png` needs review
- Kairo / Resident_K: `resident_k_walk_right.png` needs review
- Luna / Resident_L: `resident_l_walk_front.png` needs review

Graphics foundation handoff:

- `docs/review_gallery/imports/stage4_dq3r_character_review_2026-05-09/graphics_foundation_handoff.md`

Conclusion:

- `32x48` remains acceptable for prototype import.
- Dario, Kairo, Luna, and Karla should be reviewed seriously at `64x96` before locking Chapter 1 production visuals.
- Dario and Luna are the strongest candidates for real 6f/8f redraw if the project moves toward a DQ3R-class presentation.
- The costume/detail polish candidate is reference-only and should not replace runtime PNGs without user review.
