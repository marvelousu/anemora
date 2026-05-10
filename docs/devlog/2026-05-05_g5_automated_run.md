# G5 Automated Run

Date: 2026-05-05

## 1. Go / No-Go

Result: Go for the automated G5 portion.

This run used a clean temporary worktree at:

`<worktree:Anemora-g5-automated>`

Base commit:

`ec1bbb0 Add locale switch dialog E2E PlayMode test`

Preflight §4 was executed as the automated scope allows:

| Area | Result | Notes |
| --- | --- | --- |
| Environment | Pass | Temporary worktree, `git pull --ff-only origin main` succeeded, no unrelated dirty files before Unity runs. |
| Unity startup | Pass | Unity 6000.3.14f1 opened the project in batchmode; no compile or package import blocker. |
| Tests | Pass | EditMode 32/32 passed; PlayMode 23/23 passed with graphics enabled. |
| Build | Pass | Windows Standalone build succeeded. |
| Scene health | Pass | `Anemora_Main` loaded; missing scripts count 0; required roots and instances found. |
| Asset references | Pass | Character, dialogue, book, and Zone1 prefabs loaded without missing mesh/material references. |
| Localization | Pass | `LocalizationSettings`, ja-JP/en StringTables, and dialogue placeholder resolution tests passed. |
| Audio | Deferred | Clean `origin/main` does not contain `Assets/Audio`; §H remains user/manual verification per this task scope. |

No outright No-Go blocker was found for the automated sections. Full G5 sign-off still requires the user manual sections §H, §I, §L, and §M.

## 2. Automated Results Summary

| Matrix section | Automated result | Evidence |
| --- | --- | --- |
| §A Engine / Pipeline | Pass | URP scene startup, `PortalStencilFeature` present with bit 3 / mask 8; portal stencil smoke test passed. |
| §B Scene / Hierarchy | Pass | Roots, cameras, layer assignments, NPC instances, and ActionRecord reflection root found. |
| §C Symbol / Portal | Pass | `AnemoraMainPortalWiringRoundTripTests`, `TimeFramePortalControllerIntegrationTests`, and hysteresis EditMode tests passed. |
| §D ActionRecord | Pass | `BookReflectorIntegrationTests` and `G4ActionRecordReflectionE2ETests` passed. |
| §E Buildings | Pass | 15 Zone1 prefabs including `Book_Family_Current` loaded; missing mesh/material counts were 0. Visual quality remains user review. |
| §F Character | Pass | `CharacterPrefabStructureTests` and `HeroAnimatorBinderTests` passed. |
| §G Dialogue | Pass | `DialogueAssetIntegrationTests`, `NpcDialogueFlowTests`, and localization switch tests passed. |
| §J Save / Load | Pass | `SaveLoadRoundTripE2ETests` passed; EditMode save/store/migration tests passed. |
| §K Build / Performance | Pass with measurement caveat | Build succeeded, player launched, baseline performance reflected, 30s external runtime memory/CPU/VRAM sampled. |

Manual/user sections recorded in the matrix as not executed:

| Matrix section | Status |
| --- | --- |
| §H Audio | 未実施 (user 検証用) |
| §I UI / Localization visual check | 未実施 (user 検証用) |
| §L E2E manual playthrough | 未実施 (user 検証用) |
| §M 層 2 片鱗演出 | 未実施 (user 検証用) |

## 3. Test Results

| Test platform | Result | Notes |
| --- | ---: | --- |
| EditMode | 32/32 passed | Includes ActionRecord, SaveEnvelope, SaveMigration, Character prefab/import, Dialogue data, and portal hysteresis tests. |
| PlayMode | 23/23 passed | Includes portal roundtrip, stencil smoke, ActionRecord reflection, Hero binder, Dialogue/NPC, locale switch, and Save/Load E2E tests. |

One PlayMode attempt with `-nographics` failed in `PortalStencilFeatureSmokeTest` because RenderTexture creation requires a graphics device. The accepted G5 run was re-executed without `-nographics` and passed 23/23.

## 4. Build / Performance

Windows Standalone output:

`<worktree:Anemora-g5-automated>\Builds\G5Automated\Anemora_G5.exe`

Build metrics:

| Metric | Value |
| --- | ---: |
| Build result | Success |
| Wall-clock build duration | 96.048 s |
| Unity BuildReport complete size | 114.9 MB |
| Disk folder size | 115.081 MiB |

Build folder breakdown:

| Item | Size |
| --- | ---: |
| `Anemora_G5_Data/` | 64.850 MiB |
| `UnityPlayer.dll` | 34.657 MiB |
| `MonoBleedingEdge/` | 8.700 MiB |
| `D3D12/` | 4.506 MiB |
| `UnityCrashHandler64.exe` | 1.547 MiB |
| `Anemora_G5.exe` | 0.637 MiB |
| `Anemora_BurstDebugInformation_DoNotShip/` | 0.185 MiB |

Runtime external sample:

| Metric | Value |
| --- | ---: |
| Sample duration | 30 s |
| Window ready time | 5.542 s |
| Working set average / peak | 187.983 MiB / 189.625 MiB |
| Private memory average | 280.374 MiB |
| CPU average / peak | 1.273% / 2.202% of machine |
| GPU dedicated memory average / peak | 31.527 MiB / 31.531 MiB |
| GPU shared memory average / peak | 19.332 MiB / 19.332 MiB |

Performance baseline reflected from `2e3569f`:

| Baseline item | Value |
| --- | --- |
| Standalone average FPS | 59.909 FPS at 1920 x 1200 |
| Standalone p95 frame time | 16.683 ms |
| Baseline GPU dedicated peak | 78.430 MiB |
| Baseline GPU shared peak | 41.598 MiB |
| Baseline build folder size | 115.056 MiB |

G5 actual FPS was not remeasured because the committed player has no in-build frame sampler and PresentMon is not installed. The build was still launched and externally sampled for CPU, process memory, and GPU process memory.

## 5. Issues

| Issue | Status | Notes |
| --- | --- | --- |
| URP RenderGraph warning for `DrawObjectsPass` | Existing caveat / escalation candidate | Observed in PlayMode and player logs. Automated portal/stencil tests pass, so this is not an automated No-Go blocker. |
| `Assets/Audio` absent from clean `origin/main` | User/manual pending | Audio §H cannot be validated from the clean G5 worktree. |
| G5 actual FPS not captured | Tooling caveat | Need PresentMon or an in-game sampler for future true runtime FPS remeasurement. |

No implementation file was changed as part of this run.

## 6. User Manual Procedure

Use the Windows build above.

1. Launch `Anemora_G5.exe`.
2. Confirm the player appears and movement responds.
3. Open SymbolWheel and choose the red symbol.
4. Enter the time window from Current to Past.
5. Interact with the Past book.
6. Return to Current and verify one `Book_Family_Current` appears on the bed.
7. Listen/observe §H audio, §I UI, §L full flow feel, and §M layer-2 hint if present.

## 7. Conclusion

Automated G5 status: Pass for automated sections A-G, J, and K.

Manual G5 status: pending user review for H, I, L, and M.

The main automated risks to carry forward are the repeated URP RenderGraph warning and the lack of a committed runtime FPS sampler.
