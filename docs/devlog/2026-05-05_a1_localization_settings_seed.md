# A1 LocalizationSettings + StringTable Seed (Retroactive)

Date: 2026-05-05
Status: Retroactive (本 devlog は task 完了後に orchestrator が memory + commit + handover から逆引きで起草)

## 1. スコープ

A1 Codex セッションが Anemora の Localization 基盤を seed:

- LocalizationSettings.asset 作成
- Locale 2 件 (ja-JP / en) 設定
- TableCollection `Anemora_Strings` 作成
- Addressables group 設定
- batchmode で key fallback / 非 batchmode で StringDatabase 経由解決の動作仕様確立

## 2. 実施内容

| commit | 内容 |
| --- | --- |
| `2f3197b` | Add LocalizationSettings and StringTable seed for dialogue resolution |

主要 file 追加 (推定、A1 handover 参照):

- `Assets/UI/Localization/Settings/LocalizationSettings.asset`
- `Assets/UI/Localization/Locales/ja-JP.asset` + `en.asset`
- `Assets/UI/Localization/Tables/Anemora_Strings_*.asset` (TableCollection + ja-JP / en の StringTable)
- `Assets/UI/Localization/Fonts/Anemora_JP.asset` (TMP 美咲ゴシック atlas、別 commit `2f3197b` 前後で投入)

placeholder key 命名規則:
- `dialogue.placeholder.<npc>.<topic>` (旧、後で `dialogue.niro.*` / `dialogue.encounter.*` に migrate, A1 G3 final dialogue `da6040f`)
- `ui.*` / `system.*`

## 3. 検証

| 項目 | 結果 |
| --- | --- |
| compile | success |
| LocalizationSettingsResolutionTests | pass (新規追加) |
| EditMode test +1 | 32→32 (test count reconcile `1c7ac12` で 31 に確定) |
| PlayMode test +3 | 18→21 (G3 partial `4029cc0` 18/18 → A1 LocalizationSettings 後 21/21) |

YAML trailing space caveat (Unity SerializeReference) を `git diff --check` 警告として確認、trim すると初期化壊れるため維持判断。

## 4. 関連 doc

- `docs/api/dialogue_localization.md` (`cc72aa7`、A1 別 session で起草)
- `docs/adr/0008-localization.md` v0.3 (`2cf0dfa`、LocalizationSettings 完成反映)

## 5. caveats / 既知 issue

- placeholder key の命名規則は seed 時点では `dialogue.placeholder.*` 系、後の G3 final dialogue (`da6040f`) で `dialogue.niro.*` / `dialogue.encounter.*` に migrate (audit `77e5dee` で stale 例を api doc から除去 `47aa775`)
- com.unity.textmeshpro は Unity 6000.x で engine 統合済 (manifest.json に独立 package 不要)

## 6. 次の task / 引継ぎ

- G3 final dialogue 投入 (`da6040f` で実施)
- Locale switch dialog E2E test (`ec1bbb0`)
- Save/Load + Locale integration test (`5f45a29`、locale が SaveEnvelope に含まれない設計判明)
