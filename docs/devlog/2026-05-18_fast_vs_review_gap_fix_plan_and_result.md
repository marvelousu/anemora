# Fast VS Review Gap Fix Plan And Result

Date: 2026-05-18

## Scope

User review flagged several visible gaps in the Fast VS V24 sample:

- Niro still appeared buried at the opening start.
- The lower-left guide log should be present from the start and remain available as objective guidance.
- The first house-exit brush event was still being skipped.
- Reto desk books overlapped and were too far from Reto.
- Past library side shelves and tables did not match the intended current/past contrast.
- Past library tables still included old chaotic table blockout objects.
- The returned current-side book appeared too early after taking the past book.
- The past target book did not disappear after pickup.
- The book prop readability and past back-wall shelf density were weak.
- Past plaza still had a stray temporary person-like object.

## Plan

1. Dispatch a bounded `gpt-5.4-mini` worker for scene-construction-only cleanup in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`.
2. In the parent session, review and integrate the worker result, then fix runtime story/HUD state where the issue is not purely scene layout.
3. Add or update validation so old behavior fails automatically:
   - past plaza temporary person must not exist,
   - past library must use clean table rows only,
   - past target book and marker must disappear after pickup,
   - returned current-side book must stay hidden until the Reto handoff conversation completes,
   - current book window cue must align with the past-side book coordinate.
4. Rebuild the generated scene/player, recapture screenshots, and run the generated EXE smoke test.

## Worker Use

Worker:

- Agent: `019e3880-7040-7f90-8e65-b4d21d188875`
- Model requested: `gpt-5.4-mini`
- Assigned write scope: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Worker output integrated:

- Raised and repositioned the initial Niro start.
- Repositioned current Reto desk books.
- Removed the past plaza vendor/person blockout.
- Added current-side side-shelf silhouettes.
- Added six orderly past clean reading tables.
- Improved readable book prop construction.
- Added validation coverage for several scene objects.

Parent corrections after review:

- Moved opening start to the bed side so the feet are less likely to sit behind the dialogue panel.
- Reworked the lower-left runtime objective HUD to stay persistent from the start.
- Raised the persistent objective HUD while a dialogue panel is visible so the guide panel does not overlap the conversation box.
- Fixed the house-exit brush beat trigger so it starts from the exterior active-area state.
- Removed the overlapping generic table book from Reto's desk.
- Removed current-side table blockouts from the past library.
- Moved the past target book to an orderly table and aligned the current-side red cue to that same coordinate.
- Made the returned current-side book appear only after the Reto handoff conversation completes.
- Added validation that the past target book and red marker are hidden after pickup.
- Increased the past back-wall shelf density with board rows and many readable book spines.

## Files Changed

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryFlowController.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scripts\FastVS\FastVsStoryRuntimeHud.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\INDEX.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\2026-05-18_fast_vs_review_gap_fix_plan_and_result.md`

## Validation

Build and scene validation:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_build_validate_20260518_review_gap_fix_rerun3.log`
- Result: success.

Screenshot capture:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_capture_review_20260518_review_gap_fix_rerun3.log`
- Result: success.

Review screenshots:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_story_reto_shadow_20260518\01_interior_niro_shadow.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_story_reto_shadow_20260518\03_library_reto_desk.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\docs\devlog\screenshots\fast_vs_story_reto_shadow_20260518\05_library_past_no_temp_people.png`

Generated EXE smoke test:

- EXE: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_player_smoke_20260518_review_gap_fix_rerun3.log`
- Result: success.

## MCP Note

The Unity MCP foundation is useful for this exact category of structural scene bug, but the active Codex tool list in this session did not expose a live Unity MCP scene API. This pass therefore used the existing Unity batch integrator, generated-scene validation, PNG review screenshots, and EXE smoke test. Once live `mcp-unity` handshake is available, it should be added after Integrator regeneration and before PNG review.

## Remaining Review Risk

The implementation now blocks the specific regressions above in validation, but final judgment on book visual quality, font readability, and opening-feet perception still needs direct Play review because those are camera/UI perception issues rather than pure hierarchy assertions.
