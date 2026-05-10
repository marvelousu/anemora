# A2 Stage 3 Lore Reflection (Retroactive)

Date: 2026-05-05
Status: Retroactive (本 devlog は task 完了後に orchestrator が memory + commit + handover から逆引きで起草)

## 1. スコープ

Linux Claude orchestrator session で実施した /spec resolution interview の確定事項を、Anemora 全 doc に反映する大型 reflection task。

interview 結果 (詳細は `<orchestrator-memory>/memory/project_anemora_lore_decisions.md`):

- 主人公: **Niro** (provisional) / 中性表現で最終確定 / 15-19 歳 / スナフキン的帽子
- 主人公の家族 / 知人: 不在 (CONCEPT v1.3 整合)
- 第 1 ゾーン: **Antela** (provisional)
- 過去側 NPC: 街の過去住人 (面識なし、Hook 廃墟側指差し witness)
- 現在側 NPC: 廃墟 / 図書館跡で座る観察者 / 記録者 (面識なし)
- F2 v1 sprite / palette v0 / TMP 美咲ゴシック JP / Press Start 2P EN / Plaza monument B 噴水跡 / Tree_Decay near-leafless / House_Player 内装: 全 provisional 採用
- F3 Retro Diffusion: VS 不要、Stage 4 候補
- ADR-0009 Asset pipeline: Status: Proposed → **Accepted**
- Code license: All Rights Reserved 継続、Stage 4 入口で再評価
- Public release: **Steam Early Access 予定で lock-in**
- 「層 / ベール剥離 / メタ④ / 観測者」 = 設計用便宜語、player-facing で出さない (CONCEPT v1.4 `127da7d` で注記済)

## 2. 実施内容

| commit | 内容 |
| --- | --- |
| `c0cb631` | Reflect Stage 3 A-track lore decisions across docs |
| `63de141` | Record Stage 3 TBD resolution closure |

反映 11 file:

| file | 反映内容 |
| --- | --- |
| `SPEC.md` (root) | §13.3 改訂、Niro / Antela / 中性 15-19 / NPC 役割 / 中影響度 TBD 全確定 反映 (注: `docs/SPEC.md` は repo 内に存在せず root SPEC.md に反映) |
| `docs/VS_SCOPE.md` v0.4 | §3.x の主人公・NPC・ゾーン記述を確定値に |
| `docs/STAGE3_TBD_RESOLUTION.md` | 23 項目 strike-through + 確定日 + reference commit |
| `docs/legal/asset_ledger.md` | F2 / palette v0 / TMP atlas 等の 採用 flag + provisional/最終 表記 (A5 `50ab8c0` SFX 30 行は touch なし) |
| `docs/adr/0009-asset-pipeline.md` | Status: Proposed → **Accepted**、改訂履歴 v0.2 (2026-05-05 user 承認) |
| `NOTICES.md` | §1 / §6 に Code license = All Rights Reserved 継続、Public release = Steam EA 予定 lock-in 追記 |
| `README.md` | Niro (主人公名) / Antela (Zone1 名) / Steam EA 公開予定 反映、TBD 項目 update |
| `CHANGELOG.md` | [Unreleased] に lore decisions entry 追加 |
| `docs/STAGE3_REVIEW_AIDS.md` | 各項目を strike-through + 確定 commit hash |
| `docs/api/dialogue_localization.md` | placeholder key 命名 (`dialogue.placeholder.resident_a.greet` → `dialogue.niro.encounter_resident_a.greet` 系) を update 方針として注記 |
| `CONTRIBUTING.md` | TBD 項目 update (主人公名 / Zone1 名 / Public release / Code license の確定状態反映) |

## 3. 検証

| 項目 | 結果 |
| --- | --- |
| 文書のみ変更 | Unity test 未実行 |
| `git diff --check` (HEAD~1 HEAD) | pass |
| pathspec stage | 11 file のみ、`Assets/Scenes/Anemora_Main.unity` は touch せず (A4 audio commit と並行衝突回避) |
| `Assets/Tests/EditMode/LocalizationSettingsResolutionTests.asset` の YAML trailing space | 触らず維持 |

## 4. 影響

A2 lore reflection は本日 push の中で **最も meta-level の event**。以後の全 doc (PITCH_PUBLIC v0 / TRAILER_SCRIPT v0 / G3 final dialogue / scene tour v0.2 / GLOSSARY v0.1 / etc.) はこの reflection を前提として起草された。

## 5. caveats / 既知 issue

- `docs/SPEC.md` は repo 内に存在せず、root `SPEC.md` に反映 (find で確認済)
- memory file `project_anemora_lore_decisions.md` は Windows / WSL 側で見つからなかったため、A2 は orchestrator 提示の interview 要約 + 既存 repo context を source として進行
- Niro / Antela は **provisional 表記**、Stage 4 入口で再評価可
- 「層 / ベール剥離 / メタ④ / 観測者」は internal design 用便宜語として全 doc で player-facing 不使用を確認

## 6. 次の task / 引継ぎ

- A1 G3 final dialogue draft (`da6040f`) で実 lore content 投入 (Niro 内面独白 / Resident_A / Resident_B 台詞)
- A1 G3 audit (`77e5dee`) で 4 項目改善提案 → A1 fix (`47aa775`) で適用
- 残: F2 v2 redraw (Niro 帽子反映、provisional → 最終 / Stage 4 polish 候補)
