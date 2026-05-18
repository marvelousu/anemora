# Stage 4 Aria Old-Style Preservation Probe

Date: 2026-05-10

## Scope

Verify the user's review that Retro Diffusion seed `260510` is a different character from the accepted old Aria design, and test whether the old v10 taste can be preserved with a better path.

All files remain review-only under `source_rejected/`. Nothing was copied into live `source/`, runtime sprite folders, or Unity `Assets/`.

## Source Of Truth

Accepted old Aria front:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/runtime_candidate_64x96_review_only/resident_a_aria_walk_front_v10a_64x96_6f_review_only.png
```

Old style requirements:

- Brown hair, tied hair/ribbon, soft face.
- Warm mustard short overcoat, cream apron/dress, muted underskirt.
- Taller child/early-teen read, not chibi.
- Lower saturation and softer old-v10 rendering.
- No glossy RD-like highlights.

## Retro Diffusion Result

Retro Diffusion is structurally useful but style-rejected for final Aria.

Observed:

- Seed `260510`: correct 4-direction sheet structure, but blond/glossy/chibi and visibly not old Aria.
- Design-lock seed `260513`: improved hair color and body read, but still too sharp and less old-v10/painterly.
- Scenario documentation explains why this is expected: RD Animation reference images guide pose/design, but are not used as starting frames.

Decision:

- Do not use RD direct output as accepted character art.
- Keep RD only as a motion/grid guide.

## PixelLab Rotate Probe

New helper:

```text
tools/probe_pixellab_rotate_aria_old_style.py
```

Probe output:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/
```

Review image:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/aria_pixellab_rotate_old_style_review_4x.png
```

Method:

- Crop frame 0 from the accepted old `64x96` runtime strip.
- Place it on a clean transparent `128x128` PixelLab rotate canvas.
- Generate a small old-color palette image from the same reference.
- Call PixelLab `/rotate`, `from_direction=south`, `to_direction=north/east/west`, `image_guidance_scale=10`.
- Store all candidates in `source_rejected/`.

Notes:

- A first `64x96` rotate call was rejected by PixelLab with HTTP 422 because `/rotate` requires square canvases.
- One intermediate bad-reference `128x128` run was excluded after finding the reference image was assembled from mismatched source/alpha dimensions.
- The corrected probes used the accepted runtime strip directly.
- Corrected PixelLab responses reported `usage.usd = 0.0`.

## PixelLab Rotate Result

The corrected PixelLab Rotate outputs preserve old Aria far better than RD.

Metrics from the review summary:

| Candidate | Alpha width | Alpha height | Visual result |
|---|---:|---:|---|
| `north` | 25 px | 65 px | Good old-style back still |
| `east` | 22 px | 66 px | Good old-style side still |
| `west` | 22 px | 66 px | Good old-style side still |

Visual assessment:

- Hair color and mustard/cream palette stay close to old v10.
- Body height remains close to old front (`65-66px`) and avoids the RD chibi read.
- Side views are plausible and keep the same soft, restrained style.
- These are still stills, not accepted walk cycles.

Decision:

PixelLab Rotate is now the preferred old-style preservation path for Aria directional stills.

## Seed Matrix Follow-Up

Small additional seed search was run for PixelLab Rotate, still under `source_rejected/`:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/aria_pixellab_rotate_old_style_seed_matrix_review_4x.png
```

Probe set:

- `north`: seeds `260520`, `260521`, `260522`
- `east`: seeds `260520` through `260524`
- `west`: seeds `260520` through `260524`

Observed:

- `north` seed `260522` has a floating alpha artifact and is rejected.
- `north` seed `260521` is the best back-view base.
- `east` / `west` remain imperfect, but later seeds reduce some of the side-profile distortion.
- Using separate left/right seeds causes subtle side inconsistency; mirroring one better side is more stable for a game sprite.

Selected review-only directional bases were generated at:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/aria_pixellab_rotate_selected_directional_bases_64x96_review_4x.png
```

Selection:

| Runtime direction | Source |
|---|---|
| `walk_front` | accepted old v10 frame 0 |
| `walk_back` | PixelLab Rotate `north`, seed `260521` |
| `walk_left` | PixelLab Rotate `west`, seed `260524` |
| `walk_right` | mirrored from selected `walk_left` |

Metrics:

| Runtime direction | Alpha bbox | Status |
|---|---|---|
| `walk_front` | `25x66` | accepted old anchor |
| `walk_back` | `25x65` | promising base still |
| `walk_left` | `23x66` | usable side base, needs face/hair/foot touch-up |
| `walk_right` | `23x66` | mirrored side base, needs same touch-up |

Decision update:

- Back is close enough to use as a base.
- Side stills are not final, but they now preserve old Aria's scale and palette well enough to justify local brush-up.
- Next work should be local/Aseprite-style cleanup and then 4f walk-cycle construction, not more RD seed search.

## Side Cleanup Follow-Up

Local side cleanup variants were created after user review requested a stricter pass before presenting again.

Debug / working folder:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/side_cleanup_work/
```

Rejected during review:

- Hand-drawn side variants: too simplified and lower-density than old v10.
- `hybrid_v01`: over-corrected face/feet.
- `hybrid_v02_minimal` / `hybrid_v03_slim`: foot mass became blocky.
- `hybrid_v05_foot_trim`: introduced stray foot pixels.

Initial selected, later superseded:

```text
hybrid_v06_arm_fix
```

Why selected:

- Preserves the PixelLab Rotate side face/body density instead of replacing it with a simplified redraw.
- Fixes the gray/black rear hair block into old-brown hair/ribbon.
- Reduces the oversized hand/bright side patch.
- Leaves the original foot/dress mass mostly intact to avoid blocky shoe regression.

Selected review:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/side_cleanup_work/selected_v06_review_only/aria_side_cleanup_selected_v06_64x96_review_4x.png
```

Before/after review:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/side_cleanup_work/selected_v06_review_only/aria_side_cleanup_before_after_v06_review_4x.png
```

Validation:

| Sprite | Bbox | Bottom |
|---|---:|---:|
| front old | `25x66` | `94` |
| back base | `25x65` | `94` |
| left clean | `23x66` | `94` |
| right clean | `23x66` | `94` |

Additional checks:

- `right_clean` is an exact mirror of `left_clean` (`mirror_diff_bbox=None`).
- All outputs remain review-only under `source_rejected/`.
- Superseded by the v12 side revision below after user review.

## Side Cleanup v12 Revision

After preview, user review flagged three remaining issues in v06:

- Side eye still read oddly.
- Alpha holes remained below the tucked hand.
- Body still read too wide.

Additional variants were generated in the same review-only working folder:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/side_cleanup_work/
```

Rejected during the second pass:

- `hybrid_v07`: eye/gap improvement, but alpha holes still remained under the hand.
- `hybrid_v08` / `hybrid_v09`: slimming line detached from the side body silhouette.
- `hybrid_v13` / `hybrid_v14`: lower-body repaint removed too much old-v10 texture density.

Selected current review candidate:

```text
hybrid_v12_eye_gap_visual_slim
```

Why selected:

- Keeps the PixelLab Rotate old-style head/body density instead of replacing the sprite with a simplified redraw.
- Uses a one-pixel side eye.
- Fills the hand underside completely.
- Reduces the bulky side-body read by darkening the rear apron/coat side while preserving the original baseline and texture.

Selected review:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/side_cleanup_work/selected_v12_review_only/aria_side_cleanup_selected_v12_64x96_review_4x.png
```

Before/after review:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/side_cleanup_work/selected_v12_review_only/aria_side_cleanup_before_after_v12_review_4x.png
```

Review Gallery filter:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_side_cleanup_selected_v12_64x96_review
```

Validation:

| Check | Result |
|---|---:|
| front old bbox | `25x66`, bottom `94` |
| back base bbox | `25x65`, bottom `94` |
| left clean bbox | `23x66`, bottom `94` |
| right clean bbox | `23x66`, bottom `94` |
| hand-under alpha holes `(31,67)-(37,77)` | `0` |
| right exact mirror diff bbox | `None` |

All v12 outputs remain review-only under `source_rejected/`. This is still a directional side still/base candidate, not a completed walk cycle.

## Side Eye Variant Review

After v12 preview, the user noted the side still still reads somewhat wide and that the eye direction was not as intended. Body shape remains unchanged for this pass so eye preference can be judged independently.

New review-only helper:

```text
tools/build_aria_side_eye_variants.py
```

Review folder:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/side_cleanup_work/eye_variant_review_v01/
```

Review Gallery filter:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_side_eye_variants_v01
```

Variants:

- `a_current_v12`: current v12 eye.
- `b_forward_dot`: 1px eye moved forward.
- `c_forward_soft`: forward eye using softer dark brown.
- `d_small_lash`: small dark lash plus eye.
- `e_vertical_old`: denser vertical 2px old-style eye.
- `f_lower_dot`: eye lowered by 1px.
- `g_no_dot_shadow`: no hard dot, shadow-only read.
- `h_profile_line`: stronger profile/eyelid line.

Generated review images:

```text
aria_side_eye_variants_v01_head_closeup_12x.png
aria_side_eye_variants_v01_full_4x.png
aria_side_eye_variants_v01_lr_pairs_4x.png
```

Status: waiting for user preference. All variants keep the v12 body unchanged.

## Seed260520 East Reference Pass

User pointed out that the eye in the seed matrix `seed260520` / `east` cell was closer to the desired look, and correctly noted that v12 did not materially change the body shape. v12's silhouette width remained effectively unchanged; it only darkened the side body.

New review-only helper:

```text
tools/build_aria_side_seed260520_east_reference_variants.py
```

Review folder:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/side_cleanup_work/seed260520_east_reference_review_v01/
```

Review Gallery filters:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_side_seed260520_east_reference_v01_pairs_4x
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_side_seed260520_east_reference_v01_head_12x
```

Candidates:

- `current_v12`: current side-cleanup candidate.
- `east260520_raw`: seed-matrix `east` / `260520` converted to a 64x96 right/left pair.
- `eye_from_east`: v12 body with only the east260520 eye/face patch.
- `eye+body_compress_1px`: east260520 eye plus v12 body crop compressed by 1px from the rear side.
- `eye+body_compress_2px`: same but compressed by 2px.

Body metrics:

| Candidate | Alpha width | Body max row width y50-85 | Body avg row width y50-85 |
|---|---:|---:|---:|
| `current_v12` | `23` | `21` | `17.47` |
| `east260520_raw` | `22` | `20` | `17.14` |
| `eye_from_east` | `23` | `21` | `17.47` |
| `eye+body_compress_1px` | `23` | `20` | `16.47` |
| `eye+body_compress_2px` | `23` | `19` | `15.47` |

Assessment:

- `eye_from_east` isolates the eye change but confirms the body remains too similar to v12.
- `east260520_raw` is naturally slimmer and has the referenced eye, but changes more of the side still.
- `eye+body_compress_1px` is the current best controlled edit if preserving v12 texture is preferred.
- `eye+body_compress_2px` may be too thin and should be treated as a stress test.

## Current-Eye Body Slim Pass

User then decided the `current_v12` eye may be acceptable after direct comparison, and asked to keep that eye while making the body slimmer.

New review-only helper:

```text
tools/build_aria_side_body_slim_current_eye_variants.py
```

Review folder:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/side_cleanup_work/body_slim_current_eye_review_v01/
```

Review Gallery filters:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_side_body_slim_current_eye_v01_pairs_4x
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_side_body_slim_current_eye_v01_direction_review_4x
```

Candidates:

- `current_v12_eye`: unchanged current v12 eye/body baseline.
- `body_slim_1px`: full side body compressed by 1px from the rear side.
- `body_slim_2px`: full side body compressed by 2px from the rear side.
- `body_slim_3px`: full side body compressed by 3px, stress test.
- `lower_slim_2px`: lower body only compressed by 2px.
- `rear_trim_2px`: opaque rear-edge pixels trimmed by 2px, stress test.

Body metrics:

| Candidate | Alpha width | Body max row width y50-85 | Body avg row width y50-85 |
|---|---:|---:|---:|
| `current_v12_eye` | `23` | `21` | `17.47` |
| `body_slim_1px` | `23` | `20` | `16.47` |
| `body_slim_2px` | `23` | `19` | `15.47` |
| `body_slim_3px` | `23` | `19` | `14.94` |
| `lower_slim_2px` | `23` | `19` | `16.14` |
| `rear_trim_2px` | `23` | `19` | `16.78` |

Assessment:

- `body_slim_1px` is safer but still modest.
- `body_slim_2px` currently looks like the best balance between a visibly slimmer body and preserving the v12/current-eye identity.
- `body_slim_3px` and `rear_trim_2px` should be treated as stress tests because body/foot continuity starts to weaken.

## Body Slim 1px Foot Fix Pass

User selected `body_slim_1px` as the best proportion, but noted that the foot appears missing. Inspection showed the issue is not the foot bitmap itself but the missing y=86 alpha bridge between hem/legs and foot after the body compression.

New review-only helper:

```text
tools/build_aria_side_body_slim_footfix_variants.py
```

Review folder:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/side_cleanup_work/body_slim_1px_footfix_review_v01/
```

Review Gallery filters:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_side_body_slim_1px_footfix_v01_pairs_4x
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_side_body_slim_1px_footfix_v01_direction_review_4x
```

Candidates:

- `current_v12_eye`: unchanged baseline.
- `body_slim_1px_problem`: chosen body width, but foot bridge missing (`has_y86_alpha=false`).
- `foot_restore_current`: `body_slim_1px` proportions with current-v12 lower leg/foot restored.
- `foot_bridge_redraw`: hand-redrawn bridge/feet; rejected in visual assessment because the foot becomes blocky.
- `foot_compact_restore`: compact restore variant; usable but the foot reads weaker than `foot_restore_current`.

Assessment:

- `foot_restore_current` is the current best candidate: it keeps `body_slim_1px` body metrics (`body_max_row_width_y50_85=20`, `body_avg_row_width_y50_85=16.47`) and restores foot continuity (`has_y86_alpha=true`, bottom `94`).
- All outputs remain review-only.

User decision:

```text
foot_restore_current selected
```

Selected review-only output:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/side_cleanup_work/selected_body_slim_1px_foot_restore_review_only/
```

Selected Review Gallery filter:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_side_selected_body_slim_1px_foot_restore
```

Selected validation:

| Check | Result |
|---|---:|
| front old bbox | `25x66`, bottom `94` |
| back base bbox | `25x65`, bottom `94` |
| left clean bbox | `23x66`, bottom `94` |
| right clean bbox | `23x66`, bottom `94` |
| left/right body max row width y50-85 | `20` |
| left/right body avg row width y50-85 | `16.47` |
| hand-under alpha holes `(31,67)-(37,77)` | `0` |
| foot bridge y86 alpha, left/right | `true` |
| right exact mirror diff bbox | `None` |
| back head protrusion pixel `(28,30)` | transparent `(0,0,0,0)` |
| back row 30 opaque x range | `32..38` only |

This is now the accepted review-only side still/base candidate for Aria, pending walk-cycle construction.

Back head 1px cleanup:

- User accepted the selected still pack except for one protruding pixel on the back head.
- Added a deterministic cleanup in `tools/build_pixellab_rotate_old_style_selected_bases.py` for `walk_back` only: clear pixel `(28,30)` after the selected north seed is placed on the 64x96 cell.
- Rebuilt the selected directional bases, selected side review, and Review Gallery.
- Verified `resident_a_aria_walk_back_pixellab_rotate_seed260521_base_64x96.png` has `(28,30) == (0,0,0,0)` and row 30 opacity only at `x=32..38`.
- The accepted side selection remains `body_slim_1px_foot_restore_current`.

## Rejected Local Walk-Cycle Probe v01

After the still pack was accepted with the back-head micro-cleanup, a first review-only walk-cycle candidate was generated locally from the selected stills:

```text
tools/build_aria_old_style_local_walk_cycle_candidates.py
```

Output folder:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/local_walk_cycle_from_selected_stills_review_v01/
```

Review Gallery filter:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_old_style_selected_local_walk_cycle_micro_stride
```

Outputs:

- `aria_old_style_selected_local_walk_cycle_micro_stride_anim_preview.gif`
- `aria_old_style_selected_local_walk_cycle_micro_stride_64x96_review_4x.png`
- `resident_a_aria_walk_front_v10a_passthrough_64x96_6f_review_only.png`
- `resident_a_aria_walk_back_selected_still_lower_stride_1px_64x96_6f_review_only.png`
- `resident_a_aria_walk_left_body_slim_1px_foot_restore_micro_stride_64x96_6f_review_only.png`
- `resident_a_aria_walk_right_body_slim_1px_foot_restore_micro_stride_64x96_6f_review_only.png`

Validation:

| Direction | Head drift | Bottom drift | Width | Mean delta |
|---|---:|---:|---:|---:|
| `walk_front` | `0` | `0` | `25..25` | `714.2` |
| `walk_back` | `0` | `0` | `25..25` | `55.2` |
| `walk_left` | `0` | `0` | `23..24` | `148.8` |
| `walk_right` | `0` | `0` | `23..24` | `130.0` |

Assessment:

- Front uses the accepted v10a strip unchanged.
- Back/side keep the approved still identity, but the procedural lower-foot-only motion fails as animation.
- The head/torso read frozen while the feet slide or tear under the body, especially in side view.
- On lift frames, the foot appears disconnected from the leg/hem instead of being part of a coherent lifted-leg pose.
- Numeric head/bottom drift metrics were insufficient for animation approval; this probe is `rejected_visual_qa_failed`.
- Do not import, refine, or present this local-procedural method as an accepted candidate.

Next production path:

1. Use the PixelLab Rotate stills as directional base poses.
2. Do not continue the lower-foot-only procedural synthesis path.
3. Use PixelLab Skeleton or Aseprite/manual per-frame redraw for real walk-cycle posing, keeping foot, leg, hem, and weight shift connected while using the accepted still pack as the identity/style lock.
4. Use the accepted old front as the locked style reference for all touch-up.
5. Keep RD only as a gait/pose reference if useful.

## Connected-Pose Walk Probe v05

After user feedback that the local animation still showed foot/leg disconnection on lift frames, the lower-foot-only method was discarded and a connected-pose redraw pass was added:

```text
tools/build_aria_old_style_connected_walk_cycle_candidates.py
```

Output folder:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixellab_rotate_old_style_probe/selected_directional_bases_64x96_review_only/local_walk_cycle_connected_pose_review_v05/
```

Review Gallery filter:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_old_style_connected_pose_walk_probe_v05
```

Internal iteration notes:

- `v02`: rejected internally because full lower redraw made the legs read as sticks.
- `v03`: rejected internally because the skirt was preserved better, but side/back leg shapes were still too exposed.
- `v04`: rejected internally because neutral frames were restored, but passing frames still read too stick-like.
- `v05`: keeps the accepted still frames as anchors and hides most leg motion under the skirt/hem shadow.

Validation:

| Direction | Head drift | Bottom drift | Width | Lower connection |
|---|---:|---:|---:|---|
| `walk_front` | `0` | `0` | `25..25` | `true` |
| `walk_back` | `0` | `1` | `25..25` | `true` |
| `walk_left` | `0` | `0` | `23..23` | `true` |
| `walk_right` | `0` | `0` | `23..23` | `true` |

QA gate:

- Every lower foot-like alpha component must connect upward to `y<=86`; v05 passes this for all frames/directions.
- Static 5x review was rechecked after v02/v03/v04 failures before exposing v05 in Review Gallery.
- Still `review_only`, `runtime_ready=false`, and `unity_import_allowed=false`.

## External Site Assessment

No new subscription is justified yet.

Potential fallback sites if PixelLab Skeleton/Aseprite cannot produce acceptable cycles:

- SpriteBrew: promising because it claims existing-art upload, walk-cycle generation, built-in pixel editing, and free daily tokens. Needs a no-card trial before spend.
- AISpriteSheet: promising on paper because it claims reference-image identity lock, grid-aligned transparent sprite sheets, and RPG 3x4/4x4 layouts. Needs a no-card trial before spend.
- Layer: more suitable for style-trained roster/concept generation than immediate locked locomotion. Consider only if the project later needs many characters trained from existing art.

Less suitable for immediate old-style Aria:

- More Retro Diffusion seed search: likely wasteful until the workflow can use references as starting frames or stronger identity locks.
- Generic image models: useful for concept/style exploration only, not strict character-preserving sprite sheets.

## References Checked

- PixelLab docs: `https://www.pixellab.ai/docs`
- PixelLab API page: `https://www.pixellab.ai/pixellab-api/`
- PixelLab Rotate docs: `https://www.pixellab.ai/docs/tools/create-8-rotations-pro`
- PixelLab Skeleton docs: `https://www.pixellab.ai/docs/tools/animate-with-skeleton`
- Scenario RD Animation parameter docs: `https://docs.scenario.com/docs/image-generation-models-parameters-reference`
- Scenario RD essentials: `https://help.scenario.com/articles/4202673551-retro-diffusion-models-the-essentials`
- SpriteBrew: `https://www.spritebrew.com/`
- AISpriteSheet: `https://www.aispritesheet.com/`
- Layer character design: `https://www.layer.ai/use-cases/character-design`

## External Animation Service Trial Pass

Autonomous follow-up after the local walk-cycle probes were rejected.

Review-only input and comparison outputs:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/ai_service_trial_pack/
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/gensprite_probe/
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/spritesheets_ai_probe/
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/spritelab_probe/
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixelengine_probe/
```

Trial results:

- SpriteBrew: existing PNG upload failed in the browser with `Failed to load image`; no generation.
- GenSprite Rotate 4: completed, costing 12 tokens (`50 -> 38`), but side/back identity drift is too high for an animation source. GenSprite Animate remains blocked because the UI charges `117 tokens` even when only one direction is filled, and the account has `38` tokens.
- Spritesheets.ai: completed one reference-style character generation (`750 -> 650` credits), but returned a front character image only; `spritesheets=[]`.
- AutoSprite: strong fit on paper, but blocked at Google OAuth account chooser.
- Spriteflow: image upload succeeded, but generation prompted sign-in.
- SpriteLab: app exposes Animate and 25 free credits, but the auth gate blocks mode selection/generation until sign-in. It also enforces animation input max `256x256`; a `128x192` Aria probe input was prepared.
- PixelEngine: best next API target. Official API accepts PNG up to `256x256`, `output_frames=2..16`, and `output_format="spritesheet"` at about `20 credits`; free plan lists `100 credits/month`. A ready-to-run probe script was added:

```text
tools/probe_pixelengine_aria_animation.py
```

Run condition:

```powershell
$env:PIXELENGINE_API_KEY='<key>'
python tools\probe_pixelengine_aria_animation.py
```

Current gate:

- `PIXELENGINE_API_KEY` was later set by the user and the PixelEngine trial was executed.
- `SPRITECOOK_API_KEY` is not set locally.
- `PIXELLAB_API_TOKEN` is set, but PixelLab skeleton/animate has already been weaker for this specific foot-disconnection problem than the new PixelEngine/SpriteCook route.

Recommendation:

1. Try PixelEngine first via API key; it is the most direct fit and includes free starter credits.
2. If PixelEngine drifts, try SpriteCook next; free plan lists `40 credits every 30 days`, average animation `~20 credits`, and API/MCP access.
3. Use GenSprite Animate only if tokens are topped up. Minimum visible shortfall is `79 tokens` (`117 - 38`), but identity drift in GenSprite Rotate 4 makes this lower priority than PixelEngine/SpriteCook.

## PixelEngine Trial Results

Execution date: 2026-05-10.

Source input:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixelengine_probe/input/aria_old_v10_front_frame0_128x192_transparent_rgba.png
```

Credit usage:

- Starting balance: `100`.
- `front_walk`: `/animate` 20 credits + `/remove-background` 2 credits.
- `front_walk_v02_front_locked`: `/animate` 20 credits + `/remove-background` 2 credits.
- `front_walk_v03_front_locked_4f`: `/animate` 20 credits + `/remove-background` 2 credits.
- Remaining balance after v03: `34`.

Review outputs:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_pixelengine_front_walk
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_pixelengine_front_walk_v02_front_locked
http://127.0.0.1:8765/docs/review_gallery/index.html?q=aria_pixelengine_front_walk_v03_front_locked_4f
```

Candidate notes:

- `front_walk`: first pass, good identity preservation, but head/body drift into 3/4 view on some frames.
- `front_walk_v02_front_locked`: better front lock and 8-frame motion, but the final frame still tilts slightly.
- `front_walk_v03_front_locked_4f`: best current candidate. Motion is subtler, but face/body drift is lower and lower-foot disconnection count is `0/4`.

Status:

- Still `review_only`.
- Do not import into Unity until user approves the PixelEngine candidate.
- Recommended review candidate before post-process: `front_walk_v03_front_locked_4f`.

## PixelEngine v03 Lower-Body Mirror Post-Process

Execution date: 2026-05-10.

Reason:

- User noted the 4-frame PixelEngine v03 candidate ends with one foot forward.
- Full-body horizontal mirroring was rejected because it would flip asymmetric hair/ribbon details and weaken identity preservation.
- A lower-body-only mirror loop was generated locally from v03, with no extra PixelEngine credit usage.

Review outputs:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=front_walk_v03_lower_mirror
```

Candidate notes:

- `front_walk_v03_lower_mirror_y72_8f`: 8f loop; lower disconnection count `0/8`, but the mirror boundary is high enough to move the hem/body area too much.
- `front_walk_v03_lower_mirror_y76_8f`: 8f loop; lower disconnection count `0/8`; best balance of alternating foot motion while keeping upper-body identity stable.
- `front_walk_v03_lower_mirror_y80_8f`: 8f loop; lower disconnection count `0/8`, but motion is weaker because only the lowest foot area changes.
- `front_walk_v03_lower_mirror_y76_6f`: 6f loop; lower disconnection count `0/6`, but the 8f y76 loop reads smoother for review.

Current recommendation:

- Keep the work `review_only`.
- Recommended review candidate: `front_walk_v03_lower_mirror_y76_8f`.

## PixelEngine Side Trial

Execution date: 2026-05-10.

Credit usage:

- `side_left_walk_v01_profile_locked_4f`: `/animate` 20 credits.
- `/remove-background` was not used. The raw output had a flat matte background, so a local connected-matte removal pass was sufficient.
- Estimated remaining balance after side trial: `14` credits.

Source input:

```text
docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixelengine_probe/input/aria_old_v10_left_body_slim_1px_foot_restore_128x192_transparent_rgba.png
```

Review outputs:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=side_left_walk_v01_profile_locked_4f
http://127.0.0.1:8765/docs/review_gallery/index.html?q=side_left_walk_v01_bodylock_y80_4f
```

Candidate notes:

- Raw PixelEngine side result preserves the old Aria side identity better than prior local attempts and has no detected lower-body disconnects (`lower_components_y70=1` for all 4 frames).
- Raw frames 2-3 make the lower coat/skirt read a little wider than the accepted still.
- `bodylock_y80_4f` keeps the accepted old-style upper body and most of the slim side silhouette, using the generated lower foot motion only.
- A mirrored-right version was generated locally from the left side result and compared against the accepted right still.

Current recommendation:

- `side_left_walk_v01_bodylock_y80_4f` was later judged too static: the upper body does not move, and the foot motion reads like cautious sneaking.
- Use the raw side result only if stronger foot motion is preferred over strict still preservation.
- Back direction still needs one PixelEngine `/animate` pass. At 20 credits per pass, the current estimated balance is short by about `6` credits; add at least `10` credits, preferably `30-40` if one retry buffer is desired.

## PixelEngine Side Motion-Slim Variants

Execution date: 2026-05-10.

Reason:

- User rejected the body-locked side candidate because only the feet moved.
- New local candidates preserve the PixelEngine raw whole-body side motion and only slim over-wide torso/skirt rows.
- No additional PixelEngine credits were used.

Review outputs:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=side_left_walk_v01_motion_slim
```

Candidate notes:

- `side_left_walk_v01_motion_slim_y44_86_m1_center`: strictest slimming; preserves whole-body motion, lower-body component count remains `1` for all 4 frames.
- `side_left_walk_v01_motion_slim_y44_86_m2_center`: slightly less strict; body still reads a little wider on the stride frames.
- `side_left_walk_v01_motion_slim_y50_84_m2_center`: similar to m2 but avoids changing upper rows; not materially better.
- `side_left_walk_v01_motion_slim_y44_86_m2_front`: preserves front edge; not clearly better than centered slimming.

Current recommendation:

- Prefer `side_left_walk_v01_motion_slim_y44_86_m1_center` over the static body-lock candidate.
- The side source generation is still small-stride because the PixelEngine prompt asked for tiny steps. A better side retry should explicitly request a normal confident RPG walk with torso bob and coat swing, not "tiny" or "restrained" steps.

## PixelEngine Side Gait-Phase Local Variants

Execution date: 2026-05-10.

Reason:

- User noted the motion-slim side candidate still reads like each foot is being placed forward one at a time.
- This indicates the PixelEngine source pass produced contact-heavy step poses rather than a full side gait with clear passing positions.
- Local pass-frame variants were generated without additional credits, inserting feet-tucked passing poses between contact frames.

Review outputs:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=side_left_walk_v01_gait_phase
```

Candidate notes:

- `side_left_walk_v01_gait_phase_pass82_6f`: adds pass frames, but the inserted pass frames split lower-body components (`lower_components_y70=2`) and still feel partly assembled.
- `side_left_walk_v01_gait_phase_pass82_soft_6f`: softens the forward foot on contact frames, but does not fully fix the underlying gait.
- `side_left_walk_v01_gait_phase_pass82_8f`: smoother timing, but frame order still inherits the source's contact-pose bias.

Current recommendation:

- Do not treat the gait-phase local variants as final.
- Best next step is a PixelEngine side retry with a corrected motion prompt: normal RPG side walk, alternating front/back leg silhouettes, clear passing frames, torso bob, small coat/arm swing, and explicitly not sneaking/tip-toeing.
- Additional credit is required for the retry; estimated current balance remains below one 20-credit `/animate` pass.

## PixelEngine Side Retry v02 And Back v01

Execution date: 2026-05-10.

Credit usage:

- `side_left_walk_v02_normal_gait_6f`: `/animate` 20 credits.
- `back_walk_v01_normal_gait_6f`: `/animate` 20 credits.
- `/remove-background` was not used for either result. Both raw outputs used a flat matte background that was removed locally.

Side retry prompt correction:

- Removed `tiny` and `restrained`.
- Explicitly requested normal RPG side walk, alternating contact/passing frames, one leg forward while the other trails backward, feet passing under the body, torso bob, coat hem movement, and no sneaking/tiptoe/shuffling.

Review outputs:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=side_left_walk_v02_normal_gait_6f
http://127.0.0.1:8765/docs/review_gallery/index.html?q=side_left_walk_v02_motion_slim
http://127.0.0.1:8765/docs/review_gallery/index.html?q=back_walk_v01_normal_gait_6f
```

Candidate notes:

- `side_left_walk_v02_normal_gait_6f`: much better gait than v01, with clearer front/back leg positions and reduced sneaking feel. Frames 1-3 widen the coat/skirt slightly.
- `side_left_walk_v02_motion_slim_y44_86_m1_center`: current best side candidate; preserves v02's whole-body gait while compressing over-wide torso/skirt rows. Lower-body component count remains `1` for all frames.
- `side_left_walk_v02_motion_slim_y44_86_m2_center`: less strict slimming; still slightly wider than desired.
- `back_walk_v01_normal_gait_6f`: usable first-pass back walk. It preserves back-facing direction, avoids face/turning artifacts, and lower-body component count remains `1` for all frames.

Current recommendation:

- Present `side_left_walk_v02_motion_slim_y44_86_m1_center` and `back_walk_v01_normal_gait_6f` for review.
- Do not import into Unity until user approves these directions together with the already approved front lower-body mirror candidate.

## PixelEngine Back Approval And Side Crossing Delag

Execution date: 2026-05-10.

User review:

- `back_walk_v01_normal_gait_6f` was approved as final-review quality for the back direction.
- `side_left_walk_v02_motion_slim_y44_86_m1_center` was close, but had a small perceived lag when the feet crossed.

Local side diagnosis:

- The side source had two consecutive crossing/contact-like beats around source frames 2 and 3.
- This made the feet appear to pause briefly during the crossing phase even though the character identity and body shape were otherwise acceptable.
- No additional PixelEngine credits were used for this pass.

Review outputs:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=side_left_walk_v02_delag
http://127.0.0.1:8765/docs/review_gallery/index.html?q=side_left_walk_v02_classic4
http://127.0.0.1:8765/docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixelengine_probe/review/aria_pixelengine_side_v02_delag_candidate_compare_anim_4x.gif
```

Candidate notes:

- `side_left_walk_v02_delag_drop_f2_5f`: best current side candidate. Removes the wider of the two crossing beats, keeps lower-body component count at `1`, and preserves the approved slim body and eye style.
- `side_left_walk_v02_delag_drop_f3_5f`: also removes the double beat, but keeps the wider crossing frame and has a sharper transition back to neutral.
- `side_left_walk_v02_delag_fast_cross_6f_preview`: shows that the issue is mainly timing, but it relies on shortened frame durations rather than a uniform sprite strip.
- `side_left_walk_v02_classic4_*`: removes the crossing lag, but the whole-body rhythm becomes more abrupt than the 5-frame delag candidates.

Current recommendation:

- Keep `back_walk_v01_normal_gait_6f` unchanged.
- Present `side_left_walk_v02_delag_drop_f2_5f` as the recommended side fix, with `drop_f3_5f` only as a backup comparison.
- Keep all outputs review-only until user explicitly approves import into Unity assets.

Follow-up user preference:

- User noted the intermediate `classic4` candidate looked good.
- Treat `side_left_walk_v02_classic4_f0_f1_f5_f3` as the preferred side candidate for the next review/import decision, because it removes the crossing lag more decisively and matches a 4-frame RPG walk cadence.
- `drop_f2_5f` remains a backup if the 4-frame cadence is later judged too snappy in Unity.

Second follow-up:

- User identified `aria_pixelengine_side_left_walk_v02_classic4_f0_f2_f5_f3_mirrored_right_anim_4x.gif` as the closest movement, while noting the body still felt slightly off.
- Local body-fix variants were generated from the same `f0_f2_f5_f3` frame order without additional PixelEngine credits.
- `side_left_walk_v02_classic4_f0_f2_f5_f3_refbody_y28_76_bob` is the current preferred side candidate: it keeps the user-preferred 4-frame motion but replaces the upper/body mass with the approved side still, shifted per-frame to preserve a small bob.
- `refbody_y28_80_bob` is stricter but makes the lower body slightly more static; `silhouette_clamp_ref0` preserves original motion but does not correct the body enough.

Review outputs:

```text
http://127.0.0.1:8765/docs/review_gallery/index.html?q=side_left_walk_v02_classic4_f0_f2_f5_f3_refbody_y28_76_bob
http://127.0.0.1:8765/docs/review_gallery/imports/stage4_dq3r_character_aria_locomotion_pilot_v10_2026-05-09/source_rejected/pixelengine_probe/review/aria_pixelengine_side_v02_classic4_f0_f2_f5_f3_bodyfix_compare_mirrored_right_anim_4x.gif
```

Decision:

- User approved `side_left_walk_v02_classic4_f0_f2_f5_f3_refbody_y28_76_bob` as the side direction final model.
- Use this movement/body relationship as the side-animation reference for subsequent character animation production.
- Keep `back_walk_v01_normal_gait_6f` as approved and leave it unchanged.
