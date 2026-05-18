# Stage 4 Retro Diffusion Trial Instructions

Date: 2026-05-10

## Goal

Run exactly one low-cost or free Retro Diffusion Animation probe before buying any subscription.

The target is not production import. The target is to decide whether Retro Diffusion is worth using for Chapter 1 locomotion sprites.

## Preferred Route: Replicate

Use Replicate first if it allows a tiny pay-as-you-go or free-credit run.

Reason:

- It avoids subscribing to Scenario Pro before seeing one output.
- The hosted model is `retro-diffusion/rd-animation`.
- The schema exposes the exact fields needed for this probe: `style`, `width`, `height`, `input_image`, and `return_spritesheet`.

User steps:

1. Open `https://replicate.com/retro-diffusion/rd-animation`.
2. Sign in.
3. Check whether the playground can run one trial, or whether a small pay-as-you-go balance is required.
4. If using API, create a token at `https://replicate.com/account/api-tokens`.
5. Set it only in the current shell:

```powershell
$env:REPLICATE_API_TOKEN="r8_..."
```

6. From repo root, first run dry-run:

```powershell
python tools\probe_replicate_retro_diffusion_aria.py
```

7. If the dry-run summary looks correct, run exactly one paid/free probe:

```powershell
python tools\probe_replicate_retro_diffusion_aria.py --execute
```

Output goes only to:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/retro_diffusion_probe/
```

Do not copy the output into `source/` until it is reviewed.

## Replicate Attempt Log

2026-05-10:

- Dry-run succeeded.
- API request with `REPLICATE_API_TOKEN` reached Replicate.
- Replicate returned HTTP 402 `Insufficient credit`.
- No spritesheet output was generated.
- After payment information was reportedly registered, the same one-output probe was retried and still returned HTTP 402 `Insufficient credit`.
- This indicates the API account still has no spendable credit, billing has not propagated yet, credit was not actually purchased, or the token belongs to a different Replicate account than the billing setup.
- After `$1.00` credit was visible in Replicate billing, the probe still returned HTTP 402 with the original `Cancel-After: 3m`.
- The helper was changed to default `Cancel-After: 60s`, then the same one-output probe succeeded.
- Replicate logs reported `Generated in 53.5sec`.
- Downloaded output:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/retro_diffusion_probe/aria_replicate_rd_animation_four_angle_walking_48x48_probe.png
```

- Extracted review candidates:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/retro_diffusion_probe/extracted_candidates/
```

- Current summary:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/retro_diffusion_probe/replicate_rd_animation_probe_summary.json
```

Interpretation:

- The token path is wired correctly.
- `$1.00` credit was enough for one generation only after reducing the maximum run window to 60 seconds.
- Keep future probes constrained; long `Cancel-After` values can fail prepaid-credit checks before generation starts.

Initial output assessment:

- Pass: transparent RGBA spritesheet.
- Pass: exact `192x192` output with 4 rows x 4 columns of `48x48` cells.
- Pass: clear back/front/two-side candidate rows.
- Pass: identity is much closer to Aria than the PixelLab raw API probes: mustard coat, cream apron/dress, small child silhouette.
- Watch: side rows are slightly 3/4-like and need direction assignment/possible mirroring.
- Watch: visual style is a little glossy/high-contrast and will need Aseprite normalization against the current cast.
- Watch: runtime import still needs a deliberate adapter to `32x48` / 4-frame strips.

Follow-up validation:

- Aria seed `260511` also succeeded with the same `192x192` / `48x48` 4x4 transparent structure.
- Mia seed `260512` also succeeded, proving the route is not limited to Aria/child proportions.
- See `docs/devlog/2026-05-10_stage4_retro_diffusion_validation_results.md`.

## Fallback Route: Scenario

Use Scenario only if the web UI/trial can run Retro Diffusion Animation before paid API commitment.

User steps:

1. Open `https://www.scenario.com/models/retro-diffusion-animation`.
2. Sign in or start free trial if available.
3. Select Retro Diffusion Animation.
4. Use:
   - style: `four_angle_walking`
   - width: `48`
   - height: `48`
   - return spritesheet: enabled
   - prompt: same Aria prompt from the script
   - reference image: `source_rejected/retro_diffusion_probe/aria_front_reference_48x48_rgb.png`
5. Export the output into:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/retro_diffusion_probe/
```

Do not subscribe to Scenario Pro solely for this probe unless the UI/trial output already passes the gate.

## Aria Probe Prompt

```text
Anemora Aria, ordinary early teen village child, warm mustard short coat, cream apron dress, tied hair ribbon, practical shoes, small readable body, limited color pixel art, clean transparent sprite sheet, 4 direction walking cycle.
```

## Pass / Fail Gate

Pass only if all are true:

- The output is a transparent spritesheet or has a cleanly removable background.
- It contains a clear 4-direction walking layout.
- Each direction has a stable frame grid.
- Aria remains readable as a small ordinary early-teen village child.
- The outfit is close: mustard coat, cream apron/dress, tied hair/ribbon, practical shoes.
- Feet share a stable baseline.
- The manual Aseprite repair estimate is lower than drawing the directions locally.

Fail if any are true:

- The model outputs labels, text, UI panels, watermarks, or multi-character compositions.
- Direction views are wrong or ambiguous.
- The character identity drifts more than PixelLab probes.
- Background cleanup would take longer than manual pixel work.
- The result cannot be adapted to either `48x48 -> 32x48` runtime strips or current review sheets.

## After A Probe Succeeds

If one Aria probe passes, then consider:

1. One more Aria seed to check repeatability.
2. One Mia probe to test adult character identity.
3. Only then decide whether to spend on a wider Chapter 1 batch.

## Sources

- Replicate model: `https://replicate.com/retro-diffusion/rd-animation/readme`
- Replicate API schema: `https://replicate.com/retro-diffusion/rd-animation/versions/2e4eabbc836578423e151ba27352a1d8a566c6f1186c0da3ddf66c54deacf28b/api`
- Replicate API docs: `https://replicate.com/docs/reference/http/`
- Scenario model: `https://www.scenario.com/models/retro-diffusion-animation`
- Scenario API model parameters: `https://docs.scenario.com/docs/image-generation-models-parameters-reference`
