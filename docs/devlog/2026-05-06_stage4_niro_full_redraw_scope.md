# 2026-05-06 Stage 4 Niro Full Redraw Scope

## Summary

- Created `docs/asset_prompts/hero_v2_full_redraw.md`.
- Recorded the user decision that Niro / Hero is a Stage 4 full redraw, not a minor revision and not a hold.
- Kept this as a docs-only planning task. No PNGs, prefabs, scenes, ProjectSettings, asset ledger rows, or existing assets were created or edited.

## Inputs Read

- `docs/STAGE4_ROADMAP.md`
- `docs/STAGE4_PHASE0_TRIAGE.md`
- `docs/STAGE3_REVIEW_AIDS.md`
- `docs/STAGE3_TBD_RESOLUTION.md`
- `docs/STAGE3_RETROSPECTIVE.md`
- `docs/scene_tour_anemora_main.md`
- `docs/devlog/2026-05-05_g3_aseprite_residents.md`
- `docs/ASSET_STRUCTURE.md`
- `docs/legal/asset_ledger.md`

## Decision Recorded

Stage 4 should treat Niro / Hero as a full redraw. The existing Stage 3 `v1` Hero sprite set remains preserved as the provisional baseline and should not be overwritten.

The future redraw target is `Assets/Art/Sprites/Hero/v2/`, with later import and prefab wiring handled by a separate asset task.

## Scope Captured

The brief defines:

- Gender-neutral 15-19 Niro read.
- Original quiet wanderer hat silhouette direction inspired by a Snufkin-like mood without copying a protected character.
- Small top-down / isometric-ish sprite readability.
- Calm, quiet tone with no exaggerated emotion.
- Compatibility with Anemora palette v0 and current Zone1 backgrounds.
- Current Hero animation surface: stand, idle, walk front/back/left/right, with D-7 hands reviewed separately if needed.
- Transparent PNG, 32x48 frame-cell, PPU 32, point-filter, no-mipmap import assumptions.
- `v2` folder path and `v1` preservation rule.
- Resident_A/B review-first rule.

## NPC Boundary

Resident_A and Resident_B are not part of the automatic redraw scope.

They should be redrawn only if review finds a concrete age, gender, continuity, darkness, or style issue. If triggered, that should become a separate resident brief or an explicit update to `docs/asset_prompts/npc_residents.md`.

## Validation / Handoff

The brief includes a pre-import validation checklist, post-import validation checklist, and next worker handoff for actual generation/import.

Next worker should generate candidates under `art/_intermediate/hero_v2_full_redraw/`, produce accepted PNGs under `Assets/Art/Sprites/Hero/v2/`, import and preview in Unity, then update the asset ledger and a new devlog after real asset work is complete.

## Files Touched

- `docs/asset_prompts/hero_v2_full_redraw.md`
- `docs/devlog/2026-05-06_stage4_niro_full_redraw_scope.md`
