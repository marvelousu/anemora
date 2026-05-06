# Stage 4 Resident_A Follow-Up Review (2026-05-06)

Scope: review-only character art follow-up after runtime visual review. No runtime sprites, prefabs, animation clips, scenes, asset ledger rows, or import settings were modified.

## 1. Reason

Post-import runtime review reopened Resident_A:

- Hero and Resident_B are the accepted visual reference.
- Current Resident_A v2 reads with a stronger pixel feel than the reference characters.
- Resident_A face/head scale reads visibly larger than the Hero, creating mismatch in the running build.

This task creates a user-review sheet only. It does not select, crop, sheet, or import a replacement.

## 2. Review Sheet

Tracked review evidence:

- `docs/devlog/screenshots/stage4_resident_a_followup_review_sheet_abc.png`
- `docs/devlog/screenshots/stage4_resident_a_candidate_c_size_compare.png`
- `docs/devlog/screenshots/stage4_resident_a_followup_review_sheet_c2_c3.png`
- `docs/devlog/screenshots/stage4_resident_a_candidate_c2_c3_size_compare.png`

Local intermediate files, intentionally under ignored `art/_intermediate/`:

- `art/_intermediate/stage4_resident_a_followup_review/resident_a_v2_followup_candidates_abc_raw.png`
- `art/_intermediate/stage4_resident_a_followup_review/resident_a_v2_followup_review_sheet_abc.png`
- `art/_intermediate/stage4_resident_a_followup_review/resident_a_v2_followup_candidates_c2_c3_raw.png`

The top of the sheet shows the current runtime contact sheet for Hero, Resident_A, and Resident_B. The bottom shows three new Resident_A concept options labeled A / B / C.

## 3. Prompt Summary

Built-in image generation was used for one review-only concept sheet.

Intent:

- Generate Resident_A alternatives that reduce the harsh/blocky pixel feel.
- Bring face/head scale and body proportion closer to Hero and Resident_B.
- Keep Resident_A ordinary, quiet, and past-side readable without hero, fantasy, or mascot styling.
- Show front, back, left, and right standing views for each candidate.

Main avoid list:

- Oversized head or face.
- Oversized eyes or large facial features.
- Chibi proportions.
- Harsh chunky pixels.
- Fantasy armor, weapons, magic aura, glowing eyes, ornate accessories, dramatic emotion, watermark.

## 4. Candidate Notes

| Candidate | Initial read | Risk |
|---|---|---|
| A | Ordinary town resident, simple clothing, clear front/back/side read. | Still somewhat large-headed; may remain close to the current mismatch. |
| B | Similar ordinary-resident direction with slightly calmer outfit block. | Head and hair volume still read large against Hero. |
| C | Softer pixel feel and smaller face read; closer to the requested reduction. | Longer hair and skirt-like lower silhouette may need user confirmation for Resident_A role fit. |

No candidate is accepted yet. Runtime import should wait for user selection or a regeneration instruction.

## 4.1 Size Comparison Follow-Up

After user feedback that Candidate C still seemed slightly large-headed, Candidate C was temporarily fit into a 32x48 cell and compared against current Hero, Resident_A, and Resident_B.

Approximate visual metrics from the fit comparison:

| Sprite | BBox in 32x48 cell | Rough head ratio |
|---|---:|---:|
| Hero v2 | 19x45 | 0.29 |
| Resident_A current | 20x45 | 0.36 |
| Resident_B v2 | 25x45 | 0.33 |
| Candidate C fit | 16x45 | 0.36 |

Result: Candidate C direction is useful, but its head ratio remains closer to current Resident_A than to Hero. It should not be imported as-is.

A narrower follow-up prompt produced C2 / C3:

| Sprite | BBox in 32x48 cell | Rough head ratio |
|---|---:|---:|
| C2 fit | 17x45 | 0.33 |
| C3 fit | 16x45 | 0.33 |

Result: C2 / C3 reduce the mismatch, with C3 closest to the requested direction. Hero remains smaller-headed at 0.29, so the recommended runtime path is to use C3 as the visual base but shave head / hair volume by roughly one 32x48-cell pixel during final extraction or a targeted redraw pass.

## 5. Next Step

User review gate:

- Pick C3 as the current best base, or request one more regeneration targeting Hero-like head ratio.
- After selection, create actual 32x48 gameplay cells and idle/walk sheets.
- Compare the extracted gameplay cells against Hero and Resident_B before replacing `Assets/Art/Sprites/NPC/Resident_A/v2/`.
- During final extraction, keep C3 body direction but reduce head / hair volume by about one gameplay-cell pixel if it still reads larger than Hero.
- Add asset ledger provenance only when a selected replacement becomes runtime/player-consumed.

## 6. Verification

- Generated review sheet saved locally and copied to tracked devlog screenshots.
- No Unity project assets were modified.
- No tests were run because this is review evidence only.

## 7. Revision History

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-06 | Initial Resident_A follow-up review sheet and candidate notes |
