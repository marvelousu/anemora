# Stage 3 TBD Resolution Tracking Sheet

> 本 sheet は、Stage 3 Day 1 以降に残っている user 判断ポイントを一覧化するための tracking sheet。
> `feedback_anemora_no_premature_lockin.md` の方針に従い、候補の優劣付け、案の誘導、収束予測は書かない。

---

## 1. 概要

### 1.1 目的

- user 判断が必要な TBD を一覧化する。
- 各 TBD の状態、反映先、Stage 3 完了条件への影響を tracking する。
- Stage 3 の実装 session が、未確定事項を確定扱いで進めないようにする。

### 1.2 更新方針

- user が明示的に「決めた」と言うまで、各項目は未確定として扱う。
- 各 TBD が解決したら、該当 row を strike-through し、確定日と反映 commit hash を `反映先 (確定後)` または row 末尾に追記する。
- 新規 TBD が発生したら、§2 の表へ 1 row / 1 項目で追記する。
- 複数案が並立する場合は併記する。`draft`、`provisional`、`intermediate`、`TBD` の状態を明示し、案を絞らない。
- 「決めない」も有効な状態として扱い、Stage 4 以降でよい項目はそのまま保持する。

---

## 2. TBD 項目一覧

| 項目 ID | カテゴリ | 現状 | 候補 / 案 | user 判断必要か | ブロック影響度 | 反映先 (確定後) | 期日目安 |
|---|---|---|---|---|---|---|---|
| CHR-01 | キャラクター | 主人公名は未確定。localization glossary では `主人公` / `Protagonist` が tbd 扱い。 | TBD。候補名が出たら併記する。現状は固有名なし / `主人公` 表記のまま。 | Yes | 中 (Stage 3 quality に影響) | `docs/localization/glossary.md`, dialogue / UI text, future character sheet | Stage 3 内 (VS 完成前) |
| CHR-02 | キャラクター | 主人公の性別は user 最終判断待ち。F2/F4 は visual draft として進行。 | TBD: 中性 / 男 / 女 / その他。中性表現は draft review 軸であり、確定ではない。 | Yes | 高 (Stage 3 死守ライン直撃) | `docs/asset_prompts/hero_v1.md`, `Assets/Art/Sprites/Hero/`, `Assets/Prefabs/Characters/Hero.prefab` | Stage 3 内 (VS 完成前) |
| CHR-03 | キャラクター | 主人公の年齢は user 最終判断待ち。 | provisional: 10 代後半から 20 代前半。TBD: それ以外の年齢帯も未排除。 | Yes | 中 (Stage 3 quality に影響) | hero prompt, sprite review notes, future character sheet | Stage 3 内 (VS 完成前) |
| CHR-04 | キャラクター | 主人公の出身詳細は未確定。VS では深掘りしない運用。 | TBD: 家族構成 / 職業 / 居住歴 / 偽記憶の具体内容 / その他。 | Yes | 低 (Stage 4 以降で OK) | SPEC, future narrative bible, character sheet | Stage 4 入口 |
| NPC-01 | キャラクター | Resident_A は NPC draft として進行。個別シートは未確定。 | draft: 中年から初老の普通の住人。TBD: 名前 / 年齢 / 生活背景 / 対話差分 / 現在反映の見え方。 | Yes | 高 (Stage 3 死守ライン直撃) | `docs/draft/g3_npc_dialogue.md`, NPC prefab / DialogueAsset, character sheet | Stage 3 内 (VS 完成前) |
| NPC-02 | キャラクター | Resident_B は NPC draft として進行。個別シートは未確定。 | draft: 静かに座る普通の住人。TBD: 名前 / 年齢 / 生活背景 / 対話差分 / Resident_A との対比。 | Yes | 中 (Stage 3 quality に影響) | `docs/draft/g3_npc_dialogue.md`, NPC prefab / DialogueAsset, character sheet | Stage 3 内 (VS 完成前) |
| STORY-01 | 物語 | 衰退原因の扱いは未確定。Stage 3 では見せ方のみ最小化。 | TBD: 比喩 / 実体 / 比喩と実体の併存 / 判定保留。 | Yes | 低 (Stage 4 以降で OK) | SPEC, VS_SCOPE, future narrative bible | Stage 4 入口 |
| STORY-02 | 物語 | 時の筆の起源の詳細は未確定。 | draft/provisional: 物として存在する時の筆。TBD: 古代遺物 / 世界由来 / 観測者由来 / 起源非開示 / その他。 | Yes | 低 (Stage 4 以降で OK) | SPEC, ADR-0005 notes, future narrative bible | Stage 4 入口 |
| STORY-03 | 物語 | 主人公が選ばれた理由は未確定。 | TBD: 世界が選んだ / 観測者が選んだ / 誰も選んでいない / 偶発 / その他。 | Yes | 低 (Stage 4 以降で OK) | SPEC, future narrative bible | Stage 4 入口 |
| STORY-04 | 物語 | 第 1 ゾーン正式名は未確定。現状は仮称として「街」を使用。 | TBD: 「街」継続 / 新名称 / 地名なし / その他。候補名が出たら併記する。 | Yes | 中 (Stage 3 quality に影響) | `docs/localization/glossary.md`, `docs/VS_SCOPE.md`, UI / dialogue text | Stage 3 内 (VS 完成前) |
| ART-01 | アート | F2 Hero / NPC v1 は draft review 待ち。 | 確認軸: 中性表現 / 同一性 / 年齢対比 / palette 統一 / Resident_B の暗さ。各軸は user review まで未確定。 | Yes | 高 (Stage 3 死守ライン直撃) | `Assets/Art/Sprites/Hero/`, `Assets/Art/Sprites/NPC/`, F2/F4 devlog | Stage 3 内 (VS 完成前) |
| ART-02 | アート | F3 Retro Diffusion 補助の要否は未確定。 | TBD: 使用する / 使用しない / Hero のみ / NPC のみ / 差し替え候補作成のみ。 | Yes | 中 (Stage 3 quality に影響) | F3 prompt / devlog, `docs/legal/asset_ledger.md` if used | Stage 3 内 (VS 完成前) |
| ART-03 | アート | Anemora パレット v0 は draft。最終採用は未確定。 | draft: palette v0。TBD: v0 採用 / v0 改訂 / 別 palette / Stage 4 で再評価。 | Yes | 中 (Stage 3 quality に影響) | UI palette assets, sprite polish notes, ADR / asset docs | Stage 3 内 (VS 完成前) |
| ART-04 | アート | TMP 美咲ゴシック JP atlas は draft。最終採用は未確定。 | draft: 美咲ゴシック JP atlas。TBD: 採用 / fallback 追加 / 別 JP font / Stage 4 で再評価。 | Yes | 中 (Stage 3 quality に影響) | `docs/adr/0008-localization.md`, TMP font assets, localization docs | Stage 3 内 (VS 完成前) |
| ART-05 | アート | Press Start 2P EN atlas は導入済み。最終採用は未確定。 | draft: Press Start 2P。TBD: 採用 / 別 EN font / Stage 4 で再評価。 | Yes | 低 (Stage 4 以降で OK) | `docs/adr/0008-localization.md`, TMP English Atlas, localization docs | Stage 4 入口 |
| ENV-01 | 環境 | Plaza monument は draft / intermediate が並存。 | draft: B 噴水跡。intermediate: A bench / C pedestal。TBD: B 採用 / A 採用 / C 採用 / 複数併用 / 再生成。 | Yes | 高 (Stage 3 死守ライン直撃) | `Assets/Prefabs/Zone1/`, A3 devlog, scene placement | Stage 3 内 (VS 完成前) |
| ENV-02 | 環境 | Tree_Decay の落葉度は user review 待ち。 | draft: sparse / near-leafless。TBD: sparse 採用 / near-leafless 採用 / 両方使用 / 再調整。 | Yes | 中 (Stage 3 quality に影響) | `Assets/Prefabs/Zone1/`, material / prefab variants, A3 devlog | Stage 3 内 (VS 完成前) |
| ENV-03 | 環境 | House_Player 内装スコープは未確定。 | TBD: Bed / Bookshelf 2 variants / Table+Chair / Door の全部 / 一部 / placeholder 維持。 | Yes | 高 (Stage 3 死守ライン直撃) | `Assets/Prefabs/Zone1/House_Player*`, G1 scene, opening flow | Stage 3 内 (VS 完成前) |
| AUD-01 | 音響 | A4 が MCP AIVA で BGM 進行中。現状、user 判断保留として記録された音響 TBD はなし。 | 現状なし。新規判断が出たら別 row で追記する。 | No | 低 (Stage 4 以降で OK) | Audio prompt / devlog if a new TBD appears | 任意 |
| TECH-01 | 技術 | LocalizationSettings + StringTable seed の key 命名は A1-followup で実装予定。構造方針は user 確認待ち。 | TBD: namespace あり / scene prefix あり / flat key / JP source key / その他。 | Yes | 中 (Stage 3 quality に影響) | localization assets, `docs/localization/glossary.md`, tests | Stage 3 内 (VS 完成前) |
| DOC-01 | 文書 | ADR-0009 Asset pipeline は Proposed。Accepted 化は user 承認待ち。 | TBD: Accepted / Proposed 維持 / 改訂後 Accepted。 | Yes | 低 (Stage 4 以降で OK) | `docs/adr/0009-asset-pipeline.md`, `docs/legal/asset_ledger.md` | Stage 4 入口 |
| PUB-01 | 公開 | Code license は未確定。 | TBD: ライセンス未定 / OSS license 候補 / proprietary / dual license / その他。 | Yes | 低 (Stage 4 以降で OK) | `README.md`, `LICENSE`, release docs | Stage 4 入口 |
| PUB-02 | 公開 | Public release の時期と形態は未確定。 | TBD: Steam Early Access / itch.io / GitHub Public のみ / 非公開継続 / その他。 | Yes | 低 (Stage 4 以降で OK) | `README.md`, release checklist, Steam / itch docs | Stage 4 入口 |

---

## 3. 列挙済み TBD 項目

§2 の表には、以下の user 判断保留項目を 1 row / 1 項目で登録済み。

### 3.1 キャラクター / 物語

- CHR-01: 主人公名
- CHR-02: 主人公性別
- CHR-03: 主人公年齢
- CHR-04: 主人公出身詳細
- NPC-01: Resident_A 個別シート
- NPC-02: Resident_B 個別シート
- STORY-01: 衰退原因
- STORY-02: 時の筆の起源
- STORY-03: 主人公が選ばれた理由
- STORY-04: 第 1 ゾーン正式名

### 3.2 アート

- ART-01: F2 Hero / NPC v1 アートレビュー
- ART-02: F3 Retro Diffusion 補助の要否
- ART-03: Anemora パレット v0 採用最終確定
- ART-04: TMP 美咲ゴシック採用最終確定
- ART-05: Press Start 2P 採用最終確定

### 3.3 環境

- ENV-01: Plaza monument 採用最終決定
- ENV-02: Tree_Decay 落葉度
- ENV-03: House_Player 内装スコープ

### 3.4 音響

- AUD-01: 現状 A4 BGM 進行中、判断保留なし

### 3.5 技術 / 文書

- TECH-01: LocalizationSettings + StringTable seed の最終 key 命名
- DOC-01: ADR-0009 Asset pipeline Status: Proposed -> Accepted

### 3.6 公開

- PUB-01: Code license
- PUB-02: Public release 判断

---

## 4. 解決ガイドライン

- 「決めない」も valid。複数案を並列維持してよい。
- VS 死守ラインに直撃する判断のみ Stage 3 内で解決対象にする。
- Stage 4 入口以降でよい判断は、未確定のまま保持してよい。
- 確定時は §2 の該当 row を strike-through し、確定日、反映 commit hash、反映先を追記する。
- A2 / Codex は候補を評価しない。user が明示的に決めるまで確定扱いにしない。

---

## 5. 更新履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0.1 | 2026-05-05 | Stage 3 Day 1 の user 判断保留 tracking sheet として初版起草 |
