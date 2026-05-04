# 2026-05-05 TMP Japanese Atlas v0.1 Missing Chars Review

## Status

Review complete. `docs/devlog/2026-05-05_tmp_jp_atlas_v0.md` で報告された `Anemora_JP_Atlas` の missing 70 字について、Stage 3 VS の実表示候補に影響するかを確認した。

## Input

- Atlas draft: `Assets/UI/Localization/Fonts/Anemora_JP.asset`
- Atlas devlog: `docs/devlog/2026-05-05_tmp_jp_atlas_v0.md`
- Character set builder: `Assets/Editor/AnemoraTmpJapaneseAtlasBuilder.cs`
- Requested characters: 6,734
- Reported missing characters: 70

Atlas asset の `m_CharacterTable` と `BuildJapaneseCharacterSet()` を再構成して raw 差分を取ると 73 code points になる。差分 3 字は `U+2028 LINE SEPARATOR`, `U+2029 PARAGRAPH SEPARATOR`, `U+202F NARROW NO-BREAK SPACE` で、表示用 glyph として扱わないため、Unity 実行時ログの reported missing 70 に合わせてレビュー対象外にした。

## Missing 70

| Code | Char / Label | Name |
|---|---|---|
| U+2017 | ‗ | DOUBLE LOW LINE |
| U+201A | ‚ | SINGLE LOW-9 QUOTATION MARK |
| U+201B | ‛ | SINGLE HIGH-REVERSED-9 QUOTATION MARK |
| U+201E | „ | DOUBLE LOW-9 QUOTATION MARK |
| U+201F | ‟ | DOUBLE HIGH-REVERSED-9 QUOTATION MARK |
| U+2022 | • | BULLET |
| U+2023 | ‣ | TRIANGULAR BULLET |
| U+2024 | ․ | ONE DOT LEADER |
| U+2027 | ‧ | HYPHENATION POINT |
| U+202A | `<LRE>` | LEFT-TO-RIGHT EMBEDDING |
| U+202B | `<RLE>` | RIGHT-TO-LEFT EMBEDDING |
| U+202C | `<PDF>` | POP DIRECTIONAL FORMATTING |
| U+202D | `<LRO>` | LEFT-TO-RIGHT OVERRIDE |
| U+202E | `<RLO>` | RIGHT-TO-LEFT OVERRIDE |
| U+2031 | ‱ | PER TEN THOUSAND SIGN |
| U+2034 | ‴ | TRIPLE PRIME |
| U+2035 | ‵ | REVERSED PRIME |
| U+2036 | ‶ | REVERSED DOUBLE PRIME |
| U+2037 | ‷ | REVERSED TRIPLE PRIME |
| U+2038 | ‸ | CARET |
| U+2039 | ‹ | SINGLE LEFT-POINTING ANGLE QUOTATION MARK |
| U+203A | › | SINGLE RIGHT-POINTING ANGLE QUOTATION MARK |
| U+3004 | 〄 | JAPANESE INDUSTRIAL STANDARD SYMBOL |
| U+3016 | 〖 | LEFT WHITE LENTICULAR BRACKET |
| U+3017 | 〗 | RIGHT WHITE LENTICULAR BRACKET |
| U+3018 | 〘 | LEFT WHITE TORTOISE SHELL BRACKET |
| U+3019 | 〙 | RIGHT WHITE TORTOISE SHELL BRACKET |
| U+301A | 〚 | LEFT WHITE SQUARE BRACKET |
| U+301B | 〛 | RIGHT WHITE SQUARE BRACKET |
| U+301E | 〞 | DOUBLE PRIME QUOTATION MARK |
| U+3020 | 〠 | POSTAL MARK FACE |
| U+3021 | 〡 | HANGZHOU NUMERAL ONE |
| U+3022 | 〢 | HANGZHOU NUMERAL TWO |
| U+3023 | 〣 | HANGZHOU NUMERAL THREE |
| U+3024 | 〤 | HANGZHOU NUMERAL FOUR |
| U+3025 | 〥 | HANGZHOU NUMERAL FIVE |
| U+3026 | 〦 | HANGZHOU NUMERAL SIX |
| U+3027 | 〧 | HANGZHOU NUMERAL SEVEN |
| U+3028 | 〨 | HANGZHOU NUMERAL EIGHT |
| U+3029 | 〩 | HANGZHOU NUMERAL NINE |
| U+302A | 〪 | IDEOGRAPHIC LEVEL TONE MARK |
| U+302B | 〫 | IDEOGRAPHIC RISING TONE MARK |
| U+302C | 〬 | IDEOGRAPHIC DEPARTING TONE MARK |
| U+302D | 〭 | IDEOGRAPHIC ENTERING TONE MARK |
| U+302E | 〮 | HANGUL SINGLE DOT TONE MARK |
| U+302F | 〯 | HANGUL DOUBLE DOT TONE MARK |
| U+3030 | 〰 | WAVY DASH |
| U+3031 | 〱 | VERTICAL KANA REPEAT MARK |
| U+3032 | 〲 | VERTICAL KANA REPEAT WITH VOICED SOUND MARK |
| U+3033 | 〳 | VERTICAL KANA REPEAT MARK UPPER HALF |
| U+3034 | 〴 | VERTICAL KANA REPEAT WITH VOICED SOUND MARK UPPER HALF |
| U+3035 | 〵 | VERTICAL KANA REPEAT MARK LOWER HALF |
| U+3036 | 〶 | CIRCLED POSTAL MARK |
| U+3037 | 〷 | IDEOGRAPHIC TELEGRAPH LINE FEED SEPARATOR SYMBOL |
| U+3038 | 〸 | HANGZHOU NUMERAL TEN |
| U+3039 | 〹 | HANGZHOU NUMERAL TWENTY |
| U+303A | 〺 | HANGZHOU NUMERAL THIRTY |
| U+303B | 〻 | VERTICAL IDEOGRAPHIC ITERATION MARK |
| U+303C | 〼 | MASU MARK |
| U+303D | 〽 | PART ALTERNATION MARK |
| U+303E | 〾 | IDEOGRAPHIC VARIATION INDICATOR |
| U+303F | 〿 | IDEOGRAPHIC HALF FILL SPACE |
| U+3094 | ゔ | HIRAGANA LETTER VU |
| U+3095 | ゕ | HIRAGANA LETTER SMALL KA |
| U+3096 | ゖ | HIRAGANA LETTER SMALL KE |
| U+30F7 | ヷ | KATAKANA LETTER VA |
| U+30F8 | ヸ | KATAKANA LETTER VI |
| U+30F9 | ヹ | KATAKANA LETTER VE |
| U+30FA | ヺ | KATAKANA LETTER VO |
| U+30FB | ・ | KATAKANA MIDDLE DOT |

## Checked Text Set

VS 関連文言として以下を確認した。

- `docs/localization/glossary.md` 全文
- `docs/draft/g1_opening_text.md` 全文
- `docs/draft/g3_npc_dialogue.md` 全文
- `SPEC.md` / `PITCH.md` の Stage 3 UI 表示候補周辺

## Intersection

Strict full-document scan では `U+30FB ・` のみ交差した。

| File | Hit | 用途判定 |
|---|---|---|
| `docs/localization/glossary.md` | `U+30FB ・` | §1.1 の scope 説明 (`世界観・固有名詞`)。メタ説明で、現時点の String Table 候補ではない |
| `SPEC.md` | `U+30FB ・` | 仕様本文の列挙・説明文に出現。Stage 3 UI label / dialogue の確定文言ではない |
| `PITCH.md` | `U+30FB ・` | pitch prose / 説明文に出現。インゲーム表示文言ではない |
| `docs/draft/g1_opening_text.md` | none | G1 表示候補に missing 70 は出現しない |
| `docs/draft/g3_npc_dialogue.md` | none | G3 対話表示候補に missing 70 は出現しない |

## Conclusion

現時点の VS 実表示候補 (G1 オープニング、G3 NPC 対話、glossary の UI label 群、SPEC/PITCH の UI display wording) には missing 70 の必須文字はない。したがって **v0 Atlas のままでも Stage 3 VS 体験には直ちに影響しない**。

ただし `U+30FB ・` は日本語 UI / docs で使いやすい区切り文字であり、全文スキャンでは交差している。Stage 4 入口の v0.1 改訂で以下を再評価する。

1. String Table 生成後に、実表示文字集合と `Anemora_JP.asset` の `m_CharacterTable` を比較する smoke check を追加する
2. `U+30FB ・` を含む UI copy を採用する場合は、Atlas 再 bake で収録可否を確認する
3. 再 bake でも欠落する場合は、TMP fallback に商用可・GitHub Public 可の日本語フォント subset を追加する。候補は美咲明朝を含めて coverage check し、必要なら Noto Sans JP subset など glyph coverage 優先の fallback を使う

## Carryover

- Stage 3 VS: v0 Atlas 継続可
- Stage 4 入口: `U+30FB ・` と String Table 実文字集合を再検査
- v0.1 改訂判断: fallback 追加は String Table 確定後に決める
