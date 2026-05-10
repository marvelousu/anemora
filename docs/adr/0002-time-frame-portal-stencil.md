# ADR-0002: Time Frame ポータルを URP + Stencil Buffer + Renderer Feature で実装する

## Status

Accepted (Stage 4 v1.2 で RenderGraph warning cleanup 済み)

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
- 開発機: ノートPC (統合 Radeon, VRAM 2GB) で軽量検証 → デスクトップ (RTX 2070S) で仕上げ (STAGE3_PLAN §10)

### E1 確定メモ (2026-05-05)

- URP 17.3.0 の `StencilUsage` 確認で、`StencilUsage.UserMask = 0b00001111` の範囲は user 用だが、`bit 4` (`0b00010000`) は `StencilLight` と競合することが判明した
- Anemora portal 用 stencil は user mask 内の最上位である **bit 3** に固定する。Unity shader の `Ref` は bit index ではなく実 stencil 値なので、E1 実装では **Mask = 8 / Ref = 8** として扱う
- E1 の最小描画では `PortalStencilFeature` が `AnemoraPortalMask` / `AnemoraPortalInside` の 2 pass を enqueue し、ポータル越しの表示を検証済み
- 自動 screenshot / smoke test と通常 URP 描画経路の defense in depth として、ポータル shader は `UniversalForward` pass と custom LightMode pass (`AnemoraPortalMask` / `AnemoraPortalInside`) を併置する

### Stage 4 v1.2 RenderGraph cleanup (2026-05-06)

- `PortalStencilFeature` は URP internal `DrawObjectsPass` 依存をやめ、public `RenderObjectsPass` に移行した
- `UnityEngine.Rendering.Universal.Internal` import は不要になった
- `PortalStencilFeature.SetLayerMasks()` は ADR-0005 atomic flip ordering の public API として維持する
- EditMode `32/32`、PlayMode `29/29`、Windows Standalone build success、30 秒 player log warning count `0` を確認した

### 重要度

VS_SCOPE §7 で **「FIX エリア (Stage 4 でも改修しない、コア機構のみ)」に時の窓ポータルシェーダ + ステンシル実装を含めている**。本機構の実装方針は VS の核体験を直接決定し、後続の Vertical Slice 制作 (E トラック) の前提となる。

---

## Decision

### 採用方針: URP Renderer Feature + Stencil Buffer + 別カメラレンダリング

#### 最小実装像

実装境界を固定するため、最小実装像を以下に定義する:

- ポータル枠は **1 枚の Quad メッシュ** で表現する
- ポータル内側は **1 つの専用カメラ** で描画する
- **再帰ポータルは採用しない**
- ポータル外側は通常カメラ、内側のみ **stencil test でマスク** する

#### URP 描画パスと Stencil ビット運用

- **URP は Forward Renderer を前提とする** (Deferred は採用しない)
- ポータル用途の **stencil ビットは本機構専用に予約** する。E0/E1 検証により URP 17.3.0 の `StencilUsage.UserMask = 0b00001111` 内から **`bit 3` (`0b00001000`)** を採用し、`bit 4` は `StencilLight` と競合するため使用しない
- ShaderLab Stencil 値は **Mask = 8 / Ref = 8** に固定する。`PortalMask.shader` は `Comp Always` + `Pass Replace` で bit 3 を書き込み、`InsideOnly.shader` は `Comp Equal` + `Pass Keep` で bit 3 領域のみ描画する
- Renderer Feature の挿入位置は **`RenderPassEvent` で明示** し、通常描画後にポータル内側描画を差し込む
- `PortalStencilFeature` は URP public `RenderObjectsPass` 経路で `AnemoraPortalMask` / `AnemoraPortalInside` の 2 pass を enqueue する。Stage 3 で使っていた URP internal `DrawObjectsPass` 経路は、Stage 4 v1.2 で RenderGraph warning cleanup のため廃止した

#### ポータル内側カメラの同期条件

- ポータル内側カメラは主人公カメラの **位置・向き・視野を同期** する
- **near / far clip は固定値で統一** する
- **culling mask は時代ごとに分離** する
- ポータル内描画は **非再帰**、**単一ポータル前提** とする

#### 構築手順

1. **ポータル四角枠は Quad メッシュ** (シェーダで Stencil Buffer に "ポータル内側" マークを書き込む)
2. **URP Renderer Feature** で追加レンダリングパスを構築:
   - 通常パス: 現在世界をレンダー (ステンシル外)
   - `AnemoraPortalMask`: `PortalMask.shader` の custom LightMode pass で **Stencil Ref 8 / Mask 8** を書き込む
   - `AnemoraPortalInside`: `InsideOnly.shader` の custom LightMode pass で Stencil テストに通った領域だけ別時代の世界をレンダー
   - defense in depth として、両 shader は通常描画向け `UniversalForward` pass も保持する
3. **別時代の描画**: ポータル内側用の専用カメラで、ポータル領域だけ描画される
4. **踏込み判定**: プレイヤーがポータル平面を越えたフレームで、メインシーン / ポータル内シーンの主従を反転 (この遷移詳細は ADR-0005 時間管理 / シーン切替で記録)

### 実装の核となる技術スタック

| 領域 | 技術 |
|---|---|
| レンダリング拡張 | URP Renderer Feature (`ScriptableRendererFeature`)、**Forward 固定** |
| マスク機構 | Stencil Buffer (URP の Stencil State 制御)、**専用ビット予約** |
| シェーダ | URP HLSL カスタムシェーダ (ポータル枠 + ポータル内表示)、`UniversalForward` + custom LightMode pass の dual-pass |
| シーン管理 | 過去/未来時代を別 GameObject ヒエラルキーで保持、レイヤー分離で別カメラに描画 |
| 踏込み遷移 | C# スクリプトでプレイヤー位置監視、シーン主従反転 (詳細は ADR-0005) |

### 責務分割 (実装と保守の境界)

- **Renderer Feature** は `AnemoraPortalMask` / `AnemoraPortalInside` の 2 pass enqueue と layer mask 選択を担当する
- **シェーダ** は stencil 書き込みとポータル境界表現を担当する。`UniversalForward` pass は通常 URP 経路 / smoke test 安定化用、custom LightMode pass は `PortalStencilFeature` 専用経路とする
- **C# 側** はポータル生成、カメラ同期、踏込み判定のみ担当する
- **`PortalStencilFeature.SetLayerMasks()`** は、ADR-0005 の atomic flip ordering で Current / Past の stencil 対象 layer を反転するための public setter として維持する

### ポータル用シーンのライフサイクル境界

- ポータル内側の時代シーンは、**常駐ヒエラルキー** または **Additive 読み込み** のいずれかを ADR-0005 (時間管理 / シーン切替) で確定する
- 本 ADR では、描画上は **単一ポータル・単一時代** の前提のみを保証する

### HD-2D Tier 2 との整合

- 動的影 (Tier 2) は **stencil そのものとは独立** だが、URP の **内部予約ビットやレンダリング設定と干渉しないことを実機で確認** する
- 影・ポータル・ライトレイヤーの組み合わせは **Stage 3 E トラックで検証** し、必要なら **Forward 固定と予約ビット運用に寄せる**
- 単一方向光環境のため、ポータル内外で光源を統一 / 個別化する選択は VS 試作で判定 (Stage 3 E トラック中)
- ポータル内外で **Volume Profile を分けるか、世界全体で 1 つにするかは Stage 3 E トラックで比較検証** する。差分が大きい場合のみ時代別切替を採用し、VS では最小構成を優先する

### 制約

- **同時ポータル数 = 1** — 複数枠の同時操作は採用しない (SPEC §5.1.3 確定済)
- **クールダウンなし** — 詰み防止のため何度でも再描画可能 (VS_SCOPE §3.1 詰み防止と整合)
- **時間侵食状態では時の窓の再描画を禁止する** — ただしこの状態遷移の定義は ADR-0005 で行い、本 ADR では描画機構の制約としてのみ扱う (VS では時間侵食状態を発動させない方針も VS_SCOPE §3.1 / ADR-0005 側の判断)

---

## Consequences

### 利点

- **Anemora の Hook (3D ポータルに立体ジオラマが立ち上がる) を直接実現できる** — テクスチャ平面ではない、奥行きのあるポータル体験
- **URP Renderer Feature は再利用可能** — 痕跡可視化 / 層遷移片鱗演出 / 違和感ハイライトなど他の VFX に流用できる
- **公式ドキュメント + コミュニティ事例が豊富** — Unity 公式 URP サンプル、Brackeys / Code Monkey / Catlike Coding 等の portal effect 実装事例
- **Stencil ベースによりポータル内外を空間的に分離しやすい** — Render Texture 平面方式と異なり、ポータル内の物体が枠の奥行きに合わせて描画される。深度感は stencil そのものではなく、別カメラ同期 + 深度処理 + クリップ設計の組合せで確保する

### 欠点 / 注意点

- **シェーダ作業に HLSL カスタムシェーダが必要** — URP のシェーダグラフだけでは Stencil 制御が完結しないケースがある、HLSL を直接書く工程が発生
- **マルチパスレンダリングで GPU 負荷増** — ノートPC の統合 Radeon (VRAM 2GB) で動作確認必須、VS_SCOPE §7 FIX エリアの実装は **デスクトップ (RTX 2070S) での仕上げ** が前提 (STAGE3_PLAN §10.2 切替トリガー)
- **HD-2D Tier 2 動的影との干渉確認が必須** — 動的影自体は stencil とは独立だが、URP の内部予約ビット・レンダリング設定との非競合を実機検証で確認する。必要なら Forward 固定 + 予約ビット運用に寄せる (`_RenderingLayerMask` 経由の分離も検証候補)
- **URP public pass 依存へ移行済み** — E1/E4 時点では `DrawObjectsPass` internal API に依存していたが、Stage 4 v1.2 で public `RenderObjectsPass` へ移行した。今後 URP package が `RenderObjectsPass` API を変える場合は、RenderGraph-capable custom pass への再移行を検討する
- **ポータル踏込み時のシーン遷移ロジックは別 ADR** — ADR-0005 (時間管理 / シーン切替) で詳細化、本 ADR では「踏込みフレームで主従反転」という方針のみ定義
- **複数ポータルの同時描画は将来も拡張しない方針** — 現方針で複数描画したい場合は Renderer Feature の大幅改修が必要、Stage 4 以降に新案として議論する場合は本 ADR の Superseded として別 ADR を起こす

### 後続への影響

- **ADR-0005 (時間管理 / シーン切替)**: 本 ADR の踏込み遷移を詳細化。E4 では `PortalStencilFeature.SetLayerMasks()` を atomic flip ordering の一部として使い、Current / Past の stencil 対象 layer を反転する
- **ADR-0007 (UI フレームワーク)**: シンボル選択 UI (赤のみ選択可、白/青グレーアウト、VS_SCOPE §3.1) との連携
- **VS_SCOPE §7 FIX エリア**: 時の窓ポータルシェーダ + ステンシル実装は **Stage 4 でも改修しない FIX** とする方針を本 ADR が裏打ち
- **ADR-0003 (アセットパイプライン)**: ポータル枠 Quad の VFX (時間境界エフェクト) は AI 生成 + 手仕上げのパイプラインで作成
- **ADR-0004 (プロジェクトディレクトリ構造)**: `Assets/Scripts/TimeManagement/Portal/`、`Assets/Art/Materials/Portal/`、`Assets/Settings/` 等の配置と整合

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
- 静的表示と踏込み時演出の責務が分かれ、デバッグ経路が増える
- VS の初期段階では「ポータルが見える」「踏み込める」「反映される」を先に安定化すべきで、フェード追加は後段で十分
- 体験向上は見込めるが、VS 完成条件 (`VS_SCOPE.md` §8 必須) に対する寄与は限定的
- Stencil の動的更新コストが計測必要

**判定:** **保留**。E トラックで基本系 (候補 A) が安定し、踏込み演出の違和感が残る場合のみ再検討する。Stage 4 以降の体験向上案として候補に残す。

### 候補 E: 独自レンダリングパイプライン

**実装:** SRP (Scriptable Render Pipeline) の独自実装

**判定:** **不採用**。1 ヶ月集中スコープと完全に矛盾、URP の表現力で十分。

---

## 検証ポイント (Stage 3 E トラックで実機確認)

VS 制作開始時の Vertical Slice プロトタイプで以下を検証:

### 機能動作

1. **Stencil Buffer + Renderer Feature の基本動作** — 単純な四角ポータルで内側に別オブジェクトが描画されるか。E1 では `StencilBit = 3` / `StencilMask = 8` / shader `Ref = 8` で検証済み
2. **HD-2D Tier 2 動的影との干渉** — ポータル内外で動的影が破綻しないか、Stencil ビット競合がないか、Forward 固定 + 予約ビット運用への寄せが必要か。URP 17.3.0 では `bit 4` が `StencilLight` と競合するため、portal は bit 3 を使う
3. **踏込み遷移の自然さ** — ポータル平面を越えた瞬間の主従反転に違和感がないか
4. **複数枠の同時表示制限** — 1 ポータル前提のロジックが破綻しないか (UI / コアループ / 遷移の全層で破綻なし)

### 描画品質と GPU 負荷

5. **GPU 負荷 (ノートPC 統合 Radeon)** — 60 FPS 維持できるか、ノートPC でのフレーム時間内訳が許容範囲か、できない場合のフォールバック (Tier 2 → Tier 1 への退避基準)
6. **Visual テスト基準** — デスクトップ (RTX 2070S) でターゲット品質を確認、ノート PC との見え方差を記録

### 可視アーティファクト

7. **可視アーティファクトの確認** — 枠の縁のにじみ、Z-fighting、カメラ移動時のずれ、UI 前後関係、プレイヤーが枠に近づいた時のクリッピング破綻、ポータル枠の内外で描画順が安定するか
8. **ビルド版と Editor の差** — Windows ビルド版で Editor 動作と差がないか (Editor 依存ではない確認、`VS_SCOPE.md` §8 必須「Windows ビルド起動」と整合)

検証で破綻が出たら本 ADR を改訂、または別 ADR (Superseded) で記録する。

---

## 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v1.0 | 2026-05-04 | 初版。Time Frame ポータルを URP + Stencil Buffer + Renderer Feature で実装する方針を定義 |
| v1.1 | 2026-05-05 | E1 確定値反映。Stencil bit 3 / Mask 8 / Ref 8、dual-pass shader、`DrawObjectsPass` internal API caveat、`PortalStencilFeature.SetLayerMasks()` public setter を追記 |
| v1.2 | 2026-05-06 | Stage 4 RenderGraph warning cleanup を反映。`PortalStencilFeature` を URP internal `DrawObjectsPass` から public `RenderObjectsPass` へ移行し、Player log warning count `0` を確認 |

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
- HD-2D 風シェーダ事例: HD-2D 系既存作の公開資料 / 大手 HD-2D タイトルの GDC 講演

### Anemora 内部文書

- `ADR-0001` (エンジン Unity 6.3 LTS 採用)
- `docs/devlog/2026-05-06_urp_renderobjects_pass_migration.md`
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
- `ADR-0004`: プロジェクトディレクトリ構造 — `Assets/Scripts/TimeManagement/Portal/`, `Assets/Art/Materials/Portal/` 等の配置
- `ADR-0005`: 時間管理 / シーン切替 — 踏込み時の主従反転詳細
- `ADR-0007`: UI フレームワーク (uGUI vs UI Toolkit) — シンボル選択 UI の実装
