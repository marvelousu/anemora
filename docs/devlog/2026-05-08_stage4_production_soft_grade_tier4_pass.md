# Stage 4 production soft grade Tier 4 pass

Date: 2026-05-08
Scope: Tier 4 graphics push / production post-process baseline
Branch: `codex/stage4-graphics-foundation-20260508`

## Summary

Promoted the restrained Stage 4 soft-grade direction into the production main scene with a dedicated global Volume. This keeps the review-only profile separate, keeps the portal flash Volume separate, and avoids changing camera framing, map layout, story, character art, or palette direction.

## Changes

- `Assets/Settings/Stage4ProductionSoftGradeVolumeProfile.asset`
  - Adds a production-facing global grade profile.
  - Uses restrained ColorAdjustments, Bloom, and Vignette values:
    - postExposure `0.03`
    - contrast `5`
    - saturation `3`
    - bloom threshold `1.2`
    - bloom intensity `0.07`
    - bloom scatter `0.4`
    - vignette intensity `0.045`
    - vignette smoothness `0.28`

- `Assets/Scenes/Anemora_Main.unity`
  - Adds `Stage4 Production Soft Grade Volume` as a separate global Volume.
  - Assigns the production soft-grade profile at `weight: 1`.
  - Leaves the existing portal flash Volume on its own runtime profile path.

- `Stage4GraphicsBaselineCapture`
  - Adds production profile creation automation.
  - Adds an Editor API entry point for assigning the production soft grade to the main scene.
  - Keeps the review profile values distinct from the production profile.

- `GraphicsFoundationAssetTests.Stage4ProductionSoftGradeProfileIsAssignedToMainScene`
  - Guards production profile values.
  - Guards that `Anemora_Main` references the production profile and not the review-only profile.

## Capture Artifacts

Updated by `Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview` after the production Volume was assigned:

- `docs/devlog/screenshots/stage4_main_scene_graphics_current.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_proposed_soft.png`
- `docs/devlog/screenshots/stage4_main_scene_graphics_review_sheet.png`

Stable SHA-256 hashes:

- `stage4_main_scene_graphics_current.png`: `4CCB69FACFA86D18B1C3BE8D6F99A80B8CBCDE64CAE01ACE1638279CEC061366`
- `stage4_main_scene_graphics_proposed_soft.png`: `7BA7DE3CB94C684681701B606E1D17FA2AFD962815C0C2E0B2878EF8FCDFC244`
- `stage4_main_scene_graphics_review_sheet.png`: `FE9AB79668F8B3E49E78A99589CB9E18CB5C34F6BCEEFD324C0F681D5B7E18CB`

## Verification

- Production profile / scene apply executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.ApplyProductionSoftGradeToMainScene`
  - Result: Unity exit code `0`

- Main-scene capture executeMethod
  - Command: `-executeMethod Anemora.EditorTools.Stage4GraphicsBaselineCapture.CaptureMainSceneSoftGradeReview`
  - Result: Unity exit code `0`

- Targeted EditMode
  - Command: `-runTests -testPlatform EditMode -testFilter Anemora.Tests.EditMode.GraphicsFoundationAssetTests`
  - Result: `13/13` passed

- Full EditMode
  - Result: `53/53` passed
  - Source scan: EditMode `52`, PlayMode `34`

- Targeted PlayMode
  - Command: `-runTests -testPlatform PlayMode -testFilter Anemora.Tests.PlayMode.MainSceneStartupLogTests`
  - Result: `3/3` passed

- Windows build smoke
  - Output: `Builds/Stage4Smoke/2026-05-08-graphics-foundation-production-soft-grade/Anemora_Stage4_GraphicsFoundation_ProductionSoftGrade_Smoke.exe`
  - Build folder files: `192`
  - Build folder disk size: `131,789,556` bytes / `125.684 MiB`
  - Unity exit code: `0`
  - Build log marker: `Build Finished, Result: Success.`

- Player smoke
  - Ran generated Windows player for 30 seconds at `1280 x 720`, fullscreen off.
  - Process exit `-1` is expected because the smoke window stops the player.
  - Checked player-log patterns: Error `0`, Exception `0`, Assert `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, NullReference `0`, MissingReference `0`, Failed `0`, TextMesh Pro Essential Resources `0`.

Checked editor/test/capture log patterns: `error CS` `0`, shader error `0`, shader warning `0`, exception `0`, assertion `0`, DrawObjectsPass `0`, RecordRenderGraph `0`, RenderGraph `0`, TextMesh Pro Essential Resources `0`, NullReference `0`, MissingReference `0`, YAML `0`.

Build log caveats:

- `Exception` matches are package path / reflection lines, not runtime exceptions.
- `RenderGraph` / `DrawObjectsPass` matches are build size report asset paths, not runtime warnings.

## Tier 4 Notes

- This is production-facing and visible in `Anemora_Main`, unlike the earlier review-only soft-grade profile.
- The values are deliberately lower than the review profile to avoid locking the final palette or Zone1 lighting mood too early.
- Longer FPS / memory profiling should be refreshed after the next lighting-density or material-density pass.
