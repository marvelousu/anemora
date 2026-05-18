# Stage 4 HD-2D Direction Review Tier 4 Pass

Date: 2026-05-08

## Summary

This pass records the first deliberately stronger HD-2D direction review after the subtle graphics-foundation passes proved too conservative for a DQ3R-like target.

Two changes are included:

- Production-facing atmospheric baseline in `Anemora_Main`
  - Enables subtle exponential-squared fog.
  - Aligns the camera clear color with the fog color.
  - Enables camera dithering to reduce banding risk.
- Review-only HD-2D direction capture
  - Adds a temporary proposed capture layer with a painted backdrop, floor skirts, contact-shadow pads, stronger post-process, Gaussian DOF, warm key / cool fill lighting, and an orthographic diorama camera.
  - The proposed HD-2D capture is not applied to the production scene. It exists to make the larger visual pivot reviewable before changing camera composition or scene layout.

## Design Conclusion

Small URP / fog / post-process tweaks are not enough to reach a DQ3R-like HD-2D look. The current main scene is still mostly a flat floor grid with sparse vertical set dressing. Reaching a Tier 4 direction needs a larger visual foundation:

- denser background / side geometry so the world reads as a diorama rather than a floating tile plane,
- production camera composition review, likely including orthographic or tighter isometric framing,
- stronger but controlled post-process and DOF,
- better floor material hierarchy and less grid-dominant tile repetition,
- more vertical prop/building density around the playable area.

This pass keeps those larger choices review-only and avoids committing a production camera change.

## Files

- `Assets/Editor/Stage4AtmosphereSetup.cs`
- `Assets/Editor/Stage4DioramaBoundarySetup.cs`
- `Assets/Editor/Stage4Hd2dDirectionCapture.cs`
- `Assets/Scenes/Anemora_Main.unity`
- `Assets/Art/Materials/Zone1/Stage4_FloorBreakup_Current.mat`
- `Assets/Art/Materials/Zone1/Stage4_FloorBreakup_Past.mat`
- `Assets/Art/Materials/Zone1/Stage4_FloorDetail_Current.mat`
- `Assets/Art/Materials/Zone1/Stage4_FloorDetail_Past.mat`
- `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Current.prefab`
- `Assets/Prefabs/Zone1/Stage4_DioramaBoundary_Past.prefab`
- `Assets/Tests/EditMode/GraphicsFoundationAssetTests.cs`
- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`
- `docs/devlog/screenshots/stage4_hd2d_direction_current.png`
- `docs/devlog/screenshots/stage4_hd2d_direction_proposed.png`
- `docs/devlog/screenshots/stage4_hd2d_direction_review_sheet.png`

## Screenshot Artifacts

Updated main-scene review captures:

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
  - SHA256: `D42D79706E1185B04BD4A725725C7748A92E32FFC235693444F2D1020BDC2815`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
  - SHA256: `17FC8F4CAC27CEFDA21E88DCDBC72F25176F899BB3A2370353CECDDDAE281606`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`
  - SHA256: `B6DFC749A9DFBF65D547F0EA53F9C15AA49399895B7DCAADDECD1752F8B494A9`

New HD-2D direction review captures:

- `docs/devlog/screenshots/stage4_hd2d_direction_current.png`
  - SHA256: `9A31528F728783075249C67A5540AD44A17AF5E265EA683530AE07A202C1DC8F`
- `docs/devlog/screenshots/stage4_hd2d_direction_proposed.png`
  - SHA256: `06E0946892EBDCD6DF973D2A2534B2688B9C2390962C85252846FD59F1EF919D`
- `docs/devlog/screenshots/stage4_hd2d_direction_review_sheet.png`
  - SHA256: `A41A842401EC2C8CE088FF4E2808A9846C7091D0901B664144358023E471AB49`

Follow-up refresh later on 2026-05-08:

- Added review-only floor breakup overlays, layered town-backdrop silhouettes, and Chapter1 prefab placement attempts to the proposed HD-2D capture path.
- Refreshed captures:
  - `stage4_hd2d_direction_current.png`: `B6F7F89C8E28379445C309C7B6C94E16B8F08E5F14A4CA472DFC2548CA5F200E`
  - `stage4_hd2d_direction_proposed.png`: `09A6131A04FFFA452FBC8A38E191F9C127BA3EDEAA4D74BEA3E2973CFC086E31`
  - `stage4_hd2d_direction_review_sheet.png`: `64D433E322E3570360ABA4F3A8ADBA6876028C7F02263756EACD3B73A3B6B6F1`
- Visual caveat: the production main-scene walls and existing composition still dominate the review image, so the Chapter1 prefab density remains subtler than the dedicated Antela density capture. This reinforces that a production DQ3R-like jump likely needs a map/camera composition pass, not only renderer or overlay tweaks.

Second follow-up refresh later on 2026-05-08:

- Fixed the proposed-capture preview layer so temporary review geometry is visible through a scoped camera culling-mask expansion, then restored the original culling mask when the capture scope exits.
- Rebalanced the Chapter1 prefab placement attempts toward side / midground density instead of oversized foreground roofs.
- Split the temporary Antela storefront beam so the center character and bed remain readable.
- Brightened the review-only lighting / post-process slightly while keeping the production scene unchanged.
- Refreshed captures:
  - `stage4_hd2d_direction_current.png`: `C2446F802D46B2087176AB248722488DBE14781799C3BAA513A36DD7F726B450`
  - `stage4_hd2d_direction_proposed.png`: `6E62FD0451AAA01E58E59EE91EA972096840B2772418E9F3648B21A3E1237544`
  - `stage4_hd2d_direction_review_sheet.png`: `B2CE6EEF9A9132CA0D86FD9605EC8FF014CD8623F24CFC204290F80EE0823C20`
- Visual caveat: this is now meaningfully denser and more HD-2D-like than the earlier subtle pass, but it is still a procedural review composition. A DQ3R-like result still needs production-authored buildings, floor art, facade sprites/textures, and final camera composition rather than relying only on temporary review geometry.

Third follow-up refresh later on 2026-05-08:

- Moved the HD-2D proposal renderers and review Volume from ad hoc Default-layer visibility to dedicated review visual layer `10`, so captures do not accidentally include unrelated production Default-layer renderers or Volume layers.
- Added dirty-scene refusal and clean main-scene reload behavior to keep `Anemora_Main` free of review-only capture dirt.
- Tracked runtime materials, textures, VolumeProfile, and Volume override instances for destruction when the proposed capture scope exits.
- Rebalanced production-facing floor-breakup / chipped-detail overlay alpha and added left / right side-wear floor pads to the diorama-boundary prefabs to reduce grid dominance without changing gameplay collision.
- Refreshed HD-2D direction captures:
  - `stage4_hd2d_direction_current.png`: `987DBD85609C51164C14F789B4F385FAAE719DED10B0C1A6B1E1C696EAD2B2B0`
  - `stage4_hd2d_direction_proposed.png`: `6A24BB9B4DFBAF728738D756735C46258404F7464159AB7D402DEAD02918D2A4`
  - `stage4_hd2d_direction_review_sheet.png`: `1A778E200BBB21CE5D2964AE3DCDE379EFA9F48A6F1226541B20F943402D81B6`

## Verification

- `Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`
  - Exit code: `0`
  - Checked shader error / shader warning / DrawObjectsPass / RecordRenderGraph / RenderGraph matches: `0`
- `Anemora.EditorTools.Stage4Zone1MaterialSetup.ApplyStage4FloorUnderlayMaterials`
  - Exit code: `0`
  - Applied the floor-surface palette normalization for warm stone, dark stone, moss, and wood slots.
- `Anemora.EditorTools.Stage4DioramaBoundarySetup.ApplyStage4DioramaBoundaries`
  - Exit code: `0`
  - Applied the floor-wear alpha rebalance and side-wear quads to the Current / Past diorama-boundary prefabs.
- `Anemora.EditorTools.Stage4Hd2dDirectionCapture.Capture`
  - Exit code: `0`
  - Checked C# compile error / shader error / shader warning / DrawObjectsPass / RecordRenderGraph / RenderGraph / exception / missing reference matches: `0`
  - Log caveat: Unity licensing handshake and socket startup `Failed` strings appear during batchmode startup.
- `GraphicsFoundationAssetTests`
  - Targeted run: `18/18` passed in the original pass.
  - Follow-up targeted run after character animation baseline review guard: `20/20` passed.
  - Follow-up targeted run after HD-2D proposed-capture culling mask fix: `20/20` passed.
  - Follow-up targeted run after HD-2D capture safety and floor-wear guards: `21/21` passed.
  - Result XML: `%TEMP%/AnemoraCodexLogs/20260508_gfx_foundation_targeted/graphics_foundation_tests_after_floor_surface_palette.xml`
- Full EditMode
  - Unity Test Runner: `58/58` passed
  - Log caveat: Unity licensing handshake `Error` strings and `LogAssemblyErrors` section names remain present; test result is `Passed`.
- `MainSceneStartupLogTests`
  - Targeted run after floor-surface palette normalization: `3/3` passed
  - Result XML: `%TEMP%/AnemoraCodexLogs/20260508_gfx_foundation_targeted/main_scene_startup_log_tests_after_floor_surface_palette.xml`
- Windows build smoke
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-hd2d-direction/Anemora_Stage4_GraphicsFoundation_HD2DDirection_Smoke.exe`
  - Build result: success
  - Build folder: `126.246 MiB`, `192` files
  - Build log caveat: `RenderGraph` / `Exception` matches were build report file-path entries, not runtime failures.
- 30 second player smoke
  - Player was intentionally stopped after the smoke window; process exit code after forced stop: `-1`
  - Checked `Error`, `Exception`, `Assert`, `DrawObjectsPass`, `RecordRenderGraph`, `RenderGraph`, `NullReference`, `MissingReference`, `Failed`, and TMP Essential Resources patterns: `0`

## Next Graphics Work

The next Tier 4 pass should move from temporary review composition toward production scene structure:

- reduce the repeated floor-grid dominance,
- add real background / side-boundary geometry to the playable scene,
- create a production camera-composition review with current gameplay constraints,
- then decide which parts of the HD-2D direction capture should become production defaults.
