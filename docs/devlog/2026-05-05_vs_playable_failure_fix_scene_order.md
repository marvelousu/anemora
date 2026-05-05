# VS Playable Failure Fix — EditorBuildSettings Scene Order

Date: 2026-05-05

## 1. 経緯

A3 diagnostic (`9f7a1d8` `2026-05-05_vs_playable_failure_diagnostic.md`) で root cause 特定:

- `ProjectSettings/EditorBuildSettings.asset` の scene 順序で `Sandbox_E1_Stencil.unity` が **index 0 (startup scene)** に登録されていた
- `Anemora_Main.unity` は index 1 で build に含まれているが Unity Standalone は index 0 を起動 → Sandbox が起動
- Sandbox の中身: Main Camera + Directional Light + PortalMask_Quad + 参照 Cube 2 個 (`Reference_Current_Cube_OutsidePortal` + `InsideOnly_Cube_VisibleThroughPortal`)
- これが user 起動で見えた **「箱 2 つ」 + Hero/NPC/Audio なし + 操作不可** の正体

## 2. 修復

`ProjectSettings/EditorBuildSettings.asset` の scene 順序入替:

| index | 旧 | 新 |
| ---: | --- | --- |
| 0 | `Sandbox_E1_Stencil.unity` | **`Anemora_Main.unity`** |
| 1 | `Anemora_Main.unity` | `Sandbox_E1_Stencil.unity` (test 用に維持、`enabled: 1`) |

両方 enabled のままで順序のみ swap。Sandbox は E1 stencil 試験用として残す (build に含まれるが startup ではない)。

## 3. 必要な後続 task

| task | 担当 |
| --- | --- |
| Windows Standalone build 再生成 (Anemora_Main 起動) | Codex (Windows env 必要) |
| user 再起動 verify (Anemora_Main scene が actually playable か) | user |
| user manual G5 (§H Audio listen / §I UI 目視 / §L 5-8 分通し体験 / §M 片鱗演出) | user |
| Stage 3 完成判定 | user 明示承認後 |

build 出力 path は Codex セッションが新規生成 → handover に absolute path 記載。

## 4. 学び

| 学び | 内容 |
| --- | --- |
| build settings は CRITICAL config | scene 順序 / enabled flag 1 つ間違えると Unity 起動 scene が変わる |
| BuildReport で「asset 含有」を確認しても、それが起動 scene かは別 | A3 G5 automated `c17d62f` で audio inclusion 確認した時点でも、Sandbox 起動の事実は見落とした (BuildReport は asset レベル、scene 起動順は EditorBuildSettings で確認要) |
| Linux Claude orchestrator の直接調査責任 | `EditorBuildSettings.asset` は repo root から text として読める軽量 file、orchestrator が直接確認すべきだった (この devlog 起草前に Read していたが scene 順序の意味を判断せず無視した二重 error) |
| diagnostic dispatch 前に orchestrator が見れる範囲を消化 | 重い Codex round trip より、Linux Claude が text-readable な repo state (build settings / scene file / prefab YAML) を直接 grep / read する方が速い場合多い |

`feedback_anemora_test_pass_vs_playable.md` に「BuildReport asset inclusion ≠ startup scene」の caveat を補強する候補。

## 5. 関連 commit / devlog

- `0c3660d` `2026-05-05_vs_playable_failure_orchestration_postmortem.md` (Linux Claude 過誤 postmortem)
- `9f7a1d8` `2026-05-05_vs_playable_failure_diagnostic.md` (A3 root cause 特定)
- 本 commit (修復) — scene order swap
- 後続 (Codex) — build 再生成 + handover
