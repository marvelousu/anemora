# VS Playable Failure Diagnostic

Date: 2026-05-05

Target build:

`C:\Users\maro6\Documents\Unity\Anemora-g5-audio-build\Builds\G5Audio\Anemora_G5_Audio.exe`

Target commit:

`e6e3c61 Rebuild G5 build with audio and refresh §K metrics`

## 1. Scope

This diagnostic investigates why the audio-enabled G5 Windows Standalone build opened to an unplayable view: two floating boxes, no Zone1 audio, no Hero / NPC / building visuals, and no usable controls.

No implementation, scene, project setting, or asset change is included in this task. The only intended repository change is this diagnostic report.

## 2. BuildReport / Build Artifact Findings

Sources checked:

- `C:\Users\maro6\Documents\Unity\Anemora-g5-audio-build\Library\Bee\buildreport.json`
- `C:\Users\maro6\Documents\Unity\Anemora-g5-audio-build\Library\LastBuild.buildreport`
- `C:\Users\maro6\Documents\Unity\Anemora-g5-audio-build\Builds\G5Audio\Anemora_G5_Audio_Data\globalgamemanagers`
- `C:\Users\maro6\Documents\Unity\Anemora-g5-audio-build\ProjectSettings\EditorBuildSettings.asset`

Build scene order is the primary finding:

| Scene index | Scene | Evidence |
| ---: | --- | --- |
| 0 | `Assets/Scenes/Sandbox_E1_Stencil.unity` | `EditorBuildSettings.asset`, `LastBuild.buildreport`, and built `globalgamemanagers` all list this first. |
| 1 | `Assets/Scenes/Anemora_Main.unity` | Present in the build, but second. |

Unity Standalone starts scene index 0 by default. Therefore `Anemora_G5_Audio.exe` starts in `Sandbox_E1_Stencil`, not `Anemora_Main`.

Asset inclusion status from `LastBuild.buildreport`:

| Asset family | Status | Notes |
| --- | --- | --- |
| `Anemora_Main.unity` | Included | Packaged as scene 1, not startup scene. |
| Hero sprites | Included | `hero_idle`, `hero_walk_front`, `hero_walk_back`, `hero_walk_right` were packed. |
| Resident sprites | Included | `resident_a_idle`, `resident_a_walk_front`, `resident_b_idle` were packed. |
| Character animators/clips | Included | Hero / Resident A / Resident B animator controllers and clips were packed. |
| Zone1 audio | Included | `Zone1_Ambient.ogg` and the canonical 30 `Assets/Audio/SFX/Zone1/...` clips were packed. |
| URP assets | Included | `UniversalRenderPipeline.asset`, renderer asset, global settings, default volume profile were packed. |
| Zone1 buildings | Not included except book assets | BuildReport contains `Book_Family_Current.prefab`, `Book_Family_Past.fbx`, and its texture, but not `House_Player`, `Plaza_Fountain_Dry_Broken`, `Library_Ruin`, `StreetLamp`, `Tree_Decay`, `Floor_Stone`, or `Floor_Wood`. This is because `Anemora_Main` does not instantiate those building prefabs. |

Build log warnings / errors:

- No build failure was found in the available BuildReport data.
- The only explicit BuildReport warning-like entry found was Unity's splash screen inclusion note.
- Player runtime log contains repeated URP RenderGraph warning lines, but no `NullReferenceException`, `MissingReferenceException`, missing script, missing asset, or audio load failure entries were found with local log search.

## 3. Player.log Findings

Latest local Player log checked:

`C:\Users\maro6\AppData\LocalLow\DefaultCompany\Anemora\Player.log`

Key lines:

- Process path points to `Anemora-g5-audio-build\Builds\G5Audio\Anemora_G5_Audio_Data`.
- Engine initialized as Unity `6000.3.14f1`.
- GPU path initialized successfully on AMD Radeon integrated graphics through D3D11.
- `DrawObjectsPass does not have an implementation of the RecordRenderGraph method` appears 2446 times in the latest local log before shutdown.
- No game-side exceptions or missing-reference errors were found by searching for `Exception`, `Error`, `NullReference`, `MissingReference`, `failed`, `Audio`, `Zone1Audio`, `Hero`, `Resident`, or `Niro`.

The Player log therefore does not indicate a missing asset crash. It is consistent with a valid Unity player running the wrong startup scene.

## 4. Scene Hierarchy Findings

### 4.1 Startup Scene: `Sandbox_E1_Stencil`

`Assets/Scenes/Sandbox_E1_Stencil.unity` contains:

| GameObject | Relevant details |
| --- | --- |
| `Main Camera` | Active, `AudioListener` present, position `(0, 1.25, -4.2)`, FOV 45, culling mask all layers. |
| `Directional Light` | Active. |
| `PortalMask_Quad` | Active stencil/debug object. |
| `Reference_Floor` | Active. |
| `Reference_Current_Cube_OutsidePortal` | Active, layer 10, position `(-1.45, 0.45, 0.65)`, cube mesh/collider. |
| `InsideOnly_Cube_VisibleThroughPortal` | Active, layer 11, position `(0, 1, 1.45)`, cube mesh/collider. |

Absent from `Sandbox_E1_Stencil`:

- `Player`
- `PrototypePlayerController`
- `TimeFramePortalController`
- `Zone1_Audio`
- `Zone1AudioController`
- Hero / Resident prefab instances
- Dialogue UI
- ActionRecord runtime
- Zone1 building instances

This exactly matches the user symptom: two visible boxes, no Zone1 audio, no Hero/NPC/building graphics, and no playable controls.

### 4.2 Main Scene: `Anemora_Main`

`Assets/Scenes/Anemora_Main.unity` is included in the build and has more VS wiring:

| Item | Finding |
| --- | --- |
| `Main Camera` | Active, FOV 45, near/far `0.1 / 100`, position `(0, 2.35, -5.5)`, `AudioListener` present. |
| Main camera culling | `m_Bits: 1056`, which is UI layer 5 plus `Layer_Current_Visual` 10. This is consistent with the current-side visual approach, not an all-culling failure. |
| `Directional Light` | Active. |
| `Player` | Present with `PrototypePlayerController`, layer 8. |
| Hero visuals | Present as Hero prefab instances, but there are multiple `Player_Visual_Current` / `Player_Visual_Past` prefab instances under `Player`, suggesting duplicate visual children. This is not the cause of the two-box startup failure, but it is a cleanup risk for the next repair task. |
| `Resident_A_Instance` / `Resident_B_Instance` | Present as prefab instances, layer 11. |
| `Zone1_Audio` | Present and active with `Zone1AudioController`, `autoPlayOnStart: 1`, music source, ambience sources, one-shot source, and clip references assigned. |
| `TimeFramePortalSystem` | Present with `TimeFramePortalController`, portal prefab, crossing detector, visual switcher, and action record/book reflector wiring. |
| Buildings | Not present as scene instances. Only placeholder floor/bed/table/book objects are present. The 14 Zone1 building/environment prefabs are loadable assets, but not integrated into `Anemora_Main`. |
| Niro monologue | `e6e3c61` predates the later Niro commits; the failed binary does not contain `NiroMonologueController`. Current `origin/main` does contain it on `Player`, but current build settings still list `Sandbox_E1_Stencil` first. |

## 5. Editor Play vs Build Comparison

GUI Editor Play was not re-run in this diagnostic pass. The deterministic configuration difference is sufficient:

- Automated tests and manual Editor checks that explicitly load/open `Assets/Scenes/Anemora_Main.unity` validate scene 1.
- The Windows Standalone player starts scene 0 from `EditorBuildSettings`.
- Scene 0 is `Sandbox_E1_Stencil`, which intentionally contains only the stencil debug setup and two cube objects.

This explains how tests, scene verifiers, and build success could all pass while the generated executable was not the vertical slice.

## 6. "Two Boxes" Identity

The two visible boxes are most likely these `Sandbox_E1_Stencil` objects:

1. `Reference_Current_Cube_OutsidePortal`
2. `InsideOnly_Cube_VisibleThroughPortal`

They are not failed Hero/NPC/building fallback meshes. They are intentional debug cube objects in the stencil sandbox scene.

## 7. Cause Classification

| Class | Result | Evidence |
| --- | --- | --- |
| A. Sprite / asset reference broken | Not primary | Hero and Resident sprites are packed. No missing-reference log was found. |
| B. Camera / lighting setting problem | Not primary | Startup scene camera and light are valid; it is simply the wrong scene. `Anemora_Main` camera/listener also look valid from YAML. |
| C. URP renderer feature over-culling | Not primary | URP warning exists, but the symptom is explained before rendering features: the player starts `Sandbox_E1_Stencil`. |
| D. Build pipeline / scene list problem | Primary root cause | `Sandbox_E1_Stencil` is enabled as build scene index 0; `Anemora_Main` is index 1. |
| E. Audio system failure cascade | Not primary | Audio assets and `Zone1_Audio` are wired in `Anemora_Main`, but that scene is not loaded at startup. |
| F. Other | Secondary content integration gaps | `Anemora_Main` has no Zone1 building instances, and Player has duplicate Hero visual prefab instances. These would remain after fixing scene order. |

## 8. Repair Direction (Stage 0)

Recommended repair sequence:

1. Fix Build Settings so `Assets/Scenes/Anemora_Main.unity` is the first enabled scene for VS/G5 player builds, or build with an explicit scene list containing only `Anemora_Main`.
2. Keep `Sandbox_E1_Stencil.unity` out of release/manual-review builds; retain it only as a developer test scene.
3. Rebuild Windows Standalone and confirm the first Player log/session opens `Anemora_Main` rather than sandbox.
4. Add a build/preflight guard that fails if the first enabled build scene is not `Assets/Scenes/Anemora_Main.unity`.
5. Add a runtime smoke check or PlayMode test that validates the built-player startup scene contract, not just `SceneManager.LoadScene("Anemora_Main")`.
6. Integrate the 14 Zone1 building/environment prefabs into `Anemora_Main`; current loadability tests are insufficient for visual slice readiness.
7. Clean up duplicate Hero visual prefab instances under `Player` and add a scene assertion for exactly one current and one past Hero visual.

Estimated repair size:

- Scene-order/build guard: light.
- Building placement and visual validation: medium.
- Full playable VS acceptance repair with screenshot/manual gates: medium.

## 9. Conclusion

The catastrophic failure is not an asset import crash and not an audio decoder failure. The built executable starts the stencil sandbox scene because `ProjectSettings/EditorBuildSettings.asset` lists `Sandbox_E1_Stencil.unity` before `Anemora_Main.unity`. The two visible boxes are the sandbox debug cubes `Reference_Current_Cube_OutsidePortal` and `InsideOnly_Cube_VisibleThroughPortal`.

The first repair task should change the build startup scene contract and add a guard so this cannot pass automated G5 again. After that, `Anemora_Main` still needs visual-content integration checks because Zone1 building prefabs are not scene instances and Hero visual instances appear duplicated.
