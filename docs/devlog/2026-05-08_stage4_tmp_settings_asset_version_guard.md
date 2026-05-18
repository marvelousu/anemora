# Stage 4 TMP Settings Asset-Version Guard

Date: 2026-05-08

## Summary

Removed the Editor shutdown error:

```text
TextMesh Pro Essential Resources are missing
```

The required TMP Settings asset already existed, but its serialized `assetVersion` field was empty. TMP package code treats that as an outdated / missing essential resource state when the Editor shuts down.

This is a graphics/UI foundation cleanup only. It does not change dialogue layout, font art direction, localization strings, camera, scene layout, or runtime text content.

## Changes

- `Assets/TextMesh Pro/Resources/TMP Settings.asset`
  - Set `assetVersion: 2`, matching the current TMP package `TMP_Settings.s_CurrentAssetVersion`.
- `Assets/Editor/AnemoraTmpSettingsUtility.cs`
  - Adds a small Editor helper that reads the current TMP package asset version by reflection and normalizes the serialized TMP Settings asset.
- `Assets/Editor/AnemoraTmpJapaneseAtlasBuilder.cs`
  - Normalizes TMP Settings asset version when creating or reusing the settings asset.
- `Assets/Editor/AnemoraDemoSceneSetup.cs`
  - Preserves the same normalization in demo scene setup so the error is not regenerated.
- `Assets/Tests/EditMode/LocalizationFontCoverageTests.cs`
  - Adds `TmpSettingsAssetVersionMatchesTextMeshProPackage`.

## Verification

Targeted EditMode:

```powershell
Start-Process -FilePath "C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe" -ArgumentList @(
  "-batchmode",
  "-projectPath", "<worktree>",
  "-runTests",
  "-testPlatform", "EditMode",
  "-testFilter", "Anemora.Tests.EditMode.LocalizationFontCoverageTests",
  "-testResults", "<worktree>\stage4_gfx_tmp_editmode.xml",
  "-logFile", "<worktree>\stage4_gfx_tmp_editmode.log"
) -Wait -PassThru -WindowStyle Hidden
```

Result:

- Unity exit code: `0`
- `LocalizationFontCoverageTests`: `4/4 passed`
- No `TextMesh Pro Essential Resources`, C# compile, shader compile, unhandled exception, or assertion match

Full PlayMode:

- Unity exit code: `0`
- PlayMode: `33 passed / 34 total`, with one manual capture skipped
- No `TextMesh Pro Essential Resources`, `DrawObjectsPass`, `RecordRenderGraph`, C# compile, or shader compile match
- The only broad `Exception` text matches in the log are Test Runner stack frame type names containing `RecordExceptions`.

Full EditMode:

- Unity exit code: `0`
- EditMode: `41/41 passed`
- No `TextMesh Pro Essential Resources`, C# compile, shader compile, unhandled exception, or assertion match

## Caveats

- This does not import TMP Examples & Extras.
- This does not change default TMP font selection; existing dialogue/UI prefab font references remain the source of truth.
- The fix only addresses the TMP Settings package-version check; rendered font quality remains a separate manual review topic.

## Next Graphics Foundation Tasks

- Keep reducing Editor / player-log noise before any heavier post-process changes.
- Build a second graphics baseline capture for Volume / post-process review.
