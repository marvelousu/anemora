# 2026-05-18 Unity MCP 基盤整備 (Synaptic Code 調査からの派生)

Date: 2026-05-18

## Summary

ユーザー依頼「anemora の開発に Synaptic Code が有用かを調査」から開始。比較調査の結果、Synaptic Code 本体は不採用とし、その過程で浮上した「エージェントが Unity Editor を直接操作する能力」を `CoderGamester/mcp-unity` で導入する方針を ADR-0010 として確定。本セッションでは設定・依存・文書の基盤整備までを実施した。ライブ疎通検証は手動で残る。

セッション: 主対話セッション (Claude, Opus 4.7 1M)。本作業は実装ではなく調査 + 文書/設定基盤パスのため、ADR-0009 の docs/infra パス形式で記録する。

## ユーザープロンプト (verbatim 抜粋)

- 「anemmoraの開発にsynaptic codeが有用かどうかを調査してください」
- 「現在の運用以上の価値がMCP利用に見いだせるということですか？」
- 「その口調は何ですか？結論改善するなら、導入を進めてください。あなたが基盤を作ってください。また、その場合devlogも作成してください」

3 番目のプロンプトで、(a) 文体の是正指示、(b) MCP 導入の実行指示、(c) devlog 作成指示を受けた。文体是正は別途 user memory へ記録。

## Inputs

- Synaptic Code: https://www.synaptic-ai.net/code, https://github.com/miu-chang/Synaptic-Code
- MCP 候補: CoderGamester/mcp-unity, CoplayDev/unity-mcp, AnkleBreaker-Studio/unity-mcp-plugin 他
- anemora 構成: Unity 6000.3.14f1 / URP 17.3.0 (`Packages/manifest.json`)
- 規約: `docs/adr/README.md`, `docs/adr/0009-asset-pipeline.md`, `docs/devlog/README.md`, `docs/devlog/INDEX.md`
- memory: provenance gap / TimeWindow aperture / gfx quality bar / smoke bypasses Update

## Result / 決定

- **Synaptic Code 不採用**: Claude Code の代替であり拡張ではない。ローカル LLM 置換は難所で推論格下げ。Unity/Blender ブリッジが自前クライアントに閉じ Codex クロスレビューを捨てることになる。成熟度リスク (単独開発・v0.1 系)。
- **CoderGamester/mcp-unity 採用 (ADR-0010, Accepted)**: Claude Code (`.mcp.json`) と Codex CLI (`.codex/config.toml`) 双方の設定が公式文書化された唯一の候補。クロスモデル運用を分断しない決定打。MIT / UPM git / Unity 6+ 適合。
- **CoplayDev/unity-mcp は第一代替として保留**: 高成熟だが Codex 設定が未文書化。
- **運用ガード G1-G3 を導入前提として確定** (ADR-0010 §3): G1 MCP 編集の必ずコード化、G2 視覚ゲート維持 (構造アサートは PNG/Play 目視を代替しない)、G3 Apply/Integrator 規律が MCP に先行。MCP の価値は「構造バグ検知の閉ループ化」一点に限定。

### 生成 / 更新ファイル

- 新規: `docs/adr/0010-unity-mcp-editor-bridge.md`
- 新規: `tools/mcp/SETUP.md` (ローカル最終化 + 疎通検証手順)
- 新規: `.mcp.json.example`, `.codex/config.toml.example` (placeholder 入りテンプレート)
- 更新: `Packages/manifest.json` (`com.gamelovers.mcp-unity` git 依存追加)
- 更新: `docs/adr/README.md` (一覧表に 0010 追加。併せて未掲載だった 0009 行を補完)
- 更新: `docs/devlog/INDEX.md` (本 devlog の index 追加、coverage 60→61、版 v2.5)
- 更新: `.gitignore` (機械解決される実 `.mcp.json` / `.codex/config.toml` を ignore、`.example` は追跡)

## Verification

- ヘッドレスで実施可能な範囲: `Packages/manifest.json` / `.mcp.json.example` の JSON 妥当性、文書内クロスリンク、ADR/devlog 書式整合。
- **未実施 (検証境界)**: MCP ↔ Unity Editor のライブ疎通。Editor GUI 起動 + MCP サーバ手動起動が必要でヘッドレス不可。手順は `tools/mcp/SETUP.md`。この境界は ADR-0010 Consequences にも明記。
- staging は ADR-0009 §7 に従い pathspec 限定。`git add -A` 不使用。

## Next

1. ローカル環境で `tools/mcp/SETUP.md` を実行: Unity 起動 → UPM 解決 → `Packages/packages-lock.json` を commit (provenance アンカー) → MCP サーバ起動 → smoke クエリ (`get_scene_info` 等) で疎通確認。
2. 疎通後、UPM git URL を `#vX.Y.Z` 固定へ移行検討。
3. G3 に従い、MCP 活用の前に Apply/Integrator を必ず回す規律の修正を先行させる。
4. 実運用開始時、MCP 由来のシーン変更を Integrator スクリプトへコード化する運用 (G1) を devlog で追跡。

## 改訂履歴

- 2026-05-18: 初版。Synaptic 調査 → ADR-0010 確定 → 基盤整備までを記録。
