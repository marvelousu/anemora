# Stage 3 Retrospective

Status: v1.0 Stage 3 closeout (2026-05-06)

本 doc は、Stage 3 Vertical Slice 完了後の retrospective である。`docs/G5_ACCEPTANCE_MATRIX.md` に user manual G5 結果を反映し、`docs/VS_SCOPE.md` v1.0 で Stage 3 完了判定を確定した。

## 1. VS_SCOPE evolution

Stage 3 は Vertical Slice の完成定義から開始し、実装事実が増えるたびに VS_SCOPE を現実同期した。

| Version | Commit | retrospective note |
|---|---:|---|
| v0.1 | `7e88f62` | Codex review の指摘を反映し、文書の役割分離、pacing、完成条件、削減順、コア操作境界を整理した。 |
| v0.2 | `f486c28` | Stage 3 Day 1 の実装進捗を反映した。E0-E5、A1-A3、F1-F4、G4 の完了状態と、コアループ最小成立を記録した。 |
| v0.3 | `9f1d5c7` | Audio 完成と G3 Localization 完成を反映し、残る Stage 3 completion blocker を G5 verification に絞った。 |

その後の v0.4 (`c0cb631`) では、`/spec` resolution interview の結果として Niro、Antela、NPC 役割、art / font / palette の provisional 採用、内部設計ラベルを player-facing text に出さない方針を反映した。本 v1.0 retrospective では、v0.4 を v0.3 実装状態整理後の文書同期として扱う。

## 2. Completion milestones

以下は Stage 3 の主要 block milestone である。全 follow-up commit ではなく、各 block の principal commit または現時点 status を記録する。

| Block | Preliminary status | Principal commit / reference | Notes |
|---|---|---:|---|
| E0 URP Pipeline | Done | `f854466` | URP Pipeline Asset と renderer setup を Stage 3 用に確立した。 |
| E1 Portal Stencil | Done | `773d35f`, `02f5c22` | PortalStencilFeature と shader が stencil bit 3 / Mask 8 の確定経路へ到達した。ADR-0002 v1.1 が最終値を記録している。 |
| E2 Scene hierarchy skeleton | Done | `773d35f` | SceneRootRegistry と Camera_Past skeleton を導入した。後続の VS runtime では Camera_Past rendering ではなく Main Camera culling 反転を使う。 |
| E3 SymbolWheel | Done | `773d35f` | Symbol selection baseline を導入した。Stage 3 で選択可能なのは赤 symbol。 |
| E4 Portal crossing flip | Done | `11c9590`, `3a29757` | 境界 flip 実装と ADR-0005 v1.1 で hysteresis、movement、cooldown、flash、atomic flip ordering を確定した。 |
| E5 ActionRecord | Done | `61edb4e` | ActionRecord data、runtime、catalog、reflector integration を確立した。 |
| A1 Dialogue / Localization data | Done | `523c048`, `2f3197b`, `ec1bbb0` | DialogueAsset 2-layer 設計、LocalizationSettings seed、locale switch PlayMode coverage が入った。 |
| A2 Anemora_Main wiring | Done | `cb2b6ed` | PrototypePlayerController と boundary round-trip PlayMode wiring を main scene に接続した。 |
| A3 Zone1 buildings | Done | `a547e96` | Meshy + Blender + Unity の building set を Zone1 用に import / organize した。 |
| A4 Audio | Done | `6809c4b`, `b9daccb` | `Zone1_Ambient.ogg`、Zone1 SFX 30 種、Zone1AudioController、audio PlayMode wiring tests が origin/main に到達した。 |
| A5 UI foundation / fonts | Done for Stage 3 | `a8f2710`, `f5b4685`, `2f3197b` | Palette v0、Japanese TMP Atlas、English TMP Atlas、localization settings を Stage 3 provisional asset として採用した。 |
| F1 PixelLab drafts | Done | `4a420a5` | Hero / Resident draft generation を記録した。 |
| F2 Aseprite finish | Done for Stage 3 | `08f61b8`, `4d2092a` | Stage 3 sprite を Aseprite 経由で re-export した。Niro の hat silhouette は Stage 4 redraw item として残る。 |
| F3 Retro Diffusion support | Not used for VS | `63de141` | Stage 3 VS path では不要として記録した。必要なら Stage 4 で再評価する。 |
| F4 Hero/NPC prefabs and Animator | Done | `d2c95c2` | Hero、Resident_A、Resident_B prefab と AnimatorController が scene integration に到達した。 |
| G1/G2 environment choice | Done | `a547e96` | Zone1 Meshy regeneration path が実装済み environment route になった。 |
| G3 NPC placement / dialogue | Done for Stage 3 | `4029cc0`, `da6040f` | NPC placement、dialogue scaffold、Stage 3 final dialogue draft が揃った。最終 text polish は Stage 4 へ送る。 |
| G4 ActionRecord trigger loop | Done | `0644822` | Past book interaction、Current reflection、Bed spawn、duplicate prevention が E2E PlayMode coverage に到達した。 |
| G5 automated verification | Done | `c17d62f`, `e6e3c61`, `df19870`, `a0bd50b` | Automated G5 sections、audio rebuild、performance baseline、latest demo repair verification を記録した。 |
| G5 user manual sign-off | Done | `a0bd50b`, `docs/G5_ACCEPTANCE_MATRIX.md` | User confirmed latest demo feel after drag precision repair. No remaining Stage 3 blocker was reported; polish items move to Stage 4. |

## 3. Numerical results

以下の数値は Stage 3 closeout accounting である。Historical G5 run と latest demo repair run の差分があるため、単一の source-scan row と latest executed row を分けて記録する。

| Area | Closeout value | Source / caveat |
|---|---:|---|
| Current committed source test markers | 31 EditMode + 29 PlayMode method markers | `a0bd50b` 時点の `Assets/Tests/EditMode` と `Assets/Tests/PlayMode` の `rg "\\[(Test\\|UnityTest)\\]"` scan。EditMode は Unity Test Runner 実行件数と 1 件差があるため、実行結果を優先する。 |
| Latest closeout automated run | 32/32 EditMode + 29/29 PlayMode passed | `a0bd50b` 後、`<worktree:Anemora-demo-repair>` で Codex が再実行。`codex_editmode.log` / `codex_playmode.log` は exit code 0。 |
| Historical accepted G5 automated run | 32/32 EditMode + 23/23 PlayMode passed | `docs/devlog/2026-05-05_g5_automated_run.md` at `c17d62f`。 |
| Test-count reconciliation note | Source marker scan と Unity Test Runner count の差は docs 上の caveat として残す | `docs/devlog/2026-05-05_test_count_reconcile.md` は source baseline を 31/31 EditMode と記録している。Stage 3 closeout では latest executed count `32/32` を acceptance source とする。 |
| Performance baseline v0.1 | Build folder 115.056 MiB, average 59.909 FPS | `docs/devlog/2026-05-05_performance_baseline.md` at `2e3569f`。この baseline は audio 未統合。 |
| Performance baseline v0.2 | Build folder 117.853 MiB, average 59.989 FPS, p95 frame time 16.683 ms | `docs/devlog/2026-05-05_performance_baseline_v0_2.md` at `df19870`。Audio-loaded 120 second idle sample。 |
| Build size | 117.853 MiB | v0.2 build folder disk size。BuildReport total size は 117.669 MiB。 |
| Runtime memory v0.2 | Working set peak 290.762 MiB, GPU dedicated peak 52.664 MiB, GPU shared peak 30.586 MiB | v0.2 audio-loaded 120 second sample。 |
| Audio assets | BGM 1 + SFX 30 | `Zone1_Ambient.ogg`。SFX 内訳は environment 6、footstep 12、time-window 6、NPC 3、UI 3。 |
| Known technical caveat | v0.2 player log で URP RenderGraph warning 14,402 repeats | 既知の DrawObjectsPass compatibility caveat。v0.2 sample では idle FPS、build size、memory blocker は見つかっていない。 |

## 4. Lessons

### Codex Pro 5 parallel operation

指定された `reference_anemora_codex_parallel_lessons.md` は、本 draft 中の local repo、`~/shared-context`、`~/.codex`、`~/.claude` scan では見つからなかった。そのため、以下は `project_codex_main_trial.md`、`reference_claude_codex_dialogue.md`、Stage 3 devlogs、handover 運用から整理した観察である。

5 track の並列進行は、各 session が write scope を狭く保ち、短い handover を残したことで成立した。特に効いた運用は、pathspec-limited staging、`docs/legal/asset_ledger.md` の hot-file 扱い、push 前の `git fetch origin main`、Unity state を再現しなくても追える devlog である。

摩擦が大きかったのは実装速度ではなく state reconciliation だった。別 session 由来の staged file、dirty ProjectSettings、変動する test count、実装 commit に遅れる docs が同時に発生した。Stage 4 でも並列性は維持できるが、実装 cluster ごとに明示的な reality-sync pass を置く必要がある。

### `/spec` resolution timing

Lore / identity interview では、Niro、Antela、NPC 役割、art / font / palette provisional 採用、Steam Early Access 方向、license status を解決した。ただし一部 asset generation の後に決まったため、Niro の Stage 3 sprite set は hat silhouette direction を十分に反映していない。

Stage 4 では、identity-critical decisions を大きな asset batch より前に解決する。未決のまま進める場合は、asset prompt 側で deliberate placeholder と明記し、generated art が silent default にならないようにする。

### Internal design labels and player-facing language

CONCEPT v1.4 は、特定の設計ラベルを planning 専用にする方針を記録した。実務上の学びは、内部 taxonomy を dialogue、UI、store copy、player-facing pitch に出さないことである。Player-facing text は、planning docs の実装語彙ではなく、player が見るもの、聞くもの、覚えているもの、変えたものとして表現する。

### Provisional adoption and Stage 4 re-evaluation

Stage 3 では、Vertical Slice を止めないために provisional adoption を繰り返し使った。対象は F2 sprite set、palette v0、Japanese / English font choices、Plaza monument、Tree_Decay density、House_Player interior scope である。この pattern は、採用状態を `docs/STAGE3_TBD_RESOLUTION.md` に記録し、Stage 4 review item と結び付けた場合に機能した。

代償として review debt が残る。Stage 4 production が広がる前に、各 provisional item へ owner、review trigger、更新対象 doc または asset folder を与える必要がある。

### Cross-model review

Review-first pattern は ADR、prompt docs、cross-links、scene / API onboarding docs で有効だった。独立した review pass により、古い DAW terminology、stencil-bit assumption、path drift、test-count conflict が広がる前に検出された。

Stage 4 で cross-model review の価値が高いのは、save/load format、localization keys、legal notices、public-facing text、Steam disclosure、renderer / build pipeline decisions などの contract boundary である。Routine small docs は、contributor onboarding の source of truth にならない限り同じ深さの review を必須にしない。

## 5. Stage 4 handoff

以下は、Stage 3 closeout 後に Stage 4 planning へ送る項目である。いずれも Stage 3 blocker ではない。

| Item | Why it carries forward | Current reference |
|---|---|---|
| F2 v2 redraw for Niro | Stage 3 sprite は provisional 採用済みだが、hat silhouette direction が十分に反映されていない。 | `docs/G5_MANUAL_RUBRIC.md`, `docs/scene_tour_anemora_main.md` |
| Lore content polish | Stage 3 dialogue は VS 用 final draft に到達したが、line polish と broader continuity は content expansion 前に必要。 | `docs/draft/g3_npc_dialogue.md`, `da6040f` |
| Audio polish | BGM と 30 SFX は統合済み。manual listening により placeholder SFX、loop、balance、ambience follow-up が出る可能性がある。 | `docs/G5_MANUAL_RUBRIC.md`, `docs/devlog/2026-05-05_g5_audio_rebuild.md` |
| Demo brush / local time-window polish | Stage 3 で drag preview と generated window の一致は修復済み。Stage 4 では brush feel、visual affordance、tutorialization を磨く。 | `a0bd50b`, `docs/devlog/2026-05-06_stage3_closeout.md` |
| Stage 4 roadmap | Stage 3 で多くの follow-up docs が発生したため、Stage 4 では sequencing と ownership を単一 roadmap にまとめる必要がある。 | `docs/VS_SCOPE.md`, `docs/STAGE3_TBD_RESOLUTION.md` |
| Steam Early Access preparation | Public release direction は Steam Early Access。Store page、age rating、partner registration、pricing、disclosure prep が残る。 | `README.md`, `NOTICES.md`, `docs/PITCH_PUBLIC.md` |
| Code license final decision | All Rights Reserved が現状 default。Public contribution intake 前に Stage 4 で継続または変更を判断する。 | `NOTICES.md`, `CONTRIBUTING.md` |

Closeout: Stage 3 implementation は Vertical Slice 完了として受け入れた。Stage 4 は `docs/STAGE4_ROADMAP.md` Phase 0 から開始し、G5 観察結果の triage、provisional asset review、audio/font/palette/dialogue polish、URP DrawObjectsPass warning cleanup を扱う。
