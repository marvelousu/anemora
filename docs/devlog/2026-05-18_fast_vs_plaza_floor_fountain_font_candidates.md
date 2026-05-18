# 2026-05-18 Fast VS Plaza Floor, Fountain Collider, Font Candidates

## Request

- Extend the central plaza floor texture farther toward the back.
- Move/extend the left-side notice sign with the wider plaza.
- Make the top of the central fountain area not walkable.
- Start choosing a readable font, first from free options.

## Implementation

- Extended `CentralPlaza_PixelGround` depth from `18.8` to `21.4`.
- Expanded the central plaza back boundary and left/right invisible boundaries to match the deeper ground.
- Moved the left notice board farther back/left and added a small base plank so it reads as a placed sign, not a floating marker.
- Added invisible `CentralPlaza_FountainNoStepCollider` in both current and past maps so the player cannot step onto the center fountain top.
- Added validation that the fountain no-step collider exists, is invisible, and has enough size/height to block the player body.

## Font Candidate Notes

Free candidates checked first:

- `BIZ UDPGothic`: best first candidate for dialogue and UI. It is a universal design Gothic font intended for readability, and the project is under SIL Open Font License 1.1.
- `Noto Sans JP`: strong fallback and broad coverage option. Noto documentation states Noto fonts are under the Open Font License and can be bundled with apps.
- `M PLUS 2`: good secondary candidate when a slightly softer/classic-modern tone is desired. The project page describes Japanese support and SIL Open Font License usage.
- `DotGothic16`: useful for headings, location labels, or retro/pixel flavor only. It has a strong old-screen/game texture, but full dialogue may become tiring.

Proposed direction:

- Main dialogue/UI: `BIZ UDPGothic`
- Fallback/general: `Noto Sans JP`
- Title/location accent: `DotGothic16` only if the pixel tone improves the scene

Sources:

- https://github.com/googlefonts/morisawa-biz-ud-gothic
- https://notofonts.github.io/noto-docs/website/use/
- https://mplusfonts.github.io/
- https://github.com/fontworks-fonts/DotGothic16

## Verification

- Worker implemented the first pass in `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
- Main review added stricter validation for the fountain no-step collider.
- `git diff --check` passed for the edited setup file.
- Unity batch generation and validation passed via `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`.
- Player build succeeded:
  `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 18-second `-batchmode -nographics` smoke run produced no error, exception, crash, or `NullReference` log entries.

Known non-fatal batch warnings:

- Unity licensing access-token update warning.
- `RenderTexture.Create failed` warnings under `-nographics`.
