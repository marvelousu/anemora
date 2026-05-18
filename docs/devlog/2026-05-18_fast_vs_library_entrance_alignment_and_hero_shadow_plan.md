# 2026-05-18 Fast VS Library Entrance Alignment and Hero Shadow Plan

## Request

- Library layout direction is good, but the entrance position is offset.
- Question raised: can the protagonist have shading on the sprite itself and a shadow, since the project is aiming for an HD-2D feel?

## Cycle

- Main session diagnosed the entrance mismatch.
- A `gpt-5.4-mini` worker received a narrow instruction for only:
  - `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- Worker aligned the visible entry threshold and route trigger.
- Main review caught that the first landing target was still too close to the return trigger.
- Main session moved the landing point slightly farther inside and reran validation/build/smoke.

## Entrance Fix

Updated:

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`

The deeper library pass moved:

- `Library_EntryThreshold` to local `Z -6.35`

But route constants still pointed around the old entrance. Now:

- `LibraryToPlazaTriggerCenter` is aligned to the threshold at local `Z -6.35`.
- `LibraryFromPlazaTarget` lands just inside the entrance at local `Z -4.75`.

Validation:

- Added `ValidateLibraryEntryAlignment(...)`.
- It checks current/past entry thresholds stay aligned with `LibraryToPlazaTriggerCenter`.
- It also checks `LibraryFromPlazaTarget` remains inside the entry without being so close that the player immediately bounces back to the plaza.

## Hero Shadow / Shading Plan

Possible and worth doing.

Fastest safe pass:

- Add a soft oval/contact shadow under Niro.
- Keep it independent from the time-window culling and player layer behavior.
- Scale/opacity can be tuned per area if needed.

Next visual pass:

- Add a small sprite-side shading overlay or tint ramp so Niro has a lit/dark side even before full material work.
- Keep the source sprite unchanged at first, so rejected shading can be removed without touching character art.

Higher fidelity later:

- Use a lit sprite shader / generated normal map / direction-specific shaded sprite variants.
- This is more HD-2D-like, but should come after the simple contact shadow proves the look.

## Verification

- First Unity validation caught the landing target being too close to the library return trigger.
- Adjusted `LibraryFromPlazaTarget` from local `Z -5.20` to `Z -4.75`.
- `git diff --check` passed for the setup file.
- Unity batch generation and validation passed via:
  `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Player build succeeded:
  `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 18-second `-batchmode -nographics` smoke run produced no error, exception, crash, or `NullReference` log entries.

Known non-fatal batch warnings:

- Unity licensing access-token update warning.
- `RenderTexture.Create failed` warnings under `-nographics`.
