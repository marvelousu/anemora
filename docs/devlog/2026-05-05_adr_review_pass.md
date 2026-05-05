# ADR review pass (2026-05-05)

## 1. 概要

Stage 3 Day 1 の ADR 改訂後に、ADR-0001 から ADR-0009 までを通読し、cross-reference / Status / 用語 / 前提 / 形式の整合性を確認した。

対象 ADR:

| ADR | ファイル | Status |
|---|---|---|
| ADR-0001 | `docs/adr/0001-engine-unity6.3-lts.md` | Accepted |
| ADR-0002 | `docs/adr/0002-time-frame-portal-stencil.md` | Accepted |
| ADR-0003 | `docs/adr/0003-asset-pipeline.md` | Accepted |
| ADR-0004 | `docs/adr/0004-project-directory-structure.md` | Accepted |
| ADR-0005 | `docs/adr/0005-time-management-scene-switching.md` | Accepted |
| ADR-0006 | `docs/adr/0006-save-system.md` | Accepted |
| ADR-0007 | `docs/adr/0007-ui-framework-ugui.md` | Accepted |
| ADR-0008 | `docs/adr/0008-localization.md` | Accepted |
| ADR-0009 | `docs/adr/0009-asset-pipeline.md` | Proposed |

件数は発見箇所の行数ではなく、同種の不整合をまとめた finding group 単位で数えた。

## 2. 整合性 check 結果

| ADR | Cross-reference | Status | 用語 | 前提 | 形式 | 結果 |
|---|---|---|---|---|---|---|
| ADR-0001 | OK | OK | OK | OK | 要修正 | 改訂履歴が無かったため追加 |
| ADR-0002 | 要修正 | OK | OK | 次回候補あり | OK | ADR-0004 参照の旧パスと起草中表記を更新。専用カメラ前提は ADR-0005 v1.1 の Main Camera 方針とずれがあるため §4 に記録 |
| ADR-0003 | 要修正 | OK | OK | OK | OK | ADR-0004 参照の `Assets/Audio/BGM/` を `Assets/Audio/Music/` に更新 |
| ADR-0004 | OK | OK | 要修正 | OK | 要修正 | `Assets/Scripts/Game/` と `Anemora.Data` / `Anemora.Game` asmdef 現状を反映し、改訂履歴見出しを統一 |
| ADR-0005 | 要修正 | OK | OK | 要修正 | OK | ADR-0006 / ADR-0004 の旧ラベルを更新し、別カメラ描画の古い説明を Main Camera + Stencil layer mask へ統一 |
| ADR-0006 | 要修正 | OK | OK | OK | 要修正 | ADR-0004 の起草中表記を更新し、改訂履歴を追加 |
| ADR-0007 | 要修正 | OK | 要修正 | OK | 要修正 | ADR-0004 / ADR-0008 / ADR-0005 の旧ラベルを更新し、TMP font finality 表現を TBD tracking と整合。改訂履歴を追加 |
| ADR-0008 | OK | 要修正 | 要修正 | OK | 要修正 | Stage 3 A1 導入状態、`com.unity.localization@1.5.9`、§5 重複見出しを整理 |
| ADR-0009 | OK | OK | OK | OK | OK | Status: Proposed はユーザー承認待ちとして妥当。ADR-0003 / 0004 / 0007 / 0008 参照も有効 |

finding group breakdown:

| 観点 | 件数 | 本 commit の扱い |
|---|---:|---|
| Cross-reference | 5 | 全件修正 |
| Status | 1 | 修正 |
| 用語 / 概念 | 3 | 全件修正 |
| 前提の更新漏れ | 2 | 1 件修正、1 件を §4 に記録 |
| 形式 | 5 | 全件修正 |
| 合計 | 16 | 15 件修正、1 件は次回改訂候補 |

補足:

- `Reaper` は ADR-0003 v1.1 の改訂履歴に「Reaper → Studio One 統一」として残るのみで、現行方針の DAW 表記は Studio One に統一済み。
- `起草中` は ADR-0006 v1.1 の改訂履歴に「起草中表記を更新」として残るのみで、現行 cross-reference の状態表現としては残っていない。

## 3. 適用した修正

- `docs/adr/0001-engine-unity6.3-lts.md`
  - `## 改訂履歴` を追加。
- `docs/adr/0002-time-frame-portal-stencil.md`
  - ADR-0004 参照の旧ディレクトリ (`Assets/Scripts/TimeFrame/`, `Assets/Shaders/Portal/`, `Assets/Renderer/`) を現行配置 (`Assets/Scripts/TimeManagement/Portal/`, `Assets/Art/Materials/Portal/`, `Assets/Settings/`) へ更新。
  - ADR-0004 / ADR-0005 / ADR-0007 の「起草中」「今後起草」表記を削除。
- `docs/adr/0003-asset-pipeline.md`
  - ADR-0004 参照の audio path を `Assets/Audio/Music/` へ更新。
- `docs/adr/0004-project-directory-structure.md`
  - `Assets/Scripts/Game/` を追加し、DialogueAsset ScriptableObject の配置先を ADR-0008 と整合。
  - asmdef 境界を「未導入」から `Anemora.Data` / `Anemora.Game` の selective introduction に更新。
  - `Revision History` を `改訂履歴` に統一し、v1.2 を追加。
- `docs/adr/0005-time-management-scene-switching.md`
  - ADR-0006 / ADR-0004 の旧ラベルを削除。
  - ポータル描画の説明を「別カメラ」から Main Camera + Stencil layer mask 方針へ統一し、v1.2 を追加。
- `docs/adr/0006-save-system.md`
  - ADR-0004 の旧ラベルを削除。
  - `## 改訂履歴` を追加。
- `docs/adr/0007-ui-framework-ugui.md`
  - ADR-0004 / ADR-0008 / ADR-0005 の旧ラベルを削除。
  - TMP font strategy の確定表現を、`docs/STAGE3_TBD_RESOLUTION.md` で最終採用 tracking する表現に更新。
  - `## 改訂履歴` を追加。
- `docs/adr/0008-localization.md`
  - Status を Stage 3 A1 の DialogueAsset / package baseline 着手状態へ更新。
  - Unity Localization Package を `com.unity.localization@1.5.9` に固定。
  - §5 の重複した `5.2 命名整合` を整理し、v0.3 を追加。

## 4. 次回改訂候補 (本 commit では未修正)

- ADR-0002 v1.2 候補:
  - ADR-0002 の Decision には「ポータル内側は 1 つの専用カメラ」「ポータル内側カメラ」という説明が残る。
  - ADR-0005 v1.1 / v1.2 では VS 実装として `Camera_Past` を使わず、Main Camera の culling mask と `PortalStencilFeature.SetLayerMasks()` で反転する方針が明記済み。
  - これは軽微な用語差し替えではなく、ADR-0002 の描画モデル説明を v1.2 として再整理する候補。専用カメラ案を Stage 4 以降の alternatives / future option として残すか、VS 実装に合わせて Decision を明確化するかは別タスクで扱う。

新規 ADR が必要な領域は今回の review では見つからなかった。

## 5. 結論

ADR-0001 から ADR-0009 までの review pass を完了した。軽微な cross-reference / 用語 / 形式 / 前提表現の不整合は本 commit で修正済み。ADR-0002 の専用カメラ説明と ADR-0005 の Main Camera 方針の関係だけは Decision レベルの明確化候補として §4 に残した。

ADR-0009 は Status: Proposed のまま維持する。ユーザー承認後に Accepted へ昇格する運用と整合している。
