# 2026-05-05 G3 NPC PixelLab Draft

## 概要

- 目的: `docs/asset_prompts/npc_residents.md` v0 を使い、Resident_A / Resident_B の v1 draft 候補を作る。
- 実行環境: Windows Codex + PixelLab API v2。
- Plan: PixelLab Tier 1: Pixel Apprentice paid confirmed (Paddle receipt, order suffix `...638`)。
- 主 API: `create-image-bitforge`。
- Resident_B 追加 API: `create-image-pixflux`。
- 出力サイズ: 32x48 px, transparent PNG。
- 仕上げ: なし。G3/F2 後続で Aseprite 仕上げ前提。

## 生成枚数

| 対象 | 内容 | 枚数 | 保存先 |
|---|---:|---:|---|
| Resident_A | front/back/left 各 4 | 12 | `art/_intermediate/npc_resident_a_pixellab/` |
| Resident_A extra | direction-guided front/back/left 各 2 | 6 | `art/_intermediate/npc_resident_a_pixellab/` |
| Resident_B | seated prompt, Bitforge | 8 | `art/_intermediate/npc_resident_b_pixellab/` |
| Resident_B extra | strengthened seated prompt, PixFlux | 4 | `art/_intermediate/npc_resident_b_pixellab/` |

合計: 30 枚。F1 の timeout 分を含め、セッション終了時の PixelLab balance は 1951 / 2000 generations。

## Draft 配置

| 対象 | 採用元 | draft |
|---|---|---|
| Resident_A front | `art/_intermediate/npc_resident_a_pixellab/resident_a_front_5_seed553011.png` | `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/front_v1.png` |
| Resident_A back | `art/_intermediate/npc_resident_a_pixellab/resident_a_back_6_seed553112.png` | `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/back_v1.png` |
| Resident_A left | `art/_intermediate/npc_resident_a_pixellab/resident_a_left_1_seed553201.png` | `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/left_v1.png` |
| Resident_B seated | `art/_intermediate/npc_resident_b_pixellab/resident_b_seated_pixflux_1_seed554101.png` | `Assets/Art/Sprites/NPC/Resident_B/v1/_draft/seated_v1.png` |

## 所感

- Resident_A は中年〜初老感と普通の住人感が出た。front/back/left の服装一致は弱いので、Aseprite で統一が必要。
- Resident_A は魔法/武器/発光/宗教/王侯/英雄記号がなく、異物原則には大きく反していない。
- Resident_A の年齢対比は主人公より明確に上に見える。ただし性別はやや曖昧で、ユーザー判断待ち。
- Resident_B は Bitforge 8 枚がほぼ立ち姿に寄り、座位が成立しなかった。
- PixFlux 追加 4 枚で座位が成立し、`pixflux_1` を draft 採用した。
- Resident_B は画風が少し暗く細かい。座位は成立しているが、Anemora 共通パレットへの統合は Aseprite で必要。
- A/B の年齢対比は立つが、B が想定より若く暗めに見えるため、内向性表現として有効かはユーザー確認が必要。

## ユーザーレビュー待ち

- Resident_A の性別/年齢読みが Anemora の街の住人として自然か。
- Resident_B の座位・内向性表現が成立しているか。
- A/B の年齢差と雰囲気差が対比として機能しているか。
- 異物原則違反がないか。
- F2/Aseprite 仕上げに進めるか、追加生成が必要か。
