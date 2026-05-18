# Anemora

> HD-2D 探索アクション・アドベンチャー。衰退する街で「時の窓」を開き、過去で取った小さな行動が現在の風景を変える、短い循環の体験を目指しています。

> 2026-05-18 時点の Fast VS 公開 baseline は `vs-public-2026-05-18` です。Chapter 1 のニロの家、外、広場、図書館、時の窓 V24、レトの本イベントまでを確認できます。

## Play the Fast VS

Windows build を受け取っている場合は、展開したフォルダ内の `Anemora_FastVS_HouseSlice.exe` を実行してください。`.exe` だけを別の場所へ移動せず、同じ階層の `Anemora_FastVS_HouseSlice_Data/`, `UnityPlayer.dll`, `MonoBleedingEdge/`, `D3D12/` と一緒に置いたまま起動します。

この作業環境で直接確認する場合:

```text
Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe
```

この作業環境には release 添付用の zip も生成しています。

```text
Builds/Anemora_FastVS_HouseSlice_20260518.zip
```

`Builds/` は Git 管理対象外です。GitHub で公開する場合は、この zip を release asset として添付してください。

### Controls

| Action | Input |
|---|---|
| Move | `WASD` / Arrow keys |
| Interact / advance text | `E`, `Space`, or `Enter` |
| Move between maps | Walk onto glowing floor pads |
| Create Time Window | After the Reto event unlocks it, left-drag on the screen |
| Close Time Window | Right-click or `Esc` while Niro is in the current-time side |

Expected route:

```text
Niro house interior -> house exterior -> central plaza -> library -> Reto / book / Time Window event -> VS clear
```

## このリポジトリの読みどころ

| 目的 | 推奨 entry point |
|---|---|
| **企画意図とゲーム体験を 5 分で把握** | [`docs/PITCH_PUBLIC.md`](docs/PITCH_PUBLIC.md) (Steam description / trailer / press-kit 用) |
| **コンセプト設計のロジックと議論経緯** | [`CONCEPT.md`](CONCEPT.md) + [`docs/devlog/2026-05-04_stage1_concept_dialogue.md`](docs/devlog/2026-05-04_stage1_concept_dialogue.md) |
| **業務粒度の企画書（10 章、AI 主体開発の運用ポリシー含む）** | [`PITCH.md`](PITCH.md) (§8 AI-Driven Solo Production Pipeline 推奨) |
| **技術仕様と Vertical Slice の完成条件** | [`SPEC.md`](SPEC.md), [`docs/VS_SCOPE.md`](docs/VS_SCOPE.md) |
| **アーキテクチャ判断の根拠** | [`docs/adr/`](docs/adr/) |
| **日々の意思決定と検証ログ** | [`docs/devlog/`](docs/devlog/) (`INDEX.md` 経由が読みやすい) |
| **コードとシーン構造の navigation** | [`docs/scene_tour_anemora_main.md`](docs/scene_tour_anemora_main.md), [`docs/ASSET_STRUCTURE.md`](docs/ASSET_STRUCTURE.md) |
| **アセット出典 / ライセンス / AI 開示分類** | [`NOTICES.md`](NOTICES.md), [`docs/legal/asset_ledger.md`](docs/legal/asset_ledger.md) |

## Status

| Item | Status |
|---|---|
| Development stage | Fast VS public baseline fixed at `vs-public-2026-05-18`; post-VS polish continues on branches |
| Playable state | Windows Fast VS build exists locally under `Builds/FastVS_HouseSlice/`; attach the full folder as a release zip for public play |
| Unity / URP | `6000.3.14f1` / URP `17.3.0` |
| Public release path | Steam Early Access を主軸として想定 (時期・条件は Stage 4 review で確定) |
| Code license | All Rights Reserved by default (Stage 4 entry で再評価) |
| Provisional names | 主人公 Niro / ニロ、第 1 ゾーン Antela / アンテラ (Stage 4 review まで provisional) |

## Core Features (設計目標)

- **時の窓 (Time Frame Portal)**: 空中にフレームを描き、3 つのシンボルから選んで過去 / 現在を覗くポータル機構。境界を踏み越えると映った時間に入る、を目指して実装中。
- **行動記録 (ActionRecord)**: 過去で取った行動 (拾う・話す・到達) を記録し、戻った後の現在側の風景や進行に反映する仕組み。
- **第 1 ゾーン Antela**: 衰退した街、主人公の家、図書館跡を含む Vertical Slice エリア。10-15 分の通し体験を想定して制作中。
- **日英ローカライズ基盤**: TextMeshPro SDF Atlas + LocalizationSettings + Addressables による日本語 / 英語切替。

## Tech Stack

| Layer | Choice |
|---|---|
| Engine | Unity `6000.3.14f1` + URP `17.3.0` |
| Language | C# (`Anemora.Data` POCO, `Anemora.Save`, `Anemora.Game` を asmdef 分離) |
| 2D asset pipeline | PixelLab + Aseprite |
| 3D asset pipeline | Meshy v6 + Blender 4.5 LTS |
| Audio pipeline | AIVA + Suno + ElevenLabs SFX + Stable Audio + Studio One |
| Localization | TMP SDF Atlas (JP: 美咲ゴシック / EN: Press Start 2P) + LocalizationSettings |
| Source control | Git + GitHub (worktree-per-feature ワークフロー) |

## Technical Basics

Fast VS は、旧 `Anemora_Main` の統合状態とは別に、V24 の時の窓挙動をベースに最短で遊べる形へまとめた公開用スライスです。

| Area | Entry point |
|---|---|
| Fast VS generated scene | `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` |
| Scene generator / build validation | `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs` |
| Time Window V24 controller | `Assets/Scripts/TimeManagement/TimeWindowPairedSpacePortalController.cs` |
| Fast VS route / map switching | `Assets/Scripts/FastVS/FastVsHouseAreaVisibility.cs`, `Assets/Scripts/FastVS/FastVsAreaDoorTransition.cs` |
| Fast VS story flow | `Assets/Scripts/FastVS/FastVsStoryFlowController.cs` |
| Devlog index | [`docs/devlog/INDEX.md`](docs/devlog/INDEX.md) |
| Public baseline record | [`docs/devlog/2026-05-18_fast_vs_public_repo_promotion.md`](docs/devlog/2026-05-18_fast_vs_public_repo_promotion.md) |
| Verification log for current build | `Logs/fast_vs_build_validate_20260518_skip_opening_wake_line.log` |

Unity Editor で再生成 / 再ビルドする場合は、Unity `6000.3.14f1` でプロジェクトを開き、menu から `Anemora/Fast VS/Create House Slice` または `Anemora/Fast VS/Build House Slice` を実行します。

## Development Model

Anemora は個人開発プロジェクトで、AI を協働者として扱うパイプラインで運用しています。

| Role | Tool | 担当 |
|---|---|---|
| 設計 / 文書 / 最終判断補助 | Claude (Opus 4.7) | `/spec` 対話、ADR 整備、Plan モード、レビュー反映 |
| 実装 / QA / 反復 | Codex CLI | コード実装、テスト生成、独立 QA レビュー (`/codex-qa`) |
| 3D アセット生成・修正 | Blender + Claude MCP | HD-2D 用シーン構築、低ポリ環境 |
| Cross-model review | Claude / 独立 Claude / Codex の 3 ラウンド | Stage 1 で確立、コンセプト・仕様・レビュー各段階で適用 |

ドキュメント、ADR、devlog、asset ledger をすべて開発と同じ粒度で commit しており、判断の経緯と検証結果がリポジトリ内で追えます。

詳細は [`PITCH.md`](PITCH.md) §8 (AI-Driven Solo Production Pipeline) を参照してください。

## Getting Started

Editor 上で Fast VS を確認したい場合の手順:

1. Unity Hub で Unity `6000.3.14f1` をインストール
2. このリポジトリを clone
3. Unity Hub でプロジェクト root を開く
4. `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` を開く
5. Unity Editor で Play

広い本編側の作業状態を確認したい場合は `Assets/Scenes/Anemora_Main.unity` を開きます。ただし、公開 VS として固定しているのは `Anemora_FastVS_HouseSlice.unity` です。

## Directory Layout

- `Assets/` — Unity project 本体 (Scenes, Scripts, Prefabs, Art, UI, Settings)
- `docs/` — ADR, SPEC, VS_SCOPE, devlog, asset ledger 等の設計・記録文書
- `tools/` — Meshy / Blender postprocess の Python helper
- `art/_intermediate/`, `audio/_intermediate/` — 生成中間ファイル (gitignore 対象)

## License

- **Code**: All Rights Reserved by default。Stage 4 入口で公開方針と合わせて再評価予定。この README は open-source license を付与しません。
- **Third-party assets**: 個別ライセンスは [`docs/legal/asset_ledger.md`](docs/legal/asset_ledger.md) を参照。
- **AI 生成アセット**: 各ツールの paid plan を前提に生成、商用利用可否と公開可否を `asset_ledger.md` に記録。
- **Fonts**: 美咲ゴシック (フリー使用許諾)、Press Start 2P (SIL Open Font License 1.1)。

## Roadmap

- **Fast VS public baseline** (2026-05-18): ニロの家から図書館、レトの本イベント、時の窓 V24 操作までの公開確認版。
- **Post-VS polish**: graphics polish、Steam Early Access 提出準備、public-facing docs / store assets の整備。
- **Stage 5+**: Steam Early Access feedback / full release planning.

## Contributing

現状は単独開発です。Issue / Pull Request の受付方針は Stage 4 以降に Steam Early Access 予定と code license 再評価に合わせて決定します。
