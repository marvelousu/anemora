# Stage 3 TBD Resolution Tracking Sheet

> 本 sheet は、Stage 3 Day 1 以降に残っている user 判断ポイントを一覧化するための tracking sheet。
> v0.2 では Stage 3 /spec resolution interview の結果を反映し、Stage 3 A track の 23 項目を解決済みとして閉じる。
> `feedback_anemora_no_premature_lockin.md` の方針に従い、候補の優劣付け、案の誘導、収束予測は書かない。

---

## 1. 概要

### 1.1 目的

- user 判断が必要な TBD を一覧化する。
- 各 TBD の状態、反映先、Stage 3 完了条件への影響を tracking する。
- Stage 3 の実装 session が、未確定事項を確定扱いで進めないようにする。

### 1.2 更新方針

- user が明示的に「決めた」と言うまで、各項目は未確定として扱う。
- 各 TBD が解決したら、該当 row の `項目 ID` を strike-through し、確定日と反映 commit hash を `反映先 (確定後)` に追記する。
- 新規 TBD が発生したら、§2 の表へ 1 row / 1 項目で追記する。
- 複数案が並立する場合は併記する。`draft`、`provisional`、`intermediate`、`TBD` の状態を明示し、案を絞らない。
- 「決めない」も有効な状態として扱い、Stage 4 以降でよい項目はそのまま保持する。

---

## 2. TBD 項目一覧

| 項目 ID | カテゴリ | 現状 | 候補 / 案 | user 判断必要か | ブロック影響度 | 反映先 (確定後) | 期日目安 |
|---|---|---|---|---|---|---|---|
| ~~CHR-01~~ | キャラクター | Resolved 2026-05-05。主人公名は Niro / ニロ。 | provisional name として採用。 | Resolved | 中 (Stage 3 quality に影響) | Reflected in `SPEC.md`, `docs/VS_SCOPE.md`, `README.md`, `CHANGELOG.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~CHR-02~~ | キャラクター | Resolved 2026-05-05。主人公の性別表現は中性表現で最終確定。 | 男 / 女 / その他案は Stage 3 VS では採用しない。 | Resolved | 高 (Stage 3 死守ライン直撃) | Reflected in `SPEC.md`, `docs/VS_SCOPE.md`, `docs/legal/asset_ledger.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~CHR-03~~ | キャラクター | Resolved 2026-05-05。主人公の年齢は若者 15-19 歳。 | Stage 4 で必要に応じ sprite / copy の調整対象。 | Resolved | 中 (Stage 3 quality に影響) | Reflected in `SPEC.md`, `docs/VS_SCOPE.md`, `docs/legal/asset_ledger.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~CHR-04~~ | キャラクター | Resolved 2026-05-05。見た目はつばのある旅人風の帽子。家族 / 知人は不在。 | 出身は Antela の住人扱い。詳細 story bible は Stage 4 以降。 | Resolved | 低 (Stage 4 以降で OK) | Reflected in `SPEC.md`, `docs/VS_SCOPE.md` by `c0cb631` | Stage 4 入口 |
| ~~NPC-01~~ | キャラクター | Resolved 2026-05-05。Resident_A は過去側の街の過去住人。 | Niro と面識なし。廃墟 / 図書館跡を指差す witness / hook。 | Resolved | 高 (Stage 3 死守ライン直撃) | Reflected in `SPEC.md`, `docs/VS_SCOPE.md`, `docs/api/dialogue_localization.md`, `docs/legal/asset_ledger.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~NPC-02~~ | キャラクター | Resolved 2026-05-05。Resident_B は現在側の廃墟 / 図書館跡で座る観察者 / 記録者。 | Niro と面識なし。 | Resolved | 中 (Stage 3 quality に影響) | Reflected in `SPEC.md`, `docs/VS_SCOPE.md`, `docs/api/dialogue_localization.md`, `docs/legal/asset_ledger.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~STORY-01~~ | 物語 | Resolved 2026-05-05。衰退原因は表向きは実体的な環境変化 / 自然災害、真因は観測者累積による世界摩耗の二層構造。 | 細部演出は Stage 4 で具体化。 | Resolved | 低 (Stage 4 以降で OK) | Reflected in `SPEC.md` by `c0cb631` | Stage 4 入口 |
| ~~STORY-02~~ | 物語 | Resolved 2026-05-05。時の筆は古代の遺物で、主人公は元から持っていた。 | 起源詳細の演出は Stage 4 story bible で扱う。 | Resolved | 低 (Stage 4 以降で OK) | Reflected in `SPEC.md` by `c0cb631` | Stage 4 入口 |
| ~~STORY-03~~ | 物語 | Resolved 2026-05-05。主人公は誰かに選ばれた者ではなく、ループを止めるため突然変異的に発生した異物。 | 創造主体の細部は Stage 4-5 で扱う。 | Resolved | 低 (Stage 4 以降で OK) | Reflected in `SPEC.md` by `c0cb631` | Stage 4 入口 |
| ~~STORY-04~~ | 物語 | Resolved 2026-05-05。第 1 ゾーン名は Antela / アンテラ。 | provisional zone name として採用。 | Resolved | 中 (Stage 3 quality に影響) | Reflected in `SPEC.md`, `docs/VS_SCOPE.md`, `README.md`, `CHANGELOG.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~ART-01~~ | アート | Resolved 2026-05-05。F2 Hero / NPC v1 sprite は provisional 採用。 | Stage 4 で revision。 | Resolved | 高 (Stage 3 死守ライン直撃) | Reflected in `docs/legal/asset_ledger.md`, `docs/STAGE3_REVIEW_AIDS.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~ART-02~~ | アート | Resolved 2026-05-05。F3 Retro Diffusion は VS 不要。 | Stage 4 で検討。 | Resolved | 中 (Stage 3 quality に影響) | Reflected in `SPEC.md`, `docs/VS_SCOPE.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~ART-03~~ | アート | Resolved 2026-05-05。Anemora パレット v0 は provisional 採用。 | Stage 4 で再評価。 | Resolved | 中 (Stage 3 quality に影響) | Reflected in `SPEC.md`, `docs/VS_SCOPE.md`, `docs/legal/asset_ledger.md`, `docs/STAGE3_REVIEW_AIDS.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~ART-04~~ | アート | Resolved 2026-05-05。TMP 美咲ゴシック JP atlas は provisional 採用。 | Stage 4 で再評価。 | Resolved | 中 (Stage 3 quality に影響) | Reflected in `SPEC.md`, `docs/VS_SCOPE.md`, `docs/legal/asset_ledger.md`, `docs/STAGE3_REVIEW_AIDS.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~ART-05~~ | アート | Resolved 2026-05-05。Press Start 2P EN atlas は provisional 採用。 | Stage 4 で再評価。 | Resolved | 低 (Stage 4 以降で OK) | Reflected in `SPEC.md`, `docs/VS_SCOPE.md`, `docs/legal/asset_ledger.md`, `docs/STAGE3_REVIEW_AIDS.md` by `c0cb631` | Stage 4 入口 |
| ~~ENV-01~~ | 環境 | Resolved 2026-05-05。Plaza monument は B 噴水跡を provisional 採用。 | A bench / C pedestal は intermediate として維持。 | Resolved | 高 (Stage 3 死守ライン直撃) | Reflected in `SPEC.md`, `docs/legal/asset_ledger.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~ENV-02~~ | 環境 | Resolved 2026-05-05。Tree_Decay は near-leafless を provisional 採用。 | sparse 案は必要時の比較材料。 | Resolved | 中 (Stage 3 quality に影響) | Reflected in `SPEC.md`, `docs/legal/asset_ledger.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~ENV-03~~ | 環境 | Resolved 2026-05-05。House_Player 内装は Bed / Bookshelf x2 / Table+Chair / Door を全項目採用。 | VS の家内装 scope として扱う。 | Resolved | 高 (Stage 3 死守ライン直撃) | Reflected in `SPEC.md`, `docs/legal/asset_ledger.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~AUD-01~~ | 音響 | Resolved 2026-05-05。Stage 3 A track の user 判断保留としての音響 TBD はなし。 | A4 実装・検証は audio task / G5 側で扱う。 | Resolved | 低 (Stage 4 以降で OK) | No doc decision change required; status aligned with `docs/VS_SCOPE.md` by `c0cb631` | 任意 |
| ~~TECH-01~~ | 技術 | Resolved 2026-05-05。Dialogue key naming direction は Niro / encounter / Resident role を反映する方針。 | `dialogue.niro.encounter_resident_a.greet` / `dialogue.niro.encounter_resident_b.idle` 系。migration は別タスク。 | Resolved | 中 (Stage 3 quality に影響) | Reflected in `docs/api/dialogue_localization.md` by `c0cb631` | Stage 3 内 (VS 完成前) |
| ~~DOC-01~~ | 文書 | Resolved 2026-05-05。ADR-0009 Asset pipeline は Accepted。 | Proposed から Accepted へ昇格。 | Resolved | 低 (Stage 4 以降で OK) | Reflected in `docs/adr/0009-asset-pipeline.md` by `c0cb631` | Stage 4 入口 |
| ~~PUB-01~~ | 公開 | Resolved 2026-05-05。Code license は All Rights Reserved 継続。 | Stage 4 で再評価。 | Resolved | 低 (Stage 4 以降で OK) | Reflected in `README.md`, `CONTRIBUTING.md`, `NOTICES.md`, `SPEC.md` by `c0cb631` | Stage 4 入口 |
| ~~PUB-02~~ | 公開 | Resolved 2026-05-05。Public release は Steam Early Access 予定で lock-in。 | itch.io / GitHub Public build は補助公開または検証配布が必要な場合のみ。 | Resolved | 低 (Stage 4 以降で OK) | Reflected in `README.md`, `CONTRIBUTING.md`, `NOTICES.md`, `SPEC.md`, `docs/legal/asset_ledger.md` by `c0cb631` | Stage 4 入口 |

---

## 3. 解決済み項目サマリ

§2 の 23 項目は Stage 3 /spec resolution interview の結果を受けて、2026-05-05 に解決済みとして閉じた。反映 commit は `c0cb631`。

### 3.1 キャラクター / 物語

- ~~CHR-01~~: 主人公名 = Niro / ニロ (provisional)
- ~~CHR-02~~: 主人公性別 = 中性表現で最終確定
- ~~CHR-03~~: 主人公年齢 = 15-19 歳
- ~~CHR-04~~: 主人公出身詳細 = Antela 住人扱い、家族 / 知人不在、つばのある旅人風の帽子
- ~~NPC-01~~: Resident_A = 過去側の街の過去住人、面識なし、witness / hook
- ~~NPC-02~~: Resident_B = 現在側の廃墟 / 図書館跡の観察者 / 記録者、面識なし
- ~~STORY-01~~: 衰退原因 = 表向きは実体的環境変化、真因は観測者累積による世界摩耗
- ~~STORY-02~~: 時の筆の起源 = 古代の遺物、主人公は元から持っていた
- ~~STORY-03~~: 主人公が選ばれた理由 = 誰かに選ばれた者ではなく、ループを止めるため発生した異物
- ~~STORY-04~~: 第 1 ゾーン名 = Antela / アンテラ (provisional)

### 3.2 アート

- ~~ART-01~~: F2 Hero / NPC v1 sprite = provisional 採用、Stage 4 revision
- ~~ART-02~~: F3 Retro Diffusion = VS 不要、Stage 4 で検討
- ~~ART-03~~: Anemora パレット v0 = provisional 採用
- ~~ART-04~~: TMP 美咲ゴシック JP atlas = provisional 採用
- ~~ART-05~~: Press Start 2P EN atlas = provisional 採用

### 3.3 環境

- ~~ENV-01~~: Plaza monument = B 噴水跡 provisional 採用
- ~~ENV-02~~: Tree_Decay = near-leafless provisional 採用
- ~~ENV-03~~: House_Player 内装 = Bed / Bookshelf x2 / Table+Chair / Door を全項目採用

### 3.4 音響

- ~~AUD-01~~: Stage 3 A track の user 判断保留としての音響 TBD はなし

### 3.5 技術 / 文書

- ~~TECH-01~~: Dialogue key naming direction = `dialogue.niro.encounter_resident_a.greet` / `dialogue.niro.encounter_resident_b.idle` 系、migration は別タスク
- ~~DOC-01~~: ADR-0009 Asset pipeline Status = Accepted

### 3.6 公開

- ~~PUB-01~~: Code license = All Rights Reserved 継続、Stage 4 で再評価
- ~~PUB-02~~: Public release = Steam Early Access 予定で lock-in

---

## 4. 解決ガイドライン

- 「決めない」も valid。複数案を並列維持してよい。
- VS 死守ラインに直撃する判断のみ Stage 3 内で解決対象にする。
- Stage 4 入口以降でよい判断は、未確定のまま保持してよい。
- 確定時は §2 の該当 row を strike-through し、確定日、反映 commit hash、反映先を追記する。
- A2 / Codex は候補を評価しない。user が明示的に決めるまで確定扱いにしない。

---

## 5. 更新履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0.1 | 2026-05-05 | Stage 3 Day 1 の user 判断保留 tracking sheet として初版起草 |
| v0.2 | 2026-05-05 | Stage 3 /spec resolution interview の結果を反映。23 項目を解決済みとして close し、反映 commit `c0cb631` を記録 |
