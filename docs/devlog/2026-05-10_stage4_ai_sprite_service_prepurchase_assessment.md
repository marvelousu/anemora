# Stage 4 AI Sprite Service Pre-Purchase Assessment

Date: 2026-05-10

## Scope

This note evaluates whether Anemora Chapter 1 character locomotion work should justify additional paid AI-service spend.

Current Anemora requirements:

- Runtime character strips: `32x48`, 4 frames, `128x48` per direction.
- Required directions: stand/idle plus `walk_front`, `walk_back`, `walk_left`, `walk_right`.
- Review builders currently use larger `64x96` / 6-frame candidates for visual gating.
- Final accepted sprites must pass proportion, silhouette, palette, gait, and cross-character checks.
- ADR pipeline requires AI generation plus Aseprite/manual finishing; raw AI output is not final.

## Existing Access

Only `PIXELLAB_API_TOKEN` is present in the local environment.

No local environment variables were found for:

- Retro Diffusion
- Replicate
- Scenario

## PixelLab

Current status:

- The product UI showed `Generations 49 / 2,000` and `Credits $0.00`.
- API probe confirmed `$0.00` means paid credits, not exhausted monthly generations.
- PixelLab API generation succeeded with `usage.usd = 0.0`.

Official current API capabilities relevant to this task:

- Pixflux supports text-to-pixel-art, init images, palettes, optional transparent background, max area `400x400`.
- Rotate supports character/object rotation up to `128x128`.
- Animate with Skeleton supports up to `128x128`.
- Estimate Skeleton is intended for Animate with Skeleton.

Local probe result:

- Pixflux `384x96`: wrong structure and unsuitable side-walk sheet.
- Pixflux `400x400`: labeled multi-row sheet, 4 detectable components, not required direction strip.
- Animate with Text `64x64`: returned 4 frames but with magenta artifacts/background and wrong review format.
- Estimate Skeleton: returned useful normalized keypoints.
- Animate with Skeleton: returned frames but direction/back-view compliance was weak.

Purchase decision:

- Do not buy more PixelLab credits solely for the current strict locomotion task.
- Existing monthly quota can still be used for UI/manual experiments, reference concepts, rotate support, and rough animation candidates.
- PixelLab remains a support tool, not the blind batch production path.

## Retro Diffusion / Replicate

Official current fit:

- Replicate hosts `retro-diffusion/rd-animation`.
- The model is designed for animated pixel-art sprites and spritesheets.
- API schema supports `style`, `prompt`, optional `input_image`, `return_spritesheet`, and fixed style sizes.
- `four_angle_walking` and `walking_and_idle` support `48x48`.
- `small_sprites` supports `32x32`.
- Replicate is pay-as-you-go; pricing is generally based on model/hardware runtime or model-specific pricing shown on model pages.

Fit against Anemora:

- Stronger fit than PixelLab raw Pixflux because `four_angle_walking` directly targets 4-direction humanoid walking.
- `48x48` can be adapted toward the runtime `32x48` contract more naturally than generic sheet generation.
- Risk remains: image references are not direct init images, so exact Aria identity preservation is not guaranteed.

Purchase decision:

- Prefer a minimal Replicate probe over any larger monthly subscription, if a tiny pay-as-you-go test is available.
- Do not subscribe to a larger plan until one Aria probe proves:
  - transparent spritesheet output,
  - exact 4-direction / 4-frame grid,
  - acceptable identity retention,
  - clean baseline and frame registration,
  - lower Aseprite repair effort than local/manual drawing.

## Scenario

Official current fit:

- Scenario exposes `model_retrodiffusion-animation` with `four_angle_walking`, `walking_and_idle`, `small_sprites`, and `vfx`.
- API access is documented as Pro-and-above.
- Plans currently list Starter `$15/mo`, Pro `$45/mo`, Max `$75/mo`.
- API/credit docs support `dryRun=true` cost estimation before spending compute units.

Fit against Anemora:

- Feature fit is good because Scenario also offers Retro Diffusion Animation.
- Purchase risk is higher than Replicate if API access requires Pro, because the first useful probe may require a monthly plan.
- Scenario is more attractive only if the account already has usable free daily credits, trial access, or if the web UI can run Retro Diffusion Animation before paid API use.

Purchase decision:

- Do not buy Scenario Pro solely for this task before proving the model through free trial/web UI/dryRun.
- If Scenario is already contracted or trial-accessible, use it for the same one-character Aria probe.

## Other General Image Models

Generic image models are not recommended for production locomotion sheets.

Use them only for:

- static concepts,
- clothing/color exploration,
- marketing or reference imagery,
- possible redraw guidance.

They do not solve the hard requirements: transparent pixel grid, consistent direction set, stable walk cycles, and frame-to-frame identity.

## Low-Risk Decision Gate

Before spending new money, run only this gate:

1. Confirm whether Replicate can run `retro-diffusion/rd-animation` with minimal pay-as-you-go spend or free credits.
2. If yes, run exactly one Aria probe:
   - style: `four_angle_walking`
   - size: `48x48`
   - `return_spritesheet=true`
   - input/reference: current Aria front/reference
   - output folder: `source_rejected/retro_diffusion_probe/`
3. Measure whether it beats PixelLab probes on:
   - grid correctness,
   - direction correctness,
   - transparent background,
   - identity retention,
   - manual repair time.
4. Only after a pass should paid production usage be considered.

Prepared local helper:

- `tools/probe_replicate_retro_diffusion_aria.py`
- `docs/devlog/2026-05-10_stage4_retro_diffusion_trial_instructions.md`

The helper creates a 48x48 Aria reference from the current v10 front source, writes a dry-run summary, and refuses to execute unless `REPLICATE_API_TOKEN` is set and `--execute` is passed.

2026-05-10 execution attempt:

- Replicate API returned HTTP 402 `Insufficient credit`.
- No probe sheet was generated.
- Summary was recorded at `source_rejected/retro_diffusion_probe/replicate_rd_animation_probe_summary.json`.
- This means the current Replicate account needs credit/billing before even the one-output API trial can run.
- Recommendation remains: do not buy a monthly subscription. If continuing, buy only the smallest feasible Replicate pay-as-you-go credit after checking the model page estimate; otherwise try Scenario web UI/trial first.
- After payment information was reportedly added, the same API probe was retried and still returned HTTP 402. Treat this as "billing method present is not enough"; the account needs spendable credit/propagation, or the API token must match the billed account.
- After `$1.00` credit was visible, the probe still failed with the original `Cancel-After: 3m`; reducing `Cancel-After` to `60s` allowed the same probe to succeed.
- Result: Retro Diffusion Animation produced a transparent `192x192`, 4-row x 4-column, `48x48` spritesheet in 53.5 seconds.
- This is the first AI service probe that satisfies the hard sheet-structure gate for Aria locomotion.
- It is not final art yet, but it is good enough to justify a second controlled probe before any wider batch.
- Follow-up validation ran one additional Aria seed and one Mia adult probe. Both also produced transparent `192x192` / `48x48` 4x4 spritesheets.
- Validation results are recorded in `docs/devlog/2026-05-10_stage4_retro_diffusion_validation_results.md`.
- Recommendation: stop paid API runs until a local RD-to-runtime adapter/review pipeline exists.

## Recommendation

Do not purchase additional PixelLab credits for this current locomotion task.

Do not subscribe to Scenario Pro only for this task unless a free/trial UI probe succeeds first.

If any new spend is considered, the least-wasteful path is a small Replicate Retro Diffusion Animation probe, because it tests the exact model family and task shape before committing to a monthly subscription.

## Sources Checked

- PixelLab API: `https://www.pixellab.ai/pixellab-api`
- PixelLab FAQ: `https://www.pixellab.ai/docs/faq`
- PixelLab Refund Policy: `https://www.pixellab.ai/refundpolicy`
- Replicate RD Animation: `https://replicate.com/retro-diffusion/rd-animation/readme`
- Replicate RD Animation API schema: `https://replicate.com/retro-diffusion/rd-animation/versions/2e4eabbc836578423e151ba27352a1d8a566c6f1186c0da3ddf66c54deacf28b/api`
- Replicate pricing: `https://replicate.com/pricing`
- Retro Diffusion API examples: `https://github.com/Retro-Diffusion/api-examples`
- Scenario pricing: `https://www.scenario.com/pricing`
- Scenario API credits: `https://help.scenario.com/articles/7934059476-api-usage-and-credits-compute-units`
- Scenario model parameter reference: `https://docs.scenario.com/docs/image-generation-models-parameters-reference`
