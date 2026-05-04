# Stage 3 E トラック実装計画書 — Time Frame プロトタイプ

> ADR-0002 (URP + Stencil + Renderer Feature) を最小 Unity 実装手順に落とし込み、ADR-0005 (時間管理 / シーン切替)、ADR-0007 (UI フレームワーク)、VS_SCOPE.md §3.1 / §4.3 / §7 と接続する。
>
> 本書は **設計判断ではなく実装計画**。判断の論拠は ADR を参照する。

> **Status (2026-05-04 = Day 1 起草)**: v0.1。Phase 分解と検証マトリクスを確定、E0-E1 着手前のレビュー対象。

---

## 0. 目的とスコープ

### 0.1 E トラックで成立させるもの

VS_SCOPE.md §3.1 の **コアループ最小成立**:

1. プレイヤーが第 1 ゾーン内の所定位置で **赤シンボルを選択** → ポータルが開く
2. ポータル越しに **過去のジオラマ (3D 立体) が見える**
3. プレイヤーが **ポータル平面を越えると過去側へ踏込み**、操作系がそのまま動く
4. 過去で **能動行動を 1 種行う** (例: 本を取る) → ActionRecordEntry 生成
5. ポータル境界を **逆向きに越えて帰還**
6. 帰還後、**現在側に痕跡が反映される** (例: 本がベッドに置いてある)

### 0.2 E トラックで作らないもの

- 白 / 青シンボル (グレーアウトのみ、選択不可。VS_SCOPE §3.1)
- 時間侵食状態の発動 (機構は ADR-0005 で定義済、E では発動条件を満たさない)
- 痕跡可視化の凝った VFX (本のハイライト等。VS では「物がある / ない」程度で十分)
- 層 2 片鱗演出 (G トラック後半の 1 カット範囲)
- セーブシステム実装 (ADR-0006 の SaveEnvelope 入口は E5 で接続するが、autosave の実発火は G トラック)

### 0.3 完了条件

- 上記 §0.1 の 1 → 6 が **連続して破綻なく動く** (ノート PC TOM での 60 FPS 維持、デスクトップ UJPVOG2 で見栄え確認)
- VS_SCOPE.md §7 「FIX エリア」(時の窓ポータルシェーダ + ステンシル) が Stage 4 でも改修不要な品質に達する
- 検証マトリクス §6 の必須項目すべて pass

---

## 1. 前提資料

| 文書 | 関係 |
|---|---|
| `docs/adr/0002-time-frame-portal-stencil.md` | レンダリング設計判断 |
| `docs/adr/0005-time-management-scene-switching.md` | 主従反転 / 常駐ヒエラルキー / ActionRecord |
| `docs/adr/0007-ui-framework-ugui.md` | シンボル選択 UI、Canvas モード |
| `docs/adr/0004-project-directory-structure.md` | `Assets/Scripts/TimeManagement/`, `Assets/ScriptableObjects/ActionRecords/` 等 |
| `docs/adr/0001-engine-unity6.3-lts.md` | Unity 6000.3.14f1 + URP |
| `docs/VS_SCOPE.md` §3.1 / §4.3 / §7 | スコープ境界、FIX 領域 |
| `SPEC.md` §5.1 | 時の窓システム機能要件 |
| `docs/STAGE3_PLAN.md` §10 | ノート / デスクトップ切替 |

---

## 2. Phase 分解

### Phase E0: URP + Renderer Feature 準備

**目的**: ポータル実装の土台となる URP Pipeline Asset / Forward Renderer Data を作成し、Renderer Feature と Stencil bit 予約を調整する。

**現状 (Windows Codex `2026-05-05_urp_setup_check.md` 時点)**:
- URP package `17.3.0` 解決済 ✓
- `com.unity.collab-proxy` を manifest から除去済 (Unity VCS 不要) ✓
- `Assets/UniversalRenderPipelineGlobalSettings.asset` / `Assets/DefaultVolumeProfile.asset` 生成済 ✓
- **未着手**: `Assets/Settings/` 配下の URP Pipeline Asset + Forward Renderer Data の作成と割当
- **未着手**: `ProjectSettings/GraphicsSettings.asset` / `QualitySettings.asset` の `m_CustomRenderPipeline` 割当

**成果物**:
- `Assets/Settings/UniversalRenderPipeline.asset` (URP Pipeline Asset、Forward+)
- `Assets/Settings/UniversalRenderPipeline_Renderer.asset` (Forward Renderer Data)
- 上記 2 つを GraphicsSettings + QualitySettings 全レベルに割当
- Renderer Feature `PortalStencilFeature` のスケルトンを Forward Renderer Data に追加
- Stencil bit 予約検証結果: `bit 4` で URP 内部予約と非競合かを実機確認、競合時は bit 6 / 7 へ
- `Assets/UniversalRenderPipelineGlobalSettings.asset` / `Assets/DefaultVolumeProfile.asset` を `Assets/Settings/` 配下へ整理 (任意、ADR-0004 整合の観点で)
- 検証メモ `docs/devlog/2026-05-XX_e0_urp_pipeline_asset.md`

**手順**:
1. `Assets/Settings/` 配下に URP Pipeline Asset を作成 (Create > Rendering > URP Asset (with Universal Renderer))
2. 自動生成される Forward Renderer Data の Rendering Path を **Forward+** に設定 (ADR-0002 Decision: Forward 固定)
3. Project Settings > Graphics の `Scriptable Render Pipeline Settings` に作成した URP Asset を割当
4. Project Settings > Quality の各 Quality Level (Low/Medium/High など) に同 URP Asset を割当
5. Forward Renderer Data に Renderer Feature `PortalStencilFeature` (空スケルトン) を追加
6. Stencil bit 4 を URP の SSAO / Decal / Lighting レイヤー予約と重ならないか実機確認 (URP 17.x 予約マップを確認、`StencilUsage` enum を参照)
7. 競合があれば bit 6 / 7 を候補にスライド、確定値を本書 §5 と ADR-0002 Decision に反映

**担当**: Windows Codex (Unity Editor 操作)
**Linux 関与**: 検証メモのレビュー、Stencil bit 確定の妥当性判断、E0 後に ADR-0002 改訂が必要なら起草

---

### Phase E1: Stencil ポータル最小描画

**目的**: 「ポータル越しに別の物体が見える」最小ケースを成立させる。シーン切替も踏込みもまだやらない。

**成果物**:
- `Assets/Scripts/TimeManagement/Portal/PortalStencilFeature.cs` (Renderer Feature)
- `Assets/Art/Materials/Portal/PortalMask.shader` (HLSL カスタムシェーダ、Stencil 書き込み)
- `Assets/Art/Materials/Portal/InsideOnly.shader` (Stencil 内側のみ描画)
- `Assets/Scenes/Sandbox_E1_Stencil.unity` (検証専用シーン)

**手順**:
1. Quad メッシュをポータル平面として配置 (1 m × 2 m 程度、地面に垂直)
2. `PortalMask.shader`: ColorMask 0 + Stencil Ref 1 + Stencil Pass Replace で Stencil bit 4 に書き込む。深度テストは LessEqual、深度書込みは Off
3. `InsideOnly.shader`: Stencil Ref 1 + Stencil Comp Equal で内側のみ描画。マテリアルを単純な Lit にして可視化
4. Renderer Feature `PortalStencilFeature` を `RenderPassEvent.AfterRenderingOpaques` で挿入し、PortalMask → InsideOnly の順で描画
5. Sandbox シーンで Quad の奥に色違い Cube を 1 個置き、Quad 越しに見える / Quad 外側からは見えないことを確認

**検証**:
- Quad を回り込むと Cube が見えない (Stencil が機能している)
- Z-fighting / 縁のにじみが許容範囲内

**担当**: Windows Codex (shader / Renderer Feature 実装), Linux Claude (PR レビュー)

---

### Phase E2: 過去シーンの常駐ヒエラルキー

**目的**: ADR-0005 の `Root_Current` / `Root_Past` 常駐方式を確立。

**成果物**:
- `Assets/Scenes/Anemora_Main.unity` (1 シーン構成、Root_Current + Root_Past 常駐)
- `Assets/Scripts/TimeManagement/SceneRootRegistry.cs` (Root の登録・参照)
- レイヤー定義: `Layer_Current_Collider` (= 8), `Layer_Past_Collider` (= 9), `Layer_Current_Visual` (= 10), `Layer_Past_Visual` (= 11)
- ポータル内側カメラ `Camera_Past` (現在カメラと位置・向き・FOV 同期、culling mask = Past_Visual のみ)

**手順**:
1. `Anemora_Main` シーンに `Root_Current`, `Root_Past` を追加
2. `Root_Past` 配下に最小ジオラマ (床 + Cube 数個 + 1 NPC プレースホルダ) を配置、過去側を視覚区別する仮マテリアル (やや色温度を下げる)
3. `Camera_Past` を `Root_Past` 配下、または別 GameObject として常駐。`LateUpdate` で main camera の transform / fov / near / far を同期
4. 通常時は Root_Past を `SetActive(false)` または描画レイヤーで隠す。E1 のシェーダは Root_Past Visual layer を対象に書く

**検証**:
- ポータル展開時に Root_Past が描画される、非展開時は描画されない
- Camera_Past の同期で位置ずれがない

**担当**: Windows Codex
**Linux 関与**: SceneRootRegistry の API 設計レビュー

---

### Phase E3: シンボル選択 UI

**目的**: 赤シンボルでポータル展開トリガーを発火する UI を ADR-0007 仕様で実装。

**成果物**:
- `Assets/UI/Prefabs/SymbolWheel.prefab`
- `Assets/UI/Scripts/SymbolWheelController.cs`
- `Assets/UI/Sprites/symbol_red.png` (プレースホルダ — 仮アイコン、F トラックで本採用)
- `Assets/UI/Sprites/symbol_white_disabled.png`, `symbol_blue_disabled.png`
- Canvas: Screen Space - Camera (ADR-0007 §UI 要素別の実装方針)

**手順**:
1. SymbolWheel prefab: 3 シンボル (赤 = 中央、白 = 上、青 = 下、配置は本実装時に微調整)
2. 赤のみ Selectable、白/青は Image alpha 0.4 + raycast off でグレーアウト
3. 入力: 上下キー / ゲームパッド十字 / マウスホバーで focus 切替、決定で `OnSymbolSelected(SymbolType.Red)` を発火
4. SymbolType enum: `Red`, `White`, `Blue` (ADR-0002 / VS_SCOPE §3.1 整合)
5. 発火イベントを `TimeFramePortalController` (E4 で実装) が受信

**検証**:
- 入力ナビゲーションでキーボード / ゲームパッドが破綻しない
- グレーアウト 2 つは選択不可 (フォーカスもスキップ)

**担当**: Windows Codex (Unity uGUI / Animator)
**Linux 関与**: イベント API レビュー

---

### Phase E4: 踏込みフレーム反転

**目的**: ADR-0005 §1 の主従反転を実装。最も難度が高い Phase。

**成果物**:
- `Assets/Scripts/TimeManagement/TimeFramePortalController.cs`
- `Assets/Scripts/TimeManagement/PortalCrossingDetector.cs` (法線判定 + ヒステリシス)
- `Assets/Scripts/TimeManagement/SceneSidePolarity.cs` (主従状態を保持)
- 反転フレーム VFX: 一瞬の白フェード (URP Volume の Color Adjustments を 0.05s だけブースト)

**手順**:
1. PortalCrossingDetector:
   - ポータル平面の法線 N と Player 位置 P を毎 FixedUpdate で評価
   - 前フレームの sign(P - planeOrigin · N) と今フレームの sign が違い、かつ最小移動量 (0.05 m) を満たす場合のみ反転トリガー
   - ヒステリシス帯: ±0.02 m のバンド内では sign 変化を無視
2. SceneSidePolarity: `Current` / `Past` の enum、外部から `FlipTo(side)` で切替、変更イベント発火
3. TimeFramePortalController:
   - SymbolWheel から赤選択イベント受信 → ポータル prefab を player の正面に instantiate (固定距離 1.5 m, 固定 yaw)
   - PortalCrossingDetector を起動
   - 反転イベント受信時:
     - `SceneSidePolarity.FlipTo(opposite)`
     - Player の collision layer mask を `Layer_Current_Collider` ↔ `Layer_Past_Collider` で切替
     - Camera_Main の culling mask を反転 (Current_Visual ↔ Past_Visual)
     - PortalMask shader の Stencil 書き込み方向を反転 (Current 側 vs Past 側)
     - 反転フレームに白フェード 0.05s
4. 帰還: 同じ PortalCrossingDetector で逆方向越えを検知、再度 `FlipTo` を呼ぶ。SceneSidePolarity は Current に戻る

**検証**:
- ポータル境界の往復で擦り抜け / 詰まり / 二重反転が発生しない
- 反転後の Collider が正しい側だけ有効
- 白フェードが目立ちすぎない (空気感優先)
- 1 秒以内の演出に収まる

**担当**: Windows Codex
**Linux 関与**: 法線判定 + ヒステリシス値のレビュー、コードレビュー (PR 単位)

---

### Phase E5: ActionRecord 記録 + 痕跡反映

**目的**: 過去の能動行動 1 種を記録し、帰還時に現在側へ反映する最小ケース。

**成果物**:
- `Assets/Scripts/Data/ActionRecordEntry.cs` (POCO, ADR-0005 §6)
- `Assets/Scripts/TimeManagement/ActionRecordStore.cs` (List 保持、ADR-0005)
- `Assets/ScriptableObjects/ActionRecords/ActionRecordCatalog.asset` (静的定義、ADR-0005)
- `Assets/Scripts/TimeManagement/Reflectors/BookReflector.cs` (痕跡反映の最小実装)
- 検証用 prefab: `Assets/Prefabs/PastBook.prefab` (過去側で取れる本), `Assets/Prefabs/CurrentBookPlaceholder.prefab` (現在側で痕跡として現れる本)

**手順**:
1. ActionRecordEntry: `actionId`, `targetObjectId`, `type`, `gameTimeTicks`, `reflected` (ADR-0005 §6 のスニペットと一致)
2. ActionRecordCatalog: 「本を取る」action を 1 件登録 (`actionId = "take_book_001"`, `type = TakeItem`, `currentSideEffect = SpawnBookOnBed`)
3. ActionRecordStore: List<ActionRecordEntry>、`Add(entry)`, `GetReflected()`, `MarkReflected(id)`
4. 過去側 PastBook に Interactable コンポーネント。プレイヤー操作で `ActionRecordStore.Add(new ActionRecordEntry { actionId = "take_book_001", ... })` し、PastBook を非表示
5. 帰還トリガー (PortalCrossingDetector の Past → Current イベント) で:
   - ActionRecordStore をスキャン → reflected == false のエントリを Catalog で照合
   - BookReflector.OnReflect(entry) を呼び、Current 側 prefab `CurrentBookPlaceholder` を所定位置 (Bed の上) にインスタンス化
   - reflected = true

**検証**:
- 過去で本を取る → 帰還 → 現在のベッドに本が現れる が連続して動く
- 同じエントリが二重反映されない (reflected フラグ)
- 過去で取らずに帰還 → 現在に本は現れない

**担当**: Windows Codex (Unity 実装), Linux Claude (Entry / Store / Catalog 設計レビュー)

---

### Phase E6: 通し検証 + 検証マトリクス埋め

**目的**: §0.1 の 1 → 6 を連続で動かし、検証マトリクス §6 を埋める。

**成果物**:
- `docs/devlog/2026-05-XX_e_track_walkthrough.md` (通し検証ログ)
- `docs/devlog/2026-05-XX_e_track_verification_matrix.md` (§6 の埋め)
- 必要なら ADR-0002 / ADR-0005 への改訂提案 (本 ADR の Status を「実装で確認済」に更新)

**手順**:
1. ノート PC TOM (統合 Radeon) で Editor 起動 → 60 FPS 維持確認
2. Windows ビルド版で同シーンを再生、Editor との差を記録
3. デスクトップ UJPVOG2 (RTX 2070S) で見栄え確認 (色、影、ポータル境界の質感)
4. 検証マトリクス §6 の各項目を埋める

**担当**: Windows Codex (実機計測), Linux Claude (matrix 整理 / ADR 改訂判断)

---

## 3. 依存関係 / 並列性

```
E0 ─┐
    ├─ E1 ─┐
E2 ─┘     │
          ├─ E4 ─ E5 ─ E6
E3 ───────┘
```

- E0 / E2 / E3 は独立、並列着手可
- E1 は E0 必須
- E4 は E1 / E2 / E3 すべて必須
- E5 は E4 必須
- E6 は E5 完了後

ノート PC で E0 / E2 / E3 を進め、E4 以降は描画負荷が読めないため **デスクトップ UJPVOG2 への移動を視野に入れる** (STAGE3_PLAN §10.2 切替トリガー)。

---

## 4. ディレクトリ配置 (ADR-0004 準拠)

```
Assets/
├── Scripts/
│   ├── TimeManagement/
│   │   ├── Portal/
│   │   │   └── PortalStencilFeature.cs
│   │   ├── TimeFramePortalController.cs
│   │   ├── PortalCrossingDetector.cs
│   │   ├── SceneSidePolarity.cs
│   │   ├── SceneRootRegistry.cs
│   │   ├── ActionRecordStore.cs
│   │   └── Reflectors/
│   │       └── BookReflector.cs
│   ├── Data/
│   │   └── ActionRecordEntry.cs
│   └── UI/                    (E3 はここではなく Assets/UI/Scripts/ 側を使う、ADR-0007)
├── ScriptableObjects/
│   └── ActionRecords/
│       └── ActionRecordCatalog.asset
├── Art/
│   └── Materials/
│       └── Portal/
│           ├── PortalMask.shader
│           └── InsideOnly.shader
├── UI/
│   ├── Prefabs/
│   │   └── SymbolWheel.prefab
│   ├── Sprites/
│   │   ├── symbol_red.png
│   │   ├── symbol_white_disabled.png
│   │   └── symbol_blue_disabled.png
│   └── Scripts/
│       └── SymbolWheelController.cs
├── Prefabs/
│   ├── PastBook.prefab
│   └── CurrentBookPlaceholder.prefab
├── Scenes/
│   ├── Anemora_Main.unity
│   └── Sandbox_E1_Stencil.unity
└── Settings/
    └── (URP Renderer Data 等)
```

---

## 5. レイヤー / Stencil bit 予約表

| 領域 | 値 | 用途 |
|---|---|---|
| Layer 8 | `Layer_Current_Collider` | 現在側 collider |
| Layer 9 | `Layer_Past_Collider` | 過去側 collider |
| Layer 10 | `Layer_Current_Visual` | 現在側 visual (Camera_Main culling) |
| Layer 11 | `Layer_Past_Visual` | 過去側 visual (Camera_Main culling, Camera_Past culling) |
| Stencil bit 4 | Portal mask | E0 で URP 内部予約と非競合確認、競合時は bit 6 / 7 へ |

Layer 番号は ADR-0005 / E0 で実機確認後に確定。Unity Builtin 0-7 (Default / TransparentFX / Ignore Raycast / Water / UI 等) と衝突しない範囲を選ぶ。

---

## 6. 検証マトリクス

| ID | 項目 | 必須 | 環境 | 確認方法 |
|---|---|---|---|---|
| V1 | Stencil 描画が破綻しない (Z-fighting / 縁にじみ) | ✓ | TOM + UJPVOG2 | E1 Sandbox + 通し |
| V2 | HD-2D Tier 2 動的影と非干渉 | ✓ | TOM + UJPVOG2 | E1 + 通し |
| V3 | ポータル境界の往復で擦り抜けない | ✓ | TOM | E4 単体 + 通し |
| V4 | 反転後の collider が正しい側だけ有効 | ✓ | TOM | E4 単体 |
| V5 | ActionRecord の record / replay が成立 | ✓ | TOM | E5 通し |
| V6 | reflected フラグで二重反映が起きない | ✓ | TOM | E5 単体 |
| V7 | ノート PC TOM で 60 FPS 維持 | ✓ | TOM | E6 計測 |
| V8 | Editor とビルド版で動作差なし | ✓ | TOM | E6 |
| V9 | Camera_Past の同期で位置ずれなし | ✓ | TOM | E2 + 通し |
| V10 | シンボルグレーアウト 2 つが選択不可 | ✓ | TOM | E3 |
| V11 | 入力ナビゲーション (KB / Pad) で破綻なし | ✓ | TOM | E3 |
| V12 | 反転フレームの白フェードが派手すぎない | ✓ | UJPVOG2 | E4 + UJPVOG2 確認 |
| V13 | デスクトップ UJPVOG2 で見栄え確認 | ✓ | UJPVOG2 | E6 |
| V14 | URP Volume Profile 切替の必要性判定 | △ | UJPVOG2 | E6 (差分が小さければ不要、ADR-0002) |
| V15 | ポータル枠の AI VFX 投入余地 | △ | UJPVOG2 | E6 後段、F トラックで素材投入 |

`✓` = E トラック完了に必須。`△` = 観察項目、結果次第で次トラック持ち越し可。

---

## 7. リスクと早期検知

| リスク | 兆候 | 対応 |
|---|---|---|
| Stencil bit が URP 内部予約と衝突 | E0 で SSAO や Decal の表示崩れ | bit 4 → bit 6 → bit 7 と試行、それでもダメなら ADR-0002 改訂 (Forward 固定見直し or 別マスク方式) |
| ノート PC で 60 FPS 出ない | E1 段階で 30 FPS 切る | デスクトップ UJPVOG2 へ即移行、TOM はビルド検証用に位置づけ。STAGE3_PLAN §10.2 切替トリガーを発火 |
| 反転フレームで擦り抜け頻発 | E4 単体で再現する | ヒステリシス幅拡大、最小移動量を上げる、CharacterController を `Move` ではなく `SetPosition` 強制復元へ切替 |
| ActionRecord 二重反映 | E5 で帰還を 2 度すると本が 2 個現れる | reflected フラグの set タイミングを reflect 直前に変更、UnitTest で二重防止を担保 |
| 動的影とポータルが破綻 | E1-E2 で影が枠を貫通する / 影が消える | Forward 固定 + 予約ビット運用に寄せる、Volume Profile を分ける、それでもダメなら HD-2D Tier 1 退避 (動的影なし) を ADR-0002 改訂で記録 |
| 白フェードが空気感を壊す | E4 段階で派手に感じる | duration を 0.05s → 0.02s、強度を半減、最終的に削除も可 (反転フレーム検知できれば必須演出ではない) |

---

## 8. 完了報告のフォーマット

E トラック終了時、`docs/devlog/2026-05-XX_e_track_complete.md` に以下を記載:

```markdown
# Stage 3 E トラック完了 (YYYY-MM-DD)

## §0.1 コアループ動作確認
1. 赤シンボル選択 → ポータル展開: ✓ / × + 観察
2. ポータル越しに過去ジオラマ表示: ✓ / × + 観察
3. 踏込み: ✓ / × + 観察
4. 過去で本を取る (ActionRecord 記録): ✓ / × + 観察
5. 帰還: ✓ / × + 観察
6. 現在に本反映: ✓ / × + 観察

## 検証マトリクス §6
(V1-V15 の結果、各項目に観察コメント)

## ADR 改訂提案
- ADR-0002: (改訂が必要なら箇条書き)
- ADR-0005: (同上)

## 次トラックへの引継ぎ
- F トラック (ヒーロービジュアル) への素材要求
- G トラック (第 1 ゾーン) への配置要求
```

---

## 9. 担当割り

| Phase | Linux Claude | Windows Codex |
|---|---|---|
| E0 URP setup | 検証メモレビュー、Stencil bit 妥当性 | URP / Renderer Feature 設定、bit 競合確認 |
| E1 Stencil 最小描画 | shader / Renderer Feature の PR レビュー | shader 実装、Sandbox シーン |
| E2 常駐ヒエラルキー | SceneRootRegistry API レビュー | Unity scene 構築、Camera_Past 同期 |
| E3 シンボル UI | イベント API レビュー、a11y 観点 | uGUI prefab、Animator、入力 |
| E4 踏込み反転 | 法線判定 / ヒステリシス値レビュー、PR レビュー | C# 実装、collider / camera 切替 |
| E5 ActionRecord | Entry / Store / Catalog 設計レビュー、PR レビュー | C# 実装、Reflector、prefab |
| E6 通し検証 | matrix 整理、ADR 改訂判断 | 実機計測、ビルド検証 |

Linux Claude は **コードを書かず、設計レビューと PR レビューに徹する**。Windows Codex 実装に対するクロスモデルレビューを Codex fast (cross-model-review.sh) でも別途依頼してよい。

---

## 10. 改訂履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0.1 | 2026-05-04 | Stage 3 Day 1 起草、Phase E0-E6 + 検証マトリクス + 担当割り定義 |
| v0.2 | 2026-05-04 | Windows Codex の URP setup check 結果 (`2026-05-05_urp_setup_check.md`) を Phase E0 に反映: URP Pipeline Asset / Forward Renderer Data の作成・割当を明示、現状ステータスを記載 |
