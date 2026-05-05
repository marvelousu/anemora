# A1 G3 Final Dialogue Draft (Retroactive)

Date: 2026-05-05
Status: Retroactive (本 devlog は task 完了後に orchestrator が memory + commit + handover から逆引きで起草)

## 1. スコープ

A1 Codex セッションが Niro / Resident_A / Resident_B の VS 用 minimum lore-aware dialogue を draft v0 として StringTable + DialogueAsset へ投入。A2 lore reflection (`c0cb631` + `63de141`) で確定した lore に基づく。

## 2. 実施内容

| commit | 内容 |
| --- | --- |
| `da6040f` | Add G3 final dialogue draft for vertical slice |
| `47aa775` | Apply G3 dialogue audit improvements (後段、self-audit 反映) |

### 2.1 投入された dialogue draft

#### Niro 内面独白
- `dialogue.niro.intro.line_1` 〜 `line_4` (4 行、Anemora_Main scene 起動時、Hook 後の Antela 歩行の心情)
- `dialogue.niro.past_portal.line_1` 〜 (1-2 行、Past portal 通過時の心の動き)
- 配置: NiroMonologueController (新規 component、`Assets/Scenes/Anemora_Main.unity` に追加 + 起動時独白 + Past portal 通過 hook 配線)

#### Resident_A (Hook 過去少女、街の過去住人 witness)
- `dialogue.encounter.past_resident_a.line_1` 〜 (2-3 行、廃墟側指差し witness、子供 〜 同年代音感)
- 配置: Past 側 NpcInteractable trigger、既存 `Resident_A_Instance`

#### Resident_B (現在 観察者 / 記録者、図書館跡で座る)
- `dialogue.encounter.present_resident_b.line_1` 〜 (3-5 行、衰退観察、感情薄め、年齢不詳の中年トーン)
- 配置: Current 側 NpcInteractable trigger、既存 `Resident_B_Instance`

### 2.2 Key migration

旧 `dialogue.placeholder.*` 系 (A1 LocalizationSettings seed `2f3197b` 時点の placeholder) を全削除、`dialogue.niro.*` / `dialogue.encounter.*` 系に migrate。

| 旧 key | 新 key |
| --- | --- |
| `dialogue.placeholder.resident_a.greet` | `dialogue.encounter.past_resident_a.line_1` |
| (他 placeholder series) | 同様 |

### 2.3 Script 追加

`Assets/Scripts/Dialogue/NiroMonologueController.cs` (推測 path、handover 確認):
- 独白の trigger 制御 (scene 起動時 / Past portal 通過時)
- 既存 DialogueDisplay 流用または最小 hook

### 2.4 Audit 改善 (`47aa775`)

A1 self-audit (`77e5dee`) で identify した 4 改善提案を適用:

| 改善 | 内容 |
| --- | --- |
| true-cause hint line | `dialogue.niro.intro.line_5` 追加 (「ここを歩くたびに、何かが少しだけ薄れていく気がする」 / "Each time I pass through here, something seems to fade a little.") = 観測者影響を player-facing で subtle に nudge |
| Resident_B EN line 4 | "If you pass through, watch what remains underfoot." に修正 (ja-JP nuance drift 解消) |
| Resident_B speaker label EN | "Recorder" → "Record Keeper" |
| dialogue_localization.md API doc | 旧 `dialogue.placeholder.*` 例を current key に置換 |

## 3. 検証

| 項目 | 結果 |
| --- | --- |
| compile | success |
| PlayMode test | 27/27 pass (NpcDialogueFlowTests / LocalizationSettingsResolutionTests / SaveLoadLocaleIntegrationTests 含) |
| `dialogue.placeholder.*` / `[TBD: Resident...]` 残存 (scoped runtime assets) | no matches |
| 禁止語 (層 / ベール剥離 / layer / veil / 観測者 / メタ④ / 第 N 層) | scoped runtime assets で no matches |
| ja-JP / en 整合 | A1 audit `77e5dee` で ⚠️ Resident_B EN nuance drift 検出 → `47aa775` で解消 |
| key 命名規則 | `dialogue.niro.intro.line_<n>` / `dialogue.encounter.past_resident_a.line_<n>` / `dialogue.encounter.present_resident_b.line_<n>` で統一 |

## 4. 関連 doc

- `docs/api/dialogue_localization.md` (A1 `cc72aa7`、後段 `47aa775` で stale placeholder 例除去)
- `docs/api/dialogue_asset_authoring.md` (A5 `c4b1fd4`、6 step authoring guide)
- `docs/devlog/2026-05-05_g3_final_dialogue_self_audit.md` (A1 `77e5dee`、5 観点 audit + 4 改善提案)
- `~/.claude/projects/-home-maro1-learning-games-anemora/memory/project_anemora_lore_decisions.md` (interview 確定情報の orchestrator memory)

## 5. caveats / 既知 issue

- 本 dialogue draft は **VS 用 minimum scope (3-5 行 each)**、Stage 4 で polish 予定 (lore content 深化)
- 「層」「ベール剥離」「観測者」use 禁止 (`feedback_anemora_in_game_layer_terms.md` 遵守)、player-facing 表現で書く
- 主人公 Niro の性別固定代名詞 / 一人称 use 回避 (中性表現)
- 主人公の家族 / 知人 不在 の lore truth を遵守 (Resident_A / B は街の住人、面識なし)
- en draft は Codex 起草、Stage 4 で Bilingual review prep 推奨 (STAGE4_ROADMAP `a49ee52`)

## 6. 次の task / 引継ぎ

- A1 G3 self-audit (`77e5dee`) → 4 改善提案 → A1 適用 (`47aa775`) で完了
- Stage 4: lore content polish (G3 dialogue v0 → v1)、Niro 独白の深化、Resident dialogue の文学的 polish
- 重要: 本 dialogue + scene wiring + audio 全揃った後、user 起動で VS 自体の catastrophic failure 発覚 (箱 2 つ / 音なし / graphics なし、`docs/devlog/2026-05-05_vs_playable_failure_orchestration_postmortem.md` `0c3660d`)。本 dialogue draft 自体は問題なし、scene / build / asset inclusion 側に問題が潜在
