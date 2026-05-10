# Stage 3 計画書 (Vertical Slice 設計 + 着手)

> Anemora の Stage 3 は **Vertical Slice (VS) 制作のための基盤整備 + 着手** フェーズ。
> 「縦切り完成型」のゲーム体験を、最小範囲で品質保証付きで作る。
> 1 ヶ月集中開発の Day 3-10 を目安、ただし数値はバイアス込みで参考値。

> **Status (2026-05-04)**: v0.1。Stage 3 着手日 = Day 0。本書はトラック分解 + 依存グラフ + 入口を定義し、サブタスクの実行は別途 commit/ADR/devlog で記録する。v0.1 で §10 開発環境の使い分け戦略を追加。

---

## 1. Stage 3 の完了条件

以下の状態を満たしたとき、Stage 3 完了 → Stage 4 (α) 着手可能とする:

| # | 完了条件 | 検証手段 |
|---|---|---|
| 1 | Vertical Slice (第 1 ゾーンのコアループ完走) がプレイ可能 | 自プレイ + ビルド成功 |
| 2 | 時の窓プロトタイプ (3D 空間ステンシル + シンボル選択 + 過去踏込み) が動作 | 自プレイ + 動画キャプチャ |
| 3 | 主人公ヒーロービジュアル v1 が確定 | PixelLab + Aseprite で出力 |
| 4 | SPEC §13.3 オープン要件のうち Stage 3 で解決すべき項目を確定 | SPEC v1 改訂 |
| 5 | ADR が主要技術決定をカバー | `docs/adr/` に 5-10 件 |
| 6 | GitHub Public で Day 0 から逐次公開されている | `marvelousu/anemora` 等 (TBD) |

---

## 2. トラック分解 (7 系統)

| ID | トラック名 | 内容 |
|---|---|---|
| A | TBD 解決 | 物語・主人公・世界観の Stage 3 で確定すべき項目 |
| B | 技術セットアップ | git init / Unity install / プロジェクト初期化 / GitHub Public |
| C | ADR 作成 | 技術決定記録 (エンジン / レンダリング / ステンシル / アセット PL) |
| D | VS スコープ確定 | 「何を作れば VS と呼べるか」を SPEC v1 に落とす |
| E | 時の窓プロトタイプ | Unity URP + Stencil Buffer + シンボル UI |
| F | 主人公ヒーロービジュアル | PixelLab + Aseprite + Retro Diffusion |
| G | 第 1 ゾーン実装 | 仮ゾーン構築 + コアループ + NPC 配置 |

---

## 3. 依存グラフ

```
[ B 技術セットアップ ] ─┬─→ [ E 時の窓プロト ]
                        ├─→ [ G 第 1 ゾーン ]
                        └─→ [ F ヒーロービジュアル ]
                                        ↑
                                        │
[ A TBD 解決 ] ──→ [ D VS スコープ ] ──┤
[ C ADR 作成 ] (B/E/F/G と並行) ────────┘
```

- **B (技術セットアップ)** は全実装の前提
- **A (TBD 解決)** は D / F の前提 (主人公が決まらないとビジュアルが作れない)
- **C (ADR)** は B/E/F/G と並行可能、決定が固まったら逐次起こす
- **E / F / G** は B 後に並列実行可能

---

## 4. 入口 (Day 0 = 今日 = 2026-05-04 後半)

Stage 3 の最初に着手する foundational tasks:

| # | タスク | トラック |
|---|---|---|
| 0.1 | Stage 3 計画書 (本書) 作成 | (メタ) |
| 0.2 | ADR-001: エンジン Unity 6.3 LTS 確定 | C |
| 0.3 | `docs/adr/README.md` (ADR インデックス) | C |
| 0.4 | .gitignore を Unity 対応に拡張 | B |
| 0.5 | `git init` + 初期コミット (CONCEPT/PITCH/SPEC/devlog/ADR) | B |
| 0.6 | README.md を Stage 3 着手反映に更新 | B |
| 0.7 | GitHub Public repo 作成 (ユーザー操作) → remote 設定 + push | B (要ユーザー操作) |

0.7 は GitHub での repo 作成が必要なため、ユーザー側のアクション。

---

## 5. Day 0 後の進路 (3 候補から選択)

入口完了後、次の判断点:

### 候補 X: TBD 解決を /spec で先行 (トラック A)
- 主人公の輪郭 / 衰退原因の比喩か実体か / 時の筆の起源 / 真層の収束パターン / ゾーン仮称 → 確定
- ヒーロービジュアル制作 (F) と VS スコープ確定 (D) の前提
- 1-2 セッション想定

### 候補 Y: Unity install + プロジェクト初期化 (トラック B 続行)
- Unity Hub install → 6.3 LTS install → URP テンプレートで新規プロジェクト
- Linux 版 Unity の動作確認
- インストール時間が必要 (実機作業)

### 候補 Z: VS スコープ確定 (トラック D)
- 「何が動けば Vertical Slice と呼べるか」を SPEC v1 に落とす
- A の TBD は仮置きしてスコープだけ先に固定する判断もあり

---

## 6. マイルストン (目安、保守的見積もり)

> **注意**: Claude の時間見積もりは保守的バイアスが強い。実際は前倒し可能、本表は最大値の目安。

| Day | マイルストン |
|---|---|
| Day 0 (今日) | Stage 3 入口完了 (本書 + ADR-001 + git init) |
| Day 1-2 | TBD 解決 + Unity プロジェクト初期化 |
| Day 3-5 | 時の窓プロトタイプ動作 + 主人公ビジュアル v1 |
| Day 6-8 | 第 1 ゾーン構築 + コアループ完走 |
| Day 9-10 | 自プレイ調整 + Stage 3 完了判定 |

---

## 7. リスク (Stage 3 固有)

| リスク | 影響 | 軽減策 |
|---|---|---|
| Unity インストール / Linux 動作不安定 | 高 | Godot 撤退候補を保持、Windows 側でのインストール選択肢 |
| ステンシルバッファ実装難易度 | 高 | 公式サンプル参照、Codex に実装相談、URP Renderer Feature の事例調査 |
| AI 生成アセットの一貫性不足 | 中 | プロンプトテンプレート確立、Aseprite 手仕上げ前提 |
| TBD 解決が長引く | 中 | 仮置きで VS 着手、後で差し替え可能な設計 |
| Day 0 GitHub Public で先行公開のリスク | 低 | ライセンス + README で意図を明示、未完成前提を強調 |

---

## 8. ADR ロードマップ (Stage 3 で起こす想定)

| ADR | 主題 | 着手タイミング |
|---|---|---|
| 0001 | エンジン Unity 6.3 LTS 採用 | Day 0 (本日) |
| 0002 | URP + Stencil Buffer + Renderer Feature によるポータル実装方針 | E 着手時 |
| 0003 | アセットパイプライン (PixelLab / Aseprite / Meshy / Blender) | F 着手時 |
| 0004 | プロジェクトディレクトリ構造 (Assets/ / Scripts/ / Art/ 等) | B プロジェクト初期化時 |
| 0005 | 時間管理 / シーン切替の実装方針 | E 設計時 |
| 0006 | セーブシステムの実装方針 | G 中盤 |
| 0007 | UI フレームワーク (uGUI vs UI Toolkit) | UI 着手時 |
| 0008 | ローカライズ実装 | Stage 4 入口の可能性 |

---

## 10. 開発環境の使い分け戦略

二拠点運用 (ノートPC + デスクトップ) を前提に、作業性質ごとに機材と環境を使い分ける。

### 10.1 機材スペック概要

| 機材 | OS | CPU | RAM | GPU | 用途 |
|---|---|---|---|---|---|
| **ノートPC** (主軸) | Windows 11 Home + WSL2 | Ryzen 5 7430U (6C12T Zen 3, 15W) | 16GB DDR4 | 統合 Radeon (VRAM 2GB) | 平時の主作業機 |
| **デスクトップ** | Windows 11 | i7-10700 (8C16T) | 16GB | RTX 2070S (8GB) | 重い局面のみ赴く |

ノートPC は Windows + WSL2 構成で **Linux CLI と Windows GUI を 1 台で完結**できる。デスクトップは GPU 強化 (RTX 2070S) と CPU マルチコア (8C16T) で重い作業に優位。

### 10.2 ロール分担表

| 作業 | 機材 | OS / 環境 | 理由 |
|---|---|---|---|
| ドキュメント / 仕様 / Codex / Git / Claude | ノートPC | **WSL2 (Linux)** | テキスト中心、Claude/Codex/CLI に最適 |
| Unity Editor 軽編集 (スクリプト・基本シーン) | ノートPC | Windows | Unity は Windows GUI が公式サポート手厚い |
| Aseprite / 軽量 Blender (低ポリ) | ノートPC | Windows | 軽量 GUI 作業、ノートで十分 |
| Reaper / 音響制作 | ノートPC | Windows or WSL2 | 主に CPU 負荷、ノートで実用 |
| **HD-2D シェーダ仕上げ / Visual テスト** | **デスクトップ** | Windows | RTX 2070S + 8C16T、ターゲット PC 性能で確認 |
| 重い Blender (高ポリ・サブディビジョン) | デスクトップ | Windows | GPU + メモリ余裕 |
| 大規模 Build / プロファイラ | デスクトップ | Windows | Build 時間短縮、計測精度 |
| Steam リリースビルド (最終) | デスクトップ | Windows | 品質保証、ターゲット環境 |

### 10.3 切替トリガー (デスクトップに赴くタイミング)

ノートPC での作業が以下に該当したら、commit + push でデスクトップに移動:

- HD-2D Tier 2 のシェーダ調整 (URP Renderer Feature の試行錯誤)
- ライティング / ポストプロセスの仕上げ
- Visual テスト (実機性能で動作確認)
- 大規模 Build (CI 相当の検証)
- 高ポリ Blender 作業 (サブディビジョン / リトポロジー)
- プロファイラ / Frame Debugger
- Steam リリース最終ビルド

> ノートPC の統合 GPU (VRAM 2GB) は HD-2D Tier 2 程度なら動くが、シェーダ調整時のトライ&エラーで thermal throttling、Visual テストで実機性能を見誤るリスクが高い。早めに切替えるのが安全。

### 10.4 同期戦略

| 同期対象 | 方法 |
|---|---|
| コード / docs (Anemora repo) | **Git remote** (GitHub Public 予定) |
| Obsidian / handover 文書 | **notes vault** (別 git repo) |
| Unity Personal ライセンス | 複数マシンで使用可 (Unity ID 共通) |
| Unity Asset Store 購入物 | 同一 Unity ID で両機ダウンロード可 |
| Unity プロジェクトの `Library/` `Temp/` | **Git 除外済** (`.gitignore` 反映済)、各機で再生成 |

### 10.5 ノートPC 単独運用の回避リスク (備忘)

- 統合 GPU (VRAM 2GB) で HD-2D シェーダ作業 → thermal throttling、編集ストレス
- Visual テストで実機性能を見誤る (統合 GPU 基準 → ターゲット PC で過剰品質と気づく)
- 大規模 Build 時間が伸びる (CPU は十分だが Disk IO 含めて差が出る)
- VRAM 2GB ではテクスチャアトラス確認が辛い

### 10.6 WSL2 と Windows のファイル共有

WSL2 (Ubuntu 想定) と Windows 間のファイル共有は以下を運用:

- **WSL2 → Windows**: `/mnt/c/...` 経由でアクセス可、ただし I/O は遅い
- **Windows → WSL2**: `\\wsl$\Ubuntu\...` でアクセス可
- **Anemora プロジェクト**: WSL2 ホーム配下に置く現状運用を維持
  - Unity Editor を Windows で起動する場合、`\\wsl$\...` 経由でプロジェクトを開く実用性は要検証 (起動遅延・ファイルロックの懸念)
  - 代替: Windows 側ホームに別ディレクトリで checkout、Git で同期する手もある (検証ポイント)

> **Stage 3 入口の検証タスク**: Unity install + プロジェクト初期化時に、WSL2 配下と Windows 配下のどちらに Unity プロジェクトを置くべきかを実機で判定する (ADR-0004 プロジェクトディレクトリ構造 で記録)。

---

## 11. 関連文書

- `CONCEPT.md` (Stage 1 v1.3、コンセプト固め)
- `PITCH.md` (Stage 2 公開ピッチ、10 章)
- `SPEC.md` (Stage 2 GDD v0.2、breadth-first 13 章)
- `docs/VS_SCOPE.md` (Stage 3 D トラック v0.2)
- `docs/STAGE3_TBD_RESOLUTION.md` (Stage 3 A トラック /spec 確定事項)
- `docs/adr/` (本 Stage で起草開始)
- `docs/devlog/` (Stage 3 devlog は完了時に Stage 全体まとめを起こす)
- ノートPC スペック (ローカル参照、リポジトリ外)
- デスクトップ スペック (ローカル memory、リポジトリ外)

---

## 12. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草 (7 トラック分解 + 依存グラフ + 入口) |
| v0.1 | 2026-05-04 | §10 開発環境の使い分け戦略を追加 (二拠点運用、WSL2/Windows、切替トリガー、同期戦略) |

---

> **End of Stage 3 計画書 v0.1**
