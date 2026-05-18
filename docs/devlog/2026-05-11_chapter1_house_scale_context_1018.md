# Chapter 1 House Map Isolation Support - 2026-05-11

Scope: HouseInterior and HouseExterior only. TimeWindow visuals are intentionally not treated as canonical in this note.

## Finding

The current house proof uses the preferred low-risk strategy: active-root isolation.

- `MapRoot_HouseInterior` starts active.
- `MapRoot_HouseExterior` starts inactive.
- `AreaSwitch_HouseInterior_To_HouseExterior` activates the exterior root, deactivates the interior root, and disables its own collider after the switch.

The blocker was not the isolation approach. The exterior visual map had been moved to `HouseExteriorCenter = (8.20, 0.00, 8.20)`, but the switch spawn, exterior section trigger/camera, and disabled plaza cue still used the older `(-4.95, -5.45)` exterior coordinates. That can make the player switch into an empty/hidden area while the real exterior map exists elsewhere.

## Patch Intent

- Keep Niro v12 visual scale at `0.60`.
- Keep HouseInterior center at `(-8.35, 0.00, -8.35)`.
- Keep HouseExterior physically isolated at `(8.20, 0.00, 8.20)`.
- Remove the old broad `Current_Ground_Southwest_NiroHouse` plane from the exterior root because it duplicates the exterior yard and can reintroduce visual overlap.
- Align exterior section/camera, switch spawn, and unavailable plaza cue around `HouseExteriorCenter`.
- Keep the exterior composition requirement:
  - house mass at the back/upper center of the exterior map,
  - door/step readable near the local center,
  - road/path continuing northeast toward the plaza as a disabled future-route cue.

## Concrete Coordinates

HouseInterior:

- Center: `(-8.35, 0.00, -8.35)`.
- Section trigger: `(-8.35, 0.60, -8.35)`, size `(3.70, 1.80, 3.00)`.
- Camera anchor: `(-8.30, 0.25, -8.35)`, orthographic size `1.95`.
- Exit gate: `(-7.15, 0.62, -8.35)`, size `(0.95, 1.35, 0.85)`.

HouseExterior:

- Center: `(8.20, 0.00, 8.20)`.
- Section trigger: `(8.20, 0.60, 8.20)`, size `(4.90, 1.80, 3.80)`.
- Camera anchor: `(8.30, 0.25, 8.35)`, orthographic size `2.15`.
- Spawn after house exit: `(8.20, 0.62, 8.30)`.
- Disabled plaza cue: `(9.92, 0.62, 9.25)`, target marker `(10.75, 0.62, 10.05)`.
- NE road pieces remain visual-only, no collider/gate behavior.

## Guard Added

`Chapter1AreaSwitchGatesConnectHouseInteriorExteriorProof` now asserts that an area switch spawn lies inside the target section trigger bounds. This catches the exact mismatch that caused the house exterior root, camera section, and player spawn to diverge.

## Graphics Minimum Map Kit Integration Support

Graphics delivered a visual-only house kit:

- `Assets/Prefabs/Zone1/Chapter1HouseMinimumMap/Ch1_HouseInterior_MinimumMap.prefab`
- `Assets/Prefabs/Zone1/Chapter1HouseMinimumMap/Ch1_HouseExterior_MinimumMap.prefab`
- `Assets/Art/Materials/Zone1/Chapter1HouseMinimumMap/`

Recommended integration without conflicting with current gate work:

- Copy the kit prefabs/materials/manifest with `.meta` files.
- Instantiate `Ch1_HouseInterior_MinimumMap.prefab` under `MapRoot_HouseInterior`.
- Instantiate `Ch1_HouseExterior_MinimumMap.prefab` under `MapRoot_HouseExterior`.
- Place each prefab at its map center root with local position `(0, 0, 0)`, local rotation identity, and local scale `(1, 1, 1)`.
- Keep `MapRoot_HouseInterior` active at start and `MapRoot_HouseExterior` inactive until `AreaSwitch_HouseInterior_To_HouseExterior`.
- Keep the gameplay gate/collider roots separate from the visual prefabs.
- Disable any `NiroV12_060ScaleGuide_DisableInProduction` children after placement because the scene already contains the real Niro v12 player at visual scale `0.60`.

Suppress or replace these current generated placeholders if the prefabs are integrated:

- `VS_HouseInterior_Floor`
- `VS_HouseInterior_Wall_*`
- `VS_HouseInterior_Bed`
- `VS_HouseInterior_Table`
- `VS_HouseInterior_DoorGlow`
- `VS_HouseInterior_DoorCue`
- `VS_HouseExterior_YardFloor`
- `VS_HouseExterior_House_BackUpperCenter`
- `VS_HouseExterior_Roof_BackUpperCenter`
- `VS_HouseExterior_Door`
- `VS_HouseExterior_DoorStep`
- `VS_HouseExterior_Road_NE_*`
- `VS_HouseExterior_NE_RouteCue_Disabled`
- `VS_HouseExterior_NextRoadCue`
- Any remaining broad `Current_Ground_Southwest_NiroHouse` instance.

Recommended camera bounds after prefab placement:

- HouseInterior: keep orthographic size `1.95` initially. Raise only toward `2.20-2.40` if the kit room bounds crop at runtime.
- HouseExterior: keep orthographic size `2.15` initially. Raise toward `2.40-2.70` only if the house upper-center anchor, arrival patch, and northeast road cue cannot all fit.
- Do not change Niro scale, player collider, movement collision radius, milestone ranges, AreaSwitch trigger size, or TimeWindow visuals as part of this kit import.

## Validation Notes

Scene regeneration is still required for `Anemora_Chapter1.unity`/EXE to pick up the generator changes. Targeted validation should run after regeneration:

- EditMode: `Anemora.Tests.EditMode.Chapter1SceneStructureTests`
- PlayMode: `Anemora.Tests.PlayMode.Chapter1SceneLoadSmokeTests.Chapter1AreaSwitchGatesConnectHouseInteriorExteriorProof`
- Runtime smoke: start inside HouseInterior, interact with the exit cue, confirm the exterior root appears and the player lands in the exterior yard with the house back/upper center and NE road cue visible.
