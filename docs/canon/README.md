# `docs/canon/` — Anemora 物語 canon (living state)

> Anemora の **確定 canon (現在の正)** をここに集約する。**唯一の mutable な物語 doc 置き場**。
> 他の物語 doc (`docs/devlog/`, `docs/STORY_BIBLE_v1.md` 等) は **immutable な歴史記録**。

## 運用ルール (必読)

### 1. canon は `docs/canon/` のみ更新

- 現在の確定 canon を反映する場所は本 dir **だけ**
- 物語に関する判断・台詞・設定の更新は本 dir のファイルに直接反映する
- 他セッション (Codex / map / レビュー) への引き渡しは本 dir のファイルを参照させる

### 2. devlog は immutable (時点記録)

- `docs/devlog/` は **append-only**、過去 entry の編集禁止
- 訂正・更新が必要な場合: **新 devlog ファイルを別日付で作成**、旧 entry は触らない
- 旧 entry の「改訂履歴」表は legacy、新規 devlog では作らない (新 devlog 自体が改訂を示す)
- devlog の意味 = 「その日に何を考えて何を決めたかの歴史」。歴史を書き換えない

### 3. canon と devlog の関係

- devlog で決まったことを canon に反映 → canon を更新、devlog は触らない
- canon と古い devlog が食い違う場合 = **canon が正**、devlog は当時の記録として保持

### 4. STORY_BIBLE_v1.md との関係 (legacy)

- `docs/STORY_BIBLE_v1.md` は全章物語骨格、過去に in-place 編集されて v1.7 化されている (legacy)
- 本 dir に migrate 予定だが、現状は legacy doc として参照
- 矛盾時は本 dir 優先

## 構成

| ファイル | 内容 |
|---|---|
| `chapter1.md` | Chapter 1「忘れられた街」全 canon (序章 + S1-S6) |
| (今後) `chapter2.md` 等 | 各章追加時 |
| (今後) `story_bible.md` | STORY_BIBLE_v1.md の migrate 先 |

## 更新時の注意

- canon を更新する時は **本ファイルの末尾改訂履歴を更新** (canon ファイル内に改訂履歴を残す)
- 改訂の理由・経緯は devlog (新規) に記録、canon ファイルは現状のみ反映
