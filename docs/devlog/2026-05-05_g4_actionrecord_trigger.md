# 2026-05-05 G4 ActionRecord Trigger Wiring

## Scope

- Completed the VS_SCOPE 3.1 core loop wiring for the first current-side reflected object:
  Red portal selection -> Past crossing -> family book take record -> Current return -> book on bed.
- Kept changes scoped to G4 ActionRecord trigger, scene wiring, one current-side prefab variant, one PlayMode E2E test, and the asset ledger row.

## Implementation

- Added `take_book_001` to `ActionRecordCatalog.asset` with `ActionType.Take` and `SpawnBookOnBed`.
- Added `PastBookInteractable`, which accepts `E` or `Space` while the player is in range, writes an `ActionRecordEntry` through `ActionRecordRuntime.Instance`, then hides the past source book.
- Added `Book_Family_Current.prefab` as a visual-only current-side variant of the family book model on `Layer_Current_Visual`.
- Wired `Anemora_Main.unity`:
  - `ActionRecordRuntime` and `BookReflector` on `TimeFramePortalSystem`.
  - `BookSpawn_Bed` under `Current_BedPlaceholder`.
  - `ActionRecordReflections_Current` under `Root_Current`.
  - Past source book at the former `Past_BookPlaceholder` location.

## Verification Plan

- `G4ActionRecordReflectionE2ETests` loads `Anemora_Main`, opens the red portal, crosses to Past, interacts with the book, returns to Current, and asserts exactly one reflected book under `ActionRecordReflections_Current`.
- The same test repeats the Past/Current return and asserts no duplicate spawn.
