# Stage7n: Plaza Shadow Soft Balance

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Stage 7n: Central Plaza の realtime shadow / receiver を Stage7m より柔らかい値へ寄せた。古い cycle146/153/158/161/162 validation は、Stage7n の softened receiver property block を数える gate に更新した。

Build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

起動時は `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice` フォルダごと使う。

Tom 撮影依頼: 5 エリアスクショは `C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7_plaza_shadow_soft_balance\` に保存済み。必要時は同名フォルダへ上書き撮影。

## Images

- [home](home.png)
- [Home_outside](Home_outside.png)
- [plaza_01](plaza_01.png)
- [plaza_02_niro_in_shadow](plaza_02_niro_in_shadow.png)
- [library](library.png)
- [plaza_shadow_soft_balance_close](plaza_shadow_soft_balance_close.png)
- [tw_current_aperture](tw_current_aperture.png)

## Verification

- Validate: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7PlazaShadowSoftBalanceBatch` exit 0.
- Capture: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7PlazaShadowSoftBalanceReferenceScreenshotsBatch` exit 0.
- Build: `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch` exit 0.
- Smoke: `Anemora_FastVS_HouseSlice.exe -batchmode -nographics` 20 seconds, `Exception|Error|Failed|NullReference|MissingReference|Assertion` match count 0.
- Renderer Feature: `PortalStencilFeature`, Stage7 TiltShift, Stage7 Outline active in `UniversalRenderPipeline_Renderer.asset`.
- TimeWindow aperture: `tw_current_aperture.png` manually viewed; not black. The aperture border and window/light-strip values remain visually harsh.
- Chapter1 pipeline: `Assets/Scenes/Anemora_Chapter1.unity` absent; `AnemoraChapter1SceneSetup.cs` / integrator not touched.

## Current Gap

- Internal target references still show much stronger terrain layering, atmospheric depth, localized warm emissive pools, foliage silhouettes, and sun/shadow material response than this public Stage7n capture set.
- Plaza facade relief is visible but shallow. The surface reads as tiled primitives rather than carved architecture or hand-authored HD-2D geometry.
- Route/glow pads and white strips still draw more attention than intended material highlights.
- TimeWindow aperture is functional, but the orange frame, white window panels, and vertical light strip are still far from the reference look.

判定待ち。
