# Anemora

> HD-2D 探索アクション・アドベンチャー。衰退する街で「時の窓」を開き、過去で取った小さな行動が現在の風景を変える、短い循環の体験を目指しています。

## 開発状況（2026-09-05）

**Anemora は長期プロジェクトとして、現在は開発を凍結しています。** 今後は Unreal Engine（UE）への移行を予定しています。開発再開・移行・リリースの時期は未定です。

このリポジトリでは、これまでに制作した Unity 版のプロトタイプと設計資料・開発記録を公開しています。以下のプレイ可能ビルド、技術情報、セットアップ手順は、この Unity 版に関するものです。

> 2026-05-18 時点の Fast VS 公開ベースラインは `vs-public-2026-05-18` です。Chapter 1 のニロの家、外、広場、図書館、時の窓、レトの本イベントまでを確認できます。

> これまでの開発資料（各 work branch の screenshots / docs / Markdown）を閲覧できる iPhone / PC 対応ビューア: <https://anemora-viewer.pages.dev/> 。詳細は [`docs/review/README.md`](docs/review/README.md)。

## Fast VS を遊ぶ（Windows）

最新のプレイ可能ビルドは GitHub の Releases から入手できます。

▶ **[最新リリースをダウンロード](https://github.com/marvelousu/anemora/releases/latest)**

zip をダウンロードして展開し、フォルダ内の `Anemora_FastVS_HouseSlice.exe` を実行します。署名なしビルドのため、Windows SmartScreen の警告が出たら「詳細情報」→「実行」で起動できます。

配布した Windows ビルドを受け取っている場合も、展開したフォルダ内の `Anemora_FastVS_HouseSlice.exe` を実行してください。`.exe` だけを別の場所へ移動せず、同じ階層の `Anemora_FastVS_HouseSlice_Data/`、`UnityPlayer.dll`、`MonoBleedingEdge/`、`D3D12/` と一緒に置いたまま起動します。

ローカルのビルド環境で直接確認する場合のパスは次のとおりです。

```text
Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe
```

ローカルのビルド環境には、リリース添付用の zip も生成済みです。

```text
Builds/Anemora_FastVS_HouseSlice_20260518.zip
```

`Builds/` は Git の管理対象外のため、リポジトリのクローンに動くビルドは含まれません。配布用ビルドは [Releases](https://github.com/marvelousu/anemora/releases/latest) で公開しています。

### 操作

| 操作 | 入力 |
|---|---|
| 移動 | `WASD` / 矢印キー |
| インタラクト / テキスト送り | `E`、`Space`、`Enter` |
| マップ間の移動 | 光る床パッドの上を歩く |
| 時の窓を生成 | レトのイベントで解放後、画面を左ドラッグ |
| 時の窓を閉じる | ニロが現在側にいるとき右クリック または `Esc` |

想定ルート:

```text
ニロの家(室内) → 家の外 → 中央広場 → 図書館 → レト / 本 / 時の窓イベント → VS クリア
```

## リポジトリの読みどころ

| 目的 | 推奨の入口 |
|---|---|
| **企画意図とゲーム体験を 5 分で把握** | [`docs/PITCH_PUBLIC.md`](docs/PITCH_PUBLIC.md)（Steam 説明文 / トレーラー / プレスキット用） |
| **コンセプト設計のロジックと議論経緯** | [`CONCEPT.md`](CONCEPT.md) + [`docs/devlog/2026-05-04_stage1_concept_dialogue.md`](docs/devlog/2026-05-04_stage1_concept_dialogue.md) |
| **業務粒度の企画書（10 章、AI 主体開発の運用ポリシー含む）** | [`PITCH.md`](PITCH.md)（§8 AI 主体ソロ制作パイプライン を推奨） |
| **技術仕様と Vertical Slice の完成条件** | [`SPEC.md`](SPEC.md)、[`docs/VS_SCOPE.md`](docs/VS_SCOPE.md) |
| **アーキテクチャ判断の根拠** | [`docs/adr/`](docs/adr/) |
| **日々の意思決定と検証ログ** | [`docs/devlog/`](docs/devlog/)（`INDEX.md` 経由が読みやすい） |
| **コードとシーン構造のナビゲーション** | [`docs/scene_tour_anemora_main.md`](docs/scene_tour_anemora_main.md)、[`docs/ASSET_STRUCTURE.md`](docs/ASSET_STRUCTURE.md) |
| **アセット出典 / ライセンス / AI 開示分類** | [`NOTICES.md`](NOTICES.md)、[`docs/legal/asset_ledger.md`](docs/legal/asset_ledger.md) |

## ステータス

| 項目 | 状態 |
|---|---|
| 開発ステージ | 長期プロジェクトとして開発凍結中（2026-09-05 時点） |
| 今後の方針 | Unreal Engine への移行を予定。開発再開・移行時期は未定 |
| プレイ可能状態 | Unity 版 Fast VS を [Releases](https://github.com/marvelousu/anemora/releases/tag/vs-public-2026-05-18) で公開済み |
| 現行 Unity / URP | `6000.3.14f1` / URP `17.3.0` |
| 公開リリース計画 | 時期は未定。従来の Steam Early Access 計画は開発再開時に見直し |
| コードライセンス | 既定で All Rights Reserved（Stage 4 入口で再評価） |
| 仮名称 | 主人公 Niro / ニロ、第 1 ゾーン Antela / アンテラ（Stage 4 レビューまで仮） |

## コア機能（設計目標）

- **時の窓 (Time Frame Portal)**: 空中にフレームを描き、3 つのシンボルから選んで過去 / 現在を覗くポータル機構。境界を踏み越えると映った時間に入る挙動を目指しています。
- **行動記録 (ActionRecord)**: 過去で取った行動（拾う・話す・到達）を記録し、戻った後の現在側の風景や進行に反映する仕組み。
- **第 1 ゾーン Antela**: 衰退した街、主人公の家、図書館跡を含む Vertical Slice エリア。10〜15 分の通し体験を想定しています。
- **日英ローカライズ基盤**: TextMeshPro SDF Atlas + LocalizationSettings + Addressables による日本語 / 英語切替。

## 技術スタック（現行の Unity 版）

| 領域 | 採用 |
|---|---|
| エンジン | Unity `6000.3.14f1` + URP `17.3.0` |
| 言語 | C#（`Anemora.Data` POCO、`Anemora.Save`、`Anemora.Game` を asmdef 分離） |
| 2D アセットパイプライン | PixelLab + Aseprite |
| 3D アセットパイプライン | Meshy v6 + Blender 4.5 LTS |
| オーディオパイプライン | AIVA + Suno + ElevenLabs SFX + Stable Audio + Studio One |
| ローカライズ | TMP SDF Atlas（日本語: DotGothic16 / 英語: Press Start 2P）+ LocalizationSettings |
| ソース管理 | Git + GitHub（feature ごとに worktree を切るワークフロー） |

## 技術的な要点

Fast VS は、旧 `Anemora_Main` の統合状態とは別に、時の窓の挙動をベースに最短で遊べる形へまとめた公開用スライスです。

| 領域 | 参照先 |
|---|---|
| Fast VS 生成シーン | `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` |
| シーン生成 / ビルド検証 | `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs` |
| 時の窓コントローラ | `Assets/Scripts/TimeManagement/TimeWindowPairedSpacePortalController.cs` |
| Fast VS のルート / マップ切替 | `Assets/Scripts/FastVS/FastVsHouseAreaVisibility.cs`、`Assets/Scripts/FastVS/FastVsAreaDoorTransition.cs` |
| Fast VS ストーリーフロー | `Assets/Scripts/FastVS/FastVsStoryFlowController.cs` |
| Devlog 索引 | [`docs/devlog/INDEX.md`](docs/devlog/INDEX.md) |
| 公開ベースライン記録 | [`docs/devlog/2026-05-18_fast_vs_public_repo_promotion.md`](docs/devlog/2026-05-18_fast_vs_public_repo_promotion.md) |
| 現ビルドの検証ログ | `Logs/fast_vs_build_validate_20260518_skip_opening_wake_line.log` |

Unity Editor で再生成 / 再ビルドする場合は、Unity `6000.3.14f1` でプロジェクトを開き、メニューから `Anemora/Fast VS/Create House Slice` または `Anemora/Fast VS/Build House Slice` を実行します。

## 開発体制

Anemora は個人開発プロジェクトで、AI を協働者として扱うパイプラインで運用しています。

| 役割 | ツール | 担当 |
|---|---|---|
| 設計 / 文書 / 最終判断補助 | Claude (Opus 4.7) | `/spec` 対話、ADR 整備、Plan モード、レビュー反映 |
| 実装 / QA / 反復 | Codex CLI | コード実装、テスト生成、独立 QA レビュー（`/codex-qa`） |
| 3D アセット生成・修正 | Blender + Claude MCP | HD-2D 用シーン構築、低ポリ環境 |
| クロスモデルレビュー | Claude / 独立 Claude / Codex の 3 ラウンド | Stage 1 で確立、コンセプト・仕様・レビュー各段階で適用 |

ドキュメント、ADR、devlog、アセット台帳をすべて開発と同じ粒度でコミットしており、判断の経緯と検証結果がリポジトリ内で追えます。

詳細は [`PITCH.md`](PITCH.md) §8（AI 主体ソロ制作パイプライン）を参照してください。

## セットアップ手順

Editor 上で Fast VS を確認したい場合の手順:

1. Unity Hub で Unity `6000.3.14f1` をインストール
2. このリポジトリをクローン
3. Unity Hub でプロジェクトのルートを開く
4. `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` を開く
5. Unity Editor で Play

広い本編側の作業状態を確認したい場合は `Assets/Scenes/Anemora_Main.unity` を開きます。ただし、公開 VS として固定しているのは `Anemora_FastVS_HouseSlice.unity` です。

## ディレクトリ構成

- `Assets/` — Unity プロジェクト本体（Scenes、Scripts、Prefabs、Art、UI、Settings）
- `docs/` — ADR、SPEC、VS_SCOPE、devlog、アセット台帳などの設計・記録文書
- `tools/` — Meshy / Blender 後処理の Python ヘルパー
- `art/_intermediate/`、`audio/_intermediate/` — 生成中間ファイル（gitignore 対象）

## ライセンス

- **コード**: 既定で All Rights Reserved。Stage 4 入口で公開方針と合わせて再評価予定。この README はオープンソースライセンスを付与しません。
- **第三者アセット**: 個別ライセンスは [`docs/legal/asset_ledger.md`](docs/legal/asset_ledger.md) を参照。
- **AI 生成アセット**: 各ツールの有料プランを前提に生成し、商用利用可否と公開可否を `asset_ledger.md` に記録。
- **フォント**: DotGothic16（SIL Open Font License 1.1）、Press Start 2P（SIL Open Font License 1.1）。

## ロードマップ

- **Fast VS 公開ベースライン**（2026-05-18）: ニロの家から図書館、レトの本イベント、時の窓操作までの公開確認版。
- **現在**（2026-09-05）: 長期プロジェクトとして開発を凍結。Unity 版の成果と開発記録を公開・保管。
- **今後**: Unreal Engine への移行を予定。再開時に開発範囲と公開計画を見直します。日程は未定です。

## コントリビューション

現状は単独開発で、開発凍結中です。Issue / Pull Request の受付方針は、開発再開時に公開計画とコードライセンスの再評価に合わせて決定します。
