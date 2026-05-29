# Unity MCP (CoderGamester/mcp-unity) ローカル最終化手順

> 根拠決定: `docs/adr/0010-unity-mcp-editor-bridge.md`
> 調査記録: `docs/devlog/2026-05-18_unity_mcp_foundation.md`

この基盤は「設定・依存・文書」までを repo に commit している。MCP ↔ Unity Editor の
ライブ疎通は Editor GUI 起動が必要でヘッドレス検証できないため、以下を **ローカルで手動実行** する。

## 前提

- Unity 6000.3.14f1 / URP 17.3.0
- Node.js (mcp-unity の MCP サーバは Node 実行)
- Claude Code および / または Codex CLI

## 手順

### 1. UPM パッケージ解決

`Packages/manifest.json` に既に追加済み:

```json
"com.gamelovers.mcp-unity": "https://github.com/CoderGamester/mcp-unity.git"
```

Unity Editor で anemora を開くと UPM が解決し、`Library/PackageCache/com.gamelovers.mcp-unity@<解決ハッシュ>/`
に展開される。`<解決ハッシュ>` は環境ごとに変わる非決定値であり、`Library/` は gitignore 対象。
このため設定ファイルにハッシュを直書き commit しない (ADR-0010 浮動依存ノート)。

### 2. Node サーバをローカル build

`mcp-unity` の UPM パッケージは `Server~/src/` を含むが、`Server~/build/index.js` は初期状態では存在しない。
クライアント設定は `build/index.js` を起動するため、PackageCache 内で Node サーバを build する。
`Library/PackageCache/` 配下なので、この生成物は commit しない。

```powershell
cd Library/PackageCache/com.gamelovers.mcp-unity@<解決ハッシュ>/Server~
npm install
npm run build
```

完了後、以下が存在することを確認する:

```powershell
Test-Path .\build\index.js
```

### 3. 再現性アンカーを commit

UPM 初回解決後、`Packages/packages-lock.json` に解決 SHA が書かれる。これが UPM git 依存の
provenance アンカーなので **必ず commit する**:

```powershell
git add Packages/packages-lock.json
git commit -m "chore(mcp): pin mcp-unity resolved SHA via packages-lock"
```

検証完了後、より明示的に固定したい場合は manifest の URL を `...mcp-unity.git#vX.Y.Z` に移行してよい。

### 4. MCP サーバ起動 (Unity 側)

Unity Editor: `Tools > MCP Unity > Server Window` → **Start Server** (既定 port 8090)。
WebSocket transport。Editor を開いている間のみ疎通する。

### 5. クライアント設定の最終化

PackageCache の解決パスを実値に置き換える。Unity の MCP ウィンドウに auto-configure があれば
それを使う。手動の場合:

- `.mcp.json.example` → `.mcp.json` にコピーし、`RESOLVED_HASH` を
  `Library/PackageCache/com.gamelovers.mcp-unity@` 直下の実ディレクトリ名 (`@` 以降) に置換。
- Codex CLI を使う場合は `.codex/config.toml.example` → `.codex/config.toml` に同様に。

実 `.mcp.json` / `.codex/config.toml` は機械解決パスを含むため `.gitignore` 済み
(テンプレート `.example` のみ追跡)。

### 6. 疎通スモーク

Claude Code または Codex を起動し、Unity Editor を開いてサーバ起動した状態で、構造クエリを 1 つ:

- `get_scene_info` / `select_gameobject` / `get_gameobject` のいずれかで現在シーンの
  hierarchy が返ることを確認。

返れば疎通成立。返らなければ Editor 起動・Start Server・port 8090・Node の有無を順に確認。

## 運用ガード (ADR-0010 §3 — 必読)

- **G1**: MCP の対話的シーン編集を、コード化せず主要 authoring 経路にしない。構造変更は
  Integrator / シーン構築スクリプトへ反映し再現可能形に固定。ad-hoc は調査・検知・試作に限る。
- **G2**: MCP は構造の知覚であって視覚品質判断ではない。構造アサート緑だけで完了にしない。
  PNG / Play モードの人間目視ゲートを必ず後段に残す。
- **G3**: MCP は検知の前倒しであってオーケストレーション修正の代替ではない。
  Apply/Integrator を必ず回す規律の修正を MCP 活用に先行させる。
