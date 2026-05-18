# Stage 4 Character Generation Tooling Decision

Date: 2026-05-10

## Decision

Do not batch the current Chapter 1 locomotion sources through raw PixelLab Pixflux.

For the immediate character-generation task, use this revised order after the old-style preservation probe:

1. **Old v10 accepted source art** is the style and identity source of truth.
2. **PixelLab Rotate** for back/side directional stills from the old front reference.
3. **PixelLab Skeleton / Aseprite / local strip tooling** for walk-cycle frames from accepted stills.
4. **Retro Diffusion Animation** only as a motion/grid guide, not final character art.
5. Use generic image models only for static concept or style reference, not final locomotion sheets.

## Why

Anemora specs require:

- Runtime target: `32x48`, 4-frame strips (`chapter1_character_runtime_import_spec.md`).
- Review/generation target: current v10 builders expect `64x96` review cells and strict frame extraction.
- Character rubric requires proportion read, silhouette read, palette separation, walk gait, and cross-character harmony (`docs/dq3r_character_rubric.md`).
- ADR-0003 says 2D character/object generation is **PixelLab + Retro Diffusion**, with **Aseprite** finishing.
- ADR-0009 says AI generation is only the first step; final import must be after intermediate repair/verification and ledger/devlog recording.

## PixelLab API Probe Results

PixelLab API access works through the monthly generation allowance. The product UI `$0.00` value is paid credits after the monthly quota, not a blocker. API responses returned `usage.usd = 0.0`.

However, raw API outputs did not satisfy the current locomotion source contract:

| Endpoint | Result | Decision |
|---|---|---|
| `/generate-image-pixflux`, `384x96` | Generated side-walk-like small sheet; extractor detected the wrong structure | Reject |
| `/generate-image-pixflux`, `400x400` | Generated labeled two-row sheet (`Aleinate:` / `Aria:`), 4 detectable components instead of 6 | Reject |
| `/animate-with-text`, `64x64` | Generated 4 frames, but magenta artifacts/background and wrong format for `64x96` 6f review pipeline | Not enough |
| `/estimate-skeleton` | Returned 18 keypoints from Aria reference | Useful support tool |
| `/animate-with-skeleton`, `64x64` | Generated 3 frames, still front-facing/weak back-view compliance and magenta background | Not enough |

Rejected probes are stored under:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/
```

## PixelLab Assessment

PixelLab is still useful, but not as a blind batch source for current 6-frame sheets.

Use PixelLab when:

- Creating or refining a single character/front/source concept.
- Using Rotate from the accepted old v10 front where the tool can preserve identity and directional views.
- Generating rough animation candidates that will be repaired manually.

Do not use PixelLab raw Pixflux batch when:

- The output must be a strict single-row 6-frame sheet.
- No labels/text/panel layout can be tolerated.
- Back/side direction must be exact on the first pass.

## Retro Diffusion Assessment

Retro Diffusion matches the sheet-structure problem better than raw PixelLab Pixflux, but it does not match the final style-preservation problem better than PixelLab Rotate.

Evidence from current public docs:

- RD Animation is described as style-consistent animated pixel art sprite generation with sheets organized for game-engine compatibility.
- It has dedicated animation styles for `four_angle_walking` and `walking_and_idle`.
- The API examples document `return_spritesheet: true` for transparent PNG spritesheet output.
- `rd_animation__four_angle_walking` and `rd_animation__walking_and_idle` are 48x48 only, which maps more naturally to Anemora's runtime contract (`32x48`) than PixelLab's current 64x64 animation endpoint.
- Advanced animation styles support `frames_duration` including 4 and 6, with `return_spritesheet: true`.

The executed probes confirmed the structure claim but failed the Aria old-style identity gate. RD direct outputs should be rejected as final art and retained only as a motion/grid guide.

## Recommended Next Probe

Do not buy a new subscription before a minimal proof.

The next useful probe is not more RD seed search. Use the accepted old v10 Aria front as the fixed reference and continue the PixelLab Rotate / Skeleton path:

```text
tools/probe_pixellab_rotate_aria_old_style.py
```

Initial target:

- Character: Aria
- Source: `runtime_candidate_64x96_review_only/resident_a_aria_walk_front_v10a_64x96_6f_review_only.png`
- Mode A: PixelLab `/rotate`, `128x128`, south to north/east/west, old palette reference
- Mode B: PixelLab `/animate-with-skeleton` or Aseprite hand-authored 4f cycles using the accepted stills
- Output: `source_rejected/pixellab_rotate_old_style_probe/` first, not live `source/`

Gate:

- Must preserve the old v10 face, hair color, mustard/cream palette, and taller child-read proportion.
- Must output clean alpha with stable directional silhouettes.
- Must be convertible into the review/runtime contract without changing accepted front style.

## Fallback

If PixelLab Rotate / Skeleton cannot produce acceptable walk cycles:

1. Build deterministic 4f walk strips with local Aseprite/Pillow editing from accepted stills.
2. Trial one upload-reference sprite-specific service before any subscription.
3. Accept 4-frame runtime-first strips for Chapter 1 NPCs and keep 6f v10 review strips as optional polish, because the runtime spec already uses 4 frames.

## References

- `README.md`: 2D asset pipeline is PixelLab + Aseprite; AI services include PixelLab, Meshy, AIVA, Suno, ElevenLabs SFX, Stable Audio.
- `docs/adr/0003-asset-pipeline.md`: 2D character/object generation is PixelLab + Retro Diffusion; Aseprite finishing required.
- `docs/adr/0009-asset-pipeline.md`: generation -> intermediate processing -> final import; raw AI files are intermediate; asset ledger/devlog required.
- `docs/asset_prompts/chapter1_character_runtime_import_spec.md`: runtime sprites are 32x48, 4-frame strips.
- `docs/dq3r_character_rubric.md`: proportion, silhouette, palette, gait, and cross-character gates.
- PixelLab: `https://www.pixellab.ai/`, `https://api.pixellab.ai/v1/openapi.json`
- Retro Diffusion API examples: `https://github.com/Retro-Diffusion/api-examples`
- Retro Diffusion / Scenario RD Animation docs: `https://www.scenario.com/models/retro-diffusion-animation`, `https://help.scenario.com/articles/4202673551-retro-diffusion-models-the-essentials`
- Pre-purchase assessment: `docs/devlog/2026-05-10_stage4_ai_sprite_service_prepurchase_assessment.md`
