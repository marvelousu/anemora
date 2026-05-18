# Stage 4 PixelLab API Source Script / Monthly-Quota Probe

Date: 2026-05-10

## Summary

Added a review-only PixelLab API source-generation helper for Stage 4 character locomotion source PNGs:

- `tools/generate_pixellab_source_pngs.py`

The helper discovers existing v10/v8 locomotion builders, reads prompt markdown fenced blocks, estimates Pixflux fallback cost, checks `PIXELLAB_API_TOKEN` via the PixelLab balance endpoint, and can write generated base64 PNG responses into each builder's `source/` folder when `--execute` is explicitly passed.

## API Confirmation

Official PixelLab API docs / OpenAPI were checked from:

- `https://www.pixellab.ai/pixellab-api`
- `https://api.pixellab.ai/v1/openapi.json`

Confirmed operational surface:

- `GET https://api.pixellab.ai/v1/balance`
- `POST https://api.pixellab.ai/v1/generate-image-pixflux`
- Bearer authentication with `PIXELLAB_API_TOKEN`
- JSON response containing base64 PNG image data and `usage.usd`

Pixflux was selected because the planned 6-frame horizontal sheets use `384x96`, which fits the 400 px Pixflux dimension limit and does not fit Bitforge's 200 px width limit.

## Verification

Commands run:

```powershell
python -m py_compile tools\generate_pixellab_source_pngs.py
python tools\generate_pixellab_source_pngs.py --list
python tools\generate_pixellab_source_pngs.py --character aria --direction walk_back --limit 1
python tools\generate_pixellab_source_pngs.py --balance --character aria --direction walk_back --limit 1
python tools\generate_pixellab_source_pngs.py --execute --character aria --direction walk_back --limit 1
```

Observed:

- Full discovery: 31 missing source PNG tasks.
- Token was present and only logged in masked form.
- PixelLab `/balance` returned `$0.0000`; the product UI later clarified this is paid credits after monthly quota, not the monthly generation allowance.
- `--execute --skip-balance-check` proved monthly quota works through the API: the Aria walk_back probe returned `usage.usd = 0.0`.

## Pixflux Probe Result

Two Aria walk_back Pixflux probes were generated and rejected:

- `384x96`: too small for the existing extractor and produced the wrong side-walk composition.
- `400x400`: API call succeeded, but Pixflux produced a labeled two-row character sheet (`Aleinate:` / `Aria:`) with only 4 detectable components, not the required single-row 6-frame back-view strip.

Rejected probes were moved under:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/
```

The live `source/` path was restored to missing, and the Aria builder now graceful-skips back/left/right again.

## Animation / Skeleton Probe Result

PixelLab animation endpoints were also tested against an Aria 64x64 reference.

| Endpoint | Result | Decision |
|---|---|---|
| `/animate-with-text` | Succeeded after sending pure base64 image data. Returned 4 frames at no paid-credit cost, but the result had magenta artifacts/background and did not match the 64x96 6f review contract. | Not production-ready |
| `/estimate-skeleton` | Returned 18 normalized keypoints from the Aria reference. | Potential support tool |
| `/animate-with-skeleton` | Returned frames from simple shifted poses, but direction/back-view compliance remained weak and the output did not match the current review format. | Not production-ready |

These probes confirm PixelLab is available through the monthly quota, but the raw API endpoints are not a safe blind-batch path for the current locomotion sources.

## Script Follow-Up

`tools/generate_pixellab_source_pngs.py` was adjusted so `$0.00` paid credits are a warning, not a hard block, because monthly generations can cover API usage. The hard block is now opt-in with `--require-credit-balance`.

`tools/_dq3r_locomotion_pilot_v10_lib.py` was also made slightly more tolerant of smaller API-sized source sheets by preserving existing alpha and scaling component-size thresholds by image size. Existing high-resolution v10 source behavior remains covered by the same thresholds.

## Next

Do not batch PixelLab Pixflux or animation locomotion sheets yet.

The next useful path is:

1. Probe Retro Diffusion Animation if account/API access is available, especially `four_angle_walking` / `walking_and_idle` style spritesheets.
2. If Retro Diffusion is unavailable, use PixelLab Creator / Rotation / Animation UI manually for directional stills, then repair in Aseprite.
3. If neither path preserves identity and direction reliably, build runtime-first 4-frame strips from accepted stills; the runtime spec already targets 32x48 / 4f.

Current safe status check:

```powershell
python tools\build_stage4_dq3r_aria_locomotion_pilot_v10.py
```

It should build Aria front and skip the three missing directions.
