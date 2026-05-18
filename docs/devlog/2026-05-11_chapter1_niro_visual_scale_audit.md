# Chapter 1 Niro Visual Scale Audit

Date: 2026-05-11

## Scope

Implementation-side audit for the user report that Niro looks old and the player character is too large relative to the Chapter 1 map.

This pass does not import or generate any character art. Character identity / final Niro sprite approval remains user-driven.

## Current Runtime Wiring

- Scene: `Assets/Scenes/Anemora_Chapter1.unity`
- Player root: `Player`
- Root scale: `(1, 1, 1)`
- Runtime controller: `Anemora.Player.PrototypePlayerController`
- Root collider: `CapsuleCollider`, radius `0.22`, height `1.2`
- Visual children:
  - `Player/Player_Visual_Current`
  - `Player/Player_Visual_Past`
- Visual prefab source: `Assets/Prefabs/Characters/Hero.prefab`
- Visual sprite source: `Assets/Art/Sprites/Hero/v2/hero_idle.png`
- Sprite name observed by PlayMode guard: `hero_idle`

## Scale Decision

The active implementation already carries the graphics-audit correction:

- `AnemoraChapter1SceneSetup.Chapter1PlayableCharacterVisualScale = 0.60f`
- `Anemora_Chapter1.unity` serializes both `Player_Visual_Current` and `Player_Visual_Past` prefab instance overrides at local scale `(0.6, 0.6, 0.6)`.

No root/controller/collider scale change was made. Movement speed, movement collision radius, Time Window membership, milestone ranges, and dialogue proximity radii are intentionally independent from the visual child scale.

Fallback `0.64f` was not used because the scene and test guard accept `0.60f`.

## Sprite State

The currently wired Niro/player visual is still the existing `Hero` v2 sprite set. It is functional for route/playability validation, but it should be treated as placeholder/old for identity review:

- It predates user-approved final Chapter 1 Niro import.
- It is shared through the generic `Hero.prefab` path rather than a Chapter 1 final Niro asset.
- The implementation patch keeps this asset in place to avoid unapproved character import.

## Guard Added

`Chapter1PlayableFlowControllerTests.PlayerVisualScaleCorrectionPreservesColliderAndMilestoneReachability` now verifies:

- Player root scale remains `(1, 1, 1)`.
- Player collider radius/height remain unchanged.
- Both current/past visual children are local-position zero and local-scale `0.60`.
- Both visual children still render `hero_idle`.
- Primary route milestone interaction ranges remain `2.6`.
- Milestone range checks still succeed at the corrected visual scale.

## Recommended Capture List

After build/capture is available, review these views for map-to-character proportion:

- `route_start_niro_house`
- `scene1_library` / first objective
- central plaza / elder bench
- scene3 street / signboard
- scene4 field
- scene5 north ruins
