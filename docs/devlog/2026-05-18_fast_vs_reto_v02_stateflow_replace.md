# 2026-05-18 Fast VS Reto v02 Stateflow Replacement

## Request

- The Reto graphic used in the Fast VS build was the old version and should be completely discarded.
- Use the confirmed animation-included Reto version.

## Source Decision

Accepted source:

- `C:\Users\maro6\Documents\Unity\Anemora-stage4-hero-v2\docs\review_gallery\imports\stage4_character_stateflow_minimum_v02_reto_scale_2026-05-11\`

Reason:

- `stage4_character_stateflow_minimum_v02_reto_scale_status.md` states that Reto v02 is the seated/chair scale correction.
- The manifest marks Resident_B / Reto as user accepted final state-flow.
- The pack includes the required animation states:
  - writing normal loop
  - normal to talk / lower arms
  - talk loop / face raised breath
  - talk to normal / raise arms

Discarded:

- `Assets/Art/Sprites/NPC/Resident_B/`
- The old generated `reto_idle_writing_sprite` material and shaded texture.
- The temporary kitbashed arm / pen overlay implementation.

## Imported Assets

Copied into:

- `Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png`
- `Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png`
- `Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png`
- `Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png`

## Implementation

Updated:

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- `Assets/Scripts/FastVS/FastVsRetoWritingAnimator.cs`

Reto is now driven by full stateflow sprite strips rather than separate overlay quads.

Runtime mapping:

- Writing / hand raised: `reto_writing_normal_loop_v02_6f_64x96.png`
- Talk starts / lower arms: `reto_lower_arms_v02_6f_64x96.png`
- Dialogue idle / looks up: `reto_talk_loop_v02_4f_64x96.png`
- Conversation ends / raise arms: `reto_raise_arms_v02_6f_64x96.png`

Validation now rejects:

- old `Resident_B` / `resident_b_idle` paths for Fast VS Reto
- old arm / pen overlay scene objects
- missing or incorrectly sized v02 stateflow strips

## Visual Review

Screenshots regenerated:

- `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/03_library_reto_desk.png`
- `docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/04_library_reto_talk_loop.png`

Observed result:

- Reto now appears as the scale-corrected seated v02 sprite.
- The old large seated idle Reto is gone.
- The temporary overlay arms / pen are gone.
- Talk-loop state renders from the v02 talk-loop strip.

## Verification

- `git diff --check` passed.
- Unity batch validation passed:
  - `Fast VS house slice validation passed.`
- Player build succeeded:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 18-second `-batchmode -nographics` smoke run produced no error, exception, crash, or `NullReference` log entries.

Known non-fatal batch warnings:

- Unity licensing access-token update warning.
- Existing Code Coverage `System.Numerics.Vector*` resolution warnings.
