# G5 Manual Evaluation Rubric
> Status: v0.1 draft (2026-05-05)
> Purpose: `docs/G5_ACCEPTANCE_MATRIX.md` の user manual sections (§H / §I / §L / §M) を、user が `OK / NG / 微妙` で判断しやすくするための評価軸。

## 1. 評価方法
本 rubric は採否を先に決める文書ではない。実際の audio 入り build を触り、各観点に対して「観察した事実」を短く残す。
| 評価 | 使い方 |
|---|---|
| OK | G5 時点の Vertical Slice として支障なし。Stage 4 で磨く余地は残っていてよい。 |
| 微妙 | 進行は止めないが、Stage 4 entry / review backlog に残したい違和感がある。 |
| NG | G5 完了判断を止める。再 build / asset revise / wiring 修正が必要。 |
記録は `docs/G5_ACCEPTANCE_MATRIX.md` の該当 row の result / notes に転記する。`NG` / `微妙` は、原因推測より先に具体観察を 1 行で書く。

## H. Audio Listen Rubric
### H-01 BGM (`Zone1_Ambient.ogg`)
- 衰退世界の atmosphere と整合しているか。静謐、メランコリック、希望が少し残る感触があるか。
- 5-8 分の通し中、3:04.84 の loop / fade が自然に聞こえるか。
- -18 LUFS 前後の level が、dialogue / SFX / UI feedback を妨げていないか。
- Past / Current の場面転換で BGM が急に主張しすぎないか。
判断メモ例:
- OK: loop point に気付かず、SFX と dialogue の邪魔にならない。
- 微妙: 曲調は合うが、loop 直前だけ音量変化が気になる。
- NG: BGM が大きすぎて interaction cue を聞き落とす。

### H-02 SFX 30 種
- 環境音: wind / silence pad が背景に溶け、BGM と帯域や雰囲気でぶつからないか。
- 足音 12 種: wood / stone / grass / sand の違いが、意識すれば知覚できるか。
- 時の窓 SFX: wheel open / close、symbol hover / select、portal open / flip が player feedback として機能するか。
- NPC cue: greeting / acknowledgement / departure が対話可能性や反応に気付きやすい音になっているか。
- UI cue: button click / menu open / menu close が近すぎず、遠すぎず、連打して不快でないか。
判断メモ例:
- OK: 操作結果が音で分かり、環境音は存在感が控えめ。
- 微妙: 足音素材差は分かるが、grass と sand が近い。
- NG: portal open / flip が鳴っても状態変化が分からない。

## I. UI Visual Rubric
### I-01 TMP Font Visual
- 美咲ゴシック JP: dialogue panel で字形が読みやすく、欠字 / tofu / TMP warning が目立たないか。
- Press Start 2P EN: 8x8 pixel-based font の retro 感が強すぎず、英語 UI の実用性を損なわないか。
- Dialogue panel: 改行、line height、panel 内余白が自然か。長い文で窮屈に見えないか。
- JP / EN 切替: 同じ UI でレイアウト破綻や fallback の違和感が出ないか。
判断メモ例:
- OK: JP/EN とも読め、panel 内の余白が自然。
- 微妙: EN は雰囲気に合うが小さい文字が疲れる。
- NG: VS 文言に欠字または tofu が出る。

### I-02 Palette / Sprite / UI Cohesion
- Anemora パレット v0 が sprite / building / UI で統一感を作っているか。
- muted earth tone が画面全体で機能し、暗すぎる / 地味すぎる / UI が埋もれる状態になっていないか。
- F2 sprite: Niro provisional sprite が中性的に見えるか。
- F2 sprite: 帽子の有無や輪郭が、Stage 4 redraw 候補として記録すべき違和感になっていないか。
- Resident_A / Resident_B が背景や UI と過剰に浮かず、必要な見分けやすさを保っているか。
判断メモ例:
- OK: character / environment / UI が同じ画面で馴染む。
- 微妙: 主人公の帽子要件は Stage 4 redraw 候補として残したい。
- NG: UI text が背景色に埋もれて読めない。

## L. 5-8 Minute Playthrough Rubric
### L-01 Hook 3 分の再現
- 衰退街を歩く感触が、開始 1-2 分で伝わるか。
- 残響を察知し、筆を構え、フレームを開き、結晶を選ぶ流れが分かるか。
- 立体ジオラマ、境界またぎ、過去少女、廃墟側指差し、フレーム消滅が一連の体験として繋がるか。
- 3 分時点で「もっと知りたい」が発火するか。

### L-02 操作と統合感
- WASD / Shift / 左クリック / E の入力導線で迷わないか。
- audio + visual + interaction が一体で動いているように感じるか。
- Softlock、入力不能、camera / layer 表示破綻がないか。
- 60 FPS / p95 16 ms 目標に対して、体感で引っかかりや frame drop が目立たないか。

### L-03 Core Flow Completion
- Past portal 通過 → book 取得 → Current 帰還 → Bed 上 spawn が成立するか。
- Resident_A (過去少女) / Resident_B (現在の観察者) と 1 回ずつ対話できるか。
- Dialogue が ja-JP / en 切替で表示されるか。
- 5-8 分で VS の最小体験として一区切りが付くか。
判断メモ例:
- OK: 5-8 分で book reflection と 2 NPC 対話まで自然に到達。
- 微妙: flow は成立するが、次に何をするか一度迷う。
- NG: portal 往復後に進行不能、または book reflection が見えない。

## M. Hint Presentation Rubric
> Internal note: G5 matrix §M の見出しに含まれる「層 2」は設計用便宜語。player-facing text には出さない。評価時は「次の認識段階の片鱗が伝わるか」だけを見る。

### M-01 Minimum Hint
- book 取得後、現在側で本がない / 本が移動した / 取った痕跡が残るなど、能動行動の結果が見えるか。
- 「観測するだけでは変わらないが、行動すると変わる」という minimum 体験が伝わるか。
- 説明台詞に頼りすぎず、「何かが変わった」感が player に伝わるか。
- まだ大きな種明かしをしすぎていないか。

### M-02 Forbidden Surface Terms
- UI / dialogue / menu / narration に「層」「ベール剥離」などの設計用語が出ていないか。
- 表現が必要な場合は、違和感、記憶、痕跡、風景変化、視野の広がりとして見えているか。
判断メモ例:
- OK: 本の痕跡で「行動が現在に届いた」ことが分かる。
- 微妙: 変化はあるが、気付くには少し弱い。
- NG: 設計用語がそのまま UI / dialogue に出る、または変化が全く伝わらない。

## 2. Matrix 記録テンプレート
`docs/G5_ACCEPTANCE_MATRIX.md` の該当 row に以下の形で追記する。
```text
Result: OK / 微妙 / NG
Observation: <実際に見た/聞いたことを 1 行>
Follow-up: <必要なら Stage 4 backlog / immediate fix / no action>
```

## 3. Revision History
| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-05 | Initial manual rubric for G5 §H / §I / §L / §M user evaluation |
