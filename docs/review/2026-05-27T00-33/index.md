# Stage7u: Sprite Card Cutout Depth

Branch: `work/fast-vs-hd2d-shading-foundation-20260522`

Stage7u closes the sprite-card rendering path gap from the HD-2D implementation plan: Niro/Reto/Aria and vegetation sprite cards now use a lit alpha-cutout shader with depth writing and ShadowCaster participation. This is a technical rendering checkpoint, not a final visual judgment.

## Build

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

Launch the whole folder: **フォルダごと起動**

`C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice`

## Capture

Internal capture output:

`C:\Users\maro6\OneDrive\work\projects\anemora_reference\reference\20260525_stage7u_sprite_card_cutout_depth`

This public review copy intentionally excludes external target reference images, comparison boards, and route-close obstruction diagnostics.

## Verification

- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHd2dStage7SpriteCardCutoutDepthBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch`: exit 0.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dStage7SpriteCardCutoutDepthReferenceScreenshotsBatch`: exit 0 with graphics enabled.
- `Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`: `Build Finished, Result: Success.`
- Player smoke: `Logs\stage7u_sprite_card_cutout_smoke.log`, no exception or missing-reference matches.
- Public image candidates were checked for central obstruction; close-route diagnostics were not copied.

## Images

![home](home.png)


![plaza_01](plaza_01.png)

![plaza_02_niro_in_shadow](plaza_02_niro_in_shadow.png)

![library](library.png)

![tw_current_aperture](tw_current_aperture.png)

## Current Gap Evaluation

- Sprite cards now participate in alpha-cutout depth and shadow rendering, but this remains a technical closure.
- The plaza still depends on large, visibly mechanical shadow shapes and planar surfaces.
- The library remains sparse in dense authored prop silhouettes, warm/cool atmospheric separation, and painterly material response.
- The TimeWindow aperture is visible, but still reads as a flat technical overlay with hard boundaries.
- Overall HD-2D depth, material richness, fog/depth staging, and composition remain substantially below the target.

Judgment pending. Work continues until an explicit stop instruction.
