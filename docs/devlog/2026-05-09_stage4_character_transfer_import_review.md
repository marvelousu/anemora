# Stage 4 Character Transfer Import Review

Date: 2026-05-09
Branch: `codex/stage4-graphics-foundation-20260508`

## Scope

This pass consumes the character-generation DQ3R transfer packs without replacing approved runtime sprites.

Source packs:

- v1: `<worktree>/docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_2026-05-09/`
- v2: `<worktree>/docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v2_2026-05-09/`
- v3: `<worktree>/docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v3_2026-05-09/`
- v4 gate: `<worktree>/docs/review_gallery/imports/stage4_dq3r_character_unity_transfer_v4_2026-05-09/`

Imported review roots:

- `Assets/Art/Sprites/Review/Chapter1DQ3RTransfer/`
- `Assets/Art/Sprites/Review/Chapter1DQ3RTransferV2/`
- `Assets/Art/Sprites/Review/Chapter1DQ3RTransferV3/`
- `Assets/Art/Sprites/Review/Chapter1DQ3RTransferV4/`

The import is intentionally isolated from current runtime folders. It gives graphics foundation a Unity-visible review set for 64x96 DQ3R candidate sprites while preserving current runtime prefab and sprite GUIDs.

## Automation

Updated `Assets/Editor/Stage4CharacterTransferReviewSetup.cs`.

Menu / batchmode entry points:

- `Anemora/Assets/Sync DQ3R Character Transfer Pack`
- `Anemora/Assets/Sync DQ3R Character V4 Proportion Gate`
- `Anemora/Assets/Build DQ3R Character Transfer Preview Prefabs`
- `Anemora/Review/Capture DQ3R Character Transfer Review`
- `Anemora/Review/Capture DQ3R Character Scene Fit Review`
- `Anemora/Review/Capture DQ3R Character Close Scene Fit Review`
- `Anemora.EditorTools.Stage4CharacterTransferReviewSetup.SyncCharacterTransferPack`
- `Anemora.EditorTools.Stage4CharacterTransferReviewSetup.SyncCharacterTransferV4ProportionGate`
- `Anemora.EditorTools.Stage4CharacterTransferReviewSetup.BuildCharacterTransferPreviewPrefabs`
- `Anemora.EditorTools.Stage4CharacterTransferReviewSetup.CaptureCharacterTransferReview`
- `Anemora.EditorTools.Stage4CharacterTransferReviewSetup.CaptureCharacterTransferSceneFitReview`
- `Anemora.EditorTools.Stage4CharacterTransferReviewSetup.CaptureCharacterTransferCloseSceneFitReview`

The current v3 sync step copies the import-relevant v3 folders and manifests:

- `runtime_locomotion_candidates/`
- `runtime_locomotion_fallback_32x48/`
- `runtime_idle_pose_candidates/`
- `animation_contact_frame_strips/`
- `character_shadow_helpers/`
- `character_rim_helpers/`
- `character_occlusion_masks/`
- `crowd_bank/`
- `master_96x144/`
- `master_96x144_contact_sheets/`
- `downscale_comparison_96_to_64_to_32/`
- `lighting_background_compatibility/`
- `review_failed_or_redraw_needed/`
- `review_only_role_pose/`
- `portrait_only/`
- `expression_only/`
- `graphics_foundation_transfer_manifest_v3.json`
- `import_decision_table_v3.md`
- `animation_quality_metrics_v3.csv`
- `animation_quality_metrics_v3.json`
- `animation_timing_recommendations_v3.md`
- `scene_compatibility_matrix_v3.md`
- `contact_shadow_helper_manifest_v3.json`
- `crowd_bank_manifest_v3.json`
- `role_pose_manifest_v3.json`
- `portrait_expression_manifest_v3.json`
- `unity_import_checklist_v3.md`
- `redraw_request_list_v3.md`
- `graphics_foundation_priority_import_list_v3.md`
- `transfer_pack_verification_v3.json`

The importer sets candidate sprites to Sprite, Point filter, no mipmaps, uncompressed, alpha transparency enabled, PPU 64 for 64x96 candidates, and PPU 32 for 32x48 fallback references. Horizontal 64x96 animation strips are sliced as Multiple sprites with 64 x 96 cells and bottom-center pivots.

The v4 sync is intentionally narrower. It copies `proportion_gate/` and the v4 decision / risk / verification documents only. It does not build v4 preview prefabs, does not slice v4 locomotion, and does not approve any runtime replacement.

## Imported Candidates

v1 staged 86 PNGs:

- 36 64x96 production-candidate PNGs for `resident_d`, `resident_j`, `resident_k`, and `resident_l`
- 50 32x48 fallback PNGs

v2 staged 306 PNGs:

- 64x96 runtime locomotion candidates for `hero`, `resident_a`, `resident_c`, `resident_d`, `resident_f`, `resident_j`, `resident_k`, and `resident_l`
- 32x48 fallback locomotion references
- Resident_B 64x96 idle-pose candidates, with directional walks kept in redraw-needed review
- crowd, role-pose, portrait/expression, composition-compatibility, and redraw review assets kept outside runtime locomotion

v3 staged 545 PNGs:

- 64x96 runtime locomotion candidates for `hero`, `resident_a`, `resident_c`, `resident_d`, `resident_f`, `resident_j`, `resident_k`, and `resident_l`
- 32x48 fallback locomotion references for scale comparison
- Resident_B idle-only production candidates kept separate from locomotion
- 96x144 master review copies and contact sheets
- contact-frame, contact-shadow, rim, and lower-body occlusion helper masks
- crowd bank, role-pose, portrait-only, expression-only, redraw-needed, lighting/background compatibility, and 96-to-64-to-32 downscale comparisons

v4 staged as gate evidence only:

- `proportion_gate/character_proportion_gate_v4.md`
- `proportion_gate/character_proportion_gate_current_lineup_v4.png`
- `proportion_gate/character_proportion_scale_trials_v4.png`
- `proportion_gate/character_proportion_metrics_v4.csv`
- `proportion_gate/character_proportion_metrics_v4.json`
- v4 manifest / import decision / production-lock risk / rejected-strip documents

Status: fail-open for tooling only, fail-closed for production import.

The v4 PNG and manifest checks pass, but visual head/body proportion is not accepted. Treat Hero / Niro and Resident_B / Reto as the current visual anchors. Every other character needs user review before production replacement.

v3 first-import candidates from the character session:

- `resident_f` / Mia
- `resident_c` / Kaia
- `resident_d` / Dario
- `resident_k` / Kairo
- `resident_l` / Luna

v3 review-first / watch:

- `hero` / Niro and `resident_a` / Aria: useful for scale review, but still need lock review before runtime replacement.
- `resident_j` / Karla: v3 includes stabilized review candidates, but walk-back should be compared in-scene before acceptance.
- `resident_b` / Reto: idle-only; directional movement remains redraw-needed.

## Preview Prefabs

Generated review-only Unity assets for v3:

- `Assets/Prefabs/Characters/Review/Chapter1DQ3RTransferV3_64x96/`: 8 preview prefabs
- `Assets/Animators/Review/Chapter1DQ3RTransferV3_64x96/`: 8 AnimatorControllers
- `Assets/Animators/Clips/Review/Chapter1DQ3RTransferV3_64x96/`: 46 preview clips

These assets are for isolated placement / scale / readability review. They do not replace `Hero.prefab`, `Resident_A.prefab`, `Resident_B.prefab`, runtime controllers, or runtime animation clips.

## Review Captures

v1:

`docs/devlog/screenshots/stage4_character_transfer_64x96_review.png`

SHA256:

`6348539A0592B3C30EB9E03033709E85F0FBFED4A0C329D5A9EB4E6C63B99CDA`

v2:

`docs/devlog/screenshots/stage4_character_transfer_v2_64x96_review.png`

SHA256:

`B307140FE6D1377D86E44D85B3BA554E3EC8F33AC1FD25A45D1986407B7F1952`

v2 scene-fit:

`docs/devlog/screenshots/stage4_character_transfer_v2_scene_fit_review.png`

SHA256:

`68ACE0E2C4F518A7F385A2A8343BAFE5F75960A7352A39765D6A447ABB3CB6FF`

v3:

`docs/devlog/screenshots/stage4_character_transfer_v3_64x96_review.png`

SHA256:

`EBEC9BF0C8B5A3CC8530BF093B166BA9F63B9C087278AA1E621B09A6D89A685A`

v3 scene-fit:

`docs/devlog/screenshots/stage4_character_transfer_v3_scene_fit_review.png`

SHA256:

`D6E14C21EA7611F6B0D21909E0DAE081303CA53C2C7F6107BC621403E55FE616`

v3 close-scene fit:

`docs/devlog/screenshots/stage4_character_transfer_v3_close_scene_fit_review.png`

SHA256:

`01194082B3AC1ACB64174A3836715768AE98D84636BE3EA9CA31259DCB559164`

The v3 review sheet compares the 32x48 fallback stand, 64x96 standing candidate, idle strip, 6f front/side/back walk strips, and 8f front-walk review where available. The scene-fit sheet places v3 stand sprites over the Chapter1Map Unity placement review capture and blends the v3 contact-shadow, soft foot-shadow, lower-body occlusion, and warm/cool rim helper masks so character integration can be judged against imported map density. The close-scene fit sheet repeats the same helper-mask blend over S3 Current, S3 Past, S4 Kaia field, and S5 north ruin close-density backgrounds.

## Verification

- Unity batchmode `Anemora.EditorTools.Stage4CharacterTransferReviewSetup.CaptureCharacterTransferReview`: passed for v3.
- Unity batchmode `Anemora.EditorTools.Stage4CharacterTransferReviewSetup.CaptureCharacterTransferSceneFitReview`: passed for v3.
- Unity batchmode `Anemora.EditorTools.Stage4CharacterTransferReviewSetup.CaptureCharacterTransferCloseSceneFitReview`: passed for v3.
- Unity batchmode `Anemora.EditorTools.Stage4CharacterTransferReviewSetup.SyncCharacterTransferV4ProportionGate`: passed.
- Targeted Unity EditMode `Anemora.Tests.EditMode.Chapter1CharacterTransferAssetTests`: `8/8` passed.
- Latest results: `%TEMP%/AnemoraCodexLogs/20260509_gfx_foundation_next3_tests/character_transfer_tests.xml`.
- The test verifies manifest / decision / checklist / metric files, 64x96 candidate dimensions, Point / Single-or-Multiple Sprite / PPU import settings, review-only preview prefabs / AnimatorControllers / clips, 1920 x 1080 v3 review + scene-fit + close-scene-fit capture existence, and the v4 fail-closed proportion gate.
- Capture / test log scans found no compiler errors, shader errors, exceptions, asserts, or missing-method/null-reference issues beyond known Unity licensing/socket startup noise.

## Read

The v3 set is the first transfer pack that can support a serious DQ3R-style Chapter 1 placement review. Compared with the current 32x48 runtime sprites, the 64x96 candidates carry much stronger silhouette, costume, and face-read information, and they survive the imported Chapter1Map density better in the scene-fit sheet.

The remaining weakness is not sprite import plumbing; it is composition polish. The close-scene fit sheet shows the 64x96 characters are finally reading as authored characters inside S3 / S4 / S5 density, but final runtime adoption still needs per-scene placement, scale, sorting, and shadow/rim intensity choices inside Unity rather than screenshot-only composition.

The v4 proportion gate changes the import stance: do not treat v3 or v4 as production import approval. The current graphics-foundation path may keep using these sprites for temporary map-density and contact-shadow review, but production replacement must wait for user-approved head/body proportion against Hero / Niro and Resident_B / Reto. Resident_B remains idle-only; directional locomotion must not be implied.
