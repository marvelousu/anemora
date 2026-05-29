# feat(hd2d): exaggerate grounded shadows

## Scope

Cycle 101 continues the Fast VS HD-2D shading foundation work after Cycle 100 unified the sun direction. This cycle intentionally avoids map layout, house facade repairs, dialogue, character assets, sky, sunbeam, and lens flare work.

Authored file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Audit sync file:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs`

Generated side-effect files expected from batch validation/capture:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_character_contact_shadow.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_hd2d_character_ground_bounce_soft.asset`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_character_directional_cast_shadow_soft.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_static_directional_cast_shadow_soft.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_character_directional_cast_shadow.mat`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_static_directional_cast_shadow.mat`

## Goal Prompt

Use the reference images under `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference` as the visual target for stronger HD-2D contact and cast shadows. The desired result is not simply a darker image; characters, desks, shelves, and large facades should feel attached to the ground through shaped, directional shadow masses.

## Worker Cycle

- Cycle-worker `019e567e-2893-7130-a11d-85e99816335e` implemented the scoped prompt.
- Parent review kept the change inside the requested shadow foundation scope and added missing close-capture output assertions.

Scoped prompt trace:

`SCOPED_PROMPT_ISSUED cycle=101 authored_file=C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateExaggeratedGroundedShadowsBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dExaggeratedGroundedShadowsCycle101ScreenshotsBatch`

## Implementation

- Increased character directional cast-shadow footprint sizes.
- Increased static directional cast-shadow footprint sizes for house exterior, central plaza/library facade, and library back shelf.
- Strengthened character contact shadow generation with denser foot cores and a softer body/tail.
- Strengthened character directional cast-shadow PNG generation with a darker near-body core and longer soft tail.
- Strengthened static directional cast-shadow PNG generation while preserving transparent edges and corners.
- Reduced the warm character ground bounce so it supports the darker shadows instead of reading as a yellow disk.
- Synchronized HD-2D overlay profile audit footprint expectations with the stronger shadow scales.
- Added `ValidateExaggeratedGroundedShadowsBatch`.
- Added `CaptureHd2dExaggeratedGroundedShadowsCycle101ScreenshotsBatch`.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle101_exaggerated_grounded_shadows_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_niro_grounding_overview.png`
- `parent_review_02_current_library_reto_desk_grounding_close.png`
- `parent_review_03_past_central_plaza_grounding_overview.png`
- `parent_review_04_current_house_exterior_player_grounding_close.png`

## Validation

Planned cycle-runner command:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 101 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateExaggeratedGroundedShadowsBatch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dExaggeratedGroundedShadowsCycle101ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_exaggerated_grounded_shadows_cycle101.md' `
  -Audience parent_review `
  -CommitPath @(
    'Assets/Editor/AnemoraFastVsHouseSliceSetup.cs',
    'Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset.meta',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset.meta',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_directional_cast_shadow_soft.png',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_directional_cast_shadow_soft.png.meta',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_static_directional_cast_shadow_soft.png',
    'Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_static_directional_cast_shadow_soft.png.meta',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_character_directional_cast_shadow.mat',
    'Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_static_directional_cast_shadow.mat',
    'docs/devlog/2026-05-24_fast_vs_hd2d_exaggerated_grounded_shadows_cycle101.md',
    'docs/devlog/INDEX.md',
    'docs/devlog/screenshots/fast_vs_hd2d_cycle101_exaggerated_grounded_shadows_parent_review_20260524_01'
  ) `
  -NoRollback
```

## Visual Gate

The pass bar is that the screenshots show visibly stronger but still shaped contact/cast shadows. If this passes, the next theme is sunbeam/lens flare; if it fails, continue tuning shadow textures and overlay placement before moving on.

## Cycle 101 failure (validate) -- 20260524-052626

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat using Guid(373d809c8f785b3439635e59312211de) (NativeFormatImporter) -> (artifact id: '70688f62928dc32fdf23a0c22c596252') in 0.0025032 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0024769 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0031009 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0027171 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0024252 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0022183 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0022467 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0022246 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0020222 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0020407 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0021592 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0025566 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0023085 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.002253 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '70c3e11a7099b4abd4bf5b645715ca67') in 0.002098 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.002692 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0028468 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0025546 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0027923 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0026501 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0022593 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0030491 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0026322 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0030265 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0022596 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0024866 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0026942 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0031318 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0026968 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0026335 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0021289 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0023369 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0031529 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0030672 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0027687 seconds
Refreshing native plugins compatible for Editor in 0.60 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=612791af769f2fd408603cef16bd4c84): Total: 0.205 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: 'b1a0829c93fa270f76322c0991c8b394') in 0.0012343 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=1269786d36ec3da419e629e68ec817ba): Total: 0.042 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=4210c9fd5f31e0941b645673cf081e2e): Total: 0.027 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:232)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateExaggeratedGroundedShadowsBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:40282)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 232)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
[Licensing::Client] Successfully resolved entitlement details
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            261.011 ms
	Integration:            289.110 ms
	Integration of assets:  2.198 ms
	Thread Wait Time:       -2.175 ms
	Total Operation Time:   550.144 ms
Unloading 551 unused Assets / (5.5 MB). Loaded Objects now: 23975.
Memory consumption went from 236.9 MB to 231.4 MB.
Total: 12.323000 ms (FindLiveObjects: 1.964300 ms CreateObjectMapping: 0.419200 ms MarkObjects: 8.194200 ms  DeleteObjects: 1.744200 ms)

InvalidOperationException: House slice validation failed: seventeenth-cycle contact-shadow pass max alpha 0.451 is outside the 0.48-0.54 range.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCharacterContactShadowTextureMetrics (UnityEngine.Texture2D texture, System.String context) [0x002f6] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31307 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCharacterContactShadowTextureMetrics (System.String context) [0x00047] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31236 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dSeventeenthCycleCharacterContactShadows () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31038 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateExaggeratedGroundedShadowsBatch () [0x00011] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:40284 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 31236)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateExaggeratedGroundedShadowsBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[05:33:23] Phase 'validate' FAILED with exit 1
[05:33:23] NoRollback set; preserving worktree after validate failure
```

## Cycle 101 failure (validate) -- 20260524-053413

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite.mat using Guid(56b5ac4afa850e94fa26bf722f31b14b) (NativeFormatImporter) -> (artifact id: 'f7e017ad6bce709be21936a4bb8efe19') in 0.0027759 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_depth_shadow.mat using Guid(07afc9c1f7f8ab4439760cab6c8b9c1e) (NativeFormatImporter) -> (artifact id: '68a4feae9900f9534bb049459ffb3678') in 0.0028085 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat using Guid(373d809c8f785b3439635e59312211de) (NativeFormatImporter) -> (artifact id: '70688f62928dc32fdf23a0c22c596252') in 0.0038916 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0032381 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0031006 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0030959 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0032663 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.003013 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0027912 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0025781 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0028653 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0031484 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0031774 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0029913 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.003082 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0037294 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0031379 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0032248 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0030814 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0031725 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0046744 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0027577 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0026775 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0039657 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0029851 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0034971 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0043183 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0045262 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0072264 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0052148 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.003432 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0033132 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0038189 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0048965 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0034315 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0049494 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0035892 seconds
Refreshing native plugins compatible for Editor in 0.83 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=cb5d9f5ebe997f3459645c348e2d9266): Total: 0.269 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '23232239bd4ce215f36503fe83cf308e') in 0.002174 seconds
Refreshing native plugins compatible for Editor in 0.97 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=fcb502e35a4751847b4e261b087d3856): Total: 0.075 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=4f934435b4bddb843b86bde11b517800): Total: 0.035 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:232)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateExaggeratedGroundedShadowsBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:40282)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 232)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            322.923 ms
	Integration:            349.697 ms
	Integration of assets:  2.237 ms
	Thread Wait Time:       -2.160 ms
	Total Operation Time:   672.697 ms
Unloading 551 unused Assets / (7.4 MB). Loaded Objects now: 23975.
Memory consumption went from 257.6 MB to 250.2 MB.
Total: 13.685000 ms (FindLiveObjects: 2.090300 ms CreateObjectMapping: 0.523300 ms MarkObjects: 9.065700 ms  DeleteObjects: 2.004200 ms)

InvalidOperationException: House slice validation failed: character_directional_cast_shadow_soft max alpha must stay in the 0.36-0.44 range, but was 0.333.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateCharacterDirectionalCastShadowTexture () [0x001c8] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31410 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dCharacterDirectionalCastShadows () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31053 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateExaggeratedGroundedShadowsBatch () [0x00016] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:40285 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 31410)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateExaggeratedGroundedShadowsBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[05:35:22] Phase 'validate' FAILED with exit 1
[05:35:22] NoRollback set; preserving worktree after validate failure
```

## Cycle 101 failure (validate) -- 20260524-053606

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_depth_shadow.mat using Guid(07afc9c1f7f8ab4439760cab6c8b9c1e) (NativeFormatImporter) -> (artifact id: '68a4feae9900f9534bb049459ffb3678') in 0.001812 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat using Guid(373d809c8f785b3439635e59312211de) (NativeFormatImporter) -> (artifact id: '70688f62928dc32fdf23a0c22c596252') in 0.0016901 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.001773 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0019396 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.001967 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0018733 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0019598 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0019312 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0019704 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0018069 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0017031 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0017207 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0021489 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0033552 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0023979 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.0024815 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.0023932 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0027231 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0022818 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0020861 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.002037 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.0018639 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0023004 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.002141 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0024753 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0020904 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0020976 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.0021643 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0021608 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.0021692 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0027277 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0027001 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0027595 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0026027 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0023479 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0024865 seconds
Refreshing native plugins compatible for Editor in 0.56 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=c0a596e4d8eb61e41b7f642e48baea8c): Total: 0.169 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: 'eed134782d59ab368856fefb4e85b397') in 0.0012986 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=44154bebe71e80a4f857fa575be7596f): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=444f4006f37e8fc41aaf0bcd8887efc3): Total: 0.023 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:232)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateExaggeratedGroundedShadowsBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:40282)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 232)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            229.920 ms
	Integration:            284.630 ms
	Integration of assets:  2.280 ms
	Thread Wait Time:       -2.265 ms
	Total Operation Time:   514.566 ms
Unloading 551 unused Assets / (7.0 MB). Loaded Objects now: 23975.
Memory consumption went from 257.3 MB to 250.3 MB.
Total: 12.250600 ms (FindLiveObjects: 1.905600 ms CreateObjectMapping: 0.414600 ms MarkObjects: 8.290200 ms  DeleteObjects: 1.638400 ms)

InvalidOperationException: House slice validation failed: static_directional_cast_shadow_soft center alpha must stay in the 0.22-0.34 range, but was 0.216.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateStaticDirectionalCastShadowTextureMetrics (UnityEngine.Texture2D validationTexture, System.String context) [0x00156] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31657 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateStaticDirectionalCastShadowTexture () [0x000c3] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31487 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dStaticDirectionalCastShadows () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31494 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateExaggeratedGroundedShadowsBatch () [0x0001b] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:40286 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 31487)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateExaggeratedGroundedShadowsBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[05:44:04] Phase 'validate' FAILED with exit 1
[05:44:04] NoRollback set; preserving worktree after validate failure
```

## Cycle 101 failure (validate) -- 20260524-054442

```
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_map_move_floor_glow.mat using Guid(373d809c8f785b3439635e59312211de) (NativeFormatImporter) -> (artifact id: '70688f62928dc32fdf23a0c22c596252') in 0.0031377 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite.mat using Guid(57d1354643503484f8f9504dec7939e7) (NativeFormatImporter) -> (artifact id: '8d6144afb428b20ddb5b28b485258685') in 0.0038893 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite.mat using Guid(f79bd52245f424d42b668da63a26eed7) (NativeFormatImporter) -> (artifact id: '6c7634a35d7a5e593d016924efb11022') in 0.0041758 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite.mat using Guid(f71c1a4bbbfa65e4a9dc8ceb1dbaab05) (NativeFormatImporter) -> (artifact id: '067c3ca3644ebbf71c59456785c01e71') in 0.0031515 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite.mat using Guid(28f2cae61d95125439828020176a4cfc) (NativeFormatImporter) -> (artifact id: '72016ad2a7c404cc799f9f50c6febaf5') in 0.0035174 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite.mat using Guid(a8c94ff07ef65ea4f8ad0dedea407b1e) (NativeFormatImporter) -> (artifact id: '63c8344cf78a915d285cd3eae78cb1fd') in 0.0032262 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite.mat using Guid(b8841e13281e616468837ca346676037) (NativeFormatImporter) -> (artifact id: '1adcf1ea1109f3defbf059ddda5acb22') in 0.0033742 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite.mat using Guid(3acdb3d0d51b28d40854136863dba5b3) (NativeFormatImporter) -> (artifact id: '085d72f1844e837c1fa998fa9467b7c2') in 0.0033215 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0026445 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce.mat using Guid(0bc31eecccdf1a840a5477d771512775) (NativeFormatImporter) -> (artifact id: '957df3d5816f00a64823db316222cbe2') in 0.0029721 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool.mat using Guid(0dd9370c56289ae4a8d0e1dddfc59477) (NativeFormatImporter) -> (artifact id: 'dff4420f29bfcf0b5e37a561766cc28b') in 0.0024912 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_route_move_floor_glow.mat using Guid(0dbdea9defe689a4ca05680804b173ca) (NativeFormatImporter) -> (artifact id: '76abd1cd4f418349642ae7527f51d632') in 0.0027186 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_niro_back_sprite.mat using Guid(8d4799b24d7badb4d9d50d8f3546587c) (NativeFormatImporter) -> (artifact id: 'ea1cc68dacc196e87140893dcea1fad6') in 0.0027173 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.mat using Guid(0e535f48a85f05b43ad8f1b5cb2ecb4a) (NativeFormatImporter) -> (artifact id: '95a63720d5377afca26fb765e42e17fd') in 0.0028758 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_character_contact_shadow.asset using Guid(40c9d8116f275fd4c98bbdde1e793a32) (NativeFormatImporter) -> (artifact id: '17d86434566b3eaf148ab06195b0eaff') in 0.003812 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_back_sprite_shaded.asset using Guid(11367ee44066a2c4a933e1a65a54fa71) (NativeFormatImporter) -> (artifact id: 'b4ea198999a098bff466fc482aa2e085') in 0.005065 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_right_sprite_shaded.asset using Guid(31848d2268cea5d4a93190a2365133ca) (NativeFormatImporter) -> (artifact id: 'dfcf40487733ba25c6e6bad9c26fa7e7') in 0.0040151 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_library_window_light_cookie_soft.asset using Guid(e10a87518f970b6498f120403b5e87fa) (NativeFormatImporter) -> (artifact id: 'b6624ac7fb17375ff687066ce4c5208b') in 0.0041602 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_left_sprite_shaded.asset using Guid(f13736bef9726b045ba18071b949e2d2) (NativeFormatImporter) -> (artifact id: 'f6e48f8240644c12833b02b6b33897d1') in 0.0037444 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0033942 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_character_ground_bounce_soft.asset using Guid(0442e2f80b7dbc34e99a112ae1095f20) (NativeFormatImporter) -> (artifact id: 'a65e2dbc19e50b0fc0fa4598f134ffd8') in 0.002988 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_back_sprite_shaded.asset using Guid(246d9d1ed78f72147aa1f537daf9164e) (NativeFormatImporter) -> (artifact id: '59a4f02825ed43e073fd3814395235f5') in 0.0038438 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_timewriter_pocket_yellow_glow.asset using Guid(64767a48f06c7a4439175ba9f3f76969) (NativeFormatImporter) -> (artifact id: '2c34ef17951f1b28aef444ba8ef4c761') in 0.0035725 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_back_sprite_shaded.asset using Guid(646afdaa35aa1c84bbfca6d10ad8d956) (NativeFormatImporter) -> (artifact id: '71d57c8ff1c974a8f2d165c436f15504') in 0.0036265 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_warm_light_pool_soft.asset using Guid(97afe9889f5e56f449c8468a1d0bfdf9) (NativeFormatImporter) -> (artifact id: 'b007a80dd3775e821df40936b5d7e159') in 0.0026407 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_back_sprite_shaded.asset using Guid(383cf440301bf504a9b6c2a8a85c5c10) (NativeFormatImporter) -> (artifact id: '9a14722ac27a7c483b3d93443ba97166') in 0.0028486 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_left_sprite_shaded.asset using Guid(e8b91a1d7f94f9d419ae3d2cb8aca69c) (NativeFormatImporter) -> (artifact id: '19f48d248392446507eb0aa3bd573d8d') in 0.003885 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_left_sprite_shaded.asset using Guid(29c76ef0ee79e294abc159d852e30d6f) (NativeFormatImporter) -> (artifact id: '7a1e2d14c3f7f57972b5a415bb755af8') in 0.0032961 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_left_sprite_shaded.asset using Guid(8af717035fe354841a2778b67ef47f60) (NativeFormatImporter) -> (artifact id: '636319e0046c8cb6ef7b2f1e0d8a2fbf') in 0.004265 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_right_sprite_shaded.asset using Guid(bcf85f4b6024bd345a6725cd7e3f8122) (NativeFormatImporter) -> (artifact id: '1134ffa9b5a09269a7a99f311dd08502') in 0.0036888 seconds
Start importing Assets/Settings/DefaultVolumeProfile.asset using Guid(2dc156ea366288a46859f93b6d3c2cd0) (NativeFormatImporter) -> (artifact id: 'b03cefda0696e6248b213f7dfd6e20a5') in 0.0030682 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_right_sprite_shaded.asset using Guid(7edaa2e8ce6f3f040896d2d92f57e792) (NativeFormatImporter) -> (artifact id: '4e696edc4775714bf55149c9029e9a87') in 0.0039786 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_walk_front_sprite_shaded.asset using Guid(8e8558fedae4d5441a0880d3e0ae5dde) (NativeFormatImporter) -> (artifact id: '7a36765b1626f751d5a96e3827f13ea3') in 0.0031914 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_past_right_sprite_shaded.asset using Guid(afafc97e4daeeb2408f5830dde7c5f7c) (NativeFormatImporter) -> (artifact id: 'abb16328271b3f261d7f87df395bbbcc') in 0.0032041 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_niro_walk_front_sprite_shaded.asset using Guid(cfe6c4e4efe6c2d4bae69a5a1cb896ad) (NativeFormatImporter) -> (artifact id: 'a1fc470689ab7b251db5843fc102d4a4') in 0.0038335 seconds
Refreshing native plugins compatible for Editor in 0.77 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=eda9d6b7ecd4a9348a94961dc2043128): Total: 0.241 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Scenes/Anemora_FastVS_HouseSlice.unity using Guid(5c2c1333e7d65ba4eac9369f107a6118) (DefaultImporter) -> (artifact id: '0e529fb75b0bfaf630f72c07c688f76b') in 0.0016514 seconds
Refreshing native plugins compatible for Editor in 0.66 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=3561eeb836c3ed848ba905b0b9dbf643): Total: 0.058 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Asset Pipeline Refresh (id=30e2b19bec9a8c8439fd3686515fd126): Total: 0.033 seconds - Initiated by RefreshV2(NoUpdateAssetOptions)
Fast VS house slice scene created: Assets/Scenes/Anemora_FastVS_HouseSlice.unity
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:232)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateExaggeratedGroundedShadowsBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:40282)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 232)

Opening scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
[Licensing::Client] Successfully resolved entitlement details
Unloading 0 Unused Serialized files (Serialized files now loaded: 0)
Loaded scene 'Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
	Deserialize:            317.568 ms
	Integration:            374.247 ms
	Integration of assets:  3.201 ms
	Thread Wait Time:       -3.124 ms
	Total Operation Time:   691.892 ms
Unloading 551 unused Assets / (11.4 MB). Loaded Objects now: 23975.
Memory consumption went from 253.2 MB to 241.9 MB.
Total: 14.319500 ms (FindLiveObjects: 2.109700 ms CreateObjectMapping: 0.459500 ms MarkObjects: 9.544900 ms  DeleteObjects: 2.204200 ms)

InvalidOperationException: House slice validation failed: static_directional_cast_shadow_soft core alpha at (64,42) must stay in the 0.22-0.34 range, but was 0.396.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateStaticDirectionalCastShadowTextureMetrics (UnityEngine.Texture2D validationTexture, System.String context) [0x00180] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31662 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateStaticDirectionalCastShadowTexture () [0x000c3] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31487 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dStaticDirectionalCastShadows () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:31494 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateExaggeratedGroundedShadowsBatch () [0x0001b] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:40286 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 31487)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateExaggeratedGroundedShadowsBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[05:45:54] Phase 'validate' FAILED with exit 1
[05:45:54] NoRollback set; preserving worktree after validate failure
```

## Cycle 101 failure (build) -- 20260524-054635

```
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.005136 seconds
Refreshing native plugins compatible for Editor in 0.58 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=dcd5ea5243bb0ad42b23f191f5fa2643): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0038481 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=cfa724908e386514f8ea65f5c8cfdb54): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:34)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:262)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:519)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 34)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:98)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:263)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:519)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 98)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:38)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:264)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:519)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 38)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:142)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:269)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:519)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 142)

InvalidOperationException: HD2D overlay profile audit failed:
- HD2D overlay profile FastVS_PlayerDirectionalCastShadow_Niro must keep footprintWorldSize near (0.78, 0.20), but was (0.88, 0.24).
- HD2D overlay profile Current_Library_Reto_DirectionalCastShadow must keep footprintWorldSize near (0.66, 0.18), but was (0.76, 0.22).
- HD2D overlay profile Past_Library_Aria_DirectionalCastShadow must keep footprintWorldSize near (0.66, 0.18), but was (0.76, 0.22).
- HD2D overlay profile Current_HouseExterior_StaticDirectionalCastShadow_HouseFacade must keep footprintWorldSize near (2.16, 0.20), but was (2.55, 0.25).
- HD2D overlay profile Past_HouseExterior_StaticDirectionalCastShadow_HouseFacade must keep footprintWorldSize near (2.16, 0.20), but was (2.55, 0.25).
- HD2D overlay profile Current_CentralPlaza_StaticDirectionalCastShadow_LibraryFacade must keep footprintWorldSize near (3.12, 0.18), but was (3.58, 0.23).
- HD2D overlay profile Past_CentralPlaza_StaticDirectionalCastShadow_LibraryFacade must keep footprintWorldSize near (3.12, 0.18), but was (3.58, 0.23).
- HD2D overlay profile Current_Library_StaticDirectionalCastShadow_BackShelf must keep footprintWorldSize near (5.18, 0.18), but was (5.70, 0.24).
- HD2D overlay profile Past_Library_StaticDirectionalCastShadow_BackShelf must keep footprintWorldSize near (5.18, 0.18), but was (5.70, 0.24).
  at Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit.VerifyOverlayProfilesV1 () [0x007b1] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:403 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000c7] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:270 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:519 

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 403)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[05:48:53] Phase 'build' FAILED with exit 1
[05:48:53] NoRollback set; preserving worktree after build failure
```
