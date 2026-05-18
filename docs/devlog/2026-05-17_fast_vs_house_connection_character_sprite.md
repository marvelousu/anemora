# Fast VS House Connection And Character Sprite

Date: 2026-05-17
Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
Branch: `codex/fast-vs-v24-sample-20260517`

## Purpose

Connect Niro's house interior and exterior more clearly and replace the temporary blocky player
paper-card with an approved Niro review sprite where possible.

## Implemented

- Added explicit interior/exterior route segments:
  - interior door to broad route
  - broad route across the 42m coordinate field
  - broad route to exterior front door
- Split the exterior facade into left/right/lintel wall panels so the door has a real walkable
  opening instead of a full-wall collider.
- Changed the exterior door from a closed door card to an open door panel plus a dark doorway gap.
- Added current/past door glow markers at both the interior and exterior door positions.
- Imported approved Niro v45 64x96 review sprites into:
  `Assets/Art/Characters/FastVS/Niro/`
- Replaced the player visual with a textured Niro sprite quad while keeping the
  `CharacterController` root stable.
- Replaced the past memory Niro placeholder with the same Niro sprite material tinted for the
  other-time side.

## Notes

- The sprite is intentionally still a billboard child, not the player root, so the earlier
  Time Window entry rotation issue should remain fixed.
- Directional sprite switching is not implemented yet. This pass uses the approved front idle
  sprite for fastest review.
- No paid external asset was added in this cycle.

## Validation Target

Run `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`, then smoke the player.
The V24 current/past same-coordinate transfer behavior must remain intact.
