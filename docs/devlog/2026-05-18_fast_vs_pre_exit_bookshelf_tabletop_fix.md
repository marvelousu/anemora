# Fast VS Pre-Exit Event, Bookshelf, And Tabletop Fix

Date: 2026-05-18

## Scope

User review found three issues:

- The brush event fired after leaving Niro's house, but it should fire just before leaving.
- The current library side objects should read as empty bookshelves, and the past side should use matching bookshelves stocked with books.
- Several desk/table books were floating instead of resting on the tabletop.

## Plan

1. Assign the bookshelf and tabletop object placement to `gpt-5.4-mini` with a single-file write scope.
2. Keep the house-exit event logic in the parent session because it touches runtime transition behavior.
3. Review the worker diff, correct any overly broad or physically wrong placement, then add validation.
4. Rebuild, recapture review screenshots, and run the generated EXE smoke test.

## Worker Use

Worker:

- Agent: `019e38a6-8510-78c1-8e2e-d6a7864de41f`
- Model requested: `gpt-5.4-mini`
- Assigned write scope: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Worker result:

- Rebuilt current/past library side shelf construction around a shared frame helper.
- Kept current side shelves empty.
- Stocked past side shelves with book rows.
- Added bookshelf parity validation.
- Lowered table books.

Parent review corrections:

- The worker lowered tabletop books too far, so the parent restored the book roots to the actual table surface band.
- Validation now rejects both floating books and books sunk into the tabletop.
- The house exit event now intercepts the interior-to-exterior transition before the area change. The player remains in the interior map for the brush event, then the same door trigger proceeds to the exterior after the event completes.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsAreaDoorTransition.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryFlowController.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\2026-05-18_fast_vs_pre_exit_bookshelf_tabletop_fix.md`

## Validation

Build and scene validation:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_build_validate_20260518_pre_exit_bookshelf_tabletop_fix_rerun2.log`
- Result: success.

Screenshot capture:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_capture_review_20260518_pre_exit_bookshelf_tabletop_fix.log`
- Result: success.

Generated EXE smoke test:

- EXE: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_player_smoke_20260518_pre_exit_bookshelf_tabletop_fix.log`
- Result: success.

Updated screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_story_reto_shadow_20260518\01_interior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_story_reto_shadow_20260518\03_library_reto_desk.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_story_reto_shadow_20260518\05_library_past_no_temp_people.png`

## Remaining Review Risk

The event ordering is structurally validated, but the exact user feel of stopping at the door should be checked in the interactive EXE. The side bookshelves now have matching current/past structure, but visibility at the current camera angle is still a visual review item.
