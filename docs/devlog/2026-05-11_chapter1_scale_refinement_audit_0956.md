# Chapter 1 Scale Refinement Audit After 0956

Date: 2026-05-11

## Baseline

- Current baseline from main: `anemora_ch1_playable_smokefix_20260511_0956`.
- Niro v12 at player visual child scale `0.60` is accepted.
- TimeWindow v3.2 volume cue is accepted.
- Per-section camera sizes from 0956 are accepted and should not be changed by this slice:
  - O `2.40`
  - S1 `2.60`
  - S2 `2.60`
  - S3 `2.70`
  - S4 `3.30`
  - S5 `3.05`

## Evidence Checked

Latest local gameplay-camera capture reports:

- `docs/devlog/screenshots/chapter1_gameplay_camera/20260511_095358/capture_report.md`
- `docs/devlog/screenshots/chapter1_gameplay_camera/20260511_101443/capture_report.md`

Both reports captured six current gameplay-camera views with `Errors: 0` and `Warnings: 0`:

- `route_start_niro_house`
- `scene1_library`
- `scene2_mia_house`
- `scene3_street_aria`
- `scene4_kaia_field`
- `scene5_north_ruins`

The 101443 report records `Character sprite state: v12-niro-placeholder-npcs`, so it is the relevant local evidence after the v12 import.

## Current Implementation State

`Assets/Editor/AnemoraChapter1SceneSetup.cs` still has the scale baseline:

- `Chapter1PlayableCharacterVisualScale = 0.60f`

It also still creates the old current plaza fountain object:

- `SpawnZonePrefab(... "Plaza_Fountain_Dry_Broken", "Ch1_Current_Plaza_Fountain" ...)`

The same setup then suppresses renderers whose hierarchy path contains:

- `Ch1_Current_Plaza_Fountain`
- `Plaza_Fountain_Dry_Broken`

So the current route avoids the prior ring-like fountain read by hiding that visual, not by replacing it with the graphics no-loop broken-fountain kit.

## Graphics Scale Refinement Kit Status

The graphics worktree has the kit:

- `Assets/Prefabs/Zone1/Chapter1ScaleRefinement/Ch1_ScaleRef_NiroHouse_RouteStartContext.prefab`
- `Assets/Prefabs/Zone1/Chapter1ScaleRefinement/Ch1_ScaleRef_Library_FirstObjectiveContext.prefab`
- `Assets/Prefabs/Zone1/Chapter1ScaleRefinement/Ch1_ScaleRef_CentralPlaza_BrokenFountain_NoLoop.prefab`
- `Assets/Prefabs/Zone1/Chapter1ScaleRefinement/Ch1_ScaleRef_StreetCorner_SignboardReadability.prefab`
- `Assets/Prefabs/Zone1/Chapter1ScaleRefinement/Ch1_ScaleRef_KaiaField_ScaleAnchors.prefab`

The implementation worktree does not currently contain:

- `Assets/Prefabs/Zone1/Chapter1ScaleRefinement/`
- `Assets/Art/Materials/Zone1/Chapter1ScaleRefinement/`
- `Assets/Editor/AnemoraChapter1ScaleRefinementKitBuilder.cs`

## Recommendation

Do not change Niro scale or cameras. The useful low-risk scale refinement is still the central plaza fountain replacement, but it should be done as a separate visual-only patch after the current gate-label/prompt path is stable.

Recommended implementation sequence:

1. Copy the scale-refinement kit and `.meta` files from graphics into implementation.
2. In `AnemoraChapter1SceneSetup`, keep the existing old-fountain renderer suppression as a fallback.
3. Spawn `Ch1_ScaleRef_CentralPlaza_BrokenFountain_NoLoop.prefab` under `Root_Current/Chapter1_Route_Current` at the current fountain position, visual layer only, no colliders.
4. Do not alter section camera sizes, gate labels/prompts, player root, player collider, milestone ranges, dialogue trigger radii, or TimeWindow membership.
5. Recapture:
   - central plaza / elder bench / fountain
   - first objective TimeWindow regression
   - route start / Niro house

The other scale-refinement prefabs are optional. Based on the latest pass evidence, they are not blockers now; import them only if graphics asks for additional prop-density or readability polish after reviewing the 0956/101443 captures.
