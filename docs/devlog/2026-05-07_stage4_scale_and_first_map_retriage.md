# Stage 4 Scale and First Map Re-Triage

Status: v0.1 recorded 2026-05-07

## 1. Trigger

User runtime review after the Resident_A P1 import raised three connected issues:

- Resident_A and Resident_B may read too large, or Hero may read too small.
- Resident_A eyes are unstable between animation frames.
- The current first playable map looks bland, object scale feels wrong, and the map is too small to read as the intended first zone.

This is not treated as a small character-only polish item. It exposes a missing Stage 4 gate: character scale, prop scale, camera framing, and Zone1 first-map layout must be rebaselined before broader content expansion.

## 2. Findings

`Anemora_Main` remains valid as the Stage 3 Vertical Slice minimum playable and wiring verification scene. It is not accepted as the finished first map for Antela.

Read-only scene and asset audits found:

- Hero, Resident_A, and Resident_B all use `32 x 48` sprite cells, PPU `32`, bottom-center pivots, prefab scale `1`, and a nominal `1.5` unit world height.
- The perceived size issue comes mostly from visible sprite mass, head / face ratio, pixel density, and camera context rather than a simple prefab scale mismatch.
- Resident_B reads wider because the visible idle mass is about `25 x 45` px, compared with Hero at about `19 x 45` px.
- Resident_A reads unstable because face / eye pixels and head / hair contour shift between frames.
- Current / Past floor scale in `Anemora_Main` is about `7.5 x 8.5` units, with several Zone1 prop instances scaled by ad hoc import factors. This reads as a compressed demo board rather than a city block.

## 3. Decision

Stage 4 Phase 1 is re-prioritized:

1. Establish `docs/level_design/scale_metrics.md`.
2. Establish `docs/level_design/zone1_antela_first_map.md`.
3. Treat Zone1 first-map greybox, scale lineup, and user screenshot review as an entry gate before Zone 2-6 expansion.
4. Keep `AnemoraDemoSceneSetup.cs` / current `DemoZone1_*` layout as a demo / regression fixture unless explicitly promoted through a first-map review gate.
5. Do not solve Resident size concerns by prefab scale alone. Update art bbox, face ratio, pixel density, frame consistency, prop scale, and camera context together.

## 4. New Docs

- `docs/level_design/scale_metrics.md`
  - Defines the current character measurements, provisional `1 Unity unit = 1 m` scale rule, character target table, environment target table, camera framing rules, and acceptance checklist.
- `docs/level_design/zone1_antela_first_map.md`
  - Defines the current `Anemora_Main` as Stage 3 VS / wiring nucleus, not final first map; proposes the Antela first-map structure, Current/Past environmental storytelling, greybox pass order, and user review gates.

## 5. Next Dispatch

- Character scale pass: produce a same-plane lineup screenshot / clip of Hero, Resident_A, Resident_B, door, bed, table, fountain, and library scale references.
- Resident_A art fix: stabilize eye / face pixels across idle and walk frames while matching Hero / Resident_B pixel density.
- Zone1 greybox pass: block out Niro house -> central plaza -> library ruin -> side lane / small plaza, then review top-down and 1280 / 1920 gameplay screenshots before scene finalization.
- Scene tooling decision: split production first-map layout from `AnemoraDemoSceneSetup.cs` demo fixture or add a separate production layout source.

## 6. Verification

Docs-only change. No Unity scene, prefab, sprite, test, or build output was changed in this re-triage step.
