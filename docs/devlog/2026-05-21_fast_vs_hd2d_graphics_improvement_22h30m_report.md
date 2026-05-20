# 2026-05-21 Fast VS HD2D Graphics Improvement 22h30m Report

## Scope

- Workstream: Fast VS playable range HD-2D graphics improvement.
- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Current branch: `work/fast-vs-hd2d-polish-20260520`
- Base branch: `work/post-vs-public-20260518`
- Current branch relationship: `work/post-vs-public-20260518` is an ancestor of `work/fast-vs-hd2d-polish-20260520`; this branch is 51 commits ahead of that base before the current uncommitted Cycle54/report work.
- Push state at report time: `work/fast-vs-hd2d-polish-20260520` has no remote tracking branch yet and has not been pushed.
- Report timing: written after roughly 22.5 hours of continuous graphics-focused iteration.

## Operating Model

- The work followed the requested cycle: parent session designs a narrow plan and worker instructions, `gpt-5.4-mini` worker implements bounded changes, parent session reviews screenshots/logs, accepts or rejects, then commits accepted cycles.
- Main/public branch was not used for this workstream. The public VS state remains separate.
- Changes were kept in small commits where accepted. Failed visual attempts were reverted instead of left as partial work.
- The core gameplay contract was preserved: VS route, time-window behavior, map transitions, story/event implementation, and font/dialogue systems were treated as regression-sensitive and not intentionally modified during HD-2D polish cycles.

## Accepted Work Summary

- Local shape and atmosphere:
  - Added first/second HD-2D atmosphere passes.
  - Added surface texture pass, object-detail pass, hero-prop texture pass, depth framing, and close review screenshot infrastructure.
  - Balanced lighting so the result avoids simply darkening the whole scene.
- Niro house interior:
  - Added wall/floor warmth, room depth, life props, bed textile details, soft textile details, and current Cycle54 layered textile slabs.
  - Added small prop and floor accents while keeping the compact scale that already matched the character scale well.
- Niro house exterior:
  - Added house exterior detail, facade texture, porch/road dressing, tree silhouette work, external tree sprite use, and hedge sprites.
  - Kept collision shells separate from visual sprite panels where external sprites are used.
- Central plaza:
  - Added plaza detail, floor/fountain polish, library approach steps/curbs, and tree line sprites.
  - Kept the plaza readable as a wider approach space rather than a dense object field.
- Library facade:
  - Added facade detail, close detail, architecture improvements, door/window visibility, and approach framing.
  - Earlier sky/backdrop attempt was rejected because it looked too rough and was reverted.
- Library interior:
  - Added prop detail, reading table detail, bookshelf readability, side shelf cleanup, upper gallery, current-side atmosphere, current ruin floor detail, wall shelf depth, window light, and external bookshelf texture pass.
  - Past library now uses a CC0 bookshelf source to read more like filled shelves; current library keeps an empty/ruined contrast.
- Character grounding:
  - Added contact shadow, foot contact, and ground bounce passes.
  - These improved attachment to the ground without reworking the character sprite pipeline.

## External Asset Usage

- Adopted CC0 tree sprite:
  - Source page: https://opengameart.org/content/tree-sprites-0
  - Direct source file: https://opengameart.org/sites/default/files/tree3_0.png
  - Author credit on source page: `edomin`
  - License: CC0
  - Local source path: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\External\OpenGameArt\edomin_tree_sprites_cc0\tree3_0.png`
  - Local attribution path: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\External\OpenGameArt\edomin_tree_sprites_cc0\README.md`
- Adopted CC0 bookshelf texture:
  - Source page: https://opengameart.org/content/bookshelf-3
  - Direct source file: https://opengameart.org/sites/default/files/bookshelf_2.png
  - Author: `AlejandroHaibi`
  - License: CC0
  - Local source path: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\External\OpenGameArt\alejandrohaibi_bookshelf_cc0\bookshelf_2.png`
  - Local attribution path: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\External\OpenGameArt\alejandrohaibi_bookshelf_cc0\README.md`
- Paid assets:
  - No paid asset was adopted.
  - No paid asset was downloaded into the repository.
  - If the library becomes a visual showcase, a paid or custom large library bookshelf/background set may be worth evaluating before adoption.

## Rejected Or Reverted Attempts

- Sky/background attempt:
  - A CC0 OpenGameArt sky source was tried, but it looked too rough and did not frame the exterior maps well.
  - The attempt was reverted and not committed.
- External 2D bed sprite attempt:
  - A CC0 top-down bed sprite source was tried as a visual overlay.
  - Parent review showed it read as a floating flat board on top of the 3D bed, so the entire attempt was reverted.
  - Cycle54 deliberately changed direction: no large bed sprite panel, only small non-colliding 3D textile slabs on the existing bed geometry.

## Current Cycle54 State

- Implemented but not yet committed at this report timestamp:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-20_fast_vs_hd2d_house_bed_layered_textile_cycle.md`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_bed_layered_textile_20260520\`
- Visual review:
  - Accepted as a small improvement candidate because it does not reproduce the failed floating-sprite-board problem.
  - Improvement is subtle; it adds textile depth but does not fully redesign the bed asset.
- Parent validation:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle54_bed_layered_textile_parent_validate_20260521.log`
  - Result: `Fast VS house slice validation passed.`
- Parent screenshot capture:
  - First parent capture hit a transient Unity project-open lock.
  - Retry log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle54_bed_layered_textile_parent_capture2_20260521.log`
  - Result: `Fast VS fifty-fourth-cycle screenshots captured`.
- Parent build:
  - First parent build hit the same transient Unity project-open lock.
  - Retry log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle54_bed_layered_textile_parent_build2_20260521.log`
  - Result: validation passed and `Build Finished, Result: Success.`
- Parent player smoke:
  - Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle54_bed_layered_textile_parent_smoke_20260521.log`
  - Result: stopped after 20 seconds, `match_count=0`.

## Evidence Inventory

- HD-2D devlogs currently present for 2026-05-20: 53 Fast VS HD-2D cycle files, plus one post-VS branch strategy record.
- Screenshot evidence under `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\` currently totals 442 linked evidence files after Cycle54 capture.
- Representative evidence directories:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_interior_room_depth_20260520\`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_exterior_external_tree_sprite_20260520\`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_plaza_library_approach_20260520\`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_library_bookshelf_external_texture_20260520\`
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_house_bed_layered_textile_20260520\`

## Known Risks

- Visual quality is improved but still fundamentally built from procedural cubes/slabs plus limited pixel textures. Some props remain placeholder-grade compared with custom illustrated HD-2D production art.
- The library bookshelf external source is CC0 and useful, but it is a small source image tiled over a large surface. It reads better than the procedural shelf, but a larger custom source would be stronger.
- Niro's bed has improved textile detail, but the asset still depends on block geometry. A full custom bed model or hand-authored sprite/model hybrid may be needed later.
- Unity batch operations generate noisy scene/project/material side effects. These need cleanup before each commit.
- The branch has not been pushed yet. Current accepted work is local until explicitly pushed.
- This report includes local full paths for operational clarity. If this branch is pushed to a public-facing repository, paths and internal process notes may need another sanitization pass.

## Recommended Next Steps

1. Commit the accepted Cycle54 bed layered textile pass and this report after cleaning Unity-generated side effects that are not part of the intended change.
2. Push `work/fast-vs-hd2d-polish-20260520` if this HD-2D branch should be backed up remotely as-is.
3. Continue HD-2D polish in small visual slices, still using parent plan -> 5.4-mini worker -> parent screenshot review -> validation/build/smoke -> commit.
4. Prefer external/API assets only for targets where procedural cubes are visibly failing: library shelf panels, large environmental backdrops, and hero props.
5. For paid assets, evaluate and report before adoption; do not import paid content without explicit approval.
