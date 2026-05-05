# Hero v2 Full Redraw Art Brief

Status: Stage 4 Phase 0 planning brief (2026-05-06)

This is a docs-only asset brief. It records the Stage 4 decision and handoff for a future asset generation/import worker. Do not create or edit PNGs, prefabs, scenes, ProjectSettings, the asset ledger, or existing assets as part of this brief.

## 1. Decision

User decision: treat Niro / Hero as a full redraw for Stage 4, not a minor revision and not a hold.

The Stage 3 `v1` Hero sprite set remains the preserved provisional baseline. Stage 4 `v2` should be generated beside it, reviewed, imported, and wired only in a later asset task after this brief.

Relevant preserved baseline:

- `Assets/Art/Sprites/Hero/v1/hero_stand.png`
- `Assets/Art/Sprites/Hero/v1/hero_idle.png`
- `Assets/Art/Sprites/Hero/v1/hero_walk_front.png`
- `Assets/Art/Sprites/Hero/v1/hero_walk_back.png`
- `Assets/Art/Sprites/Hero/v1/hero_walk_left.png`
- `Assets/Art/Sprites/Hero/v1/hero_walk_right.png`
- `Assets/Art/Sprites/Hero/v1/hero_hands_d7.png`
- `Assets/Prefabs/Characters/Hero.prefab`
- `Assets/Animators/HeroLocomotion.controller`
- `Assets/Animators/Clips/Hero_Idle.anim`
- `Assets/Animators/Clips/Hero_Walk.anim`

## 2. Visual Requirements

Niro must read as:

- Gender-neutral, with no clearly male-only or female-only silhouette cues.
- Age range 15-19.
- A quiet Antela ordinary person, not a chosen-one hero, royal, mage, warrior, mascot, or comedy figure.
- Calm and restrained: neutral closed mouth, no exaggerated joy, anger, fear, crying, shouting, or dramatic pose.
- Readable at small top-down / isometric-ish gameplay scale, with a clean full-body silhouette.
- Compatible with Anemora palette v0, Zone1 muted backgrounds, current UI/font tone, and the quiet Stage 3 scene mood.

Hat direction:

- Use a broad, soft, travel-worn hat silhouette direction that evokes a wandering, Snufkin-like mood.
- Do not copy Snufkin or any protected character design. Avoid exact green outfit, feather placement, face shape, scarf, pipe, character-specific proportions, or recognizable costume combinations.
- The hat should be an original readable silhouette cue for Niro: slightly drooping brim or soft pointed crown is acceptable if it remains subtle and original.

Top-down / isometric-ish readability:

- Front, back, and side views must keep the same character identity.
- Hat brim, torso mass, legs, and arm positions must remain legible in 32x48-style gameplay use.
- Avoid hair/hat shapes that merge into the coat at small size.
- Avoid thin high-frequency details that disappear after palette compression.

Palette / tone:

- Prefer muted earth and moss-adjacent colors already compatible with `Assets/Art/anemora_palette_v0.gpl`.
- Keep value contrast strong enough against both Current and Past Zone1 backgrounds.
- Avoid saturated fantasy colors, glowing accents, black-heavy silhouette, or one-color clothing mass.

## 3. Deliverables

Target new asset folder:

- `Assets/Art/Sprites/Hero/v2/`

Required transparent PNG deliverables:

- `hero_stand.png`
- `hero_idle.png`
- `hero_walk_front.png`
- `hero_walk_back.png`
- `hero_walk_left.png`
- `hero_walk_right.png`
- `hero_hands_d7.png` only if the D-7 hands visual must change to remain consistent with v2; otherwise keep v1 hands as preserved baseline until separately reviewed.

Animation set:

- Match the current Stage 3 animation surface unless the import worker confirms a repo-side animation change.
- Current Hero set is stand, idle, walk front, walk back, walk left, walk right.
- Idle and walk sheets should preserve the existing practical frame assumptions: 4-frame horizontal sheets, 32x48 frame cells, transparent background.
- Right walk may be generated directly or mirrored from left if the silhouette and lighting still read consistently.

Unity import assumptions for the future asset worker:

- Texture Type: Sprite.
- Sprite Mode: Single for sheets unless the worker intentionally performs editor slicing for clips.
- Pixel Per Unit: 32.
- Filter Mode: Point.
- Mip Maps: disabled.
- Compression: uncompressed or equivalent lossless sprite setting.
- Alpha transparency enabled.
- No baked ground shadow, no background, no crop.

Preservation rule:

- Do not overwrite `Assets/Art/Sprites/Hero/v1/`.
- Do not rewrite existing prefab / animator references until v2 is generated, reviewed, ledgered, and accepted.
- If v2 is adopted, update prefab / clips / ledger in a separate asset import task with explicit before/after validation.

## 4. Prompt Base

Use this as the starting visual prompt for generation, then adapt per view and animation.

```text
A small full-body pixel art sprite of Niro, a gender-neutral quiet teenager around
15 to 19 years old, ordinary Antela town wanderer, calm closed-mouth expression,
soft travel-worn hat with an original broad drooping brim silhouette, simple muted
earth-tone layered clothing, slim average build, gentle still posture, readable
top-down isometric-ish gameplay sprite silhouette, transparent background, limited
Anemora palette v0 compatible colors, soft upper-left lighting, quiet melancholic
tone without drama, no exaggerated emotion.
```

Negative direction:

```text
NOT: Snufkin copy, exact protected character costume, green Snufkin outfit,
recognizable existing character, feathered mascot hat, pipe, scarf-copy design,
clearly male-only features, clearly female-only features, childlike chibi body,
adult middle-aged face, fantasy armor, weapons, magic aura, glowing eyes, runes,
royal clothing, religious clothing, futuristic parts, dramatic expression, shouting,
crying, big smile, big frown, saturated neon colors, background, ground shadow,
cropped body, multiple characters.
```

## 5. Resident NPC Scope

Resident_A and Resident_B remain review-first.

Do not redraw NPC residents by default during the Niro v2 task. Redraw a resident only if visual review finds a concrete issue in one or more of these categories:

- Age read conflicts with Niro's 15-19 range or weakens intended contrast.
- Gender read creates an unwanted or confusing comparison against Niro.
- Directional continuity or clothing continuity fails across existing Resident_A frames.
- Resident_B is too dark, too visually separate, or inconsistent with palette/background.
- Style diverges from the accepted Stage 4 Niro v2 direction after Niro is reviewed.

If a resident redraw is needed, create a separate brief or update `docs/asset_prompts/npc_residents.md`; do not fold it silently into the Hero v2 import task.

## 6. Validation Checklist

Before importing v2:

- [ ] User confirms candidate is a full redraw result, not a small edit of v1.
- [ ] Niro reads gender-neutral and 15-19 in front/back/side.
- [ ] Hat silhouette gives the intended quiet wanderer cue without copying Snufkin or any protected character.
- [ ] Sprite is readable at small top-down / isometric-ish gameplay scale.
- [ ] No exaggerated emotion or heroic/fantasy iconography is introduced.
- [ ] PNGs are transparent and uncropped.
- [ ] Palette is compatible with Anemora palette v0 and current Zone1 backgrounds.
- [ ] Frame sizes and frame counts match the current Hero animation surface or the deviation is explicitly documented.
- [ ] `v1` assets remain untouched and reviewable.

After importing v2 in a later task:

- [ ] Unity metas use PPU 32, point filter, no mipmaps, alpha transparency.
- [ ] Prefab preview shows v2 clearly on both Current and Past visual layers.
- [ ] Idle and walk clips animate without frame jitter or unexpected cropping.
- [ ] `Hero.prefab` replacement, if any, is documented separately.
- [ ] `docs/legal/asset_ledger.md` receives new v2 rows only after actual asset generation/import.
- [ ] A devlog records tool, prompt, seed or candidate IDs, manual edits, and review result.

## 7. Next Worker Handoff

Next worker should:

1. Generate Niro v2 candidates under `art/_intermediate/hero_v2_full_redraw/`.
2. Compare front/back/side readability against the preserved v1 assets and `Hero.prefab` preview.
3. Produce the accepted transparent PNG set under `Assets/Art/Sprites/Hero/v2/`.
4. Import with PPU 32, point filter, no mipmaps, alpha transparency.
5. Slice or wire clips according to the current Hero animation setup.
6. Preview in Unity prefab and, if practical, in `Anemora_Main`.
7. Update the asset ledger and a new devlog after generation/import, not before.
8. Leave Resident_A/B untouched unless the review-first criteria above trigger a separate resident redraw task.

## 8. Revision History

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-06 | Initial Stage 4 Phase 0 full-redraw brief for Niro / Hero v2. Records user decision, visual requirements, deliverables, NPC scope, validation, and asset-worker handoff. |
