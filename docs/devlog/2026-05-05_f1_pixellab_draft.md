# 2026-05-05 F1 PixelLab Hero v1 Draft

## 概要

- 目的: `docs/asset_prompts/hero_v1.md` v0.1 を使い、主人公 v1 draft の front / side / back 候補を作る。
- 実行環境: Windows Codex + PixelLab API v2。
- Plan: PixelLab Tier 1: Pixel Apprentice paid confirmed (Paddle receipt, order suffix `...638`)。
- API: `create-image-bitforge`。
- 出力サイズ: 32x48 px, transparent PNG。
- 仕上げ: なし。F2 Aseprite で 32x48 の統一、色、輪郭、方向差分を手修正する前提。

## 生成枚数

| Test | 内容 | 枚数 | 保存先 |
|---|---:|---:|---|
| Test A | front direct | 4 | `art/_intermediate/hero_v1_pixellab/test_a/` |
| Test B | Test A best (`a2`) から variations | 4 | `art/_intermediate/hero_v1_pixellab/test_b/` |
| Test C | Test B best (`b3`) から side/back init generation | 2 | `art/_intermediate/hero_v1_pixellab/test_c/` |
| Test C extra | direction-guided side/back direct | 8 | `art/_intermediate/hero_v1_pixellab/test_c/` |
| Test D | Idle 4 frames | 0 | optional のため未実施 |

合計: 18 枚。途中で Test B の init-image request が 1 回 timeout し、保存ファイルなしで subscription generation を 1 消費した可能性がある。

## Draft 配置

| 方向 | 採用元 | draft |
|---|---|---|
| front | `art/_intermediate/hero_v1_pixellab/test_b/hero_front_b3_seed551103.png` | `Assets/Art/Sprites/Hero/v1/_draft/front_v1.png` |
| side | `art/_intermediate/hero_v1_pixellab/test_c/hero_side_c4_seed551213.png` | `Assets/Art/Sprites/Hero/v1/_draft/side_v1.png` |
| back | `art/_intermediate/hero_v1_pixellab/test_c/hero_back_c3_seed551222.png` | `Assets/Art/Sprites/Hero/v1/_draft/back_v1.png` |

## 所感

- Test A は front prompt でも横向きに寄る出力が混ざった。`a2` が最も正面として成立。
- Test B は variations として安定し、`b3` が中性的・若年・普通の住人感のバランスが比較的よい。
- init image 付きの side/back は前面候補に引っ張られて方向が崩れたため、direction-guided direct generation を追加した。
- side/back は方向としては成立したが、front との衣装・髪色・体格の同一性は F2 Aseprite での調整が必要。
- 異物原則違反は大きくは見えない。魔法/武器/発光/紋様/異界装飾はなし。
- 中性表現は front ではおおむね成立。ただし髪色が少し明るく、顔立ちの読みはユーザー確認が必要。

## ユーザーレビュー待ち

- 中性表現が成立しているか。
- side/back が同一人物として許容できるか。
- 髪色・服色が Anemora の「普通の住人」から外れていないか。
- F2 で Aseprite 仕上げに進めるか。
