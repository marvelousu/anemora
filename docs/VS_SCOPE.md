# Vertical Slice スコープ定義 v1.0

> Anemora の **Vertical Slice (VS) = ここまで作れば「縦切り完成」と呼べる** 範囲を定義する。
> Stage 3 (Day 0-10 目安) で本書を達成し、達成判定 = Stage 4 (α) 着手の前提。
> SPEC.md / PITCH.md と整合し、VS_SCOPE は完成定義・確定済み仕様・残タスクを扱う。流動的な TBD は `docs/STAGE3_TBD_RESOLUTION.md` で tracking する。

> **役割分離**: 本書 (VS_SCOPE) は **VS の "定義書"** (何を作れば VS 完成か)。`docs/STAGE3_PLAN.md` は **VS の "実行計画書"** (どう進めるか・どの順序で何日かけて作るか)。両者の混線を防ぐため、完了条件は本書 §8 を主とし、STAGE3_PLAN 側はそこへの参照に統一する。

> **現状 (2026-05-18 更新)**: 下の "Status (2026-05-06 = Stage 3 closeout)" および本書中の `Anemora_Demo_Playable.exe` 等のビルドパス参照は **当時のスナップショット(歴史的記録)** です。その後 Chapter 1 を絞った Fast Vertical Slice を制作し、2026-05-18 に公開ベースライン `vs-public-2026-05-18`(Windows ビルドを GitHub Releases に添付)をリリースしました。プレイ可否・最新仕様は README「ステータス」、GitHub Releases、最新の `docs/devlog/` を参照してください。
>
> **Status (2026-05-06 = Stage 3 closeout)**: v1.0。E0-E5 + A1-A5 + F1-F4 + G3-G5 の Vertical Slice 必須範囲は完了。Latest closeout commit は `a0bd50b`。G5 manual confirmation により、Windows demo build の通し操作、時の窓 brush preview / generation 精度、右クリック削除、UI 表示、book reflection を Stage 3 完了として受け入れた。残項目は Stage 4 backlog / polish として扱う。

---

## 1. VS の目的 (この VS は何のために作るか)

| 目的 | 優先度 | 何が言えれば達成か |
|---|---|---|
| **コアメカニクス検証** | 最優先 | 時の窓 → 過去踏込み → 持ち帰り → 現在の痕跡確認 のループが「楽しい / 違和感がない」と判定できる |
| **トレイラー素材確保** | 高 | 30 秒トレイラーに使える 6 カット (PITCH §3) 全てが実機キャプチャできる |
| **物語の入口提示** | 中 | プレイヤーが「世界が衰退している」「主人公が能動的に行動できる」「層が剥離する予兆」の 3 点を 10-15 分で体感できる |
| **AI 開発パイプライン実証** | 中 | Claude × Codex × Blender × PixelLab × AIVA で 1 ゾーン分のアセットを 1 ヶ月以内に組み上げる工程を成立させる |

---

## 2. プレイヤー体験 (10-15 分の連続体験)

### シナリオ流れ

```
[ 0:00-1:00 ] オープニング (Stage 3 A トラック /spec で詳細化済。未確定項目は `docs/STAGE3_TBD_RESOLUTION.md` で tracking)
  - Niro (ニロ、provisional) が **Antela の家のベッドで目覚める** (家は窓なし・閉塞感)
  - 含み演出: D-3 (夢を見ていたような、夢を見ていなかったような) + D-7 改 (俯瞰視点で手を見る一瞬の動作) + D-6 弱版 (今日は体がだるい、削除可能フラグ)
  - 主人公は沈黙主人公、感情はテキストウィンドウのみ
  - **ドアの前で家を出るとき、ポケットから時の筆を取り出して気づく**
  - 外に出た瞬間: 朝日 + 風 + 何らかの音 (鳥音の採否は `docs/STAGE3_TBD_RESOLUTION.md` で tracking)、その後は BGM のみで静謐・衰退
  - チュートリアルは過剰なガイドを避け、街の探索の中で時の窓の使い方を体得

[ 1:00-3:00 ] チュートリアル違和感 (主要 1 つ)
  - 街の中心、失われた本のあった場所
  - プレイヤーが時の窓を描く → 赤シンボル選択 → 過去のシーンが立ち上がる
  - 過去に踏込み、本を持ち帰る
  - 現在に戻ると「失われた痕跡」が可視化、住人のセリフが変化

[ 3:00-6:00 ] コアループ再演 (主要違和感の応用 or サイド違和感 1 個)
  - 街の別地点で同じコアループを別の物語素材で繰り返し、「理解した感」を早めに与える
  - サイド違和感 1 個を軽く見せる、または主要違和感を別解で再演
  - 持ち帰る対象は「記憶の断片」(本でも手紙でも歌の譜面でも)

[ 6:00-12:00 ] 物語の入口
  - 主要 NPC との対話 (1-2 人): 過去側 Resident_A は街の過去住人 witness、現在側 Resident_B は廃墟 / 図書館跡の観察者 / 記録者
  - 「衰退は止められないかもしれない」「でも何かは変えられる」というテーマの提示
  - プレイヤーの能動行動が NPC に「現在反映」を起こす (生存・移住・会話変化のいずれか 1 つ、痕跡が見える形で)

[ 12:00-15:00 ] 層遷移片鱗 (層 2 への布石、VS のフィナーレ)
  - VS 終盤、世界に「ルールが書き換わる予兆」を 1 カット入れる
  - 例: 過去で「持ち帰った」はずのものが、未来時代を覗くと別の場所にある
  - 例: NPC が「見たことのない記憶」を語り始める
  - 「次の層がある」ことだけ示し、ルール変化の本体は本編で
  - 30 秒トレイラーの「もっと知りたい」を生む最後のフック
```

### 学習曲線

- **0:00-1:00** — 強制ガイドで時の窓の描き方を教える
- **1:00-3:00** — 主要違和感 1 つを通じてコアループを体得
- **3:00-6:00** — コアループ再演で「理解した感」を確立
- **6:00-15:00** — 物語と層遷移片鱗で「次が見たい」と思わせる

---

## 3. 実装範囲

### 3.1 コアループ要素 (v0.2: 最小成立達成済)

| 要素 | VS 実装 | Stage 4 以降 |
|---|---|---|
| 時の窓描画 | ✅ 実装 (赤シンボル選択 UI 含む) | (機能拡張) |
| シンボル | **赤 (過去) のみ** 1 種 | 白 (現在) / 青 (未来) / 追加シンボル |
| 能動行動 | **持ち帰る** 1 種のみ | 告げる / 押す/動かす |
| 過去への踏込み | ✅ 実装 (現在時間停止 + ジオラマ表示) | 未来踏込み |
| 現在への反映 | ✅ 実装 (痕跡可視化 + NPC セリフ変化) | より広範な世界書換 |

**v0.2 達成状況**: E0-E5 + A2 + G4 のチェーンにより、VS のコアループ最小成立は達成済み (`0644822`)。

達成済みの手触り:

1. SymbolWheel で赤シンボルを選択する。
2. Portal が開く。
3. Player が Past へ移動する。
4. Past 側の本を取得し、ActionRecord に記録する。
5. Current へ帰還する。
6. Bed 上に `Book_Family_Current` が spawn し、過去での行動が現在へ反映される。

検証状態:

- `AnemoraMainPortalWiringRoundTripTests`: green。
- G4 ActionRecord reflection E2E PlayMode test: green。
- Day 1 時点の PlayMode suite: 16/16 green。

> **シンボル UI 表示方針**: シンボル選択 UI には白/青も表示するが、**薄色 / グレーアウト + 選択不可** で「未来に拡張される」予告として機能させる (Stage 4 での誤実装防止)。
>
> **詰み防止**: VS では「時間侵食」状態は発動させず、時の窓は何度でも再描画可能 (詰みが起きない、またはリトライで必ず解除)。
>
> **未来側の扱い**: 未来側 (青シンボル) の具体仕様は Stage 4 で確定 (SPEC §5.2 と整合)。VS 段階では UI 上の演出のみで、機能実装は行わない。
>
> **操作系**: VS はキーボード + マウスで完結。ゲームパッド対応は Stage 4 以降。

### 3.2 第 1 ゾーン: Antela (アンテラ、provisional)

| 項目 | VS 実装規模 | 備考 |
|---|---|---|
| 物理規模 | Niro の家 (窓なし、閉塞) + 中央広場 + 図書館跡 + 周辺 2-3 棟 (歩いて 1-2 分で回れる) | Antela 全体ではなく、街の一画 + 家の内部 |
| 違和感配置 | **主要 1 + サイド 1 = 計 2 個** | **主要 = 失われた記録 / 本の痕跡**、サイドは別の住人の記憶 etc. |
| NPC 配置 | **普通の住人 1-2 人** (異物設定なし、Niro と面識なし) | Resident_A = 過去側 witness / hook、Resident_B = 現在側の観察者 / 記録者。**異物原則**: 異物は主人公のみ (SPEC §4.2 / `STAGE3_TBD_RESOLUTION.md` §2) |
| 探索的バリア | なし (空気感優先) | メトロイドヴァニア要素は VS 範囲外 |

> **新規作成上限**: VS で新規作成する建物モジュールは **最大 3 棟程度**、残りは **再利用モジュール** (向き / テクスチャ / スケール変化) で構成。アセット暴走防止のための上限。

### 3.3 主人公 / アバター

| 項目 | VS 実装 |
|---|---|
| ヒーロービジュアル v1 | ✅ 実装 (PixelLab + Aseprite 仕上げ) |
| 4 方向歩き / 走り / アイドル | ✅ 実装 |
| 表情差分 | プレースホルダ可 (Stage 4 で拡充) |
| 名前 / 性別 / 年齢 | Niro (ニロ、provisional) / 中性表現で最終確定 / 15-19 歳 |
| 見た目 | つばのある旅人風の帽子を含む静かな旅人シルエット |
| 家族 / 知人 | 不在。VS では不在そのものを違和感として扱う |

### 3.4 層遷移片鱗 (VS フィナーレ)

| 項目 | VS 実装 |
|---|---|
| 層 1 完結 | ✅ プレイヤーが「衰退の予兆 → 能動行動 → 痕跡」を理解 |
| 層 2 片鱗 | **1 カットのみ**: 「ルールが書き換わる予兆」を視覚演出 |
| 層 2 のルール本体 | ❌ VS では実装しない (Stage 4 マイルストン) |

> **設計用語注記**: 「層」「段階反転」(旧称: ベール剥離) は制作・設計用の便宜語であり、インゲーム UI / dialogue には出さない。VS の表示文言は違和感、記憶、痕跡、風景変化として表現する。

### 3.5 Stage 3 Day 1 機能ブロック状態

| ブロック | v0.1 状態 | v1.0 反映 |
|---|---|---|
| E0 URP Pipeline | TBD | ✅ 完了。`AnemoraE0Setup.cs` editor automation により URP pipeline baseline を構築。 |
| E1 PortalStencil | TBD | ✅ 完了。`PortalStencilFeature` + `PortalMask.shader` / `InsideOnly.shader`、stencil bit 3 / Mask = 8 / Ref = 8、dual-pass 設計。ADR-0002 v1.1 反映。 |
| E2 ヒエラルキー | TBD | ✅ 完了。`SceneRootRegistry` + `Camera_Past` skeleton。VS では `Camera_Past` を使わず Main Camera の culling 反転で運用。 |
| E3 SymbolWheel | TBD | ✅ 完了。3 シンボル表示、赤のみ活性、白 / 青は preview / disabled。 |
| E4 PortalCrossing | TBD | ✅ 完了。6 状態 state machine + atomic flip。hysteresis 0.02m / minimum normal movement 0.05m / cooldown 0.1s / flash 0.05s。ADR-0005 v1.1 反映。 |
| E5 ActionRecord | TBD | ✅ 完了。`IReflector` + `BookReflector` + `ActionRecordCatalog` + `ActionRecordRuntime`。 |
| A1 DialogueAsset 構造 | TBD | ✅ 完了。`Anemora.Data` POCO + `Anemora.Game` asmdef + `DialogueAsset` SO + `com.unity.localization@1.5.9`。`LocalizationSettings` + `StringTable` seed 実装済み。Batchmode key fallback と non-batchmode StringDatabase 解決を確認済み。 |
| A2 Anemora_Main wiring | TBD | ✅ 完了 (`cb2b6ed`)。`PrototypePlayerController` + 境界往復 PlayMode test。 |
| A3 Zone1 Buildings | TBD | ✅ 完了。Meshy v6 + 3/14 Blender 修復 + atlas + manifest + tools/scripts。 |
| A4 Audio | TBD | ✅ 完了。BGM `Zone1_Ambient.ogg` + SFX 30 種 (環境 6 / 足音 12 / 時の窓 6 / NPC 3 / UI 3) + `Zone1AudioController` wiring 到達済み。 |
| A5 UI 基盤 + ローカライズ | TBD | ✅ JP TMP Atlas (美咲ゴシック) + EN draft (Press Start 2P) + Anemora パレット v0。Stage 3 /spec resolution で provisional 採用。 |
| F1 PixelLab drafts | TBD | ✅ 完了。Hero front / side / back、Resident_A front / back / left、Resident_B seated。 |
| F2 Aseprite 仕上げ | TBD | ✅ 完了。Aseprite 正式版で再エクスポート (`08f61b8`, `4d2092a`)。 |
| F3 Retro Diffusion 補助 | TBD | VS では不要。Stage 4 で revision / alternate candidate が必要になった場合のみ再検討。 |
| F4 Hero/NPC.prefab + Animator | TBD | ✅ 完了 (`d2c95c2`)。Hero / Resident_A / Resident_B prefab + 個別 `AnimatorController` + `HeroAnimatorBinder` + `Anemora_Main` placeholder 置換。 |
| G1/G2 Buildings 採用方針 | TBD | ✅ 解決。A3 Meshy 再生成 = 案 b 採用。 |
| G3 NPC 配置 + 対話 | TBD | ✅ 完了。Resident_A/B placement + `NpcInteractable` + `DialogueDisplay` scaffold + `DialogueAsset` SO 到達済み。A1 `LocalizationSettings` 完了と Locale switch dialog E2E PlayMode test を反映。Resident_A = 過去側 witness、Resident_B = 現在側 observer / recorder として扱う。 |
| G4 ActionRecord トリガー設置 | TBD | ✅ 完了 (`0644822`)。`take_book_001` + `Book_Family_Current.prefab` + `PastBookInteractable` + E2E PlayMode test。 |
| G5 通し体験 + Windows ビルド + 検証マトリクス | TBD | ✅ 完了。`a0bd50b` latest demo buildで user manual confirmation 到達。EditMode `32/32`、PlayMode `29/29` pass。Windows build success、drag preview / generated window一致、右クリック削除、UI表示、book reflectionを確認。 |

---

## 4. アセット規模 (品質と数の境界)

### 4.1 ドット絵 (キャラクター + 重要オブジェクト)

| カテゴリ | VS 実装数 | 品質 |
|---|---|---|
| 主人公スプライト | 1 体 × 4 方向 × 3-4 アニメ (歩/アイドル/筆構え/踏込み) | VS 時点暫定完成 |
| NPC スプライト | 1-2 体 × 4 方向 × 2-3 アニメ | VS 時点暫定完成 |
| 重要オブジェクト (本/樹/手紙) | 2-3 個 | VS 時点暫定完成 |
| 街の小物 | 10-15 個 (再利用前提) | プレースホルダ可、Stage 4 で追加 |

### 4.2 3D 背景

| カテゴリ | VS 実装数 | 品質 |
|---|---|---|
| 建物モジュール | **新規 2-3 棟** + 再利用 (向き/テクスチャ変化で 4-6 棟分の見え) | VS 時点暫定完成 (HD-2D Tier 2 シェーダ適用) |
| 環境装飾 (樹/塀/階段) | 8-12 個 (再利用前提) | VS 時点暫定完成 |
| 街の床タイル | 2-3 種 | VS 時点暫定完成 |
| 遠景 / スカイボックス | 1 セット | プレースホルダ可、Stage 4 で差替え |

### 4.3 VFX / シェーダ

VFX は時の窓 / 痕跡可視化 / 層 2 片鱗の **3 つに限定** (環境装飾パーティクルは VS 範囲外):

| カテゴリ | VS 実装 | 品質 |
|---|---|---|
| 時の窓ポータル (ステンシル + 時間境界エフェクト) | ✅ 実装 | **完成品質 FIX** (これが目玉、Stage 4 でも改修しない) |
| 痕跡可視化 (光の粒 / 色変化) | ✅ 実装 | **完成品質 FIX** |
| 層 2 片鱗演出 (1 カット) | ✅ 実装 | **完成品質 FIX** |
| 動的影 (HD-2D Tier 2) | ✅ 実装 | VS 時点暫定完成 |
| パーティクル全般 (環境装飾) | VS 範囲外 | Stage 4 で実装 |

---

## 5. 音響規模

音響方向性は v0.1 の **静謐 / 衰退 / メランコリック、暗黒一辺倒ではない** 方針を維持する。DAW は Studio One に統一済み (Reaper 表記は ADR-0003 と asset prompt 側で訂正済み)。

### 5.1 BGM

VS 必須は街アンビエント 1 曲のみ。それ以外は変調 / ループ再利用で代用可。A4 BGM は Suno `Dustlight Piano B` の一発出しを採用し、AIVA は比較素材として不採用。SFX 30 種は ElevenLabs / Stable Audio / Studio One foley で生成・整理済み。

| トラック | VS 実装 | 品質 |
|---|---|---|
| 街アンビエント (常時 BGM) | Suno `Dustlight Piano B` 採用 / `Zone1_Ambient.ogg` import 済み。`Zone1AudioController` から再生する | VS 時点暫定完成 |
| 時の窓使用時の演出曲 | 街アンビエントの変調で代用。Low-pass + 楽器抜き + pitch shift -2 semitones を VS で実装 | プレースホルダ可 |
| 層遷移片鱗演出曲 | 既存 BGM の変調で代用 | Stage 4 で固有曲に差替 |

詳細 prompt / export 方針は `docs/asset_prompts/bgm_zone1_ambient.md` を参照。

### 5.2 環境音 / SFX

| カテゴリ | VS 実装 |
|---|---|
| 環境音 | 6 種 |
| 足音 | 12 種 |
| 時の窓 SFX | 6 種 |
| NPC 反応 SFX | 3 種 |
| UI SFX | 3 種 |

SFX 30 種は `docs/asset_prompts/sfx_zone1.md` v1.0 draft に沿って生成・import 済み。`Zone1AudioController` は環境音、足音、時の窓、NPC、UI の各カテゴリを参照し、G5 で聴感 / trigger wiring を確認する。

### 5.3 ボイス

採用しない (SPEC §3.4 / §8.1 確定)。テキスト + 環境音 + 余白で語る。

---

## 6. UI 規模

| 要素 | VS 実装 | 品質 |
|---|---|---|
| タイトル画面 | プレースホルダ可 | Stage 4 で完成 |
| HUD (最小限ヒント表示) | ✅ 実装 | VS 時点暫定完成 |
| 時の窓シンボル選択 UI | ✅ 実装 (赤のみ選択可、白/青は薄色グレーアウトで予告のみ) | VS 時点暫定完成 |
| 対話 UI (テキストボックス + 選択肢) | ✅ 実装 (沈黙主人公: NPC 一方的話 + 主人公感情/反応の選択肢のみ) | VS 時点暫定完成 |
| インベントリ | プレースホルダ可、または非表示 | Stage 4 で実装 |
| 進行ログ | プレースホルダ可、非表示でも可 | Stage 4 で実装 |
| メニュー (ESC) | 最小機能 (タイトルへ戻る + 設定 + セーブ) | VS 時点暫定完成 |
| セーブ / ロード | **オートセーブ必須**、手動セーブはプレースホルダ可 | Stage 4 で手動セーブ実装 |

---

## 7. 完成度品質の境界

VS = 「最終クオリティで作り込んだ縦切り」と理想は持ちつつも、Day 0-10 の現実スコープに合わせて **FIX / VS 時点暫定完成 / プレースホルダ可** の 3 段階で品質管理する:

### FIX エリア (Stage 4 でも改修しない、コア機構のみ)

- 時の窓ポータルシェーダ + ステンシル実装
- 時の窓のコアループ動作 (描画 → シンボル選択 → 過去踏込み → 持ち帰り → 現在反映)
- 層 2 片鱗演出の核 (1 カット)
- 主要違和感 1 個の反応演出 (痕跡可視化)

### VS 時点暫定完成エリア (Stage 4 で小修正許容)

- 主人公ドット絵スプライト v1 (4 方向 × 3-4 アニメ)
- NPC スプライト 1-2 体
- 街中央広場 + 周辺 2-3 棟の HD-2D Tier 2 レンダリング
- 街アンビエント BGM 1 曲
- 時の窓 SFX
- HUD / 対話 UI / ESC メニュー (最小機能)
- 動的影 (HD-2D Tier 2)

### プレースホルダ可エリア (Stage 4 で本実装)

- タイトル画面 / クレジット
- インベントリ / 進行ログ UI
- 手動セーブ
- 街の小物 (再利用 + 一部灰色)
- 遠景 / スカイボックス
- 時の窓演出曲 / 層 2 片鱗演出曲 (既存 BGM 変調で代用)
- 環境装飾パーティクル

---

## 8. 完了条件チェックリスト

VS 達成判定は **3 段階** に分ける。**必須** 全 YES = VS 完成。**推奨** は削減トリガー時に外せる。**Stage 4-5 寄り** は VS 判定では問わない。v1.0 では G5 manual confirmation と latest test/build result を反映し、Stage 3 は完了として扱う。

### 必須 (VS 達成判定の死守ライン、全 YES が VS 完成の条件)

| 状態 | 項目 | v1.0 備考 |
|---|---|---|
| ✅ | **ニューゲームから VS 終端まで、1 セッションで破綻なく通しプレイ可能** (10-15 分) | `a0bd50b` latest demo buildで user manual confirmation 到達。Stage 4 は polish / expansion から開始。 |
| ✅ | 時の窓描画 → 赤シンボル選択 → 過去踏込み → 持ち帰り → 現在反映 の **コアループが破綻なく動作** | E0-E5 + A2 + G4 + demo brush repairで達成済み。Latest PlayMode `29/29` green。 |
| ✅ | **主要違和感 1 個** が機能 (反映の痕跡が確認できる) | G4 の本取得 → Current 側 Bed 上 book spawn で達成済み。 |
| ✅ | **層 2 への片鱗演出が 1 カット** 入っている | VSでは本の反映、現在側痕跡、時の窓ジオラマによる minimum hint を採用。ルール本体はStage 4以降。 |
| ✅ | 主人公スプライト v1 が動作 (品質は VS 時点暫定で可) | F2 / F4 完了。Hero prefab + Animator + `HeroAnimatorBinder` 導入済み。 |
| ✅ | 街中央広場 + 周辺の HD-2D Tier 2 レンダリングが動作 (1 ゾーン成立) | A3 buildings 完了。F4 prefab、UI 基盤 v0 も到達済み。 |
| ✅ | **Windows ビルドが起動 → タイトル → ゲーム本体まで動作** | Latest build: `<worktree:Anemora-demo-repair>\Builds\DemoPlayable\Anemora_Demo_Playable.exe`。build success、runtime Player.log exception-free。 |
| ✅ | 詰みが起きない (時の窓再描画で必ず解除可能) | `Shift` + left-drag preview / generated window一致、右クリック削除、再描画導線を user manual confirmation 済み。 |

### 推奨 (達成すべきだが、削減トリガー時に外せる)

| 状態 | 項目 | v1.0 備考 |
|---|---|---|
| Stage 4 | サイド違和感 1 個 | Stage 3 完成判定からは削減。Stage 4 content expansion で再評価。 |
| ✅ | NPC 1-2 人と対話可能、**少なくとも 1 人に「現在反映」が見える** | Resident_A/B dialogue、book reflection、locale switch、save/load related PlayMode tests green。 |
| ✅ | 街アンビエント BGM 1 曲 | `Zone1_Ambient.ogg` import済み。Stage 3 blockerなし。細かい loop / balance polish は Stage 4。 |
| ✅ | 時の窓 SFX (描画/シンボル/踏込み/持ち帰り) | SFX 30 種 import済み。`Zone1AudioController` wiring test green。細かい音量 / 素材差替えは Stage 4。 |
| ✅ | オートセーブが動作 | SaveEnvelope / ActionRecord round-trip と PlayMode save/load integration green。手動 save UI は Stage 4。 |
| Stage 4 | ESC メニュー → タイトルへ戻る が動作 | VS 判定からは削減。Stage 4 UI/menu workstream で実装。 |

### Stage 4-5 寄り (VS 判定では問わない、達成できれば加点)

| 状態 | 項目 | v1.0 備考 |
|---|---|---|
| 残 | 30 秒トレイラー (PITCH §3) の 6 カット全てが実機キャプチャ可能 | G5 通し体験後に素材化判断。 |
| 残 | Linux ビルドが起動 → タイトル → ゲーム本体まで動作 (Mac は Stage 4 まで保留可) | VS 判定外。 |
| 進行中 | アセット完成品質 FIX (Stage 4 でも改修しない水準) | コア機構は FIX。sprite / building / audio は VS 時点暫定完成として扱う。 |
| ✅ | BGM / SFX が固有曲・固有素材で完成品質 | BGM + SFX 30 種は VS 時点素材として到達済み。G5 で build / 聴感 / trigger を確認する。 |

---

## 9. スコープ削減トリガー (VS 内)

VS 制作中に進捗が遅れた場合、以下の順で削減する。**死守ライン (コアループ + 主要違和感 1 個 + 層 2 片鱗 + 1 セッション完走)** は最後まで守る。後工程依存の重い項目から先に落とす:

| 削減順 | 項目 | 削減後の体験 |
|---|---|---|
| 1 | **§8 Stage 4-5 寄り全項目** (Linux ビルド / 30 秒トレイラー素材 / アセット完成品質 FIX / BGM・SFX 固有素材) | VS 判定要件外、即削減可、最優先 |
| 2 | BGM / SFX の完成品質 | BGM 変調 + 環境音 + 最小 SFX のみ |
| 3 | サイド違和感 1 個 → 0 個 | 主要違和感のみ + 物語入口で 5-7 分の VS |
| 4 | NPC を 2 人 → 1 人 | 普通の住人 1 人だけで物語入口、現在反映は痕跡可視化で代替 |
| 5 | NPC「現在反映」要件を緩和 | 会話変化のみ (生存・移住の演出は省略) |
| 死守 | コアループ + 主要違和感 + 層 2 片鱗 + 1 セッション完走 | ここから先は削減不可、削るなら VS の意味が消える |

---

## 10. Stage 3 から Stage 4 への引継ぎ

VS 達成後、Stage 4 (α) で対応:

- 白 / 青シンボルの実装
- 告げる / 押す/動かす の能動行動
- 残りゾーン (B/C/D/E) の実装
- 層 2-5 + 真層の本実装
- インベントリ / 進行ログ UI
- 手動セーブ / ロード
- タイトル画面完成

---

## 11. SPEC v1 への反映予定

VS 達成後、SPEC を v1 に改訂:

- `docs/STAGE3_TBD_RESOLUTION.md` で user 確定済みになった項目だけを反映
- §13.3 オープン要件の更新 (Stage 3 で解決済を消し込み)
- §13.4 改訂履歴に v1 エントリ追加
- VS 体験で発見した設計上の問題を該当章に反映

---

## 12. ADR 改訂結果 / 実装参照

Stage 3 Day 1 の実装結果は以下の ADR / docs に反映済み。

| 参照 | 状態 | VS_SCOPE v0.4 への反映 |
|---|---|---|
| `docs/adr/0002-time-frame-portal-stencil.md` | v1.1 / Accepted (`02f5c22`) | E1 確定値。stencil bit 3、Mask = 8 / Ref = 8、dual-pass shader、URP StencilLight 競合経緯を §3.1 / §3.5 へ反映。 |
| `docs/adr/0005-time-management-scene-switching.md` | v1.1 / Accepted (`3a29757`) | E4 確定値。PortalState 6 状態、atomic flip ordering、hysteresis 0.02m / minimum 0.05m / cooldown 0.1s / flash 0.05s を §3.1 / §3.5 へ反映。 |
| `docs/adr/0008-localization.md` | v0.3 / Accepted (`2cf0dfa`) | `Anemora.Data` POCO と runtime/UI 層の asmdef 境界、TMP Atlas 方針、`LocalizationSettings` / StringTable seed 完了を A1 / A5 / G3 状態へ反映。 |
| `docs/adr/0009-asset-pipeline.md` | Proposed (`cbb6ac1`) | PixelLab + Aseprite、Meshy + Blender、Studio One、TMP Atlas、ledger 運用を asset pipeline の Stage 4 入口参照として追加。 |
| `docs/G5_ACCEPTANCE_MATRIX.md` | draft ready (`7c4a258`) | G5 通し体験 / Windows build / acceptance test 36 項目の実行表として §8 へ接続。 |

## 13. TBD tracking

VS_SCOPE は完成定義と確定済み実装状態を扱う。Stage 3 /spec resolution interview で解決した user 判断項目は `docs/STAGE3_TBD_RESOLUTION.md` に解決日と反映 commit hash を残す。未確定または Stage 4 再評価の項目は同 sheet で tracking し、本書では個別候補を列挙しない。

対象例:

- Stage 4 で再評価する art / palette / font revision。
- 真層の収束パターン、主人公の創造主体など Stage 4 以降の story bible 項目。
- Public release / license の運用詳細。

確定後は `docs/STAGE3_TBD_RESOLUTION.md` の該当 row に確定日と反映 commit hash を残し、必要なものだけ VS_SCOPE / SPEC / localization / asset docs へ反映する。

## 14. 関連文書

- `SPEC.md` (Stage 2 GDD v0.1)
- `PITCH.md` (Stage 2 公開ピッチ)
- `docs/STAGE3_PLAN.md` (Stage 3 計画書、本書はその D トラックの成果物)
- `docs/STAGE3_TBD_RESOLUTION.md` (Stage 3 user 判断保留 tracking)
- `docs/G5_ACCEPTANCE_MATRIX.md` (G5 acceptance test 実行表)
- `docs/adr/0001-engine-unity6.3-lts.md` (エンジン採用根拠)
- `docs/adr/0002-time-frame-portal-stencil.md`
- `docs/adr/0005-time-management-scene-switching.md`
- `docs/adr/0008-localization.md`
- `docs/adr/0009-asset-pipeline.md`

---

## 15. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草 (ユーザー判断 3 軸: プレイ時間 10-15 分 / コアループ最小 / 層 2 片鱗) |
| v0.1 | 2026-05-04 | Codex (fast) レビュー 12 件全件反映 (P0×4 / P1×4 / P2×4): §1 役割分離明文化 / §2 ペーシング短縮 / §3.1 グレーアウト+詰み防止+未来側+操作系明文化 / §3.2 建物上限 / §4 アセット規模縮小 / §6 UI を VS 暫定完成に / §7 FIX 範囲をコア機構のみに / §8 完了条件 3 段階化 / §9 削減順再編 |
| v0.1a | 2026-05-04 | Stage 3 A トラック /spec 反映: §2 オープニング詳細化 (家ベッド + D-3/D-7/D-6 含み演出 + 時の筆発見タイミング) / §3.2 NPC 配置を「普通の住人 1-2 人、異物原則」に訂正 (老人撤回) / §6 対話 UI に沈黙主人公 反映 |
| v0.2 | 2026-05-05 | Stage 3 Day 1 進捗の整合反映 (E0-E5 / A1-A3 / F1-F4 / G4 完了状態) / ADR-0002, ADR-0005, ADR-0008, ADR-0009 改訂結果反映 / TBD 項目を `docs/STAGE3_TBD_RESOLUTION.md` へ移譲 / §3.1 コアループ最小成立達成を明記 / 残タスク表 (G3 final / G5 / Audio) を最新化 |
| v0.3 | 2026-05-05 | Audio 完成 (BGM + SFX 30 種 + `Zone1AudioController`) / G3 Localization 完成 (`LocalizationSettings` + Locale switch test) / §8 死守ラインを G5 残のみに整理 |
| v0.4 | 2026-05-05 | Stage 3 /spec resolution interview 反映: Niro / Antela provisional 採用、主人公中性表現・15-19 歳・旅人風の帽子、Resident_A / Resident_B 役割、art / palette / font provisional 採用、設計用語をインゲームに出さない注記 |
| v1.0 | 2026-05-06 | Stage 3 closeout。`a0bd50b` demo brush repair、EditMode `32/32`、PlayMode `29/29`、Windows demo build success、user manual confirmation を反映し、VS 必須条件を完了扱いへ更新。 |

---

> **End of VS_SCOPE v0.4**
> Stage 3 実装の完成定義として運用。Stage 3 完了後、SPEC v1 改訂と合わせて v1 へ昇格。
