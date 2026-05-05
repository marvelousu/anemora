# Anemora

## Project Overview

Anemora は、衰退する街で過去と現在を行き来する HD-2D 探索アクション・アドベンチャーです。Stage 3 の Vertical Slice では、Niro (ニロ、provisional) が第 1 ゾーン Antela (アンテラ、provisional) で時の窓を開き、街の過去を覗き、そこで取った行動が現在側の風景や進行に反映される体験を最小範囲で実装します。

## Status

| Item | Status |
|---|---|
| Development stage | Stage 3: Vertical Slice in progress |
| Unity | 6000.3.14f1 |
| Render pipeline | URP 17.3.0 |
| Public release | Steam Early Access planned |
| Code license | All Rights Reserved by default; Stage 4 で再評価 |
| Protagonist name | Niro / ニロ (provisional) |
| First zone name | Antela / アンテラ (provisional) |

## Core Features

- 時の窓 (Time Frame Portal): シンボル選択で過去 / 現在の境界を開くポータル機構。
- 行動記録 (ActionRecord): 過去で取った行動を記録し、現在側の状態に反映する仕組み。
- 第 1 ゾーン Antela: 衰退した街、Niro の家、図書館跡を含む Vertical Slice 用エリア。
- 日英ローカライズ基盤: TextMeshPro の事前生成 SDF Atlas を使用。日本語は美咲ゴシック、英語は Press Start 2P を Stage 3 provisional として採用。

## Getting Started

1. Unity Hub で Unity `6000.3.14f1` をインストールします。
2. このリポジトリを clone します。
3. Unity Hub で clone した `anemora/` project を追加して開きます。
4. `Assets/Scenes/Anemora_Main.unity` を開きます。
5. Unity Editor の Play で実行します。

## Directory Layout

- `Assets/`: Unity project 本体。Scenes、Scripts、Prefabs、Art、UI、Settings を含みます。
- `docs/`: ADR、SPEC、VS_SCOPE、devlog、asset ledger などの設計・記録文書。
- `tools/`: Meshy / Blender postprocess などの Python helper。
- `art/_intermediate/`: 生成候補や作業途中アセット置き場。gitignore 対象です。
- `audio/_intermediate/`: 音源候補や作業途中 audio 置き場。gitignore 対象です。

## Tech Stack

- Engine: Unity `6000.3.14f1`, URP `17.3.0`, TextMeshPro。
- Languages: C#。`Anemora.Data` POCO、`Anemora.Save`、`Anemora.Game` を asmdef で分離。
- 2D asset pipeline: PixelLab + Aseprite。
- 3D asset pipeline: Meshy + Blender。
- Audio pipeline: AIVA + Suno + ElevenLabs + Stable Audio + Studio One。
- Localization: TMP SDF Atlas。JP は美咲ゴシック、EN は Press Start 2P provisional。

## License

- Code: All Rights Reserved by default。Stage 4 入口で公開方針と合わせて再評価します。
- Third-party assets: 個別ライセンスは [`docs/legal/asset_ledger.md`](docs/legal/asset_ledger.md) を参照してください。
- AI-generated assets: 各ツールの paid plan を前提に生成し、商用利用可否と公開可否を `asset_ledger.md` に記録します。
- Fonts: 美咲ゴシックはフリー使用許諾、Press Start 2P は SIL Open Font License 1.1。

## Roadmap

- Stage 3: Vertical Slice。時の窓、ActionRecord、第 1 ゾーン Antela の最小体験を実装中。
- Stage 4: 第 2 ゾーン拡張、ローカライズ完備、Steam Early Access 提出準備。
- Stage 5+: Steam Early Access feedback / full release planning.

## Contributing

現状は単独開発です。Issue / Pull Request の受付方針は、Stage 4 以降に Steam Early Access 予定と code license 再評価に合わせて決定します。

## References

- [`docs/VS_SCOPE.md`](docs/VS_SCOPE.md): Stage 3 Vertical Slice の完成条件。
- [`docs/STAGE3_PLAN.md`](docs/STAGE3_PLAN.md): Stage 3 の実行計画。
- [`docs/adr/`](docs/adr/): Architecture Decision Records。
- [`docs/devlog/`](docs/devlog/): 制作日誌。
- [`docs/legal/asset_ledger.md`](docs/legal/asset_ledger.md): アセット出典・ライセンス・公開可否の記録。
