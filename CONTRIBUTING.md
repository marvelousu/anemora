# Contributing to Anemora

> Status: draft v0.3 (2026-05-06). Anemora is currently near the end of Stage 3 Vertical Slice verification. The primary public release path is Steam Early Access, and the code license remains All Rights Reserved by default until Stage 4 re-evaluation.

## 1. Current Contribution Posture

Anemora is currently a single-developer project. External issue intake, pull request intake, review rules, contributor legal terms, and moderation policy are not open workflows yet.

This document is a Stage 4 preparation draft. It records the current project state and the decisions that must be made before public contribution intake can be treated as supported.

Current public-facing project facts:

- Protagonist: Niro / ニロ (provisional).
- First zone: Antela / アンテラ (provisional).
- Public release direction: Steam Early Access as the primary public path.
- Code and project-owned assets: All Rights Reserved by default unless a third-party license explicitly applies.
- Contribution intake: single-developer workflow continues until Stage 4 decides otherwise.

## 2. Development Setup

Use the setup flow in `README.md`:

1. Install Unity `6000.3.14f1` through Unity Hub.
2. Clone the repository.
3. Add the cloned `anemora/` project in Unity Hub.
4. Open `Assets/Scenes/Anemora_Main.unity`.
5. Press Play in the Unity Editor.

Additional layout / automation references:

- `docs/ASSET_STRUCTURE.md` — canonical asset directory layout.
- `docs/EDITOR_AUTOMATION.md` — Unity editor automation and helper script usage.
- `docs/adr/` — Architecture Decision Records.

## 3. Coding Style

Formal public coding style guide: TBD for Stage 4.

Current observed code structure:

- `Assets/Scripts/Data/` contains the `Anemora.Data` POCO layer.
- `Assets/Scripts/Save/` contains the `Anemora.Save` layer.
- `Assets/Scripts/Game/` contains game-facing ScriptableObject / runtime code.
- `Assets/Scripts/TimeManagement/`, `Assets/Scripts/Player/`, `Assets/Scripts/Dialogue/`, and `Assets/UI/Scripts/` hold feature-specific runtime components.
- `Assets/Scripts/PerformanceHarness/` contains Stage 4 stress-sample harness scaffolding.

If this repository accepts external pull requests later, formatting, naming, asmdef boundaries, nullable policy, Unity serialization conventions, and generated-asset boundaries should be documented here.

## 4. Commit Message Conventions

Formal commit convention: TBD for Stage 4.

Observed Stage 3 Day 1 commit subjects use short imperative phrases, for example:

- `Add trailer script v0 (30s / 60s / 180s)`
- `Polish README post-lore-reflection`
- `Add Stage 4 roadmap preliminary draft`
- `Add stress sample harness design and skeleton`

The current history does not follow strict Conventional Commits syntax such as `feat:`, `fix:`, or `docs:`. Stage 4 should decide whether to keep the existing imperative-subject convention, adopt Conventional Commits, or use another release-note workflow.

## 5. Issues and Pull Requests

Pull request intake: TBD for Stage 4.

Issue template set: TBD for Stage 4.

At this draft point, external PRs should not be treated as an accepted workflow. Before opening public PR intake, decide:

- Whether pull requests are accepted at all.
- Review owner and review expectations.
- Required tests / manual verification before merge.
- Branch naming and target branch policy.
- Whether Unity scene / prefab / asset changes require separate review from code changes.
- Whether contributors may submit generated assets or only source code / docs.

At scan time, no `.github/ISSUE_TEMPLATE/` directory was present. If public issue intake opens, add templates for bugs, feature requests, asset review, localization feedback, and documentation fixes as appropriate.

## 6. Code License / Conduct / DCO / CLA

Code license remains All Rights Reserved by default. Stage 4 may re-evaluate this, but this contributing draft does not grant an open-source license. See `NOTICES.md` for the consumer-facing license summary.

Code of Conduct: TBD for Stage 4.

DCO / CLA handling: TBD for Stage 4.

At scan time, no `CODE_OF_CONDUCT.md` file was present. Contributor legal handling depends on the Stage 4 re-evaluation of the current All Rights Reserved default. Options such as no external contributions, project-license-only contribution, Developer Certificate of Origin, Contributor License Agreement, or custom written contribution terms are not selected here.

Related docs:

- `NOTICES.md` — current consumer-facing license and third-party notices.
- `docs/legal/code_license_options.md` — neutral code license option notes.

## 7. Asset Contribution Policy

External asset contribution acceptance: TBD for Stage 4.

Current asset provenance is tracked through `docs/legal/asset_ledger.md`. Stage 3 Day 1 state includes:

- Niro / Antela provisional identity decisions reflected in sprite and documentation rows.
- Zone1 BGM `Assets/Audio/Music/Zone1_Ambient.ogg`.
- Zone1 SFX 30 entries across environment, footsteps, time-window, NPC, and UI categories.
- TMP Japanese / English font atlases and third-party font license references.
- Meshy / Blender building assets, PixelLab / Aseprite sprite assets, and AI-assisted audio records.

Any future asset contribution policy must account for:

- Third-party source licenses.
- AI-generation tool terms and paid-plan evidence.
- Steam AI disclosure category.
- Intermediate files under `art/_intermediate/` and `audio/_intermediate/`.
- Final Unity import paths under `Assets/`.

Do not assume that a code license, once selected or re-evaluated, also licenses sprites, audio, models, fonts, TMP atlases, or other assets. Asset licensing remains separate until Stage 4 re-evaluates it.

## 8. Testing Expectations

Required public PR test gates: TBD until a PR workflow opens.

Current verification references:

- EditMode expected count: 31 tests by source scan under `Assets/Tests/EditMode/`.
- PlayMode expected count: 27 UnityTests by source scan under `Assets/Tests/PlayMode/`.
- Verification catalog: `docs/VERIFICATION_SUITE.md`.
- G5 acceptance checklist: `docs/G5_ACCEPTANCE_MATRIX.md`.
- G5 preflight: `docs/G5_PREFLIGHT.md`.
- Manual G5 rubric: `docs/G5_MANUAL_RUBRIC.md`.

Before any future PR process is opened, this section should define which checks are required for code changes, Unity scene / prefab changes, generated asset changes, documentation-only changes, and release candidates.

## 9. Documentation Updates

Current navigation docs that may need updates when a contribution changes project behavior:

- `README.md` — public project entry point.
- `CHANGELOG.md` — milestone / release-note level.
- `docs/devlog/INDEX.md` — devlog navigation.
- `docs/ASSET_STRUCTURE.md` — directory layout.
- `docs/EDITOR_AUTOMATION.md` — automation usage.
- `docs/legal/asset_ledger.md` — asset provenance and license tracking.
- `docs/STAGE4_ROADMAP.md` — preliminary Stage 4 sequencing.

## 10. Public Release and Public-Facing References

The current primary public release path is Steam Early Access. Exact release timing, price, store-page copy, trailer final cut, and final license handling remain Stage 4 review items.

- `docs/PITCH_PUBLIC.md` — Steam description / trailer / press-kit source text.
- `docs/TRAILER_SCRIPT.md` — 30s / 60s / 180s trailer script draft.
- `docs/STAGE3_RETROSPECTIVE.md` — Stage 3 retrospective draft.
- `docs/STAGE4_ROADMAP.md` — Stage 4 preliminary roadmap.
- `AUTHORS.md` — primary author and contribution status.
- `NOTICES.md` — consumer-facing third-party notices and current license status.

## 11. Change History

| Version | Date | Notes |
|---|---|---|
| v0.1 | 2026-05-05 | Initial draft for Stage 4 entry preparation. |
| v0.2 | 2026-05-05 | Reflected Stage 3 /spec resolution: Niro / Antela provisional names, Steam Early Access release direction, and All Rights Reserved default license pending Stage 4 re-evaluation. |
| v0.3 | 2026-05-06 | Updated for Stage 3 Day 1 near-completion: single-developer posture, Steam Early Access path, All Rights Reserved default, asset ledger state, test counts, and public-facing cross-references. |
