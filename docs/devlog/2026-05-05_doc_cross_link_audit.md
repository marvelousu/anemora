# Doc Cross-Link Integrity Audit

Date: 2026-05-05

## 1. Scope

Audited Markdown files under `docs/` plus repository-root Markdown files for internal link and path integrity.

Scan targets:

- `docs/**/*.md`
- repository-root `*.md`

Scan count: 68 Markdown files.

## 2. Method

Checks performed:

- Markdown inline links: relative file targets in normal link and image-link syntax.
- Raw repository path references: `docs/...`, `Assets/...`, `ProjectSettings/...`, `Packages/...`, `tools/...`, `art/...`, `audio/...` patterns ending in common file extensions.
- Known path-drift check for the old character-art directory pattern after the ASSET_STRUCTURE correction.

External URLs, anchor-only links, and dated placeholder paths such as `2026-05-XX` were not treated as dead links.

## 3. Findings

| Check | Result |
|---|---:|
| Markdown files scanned | 68 |
| Dead Markdown relative links | 0 |
| Raw unresolved path candidates before filtering / fixes | 37 |
| Raw unresolved path references remaining after fixes | 10 |
| Old character-art directory stale references | 0 |

The Markdown link graph itself is intact. The issues found were raw path strings in prose, plans, and devlogs.

## 4. Fixes Applied

Applied lightweight path / typo fixes only. No ADR Decision text was changed.

| File | Fix summary |
|---|---|
| `docs/adr/0008-localization.md` | Updated the StringTableCollection reference to `Assets/Localization/StringTables/Anemora_Strings.asset`. |
| `docs/devlog/2026-05-05_urp_setup_check.md` | Updated URP generated asset paths to `Assets/Settings/...`. |
| `docs/STAGE3_E_PLAN.md` | Updated URP support asset paths, collapsed ActionRecord DTO/store references to `Assets/Scripts/Data/ActionRecord.cs`, and updated book prefab paths to `Assets/Prefabs/Zone1/Book_Family_{Past,Current}.prefab`. |
| `docs/STAGE3_F_PLAN.md` | Updated Hero prefab path to `Assets/Prefabs/Characters/Hero.prefab`. |
| `docs/STAGE3_G_PLAN.md` | Updated BookReflector script path, Resident prefab paths, and dialogue asset names to current G3/F4 paths. |
| `docs/draft/g3_npc_dialogue.md` | Updated example dialogue asset path to `Assets/ScriptableObjects/Dialogues/Resident_A_Greeting.asset`. |

Applied fix count: 21 raw path references.

## 5. Remaining Candidates

The following unresolved raw path references were not changed in this commit because they require either a content decision, a future asset generation batch, or a separate plan-doc reality update.

| Group | References | Status |
|---|---|---|
| A4 SFX placeholder | `Assets/Audio/SFX/env/wind_subtle.ogg` in `docs/draft/g1_opening_text.md` and `docs/STAGE3_G_PLAN.md` | Wait for A4 SFX final import / naming. |
| G plan-era prefab names | `House_Player_Past.prefab`, `Plaza_Center.prefab`, `Library_Ruin_Past.prefab`, `Layer2_Hint.prefab` in `docs/STAGE3_G_PLAN.md` | Needs a focused `STAGE3_G_PLAN` reality-sync pass against A3/G4/G5 outputs. |
| OSS ledger doc | `docs/legal/oss_ledger.md` in `SPEC.md` | File is explicitly marked planned / TBD. Create the ledger or revise SPEC in a later legal-doc pass. |

Two unresolved raw path candidates were intentionally ignored:

- `docs/CHANGELOG.md` in `docs/devlog/2026-05-05_audio_prompts_integration_check.md`: the sentence already notes that the task prompt used this path while the actual file is root `CHANGELOG.md`.
- `Assets/PerfBaselineTemp/GeneratedPerfBootstrap.unity` in `docs/devlog/2026-05-05_performance_baseline.md`: temporary measurement scene path, not a committed project asset.

## 6. Conclusion

The docs have no dead Markdown relative links after this audit. Lightweight stale path references were fixed in this commit. Remaining unresolved raw path references are limited to future asset/legal follow-ups or intentionally historical notes.
