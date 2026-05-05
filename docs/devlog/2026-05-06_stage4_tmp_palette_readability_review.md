# Stage 4 TMP / Palette Readability Review (2026-05-06)

Scope: documentation-only review of the current TMP font, dialogue panel, and palette v0 readability surface. No font assets, UI prefabs, localization tables, scenes, ProjectSettings, generated logs, or builds were modified.

## 1. Inputs

Observed runtime / asset inputs:

- Dialogue screenshot: `C:\Users\maro6\Documents\Unity\Anemora-demo-repair\Builds\DemoPlayable\demo4_dialogue.png`
- Dialogue prefab: `Assets/UI/Prefabs/DialoguePanel.prefab`
- JP font asset: `Assets/UI/Localization/Fonts/Anemora_JP.asset`
- EN font asset: `Assets/UI/Localization/Fonts/Anemora_EN.asset`
- Palette files: `Assets/Art/anemora_palette_v0.gpl`, `Assets/Art/anemora_palette_v0.png`
- Prior font docs: `docs/devlog/2026-05-05_tmp_jp_atlas_v0.md`, `docs/devlog/2026-05-05_tmp_en_atlas_v0.md`, `docs/devlog/2026-05-05_tmp_jp_atlas_v0_1_missing_chars_review.md`

## 2. Current UI Surface

`DialoguePanel.prefab` currently uses `Anemora_JP.asset` as the primary TMP font for speaker, body, and advance indicator text. `Anemora_JP.asset` falls back to `Anemora_EN.asset`, and `Anemora_EN.asset` falls back to `Anemora_JP.asset`.

Prefab settings observed from YAML:

| Element | Setting |
|---|---|
| Panel | bottom anchored, `anchorMax.y = 0.32`, `offsetMin = (64, 36)`, `offsetMax = (-64, -18)` after runtime normalization |
| Panel color | black with alpha `0.7` |
| Speaker label | font size `22`, white text, no autosizing |
| Dialogue body | font size `24`, white text, wrapping enabled, no autosizing |
| Advance indicator | font size `22`, white text, no autosizing |

Palette v0 provides dedicated UI colors:

| Palette swatch | RGB | Intended use |
|---|---:|---|
| `UI Text` | `232,223,200` | warm readable body text |
| `UI Muted Text` | `183,170,145` | secondary text |
| `UI Frame` | `140,118,87` | frames / separators |
| `Charcoal Ink` | `30,29,26` | dark UI/background base |
| `Deep Shadow` | `43,42,37` | secondary dark base |

The current dialogue panel does not yet use those palette swatches directly; it uses pure white text and a black translucent backing.

## 3. Findings

| Priority | Finding | Evidence | Recommendation |
|---|---|---|---|
| P1 | Runtime visual evidence is not yet enough to lock the font/palette decision. | Existing `demo4_dialogue.png` shows the dialogue panel active, but the panel sits at the very bottom of the captured frame and is not enough for a final readability decision. | Capture a fresh JP/EN dialogue screenshot set from the current v2 build before replacing fonts or committing prefab visual changes. |
| P1 | Dialogue panel colors are functional but not aligned with palette v0. | Prefab uses white text and black `0.7` panel while palette v0 defines `UI Text`, `UI Muted Text`, `UI Frame`, and dark base swatches. | First polish pass should try palette-backed text/background/frame colors rather than a font replacement. |
| P1 | Body font size has fit risk for EN long lines. | Body text is size `24`, no autosizing, and current EN strings include longer lines such as Resident_B line 2. | Review EN at 1280x720 and 1920x1080. If crowded, reduce body font to `20-22` or enable a bounded autosize range before changing content. |
| P2 | JP glyph coverage is currently acceptable for VS text. | Prior missing-char review found 70 missing requested chars, but current G3 runtime strings are covered and PlayMode localization tests pass. | Keep `Anemora_JP` for now; rerun coverage when new Stage 4 copy introduces punctuation such as middle dots or additional kanji. |
| P2 | Press Start 2P is safe technically but may be tiring for dialogue body text. | EN atlas has zero missing characters and is licensed, but Press Start 2P is dense at dialogue scale. | Treat it as a review candidate: keep for short UI labels if accepted, but consider a softer pixel body font if EN dialogue feels heavy. |

## 4. Recommended Next Pass

Before changing assets, capture a review sheet with:

- JP Resident_A line 1 and Resident_B longest line at 1280x720.
- EN Resident_A line 3 and Resident_B line 2 at 1280x720.
- The same four shots at 1920x1080.
- One view with the brush hint visible and one with dialogue visible, to confirm overlay hierarchy and text contrast.

If the screenshots confirm the risk, the smallest implementation pass should be:

1. Apply palette v0 UI swatches to dialogue text / muted text / panel / frame.
2. Reduce body font size or add bounded autosizing.
3. Keep current JP/EN font assets until the user explicitly rejects the visual style.
4. Re-run PlayMode and a screenshot check after prefab changes.

## 5. Decision State

Current recommendation: keep the current fonts as provisional and do not replace them blindly. The likely first change is a dialogue panel color / sizing polish pass, not a new font pipeline.

## 6. Verification

- Source scan of `DialoguePanel.prefab`, TMP font assets, palette file, prior font devlogs, and current StringTable entries.
- Visual spot-check of `demo4_dialogue.png`.
- Unity was not run because this pass is documentation-only and makes no runtime changes.

## 7. Revision History

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-06 | Initial Stage 4 TMP / palette readability review |
