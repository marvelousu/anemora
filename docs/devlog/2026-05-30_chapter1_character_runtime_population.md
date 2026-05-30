# 2026-05-30 Chapter 1 Character Runtime Population

Status: Implemented

## Summary

- Promoted first-party Chapter 1 character sprite art into the FastVS character folders.
- Added runtime placement for Mia, Kaia, Dario, Karla, Kairo, Luna, plus ambient residents in existing Chapter 1 spaces.
- Added shared sprite-card breathing materials, contact shadows, import validation, and generated-scene validation.

## Verification

- `CreateHouseSliceScene`: pass.
- `ValidateHouseSliceBatch`: pass.
- `BuildAndValidateBatch`: pass; FilmGrain active count remained 20 after build.
- Player smoke: 18 seconds with no error or exception hits in the player log.
- Review screenshots were prepared for visual placement review.

## Notes

- Character placement is provisional and intended for visual review; final adjustment can move characters without changing canon.
- Generated scene, APV bake output, logs, and review screenshots remain uncommitted build artifacts.
