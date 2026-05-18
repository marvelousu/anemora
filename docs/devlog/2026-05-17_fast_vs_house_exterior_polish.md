# Fast VS House Exterior Polish

Date: 2026-05-17
Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
Branch: `codex/fast-vs-v24-sample-20260517`

## Cycle Plan

- Fix the reported 90-degree player rotation after entering the Time Window.
- Keep the house slice scope limited to Niro's house interior/exterior.
- Increase exterior density without importing the rejected current map graphics.
- Reuse existing Zone 1 music and ambience if the clips are already present.
- Validate with the same V24 same-coordinate transfer checks.

## Worker Findings Reviewed

- Rotation is not caused by the V24 transfer controller. The controller only changes player
  position and material/layer state.
- The rotation came from `FastVsPaperBillboard` being attached to the player root, which also
  owns the `CharacterController`.
- Existing audio assets are available under `Assets/Audio`, including `Zone1_Ambient.ogg`,
  wind, and birds.
- Paid external assets are not needed for this slice. Procedural point-filtered materials and
  simple geometry are enough for this review pass.

## Implemented

- Moved the Niro paper billboard to a child visual object:
  `FastVS_PlayerVisual_NiroPaper`.
- Kept the player root stable for `CharacterController` and V24 transfer coordinates.
- Added existing audio loops:
  - `Assets/Audio/Music/Zone1_Ambient.ogg`
  - `Assets/Audio/SFX/env/sfx_env_wind_loop_01.ogg`
  - `Assets/Audio/SFX/env/sfx_env_birds_01.ogg`
- Added exterior detail primitives on both current and past sides:
  - roof ridge and under-eave shadow
  - door step and porch posts
  - window trim, sill, and shadow lip
  - flower bed and small flower clusters
  - water barrel with water top
  - wood pile
  - clothesline, rope, and cloth sheets
  - way sign
  - grass tufts
- Added current-only damage details:
  - splintered roof patch
  - loose door plank
  - extra collapsed fence pile remains
- Added past-only lived-in details:
  - hanging lantern glow
  - repaired fence gate

## Validation Target

Run `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`.
The batch must still create the scene, open the V24 Time Window, create the live aperture, transfer
current to past, transfer past to current, reject old visual-reference tokens, and build the Windows
player.
