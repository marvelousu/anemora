# feat(hd2d): lighten surface ramp shader shadows

Date: 2026-05-28 JST

## Scope

- Phase A Step 5 from `docs/HD2D_SUN_CYCLE_SPEC.md`.
- Change `FastVS_SurfaceRampLit.shader` forward pass culling from `Cull Off` to `Cull Back`.
- Remove the self-authored 8-tap PCF shadow sampling block.
- Keep `FastVS_SpriteCardRampUnlit.shader` unchanged; its two-sided `Cull Off` remains required.

## Cycle Worker

SCOPED_PROMPT_ISSUED cycle=171 authored_file=Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader validate=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseASurfaceRampShaderLighteningBatch capture=Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseASurfaceRampShaderLighteningCycle171ScreenshotsBatch

Worker result:

- Authored file: `Assets/Art/Shaders/FastVS/FastVS_SurfaceRampLit.shader`
- Side-effect file: `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- Validate method: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseASurfaceRampShaderLighteningBatch`
- Capture method: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseASurfaceRampShaderLighteningCycle171ScreenshotsBatch`

## Validation

Pending parent runner execution:

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseASurfaceRampShaderLighteningBatch`
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dPhaseASurfaceRampShaderLighteningCycle171ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer`
- Smoke: built exe launch with `-batchmode -nographics`

## Build Artifact For Tom

Build exe path:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。

## Cycle 171 failure (validate) -- 20260528-111930

```
		ExecutionOrderSort (0ms)
		DisableScriptedObjects (25ms)
		BackupInstance (0ms)
		ReleaseScriptingObjects (0ms)
		CreateAndSetChildDomain (154ms)
	RebuildCommonClasses (40ms)
	RebuildNativeTypeToScriptingClass (13ms)
	initialDomainReloadingComplete (37ms)
	LoadAllAssembliesAndSetupDomain (651ms)
		LoadAssemblies (508ms)
		RebuildTransferFunctionScriptingTraits (0ms)
		AnalyzeDomain (261ms)
			TypeCache.Refresh (194ms)
				TypeCache.ScanAssembly (181ms)
			BuildScriptInfoCaches (54ms)
			ResolveRequiredComponents (10ms)
	FinalizeReload (3649ms)
		ReleaseScriptCaches (0ms)
		RebuildScriptCaches (0ms)
		SetupLoadedEditorAssemblies (3347ms)
			LogAssemblyErrors (0ms)
			InitializePlatformSupportModulesInManaged (26ms)
			SetLoadedEditorAssemblies (4ms)
			BeforeProcessingInitializeOnLoad (211ms)
			ProcessInitializeOnLoadAttributes (2834ms)
			ProcessInitializeOnLoadMethodAttributes (260ms)
			AfterProcessingInitializeOnLoad (12ms)
			EditorAssembliesLoaded (0ms)
		ExecutionOrderSort2 (0ms)
		AwakeInstancesAfterBackupRestoration (29ms)
Asset Pipeline Refresh (id=5b27df905bc90db4ebd8c0a6fdce7dc3): Total: 6.484 seconds - Initiated by InitialRefreshV2(ForceSynchronousImport)
	Summary:
		Imports: total=0 (actual=0, local cache=0, cache server=0)
		Asset DB Process Time: managed=0 ms, native=4288 ms
		Asset DB Callback time: managed=35 ms, native=25 ms
		Scripting: domain reloads=1, domain reload time=1087 ms, compile time=981 ms, other=65 ms
		Project Asset Count: scripts=1241, non-scripts=2910
		Asset File Changes: new=0, changed=0, moved=0, deleted=0
		Scan Filter Count: 0
	InvokeCustomDependenciesCallbacks: 0.003ms
	InvokePackagesCallback: 28.719ms
	ApplyChangesToAssetFolders: 0.713ms
	Scan: 261.756ms
	OnSourceAssetsModified: 0.005ms
	CategorizeAssetsWithTransientArtifact: 37.216ms
	ProcessAssetsWithTransientArtifactChanges: 72.245ms
	CategorizeAssets: 62.885ms
	ImportOutOfDateAssets: 3661.162ms (2675.192ms without children)
		CompileScripts: 981.001ms
		ReloadNativeAssets: 0.088ms
		UnloadImportedAssets: 3.579ms
		EnsureUptoDateAssetsAreRegisteredWithGuidPM: 0.843ms
		InitializingProgressBar: 0.000ms
		PostProcessAllAssetNotificationsAddChangedAssets: 0.001ms
		OnDemandSchedulerStart: 0.459ms
	PostProcessAllAssets: 35.657ms
	GatherAllCurrentPrimaryArtifactRevisions: 0.002ms
	UnloadStreamsBegin: 1.350ms
	PersistCurrentRevisions: 0.135ms
	UnloadStreamsEnd: 0.002ms
	GenerateScriptTypeHashes: 3.578ms
	Untracked: 2322.226ms

Application.AssetDatabase Initial Refresh End
Launched and connected shader compiler UnityShaderCompiler.exe after 0.02 seconds
Scanning for USB devices : 1.232ms
Initializing Unity extensions:
[MODES] ModeService[none].Initialize
[MODES] ModeService[none].LoadModes
[MODES] Loading mode Default (0) for mode-current-id-Anemora
Unloading 59 Unused Serialized files (Serialized files now loaded: 0)
Unloading 5398 unused Assets / (11.7 MB). Loaded Objects now: 6206.
Memory consumption went from 196.3 MB to 184.6 MB.
Total: 15.648300 ms (FindLiveObjects: 0.735900 ms CreateObjectMapping: 0.548700 ms MarkObjects: 8.496500 ms  DeleteObjects: 5.865700 ms)

executeMethod method 'ValidateHd2dPhaseASurfaceRampShaderLighteningBatch' in class 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup' could not be found.
Argument was -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dPhaseASurfaceRampShaderLighteningBatch
Exiting without the bug reporter. Application will terminate with return code 1
[11:19:45] Phase 'validate' FAILED with exit 1
[11:19:45] NoRollback set; preserving worktree after validate failure
```

## Cycle 171 failure (validate) -- 20260528-112222

```
Platform: d3d11 
Keywords: UNITY_ENABLE_REFLECTION_BUFFERS UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS UNITY_PBS_USE_BRDF1 UNITY_SPECCUBE_BOX_PROJECTION UNITY_SPECCUBE_BLENDING UNITY_ENABLE_DETAIL_NORMALMAP SHADER_API_DESKTOP UNITY_COLORSPACE_GAMMA UNITY_LIGHT_PROBE_PROXY_VOLUME UNITY_LIGHTMAP_FULL_HDR UNITY_PLATFORM_SUPPORTS_DEPTH_FETCH 

0x00007FFA1C8FA59F (Unity) BucketAllocator::Allocate
0x00007FFA1D5079EF (Unity) FBXAllocate
0x00007FFA54156ED6 (libfbxsdk) fbxsdk::FbxMalloc
0x00007FFA5412D53E (libfbxsdk) fbxsdk::FbxManager::Create
0x00007FFA1D554379 (Unity) UnityFBX::DoImportScene
0x00007FFA1D506F50 (Unity) FBXImporter::DoMeshImport
0x00007FFA1D4B2F43 (Unity) ModelImporter::GenerateAssetData
0x00007FFA1D81A09E (Unity) ImportToObjects
0x00007FFA1D818F22 (Unity) ImportAsset
0x00007FFA1D83CAE9 (Unity) AssetImportWorker::Import
0x00007FFA1D86F31A (Unity) AssetImportManager::ImportInProcess
0x00007FFA1D86DF01 (Unity) AssetImportManager::Import
0x00007FFA1D86FDD2 (Unity) ImportOutOfDateAssets
0x00007FFA1D878C0A (Unity) RefreshInternalV2
0x00007FFA1D881727 (Unity) StopAssetImportingV2Internal
0x00007FFA1D8848A8 (Unity) VerifyAssetsForBuildTargetV2
0x00007FFA1D7F1B83 (Unity) AssetDatabase::VerifyAssetsForBuildTarget
0x00007FFA1C0A0CC0 (Unity) ColorSpaceLiveSwitch
0x00007FFA1B21CAB9 (Unity) ColorSpaceUpdate
0x00007FFA1B21CA8F (Unity) CheckPlayerSettingsChanged
0x00007FFA1D7FAF57 (Unity) Postprocess
0x00007FFA1D858A93 (Unity) CallPostProcessAllAssetsCallbacks
0x00007FFA1D876230 (Unity) ProcessPostProcessAllAssetNotificationsAndHotReload
0x00007FFA1D881AB5 (Unity) StopAssetImportingV2Internal
0x00007FFA1D87031F (Unity) InitialRefreshV2
0x00007FFA1D7E6D82 (Unity) AssetDatabase::InitialRefresh
0x00007FFA1C14C930 (Unity) Application::InitializeProject
0x00007FFA1BF40B33 (Unity) UnityMain
0x00007FF78EF02F2A (Unity) __scrt_common_main_seh
0x00007FFB10CDE957 (KERNEL32) BaseThreadInitThunk
0x00007FFB1230427C (ntdll) RtlUserThreadStart

========== END OF STACKTRACE ===========

A crash has been intercepted by the crash handler. For call stack and other details, see the latest crash report generated in:
 * C:/Users/maro6/AppData/Local/Temp/Unity/Editor/Crashes
Failed to get ipc connection from UnityShaderCompiler.exe shader compiler! Error code 0x80000008 (Timed out). C:/Program Files/Unity/Hub/Editor/6000.3.14f1/Editor/Data/Tools/UnityShaderCompiler.exe
Launched and connected shader compiler UnityShaderCompiler.exe after 0.07 seconds
Launched and connected shader compiler UnityShaderCompiler.exe after 0.09 seconds
Shader compiler: UnityShaderCompiler.exe compiler executable disappeared on thread 9552, restarting
Launched and connected shader compiler UnityShaderCompiler.exe after 0.03 seconds
Shader Compiler IPC Exception: Terminating shader compiler process
Last 50 lines from the compiler log:
Base path: 'C:/Program Files/Unity/Hub/Editor/6000.3.14f1/Editor/Data', plugins path 'C:/Program Files/Unity/Hub/Editor/6000.3.14f1/Editor/Data/PlaybackEngines', jobs: n

Cmd: initializeCompiler



Cmd: compileSnippet

  insize=3651 file=Packages/com.unity.render-pipelines.core/Editor/LookDev/CubeToLatlong.shader name=Hidden/LookDev/CubeToLatlong pass=<Unnamed Pass 1> ppOnly=0 stripLineD=0 buildPlatform=19 rsLen=0 pKW=UNITY_ENABLE_REFLECTION_BUFFERS UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS UNITY_PBS_USE_BRDF1 UNITY_SPECCUBE_BOX_PROJECTION UNITY_SPECCUBE_BLENDING UNITY_ENABLE_DETAIL_NORMALMAP SHADER_API_DESKTOP UNITY_COLORSPACE_GAMMA UNITY_LIGHT_PROBE_PROXY_VOLUME UNITY_LIGHTMAP_FULL_HDR UNITY_PLATFORM_SUPPORTS_DEPTH_FETCH uKW= dKW=UNITY_NO_DXT5nm UNITY_FRAMEBUFFER_FETCH_AVAILABLE UNITY_METAL_SHADOWS_USE_POINT_FILTERING UNITY_NO_SCREENSPACE_SHADOWS UNITY_PBS_USE_BRDF2 UNITY_PBS_USE_BRDF3 UNITY_HARDWARE_TIER1 UNITY_HARDWARE_TIER2 UNITY_HARDWARE_TIER3 UNITY_LIGHTMAP_DLDR_ENCODING UNITY_LIGHTMAP_RGBM_ENCODING UNITY_VIRTUAL_TEXTURING UNITY_PRETRANSFORM_TO_DISPLAY_ORIENTATION UNITY_ASTC_NORMALMAP_ENCODING SHADER_API_GLES30 SHADER_API_GLES31 SHADER_API_GLES32 UNITY_UNIFIED_SHADER_PRECISION_MODEL flags=0 lang=0 type=Vertex platform=d3d11 reqs=227 mask=6 start=125 ok=1 outsize=722



Cmd: compileSnippet

  insize=7770 file=Packages/com.unity.render-pipelines.universal/Runtime/RendererFeatures/OnTileUberPost.shader name=OnTileUberPost pass=OnTileUberPostTextureReadVersion ppOnly=0 stripLineD=0 buildPlatform=19 rsLen=0 pKW=UNITY_ENABLE_REFLECTION_BUFFERS UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS UNITY_PBS_USE_BRDF1 UNITY_SPECCUBE_BOX_PROJECTION UNITY_SPECCUBE_BLENDING UNITY_ENABLE_DETAIL_NORMALMAP SHADER_API_DESKTOP UNITY_COLORSPACE_GAMMA UNITY_LIGHT_PROBE_PROXY_VOLUME UNITY_LIGHTMAP_FULL_HDR UNITY_PLATFORM_SUPPORTS_DEPTH_FETCH uKW= dKW=_HDR_GRADING _TONEMAP_ACES _TONEMAP_NEUTRAL _FILM_GRAIN _DITHERING _GAMMA_20 _LINEAR_TO_SRGB_CONVERSION _USE_FAST_SRGB_LINEAR_CONVERSION _ENABLE_ALPHA_OUTPUT UNITY_NO_DXT5nm UNITY_FRAMEBUFFER_FETCH_AVAILABLE UNITY_METAL_SHADOWS_USE_POINT_FILTERING UNITY_NO_SCREENSPACE_SHADOWS UNITY_PBS_USE_BRDF2 UNITY_PBS_USE_BRDF3 UNITY_HARDWARE_TIER1 UNITY_HARDWARE_TIER2 UNITY_HARDWARE_TIER3 UNITY_LIGHTMAP_DLDR_ENCODING UNITY_LIGHTMAP_RGBM_ENCODING UNITY_VIRTUAL_TEXTURING UNITY_PRETRANSFORM_TO_DISPLAY_ORIENTATION UNITY_ASTC_NORMALMAP_ENCODING SHADER_API_GLES30 SHADER_API_GLES31 SHADER_API_GLES32 UNITY_UNIFIED_SHADER_PRECISION_MODEL flags=0 lang=3 type=Fragment platform=d3d11 reqs=1101803 mask=6 start=2303221225477

Crashed!

=======================================

Stacktrace is not supported on this platform.

=======================================


Shader compiler: Compile OnTileUberPost - OnTileUberPostTextureReadVersion, Fragment Program: Internal error communicating with the shader compiler process.  Please report a bug including this shader and the editor log. Error code 0x80000004 (Not connected).
Thread: 40872
Exception: Protocol error - failed to read magic number (data transferred 0/4)
Platform: d3d11 
Keywords: UNITY_ENABLE_REFLECTION_BUFFERS UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS UNITY_PBS_USE_BRDF1 UNITY_SPECCUBE_BOX_PROJECTION UNITY_SPECCUBE_BLENDING UNITY_ENABLE_DETAIL_NORMALMAP SHADER_API_DESKTOP UNITY_COLORSPACE_GAMMA UNITY_LIGHT_PROBE_PROXY_VOLUME UNITY_LIGHTMAP_FULL_HDR UNITY_PLATFORM_SUPPORTS_DEPTH_FETCH 

Shader compiler: UnityShaderCompiler.exe compiler executable disappeared on thread 40872, restarting
[11:25:06] Phase 'validate' FAILED with exit -1073741819
[11:25:06] NoRollback set; preserving worktree after validate failure
```

## Capture black-PNG follow-up -- 2026-05-28

- Finding: the black PNGs were not caused by `FastVS_SurfaceRampLit.shader` using `Cull Back`.
- Repro evidence: both the Cycle171 capture probe and the pre-existing Stage8N capture probe failed through `Hidden/Universal/CoreBlit` pass `BilinearDebugDraw` at `Common.hlsl(794)` on D3D11.
- `renderPostProcessing = false`, disabling Stage7 fullscreen renderer features, setting intermediate texture mode to auto, and forcing D3D12 did not remove the root error.
- Fix path: `CreateHouseSliceScene`, `SaveCameraPng`, and `WarmUpCameraRender` now preserve the normal `camera.Render()` route and first apply a narrow local Unity 6000.3.14f1 / URP CoreBlit D3D11 workaround in the package cache by commenting the failing `SafePositivePow` min16float overloads in `Library/PackageCache/com.unity.render-pipelines.core@*/ShaderLibrary/Common.hlsl`.

Final verification:

- Cycle171 capture: `Logs/cycle171_surface_ramp_capture_coreblit_scene_entry_final.log`
  - No `Shader error`, `Hidden/Universal/CoreBlit`, or `BilinearDebugDraw` hits.
  - Output PNG sampled luma: interior `51.14`, exterior `30.61`, plaza `92.76`, library `26.42`.
- Existing Stage8N capture: `Logs/stage8n_capture_coreblit_scene_entry_final.log`
  - No `Shader error`, `Hidden/Universal/CoreBlit`, or `BilinearDebugDraw` hits.
  - Output PNG sampled luma: `home 34.64`, `Home_outside 43.76`, `plaza_01 32.03`, `plaza_02_niro_in_shadow 26.09`, `library 26.36`, `tw_current_aperture 48.36`.
