# ADR-0009: Anemora アセット制作パイプライン正式手順

## Status

Accepted

## Date

2026-05-05 (Stage 3 Day 1)

## Context

ADR-0003 は AI 主体 + 人手仕上げのアセット制作方針を定義した。Stage 3 Day 1 では、その方針を実運用に移し、PixelLab + Aseprite、Meshy + Blender + Unity、TextMeshPro Atlas、音響プロンプト整備、`asset_ledger.md` 台帳運用を並列セッションで進めた。

この過程で、単なるツール選定ではなく、**生成 → 中間処理 → 最終 import** の責務分離、gitignore された中間ファイルの扱い、Unity import 設定、Steam AI 開示区分、並列 dirty 混在を避ける staging 規律が必要になった。

本 ADR は ADR-0003 の上位方針を置き換えず、Stage 4 以降の制作・保守・新規 contributor 参加時に参照する **実務手順 ADR** として正式化する。

Stage 3 /spec resolution interview (2026-05-05) でユーザー承認を受け、Status を Proposed から Accepted へ昇格した。

### 関連文書

- ADR-0003: AI 主体 + 人手仕上げのアセットパイプライン方針
- ADR-0004: Unity project directory structure
- ADR-0007: UI フレームワーク
- ADR-0008: ローカライズ実装方針
- `docs/legal/asset_ledger.md`: 生成アセットの権利・出典・Steam 開示台帳
- `docs/asset_prompts/`: BGM / SFX / sprite / 3D 生成プロンプト

### 制約

- GitHub Public に出すのは最終成果物と検証可能な手順であり、AI 生成 raw / DAW song / stem WAV / Meshy raw export 等の中間ファイルは公開対象外
- Unity main project には他セッション由来の dirty / untracked が混在しやすい
- Steam 提出時には player-consumed AI 生成物を台帳ベースで説明できる必要がある
- Stage 3 の Vertical Slice は速度優先だが、Stage 4 以降に同じ手順を再現できる必要がある

---

## Decision

### 1. Pipeline 全体構成

すべてのアセット制作を **生成 → 中間処理 → 最終 import** の 3 段階に分ける。

| 段階 | 目的 | 保管 / commit 方針 |
|---|---|---|
| **生成** | AI / external tool から候補を大量に出す | raw は `art/_intermediate/` または `audio/_intermediate/` に保存し、gitignore |
| **中間処理** | Aseprite / Blender / Studio One / Unity temp project で修復・比較・書き出し | working file は gitignore。検討ログは `docs/devlog/` に残す |
| **最終 import** | Unity main project に取り込み、prefab / material / import settings を確定 | 最終 `.png` / `.fbx` / `.ogg` / `.asset` / `.prefab` のみ `Assets/` 配下へ commit |

中間処理で「人手仕上げ」を行ったことは、`asset_ledger.md` の `手修正` 列と devlog に記録する。最終成果物だけを commit しても、制作過程が説明できるようにする。

### 2. ツール分担と必要プラン

| 種別 | 生成 | 中間処理 / 仕上げ | 必要プラン / 備考 |
|---|---|---|---|
| **2D character / object** | PixelLab | Aseprite | PixelLab Pixel Apprentice paid plan、Aseprite は購入済み |
| **3D building / prop** | Meshy v6 paid | Blender 4.5.5 LTS、Unity prefab 化 | Meshy は credits 制 paid plan。Blender で scale / origin / material / collider / mesh 破綻を修復 |
| **Audio BGM** | AIVA Pro + Suno paid + Stable Audio | Studio One | 役割分担は ADR-0003 / `docs/asset_prompts/bgm_zone1_ambient.md` に従う |
| **Audio SFX** | ElevenLabs / Stable Audio | Studio One | 詳細は `docs/asset_prompts/sfx_zone1.md`。Voice 用途とは分離 |
| **Font** | Google Fonts OFL など商用可フォント | Unity Editor TMP Atlas | TMP Atlas 化は Unity Editor 機能。OFL 等の license は `asset_ledger.md` に記録 |
| **UI asset** | 手作業 / 必要時 AI 補助 | Unity uGUI / Aseprite / TMP | ADR-0007 / ADR-0008 と整合させる |

ツールの plan / receipt / invoice excerpt は、生成時点の `asset_ledger.md` と必要に応じて `docs/devlog/` に要約する。個人情報や決済アカウント詳細は commit しない。

### 3. 中間ファイル保管

公開対象外の中間ファイルは以下に置く。

| パス | 内容 | Git 方針 |
|---|---|---|
| `art/_intermediate/` | PixelLab raw、Aseprite working、Meshy export FBX raw、比較用 screenshot | `.gitignore` 済み |
| `audio/_intermediate/` | DAW song file、stem WAV、AI audio raw、音量比較用 export | `.gitignore` 済み |
| `3d/_intermediate/` | 3D tool raw、変換途中の mesh、検証 export | `.gitignore` 済み |

中間ファイルの保存目的は再検討・再生成・台帳確認であり、GitHub Public での透明性は devlog / prompt / ledger で担保する。

### 4. 最終アセット配置 convention

最終成果物は ADR-0004 の directory layout に従い、以下へ配置する。

| 種別 | 配置 |
|---|---|
| Sprite / palette / material / font-adjacent art asset | `Assets/Art/` |
| Character sprite | `Assets/Art/Sprites/<Actor>/<version>/` |
| Building / prop prefab | `Assets/Prefabs/Zone1/` など zone 別 prefab directory |
| 3D model / material | `Assets/Art/Models/`、`Assets/Art/Materials/` |
| BGM | `Assets/Audio/Music/` |
| SFX | `Assets/Audio/SFX/` |
| UI prefab / UI sprite | `Assets/UI/Prefabs/`、`Assets/UI/Sprites/` |
| TMP font asset / atlas | `Assets/UI/Localization/Fonts/` |

アセット名は用途・zone・version が分かるようにし、AI tool の内部ファイル名をそのまま最終名にしない。最終 import 後は prefab / scene 参照が安定するため、rename は必要最小限にする。

### 5. Unity import 設定の標準

Stage 3 で確定した import settings の初期標準を以下にする。例外は devlog に理由を残す。

#### 5.1 2D sprite

- Pixels Per Unit: **32**
- Filter Mode: **Point**
- Mip Maps: **off**
- Alpha is Transparency: **on**
- Compression: sprite の見た目が壊れる場合は none / lossless 寄りを優先

#### 5.2 3D mesh

- Read/Write: **off** (runtime で mesh 改変しない限り)
- Generate Lightmap UVs: **必要時のみ on**
- Material Creation Mode: **Standard**
- Scale / origin / rotation は Blender で整えてから import する
- Collider は prefab 側で明示し、自動生成 mesh collider に依存しない

#### 5.3 Audio

- Format: **OGG Vorbis quality 6**
- SFX: **Force To Mono** を基本、Load Type は **Decompress On Load**
- BGM: stereo 可、Load Type は **Streaming**
- 音量基準・ループ点は Studio One 側で確認し、Unity AudioMixer で最終調整する

#### 5.4 Font / TMP Atlas

- Atlas: **SDF**
- Texture format: **Alpha8**
- Padding: **1**
- 日本語は事前生成 Atlas を基本とし、Dynamic Font は使わない (ADR-0008)
- Missing glyph は String Table 実文字集合との intersection で判断し、必要なら fallback font を追加する

### 6. asset_ledger 記載責任

すべての AI 生成アセットは `docs/legal/asset_ledger.md` に記録する。Google Fonts OFL など非 AI の third-party asset も、license と公開可否を確認する必要がある場合は同台帳に記録するが、Steam AI 開示 Tier とは分けて扱う。

必須項目:

- ID
- アセット名 / 保存先
- 生成日
- ツール
- プラン
- 入力素材 / prompt 参照
- 手修正
- 商用可否
- 公開可否
- Steam 開示区分 (Tier 1 / Tier 2 / Tier 3)
- 備考

運用規律:

- 1 アセットまたは 1 logical asset group につき 1 行 entry を基本にする
- `asset_ledger.md` は hot file なので、並列 session 中は `git add -p docs/legal/asset_ledger.md` で hunk 選別する
- ledger だけの修正と asset import commit を分けてもよいが、最終的に asset と ledger の対応が追える状態にする
- receipt excerpt は金額・plan・billing date・issuer など必要最小限を要約し、個人情報は載せない

### 7. 並列セッション運用での dirty 混在回避

Stage 3 Day 1 では、Unity asset import、TMP Atlas 生成、Aseprite sprite 仕上げ、ADR 更新が並列に走り、main working tree に多数の dirty / untracked が混在した。以降は以下を標準とする。

- 大量アセット生成や Unity Editor batch は **一時 Unity project** で実行できる場合は一時 project を使う
- main project へは最終 `.asset` / `.png` / `.fbx` / `.ogg` / `.prefab` のみ転送する
- TMP Atlas のように Unity が多数の side-effect asset を生成する作業は、A5 方式として一時 project で先に検証し、必要成果物のみ main project に入れる
- stage は必ず pathspec 限定にする。`git add -A` は禁止
- commit 前に `git diff --cached --name-status` と `git diff --check --cached` を確認する
- push 前に以下を実行し、並列 push を確認する:

```powershell
git fetch origin main
git log HEAD..origin/main --oneline
```

先行 commit が 1 件以上あれば `git pull --rebase origin main` 後に push する。ただし dirty が rebase を阻害する場合は、対象 task の staged / unstaged を確認し、必要なら task handover を作って整理してから rebase する。

### 8. Steam 開示区分 (Tier 1 / 2 / 3)

本 ADR では `asset_ledger.md` の Steam 開示整理用に、AI 関与を以下 3 段階で運用する。

| Tier | 意味 | 例 | Steam 開示方針 |
|---|---|---|---|
| **Tier 1: player-consumed** | プレイヤーが直接見聞きする最終アセットに AI 生成物が含まれる | sprite、BGM、SFX、AI 生成 3D model | Steam 提出時に開示必須 |
| **Tier 2: intermediate** | 開発過程で AI を使ったが、最終アセットは人手で再制作 / 十分に置換された | AI concept → 手描き sprite、AI layout → 手配置 prefab | 開示推奨。判断根拠を ledger に残す |
| **Tier 3: development tool** | コード補完、文書ドラフト、調査、手順レビューなど、最終 player-consumed asset ではない | Claude / Codex による実装支援、ADR 草稿、レビュー | 原則として開示不要。ただし提出時点の Steamworks フォームを再確認する |

Tier は「使用した tool」ではなく「最終成果物への AI 生成物の残り方」で判定する。同じ PixelLab 使用でも、PixelLab 出力を Aseprite で調整して最終 sprite として使うなら Tier 1、完全に手描きで描き直して参考に留めたなら Tier 2 とする。OFL font のような非 AI third-party asset は license ledger 対象ではあるが、Steam AI 開示 Tier の対象外とする。

---

## Consequences

### 利点

- **再現性が上がる** — Stage 4 以降に同じ asset category を増やす時、生成から import までの責務が明確になる
- **GitHub Public に載せる範囲が明確** — 中間ファイルを除外しつつ、devlog / prompt / ledger で透明性を保てる
- **並列開発に強くなる** — pathspec 限定 staging と一時 project 運用で、他セッションの dirty を巻き込みにくい
- **Steam AI 開示に備えられる** — Tier 1 / 2 / 3 を asset ledger の行単位で説明できる
- **new contributor が参加しやすい** — `Assets/` 配置、import settings、ledger 記載責任が文書化される

### 欠点 / 注意点

- **記録コストが増える** — asset 生成ごとに ledger と devlog を残すため、短期速度は少し落ちる
- **hot file が増える** — `asset_ledger.md` は競合しやすく、`git add -p` と小 commit の規律が必要
- **一時 project 運用は転送漏れに注意** — `.meta` / material / TMP Settings など必要成果物を main project に移し忘れるリスクがある
- **Steam 開示区分は最終判断ではない** — 提出時点の Steamworks 文言を必ず再確認する

### 後続への影響

- ADR-0003 は「採用ツールと方針」、本 ADR は「実務手順」として併存する
- ADR-0004 の directory layout は、本 ADR の最終配置 convention の基準になる
- ADR-0008 の TMP Atlas / localization asset は、本 ADR の font import と missing glyph review の対象になる
- `docs/legal/asset_ledger.md` は Stage 4 以降も asset 制作の必須 gate になる
- `docs/asset_prompts/` は生成 tool ごとの prompt source-of-truth として維持する

---

## Alternatives

### 候補 B: AI raw / working file もすべて Git に含める

**判定:** 不採用。

理由:

- GitHub Public に出すべきでない中間生成物や receipt-adjacent file が混ざる
- repo size が増え、clone / review / CI の負荷が上がる
- 最終成果物と検証ログがあれば公開透明性は担保できる

### 候補 C: asset_ledger を後でまとめて書く

**判定:** 不採用。

理由:

- 生成日、plan、prompt、商用可否の記憶がすぐ曖昧になる
- Steam 開示の根拠が薄くなる
- 複数 tool を並列使用する Stage 3-4 では後追い復元コストが高い

### 候補 D: main Unity project で全生成作業を行う

**判定:** 一部不採用。

小規模 import は main project でよいが、TMP Atlas 生成や大量 asset batch のように side-effect が多い作業は一時 project を優先する。Stage 3 Day 1 の dirty 混在実績から、main project 直実行は commit 分離のリスクが高い。

---

## References

### Anemora 内部文書

- `ADR-0003` — AI 主体 + 人手仕上げのアセットパイプライン方針
- `ADR-0004` — Project Directory Structure
- `ADR-0007` — UI フレームワーク
- `ADR-0008` — ローカライズ実装方針
- `docs/legal/asset_ledger.md` — アセット権利 / Steam 開示台帳
- `docs/asset_prompts/bgm_zone1_ambient.md` — BGM 生成 prompt / Studio One 仕上げ
- `docs/asset_prompts/sfx_zone1.md` — SFX 生成 prompt / Studio One 仕上げ
- `docs/devlog/2026-05-05_tmp_jp_atlas_v0_1_missing_chars_review.md` — TMP JP Atlas missing glyph review
- `VS_SCOPE.md` §4 / §5 / §7 — asset scope / audio scope / FIX 境界

### 外部 tool / license source

- PixelLab: Pixel Apprentice plan
- Aseprite: 購入済み正式版
- Meshy v6 paid credits
- Blender 4.5.5 LTS
- Studio One
- Google Fonts OFL
- Unity TextMeshPro / TMP Font Asset Creator

---

## 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0.1 | 2026-05-05 | Stage 3 Day 1 実運用をもとに草稿化。生成 → 中間処理 → 最終 import、ツール分担、中間ファイル gitignore、最終配置、Unity import 標準、asset_ledger 責務、並列 session 規律、Steam 開示 Tier 1/2/3 を定義 |
| v0.2 | 2026-05-05 | Stage 3 /spec resolution interview で user 承認を受け、Status を Proposed から Accepted へ変更 |
