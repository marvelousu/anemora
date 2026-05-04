# NPC Residents v1 Generation Prompt Template

> G3 (NPC 配置) で使用する Resident_A / Resident_B のスプライト生成プロンプトテンプレート。
> `STAGE3_G_PLAN.md` §3 Phase G3 / `docs/draft/g3_npc_dialogue.md` で参照する性格軸を視覚化する。

> **Status (2026-05-04)**: v0 起草。F1 (主人公) 試行結果を見てから本書も v1 改訂。

---

## 1. 共通方針

### 1.1 異物原則 (主人公以外は普通の住人)

- 全 NPC を「ループ世界に普通に住んでいる人」として描く
- 特殊な役割を視覚化しない: 語り部 / 守り人 / 前任者の暗喩を入れない
- 主人公との視覚対比: 主人公は中性的・若い (10-20 前半)、NPC は性別 / 年齢を明示してコントラストを作る

### 1.2 出力規格 (`STAGE3_F_PLAN.md` §4 と統一)

- 解像度: **32x48 px** (主人公と同じ、画面整合)
- Pixel Per Unit: 32
- Idle 4 frames + Walk 4 方向 4 frames each (Resident_A)
- Idle のみ 4 frames (Resident_B、座位)
- パレット: Anemora パレット v0 (主人公と共通、Aseprite で統合)

### 1.3 美術参照 (主人公と同じ系統)

- HD-2D Tier 2 (Octopath Traveler / Triangle Strategy / Sea of Stars 序盤)
- 動的影 + 単一方向光に乗る単純シルエット
- 衰退した街の住人として整合 (派手 / 富裕 / 異界感を出さない)

---

## 2. Resident_A: 中年〜初老の通行人

### 2.1 設定

- 年齢: 40 代後半〜60 代前半 (主人公とのコントラスト)
- 性別: 中性表現でも明確でも可、ユーザー判断 (本書 v1 で確定)
- 体格: 普通、やや小柄
- 服装: 街の普通の服 (色褪せた上着 + 落ち着いた色のズボン or スカート)
- 髪: グレー混じり、短〜中
- 表情: 穏やか、わずかに疲れた感じ

### 2.2 Master Prompt

```
A small pixel art sprite of an ordinary middle-aged village resident in their late
forties to early sixties, calm worn expression with a softly closed mouth, plain
faded everyday clothing in muted earth tones (worn grey-blue jacket over a beige
shirt, simple cloth trousers, soft canvas shoes), short to medium grey-streaked dark
hair, average build of slightly shorter than average height, gentle relaxed posture,
rendered in HD-2D inspired pixel art style at 32x48 resolution, limited palette of
16-24 colors shared with the protagonist sprite, soft directional lighting from
upper-left, clean readable silhouette, inspired by Octopath Traveler and Sea of Stars
early-game NPC sprites, quiet weathered mood befitting a fading town inhabitant.
```

### 2.3 ポーズ別 (`{{POSE}}` を差し替え)

- **front**: `facing the viewer, front view, standing still with arms relaxed at sides`
- **back**: `facing away from the viewer, back view, standing still`
- **left**: `side profile facing left, standing still, weight on the back foot`
- **right**: `side profile facing right, standing still, weight on the back foot` (left の mirror で代替可能)

### 2.4 Animation

- **Idle (4 frames)**: `{{Master with POSE=front}}, idle animation, 4 frames, very subtle weight shift, slow breathing, occasional glance to the side`
- **Walk (4 frames each, 4 directions)**: `{{Master with POSE=<dir>}}, walking animation, 4 frames, slow steady pace, slightly tired step cycle, hands at sides`

---

## 3. Resident_B: 静かに座る若者〜中年

### 3.1 設定

- 年齢: 20 代前半〜30 代前半 (主人公と近いが少し上、または明確に上)
- 性別: 中性表現でも明確でも可、ユーザー判断 (Resident_A の逆寄りで対比)
- 体格: 細身〜普通
- 服装: 落ち着いた色、フード or マフラー or ストール (顔まわりに何かをかける = 内向性表現)
- 髪: 暗めの色、目元にかかる長さ可
- 表情: 視線を伏せ気味、口元静か

### 3.2 Master Prompt

```
A small pixel art sprite of a quiet young to early middle-aged village resident in
their twenties to early thirties, downcast peaceful expression with a closed mouth
and slightly lowered gaze, simple muted clothing in cool earth tones (soft dark
green or charcoal cloak / hooded jacket over a plain shirt, dark cloth trousers,
worn leather shoes), shoulder-length dark hair partially covering the eyes, slim
build of average height, seated posture (sitting on a low stone bench, hands resting
loosely in the lap, knees together), rendered in HD-2D inspired pixel art style at
32x48 resolution, limited palette of 16-24 colors shared with the protagonist
sprite, soft directional lighting from upper-left, clean readable silhouette,
inspired by Octopath Traveler and Sea of Stars NPC sprites, contemplative reserved
mood, no smile, no obvious emotional display.
```

### 3.3 ポーズ

Resident_B は基本的に **座位のみ** (ベンチに座る Idle)。立ち姿 / 歩行は VS では作らない (Stage 4 候補)。

### 3.4 Animation

- **Idle 座位 (4 frames)**: `{{Master Prompt}}, idle animation, 4 frames, almost no motion, occasional slight nod or shift of weight, eyes occasionally closing briefly, very still`

座位 Idle はほぼ静止画に近く、4 frames 中 2 frames は同じでも良い (微動 + 小さな呼吸のみ)。

---

## 4. Negative Prompt (両 NPC 共通)

```
NOT: glowing eyes, exotic eye color, special markings on face or body, fantasy armor,
weapons, magical aura, particle effects, ornate accessories, ancient runes, religious
vestments, royalty or noble outfit, vibrant unnatural hair color, dramatic facial
expression, big smile, big frown, anime-style large eyes, chibi proportions, super-
deformed style, exaggerated muscles, revealing clothing, futuristic technology, glowing
items, oversized props, photo-realistic style, cyberpunk style, isekai protagonist
look, hero-of-prophecy iconography, scars or wounds (Resident_A 健康そう), tears,
crying.
```

### 4.1 補足解説

- `glowing items / runes / aura`: 異物原則
- `dramatic expression / big smile / frown`: NPC の感情をフラットに保つ (主人公が反応する余地)
- `religious vestments / royalty / hero iconography`: 普通の住人として描く
- `tears / crying`: Resident_B の伏せ目は「内向き」であって「悲しみ」ではない

---

## 5. Test Sequence (F1 と同じ流れ)

PixelLab paid 加入後:

1. **Test A (Resident_A front)**: §2.2 Master + POSE=front で 4 枚
2. **Test B (Resident_A side / back)**: ベスト 1 枚から派生
3. **Test C (Resident_A Walk Idle)**: アニメ 2 種
4. **Test D (Resident_B 座位 Idle)**: §3.2 Master を 4 枚生成、座位の自然さ確認
5. **Test E (両者並べ)**: A と B を同画面に並べて視覚対比 (年齢 / 服装 / シルエットが区別できるか)、主人公とも並べて 3 体の整合確認

各 Test の所感を `docs/devlog/2026-05-XX_g3_npc_prompt_check.md` に記録。

---

## 6. ユーザー判断ポイント

v1 確定の前に、以下をユーザーに問う:

- **Resident_A の性別**: 中性 / 男性 / 女性 のいずれを Anemora の街の雰囲気に合わせるか
- **Resident_B のフード / マフラー**: 「内向性表現」として有効か、または別のアプローチ (例: 視線を落としているだけで顔を隠さない) が良いか
- **A と B の組合せ**: 年齢差 / 性別差で対比が立っているか、それとも揃えた方が自然か

---

## 7. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草。Resident_A / Resident_B の Master + ポーズ + Animation + Negative + Test sequence |
