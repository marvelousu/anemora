# Anemora STATUS — 現在の作業 frontier (常時最新化)

> **これは最上位の読書起点**。セッション開始時、git archaeology / recovery file 漁りの前に **まずこれを読む**。
> 詳細実装履歴は `docs/devlog/`、物語骨格は `docs/STORY_BIBLE_v1.md`、運用 gotcha は `AGENTS.md`。
> **更新規律**: frontier が動いたら同じ commit でここを更新する。古いまま放置しない (pre-push hook が7日超の漂流を検査する)。

最終更新: 2026-06-26 (Codex / HD2D terrain detail vista review build)

---

## 1. いま何の状態か (1 段落)

Fast VS House Slice は **public VS baseline** として `main` に公開済み。現在の実装活動は `wip/hd2d-point15-recovery-20260612` 上の **HD2D point15 レンダラ調整ループ + 環境アップリフト**で、Chapter 1 設計トラック (6シーン v1 設計完了済) は 2026-05-24 から停止中。2026-06-13 に環境監査を実施し、**レンダラ凍結 + 環境アセット物量投入への転換**を採用。2026-06-26 時点の最新レビュー build は Time Window aperture depth-order fix、CC0 textured nature pass、imported nature scale uplift、realistic nearfield nature clusters、authored under-canopy foliage、dark-pixel leaf alpha keying、全マップ前景 wild-grass layer、全マップ tree grove layer、current leaf/grass の低彩度化、遠景の forest/rock/woodland depth layer + camera-facing back arc + ScenicRelief forest/ridge layer + close/mid realistic specimen tree framing layer + specimen canopy branch/leaf detail layer + terrain detail vista material-density pass を含む `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`。黒い副作用が強かった photo vegetation card 案は accepted path から外した。

## 2. branch / baseline

| ref | 役割 | 注意 |
|---|---|---|
| `main` (`f8e8109`) | **public VS baseline** (安定・凍結) | 基本触らない。devlog/設計 doc も main に直接置かない |
| `wip/hd2d-point15-recovery-20260612` | **現行実装 branch** (point15 レンダラループ) | 2026-06-13 に origin へ push 済 (それまでローカルのみ547コミット)。devlog 32本+modified 44 が未コミット — ループ停止時に pathspec でコミットする |
| `work/chapter1-continuation-20260520` | Chapter 1 設計+実装 (head `cbeedfe`) | bundle (2026-06-11) から origin へ復元済み。設計 frontier = S3 詳細設計 |
| `origin/wip/snapshot-repair-proof-20260603` | 平行分岐した旧「最新」 | 現行 branch と merge-base 不一致。**処遇未決** (要ユーザー判断: 廃棄 or 取込み) |
| `work/post-vs-public-20260518` 等 | 旧 branch | 参照のみ |

## 3. 現在の frontier (アクティブな一手)

- **point15 レンダラループ / 環境アップリフト**: 2026-06-26 最新レビュー build で Time Window aperture を frame behind の depth-aware composite にし、CC0 textured tree subset + imported nature scale uplift + realistic nearfield nature clusters + authored under-canopy foliage + dark-pixel leaf alpha keying + all-map foreground wild-grass layer + all-map realistic tree grove layer + realistic distant panorama depth layer + ScenicRelief forest/ridge layer + close/mid realistic specimen tree framing layer + specimen canopy branch/leaf detail layer + terrain detail vista material-density pass を導入。黒い副作用が強かった photo vegetation card 案は accepted path から外した。全マップ capture、ValidateHouseSliceBatch、AssetValidation、EditMode renderer freeze、BuildAndValidateBatch、player smoke 済み。レビュー packet は `docs/review/2026-06-26T03-59_terrain_detail_vista_r6/`。
- **2026-06-13 環境監査の提案と着地**: ①✅レンダラ凍結=`Assets/Tests/RendererContract/` のゴールデン契約テストで実装済 (Unity検証: 初回ベースライン生成→2回目 36 EditMode緑/freeze=Passed)。renderer feature を変えるとテストが落ちる。意図的変更時は `ANEMORA_RENDERER_REBASELINE=1` で再生成しコミット。②✅アセット検収=`Assets/Editor/AnemoraAssetValidation.cs` の `ValidateImportedAssetsBatch` (missing ref/review_only混入/ポリ数、実走OK)。③authored file (81k行) 減量は **cycle 方式が当ファイルを毎サイクル編集する間は未着手** (再開時 merge 衝突を避けるため。cycle 方式を畳む時に実施)。環境アセット物量 (テクスチャ/植生/空) は Tier 目標確定後。
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
| 2026-06-23 | Codex | Build review 対応: Time Window aperture の前後関係を depth-aware composite に修正し、外部 CC0 textured tree subset + deterministic undergrowth companions で自然/木の見えを改善。最新 build: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe`。 |
| 2026-06-23 | Codex | Photo vegetation understory 追加: tree/cluster companion に fern/clover/small-plant カードを増やし、最新 review packet `docs/review/2026-06-23T13-07_photo_vegetation_understory_r1/` と最新 build を更新。 |
| 2026-06-23 | Codex | Photo branch canopy cards 追加: tree crown に branch cutout カードを重ね、最新 review packet `docs/review/2026-06-23T15-07_photo_vegetation_canopy_cards_r1/` と最新 build を更新。 |
| 2026-06-23 | Codex | Imported nature scale uplift: 浮いた遠景カード案を破棄し、既存の CC0/Textured Nature tree/grass/bush/plant companions を wide camera で樹形として読めるスケールへ調整。最新 review packet `docs/review/2026-06-23T19-00_imported_nature_scale_r1/` と最新 build を更新。 |
| 2026-06-23 | Codex | Realistic nature leaf lift: 黒い副作用が強かった photo branch/canopy card 案を accepted path から外し、広葉樹 sapling、photo ground layer、自然マテリアル暗部リフト、authored vegetation shadow off で木・草の読みを改善。最新 review packet `docs/review/2026-06-23T22-59_realistic_nature_leaf_lift_r1/` と最新 build を更新。 |
| 2026-06-24 | Codex | Realistic nature under-canopy: build review の Time Window aperture 前後関係を補正し、黒い副作用が残った photo vegetation card を accepted path から外し、authored under-canopy foliage、nearfield nature clusters、dark-pixel leaf alpha keying で自然の読みを改善。最新 review packet `docs/review/2026-06-24T02-33_realistic_nature_under_canopy_r1/` と最新 build を更新。 |
| 2026-06-24 | Codex | Realistic foreground wild grass: 全 outdoor current/past map に deterministic foreground wild-grass layer を追加し、CC0 imported grass/plant + authored ground cover/grass tuft で下端の自然密度を改善。最新 review packet `docs/review/2026-06-24T14-36_realistic_foreground_wild_grass_r1/` と最新 build を更新。 |
| 2026-06-24 | Codex | Realistic tree groves: 全 outdoor current/past map に deterministic tree grove layer を追加し、CC0/Textured Nature tree model を群れで配置、current leaf/grass を低彩度化して木としての読みを改善。最新 review packet `docs/review/2026-06-24T16-19_realistic_tree_groves_r1/` と最新 build を更新。 |
| 2026-06-24 | Codex | Realistic depth panorama: 全 outdoor current/past map の遠景に forest/rock/woodland の depth mesh と camera-facing back arc を追加し、平板な外周帯から奥行きのある遠景へ寄せた。最新 review packet `docs/review/2026-06-24T17-05_realistic_depth_panorama_r1/` と最新 build を更新。 |
| 2026-06-24 | Codex | Scenic relief backdrop: 全 outdoor current/past map の遠景に ScenicRelief forest/ridge layer を追加し、D/E/F の広域レビューで山裾と森林帯の読みに寄せた。最新 review packet `docs/review/2026-06-24T20-52_scenic_relief_backdrop_r1/` と最新 build を更新。 |
| 2026-06-24 | Codex | Realistic specimen trees: 全 outdoor current/past map に close/mid specimen tree framing layer を追加し、巨大幹で主景を塞いだ初期案を縮小・端配置へ修正して自然の読みを改善。最新 review packet `docs/review/2026-06-24T23-43_realistic_specimen_trees_r2/` と最新 build を更新。 |
| 2026-06-25 | Codex | Specimen canopy detail: 全 outdoor current/past specimen tree に branch lace / branch fork / canopy-breakup fan/spray / outer leaf spray / root fern detail を追加し、r1 の視認不足を r2 で外縁配置へ修正。最新 review packet `docs/review/2026-06-25T01-23_specimen_canopy_detail_r2/` と最新 build を更新。 |
| 2026-06-26 | Codex | Terrain detail vista: r2/r4 の視認不足と r3 の半径ガード失敗を経て、全 outdoor current/past 遠景の terrain detail mesh + RealisticDepth/ScenicRelief/ProductionDepth のテクスチャ密度を r6 で可視化。最新 review packet `docs/review/2026-06-26T03-59_terrain_detail_vista_r6/` と最新 build を更新。 |
