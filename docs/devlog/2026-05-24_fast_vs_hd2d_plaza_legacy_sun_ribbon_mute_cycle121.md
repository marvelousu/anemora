# feat(hd2d): mute plaza legacy sun ribbons

## Scope

Cycle 121 removes a major blocker for reference-level shadow quality: the older horizontal and diagonal sun-ribbon stack from cycles 104, 106, 109, 111, 112, 113, 116, 118, and 119 was still visible under the Cycle120 light-column pass. This cycle keeps those legacy objects for historical validation, but reduces their material alpha so the current plaza is driven by the newer vertical light column, darker occlusion masses, and restrained air depth.

After the first capture, the foreground was still dominated by white diagonal bands. The cycle therefore suppresses the remaining Cycle119 sun fleck / air veil pass and narrows the Cycle120 light-column texture so its vertical shaft reads harder while ground catch and foreground dust stop forming broad ribbons. A final high-queue dark kill pass is added over the foreground/player/library approach lanes so any legacy ribbon that still leaks through is crushed into shadow. The shared warm/cool/outdoor light-pool materials are also muted because the remaining bright line was coming from that older shared light layer, not from the new Cycle119/120 plaza overlays.

After the ribbon source was removed, the plaza became too flat and dark versus the reference images. The final pass adds a Cycle121 floor-only sun texture with no diagonal shaft component, then renders it after the dark shadow suppressor and before the line-kill strips. This restores a broad warm cobble highlight without bringing back the thin white artifacts.

Authored files:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\INDEX.md`

Expected regenerated materials:

- `FastVS_House_hd2d_plaza_clean_floor_sun_cycle121.mat`
- `FastVS_House_hd2d_plaza_legacy_line_kill_cycle121.mat`
- `FastVS_House_hd2d_plaza_sunbreak_cycle104.mat`
- `FastVS_House_hd2d_plaza_sun_slash_cycle106.mat`
- `FastVS_House_hd2d_plaza_sunlit_islands_cycle109.mat`
- `FastVS_House_hd2d_plaza_broad_sunfield_cycle111.mat`
- `FastVS_House_hd2d_plaza_sun_exposure_base_cycle112.mat`
- `FastVS_House_hd2d_plaza_sunbeam_shafts_cycle113.mat`
- `FastVS_House_hd2d_plaza_solar_reset_sun_cycle116.mat`
- `FastVS_House_hd2d_plaza_solar_reset_air_cycle116.mat`
- `FastVS_House_hd2d_plaza_sun_breakthrough_cycle118.mat`
- `FastVS_House_hd2d_plaza_reference_sun_fleck_cycle119.mat`
- `FastVS_House_hd2d_plaza_reference_air_veil_cycle119.mat`
- `FastVS_House_hd2d_plaza_reference_light_column_sun_cycle120.asset`
- `FastVS_House_hd2d_plaza_reference_light_column_air_cycle120.asset`
- `FastVS_House_hd2d_warm_light_pool.mat`
- `FastVS_House_hd2d_cool_light_pool.mat`
- `FastVS_House_hd2d_outdoor_warm_stage_light.mat`

Expected regenerated textures:

- `FastVS_House_hd2d_plaza_clean_floor_sun_cycle121.asset`
- `FastVS_House_hd2d_plaza_legacy_line_kill_cycle121.asset`
- `FastVS_House_current_path_hd2d_plate.asset`
- `FastVS_House_hd2d_plaza_reference_light_column_sun_cycle120.asset`
- `FastVS_House_hd2d_plaza_reference_light_column_air_cycle120.asset`
- `FastVS_House_hd2d_warm_light_pool_soft.asset`
- `FastVS_House_hd2d_cool_light_pool_soft.asset`
- `FastVS_House_hd2d_outdoor_warm_stage_light_soft.asset`
- `FastVS_House_hd2d_library_window_light_cookie_soft.asset`

Out of scope:

- Main branch, route logic, story/UI behavior, map geometry redesign, and unrelated Unity ProjectSettings churn.

## Goal Prompt

Continue toward reference-image shadow quality with speed prioritized. Cycle120 created the correct direction, but old sun/air ribbons still wash across the foreground, so mute the legacy sources directly.

## Implementation Plan

- Keep legacy overlay geometry and texture contracts intact so earlier validations still protect object placement and asset ownership.
- Lower legacy LightPool and Atmosphere material alpha aggressively.
- Suppress the remaining Cycle119 fleck/veil materials so they no longer read as white bands.
- Narrow the Cycle120 sun and air textures around the vertical shaft while reducing ground catch and foreground dust.
- Add Cycle121 hard-kill shadow strips above the legacy ribbon lanes using the high-queue Cycle120 shadow material.
- Add a separate line-kill material/texture for the final thin clamps so they do not depend on old light-pool assets.
- Add a separate clean floor-sun material/texture that creates broad warm cobble light without any narrow diagonal shaft.
- Render the clean floor-sun material after shadow suppression, while keeping thin line-kill strips above it.
- Mute the shared warm/cool/outdoor light-pool material alpha so pre-plaza light streaks cannot dominate the current frame.
- Keep the Cycle120 vertical light column and high-queue dark shadow suppressor as the dominant visual source.
- Add Cycle121 validation that fails if the legacy sun-ribbon material alphas rise again or if the new clean floor-sun/line-kill overlays lose their expected material/texture/profile contracts.
- Capture current overview/close plus past/library guard frames.

## Expected Evidence

Parent-review screenshots are expected under:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle121_plaza_legacy_sun_ribbon_mute_parent_review_20260524_01`

Expected files:

- `parent_review_01_current_central_plaza_legacy_sun_ribbon_mute_overview.png`
- `parent_review_02_current_central_plaza_legacy_sun_ribbon_mute_close.png`
- `parent_review_03_past_central_plaza_legacy_sun_ribbon_mute_guard.png`
- `parent_review_04_current_library_legacy_sun_ribbon_mute_guard.png`

## Validation

Planned cycle-runner command:

```powershell
& .\tools\cycle-runner.ps1 -CycleNumber 121 `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidatePlazaLegacySunRibbonMuteCycle121Batch' `
  -CaptureMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CapturePlazaLegacySunRibbonMuteCycle121ScreenshotsBatch' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath 'docs/devlog/2026-05-24_fast_vs_hd2d_plaza_legacy_sun_ribbon_mute_cycle121.md' `
  -Audience parent_review `
  -NoRollback
```

## Visual Gate

Passing criteria:

- Foreground should no longer be dominated by old broad white/gold ribbons.
- The main read should be dark plaza shadow plus a narrower vertical light column and broad warm cobble light.
- Current close frame should retain geometry and route readability.
- No thin white diagonal line should cross the overview or close frame.
- Past plaza and library guard captures remain usable.

## Cycle 121 failure (build) -- 20260524-201856

```

Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0030752 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=3acc99838afc51a4380eceb53a18028a): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/External/OpenGameArt/alejandrohaibi_bookshelf_cc0/bookshelf_2.png using Guid(7ac3b7f8dfc441c58c04a5a0a64554e1) (TextureImporter) -> (artifact id: '1b156b30a510e1e811b502056554b699') in 0.0028132 seconds
Refreshing native plugins compatible for Editor in 0.69 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=4c65cfcae943e984f8cba57f6bef4ca6): Total: 0.024 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Fast VS house exterior eave shadow softening validation passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateFastVsHd2dShadowFoundationCycle87HouseExteriorEaveShadowSoftening () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:44260)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:442)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:607)

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 44260)

Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_outdoor_void_background.mat using Guid(57539333774454a4b9b68a2662c07ad5) (NativeFormatImporter) -> (artifact id: '99c018d2123986061894ad403d77f6fe') in 0.0027167 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle.mat using Guid(8a4d3fcd3c585a645b002c3231436022) (NativeFormatImporter) -> (artifact id: '063a97a62c81e77ab8aeb74ad882f999') in 0.0021158 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_outdoor_void_background.mat using Guid(0ec66c653876c7244a13d4f32da1526a) (NativeFormatImporter) -> (artifact id: 'e1db2a1427d1c97a066681d67848d63a') in 0.0019771 seconds
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: '84254ef753b242d39fea27ca378b0b03') in 0.0020808 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_atmosphere_particle_soft.asset using Guid(0273fe2ad4aa90249a716904fc5ff87a) (NativeFormatImporter) -> (artifact id: '29e9a8b9b8606953b5e203e54f1d3bdf') in 0.0020038 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(5aa393eea6e946245b2e701795e51af0) (NativeFormatImporter) -> (artifact id: '184b95cb35aec2eba012f0ac3b8c8e5a') in 0.0023796 seconds
Refreshing native plugins compatible for Editor in 0.50 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=7d588c66903320e4ca92b1b6ab5df958): Total: 0.058 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_current_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(efe4ace4ebe298d4bada9fe5c2546e89) (NativeFormatImporter) -> (artifact id: 'a69f20e4e842686af22fc83b45a76019') in 0.0026658 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=3e13d62d4f1f5014d861fae9ad176662): Total: 0.040 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '2dfb0ee97db515b87db3e427f999c2a7') in 0.0025327 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.asset using Guid(b43c1d26d8af7144eac532f733d613ed) (NativeFormatImporter) -> (artifact id: '72b93ea802d61590522168319e686932') in 0.0023365 seconds
Refreshing native plugins compatible for Editor in 0.54 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e47d356546c35ca4d9f157dfe925cef8): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_past_house_exterior_scenic_backdrop_sky_curtain.mat using Guid(0a33e84f626797a40b8d5687454e6620) (NativeFormatImporter) -> (artifact id: '1811c45b24cb5a42b0d119f903087e3b') in 0.0026427 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=4d07f1f94a07ec646bab5530cbce2724): Total: 0.038 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: 'dec47bc9f235a292b061ba7705817a0e') in 0.0024454 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.asset using Guid(6b757f77ea5d7164e8537193c67506bd) (NativeFormatImporter) -> (artifact id: 'edb251d8a3d8ffd6a09591c3a454c8a2') in 0.0024443 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=9b873df8fef3ac64f8f7628cbf2d376e): Total: 0.043 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_current_house_exterior.mat using Guid(ba3c455e34bfdef488b2dbb1bdd0cd70) (NativeFormatImporter) -> (artifact id: '1a5dfe9209479e4411a9a5728bc36da8') in 0.0026049 seconds
Refreshing native plugins compatible for Editor in 0.59 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=270c98bf25f4d224fa8232473af88040): Total: 0.039 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: '3e5b99ed69b07b0978cb63a29b9cb0d4') in 0.0025066 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.asset using Guid(2bb9710db4a50874c944ff2cece89232) (NativeFormatImporter) -> (artifact id: '91e098c066551ce9c7048c5e7b3cb0d8') in 0.0024796 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=974c940eda8c50b4fa6d7a9faba3f07a): Total: 0.045 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_sky_wash_past_house_exterior.mat using Guid(b09502d957a9b5141836a6b6ab31797f) (NativeFormatImporter) -> (artifact id: 'df71a74559f1cd0ef87c4679dd6814a6') in 0.0025675 seconds
Refreshing native plugins compatible for Editor in 0.52 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b5d9bd6cdb347824f8818de9081a3c70): Total: 0.038 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient.mat using Guid(d58f2dd58f72df641b0b13d70b9aaffb) (NativeFormatImporter) -> (artifact id: 'cce592b71f40c96c4099f5295e653a71') in 0.0024401 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset using Guid(c77c6612505c47d4aa1dac2b81f7897e) (NativeFormatImporter) -> (artifact id: '0a0b3058b133df474de29a6690dee7ed') in 0.0027207 seconds
Refreshing native plugins compatible for Editor in 0.55 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=c207db16c879efe4f8880f2d8cb8ae83): Total: 0.032 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
InvalidOperationException: House slice validation failed: Current_CentralPlaza_LightComposition_LibraryDoorCoolGlowA local scale expected within (0.80, 0.02, 0.08) and (1.10, 0.03, 0.14), but got (0.04, 0.01, 0.04).
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateVectorWithinRange (System.String label, UnityEngine.Vector3 actual, UnityEngine.Vector3 minInclusive, UnityEngine.Vector3 maxInclusive) [0x00054] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:49016 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateNonArrivalLandmarkCubeObject (System.String objectName, System.String expectedParentName, UnityEngine.Vector3 referenceCenter, System.String expectedMaterialToken, Anemora.TimeManagement.TimeWindowPairedSpaceLandmarkKind expectedKind, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, UnityEngine.Vector3 minLocalScale, UnityEngine.Vector3 maxLocalScale) [0x00196] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:44980 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dOneHundredEleventhCycleOutdoorLightCompositionContactGrounding () [0x006a0] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43539 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x003b0] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:506 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:607 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 43539)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[20:21:52] Phase 'build' FAILED with exit 1
[20:21:52] NoRollback set; preserving worktree after build failure
```
