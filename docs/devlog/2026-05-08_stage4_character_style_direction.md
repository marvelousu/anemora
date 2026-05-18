# Stage 4 Character Style Direction

> Date: 2026-05-08
> Scope: character art direction review / production handoff
> Result: user selected the `radical01_watercolor_pixel_mix_03` direction as the next character-production baseline.

## 1. Decision

After the radical style review and the focused `radical01` refinement pass, the selected direction is:

- Approved source: `docs/review_gallery/imports/stage4_character_style_direction_accepted_2026-05-08/character_style_direction_radical01_watercolor_pixel_mix_03_approved.png`
- Original candidate: `docs/review_gallery/imports/stage4_radical01_refine_review_2026-05-08/radical01_watercolor_pixel_mix_03.png`
- Review set: `stage4_radical01_refine_review`

The user was strongly split between `radical01_main_refine_01` and `radical01_watercolor_pixel_mix_03`, then selected `03`.

## 2. Why This Direction

`03` keeps the practical readability of the chunky-pixel `01` direction while reducing the AI-polished character-sheet feel. The rougher line / watercolor-pixel mix reads more handmade and atmospheric, but still preserves enough silhouette and pixel structure to be a viable production baseline.

`01` remains the nearest fallback if later sprite extraction loses too much pixel readability. `02` and `06` remain useful references for shape simplification / hand-drawn warmth. `08` is not the main character-rendering style; keep it as an optional motif for interstitials, memory cuts, silhouettes, or UI / title-card experiments.

## 3. Locked Carry-Forward Notes

- Niro: use the latest `3/4` correction direction, not the earlier tall or bulky variants. Niro is a slim adolescent, same height as or slightly taller than Resident_A, clearly shorter than adult residents, with a low floppy hat hiding the eyes.
- Resident_B: use the darker rightmost seated mood from `residentb_dark_restoration_03`, not the cleaner brighter seated version. Long hair, slumped posture, heavy dark coat, face mostly in shadow.
- Past residents: keep warm, ordinary, living-town clothing. They should not become fantasy classes or post-collapse wanderers.
- Current / future residents: keep the darker post-collapse human clothing language: patched coats, hoods, wrapped scarves, frayed hems, faded satchels, no monster / raider / cyberpunk read.
- Runtime sprite style target: rough handmade line + moderate pixel clusters, less smooth than the previous v2 runtime sprites, but still readable at 32x48-ish gameplay scale.

## 4. Next Production Step

Do not crop production sprites directly from the approved cast sheet unless only a temporary mock is needed. The next art batch should generate dedicated production sheets in this selected style:

- Niro front / back / left / right + idle / walk cells.
- Resident_A rebuilt in the same style, with stable eyes and scale aligned against Niro.
- Resident_B dark seated idle sheet, with the rightmost dark seated mood preserved.

After user review of the dedicated production sheets, import as a new sprite pass instead of overwriting the current v2 sources blindly.

## 5. Verification

- Static review gallery regenerated with the approved copy included.
- No Unity scene, prefab, runtime asset, shader, or test file was changed in this decision record.
