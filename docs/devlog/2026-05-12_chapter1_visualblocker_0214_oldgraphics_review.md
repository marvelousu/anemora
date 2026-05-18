# Chapter 1 Visual Blocker 0214 Oldgraphics Review

Date: 2026-05-12

Build reviewed:
`<temp>\anemora_ch1_playable_visualblockerfix_20260512_0214\Anemora_Chapter1.exe`

Evidence source:
`<temp>\anemora_ch1_visualblocker_visible_smoke_20260512_0214`

Repo-local evidence:
`docs/devlog/screenshots/chapter1_visualblocker_0214_oldgraphics_review/`

Contact sheet:
`docs/devlog/screenshots/chapter1_visualblocker_0214_oldgraphics_review/chapter1_visualblocker_0214_contact_sheet.png`

Patch preview:
`docs/devlog/screenshots/chapter1_visualblocker_0214_oldgraphics_review/chapter1_visualblocker_0214_patch_preview.png`

Patch manifest:
`docs/devlog/screenshots/chapter1_visualblocker_0214_oldgraphics_review/patch_manifest.json`

## Verdict

Needs polish. No current 0214 frame is a blocker for route comprehension, but the central plaza library destination still reads weaker than the rest of the route/background stack.

## Review Notes

- House interior wall/boundary readability: pass. The interior has enough back wall, floor, door, and furniture separation for closer route framing. The right-side boundary crop is dark but legible.
- Exterior/background scale and closer-camera coverage: needs polish, not blocker. The house facade and foreground route coverage read clearly. The large right route prop is visually useful as edge coverage, though it competes with the house and should remain secondary if another pass touches it.
- Camera-edge coverage: pass. The 0214 frames no longer expose an obvious empty/blank route edge in the reviewed path.
- Central plaza library facade/readboost visibility: needs polish. Frames `06_central_plaza_after_switch.png` and `07_central_plaza_library_prompt.png` show floor/debris coverage, but the requested library destination is still not the dominant north/back read. The viewer reads scattered props and the floor first, then a generic dark doorway/table shape.
- Library/reto composition readability: pass from route/background ownership. The library interior reads as a library, Reto placement is understandable, and no route/background element blocks the event composition. The left exterior side block is heavy but acceptable unless future closer framing exposes it more.

## Visual-Only Patch Started

Started `Chapter1VisualBlocker0214RouteCompositionPolish` as a no-Unity static patch package:

- Keep existing house interior and exterior coverage. Do not change route logic, TimeWindow, prompts, character assets, or plaza ground material.
- Add or activate a stronger central plaza north/back library facade read boost: wider roof cap, clearer rectangular facade mass, warm window/book-strip accents, and a darker but framed doorway.
- Place the read boost behind signs/debris and above the existing ground plane so `CentralPlazaRescue` remains the floor/material owner.
- If the library endpoint receives another close-camera pass, add only a left-boundary/crop mask to reduce the heavy empty side block; keep the interior shelf/counter/read table intact.

## Validation

- Generated 22-frame repo-local screenshot copy from the 0214 evidence source.
- Generated contact sheet: `1016x1838`.
- Generated patch preview from `02_exterior_after_switch.png`, `07_central_plaza_library_prompt.png`, and `08_library_after_switch.png`.
- Generated `patch_manifest.json` with owned scope and patch intent.
- No implementation edits, no TimeWindow edits, no character work.

## Next Concrete Task

Convert the static patch intent into a small visual-only plaza library readboost layer, reusing the existing route/background package pattern and limiting changes to facade/backdrop assets. Validate against the same 0214 frame list, with special attention to `07_central_plaza_library_prompt.png`.
