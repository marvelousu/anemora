# 2026-05-18 Fast VS aperture player clipping / drop guard

## User-visible issues

- Keeping Niro on a neutral visible layer made him stable, but when he entered other time the main camera still rendered him outside the Time Window frame.
- That broke the feeling that Niro had moved into the other world and would also make future shadow handling look wrong.
- The front side of both the interior and exterior maps allowed the player to fall off the playable surface.

## Cycle

- Planned the split:
  - worker: main-camera / aperture-camera player culling
  - local: interior and exterior front-side fall prevention
- Reviewed and integrated the worker patch.
- Added validation/build/smoke checks.

## Changes

- Main camera now renders Niro only while he is in current time.
- When Niro is in other time, the main camera excludes `playerVisibleRenderLayer`; the current-to-other aperture camera still includes it, so Niro appears only inside the Time Window texture.
- Existing portal camera target-root checks remain in place, so current/past roots are still isolated and not globally visible.
- Added invisible front drop guards to:
  - current/past house interior maps
  - current/past house exterior maps
- The guards use `BoxCollider` only, with no renderer, so they do not reintroduce the removed front wall visually.
- Added validation that the guards exist, have colliders, and are not visible.

## Validation

- `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch` passes.
- Build output:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- Runtime smoke log did not report `NullReference`, `Exception`, or `Error`.

## Notes

- The player is still on the neutral player layer, but that layer is now excluded from the main camera while Niro is in other time. This keeps the stability benefit while clipping his visible presence to the Time Window aperture.
- Unity still emits non-fatal `System.Numerics` messages from the Code Coverage package during build; the player build succeeds.
