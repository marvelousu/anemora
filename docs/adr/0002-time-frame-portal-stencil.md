# ADR-0002: Time Frame ポータルを URP + Stencil Buffer + Renderer Feature で実装する

## Status

Accepted (実装は Stage 3 E トラックで検証 → 必要なら改訂)

## Date

2026-05-04 (Stage 3 Day 0)

## Context

Anemora の中核機構である **時の窓 (Time Frame)** は、3D 空間中の任意の位置に「四角枠」を描き、その内側に該当時代 (赤=過去 / Stage 4 以降に白=現在 / 青=未来) のジオラマが立体的に立ち上がる体験を提供する。プレイヤーは枠を踏み越えて該当時代に「踏込み」、能動行動 (持ち帰る等) を行う。

### 機能要件 (SPEC.md §5.1 / VS_SCOPE.md §3.1)

1. プレイヤー入力で空中に四角枠を描画 (主人公の正面、サイズ固定)
2. 枠の内側に **該当時代のシーンが立体的に表示される** (テクスチャ平面ではなく 3D 空間として奥行きが見える)
3. 枠の外側は通常の現在世界
4. プレイヤーが枠を踏み越えると該当時代へ遷移
5. 同時生成枠は 1 のみ (複数枠操作は採用しない、SPEC §5.1.3)
6. 枠生成中は時間がスローモーション or 停止 (TBD、Stage 3 試作で確定)

### 技術前提

- Unity 6.3 LTS + URP (Universal Render Pipeline) を採用 (ADR-0001)
- HD-2D Tier 2 (動的影 + 単一方向光) を採用、Tier 3-4 (volumetric / sprite normal map / multiple lights) は不採用
- 固定アイソメ視点 (自由視点排除、SPEC §7.4)
- 開発機: ノート PC TOM (統合 Radeon, VRAM 2GB) で軽量検証 → デスクトップ UJPVOG2 (RTX 2070S) で仕上げ (STAGE3_PLAN §10)

### 重要度

VS_SCOPE §7 で **「FIX エリア (Stage 4 でも改修しない、コア機構のみ)」に時の窓ポータルシェーダ + ステンシル実装を含めている**。本機構の実装方針は VS の核体験を直接決定し、後続の Vertical Slice 制作 (E トラック) の前提となる。

---

## Decision

### 採用方針: URP Renderer Feature + Stencil Buffer + 別カメラレンダリング

時の窓ポータルは以下の実装パターンで構築する:

1. **ポータル四角枠は Quad メッシュ** (シェーダで Stencil Buffer に "ポータル内側" マークを書き込む)
2. **URP Renderer Feature** で追加レンダリングパスを構築:
   - 通常パス: 現在世界をレンダー (ステンシル外)
   - ポータルパス: Stencil テストでマスク領域内のみ別時代の世界をレンダー
3. **別時代の描画**: ポータル内側用に別カメラ or 別シーンを用意し、ポータル領域だけ描画される
4. **踏込み判定**: プレイヤーがポータル平面を越えたフレームで、メインシーン / ポータル内シーンの主従を反転 (この遷移詳細は ADR-0005 時間管理 / シーン切替で記録)

### 実装の核となる技術スタック

| 領域 | 技術 |
|---|---|
| レンダリング拡張 | URP Renderer Feature (`ScriptableRendererFeature`) |
| マスク機構 | Stencil Buffer (URP の Stencil State 制御) |
| シェーダ | URP HLSL カスタムシェーダ (ポータル枠 + ポータル内表示) |
| シーン管理 | 過去/未来時代を別 GameObject ヒエラルキーで保持、レイヤー分離で別カメラに描画 |
| 踏込み遷移 | C# スクリプトでプレイヤー位置監視、シーン主従反転 (詳細は ADR-0005) |

### HD-2D Tier 2 との整合

- 動的影 (Tier 2) は Stencil Buffer の利用ビットと競合しない (Unity URP の標準 Shadowmap は別バッファ)
- 単一方向光環境のため、ポータル内外で光源を統一 / 個別化する選択は VS 試作で判定 (Stage 3 E トラック中)
- カラーグレーディング差を時代別に与える場合は Volume Profile を時代別に切替

### 制約

- **同時ポータル数 = 1** — 複数枠の同時操作は採用しない (SPEC §5.1.3 確定済)
- **クールダウンなし** — 詰み防止のため何度でも再描画可能 (VS_SCOPE §3.1 詰み防止と整合)
- **時間侵食状態時のみ描画不可** — VS では発動させない (VS_SCOPE §3.1)

---

## Consequences

### 利点

- **Anemora の Hook (3D ポータルに立体ジオラマが立ち上がる) を直接実現できる** — テクスチャ平面ではない、奥行きのあるポータル体験
- **URP Renderer Feature は再利用可能** — 痕跡可視化 / 層遷移片鱗演出 / 違和感ハイライトなど他の VFX に流用できる
- **公式ドキュメント + コミュニティ事例が豊富** — Unity 公式 URP サンプル、Brackeys / Code Monkey / Catlike Coding 等の portal effect 実装事例
- **Stencil ベースなので深度感が保たれる** — Render Texture 平面方式と異なり、ポータル内の物体がポータル枠の奥行きに合わせて描画される

### 欠点 / 注意点

- **シェーダ作業に HLSL カスタムシェーダが必要** — URP のシェーダグラフだけでは Stencil 制御が完結しないケースがある、HLSL を直接書く工程が発生
- **マルチパスレンダリングで GPU 負荷増** — ノート PC TOM の統合 Radeon (VRAM 2GB) で動作確認必須、VS_SCOPE §7 FIX エリアの実装は **デスクトップ UJPVOG2 (RTX 2070S) での仕上げ** が前提 (STAGE3_PLAN §10.2 切替トリガー)
- **HD-2D Tier 2 動的影との干渉確認が必須** — Stencil 使用ビットの競合検証 (Unity URP 標準では `_RenderingLayerMask` 経由で分離可能だが、実機検証で確認)
- **ポータル踏込み時のシーン遷移ロジックは別 ADR** — ADR-0005 (時間管理 / シーン切替) で詳細化、本 ADR では「踏込みフレームで主従反転」という方針のみ定義
- **複数ポータルの同時描画は将来も拡張しない方針** — 現方針で複数描画したい場合は Renderer Feature の大幅改修が必要、Stage 4 以降に新案として議論する場合は本 ADR の Superseded として別 ADR を起こす

### 後続への影響

- **ADR-0005 (時間管理 / シーン切替)**: 本 ADR の踏込み遷移を詳細化
- **ADR-0007 (UI フレームワーク)**: シンボル選択 UI (赤のみ選択可、白/青グレーアウト、VS_SCOPE §3.1) との連携
- **VS_SCOPE §7 FIX エリア**: 時の窓ポータルシェーダ + ステンシル実装は **Stage 4 でも改修しない FIX** とする方針を本 ADR が裏打ち
- **ADR-0003 (アセットパイプライン)**: ポータル枠 Quad の VFX (時間境界エフェクト) は AI 生成 + 手仕上げのパイプラインで作成
- **ADR-0004 (プロジェクトディレクトリ構造)** (Windows Codex が B トラックで起草): `Assets/Scripts/TimeFrame/`, `Assets/Shaders/Portal/`, `Assets/Renderer/` 等のディレクトリを想定

---

## Alternatives

### 候補 B: Render Texture + ポータル平面方式

**実装:** 別カメラで過去/未来シーンをレンダー → Render Texture に書き出し → ポータル平面 (Quad) にテクスチャを貼る

**利点:**
- 実装シンプル、Renderer Feature 不要
- Stencil 制御不要

**欠点:**
- ポータル内の物体に「奥行き」が出ない (テクスチャ平面)、Anemora の Hook (立体ジオラマ) が成立しない
- Anemora の核体験を直接損なう

**判定:** **不採用**。Anemora の Hook を満たさない。

### 候補 C: シーン分割 + フェードトランジション

**実装:** 時の窓を「枠」ではなく「シーン切替」として実装、ポータルを覗くとフェードで切替、戻るとフェードで戻る

**利点:**
- ステンシル不要、URP 標準機能のみで実装可能
- 実装最も軽量

**欠点:**
- 時の窓が「立体的に立ち上がる」体験にならない、Anemora の Hook を損なう
- トレイラー素材として弱い (PITCH §3 シグネチャー画が成立しない)

**判定:** **不採用**。Hook 強度が出ない、Anemora の独自性が消える。

### 候補 D: ハイブリッド (静的=Stencil、動的=フェード)

**実装:** 静的描画は候補 A (Stencil + Renderer Feature)、踏込みでフェード切替

**利点:**
- 視覚的に Anemora らしい + 踏込み実装が簡単

**欠点:**
- 複雑度が高い、VS の Day 0-10 スコープに対して過剰
- Stencil の動的更新コストが計測必要

**判定:** **保留**。VS では候補 A 単独で十分、Stage 4 以降に体験向上の余地として再評価する場合に検討。

### 候補 E: 独自レンダリングパイプライン

**実装:** SRP (Scriptable Render Pipeline) の独自実装

**判定:** **不採用**。1 ヶ月集中スコープと完全に矛盾、URP の表現力で十分。

---

## 検証ポイント (Stage 3 E トラックで実機確認)

VS 制作開始時の Vertical Slice プロトタイプで以下を検証:

1. **Stencil Buffer + Renderer Feature の基本動作** — 単純な四角ポータルで内側に別オブジェクトが描画されるか
2. **HD-2D Tier 2 動的影との干渉** — ポータル内外で動的影が破綻しないか、Stencil ビット競合がないか
3. **GPU 負荷 (ノート PC 統合 GPU)** — 60 FPS 維持できるか、できない場合のフォールバック (Tier 2 → Tier 1 への退避基準)
4. **踏込み遷移の自然さ** — ポータル平面を越えた瞬間の主従反転に違和感がないか
5. **複数枠の同時表示制限** — 1 ポータル前提のロジックが破綻しないか
6. **Visual テスト基準** — デスクトップ UJPVOG2 (RTX 2070S) でターゲット品質を確認、ノート PC との見え方差を記録

検証で破綻が出たら本 ADR を改訂、または別 ADR (Superseded) で記録する。

---

## References

### 公式

- [Unity URP Renderer Feature 公式](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/manual/urp-renderer-feature.html)
- [Unity ShaderLab Stencil 公式](https://docs.unity3d.com/Manual/SL-Stencil.html)
- [URP Custom Renderer Feature サンプル](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/manual/customize/custom-pass-injection-points.html)

### コミュニティ事例 (実装着手時に参照)

- Brackeys "Portals in Unity" 系チュートリアル
- Code Monkey "Portal Effect Unity"
- Catlike Coding "Custom SRP" シリーズ
- HD-2D 風シェーダ事例: Sea of Stars (公開資料があれば) / Octopath Traveler GDC 講演

### Anemora 内部文書

- `ADR-0001` (エンジン Unity 6.3 LTS 採用)
- `SPEC.md` §5.1 (時の窓システム機能要件)
- `SPEC.md` §10.1-10.2 (Technology / 主要技術)
- `VS_SCOPE.md` §3.1 (コアループ要素 / グレーアウト方針 / 詰み防止)
- `VS_SCOPE.md` §4.3 (VFX / シェーダ FIX エリア)
- `VS_SCOPE.md` §7 (FIX / 暫定完成 / プレースホルダ可の境界)
- `STAGE3_PLAN.md` §10 (開発環境の使い分け、ノート / デスクトップ切替)
- `PITCH.md` §3 (Signature Moment、30 秒トレイラー想定)
- `PITCH.md` §8 (AI-Driven Solo Production Pipeline、技術スタック)

### 関連 ADR (本 ADR と相互参照)

- `ADR-0001`: エンジン Unity 6.3 LTS 採用 (本 ADR の前提)
- `ADR-0004` (Windows Codex 起草中): プロジェクトディレクトリ構造 — `Assets/Scripts/TimeFrame/`, `Assets/Shaders/Portal/` 等の配置
- `ADR-0005` (今後起草): 時間管理 / シーン切替 — 踏込み時の主従反転詳細
- `ADR-0007` (今後起草): UI フレームワーク (uGUI vs UI Toolkit) — シンボル選択 UI の実装
