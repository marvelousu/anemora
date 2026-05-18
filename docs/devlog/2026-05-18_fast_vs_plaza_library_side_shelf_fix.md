# Fast VS plaza/library side shelf review fix

Date: 2026-05-18

## Scope

Addressed the latest visual/structure review notes for the Fast VS V24 sample:

- Library interior entrance no longer has a large front slab/wall-like threshold.
- Library current-side debris near the entrance is split into small board/dust pieces instead of one large gray plate.
- Library east/west side shelves are present on both current and past maps. Current shelves remain empty; past shelves contain book runs.
- Past library back-wall shelves use individual readable book runs instead of flat texture-panel placeholders.
- Current-side red time-window guidance lights use the same round floor-glow primitive family as map-move lights.
- Central plaza library facade uses framed doors/windows instead of flat blockout objects.
- The past plaza market awning is no longer a white floating box; it now has colored cloth, posts, and a counter.
- Dialogue and guide text paths default to typewriter presentation.

## Worker Cycle

- UI worker (`gpt-5.4-mini`) changed `FastVsStoryRuntimeHud.ShowGuide()` to use typewriter text and exposed `CharactersPerSecondForReview`.
- Scene worker (`gpt-5.4-mini`) partially applied scene-generation changes, then timed out.
- Parent session reviewed and integrated the scene changes, fixed the remaining entrance/debris issues, added stronger validation, and reran Unity build/capture/player checks.
- A read-only `gpt-5.4-mini` review pass reported: "チェックリスト上の未対応は見当たらない。"

## Files

- `<repo>/Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- `<repo>/Assets/Scripts/FastVS/FastVsStoryRuntimeHud.cs`
- `<repo>/Assets/Scenes/Anemora_FastVS_HouseSlice.unity`

## Validation

- Build and structure validation:
  - `<repo>/Logs/fast_vs_build_validate_20260518_plaza_library_side_shelf_fix_final.log`
  - Result: `Fast VS house slice validation passed.`
  - Result: `Build Finished, Result: Success.`
- Screenshot capture:
  - `<repo>/Logs/fast_vs_capture_review_20260518_plaza_library_side_shelf_fix_final.log`
  - Output directory: `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518`
- Player smoke:
  - `<repo>/Logs/fast_vs_player_smoke_20260518_plaza_library_side_shelf_fix_final.log`
  - No matching runtime exception patterns were found.

## Review Images

- Current library desk / entrance debris:
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/03_library_reto_desk.png`
- Past library shelves / book runs:
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/05_library_past_no_temp_people.png`
- Current plaza library facade:
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/07_plaza_library_facade_current.png`
- Past plaza library facade / market awning:
  - `<repo>/docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/08_plaza_library_facade_past.png`

## MCP Note

The Unity MCP foundation described by ADR-0010 is useful for this project because the recurring failures are scene-structure mistakes that should be caught by editor-side inspection. In this Codex session, no Unity MCP resource/tool was visible from the active MCP registry, so this pass used batchmode Unity validation plus screenshot review. Once the Unity Editor bridge is live, it should be added as a structure-inspection step before screenshot capture, while keeping the PNG/manual review gate.
