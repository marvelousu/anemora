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

Local intermediate files, intentionally under ignored `art/_intermediate/`:

- `art/_intermediate/stage4_resident_a_followup_review/resident_a_v2_followup_candidates_abc_raw.png`
- `art/_intermediate/stage4_resident_a_followup_review/resident_a_v2_followup_review_sheet_abc.png`

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

## 5. Next Step

User review gate:

- Pick A / B / C as a base, or request regeneration with a narrower target.
- After selection, create actual 32x48 gameplay cells and idle/walk sheets.
- Compare the extracted gameplay cells against Hero and Resident_B before replacing `Assets/Art/Sprites/NPC/Resident_A/v2/`.
- Add asset ledger provenance only when a selected replacement becomes runtime/player-consumed.

## 6. Verification

- Generated review sheet saved locally and copied to tracked devlog screenshots.
- No Unity project assets were modified.
- No tests were run because this is review evidence only.

## 7. Revision History

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-06 | Initial Resident_A follow-up review sheet and candidate notes |
