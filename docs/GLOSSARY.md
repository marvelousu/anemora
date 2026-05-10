# Anemora Glossary

Status: v0.1 draft (2026-05-06)

本 glossary は、Anemora の用語を参照しやすくする索引である。用語の意味を新規に確定する authority ではなく、`CONCEPT.md`、`SPEC.md`、`docs/VS_SCOPE.md`、`docs/PITCH_PUBLIC.md`、`docs/TRAILER_SCRIPT.md`、`docs/localization/glossary.md` の記述を整理する。

`docs/localization/glossary.md` は翻訳・ローカライズ向けの用語集であり、本 doc は player-facing / internal / design / production の利用区分を扱う。

## 1. Player-facing 用語

公開 doc、Steam description、trailer、インゲーム UI / dialogue で使用してよい用語。Provisional と書いたものは現時点の採用状態であり、Stage 4 以降の見直し余地を残す。

| 用語 | 公式表記 | 区分 | 定義 | 出現 doc |
|---|---|---|---|---|
| Niro | Niro / ニロ | player-facing | 主人公名。Stage 3 /spec resolution で provisional 採用。中性表現、15-19 歳。 | `SPEC.md`, `docs/VS_SCOPE.md`, `docs/PITCH_PUBLIC.md`, `docs/TRAILER_SCRIPT.md` |
| Antela | Antela / アンテラ | player-facing | 第 1 ゾーン名。Niro の出発地点で、家・中央広場・図書館跡を含む街の一画。Provisional。 | `SPEC.md`, `docs/VS_SCOPE.md`, `docs/PITCH_PUBLIC.md`, `README.md` |
| Time Frame | Time Frame / 時の窓 / フレーム | player-facing | 空中に描く四角枠。枠内に別時間の景色を映し、境界を踏み越えて行動できる中核メカニクス。 | `SPEC.md`, `docs/VS_SCOPE.md`, `docs/PITCH_PUBLIC.md`, `docs/localization/glossary.md` |
| Timewriter | Timewriter / 時の筆 | player-facing | 時の窓を描く媒介。Stage 3 では Niro が元から持っていた古代の遺物として扱う。 | `SPEC.md`, `docs/VS_SCOPE.md`, `docs/localization/glossary.md` |
| 残響 | resonance / trace | player-facing | 前の行動・記憶・時間の痕跡として player に見える / 聞こえる手がかり。翻訳語は文脈ごとに調整。 | `docs/TRAILER_SCRIPT.md`, `SPEC.md`, `docs/PITCH_PUBLIC.md` |
| 結晶シンボル | crystal symbols / symbols | player-facing | 時の窓で選ぶ 3 つの時代選択 token。Stage 3 では赤のみ使用可、白 / 青は将来拡張の表示。 | `SPEC.md`, `docs/VS_SCOPE.md`, `docs/PITCH_PUBLIC.md` |
| 立体ジオラマ | diorama / small 3D diorama | player-facing | 時の窓の枠内に立ち上がる 3D 的な景色表現。HD-2D の depth を伝える説明語。 | `SPEC.md`, `docs/VS_SCOPE.md`, `docs/PITCH_PUBLIC.md`, `docs/TRAILER_SCRIPT.md` |
| 観測 | observation / notice / look | player-facing | Niro がフレームを開いて見る・気付く行為として使用可。ただし internal な「観測者」概念とは区別する。 | `SPEC.md`, `docs/PITCH_PUBLIC.md`, `docs/G5_MANUAL_RUBRIC.md` |

## 2. Internal 用語

CONCEPT / SPEC / ADR / 開発 doc 用。インゲーム UI、dialogue、Steam description、trailer card、public pitch には出さない。

| 用語 | 公式表記 | 区分 | 定義 | 出現 doc |
|---|---|---|---|---|
| 第 N 層 | 第 1-5 層 + 真層 | internal | 物語構造・認識更新段階を管理する設計単位。Player-facing では現象や記憶変化として表現する。 | `CONCEPT.md`, `SPEC.md`, `docs/VS_SCOPE.md` |
| 段階反転 (旧称: ベール剥離) | Veil Peeling / Staged Reveal | internal | 各段階で世界理解や主語が変わる構造の設計名。CONCEPT v1.4 以降、player-facing では使用不可。 | `CONCEPT.md`, `SPEC.md`, `docs/localization/glossary.md` |
| メタ④ | Meta 4 / メタ④ | internal | Stage 1-2 で扱ったメタ構造の内部メモ用 shorthand。Public text では説明しない。 | `CONCEPT.md`, `SPEC.md` |
| 観測者の輪廻 / 観測者磨耗 | Observer recurrence / observer erosion | internal | Player の累積観測や世界の摩耗を扱う上位物語概念。Niro の通常の観測行為とは別。 | `CONCEPT.md`, `SPEC.md`, `PITCH.md` |
| 主語の二層構造 | two-subject structure | internal | 表向きの主人公行動と、上位の観測主体を分けて扱う設計概念。 | `SPEC.md`, `CONCEPT.md` |
| インゲーム基本因果 / メタ層の真実 | in-game causality / meta truth | internal | 目の前の行動結果と、後半で開示する上位構造を分けて管理するための開発用語。 | `SPEC.md`, `CONCEPT.md` |

## 3. 設計 / 技術 用語

ADR、実装 doc、test catalog、devlog で使う技術用語。Player-facing text では原則として説明しない。

| 用語 | 公式表記 | 区分 | 定義 | 出現 doc |
|---|---|---|---|---|
| PortalStencilFeature | `PortalStencilFeature` | 設計 / 技術 | 時の窓の mask / inside 描画を扱う URP Renderer Feature。Stage 4 Phase 0 で public `RenderObjectsPass` 経路へ移行済み。 | `docs/adr/0002-time-frame-portal-stencil.md`, `docs/VS_SCOPE.md`, `docs/devlog/2026-05-05_urp_drawobjects_warning_investigation.md`, `docs/devlog/2026-05-06_urp_renderobjects_pass_migration.md` |
| stencil bit / dual-pass | stencil bit 3 / Mask 8 / Ref 8 / dual-pass | 設計 / 技術 | Portal mask / inside rendering の確定値と defense-in-depth shader 方針。 | `docs/adr/0002-time-frame-portal-stencil.md`, `docs/VS_SCOPE.md` |
| hysteresis | hysteresis | 設計 / 技術 | Portal 境界付近の往復 flip を安定させる許容帯。E4 で 0.02m 等の値を確定。 | `docs/adr/0005-time-management-scene-switching.md`, `docs/VS_SCOPE.md` |
| ActionRecord | `ActionRecord` | 設計 / 技術 | 過去 / 未来側で行った能動行動を記録し、現在側反映へ渡す data / runtime 概念。 | `SPEC.md`, `docs/VS_SCOPE.md`, `docs/localization/glossary.md` |
| SaveEnvelope / LoadFromSaveData | `SaveEnvelope` / `LoadFromSaveData` | 設計 / 技術 | Save 層の JSON envelope と runtime 復元 hook。ActionRecord persistence を含む。 | `docs/api/save_load.md`, `docs/adr/0006-save-system.md`, `docs/VERIFICATION_SUITE.md` |
| LocalizationSettings / StringTable | `LocalizationSettings` / `StringTable` | 設計 / 技術 | Unity Localization の active settings と text key table。Stage 3 で ja-JP / en seed を導入。 | `docs/adr/0008-localization.md`, `docs/api/dialogue_localization.md` |
| DialogueAsset | `DialogueAsset` | 設計 / 技術 | Unity-dependent ScriptableObject dialogue asset。`Anemora.Data` POCO と分離して runtime / UI 層で解決する。 | `docs/adr/0008-localization.md`, `docs/api/dialogue_asset_authoring.md`, `docs/VS_SCOPE.md` |

## 4. AI tool / production 用語

Asset pipeline、ledger、prompt docs、handover で使う production 用語。Public-facing text では必要な disclosure / notice 以外に詳細を出しすぎない。

| 用語 | 公式表記 | 区分 | 定義 | 出現 doc |
|---|---|---|---|---|
| PixelLab | PixelLab | production | 2D character / object sprite draft generation tool。Stage 3 では Hero / Resident draft に使用。 | `docs/adr/0009-asset-pipeline.md`, `docs/asset_prompts/hero_v1.md`, `docs/VS_SCOPE.md` |
| Meshy | Meshy v6 | production | 3D building / prop generation tool。Stage 3 A3 で Zone1 building set に使用。 | `docs/adr/0009-asset-pipeline.md`, `docs/asset_prompts/zone1_buildings.md`, `docs/VS_SCOPE.md` |
| AIVA | AIVA Pro | production | BGM skeleton / comparison source として扱う audio AI tool。Stage 3 Zone1 BGM では比較素材として不採用。 | `docs/adr/0003-asset-pipeline.md`, `docs/asset_prompts/bgm_zone1_ambient.md`, `docs/PITCH_PUBLIC.md` |
| Suno | Suno v5.5 | production | BGM mood / candidate generation tool。Stage 3 Zone1 ambient は Suno `Dustlight Piano B` 採用。 | `docs/asset_prompts/bgm_zone1_ambient.md`, `docs/VS_SCOPE.md`, `NOTICES.md` |
| ElevenLabs | ElevenLabs SFX v2 | production | SFX generation workflow。Stage 3 では Zone1 SFX 30 種の production source として tracking。 | `docs/asset_prompts/sfx_zone1.md`, `docs/VS_SCOPE.md`, `NOTICES.md` |
| Studio One | Studio One | production | BGM / SFX finishing 用 DAW。Reaper 表記から Studio One に統一済み。 | `docs/adr/0003-asset-pipeline.md`, `docs/asset_prompts/bgm_zone1_ambient.md`, `docs/asset_prompts/sfx_zone1.md` |
| Aseprite | Aseprite | production | Pixel art cleanup / palette / indexed PNG export tool。Stage 3 F2 で正式版を使用。 | `docs/adr/0009-asset-pipeline.md`, `docs/STAGE3_RETROSPECTIVE.md`, `docs/VS_SCOPE.md` |

## 5. Usage rules

- Player-facing 用語は、Steam store copy、trailer card、dialogue、UI、public pitch で使用できる。
- Internal 用語は、CONCEPT、SPEC、ADR、devlog、handover、review note で使用できる。
- Internal 用語を player-facing text に置く必要が出た場合は、現象・記憶・違和感・風景変化・手触りの表現に置き換える。
- 「観測」は player-facing では Niro が見る / 気付く行為として扱う。Internal な観測者概念と混同しない。
- Public English copy では `Time Frame` と `Timewriter` を優先する。翻訳上の例外は `docs/localization/glossary.md` に理由を残す。
- Niro / Antela は provisional 表記を保持する。Public draft で使う場合も、final lock ではないことを source doc 側に残す。
- AI tool / production 用語は、public copy では disclosure / notice / development-log 文脈に限定する。
- New term を追加する場合は、公式表記、区分、source doc、player-facing 可否を同時に書く。
- Source doc と本 glossary が矛盾した場合は、本 glossary を修正する。本 glossary は source of truth ではない。
- Stage 4 で story bible / Steam copy / localization glossary が更新されたら、本 glossary も合わせて再確認する。

## 6. Source priority

| Source | Priority in this glossary |
|---|---|
| `CONCEPT.md` | High for core concept / internal structure terms. |
| `SPEC.md` | High for mechanics, narrative structure, character / zone decisions, and open items. |
| `docs/VS_SCOPE.md` | High for Stage 3 / Stage 4 implementation boundary and player-facing restrictions. |
| `docs/PITCH_PUBLIC.md` | High for public-facing wording candidates. |
| `docs/TRAILER_SCRIPT.md` | High for trailer-facing wording and caption constraints. |
| `docs/localization/glossary.md` | High for JP / EN official wording and translation notes. |
| ADR / API docs | High for design / technical terms only. |

## 7. Update policy

- v0.1 は Stage 3 Day 1 / Stage 4 entry 時点の索引。
- v0.2 以降は、G5 manual result、Stage 4 story bible、Steam copy、localization key migration のいずれかが更新された時点で見直す。
- Public text に出せるか迷う用語は、いったん internal として扱い、public-facing へ移すには source doc 側の明示を待つ。
- Technical term の rename は、code / ADR / test catalog の rename と同じ commit または直後の documentation commit で反映する。

## 8. Revision history

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-06 | Initial glossary index for player-facing, internal, design / technical, and production terms. |
