# ADR-0001: エンジン Unity 6.3 LTS 採用

## Status

Accepted

## Date

2026-05-04 (Stage 3 Day 0)

## Context

Anemora の中核機構である「時の窓 (Time Frame)」は、3D 空間に立ち上がるステンシルポータルを介して過去/未来時代へ踏込む体験を提供する。本機構の実装には、ステンシルバッファ + ポストプロセス + シェーダ拡張が密結合する設計が必要。

加えて以下の要件:

- HD-2D Tier 2 (動的影 + 単一方向光、SPEC §1.3 / §7.1) のシェーダ実装が必要
- ドット絵スプライト + 低ポリ 3D の混在 (HD-2D 系) を破綻なく描画
- Windows / Linux / Mac 向けクロスビルド (PITCH §1)
- AI 開発支援 (Claude × Codex) の知識ベースが充実していること
- 1 ヶ月集中開発スケジュール (PITCH §9) に耐えるドキュメント / 学習リソースの厚み

候補は以下:

- Unity 6.3 LTS + URP
- Godot 4.6 (4.5 以降ネイティブステンシル対応)
- Unreal Engine 5.x
- 独自エンジン

## Decision

**Unity 6.3 LTS + URP (Universal Render Pipeline) を本線エンジンとして採用する。**

- Renderer Feature による Stencil 制御で時の窓ポータルを実装
- URP カスタムシェーダ (HLSL) で HD-2D Tier 2 (動的影 + 単一方向光) を構築
- Build Target: Windows / Linux / Mac
- ライセンス: Personal Edition (収益条件以下のため無償利用可)

## Consequences

### 利点

- ステンシルバッファ + Renderer Feature の事例 / アセット / コミュニティが豊富
- HD-2D 風シェーダの実装事例が多数 (Octopath / Triangle Strategy 系の研究記事)
- C# の表現力と AI コード支援 (Claude / Codex の C# 知識) の親和性が高い
- Unity Asset Store で時間 / VFX / UI 系の補助資産が入手可能
- マルチプラットフォーム対応の手数が最小

### 欠点 / 注意点

- ライセンス改訂リスク (過去事例あり) を継続監視。撤退路として Godot 4.6 を保持
- Unity Hub / ライセンス認証が初回起動時に必要、Linux での動作確認が Day 1-2 タスク
- インストールサイズが大きい (10-20 GB)
- URP のバージョン互換性に注意 (6.3 LTS の URP 固定運用)

### 後続への影響

- ADR-0002 で URP + Stencil + Renderer Feature の具体実装方針を起こす
- ADR-0004 で Unity 標準のディレクトリ構造 + 拡張ルールを定義
- 全実装作業は B (技術セットアップ) 完了 = Unity プロジェクト初期化後に着手

## Alternatives

### Godot 4.6 (撤退候補として保持)

- 4.5 以降ネイティブステンシル対応で技術的に実現可能
- OSS / 軽量 / Linux 開発との親和性が高い
- HD-2D 風実装事例は Unity に比べて少ない
- AI 支援知識ベースが Unity より薄い
- → **撤退候補として保持**: Unity で時の窓実装が破綻した場合の移行先

### Unreal Engine 5.x

- HD-2D 系の表現には不向き (高品質 3D 寄りの設計思想)
- 個人開発のスコープに対して過剰
- → **不採用**

### 独自エンジン

- 1 ヶ月集中開発スコープと完全に矛盾
- AI 主体個人開発の優位性 (既存エコシステム活用) を失う
- → **不採用**

## 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v1.0 | 2026-05-04 | 初版。Unity 6.3 LTS + URP を本線エンジンとして採用 |

## References

- [Unity 6.3 LTS Release Notes](https://unity.com/releases/lts) (TBD: 公式 URL は記事公開時に確定)
- [URP Documentation](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest)
- SPEC.md §10.1 (Technology / Engine 選定)
- PITCH.md §8.4 (Engine + Dev Environment Stack)
- Stage 2 Codex (fast) レビュー: PITCH と SPEC の Engine 確定度を統一する P0 指摘 (2026-05-04)
