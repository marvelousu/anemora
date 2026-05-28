# feat(hd2d): add phase b alpha sun runtime controls

Cycle 174 implements the first Phase B-alpha slice: SunPresetData and AnemoraSunCycleDriver now carry the runtime control values needed by URP standard volumetric intent, screen-space lens flare, and the directional sun lens flare.

## Scope

- Added per-preset B-alpha fields for volumetric fog intent: enabled, anisotropy 0.6, mean free path 100, base height 0, maximum height 30.
- Added per-preset screen-space lens flare intensity and sun lens flare intensity.
- Extended SunRuntimeValues so Phase A preset lerp carries the B-alpha values without changing the MapSunAnchor API.
- Added optional runtime application for Light volumetric-scattering property names, ScreenSpaceLensFlare volume intensity, LensFlareComponentSRP intensity, and a reflection-only VolumetricFog volume path.
- Kept URP VolumetricFog optional because this local URP package exposes ScreenSpaceLensFlare and LensFlareComponentSRP but no compiled VolumetricFog component class.

## Validation Plan

- Validate: `Anemora.FastVS.SunCycle.AnemoraSunCycleDriver.ValidateHd2dPhaseBAlphaSunCycleRuntimeBatch`
- Capture: `Anemora.FastVS.SunCycle.AnemoraSunCycleDriver.CaptureHd2dPhaseBAlphaSunCycleRuntimeCycle174ScreenshotsBatch`
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
- Smoke: built player with `-batchmode -nographics`, pattern count must be 0.

## Review Notes

- This cycle is runtime plumbing only; visible god rays and the directional sun lens flare scene attachment are still a gap against reference_01.
- Next B-alpha cycle should create the LensFlareData asset, attach LensFlareComponentSRP to the generated directional sun, wire the Volume/Profile values, and refresh public review screenshots.
- Build exe path for Tom: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`
- 起動時は `Builds/FastVS_HouseSlice/` フォルダごと起動。
