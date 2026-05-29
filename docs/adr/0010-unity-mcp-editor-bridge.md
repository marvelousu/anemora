# ADR-0010: Unity Editor 操作の MCP ブリッジ採用 (CoderGamester/mcp-unity) と運用ガード

## Status

Accepted

## Date

2026-05-18 (Stage 4)

## Context

anemora の慢性課題は、コード/スクリプト編集とシーン組み立ての断絶にある。具体的には次が再発している。

- provenance ギャップ: Refresh のみ実行され Apply/Integrator が回らず、Integrator が行うはずのシーン組み立て編集がデプロイされない。検証はシーン資産の grep に依存している。
- TimeWindow アパーチャ: 地面縦窓 + 俯瞰カメラで視錐台が地面下へ落ちる構造バグ。content 数ゲート PASS でも黒。最終的に PNG 目視でしか捕捉できなかった。
- 図書館二重床など、解決後 (instantiate / prefab override 適用後) に初めて創発する構造不整合。

これらはテキスト編集エージェント (Claude Code / Codex CLI) が `.unity` / `.prefab` の YAML を確実には触れないことに起因する。現行パイプラインはシーン組み立てを Integrator スクリプトに委ね、結果を grep でしか確認できない開ループになっている。

調査の発端はユーザーからの「anemora の開発に Synaptic Code が有用かを調査」という依頼であり、その比較の中で「Unity をエージェントが直接操作する能力」自体は anemora の最大の弱点に対応すると判明した。本 ADR はその能力の導入方針と、導入に伴う運用ガードを確定する。

### 関連文書 / 制約

- ADR-0004: Unity project directory structure
- ADR-0009: アセット制作パイプライン正式手順 (中間物 gitignore / 最終物 + 再現手順のみ commit、pathspec 限定 staging)
- PITCH.md §8: AI-Driven Solo Production Pipeline (Claude 設計/レビュー + Codex 実装のクロスモデル運用)
- memory: anemora pipeline provenance gap / TimeWindow aperture / gfx quality bar / smoke bypasses Update
- 制約: Unity 6000.3.14f1 / URP 17.3.0。Claude Code と Codex CLI のクロスモデル運用を分断しないこと。GitHub Public に非再現的なローカル解決値を commit しないこと。

## Decision

### 1. Unity MCP ブリッジを採用する

`CoderGamester/mcp-unity` (MIT, UPM git URL 配布) を採用する。

判断根拠:

- **Claude Code (`.mcp.json`) と Codex CLI (`.codex/config.toml`) の両方の設定が公式に文書化されている唯一の候補**。anemora の制約はクロスモデル運用そのものであり、ここが決定打。
- Unity 6+ 必須に合致 (anemora は 6000.3.14f1)。URP 環境で動作。
- `select_gameobject` / `get_gameobject` / `update_gameobject` / `update_component` / `set_transform` / `get_scene_info` 等、解決後シーングラフの問い合わせと構造編集を提供。
- WebSocket transport、Unity Editor 起動 + サーバ手動起動が前提 (port 8090)。

### 2. Synaptic Code は採用しない

理由:

- Claude Code の「代替」であって「拡張」ではない。ローカル LM Studio モデルへの置換は、TimeWindow ステートマシンや ActionRecord 時間因果のような難所で明確な推論格下げになる。anemora の律速はそこではない。
- 唯一刺さる Unity/Blender ブリッジが Synaptic 自身のクライアントに閉じており (HTTP、外部エージェント駆動口の公開なし)、Codex クロスレビュー運用を捨てることになる。
- 成熟度リスク (単独開発・公開リポジトリは認証コードのみ・v0.1 系)。

### 3. 運用ガード (本 ADR の中核 — 導入の前提条件)

MCP の価値は「構造バグ検知の閉ループ化」一点に限定され、以下を運用に縛れたときのみ実利になる。縛れない場合、MCP はむしろ provenance を悪化させるため導入してはならない。

- **G1 (編集のコード化)**: MCP による対話的シーン編集を、コード化せずに主要なシーン authoring 経路にしてはならない。MCP で行った構造変更は、必ず Integrator / シーン構築スクリプトへ反映し、checked-in の再現可能な形に固定化する。ad-hoc な MCP 編集は調査・検知・プロトタイプに限る。
- **G2 (視覚ゲートの維持)**: MCP が提供するのは構造 (hierarchy / transform / component) の知覚であり、視覚品質判断ではない。MCP の構造アサーションは PNG / Play モードの人間目視ゲートを代替しない。「構造アサート緑」だけで完了 (green) としない。TimeWindow アパーチャと smoke bypass の履歴がこの根拠。
- **G3 (規律がツールに先行する)**: provenance ギャップの根因は道具不足ではなく Apply/Integrator を回さない規律。MCP はその検知を grep/目視の後追いから閉ループ assert へ前倒しするものであって、オーケストレーション修正の代替ではない。Apply/Integrator を必ず回す規律の確立を MCP 活用に先行させる。

### 4. 適用範囲

- 対象: provenance ギャップ検知、カメラ/視錐台と地面 bounds の構造判定、同座標 overlapping renderer 列挙など、構造で表現できるバグクラスの閉ループ検知。
- 非対象: 視覚品質の判定、実装の推論品質・速度。これらは MCP では改善しない。

## Consequences

### 利点

- 構造バグの検知が「grep / 目視で遅れて発見」から「閉ループで assert 可能」へ前倒しになる。
- Claude Code と Codex CLI の双方から駆動でき、既存のクロスモデル運用 (PITCH.md §8) を温存したまま additive に能力追加できる。
- ローカル LLM への置換やベンダーロックを伴わない (MIT, UPM git)。

### 欠点 / 注意点 / 検証境界

- **ライブ検証境界**: MCP ↔ Unity Editor のハンドシェイクは Editor GUI 起動 + サーバ手動起動が必要で、ヘッドレスでは検証できない。本 ADR 受理時点で完了しているのは「設定・依存・文書の基盤整備」までであり、ハンドシェイク疎通はローカルでの手動検証が残る (手順: `tools/mcp/SETUP.md`)。
- **G1 違反時の悪化**: MCP 編集をコード化せず主経路にすると、checked-in スクリプトより再現性が落ち provenance はむしろ悪化する。
- **浮動依存**: UPM git URL は ref 未固定だと解決時点の既定ブランチを引く。再現性アンカーは Unity 初回解決時に書かれる `Packages/packages-lock.json` の解決 SHA であり、これを必ず commit する。検証後に `#vX.Y.Z` 固定へ移行してよい。
- ツール追加分の保守 (Unity 版との結合、サーバ起動運用) が増える。

### 後続への影響

- ADR-0004 の directory layout と ADR-0009 の staging/中間物規律に従う (本 ADR の設定テンプレートは中間物扱いとして gitignore、`.example` と SETUP のみ commit)。
- G1-G3 は Stage 4 以降のシーン組み立て作業の前提 gate になる。

## Alternatives

### 候補 B: Synaptic Code

不採用。理由は Decision §2 を参照。推論格下げ・ブリッジのクライアント閉じ込め・成熟度リスク。

### 候補 C: CoplayDev/unity-mcp

将来の代替として保留。9.7k stars と高成熟だが、Claude Code 設定の具体例がなく Codex CLI への言及なし。anemora の Codex 側が文書化されておらずクロスモデル運用が分断されるため、現時点では非選択。CoderGamester 側に重大な障害が出た場合の第一代替とする。

### 候補 D: 他 OSS (AnkleBreaker-Studio/unity-mcp-plugin 268 tools, Bluepuff71/UnityMCP 等)

不採用 (現時点)。ツール網羅性は高いが、Claude Code + Codex 双方の設定可否が未確認。クロスモデル運用の確証が取れる候補を優先する。

### 候補 E: ツール導入せず Apply/Integrator 規律のみ修正

一部採用。G3 がこれを MCP に先行する前提として取り込んでいる。ただし規律修正は開ループ問題 (解決後創発バグの不可視性) を解消しないため、規律修正のみでは不十分。

## References

### Anemora 内部文書

- ADR-0004 — Project Directory Structure
- ADR-0009 — アセット制作パイプライン正式手順 (staging / 中間物規律)
- PITCH.md §8 — AI-Driven Solo Production Pipeline
- `docs/devlog/2026-05-18_unity_mcp_foundation.md` — 本決定の調査・基盤整備記録
- `tools/mcp/SETUP.md` — ローカル最終化と疎通検証手順

### 外部 tool / source

- CoderGamester/mcp-unity — https://github.com/CoderGamester/mcp-unity (MIT, v1.3.0 2026-04-26)
- CoplayDev/unity-mcp — https://github.com/CoplayDev/unity-mcp (MIT, 代替候補)
- Synaptic Code — https://www.synaptic-ai.net/code / https://github.com/miu-chang/Synaptic-Code (不採用)

---

## 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0.1 | 2026-05-18 | Synaptic Code 調査から派生。CoderGamester/mcp-unity 採用、Synaptic 不採用、運用ガード G1-G3、適用範囲、検証境界を定義し Accepted で起草 |
