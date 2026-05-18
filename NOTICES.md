# NOTICES（第三者ライセンス通知）

> ステータス: v0.2 ドラフト（2026-05-05）。本ファイルは Anemora リポジトリの第三者通知を公開向けに要約したものです。公開前の法務レビューの代替にはなりません。

## 1. 概要

本ファイルは、Anemora に含まれる、または関連する第三者フォント、Unity パッケージ、生成アセットの出典を要約します。

- **対象範囲**: 本リポジトリに含まれる、またはプレイ可能な Anemora ビルドに含まれる見込みのバイナリ、メディアアセット、フォント、ランタイムパッケージ。
- **アセット台帳との関係**: 本ファイルは利用者向けです。`docs/legal/asset_ledger.md` は、生成日・プロンプト・出典 ID・有料プランの証跡・レビュー状況を記録する内部のアセット別追跡表です。
- **対象外**: ゲームランタイムの一部として配布されない開発ツール（Aseprite、Blender、Python、Studio One など）。これらは §5 で言及します。

Anemora 自身のコードおよびアセットのライセンスは、既定で All Rights Reserved のままです。Stage 3 の /spec resolution で公開リリース方針を Steam Early Access に確定し、ライセンスは Stage 4 で再評価します。§6 を参照。

## 2. 第三者フォント

### 2.1 美咲ゴシック

- **名称**: 美咲ゴシック (`misaki_gothic.ttf`)
- **ライセンス**: フリーソフトウェアライセンス。同梱の美咲フォント付属文書により、商用・非商用を問わず、利用・複製・再配布・改変が無保証で許諾されています。
- **出典**: https://littlelimit.net/misaki.htm
- **同梱ファイル**: `Assets/UI/Localization/Fonts/ThirdParty/misaki_gothic.ttf`
- **派生 TMP アセット**:
  - `Assets/UI/Localization/Fonts/Anemora_JP.asset`
  - `Assets/UI/Localization/Fonts/Anemora_JP_Atlas.asset`
- **ライセンス本文の状況**: 現時点で `Assets/UI/Localization/Fonts/ThirdParty/` に美咲フォントのライセンス本文ファイルはコミットされていません。ダウンロード元アーカイブには `misaki.txt` と `readme.txt` が含まれ、確認したライセンス出典は `docs/legal/asset_ledger.md` に記録しています。TTF を同梱したまま本リポジトリを公開する場合は、フォントと併せて美咲フォントのライセンス本文を追加するか、本通知に正確なライセンス出典を反映し続けてください。

### 2.2 Press Start 2P

- **名称**: Press Start 2P
- **ライセンス**: SIL Open Font License 1.1
- **出典**:
  - https://fonts.google.com/specimen/Press+Start+2P
  - https://github.com/google/fonts/tree/main/ofl/pressstart2p
- **同梱ファイル**: `Assets/UI/Localization/Fonts/ThirdParty/PressStart2P-Regular.ttf`
- **ライセンスファイル**: `Assets/UI/Localization/Fonts/ThirdParty/PressStart2P_LICENSE.txt`
- **派生 TMP アセット**:
  - `Assets/UI/Localization/Fonts/Anemora_EN.asset`
  - `Assets/UI/Localization/Fonts/Anemora_EN_Atlas.asset`

## 3. 第三者 Unity パッケージ

### 3.1 Unity エンジン

- **エンジンバージョン**: Unity `6000.3.14f1`（`ProjectSettings/ProjectVersion.txt` より）
- **ライセンス**: ユーザーの Unity プランおよび Unity Editor Software Terms に従います。
- **参照**: https://unity.com/legal/editor-terms-of-service/software
- **注記**: Anemora は Unity Editor を再配布しません。ビルドしたゲームのリリースには、適用される Unity 規約に基づき Unity Runtime コンポーネントが含まれる場合があります。

### 3.2 Universal Render Pipeline (URP)

- **パッケージ**: `com.unity.render-pipelines.universal`
- **バージョン**: `17.3.0`
- **ライセンス**: Unity Companion License
- **参照**: https://unity.com/legal/licenses/unity-companion-license

### 3.3 TextMeshPro

- **ランタイム利用**: Anemora は TextMeshPro のフォントアセットと `Assets/TextMesh Pro/Resources/TMP Settings.asset` を使用します。
- **パッケージ状況**: `Packages/manifest.json` および `Packages/packages-lock.json` には現在、単体の `com.unity.textmeshpro` パッケージは記載されていません。TextMeshPro 機能は、この Unity バージョンの Unity/TMP プロジェクトリソースおよび Unity UI 統合を通じて提供されます。
- **ライセンス**: Unity 提供の TextMeshPro コンポーネントには Unity のパッケージ / ランタイム規約が適用されます。後に Unity が単体の `com.unity.textmeshpro` パッケージ項目を追加した場合は、本節を再確認してください。

### 3.4 Localization

- **パッケージ**: `com.unity.localization`
- **バージョン**: `1.5.9`
- **ライセンス**: Unity Companion License
- **参照**: https://unity.com/legal/licenses/unity-companion-license
- **パッケージの第三者通知**: このパッケージのローカル Unity PackageCache には、SmartFormat、Google APIs、CsvHelper などのコンポーネントの第三者通知が含まれます。バイナリリリース前にパッケージ単位の通知を確認してください。

### 3.5 Addressables

- **パッケージ**: `com.unity.addressables`
- **バージョン**: `2.9.1`
- **本プロジェクトでの取り込み元**: `Packages/packages-lock.json` の推移的依存。`com.unity.localization` により取り込まれます。
- **ライセンス**: Unity Companion License
- **参照**: https://unity.com/legal/licenses/unity-companion-license

### 3.6 その他の Unity パッケージ

`Packages/manifest.json` の直接依存を以下に示します。解決済みバージョンは `Packages/packages-lock.json` に基づきます。

| パッケージ | manifest 版 | 解決版 | 取得元 | ライセンス / 通知 |
|---|---:|---:|---|---|
| `com.unity.feature.development` | `1.0.2` | `1.0.2` | builtin | Unity パッケージ規約。開発用フィーチャセット |
| `com.unity.ide.visualstudio` | `2.0.25` | `2.0.27` | registry | Unity Companion License。MIT および Zero-Clause BSD の第三者通知を含む |
| `com.unity.inputsystem` | `1.14.0` | `1.14.0` | registry | Unity Companion License |
| `com.unity.localization` | `1.5.9` | `1.5.9` | registry | Unity Companion License。§3.4 参照 |
| `com.unity.nuget.newtonsoft-json` | `3.2.1` | `3.2.1` | registry | Unity Companion License パッケージ。同梱の Newtonsoft 関連コンポーネントは MIT 通知を使用 |
| `com.unity.render-pipelines.universal` | `17.3.0` | `17.3.0` | builtin | Unity Companion License。§3.2 参照 |
| `com.unity.test-framework` | `1.5.1` | `1.6.0` | builtin | Unity Companion License。テスト用パッケージで、ゲームランタイム機能ではない |
| `com.unity.timeline` | `1.8.7` | `1.8.7` | registry | Unity Companion License |
| `com.unity.ugui` | `2.0.0` | `2.0.0` | builtin | Unity Companion License |

ランタイム / ローカライズ向けに明示的に追跡している追加の推移的パッケージ:

| パッケージ | 解決版 | 取得元 | ライセンス / 通知 |
|---|---:|---|---|
| `com.unity.addressables` | `2.9.1` | registry | Unity Companion License。§3.5 参照 |

`Packages/packages-lock.json` には、上記依存が使用する Unity 組み込みモジュールおよび推移的パッケージも含まれます。バイナリリリースに際しては、リリースビルドに使用した正確な Unity Editor バージョンが生成する Unity PackageCache の `LICENSE.md` および第三者通知ファイルを確認してください。

ローカル PackageCache で確認したパッケージ単位の第三者通知には次が含まれます:

- `com.unity.render-pipelines.universal`: NVIDIA FXAA3_11 通知。
- `com.unity.localization`: SmartFormat (MIT)、Google APIs (Apache 2.0)、CsvHelper (MS-PL / Apache 2.0) の通知。
- `com.unity.nuget.newtonsoft-json`: Newtonsoft.Json および関連する JSON.NET-for-Unity の MIT 通知。
- `com.unity.ide.visualstudio`: VSWhere (MIT) および EnvDTE (Zero-Clause BSD) の通知。エディタ統合のみ。

これらのパッケージ単位の通知はここに要約したものであり、バイナリリリース前に最終的な PackageCache と照合し直してください。

## 4. AI 生成アセット（商用利用 / 公開可否の注記）

アセット別の詳細な来歴は `docs/legal/asset_ledger.md` で追跡しています。本節は生成元と現時点（Stage 3 Day 1）の状況の要約です。

| ツール / サービス | Anemora での現在の役割 | `asset_ledger.md` 上のライセンス / 公開可否 |
|---|---|---|
| PixelLab | キャラクタースプライトのドラフトおよび派生の完成スプライト | Pixel Apprentice 有料プラン確認済み。生成 / 派生スプライトは Tier 1（プレイヤーが触れる）として追跡 |
| Meshy v6 | Zone1 の 3D 建物メッシュ、テクスチャ、Unity プレハブ派生物 | API 利用 / クレジット確認済み。有料 / premium 出力の所有権を生成アセットごとに追跡 |
| AIVA Pro | BGM 比較ワークフロー。Stage 3 Day 1 の候補は最終採用を見送り | Pro プランを追跡。不採用の比較素材は中間ファイルのまま |
| Suno v5.5 | BGM 候補生成および採用した Zone1 アンビエント音源（`Dustlight Piano B`） | 有料プランを追跡。採用 BGM の行は `asset_ledger.md` に記録。中間候補は gitignore のまま |
| Stable Audio 2.5 | フォールバック / インペイント、または SFX アンビエンス補助として予定 | 最終アセットを含める前に、生成時点で商用 / API の利用条件を確認すること |
| ElevenLabs SFX v2 | SFX 生成ワークフロー | Creator 有料プランを追跡。Zone1 の SFX 30 種の生成 / 取り込みは `asset_ledger.md` で追跡 |
| Studio One | BGM / SFX 仕上げ用の DAW | 開発 / 制作用ツールのみ。最終的な書き出し音声の権利は、適法な入力素材とプロジェクトの権利に依存 |

本リポジトリには AI 生成または AI 支援アセットが含まれる場合があります。正確なアセット ID、プロンプト、採用テイク、手作業の編集、Steam の AI 開示分類については `docs/legal/asset_ledger.md` を参照してください。

## 5. 開発ツール（謝辞）

以下のツールはアセットの作成・加工に使用しますが、本リポジトリでは Anemora ランタイムの一部として配布されません:

- Aseprite（ユーザー保有）。ピクセルアートのパレット / インデックススプライトの仕上げに使用。
- Blender 4.5.5 LTS。GPL ライセンスの開発ツールで、3D アセットの修復・後処理に使用。Blender の GPL は書き出した FBX アセットに自動的にライセンスを及ぼしません。書き出しアセットのライセンスは、元アセットとプロジェクトの権利に依存します。
- `tools/` 配下の Python 標準ライブラリスクリプト（Meshy API ヘルパーや Blender 自動化を含む）。コミット済みの Python 依存ファイルに Pillow など第三者 Python パッケージは現在記載されていません。
- Studio One（ユーザー保有の DAW ライセンス）。オーディオ仕上げワークフローの計画に使用し、BGM / SFX の仕上げに利用可能。

ツールのバイナリ、プラグイン、第三者 Python 依存を後にコミットまたは再配布する場合は、開発ツールのライセンスを別途確認してください。

## 6. Anemora 自身のライセンス

- **コードライセンス**: 現時点の既定は All Rights Reserved のままです。Stage 4 で再評価する可能性がありますが、本ファイルはいかなる OSS コードライセンスも付与しません。
- **プロジェクト所有アセット**: Anemora オリジナルのスプライト、モデル、採用音声、UI アセット、派生 TMP アトラスアセットは、第三者ライセンスが明示的に適用される場合を除き、既定で All Rights Reserved のままです。
- **公開リリース方針**: Stage 3 の /spec resolution インタビュー時点で、公開リリース経路は Steam Early Access を予定しています。
- **現在の既定**: ライセンスが追加されるまで、Anemora オリジナルのコード / アセットの商用利用、再配布、二次的著作物の作成には、第三者ライセンスが明示的に適用される場合を除き、著作者の許諾が必要です。

## 7. 更新履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v0.1 | 2026-05-05 | 初版起草。Stage 3 Day 1 時点の第三者フォント、Unity パッケージ、AI 生成アセットの出典、開発ツールの謝辞、Anemora ライセンス未定状況を集約 |
| v0.2 | 2026-05-05 | Stage 3 の /spec resolution を反映。コードライセンスは All Rights Reserved 継続、公開リリース方針は Steam Early Access 予定として記録 |
