# G5 Pre-flight Check

Status: Finalized for Stage 3 closeout (2026-05-06)

## 1. 概要

本 doc は、G5 通し体験を実行する前に確認する pre-flight check 手順書である。目的は、`docs/G5_ACCEPTANCE_MATRIX.md` の 36 項目を実行する前に、前提状態、ビルド健全性、参照破損、未完了項目の扱いを確認し、Go / No-Go を判定できる状態にすること。

`docs/G5_ACCEPTANCE_MATRIX.md` は検証実行と結果記入のための matrix であり、本 doc はその前段の verification を扱う。matrix の各検証手順はここでは複製せず、pre-flight pass 後に matrix へ進む。

想定 reader は G5 セッション担当者 (Codex または人間)。G5 実行時点の実装状態に合わせて、各 checklist の結果を作業ログまたは matrix の備考欄に記録する。

## 2. 必須前提状態

G5 実行前に、以下の状態を満たしていることを確認する。Stage 3 closeout 時点の latest executed baseline は EditMode `32/32`、PlayMode `29/29`。Source marker scan は EditMode 31 / PlayMode 29 で、EditMode は Unity Test Runner 実行件数を acceptance source とする。

### 2.1 commit / push 状態

- 全 critical path 機能が `origin/main` に merge 済みである。対象は E0-E5、A1-A5、F1-F4、G1-G5。
- G5 は clean working tree で実行する。main worktree に他セッション由来 dirty / untracked が残る場合は、temporary worktree で実行する。

### 2.2 ビルド健全性

- `git pull --ff-only origin main` で最新を取得できる。
- `git status` で G5 対象外の dirty / untracked がない、または temporary worktree で分離されている。
- Unity で project を開き、compile error がない。
- Unity package import error がない。
- EditMode test が全件 pass する。Stage 3 closeout baseline は `32/32`。
- PlayMode test が全件 pass する。Stage 3 closeout baseline は `29/29`。
- Windows Standalone build が error なしで成功する。

### 2.3 アセット参照健全性

- `Assets/Scenes/Anemora_Main.unity` の hierarchy に missing reference、broken prefab、missing script がない。
- `Hero.prefab`、`Resident_A.prefab`、`Resident_B.prefab` が scene に正しく instantiate されている。
- `Book_Family_Current.prefab` と `Book_Family_Past.prefab` が ActionRecord 系の component から参照可能である。
- TMP atlas (JP / EN) が UI canvas で fallback chain として機能する。
- Anemora パレット v0 が sprite / building / UI の対象箇所で参照されている。

### 2.4 Localization 基盤

- `LocalizationSettings` asset が Project Settings で Active になっている (A1 完了後)。
- `StringTable` (ja-JP / en) に Stage 3 final dialogue key が投入済みである。
- `Resident_A_Greeting.asset` / `Resident_B_Idle.asset` が Stage 3 final dialogue key を StringTable 経由で resolve できる。

### 2.5 Audio 基盤

- `Zone1_Ambient.ogg` が `Assets/Audio/Music/` に配置済みである (A4 BGM 完了後)。
- SFX 30 種が `Assets/Audio/SFX/Zone1/` 配下に配置済みである (A4 SFX 完了後)。
- AudioMixer に時の窓変調用の設定がある。対象は Low-pass filter、楽器抜き相当の routing、pitch shift -2 semitones。

## 3. Outstanding items 一覧

G5 実行時点の outstanding は、Go / No-Go 判定への影響で分けて扱う。

### 3.1 必須完了

- §2 の全項目。
- `docs/G5_ACCEPTANCE_MATRIX.md` の実行に必要な scene、prefab、localization、audio、build pipeline の参照状態。

### 3.2 許容 outstanding

以下は G5 担当が matrix の備考欄または作業ログに明示すれば No-Go とは扱わない。各項目の user 判断状態は `docs/STAGE3_TBD_RESOLUTION.md` を参照する。

- User art review が未確定。対象は F2 / palette / font。review aid (`docs/STAGE3_REVIEW_AIDS.md`) で判断保留のまま扱える。
- Lore content の長期 polish は Stage 4 へ残る。Stage 3 G5 は Niro / Resident_A / Resident_B の minimum final dialogue draft で実行できる。
- ADR-0009 が Proposed 状態。Accepted 化は user 承認後であり、G5 実行の pre-flight blocker にはしない。

### 3.3 Outright blockers

以下に該当する場合は No-Go とする。該当項目を記録し、修復 task へ escalate する。

- EditMode test または PlayMode test が失敗する。
- Windows Standalone build が失敗する。
- Compile error または package import error がある。
- `Assets/Scenes/Anemora_Main.unity` が開けない。
- Critical asset が missing である。対象は prefab、palette、TMP atlas、scene 参照 asset。

## 4. Pre-flight check 手順

### 4.1 環境確認

- [ ] git working tree が clean、または temporary worktree で実行している。
- [ ] `git pull --ff-only origin main` が成功する。
- [ ] `git status` で他セッション由来 dirty がない、または temporary worktree により分離済みである。

### 4.2 Unity 起動 verification

- [ ] Unity Hub から Anemora project を開く。
- [ ] Compile error がない。Console が clean である。
- [ ] Package import error がない。

### 4.3 テスト走行

- [x] EditMode test が `32/32` pass、または最新 expected count で全件 pass。
- [x] PlayMode test が `29/29` pass、または最新 expected count で全件 pass。
- [ ] テスト中に想定外の Console error / warning が出ない。

### 4.4 ビルド verification

- [ ] Windows Standalone build が成功する。
- [ ] Build size が想定範囲である。baseline は `docs/G5_ACCEPTANCE_MATRIX.md` §K、performance baseline doc が追加されている場合はその文書を参照する。

### 4.5 シーン健全性

- [ ] `Assets/Scenes/Anemora_Main.unity` を開ける。
- [ ] hierarchy に missing reference または赤字 GameObject がない。
- [ ] `Player` / `SymbolWheel` / `TimeFramePortalSystem` / `Resident_A_Instance` / `Resident_B_Instance` が配置済みである。
- [ ] 各 instance に必要 component が attached されている。対象は Animator、SpriteRenderer、NpcInteractable など。

### 4.6 アセット参照

- [ ] `Hero.prefab` / `Resident_A.prefab` / `Resident_B.prefab` が scene 参照と整合している。
- [ ] DialogueAsset SO (`Resident_A_Greeting` / `Resident_B_Idle`) が `NpcInteractable` に assigned されている。
- [ ] `Book_Family_Current.prefab` / `Book_Family_Past.prefab` が `BookReflector` / `PastBookInteractable` に assigned されている。

### 4.7 Localization

- [ ] `LocalizationSettings` asset が Active である。
- [ ] `StringTable` (ja-JP / en) が `Anemora_Strings` collection に登録済みである。
- [x] Stage 3 final dialogue key が ja-JP / en で解決できる。

### 4.8 Audio

- [ ] BGM / SFX 完了済みの場合、`Zone1_Ambient.ogg` が AudioMixer 経由で再生できる。
- [ ] BGM / SFX 完了済みの場合、SFX trigger が想定箇所で発火する。対象は interactable、footstep、portal、NPC、UI。

## 5. Go / No-Go 判定

### 5.1 Go

- §4 の全項目が pass している。
- §3.2 の outstanding が、G5 担当によって許容 outstanding として記録されている。
- `docs/G5_ACCEPTANCE_MATRIX.md` へ進み、§A から §M を順に実行できる。

### 5.2 No-Go

- §3.3 の outright blocker に該当する。
- §4 の必須項目が fail している。
- fail の原因が G5 実行中に解消できない。

No-Go 時は、該当 checklist item、観測した error / warning、対象 file / scene / prefab、再現手順を report し、修復 task に escalate する。

## 6. G5_ACCEPTANCE_MATRIX への接続

Pre-flight pass 後、`docs/G5_ACCEPTANCE_MATRIX.md` の §A から §M を順に execute し、各 row の `実測 (G5 で記入)` と `pass-fail (G5 で記入)` を記入する。

Lore / art / audio の未完了項目が残る場合は、該当 matrix row の備考欄に許容範囲と参照元 (`docs/STAGE3_TBD_RESOLUTION.md` など) を明記する。pre-flight で No-Go になった項目は、matrix 実行前に修復 task へ分離する。

## 7. 更新履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v1.0 | 2026-05-06 | Stage 3 closeout baseline に更新。Latest run は EditMode `32/32`、PlayMode `29/29` pass。 |
| v0.2 | 2026-05-05 | EditMode baseline を source scan に基づき 31/31 へ reconcile。PlayMode baseline は 18/18 維持。 |
| v0.1 | 2026-05-05 | 初版起草。G5 実行前の必須前提、outstanding items、pre-flight checklist、Go / No-Go 判定を定義 |
