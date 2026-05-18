# 2026-05-18 Fast VS Visible Plaza Floor Center and Pixel Font Pass

## Request

- The previous plaza change expanded the wrong surface.
- The user meant the visible plaza floor/paving, not the broad grass/base ground.
- Put the fountain ruin at the center of the visible floor-covered area.
- Keep the new HTML font preview tool because it is useful, even though the original intent was the image preview workflow.
- Move the font direction slightly more toward dot/pixel, while avoiding unreadable over-stylization.

## Correction

Previous mistake:

- `CentralPlaza_PixelGround` was treated as the plaza floor.
- In the scene, the visible plaza floor the user is reacting to is `CentralPlaza_StoneSquare` plus the path pieces.

Current correction:

- Expanded `CentralPlaza_StoneSquare` from `7.2 x 5.6` to `12.8 x 12.2`.
- Moved `CentralPlaza_StoneSquare` to local Z `+2.25`.
- Moved the fountain ruin cluster to the same local Z `+2.25`, placing it at the center of the visible paved floor.
- Kept `CentralPlaza_FountainBase` collider disabled.
- Kept the tall invisible `CentralPlaza_FountainNoStepCollider` at `2.25 x 1.60 x 2.25`.
- Added validation that:
  - Current/Past stone squares are large enough.
  - Current/Past fountain no-step colliders stay aligned with the stone square center.
  - Fountain no-step colliders remain large/tall enough to block the player body.

## Font Preview Update

Kept and updated:

- `docs/font_preview/anemora_font_preview.html`

Added:

- `PixelMplus12` preview via Leafscape webfont.

Current font direction:

- `BIZ UDPGothic`: still strongest readability baseline.
- `DotGothic16`: mild dot-style vector font, useful for a readable pixel-adjacent look.
- `PixelMplus12`: stronger pixel look; promising if dialogue text is displayed larger with generous line-height.
- Best next test: compare `DotGothic16` and `PixelMplus12` in an in-game TextMeshPro dialogue box before committing.

Sources:

- PixelMplus distribution/license notes:
  https://itouhiro.hatenablog.com/entry/20130602/font
- Leafscape PixelMplus webfont preview:
  https://leafscape.be/fonts/118.html
- DotGothic16:
  https://github.com/fontworks-fonts/DotGothic16

## Verification

- Worker implemented the visible-floor correction in `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`.
- Main review confirmed the paved `CentralPlaza_StoneSquare` is now the expanded floor target.
- `git diff --check` passed for the edited setup file and font preview HTML.
- Browser preview loaded after adding `PixelMplus12`; no console errors or warnings were reported after reload.
- Unity batch generation and validation passed via `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`.
- Player build succeeded:
  `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 18-second `-batchmode -nographics` smoke run produced no error, exception, crash, or `NullReference` log entries.

Known non-fatal batch warnings:

- Unity licensing warning during batchmode startup.
- `RenderTexture.Create failed` warnings under `-nographics`.
