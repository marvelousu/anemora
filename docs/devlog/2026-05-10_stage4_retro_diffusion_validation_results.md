# Stage 4 Retro Diffusion Validation Results

Date: 2026-05-10

## Scope

Validate whether Replicate-hosted Retro Diffusion Animation is a practical candidate for Anemora Chapter 1 character locomotion sprites before any wider spend or subscription.

All outputs remain rejected/probe assets only. Nothing was copied into live `source/`, runtime sprite folders, or Unity `Assets/` folders.

Probe sandbox:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/retro_diffusion_probe/
```

## API Setup Findings

- Replicate account/token authentication worked.
- `$1.00` prepaid credit was visible before the successful run.
- The original `Cancel-After: 3m` request still failed with HTTP 402.
- Reducing `Cancel-After` to `60s` allowed the same one-output probe to run.
- Keep future prepaid probes at `Cancel-After: 60s` unless the credit balance is much larger.

## Successful Probes

| Character | Seed | Runtime | Output | Structure |
|---|---:|---:|---|---|
| Aria | `260510` | `53.5s` | `aria_replicate_rd_animation_four_angle_walking_48x48_probe.png` | Pass |
| Aria | `260511` | `47.2s` | `aria_replicate_rd_animation_four_angle_walking_48x48_probe_aria_seed260511.png` | Pass |
| Mia | `260512` | `20.5s` | `mia_replicate_rd_animation_four_angle_walking_48x48_probe_mia_seed260512.png` | Pass |

All three successful outputs were:

- `192x192`
- 4 rows x 4 columns
- `48x48` cells
- transparent RGBA
- binary alpha (`0` or `255`)
- no labels, no text, no UI panels

## Extracted Review Files

Reusable adapter:

```text
tools/extract_retro_diffusion_four_angle_runtime_candidates.py
```

Extracted row strips and runtime-width crops are under:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/retro_diffusion_probe/extracted_candidates/
```

Key review sheets:

- `aria_rd_four_angle_48x48_rows_review_4x.png`
- `aria_rd_four_angle_runtime32x48_rows_review_4x.png`
- `aria_rd_four_angle_seed_compare_48x48_rows_review_3x.png`
- `mia_rd_four_angle_48x48_rows_review_4x_seed260512.png`
- `rd_aria_mia_runtime32x48_lineup_review_4x.png`
- `runtime_direction_candidates/rd_runtime_direction_candidates_aria_mia_review_4x.png`
- `runtime_direction_candidates/rd_runtime_direction_candidates_vs_niro_review_4x.png`
- `runtime_direction_candidates/rd_runtime_direction_candidates_vs_niro_height_norm_review_4x.png`

Named runtime-direction candidates:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/retro_diffusion_probe/runtime_direction_candidates/
```

Reusable builder:

```text
tools/build_retro_diffusion_runtime_direction_review.py
```

Direction mapping used:

| RD row | Runtime direction |
|---:|---|
| 2 | `walk_front` |
| 0 | `walk_back` |
| 3 | `walk_left` |
| 1 | `walk_right` |

## Quality Assessment

### Strengths

- The sheet contract is reliable across three probes.
- Back/front/two-side row structure is clear.
- Transparent output avoids PixelLab's magenta cleanup issue.
- Aria seed `260510` preserves the intended mustard coat / cream apron child read better than PixelLab API probes.
- Mia preserves adult proportions and household/apron read from the prompt/reference.
- Runtime `32x48` conversion is straightforward as a centered crop from each `48x48` cell.

### Risks

- User review identified that the Retro Diffusion outputs changed the prior character-design taste too much.
- Aria identity varies between seeds:
  - `260510`: closer to current Aria color/read.
  - `260511`: cleaner but drifts toward brighter blond hair and lighter palette.
- Side rows are sometimes 3/4-like and need explicit direction assignment / mirroring.
- Style is glossier and higher contrast than some existing v10 cast sources.
- Runtime `32x48` crops need manual review; centered crop works for these probes but should not become an unreviewed batch assumption.
- This does not directly produce the existing v10 `64x96` / 6-frame review source contract. It is better aligned with the runtime spec (`32x48` / 4f).

## Runtime Scale Check

The raw centered `32x48` crop was structurally correct but too short against the current Niro runtime anchor:

| Character | Raw alpha height | Height-normalized target |
|---|---:|---:|
| Niro current | `43px` | n/a |
| Aria RD seed `260510` | `34-37px` | `39px` |
| Mia RD seed `260512` | `35-38px` | `43px` |

Height-normalized candidates were generated alongside raw candidates:

- Aria target `39px`, preserving child-read relative to Niro's `43px`.
- Mia target `43px`, matching adult-height Niro.

The normalized review is more promising for production review:

```text
runtime_direction_candidates/rd_runtime_direction_candidates_vs_niro_height_norm_review_4x.png
```

## Design-Adherence Follow-Up

Existing preview tool found:

```text
tools/build_review_gallery.py
docs/review_gallery/index.html
```

The gallery was regenerated with:

```powershell
python tools\build_review_gallery.py
```

and opened at:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?v=aria-design-adherence
```

A design-adherence review board was added:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/retro_diffusion_probe/design_adherence_review/aria_rd_design_adherence_review_4x.png
```

Comparison:

- Old v10 Aria reference: taller, softer, lower saturation, brown hair, less toy-like.
- RD seed `260510`: structurally strong, but too glossy/chibi and too blond.
- RD design-lock seed `260513`: better hair color, lower saturation, taller read, but still sharper/less painterly than old v10.
- RD 64x96 reference seed `260514`: attempted with larger old-v10 reference, but aborted at `Cancel-After: 60s` with no output; do not rerun until credit/cancel window is intentionally approved.

Decision:

- Retro Diffusion should remain the 4-direction motion/grid generator, not the final character-style source.
- Final visual style should be pulled back toward the old v10 / Unity-transfer character design through prompt discipline and Aseprite/manual normalization.
- Next successful generation should require: old design preserved, lower saturation, brown hair, smaller head, taller body/legs, no glossy highlights.

## Updated Decision After User Style Review

Retro Diffusion Animation passes the structure-validation gate only.

It fails the current final-style gate for Aria. Seed `260510` and the later design-lock probe are visibly different from the accepted old v10 Aria: lower apparent age, lower body proportion, glossier/high-contrast rendering, and weaker preservation of the brown-hair/soft-mustard old-v10 taste. Do not use RD direct output as final character art.

RD remains useful as a motion/grid reference because it reliably produced transparent 4-direction, 4-frame sheets. It should not be the primary source for character identity or visual style.

PixelLab Rotate was then tested with the accepted old v10 runtime front frame as the reference source. That path preserved the old taste much better for directional stills. See:

```text
docs/devlog/2026-05-10_stage4_aria_old_style_preservation_probe.md
```

Revised production order:

1. Preserve old v10 front as the design source of truth.
2. Use PixelLab Rotate or equivalent reference-locked rotation for back/side stills.
3. Build walk cycles from those stills with PixelLab Skeleton / Aseprite / local strip tooling.
4. Use RD only as a pose/gait guide, not as accepted art.

Do not batch the full cast yet. A first local adapter now exists for:

1. `48x48` RD spritesheet input.
2. row-to-direction assignment.
3. `32x48` runtime strip crops.
4. scaled review sheets.

Remaining adapter work before any RD-guided production use:

1. explicit direction naming and optional side mirroring.
2. palette/style normalization plan before Aseprite finishing.
3. Niro/Mia/Aria scale gate approval using the height-normalized review.
4. optional side mirroring if the in-game controller expects mirrored pair behavior.

After that adapter exists, run a small cast subset before spending more:

- Aria chosen seed candidate.
- Mia chosen seed candidate.
- one male adult candidate, likely Dario or Kairo.

## Current Recommendation

Stop further paid API runs until the adapter/review pipeline is in place. The core question has been answered: Retro Diffusion can generate the required transparent 4-direction walking sheet structure.

## Sources

- Replicate RD Animation API schema: `https://replicate.com/retro-diffusion/rd-animation/versions/2e4eabbc836578423e151ba27352a1d8a566c6f1186c0da3ddf66c54deacf28b/api`
- Replicate prepaid credit docs: `https://replicate.com/docs/topics/billing/prepaid-credit`
