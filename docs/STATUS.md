# Anemora STATUS — 現在の作業 frontier (常時最新化)

> **これは最上位の読書起点**。セッション開始時、git archaeology / recovery file 漁りの前に **まずこれを読む**。
> 詳細実装履歴は `docs/devlog/`、物語骨格は `docs/STORY_BIBLE_v1.md`、運用 gotcha は `AGENTS.md`。
> **更新規律**: frontier が動いたら同じ commit でここを更新する。古いまま放置しない (pre-push hook が7日超の漂流を検査する)。

最終更新: 2026-06-13 (Win Claude / Fable 5)

---

## 1. いま何の状態か (1 段落)

Fast VS House Slice は **public VS baseline** として `main` に公開済み。現在の実装活動は `wip/hd2d-point15-recovery-20260612` 上の **HD2D point15 レンダラ調整ループ** (probe / isolation / ablation、レビュー cycle 125 到達、air alpha 0.60) で、Chapter 1 設計トラック (6シーン v1 設計完了済) は 2026-05-24 から停止中。2026-06-13 に環境監査を実施し、**レンダラ凍結 + 環境アセット物量投入への転換**を提案中 (`docs/devlog/2026-06-13_env_audit_renderer_freeze_proposal.md`)。

## 2. branch / baseline

| ref | 役割 | 注意 |
|---|---|---|
| `main` (`f8e8109`) | **public VS baseline** (安定・凍結) | 基本触らない。devlog/設計 doc も main に直接置かない |
| `wip/hd2d-point15-recovery-20260612` | **現行実装 branch** (point15 レンダラループ) | 2026-06-13 に origin へ push 済 (それまでローカルのみ547コミット)。devlog 32本+modified 44 が未コミット — ループ停止時に pathspec でコミットする |
| `work/chapter1-continuation-20260520` | Chapter 1 設計+実装 (head `cbeedfe`) | bundle (2026-06-11) から origin へ復元済み。設計 frontier = S3 詳細設計 |
| `origin/wip/snapshot-repair-proof-20260603` | 平行分岐した旧「最新」 | 現行 branch と merge-base 不一致。**処遇未決** (要ユーザー判断: 廃棄 or 取込み) |
| `work/post-vs-public-20260518` 等 | 旧 branch | 参照のみ |

## 3. 現在の frontier (アクティブな一手)

- **point15 レンダラループ**: cycle125 (air alpha 0.60) のアブレーション切り分けが進行中 (動作中アーティファクトの犯人探し)。
- **2026-06-13 環境監査の提案**: ①cycle125 でレンダラ設定を凍結し契約テスト化 ②エネルギーを環境アセット (テクスチャ/植生/空/ライティング) へ転換 ③authored file (81k行) の段階的減量。適用物は incoming ステージング (監査 devlog 参照) に準備済みで、**ループ一時停止時に適用**。
- Chapter 1 設計トラックは S3 詳細設計で停止中 (canon は `docs/canon/chapter1.md`)。

## 4. 次に来るもの (frontier 候補)

- ループ一時停止時: 未コミット devlog+INDEX のコミット → incoming ステージング適用 (レンダラ契約テスト / ValidateImportedAssetsBatch / dispatcher+shotdiff 配線) → 検証
- **ビジュアルターゲットの確定 (要ユーザー判断)**: 設計文書の「Tier 2 意図的選択」と目標「最高ティア (Tier 4)」の矛盾解消。広場1枚のターゲット合成画を正とする
- 環境アセット物量フェーズ (PolyHaven/meshy/PixelLab パイプライン、512アトラスの2K化)
- Chapter 1 設計トラック再開 (S3 詳細設計 → S4-6 台詞 refine → 通し確認)

## 5. 直近で踏んだ落とし穴 (再発防止)

- **STATUS/INDEX の3週間漂流が再発** (2026-06-13 検出): STATUS=05-24、INDEX=05-20 のまま point15 ループが2週間進行、devlog 32本が untracked、branch 未push。→ pre-push hook に STATUS 鮮度ガードを追加。夜間バックアップ (ローカルタスク AnemoraNightlyBackup) を常設。
- **承認アセットの紛失** (v57/v58 ジェネリック NPC、削除済み worktree と共に消失): → 承認が出たアセットはその場で commit+push してから次へ (AGENTS.md 規律)。
- **stale context 事故** (2026-05-08): recovery file を最新と誤認。→ 本 STATUS の最終更新日と今日を必ず照合。

## 6. セッション開始時の読書順

1. 本 `docs/STATUS.md` (現在地)
2. `AGENTS.md` (project gotcha、canon/devlog 運用ルール含む。root `CLAUDE.md` は Claude Code 用の import 1行)
3. `docs/canon/` (**確定 canon の living state**) — 特に `chapter1.md`
4. `docs/MAP.md` (Antela canonical 配置 + Fast VS zone、方位ドリフト防止)
5. `docs/devlog/INDEX.md` 最新日付エントリ (実装履歴の一次ナビ、**immutable**)
6. 必要に応じ `docs/STORY_BIBLE_v1.md` (legacy) / 各シーン原 devlog

## 7. 更新履歴

| 日付 | 更新者 | frontier 変化 |
|---|---|---|
| 2026-05-19 | Linux Claude | 初版。シーン 1 [1.E] アリア canon 確定 → Codex patch 待ち |
| 2026-05-19 | Linux Claude | VS スコープ確定 (別VS=1章終了まで)。シーン1[1.E] canon 訂正 (図書館人物=無名、Option B 撤回、既存行据え置き)。frontier をシーン3 v1 canon draft に移行 |
| 2026-05-21 | Linux Claude | `docs/MAP.md` 新設 (canonical Antela 配置、2026-05-09 東展開+05-18 Fast VS zone 統合)。古いマップ文言 (2026-05-08 系の南進ルート、2026-05-09 シーン2 devlog 内のミア「南へ」等) との drift 明示 |
| 2026-05-21 | Linux Claude | シーン 2 devlog v1.1 (方位整合、ミア「南へ」→「東へ」)。**シーン 3 v1 final 確定** (`2026-05-21_..._scene3_v1_final.md`、canonical Aria 初登場、エリュトリア二重伏線、シーン 1 リンクなし、方位 MAP.md 準拠)。frontier をシーン 4 へ |
| 2026-05-22 | Linux Claude | **Chapter 1 = 6 シーン再構成** (新 S5 廃墟エリア=探索+パズル、旧 S5 → S6 廃墟予兆)。カイア「畑」→「農園」用語整合 (果樹園+小畑、種は畑用)。シーン 4 flow 確定 (台詞 draft)。frontier をシーン 5 設計へ |
| 2026-05-22 | Linux Claude | **シーン 5「廃墟エリア」v1 確定** (`2026-05-22_..._scene5_v1.md`): 橋パズル = 修繕中+2ホップ+現在石材持ち込み (シーン1の逆=現在→過去のメカ明示)、過去でのみ入れる家+「暮らしの記録」観測者磨耗伏線、案1棲み分け。frontier をシーン 6 へ |
| 2026-05-22 | Linux Claude | エリュトリアを シーン 5-6 地理から除去 (3-4 章の台詞伏線に留め)。**シーン 6 v1 確定** (章を閉じる 2D 真横・夕焼け自動シーケンス)。**Chapter 1 全 6 シーン v1 設計完了**。frontier = Codex reconcile + 台詞 refine |
| 2026-05-22 | Linux Claude | 設計 8 commit を public main → `work/chapter1-continuation-20260520` branch へ移送、main を VS baseline (e9d61c2) に復帰。reconciliation devlog で Codex 実装との divergence 引継ぎ |
| 2026-05-24 | Linux Claude | **S2 詳細設計 v1.2 + 過去建物 canon 確定 + `docs/canon/` 新設** (consolidated 原本、他セッション引き渡し用)。S2 構造改訂 (camera south 整合、ミア前庭出会い、建物 transition、時の窓 S2 不使用)、台詞 refine (ミアの語り [2.D] + 種の依頼 [2.E] + 出発 [2.F] のカメラ演出)。過去建物 = 全て見た目変える (図書館除く、内部仕様で同形を許容)。**canon/devlog 運用ルール確立**: canon = `docs/canon/` のみ mutable、devlog は immutable、CLAUDE.md + canon/README.md 明文化。frontier を S3 詳細設計へ |
| 2026-06-13 | Win Claude (Fable 5) | **3週間の漂流を解消**: 現在地を point15 レンダラループ (cycle125) に更新。branch push / chapter1 bundle 復元 / バックアップ自動化 (AnemoraNightlyBackup) / pre-push STATUS 鮮度ガード導入。レンダラ凍結+環境アセット転換を提案 (`2026-06-13_env_audit_renderer_freeze_proposal.md`)。frontier 候補に「ビジュアルターゲット確定 (Tier 2 vs Tier 4)」を明示 |
