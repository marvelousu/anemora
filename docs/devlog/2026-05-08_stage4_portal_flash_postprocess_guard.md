# Stage 4 Portal Flash Post-Process Guard

Date: 2026-05-08

## Summary

Enabled the already-wired portal flash Volume path in `Anemora_Main` and added PlayMode guards so the effect cannot silently become camera-disabled again.

This is a narrow graphics-foundation fix. It does not change camera composition, lighting direction, map layout, character art, palette, or the time-window gameplay logic.

## Changes

- `Assets/Scenes/Anemora_Main.unity`
  - Enabled Main Camera URP post-processing (`m_RenderPostProcessing: 1`).
  - This allows `PortalFlashPlayer` to drive its runtime `ColorAdjustments.postExposure` flash through the scene's global Volume.
- `Assets/Tests/PlayMode/MainSceneStartupLogTests.cs`
  - Added `MainSceneCameraPostProcessingSupportsPortalFlash`.
  - Added `PortalFlashPlayerCreatesRuntimeProfileAndAnimatesWeight`.
- `docs/VERIFICATION_SUITE.md`
  - Updated source scan from EditMode 38 / PlayMode 32 to EditMode 38 / PlayMode 34.
  - Recorded the targeted PlayMode result.

## Verification

Targeted PlayMode:

```powershell
Start-Process -FilePath "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -ArgumentList @(
  "-batchmode",
  "-projectPath", "<worktree>",
  "-runTests",
  "-testPlatform", "PlayMode",
  "-testFilter", "Anemora.Tests.PlayMode.MainSceneStartupLogTests",
  "-testResults", "<worktree>\stage4_gfx_main_scene_playmode.xml",
  "-logFile", "<worktree>\stage4_gfx_main_scene_playmode.log"
) -Wait -PassThru -WindowStyle Hidden
```

Result:

- Unity exit code: `0`
- `MainSceneStartupLogTests`: `3/3 passed`
- Script compile: no `error CS` after the final run
- Unity-generated `AddressableAssetsData/link.xml`, `ProjectSettings/ProjectSettings.asset`, temporary `SceneTemplateSettings.json`, and local test log / XML artifacts were restored or removed before staging.

Full suite refresh:

- EditMode: `39/39 passed`
- PlayMode: `33 passed / 34 total`, with one manual capture skipped
- Unity exit code: `0` for both runs
- No `DrawObjectsPass` / `RecordRenderGraph` matches in the final full-suite logs

## Caveats

- This is not a full PlayMode / EditMode suite refresh.
- No Windows standalone build or player-log smoke has been run yet after this change.
- `DefaultVolumeProfile.asset` remains empty. `PortalFlash_VolumeProfile.asset` now carries inspectable `ColorAdjustments` defaults matching the runtime flash path, while `PortalFlashPlayer` still creates and owns a runtime profile instance at play time.

## Next Graphics Foundation Tasks

- Capture a current/proposed visual baseline sheet for the local time-window effect.
- Consider a custom lightweight transparent veil shader for `TimeVolume_SpaceVeil`.
- Re-run build/player-log smoke after the next visible shader or capture automation change.
