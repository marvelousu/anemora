# Stage 4 TMP Localization Objective Coverage

Date: 2026-05-07

## Summary

This batch adds objective EditMode coverage for the TMP / localization portion of Stage 4 Phase 1. It does not change production assets, scenes, localization text, font assets, or runtime behavior.

## Added Coverage

- `LocalizationFontCoverageTests.StringTablesCoverDialogueUiAndSystemKeysForSupportedLocales`
  - Loads `Anemora_Strings_ja-JP` and `Anemora_Strings_en`.
  - Verifies both locale tables share the same `SharedTableData`.
  - Verifies current `dialogue.`, `ui.`, and `system.` keys have non-empty rows in both supported locales.
- `LocalizationFontCoverageTests.StringTablesDoNotContainDialoguePlaceholderKeys`
  - Verifies legacy `dialogue.placeholder.*` keys are absent from shared key data.
  - Verifies localized values do not contain unresolved `dialogue.placeholder.*` references.
- `LocalizationFontCoverageTests.DialogueTmpFontAssetsHaveAtlasData`
  - Verifies `DialoguePanel.prefab` TMP font references are not null.
  - Verifies TMP FontAsset assets under `Assets/UI/Localization/Fonts` expose atlas texture, glyph, character, and atlas size data.

## Verification

- EditMode: `39/39 passed`
- PlayMode: not rerun in this batch; latest baseline remains `31 passed / 32 total` with one `[Explicit]` TMP screenshot capture skipped.

## Notes

- This closes the purely objective part of the TMP / localization risk: key coverage, old placeholder removal, prefab font references, and font atlas data.
- It does not replace visual review. Rendered readability, palette fit, and text fit still require the existing explicit screenshot harness and user review before any font or panel replacement.
- Unity Test Runner generated transient Addressables side effects (`link.xml` deletion and `Windows.meta`); these were reverted / removed and are not part of the commit.
