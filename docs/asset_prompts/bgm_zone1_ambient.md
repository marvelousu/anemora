# Zone 1 Ambient BGM Generation Prompt Template (AIVA + Suno + Stable Audio + Reaper)

> G5 で使用する街アンビエント BGM 1 曲のプロンプトテンプレート。
> ADR-0003 §Decision に従い、**AIVA Pro = 骨格 / Suno v5.5 = ムード探索 / Stable Audio 2.5 = inpainting** の役割分担、**Reaper (DAW)** で最終仕上げ。

> **Status (2026-05-04)**: v0 起草。3 ツール試行前の抽象プロンプト。Reaper でステム編集を前提に複数ツールの出力を組み合わせる方針。

---

## 1. 楽曲仕様

### 1.1 役割

- 第 1 ゾーン (家 + 街中央広場 + 図書館跡) の常時 BGM
- ループ可能、約 3-4 分尺 (内部はループ、シームレス)
- 静謐 / 衰退 / メランコリック、ただし暗黒一辺倒ではない
- プレイヤーが時の窓を使用すると **変調** (フィルタ / 音量 / 一部楽器抜き) で代用 (ADR-0003、VS では時の窓固有曲は作らない)

### 1.2 音楽スタイル

- **ジャンル**: アンビエント / ミニマル・クラシカル / フィルム・スコア
- **キー**: マイナーモード (推奨: A minor, D minor) だが半音階・不協和は使わない
- **テンポ**: 60-72 BPM、ほぼ感じさせない流れ
- **拍子**: 4/4、または free-time
- **楽器**: ピアノ + 弦楽 (cello + violin を中心) + 環境音 (微かな風 / 遠い水音) のみ
- **打楽器**: 不採用 (ドラム / パーカッション / リズム楽器は使わない)
- **ボイス**: 不採用 (Silent protagonist 整合)

### 1.3 参照作品 (絵柄ではなく音響)

- ICO / Shadow of the Colossus アンビエント (大島ミチル系)
- Yume Nikki / OFF の sparse ambient
- 久石譲のミニマル・ピアノ曲 (もののけ姫 静かな場面など)
- Sakuzyo / TIDLOOP のアンビエント
- William Basinski - Disintegration Loops の静的反復

ただし**直接的な模倣は避ける** (法的整合 / Anemora の独自性確保)。雰囲気の方向性を共有する目安として使う。

---

## 2. AIVA Pro Prompt (骨格生成)

AIVA は emotional template と instruments を選択するスタイル。テキストプロンプトは補助。

### 2.1 設定

- **Style**: "Cinematic" or "Modern Classical" or "Ambient" (試行で確定)
- **Emotion / Mood**: "Melancholic", "Mysterious", "Reflective"
- **Instruments**:
  - Required: Piano (solo, sparse)
  - Optional: Strings (Cello primary, Violin secondary, Viola supporting)
  - Optional: Ambient Pad (subtle, low presence)
- **Duration**: 3:30-4:00
- **Key**: A minor or D minor
- **Tempo**: 60-72 BPM
- **Time Signature**: 4/4 or free time

### 2.2 補助プロンプト (AIVA description フィールド)

```
A slow melancholic ambient piece for a fading abandoned town in a HD-2D adventure
game. Sparse solo piano with occasional cello phrases, no percussion at all. Quiet
contemplative mood, like walking alone through a quiet street at dusk. Minor key but
not despairing. Should loop seamlessly. Avoid bright major-key resolutions, dramatic
swells, drums, electronic elements, and modern pop chord progressions. Inspired by
the quiet ambient music of Studio Ghibli's introspective scenes and the early-game
exploration music of Octopath Traveler.
```

### 2.3 出力期待値

- 楽譜 + MIDI + ステム (instrument 別) を取得
- ステム単位で Reaper に取り込み、Suno / Stable Audio の出力と差し替え可能にする

---

## 3. Suno v5.5 Prompt (ムード探索)

Suno はテキストプロンプト + style tags のスタイル。AIVA の骨格を補完するムード素材として使う。

### 3.1 Lyrics

**lyrics は空** (instrumental, Silent protagonist 整合):

```
[Instrumental]
```

### 3.2 Style プロンプト

```
slow melancholic ambient instrumental, sparse solo piano, cello, no drums, no
percussion, no beat, no vocals, quiet contemplative mood, fading town atmosphere,
HD-2D RPG exploration music, minor key, 60 BPM, looping ambient, Studio Ghibli
inspired quiet scenes, no electronic elements, no modern pop, no auto-tune, no rap,
no aggressive instruments
```

### 3.3 出力期待値

- 4-8 候補生成
- ベスト 1-2 を選び、AIVA の骨格に対するムード補強として Reaper で重ねる
- Suno 出力は **paid plan で生成したものに限る** (`asset_ledger.md` §1.2)

---

## 4. Stable Audio 2.5 Prompt (inpainting / 部分差替)

AIVA + Suno の合成で「ここの 8 小節だけ違う質感が欲しい」場合に使う。VS では使わない可能性が高いが、後段で必要になったら:

```
Sparse solo piano improvisation in A minor, slow tempo 60 BPM, very quiet, gentle
sustain pedal, occasional single note phrases with long silences, no other
instruments, melancholic introspective mood, 8 bars duration.
```

Stable Audio の **prompt + duration + key signature** で生成、Reaper で該当箇所に差し替え。

---

## 5. Reaper 仕上げワークフロー

### 5.1 ステップ

1. **AIVA ステム取込み**: Piano / Cello / Violin / Ambient Pad を別トラックに配置
2. **Suno 候補取込み**: ベスト 1-2 を別トラックに配置 (mute 状態で開始)
3. **マッシュアップ判定**: AIVA だけで完成度が出るか、Suno を混ぜると向上するかを試聴
4. **編集**:
   - 不要な dramatic swell をカット
   - ループ点 (例: 0:00 と 3:30 が滑らかに繋がる) を確認・調整
   - パン: Piano center, Cello slightly left, Violin slightly right
   - Ambient pad は -12 dB 以下で背景に埋める
5. **マスタリング**:
   - Loudness: -18 to -16 LUFS (ゲームの BGM として控えめ)
   - High-pass filter @ 40 Hz (低音域を整理)
   - Subtle reverb (room or hall, wet 15-20%)
   - 最終 limiter は -1 dB ceiling
6. **Export**:
   - Format: OGG Vorbis (Unity 推奨)、quality 6 (192 kbps 相当)
   - File: `Assets/Audio/Music/Zone1_Ambient.ogg`
   - 中間 .RPP ファイルと stem WAV は `audio/_intermediate/zone1_ambient/` (gitignore)

### 5.2 時の窓使用時の変調 (VS では Reaper 出力に対するランタイム処理で代用)

ADR-0003 §音響方向性 + VS_SCOPE §5.1: VS では時の窓固有曲を作らず、街アンビエントの **変調** で代用:

- Unity AudioMixer で Low-pass filter (cutoff 800 Hz) を時の窓使用時に適用
- 一部楽器を mute (Cello / Violin を mute、Piano + Ambient のみ残す)
- Pitch shift -2 semitones (subtle、半音 2 つ下げて時間が遅くなった感)
- これらは G5 で実装、Reaper 側では特別な作業は不要

---

## 6. 検証ポイント

VS_SCOPE §5.1 / §5.2 と整合:

- [ ] 3-4 分尺、シームレスループ可能
- [ ] 打楽器なし
- [ ] Voice なし
- [ ] 静謐 / 衰退の質感が立ち上がる
- [ ] ゲーム内で 10-15 分連続再生しても疲れない
- [ ] 時の窓使用時の変調 (Low-pass + 楽器抜き) でも音楽として成立する
- [ ] 商用ライセンス (AIVA Pro + Suno paid plan + Stable Audio 確認済プラン) を満たす

---

## 7. asset_ledger 記載例

`docs/legal/asset_ledger.md` §2.3 BGM に追記:

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| bgm_zone1_v1 | Zone1_Ambient.ogg | 2026-05-XX | AIVA Pro + Suno v5.5 + Reaper | AIVA Pro / Suno paid / Reaper owned | AIVA description / Suno style prompt | Reaper 編集・マスタリング | 可 (AIVA Pro + Suno paid) | GitHub Public 可 | Tier 1 player-consumed | 街アンビエント、3-4 分尺ループ |

---

## 8. ユーザー判断ポイント

- **Suno を実際に重ねるか**: AIVA だけで完成度が出れば Suno は使わない。試聴してユーザー判断
- **Cello を主役にするか Piano を主役にするか**: Piano 主役推奨だが、雰囲気次第で Cello に寄せても可
- **環境音 (微かな風 / 遠い水音)** を BGM に含めるか、SFX レイヤーで別途流すか: 推奨は SFX 別レイヤー (`sfx_zone1.md` 参照) で BGM はシンプルに

---

## 9. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草。AIVA + Suno + Stable Audio + Reaper の役割分担と各プロンプト |
