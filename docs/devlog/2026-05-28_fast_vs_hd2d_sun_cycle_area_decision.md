# docs(hd2d): record sun cycle area decision

## Scope

Phase A Step 0 for `docs/HD2D_SUN_CYCLE_SPEC.md`.

## Area Management Decision

`Anemora_FastVS_HouseSlice.unity` manages Interior / Exterior / CentralPlaza / Library inside one Unity scene. It does not use separate scene files for these areas in the FastVS HouseSlice build target.

Evidence:

- `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` contains all eight current/past area roots:
  - `Current_HouseInteriorMap_SeparateSpace`
  - `Past_HouseInteriorMap_SeparateSpace`
  - `Current_HouseExteriorMap_SeparateSpace`
  - `Past_HouseExteriorMap_SeparateSpace`
  - `Current_CentralPlazaMap_SeparateSpace`
  - `Past_CentralPlazaMap_SeparateSpace`
  - `Current_LibraryMap_SeparateSpace`
  - `Past_LibraryMap_SeparateSpace`
- The same scene serializes one `Anemora.FastVS.FastVsHouseAreaVisibility` component with fields for the eight area roots and `activeArea: 0`.
- `Assets/Scripts/FastVS/FastVsHouseAreaVisibility.cs` defines the single `FastVsHouseArea` enum and toggles the root GameObjects through `ApplyVisibility`.
- `ProjectSettings/EditorBuildSettings.asset` currently references `Assets/Scenes/Anemora_Main.unity`; the FastVS HouseSlice player build path is generated separately through `AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`.

Adopted implementation direction:

- Add one scene-level `AnemoraSunCycleDriver`.
- Add one scene-level or area-visibility-coupled `MapSunAnchor` path for Phase A, then call `MapSunAnchor.SetPresetAtRuntime()` from the existing area transition path.
- Because this is single-scene area management, Phase A does not need one anchor per separate scene.

## LUT Import

Imported four 1024x32 LUT PNG files under `Assets/Art/LUT/`:

- `LUT_Morning_Warm.png`
- `LUT_Daylight.png`
- `LUT_GoldenHour.png`
- `LUT_Night_CoolBlue.png`

Source:

- `Philipp-Seifried/Unity-PostProV2-User-LUT-And-Z-Grading`
- License: MIT
- License entry: `docs/THIRD_PARTY_LICENSES.md`

These are MIT Unity-compatible sample LUTs renamed to the Phase A preset slots. They are a starting point for the dynamic pipeline and not a final visual-quality judgment against the HD-2D references.

## Build Artifact For Tom

Build exe path for review:

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。

## Gate Note

変更を適用しました。参考画像とのギャップは、まだ Phase A Step 0 のため動的太陽・光柱・暖寒対比の差分が未実装です。Tom 判定は Phase A の実装・capture 完了後にお願いします。
