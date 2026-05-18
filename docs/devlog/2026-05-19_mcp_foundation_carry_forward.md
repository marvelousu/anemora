# 2026-05-19 MCP Foundation Carry-Forward

Date: 2026-05-19

## Summary

`feat/unity-mcp-foundation` was a temporary tooling branch. The reusable Unity MCP foundation was carried forward into `work/post-vs-public-20260518` so post-VS development can continue from the VS branch without keeping MCP setup isolated.

Only the MCP foundation commit was migrated. The later `Recover missing devlog records` commit was intentionally not merged because it restores a large set of pre-public devlogs and screenshots that are not part of the MCP runtime foundation.

## Branch Handling

- Kept active remote branches: `main`, `work/post-vs-public-20260518`, `feat/unity-mcp-foundation`.
- Deleted accidentally re-pushed non-active remote branches from `origin`.
- Preserved local branches and worktrees; remote cleanup did not delete local work.

## Carried Forward

- `Packages/manifest.json`: added `com.gamelovers.mcp-unity`.
- `Packages/packages-lock.json`: resolved `com.gamelovers.mcp-unity` to hash `a32e47d4ec8731685394dd562aa6f4f119f2bf79`.
- `.mcp.json.example` and `.codex/config.toml.example`: project-local client templates.
- `.gitignore`: ignores concrete `.mcp.json` and `.codex/config.toml`.
- `docs/adr/0010-unity-mcp-editor-bridge.md`: MCP adoption decision and G1-G3 guardrails.
- `tools/mcp/SETUP.md`: local finalization instructions.
- `docs/devlog/2026-05-18_unity_mcp_foundation.md`: original investigation and foundation record.

## Verification

- Unity batchmode package resolution completed successfully enough to modify `Packages/packages-lock.json`.
- Resolved package identity confirmed:
  - package name: `com.gamelovers.mcp-unity`
  - package version: `1.3.0`
  - resolved cache: `Library/PackageCache/com.gamelovers.mcp-unity@a32e47d4ec87`
- Node tooling confirmed:
  - Node.js: `v24.13.0`
  - npm: `11.6.2`
- Local MCP server build completed:
  - command: `npm install`
  - command: `npm run build`
  - verified generated file: `Library/PackageCache/com.gamelovers.mcp-unity@a32e47d4ec87/Server~/build/index.js`
- JSON validation passed for `Packages/manifest.json` and `.mcp.json.example`.
- `git diff --check` passed.

## Boundaries

- MCP to Unity Editor live handshake remains unverified because it requires a GUI Editor session, MCP Unity server window startup, and an active MCP client.
- The generated `Library/PackageCache/.../Server~/build/` output is local-only and is not committed.
- npm reported vulnerabilities in package-local dependencies during `npm install`; these are upstream package dependencies under ignored `Library/PackageCache/`, not committed project dependencies.

## Operational Rule

MCP remains a structural inspection and controlled editing aid. Any scene change made through MCP must still be fixed into the Integrator or scene construction code before it counts as production work.
