# HD2D Phase A Shader Lightening Diagnostics

- Surface shader: `Assets\Art\Shaders\FastVS\FastVS_SurfaceRampLit.shader`
- Sprite-card unlit shader: `Assets\Art\Shaders\FastVS\FastVS_SpriteCardRampUnlit.shader`
- Validate entry: `ValidateHd2dPhaseAShaderLighteningBatch`
- Capture entry: `CaptureHd2dPhaseAShaderLighteningCycle171ScreenshotsBatch`
- Grep: Surface shader contains `Cull Back`: True
- Grep: Surface shader contains `Cull Off`: False
- Grep: Surface shader `MainLightRealtimeShadow(` count: 0
- Grep: Surface shader `TransformWorldToShadowCoord(` count: 1
- Grep: Sprite-card unlit shader `Cull Off` count: 2
- Grep: Sprite-card unlit shader contains `Cull Back`: False
- Remaining ownership: URP `GetMainLight` provides the main-light shadow attenuation; sprite-card unlit culling remains unchanged for two-sided cards.
