# NOTICES

> Status: v0.2 draft (2026-05-05). This file is a public-facing summary of third-party notices for the Anemora repository. It is not a substitute for legal review before a public release.

## 1. 概要

This file summarizes third-party fonts, Unity packages, and generated-asset sources that are included in or relevant to Anemora.

- **Scope**: binaries, media assets, fonts, and runtime packages included in this repository or expected to be included in a playable Anemora build.
- **Relationship to asset ledger**: this file is consumer-facing. `docs/legal/asset_ledger.md` remains the internal per-asset tracking sheet with generation dates, prompts, source IDs, paid-plan evidence, and review status.
- **Out of scope**: development tools that are not distributed as part of the game runtime, such as Aseprite, Blender, Python, and Studio One. These are acknowledged in §5.

Anemora's own code and asset license remains All Rights Reserved by default. Stage 3 /spec resolution locked the planned public release direction to Steam Early Access, with license re-evaluation at Stage 4. See §6.

## 2. 第三者フォント

### 2.1 美咲ゴシック

- **Name**: 美咲ゴシック (`misaki_gothic.ttf`)
- **License**: free software license. The bundled Misaki documentation grants use, copy, distribution, and modification with or without commercial use, without warranty.
- **Source**: https://littlelimit.net/misaki.htm
- **Included file**: `Assets/UI/Localization/Fonts/ThirdParty/misaki_gothic.ttf`
- **Derived TMP assets**:
  - `Assets/UI/Localization/Fonts/Anemora_JP.asset`
  - `Assets/UI/Localization/Fonts/Anemora_JP_Atlas.asset`
- **License text status**: no Misaki license text file is currently committed under `Assets/UI/Localization/Fonts/ThirdParty/`. The original downloaded archive contains `misaki.txt` and `readme.txt`; `docs/legal/asset_ledger.md` records the checked license source. If this repository is made public with the TTF included, add the Misaki license text alongside the font or keep this notice updated with the exact license source.

### 2.2 Press Start 2P

- **Name**: Press Start 2P
- **License**: SIL Open Font License 1.1
- **Sources**:
  - https://fonts.google.com/specimen/Press+Start+2P
  - https://github.com/google/fonts/tree/main/ofl/pressstart2p
- **Included file**: `Assets/UI/Localization/Fonts/ThirdParty/PressStart2P-Regular.ttf`
- **License file**: `Assets/UI/Localization/Fonts/ThirdParty/PressStart2P_LICENSE.txt`
- **Derived TMP assets**:
  - `Assets/UI/Localization/Fonts/Anemora_EN.asset`
  - `Assets/UI/Localization/Fonts/Anemora_EN_Atlas.asset`

## 3. 第三者 Unity Packages

### 3.1 Unity Engine

- **Engine version**: Unity `6000.3.14f1`, from `ProjectSettings/ProjectVersion.txt`
- **License**: governed by the user's Unity plan and Unity Editor Software Terms.
- **Reference**: https://unity.com/legal/editor-terms-of-service/software
- **Note**: Anemora does not redistribute the Unity Editor. Built game releases may include Unity Runtime components under the applicable Unity terms.

### 3.2 Universal Render Pipeline (URP)

- **Package**: `com.unity.render-pipelines.universal`
- **Version**: `17.3.0`
- **License**: Unity Companion License
- **Reference**: https://unity.com/legal/licenses/unity-companion-license

### 3.3 TextMeshPro

- **Runtime use**: Anemora uses TextMeshPro font assets and `Assets/TextMesh Pro/Resources/TMP Settings.asset`.
- **Package status**: `Packages/manifest.json` and `Packages/packages-lock.json` do not currently list a standalone `com.unity.textmeshpro` package. TextMeshPro support is present through Unity/TMP project resources and Unity UI integration in this Unity version.
- **License**: Unity package/runtime terms apply to Unity-provided TextMeshPro components. Re-check this section if Unity adds a separate `com.unity.textmeshpro` package entry later.

### 3.4 Localization

- **Package**: `com.unity.localization`
- **Version**: `1.5.9`
- **License**: Unity Companion License
- **Reference**: https://unity.com/legal/licenses/unity-companion-license
- **Package third-party notices**: the local Unity PackageCache for this package includes third-party notices for components such as SmartFormat, Google APIs, and CsvHelper. Review package-level notices before a binary release.

### 3.5 Addressables

- **Package**: `com.unity.addressables`
- **Version**: `2.9.1`
- **Source in this project**: transitive dependency in `Packages/packages-lock.json`, brought in by `com.unity.localization`
- **License**: Unity Companion License
- **Reference**: https://unity.com/legal/licenses/unity-companion-license

### 3.6 その他 Unity packages

Direct dependencies from `Packages/manifest.json` are listed below. Resolved versions are from `Packages/packages-lock.json`.

| Package | Manifest version | Resolved version | Source | License / notice |
|---|---:|---:|---|---|
| `com.unity.feature.development` | `1.0.2` | `1.0.2` | builtin | Unity package terms; development feature set |
| `com.unity.ide.visualstudio` | `2.0.25` | `2.0.27` | registry | Unity Companion License; package includes MIT and Zero-Clause BSD third-party notices |
| `com.unity.inputsystem` | `1.14.0` | `1.14.0` | registry | Unity Companion License |
| `com.unity.localization` | `1.5.9` | `1.5.9` | registry | Unity Companion License; see §3.4 |
| `com.unity.nuget.newtonsoft-json` | `3.2.1` | `3.2.1` | registry | Unity Companion License package; bundled Newtonsoft-related components use MIT notices |
| `com.unity.render-pipelines.universal` | `17.3.0` | `17.3.0` | builtin | Unity Companion License; see §3.2 |
| `com.unity.test-framework` | `1.5.1` | `1.6.0` | builtin | Unity Companion License; test package, not a game runtime feature |
| `com.unity.timeline` | `1.8.7` | `1.8.7` | registry | Unity Companion License |
| `com.unity.ugui` | `2.0.0` | `2.0.0` | builtin | Unity Companion License |

Additional transitive package explicitly tracked for runtime/localization:

| Package | Resolved version | Source | License / notice |
|---|---:|---|---|
| `com.unity.addressables` | `2.9.1` | registry | Unity Companion License; see §3.5 |

`Packages/packages-lock.json` also contains Unity built-in modules and transitive packages used by the above dependencies. For a binary release, review Unity PackageCache `LICENSE.md` and third-party notice files generated by the exact Unity Editor version used for the release build.

Observed package-level third-party notices in the local PackageCache include:

- `com.unity.render-pipelines.universal`: NVIDIA FXAA3_11 notice.
- `com.unity.localization`: SmartFormat (MIT), Google APIs (Apache 2.0), and CsvHelper (MS-PL / Apache 2.0) notices.
- `com.unity.nuget.newtonsoft-json`: Newtonsoft.Json and related JSON.NET-for-Unity MIT notices.
- `com.unity.ide.visualstudio`: VSWhere (MIT) and EnvDTE (Zero-Clause BSD) notices; editor integration only.

These package-level notices are summarized here and should be rechecked against the final PackageCache before a binary release.

## 4. AI 生成アセット (商用利用 / 公開可否 notes)

Detailed per-asset provenance is tracked in `docs/legal/asset_ledger.md`. This section is a summary of generation sources and current Stage 3 Day 1 status.

| Tool / service | Current role in Anemora | License / public release status in `asset_ledger.md` |
|---|---|---|
| PixelLab | Character sprite drafts and derived finished sprites | Pixel Apprentice paid plan confirmed; generated/derived sprite assets tracked as Tier 1 player-consumed |
| Meshy v6 | Zone1 3D building meshes, textures, and Unity prefab derivatives | API key/credits confirmed; paid/premium output ownership tracked per generated asset |
| AIVA Pro | BGM comparison workflow; Stage 3 Day 1 candidate was rejected for final use | Pro plan tracked; rejected comparison material remains intermediate |
| Suno v5.5 | BGM candidate generation and selected Zone1 ambient source (`Dustlight Piano B`) | Paid plan tracked; selected BGM row recorded in `asset_ledger.md`; intermediate candidates remain ignored |
| Stable Audio 2.5 | Planned fallback/inpainting or SFX ambience support | Commercial/API status must be verified at generation time before any final asset is included |
| ElevenLabs SFX v2 | SFX generation workflow | Creator paid plan tracked; Zone1 SFX 30 generation / import is tracked in `asset_ledger.md` |
| Studio One | DAW finishing tool for BGM/SFX | Development/production tool only; final exported audio rights depend on lawful inputs and project ownership |

This repository may include AI-generated or AI-assisted assets. For exact asset IDs, prompts, selected takes, manual edits, and Steam AI disclosure classification, see `docs/legal/asset_ledger.md`.

## 5. 開発ツール (Acknowledgements)

The following tools are used to create or process assets but are not distributed as part of the Anemora runtime by this repository:

- Aseprite, owned by the user, used for pixel-art palette/indexed sprite finishing.
- Blender 4.5.5 LTS, GPL-licensed development tool, used to repair and postprocess 3D assets. Blender's GPL does not automatically license the exported FBX assets; exported asset licensing still depends on the source asset and project rights.
- Python standard library scripts under `tools/`, including Meshy API helpers and Blender automation. No committed Python dependency file currently lists Pillow or other third-party Python packages.
- Studio One, user-owned DAW license, used for audio finishing workflow planning and available for BGM/SFX finishing.

Development-tool licenses should be reviewed separately if any tool binary, plugin, or third-party Python dependency is later committed or redistributed.

## 6. Anemora 自身の license

- **Code license**: All Rights Reserved remains the current default. Stage 4 may re-evaluate, but no OSS code license is granted by this file.
- **Project-owned assets**: All Rights Reserved remains the current default for original Anemora sprites, models, audio selections, UI assets, and derivative TMP atlas assets, except where a separate third-party license expressly applies.
- **Public release direction**: Steam Early Access is the planned public release path as of the Stage 3 /spec resolution interview.
- **Current default**: Until a license is added, commercial use, redistribution, and derivative works of Anemora's original code/assets require author permission, except where a separate third-party license expressly applies.

## 7. 更新履歴

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-05 | 初版起草。Stage 3 Day 1 時点の third-party fonts, Unity packages, AI-generated asset sources, development-tool acknowledgements, and Anemora license TBD status を集約 |
| v0.2 | 2026-05-05 | Stage 3 /spec resolution を反映。Code license は All Rights Reserved 継続、public release direction は Steam Early Access 予定として記録 |
