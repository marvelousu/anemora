# Resident_A P1 Production Sheet Spec

Date: 2026-05-07

Status: generation / review spec only. Do not runtime-import before user review.

## Accepted Direction

- Base direction: P1.
- Role: Past-side ordinary young town resident.
- Read: living-town warmth and everyday activity, not gloomy witness first.
- Relationship to Niro: no prior relationship.
- Avoid: guide, protagonist, mascot, fantasy job silhouette, overly iconic hair accessory, oversized face/head.

## Runtime Constraints

- Cell size: 32 x 48 px.
- Sheet size per motion: 128 x 48 px, 4 horizontal frames.
- Pivot/import reference: bottom center, PPU 32, no mipmaps, point filtering.
- Scale reference:
  - Hero v2 first-cell bbox: 19 x 45.
  - Resident_B v2 first-cell bbox: 25 x 45, seated/darker exception.
  - Current Resident_A v2 first-cell bbox: 20 x 45, rejected as face/head too large.
  - Accepted P1 first-cell bbox: 16 x 45.

## Required Review Output

Create a review sheet before any runtime import:

- Front idle, 4 frames.
- Walk front, 4 frames.
- Walk back, 4 frames.
- Walk left, 4 frames.
- Walk right, 4 frames.
- Comparison against Hero v2, Resident_B v2, current Resident_A v2, and P1 approved cell at 5x nearest-neighbor scale.

## Visual Invariants

- Keep P1's short/medium brown hair and warm ordinary-town clothing.
- Keep a modest apron / daily-life silhouette.
- Keep the head visibly smaller than current Resident_A v2 and no larger than Hero read.
- Keep pixel granularity close to Hero v2 / Resident_B v2, not the overly chunky prior Resident_A v2.
- Use warm Past-side palette, but not saturated heroine colors.
- Side/back views should still read as the same person without adding large accessories.

## Generation Prompt Draft

Create a pixel-art game sprite sheet for one character: Resident_A from Anemora, based on the accepted P1 direction.

The character is an ordinary young girl or young town resident from the living Past side of a quiet ruined-town time-shift game. She should read warm, local, and everyday, with short-to-medium brown hair, a modest cream shirt, brown vest, simple apron, reddish-brown skirt, and small boots. She is not a hero, guide, mascot, fantasy class, or magical character. Keep her head modest in scale, smaller than the rejected current Resident_A and comparable to the accepted Hero v2 scale.

Produce a transparent-background pixel-art sheet with five rows:

1. front idle, four frames
2. walk front, four frames
3. walk back, four frames
4. walk left, four frames
5. walk right, four frames

Each frame must fit a 32 x 48 pixel cell. The complete sheet should align as a 4 column x 5 row grid. Keep consistent pixel granularity, crisp nearest-neighbor style, no anti-aliased blur, no text, no labels, no shadows, no ground plane, no background, and generous transparent padding inside each cell. Use the same character proportions and outfit in every direction.

Avoid large eyes, chibi proportions, oversized face, detached head/neck, ornate accessories, fantasy costume, glowing effects, modern clothing, and gloomy current-side mood.
