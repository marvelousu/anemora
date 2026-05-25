# Anemora STATUS — 現在の作業 frontier (常時最新化)

> **これは最上位の読書起点**。セッション開始時、git archaeology / recovery file 漁りの前に **まずこれを読む**。
> 詳細実装履歴は `docs/devlog/`、物語骨格は `docs/STORY_BIBLE_v1.md`、運用 gotcha は `CLAUDE.md`。
> **更新規律**: frontier が動いたら同じ commit でここを更新する。古いまま放置しない。

最終更新: 2026-05-24 (Linux Claude)

---

## 1. いま何の状態か (1 段落)

Fast VS House Slice (序章〜シーン 1 図書館跡: Niro 家 → 広場 → 図書館 → レト → 時の窓 → 過去観察) が **public VS baseline** として `main` に公開済。Chapter 1 は **6 シーン構成** (序章 + シーン 1-6、2026-05-22 確定)。Stage 3 VS 完了扱い、Stage 4 production 化フェーズ。

**VS スコープ (確定)**: 2 target — ① Fast VS House Slice = 序章+シーン1 (現 public baseline)　② **別 VS (本命) = 1 章終了まで = 序章+シーン1-6** (詰める対象)。

**Chapter 1 = 6 シーン**: 序章 / S1 図書館跡 / S2 ミア家 / S3 街角+アリア家 / S4 カイアの農園 (T4 climax) / S5 廃墟エリア (探索+パズル) / S6 廃墟予兆 (Z第2段階+章末)。

## 2. branch / baseline

| ref | 役割 | 注意 |
|---|---|---|
| `main` (`e9d61c2`) | **public VS baseline** (安定・凍結) | **基本触らない**。新規開発は branch で。devlog/設計 doc も main に直接置かない |
| `work/chapter1-continuation-20260520` | **Chapter 1 継続作業 branch** (現行) | Codex の Chapter 1 実装 + Linux Claude の設計 devlog。Chapter 1 作業はここ |
| `work/post-vs-public-20260518` 等 | 旧 branch | 参照のみ |

## 3. 現在の frontier (アクティブな一手)

- **Chapter 1 全 6 シーン (序章 + S1-S6) の v1 設計が一通り完了** (2026-05-22)、**consolidated 原本 `docs/canon/chapter1.md` 作成済** (2026-05-24)。
- **S2 詳細設計 v1.2 進行中**: 構造改訂 + 台詞 refine (2026-05-24)、frontier = S3 詳細設計。
- **canon/devlog 運用ルール確立** (2026-05-24): canon = `docs/canon/` のみ mutable、devlog は immutable (新 devlog で訂正)、`CLAUDE.md` + `docs/canon/README.md` に明文化。
- Chapter 1 各シーン canon/設計 (すべて `work/chapter1-continuation-20260520` branch 上):
  - シーン 1 [1.E] = 無名人物・既存行据え置き (`2026-05-19_..._aria_canon.md` v2.0)
  - シーン 2 v1 final = `2026-05-09_..._scene2_v1_final.md` v1.1 (方位整合)
  - シーン 3 v1 final = `2026-05-21_..._scene3_v1_final.md`
  - シーン 4 flow 確定 = `2026-05-22_..._6scene_restructure_and_scene4_confirm.md` (T4 climax、台詞 draft)
  - シーン 5 v1 = `2026-05-22_..._scene5_v1.md` v1.1 (廃墟エリア、橋パズル、エリュトリア地理除去)
  - シーン 6 v1 = `2026-05-22_..._scene6_v1.md` (章を閉じる自動 2D 真横・夕焼けシーケンス)
- 後続: ① Codex の Chapter 1 実装 reconcile (`2026-05-22_..._design_revision_branch_reconciliation.md` v1.1 が依頼書、シーン 1 は要協議)　② シーン 4-6 の台詞 refine パス　③ Chapter 1 通し確認

## 4. 次に来るもの (frontier 候補)

- Codex の Chapter 1 実装 reconcile 結果の受領 (シーン 1 = 要協議、シーン 4-6 = 設計に合わせ実装)
- シーン 4-6 の台詞 refine パス (現状 draft)
- Chapter 1 通し確認 (序章 + S1-S6)
- prototype (`docs/PROTOTYPE_TIME_FRAME_v0.md`) 結果反映

## 5. 直近で踏んだ落とし穴 (再発防止)

- **stale context 事故**: 2026-05-08 recovery file を最新と誤認し 10 日遅れの design を空打ち。→ **必ず本 STATUS の最終更新日と今日の日付を照合**。recovery file は「当時の状態」であって現在ではない。
- **doc 見落とし**: repo root `_recovery_*.md` / `docs/draft/*handover*` / `~/notes/_handover/` を最初に確認しなかった。→ §6 の読書順を踏む。
- 2026-05-07〜05-18 の devlog は一度 sweep → 再構築された履歴 (`docs/devlog/INDEX.md` §Recovery Notes 参照)。INDEX を一次ナビにする。

## 6. セッション開始時の読書順

1. 本 `docs/STATUS.md` (現在地)
2. `CLAUDE.md` (project gotcha、canon/devlog 運用ルール含む)
3. `docs/canon/` (**確定 canon の living state**、他セッション引き渡し先) — 特に `chapter1.md` (Chapter 1 全 canon、序章 + S1-S6)
4. `docs/MAP.md` (Antela canonical 配置 + Fast VS zone、方位ドリフト防止)
5. `docs/devlog/INDEX.md` 最新日付エントリ (実装履歴の一次ナビ、**immutable**)
6. 必要に応じ `docs/STORY_BIBLE_v1.md` (legacy、全章物語骨格) / 各シーン原 devlog (詳細経緯)

## 7. 更新履歴

| 日付 | 更新者 | frontier 変化 |
|---|---|---|
| 2026-05-19 | Linux Claude | 初版。シーン 1 [1.E] アリア canon 確定 → Codex patch 待ち |
| 2026-05-19 | Linux Claude | VS スコープ確定 (別VS=1章終了まで)。シーン1[1.E] canon 訂正 (図書館人物=無名、Option B 撤回、既存行据え置き)。frontier をシーン3 v1 canon draft に移行 |
| 2026-05-21 | Linux Claude | `docs/MAP.md` 新設 (canonical Antela 配置、2026-05-09 東展開+05-18 Fast VS zone 統合)。古いマップ文言 (2026-05-08 系の南進ルート、2026-05-09 シーン2 devlog 内のミア「南へ」等) との drift 明示 |
| 2026-05-22 | Linux Claude | 6 シーン再構成 + シーン 4 flow 確定 + シーン 5 v1。設計 8 commit を public main → `work/chapter1-continuation-20260520` branch へ移送、main を VS baseline (e9d61c2) に復帰。reconciliation devlog で Codex 実装との divergence 引継ぎ |
| 2026-05-22 | Linux Claude | エリュトリアを シーン 5-6 地理から除去 (3-4 章の台詞伏線に留め)。**シーン 6 v1 確定** (章を閉じる 2D 真横・夕焼け自動シーケンス)。**Chapter 1 全 6 シーン v1 設計完了**。frontier = Codex reconcile + 台詞 refine |
| 2026-05-21 | Linux Claude | シーン 2 devlog v1.1 (方位整合、ミア「南へ」→「東へ」)。**シーン 3 v1 final 確定** (`2026-05-21_..._scene3_v1_final.md`、canonical Aria 初登場、エリュトリア二重伏線、シーン 1 リンクなし、方位 MAP.md 準拠)。frontier をシーン 4 へ |
| 2026-05-22 | Linux Claude | **Chapter 1 = 6 シーン再構成** (新 S5 廃墟エリア=探索+パズル、旧 S5 → S6 廃墟予兆)。カイア「畑」→「農園」用語整合 (果樹園+小畑、種は畑用)。シーン 4 flow 確定 (台詞 draft)。frontier をシーン 5 設計へ |
| 2026-05-22 | Linux Claude | **シーン 5「廃墟エリア」v1 確定** (`2026-05-22_..._scene5_v1.md`): 橋パズル = 修繕中+2ホップ+現在石材持ち込み (シーン1の逆=現在→過去のメカ明示)、過去でのみ入れる家+「暮らしの記録」観測者磨耗伏線、案1棲み分け。frontier をシーン 6 へ |
| 2026-05-24 | Linux Claude | **S2 詳細設計 v1.2 + 過去建物 canon 確定 + `docs/canon/` 新設** (consolidated 原本、他セッション引き渡し用)。S2 構造改訂 (camera south 整合、ミア前庭出会い、建物 transition、時の窓 S2 不使用)、台詞 refine (ミアの語り [2.D] + 種の依頼 [2.E] + 出発 [2.F] のカメラ演出)。過去建物 = 全て見た目変える (図書館除く、内部仕様で同形を許容)。**canon/devlog 運用ルール確立**: canon = `docs/canon/` のみ mutable、devlog は immutable、CLAUDE.md + canon/README.md 明文化。frontier を S3 詳細設計へ |
