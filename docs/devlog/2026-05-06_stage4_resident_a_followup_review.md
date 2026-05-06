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
- `docs/devlog/screenshots/stage4_resident_a_c3_32x48_headfix_variants.png`
- `docs/devlog/screenshots/stage4_resident_a_followup_review_sheet_d_e_hero_ratio.png`
- `docs/devlog/screenshots/stage4_resident_a_hero_ratio_regen_compare.png`
- `docs/devlog/screenshots/stage4_resident_a_fgh_connected_nearest_compare.png`
- `docs/devlog/screenshots/stage4_resident_a_f_based_f2_f3_f4_nearest_compare.png`

Local intermediate files, intentionally under ignored `art/_intermediate/`:

- `art/_intermediate/stage4_resident_a_followup_review/resident_a_v2_followup_candidates_abc_raw.png`
- `art/_intermediate/stage4_resident_a_followup_review/resident_a_v2_followup_review_sheet_abc.png`
- `art/_intermediate/stage4_resident_a_followup_review/resident_a_v2_followup_candidates_c2_c3_raw.png`
- `art/_intermediate/stage4_resident_a_followup_review/resident_a_c3_32x48_*.png`
- `art/_intermediate/stage4_resident_a_followup_review/resident_a_v2_followup_candidates_d_e_hero_ratio_raw.png`
- `art/_intermediate/stage4_resident_a_followup_review/resident_a_fgh_connected_candidates_raw.png`
- `art/_intermediate/stage4_resident_a_followup_review/resident_a_[fgh]_connected_fit_32x48.png`
- `art/_intermediate/stage4_resident_a_followup_review/resident_a_f2_f3_f4_f_based_raw.png`
- `art/_intermediate/stage4_resident_a_followup_review/resident_a_f[234]_f_based_fit_32x48.png`

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

## 4.2 Both Follow-Up Paths

User requested both next paths:

1. Create a C3-based 32x48 head / hair reduction prototype.
2. Regenerate stricter Hero-ratio candidates.

### C3 32x48 Prototype

The local 32x48 prototype keeps the C3 body direction and reduces head / hair volume before cell fitting.

| Sprite | BBox in 32x48 cell | Rough head ratio | Read |
|---|---:|---:|---|
| Hero v2 | 19x45 | 0.29 | Accepted reference |
| Resident_A current | 20x45 | 0.36 | Too large-headed |
| Resident_B v2 | 25x45 | 0.33 | Accepted, hair-heavy seated reference |
| C3 base | 16x45 | 0.33 | Better, still slightly large-headed |
| C3 fix C | 17x45 | 0.31 | Good head-size metric, but user review found the head/body connection reads detached |

Result after user review on 2026-05-07: C3 fix C is not approved as an implementation base as-is. The head-size metric is useful, but the neck / collar / shoulder bridge was reduced too far and the head reads separated from the body. The review image also reads too blurry to evaluate pixel quality confidently.

### Hero-Ratio Regeneration

A stricter regeneration produced D / E.

| Sprite | BBox in 32x48 cell | Rough head ratio | Read |
|---|---:|---:|---|
| D fit | 15x45 | 0.33 | Head still reads larger than intended; more boyish / Hero-adjacent |
| E fit | 15x45 | 0.24 | Smaller head, but too close to Hero-like proportions and less Resident_A-specific |

Result: D / E are useful evidence that Hero-like proportions can be reached, but they drift toward a protagonist-like young resident and away from the C direction. They should not replace the Resident_A direction unless the next user review explicitly prefers this stronger youthful read.

## 4.3 Connected F/G/H Candidates

After C3 fix C was demoted, the next review pass targeted the two blockers directly:

- Restore a visible neck / collar / shoulder bridge.
- Review at crisp nearest-neighbor scale instead of a blurred enlargement.

| Sprite | BBox in 32x48 cell | Read |
|---|---:|---|
| C3 fix C | 17x45 | Rejected: head/body connection reads detached |
| F fit | 17x45 | Connected; later user review preferred this direction over G |
| G fit | 15x45 | Connected and not oversized, but later user review found the hair too short |
| H fit | 17x45 | Connected, but softer / less clear than G |

Result after user review on 2026-05-07: G should not be the implementation base because its shorter hair changes the character read too much. F is the preferred direction despite its larger face read.

## 4.4 F-Based F2/F3/F4 Candidates

The next pass kept F's longer hair and quieter Resident_A read, while reducing the face / head feel slightly.

| Sprite | BBox in 32x48 cell | Read |
|---|---:|---|
| F fit | 17x45 | Preferred direction, but face still reads larger |
| G fit | 15x45 | Demoted: hair too short |
| F2 fit | 16x45 | Strongest current candidate: F-like hair and mood with smaller silhouette |
| F3 fit | 16x45 | F-like hair, but reads slightly taller / stronger |
| F4 fit | 16x45 | F-like hair, but face / hair mass still reads broader |

Result after user review on 2026-05-07: F2 is rejected. The viable direction is F or F4. These two read less like a pure scale correction and more like different personality choices:

- F: younger, softer, more anxious / witness-like.
- F4: more composed, slightly older, more self-contained.

Resident_A role framing for the final selection:

- Past-side young town resident / witness.
- No prior relationship with Niro.
- Points emotionally toward the declining current side through unease, warning, or foreshadowing.
- Should not read as the protagonist, a fantasy page, or a confident guide.

## 5. Next Step

User review gate:

- Review F and F4 against Resident_A's role and decide which personality read should anchor the implementation base.
- If F or F4 is accepted, create actual 32x48 gameplay cells and idle/walk sheets.
- Compare the extracted gameplay cells against Hero and Resident_B before replacing `Assets/Art/Sprites/NPC/Resident_A/v2/`.
- During final extraction, preserve crisp square pixels and avoid any resampling that blurs face, hair edge, neck, or collar.
- Add asset ledger provenance only when a selected replacement becomes runtime/player-consumed.

## 6. Verification

- Generated review sheet saved locally and copied to tracked devlog screenshots.
- No Unity project assets were modified.
- No tests were run because this is review evidence only.

## 7. Revision History

| Version | Date | Change |
|---|---|---|
| v0.3 | 2026-05-07 | Records user rejection of F2 and reframes the decision as F vs F4 with Resident_A role context |
| v0.2 | 2026-05-07 | Records F-over-G user preference and adds F2/F3/F4 F-based refinement comparison |
| v0.1 | 2026-05-06 | Initial Resident_A follow-up review sheet and candidate notes |
