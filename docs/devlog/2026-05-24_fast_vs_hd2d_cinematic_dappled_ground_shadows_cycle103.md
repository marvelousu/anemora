# feat(hd2d): add cinematic dappled ground shadows

## Scope

Cycle 103 continues the Fast VS HD-2D shading foundation after Cycle 102 corrected the grounded-shadow visual evidence. The current user priority is shadow quality, so this cycle stays on shadow composition and does not implement the new map reference yet.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Generated side-effect files expected from batch validation/capture:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_hd2d_dappled_cast_shadow_cycle103.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_dappled_cast_shadow_cycle103.asset`

Documentation files authored by parent:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-24_fast_vs_hd2d_cinematic_dappled_ground_shadows_cycle103.md`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

Out of scope:

- Story, UI, doors, time-window behavior, character assets, desks/shelves, existing map layout, ProjectSettings, and main branch.
- Map reference implementation from `C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1`.

## Goal Prompt

Move the scene away from flat darkening and toward the reference-like HD-2D shadow language: broad directional shadow fields, smaller organic dark pockets, and soft transparent edges that break up the floor without becoming rectangular black boards.

Reference directory:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference`

## Worker Cycle

- Cycle-worker: `019e56ae-c8f6-7b33-9e18-126fa2a05ce7`
- Worker role: `cycle-worker` / `gpt-5.4-mini`

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=103 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCinematicDappledGroundShadowsBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dCinematicDappledGroundShadowsCycle103ScreenshotsBatch`

## Implementation Plan

- Add a deterministic generated `hd2d_dappled_cast_shadow_cycle103` texture and transparent contact-shadow material.
- Place non-colliding horizontal shadow quads on central plaza and library floors in both current/past spaces.
- Validate texture alpha metrics and object placement so the dappled shadows stay soft, horizontal, non-rectangular, and shadow-safe.
- Capture plaza/library overview and ReTo close views for parent visual review.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle103_cinematic_dappled_ground_shadows_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_dappled_shadow_overview.png`
- `parent_review_02_past_central_plaza_dappled_shadow_overview.png`
- `parent_review_03_current_library_dappled_floor_shadow_wide.png`
- `parent_review_04_current_library_reto_dappled_shadow_close.png`

## Validation

Planned cycle-runner command:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 103 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCinematicDappledGroundShadowsBatch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dCinematicDappledGroundShadowsCycle103ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_cinematic_dappled_ground_shadows_cycle103.md' `
  -Audience parent_review `
  -CommitPath @(
    'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_dappled_cast_shadow_cycle103.mat',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_dappled_cast_shadow_cycle103.mat.meta',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_dappled_cast_shadow_cycle103.asset',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_dappled_cast_shadow_cycle103.asset.meta',
    'docs/devlog/2026-05-24_fast_vs_hd2d_cinematic_dappled_ground_shadows_cycle103.md',
    'docs/devlog/INDEX.md',
    'docs/devlog/screenshots/fast_vs_hd2d_cycle103_cinematic_dappled_ground_shadows_parent_review_20260524_01'
  ) `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Shadows read as varied dappled floor shade rather than flat dark rectangles.
- Existing grounded character/prop shadows remain readable.
- Plaza and library floors gain stronger tonal rhythm without hiding navigation cues or story objects.

## Retry Note

The first validate attempt found the generated dappled texture was slightly under the intended opacity contract (`0.278` max alpha against a `0.28` lower bound). Parent adjusted only the texture density coefficients before rerunning the cycle gate.

## Cycle 103 failure (validate) -- 20260524-063255

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_depth_shadow.mat using Guid(07afc9c1f7f8ab4439760cab6c8b9c1e) (NativeFormatImporter) -> (artifact id: '68a4feae9900f9534bb049459ffb3678') in 0.0021247 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat using Guid(373d809c8f785b3439635e59312211de) (NativeFormatImporter) -> (artifact id: '70688f62928dc32fdf23a0c22c596252') in 0.0028702 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0022357 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0021839 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0021295 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0023548 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0020619 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0018086 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0020111 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0019219 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0020729 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0024485 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0021814 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0018852 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0020803 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0022671 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0024273 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0025295 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.002505 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0027505 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0024651 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0024613 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0032124 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.002391 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0026526 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.002167 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0027315 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0024096 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0024914 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0031265 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0031353 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0028452 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0031516 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0032666 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0029411 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0025556 seconds
Refreshing native plugins compatible for Editor in 0.67 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b095eef3f9a672548a3988b10d279847): Total: 0.192 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '7a534cb8d8fda3707e6271fdd350bbf3') in 0.0013683 seconds
Refreshing native plugins compatible for Editor in 0.66 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=bc6dd6d0c9c4d794d838e59dd395ec4c): Total: 0.051 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=5a031217258def444b99295a974ddcdb): Total: 0.027 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:238)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateCinematicDappledGroundShadowsBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:40744)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 238)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            258.628 ms
	Integration:            326.678 ms
	Integration of assets:  2.781 ms
	Thread Wait Time:       -2.550 ms
	Total Operation Time:   585.537 ms
Unloading 553 unused Assets / (6.9 MB). Loaded Objects now: 24025.
Memory consumption went from 257.2 MB to 250.3 MB.
Total: 12.049200 ms (FindLiveObjects: 1.861800 ms CreateObjectMapping: 0.391400 ms MarkObjects: 8.050700 ms  DeleteObjects: 1.743900 ms)

InvalidOperationException: House slice validation failed: Current_CentralPlaza_Cycle103_DappledGroundShadow_LibraryApproachA local position expected within (0.12, 0.03, 5.30) and (0.48, 0.06, 5.60), but got (21.10, 0.04, 21.25).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateVectorWithinRange (System.String label, UnityEngine.Vector3 actual, UnityEngine.Vector3 minInclusive, UnityEngine.Vector3 maxInclusive) [0x00054] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43802 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCycle103DappledGroundShadowObject (System.String objectName, System.String expectedParentName, Anemora.FastVS.FastVsHouseArea expectedArea, System.Boolean currentWorld, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, UnityEngine.Vector3 minLocalScale, UnityEngine.Vector3 maxLocalScale, System.Single expectedYawDegrees) [0x0012c] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31656 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dCycle103CinematicDappledGroundShadows () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31751 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCinematicDappledGroundShadowsBatch () [0x00011] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:40746 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 31751)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCinematicDappledGroundShadowsBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[06:42:12] Phase 'validate' FAILED with exit 1
[06:42:12] NoRollback set; preserving worktree after validate failure
```
