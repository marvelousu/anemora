# 2026-05-05 F4 Hero NPC Prefab Animator

## Scope

- Built stable character prefab and Animator structure around the F2 v1 sprite assets.
- Replaced the A2 current/past player visual placeholders in `Anemora_Main.unity` with `Hero.prefab` instances.
- Kept the task scoped to prefab/controller wiring, sprite import settings, tests, and this devlog. `asset_ledger.md` was not changed.

## Implementation

- Added `Hero.prefab` with `SpriteRenderer`, `Animator`, and `HeroAnimatorBinder`.
- Added `Resident_A.prefab` with `SpriteRenderer` and `Animator`.
- Added `Resident_B.prefab` with `SpriteRenderer` and an Idle-only `Animator`.
- Added three AnimatorControllers:
  - `HeroLocomotion.controller`: Idle, Walk, `isMoving`, `facing`.
  - `ResidentALocomotion.controller`: Idle, Walk, `isMoving`, `facing`.
  - `ResidentBIdle.controller`: Idle only.
- Added `HeroAnimatorBinder` as a temporary F-track bridge for `PrototypePlayerController`:
  - observes player root movement,
  - writes Animator parameters,
  - selects front/side/back sprites,
  - flips the side sprite for left-facing movement.
- Updated F2 sprite import metadata to PPU 32, Point filtering, no mipmaps, transparent alpha, Clamp wrap, uncompressed textures, 1024 max size, and bottom-center pivots.

## Scene Wiring

- `Player_Visual_Current` is now a `Hero.prefab` instance under `Player`, on layer 10.
- `Player_Visual_Past` is now a `Hero.prefab` instance under `Player`, on layer 11.
- Both scene instances bind `HeroAnimatorBinder.playerController` and `observedTransform` to the existing player root/controller.

## Verification Plan

- EditMode: `CharacterPrefabStructureTests`
  - prefab component structure,
  - Animator state and parameter contract,
  - sprite slicing and import settings.
- PlayMode: `HeroAnimatorBinderTests`
  - movement toggles `isMoving`,
  - facing updates front/side/back,
  - left movement uses `SpriteRenderer.flipX`,
  - main scene contains current/past Hero prefab visual instances.
