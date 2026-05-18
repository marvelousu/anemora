# 2026-05-18 Fast VS Library Depth/Mezzanine Prototype and DotGothic16 Lock

## Request

- Use `DotGothic16` as the current font direction.
- Correct the library map direction:
  - The previous pass widened the library too much.
  - Desired shape is roughly the earlier/narrower width, extended deeper toward the back.
- Try a more library-like structure, including a possible second-floor feeling.
- Treat the 2F library work as a trial that can be reverted if it feels wrong.

## Cycle

- Main session checked the current library dimensions and surrounding validation.
- A `gpt-5.4-mini` worker received a narrow instruction for only:
  - `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- Worker changed the library blockout and validation.
- Main session reviewed the worker output, then ran Unity validation/build and smoke.

## Map Changes

Updated:

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`

Library footprint:

- Changed `Library_PixelFloor` from the wide `16.6 x 11.8` footprint to `11.6 x 15.4`.
- Moved the back wall to local `Z +7.55`.
- Moved side wall hints to local `X +/-5.65`, with depth `14.8`.
- Updated front/back/side invisible boundary guards for the deeper footprint.

Library structure:

- Repositioned service desk toward the front-left.
- Repositioned reading table and open book into the mid-front area.
- Moved archive shelf, book strips, and sign toward the rear wall.
- Added visual-only second-floor/mezzanine pieces:
  - `Library_SecondFloor_LeftBalcony`
  - `Library_SecondFloor_RightBalcony`
  - `Library_SecondFloor_BackGallery`
  - `Library_SecondFloor_Railing_Left`
  - `Library_SecondFloor_Railing_Right`
  - `Library_SecondFloor_Railing_Back`
  - `Library_SecondFloor_LadderHint`
- All second-floor/mezzanine pieces are `keepCollider=false`, so they are visual blockout only and should not block player movement.

Validation:

- `ValidateLibraryMapLayout(...)` now rejects the old wide layout by requiring:
  - width between `11.2` and `12.4`
  - depth at least `14.5`
- Validation also checks that the second-floor balcony/gallery/railing objects exist for both current and past maps.

## Font Direction

- `DotGothic16` is now the selected baseline for the prototype direction.
- The preview tool still keeps other candidates for comparison, but the header/card now marks `DotGothic16` as selected.
- Updated:
  - `docs/font_preview/anemora_font_preview.html`

## Verification

- `git diff --check` passed for:
  - `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - `docs/font_preview/anemora_font_preview.html`
- Font preview reloaded at:
  `http://127.0.0.1:8765/docs/font_preview/anemora_font_preview.html`
- Browser snapshot showed:
  - `Selected: DotGothic16`
- Browser console only reported missing `favicon.ico`.
- Unity batch generation and validation passed via:
  `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Player build succeeded:
  `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 18-second `-batchmode -nographics` smoke run produced no error, exception, crash, or `NullReference` log entries.

Known non-fatal batch warnings:

- Unity licensing access-token update warning.
- `RenderTexture.Create failed` warnings under `-nographics`.
