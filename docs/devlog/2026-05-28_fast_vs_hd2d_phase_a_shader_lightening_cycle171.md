# feat(hd2d): lighten surface ramp shader shadow path

Date: 2026-05-28 JST

## Scope

- Phase A Step 5 from `docs/HD2D_SUN_CYCLE_SPEC.md`.
- Changed `FastVS_SurfaceRampLit.shader` surface pass from `Cull Off` to `Cull Back`.
- Removed the custom multi-sample main-light PCF block from `FastVS_SurfaceRampLit.shader`.
- Kept `FastVS_SpriteCardRampUnlit.shader` two-sided culling unchanged.

## Cycle Worker

SCOPED_PROMPT_ISSUED cycle=171 authored_file=Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAShaderLighteningBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseAShaderLighteningCycle171ScreenshotsBatch

Worker result:

- Worker `019e6c47-17dc-77b1-814d-706b55dde220` explored the shader and editor batch locations, then returned without landing edits.
- Parent completed the scoped shader and validation entry changes.

## Implementation

- Surface ramp pass now uses `Cull Back`.
- Surface ramp shader now keeps the URP standard `TransformWorldToShadowCoord(input.positionWS)` -> `GetMainLight(...)` -> `mainLight.shadowAttenuation` path.
- The previous offset shadow samples using `MainLightRealtimeShadow(...)`, `shadowTangent`, and `shadowBitangent` are removed.

## Validation

Pending runner:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAShaderLighteningBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseAShaderLighteningCycle171ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateFastVsHouseSliceBatch`
- Smoke scan patterns: `Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed`

## Build Artifact For Tom

Build exe path:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。

## Gate Status

- 変更を適用しました: Phase A shader lightening source changes are staged for validation.
- 参考画像とのギャップは、まだ Phase A final gate 前のため大きく残っています。5-area gate screenshots, TimeWindow aperture check, and Tom review remain pending.
- Tom 判定をお願いする段階ではまだありません。Cycle171 runner 完了後に Phase A final gate artifacts を揃えます。

## Cycle 171 failure (validate) -- 20260528-105534

```
[10:55:34] Cycle runner starting
[10:55:34]   CycleNumber    : 171
[10:55:34]   ProjectPath    : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
[10:55:34]   BatchTool      : C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
[10:55:34]   ValidateMethod : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAShaderLighteningBatch
[10:55:34]   CaptureMethod  : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseAShaderLighteningCycle171ScreenshotsBatch
[10:55:34]   BuildMethod    : Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch
[10:55:34]   Audience       : parent_review
[10:55:34]   CaptureOutDir  : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots
[10:55:34]   DevlogPath     : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-28_fast_vs_hd2d_phase_a_shader_lightening_cycle171.md
[10:55:34]   SmokeSeconds   : 24
[10:55:34]   SmokePatterns  : Exception|Shader error|error CS|NullReference|MissingReference|Assertion|Failed
[10:55:34]   CommitPath     : Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader; Assets/Editor/AnemoraFastVsHouseSliceSetup.cs; docs/devlog/2026-05-28_fast_vs_hd2d_phase_a_shader_lightening_cycle171.md; docs/devlog/screenshots/fast_vs_hd2d_cycle171_shader_lightening_parent_review_20260528_01
[10:55:34]   NoRollback     : True
[10:55:34]   RunLog         : C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\tools\logs\cycle-171-20260528-105534.log
[10:55:34] Phase 'validate' begin: Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAShaderLighteningBatch

===== validate batch log (C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-171-20260528-105534-validate.log) =====
[Licensing::Module] Trying to connect to existing licensing client channel...
Built from '6000.3/staging' branch; Version is '6000.3.14f1 (d68c3f99a318) revision 14060607'; Using compiler version '194234433'; Build Type 'Release'
OS: 'Windows 11  (10.0.26200) Core' Language: 'en' Physical Memory: 14177 MB
BatchMode: 1, IsHumanControllingUs: 0, StartBugReporterOnCrash: 0, Is64bit: 1
[Licensing::IpcConnector] Successfully connected to: "LicenseClient-maro6" at "2026-05-28T01:55:34.7593681Z"
System  architecture: x64
Process architecture: x64
Date: 2026-05-28T01:55:34Z

COMMAND LINE ARGUMENTS:
C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe
-batchmode
-quit
-projectPath
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
-executeMethod
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAShaderLighteningBatch
-logFile
C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\cycle-171-20260528-105534-validate.log
Successfully changed project path to: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work
C:/Users/maro6/Documents/Unity/Anemora-fast-vs-v24-hd2d-work
Exiting without the bug reporter. Application will terminate with return code 1
[10:55:37] Phase 'validate' FAILED with exit 1
[10:55:37] NoRollback set; preserving worktree after validate failure
```

## Cycle 171 failure (validate) -- 20260528-105851

```
		CreateAndSetChildDomain (150ms)
	RebuildCommonClasses (41ms)
	RebuildNativeTypeToScriptingClass (13ms)
	initialDomainReloadingComplete (38ms)
	LoadAllAssembliesAndSetupDomain (732ms)
		LoadAssemblies (582ms)
		RebuildTransferFunctionScriptingTraits (0ms)
		AnalyzeDomain (261ms)
			TypeCache.Refresh (192ms)
				TypeCache.ScanAssembly (177ms)
			BuildScriptInfoCaches (54ms)
			ResolveRequiredComponents (11ms)
	FinalizeReload (3673ms)
		ReleaseScriptCaches (0ms)
		RebuildScriptCaches (0ms)
		SetupLoadedEditorAssemblies (3267ms)
			LogAssemblyErrors (0ms)
			InitializePlatformSupportModulesInManaged (25ms)
			SetLoadedEditorAssemblies (3ms)
			BeforeProcessingInitializeOnLoad (196ms)
			ProcessInitializeOnLoadAttributes (2620ms)
			ProcessInitializeOnLoadMethodAttributes (409ms)
			AfterProcessingInitializeOnLoad (13ms)
			EditorAssembliesLoaded (1ms)
		ExecutionOrderSort2 (0ms)
		AwakeInstancesAfterBackupRestoration (48ms)
Refreshing native plugins compatible for Editor in 1.78 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=6fda76e5480949f458803a5c06cecb31): Total: 32.411 seconds - Initiated by InitialRefreshV2(ForceSynchronousImport)
	Summary:
		Imports: total=0 (actual=0, local cache=0, cache server=0)
		Asset DB Process Time: managed=0 ms, native=4724 ms
		Asset DB Callback time: managed=467 ms, native=40 ms
		Scripting: domain reloads=1, domain reload time=1168 ms, compile time=25934 ms, other=77 ms
		Project Asset Count: scripts=1241, non-scripts=2910
		Asset File Changes: new=0, changed=0, moved=0, deleted=1
		Scan Filter Count: 0
	InvokeCustomDependenciesCallbacks: 0.003ms
	InvokePackagesCallback: 38.085ms
	ApplyChangesToAssetFolders: 1.065ms
	Scan: 593.502ms
	OnSourceAssetsModified: 1.579ms
	CategorizeAssetsWithTransientArtifact: 59.002ms
	ProcessAssetsWithTransientArtifactChanges: 71.069ms
	UnregisterDeletedAssets: 0.004ms
	CategorizeAssets: 64.543ms
	ImportOutOfDateAssets: 3683.874ms (-22252.454ms without children)
		CompileScripts: 25933.966ms
		ReloadNativeAssets: 0.074ms
		UnloadImportedAssets: 1.254ms
		EnsureUptoDateAssetsAreRegisteredWithGuidPM: 0.621ms
		InitializingProgressBar: 0.000ms
		PostProcessAllAssetNotificationsAddChangedAssets: 0.001ms
		OnDemandSchedulerStart: 0.413ms
	PostProcessAllAssets: 465.352ms
	Hotreload: 5.800ms
	GatherAllCurrentPrimaryArtifactRevisions: 0.003ms
	UnloadStreamsBegin: 2.314ms
	PersistCurrentRevisions: 0.212ms
	UnloadStreamsEnd: 0.002ms
	GenerateScriptTypeHashes: 3.724ms
	Untracked: 27425.050ms

Application.AssetDatabase Initial Refresh End
Launched and connected shader compiler UnityShaderCompiler.exe after 0.03 seconds
Scanning for USB devices : 1.987ms
Initializing Unity extensions:
[MODES] ModeService[none].Initialize
[MODES] ModeService[none].LoadModes
[MODES] Loading mode Default (0) for mode-current-id-Anemora
Unloading 59 Unused Serialized files (Serialized files now loaded: 0)
Unloading 5398 unused Assets / (11.6 MB). Loaded Objects now: 6207.
Memory consumption went from 193.3 MB to 181.7 MB.
Total: 16.702600 ms (FindLiveObjects: 0.736100 ms CreateObjectMapping: 0.357400 ms MarkObjects: 9.242600 ms  DeleteObjects: 6.365300 ms)

executeMethod method 'ValidateHd2dPhaseAShaderLighteningBatch' in class 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup' could not be found.
Argument was -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAShaderLighteningBatch
Exiting without the bug reporter. Application will terminate with return code 1
[10:59:32] Phase 'validate' FAILED with exit 1
[10:59:32] NoRollback set; preserving worktree after validate failure
```

## Cycle 171 failure (validate) -- 20260528-110017

```
		ExecutionOrderSort (0ms)
		DisableScriptedObjects (49ms)
		BackupInstance (0ms)
		ReleaseScriptingObjects (0ms)
		CreateAndSetChildDomain (157ms)
	RebuildCommonClasses (38ms)
	RebuildNativeTypeToScriptingClass (13ms)
	initialDomainReloadingComplete (35ms)
	LoadAllAssembliesAndSetupDomain (528ms)
		LoadAssemblies (384ms)
		RebuildTransferFunctionScriptingTraits (0ms)
		AnalyzeDomain (250ms)
			TypeCache.Refresh (186ms)
				TypeCache.ScanAssembly (172ms)
			BuildScriptInfoCaches (51ms)
			ResolveRequiredComponents (9ms)
	FinalizeReload (3107ms)
		ReleaseScriptCaches (0ms)
		RebuildScriptCaches (0ms)
		SetupLoadedEditorAssemblies (2816ms)
			LogAssemblyErrors (0ms)
			InitializePlatformSupportModulesInManaged (25ms)
			SetLoadedEditorAssemblies (3ms)
			BeforeProcessingInitializeOnLoad (187ms)
			ProcessInitializeOnLoadAttributes (2316ms)
			ProcessInitializeOnLoadMethodAttributes (273ms)
			AfterProcessingInitializeOnLoad (12ms)
			EditorAssembliesLoaded (1ms)
		ExecutionOrderSort2 (0ms)
		AwakeInstancesAfterBackupRestoration (29ms)
Asset Pipeline Refresh (id=1eb8bc5547564a946912b2ba148b3c10): Total: 5.749 seconds - Initiated by InitialRefreshV2(ForceSynchronousImport)
	Summary:
		Imports: total=0 (actual=0, local cache=0, cache server=0)
		Asset DB Process Time: managed=0 ms, native=3708 ms
		Asset DB Callback time: managed=31 ms, native=23 ms
		Scripting: domain reloads=1, domain reload time=974 ms, compile time=945 ms, other=65 ms
		Project Asset Count: scripts=1241, non-scripts=2910
		Asset File Changes: new=0, changed=0, moved=0, deleted=0
		Scan Filter Count: 0
	InvokeCustomDependenciesCallbacks: 0.003ms
	InvokePackagesCallback: 29.651ms
	ApplyChangesToAssetFolders: 0.899ms
	Scan: 230.715ms
	OnSourceAssetsModified: 0.006ms
	CategorizeAssetsWithTransientArtifact: 34.508ms
	ProcessAssetsWithTransientArtifactChanges: 69.676ms
	CategorizeAssets: 60.742ms
	ImportOutOfDateAssets: 3116.659ms (2167.780ms without children)
		CompileScripts: 945.185ms
		ReloadNativeAssets: 0.077ms
		UnloadImportedAssets: 2.463ms
		EnsureUptoDateAssetsAreRegisteredWithGuidPM: 0.700ms
		InitializingProgressBar: 0.001ms
		PostProcessAllAssetNotificationsAddChangedAssets: 0.000ms
		OnDemandSchedulerStart: 0.452ms
	PostProcessAllAssets: 31.930ms
	GatherAllCurrentPrimaryArtifactRevisions: 0.003ms
	UnloadStreamsBegin: 1.570ms
	PersistCurrentRevisions: 0.117ms
	UnloadStreamsEnd: 0.001ms
	GenerateScriptTypeHashes: 3.282ms
	Untracked: 2172.683ms

Application.AssetDatabase Initial Refresh End
Launched and connected shader compiler UnityShaderCompiler.exe after 0.02 seconds
Scanning for USB devices : 1.082ms
Initializing Unity extensions:
[MODES] ModeService[none].Initialize
[MODES] ModeService[none].LoadModes
[MODES] Loading mode Default (0) for mode-current-id-Anemora
Unloading 59 Unused Serialized files (Serialized files now loaded: 0)
Unloading 5398 unused Assets / (12.2 MB). Loaded Objects now: 6206.
Memory consumption went from 192.8 MB to 180.6 MB.
Total: 22.719600 ms (FindLiveObjects: 0.978100 ms CreateObjectMapping: 0.741800 ms MarkObjects: 8.776200 ms  DeleteObjects: 12.222500 ms)

executeMethod method 'ValidateHd2dPhaseAShaderLighteningBatch' in class 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup' could not be found.
Argument was -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAShaderLighteningBatch
Exiting without the bug reporter. Application will terminate with return code 1
[11:00:30] Phase 'validate' FAILED with exit 1
[11:00:30] NoRollback set; preserving worktree after validate failure
```

## Cycle 171 failure (build) -- 20260528-110205

```
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=9ee31b13d97e5f34dbf5fe6812a6e252): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_writing_normal_loop_v02_6f_64x96.png using Guid(61b3cd5f5b4bc2b41953653a46c5c050) (TextureImporter) -> (artifact id: '7ff045c530812383dfde4d98fe46707e') in 0.0036849 seconds
Refreshing native plugins compatible for Editor in 0.76 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=15980b0452df52846831381dfca93dc7): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0039978 seconds
Refreshing native plugins compatible for Editor in 0.74 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=6db10272cd4c37e49a24cf9c9ca0dc69): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_lower_arms_v02_6f_64x96.png using Guid(df5ea5b310e225a4aa07640e443bf95b) (TextureImporter) -> (artifact id: '688806f9fb755d1625d8181367b45f7b') in 0.0038789 seconds
Refreshing native plugins compatible for Editor in 0.77 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=465d7c582fd31ad4d8b55cbc6bbb615d): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0036052 seconds
Refreshing native plugins compatible for Editor in 0.76 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=ba9303f3758e90a4b98d91d0d82540e0): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0035977 seconds
Refreshing native plugins compatible for Editor in 0.76 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b7601b7bb7193f24c8ce749da0ada39d): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0042539 seconds
Refreshing native plugins compatible for Editor in 0.80 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=067c34301e194454492f69928bd228d7): Total: 0.027 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0037748 seconds
Refreshing native plugins compatible for Editor in 0.73 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=e13bd142a9821264bae03056b411374d): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:37)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:469)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 37)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:100)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:470)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 100)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:43)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:471)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 43)

InvalidOperationException: House slice validation failed: library lighting profile must keep the Stage 3 raised key light and procedural window light enabled.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dShadingFoundationLightingDirector () [0x001a0] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:42531 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000bd] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:475 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:767 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 42531)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:05:16] Phase 'build' FAILED with exit 1
[11:05:16] NoRollback set; preserving worktree after build failure
```

## Cycle 171 failure (build) -- 20260528-110708

```
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=f7bffd534469a214fa4ada8a261ff2b8): Total: 0.028 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0038149 seconds
Refreshing native plugins compatible for Editor in 0.77 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=44eb956dd454291458f4881f897a05bd): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_talk_loop_v02_4f_64x96.png using Guid(7431e73abb997134fbfb83594af2ebba) (TextureImporter) -> (artifact id: '201b4c101e54f0caf099295eeba2a525') in 0.0033134 seconds
Refreshing native plugins compatible for Editor in 0.78 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=370615a82b7b3404a90cef5786d99096): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0037688 seconds
Refreshing native plugins compatible for Editor in 0.78 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=b1d9cd0aebb92ef429379d5945546dcc): Total: 0.025 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
Start importing Assets/Art/Characters/FastVS/Reto/reto_raise_arms_v02_6f_64x96.png using Guid(c166f8b56d9af3b46be6b393e60b1b8c) (TextureImporter) -> (artifact id: '4a57e44ff898512ebb3346c48150f08c') in 0.0040037 seconds
Refreshing native plugins compatible for Editor in 0.73 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=1b9d11894cf771948a325b29c6bd4c6a): Total: 0.026 seconds - Initiated by StopAssetImportingV2(NoUpdateAssetOptions)
HD2D material role audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dMaterialRoleFoundationAudit:VerifyMaterialRolesV1 () (at Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs:37)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:469)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 37)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:100)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:470)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 100)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:43)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:471)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 43)

InvalidOperationException: HD2D area lighting profile audit failed:
- Area lighting profile FastVS_HD2D_HouseInteriorLightingProfile must keep runtime mainLight.intensity near 1.200, but was 1.600.
- Area lighting profile FastVS_HD2D_HouseInteriorLightingProfile must keep runtime mainLight.color near RGBA(1.000, 0.850, 0.640, 1.000), but was RGBA(1.000, 0.850, 0.720, 1.000).
- Area lighting profile FastVS_HD2D_HouseInteriorLightingProfile runtime main light rotation must match (48.00, -38.00, 0.00), but angle delta was 82.940 degrees.
- Area lighting profile FastVS_HD2D_HouseExteriorLightingProfile must keep runtime mainLight.intensity near 1.800, but was 1.600.
- Area lighting profile FastVS_HD2D_HouseExteriorLightingProfile must keep runtime mainLight.color near RGBA(1.000, 0.920, 0.760, 1.000), but was RGBA(1.000, 0.850, 0.720, 1.000).
- Area lighting profile FastVS_HD2D_HouseExteriorLightingProfile runtime main light rotation must match (52.00, -38.00, 0.00), but angle delta was 83.975 degrees.
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep runtime mainLight.intensity near 1.720, but was 1.600.
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile must keep runtime mainLight.color near RGBA(1.000, 0.860, 0.620, 1.000), but was RGBA(1.000, 0.850, 0.720, 1.000).
- Area lighting profile FastVS_HD2D_CentralPlazaLightingProfile runtime main light rotation must match (38.00, -38.00, 0.00), but angle delta was 81.013 degrees.
- Area lighting profile FastVS_HD2D_LibraryLightingProfile must keep runtime mainLight.intensity near 1.700, but was 1.600.
- Area lighting profile FastVS_HD2D_LibraryLightingProfile must keep runtime mainLight.color near RGBA(1.000, 0.830, 0.620, 1.000), but was RGBA(1.000, 0.850, 0.720, 1.000).
- Area lighting profile FastVS_HD2D_LibraryLightingProfile runtime main light rotation must match (56.00, -38.00, 0.00), but angle delta was 85.154 degrees.
  at Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.VerifyAreaLightingProfilesV1 () [0x00318] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:143 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000c7] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:477 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:767 

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 143)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:10:21] Phase 'build' FAILED with exit 1
[11:10:21] NoRollback set; preserving worktree after build failure
```

## Cycle 171 failure (build) -- 20260528-111130

```

(Filename: Assets/Editor/AnemoraFastVsHd2dMaterialRoleFoundationAudit.cs Line: 37)

HD2D sprite card lighting audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSpriteCardLightingAudit:VerifySpriteCardLightingV1 () (at Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs:100)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:470)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 100)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:43)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:471)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 43)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:141)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:477)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 141)

InvalidOperationException: HD2D overlay profile audit failed:
- HD2D overlay profile FastVS_PlayerContactShadow_Niro must keep its MeshRenderer enabled.
- HD2D overlay profile FastVS_PlayerFootContact_Niro must keep its MeshRenderer enabled.
- HD2D overlay profile FastVS_PlayerDirectionalCastShadow_Niro must keep its MeshRenderer enabled.
- HD2D overlay profile Current_Library_Reto_ContactShadow must keep its MeshRenderer enabled.
- HD2D overlay profile Current_Library_Reto_FootContact must keep its MeshRenderer enabled.
- HD2D overlay profile Current_Library_Reto_DirectionalCastShadow must keep its MeshRenderer enabled.
- HD2D overlay profile Past_Library_Aria_ContactShadow must keep its MeshRenderer enabled.
- HD2D overlay profile Past_Library_Aria_FootContact must keep its MeshRenderer enabled.
- HD2D overlay profile Past_Library_Aria_DirectionalCastShadow must keep its MeshRenderer enabled.
- HD2D overlay profile Current_HouseExterior_StaticDirectionalCastShadow_HouseFacade must keep its MeshRenderer enabled.
- HD2D overlay profile Past_HouseExterior_StaticDirectionalCastShadow_HouseFacade must keep its MeshRenderer enabled.
- HD2D overlay profile Current_CentralPlaza_StaticDirectionalCastShadow_LibraryFacade must keep its MeshRenderer enabled.
- HD2D overlay profile Past_CentralPlaza_StaticDirectionalCastShadow_LibraryFacade must keep its MeshRenderer enabled.
- HD2D overlay profile Current_Library_StaticDirectionalCastShadow_BackShelf must keep its MeshRenderer enabled.
- HD2D overlay profile Past_Library_StaticDirectionalCastShadow_BackShelf must keep its MeshRenderer enabled.
- HD2D overlay profile Current_HouseExterior_SurfaceDirectionalShade_FacadeLeft must keep its MeshRenderer enabled.
- HD2D overlay profile Past_HouseExterior_SurfaceDirectionalShade_FacadeLeft must keep its MeshRenderer enabled.
- HD2D overlay profile Current_CentralPlaza_SurfaceDirectionalShade_LibraryFacade must keep its MeshRenderer enabled.
- HD2D overlay profile Past_CentralPlaza_SurfaceDirectionalShade_LibraryFacade must keep its MeshRenderer enabled.
- HD2D overlay profile Current_Library_SurfaceDirectionalShade_BackShelf must keep its MeshRenderer enabled.
- HD2D overlay profile Past_Library_SurfaceDirectionalShade_BackShelf must keep its MeshRenderer enabled.
- HD2D overlay profile Current_HouseInterior_Table_WarmLightPool must keep its MeshRenderer enabled.
- HD2D overlay profile Past_HouseInterior_Table_WarmLightPool must keep its MeshRenderer enabled.
- HD2D overlay profile Past_HouseExterior_Door_WarmPool must keep its MeshRenderer enabled.
- HD2D overlay profile Past_CentralPlaza_LibraryFacade_WindowWarmPool must keep its MeshRenderer enabled.
- HD2D overlay profile Current_Library_RetoDesk_WarmPool must keep its MeshRenderer enabled.
- HD2D overlay profile Current_Library_EntryFloor_SoftDustPool must keep its MeshRenderer enabled.
  at Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit.VerifyOverlayProfilesV1 () [0x007b1] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:403 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000cc] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:478 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:767 

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 403)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:14:46] Phase 'build' FAILED with exit 1
[11:14:46] NoRollback set; preserving worktree after build failure
```

## Cycle 171 failure (build) -- 20260528-111600

```
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dSpriteCardLightingAudit.cs Line: 100)

Shading Foundation v1 audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dShadingFoundationAudit:VerifyShadingFoundationV1 () (at Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs:43)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:471)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 43)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:141)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:477)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 141)

HD2D overlay profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:478)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:479)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:480)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

InvalidOperationException: House slice validation failed: sampled midpoint intensity must land between the interior and library profiles.
  at Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit.VerifyLightingTransitionV1 () [0x001ef] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHd2dLightingTransitionAudit.cs:91 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x000db] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:481 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:767 

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 91)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:19:16] Phase 'build' FAILED with exit 1
[11:19:16] NoRollback set; preserving worktree after build failure
```

## Cycle 171 failure (capture) -- 20260528-112010

```
		ExecutionOrderSort (0ms)
		DisableScriptedObjects (40ms)
		BackupInstance (0ms)
		ReleaseScriptingObjects (0ms)
		CreateAndSetChildDomain (175ms)
	RebuildCommonClasses (53ms)
	RebuildNativeTypeToScriptingClass (22ms)
	initialDomainReloadingComplete (46ms)
	LoadAllAssembliesAndSetupDomain (1726ms)
		LoadAssemblies (1584ms)
		RebuildTransferFunctionScriptingTraits (0ms)
		AnalyzeDomain (282ms)
			TypeCache.Refresh (211ms)
				TypeCache.ScanAssembly (196ms)
			BuildScriptInfoCaches (56ms)
			ResolveRequiredComponents (11ms)
	FinalizeReload (4064ms)
		ReleaseScriptCaches (0ms)
		RebuildScriptCaches (0ms)
		SetupLoadedEditorAssemblies (3839ms)
			LogAssemblyErrors (0ms)
			InitializePlatformSupportModulesInManaged (26ms)
			SetLoadedEditorAssemblies (4ms)
			BeforeProcessingInitializeOnLoad (214ms)
			ProcessInitializeOnLoadAttributes (3281ms)
			ProcessInitializeOnLoadMethodAttributes (304ms)
			AfterProcessingInitializeOnLoad (10ms)
			EditorAssembliesLoaded (0ms)
		ExecutionOrderSort2 (0ms)
		AwakeInstancesAfterBackupRestoration (22ms)
Asset Pipeline Refresh (id=2e1e5473249f6f94da3168495ae5b2f5): Total: 17.708 seconds - Initiated by InitialRefreshV2(ForceSynchronousImport)
	Summary:
		Imports: total=0 (actual=0, local cache=0, cache server=0)
		Asset DB Process Time: managed=0 ms, native=4801 ms
		Asset DB Callback time: managed=37 ms, native=29 ms
		Scripting: domain reloads=1, domain reload time=2282 ms, compile time=10483 ms, other=74 ms
		Project Asset Count: scripts=1241, non-scripts=2910
		Asset File Changes: new=0, changed=0, moved=0, deleted=0
		Scan Filter Count: 0
	InvokeCustomDependenciesCallbacks: 0.003ms
	InvokePackagesCallback: 32.686ms
	ApplyChangesToAssetFolders: 0.979ms
	Scan: 280.192ms
	OnSourceAssetsModified: 0.006ms
	CategorizeAssetsWithTransientArtifact: 66.383ms
	ProcessAssetsWithTransientArtifactChanges: 78.726ms
	CategorizeAssets: 58.058ms
	ImportOutOfDateAssets: 4075.530ms (-6411.314ms without children)
		CompileScripts: 10482.572ms
		ReloadNativeAssets: 0.082ms
		UnloadImportedAssets: 2.925ms
		EnsureUptoDateAssetsAreRegisteredWithGuidPM: 0.760ms
		InitializingProgressBar: 0.001ms
		PostProcessAllAssetNotificationsAddChangedAssets: 0.001ms
		OnDemandSchedulerStart: 0.503ms
	PostProcessAllAssets: 37.432ms
	GatherAllCurrentPrimaryArtifactRevisions: 0.003ms
	UnloadStreamsBegin: 1.891ms
	PersistCurrentRevisions: 0.131ms
	UnloadStreamsEnd: 0.002ms
	GenerateScriptTypeHashes: 3.313ms
	Untracked: 13076.040ms

Application.AssetDatabase Initial Refresh End
Launched and connected shader compiler UnityShaderCompiler.exe after 0.04 seconds
Scanning for USB devices : 1.828ms
Initializing Unity extensions:
[MODES] ModeService[none].Initialize
[MODES] ModeService[none].LoadModes
[MODES] Loading mode Default (0) for mode-current-id-Anemora
Unloading 59 Unused Serialized files (Serialized files now loaded: 0)
Unloading 5398 unused Assets / (12.1 MB). Loaded Objects now: 6206.
Memory consumption went from 198.3 MB to 186.2 MB.
Total: 17.253000 ms (FindLiveObjects: 0.658400 ms CreateObjectMapping: 0.938400 ms MarkObjects: 8.846300 ms  DeleteObjects: 6.808700 ms)

executeMethod method 'CaptureHd2dPhaseAShaderLighteningCycle171ScreenshotsBatch' in class 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup' could not be found.
Argument was -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseAShaderLighteningCycle171ScreenshotsBatch
Exiting without the bug reporter. Application will terminate with return code 1
[11:26:04] Phase 'capture' FAILED with exit 1
[11:26:04] NoRollback set; preserving worktree after capture failure
```

## Cycle 171 failure (validate) -- 20260528-113207

```
			BeforeProcessingInitializeOnLoad (210ms)
			ProcessInitializeOnLoadAttributes (864ms)
			ProcessInitializeOnLoadMethodAttributes (385ms)
			AfterProcessingInitializeOnLoad (12ms)
			EditorAssembliesLoaded (1ms)
		ExecutionOrderSort2 (0ms)
		AwakeInstancesAfterBackupRestoration (32ms)
Start importing Assets/Art/Models/Zone1/LibraryRuin/Bookshelf_Library_Past.fbx using Guid(ffbd18de515a4d54f89cb1b1e7877b98) (FBXImporter) -> (artifact id: 'ca1389416ff3897930b92b258c45b91d') in 0.0680324 seconds
Start importing Assets/Prefabs/Zone1/Bookshelf_FamilyBooks.prefab using Guid(402296afce3383d4ca143c4dfc10edb2) (PrefabImporter) -> (artifact id: '0432e1bd9d5e72495fac12a075d027f6') in 0.00675 seconds
Start importing Assets/Prefabs/Zone1/Tree_Decay.prefab using Guid(609ba2f80a430f74983ec9a3527bdb8e) (PrefabImporter) -> (artifact id: '9dbfb7d1e2a917dfac0b1a2974907745') in 0.0049964 seconds
Start importing Assets/Prefabs/Zone1/Floor_Wood.prefab using Guid(027e89ce4c537e5439b7eca3d258be07) (PrefabImporter) -> (artifact id: 'c2ea2e59fbf96eebe0ed068e301e9f2b') in 0.0077345 seconds
Start importing Assets/Prefabs/Zone1/House_Player.prefab using Guid(82463215e59c42e4f9e26d66e2c5bf0a) (PrefabImporter) -> (artifact id: '3194f5b373867ed5beed3522f34f55c7') in 0.0040002 seconds
Start importing Assets/Prefabs/Zone1/Door_House.prefab using Guid(135c8c816f99edb40b633f65f4fe468a) (PrefabImporter) -> (artifact id: '3eedf09bf2e5964460cd493177329d71') in 0.0043396 seconds
Start importing Assets/Prefabs/Zone1/Bookshelf_Library_Past.prefab using Guid(636352b9454612b468cac1f57f982912) (PrefabImporter) -> (artifact id: '4635a4f57b98ff5444b19729a956b03a') in 0.0041154 seconds
Start importing Assets/Prefabs/Zone1/Bookshelf_Empty.prefab using Guid(b386db7cb7b473e48b3ce57c69848616) (PrefabImporter) -> (artifact id: 'e586696531bf5875520d47793ed5f9da') in 0.0044552 seconds
Start importing Assets/Prefabs/Zone1/Plaza_Fountain_Dry_Broken.prefab using Guid(4827f216a33e3c6459ecf86dd31f5844) (PrefabImporter) -> (artifact id: 'edbccc2bc8ee535d186399e3dec3efea') in 0.0047015 seconds
Start importing Assets/Prefabs/Zone1/Bed_Player.prefab using Guid(992a1ab266baf4947817e1dfd46dd4ed) (PrefabImporter) -> (artifact id: '7aa42782ea9b6d22a7e7433e30950bfe') in 0.0043892 seconds
Start importing Assets/Prefabs/Zone1/Library_Ruin.prefab using Guid(3ac8fe211dffb634a86209cfa1957bb0) (PrefabImporter) -> (artifact id: '569df026a19797f11656378f2fc1e41d') in 0.0052993 seconds
Start importing Assets/Prefabs/Zone1/Floor_Stone.prefab using Guid(7b552a3d210b2be4ea9f37099787f998) (PrefabImporter) -> (artifact id: '52d5c04e4bc33c20e2375049293d1844') in 0.0071431 seconds
Start importing Assets/Prefabs/Zone1/Book_Family_Current.prefab using Guid(dbd3e08d3d184420b8acba24d6605d4b) (PrefabImporter) -> (artifact id: '0e3570c6d410385f7a4b48f3f2ae3c61') in 0.0052192 seconds
Start importing Assets/Prefabs/Zone1/StreetLamp.prefab using Guid(6cf2ad729d48b7a4fbb7b732459acb17) (PrefabImporter) -> (artifact id: 'b511dda61b89b3d322d149bd913cc4b7') in 0.0104246 seconds
Start importing Assets/Prefabs/Zone1/Book_Family_Past.prefab using Guid(2dd48ec3b95604146b380e1eea31e259) (PrefabImporter) -> (artifact id: '7c9139df2667dcec94d80c231cb9e5d9') in 0.0032635 seconds
Start importing Assets/Prefabs/Zone1/Table_SmallChair_Wooden.prefab using Guid(bd81f2008f1d1144f824cec77b9ca90a) (PrefabImporter) -> (artifact id: '090fb5536b5d17fd53154ff9894e3128') in 0.0044642 seconds
Refreshing native plugins compatible for Editor in 0.65 ms, found 0 plugins.
Preloading 0 native plugins for Editor in 0.00 ms.
Asset Pipeline Refresh (id=ac9882ce272fb2448a22fca1d1da9abe): Total: 7.035 seconds - Initiated by InitialRefreshV2(ForceSynchronousImport)
	Summary:
		Imports: total=46 (actual=16, local cache=30, cache server=0)
		Asset DB Process Time: managed=14 ms, native=3394 ms
		Asset DB Callback time: managed=271 ms, native=46 ms
		Scripting: domain reloads=1, domain reload time=1781 ms, compile time=1461 ms, other=66 ms
		Project Asset Count: scripts=1241, non-scripts=2910
		Asset File Changes: new=0, changed=0, moved=0, deleted=0
		Scan Filter Count: 0
	InvokeCustomDependenciesCallbacks: 0.002ms
	InvokePackagesCallback: 42.825ms
	ApplyChangesToAssetFolders: 0.771ms
	Scan: 480.854ms
	OnSourceAssetsModified: 0.005ms
	CategorizeAssetsWithTransientArtifact: 205.891ms
	ProcessAssetsWithTransientArtifactChanges: 69.975ms
	CategorizeAssets: 69.539ms
	ImportOutOfDateAssets: 1981.732ms (349.564ms without children)
		ImportManagerImport: 167.582ms (11.797ms without children)
			ImportInProcess: 155.618ms
			UpdateCategorizedAssets: 0.167ms
		CompileScripts: 1460.552ms
		ReloadNativeAssets: 0.075ms
		UnloadImportedAssets: 2.720ms
		ReloadImportedAssets: 0.007ms
		EnsureUptoDateAssetsAreRegisteredWithGuidPM: 0.694ms
		InitializingProgressBar: 0.008ms
		PostProcessAllAssetNotificationsAddChangedAssets: 0.001ms
		OnDemandSchedulerStart: 0.529ms
	PostProcessAllAssets: 274.443ms
	Hotreload: 3.504ms
	GatherAllCurrentPrimaryArtifactRevisions: 0.002ms
	UnloadStreamsBegin: 1.309ms
	PersistCurrentRevisions: 0.351ms
	UnloadStreamsEnd: 0.001ms
	GenerateScriptTypeHashes: 4.160ms
	Untracked: 3904.152ms

Application.AssetDatabase Initial Refresh End
Launched and connected shader compiler UnityShaderCompiler.exe after 0.03 seconds
Scanning for USB devices : 1.521ms
Initializing Unity extensions:
[MODES] ModeService[none].Initialize
[MODES] ModeService[none].LoadModes
[MODES] Loading mode Default (0) for mode-current-id-Anemora
Unloading 63 Unused Serialized files (Serialized files now loaded: 0)
Unloading 5403 unused Assets / (12.8 MB). Loaded Objects now: 6205.
Memory consumption went from 198.1 MB to 185.3 MB.
Total: 15.741900 ms (FindLiveObjects: 0.683100 ms CreateObjectMapping: 0.348000 ms MarkObjects: 8.398400 ms  DeleteObjects: 6.311100 ms)

executeMethod method 'ValidateHd2dPhaseAShaderLighteningBatch' in class 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup' could not be found.
Argument was -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseAShaderLighteningBatch
Exiting without the bug reporter. Application will terminate with return code 1
[11:32:24] Phase 'validate' FAILED with exit 1
[11:32:24] NoRollback set; preserving worktree after validate failure
```

## Cycle 171 failure (build) -- 20260528-113315

```

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 43)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:141)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:477)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 141)

HD2D overlay profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:478)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:479)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:480)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:481)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

InvalidOperationException: House slice validation failed: Current_HouseExterior_OutdoorVoidBackground_NorthSilhouetteLeft must keep its renderer enabled.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateOutdoorVoidBackgroundTreatmentObject (System.String objectName, System.String expectedParentName, System.String expectedMaterialToken, System.Single maxScaleX, System.Single maxScaleY, System.Single maxScaleZ) [0x00252] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:59886 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dFiftyEighthCycleOutdoorVoidBackgroundTreatment () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:43404 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x00130] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:498 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:767 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 59886)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:36:30] Phase 'build' FAILED with exit 1
[11:36:30] NoRollback set; preserving worktree after build failure
```

## Cycle 171 failure (build) -- 20260528-113913

```

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 43)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:141)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:477)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 141)

HD2D overlay profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:478)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:479)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:480)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:481)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:767)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

InvalidOperationException: House slice validation failed: Current_HouseExterior_Cycle93_FacadeLeakClosure_FrontEaveFasciaA must stay shadow-safe.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseExteriorLeakClosureObject (System.String objectName, System.String expectedParentName, System.String expectedMaterialToken, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, UnityEngine.Vector3 minLocalScale, UnityEngine.Vector3 maxLocalScale, System.Boolean allowOcclusionMaterial) [0x00134] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:38916 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dShadowFoundationCycle93HouseExteriorLeakClosure () [0x00000] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:38763 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x00158] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:506 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:767 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 38763)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:42:33] Phase 'build' FAILED with exit 1
[11:42:33] NoRollback set; preserving worktree after build failure
```

## Cycle 171 failure (build) -- 20260528-114633

```

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 43)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:141)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:478)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:768)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 141)

HD2D overlay profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:479)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:768)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:480)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:768)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:481)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:768)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:482)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:768)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

InvalidOperationException: House slice validation failed: Current_HouseExterior_Cycle95_FacadeNaturalization_LowerLeftSideShoulderA must be shadow-safe.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseExteriorFacadeNaturalizationObject (System.String objectName, System.String expectedParentName, System.String expectedMaterialToken, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, UnityEngine.Vector3 minLocalScale, UnityEngine.Vector3 maxLocalScale, System.Boolean requireNoCollider, System.String[] forbiddenTokens) [0x0029c] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:60104 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dShadowFoundationCycle95HouseExteriorFacadeNaturalization () [0x00037] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:58758 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x00162] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:509 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:768 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 58758)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:49:51] Phase 'build' FAILED with exit 1
[11:49:51] NoRollback set; preserving worktree after build failure
```

## Cycle 171 failure (build) -- 20260528-115112

```

(Filename: Assets/Editor/AnemoraFastVsHd2dShadingFoundationAudit.cs Line: 43)

HD2D area lighting profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dAreaLightingProfileFoundationAudit:VerifyAreaLightingProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs:141)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:478)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:768)

(Filename: Assets/Editor/AnemoraFastVsHd2dAreaLightingProfileFoundationAudit.cs Line: 141)

HD2D overlay profile audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dOverlayProfileFoundationAudit:VerifyOverlayProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs:402)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:479)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:768)

(Filename: Assets/Editor/AnemoraFastVsHd2dOverlayProfileFoundationAudit.cs Line: 402)

HD2D surface profile audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceProfileFoundationAudit:VerifySurfaceProfilesV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs:104)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:480)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:768)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceProfileFoundationAudit.cs Line: 104)

HD2D surface texture metric audit passed
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dSurfaceTextureMetricAudit:VerifySurfaceTextureMetricsV1 () (at Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs:72)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:481)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:768)

(Filename: Assets/Editor/AnemoraFastVsHd2dSurfaceTextureMetricAudit.cs Line: 72)

HD2D lighting transition audit passed.
UnityEngine.Debug:ExtractStackTraceNoAlloc (byte*,int,string)
UnityEngine.StackTraceUtility:ExtractStackTrace ()
UnityEngine.DebugLogHandler:Internal_Log (UnityEngine.LogType,UnityEngine.LogOption,string,UnityEngine.Object)
UnityEngine.DebugLogHandler:LogFormat (UnityEngine.LogType,UnityEngine.Object,string,object[])
UnityEngine.Logger:Log (UnityEngine.LogType,object)
UnityEngine.Debug:Log (object)
Anemora.EditorTools.AnemoraFastVsHd2dLightingTransitionAudit:VerifyLightingTransitionV1 () (at Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs:106)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:ValidateHouseSliceBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:482)
Anemora.EditorTools.AnemoraFastVsHouseSliceSetup:BuildAndValidateBatch () (at Assets/Editor/AnemoraFastVsHouseSliceSetup.cs:768)

(Filename: Assets/Editor/AnemoraFastVsHd2dLightingTransitionAudit.cs Line: 106)

InvalidOperationException: House slice validation failed: Current_HouseExterior_Cycle95_FacadeNaturalization_UpperApronLowerTrimA must be shadow-safe.
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseExteriorFacadeNaturalizationObject (System.String objectName, System.String expectedParentName, System.String expectedMaterialToken, UnityEngine.Vector3 minLocalPosition, UnityEngine.Vector3 maxLocalPosition, UnityEngine.Vector3 minLocalScale, UnityEngine.Vector3 maxLocalScale, System.Boolean requireNoCollider, System.String[] forbiddenTokens) [0x0029c] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:60104 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateFastVsHd2dShadowFoundationCycle95HouseExteriorFacadeNaturalization () [0x00697] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:58910 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch () [0x00162] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:509 
  at Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch () [0x00005] in C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs:768 

(Filename: Assets/Editor/AnemoraFastVsHouseSliceSetup.cs Line: 58910)

executeMethod method Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch threw exception.
Exiting without the bug reporter. Application will terminate with return code 1
[11:54:20] Phase 'build' FAILED with exit 1
[11:54:20] NoRollback set; preserving worktree after build failure
```
