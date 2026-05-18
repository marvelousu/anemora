# 2026-05-18 Fast VS player sprite / door fix

## User-visible issues

- Player left/right sprite directions were reversed.
- Player feet appeared to float above the ground.
- Player sprite had no animation.
- A white object appeared above the player's head.
- The house exit/return door was not readable enough and door movement needed stricter validation.

## Changes

- Swapped the screen-left/screen-right material assignment for Niro.
- Changed Niro's active player materials to the v45 4-frame direction strips, cropped to one 64x96 frame at a time.
- Added frame animation in `FastVsDirectionalSpriteAnimator`.
- Lowered the sprite quad by the PNG's 2px transparent foot padding so the visible feet sit on the ground.
- Removed the player overhead sprite name label.
- Added visible interior exit door parts:
  - dark doorway gap,
  - door panel,
  - top/left/right frame.
- Added a clearer exterior door handle cue.
- Enlarged door trigger volumes slightly.
- Added review validation for:
  - v45 strip size `256x96`,
  - animation frame count,
  - no player `TextMesh` labels,
  - visible sprite feet grounding,
  - actual door transition execution from interior to exterior and back.

## Notes

The v45 strip assets are now used intentionally as frame strips. They are no longer used as a single 256px-wide front sprite.
