# Contributing to Anemora

> Status: draft (2026-05-05). Anemora is currently in Stage 3 Vertical Slice development. External contribution policy is TBD and will be decided at the Stage 4 public-release entry point.

## 1. Welcome / Current Status

Anemora is currently a single-developer project. This repository may become public at or after Stage 4, but issue intake, pull request intake, review rules, and contributor legal terms are not finalized yet.

For now, this document is a preparation draft. It records the current state and the decisions still open, without selecting a final contribution model.

Relevant tracking docs:

- `README.md` — project overview and basic setup.
- `docs/STAGE3_TBD_RESOLUTION.md` — open Stage 3 / Stage 4 decisions, including code license and public release form.
- `docs/legal/code_license_options.md` — neutral code license option notes.
- `NOTICES.md` — third-party notices and current All Rights Reserved default.

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

TBD.

Current observed code structure:

- `Assets/Scripts/Data/` contains the `Anemora.Data` POCO layer.
- `Assets/Scripts/Save/` contains the `Anemora.Save` layer.
- `Assets/Scripts/Game/` contains game-facing ScriptableObject / runtime code.
- `Assets/Scripts/TimeManagement/`, `Assets/Scripts/Player/`, `Assets/Scripts/Dialogue/`, and `Assets/UI/Scripts/` hold feature-specific runtime components.

No final public coding style guide is selected yet. If this repository accepts external pull requests later, formatting, naming, asmdef boundaries, nullable policy, and Unity serialization conventions should be documented here.

## 4. Commit Message Conventions

TBD.

Observed Stage 3 Day 1 commit subjects use short imperative phrases, for example:

- `Add editor automation usage doc`
- `Add LocalizationSettings and StringTable seed for dialogue resolution`
- `Reconcile EditMode test count baseline`
- `Apply audio prompts integration check fixes and document inconsistencies`

The current history does not follow strict Conventional Commits syntax such as `feat:`, `fix:`, or `docs:`. Stage 4 should decide whether to keep the existing imperative-subject convention, adopt Conventional Commits, or use another release-note workflow.

## 5. Pull Request Process

TBD.

Open decisions before public PR intake:

- Whether pull requests are accepted at all.
- Review owner and review SLA.
- Required tests / manual verification before merge.
- Branch naming and target branch policy.
- Whether large Unity asset changes require separate review from code changes.
- Whether contributors may submit generated assets or only source code / docs.

Until this section is finalized, external PRs should not be treated as an accepted workflow.

## 6. Issue Templates

TBD.

At scan time, no `.github/ISSUE_TEMPLATE/` directory was present. Stage 4 can decide whether to add templates for bugs, feature requests, asset review, localization feedback, or documentation fixes.

## 7. Code of Conduct

TBD.

At scan time, no `CODE_OF_CONDUCT.md` file was present. If public issue / PR participation is opened, the project should decide whether to add a Code of Conduct and where moderation responsibility sits.

## 8. DCO / CLA

TBD.

Contributor legal handling depends on the Stage 4 code license decision. Options such as no external contributions, project-license-only contribution, Developer Certificate of Origin, Contributor License Agreement, or custom written contribution terms are not selected here.

Related docs:

- `docs/STAGE3_TBD_RESOLUTION.md` PUB-01 — code license.
- `docs/STAGE3_TBD_RESOLUTION.md` PUB-02 — public release form.
- `docs/legal/code_license_options.md` — DCO / CLA is mentioned as part of contribution willingness.

## 9. Asset Contribution Policy

TBD, with current tracking through `docs/legal/asset_ledger.md`.

Any future asset contribution policy must account for:

- Third-party source licenses.
- AI-generation tool terms and paid-plan evidence.
- Steam AI disclosure category.
- Intermediate files under `art/_intermediate/` and `audio/_intermediate/`.
- Final Unity import paths under `Assets/`.

Do not assume that a code license, once selected, also licenses sprites, audio, models, fonts, TMP atlases, or other assets. Asset licensing remains separate until Stage 4 resolves it.

## 10. Testing Expectations

TBD, with current verification guidance in `docs/VERIFICATION_SUITE.md`.

Current test references:

- EditMode tests: `Assets/Tests/EditMode/`.
- PlayMode tests: `Assets/Tests/PlayMode/`.
- Verification catalog: `docs/VERIFICATION_SUITE.md`.
- G5 acceptance checklist: `docs/G5_ACCEPTANCE_MATRIX.md`.
- G5 preflight: `docs/G5_PREFLIGHT.md`.

Before any future PR process is opened, this section should define which checks are required for code changes, Unity scene / prefab changes, generated asset changes, documentation-only changes, and release candidates.

## 11. Documentation Updates

TBD.

Current navigation docs that may need updates when a contribution changes project behavior:

- `CHANGELOG.md` — milestone / release-note level.
- `docs/devlog/INDEX.md` — devlog navigation.
- `docs/ASSET_STRUCTURE.md` — directory layout.
- `docs/EDITOR_AUTOMATION.md` — automation usage.
- `docs/legal/asset_ledger.md` — asset provenance and license tracking.
- `docs/STAGE3_TBD_RESOLUTION.md` — open user decisions.

## 12. Change History

| Version | Date | Notes |
|---|---|---|
| v0.1 | 2026-05-05 | Initial draft for Stage 4 entry preparation. Contribution intake, coding style, PR process, issue templates, Code of Conduct, DCO / CLA, and asset policy remain TBD. |
