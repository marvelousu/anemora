# Zone 1 Buildings v1 Generation Prompt Template (Meshy v6 + Blender)

> G1 (主人公の家) / G2 (中央広場 + 図書館跡) で使用する 3D 背景アセットの生成プロンプトテンプレート。
> ADR-0003 (アセットパイプライン) §Decision に従い、**Meshy v6 LowPoly Mode** で初速生成 → **Blender 4.5 LTS** で破綻補正・パレット統合 → Unity 取込み。

> **Status (2026-05-04)**: v0 起草。実生成前の抽象プロンプト。Meshy 試行後に v1 改訂。

---

## 1. 共通方針

### 1.1 美術整合 (ADR-0003 / VS_SCOPE §4.2)

- HD-2D Tier 2 簡素版: 動的影 + 単一方向光に乗る LowPoly モデル
- パレット: Anemora パレット v0 (主人公 / NPC と共通)
- 異物原則: 異界アイテム / 機械 / 古代紋様 / 浮遊物を入れない、衰退した普通の街
- 静謐 / 衰退: 派手な装飾を避け、塀の崩れ / 樹の落葉 / 灯りの消えた窓で表現
- 新規 2-3 棟、再利用前提 (向き / テクスチャ / スケール変化で構成)

### 1.2 出力規格

- **ポリゴン目安**: 1 棟あたり 500-1500 triangles (建物本体)、装飾込みで 2000 上限
- **テクスチャ**: 512x512 or 1024x1024 単一 atlas、PBR 不採用 (HD-2D Tier 2 = 単一方向光のため)
- **形式**: FBX export → Unity import → URP/Lit (Tier 2 用) マテリアル割当
- **スケール**: 1 unit = 1 m (ゲーム世界座標と一致)

### 1.3 ワークフロー

1. Meshy v6 LowPoly でテキストプロンプトから生成 (4-8 候補)
2. ベストを `art/_intermediate/zone1_meshy/{building}/` にダウンロード (gitignore)
3. Blender 4.5 LTS で開き、以下を補正:
   - スケール正規化 (1 m 基準)
   - 破綻面 / 不要 vertex 削除
   - UV 整理 (Unity import で読みやすい配置)
   - パレット統一 (texture を Anemora パレット v0 内に再着色 or vertex color)
   - 動的影が乗る outline を確保 (アウトライン強め、エッジ角度を整理)
4. FBX export → `Assets/Art/Models/Zone1/{building}/` (ADR-0004 §Decision)
5. Unity Editor で Material 適用、prefab 化

---

## 2. House_Player: 主人公の家

### 2.1 設定

- 木造、小さい (約 4m × 4m × 3m)
- **窓なし**、または塞がれた窓 (`STAGE3_TBD_RESOLUTION.md` §4.1 閉塞感)
- ドア 1 つ (出口)
- 屋根は傾斜、瓦 or 木板
- 外観は「使われている家」(廃墟ではない、住人が暮らしている)
- 内装は別 prefab (Bed / Bookshelf / Table、後段で Meshy で個別生成)

### 2.2 Meshy Prompt (外観)

```
Low poly 3D model of a small simple wooden house, single story, about 4 by 4 by 3
meters, sloped wooden roof, wooden plank walls in muted weathered brown, single
front door made of darker wood, no visible windows or windows boarded up with planks,
gentle weathering on the roof and walls, simple stone foundation, no decorations,
HD-2D inspired flat shaded style, suitable for a quiet melancholic village setting,
clean topology, 800 to 1200 triangles, single texture atlas, no PBR materials, no
metallic or glossy surfaces.
```

### 2.3 Meshy Prompt (内装パーツ別)

#### 2.3.1 Bed_Player

```
Low poly 3D model of a simple single bed, wooden frame in dark brown, plain beige
linen blanket and a single small pillow, slightly disheveled (someone just got up),
small (1.0 by 2.0 by 0.5 meters), HD-2D inspired flat shaded style, no decorations,
clean topology, 200 to 400 triangles.
```

#### 2.3.2 Bookshelf_Empty (現在版) / Bookshelf_FamilyBooks (過去版)

```
Low poly 3D model of a small wooden bookshelf, three shelves, dark walnut wood, about
1.0 by 1.5 by 0.3 meters, plain design, HD-2D inspired flat shaded style, clean
topology, 300 to 500 triangles.
```

過去版は Blender で本 (5-8 冊、色違い) を追加、または別 prefab `Books_Family_Past.prefab` として overlay。

#### 2.3.3 Table_Small + Chair_Wooden

```
Low poly 3D model of a small wooden dining table for two, square top about 0.8 by
0.8 meters, height 0.75 meters, four simple legs, dark wood. Pair with a low poly
3D model of a simple wooden chair, no cushion, straight back, same wood tone.
HD-2D inspired flat shaded style, clean topology.
```

#### 2.3.4 Door_House

```
Low poly 3D model of a simple wooden plank door, vertical planks in weathered dark
brown wood, simple iron hinges and a small iron handle, about 0.9 by 2.0 meters,
HD-2D inspired flat shaded style, clean topology, 200 to 300 triangles.
```

---

## 3. Plaza_Center: 中央広場

### 3.1 設定

- 中心に「失われた何か」のモニュメント (ベンチ / 噴水跡 / 廃墟風モニュメント)
- 石畳の床、街灯 2-3 本、樹 (落葉中) 2-3 本
- VS では「失われた本のあった場所」が広場の一角または広場すぐ脇

### 3.2 Meshy Prompt (中央モニュメント候補 3 案)

#### 3.2.1 案 A: ベンチ (シンプル)

```
Low poly 3D model of a weathered stone bench, single piece, about 2.0 by 0.5 by 0.5
meters, slightly chipped edges, mossy patches in dark green, HD-2D inspired flat
shaded style, suitable for a quiet melancholic plaza, clean topology, 300 to 500
triangles.
```

#### 3.2.2 案 B: 噴水跡 (より物語性)

```
Low poly 3D model of a small abandoned stone fountain, circular basin about 2 meters
diameter, no water, dry cracked stone bottom, central pillar broken at the top, no
ornate decoration, weathered with subtle moss, HD-2D inspired flat shaded style,
clean topology, 600 to 900 triangles.
```

#### 3.2.3 案 C: 廃モニュメント (最も曖昧)

```
Low poly 3D model of an indeterminate stone monument or pedestal, square base 1 by
1 meter, broken column on top about 1.5 meters tall (top portion missing), plain
weathered stone, no inscriptions or decorations visible, HD-2D inspired flat shaded
style, clean topology, 400 to 600 triangles.
```

ユーザー判断で 1 案採用 (推奨: B 噴水跡、最も衰退と日常の重なりが出る)。

### 3.3 Meshy Prompt (StreetLamp / Tree_Decay)

#### 3.3.1 StreetLamp

```
Low poly 3D model of a simple wrought iron street lamp, total height 3 meters, single
post with a small lantern at the top, lantern unlit / glass pane slightly cracked,
dark iron color, HD-2D inspired flat shaded style, clean topology, 200 to 400
triangles. Designed to be reused multiple times across a small plaza.
```

#### 3.3.2 Tree_Decay (落葉中)

```
Low poly 3D model of a small leafless or sparsely leafed tree, about 4 meters tall,
slim trunk with a few twisted branches, very few leaves remaining (autumn / decay
look), brown-grey bark, faded yellow-brown sparse leaves, HD-2D inspired flat shaded
style, clean topology, 600 to 1000 triangles. Designed to be reused multiple times.
```

### 3.4 床タイル (single mesh tile を Unity 側で繰り返し)

#### 3.4.1 Floor_Stone (中央広場)

```
Low poly 3D model of a 2 by 2 meter stone tile patch, slightly uneven cobblestones,
weathered grey-brown, mossy seams, HD-2D inspired flat shaded style, tileable from
all four sides, 200 to 400 triangles.
```

#### 3.4.2 Floor_Wood (家の中)

```
Low poly 3D model of a 2 by 2 meter wooden plank floor patch, dark wood planks
running parallel, slightly worn surface, HD-2D inspired flat shaded style, tileable
from all four sides, 100 to 200 triangles.
```

---

## 4. Library_Ruin: 図書館跡 (主要違和感の場所)

### 4.1 設定

- 小さい (中央広場の建物より少し大きい程度、約 6m × 6m × 5m)
- 廃墟感: ドア固く閉ざされ、屋根の一部が壊れている、植物が壁を這う
- 内部 (過去版用) は別 prefab: 本棚 + 床に散らばる本 + 中央テーブル
- 主要違和感の出所 (家族の本がここにあった)

### 4.2 Meshy Prompt (外観)

```
Low poly 3D model of a small abandoned stone library building, single story, about 6
by 6 by 5 meters, weathered light grey stone walls, wooden roof partially collapsed
on one corner, single closed wooden double-door entrance, two boarded-up windows on
the front, ivy or creeping plants partially covering the walls, no signage visible,
overall feeling of a long-closed quiet building, HD-2D inspired flat shaded style,
suitable for a melancholic abandoned library setting, clean topology, 1000 to 1500
triangles, single texture atlas.
```

### 4.3 Meshy Prompt (過去版内装、Library_Ruin_Past 用)

#### 4.3.1 Bookshelf_Library_Past (再利用)

```
Low poly 3D model of a tall wooden library bookshelf, six shelves, dark wood, about
1.0 by 2.5 by 0.4 meters, filled with books of various sizes and faded earth-tone
colors (ochre, dark green, dark red, beige), HD-2D inspired flat shaded style, clean
topology, 700 to 1100 triangles.
```

複数本配置で図書館の内装を構築。

#### 4.3.2 Book_Family_Past (Interactable)

```
Low poly 3D model of a single old hardcover book, dark brown leather cover with
faded gold-trim spine, about 0.2 by 0.3 by 0.05 meters, lying flat or standing on a
shelf, HD-2D inspired flat shaded style, clean topology, 100 to 200 triangles.
```

E5 で `Book_Family_Past.prefab` (過去で取得可能) と `Book_Family_Current.prefab` (帰還後ベッドに出現) の両方に使用、テクスチャは同じ。

---

## 5. House_Near (Stage 4 持ち越し candidate)

VS では Blockout 段階で止め、外壁テクスチャだけ House_Player の variation で代用:

```
Low poly 3D model of a small simple wooden house, single story, about 5 by 5 by 3.5
meters, similar style to House_Player but with a slightly different roof slope and
wall plank pattern, single front door, two small windows, weathered look, HD-2D
inspired flat shaded style, clean topology, 800 to 1200 triangles.
```

VS は Cube プレースホルダ + House_Player のマテリアル使い回しで OK、Stage 4 で本実装。

---

## 6. MeetingHall_Ruin (Stage 4 持ち越し candidate)

同上、VS では Blockout のみ。詳細プロンプトは Stage 4 で起草。

---

## 7. Negative Prompt (全建物共通)

```
NOT: futuristic technology, glowing runes, magical orbs, ornate gothic architecture,
oversized scale, fantasy castle, cathedral, temple, religious symbols, gold-trimmed
decorations, mechanical parts, cybernetic elements, neon lights, vibrant unnatural
colors, photorealistic textures, PBR metallic surfaces, smoke effects, particle
effects, anime-style cute exaggeration, super-deformed proportions.
```

---

## 8. Blender 仕上げチェックリスト (各 Meshy 出力に共通)

1. **スケール**: Meshy 出力のスケールは bounding box で 1 m 換算に正規化
2. **重複 vertex**: Merge by Distance (0.001 m)
3. **法線**: Recalculate Outside、negative scale を解消
4. **UV**: Unwrap またはスマート UV でテクスチャマップを再構築
5. **パレット統合**: テクスチャの色相 / 彩度を Anemora パレット v0 に揃える (Photoshop or Blender Compositor)
6. **ポリゴン数**: 各セクションの目安に収まっているか確認 (Stats viewport で確認)
7. **エクスポート**: FBX、Apply Transform=Yes、Smoothing=Edge、Path Mode=Copy
8. **Unity import 後**: Material を URP/Lit に変更、Pixel Per Unit 適用、prefab 化

---

## 9. ユーザー判断ポイント

- **Plaza_Center モニュメント**: §3.2 案 A/B/C のどれを採用するか
- **Tree_Decay の落葉度**: §3.3.2 を「ほぼ枯木 / 半分散った / 落葉直前」のどれにするか
- **House_Player 内装の規模**: VS で必要な家具を Bed / Bookshelf / Door に絞るか、Table+Chair まで含めるか

---

## 10. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0 | 2026-05-04 | 初版起草。House_Player + Plaza_Center + Library_Ruin の Meshy プロンプト + Blender 仕上げ手順 |
