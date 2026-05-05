# Stage 3 F トラック実装計画書 — 主人公ヒーロービジュアル

> ADR-0003 (アセットパイプライン)、`docs/STAGE3_TBD_RESOLUTION.md` §1 (主人公確定仕様)、ADR-0007 (UI フレームワーク + Pixel Perfect)、`docs/legal/asset_ledger.md` を接続し、主人公スプライト v1 を **PixelLab → Aseprite → (補助で Retro Diffusion) → Unity 取り込み** の流れで成立させる。
>
> 本書は実装計画。設計判断は ADR を参照する。

> **Status (2026-05-04 = Day 1 起草)**: v0.1。Phase 分解と検証マトリクスを確定、F0 着手前のレビュー対象。

---

## 0. 目的とスコープ

### 0.1 F トラックで成立させるもの

VS_SCOPE.md §4.1 / §7 「ヒーロービジュアルは VS 時点暫定完成」に到達するため:

1. **主人公スプライト v1** — 立ち姿 + 4 方向 (前 / 後 / 左 / 右) + Idle ループ
2. **D-7 演出用の手アセット** — オープニングで主人公が自分の手を見る一瞬の演出 (`STAGE3_TBD_RESOLUTION.md` §4.3)
3. **Animator Controller** — Idle / Walk 4 方向の最小遷移
4. **Asset ledger 記載** — `docs/legal/asset_ledger.md` §2.1 にエントリ追加
5. **Unity 取り込み完了** — `Assets/Art/Sprites/Hero/v1/` に配置、Pixel Perfect Camera と整合

### 0.2 F トラックで作らないもの

- Walk 以外の高度なアニメ (走り / ジャンプ / しゃがみ) — Stage 4 以降
- 戦闘モーション — VS は戦闘なし
- 高解像度ヒーロービジュアル (タイトル画面用) — Stage 4-5
- 表情差分の網羅 — VS では Idle / Walk + D-7 用最小限のみ
- ボイス関連 — Silent protagonist (ADR-0003)

### 0.3 完了条件

- §0.1 の 1-5 がすべて揃い、E5 (ActionRecord 痕跡反映の現在側) と G トラック (第 1 ゾーン配置) が **主人公スプライトを参照して画面が成立する** 状態
- 検証マトリクス §6 の必須項目すべて pass
- ユーザー (maro6052@gmail.com) が「中性的・両読み可能・Anemora らしい」と最終判断したものが v1 として確定

---

## 1. 前提資料

| 文書 | 関係 |
|---|---|
| `docs/STAGE3_TBD_RESOLUTION.md` §1, §4.3 | 主人公確定仕様 / D-7 演出 |
| `docs/adr/0003-asset-pipeline.md` | PixelLab + Retro Diffusion + Aseprite 採用根拠 |
| `docs/adr/0007-ui-framework-ugui.md` | Pixel Perfect Camera + Canvas Scaler 整合 |
| `docs/adr/0004-project-directory-structure.md` | `Assets/Art/Sprites/Hero/v1/` 配置 |
| `docs/legal/asset_ledger.md` §1.2, §2.1 | 商用ライセンス記録 |
| `docs/VS_SCOPE.md` §4 / §7 | アセット規模 / 暫定完成境界 |
| `SPEC.md` §4.1 | 主人公仕様 |

---

## 2. 主人公確定仕様の視覚化軸 (`STAGE3_TBD_RESOLUTION.md` §1 から抽出)

| 軸 | 確定値 | 視覚化への影響 |
|---|---|---|
| 性別 | 中性的・両読み可能 | 髪型 / 服装 / 体格を中性的シルエットへ。Frisk (Undertale) / Yume Nikki 系統が参照 |
| 年齢 | 仮置き 10 代後半〜20 代前半 | 顔立ちは若いが大人寄り、全体的に細身でやや背が低め程度 |
| 沈黙 | Silent protagonist | 口元は閉じ気味、叫び / 笑顔の強い表情を v1 では作らない |
| 偽記憶 | ぼんやり覚えている程度 | 表情は穏やか、神妙さや力みは出さない |
| 異物性 | 最終盤まで伏せる | 服装 / 持ち物 / 髪色で**特殊性を強調しない**。街の住人として違和感なく溶け込む |
| 動機 (表層) | 家族に会いたい | 静謐で日常的な雰囲気、悲壮感を出さない |
| 時の筆 | ポケットに忍ばせる遺物 | v1 では時の筆を視覚化しない (使用シーンのみで描画、E トラック側) |

### 2.1 やってはいけない (異物原則)

- 髪色を奇抜な色 (純白 / 赤紫など) にする
- 目を発光・特殊紋様にする
- 服装に明らかな異界アイテム (機械パーツ / 古代文字) を入れる
- 周囲に常時オーラ / パーティクルを纏わせる
- 大人びすぎ / 幼すぎでターゲット年齢から外す

これらを避けることで「あなたはまだ知らないが、この子はループの中の異物である」という最終盤の反転が機能する。

---

## 3. Phase 分解

### Phase F0: 商用ライセンス確認 + プロンプトテンプレート整備

**目的**: 生成着手前の前提条件を確定。

**成果物**:
- `docs/legal/asset_ledger.md` §1.2 の PixelLab / Retro Diffusion / Aseprite 行に **現在のプラン状態** を記載 (paid 加入済 / 未加入)
- `docs/asset_prompts/hero_v1.md` (新規ディレクトリ) — PixelLab / Retro Diffusion 用のプロンプトテンプレート
- 参照画像のキュレーション一覧 (使用しない参照画像も明記、Midjourney 系の生成物を参照しないなど)

**手順**:
1. **PixelLab paid plan** の契約状態を確認 (asset_ledger §1.2 PixelLab 行を更新)
   - 未契約の場合: ユーザー (maro6052@gmail.com) に契約意思を確認、契約済になるまで F1 着手を保留
2. **Retro Diffusion paid plan** の契約状態を確認 (補助ツール、必須ではない。F1 で必要になった時点で再判定)
3. プロンプトテンプレートを `docs/asset_prompts/hero_v1.md` に起草:
   - **基本軸**: "androgynous teenage protagonist, neutral expression, plain everyday clothing, HD-2D inspired pixel art, 32x48 resolution, side profile / front / back views"
   - **避けるべき要素**: "no glowing eyes, no exotic hair color, no fantasy armor, no obvious weapons, no aura effects"
   - **絵柄参照**: "Octopath Traveler / Triangle Strategy / Sea of Stars early game inspired"
   - **空気感**: "calm, slightly melancholic, quiet"
4. プロンプトテンプレートを v0 として commit、F1 で結果を見て v1 へ改訂

**担当**: Linux Claude (プロンプト起草) + Windows Codex (PixelLab UI から契約状態確認) + ユーザー (paid 加入意思決定)

---

### Phase F1: PixelLab v1 ドラフト生成

**目的**: 主人公の v1 ドラフトを生成、ユーザー判断のための候補出し。

**成果物**:
- PixelLab 出力 (前 / 横 / 後、各 4-8 枚 = 計 16-24 枚) を `art/_intermediate/hero_v1_pixellab/` に保管 (gitignore 対象)
- 候補絞り込み済の 1 枚ずつ (前 / 横 / 後 = 3 枚) を `Assets/Art/Sprites/Hero/v1/_draft/` に配置
- `docs/asset_prompts/hero_v1.md` v1 (実プロンプトに更新)
- `docs/devlog/2026-05-XX_f1_pixellab_draft.md` (生成過程と所感)

**手順**:
1. F0 のプロンプトテンプレートで PixelLab 生成実行 (前 4-8 枚 → 横 → 後)
2. 中間ファイル全件を `art/_intermediate/` へ保存 (gitignore のため commit されない)
3. 候補絞り込み: 中性表現 / 異物原則 / Anemora らしさを基準に各方向 2-3 枚へ
4. ユーザーレビュー (Discord / 直接見せる等) で 1 枚ずつ選定
5. 選定済 3 枚を `Assets/Art/Sprites/Hero/v1/_draft/{front,side,back}_v1.png` に配置
6. devlog に「採用したプロンプト / 不採用にした方向 / Aseprite 仕上げ前の所感」を記録

**重要な判断点**:
- 中性に振りすぎて「無個性」にならないか
- 異物原則を守りすぎて「主人公として強度がない」にならないか
- HD-2D Tier 2 (動的影 + 単一方向光) でシルエットが破綻しないか

**担当**: Windows Codex (PixelLab GUI 操作) + ユーザー (候補絞り込み最終判断) + Linux Claude (devlog 整理 / プロンプト次版起草)

---

### Phase F2: Aseprite 仕上げ

**目的**: 色調 / シルエット / 解像度を Anemora 全体パレットへ整合させる。

**成果物**:
- `Assets/Art/Sprites/Hero/v1/hero_idle.png` (4 frames idle ループ)
- `Assets/Art/Sprites/Hero/v1/hero_walk_front.png`, `hero_walk_back.png`, `hero_walk_left.png`, `hero_walk_right.png` (各 4 frames)
- `Assets/Art/Sprites/Hero/v1/hero_stand.png` (立ち姿 1 frame、Idle 起点)
- `Assets/Art/Sprites/Hero/v1/hero_hands_d7.png` (D-7 演出用、左右の手だけのカット)
- Anemora パレット v0 (Aseprite palette ファイル `Assets/Art/anemora_palette_v0.aseprite-palette` または `.gpl`)

**手順**:
1. F1 の 3 枚を Aseprite で開き、解像度 32x48 に揃える (HD-2D 風 = Octopath より少し大きめ)
2. 色数を限定パレットに圧縮 (16-32 色目安)、Anemora パレット v0 を `anemora_palette_v0` として保存
3. 立ち姿 (Idle 起点) を確定 → Idle 4 frames (微妙な呼吸 / 揺れ)
4. Walk 4 方向 (前 / 後 / 左 / 右、左右は反転で可) を各 4 frames
5. D-7 用「手を見る」カット: 左右の手のクローズアップ (32x48 とは別解像度可、画面演出に合わせる)
6. すべて PNG export → `Assets/Art/Sprites/Hero/v1/` に配置
7. 中間 .aseprite ファイルは `art/_intermediate/hero_v1_aseprite/` に保管 (gitignore)

**仕上げの完了条件 (ADR-0003 §人手仕上げの完了条件 準拠)**:
- 色調統一 (パレット v0 内の色のみで構成)
- シルエットが 32x48 内で読み取れる
- 動的影が乗る輪郭になっている (URP HD-2D Tier 2 で破綻しない)
- 表情は穏やか、Silent protagonist 整合
- 手アセットは D-7 演出に必要な情報量を持つ (見間違える曖昧さは残しつつ、明確に「手」と分かる)

**担当**: Windows Codex (Aseprite 操作) + ユーザー (パレット最終判断) + Linux Claude (人手仕上げ完了条件のレビュー)

---

### Phase F3: Retro Diffusion 補助 (条件付き)

**目的**: F2 で質感が物足りない場合、Retro Diffusion でテクスチャ / 質感を補強。

**条件**:
- F2 出力をユーザーが見て「質感がもう一段欲しい」と判断した場合のみ着手
- それ以外はスキップして F4 へ

**成果物**:
- `art/_intermediate/hero_v1_retrodiffusion/` (中間ファイル、gitignore)
- 必要に応じて F2 の Aseprite 仕上げに retrofit (Aseprite 側で再仕上げ)

**手順**:
1. F2 の立ち姿を Retro Diffusion に img2img で投入 (img2img 機能がない場合は別手段)
2. 質感差分を Aseprite で取り込み (色調 / 影の付き方 / シルエット粗 / etc.)
3. F2 v2 として更新 → `Assets/Art/Sprites/Hero/v1/` を上書き

**判定**: F2 で十分なら不要。実機で動かして F4 後の感触で再判断してもよい。

**担当**: Windows Codex (条件付き)

---

### Phase F4: Unity 取り込み + Animator 設定

**目的**: スプライトを Unity に取り込み、Animator Controller で Idle / Walk が動く。

**成果物**:
- `Assets/Art/Sprites/Hero/v1/` のスプライトに `Pixel Per Unit = 32` (32x48 sprite に対する標準値、要実機確認) と `Filter Mode = Point` を設定
- `Assets/Art/Sprites/Hero/v1/hero_idle.controller` (Animator Controller)
- `Assets/Prefabs/Characters/Hero.prefab` (Hero GameObject、SpriteRenderer + Animator + 仮 CharacterController)
- E5 / G トラック側からこの Prefab を参照可能な状態

**手順**:
1. PNG をすべて Sprite として import、Pixel Perfect 設定
2. `hero_idle.controller` を作成、States: `Idle`, `WalkFront`, `WalkBack`, `WalkLeft`, `WalkRight`
3. Parameters: `MoveX (float)`, `MoveY (float)`, `IsMoving (bool)`
4. Transitions: Idle ↔ Walk* (`IsMoving` で発火、`MoveX/MoveY` で方向選択)
5. Hero.prefab に SpriteRenderer + Animator + CharacterController を attach
6. E1 Sandbox シーンに置いて Idle / Walk が動くか確認

**担当**: Windows Codex
**Linux 関与**: Animator 構造のレビュー、E トラックとの接続確認

---

### Phase F5: asset_ledger 記載 + 完了報告

**目的**: 法務台帳を整え、F トラックを締める。

**成果物**:
- `docs/legal/asset_ledger.md` §2.1 に v1 アセットを行追加 (各方向 / Idle / Walk / D-7 用ハンド)
- `docs/devlog/2026-05-XX_f_track_complete.md` (完了報告)

**台帳記載例**:

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| hero_v1_idle | hero_idle.png | 2026-05-XX | PixelLab + Aseprite | PixelLab paid + Aseprite owned | プロンプト hero_v1.md / なし | Aseprite 全フレーム手仕上げ | 可 (PixelLab paid 商用権) | GitHub Public 可 | Tier 1 player-consumed | v1 |
| hero_v1_walk_front | hero_walk_front.png | (同上) | (同上) | (同上) | (同上) | (同上) | (同上) | (同上) | (同上) | (同上) |

**担当**: Linux Claude (台帳整理) + Windows Codex (生成日 / プラン状態の事実確認)

---

## 4. 出力規格

### 4.1 解像度 / pixel density

| 種別 | 解像度 | Pixel Per Unit (Unity) |
|---|---|---|
| 主人公スプライト (前 / 後 / 左 / 右) | 32x48 px | 32 |
| 立ち姿 (Idle 起点) | 32x48 px | 32 |
| Idle ループ | 32x48 px × 4 frames | 32 |
| Walk × 4 方向 | 32x48 px × 4 frames each | 32 |
| D-7 用ハンド (1 cut) | 64x32 px (横長想定、要調整) | 32 |

`32x48` は Octopath Traveler の `16x32` より一回り大きく、Sea of Stars 序盤の主人公 (~32x48) に近い HD-2D Tier 2 想定。E トラック / G トラックでカメラ設計と合わなければ Stage 3 中に再調整 (ADR 改訂は不要、実装計画書の改訂で吸収)。

### 4.2 パレット

- Anemora パレット v0 を Aseprite `.gpl` で管理
- 16-32 色目安 (HD-2D 系のレトロ感を残す)
- 主人公服装は中性 / 普通 / 静謐な色 (彩度低めの茶 / 青灰 / 砂色など)
- 髪色は黒〜濃茶〜灰の範囲、奇抜な色は避ける

### 4.3 アニメーション仕様

| 種類 | フレーム数 | フレームレート | ループ |
|---|---|---|---|
| Idle | 4 | 6 fps (重め) | あり |
| Walk × 4 方向 | 4 each | 8 fps | あり |
| 立ち姿 | 1 | — | — |
| D-7 ハンド (E トラック側で発火) | 静止 (1 frame) or 微妙な呼吸 (2 frames) | — | E 側演出 |

---

## 5. ディレクトリ配置 (ADR-0004 準拠)

```
Assets/
├── Art/
│   ├── Sprites/
│   │   └── Hero/
│   │       └── v1/
│   │           ├── _draft/                    (F1 の絞り込み済 3 枚)
│   │           ├── hero_idle.png              (F2)
│   │           ├── hero_stand.png             (F2)
│   │           ├── hero_walk_front.png        (F2)
│   │           ├── hero_walk_back.png         (F2)
│   │           ├── hero_walk_left.png         (F2)
│   │           ├── hero_walk_right.png        (F2)
│   │           ├── hero_hands_d7.png          (F2)
│   │           └── hero_idle.controller       (F4)
│   └── anemora_palette_v0.gpl                 (F2)
├── Prefabs/
│   └── Hero.prefab                            (F4)
└── (既存)

art/_intermediate/                              (gitignore)
├── hero_v1_pixellab/                          (F1 中間)
├── hero_v1_aseprite/                          (F2 中間)
└── hero_v1_retrodiffusion/                    (F3 中間、条件付き)
```

---

## 6. 検証マトリクス

| ID | 項目 | 必須 | 確認方法 |
|---|---|---|---|
| V1 | 中性表現が成立 (両読み可能) | ✓ | F1 ユーザー判断 + F2 仕上げ後の再確認 |
| V2 | 異物原則を守る (奇抜要素なし) | ✓ | F1 / F2 / F4 の各段階で観察 |
| V3 | Silent protagonist と矛盾しない (口元 / 表情) | ✓ | F2 仕上げ時 |
| V4 | HD-2D Tier 2 (動的影 + 単一方向光) で破綻しない | ✓ | F4 Unity 取り込み後 |
| V5 | パレット統一 (Anemora v0 内のみ) | ✓ | F2 仕上げ完了時 |
| V6 | Pixel Perfect 整合 (32x48 整数解像度、Filter=Point) | ✓ | F4 |
| V7 | Idle ループが自然 (静謐感、過剰な動きなし) | ✓ | F4 |
| V8 | Walk 4 方向が破綻なく動く | ✓ | F4 |
| V9 | D-7 用ハンドアセットが揃う | ✓ | F2 |
| V10 | 商用利用条項を満たすプランで生成 | ✓ | F0 / F5 |
| V11 | asset_ledger 記載完了 | ✓ | F5 |
| V12 | E5 / G トラックから Prefab 参照可能 | ✓ | F4 |
| V13 | デスクトップ UJPVOG2 (RTX 2070S) で見栄え確認 | △ | F4 後 |
| V14 | ノート PC TOM で動作確認 | △ | F4 後 |

`✓` = F トラック完了に必須、`△` = 観察項目。

---

## 7. リスクと早期検知

| リスク | 兆候 | 対応 |
|---|---|---|
| PixelLab paid 未契約 | F0 で確認時点 | ユーザーに契約意思確認、契約まで F1 保留。F2 / F4 は PixelLab 出力なしでは進めない |
| 中性表現が「無個性」になる | F1 候補がどれも印象薄い | プロンプトに「distinct silhouette」「memorable simple design」を追加、参照画像を Frisk / Madeline (Celeste) 等に絞る |
| 異物性が滲む (奇抜要素が消えない) | F1 でどの候補も髪色 / 装飾が派手 | ネガティブプロンプト強化、それでもダメなら手描き寄りに切替 (Aseprite 直描画で v0 を作り、PixelLab で参考バリエーション程度に格下げ) |
| HD-2D で動的影が乗らない | F4 で影が消える / 線が潰れる | アウトラインを 1 px 太く、シルエット内のディテールを減らす、光源方向に合わせた左右非対称シェーディングに修正 |
| 32x48 がカメラと合わない | E1 / G トラックで主人公が小さすぎ / 大きすぎ | Pixel Per Unit を 16 / 24 / 48 で試す、Camera orthographic size を Stage 3 中に確定 |
| パレット v0 が後で破綻 | G トラックで街の建物と色が浮く | パレット v0 を全体マスターに格上げし、街・NPC・主人公すべてをこの中で構築する運用に切替 |
| ユーザー判断で v1 が確定しない | F1 候補に十分な手応えがない | F1 ループを 1-2 周追加、それでもダメなら方針見直し (手描き比率上げる / 他ツール検討) |

---

## 8. 担当割り

| Phase | Linux Claude | Windows Codex | ユーザー |
|---|---|---|---|
| F0 ライセンス + プロンプト | プロンプトテンプレート起草、ledger §1.2 レビュー | PixelLab UI で契約状態確認 | paid 加入意思決定 |
| F1 PixelLab ドラフト | devlog 整理、プロンプト次版起草 | PixelLab 生成、中間ファイル整理 | 候補絞り込み最終判断 |
| F2 Aseprite 仕上げ | 仕上げ完了条件レビュー | Aseprite 操作 (色 / シルエット / 動き) | パレット v0 最終判断 |
| F3 Retro Diffusion (条件付き) | 投入要否判定支援 | Retro Diffusion 操作 | 投入要否最終判断 |
| F4 Unity 取り込み | Animator 構造レビュー、E 接続確認 | Pixel Perfect 設定、Animator、Prefab | (確認のみ) |
| F5 ledger + 完了 | 台帳整理、devlog レビュー | 生成日 / プラン事実確認 | (確認のみ) |

Linux Claude は **絵を描かない**。プロンプト / 仕上げ条件 / 台帳整合 / 計画書改訂に徹する。

---

## 9. 依存関係 / 並列性

```
F0 ─ F1 ─ F2 ─┬─ F3 (条件付き) ─┐
              │                  │
              └──────────────────┴─ F4 ─ F5
```

- F0 → F1 → F2 → F4 → F5 が必須経路
- F3 はオプション、F2 と F4 の間に挿入可能
- F2 完了で「v1 静止画」レビューを 1 度実施、F4 完了で「動き」レビューを再度実施
- E トラックと並列可: E トラックは F1-F2 の進行と独立 (E5 で v1 Prefab を参照する以外)
- G トラックは F4 完了が前提 (主人公スプライトがないと第 1 ゾーンに置けない)

---

## 10. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0.1 | 2026-05-04 | Stage 3 Day 1 起草、Phase F0-F5 + 出力規格 + 検証マトリクス + 担当割り定義 |
