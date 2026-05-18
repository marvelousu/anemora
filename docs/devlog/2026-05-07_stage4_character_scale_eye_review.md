# Stage 4 Character Scale / Eye Stability Review

Status: v0.1 recorded 2026-05-07

## 1. Purpose

This review captures the current runtime Hero, Resident_A, and Resident_B sprites after the Resident_A P1 import. It supports the 2026-05-07 runtime feedback that both Residents may read too large, or Hero may read too small, and that Resident_A eyes are unstable across frames.

No new character art was generated in this step. The review sheet is assembled mechanically from the current runtime PNGs.

## 2. Artifact

- Review sheet: `docs/devlog/screenshots/stage4_character_scale_eye_review_2026-05-07.png`
- Inputs:
  - `Assets/Art/Sprites/Hero/v2/hero_idle.png`
  - `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_idle.png`
  - `Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_front.png`
  - `Assets/Art/Sprites/NPC/Resident_B/v2/resident_b_idle.png`

The sheet enlarges each `32 x 48` frame with nearest-neighbor scaling and draws the non-transparent bounding box for each frame.

## 3. Measurements

| Sheet | Frame bbox summary |
|---|---|
| Hero idle | `19 x 45` for all 4 frames |
| Resident_A idle | `19 x 45`, `19 x 45`, `20 x 45`, `19 x 45` |
| Resident_A walk front | `19 x 45`, `19 x 45`, `18 x 45`, `19 x 45` |
| Resident_B idle | `25 x 45` for all 4 frames |

## 4. Notes

- Hero, Resident_A, and Resident_B share the same technical slot: `32 x 48` cells, PPU `32`, prefab scale `1`, and nominal `1.5` unit height.
- Resident_B is not taller, but its visible mass is wider, so it can read larger in runtime.
- Resident_A is close to Hero in bbox width / height, but the face, eyes, hair contour, and pixel density read more assertive; this explains why it can still feel large even after the P1 runtime import.
- The Resident_A eye / face issue should be handled in the source frames, not by prefab scale.

## 5. Next

- Produce a same-plane runtime lineup that places Hero, Resident_A, Resident_B, door, bed, table, fountain, and library scale references under the same camera.
- Stabilize Resident_A idle and walk-front eye / face pixels before another runtime sprite import.
- Review Resident_B width / seated mass after the first same-plane lineup; do not shrink blindly until camera and prop scale are visible in the same frame.

## 6. Verification

Docs / screenshot artifact only. No sprites, prefabs, scenes, tests, or builds were changed.
