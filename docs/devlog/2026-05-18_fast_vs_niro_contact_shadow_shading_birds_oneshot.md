# 2026-05-18 Fast VS Niro Contact Shadow, Sprite Shading, and Bird One-Shot

## Request

- Implement hero shading through step 2:
  - Step 1: soft contact shadow under Niro.
  - Step 2: subtle sprite-body shading overlay.
- The exterior bird chirp should not loop forever. It should play only once when Niro first goes outside.

## Cycle

- Main session inspected the player sprite hierarchy, time-window material use, and audio setup.
- A `gpt-5.4-mini` worker implemented the first player shading pass in:
  - `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- A second `gpt-5.4-mini` worker implemented the one-shot exterior bird cue in:
  - `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - `Assets/Scripts/FastVS/FastVsOneShotAreaAudioCue.cs`
- Main review corrected the contact shadow orientation and added validation that the time-window player renderer lookup still resolves to the actual Niro sprite, not the new shadow or overlay.

## Player Visual Update

Added:

- `FastVS_PlayerContactShadow_Niro`
  - Soft transparent oval placed under Niro.
  - Parent: player root, not the paper sprite visual.
  - Collider-free and label-free.
  - Rotated onto the ground plane.
- `FastVS_PlayerSpriteShadingOverlay_Niro`
  - Transparent generated texture layered over `Niro_Sprite64x96`.
  - Adds a subtle side/bottom darkening pass while leaving the source character art untouched.
  - Collider-free and label-free.

Generated texture assets:

- `Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_contact_shadow.asset`
- `Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_shading_overlay.asset`

Generated material assets:

- `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_contact_shadow.mat`
- `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_shading_overlay.mat`

## Audio Update

Changed the exterior birds from a looping ambient source to a one-shot area cue:

- Removed the looping `FastVS_Audio_HouseYardBirds` source from generation.
- Added `FastVS_Audio_HouseYardBirds_OneShotOnExterior`.
- Added `FastVsOneShotAreaAudioCue`, which waits until `FastVsHouseAreaVisibility.ActiveAreaForReview` becomes `Exterior`, then plays the bird clip once and never repeats during that run.

Music and wind remain looping ambience.

## Validation

Added validation for:

- Niro still has no debug/TextMesh labels.
- The old sprite-ground-shadow object is not present.
- Contact shadow exists under the player root, has no collider, is ground-plane rotated, and has no label.
- Sprite shading overlay exists under the paper visual, has no collider, and renders in front of the base sprite.
- The first player child renderer still resolves to `Niro_Sprite64x96`, protecting the time-window player material path.
- The old looping bird source is absent.
- The new bird cue has `loop == false`, `playOnAwake == false`, and a `FastVsOneShotAreaAudioCue` component.

## Verification

- `git diff --check` passed for:
  - `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - `Assets/Scripts/FastVS/FastVsOneShotAreaAudioCue.cs`
- Unity batch generation and validation passed via:
  - `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Player build succeeded:
  - `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 18-second `-batchmode -nographics` smoke run produced no error, exception, crash, or `NullReference` log entries.

Known non-fatal batch warnings:

- Unity licensing warning during batchmode startup.
- `RenderTexture.Create failed` warnings under `-nographics`.
- Existing Code Coverage assembly resolution warnings around `System.Numerics.Vector*`.
