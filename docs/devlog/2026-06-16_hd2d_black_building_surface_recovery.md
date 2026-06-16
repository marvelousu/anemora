# HD2D black building surface recovery

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-16

## Investigation

- The latest built-player all-map capture exposed a regression where exterior building surfaces, especially the Central Plaza library front and the C/F map buildings, rendered as near-black planes.
- The first material-only attempt improved the generated `current_exterior_wall` / `past_exterior_wall` defaults but did not fix the built player, because `FastVsRealtimeLightShadowRig` overrides the surface ramp values through a runtime `MaterialPropertyBlock`.
- A second attempt that re-enabled the old library-front pale wash made the wall readable but restored the previously rejected white haze. That result is explicitly rejected and kept only as comparison evidence.
- The accepted fix keeps the pale wash disabled and narrows the change to the realtime wall/door surface grade: facade walls now use a lower directional response, lower shadow receive strength, lower shadow texture strength, and warmer side/floor shade values. Floors, roads, roofs, props, renderer features, fog, and skybox are not changed by this fix.

## Change

- `FastVsRealtimeLightShadowRig` now detects realtime facade wall receivers separately from the broader facade/roof receiver group.
- Wall and door receivers use a facade-specific runtime grade:
  - `_DirectionalLightStrength` is reduced from the global realtime value so shadowed vertical faces no longer collapse to black.
  - `_ShadowReceiveStrength` and `_ShadowTextureStrength` are reduced for facade walls to keep shadow detail without painting the whole wall black.
  - `_SideShade` and `_FloorShade` are warmed for wall readability without reintroducing the disabled pale wash overlay.
- `AnemoraFastVsHouseSliceSetup` now gives the default exterior-wall materials the same low-directional wall profile, so editor/default material state matches the runtime intent.
- The old `library-front pale wash` renderers remain disabled in player logs. The accepted review image therefore fixes the black wall without restoring the white haze.

## Visual Review

- Accepted packet: `docs/review/2026-06-16T23-40_black_surface_fix_probe_r4/`.
- `03_b1_b3_current.png`: Central Plaza facade no longer renders as a black slab, and the old white wash is absent.
- `05_c1_c3_current.png`: C map house wall remains readable in the wide frame.
- `11_f1_f6_current.png`: F map buildings no longer collapse into black planes.
- `14_facade_regression_triptych.png`: compares the baseline black wall, rejected white-haze attempt, and accepted r4 result.
- Facade ROI luminance for Central Plaza: baseline black `meanY=32.0`, rejected white haze `meanY=78.7`, accepted r4 `meanY=55.8`. This is enough lift to read the wall while staying below the haze-restored exposure.

## Verification

- Validate: `Logs/black_surface_validate_r4.log` passed with `Fast VS house slice validation passed.`
- Build: `Logs/black_surface_build_r4.log` passed and rebuilt `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`.
- Built-player capture: `Logs/black_surface_player_capture_r4.log` passed and wrote 13 PNGs to `docs/review/2026-06-16T23-40_black_surface_fix_probe_r4/`.
- Pale-wash guard: the accepted player log reports `disabled 14 library-front pale wash renderer(s)` for the reviewed areas.
- Renderer freeze: `Logs/black_surface_editmode_r5.xml` passed 36/36 EditMode tests, including `RendererFeatureSet_MatchesFrozenBaseline`.
- Asset validation: `Logs/black_surface_asset_validation_r4.log` passed with `[AssetValidation] OK`.

## Next

- Continue the authored environment uplift from the established graphics plan. The remaining wide frames still need higher-quality authored distant landform meshes, vegetation kits, material density, and lighting contrast.
- Keep bridge art and traversal coupled: any bridge visual replacement should rerun the built-player traversal proof from current/past F1 to F6.
- Keep the Phase 7 publishing rule: every visual cycle needs Validate, renderer-freeze EditMode, AssetValidation, all-map capture, visual review, devlog, review packet with `devlog.txt`, R2 upload, viewer verification, pathspec commit, and push.
