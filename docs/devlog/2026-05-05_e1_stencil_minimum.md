# E1 Stencil Minimum Devlog (2026-05-05)

## Scope

Phase E1 の最小ポータル描画を Windows Unity で実装した。あわせて E2 の scene root skeleton、E3 の SymbolWheel skeleton、PlayMode smoke test 基盤を同一 batch で追加した。

## Implementation

- `Assets/Scripts/TimeManagement/Portal/PortalStencilFeature.cs`
  - `StencilBit = 3` / `StencilMask = 8`
  - `RenderPassEvent.AfterRenderingOpaques`
  - `AnemoraPortalMask` と `AnemoraPortalInside` の 2 pass を enqueue
  - PlayMode smoke test 用に `LastEnqueueFrame` / `LastEnqueuedPassCount` / `LastCameraName` を保持
- `Assets/Art/Materials/Portal/PortalMask.shader`
  - `Stencil Ref 8`, `ReadMask 8`, `WriteMask 8`
  - `Comp Always`, `Pass Replace`, `ColorMask 0`, `ZWrite Off`
- `Assets/Art/Materials/Portal/InsideOnly.shader`
  - `Stencil Ref 8`, `ReadMask 8`, `WriteMask 8`
  - `Comp Equal`, `Pass Keep`

Note: handover では「Stencil Ref 1 + bit 3」と表現されていたが、Unity shader の `Ref` は stencil 値なので bit 3 は `8` として扱った。

## Sandbox

- Scene: `Assets/Scenes/Sandbox_E1_Stencil.unity`
- Portal mask: `PortalMask_Quad`
- Portal 内部確認用: `InsideOnly_Cube_VisibleThroughPortal`
- 現在側参照物: `Reference_Current_Cube_OutsidePortal`, `Reference_Floor`

## Screenshots

- `docs/devlog/screenshots/e1_portal_front.png`
- `docs/devlog/screenshots/e1_portal_side.png`
- `docs/devlog/screenshots/e1_portal_back.png`

観察結果:

- front: portal quad 越しに inside-only cube が見える
- side/back: inside-only cube は見えず、現在側の床/参照 cube のみが見える
- 現時点の sandbox では Z-fighting は確認なし
- portal mask 自体は `ColorMask 0` なので視覚的には表示されない

## Verification

| Check | Result |
|---|---|
| EditMode tests | Pass: 18/18 |
| PlayMode tests | Pass: 2/2 |
| Windows Standalone build | Success |
| Renderer feature smoke | `PortalStencilFeature` が 2 pass enqueue |
| BuildSettings | `Sandbox_E1_Stencil` / `Anemora_Main` を登録 |

## Caveats

- `PortalStencilFeature` は URP 17 の `DrawObjectsPass` を使うため `UnityEngine.Rendering.Universal.Internal` 参照を含む。E4 で public API の custom pass に戻すかは再判断する。
- 自動 screenshot / smoke test を安定させるため、portal shaders には custom `LightMode` pass に加えて `UniversalForward` pass も置いている。custom-pass-only にしたい場合は E4 で renderer feature と shader の責務を再整理する。
- `Camera_Past` は E2 skeleton として `Layer_Past_Visual` のみを見る状態。E4 の frame crossing 実装時に main/past camera composition を詰める。
