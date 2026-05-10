# VS Playable Fix Rebuild

Report date: 2026-05-05

Execution date: 2026-05-06 JST

Source commit:

`6005b10 Fix VS playable failure by reordering build scenes`

Temporary worktree:

`<worktree:Anemora-vs-fix-build>`

## 1. Scope

This task rebuilt the Windows Standalone player after the scene-order fix that moved `Assets/Scenes/Anemora_Main.unity` to build index 0 and kept `Assets/Scenes/Sandbox_E1_Stencil.unity` at index 1.

No scene, setting, script, prefab, asset, or import-setting fix is included in this task. The only intended repository change is this devlog.

## 2. Build Output

| Item | Result |
| --- | --- |
| Build target | Windows Standalone 64-bit |
| Unity | `6000.3.14f1` |
| Output exe | `<worktree:Anemora-vs-fix-build>\Builds\VSFix\Anemora_VS_Fix.exe` |
| Build result | Success |
| Build duration | `69.608s` |
| Build payload size | `117.869 MiB` |
| Build payload files | `193` |

Build payload size excludes local verification artifacts generated beside the player after launch (`Anemora_VS_Fix_Player.log`, `runtime_start.png`, `runtime_after_w.png`).

## 3. Scene Startup Verification

`ProjectSettings/EditorBuildSettings.asset` in the source commit lists:

| Build index | Scene |
| ---: | --- |
| 0 | `Assets/Scenes/Anemora_Main.unity` |
| 1 | `Assets/Scenes/Sandbox_E1_Stencil.unity` |

`Library/LastBuild.buildreport` confirms the same build order:

| Build step | Scene |
| --- | --- |
| scene 0 | `Assets/Scenes/Anemora_Main.unity` |
| scene 1 | `Assets/Scenes/Sandbox_E1_Stencil.unity` |

The built `Anemora_VS_Fix_Data/globalgamemanagers` also contains `Assets/Scenes/Anemora_Main.unity` before `Assets/Scenes/Sandbox_E1_Stencil.unity`.

Runtime screenshot verification shows the build no longer opens to the two-cube stencil sandbox. The first rendered view contains the Anemora main-scene layout: Hero/Niro sprite content, a seated resident/NPC sprite, bed-side objects, and the scene floor/background.

## 4. PlayMode Verification

PlayMode tests were run in batchmode without `-nographics`:

| Metric | Result |
| --- | --- |
| Test result | Passed |
| Total | `27` |
| Passed | `27` |
| Failed | `0` |
| Skipped | `0` |
| Duration | `10.8766788s` |

Relevant covered tests include:

- `AnemoraMainPortalWiringRoundTripTests`
- `Zone1AudioWiringTests`
- `NpcDialogueFlowTests`
- `SaveLoadRoundTripE2ETests`
- `StressSampleRunnerSmokeTests`

The first attempts using Unity test runner with `-quit` imported and compiled the project but exited before writing results. The successful run omitted `-quit`; Unity exited cleanly after test completion.

## 5. Runtime 30s Sample

The player was launched with a 1280x720 window and sampled for 30 seconds.

| Metric | Result |
| --- | --- |
| Window ready time | `6.236s` |
| All samples responding | `true` |
| Avg working set | `296.576 MiB` |
| Peak working set | `297.727 MiB` |
| Avg private bytes | `400.091 MiB` |
| Peak private bytes | `400.102 MiB` |
| Avg CPU | `1.557%` |
| Peak CPU | `3.677%` |

The player process stayed responsive through the sample. A scripted `W` key injection did not produce a clear visual movement delta between screenshots, so manual control verification is still required.

Audio could not be auditorily verified from this Codex session. The runtime log did not report audio-load errors, and `Zone1AudioWiringTests` passed as part of the 27/27 PlayMode run. User listen verification remains required.

## 6. Runtime Log Findings

Player log checked:

`<worktree:Anemora-vs-fix-build>\Builds\VSFix\Anemora_VS_Fix_Player.log`

| Pattern | Count | Notes |
| --- | ---: | --- |
| `DrawObjectsPass` | `4002` | Existing URP RenderGraph warning caveat remains. |
| `Font Atlas Texture` | `3` | `Anemora_JP` atlas missing for `DialogueText`, `SpeakerLabel`, `AdvanceIndicator`. |
| `NullReferenceException` | `3` | TMP `MaterialReference` / `TextMeshProUGUI.OnPreRenderCanvas` path after missing font atlas. |
| `MissingReferenceException` | `0` | No missing reference exception found. |

The TMP font atlas issue is a separate runtime blocker/caveat from the scene-order fix. It was not repaired in this rebuild task.

## 7. Visual Caveats

The catastrophic startup-scene failure is fixed: the player starts in `Anemora_Main`, not `Sandbox_E1_Stencil`.

The rebuilt player is still not a clean final playable pass:

- A white square and a gray box-like artifact are visible around the Hero/Niro area in the runtime screenshot.
- The scripted movement smoke check was inconclusive.
- Audio needs human listen verification.
- TMP font atlas missing errors and three TMP `NullReferenceException` entries remain in Player.log.

These issues should be handled as follow-up fixes or manual G5 verification findings. They are outside the rebuild-only scope of this task.

## 8. Conclusion

`Anemora_VS_Fix.exe` is the correct build to provide for post-fix user verification because it starts from `Anemora_Main` and no longer reproduces the two-cube sandbox startup failure.

The build should not yet be treated as a fully clean VS playable candidate. Remaining findings are TMP font atlas runtime errors, visible Hero-area artifacts, unverified audible audio, and inconclusive scripted movement confirmation.
