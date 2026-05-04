# アセット法務台帳 (Asset Ledger)

> Anemora で使用するすべての AI 生成アセットの権利・出典・公開可否を記録する。
> Steam AI 開示 (Stage 5 リリース時) の申告材料として運用。
> ADR-0003 (アセットパイプライン) で運用方針を確定、本ファイルは記録責務を持つ。

> **Status**: Stage 3 Day 0 (2026-05-04) 起草、テンプレート段階。実アセット生成時に逐次追記する。
> **更新タイミング**: アセット生成直後 5 分以内 (運用負荷を分散させるため)

---

## 1. 運用方針

### 1.1 何を記録するか

各 AI 生成アセットについて、以下を記録:

- **ID** (アセット固有 ID、ファイル名と紐付け)
- **種別** (ドット絵 / 3D / BGM / SFX / UI / ストアビジュアル / ローカライズ)
- **アセット名 / ファイルパス**
- **生成日**
- **使用ツール** (PixelLab / Retro Diffusion / Meshy / AIVA / Suno / Stable Audio / ElevenLabs SFX / Adobe Firefly / Claude / 他)
- **プラン** (Free / Paid / Pro / API / on-prem 等、商用利用可否の根拠)
- **入力素材** (プロンプト / 入力画像 / 参照素材 など)
- **手修正の有無 / 内容** (Aseprite / Blender / Studio One / Photoshop / 等で何をしたか)
- **商用利用可否** (各ツールの条項に基づく判定)
- **公開可否** (GitHub Public / itch.io / Steam の段階別)
- **Steam 開示区分** (Tier 1 player-consumed / Tier 2 engineering / 開示不要)
- **備考**

### 1.2 ツールごとの権利条件 (記録時の参照)

各ツールの商用利用条項の **基準時点** (台帳記録時に再確認):

| ツール | 商用利用条件 (記録時) | 注意点 |
|---|---|---|
| PixelLab | 公開 ToS 上は生成物の商用/非商用利用可。**Tier 1: Pixel Apprentice paid 確認済み** (Paddle receipt, billing date 2026-05-04, period 2026-05-04..2026-06-03, USD 13.20 incl. tax; order suffix `...638`) → F1/G3 着手可 | 2026-05-05 Codex 確認: ToS §1.3/§3.3。API 利用は公式 API のみ、生成物で他モデルを訓練する用途は禁止/要許諾。Codex 自動生成には PixelLab API bearer token をローカル環境変数で渡す |
| Retro Diffusion (Scenario 経由) | Scenario paid plan なら商用利用可。Free plan 出力は採用しない | 2026-05-05 Codex 確認: Scenario pricing/terms。Retro Diffusion/RD Plus は Scenario 上のモデルとして確認。Aseprite 拡張版/standalone で使う場合は別途規約確認 |
| Aseprite | 自分の創作物に限り商用可 | 2026-05-05 Codex 確認: 公式 FAQ。会社利用は developer ごとに license 必要、Aseprite 本体の再配布は禁止 |
| 美咲フォント | 商用/非商用を問わず利用・複製・再配布可、改変可、無保証 | 2026-05-05 Codex 確認: `misaki_ttf_2021-05-05.zip` 同梱 `misaki.txt`。TTF 版は同梱 `readme.txt` により `misaki.txt` に従う |
| Meshy v6 | **要 plan 確認**。公式 pricing FAQ では premium plan 出力は顧客所有、free plan 出力は CC BY 4.0 と説明されている。2026-05-05 ローカル環境では `MESHY_API_KEY` 未設定のため未生成 | 生成採用前に paid/premium/API plan 状態、task id、invoice/credit 消費証跡を §3 に追記する |
| Blender | 商用利用可 (GPL) | 出力物に GPL は伝播しない |
| AIVA Pro | Pro plan でフル商用権 | プラン証跡を残す、Free は不可 |
| Suno v5.5 | **paid plan のみ commercial use rights** | Free plan 出力は採用しない |
| Stable Audio 2.5 | 要確認 (プラン / API / on-prem 別) | 2.5 の具体契約形態を生成時点で再確認 |
| ElevenLabs SFX v2 | Sound Effects Terms に従う | Voice 系条項とは別、Voice は不採用 |
| Adobe Firefly | beta なし機能は商用可、Adobe IP 補償あり | Creative Cloud 加入で利用、beta 機能は明示禁止確認 |
| Claude (Anthropic) | 出力物の利用権はユーザー帰属 (Acceptable Use Policy 遵守) | 開発支援は engineering AI tools (Tier 2) 想定 |
| Codex (OpenAI) | 出力物の利用権はユーザー帰属 (Codex 利用規約遵守) | 開発支援は engineering AI tools (Tier 2) 想定 |
| DeepL Pro | 商用利用可 (Pro plan) | ローカライズ用 |

2026-05-05 参照 URL:
PixelLab ToS `https://www.pixellab.ai/termsofservice` / PixelLab API `https://www.pixellab.ai/pixellab-api` / Scenario terms `https://www.scenario.com/terms-and-conditions` / Scenario pricing `https://www.scenario.com/pricing` / Scenario Retro Diffusion essentials `https://help.scenario.com/articles/4202673551-retro-diffusion-models-the-essentials` / Aseprite FAQ `https://www.aseprite.org/faq` / Meshy pricing `https://www.meshy.ai/pricing` / Meshy Terms `https://www.meshy.ai/terms-of-use` / Meshy API docs `https://docs.meshy.ai/en/api/quick-start` / 美咲フォント `https://littlelimit.net/misaki.htm`

### 1.3 中間ファイルの扱い

- AI 生成の中間ファイル (失敗作 / プロンプト試作 / バリエーション) は `art/_intermediate/` 等に保管
- `.gitignore` で除外し **GitHub Public には公開しない**
- 必要に応じて別途バックアップ (Stage 3-5 中)

### 1.4 公開可否の判断軸

| 段階 | 判断基準 |
|---|---|
| GitHub Public (Day 0 〜) | ソースコード + docs + 確定アセット、AI 中間ファイルは除外 |
| itch.io (VS 完成後) | ビルド + ストア素材、AI 開示文を README で明示 |
| Steam (Stage 5 オプション) | ビルド + ストア素材 + Steam Content Survey 申告 |

---

## 2. 台帳本体 (アセット記録)

### 2.1 ドット絵 (キャラクター + 重要オブジェクト)

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記) | | | | | | | | | | |
| anemora_palette_v0 | `Assets/Art/anemora_palette_v0.aseprite-palette` / `.gpl` / `.png` | 2026-05-05 | Codex + Aseprite palette format + PowerShell PNG sheet | Codex / Aseprite | ADR-0003、`STAGE3_F_PLAN.md` §4.2、F2/G3 用色要件 | 24 色ドラフトを手選定、色見本 PNG 生成 | 可 | GitHub Public 可 | Tier 2 engineering / UI foundation | ユーザー最終レビュー待ち |
| anemora_jp_tmp_font_v0 | `Assets/UI/Localization/Fonts/Anemora_JP.asset` / `Anemora_JP_Atlas.asset` / `ThirdParty/misaki_gothic.ttf` | 2026-05-05 | Unity 6000.3.14f1 TextMeshPro + 美咲ゴシック | 美咲フォント free software | JIS 第1・第2水準 kanji rows + kana/punctuation 6,734 字 | TMP 4096x4096 SDF Atlas 生成、70 字 missing を devlog 記録 | 可 | GitHub Public 可 | Tier 1 player-consumed (font asset) | フォント候補は draft。絵柄整合のユーザー判断待ち |
| hero_v1_draft_front | `Assets/Art/Sprites/Hero/v1/_draft/front_v1.png` | 2026-05-05 | PixelLab API v2 `create-image-bitforge` + Codex selection | Tier 1: Pixel Apprentice paid (Paddle receipt suffix `...638`) | `docs/asset_prompts/hero_v1.md` section 2.3 + section 3; seed 551103 | None. Awaiting F2 finish | Commercial ok (PixelLab paid) | GitHub Public ok (draft) | Tier 1 player-consumed | Test B best. Androgynous read / ordinary-person read pending user review |
| hero_v1_draft_side | `Assets/Art/Sprites/Hero/v1/_draft/side_v1.png` | 2026-05-05 | PixelLab API v2 `create-image-bitforge` + Codex selection | Tier 1: Pixel Apprentice paid (Paddle receipt suffix `...638`) | `docs/asset_prompts/hero_v1.md` section 2.2 left + section 3; seed 551213 | None. Awaiting F2 finish | Commercial ok (PixelLab paid) | GitHub Public ok (draft) | Tier 1 player-consumed | Direction-guided Test C. Identity match with front pending user review |
| hero_v1_draft_back | `Assets/Art/Sprites/Hero/v1/_draft/back_v1.png` | 2026-05-05 | PixelLab API v2 `create-image-bitforge` + Codex selection | Tier 1: Pixel Apprentice paid (Paddle receipt suffix `...638`) | `docs/asset_prompts/hero_v1.md` section 2.2 back + section 3; seed 551222 | None. Awaiting F2 finish | Commercial ok (PixelLab paid) | GitHub Public ok (draft) | Tier 1 player-consumed | Direction-guided Test C. Back view works; details pending user review |
| resident_a_v1_draft_front | `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/front_v1.png` | 2026-05-05 | PixelLab API v2 `create-image-bitforge` + Codex selection | Tier 1: Pixel Apprentice paid (Paddle receipt suffix `...638`) | `docs/asset_prompts/npc_residents.md` section 2.2 + section 4; seed 553011 | None. Awaiting G3/F2 finish | Commercial ok (PixelLab paid) | GitHub Public ok (draft) | Tier 1 player-consumed | Middle-aged to older read. Gender/age contrast pending user review |
| resident_a_v1_draft_back | `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/back_v1.png` | 2026-05-05 | PixelLab API v2 `create-image-bitforge` + Codex selection | Tier 1: Pixel Apprentice paid (Paddle receipt suffix `...638`) | `docs/asset_prompts/npc_residents.md` section 2.2 back + section 4; seed 553112 | None. Awaiting G3/F2 finish | Commercial ok (PixelLab paid) | GitHub Public ok (draft) | Tier 1 player-consumed | Back view works. Outfit continuity with front/left requires F2 adjustment |
| resident_a_v1_draft_left | `Assets/Art/Sprites/NPC/Resident_A/v1/_draft/left_v1.png` | 2026-05-05 | PixelLab API v2 `create-image-bitforge` + Codex selection | Tier 1: Pixel Apprentice paid (Paddle receipt suffix `...638`) | `docs/asset_prompts/npc_residents.md` section 2.2 left + section 4; seed 553201 | None. Awaiting G3/F2 finish | Commercial ok (PixelLab paid) | GitHub Public ok (draft) | Tier 1 player-consumed | Side view works. Ordinary resident read is strong |
| resident_b_v1_draft_seated | `Assets/Art/Sprites/NPC/Resident_B/v1/_draft/seated_v1.png` | 2026-05-05 | PixelLab API v2 `create-image-pixflux` + Codex selection | Tier 1: Pixel Apprentice paid (Paddle receipt suffix `...638`) | `docs/asset_prompts/npc_residents.md` section 3.2 + strengthened seated prompt; seed 554101 | None. Awaiting G3/F2 finish | Commercial ok (PixelLab paid) | GitHub Public ok (draft) | Tier 1 player-consumed | Bitforge seated attempts failed; selected PixFlux extra. Darker style pending user review |
| hero_v1_f2_sprite_set | `Assets/Art/Sprites/Hero/v1/hero_stand.png` / `hero_idle.png` / `hero_walk_front.png` / `hero_walk_back.png` / `hero_walk_left.png` / `hero_walk_right.png` / `hero_hands_d7.png` | 2026-05-05 | PixelLab drafts + Codex palette-v0 pixel pass | PixelLab paid + Codex | `hero_v1_draft_*` rows + `Assets/Art/anemora_palette_v0.gpl` | Aseprite CLI unavailable; palette v0 compression, local outfit/lower-body recolor, 4-frame sheets, D-7 hands asset | Commercial ok (PixelLab paid + Codex output) | GitHub Public ok (v1 draft) | Tier 1 player-consumed | All colors inside palette v0; outside palette 0. Androgynous read and direction identity pending user review |
| resident_a_v1_g3_sprite_set | `Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_idle.png` / `resident_a_walk_front.png` / `resident_a_walk_back.png` / `resident_a_walk_left.png` / `resident_a_walk_right.png` | 2026-05-05 | PixelLab drafts + Codex palette-v0 pixel pass | PixelLab paid + Codex | `resident_a_v1_draft_*` rows + `Assets/Art/anemora_palette_v0.gpl` | palette v0 compression, idle/walk 4-frame sheets, right derived from left mirror | Commercial ok (PixelLab paid + Codex output) | GitHub Public ok (v1 draft) | Tier 1 player-consumed | All colors inside palette v0; outside palette 0. Age contrast and outfit continuity pending user review |
| resident_b_v1_g3_sprite_set | `Assets/Art/Sprites/NPC/Resident_B/v1/resident_b_idle.png` | 2026-05-05 | PixelLab PixFlux draft + Codex palette-v0 pixel pass | PixelLab paid + Codex | `resident_b_v1_draft_seated` + `Assets/Art/anemora_palette_v0.gpl` | compressed PixFlux dark style into moss grey / dark trouser / weathered stone palette colors; seated idle 4-frame sheet | Commercial ok (PixelLab paid + Codex output) | GitHub Public ok (v1 draft) | Tier 1 player-consumed | Compressed from 68 colors to 7 palette-v0 colors. Darkness/style gap pending user review |

### 2.2 3D 背景 (建物 / 環境装飾)

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記) | | | | | | | | | | |
| zone1_house_player | `Assets/Art/Models/Zone1/HousePlayer/House_Player.fbx` / `Assets/Prefabs/Zone1/House_Player.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS (procedural draft; Meshy v6 not executed) | Blender GPL / Codex; Meshy API key 未設定 | `docs/asset_prompts/zone1_buildings.md` §2.2 | Blender: scale/merge/normals/UV atlas/palette/FBX; Unity: URP/Lit material + prefab | 可 (Blender output; Meshy 未使用) | GitHub Public 可; `art/_intermediate/` は除外 | Tier 1 player-consumed (AI-assisted) | 1112 tris, 512 atlas, no strange objects |
| zone1_bed_player | `Assets/Art/Models/Zone1/HousePlayer/Bed_Player.fbx` / `Assets/Prefabs/Zone1/Bed_Player.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS | Blender GPL / Codex | `zone1_buildings.md` §2.3.1 | 同上 | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 288 tris, 512 atlas |
| zone1_bookshelf_empty | `Assets/Art/Models/Zone1/HousePlayer/Bookshelf_Empty.fbx` / `Assets/Prefabs/Zone1/Bookshelf_Empty.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS | Blender GPL / Codex | `zone1_buildings.md` §2.3.2 | 同上 | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 348 tris, current empty variant |
| zone1_bookshelf_family | `Assets/Art/Models/Zone1/HousePlayer/Bookshelf_FamilyBooks.fbx` / `Assets/Prefabs/Zone1/Bookshelf_FamilyBooks.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS | Blender GPL / Codex | `zone1_buildings.md` §2.3.2 | 本 21 冊を Blender で追加、palette 統一 | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 408 tris, past/family books variant |
| zone1_table_chair | `Assets/Art/Models/Zone1/HousePlayer/Table_SmallChair_Wooden.fbx` / `Assets/Prefabs/Zone1/Table_SmallChair_Wooden.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS | Blender GPL / Codex | `zone1_buildings.md` §2.3.3 | 同上 | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 216 tris |
| zone1_door_house | `Assets/Art/Models/Zone1/HousePlayer/Door_House.fbx` / `Assets/Prefabs/Zone1/Door_House.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS | Blender GPL / Codex | `zone1_buildings.md` §2.3.4 | 同上 | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 264 tris |
| zone1_plaza_fountain_b | `Assets/Art/Models/Zone1/Plaza/Plaza_Fountain_Dry_Broken.fbx` / `Assets/Prefabs/Zone1/Plaza_Fountain_Dry_Broken.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS (Meshy v6 not executed) | Blender GPL / Codex; Meshy plan 未確認 | `zone1_buildings.md` §3.2.2 | 同上 | 可 | GitHub Public 可; A/C preview は `art/_intermediate/` 除外 | Tier 1 player-consumed (AI-assisted) | 796 tris; B 噴水跡を draft 採用、最終判断待ち |
| zone1_streetlamp | `Assets/Art/Models/Zone1/Plaza/StreetLamp.fbx` / `Assets/Prefabs/Zone1/StreetLamp.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS | Blender GPL / Codex | `zone1_buildings.md` §3.3.1 | 同上 | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 308 tris, reusable |
| zone1_tree_decay | `Assets/Art/Models/Zone1/Plaza/Tree_Decay.fbx` / `Assets/Prefabs/Zone1/Tree_Decay.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS | Blender GPL / Codex | `zone1_buildings.md` §3.3.2 | 同上 | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 736 tris; sparse leaves, decay degree review pending |
| zone1_floor_stone | `Assets/Art/Models/Zone1/Plaza/Floor_Stone.fbx` / `Assets/Prefabs/Zone1/Floor_Stone.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS | Blender GPL / Codex | `zone1_buildings.md` §3.4.1 | 同上 | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 264 tris, 2m tile |
| zone1_floor_wood | `Assets/Art/Models/Zone1/Plaza/Floor_Wood.fbx` / `Assets/Prefabs/Zone1/Floor_Wood.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS | Blender GPL / Codex | `zone1_buildings.md` §3.4.2 | 同上 | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 168 tris, 2m tile |
| zone1_library_ruin | `Assets/Art/Models/Zone1/LibraryRuin/Library_Ruin.fbx` / `Assets/Prefabs/Zone1/Library_Ruin.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS (procedural draft; Meshy v6 not executed) | Blender GPL / Codex; Meshy API key 未設定 | `zone1_buildings.md` §4.2 | Blender: collapsed roof corner, boarded windows, ivy, palette/UV/FBX; Unity prefab | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 1384 tris, no signage/ornate elements |
| zone1_library_bookshelf_past | `Assets/Art/Models/Zone1/LibraryRuin/Bookshelf_Library_Past.fbx` / `Assets/Prefabs/Zone1/Bookshelf_Library_Past.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS | Blender GPL / Codex | `zone1_buildings.md` §4.3.1 | 同上 | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 792 tris |
| zone1_book_family_past | `Assets/Art/Models/Zone1/LibraryRuin/Book_Family_Past.fbx` / `Assets/Prefabs/Zone1/Book_Family_Past.prefab` | 2026-05-05 | Codex + Blender 4.5.5 LTS | Blender GPL / Codex | `zone1_buildings.md` §4.3.2 | 同上 | 可 | GitHub Public 可 | Tier 1 player-consumed (AI-assisted) | 156 tris, interactable source |

### 2.3 BGM

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記) | | | | | | | | | | |

### 2.4 環境音 / SFX

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記) | | | | | | | | | | |

### 2.5 UI 2D / タイポグラフィ / アイコン

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記) | | | | | | | | | | |

### 2.6 ローカライズ素材 (テキスト)

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3 着手時に追記、Stage 4-5 で本格運用) | | | | | | | | | | |

### 2.7 ストアビジュアル (Steam ページ等)

| ID | アセット名 | 生成日 | ツール | プラン | 入力素材 | 手修正 | 商用可否 | 公開可否 | Steam 開示区分 | 備考 |
|---|---|---|---|---|---|---|---|---|---|---|
| (Stage 3-5 リリース判断時に追記) | | | | | | | | | | |

---

## 3. 規約変更履歴 (重要)

各 AI ツールの規約変更を Stage 3-5 中に定期チェックし、変更があれば本セクションに記録:

| 日付 | ツール | 変更内容 | 対応 |
|---|---|---|---|
| 2026-05-05 | PixelLab | Tier 1: Pixel Apprentice paid 確認済み。Paddle receipt: billing date 2026-05-04, period 2026-05-04..2026-06-03, USD 12.00 + consumption tax USD 1.20 = USD 13.20, payment PayPal, order suffix `...638` | F1/G3 着手可。Order number は公開 repo では末尾のみ記録。Codex 自動生成は `PIXELLAB_API_TOKEN` 等の bearer token 設定待ち |
| 2026-05-05 | PixelLab | F1 hero_v1 Test A / G3 NPC drafts are planned after Tier 1 paid enrollment. Receipt excerpt is recorded above; Codex still has no authenticated PixelLab UI or API bearer token in this session, so no generated files were captured. | Generate 4-8 PNG candidates from `docs/asset_prompts/hero_v1.md` §2.3 with §3 negative prompt, save them to `art/_intermediate/hero_v1_pixellab/test_a/`, then record generation IDs/seeds and selected draft rows before promoting assets to `Assets/Art/Sprites/Hero/v1/_draft/`. |
| 2026-05-05 | Meshy v6 | 公式 pricing/Terms/API docs を確認。premium plan 出力は顧客所有、free plan は CC BY 4.0 と説明。ローカル環境では `MESHY_API_KEY` / `MESHY_KEY` / `MESHY_TOKEN` 未設定、ユーザーの Meshy plan 状態は未確認 | 今回の Zone1 建物は Meshy 生成せず Blender procedural draft として記録。Meshy 出力を採用する場合は paid/premium/API plan 状態、task id、生成日時、credit 消費を本台帳へ追記してから置換する |

---

## 4. 関連文書

- `ADR-0003` (アセットパイプライン、本台帳の運用方針)
- `docs/legal/steam_ai_disclosure.md` (Steam AI 開示用文面、本台帳から提出材料を抽出)
- `PITCH.md` §8 (AI-Driven Solo Production Pipeline)
- `SPEC.md` §7 / §8 (Art / Sound)

---

## 5. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草 (テンプレート、ADR-0003 と整合) |
| v0.1 | 2026-05-05 | PixelLab / Scenario-hosted Retro Diffusion / Aseprite の公開規約確認結果を §1.2 に追記。PixelLab paid 状態は未確認として分離 |

---

## Stage 3 Day 1 Audio Draft Addendum (2026-05-05)

This addendum supplies the requested draft rows for section 2.3 BGM, section 2.4 SFX, and section 3 plan/status history. Final rows should replace `Pending` states after AIVA/Suno/ElevenLabs generation and either one-shot OGG export or Studio One export are complete.

### 2.3 BGM draft row

| ID | Asset path | Date | Tool | Plan | Input material | Manual edit | Commercial use | Public release | Steam disclosure | Notes |
|---|---|---|---|---|---|---|---|---|---|---|
| bgm_zone1_v1 | Assets/Audio/Music/Zone1_Ambient.ogg | 2026-05-05 | AIVA Pro + Suno v5.5 + Studio One | AIVA Pro / Suno paid / Studio One owned | docs/asset_prompts/bgm_zone1_ambient.md sections 2.2 and 3.2 | One-shot adoption preferred; Studio One mashup/master only if needed: -18 LUFS, HPF 40Hz, subtle reverb, limiter -1dB, OGG q6 | Pending verification after AIVA/Suno paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Draft plan row; audio not generated in this Codex session because external service authentication/download is required. |

### 2.4 SFX draft rows

| ID | Asset path | Date | Tool | Plan | Input material | Manual edit | Commercial use | Public release | Steam disclosure | Notes |
|---|---|---|---|---|---|---|---|---|---|---|
| sfx_env_wind_subtle | Assets/Audio/SFX/env/wind_subtle.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 2.1 | One-shot adoption preferred; Studio One trim/normalize/loop only if needed, HPF 40Hz, OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Draft plan row. |
| sfx_env_wind_outdoor | Assets/Audio/SFX/env/wind_outdoor.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 2.2 | One-shot adoption preferred; Studio One trim/normalize/loop only if needed, HPF 40Hz, OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Draft plan row. |
| sfx_env_distant_creak | Assets/Audio/SFX/env/distant_creak.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 2.3 | One-shot adoption preferred; Studio One trim/normalize only if needed, HPF 40Hz, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Draft plan row. |
| sfx_env_distant_water | Assets/Audio/SFX/env/distant_water.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 2.4 | One-shot adoption preferred; Studio One trim/normalize/loop only if needed, HPF 40Hz, OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Draft plan row. |
| sfx_env_bird_distant | Assets/Audio/SFX/env/bird_distant.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 2.5 | One-shot adoption preferred; Studio One trim/normalize only if needed, HPF 40Hz, mono OGG q6 | Pending user approval and ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Adoption TBD: whether birds exist in worldbuilding. |
| sfx_env_page_rustle | Assets/Audio/SFX/env/page_rustle.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 2.6 | One-shot adoption preferred; Studio One trim/normalize only if needed, HPF 40Hz, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Draft plan row. |
| sfx_footstep_stone_01 | Assets/Audio/SFX/footstep/footstep_stone_01.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.1 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Variation 1/4. |
| sfx_footstep_stone_02 | Assets/Audio/SFX/footstep/footstep_stone_02.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.1 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Variation 2/4. |
| sfx_footstep_stone_03 | Assets/Audio/SFX/footstep/footstep_stone_03.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.1 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Variation 3/4. |
| sfx_footstep_stone_04 | Assets/Audio/SFX/footstep/footstep_stone_04.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.1 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Variation 4/4. |
| sfx_footstep_wood_01 | Assets/Audio/SFX/footstep/footstep_wood_01.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.2 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Variation 1/4. |
| sfx_footstep_wood_02 | Assets/Audio/SFX/footstep/footstep_wood_02.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.2 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Variation 2/4. |
| sfx_footstep_wood_03 | Assets/Audio/SFX/footstep/footstep_wood_03.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.2 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Variation 3/4. |
| sfx_footstep_wood_04 | Assets/Audio/SFX/footstep/footstep_wood_04.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.2 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Variation 4/4. |
| sfx_footstep_grass_01 | Assets/Audio/SFX/footstep/footstep_grass_01.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.3 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending user approval and ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Adoption TBD: grass implementation. |
| sfx_footstep_grass_02 | Assets/Audio/SFX/footstep/footstep_grass_02.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.3 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending user approval and ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Adoption TBD: grass implementation. |
| sfx_footstep_grass_03 | Assets/Audio/SFX/footstep/footstep_grass_03.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.3 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending user approval and ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Adoption TBD: grass implementation. |
| sfx_footstep_grass_04 | Assets/Audio/SFX/footstep/footstep_grass_04.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 3.3 | One-shot adoption preferred; Studio One pitch/amplitude variation only if needed, mono OGG q6 | Pending user approval and ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Adoption TBD: grass implementation. |
| sfx_tf_portal_open | Assets/Audio/SFX/timeframe/portal_open.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 4.1 | One-shot adoption preferred; Studio One hall reverb/pitch envelope only if needed, mono OGG q6 | Pending user tone review and ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Tone TBD: ethereal vs mechanical. |
| sfx_tf_symbol_select | Assets/Audio/SFX/timeframe/symbol_select.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 4.2 | One-shot adoption preferred; Studio One filter variants only if needed, mono OGG q6 | Pending user tone review and ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Tone TBD: ethereal vs mechanical. |
| sfx_tf_symbol_select_disabled | Assets/Audio/SFX/timeframe/symbol_select_disabled.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 4.2 | One-shot adoption preferred; Studio One lower pitch/softer attack only if needed, mono OGG q6 | Pending user tone review and ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Tone TBD: ethereal vs mechanical. |
| sfx_tf_crossing | Assets/Audio/SFX/timeframe/crossing.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 4.3 | One-shot adoption preferred; Studio One subtle stereo spread only if needed, OGG q6 | Pending user tone review and ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Tone TBD: ethereal vs mechanical. |
| sfx_tf_take_item | Assets/Audio/SFX/timeframe/take_item.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 4.4 | One-shot adoption preferred; Studio One paper/piano layering only if needed, mono OGG q6 | Pending user tone review and ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Tone TBD: ethereal vs mechanical. |
| sfx_tf_return | Assets/Audio/SFX/timeframe/return.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 4.5 | One-shot adoption preferred; Studio One fade in/out only if needed, mono OGG q6 | Pending user tone review and ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Tone TBD: ethereal vs mechanical. |
| sfx_npc_acknowledge_a | Assets/Audio/SFX/npc/npc_acknowledge_a.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 5.1 | One-shot adoption preferred; Studio One trim/normalize only if needed; must contain no words/humming, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Resident A. |
| sfx_npc_acknowledge_b | Assets/Audio/SFX/npc/npc_acknowledge_b.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 5.1 | One-shot adoption preferred; Studio One trim/normalize only if needed; must contain no words/humming, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Resident B. |
| sfx_npc_post_reflect | Assets/Audio/SFX/npc/npc_post_reflect.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 5.2 | One-shot adoption preferred; Studio One trim/normalize only if needed; must contain no words/humming, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Draft plan row. |
| sfx_ui_focus | Assets/Audio/SFX/ui/ui_focus.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 6.1 | One-shot adoption preferred; Studio One trim/normalize only if needed, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Draft plan row. |
| sfx_ui_confirm | Assets/Audio/SFX/ui/ui_confirm.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 6.2 | One-shot adoption preferred; Studio One trim/normalize only if needed, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Draft plan row. |
| sfx_ui_cancel | Assets/Audio/SFX/ui/ui_cancel.ogg | 2026-05-05 | ElevenLabs SFX v2 + Studio One | ElevenLabs paid + Studio One owned | sfx_zone1.md 6.3 | One-shot adoption preferred; Studio One trim/normalize only if needed, mono OGG q6 | Pending verification after ElevenLabs paid generation | GitHub Public ok after final export only | Tier 1 player-consumed | Draft plan row. |

### 3 plan/status history additions

| Date | Tool | Change | Action |
|---|---|---|---|
| 2026-05-05 | AIVA Pro | Stage 3 Day 1 Zone1 BGM planned; Pro plan/license evidence and stem download not available in Codex session | Generate with docs/asset_prompts/bgm_zone1_ambient.md section 2. Prefer one-shot final output; save final/stems to audio/_intermediate/bgm_zone1_aiva/, then update final ledger row. |
| 2026-05-05 | Suno v5.5 | Stage 3 Day 1 Zone1 BGM enhancement planned; paid plan/license evidence and candidate downloads not available in Codex session | Generate 4-8 instrumental candidates with section 3.2 style prompt. Prefer one-shot final output; save candidates to audio/_intermediate/bgm_zone1_suno/, then update final ledger row. |
| 2026-05-05 | Stable Audio 2.5 | Fallback/inpainting only; not planned unless AIVA + Suno are insufficient | If used, record plan/API state and prompt/duration/key at generation time. |
| 2026-05-05 | ElevenLabs SFX v2 | Stage 3 Day 1 Zone1 SFX planned; paid plan/license evidence and generated WAV downloads not available in Codex session | Generate section 2-6 SFX candidates, save to audio/_intermediate/sfx_zone1/. Prefer one-shot OGG export; process in Studio One only when needed, then update final ledger rows. |
| 2026-05-05 | 美咲フォント | TTF 同梱 `misaki.txt` の自由利用・商用利用可条項を確認 | `anemora_jp_tmp_font_v0` の暫定フォントとして記録。最終採用はユーザーレビュー待ち。 |
