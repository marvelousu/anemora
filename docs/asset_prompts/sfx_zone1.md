# Zone 1 SFX Generation Prompt Template (ElevenLabs SFX v2 + Studio One)

> G5 で使用する Zone 1 用 SFX (環境音 + 足音 + 時の窓 SFX + その他) のプロンプトテンプレート。
> ADR-0003 §Decision に従い、**ElevenLabs SFX v2** で全 SFX を生成、**Studio One (DAW)** で音量・ループ点・無音処理。

> **Status (2026-05-05)**: v0.1。DAW 仕様を Studio One に修正。一発出しで使える SFX は ElevenLabs 出力を優先し、Studio One は必要時のみ使用。

---

## 1. 共通方針

### 1.1 SFX 種別 (VS_SCOPE §5.2)

- 環境音: 5-8 種 (鳥 / 風 / 水 / 遠い物音、ループ可能)
- 足音: 3-5 種 (床タイル別)
- 時の窓 SFX: 5 種 (描画 / シンボル選択 / 踏込み / 持ち帰り / 帰還)
- NPC 反応 SFX: 3-5 種 (息づかい程度、ボイスは不採用)

### 1.2 出力規格

- **形式**: WAV (生成時) → OGG Vorbis (Unity 取込み時、quality 6)
- **サンプルレート**: 48 kHz / 24-bit (Studio One 編集中) → 44.1 kHz / 16-bit (export)
- **モノラル / ステレオ**: 環境音はステレオ、SFX 単発はモノラル (3D オーディオで定位、Unity 側で空間化)
- **長さ**: SFX 単発 0.3-2.0 秒、環境音ループ 30-60 秒
- **音量基準**: -18 LUFS (BGM とほぼ同レベル、ゲーム内 mixer で再調整)

### 1.3 ElevenLabs SFX v2 の特性

- テキストプロンプトから 5-10 秒の SFX 生成
- **Voice 系条項とは別の Sound Effects Terms** に従う (ADR-0003 §法的整合)
- paid plan 必須 (free plan は商用利用不可、`asset_ledger.md` §1.2)
- 短い SFX は 0.3-2 秒に切り出して使用
- 一発出しで長さ・音量・ノイズ・ループが問題なければ、そのまま OGG 変換して採用
- 問題が小さい場合は Studio One 編集より先に ElevenLabs 側で prompt / duration / variation を調整して再生成
- ループ素材だけは必要に応じて Studio One でクロスフェードを作る

---

## 2. 環境音 (Ambient SFX)

### 2.1 wind_subtle.ogg (常時、家の中 + 外)

```
Soft gentle ambient wind, very quiet, distant whisper-like sound, no gusts, no leaves
rustling loudly, suitable for a quiet abandoned town atmosphere, 30 seconds long,
seamless loop.
```

- 用途: 家の中でも微かに聞こえる、外に出るとやや大きく
- Unity 側で2D AudioSource (家中) + 3D AudioSource (外) で音量切替

### 2.2 wind_outdoor.ogg (外、中央広場 + 図書館跡)

```
Soft outdoor wind through a quiet plaza, occasional very gentle gust, faint rustle
of dry leaves in the distance, no birds, no people, no vehicles, melancholic
atmosphere, 45 seconds long, seamless loop.
```

### 2.3 distant_creak.ogg (遠い物音、不定期に再生)

```
A very distant single wooden creak, like an old building settling far away, very
quiet, sparse, single occurrence, 1.5 seconds, mono.
```

- Unity 側で 30-90 秒間隔のランダムタイマーで再生 (静謐の中の小さな違和感)

### 2.4 distant_water.ogg (噴水跡近く、水音の名残のような幻聴)

```
A faint distant trickling water sound, very quiet, almost imagined, no clear source,
suitable for a place where water used to flow but doesn't anymore, 20 seconds,
seamless loop, very low volume.
```

- 用途: Plaza_Center の噴水跡 (案 B) 採用時のみ。「水音の幻聴」として近接時のみ再生

### 2.5 bird_distant.ogg (TBD、`STAGE3_TBD_RESOLUTION.md` §4.1)

鳥の声有無は Stage 3 試作で決定。仮プロンプト:

```
A single distant bird call, soft and short, like a small songbird far away, no echo,
no flock, just one quiet call, 1 second, mono.
```

- Unity 側で 60-120 秒間隔のランダムタイマー再生 (有採用時)
- 不採用時はファイル自体生成しない

### 2.6 page_rustle.ogg (図書館跡周辺、たまに)

```
A very soft single paper page turning sound, like an old book, no clear source, 1
second, mono, very quiet.
```

- 図書館跡近接時に低確率で再生 (主要違和感の前触れ)

---

## 3. 足音 (Footsteps)

### 3.1 footstep_stone.ogg (中央広場、家の周辺)

```
A single soft footstep on weathered stone tile, leather sole, gentle press without
heavy impact, no echo, mono, 0.3 seconds.
```

生成 1 回 → Studio One でピッチ shift / amplitude variation を 4 種作成 (`footstep_stone_01.ogg` 〜 `footstep_stone_04.ogg`)。Unity 側でランダム再生。

### 3.2 footstep_wood.ogg (家の中)

```
A single soft footstep on aged wooden plank floor, leather sole, slight wood creak,
no echo, mono, 0.3 seconds.
```

同じく Studio One で 4 variation 作成。

### 3.3 footstep_grass.ogg (図書館跡周辺の草地、optional)

```
A single soft footstep on dry grass and dirt, leather sole, very quiet rustle, no
echo, mono, 0.3 seconds.
```

VS で草地を実装するなら採用、しないなら省略。

---

## 4. 時の窓 SFX (5 種、VS_SCOPE §5.2)

### 4.1 portal_open.ogg (ポータル展開、赤シンボル選択直後)

```
A soft sustained whoosh that grows from silence, like air being gently displaced,
no harsh frequencies, no electronic synth artifacts, ethereal but grounded, 1.5
seconds, mono.
```

- Studio One で Reverb (Hall, wet 30%) を加える
- Pitch envelope: ゆっくり上昇 (時間が動き始める感)

### 4.2 symbol_select.ogg (シンボル選択時、赤を focus した瞬間)

```
A very subtle single chime or soft pluck, like a tiny brass bell struck once very
gently, brief and clean, 0.5 seconds, mono.
```

- 白 / 青を focus した時用に、別バリエーション (フィルタかけて鈍い音) も作る
  - `symbol_select_disabled.ogg`: 上記より低音、attack 弱め

### 4.3 crossing.ogg (踏込み、ポータル平面を越えた瞬間)

```
A brief soft whoosh combined with a very quiet bell-like resonance, like crossing a
threshold of air, 1 second, mono. No harsh impact.
```

- Studio One で stereo に展開 (左右に音が広がる、空間切替の演出)
- 帰還時は同ファイルを再生 (rewind 不要、対称的演出で十分)

### 4.4 take_item.ogg (持ち帰り、過去で本を取った瞬間)

```
A soft paper rustle combined with a very quiet single piano note (in A minor), 0.6
seconds, mono. Like picking up a small object that has weight.
```

- Studio One で paper rustle と piano note を別レイヤーで重ね、piano は -18 dB 以下に控える

### 4.5 return.ogg (帰還、現在に痕跡が反映される瞬間)

```
A very soft sustained tone fading in then out, like a quiet bell ringing in the
distance, 2 seconds, mono. Subtle, not triumphant.
```

- 痕跡反映 = 「世界が静かに変わった」の演出。ファンファーレにしない

---

## 5. NPC 反応 SFX

### 5.1 npc_acknowledge.ogg (NPC が主人公を認識する微かな息づかい)

```
A very soft barely audible breath, like someone noticing your presence and exhaling
quietly, 0.5 seconds, mono. No words, no humming, just a single soft breath.
```

- Resident_A / Resident_B の対話開始時に使用
- 各 NPC で variant 作成 (Resident_A は少し低音、Resident_B はより高音)

### 5.2 npc_post_reflect.ogg (痕跡反映後の対話変化を示唆する微かな音)

```
A very soft single subtle sigh, like quiet realization, 0.7 seconds, mono. No words,
no clear emotion beyond gentle.
```

---

## 6. UI SFX (補助、ADR-0007 関連)

### 6.1 ui_focus.ogg (UI フォーカス移動)

```
A very subtle single click, like a tiny ceramic tap, 0.1 seconds, mono.
```

### 6.2 ui_confirm.ogg (UI 決定)

```
A soft single chime, slightly warmer than ui_focus, 0.3 seconds, mono.
```

### 6.3 ui_cancel.ogg (UI キャンセル)

```
A very subtle single descending tone, 0.2 seconds, mono.
```

---

## 7. ディレクトリ配置 (ADR-0004 + STAGE3_E_PLAN §5)

```
Assets/Audio/SFX/
├── env/
│   ├── wind_subtle.ogg
│   ├── wind_outdoor.ogg
│   ├── distant_creak.ogg
│   ├── distant_water.ogg
│   ├── page_rustle.ogg
│   └── bird_distant.ogg                  (TBD、採用時のみ)
├── footstep/
│   ├── footstep_stone_01.ogg 〜 _04.ogg
│   ├── footstep_wood_01.ogg 〜 _04.ogg
│   └── footstep_grass_01.ogg 〜 _04.ogg  (採用時のみ)
├── timeframe/
│   ├── portal_open.ogg
│   ├── symbol_select.ogg
│   ├── symbol_select_disabled.ogg
│   ├── crossing.ogg
│   ├── take_item.ogg
│   └── return.ogg
├── npc/
│   ├── npc_acknowledge_a.ogg
│   ├── npc_acknowledge_b.ogg
│   └── npc_post_reflect.ogg
└── ui/
    ├── ui_focus.ogg
    ├── ui_confirm.ogg
    └── ui_cancel.ogg
```

---

## 8. Studio One / 一発出し仕上げワークフロー

### 8.1 ElevenLabs 出力 → Studio One

1. ElevenLabs SFX v2 でテキストプロンプトから 4-8 候補生成
2. ベスト 1-2 を `audio/_intermediate/sfx_zone1/{category}/` にダウンロード (gitignore)
3. 一発出しで成立するか確認。成立する場合は Studio One を通さず OGG q6 に変換して採用
4. ループ点、音量、無音、ノイズ、足音 variation、時の窓リバーブが必要な場合だけ Studio One で開く

### 8.2 共通処理

- **Trim**: 不要な無音 / 環境ノイズを切り落とし
- **Normalize**: -3 dB ピーク
- **High-pass filter**: 40 Hz 以下をカット (低音域整理)
- **Loop point** (環境音のみ): 開始と終了をクロスフェードで滑らかに
- **Pitch / Amplitude variation** (足音のみ): 1 ファイルから 4 variation を生成
- **Reverb** (時の窓 SFX のみ): 用途別に Hall / Plate

### 8.3 Export

- Format: OGG Vorbis、quality 6
- ファイル命名: `{category}_{name}_{nn}.ogg` (variation 番号付き)

---

## 9. 検証ポイント

VS_SCOPE §5.2 と整合:

- [ ] 環境音 5-8 種が揃う
- [ ] 足音 3-5 種が揃う (variation 含む)
- [ ] 時の窓 SFX 5 種が揃う (`STAGE3_E_PLAN` で参照)
- [ ] NPC 反応 SFX 3-5 種が揃う
- [ ] UI SFX 3 種が揃う
- [ ] Voice なし (Silent protagonist 整合)
- [ ] 商用ライセンス (ElevenLabs SFX v2 paid plan) を満たす

---

## 10. asset_ledger 記載例

`docs/legal/asset_ledger.md` §2.4 環境音 / SFX に追記。各 SFX を 1 行ずつ記録 (ファイル数が多いのでテーブルが長くなるが、Stage 3 中の運用負荷を Codex / ユーザーで分担):

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| sfx_env_wind_subtle | wind_subtle.ogg | 2026-05-XX | ElevenLabs SFX v2 + Studio One | EL paid + Studio One owned | §2.1 prompt | Studio One trim/normalize/loop | 可 (EL Sound Effects Terms) | GitHub Public 可 | Tier 1 player-consumed | 30 秒ループ |
| sfx_tf_portal_open | portal_open.ogg | 2026-05-XX | (同上) | (同上) | §4.1 prompt | Reverb 追加 | (同上) | (同上) | (同上) | 時の窓展開時 |

(以下、各 SFX について同様に記載)

---

## 11. ユーザー判断ポイント

- **bird_distant の採用可否**: Stage 3 試作で世界観に鳥がいるか判断
- **footstep_grass の採用可否**: 図書館跡周辺に草地を実装するか判断
- **time_window 系 SFX のトーン**: §4 のプロンプトで「ethereal」「subtle」を強調しているが、もう少し「機械的」な質感の方が良いか試行で判断

---

## 12. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草。環境音 / 足音 / 時の窓 / NPC / UI の各 SFX 用 ElevenLabs プロンプト + Studio One 仕上げ手順 |
| v0.1 | 2026-05-05 | DAW 仕様を Studio One に修正。一発出し / AI 側調整優先の採用フローを追加 |
