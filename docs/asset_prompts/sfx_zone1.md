# Zone 1 SFX Generation Prompt Detail (v1.0 draft)

> G5 / A4 で使用する Zone 1 用 SFX 30 種の生成・仕上げ指定。
> ADR-0003 §Decision に従い、**ElevenLabs SFX v2** を first try、**Stable Audio** を ambience / inpainting / 補完、**Studio One** を trim / normalize / loop / fade / pitch variation の仕上げに使う。

> **Status (2026-05-05)**: v1.0 draft。SFX 30 種の ID / duration / prompt / output / ledger row を固定。実生成後に generation ID、selected take、最終 LUFS を `docs/legal/asset_ledger.md` §2.4 に転記する。

---

## 1. 共通方針

### 1.1 参照と範囲

- **ADR-0003**: AI アセットパイプライン。環境音 / SFX は ElevenLabs SFX v2、仕上げは Studio One。Stable Audio は補完 / inpainting の位置づけ。
- **VS_SCOPE §5.2**: 環境音、足音、時の窓 SFX、NPC 反応 SFX、UI SFX。ボイスは採用しない。
- **`docs/asset_prompts/bgm_zone1_ambient.md` §8**: BGM はピアノ + 弦を中心にし、環境音は SFX 別レイヤー推奨。SFX 側でも旋律・強い和音・ドラムを避ける。
- **Steam 開示区分**: 生成 SFX はプレイヤーが消費する AI 生成物なので、draft 時点では **Tier 1 player-consumed** として台帳に記録する。

### 1.2 30 種の内訳

| カテゴリ | 数 | 内容 |
|---|---:|---|
| 環境 | 6 | 鳥 / 風 / 落ち葉 / 水 / 木の軋み / 静寂感 ambience pad |
| 足音 | 12 | 木床 / 石畳 / 草地 / 砂 × walk / run / land |
| 時の窓 | 6 | wheel_open / wheel_close / symbol_hover / symbol_select_red / portal_open / portal_flip |
| NPC | 3 | greeting_short / interaction_ack / departure |
| UI | 3 | button_click / menu_open / menu_close |

### 1.3 ツール優先順位

1. **ElevenLabs SFX v2 first try**: 単発 SFX、足音、UI、時の窓、NPC 反応。生成時は 4-8 candidates を作り、短尺に切り出して採用する。
2. **Stable Audio**: `sfx_env_silence_pad_01` のような音楽に近い ambience pad、または ElevenLabs で音像が具体的すぎる場合の補完。生成時点でプラン / API / 商用条件を再確認する。
3. **Studio One**: 一発採用できない場合のみ使用。trim、短い fade、loop crossfade、-3 dB peak normalize、40 Hz HPF、足音 pitch / amplitude variation、OGG q6 export。
4. **Studio One foley / 録音**: 落ち葉など、短く具体的な物理音で生成よりコントロールしやすい場合に優先。録音音源の出所を ledger に残す。

### 1.4 出力規格

- **生成時**: WAV 推奨。ElevenLabs / Stable Audio からの元ファイルは `audio/_intermediate/sfx_zone1/` に保存し、gitignore 対象とする。
- **最終形式**: OGG Vorbis quality 6、mono、44.1 kHz。
- **ファイルサイズ目安**: 0.1-0.8 秒の単発 5-30 KB、1-4 秒の単発 20-120 KB、8-15 秒の ambience loop 120-400 KB。
- **音量目標**: 環境 ambience は -24 to -20 LUFS。足音 / 時の窓 / NPC / UI の foreground は -18 to -12 LUFS。ただし UI は mixer 側で下げやすいよう peak -3 dB 以下。
- **禁止事項**: ボイス、歌、言語化された息づかい、強い melody、ドラム、現代的な車両・電子通知音、過剰なホラー演出。

### 1.5 asset_ledger 行フォーマット

各 entry の `asset_ledger 記載例` は、実生成後に `docs/legal/asset_ledger.md` §2.4 SFX へ 1 行ずつ追記する想定。`2026-05-XX` は生成日、`generation id / selected take` は実生成後に置換する。

| ID | Asset path | Date | Tool | Plan | Input material | Manual edit | Commercial use | Public release | Steam disclosure | Notes |
|---|---|---|---|---|---|---|---|---|---|---|

---

## 2. 環境 SFX (6)

### 2.1 `sfx_env_birds_01`

- **ID**: `sfx_env_birds_01`
- **用途**: 外エリアで 60-120 秒間隔の低確率ランダム再生。街に生命感を足すが、群れや明るい森の印象にはしない。
- **想定 duration**: 0.8-1.2 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim / fade。
- **prompt template**:

```text
A single distant small bird call in a quiet declining town, soft and sparse, far away, no flock, no cheerful forest ambience, no wind, no people, 1 second, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、12-35 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/environment/sfx_env_birds_01.ogg`
- **音量レベル目標**: -24 to -20 LUFS。
- **検証ポイント**: ループ不要。前後 30-80 ms fade。ピッチを 1-2 semitone 下げた variant を試し、明るすぎる場合は不採用。Studio One で不要な room noise を trim。
- **asset_ledger 記載例**:

| sfx_env_birds_01 | `Assets/Audio/SFX/Zone1/environment/sfx_env_birds_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §2.1 prompt | Trim, short fade, optional pitch check | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Bird presence remains user-reviewable; selected take TBD |

### 2.2 `sfx_env_wind_loop_01`

- **ID**: `sfx_env_wind_loop_01`
- **用途**: 外エリア常時 loop。中央広場、家の外、図書館跡で mixer volume を変える。
- **想定 duration**: 10-12 秒 loop。
- **推奨ツール**: ElevenLabs SFX v2 first try、Stable Audio fallback、Studio One loop crossfade。
- **prompt template**:

```text
Soft outdoor wind moving through an empty old town plaza, very gentle air movement, faint dry texture, no storm, no strong gusts, no birds, no people, melancholic and calm, 12 seconds, seamless loop, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、140-320 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/environment/sfx_env_wind_loop_01.ogg`
- **音量レベル目標**: -24 to -20 LUFS。
- **検証ポイント**: 必ず loop seam を確認。開始/終了の loudness と spectral balance を合わせ、100-300 ms crossfade。BGM の strings と帯域がぶつかる場合は 250-500 Hz を軽く整理。
- **asset_ledger 記載例**:

| sfx_env_wind_loop_01 | `Assets/Audio/SFX/Zone1/environment/sfx_env_wind_loop_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §2.2 prompt | Loop crossfade, HPF 40 Hz, normalize | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Environment loop; selected take TBD |

### 2.3 `sfx_env_dry_leaves_01`

- **ID**: `sfx_env_dry_leaves_01`
- **用途**: 外エリアで足元や端の装飾に近づいた時の低確率 one-shot。過剰に秋らしくせず、衰退した街の乾いた小音にする。
- **想定 duration**: 1.5-2.5 秒。
- **推奨ツール**: Studio One foley / 録音 first try、ElevenLabs SFX v2 fallback。
- **prompt template / recording brief**:

```text
Very soft dry leaves shifting lightly across old stone, sparse and close, no footsteps, no wind gust, no forest ambience, quiet declining town mood, 2 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、25-70 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/environment/sfx_env_dry_leaves_01.ogg`
- **音量レベル目標**: -24 to -20 LUFS。
- **検証ポイント**: ループ不要。Attack が強すぎる場合は 10 ms fade-in。録音の場合は room tone と手の摩擦音を除去。生成の場合は足音が混ざった take を捨てる。
- **asset_ledger 記載例**:

| sfx_env_dry_leaves_01 | `Assets/Audio/SFX/Zone1/environment/sfx_env_dry_leaves_01.ogg` | 2026-05-XX | Studio One foley / recording + Studio One | User-owned recording chain + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §2.3 brief | Trim, denoise if needed, short fade | Yes if recording source is user-owned; if generated, record ElevenLabs generation ID | GitHub Public ok after final export | Tier 1 player-consumed if AI generated; otherwise non-AI SFX | Foley-first entry; selected source TBD |

### 2.4 `sfx_env_distant_water_01`

- **ID**: `sfx_env_distant_water_01`
- **用途**: 噴水跡または水の記憶が残る場所の proximity loop。水源が明確に見える音にはしない。
- **想定 duration**: 8-12 秒 loop。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One loop / EQ。
- **prompt template**:

```text
Faint distant trickling water that feels almost remembered, very quiet, no clear stream source, no cave echo, no birds, no people, calm abandoned town atmosphere, 10 seconds, seamless loop, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、120-300 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/environment/sfx_env_distant_water_01.ogg`
- **音量レベル目標**: -24 to -20 LUFS。
- **検証ポイント**: Loop seam 確認。BGM と重ねても明確なメロディ成分にならないこと。2-4 kHz が強い場合は軽く下げる。水音がリアルすぎる場合は volume を下げ、近接時のみ再生。
- **asset_ledger 記載例**:

| sfx_env_distant_water_01 | `Assets/Audio/SFX/Zone1/environment/sfx_env_distant_water_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §2.4 prompt | Loop crossfade, EQ, normalize | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Proximity loop; selected take TBD |

### 2.5 `sfx_env_wood_creak_01`

- **ID**: `sfx_env_wood_creak_01`
- **用途**: 家の中、廃屋、図書館跡で 30-90 秒間隔のランダム one-shot。遠い建材の沈み込み。
- **想定 duration**: 0.8-1.4 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim。
- **prompt template**:

```text
A very distant single wooden creak from an old building settling, quiet and dry, no door slam, no footsteps, no horror jump scare, sparse abandoned town mood, 1.2 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、15-40 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/environment/sfx_env_wood_creak_01.ogg`
- **音量レベル目標**: -24 to -20 LUFS。
- **検証ポイント**: ループ不要。Attack が怖すぎる take は捨てる。0.1 秒程度の tail を残し、急な cutoff を避ける。Pitch -1 semitone まで試行可。
- **asset_ledger 記載例**:

| sfx_env_wood_creak_01 | `Assets/Audio/SFX/Zone1/environment/sfx_env_wood_creak_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §2.5 prompt | Trim, fade-out, optional pitch check | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Random distant event; selected take TBD |

### 2.6 `sfx_env_silence_pad_01`

- **ID**: `sfx_env_silence_pad_01`
- **用途**: 家の中や図書館跡の「静けさ」を補強する低音量 ambience layer。音楽にならない room tone / air pressure として扱う。
- **想定 duration**: 10-15 秒 loop。
- **推奨ツール**: Stable Audio first try、ElevenLabs SFX v2 fallback、Studio One loop / EQ。
- **prompt template**:

```text
Nearly silent ambience for an abandoned room in a declining town, soft air pressure and very faint room tone, no melody, no instruments, no wind gusts, no voices, no drones that feel musical, 12 seconds, seamless loop, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、120-400 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/environment/sfx_env_silence_pad_01.ogg`
- **音量レベル目標**: -24 to -20 LUFS。
- **検証ポイント**: Stable Audio の商用条件を生成時点で再確認。Loop seam と low rumble を確認。BGM の cello / piano sustain と重なって musical drone になれば不採用。必要なら 80 Hz 以下を整理。
- **asset_ledger 記載例**:

| sfx_env_silence_pad_01 | `Assets/Audio/SFX/Zone1/environment/sfx_env_silence_pad_01.ogg` | 2026-05-XX | Stable Audio + Studio One | Stable Audio plan/API state to verify at generation time + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §2.6 prompt | Loop crossfade, EQ, normalize | Yes only after Stable Audio commercial terms are recorded | GitHub Public ok after final export | Tier 1 player-consumed | Stable Audio primary; selected take TBD |

---

## 3. 足音 SFX (12)

### 3.1 `sfx_footstep_wood_walk_01`

- **ID**: `sfx_footstep_wood_walk_01`
- **用途**: 家の中 / 木床での通常歩行。Animation event から左右ランダム pitch で再生。
- **想定 duration**: 0.30-0.45 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim / pitch variation。
- **prompt template**:

```text
A single soft walking footstep on aged wooden plank floor, light leather sole, slight dry wood response, gentle impact, no echo, no boot stomp, 0.4 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、5-18 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_walk_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: ループ不要。Attack 前無音を 5 ms 以下に trim。Unity 側 pitch 0.96-1.04、volume 0.85-1.0 の variation 前提。Creak が強すぎる take は捨てる。
- **asset_ledger 記載例**:

| sfx_footstep_wood_walk_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_walk_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.1 prompt | Trim, normalize, optional pitch variation | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Wood walk single-shot; selected take TBD |

### 3.2 `sfx_footstep_wood_run_01`

- **ID**: `sfx_footstep_wood_run_01`
- **用途**: 木床での速い移動。VS で run 頻度が低い場合も placeholder として保持。
- **想定 duration**: 0.22-0.35 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim / transient control。
- **prompt template**:

```text
A single quick light running footstep on aged wooden plank floor, leather sole, short dry impact, slight plank response, no heavy stomp, no echo, 0.3 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、5-16 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_run_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: Walk より短く、音量は +1 dB 以内。強い床鳴りは loop 時にうるさいので削る。Unity 側 pitch variation 前提。
- **asset_ledger 記載例**:

| sfx_footstep_wood_run_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_run_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.2 prompt | Trim, normalize, transient check | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Wood run single-shot; selected take TBD |

### 3.3 `sfx_footstep_wood_land_01`

- **ID**: `sfx_footstep_wood_land_01`
- **用途**: 木床での小さな着地 / 段差着地。ジャンプ主体ではなく軽い重心移動に使う。
- **想定 duration**: 0.45-0.70 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One low-cut / tail trim。
- **prompt template**:

```text
A soft two-foot landing on old wooden planks, light character weight, muted wood thump with a tiny creak tail, no crash, no debris, no echo, 0.6 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、8-24 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_land_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: 低域が大きい場合は 80 Hz 以下を整理。Tail は 0.2 秒程度残す。衝撃音がアクションゲーム寄りなら不採用。
- **asset_ledger 記載例**:

| sfx_footstep_wood_land_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_land_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.3 prompt | Trim, HPF, normalize | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Wood land single-shot; selected take TBD |

### 3.4 `sfx_footstep_stone_walk_01`

- **ID**: `sfx_footstep_stone_walk_01`
- **用途**: 中央広場 / 家の周辺 / 石畳での通常歩行。
- **想定 duration**: 0.30-0.45 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim。
- **prompt template**:

```text
A single soft walking footstep on weathered stone tile, light leather sole, muted contact, no bright click, no large hall echo, quiet old town, 0.4 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、5-18 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_walk_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: Click が UI 音に近い take は捨てる。前無音を削る。Unity 側 pitch 0.97-1.03 と volume random で反復感を抑える。
- **asset_ledger 記載例**:

| sfx_footstep_stone_walk_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_walk_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.4 prompt | Trim, normalize, optional pitch variation | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Stone walk single-shot; selected take TBD |

### 3.5 `sfx_footstep_stone_run_01`

- **ID**: `sfx_footstep_stone_run_01`
- **用途**: 石畳での速い移動。短く乾いた接地音。
- **想定 duration**: 0.22-0.35 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One transient control。
- **prompt template**:

```text
A single quick running footstep on old stone paving, light leather sole, short muted impact, no metallic click, no large echo, no gravel scatter, 0.3 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、5-16 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_run_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: Walk より短く、明るい高域を抑える。Reverb が入った take は廃棄。Unity で連続再生して耳につかないか確認。
- **asset_ledger 記載例**:

| sfx_footstep_stone_run_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_run_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.5 prompt | Trim, normalize, transient check | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Stone run single-shot; selected take TBD |

### 3.6 `sfx_footstep_stone_land_01`

- **ID**: `sfx_footstep_stone_land_01`
- **用途**: 石畳での小さな着地。過去 / 現在の境界付近でも使える neutral な着地音。
- **想定 duration**: 0.45-0.70 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One HPF / normalize。
- **prompt template**:

```text
A soft two-foot landing on weathered stone tile, light character weight, muted stone thump, no crack, no debris, no heroic impact, no echo, 0.6 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、8-24 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_land_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: 低域と高域 click のバランス確認。Action 感が強い take は不採用。Tail が無音に切れないよう 60 ms fade-out。
- **asset_ledger 記載例**:

| sfx_footstep_stone_land_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_land_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.6 prompt | Trim, HPF, normalize | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Stone land single-shot; selected take TBD |

### 3.7 `sfx_footstep_grass_walk_01`

- **ID**: `sfx_footstep_grass_walk_01`
- **用途**: 図書館跡周辺の草地 / 土混じりの地面での通常歩行。
- **想定 duration**: 0.35-0.50 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim。
- **prompt template**:

```text
A single soft walking footstep on sparse dry grass and dirt, light leather sole, small rustle, no wet mud, no forest ambience, no insects, 0.45 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、6-20 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_walk_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: Grass loop 音が環境 SFX と混ざりすぎないか確認。Rustle が長い場合は tail を短縮。虫や鳥が混ざる take は捨てる。
- **asset_ledger 記載例**:

| sfx_footstep_grass_walk_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_walk_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.7 prompt | Trim, normalize, optional pitch variation | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Grass walk single-shot; selected take TBD |

### 3.8 `sfx_footstep_grass_run_01`

- **ID**: `sfx_footstep_grass_run_01`
- **用途**: 草地 / 土での速い移動。VS で run を使わない場合も後続 Stage 用に draft 保持。
- **想定 duration**: 0.25-0.38 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim / transient control。
- **prompt template**:

```text
A single quick running footstep on sparse dry grass and dirt, short rustle and light dirt contact, no heavy stomp, no wet mud, no forest ambience, 0.32 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、6-18 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_run_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: Rustle が耳に刺さらないよう 5-8 kHz を確認。Walk と十分に区別できる短さにする。背景環境音を含む take は捨てる。
- **asset_ledger 記載例**:

| sfx_footstep_grass_run_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_run_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.8 prompt | Trim, normalize, transient check | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Grass run single-shot; selected take TBD |

### 3.9 `sfx_footstep_grass_land_01`

- **ID**: `sfx_footstep_grass_land_01`
- **用途**: 草地 / 土での小さな着地。石や木より soft な landing。
- **想定 duration**: 0.45-0.75 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim / EQ。
- **prompt template**:

```text
A soft two-foot landing on sparse dry grass and dirt, light character weight, muted dirt thump and small grass rustle, no wet mud, no debris burst, 0.65 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、8-26 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_land_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: 低域の thump を控えめにする。Rustle tail は 0.3 秒以内。音が可愛くなりすぎる take は捨てる。
- **asset_ledger 記載例**:

| sfx_footstep_grass_land_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_land_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.9 prompt | Trim, EQ, normalize | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Grass land single-shot; selected take TBD |

### 3.10 `sfx_footstep_sand_walk_01`

- **ID**: `sfx_footstep_sand_walk_01`
- **用途**: 砂 / 乾いた土埃のある路地での通常歩行。Zone1 の床材差分として保持。
- **想定 duration**: 0.35-0.50 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim。
- **prompt template**:

```text
A single soft walking footstep on dry sand and dusty ground, light leather sole, muted granular shift, no beach waves, no desert wind, no gravel crunch, 0.45 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、6-20 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_walk_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: Beach / desert association が強い take は捨てる。High-end hiss を抑える。Stone / grass と聞き分けられる granular 成分を残す。
- **asset_ledger 記載例**:

| sfx_footstep_sand_walk_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_walk_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.10 prompt | Trim, normalize, optional pitch variation | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Sand walk single-shot; selected take TBD |

### 3.11 `sfx_footstep_sand_run_01`

- **ID**: `sfx_footstep_sand_run_01`
- **用途**: 砂 / 乾いた土埃のある路地での速い移動。
- **想定 duration**: 0.25-0.38 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One transient control。
- **prompt template**:

```text
A single quick running footstep on dry sand and dusty ground, short granular scrape, light character weight, no beach ambience, no desert wind, no gravel scatter, 0.32 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、6-18 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_run_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: Hiss が強い場合は EQ。Walk より短くする。連続再生時に noise floor が積み上がらないよう tail を短くする。
- **asset_ledger 記載例**:

| sfx_footstep_sand_run_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_run_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.11 prompt | Trim, normalize, transient check | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Sand run single-shot; selected take TBD |

### 3.12 `sfx_footstep_sand_land_01`

- **ID**: `sfx_footstep_sand_land_01`
- **用途**: 砂 / 乾いた土での小さな着地。着地先が柔らかい時に使用。
- **想定 duration**: 0.45-0.75 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim / EQ。
- **prompt template**:

```text
A soft two-foot landing on dry sand and dusty ground, light character weight, muted granular thump, small dust shift, no beach waves, no desert wind, 0.65 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、8-26 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_land_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: Low thump を控えめにする。Dust tail は短く、BGM 無音部で hiss が目立たないか確認。強い debris 音は不採用。
- **asset_ledger 記載例**:

| sfx_footstep_sand_land_01 | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_land_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §3.12 prompt | Trim, EQ, normalize | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Sand land single-shot; selected take TBD |

---

## 4. 時の窓 SFX (6)

### 4.1 `sfx_time_wheel_open_01`

- **ID**: `sfx_time_wheel_open_01`
- **用途**: Time Frame Portal の symbol wheel 表示開始。UI だが空間演出寄り。
- **想定 duration**: 0.70-0.90 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One fade / subtle reverb。
- **prompt template**:

```text
A soft circular mechanism opening made of air and dust, gentle whoosh with a tiny wooden-brass resonance, no sci-fi beep, no harsh magic sparkle, calm and restrained, 0.8 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、12-35 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/time_window/sfx_time_wheel_open_01.ogg`
- **音量レベル目標**: -18 to -12 LUFS。
- **検証ポイント**: UI 音として遅すぎないか確認。60 ms fade-in。Hall reverb は wet 15% 以下。機械音が強ければ不採用。
- **asset_ledger 記載例**:

| sfx_time_wheel_open_01 | `Assets/Audio/SFX/Zone1/time_window/sfx_time_wheel_open_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §4.1 prompt | Trim, fade, subtle reverb if needed | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Time wheel open; selected take TBD |

### 4.2 `sfx_time_wheel_close_01`

- **ID**: `sfx_time_wheel_close_01`
- **用途**: Symbol wheel 閉じる / cancel。Open の逆再生に固定せず、短く収束する音。
- **想定 duration**: 0.50-0.75 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One fade。
- **prompt template**:

```text
A soft circular time selection wheel closing, gentle inward whoosh and muted wooden-brass resonance fading into silence, no button beep, no hard snap, restrained, 0.6 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、10-28 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/time_window/sfx_time_wheel_close_01.ogg`
- **音量レベル目標**: -18 to -12 LUFS。
- **検証ポイント**: Open より短く、音量は -1 dB 程度控えめ。Tail が残りすぎる場合は fade-out。キャンセル UI 音と混同しないこと。
- **asset_ledger 記載例**:

| sfx_time_wheel_close_01 | `Assets/Audio/SFX/Zone1/time_window/sfx_time_wheel_close_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §4.2 prompt | Trim, fade-out, normalize | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Time wheel close; selected take TBD |

### 4.3 `sfx_time_symbol_hover_01`

- **ID**: `sfx_time_symbol_hover_01`
- **用途**: Symbol wheel 内の hover / focus 移動。連続して鳴る可能性が高いので短く小さい音。
- **想定 duration**: 0.08-0.15 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim。
- **prompt template**:

```text
A tiny soft focus tick, like a muted ceramic tap with a faint breath of air, very quiet, no electronic beep, no sparkle, 0.12 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、3-10 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/time_window/sfx_time_symbol_hover_01.ogg`
- **音量レベル目標**: -18 to -12 LUFS、実装時は mixer で UI hover をさらに下げる。
- **検証ポイント**: 連打しても耳につかないこと。Attack 前の無音を 0 に近づける。Click が硬すぎる take は捨てる。
- **asset_ledger 記載例**:

| sfx_time_symbol_hover_01 | `Assets/Audio/SFX/Zone1/time_window/sfx_time_symbol_hover_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §4.3 prompt | Trim, normalize, click check | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Symbol hover; selected take TBD |

### 4.4 `sfx_time_symbol_select_red_01`

- **ID**: `sfx_time_symbol_select_red_01`
- **用途**: 赤シンボル選択確定。Portal open の前段として、選択できた感触を出す。
- **想定 duration**: 0.25-0.45 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One pitch / EQ。
- **prompt template**:

```text
A soft decisive symbol selection sound, warm muted chime and tiny air pulse, grounded and restrained, no victory tone, no bright magic sparkle, no electronic beep, 0.35 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、6-18 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/time_window/sfx_time_symbol_select_red_01.ogg`
- **音量レベル目標**: -18 to -12 LUFS。
- **検証ポイント**: Hover より明確だが派手にしない。Pitch を hover より低めにする。赤選択の特別感は音量ではなく tail で表現。
- **asset_ledger 記載例**:

| sfx_time_symbol_select_red_01 | `Assets/Audio/SFX/Zone1/time_window/sfx_time_symbol_select_red_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §4.4 prompt | Trim, optional pitch/EQ, normalize | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Red symbol select; selected take TBD |

### 4.5 `sfx_time_portal_open_01`

- **ID**: `sfx_time_portal_open_01`
- **用途**: 赤シンボル選択後、過去 / 現在境界が開く瞬間。Time Frame Portal の主 SFX。
- **想定 duration**: 1.40-1.80 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Stable Audio complement if too thin、Studio One reverb / fade。
- **prompt template**:

```text
A soft sustained portal opening from silence, air gently displaced across an old quiet town, subtle low warmth and faint bell-like resonance, no sci-fi laser, no harsh magic, no choir, 1.6 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、25-70 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/time_window/sfx_time_portal_open_01.ogg`
- **音量レベル目標**: -18 to -12 LUFS。
- **検証ポイント**: BGM の piano / strings とぶつかる melody 成分がないこと。Tail は portal visual の表示時間に合わせる。Reverb wet 20% 以下。低域が膨らめば HPF。
- **asset_ledger 記載例**:

| sfx_time_portal_open_01 | `Assets/Audio/SFX/Zone1/time_window/sfx_time_portal_open_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned; Stable Audio only if used and verified | `docs/asset_prompts/sfx_zone1.md` §4.5 prompt | Trim, fade, subtle reverb, HPF | Yes after paid-plan generation; record generation ID and selected take | GitHub Public ok after final export | Tier 1 player-consumed | Portal open main SFX; selected take TBD |

### 4.6 `sfx_time_portal_flip_01`

- **ID**: `sfx_time_portal_flip_01`
- **用途**: Portal 境界を跨ぐ / 過去現在の切替が完了する瞬間。Scene wiring 側では transition の midpoint に合わせる。
- **想定 duration**: 0.90-1.20 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One stereo-to-mono balance / fade。
- **prompt template**:

```text
A brief soft threshold crossing sound, quiet air fold with a muted bell resonance passing through and settling, no rewind effect, no loud impact, no electronic glitch, 1 second, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、18-45 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/time_window/sfx_time_portal_flip_01.ogg`
- **音量レベル目標**: -18 to -12 LUFS。
- **検証ポイント**: Open と差別化し、短い transition 音にする。Reverse effect が安っぽい take は捨てる。Animation midpoint と同期して違和感がないこと。
- **asset_ledger 記載例**:

| sfx_time_portal_flip_01 | `Assets/Audio/SFX/Zone1/time_window/sfx_time_portal_flip_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §4.6 prompt | Trim, fade, normalize | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Portal crossing/flip; selected take TBD |

---

## 5. NPC 反応 SFX (3)

### 5.1 `sfx_npc_greeting_short_01`

- **ID**: `sfx_npc_greeting_short_01`
- **用途**: NPC 対話開始時の短い反応。ボイスではなく、衣擦れ / 小さな非言語反応として扱う。
- **想定 duration**: 0.35-0.60 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim。
- **prompt template**:

```text
A tiny nonverbal greeting cue made of soft cloth movement and a barely audible gentle breath, no spoken words, no humming, no identifiable voice, quiet and restrained, 0.5 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、8-22 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/npc/sfx_npc_greeting_short_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: 言葉 / humming が入った take は即不採用。Breath が生々しい場合は cloth 寄り take に差し替え。対話テキストを邪魔しない音量にする。
- **asset_ledger 記載例**:

| sfx_npc_greeting_short_01 | `Assets/Audio/SFX/Zone1/npc/sfx_npc_greeting_short_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §5.1 prompt | Trim, normalize, no-voice check | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Nonverbal NPC greeting; selected take TBD |

### 5.2 `sfx_npc_interaction_ack_01`

- **ID**: `sfx_npc_interaction_ack_01`
- **用途**: 選択肢確定 / ActionRecord 反映後の NPC 反応。会話のテンポを止めない小音。
- **想定 duration**: 0.40-0.70 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim / EQ。
- **prompt template**:

```text
A very soft nonverbal acknowledgement cue, slight fabric shift and quiet exhale-like air movement, no words, no humming, no clear emotion, subdued and human but not voiced, 0.6 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、8-24 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/npc/sfx_npc_interaction_ack_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: Voice 判定される要素を避ける。Greeting より少し長くてもよいが、emotion が強すぎる take は捨てる。Dialog text reveal と重ねて確認。
- **asset_ledger 記載例**:

| sfx_npc_interaction_ack_01 | `Assets/Audio/SFX/Zone1/npc/sfx_npc_interaction_ack_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §5.2 prompt | Trim, EQ, no-voice check | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Nonverbal NPC acknowledgement; selected take TBD |

### 5.3 `sfx_npc_departure_01`

- **ID**: `sfx_npc_departure_01`
- **用途**: NPC 会話終了 / 立ち去り / 視線を外す時の短い cloth + small step cue。
- **想定 duration**: 0.50-0.90 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim。
- **prompt template**:

```text
A quiet nonverbal departure cue, soft cloth shift and one tiny step away on old floor, no spoken words, no sigh, no dramatic emotion, restrained and close, 0.8 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、10-28 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/npc/sfx_npc_departure_01.ogg`
- **音量レベル目標**: -18 to -14 LUFS。
- **検証ポイント**: 足音素材と被りすぎないよう cloth 成分を残す。会話終了直後の無音に馴染むこと。声やため息が明瞭な take は不採用。
- **asset_ledger 記載例**:

| sfx_npc_departure_01 | `Assets/Audio/SFX/Zone1/npc/sfx_npc_departure_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §5.3 prompt | Trim, normalize, no-voice check | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | Nonverbal NPC departure; selected take TBD |

---

## 6. UI SFX (3)

### 6.1 `sfx_ui_button_click_01`

- **ID**: `sfx_ui_button_click_01`
- **用途**: 汎用 button click / confirm ではない軽い UI 押下。TextMeshPro UI と同時に使う。
- **想定 duration**: 0.06-0.12 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim。
- **prompt template**:

```text
A very small muted ceramic button click, soft tactile UI sound, no electronic beep, no plastic snap, no arcade tone, 0.08 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、3-8 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/ui/sfx_ui_button_click_01.ogg`
- **音量レベル目標**: -18 to -12 LUFS、mixer で実使用時は控えめに調整。
- **検証ポイント**: Attack 前無音を削る。連打で耳につかないこと。Time symbol hover と音色が近すぎる場合はこちらを少し低くする。
- **asset_ledger 記載例**:

| sfx_ui_button_click_01 | `Assets/Audio/SFX/Zone1/ui/sfx_ui_button_click_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §6.1 prompt | Trim, normalize, click check | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | UI button click; selected take TBD |

### 6.2 `sfx_ui_menu_open_01`

- **ID**: `sfx_ui_menu_open_01`
- **用途**: Menu / inventory / dialogue log などの panel open。
- **想定 duration**: 0.15-0.25 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim / fade。
- **prompt template**:

```text
A soft restrained menu opening sound, gentle paper-and-wood slide with tiny air movement, no electronic beep, no whooshy sci-fi UI, no bright chime, 0.2 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、4-12 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/ui/sfx_ui_menu_open_01.ogg`
- **音量レベル目標**: -18 to -12 LUFS。
- **検証ポイント**: Menu close と対で聴く。Panel animation と同期。Paper 成分が page rustle / library ambience と混同する場合は wood slide 寄りに変更。
- **asset_ledger 記載例**:

| sfx_ui_menu_open_01 | `Assets/Audio/SFX/Zone1/ui/sfx_ui_menu_open_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §6.2 prompt | Trim, fade, normalize | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | UI menu open; selected take TBD |

### 6.3 `sfx_ui_menu_close_01`

- **ID**: `sfx_ui_menu_close_01`
- **用途**: Menu / inventory / dialogue log などの panel close。
- **想定 duration**: 0.12-0.22 秒。
- **推奨ツール**: ElevenLabs SFX v2 first try、Studio One trim / fade。
- **prompt template**:

```text
A soft restrained menu closing sound, gentle paper-and-wood slide settling into silence, slightly lower than the opening sound, no electronic beep, no hard snap, 0.18 seconds, mono.
```

- **出力形式**: OGG Vorbis quality 6、mono、44.1 kHz、4-12 KB 目安。
- **配置先**: `Assets/Audio/SFX/Zone1/ui/sfx_ui_menu_close_01.ogg`
- **音量レベル目標**: -18 to -12 LUFS。
- **検証ポイント**: Open より少し低く短くする。Tail が残りすぎないこと。Cancel 音として使う場合も過度に否定的な音にしない。
- **asset_ledger 記載例**:

| sfx_ui_menu_close_01 | `Assets/Audio/SFX/Zone1/ui/sfx_ui_menu_close_01.ogg` | 2026-05-XX | ElevenLabs SFX v2 + Studio One | ElevenLabs Creator paid + Studio One owned | `docs/asset_prompts/sfx_zone1.md` §6.3 prompt | Trim, fade, normalize | Yes after ElevenLabs paid-plan generation; record generation ID | GitHub Public ok after final export | Tier 1 player-consumed | UI menu close; selected take TBD |

---

## 7. Studio One / 一発出し仕上げワークフロー

### 7.1 ElevenLabs / Stable Audio 出力

1. 各 entry の prompt で 4-8 candidates を生成する。
2. 元ファイルを `audio/_intermediate/sfx_zone1/<category>/` に保存する。
3. 一発採用できるものは Studio One を通さず OGG q6 へ変換してよい。
4. Loop、fade、noise、音量、time window reverb、footstep variation が必要なものだけ Studio One で仕上げる。
5. 実生成時の generation ID、selected take、最終 LUFS、手修正内容を `asset_ledger.md` §2.4 に転記する。

### 7.2 共通処理

- **Trim**: 前無音を短くし、不要な tail を落とす。UI / 足音は発音タイミング重視。
- **Fade**: One-shot は 5-80 ms、ambience loop は 100-300 ms crossfade。
- **Normalize**: peak -3 dB 以下。最終的な体感音量は LUFS と Unity mixer で調整。
- **High-pass filter**: 原則 40 Hz 以下を整理。Landing / portal で低域が邪魔なら 80 Hz まで上げる。
- **Pitch / amplitude variation**: Footstep は Unity random pitch を前提にし、必要なら Studio One で 2-4 alternates を追加生成する。
- **Reverb**: Time window のみ控えめに使う。Hall / Plate は wet 15-20% 以下。

---

## 8. Summary Table

| ID | 用途 | Duration | Tool | OGG file path |
|---|---|---:|---|---|
| `sfx_env_birds_01` | 外エリアの低確率 distant bird | 0.8-1.2s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/environment/sfx_env_birds_01.ogg` |
| `sfx_env_wind_loop_01` | 外エリア常時 wind loop | 10-12s loop | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/environment/sfx_env_wind_loop_01.ogg` |
| `sfx_env_dry_leaves_01` | 落ち葉の低確率 one-shot | 1.5-2.5s | Studio One foley | `Assets/Audio/SFX/Zone1/environment/sfx_env_dry_leaves_01.ogg` |
| `sfx_env_distant_water_01` | 噴水跡 / 水の記憶 proximity loop | 8-12s loop | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/environment/sfx_env_distant_water_01.ogg` |
| `sfx_env_wood_creak_01` | 遠い建材の creak one-shot | 0.8-1.4s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/environment/sfx_env_wood_creak_01.ogg` |
| `sfx_env_silence_pad_01` | 静寂感 ambience pad loop | 10-15s loop | Stable Audio | `Assets/Audio/SFX/Zone1/environment/sfx_env_silence_pad_01.ogg` |
| `sfx_footstep_wood_walk_01` | 木床 walk | 0.30-0.45s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_walk_01.ogg` |
| `sfx_footstep_wood_run_01` | 木床 run | 0.22-0.35s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_run_01.ogg` |
| `sfx_footstep_wood_land_01` | 木床 land | 0.45-0.70s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_land_01.ogg` |
| `sfx_footstep_stone_walk_01` | 石畳 walk | 0.30-0.45s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_walk_01.ogg` |
| `sfx_footstep_stone_run_01` | 石畳 run | 0.22-0.35s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_run_01.ogg` |
| `sfx_footstep_stone_land_01` | 石畳 land | 0.45-0.70s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_land_01.ogg` |
| `sfx_footstep_grass_walk_01` | 草地 walk | 0.35-0.50s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_walk_01.ogg` |
| `sfx_footstep_grass_run_01` | 草地 run | 0.25-0.38s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_run_01.ogg` |
| `sfx_footstep_grass_land_01` | 草地 land | 0.45-0.75s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_land_01.ogg` |
| `sfx_footstep_sand_walk_01` | 砂 / 乾いた土 walk | 0.35-0.50s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_walk_01.ogg` |
| `sfx_footstep_sand_run_01` | 砂 / 乾いた土 run | 0.25-0.38s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_run_01.ogg` |
| `sfx_footstep_sand_land_01` | 砂 / 乾いた土 land | 0.45-0.75s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_land_01.ogg` |
| `sfx_time_wheel_open_01` | Time wheel open | 0.70-0.90s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/time_window/sfx_time_wheel_open_01.ogg` |
| `sfx_time_wheel_close_01` | Time wheel close | 0.50-0.75s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/time_window/sfx_time_wheel_close_01.ogg` |
| `sfx_time_symbol_hover_01` | Symbol hover / focus | 0.08-0.15s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/time_window/sfx_time_symbol_hover_01.ogg` |
| `sfx_time_symbol_select_red_01` | Red symbol select | 0.25-0.45s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/time_window/sfx_time_symbol_select_red_01.ogg` |
| `sfx_time_portal_open_01` | Portal open | 1.40-1.80s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/time_window/sfx_time_portal_open_01.ogg` |
| `sfx_time_portal_flip_01` | Portal crossing / flip | 0.90-1.20s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/time_window/sfx_time_portal_flip_01.ogg` |
| `sfx_npc_greeting_short_01` | NPC greeting cue | 0.35-0.60s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/npc/sfx_npc_greeting_short_01.ogg` |
| `sfx_npc_interaction_ack_01` | NPC acknowledgement cue | 0.40-0.70s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/npc/sfx_npc_interaction_ack_01.ogg` |
| `sfx_npc_departure_01` | NPC departure cue | 0.50-0.90s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/npc/sfx_npc_departure_01.ogg` |
| `sfx_ui_button_click_01` | UI button click | 0.06-0.12s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/ui/sfx_ui_button_click_01.ogg` |
| `sfx_ui_menu_open_01` | UI menu open | 0.15-0.25s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/ui/sfx_ui_menu_open_01.ogg` |
| `sfx_ui_menu_close_01` | UI menu close | 0.12-0.22s | ElevenLabs SFX | `Assets/Audio/SFX/Zone1/ui/sfx_ui_menu_close_01.ogg` |

---

## 9. 検証チェックリスト

- [ ] 30 種の final OGG が `Assets/Audio/SFX/Zone1/` 配下に揃っている。
- [ ] 環境 6 / 足音 12 / 時の窓 6 / NPC 3 / UI 3 の内訳を維持している。
- [ ] すべて OGG Vorbis quality 6、mono、44.1 kHz で export されている。
- [ ] 環境 loop は seam が目立たず、BGM と重ねても旋律・楽器感が出ない。
- [ ] 足音は Unity の random pitch / volume と連続再生で破綻しない。
- [ ] 時の窓 SFX は UI / portal animation と同期し、SF beep / 派手な magic sparkle に寄っていない。
- [ ] NPC 反応は no words / no humming / no identifiable voice を満たす。
- [ ] UI SFX は連打しても耳につかず、time window hover と混同しない。
- [ ] ElevenLabs Creator paid、Stable Audio 使用時の商用条件、Studio One 所有状態を `asset_ledger.md` §1.2 / §2.4 に記録した。
- [ ] Steam AI 開示区分は Tier 1 player-consumed として台帳に残した。

---

## 10. ユーザー判断ポイント

- **鳥 SFX の採用可否**: Zone1 の世界に鳥の気配を置くか。採用しても低頻度・遠距離に留める。
- **Run 足音の実使用**: VS で run が少ない場合でも 12 種 draft は保持し、実装側で walk のみ先に接続してよい。
- **`sfx_env_silence_pad_01` の有無**: BGM と重ねて音楽的 drone に聞こえるなら不採用。
- **時の窓の質感**: 木 / 真鍮 / 空気寄りの現行案で進めるか、もう少し機械的にするか。
- **NPC 反応の境界**: Silent protagonist / no voice 方針に合わせ、息づかいが voice と感じられる take は避ける。

---

## 11. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草。環境音 / 足音 / 時の窓 / NPC / UI の各 SFX 用 ElevenLabs プロンプト + Studio One 仕上げ手順 |
| v0.1 | 2026-05-05 | DAW 仕様を Studio One に修正。一発出し / AI 側調整優先の採用フローを追加 |
| v1.0 draft | 2026-05-05 | SFX 30 種の ID、用途、duration、推奨ツール、prompt、OGG 出力、配置先、LUFS、検証ポイント、asset_ledger 記載例、summary table を追加 |
