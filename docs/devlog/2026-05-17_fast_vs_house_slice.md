# Fast VS House Slice

Date: 2026-05-17
Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
Branch: `codex/fast-vs-v24-sample-20260517`

## Purpose

Build the first VS implementation slice after the visual direction sample.
This slice is intentionally limited to Niro's house interior and exterior so the map scale,
surface treatment, paper character style, and V24 TimeWindow behavior can be reviewed before
the central plaza and library are added.

## Canon Inputs

- `HouseInteriorCenter = (-8.35, 0, -8.35)`
- `HouseExteriorCenter = (8.20, 0, 8.20)`
- V24 TimeWindow behavior remains the base: grounded drag window, current/past same local
  coordinate pairing, aperture view, current-to-past transfer, return transfer, and back-side
  blocking.

## Implemented

- Scene: `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
- Build target: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- One broad paired map root for current and one matching paired map root for past.
- Niro's house interior and exterior placed at the canonical local coordinates above.
- A wide symbolic route connects the interior and exterior to keep the map scale intentionally
  roomy.
- Building surfaces use generated point-filtered pixel textures under
  `Assets/Art/Textures/FastVS/HouseSlice`.
- Materials are kept under `Assets/Art/Materials/FastVS/HouseSlice`.
- Niro remains a flat paper-card character, matching the visual direction sample.
- Past side has brighter lived-in house materials, warm window light, a book on the table, and
  a memory silhouette near the exterior.
- Current side has darker, broken, dustier surfaces and a red Timewriter cue in the interior.
- Niro starts on open floor inside the house, beside the bed rather than inside the bed volume.

## Excluded

- Central plaza and library.
- Full story progression.
- Existing Chapter 1 map prefabs, current graphics leftovers, GfxPolish, Meshy furniture,
  TimeWindow_Diorama, and V32 cue assets.
- Final character sprite import. This pass still uses paper-card placeholders.

## Validation

The batch method `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
must:

- create the scene;
- open a V24 TimeWindow by test drag;
- verify live aperture creation;
- transfer current to past;
- transfer past back to current;
- reject old visual-reference tokens (`GfxPolish`, `Meshy`, `DQ3R`, `TimeWindow_Diorama`, `V32`) in
  the generated scene;
- build the Windows player.

Manual review should check:

- the interior reads as a house room;
- the exterior reads as a simple Niro house yard;
- surfaces read closer to 2D pixel art than plain colored primitives;
- the map feels broader than the previous one-room sample;
- the TimeWindow still feels like V24.
