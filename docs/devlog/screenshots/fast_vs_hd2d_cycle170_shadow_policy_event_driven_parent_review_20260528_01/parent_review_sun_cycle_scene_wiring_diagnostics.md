# HD2D Phase A Sun Cycle Scene Wiring Diagnostics

- Scene: `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
- Root: `FastVS_HD2D_SunCycle`
- Validate entry: `ValidateHd2dPhaseASunCycleSceneWiringBatch`
- Capture entry: `CaptureHd2dPhaseASunCycleSceneWiringCycle166ScreenshotsBatch`
- Default preset: `Noon`
- Transition duration: `1.8`

| Preset | Asset |
|---|---|
| Morning | `Assets/Settings/SunCycle/SunPreset_Morning.asset` |
| Noon | `Assets/Settings/SunCycle/SunPreset_Noon.asset` |
| Evening | `Assets/Settings/SunCycle/SunPreset_Evening.asset` |
| Night | `Assets/Settings/SunCycle/SunPreset_Night.asset` |

| Anchor | Parent | Preset | Transition | Priority |
|---|---|---:|---:|---:|
| `FastVS_HD2D_MapSunAnchor_Interior_Morning` | `Current_HouseInteriorMap_SeparateSpace` | Morning | False | 0 |
| `FastVS_HD2D_MapSunAnchor_Exterior_Morning` | `Current_HouseExteriorMap_SeparateSpace` | Morning | False | 0 |
| `FastVS_HD2D_MapSunAnchor_CentralPlaza_Noon` | `Current_CentralPlazaMap_SeparateSpace` | Noon | True | 0 |
| `FastVS_HD2D_MapSunAnchor_Library_Evening` | `Current_LibraryMap_SeparateSpace` | Evening | True | 0 |

| Screenshot |
|---|
| `01_current_house_interior_sun_cycle_morning.png` |
| `02_current_house_exterior_sun_cycle_morning.png` |
| `03_current_central_plaza_sun_cycle_noon.png` |
| `04_current_library_sun_cycle_evening.png` |

- YAML grep: passed for root, anchors, driver references, preset GUIDs, default preset, and transition duration.
