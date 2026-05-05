# Audio prompts integration check (2026-05-05)

## 1. 概要

Stage 3 Day 1 の A4 BGM 作業後に、audio 関連 docs の tool / DAW / prompt / ledger / G5 verification 参照を横断確認した。ADR review pass と同様、軽微な用語・状態表現の修正は本 commit で適用し、Decision 改訂が必要なものは次回候補として残す。

Review date: 2026-05-05

対象 docs:

| 種別 | Doc |
|---|---|
| 主要 | `docs/asset_prompts/bgm_zone1_ambient.md` |
| 主要 | `docs/asset_prompts/sfx_zone1.md` |
| 主要 | `docs/adr/0003-asset-pipeline.md` |
| 主要 | `docs/VS_SCOPE.md` |
| 主要 | `docs/legal/asset_ledger.md` |
| 関連 | `CHANGELOG.md` |
| 関連 | `docs/devlog/INDEX.md` |
| 関連 | `docs/G5_ACCEPTANCE_MATRIX.md` |
| 関連 | `docs/G5_PREFLIGHT.md` |
| 関連 | `NOTICES.md` |
| 関連 | `docs/adr/0007-ui-framework-ugui.md` |

Note: task prompt listed `docs/CHANGELOG.md`, but the repository path is root `CHANGELOG.md`.

## 2. 整合性 check 結果

観点:

- A. Tool / DAW 統一性
- B. 30 SFX 内訳整合
- C. 時の窓使用時の AudioMixer 変調仕様
- D. paid plan 要件
- E. asset_ledger format 整合
- F. Suno 単体採用の正当化
- G. 形式統一

| Doc | A | B | C | D | E | F | G | 結果 |
|---|---|---|---|---|---|---|---|---|
| `bgm_zone1_ambient.md` | OK | N/A | OK: 800 Hz / Cello + Violin mute / -2 semitones 記載あり | OK | 要補足 | OK: §3.3 で Suno 単体採用可 | OK | Suno 単体採用時の ledger 記載補足を追加 |
| `sfx_zone1.md` | OK | OK: 環境 6 / 足音 12 / 時の窓 6 / NPC 3 / UI 3 | N/A | OK | OK within prompt examples | N/A | OK | 修正なし |
| `ADR-0003` | OK: Studio One 統一済み。`Reaper` は v1.1 改訂履歴のみ | N/A | Partial: 方針は audio prompt / VS_SCOPE 参照 | OK | Delegates to ledger | 次回候補: Decision は AIVA 骨格 / Suno mood 探索の役割分担 | OK | Decision 改訂が必要なため本 commit では未修正 |
| `VS_SCOPE.md` | 要修正: A4 が MCP AIVA 作業中のまま | OK | OK: high-level 変調記載あり | N/A | N/A | 要修正: BGM 実態未反映 | OK | A4/BGM/SFX 状態を Suno 採用 / SFX quota 待ちへ更新 |
| `asset_ledger.md` | OK in dirty working copy: Suno / ElevenLabs plan evidence recorded | 要修正候補: §2.4 draft rows は古い ID/path 系統 | OK for BGM row | OK | 要修正候補: §2.4 draft rows と `sfx_zone1.md` v1.0 summary table が不一致 | OK: Suno `Dustlight Piano B` 実採用 row あり | Mixed: hot file, large dirty | 本 commit では未修正。§4 に記録 |
| `CHANGELOG.md` | OK | OK: SFX prompt draft 扱い | N/A | N/A | N/A | 未反映: BGM final asset commit がまだ release bullet に無い | OK | BGM asset commit 後の次回更新候補 |
| `docs/devlog/INDEX.md` | OK | OK | N/A | N/A | N/A | N/A | 要修正 | Audio integration report entry と root markdown count を更新 |
| `G5_ACCEPTANCE_MATRIX.md` | OK | OK | OK: high-level 変調記載あり | N/A | N/A | N/A | OK | 修正なし |
| `G5_PREFLIGHT.md` | OK | OK | OK: high-level 変調記載あり | N/A | N/A | N/A | OK | 修正なし |
| `NOTICES.md` | 要修正: BGM が under review/export のまま | N/A | N/A | OK for summary level | N/A | 要修正: Suno selected source 未反映 | OK | AIVA/Suno status を現状へ更新 |
| `ADR-0007` | Audio-specific issue なし | N/A | N/A | N/A | font ledger mention only | N/A | OK | 修正なし |

Finding group breakdown:

| 観点 | 件数 | 本 commit の扱い |
|---|---:|---|
| A. Tool / DAW 統一性 | 2 | VS_SCOPE / NOTICES を修正 |
| B. 30 SFX 内訳整合 | 1 | asset_ledger §2.4 の次回候補として記録 |
| C. AudioMixer 変調仕様 | 1 | G5 docs は high-level 記載のため次回候補として記録 |
| D. paid plan 要件 | 0 | 修正なし |
| E. asset_ledger format 整合 | 2 | bgm prompt 補足を修正、SFX ledger は次回候補 |
| F. Suno 単体採用 | 2 | bgm prompt / VS_SCOPE / NOTICES を修正、ADR-0003 は次回候補 |
| G. 形式統一 | 1 | devlog INDEX を修正 |
| 合計 | 9 | 5 件修正、4 件は次回候補 |

補足:

- `Reaper` は ADR-0003 v1.1 の「Reaper -> Studio One 統一」改訂履歴と CHANGELOG の訂正履歴に残るのみで、現行 tool 指定としての残存は確認されなかった。
- `asset_ledger.md` は他セッション由来の dirty が大きいため、本 commit では編集しない。

## 3. 適用した修正

- `docs/asset_prompts/bgm_zone1_ambient.md`
  - §7 に、Suno 単体一発出し採用時は ledger row の `ツール` / `入力素材` / `手修正` を実態に合わせる補足を追加。
  - 改訂履歴に v0.2 を追加。
- `docs/VS_SCOPE.md`
  - A4 Audio 状態を、Suno `Dustlight Piano B` 採用 / `Zone1_Ambient.ogg` export 済み、SFX は quota 解消後に残り生成へ更新。
  - §5.1 BGM、§5.2 SFX、§8 推奨 / Stage 4-5 寄りの audio 状態を同じ現状に合わせた。
- `NOTICES.md`
  - AIVA Pro を rejected comparison workflow として整理。
  - Suno v5.5 を selected Zone1 ambient source (`Dustlight Piano B`) として整理。
- `docs/devlog/INDEX.md`
  - Audio category に本 report を追加。
  - root markdown coverage count を更新。

## 4. 次回改訂候補 (本 commit では未修正)

- `docs/adr/0003-asset-pipeline.md` v1.2 候補
  - Decision は AIVA Pro = BGM 骨格、Suno v5.5 = mood 探索、Stable Audio = inpainting の役割分担を定義している。
  - A4 実態は Suno `Dustlight Piano B` の単体一発出し採用。`bgm_zone1_ambient.md` §3.3 / §5.1 はこれを許容しているが、ADR-0003 の Decision へ反映するには別タスクでの改訂が必要。
- `docs/legal/asset_ledger.md` §2.4 SFX draft rows 改訂候補
  - Dirty working copy の §2.4 draft rows は `Assets/Audio/SFX/env/...` や `sfx_tf_*` など、`sfx_zone1.md` v1.0 の `Assets/Audio/SFX/Zone1/<category>/...` / 30 ID と一致しない旧系統を含む。
  - hot file のため、本 commit では未修正。SFX 30 種を実生成する前に、`sfx_zone1.md` v1.0 summary table と ledger draft rows の対応を揃える必要がある。
- `CHANGELOG.md` audio bullet 追加候補
  - Root CHANGELOG は SFX 30-entry prompt draft を記録済みだが、BGM final asset commit はまだ release bullet に反映されていない。
  - `Assets/Audio/Music/Zone1_Ambient.ogg` が commit された時点で BGM 完了 bullet を追加する候補。
- G5 audio verification detail 候補
  - `G5_ACCEPTANCE_MATRIX.md` §H と `G5_PREFLIGHT.md` §2.5 は Low-pass / 楽器抜き / pitch shift -2 semitones を記載済み。
  - `bgm_zone1_ambient.md` §5.2 の詳細値 (cutoff 800 Hz、Cello / Violin mute) までは書いていない。G5 担当が exact values を matrix 実測欄で確認する運用にするか、G5 docs に詳細値を追記するかは別タスク候補。

## 5. Suno 単体採用の妥当性確認

実態:

- A4 BGM は Suno Web UI で生成した `Dustlight Piano B` を採用し、`Zone1_Ambient.ogg` として export された。
- `asset_ledger.md` dirty working copy の BGM row は、Suno metadata id、Suno Pro plan、AIVA comparison rejected、OGG q6 export、Steam Tier 1 player-consumed を記録している。

Doc 上の根拠:

- `bgm_zone1_ambient.md` §3.3 は「Suno 単体の一発出しが AIVA より良い場合は、Suno 単体採用も可。ただし paid plan 生成であることを ledger に明記」と定義している。
- `bgm_zone1_ambient.md` §5.1 は AIVA / Suno の完成版出力を試聴し、権利条件・品質・loop に問題がなければ単体採用を優先する手順を定義している。
- `asset_ledger.md` は Suno Pro paid と selected source を記録している。

確認結果:

- Suno 単体採用は `bgm_zone1_ambient.md` の既存運用ルール内に収まる。
- ただし、ADR-0003 の Decision はまだ役割分担を主に記述しているため、今後も VS の標準パターンとして Suno 単体採用を扱うかどうかは ADR-0003 v1.2 または別 audio workflow 改訂で扱う。

## 6. 結論

Audio doc integration check を完了した。Studio One 統一、SFX 30 種内訳、G5 §H / preflight 参照、paid plan evidence は大枠で整合している。軽微な状態表現の古さは `bgm_zone1_ambient.md`、`VS_SCOPE.md`、`NOTICES.md`、`docs/devlog/INDEX.md` で修正済み。

残る大きな論点は、ADR-0003 の Suno 単体採用反映、asset_ledger §2.4 SFX draft rows と `sfx_zone1.md` v1.0 の ID/path 統一、BGM asset commit 後の CHANGELOG 更新、G5 docs に exact AudioMixer values を持たせるかどうかの 4 件。
