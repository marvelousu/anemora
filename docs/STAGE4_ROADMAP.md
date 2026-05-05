# Stage 4 Roadmap

Status: v1.0 Stage 4 entry roadmap (2026-05-06)

本 doc は、Stage 3 closeout 後に Stage 4 の入口を定義する roadmap である。`docs/G5_ACCEPTANCE_MATRIX.md` と `docs/STAGE3_RETROSPECTIVE.md` v1.0 の結果を反映済み。

Stage 4 は、Stage 3 Vertical Slice を Steam Early Access に提出可能な build へ近づける段階である。Steam Early Access 予定は Stage 3 /spec resolution closure (`63de141`) で lock-in 済み。Code license は All Rights Reserved を現状 default とし、公開準備の中で再評価する。

本 roadmap は日付を決めない。Release date、Steam store 公開日、各 phase の calendar 期限は user / orchestrator の判断対象とする。

## 1. Stage 4 purpose

Stage 4 の目的は、Stage 3 で成立した最小体験を、公開可能な Early Access build の土台へ拡張することである。

| 目的 | 内容 | Stage 4 での扱い |
|---|---|---|
| Vertical Slice の確定反映 | G5 manual review で出た immediate fix / backlog / no action を整理する | Phase 0 |
| 質的補強 | Character art、font、palette、audio、test coverage、runtime warnings を公開前の基準へ近づける | Phase 0-1 |
| Content expansion | 第 1 ゾーンの polish から次ゾーン設計へ進み、4-6 zone structure の選択肢を具体化する | Phase 2 |
| Steam Early Access preparation | Store page、trailer、press kit、age rating、price、license / notice を公開向けに整える | Phase 3 |
| Release candidate | QA、beta tester feedback、release checklist を通し、Steam EA build を提出可能状態へ持っていく | Phase 4 |

## 2. Phase structure

Effort は個人開発 scale の relative band で記録する。`S` は focused session、`M` は複数 session、`L` はまとまった implementation block、`XL` は複数 block にまたがる作業を指す。Calendar date は含めない。

| Phase | Name | Main output | Entry condition | Exit condition |
|---:|---|---|---|---|
| 0 | G5 result reflection | G5 findings triage + Stage 3 carry-over fixes | User manual G5 result recorded (`a0bd50b`) | Immediate Stage 3 blockers resolved or explicitly moved to Stage 4 backlog |
| 1 | Quality reinforcement | Art / UI / localization / tests / renderer warnings brought to Stage 4 baseline | Phase 0 triage complete | Public-facing vertical-slice baseline is stable enough for content expansion |
| 2 | Zone expansion | Zone structure and next-zone production plan | Phase 1 baseline accepted | Next content block has scoped design, asset needs, and implementation entry points |
| 3 | Steam EA prep | Store, trailer, press kit, legal / disclosure, pricing inputs | Phase 2 content plan stable enough for public messaging | Steam release materials have reviewable drafts and missing-decision list |
| 4 | Release | Release candidate, beta feedback loop, Steam EA launch package | Phase 3 release materials accepted | Steam EA build can be submitted, released, or held by explicit user decision |

## 3. Phase 0: G5 result reflection

Phase 0 absorbs final Stage 3 observations before larger Stage 4 production starts. It should not expand scope unless G5 reveals a direct blocker.

| Workstream | 成果物 | 完了条件 | 推定工数 | Notes |
|---|---|---|---|---|
| G5 result triage | `docs/G5_ACCEPTANCE_MATRIX.md` final result notes + Stage 3 retrospective v1.0 inputs + `docs/STAGE4_PHASE0_TRIAGE.md` | User manual G5 items are classified as immediate fix, Stage 4 backlog, or no action | S | Stage 3 closeout result is now recorded; `STAGE4_PHASE0_TRIAGE.md` is the dispatch source for immediate Stage 4 work. |
| F2 v2 redraw | Niro / Hero v2 full redraw with a clear hat silhouette and gender-neutral 15-19 read; Resident_A / Resident_B review remains mandatory, but redraws are conditional on art-review findings | Niro v2 sprite draft is imported, visible in prefab preview, and documented in asset ledger; Resident_A / Resident_B keep or redraw decisions are recorded | M-L | User review on 2026-05-06 promoted Niro from provisional minor-review status to full redraw. Preserve Stage 3 v1 sprites as the baseline while creating v2 assets. |
| Lore content polish | G3 dialogue v0 -> v1 pass for Niro, Resident_A, Resident_B, and opening / short hint text | JP text is internally consistent; EN draft has review notes; StringTable key migration plan is recorded | M | Use current `docs/draft/g3_npc_dialogue.md`, `da6040f`, and `docs/PITCH_PUBLIC.md` as inputs. |
| Audio polish | SFX placeholder replacement list + added ambience candidates + mix notes from G5 listening | Replacement or keep decisions are recorded per SFX category; BGM loop / balance notes are resolved or backlogged | M | Keep `Zone1_Ambient.ogg` as the baseline unless G5 listening finds a blocker. |
| Demo brush UX polish | Brush preview affordance, tutorial hint, and input feel review after `a0bd50b` | Brush interaction remains understandable without developer explanation; any confusion is either fixed or documented as tutorial/content backlog | S-M | Initial runtime create / release / close hint is implemented without scene YAML changes; later UI review can replace it with a polished localized treatment. |
| URP DrawObjectsPass warning fix | Public `RenderObjectsPass` migration for `PortalStencilFeature` | Player log warning count drops to 0; portal visual tests and boundary flow still pass | Done | Stage 4 Phase 0 replaced the internal `DrawObjectsPass` path, kept `SetLayerMasks(...)`, and verified EditMode `32/32`, PlayMode `29/29`, build success, and 30 second player warning count `0`. |

## 4. Phase 1: Quality reinforcement

Phase 1 converts Stage 3 provisional choices into Stage 4 baseline decisions. It should keep player-facing experience stable while improving reliability and readability.

| Workstream | 成果物 | 完了条件 | 推定工数 | Notes |
|---|---|---|---|---|
| Palette v0 review | Palette v0 keep / revise decision; optional palette v0.1 swatch and import update | Character, building, UI, and text contrast are checked in representative scenes | S-M | Record the decision in `asset_ledger.md` and relevant art devlog. |
| TMP font review | 美咲ゴシック JP / Press Start 2P EN comparison notes and keep / replace decision | JP / EN dialogue panels remain readable; missing glyph / tofu checks are documented | S-M | Public-facing screenshots should be used before replacing font assets. |
| Verification gap coverage | Save/Load integration test, locale integration test, and interaction stress harness plan / implementation | Added tests or documented manual coverage reduce the highest-risk gaps in `docs/VERIFICATION_SUITE.md` | M-L | Focus areas: save/load persistence, locale switching, portal/action repeat stress, build warning regression. |
| Anemora.Data / Anemora.Game boundary migration | Dialogue / localization runtime boundary updated to match ADR-0008 v0.3; migration notes for existing assets | `Anemora.Data` remains POCO-only; Unity-dependent dialogue / localization code stays in runtime assembly; tests pass | M | Avoid moving engine-dependent types into Data. Document asset migration if ScriptableObject fields change. |

## 5. Phase 2: Zone expansion

Phase 2 decides how the Early Access content grows beyond the first zone. It should keep the first zone stable while designing the next content block.

| Workstream | 成果物 | 完了条件 | 推定工数 | Notes |
|---|---|---|---|---|
| Zone 2-4 / 5-6 design | Zone list options with role, setting, core interaction, key NPC / object, and asset scale | User can compare 4-zone and 6-zone content structures without hidden assumptions | L | Keep options parallel until the user selects the release structure. |
| 4-6 zone structure decision support | Comparison sheet for route length, asset burden, narrative pacing, localization volume, and Steam EA suitability | Decision inputs are visible; no calendar commitment is embedded | M | Tie the output to `docs/PITCH_PUBLIC.md` scale claims before public store copy is locked. |
| Next-zone production kit | Prompt templates, asset ledger row templates, prefab folder conventions, and scene setup checklist for the next zone | A new zone can start without re-deciding pipeline paths or ledger format | M | Reuse ADR-0009 asset pipeline and `docs/ASSET_STRUCTURE.md`. |

## 6. Phase 3: Steam EA prep

Phase 3 prepares public release materials. It should separate facts already known from decisions still owned by the user.

| Workstream | 成果物 | 完了条件 | 推定工数 | Notes |
|---|---|---|---|---|
| Steam Partner registration | Registration checklist and account / tax / payout status tracker | Required account steps are listed with owner and status | S-M | Do not store private account data in repo docs. |
| Age rating / content flags | Age-rating worksheet and content warning / questionnaire notes | Expected rating inputs are documented and reviewed against current build content | S-M | Recheck after content expansion. |
| Price / currency strategy | Pricing options sheet with comparable references, discount policy notes, and unresolved decisions | User has enough facts to select price / currency settings later | S | No price is selected in this roadmap. |
| English polish | Bilingual review pass for G3 final draft, UI strings, PITCH_PUBLIC text, and Steam copy | EN text is no longer Codex-only draft; review notes are resolved or tracked | M-L | Keep glossary terms stable before store copy and trailer subtitles lock. |
| Steam description | Store description draft derived from `docs/PITCH_PUBLIC.md` v0 | Short description, long description, tags, feature bullets, and screenshot captions are reviewable | M | Avoid internal planning vocabulary in public text. |
| Trailer production | Capture plan and edit checklist based on `docs/TRAILER_SCRIPT.md` v0 | 30s / 60s / 180s candidates have capture requirements, audio plan, and review criteria | L | Actual capture depends on final Stage 4 build state. |
| Press kit | Press kit folder plan and draft copy for summary, screenshots, logo, factsheet, notices, and contact policy | Public-facing materials are complete enough for review before Steam page publication | M | Coordinate with `NOTICES.md`, `AUTHORS.md`, and current license decision. |

## 7. Phase 4: Release

Phase 4 turns the prepared build and materials into a release candidate. Release, hold, or rework remains a user decision at the gate.

| Workstream | 成果物 | 完了条件 | 推定工数 | Notes |
|---|---|---|---|---|
| QA pass | Release candidate checklist covering build, save/load, localization, audio, performance, notices, and known issues | Blocking issues are fixed or explicitly accepted for Early Access | L | Include repeat playthrough and clean install checks. |
| Beta tester feedback | Small tester plan, feedback form, issue triage board, and build distribution notes | Feedback is categorized into blocker, pre-release fix, post-release backlog, or no action | M-L | Keep personally identifying tester data out of public repo docs. |
| Steam EA release package | Final build, store assets, trailer, release notes, notices, and disclosure materials | Package is ready for Steam review or can be held by explicit user decision | XL | Release execution is separate from this preliminary roadmap. |

## 8. Open decisions for v1.0

These items should be resolved or explicitly left open when promoting this roadmap to v1.0.

| Decision | Current state | Stage 4 handling |
|---|---|---|
| User manual G5 outcome | Resolved for Stage 3 | Immediate fixes and Stage 4 backlog items are recorded. |
| Test count baseline | Latest executed: EditMode `32/32`, PlayMode `29/29`; source marker scan: EditMode 31, PlayMode 29 | Keep both values visible until a future test-count reconcile removes the source/runner difference. |
| Release structure | Steam Early Access path locked, content scale still open | User selects or keeps open the 4-zone vs 6-zone content structure. |
| Code license | All Rights Reserved default | User decides whether Stage 4 keeps this default or prepares another license path. |
| Public contribution intake | Not opened | CONTRIBUTING / issue template policy is updated only if public intake is planned. |

## 9. Revision history

| Version | Date | Change |
|---|---|---|
| v0.1 | 2026-05-05 | Preliminary Stage 4 roadmap draft. Defines Phase 0-4 from G5 result reflection through Steam Early Access release package, with deliverables, completion conditions, effort bands, and no calendar dates. |
| v1.0 | 2026-05-06 | Promoted after Stage 3 closeout. Reflects `a0bd50b` user manual confirmation, latest test baseline, and Stage 4 Phase 0 entry workstreams. |
