# HD2D Cycle 180 Story Sun and Plaza Shafts Diagnostics

- Scene: `Assets/Scenes/Anemora_FastVS_HouseSlice.unity`
- Validate entry: `ValidateHd2dCycle180StorySunAndPlazaShaftsBatch`
- Capture entry: `CaptureHd2dCycle180StorySunAndPlazaShaftsScreenshotsBatch`
- Anchor table check: all Phase A map anchors are Morning before the library-exit beat.

| Anchor | Parent | Preset | Transition | Priority |
|---|---|---:|---:|---:|
| `FastVS_HD2D_MapSunAnchor_Interior_Morning` | `Current_HouseInteriorMap_SeparateSpace` | Morning | False | 0 |
| `FastVS_HD2D_MapSunAnchor_Exterior_Morning` | `Current_HouseExteriorMap_SeparateSpace` | Morning | True | 0 |
| `FastVS_HD2D_MapSunAnchor_CentralPlaza_Morning` | `Current_CentralPlazaMap_SeparateSpace` | Morning | True | 0 |
| `FastVS_HD2D_MapSunAnchor_Library_Morning` | `Current_LibraryMap_SeparateSpace` | Morning | True | 0 |

| Broad Sunshaft Receiver | Parent | Landmark Id | Local Position | Local Scale |
|---|---|---|---|---|
| `FastVS_HD2D_Cycle180_CentralPlaza_BroadSunshaftReceiver_WestSpan` | `Current_CentralPlazaMap_SeparateSpace` | `Current.central_plaza.story_sunshaft.receiver.west_span` | `(-4.22, 0.04, 4.02)` | `(1.76, 0.10, 0.80)` |
| `FastVS_HD2D_Cycle180_CentralPlaza_BroadSunshaftReceiver_CenterSpan` | `Current_CentralPlazaMap_SeparateSpace` | `Current.central_plaza.story_sunshaft.receiver.center_span` | `(0.00, 0.04, 3.58)` | `(1.76, 0.10, 0.80)` |
| `FastVS_HD2D_Cycle180_CentralPlaza_BroadSunshaftReceiver_EastSpan` | `Current_CentralPlazaMap_SeparateSpace` | `Current.central_plaza.story_sunshaft.receiver.east_span` | `(4.24, 0.04, 4.06)` | `(1.76, 0.10, 0.80)` |

| Dynamic Sunshaft Field | Parent | Renderer Count | Alpha / Motion |
|---|---|---:|---|
| `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaftField` | `Current_CentralPlazaMap_SeparateSpace` | 5 | base 0.24, view 0.16, pulse 0.38, parallax 0.16 |

| Dynamic Sunshaft Renderer | Parent | Material | Local Position | Local Scale |
|---|---|---|---|---|
| `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaft_WestWide` | `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaftField` | `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.mat` | `(-5.60, 2.35, 2.30)` | `(3.30, 5.20, 1.00)` |
| `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaft_WestCenter` | `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaftField` | `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.mat` | `(-2.70, 2.20, 3.32)` | `(3.70, 5.00, 1.00)` |
| `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaft_Center` | `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaftField` | `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.mat` | `(0.10, 2.10, 3.95)` | `(4.20, 5.30, 1.00)` |
| `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaft_EastCenter` | `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaftField` | `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.mat` | `(2.96, 2.18, 3.20)` | `(3.55, 4.90, 1.00)` |
| `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaft_EastWide` | `FastVS_HD2D_Cycle180_CentralPlaza_DynamicSunShaftField` | `Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.mat` | `(5.70, 2.28, 2.18)` | `(3.20, 5.10, 1.00)` |

| Screenshot |
|---|
| `01_current_house_interior_story_morning.png` |
| `02_current_house_exterior_story_morning.png` |
| `03_current_central_plaza_story_west_broad_sunshaft.png` |
| `04_current_central_plaza_story_east_broad_sunshaft.png` |
| `05_current_library_story_morning_exit.png` |

- Plaza-wide evidence spans west, center, and east receiver markers instead of relying on the library entrance alone.
