# A2 Anemora_Main wiring + boundary round-trip (2026-05-05)

## Scope

E4 で未実施だった `Anemora_Main` 実シーンへの portal runtime wiring と、Current / Past 境界往復確認を実施した。

## Scene wiring

- `Assets/Scenes/Anemora_Main.unity`
  - `Player`
    - `tag = Player`
    - root layer: `Layer_Current_Collider` (8)
    - `PrototypePlayerController` を追加
    - Current / Past 表示確認用の visual child をそれぞれ `Layer_Current_Visual` (10) / `Layer_Past_Visual` (11) に配置
  - `PortalSpawnPoint`
    - position: `(0, 0.9, -0.25)`
    - normal: `Vector3.back`
    - Player 開始位置から W 方向で Past 側、S 方向で Current 側へ戻る向き
  - `SymbolWheel`
    - `Assets/UI/Prefabs/SymbolWheel.prefab` instance
    - Screen Space - Camera / Main Camera / UI layer
  - `TimeFramePortalSystem`
    - `PortalCrossingDetector`
    - `SceneSidePolarity`
    - `PortalVisualSwitcher`
    - `Volume`
    - `PortalFlashPlayer`
    - `TimeFramePortalController`
    - `Portal_Frame.prefab` / `PortalFlash_VolumeProfile.asset` / `PortalStencilFeature` を実参照で接続

## Runtime support

- `Assets/Scripts/Player/PrototypePlayerController.cs`
  - VS 用の最小 WASD / arrow-key mover
  - Camera yaw 基準で水平移動
  - `Time.deltaTime` 駆動のため portal generation 中 (`Time.timeScale = 0`) は移動しない

## Boundary round-trip verification

- `Assets/Tests/PlayMode/AnemoraMainPortalWiringRoundTripTests.cs`
  - `Anemora_Main` を実ロード
  - `Player` / `SymbolWheel` / `TimeFramePortalController` / `PortalCrossingDetector` / `SceneSidePolarity` / `PortalVisualSwitcher` / `PortalFlashPlayer` の scene wiring を確認
  - Red symbol 選択で portal open
  - Player を portal plane の Past 側へ移動し、`Current -> Past` flip を確認
  - Player を Current 側へ戻し、`Past -> Current` flip を確認
  - 各 flip 後に Main Camera culling mask と Player collision layer が期待値に戻ることを確認

## Visual evidence

Batch render で以下を生成し、Current / Past の culling 差分を目視確認した。これは editor-side render のため、portal runtime open 状態そのものではなく、A2 の実シーン表示レイヤー切替の証跡として扱う。

- `docs/devlog/screenshots/a2_main_current_open.png`
- `docs/devlog/screenshots/a2_main_past_after_cross.png`
- `docs/devlog/screenshots/a2_main_current_after_return.png`

観察:

- Current: current floor / bed / current player visual が表示される
- Past: past floor / table / book / NPC placeholder / past player visual が表示される
- Return: current 表示へ戻る

Note: screenshot capture 時に URP RenderGraph compatibility warning が出る。既存 E1/E4 の `PortalStencilFeature` caveat と同じ内容で、今回の wiring test は runtime state / camera mask / layer flip を正として確認した。

## Verification

- PlayMode: `11/11 passed`
  - includes `AnemoraMainPortalWiringRoundTripTests.MainScenePortalWiringSupportsBoundaryRoundTrip`
- EditMode: `25/25 passed`

## Known limitations

- `PrototypePlayerController` は VS 用の暫定 mover。F/G track の正式 player controller / animation wiring 後に置換対象。
- Portal frame 自体の見た目は E1/E4 の stencil caveat を継続。A2 は実シーン wiring と境界往復の成立確認に限定。
