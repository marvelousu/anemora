# Stage 4 Scale Lineup Capture

Status: v0.1 recorded 2026-05-07

## 1. Purpose

This task adds a repeatable Editor capture path for Stage 4 scale review. It supports the 2026-05-07 runtime feedback that Residents may read too large / Hero may read too small and that the current first map has wrong object scale.

The capture is a review artifact only. It does not modify `Anemora_Main`, prefabs, sprites, or runtime scene layout.

## 2. Added Automation

- `Assets/Editor/Stage4ScaleReviewCapture.cs`
  - Menu item: `Anemora/Review/Capture Stage4 Scale Lineups`
  - Batch entry point: `Anemora.EditorTools.Stage4ScaleReviewCapture.CaptureAll`
  - Creates temporary unsaved scenes, instantiates Hero / Residents / Zone1 prop prefabs, captures orthographic 1920 x 1080 PNGs, then returns to the previously open scene.
  - Disables character Animators for capture and explicitly assigns v2 idle sprites so scale review does not depend on Editor Animator evaluation.

## 3. Captures

- `docs/devlog/screenshots/stage4_scale_lineup_current_demo.png`
  - Uses the `targetMaxDimension` values currently hardcoded in `AnemoraDemoSceneSetup`.
  - Shows the current demo normalization where door is `0.9m`, bed `1.05m`, fountain `1.25m`, house `2.1m`, and library `2.1m`.
- `docs/devlog/screenshots/stage4_scale_lineup_target_metrics.png`
  - Uses the draft targets from `docs/level_design/scale_metrics.md`.
  - Shows door `2.1m`, bed `2.0m`, table `1.2m`, fountain `2.2m`, streetlamp `2.6m`, house `4.0m`, and library `6.0m`.

## 4. Findings

- Hero and Resident_A are the same technical height in the lineup. Resident_A still reads visually larger than desired because the face / hair mass and eye density are stronger, not because of prefab scale.
- Resident_B is equal technical height but has a much wider seated / dark-mass silhouette, so it can read larger than Hero and Resident_A.
- The current demo normalization compresses large environmental objects heavily. This explains why the current playable map can read like a small demo board rather than a city block.
- The target metrics lineup makes the intended relationship clearer: Niro and ordinary residents are small within a larger world, while houses and the library regain landmark scale.

## 5. Verification

Executed:

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -batchmode -projectPath "<worktree>" -executeMethod Anemora.EditorTools.Stage4ScaleReviewCapture.CaptureAll -logFile stage4_scale_review_capture.log -quit
```

Result:

- Unity exit code: `0`
- Script compile: no `error CS` matches
- Runtime exception scan: no actionable exception / compile failure matches
- EditMode suite: `39/39 passed` (`stage4_editmode_scale_lineup.xml`)
- Expected transient Unity licensing / socket messages appeared during startup and resolved before successful quit.
- Generated ProjectSettings / Addressables side effects from batchmode were restored before staging.

## 6. Next

- Use these lineups as the objective input for the next user review.
- Use `target_metrics` dimensions as the starting point for Zone1 greybox scale, unless user review prefers a smaller or larger first-map read.
- Keep current `Anemora_Main` demo layout as a VS / wiring fixture until the blockout and scale lineup gate passes.
