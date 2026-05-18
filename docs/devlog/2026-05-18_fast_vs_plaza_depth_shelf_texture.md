# 2026-05-18 Fast VS Plaza Depth and Library Shelf Texture

## Request

- Push the central plaza space farther toward the back side.
- Move the library exterior deeper with the expanded plaza.
- Fix the missing texture on the past-side library back-wall shelf hint.
- Clarify whether Time Window generation/closing is governed by a timer.

## Changes

- Expanded `CentralPlaza` ground depth from `15.4` to `18.8`.
- Moved the plaza-to-library route pad deeper on the north/back approach.
- Moved the library facade, wings, roof, entrance, step, and windows deeper in the plaza.
- Expanded the invisible plaza boundary depth to match the larger walkable space.
- Changed `Library_ArchiveShelfHint` to always use the wood material, including the past-side map.
- Added two book-strip meshes on the archive shelf so the hint reads as a shelf instead of a flat untextured marker.

## Time Window Note

Time Window opening and closing are not currently managed by a cooldown timer in this slice.

- Closing from the current side should happen immediately.
- Closing while Niro is in the past/other-time side is intentionally rejected, because removing the window there would strand the player in the past-side space.
- If closing feels delayed, the most likely cause is that Niro is still considered inside the past/other-time side, so the close request is being rejected rather than delayed.

## Verification

- `git diff --check` for the edited C# paths passed.
- Unity batch generation and validation passed via `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`.
- Player build succeeded:
  `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 18-second `-batchmode -nographics` smoke run produced no error, exception, crash, or `NullReference` log entries.

Known non-fatal batch warnings:

- Unity licensing warning during batchmode startup.
- `RenderTexture.Create failed` warnings under `-nographics`.
