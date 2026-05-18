# HD-2D 化 実装提案 報告書（補助 / 非実装セッション作成）

| 項目 | 内容 |
|---|---|
| 対象 | `Anemora-fast-vs-v24-sample` / Builds: `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe` |
| 環境 | URP 17.3.0 / Unity 6 系 / Forward レンダラー |
| 目標 | プロジェクト自己宣言の **HD-2D Tier 2（簡素版）**（PITCH.md §6.1 / SPEC.md） |
| 本文書の役割 | 方式調査と選択肢整理のみ。**コード変更は未実施**。判断と実装は Codex メインセッション |
| 作成方針 | 各ステップは選択肢を残し固定しない。推奨に印を付けるのみ。細部判断はメイン側 |
| 作成日 | 2026-05-18 |

> 本セッションはこの報告書以外、リポジトリ内のコード・アセット・シーンを一切変更していない。

---

## 1. HD-2D の定義と本プロジェクトのスコープ

### 1.1 一般定義

HD-2D は特定商用作品群で広く認知された表現ラベルで、半ばブランド名でもある。一般化した定義は次の組み合わせを指す。

- ドット絵（ピクセルアート）の 2D スプライトキャラクター（レトロ 16bit 風）
- フル 3D で構築された環境（実ジオメトリ）
- 強い現代的ポストプロセス（特に署名的なティルトシフト風被写界深度、HDR、ブルーム、動的ライティングと影）
- ジオラマ／絵本のような提示意図
- スプライト自身がシーンのライティングで照らされ、3D 空間に馴染んでいること

重要点として、「2Dキャラ + 3D背景」という組み合わせ自体は HD-2D ではない。同種の組み合わせは過去の商用・インディー作品にも多く存在する。HD-2D を HD-2D たらしめているのは、(a) スプライトがドット絵であること、(b) ティルトシフト DOF・ブルーム・HDR・動的ライティングという重いポスト処理一式、(c) ジオラマ的提示、の 3 点である。

### 1.2 本プロジェクトの宣言スコープ（Tier 2 簡素版）

PITCH.md §6.1 / SPEC.md より、本作は HD-2D を Tier で段階定義しており、これは妥当な切り分けである。

- **Tier 2（目標）**: ドット絵スプライト + 低ポリ3D背景 + 固定（実体は Perspective）アイソメ + 動的影 + 単一方向光 + 軽量ポスト処理（ピクセライズ + 簡素な色補正）
- **Tier 3–4（不採用と明記）**: sprite normal map、複数光源、ボリュメトリック、AAA 級ティルトシフト
- 内外対比は動的ライティングではなく、カラーグレーディング差分・アセット差分・パーティクル・音で実現する方針（PITCH §6.4）

「ジャンルタグ／目標としての HD-2D」はラベルとして妥当。ただし AAA 級ではなく簡素版である点を spec 自身が正直に宣言している。

### 1.3 現時点の結論（率直）

ドット絵スプライトであることは確認済み（プレハブ・スプライト import 設定で確認）。**構造的前提（2D+3D・ドット絵）は満たしているが、現ビルドは HD-2D とは言えない。** 不足は資産の次元構成ではなく、レンダリング／ライティング／ポスト処理層に集中している。そこが「HD-2D」という語が指している中身そのものであり、「シェーディングはこれからこだわる」と表現された部分が定義上の本体まるごとに該当する。現状は目標まで相当の距離があり、軽い調整で届くものではない。

---

## 2. 現状診断（検証済み事実に基づく）

最新ビルドのキャプチャ（`docs/devlog/screenshots/fast_vs_story_reto_shadow_20260518/`）と、コードベース実調査の対応。

| 領域 | 観測される画 | 実装上の根因（検証済み） |
|---|---|---|
| スプライト | 3D背景に貼り付いた切り抜きに見える | キャラに専用マテリアル無し。URP **Sprite-Unlit-Default**（guid `9dfc825aed78fcd4ba02077103263b40`）で光・アンビエント・GI を完全無視。`CastShadows:0` `ReceiveShadows:0` |
| ライティング | 全面均一でフラット、形がモデリングされない | シーンが Flat アンビエント（`m_AmbientMode:3`、色 `(0.30,0.30,0.34)`、強度 1）で単一方向光（白、強度 1.1、Euler ≈ 52/-35/0）を打ち消す |
| 影 | 接地影が弱く立体感が出ない | パイプライン `SoftShadowsSupported:0` でライト側 Soft 指定が事実上不発。影距離 50・1 カスケード |
| オクルージョン | 隅・接地に陰りが無くブロック然 | SSAO レンダラーフィーチャ無し（`PortalStencilFeature` のみ）。反射プローブ／ライトマップ無し |
| ポスト | 署名キューが皆無で画が死ぬ | `DefaultVolumeProfile` に 18 オーバーライドあるが大半ニュートラル。Bloom 強度 0、ColorAdjustments 全 0、Vignette 0 |
| 大気 | 奥行きが出ない | `m_Fog:0`。ParticleSystem は皆無（`FastVS_House_dust.mat` は静的、塵パーティクル未実装） |
| ピクセル整合 | スプライトと3Dの解像感が不一致 | 環境テクスチャ Bilinear+mipmap。スプライトは Point・32 PPU。フルスクリーンピクセライズ未実装 |
| 内外グレード差分 | 切替時に一瞬光るだけ | spec シグニチャーだが未実装。`PortalFlashPlayer` は一瞬の露出フラッシュのみ。持続グレード無し |

---

## 3. 守るべき前提（選択肢ではなく事実・最重要）

過去にこのプロジェクトで Apply/Integrator を通らず編集がデプロイされなかった事故がある。実装者は以下を厳守すること。

### 3.1 編集の置き場所

シーン `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` は `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs` が**完全生成**する。手編集は次の生成で消える。

| 変更対象 | 編集先（行は概算、編集前に実ファイルで確認） | デプロイ要否 |
|---|---|---|
| ライティング / アンビエント / Fog | `AnemoraFastVsHouseSliceSetup.CreateLighting()` ≈ L1422 | 再生成 + 再ビルド |
| カメラ（FOV/near/far/位置） | `AnemoraFastVsHouseSliceSetup.CreateCamera()` ≈ L1379 | 再生成 + 再ビルド |
| シーン内 Volume / パーティクル / GameObject | `AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene()` ≈ L112 | 再生成 + 再ビルド |
| URP パイプライン（影品質/距離/カスケード） | `Assets/Settings/UniversalRenderPipeline.asset` | 直接編集で乗る（再生成不要） |
| レンダラーフィーチャ（SSAO/ピクセライズ） | `Assets/Settings/UniversalRenderPipeline_Renderer.asset` | 直接編集で乗る |
| グローバルポスト | `Assets/Settings/DefaultVolumeProfile.asset` | 直接編集で乗る |
| キャラのマテリアル割当 | `Assets/Prefabs/Characters/{Hero,Resident_A,Resident_B}.prefab` の SpriteRenderer | プレハブ編集で乗る |

### 3.2 デプロイ経路

1. メニュー: `Anemora → Fast VS → Create House Slice`（`CreateHouseSliceScene`）→ `Anemora → Fast VS → Build House Slice`（`BuildHouseSlicePlayer` ≈ L224）
2. batchmode（CI/自動化）:
   ```
   <Unity 6 エディタ> -batchmode -projectPath <repo> \
     -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CreateHouseSliceScene -quit
   <Unity 6 エディタ> -batchmode -projectPath <repo> \
     -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildHouseSlicePlayer -quit
   ```
   エディタの実体パス／バージョンは `ProjectSettings/ProjectVersion.txt` で確認すること（本報告書では特定バージョンを断定しない）。

### 3.3 検証プロトコル（必須）

「Refresh のみ」で未デプロイになる事故を防ぐため、各段で次を全て通す。

1. 再生成後の `Assets/Scenes/Anemora_FastVS_HouseSlice.unity` を grep し、変更値（`m_AmbientMode`、ライト強度、追加 GameObject/Volume 等）が実際に乗っているか確認
2. `Builds/FastVS_HouseSlice/Anemora_FastVS_HouseSlice.exe` の mtime が更新されているか確認
3. 実ビルドを起動して画を目視。前ターン基準（スプライトが世界に馴染むか／形がモデリングされるか／署名ポストが出るか）で能動評価する。「壊れていない」確認では不十分

### 3.4 既知の罠

- `UniversalRenderPipeline.asset` の `SoftShadowsSupported:0` を有効化しない限り、ライト側 Soft 指定は不発。
- `RenderSettings.ambientIntensity` は Skybox モード時のみ効く。現状 Flat(mode 3) では効かないため、明るさは ambient **色** で調整する。
- DOF コンポーネントは `DefaultVolumeProfile` に Gaussian(10–30) で**既に配線済み**。強度ニュートラルなので画に出ていないだけ。判断は「効かせる／切る」の調整レベル。
- Light2D / URP 2D Renderer は不使用。Forward レンダラーのため URP「Sprite-Lit」は使えない。被ライト化は自作 3D-Lit シェーダが必要。
- スプライトは `CastShadows:0` `ReceiveShadows:0`。世界の影をスプライトに乗せたい場合は別途有効化が必要。

---

## 4. ステップ別 実装方式（選択肢は固定しない・推奨に印）

各ステップ: 現状（証拠）→ 複数案（機構・編集先・工数・トレードオフ）→ 推奨既定。推奨は出発点であり、メイン側が画を見て差し替えてよい。

### Step 1. ライティング（形を作る）

現状: Flat アンビエントが方向光を打ち消す。SSAO 無し、反射プローブ／ライトマップ無し、Forward、影距離 50・1 カスケード・2048。

| 案 | 機構 | 編集先 | 工数 | トレードオフ |
|---|---|---|---|---|
| A ★ | `ambientMode = AmbientMode.Trilight` で Sky/Equator/Ground を寒色低輝度、または Flat 維持で色を 0.4–0.5 相当へ。キー光を暖色微調整 | `CreateLighting()` | 小 | 効果最大。色設計の試行が要る |
| B | 逆方向から弱い寒色フィル光追加（Forward 追加ライト 4 まで可） | `CreateLighting()` | 小〜中 | 陰が締まる。光源管理が増える |
| C ★ | SSAO（ScreenSpaceAmbientOcclusion）レンダラーフィーチャ追加 | `_Renderer.asset` | 小 | 隅の陰りが出てブロック感減。微負荷増 |
| D ★ | `SoftShadowsSupported=1`、影距離 50→25–30 で実効解像度向上 | `UniversalRenderPipeline.asset` | 極小 | 影が締まる。遠景影が切れる点に注意 |

推奨既定: **A + C + D**（いずれも安価）。B は画を見て追加判断。

実装の形（A、Flat 低輝度の例。値は仮、メイン側調整）:
```csharp
// CreateLighting() 内
RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
RenderSettings.ambientLight = new Color(0.14f, 0.15f, 0.18f); // 旧 (0.30,0.30,0.34) を寒色低輝度へ
// 方向光: 強度はそのまま 1.1 前後、色をわずかに暖色へ
light.color = new Color(1.0f, 0.96f, 0.90f);
// Fog を使うならここで（Step5 D と同経路）
RenderSettings.fog = true;
RenderSettings.fogMode = FogMode.Linear;
RenderSettings.fogColor = new Color(0.16f, 0.17f, 0.20f);
RenderSettings.fogStartDistance = 20f; RenderSettings.fogEndDistance = 90f;
```

### Step 2. スプライトを世界に入れる（視覚最重要・現状ゼロ）

現状: Hero/Resident は `SpriteRenderer + Animator + HeroAnimatorBinder`。マテリアルは URP Sprite-Unlit-Default。billboard は Binder が LateUpdate で rotation を identity 固定 + `flipX`。スプライト import は Point・32 PPU・pivot(0.5,0)・mip 無し。Light2D/2D Renderer 不使用。既存の Lit スプライト／リム／ブロブ影シェーダは無い（前例は Portal の自作 .shader 群）。

| 案 | 機構 | 編集先 | 工数 | トレードオフ |
|---|---|---|---|---|
| A ★ | 自作 3D-Lit スプライトシェーダ。ノーマル無しフラット着色で Tier 2 整合 | 新規 .shader/.shadergraph + プレハブ割当 | 中 | 効果最大。シェーダ作成が要る |
| C | グローバル既定スプライトマテリアル差し替え（`m_DefaultSpriteMaterial`） | `UniversalRenderPipelineGlobalSettings.asset` | 小 | 楽だが UI スプライトにも波及。プレハブ単位割当を推奨 |
| 影① ★ | 足元ソフトブロブ quad | プレハブ／生成スクリプト | 小 | 最小。光方向に追従しない |
| 影② | ライト方向に歪ませた疑似影 quad | 同上 | 中 | 方向整合。実装やや増 |
| 影③ | 簡易シャドウキャスタ代理メッシュ | 同上 | 中〜大 | 最も自然。Tier 2 では過剰気味 |

推奨既定: **A + 影①（または②）**。割当ルートはプレハブ直割当（確実にビルドに乗る）を第一に、`HeroAnimatorBinder` 実行時割当でも可。**Step2(A) が未達だと他を磨いても HD-2D に読めない単一最重要項目。**

実装の形（A、被ライト化シェーダの骨子。ノーマル不使用のフラット着色）:
```hlsl
// 擬似コード。実装は ShaderGraph でも手書き HLSL でも可
// 透明カットアウト + メインライト色 + SH アンビエント
half4 albedo = tex2D(_MainTex, uv) * _Color;
clip(albedo.a - _Cutoff);
half3 sh      = SampleSH(half3(0,0,1));               // フラットなアンビエント
Light main    = GetMainLight(shadowCoord);            // shadowAtten 込み
half  wrap    = saturate(0.5 + 0.5 * main.distanceAttenuation); // ハーフランバート的ラップ
half3 lit     = albedo.rgb * (sh + main.color * main.shadowAttenuation * wrap);
return half4(lit, albedo.a);
// _PixelSnap / Point フィルタは維持（ドット整合）
```
注意: `ReceiveShadows` をプレハブで有効化しないと世界の影がスプライトに落ちない。Tier 2 ではブロブ影だけでも成立する。

### Step 3. ポストプロセス（仕上げ）

現状: `DefaultVolumeProfile` は 18 オーバーライド configured だが大半ニュートラル（Bloom 強度 0/threshold 0.9、Tonemapping Neutral、ColorAdjustments 全 0、Vignette 0、DOF Gaussian 配線済み無効）。シーン固有 Volume 無し（グローバル依存）。`PortalFlash_VolumeProfile` は空。内外持続グレードは未実装。

| 案 | 機構 | 編集先 | 工数 | トレードオフ |
|---|---|---|---|---|
| A ★ | DefaultVolumeProfile を実値化（Bloom 弱・ColorAdjustments・Vignette 微弱） | `DefaultVolumeProfile.asset` | 小 | 効果大。直接編集で乗る |
| B | シーン固有 Global Volume を生成しグローバルと分離 | `CreateHouseSliceScene()` | 中 | 内外差分の土台。手追加は不発 |
| C | 内外グレード差分（spec シグニチャー）。side 切替点で 2 プロファイル weight ブレンド | `PortalVisualSwitcher.ApplyForSide()` / `TimeFramePortalController`、雛形は `PortalFlashPlayer.cs` | 中 | 体験の核心。接続箇所の理解が要る |
| DOF | 既配線 Gaussian を弱く効かせる／0 のまま | `DefaultVolumeProfile.asset` | 極小 | 古典 HD-2D の最強キュー。書面 Tier 2 を僅かに超える |

推奨既定: **A 先行 → 余力で B+C を内外メカニクスへ接続**。DOF はメイン側が A 適用後の画で判断（本報告書では断定しない）。

### Step 4. ピクセライズ / ピクセル整合（spec signature・未実装）

現状: ピクセライズ無し。前例 `Assets/Scripts/TimeManagement/Portal/PortalStencilFeature.cs`（ScriptableRendererFeature、`_Renderer.asset` の `m_RendererFeatures` 登録、RenderObjectsPass + 独自シェーダタグ）。環境テクスチャ Bilinear+mip でドットスプライトと衝突。

| 案 | 機構 | 編集先 | 工数 | トレードオフ |
|---|---|---|---|---|
| A ★ | URP17 内蔵 Full Screen Pass Renderer Feature + 量子化/ダウンサンプル ShaderGraph | `_Renderer.asset` | 小〜中 | 最小実装。グリッド制御は弱め |
| C | 自作 ScriptableRendererFeature（PortalStencilFeature 流儀） | 新規 .cs + `_Renderer.asset` | 中 | グリッド・適用範囲を制御。工数増 |
| Tex | ピクセル表現対象テクスチャ `.meta` を Point・mip 抑制 | テクスチャ import | 小（対象多） | 解像感統一。一括処理が要る |

推奨既定: **A で全体トーンを掴み、品質を詰める段で C へ格上げ**。Tex は並行。**Step6 の PixelPerfectCamera とは二重適用で崩れるため排他**。どちらか一方に決めること。

### Step 5. 環境マテリアル/テクスチャ/大気（最大工数・最終品質律速）

現状: 7/8 が URP Lit、metallic 0・smoothness 0.5（やや光沢）、テクスチャ付き Bilinear+mip。1/8 がカスタムシェーダ（bookshelf）。ParticleSystem 皆無。

| 案 | 機構 | 編集先 | 工数 | トレードオフ |
|---|---|---|---|---|
| A ★ | smoothness 0.5→0.1 前後でマット化、必要箇所のみ微スペキュラ | 各 .mat | 小 | ブロック感低減。一括調整が要る |
| D ★ | 弱い色付き Linear Fog（Step1 と同経路） | `CreateLighting()` | 極小 | 奥行き寄与。グレードと色を合わせる |
| C | 内=光の塵+煙 / 外=枯葉+砂埃 を ParticleSystem 化 | `CreateHouseSliceScene()` | 中 | ジオラマ感大。生成経路に乗せる必要 |
| B | プレースホルダ反復テクスチャ差し替え・タイリング崩し | テクスチャ/.mat | 大 | 最終品質ゲート。polish フェーズ継続 |

推奨既定: **A + D + C を先に（安価でジオラマ化）**、B は polish 継続。

### Step 6. カメラ / ジオラマ構図

現状: Perspective FOV 38、near 0.03/far 140、固定（Cinemachine/PixelPerfect 無し）、`CreateCamera()` 生成。Perspective ゆえミニチュア感は出しやすい。

| 案 | 機構 | 編集先 | 工数 | トレードオフ |
|---|---|---|---|---|
| A ★ | 現状維持 + Step3 弱 DOF でミニチュア感 | なし | 極小 | 追加実装ほぼ不要 |
| B ★ | FOV を 30–38 で振りジオラマスケール最適化 | `CreateCamera()` | 極小 | 箱庭感調整 |
| C | PixelPerfectCamera 導入 | `CreateCamera()` | 中 | Step4 ピクセライズと排他 |

推奨既定: **A + B**。C は Step4 方式を決めてから。

---

## 5. 推奨シーケンスと検証ゲート（範囲はメイン側が選ぶ・固定しない）

1. **80/20 の山（ブロックアウト→HD-2D に読める最短）**: Step1(A+C+D) → Step2(A+接地影) → Step3(A)。1 シーンで効果確認。
2. **spec シグニチャー**: Step4(A) と Step3(B+C 内外差分)。
3. **polish**: Step5(B)、Step3 DOF 微調整、Step6 微調整。

各段の終わりに §3.3 の検証プロトコル（資産 grep + exe mtime + 画の能動目視）を必ず通す。Step2(A) の被ライト化が未達のまま他を進めても HD-2D には読めない。

---

## 6. メイン側に委ねる未決事項

- DOF を効かせるか（既配線・低リスク調整）。
- ピクセライズ方式（URP17 内蔵 FullScreenPass / 自作 RendererFeature / レンダースケール）と PixelPerfectCamera の排他選択。
- ambient を Flat 低輝度にするか Trilight にするか。
- 目標到達度の野心レベル（80/20 で止めるか spec シグニチャーまで進むか）。
- 各案の具体値（ambient 色、Bloom 強度、Fog 距離、FOV 等）は本報告書の数値を出発点とし、画を見て確定。

---

## 付録 A. 検証済み事実インベントリ

実装者が再確認できるよう、調査で確定した生事実を列挙する。

### A.1 シーン生成・ビルド
- 生成スクリプト: `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- 主要メソッド（行は概算）: `CreateHouseSliceScene()`≈L112 / `ValidateHouseSliceBatch()`≈L146 / `BuildHouseSlicePlayer()`≈L224 / `CreateCamera()`≈L1379 / `CreateLighting()`≈L1422
- 定数: `BuildDirectory = "Builds/FastVS_HouseSlice"`、`BuildExePath = BuildDirectory + "/Anemora_FastVS_HouseSlice.exe"`

### A.2 ライティング（生成後シーン値）
- `m_AmbientMode: 3`（Flat）、`m_AmbientSkyColor: (0.3,0.3,0.34)`、`m_AmbientIntensity: 1`、`m_Fog: 0`
- 方向光: 白、`m_Intensity: 1.1`、Euler ≈ (52, -35, 0)、影 Soft（`m_Strength: 1`）、ShadowNearPlane 0.2、Bias 0.05/NormalBias 0.4
- 反射プローブ・ライトマップ無し

### A.3 URP パイプライン / レンダラー
- `UniversalRenderPipeline.asset`: Forward（`m_RendererType:1`）、影距離 50、カスケード 1、メイン影 2048、`SoftShadowsSupported:0`、MSAA 1x、HDR 有効、追加ライト上限 4
- `UniversalRenderPipeline_Renderer.asset`: Forward（`m_RenderingMode:2`）、レンダラーフィーチャは `PortalStencilFeature` のみ（`Anemora.TimeManagement.Portal.PortalStencilFeature`、passEvent 300）

### A.4 カメラ
- "Main Camera": Perspective、FOV 38、near 0.03、far 140、背景 (0.075,0.078,0.084)、位置 (-9.25, 2.78, -13.5)、Cinemachine/PixelPerfect 無し

### A.5 スプライト
- プレハブ: `Assets/Prefabs/Characters/{Hero,Resident_A,Resident_B}.prefab`、構成 `SpriteRenderer + Animator + HeroAnimatorBinder`
- マテリアル guid `9dfc825aed78fcd4ba02077103263b40` = URP Sprite-Unlit-Default（`UniversalRenderPipelineGlobalSettings.asset` の `m_DefaultSpriteMaterial`/`m_DefaultUnlitMaterial` と同一）
- `CastShadows:0` `ReceiveShadows:0`、SortingOrder Hero 10 / Resident 5
- billboard: `HeroAnimatorBinder` が LateUpdate で `transform.rotation = Quaternion.identity`、`spriteRenderer.flipX` を移動方向で切替
- import: filterMode 0(Point)、spritePixelsToUnits 32、pivot (0.5,0)、mip 無し、非圧縮 RGBA、グリッド 4 フレーム、Animator `Assets/Animators/HeroLocomotion.controller`（params `isMoving`/`facing`）
- Light2D / 2D Renderer 不使用。既存の Lit スプライト/リム/ブロブ影シェーダ無し

### A.6 ポスト / レンダラーフィーチャ前例 / 環境
- `DefaultVolumeProfile.asset`: 18 オーバーライド active。Bloom intensity 0 / threshold 0.9 / scatter 0.7、Tonemapping Neutral、ColorAdjustments 全 0、Vignette 0、FilmGrain 0、MotionBlur 0、ScreenSpaceLensFlare 0、DepthOfField Gaussian(start10/end30/maxRadius1) 配線済み無効
- `PortalFlash_VolumeProfile.asset`: 空（`components: []`）
- シーン固有 Volume / Global Volume 無し（グローバル依存）
- レンダラーフィーチャ前例: `Assets/Scripts/TimeManagement/Portal/PortalStencilFeature.cs`（StencilBit 3、RenderObjectsPass×2、シェーダタグ `AnemoraPortalMask`/`AnemoraPortalInside`）
- 環境マテリアル: `Assets/Art/Materials/FastVS/HouseSlice/` の 7/8 が URP Lit（guid `933532a4fcc9baf4fa0491de14d08ed7`、metallic 0、smoothness 0.5、テクスチャ付き）、1/8 カスタムシェーダ
- 環境テクスチャ import: filterMode 1(Bilinear)、mipmap 有効、wrap Clamp、sRGB
- ParticleSystem: プロジェクト内皆無。`FastVS_House_dust.mat` は静的
- 内外グレード: `PortalVisualSwitcher`（cullingMask/レイヤー/ステンシルのみ）、`PortalFlashPlayer`（一瞬の postExposure 2.5 フラッシュのみ）。持続グレード未実装

### A.7 カスタムシェーダ既存
- `Assets/Art/Materials/Portal/PortalMask.shader` / `InsideOnly.shader` / `PortalApertureOverlay.shader`

---

*本報告書は補助セッションが調査・整理のみを行い作成。リポジトリのコード・アセット・シーンは本ファイル以外変更していない。実装判断と作業は Codex メインセッションが行う。*
