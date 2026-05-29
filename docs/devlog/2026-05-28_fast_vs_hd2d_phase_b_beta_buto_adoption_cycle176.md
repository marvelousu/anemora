# feat(hd2d): add phase b beta buto adoption fallback

Cycle 176 records the Phase B-beta Buto adoption state without importing paid asset files. Local scans before implementation found no Buto import in `Assets`, `Library/PackageCache`, Unity Asset Store cache, Downloads, or Documents, so the expected state for this cycle is B-alpha fallback retained and a diagnostic report for Tom review.

## Scope

- Added B-beta Buto availability/adoption batch methods.
- Added string/reflection-only Buto detection across asset paths, loaded assemblies/types, and the URP renderer asset text.
- Kept B-alpha ScreenSpaceLensFlare and directional sun lens flare fallback validation active.
- Captures the same five review areas as B-alpha: House Interior, House Exterior, Central Plaza, Library, and TimeWindow aperture.

## Validation Plan

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseBBetaButoAdoptionBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseBBetaButoAdoptionCycle176ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player with `-batchmode -nographics`, pattern count must be 0.

## Review Notes

- 変更を適用しました: Buto が import 済みかを batch で診断し、未 import の場合は B-alpha fallback を維持する証跡を残します。
- 参考画像とのギャップ: Buto 本体がこの workspace に存在しないため、B-beta の volumetric god rays 比較は未実施です。現状は B-alpha の flare/fallback 見た目の継続確認です。
- Tom 判定をお願いします。
- Build exe path for Tom: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 起動時は `Builds/FastVS_HouseSlice/` フォルダごと起動。

## Cycle 176 failure (validate) -- 20260528-173658

```
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHd2dPhaseBBetaButoAdoptionBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:1431)

[Assets/Editor/AnemoraFastVsHouseSliceSetup.cs line 426]

Failed to create temporary cache directory 'Temp/ADB-Refreshdb1015c8a1907b9448754839fd05f58e'
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:EnsureHd2dOutdoorOcclusionGradientMaterial () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:69678)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseExteriorGroundShadowBreakupCycle96 (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:11482)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateExterior (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:10369)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseMap (UnityEngine.Transform,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:9210)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:426)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHd2dPhaseBBetaButoAdoptionBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:1431)

[Assets/Editor/AnemoraFastVsHouseSliceSetup.cs line 426]

Failed to create temporary cache directory 'Temp/ADB-Refresh8257f35ee5df5884a9a145bf28d7b7de'
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:EnsureHd2dOutdoorOcclusionGradientMaterial () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:69678)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseExteriorGroundShadowBreakupCycle96 (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:11482)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateExterior (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:10369)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseMap (UnityEngine.Transform,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:9210)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:426)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHd2dPhaseBBetaButoAdoptionBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:1431)

[Assets/Editor/AnemoraFastVsHouseSliceSetup.cs line 426]

Failed to create temporary cache directory 'Temp/ADB-Refreshf706907a6ba640745892b03e78f65efe'
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:EnsureHd2dOutdoorOcclusionGradientMaterial () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:69678)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseExteriorGroundShadowBreakupCycle96 (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:11482)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateExterior (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:10369)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseMap (UnityEngine.Transform,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:9210)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:426)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHd2dPhaseBBetaButoAdoptionBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:1431)

[Assets/Editor/AnemoraFastVsHouseSliceSetup.cs line 426]

Failed to create temporary cache directory 'Temp/ADB-Refresh383afb992dc22f14b894824ed215a08f'
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:EnsureHd2dOutdoorOcclusionGradientMaterial () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:69678)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseExteriorGroundShadowBreakupCycle96 (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:11482)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateExterior (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:10369)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseMap (UnityEngine.Transform,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:9210)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:426)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHd2dPhaseBBetaButoAdoptionBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:1431)

[Assets/Editor/AnemoraFastVsHouseSliceSetup.cs line 426]

Failed to create temporary cache directory 'Temp/ADB-Refresh2c070ef497b6b2549a4d4a834276623a'
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:EnsureHd2dOutdoorOcclusionGradientMaterial () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:69678)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseExteriorGroundShadowBreakupCycle96 (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:11482)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateExterior (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:10369)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseMap (UnityEngine.Transform,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:9210)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:426)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHd2dPhaseBBetaButoAdoptionBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:1431)

[Assets/Editor/AnemoraFastVsHouseSliceSetup.cs line 426]

Failed to create temporary cache directory 'Temp/ADB-Refresh28a22e639a103274f870da689823ff42'
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:EnsureHd2dOutdoorOcclusionGradientMaterial () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:69678)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseExteriorGroundShadowBreakupCycle96 (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:11482)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateExterior (UnityEngine.Transform,string,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:10369)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseMap (UnityEngine.Transform,bool,Anemora.EditorTools.AnemoraFastVsHouseSliceSetup/Materials) (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:9210)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:CreateHouseSliceScene () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:426)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHd2dPhaseBBetaButoAdoptionBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:1431)

[Assets/Editor/AnemoraFastVsHouseSliceSetup.cs line 426]

Start importing Assets/Art/Materials/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient.mat using Guid(d58f2dd58f72df641b0b13d70b9aaffb) (NativeFormatImporter) -> (artifact id: '00000000000000000000000000000000') in 0.0024896 seconds
Start importing Assets/Art/Textures/FastVS/HouseSlice/FastVS_House_hd2d_outdoor_occlusion_gradient_soft.asset using Guid(c77c6612505c47d4aa1dac2b81f7897e) (NativeFormatImporter)(NativeFormatImporter)
[17:47:18] Phase 'validate' FAILED with exit 1
[17:47:18] NoRollback set; preserving worktree after validate failure
```
