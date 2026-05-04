# Hero v1 Generation Prompt Template

> 主人公スプライト v1 を PixelLab + Retro Diffusion で生成するためのプロンプトテンプレート。
> `STAGE3_F_PLAN.md` §3 Phase F0 / F1 で参照。Windows Codex が PixelLab で試行 → 結果を `docs/devlog/2026-05-XX_f0_prompt_check.md` に記録 → Linux Claude が改訂で v1 確定。

> **Status (2026-05-04)**: v0 起草。PixelLab paid 加入 + 試行結果待ちで v1 へ改訂予定。

---

## 1. 確定仕様 (`STAGE3_TBD_RESOLUTION.md` §1, `STAGE3_F_PLAN.md` §2 から抽出)

| 軸 | 確定値 | プロンプトでの扱い |
|---|---|---|
| 性別 | 中性的・両読み可能 | `androgynous`, `gender-neutral` を明示 |
| 年齢 | 10 代後半〜20 代前半 | `late teens to early twenties` (狭めず、絵柄で揺らぎを許容) |
| 沈黙 | Silent protagonist | `neutral expression`, `closed mouth`, 強い表情語は使わない |
| 偽記憶 | ぼんやり覚えている | 表情に悲壮 / 緊張を込めない、静かな存在感 |
| 異物性 | 最終盤まで伏せる | **Negative prompt** で奇抜要素を排除 |
| 動機 | 家族に会いたい | プロンプトには反映せず、表情の柔らかさで間接表現 |
| 時の筆 | ポケットに忍ばせる | v1 では描画対象外、別アセット |
| 解像度 | 32x48 px | `32x48 pixel art`, `low resolution sprite` |
| 美術参照 | HD-2D Tier 2 簡素版 | `Octopath Traveler / Sea of Stars early game inspired pixel art` |

---

## 2. Master Prompt (全方向で共通の base)

```
A small pixel art sprite of an androgynous protagonist character in their late teens
to early twenties, neutral calm expression with a closed mouth, plain everyday clothing
in muted earth tones (worn brown jacket, soft beige tunic, simple cloth trousers),
short to medium dark brown hair with a slight asymmetric cut, slim build of average
height, gentle posture, rendered in HD-2D inspired pixel art style at 32x48 resolution,
limited palette of 16-24 colors, soft directional lighting from upper-left, clean
silhouette readable at small sizes, inspired by Octopath Traveler and Sea of Stars
early-game character sprites, quiet melancholic mood without being grim.
```

### 2.1 Master Prompt の使い分け

`{{POSE}}` のところに以下のいずれかを差し込んで方向別に生成:

- **front**: `facing the viewer, front view, standing still, arms relaxed at sides`
- **back**: `facing away from the viewer, back view, standing still, arms relaxed at sides`
- **left**: `side profile facing left, standing still, arm closer to viewer slightly raised`
- **right**: `side profile facing right, standing still, arm closer to viewer slightly raised` (left の mirror で代替可能、必要なら個別生成)

### 2.2 完成形プロンプト例 (front)

```
A small pixel art sprite of an androgynous protagonist character in their late teens
to early twenties, neutral calm expression with a closed mouth, plain everyday clothing
in muted earth tones (worn brown jacket, soft beige tunic, simple cloth trousers),
short to medium dark brown hair with a slight asymmetric cut, slim build of average
height, gentle posture, facing the viewer, front view, standing still, arms relaxed
at sides, rendered in HD-2D inspired pixel art style at 32x48 resolution, limited
palette of 16-24 colors, soft directional lighting from upper-left, clean silhouette
readable at small sizes, inspired by Octopath Traveler and Sea of Stars early-game
character sprites, quiet melancholic mood without being grim.
```

---

## 3. Negative Prompt (異物原則 + Silent protagonist 守備)

```
NOT: glowing eyes, exotic eye color, special markings on face or body, fantasy armor,
weapons drawn, magical aura, particle effects, ornate accessories, ancient runes,
cybernetic parts, mechanical implants, vibrant unnatural hair color (white, red,
purple, neon), open mouth shouting, dramatic facial expression, anime-style large
eyes, chibi proportions, super-deformed style, exaggerated muscles, revealing
clothing, royalty or noble outfit, religious vestments, futuristic technology, glowing
weapons, floating items, oversized props, clearly male or clearly female-only features,
gore, photorealistic style.
```

### 3.1 解説 (なぜ NOT に含めたか)

- `glowing eyes / aura / runes / cybernetic parts`: 異物原則 (主人公の異物性は最終盤まで伏せる)
- `exotic hair color`: 街の住人として違和感なく溶け込むため
- `open mouth shouting / dramatic expression`: Silent protagonist と矛盾
- `anime-style large eyes / chibi`: HD-2D Tier 2 ピクセル絵に合わない
- `clearly male or clearly female-only features`: 中性的・両読み可能を維持
- `royalty / religious / futuristic`: 衰退した街の住人として整合させる

---

## 4. Animation 生成プロンプト

PixelLab で sprite sheet モードを使う場合、以下の指定を追加:

### 4.1 Idle ループ (4 frames)

```
{{Master Prompt with POSE=front}}, idle animation, 4 frames, subtle breathing motion,
shoulders rising and falling slightly, no large movements, loop-friendly, gentle and
quiet.
```

### 4.2 Walk × 4 方向 (各 4 frames)

```
{{Master Prompt with POSE=<direction>}}, walking animation, 4 frames, natural step
cycle, alternating legs, head bobbing slightly, arms swinging gently, quiet pace.
```

`<direction>` は `front` / `back` / `left` (right は mirror で代替推奨)。

### 4.3 D-7 用 Hand close-up

```
A close-up pixel art sprite of two human hands held up close to the viewer's face,
seen from a first-person perspective looking down at one's own hands, palms slightly
visible, fingers gently curled, skin tone neutral light, no markings or scars, no
visible weapons or items, soft directional lighting, 64x32 resolution (or wider
landscape format), HD-2D inspired pixel art style, quiet introspective mood.
```

---

## 5. Test Sequence (F0 → F1 への試行手順)

PixelLab paid 加入後、以下の順で試行:

1. **Test A**: §2.2 完成形プロンプト (front) で 4 枚生成、`art/_intermediate/hero_v1_pixellab/test_a/` に保存
   - 期待: 4 枚すべてが §3 Negative prompt の禁止事項を侵していない
   - 観察: 中性表現が成立しているか、HD-2D Tier 2 の質感が出ているか
2. **Test B**: Test A の出力からベスト 1 枚を選び、Variations モードで 4 枚生成、`art/_intermediate/hero_v1_pixellab/test_b/` に保存
   - 期待: シルエットの一貫性 (服装 / 髪型 / 体格) が保たれる
3. **Test C**: ベスト 1 枚を base に side / back を生成、`art/_intermediate/hero_v1_pixellab/test_c/` に保存
   - 期待: 同一キャラと識別できる
4. **Test D**: §4.1 Idle 4 frames、`art/_intermediate/hero_v1_pixellab/test_d/` に保存
   - 期待: ループ可能なサイクル、過剰な動きなし
5. **Test E**: §4.2 Walk front 4 frames、`art/_intermediate/hero_v1_pixellab/test_e/` に保存
   - 期待: 自然な歩行サイクル

各 Test の所感を `docs/devlog/2026-05-XX_f0_prompt_check.md` に記録。Linux Claude がそれを見て本書を v1 へ改訂。

---

## 6. PixelLab 固有の留意点 (使用時に補足)

PixelLab の挙動について、Windows Codex が試行時に以下を確認・記録:

- **解像度指定**: `32x48` を直接指定できるか、または近い値 (32x32 / 32x64) しかないか
- **palette 指定**: 色数を制約できるか (PixelLab の限定パレット機能)
- **Animation export**: sprite sheet として出力できるか、frame 単位か
- **Variations 強度**: ベース画像との類似度パラメータ
- **Negative prompt の効き方**: 一般的な diffusion model と同等か、PixelLab 独自の挙動があるか

これらは v0 プロンプトでは抽象的に書いているが、PixelLab UI に応じて v1 で具体化する。

---

## 7. Retro Diffusion 補助プロンプト (F3 条件付き)

F2 で質感が物足りない場合のみ。Retro Diffusion は PixelLab とは異なり img2img / texture refinement に強い:

```
Refined pixel art texture pass on existing 32x48 character sprite, preserve original
silhouette and color palette, enhance shading depth slightly, add subtle texture grain
on cloth without breaking palette limit, maintain HD-2D Tier 2 (single directional
light, simple drop shadow), keep neutral calm expression, do not introduce new colors
beyond palette, do not add accessories, glow, or particle effects.
```

Negative prompt は §3 と同じ。

---

## 8. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草。PixelLab 試行前の抽象プロンプト。Master + 方向別 + Animation + Negative + Test sequence + Retro Diffusion 補助 |
