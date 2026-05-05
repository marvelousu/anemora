# ADR-0006: セーブシステムの実装方針

## Status

Accepted (実装は Stage 3 G トラック後半 or Stage 4 で検証 → 必要なら改訂)

## Date

2026-05-04 (Stage 3 Day 0)

## Context

ADR-0005 §6 で Anemora の「プレイ進行に意味がある状態」は定義済み。本 ADR は、そのデータを **どの形式で、どこに、どの順序で、安全に永続化するか** を確定する。

### ADR-0005 から引き継ぐセーブ対象

| 範囲 | 内容 |
|---|---|
| プレイヤー状態 | 位置、向き、所属シーン (現在 / 過去) |
| ActionRecordStore | 全エントリ (痕跡反映の元データ、`ActionRecordEntry` の List) |
| 進行フラグ | 層進行 (1〜5 + 真層)、ゾーン解放、サイドクエスト |
| 時間管理状態 | 時の窓の状態 (生成中 / 踏込み中 / 通常) |
| アクセシビリティ設定 | UI 拡大率 / 字幕サイズ / コントラスト (ADR-0007 と連携)。本 ADR ではスロット非依存の `settings.json` として保存する |

### 機能要件

| 要件 | 出典 |
|---|---|
| オートセーブ: ゾーン入場 / 時の窓使用後 / 重要会話後 | SPEC §5.4 / ADR-0005 §6 |
| 手動セーブ: Stage 3 で慎重派へのフォールバックとして検討 | SPEC §5.4 |
| セーブ / ロード: オートセーブ必須、手動セーブはプレースホルダ可 | VS_SCOPE.md §6 |
| クラウドセーブ: Steam Cloud 対応 | SPEC §5.4 / Stage 5 |
| ESC メニュー: 設定 / セーブ / タイトルへ戻る | SPEC §9 / VS_SCOPE.md §6 |

### 制約

- Unity 6.3 LTS + URP (ADR-0001)
- 1 ヶ月集中開発の VS では、実装とデバッグが速い方式を優先
- 戦闘なし / HP なし / 経済なしのため、セーブデータ改ざん耐性は高優先ではない
- AI 主体開発では、人間が差分確認しやすい人間可読フォーマットが有利
- Steam Cloud は Stage 5 のリリース判断後に詳細化し、VS ではローカル保存を完成させる

---

## Decision

### 1. ファイル形式

**Decision: Newtonsoft.Json による JSON 保存を採用する。**

#### 採用理由

1. **人間可読でデバッグしやすい** — ActionRecord / 進行フラグ / 設定値の差分を Git 管理外でも直接確認できる
2. **DTO と相性が良い** — ADR-0005 の `ActionRecordEntry` / `ActionRecordStore` を Plain Old Object として保存できる
3. **Unity JsonUtility より柔軟** — Dictionary、nullable、ネストした DTO、将来の移行処理を扱いやすい
4. **Binary / MessagePack より導入コストが低い** — VS のセーブデータ規模では速度 / サイズの利点より、可観測性を優先する
5. **将来移行しやすい** — `saveVersion` と envelope 構造を持たせれば、Stage 5 で MessagePack 等へ移行する余地を残せる

#### 基本データ構造

```csharp
public sealed class SaveEnvelope {
    public int saveVersion;
    public string buildVersion;
    public string slotId;
    public string sceneId;
    public long savedAtUnixSeconds;
    public SaveMetadata metadata;
    public PlayerSaveData player;
    public ActionRecordStoreSaveData actionRecords;
    public ProgressFlagSaveData progressFlags;
    public TimeFrameSaveData timeFrame;
}
```

`SaveEnvelope` はスロット内セーブデータの唯一の入口にする。各 subsystem は自分の DTO を返し、`SaveService` が envelope に集約して保存する。アクセシビリティ / 音量 / 表示設定はスロットに属さないため、`settings.json` 側の `SettingsEnvelope` に分離する。

```csharp
public sealed class SettingsEnvelope {
    public int settingsVersion;
    public string buildVersion;
    public AccessibilitySaveData accessibility;
    public AudioSettingsSaveData audio;
    public DisplaySettingsSaveData display;
}
```

`SettingsEnvelope` はスロット非依存設定の入口にする。ロード不能な場合でもゲーム進行データとは切り離し、既定値へフォールバックする。

### 2. 保存先

**Decision: VS / Stage 4 では `Application.persistentDataPath` 配下にローカル保存する。Steam Cloud は Stage 5 で同一ファイル群を同期対象にする。**

#### ディレクトリ構造

```
<persistentDataPath>/Anemora/
├── settings.json
├── settings.bak.json
├── saves/
│   ├── autosave/
│   │   ├── save.json
│   │   ├── meta.json
│   │   └── save.bak.json
│   ├── slot_01/
│   │   ├── save.json
│   │   ├── meta.json
│   │   ├── thumbnail.png
│   │   └── save.bak.json
│   ├── slot_02/
│   └── slot_03/
└── logs/
    └── save_errors.log
```

#### 方針

- `settings.json` はスロット非依存の設定 (アクセシビリティ / 音量 / 表示設定) を保持し、`settings.bak.json` を復旧用 backup とする
- `saves/autosave/` は最新オートセーブ専用
- `saves/slot_01`〜`slot_03` は手動セーブ用
- `thumbnail.png` は Stage 4 以降の任意実装。VS では `meta.json` のみでよい
- Steam Cloud は Stage 5 で `settings.json` と `saves/**` を同期対象にする。Cloud API / Auto-Cloud の詳細は Steam 公開判断後に別 ADR or 本 ADR 改訂で確定する

### 3. スロット管理

**Decision: オートセーブ 1 枠 + 手動セーブ 3 枠を標準とする。VS ではオートセーブを必須、手動セーブは UI プレースホルダ可。**

#### メタデータ

`meta.json` にはロード画面に必要な最小情報だけを複製する。

```csharp
public sealed class SaveMetadata {
    public int saveVersion;
    public string slotId;
    public string displayName;
    public string sceneId;
    public string zoneId;
    public int layerIndex;
    public long playTimeSeconds;
    public long savedAtUnixSeconds;
    public bool hasThumbnail;
}
```

#### スロット一覧の読込

1. `saves/autosave/meta.json` を読む
2. `saves/slot_01`〜`slot_03` の `meta.json` を読む
3. `meta.json` が壊れている場合は `save.json` からメタデータ再生成を試す
4. 再生成も失敗する場合は「破損スロット」として UI に表示し、上書き可能にする

### 4. ロード順序

**Decision: 静的アセット / 設定 / 進行状態 / シーン状態 / 視覚反映の順で適用する。**

#### 起動時ロード

1. `settings.json` を読み、`settingsVersion` を検証して、アクセシビリティ / 音量 / 表示設定を適用する
2. タイトル画面を表示する
3. コンティニュー選択時にスロットメタデータを列挙する
4. 選択された `save.json` を `SaveEnvelope` としてデシリアライズする
5. `saveVersion` と `buildVersion` を検証し、必要ならマイグレーションを実行する。ロード可否は `saveVersion` を基準にし、`buildVersion` は警告 / 調査用メタデータとして扱う

`settings.json` が存在しない場合は既定値で `SettingsEnvelope` を生成し、次回保存時に作成する。`settings.json` が壊れている場合は `settings.bak.json` から復元を試し、復元不能なら壊れたファイルを diagnostics 用に残したまま既定値で起動する。設定ファイルの破損はセーブスロットのロード可否に影響させない。

#### ゲーム状態適用

1. **静的カタログ読込** — `ActionRecordCatalog` / ゾーン定義 / NPC 定義 / Localization table など、永続化しない ScriptableObject を先にロード
2. **進行フラグ適用** — 層進行、ゾーン解放、サイドクエスト状態を復元
3. **ActionRecordStore 復元** — `ActionRecordEntry` のリストをランタイム store に注入
4. **シーン構築** — ADR-0005 の `Root_Current` / `Root_Past` 常駐ヒエラルキーを対象ゾーンで初期化
5. **痕跡反映** — ActionRecordStore と進行フラグをスキャンし、現在側オブジェクト / NPC セリフ / 環境演出を反映
6. **プレイヤー復元** — 位置、向き、所属シーン (現在 / 過去) を適用
7. **時の窓状態復元** — 通常 / 踏込み中を復元。ただし `生成中` は一時 UI 状態のため `通常` に丸める
8. **UI 初期化** — HUD / 対話 UI / 進行ログ表示を現在状態から再生成する

### 5. オートセーブと手動セーブの境界

**Decision: オートセーブは進行の安全網、手動セーブはプレイヤー安心用の UI として分離する。**

| 種類 | Stage 3 VS | Stage 4 以降 | 保存先 |
|---|---|---|---|
| オートセーブ | **必須実装** | 継続 | `saves/autosave/` |
| 手動セーブ | UI プレースホルダ可 | 3 スロット実装 | `saves/slot_01`〜`slot_03` |
| 設定保存 | 必須 | 継続 | `settings.json` |

#### オートセーブトリガー

- ゾーン入場時
- 時の窓使用後 (能動行動完了 → 現在反映後)
- 重要会話後
- ESC メニューを開いた時

#### 書込み頻度制御

- 同一フレーム内 / 短時間連続のトリガーは 1 回にまとめる
- セーブ中に次のセーブ要求が来た場合は、最新要求 1 件だけをキューに残す
- 重要会話後など進行確定直後は、UI 上で保存完了を小さく示す

### 6. 暗号化 / 圧縮

**Decision: VS / Stage 4 では暗号化も圧縮も行わない。**

#### 理由

- Anemora は PvP / 課金 / ランキングを持たず、改ざんによる経済リスクがない
- JSON の可読性がデバッグ価値を持つ
- 想定データ量は小さい。ActionRecord が増えても Stage 5 までは非圧縮で十分
- 暗号化は「秘密を守る」用途ではなく難読化に留まり、個人開発の工数に見合わない

#### 将来の再評価条件

- `save.json` が 1MB を継続的に超える
- Steam Cloud の容量 / 転送量で問題が出る
- ネタバレ防止のためにセーブデータの人間可読性を下げる必要が出る

再評価時は GZip 圧縮を第一候補、暗号化は原則採用しない。

### 7. バージョニング / マイグレーション

**Decision: `saveVersion` を必須フィールドとし、バージョンごとの migration chain を持つ。**

#### 方針

- 初期バージョンは `saveVersion = 1`、設定ファイルは `settingsVersion = 1`
- `SaveMigrator` は `fromVersion -> fromVersion + 1` の逐次移行だけを実装する
- `SettingsMigrator` も同じ逐次移行方式にし、設定破損時は migration ではなく既定値フォールバックを優先する
- 未知の新しい `saveVersion` はロード不可として扱い、上書き前に警告する
- 破壊的変更が必要な場合でも、旧 save を `save.bak.json` として残す

```csharp
public interface ISaveMigration {
    int FromVersion { get; }
    int ToVersion { get; }
    JObject Migrate(JObject source);
}
```

#### マイグレーション対象

- DTO フィールド名の変更
- 進行フラグ ID の変更
- `ActionRecordEntry` の schema 変更
- ゾーン ID / scene ID の再命名

アセット側の名称変更は、可能な限り GUID / stable ID で吸収し、表示名や GameObject 名には依存しない。

### 8. セーブ失敗時のフォールバック

**Decision: atomic write + backup + エラー通知を標準にする。**

#### 書込み手順

1. 対象 slot ディレクトリがなければ作成する
2. 現在の `save.json` が存在する場合のみ `save.bak.json` にコピーする。初回 autosave / 空の手動 slot では backup をスキップする
3. 新しい内容を `save.tmp.json` に書く
4. 書込み完了後、ファイルを flush する
5. `save.tmp.json` を `save.json` に置換する
6. `meta.tmp.json` も同様に `meta.json` へ置換する
7. 成功後に UI へ保存完了を通知する

#### 設定ファイルの書込み手順

1. 現在の `settings.json` が存在する場合のみ `settings.bak.json` にコピーする。初回起動では backup をスキップする
2. 新しい内容を `settings.tmp.json` に書く
3. 書込み完了後、ファイルを flush する
4. `settings.tmp.json` を `settings.json` に置換する

#### 失敗時

- `save.json` を破壊しない
- `save.tmp.json` は削除可能なら削除する
- `settings.json` を破壊しない。`settings.tmp.json` は削除可能なら削除し、次回起動時は `settings.bak.json` or 既定値へフォールバックする
- `save_errors.log` に例外種別 / スロット ID / 時刻を記録する
- UI に「保存に失敗しました」を表示し、プレイ継続は止めない
- ロード時に `save.json` が壊れている場合は `save.bak.json` の復元を試す

### 9. Steam Cloud 連携

**Decision: Stage 5 で Steam リリース判断後に詳細化する。VS では Steam 依存コードを入れない。**

Stage 3-4 の実装は、Steam Cloud の同期対象にしやすい単純なファイル構造に留める。

- 同期候補: `settings.json`, `saves/autosave/**`, `saves/slot_01/**`〜`slot_03/**`
- 同期対象外候補: `logs/**`, 一時ファイル `*.tmp.json`
- Steam Cloud 導入時は競合解決 (新しい保存時刻を優先するか、ユーザー選択にするか) を別途定義する

---

## Consequences

### 利点

- **デバッグ容易性が高い** — JSON を直接確認でき、AI / 人間双方で原因調査しやすい
- **ADR-0005 の DTO 分離と整合** — Runtime 履歴と ScriptableObject 静的定義を混線させない
- **VS の必須要件を満たしやすい** — オートセーブを先行実装し、手動セーブは Stage 4 に送れる
- **破損耐性がある** — atomic write と backup により、保存失敗時に既存セーブを守れる
- **Steam Cloud に移行しやすい** — ローカルファイル群を同期対象にするだけの形へ寄せている
- **将来移行余地がある** — `saveVersion` と `SaveEnvelope` により、MessagePack / 圧縮への移行が可能

### 欠点 / 注意点

- **JSON は binary / MessagePack よりサイズが大きい** — ActionRecord が増えた場合、Stage 5 で圧縮を再評価
- **Newtonsoft.Json 依存が増える** — Unity package / asmdef の参照設定を ADR-0004 のディレクトリ構造と合わせる必要
- **手動セーブ UI は Stage 4 送り** — VS ではオートセーブが壊れると復帰体験が弱い。オートセーブ検証を厚くする
- **Steam Cloud 競合解決は未確定** — Stage 5 の公開判断後に必ず詰める
- **セーブ中断への耐性は実装品質に依存** — atomic write の flush / replace 処理を Windows / Linux で確認する

### 後続への影響

- **ADR-0005 (時間管理 / シーン切替)** — §6 のセーブ対象を本 ADR の `SaveEnvelope` とロード順序で永続化する
- **ADR-0007 (UI フレームワーク)** — ESC メニュー、ロード画面、`settings.json` によるアクセシビリティ設定保存が本 ADR と連携する
- **ADR-0004 (プロジェクトディレクトリ構造)** — `Assets/Scripts/Save/`, `Assets/Scripts/Data/`, asmdef 参照設計と整合が必要
- **Stage 3 G トラック** — 第 1 ゾーン実装時に、オートセーブと ActionRecord 復元を通しで検証する
- **Stage 5 Steam 公開** — Steam Cloud と conflict handling を本 ADR 改訂 or 別 ADR で具体化する

---

## Alternatives

### 候補 B: Unity JsonUtility + JSON

**実装:** Unity 標準 `JsonUtility.ToJson` / `FromJson` を使用

**利点:**
- 追加 package が不要
- Unity 標準のため導入が簡単
- 単純な DTO では十分高速

**欠点:**
- 複雑な DTO / Dictionary / nullable / migration 補助で制約が出やすい
- JSON object を部分的に読み替える migration chain と相性が悪い
- Stage 4 以降に schema が増えたときの窮屈さが大きい

**判定:** **不採用**。初期実装の手軽さより、将来の migration と DTO 柔軟性を優先する。

### 候補 C: BinaryFormatter / 独自 Binary

**実装:** バイナリ形式で DTO を保存

**利点:**
- サイズが小さい
- 読み書きが速い
- 人間が直接読めないため軽いネタバレ防止になる

**欠点:**
- デバッグしづらい
- schema migration が難しい
- BinaryFormatter 系は安全性 / 保守性の観点で避けるべき
- Anemora のデータ規模では性能メリットが小さい

**判定:** **不採用**。個人開発 / AI 支援 / VS のデバッグ速度と矛盾する。

### 候補 D: MessagePack-CSharp

**実装:** MessagePack-CSharp で高速・小容量に保存

**利点:**
- 高速
- JSON より小さい
- schema をきちんと設計すれば長期運用に強い

**欠点:**
- 初期導入 / attribute 管理 / migration 設計の負荷が上がる
- セーブ内容を人間が直接確認できない
- VS のセーブデータ規模では過剰

**判定:** **将来候補として保持**。Stage 5 で容量 / Steam Cloud / ロード速度が問題になった場合に再評価する。

### 候補 E: セーブスロット 1 枠のみ

**実装:** `save.json` 1 個だけを上書き

**利点:**
- 実装が最小
- UI が簡単
- オートセーブ主体のゲーム体験に合う

**欠点:**
- 破損時 / 詰み時の逃げ道が弱い
- ユーザーが重要分岐前に残したい需要に応えにくい
- Steam Cloud 競合時に復元選択肢が少ない

**判定:** **不採用**。VS ではオートセーブ 1 枠だけでもよいが、設計上は手動 3 枠を標準として保持する。

---

## 検証ポイント (Stage 3 G / Stage 4 で実機確認)

### 機能動作

1. **オートセーブトリガー** — ゾーン入場 / 時の窓使用後 / 重要会話後 / ESC メニュー時に保存されるか
2. **ロード順序** — ActionRecordStore と進行フラグ適用後に痕跡可視化が再現されるか
3. **プレイヤー所属シーン復元** — 現在 / 過去のどちらで保存しても、衝突レイヤーとカメラ状態が破綻しないか
4. **時間状態の丸め** — `生成中` の保存データを `通常` として復帰し、UI 半開き状態を残さないか
5. **アクセシビリティ設定** — 起動時に UI 拡大率 / 字幕サイズ / コントラストが反映されるか

### 破損耐性

6. **atomic write** — 書込み中断を模擬しても `save.json` or `save.bak.json` から復元できるか
7. **メタデータ再生成** — `meta.json` 破損時に `save.json` からロード画面情報を復元できるか
8. **migration chain** — `saveVersion = 1` から次版へ移行するテストを最初の schema 変更時に追加できるか
9. **初回保存** — `save.json` が存在しない初回 autosave / 空の手動 slot 保存で backup 手順が失敗しないか
10. **設定ファイル復旧** — `settings.json` の欠落 / 破損 / 書込み中断時に `settings.bak.json` または既定値で起動できるか

### UX

11. **保存完了表示** — 静謐な体験を壊さない小さな表示に収まるか
12. **ロード画面の情報量** — zone / layer / play time / timestamp で再開位置が分かるか
13. **手動セーブ延期の許容度** — VS 時点でオートセーブのみでもプレイテストに支障がないか

検証で破綻が出たら本 ADR を改訂、または別 ADR (Superseded) で記録する。

---

## 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v1.0 | 2026-05-04 | 初版。JSON / persistentDataPath / autosave / migration / Steam Cloud 連携方針を定義 |
| v1.1 | 2026-05-05 | ADR review pass: ADR-0004 cross-reference の起草中表記を更新 |

---

## References

### 公式

- [Unity Application.persistentDataPath](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html)
- [Unity Newtonsoft.Json package](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@latest)
- [Steam Cloud](https://partner.steamgames.com/doc/features/cloud)

### Anemora 内部文書

- `ADR-0001` (エンジン Unity 6.3 LTS 採用)
- `ADR-0005` (時間管理 / シーン切替 — セーブ対象データ範囲の前提)
- `ADR-0007` (UI フレームワーク — ESC メニュー / アクセシビリティ設定と連携)
- `SPEC.md` §5.4 (セーブ / ロード)
- `SPEC.md` §9 (UI / セーブ・ロード導線)
- `VS_SCOPE.md` §6 (UI / セーブ・ロード規模)

### 関連 ADR

- `ADR-0001`: エンジン Unity 6.3 LTS 採用 (本 ADR の前提)
- `ADR-0004`: プロジェクトディレクトリ構造 — `Assets/Scripts/Save/` 等の配置と整合
- `ADR-0005`: 時間管理 / シーン切替 — 本 ADR の永続化対象を定義
- `ADR-0007`: UI フレームワーク — セーブ UI / 設定保存と連携
