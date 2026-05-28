# HD2D Phase B-alpha Sun Cycle Runtime Diagnostics

- Authored runtime file: `Assets/Scripts/FastVS/SunCycle/AnemoraSunCycleDriver.cs`
- Preset assets: `Assets/Settings/SunCycle/SunPreset_<Name>.asset`
- Static validate entry: `Anemora.FastVS.SunCycle.AnemoraSunCycleDriver.ValidateHd2dPhaseBAlphaSunCycleRuntimeBatch`
- Static capture entry: `Anemora.FastVS.SunCycle.AnemoraSunCycleDriver.CaptureHd2dPhaseBAlphaSunCycleRuntimeCycle174ScreenshotsBatch`

| Preset | Asset | Direction Euler | Intensity | Volumetric | Screen Flare | Sun Flare | LUT | LUT Contribution | White Balance |
|---|---|---:|---:|---|---:|---:|---|---:|---:|
| Morning | `Assets/Settings/SunCycle/SunPreset_Morning.asset` | (24, -118, 0) | 1.6 | enabled, aniso 0.6, mfp 100, base 0, max 30 | 0.4 | 0.48 | `Assets/Art/LUT/LUT_Morning_Warm.png` | 0.6 | temp 12, tint 0 |
| Noon | `Assets/Settings/SunCycle/SunPreset_Noon.asset` | (70, -12, 0) | 2.4 | enabled, aniso 0.6, mfp 100, base 0, max 30 | 0.4 | 0.3 | `Assets/Art/LUT/LUT_Daylight.png` | 0.6 | temp 8, tint 0 |
| Evening | `Assets/Settings/SunCycle/SunPreset_Evening.asset` | (12, 58, 0) | 1.7 | enabled, aniso 0.6, mfp 100, base 0, max 30 | 0.4 | 0.72 | `Assets/Art/LUT/LUT_GoldenHour.png` | 0.7 | temp 18, tint 4 |
| Night | `Assets/Settings/SunCycle/SunPreset_Night.asset` | (-35, 8, 0) | 0.4 | enabled, aniso 0.6, mfp 100, base 0, max 30 | 0.4 | 0.26 | `Assets/Art/LUT/LUT_Night_CoolBlue.png` | 0.7 | temp -12, tint -4 |
