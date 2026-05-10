# 2026-05-05 TMP English Atlas v0 Draft

## Status

Draft. This prepares the Stage 4 entry English TMP Font Asset ahead of localization wiring. The adopted candidate is still pending user art-direction review.

## Outputs

- `Assets/UI/Localization/Fonts/Anemora_EN.asset`
- `Assets/UI/Localization/Fonts/Anemora_EN_Atlas.asset`
- `Assets/UI/Localization/Fonts/ThirdParty/PressStart2P-Regular.ttf`
- `Assets/UI/Localization/Fonts/ThirdParty/PressStart2P_LICENSE.txt`

## Font Candidate

Draft font: Press Start 2P.

Rationale:

- Commercial-use compatible via SIL Open Font License 1.1.
- Pixel display style fits the ADR-0008 English candidate list.
- Google Fonts source is public and redistributable with the included OFL text.

Source:

- `https://github.com/google/fonts/tree/main/ofl/pressstart2p`
- `https://raw.githubusercontent.com/google/fonts/main/ofl/pressstart2p/PressStart2P-Regular.ttf`
- `https://raw.githubusercontent.com/google/fonts/main/ofl/pressstart2p/OFL.txt`

## Atlas Settings

- Unity: 6000.3.14f1
- TMP / uGUI: `com.unity.ugui`
- Source font: `Assets/UI/Localization/Fonts/ThirdParty/PressStart2P-Regular.ttf`
- Requested characters: 191
- Character range: US-ASCII printable `U+0020..U+007E` plus Latin-1 Supplement printable `U+00A0..U+00FF`
- Missing characters: 0
- Atlas size: 2048x2048
- Render mode: SDF
- Sampling point size: 16
- Padding: 1
- Population mode after bake: Static
- Atlas texture format: Alpha8

2048x2048 was used instead of the initial 4096x4096 target because the English set is small and the bake completed with zero missing characters. This keeps the runtime texture estimate to one quarter of a 4096 Alpha8 atlas.

## Size Measurement

Measured files:

- `Anemora_EN.asset`: 92,523 bytes (about 90.35 KiB)
- `Anemora_EN_Atlas.asset`: 8,389,649 bytes (about 8.00 MiB serialized asset)
- `PressStart2P-Regular.ttf`: 118,204 bytes (about 115.43 KiB)

Runtime texture estimate:

- 2048 x 2048 x Alpha8 = 4,194,304 bytes = 4.0 MiB VRAM
- Against the laptop VRAM 2 GiB: 4.0 MiB is about 0.20%

The serialized atlas asset is larger than runtime Alpha8 VRAM because Unity stores YAML/native asset data around the texture payload.

## Fallback Chain

- `Anemora_JP.asset` fallback list now includes `Anemora_EN.asset`.
- `Anemora_EN.asset` fallback list now includes `Anemora_JP.asset`.

This matches ADR-0008 section 3.4 for mixed Japanese / English text.

## Caveats

- Press Start 2P is a draft candidate. Final font approval remains a user review item alongside Pixel Square / Pixelmix and VCR OSD Mono alternatives.
- `docs/legal/asset_ledger.md` was intentionally not touched in this batch. Add the Press Start 2P source font and `Anemora_EN` TMP atlas rows in a later ledger batch.
