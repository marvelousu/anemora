# HANDOFF: Chapter 1 Full VS — named cast + generic NPC runtime population (map-vs)

受け手: `work/chapter1-continuation-map-vs-20260524` を担当する Codex(Windows) セッション（順次3セッションの【第3】=character runtime）。
作成: 2026-05-30 / Claude(Opus, 管理セッション)。実装は Codex 側。検証は Unity batchmode + .exe + R2 レビュー。

---

## 1. Context

- リポ: `marvelousu/anemora`(public)。worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`。
- branch: `work/chapter1-continuation-map-vs-20260524`、着手前に `git fetch origin && git merge origin/work/chapter1-continuation-map-vs-20260524`（or pull）で最新化。**現 tip は B-β/C-β 完了済み（`58e33fb9` 系）** + その後の R2 ツール微修正コミット。
- 直前までの状態（前2セッションの結果、本タスクの前提）:
  - **CI/CD 衛生伝播済み**: bloat-guard フック有効（`core.hooksPath=tools/githooks`）。`docs/review/`・`docs/devlog/screenshots/`・生成シーン・APV bytes は **git add 不可（拒否される）**。
  - **レビュー画像は R2 運用**: `docs/review/<ts>/` にローカル生成（`devlog.txt` 必須）するが **git add しない**。`tools\r2\r2-upload-review.ps1 -CycleDir docs/review/<ts> -Branch work/chapter1-continuation-map-vs-20260524` で R2 へ。`CLOUDFLARE_API_TOKEN` は User 環境変数に設定済（script が自動ロード）。viewer(`anemora-viewer.pages.dev`) は次ビルドで R2 manifest を取り込み反映。
  - **生成シーン `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` と APV `*.Cell*.bytes` は untracked + gitignore 済の build artifact**。再生成しても **commit しない/できない**。
  - **B-β/C-β（Buto/Fronkon Tilt Shift）採用済**: 有償アセット import 済で `BUTO`/`FRONKON_TILTSHIFT` define on。**有償アセット本体は gitignore 済・commit 禁止（EULA）**。本タスクで Buto/Fronkon には触らない。
- **コミット対象**: 生成器 `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`、新規 first-party スクリプト、新規キャラアート `Assets/Art/Characters/FastVS/**`（後述、first-party・EULA 問題なし）、生成 material/texture、本 handoff、devlog。**scene/APV/レビュー画像/有償アセットは commit しない**。`git add -A` 禁止（pathspec stage で WIP を巻き込まない）。

## 2. Root Cause / 前提の訂正（最重要・元計画との差分）

元の実装計画 `CHAPTER1_FULL_VS_CHARACTER_RUNTIME_IMPLEMENTATION_PLAN.md` は **旧 worktree `Anemora-chapter1-continuation-work`（branch `work/chapter1-continuation-20260520`）に対して**書かれており、「既存の `Resident_A`/`Resident_B` プレースホルダ呼び出し（`CreateMiaAtWorkTable`、`FastVS_Kaia_FieldWorkerBlockout` 等）を最終アートに**差し替える**」前提になっている。

**しかし map-vs にそのプレースホルダは存在しない。**（検証済: `resident_a/b_idle_sprite`・`FastVS_Dario_*`・`FieldWorker`・`RuinsArea` NPC は **全て 0 マッチ**。`CreateSpriteCharacter` は定義のみ、実スポーンは `PastNiro` 参照だけ。）旧 20260520 は破棄方針のため、NPC 配置作業は map-vs では **net-new** になる。

一方 **map-vs はマップ環境を全域作り込み済み**（広場/Mia家/Aria・街角/Kaia農場/遺跡）。よって本タスクは:

- 元計画の **Phase 1-2（資産 promote・パス定数・material helper・import 検証）はそのまま流用**。
- 元計画の **Phase 3-4 は「差し替え」ではなく「既存エリアへ新規配置」に読み替える**。配置位置は **仮（provisional）**: 旧 20260520 の配置と `docs/MAP.md` を目安に妥当な位置へ置くが、**最終ではない**。Tom がレビューで動かす前提なので、**位置決めに過剰投資しない**（座標微調整より「最終アート・breathing・grounding が出ている」ことを優先）。
- 元計画の **Phase 5-9（contact shadow・dialogue stateflow・検証・キャプチャ・build/smoke）はそのまま流用**。
- 元計画の §7.1「プレースホルダ回帰検出」は対象が無いので、代わりに「named char が正しい material id を使っているか」の存在検証に読み替える。

元計画本文は設計意図・FPS 値・受入基準の一次資料として参照可: `C:\Users\maro6\Documents\Unity\Anemora-chapter1-continuation-work\docs\CHAPTER1_FULL_VS_CHARACTER_RUNTIME_IMPLEMENTATION_PLAN.md`。

## 3. Canon 制約（配置・演出が必ず守る・違反は作り直し）

出典: `docs/canon/chapter1.md`, `docs/STORY_BIBLE_v1.md`, `docs/MAP.md`。

- **Niro は唯一の異物**。NPC は全員「ふつうの住人」。語り部/予言者/守護者/先代 等の**特別な役割を与えない**。Niro に「この世界に属する」ことを示唆しない。
- **Chapter 1 確定キャスト（名前 final）**: Niro(主役) / Reto(Resident_B,図書館現在) / Mia(Resident_F) / Aria(Resident_A) / Karla(Resident_J) / Dario(Resident_D) / Kairo(Resident_K) / Luna(Resident_L) / Kaia(Resident_C)。**Ordo(E)・Mare(H) は Ch1 に登場しない**。背景の無名住人は可（過去図書館の読書人、過去市場の群衆、過去先祖シルエット、過去橋の補修クルー）。
- **方位 canon（Central Plaza 基準, `docs/MAP.md`）**: Niro家=SW / 図書館=N / Mia家=SE / Aria家・街角=（Mia SE 経由）E / Kaia農場=（街角 NE）NE / 遺跡=（農場経由）さらに東 / 遺跡集落=最東。**東・北東チェーンのみ、南進ルート無し**。
- **過去図書館（S1）**: 本を読むのは**無名の人物（Aria ではない）**。Niro の3行観測は canon 固定。配置するなら無名・generic 扱い。
- **Niro 台詞**: silent + 短い感情テキストのみ（例 `(...smoke)` `(...person)`）。長台詞・VO 無し。
- **player-facing 禁止語**（UI/dialogue/公開doc）: 「層」「ベール剥離」「観測者磨耗」。固有名詞 `Antela` を UI/dialogue で前面化しない。
- **メタデータ禁止語**（commit message / PR / branch / handoff ファイル名 / 公開テキスト）: ループ / 観測者 / 真層 / 偽記憶 / Robot_X / Echo / **エリュトリア(Erythria)** / 第4の壁 / 第5章 climax。※「Erythria」は **S3 Dario の in-game 台詞（茶葉の伏線）としては canon で出てよい**が、**commit/branch/メソッド名 等の外部メタには出さない**（例: 生成器のメソッド名は `CreateRuinsMerchant` 等の中立名にし、`Erythria` を避ける）。

## 4. ソース資産（v47 named pack + generic pack）

**旧 worktree にのみ存在**（map-vs には無い）。first-party AI 生成、**EULA 問題なし**（Buto/Fronkon の有償物とは別）。64x96 PNG。

- named cast ソース root:
  `C:\Users\maro6\Documents\Unity\Anemora-chapter1-continuation-work\docs\review_gallery\imports\chapter1_v47_available_character_pack_20260530\Assets\Art\Sprites\Chapter1Characters\v47\`
  - `named_npc_directions_v12\resident_{c,d,f,j,k,l}\` … front/back/left/right の方向 still（`*_v12_64x96_review_only.png`、left の rotate seed / right は left の mirror）
  - `stateflow_loops_transitions\resident_{c,d,f,j,k,l}_<name>\` … `normal_loop_breath` / `normal_to_talk_transition` / `talk_loop_breath` / `talk_to_normal_transition_reverse`（各 `_v01_4f_64x96_review_only.png` = 4フレーム横ストリップ）
- generic NPC ソース root:
  `...\docs\review_gallery\imports\stage4_chapter1_generic_npc_v12_v59_recovery_rebuild_2026-05-30\`（`generic_adult_male_a/b`, `generic_adult_female_a/b`, `generic_elder_male/female`, `generic_child_a/b` の directions / idle_breath / selected）

resident id ↔ 名前: `resident_c=Kaia`, `resident_d=Dario`, `resident_f=Mia`, `resident_j=Karla`, `resident_k=Kairo`, `resident_l=Luna`。

## 5. Numbered Mechanical Work（work-unit A→H、各 unit でレビュー）

各 unit は小さく保ち、可視物が出る unit ごとに R2 レビュー（§6）。SoT は生成器コード（main 直編集・手編集シーンは禁止、コードに入れて再生成）。

### A. 資産 promote（コピー＋リネーム）
旧 worktree のソース PNG を **map-vs の `Assets/Art/Characters/FastVS/<Name>/` へコピー＆リネーム**（中間 `docs/review_gallery` を map-vs に持ち込まない）。リネーム規則: `resident_<x>_<name>_<rest>_review_only.png` → `<name>_<rest>.png`、方向 still は `front_v12` → `front_idle_v12`。

- `Assets/Art/Characters/FastVS/Mia/mia_front_idle_v12_64x96.png` ほか front/back/left/right
- `.../Mia/mia_normal_loop_breath_v01_4f_64x96.png` / `mia_normal_to_talk_transition_v01_4f_64x96.png` / `mia_talk_loop_breath_v01_4f_64x96.png` / `mia_talk_to_normal_transition_reverse_v01_4f_64x96.png`
- Kaia/Dario/Karla/Kairo/Luna も同様。generic は `Assets/Art/Characters/FastVS/Generic/{Directions,IdleBreath,Selected}/` にファイル名維持でコピー。
- Aria は既存ランタイム loop があるので**置換しない**（Tom が v47 Aria を新baselineに明示選択した時のみ）。Niro/Reto も触らない。
- 完了基準: Unity が各 PNG に `.meta` 生成、scene/builder 挙動は未変更。**この unit を単独 commit**（`.meta` churn と code 変更を分離）。

### B. パス定数 + material helper + import 検証
`Assets/Editor/AnemoraFastVsHouseSliceSetup.cs` に、既存 Niro/Aria/Reto 定数の近くへ追加。

```csharp
private const string MiaNormalLoopStripPath = CharacterDirectory + "/Mia/mia_normal_loop_breath_v01_4f_64x96.png";
private const string MiaTalkLoopStripPath   = CharacterDirectory + "/Mia/mia_talk_loop_breath_v01_4f_64x96.png";
private const string MiaFrontStillPath      = CharacterDirectory + "/Mia/mia_front_idle_v12_64x96.png";
// back/left/right、および Kaia/Dario/Karla/Kairo/Luna も同型で。generic は variant 別にグループ化。
```
helper（`SpriteStripMaterial` 経由＝baked shaded pipeline を通す。`SourceTextureMaterial` は使わない）:
```csharp
private static Material NamedNpcLoopMaterial(string id, string texturePath)        => SpriteStripMaterial(id, texturePath, Color.white, 4);
private static Material NamedNpcStillMaterial(string id, string texturePath)       => SpriteStripMaterial(id, texturePath, Color.white, 1);
private static Material GenericNpcLoopMaterial(string id, string path, Color tint) => SpriteStripMaterial(id, path, tint, 4);
```
`EnsureExternalCharacterAssets()` を拡張 or `EnsurePromotedCharacterAssets()` 新設: 全 promoted PNG を force-import + `EnsureTextureImporter(...)`、必須ファイル欠落で**validation を loud fail**。完了基準: builder compile・欠落検出が効く。scene 挙動は未変更。

### C. named cast を既存エリアへ **新規配置（位置は仮）**
**テンプレート**: 既存の Aria 配置（`CreateLibrary` 内 :25714、`Past_Library_AriaIdleAtTable`、height 1.18f、`FastVsSpriteStripLoopAnimator` 2.2FPS、3層 contact shadow）と Reto（`CreateRetoAtLibraryDesk`:80181）をコピー元にする。汎用は `CreateSpriteCharacter`(:80172) / `CreateSpriteCardParts`(:83135)。

各 named char を canon エリアの builder メソッド内に**新規スポーン**（中立な GameObject 名、例 `FastVS_SpriteCharacter_Mia`）。material は `NamedNpcLoopMaterial("mia_normal_loop_breath_sprite", MiaNormalLoopStripPath)`、`FastVsSpriteStripLoopAnimator`（frameCount=4, FPS は §F）。配置エリア:

| Char | 配置エリア (builder メソッド:行) | canon 方位 | 役割メモ |
|---|---|---|---|
| Mia | `CreateMiaChapter1Map`(10910)/`CreateMiaHouseExteriorContinuation`(11050) | SE | 作業机付近、normal_loop_breath。寡婦/単身 30-40、元縫製。warm |
| Kaia | `CreateKaiaFarmChapter1Map`(10934)/`CreateKaiaFarmContinuation`(11292) | NE | 畑作業。grounding 重視 |
| Dario | `CreateStreetCornerContinuation`(11204) + Kaia農場 | E | 露店/茶葉（Erythria 伏線は in-game台詞のみ・メタ名禁止） |
| Karla | `CreateAriaHousePlazaContinuation`(14398) | E | Aria に商いを教える。Aria と scale 比が分かるように |
| Kairo | 街角/Aria area | E | 背景・音楽。前面化しない |
| Luna | 街角/Aria area | E | 子供・遊び。**child scale を adult に正規化しない（小柄維持）** |

仮配置の指針: 旧 worktree 生成器（`...\Anemora-chapter1-continuation-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`）の対応配置を**読んで位置/height/向きの参考**にする（merge はしない・コピペ参考のみ）。位置が曖昧でも妥当な所に置いて先へ進む。完了基準: 各エリアキャプチャで各 char が一意に読め、debug ラベル無しで年齢/役割ラダー（Luna < Aria < Niro < 大人）が成立。

### D. generic NPC を遺跡エリアへ新規配置
役割表（元計画 §4.1）に従い `CreateRuinsBridgeContinuation`(13496)/`CreateRuinsSideHomesContinuation`(15495) 等へ generic variant をスポーン（中立名、`GenericNpcLoopMaterial`）。橋の作業者=`generic_adult_male_a/b`、市場商人=`generic_adult_female_b`/`generic_elder_female`、過去住人=`generic_adult_male_b`/`generic_adult_female_a`（過去シルエットは alpha/tint のみ、青緑キャラ化しない）。named char と silhouette が被らないこと。

### E. 共通 contact shadow（grounding）
元計画 §5: Niro の contact shadow ロジックを軽く一般化（`CreateSpriteContactShadow(parent, id, width, depth, alphaScale, localOffset)`、soft oval、transparent unlit・低 alpha・char より後ろの render queue）。**推奨は Option B**（`CreateSpriteCardParts` は不変のまま `CreateSpriteCharacter`/figure 側で付与、Niro の二重 shadow を避ける）。座位 Reto は机で接地済か手動確認してから。外部マップ primitive に model shadow を global 有効化しない。

### F. idle breath loop
`FastVsSpriteStripLoopAnimator`（frameCount=4）。FPS: 大人 2.0–2.4 / Luna・child 2.4–2.8 / 高齢 1.6–2.0。足が滑る・伸びる・接地基準が動くのを避ける。

### G.（任意）`FastVsNpcDialogueSpriteAnimator.cs` 新設
C/D のキャプチャ承認後のみ。`FastVsRetoWritingAnimator` を簡素化したモデル: `normalLoopMaterial/normalToTalkMaterial/talkLoopMaterial/talkToNormalMaterial`、各 frameCount、review メソッド `SetNormalImmediateForReview()/SetDialogueImmediateForReview()/SetDialogueForReview()/SetNormalForReview()`。**talk loop は公開フローでは既定 off**（idle のみ先行）。Niro directional / Reto stateflow に影響させない。story 配線は明示 hook がある所だけ・localized text から状態推測しない。

### H. build/smoke + devlog + R2 captures
§6 の検証一式 → R2 アップロード → Tom 目視ゲート。

## 6. Smoke Test Steps（合否は自前 EXIT echo + log の "return code"/例外 grep で判定。background 通知の exit code は信用しない）

Unity: `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe`。

1. **公開 compile-safety**: Fronkon/Buto を一時退避 + define クリアでも `CreateHouseSliceScene` が compile 通過（本タスクは #if ガード対象を増やさないが、回帰させない）。
2. `-executeMethod AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene`（CS error 0、return code 0）。
3. `ValidateHouseSliceBatch`（既存ドア/ルート検証 pass、"missing chapter 1 continuation route marker" 無し、Niro directional / Reto stateflow / Aria 既存検証が回帰しない）。
4. promoted 資産検証（`EnsurePromotedCharacterAssets` が必須ファイル発見、SpriteStripMaterial が material/texture 生成、named char が正しい material id を使用）。
5. `BuildAndValidateBatch`（.exe ビルド成功）。**FilmGrain 罠**: 再生成後 `grep -c 'active: 1' Assets/Settings/DefaultVolumeProfile.asset` が **20**（B-β/C-β の Buto/Fronkon override 2件込み）か確認。単発再生成で FilmGrain が落ちるので 2パスの `BuildAndValidateBatch` 推奨。
6. .exe 18秒 runtime smoke、Player.log で例外/error 0、キャラ由来の null/missing material 無し、fps 極端低下無し。
7. レビュー画像: §5.8 の close story space を各 unit 後にキャプチャ（Mia家/街角・Aria/Aria家内/Kaia畑/遺跡/遺跡過去）→ `docs/review/<ts>/`（`devlog.txt` 必須）→ **R2 アップロード**（`tools\r2\r2-upload-review.ps1`、git add しない）→ exe フルパス添付で Tom 目視。

## 7. Open Risks / 触ってはいけない所

- **配置は仮**: 座標微調整に時間を使わない。Tom がレビューで動かす。位置より「最終アート・breathing・接地・色（earth-tone、青緑化しない）」を優先。
- **bloat-guard**: `docs/review/`・`docs/devlog/screenshots/`・scene・APV を git add すると pre-commit/pre-push/CI が拒否。レビュー画像は R2、scene は再生成のみ（commit しない）。
- **EULA**: Buto(`Packages/com.occasoftware.buto/`)・Fronkon(`Assets/FronkonGames/`) は gitignore 済・**commit 禁止・触らない**。一方 **v47 キャラアートは first-party なので `Assets/Art/Characters/FastVS/**` を commit してよい**（EULA 問題なし）。
- **pathspec stage 厳守**（`git add -A` 禁止）: map-vs には B-β/C-β の WIP（packages-lock の Buto entry、ProjectSettings の local defines、未追跡 PackageManagerSettings/URPProjectSettings）が残る。これらを巻き込まない。
- **SoT は生成器**: 手編集シーンを commit しない（そもそも scene は untracked）。main は immutable。
- **canon 違反**（§3）: NPC に特別役割を与えない / 名前・方位を変えない / 過去図書館の読書人は無名 / メタデータにネタバレ語彙を出さない（特に Erythria はメソッド名等に出さない、in-game 台詞のみ）。
- **Niro/Reto/Aria を回帰させない**（§5.C は新規 char のみ追加）。
- 元計画一次資料: `C:\Users\maro6\Documents\Unity\Anemora-chapter1-continuation-work\docs\CHAPTER1_FULL_VS_CHARACTER_RUNTIME_IMPLEMENTATION_PLAN.md`。
- 完了後、管理セッションへ「character runtime 実装完了、map-vs tip = <hash>、配置レビュー待ち」と返す。
