# Stage 4 API / Verification Doc Refresh

> Date: 2026-05-07
> Scope: documentation / verification catalog hygiene
> Inputs: `docs/api/save_load.md`, `docs/api/dialogue_asset_authoring.md`, `docs/api/dialogue_localization.md`, `docs/VERIFICATION_SUITE.md`, `Assets/Tests/`
> Result: documentation-only refresh; no runtime code, assets, scenes, or story decisions changed.

## 1. Purpose

After the Stage 4 governance intake, the API and verification docs still had a few Stage 3-era statements that no longer matched the current branch. This pass refreshes those docs against the actual test files and current dialogue key usage.

## 2. Changes

- Updated `docs/api/save_load.md` to reflect current PlayMode save/load coverage:
  - `SaveLoadRoundTripE2ETests`
  - `SaveLoadLocaleIntegrationTests`
- Clarified that locale is not persisted in `SaveEnvelope` today; current Unity Localization selected locale remains the runtime source of truth.
- Updated `docs/api/dialogue_asset_authoring.md` to remove legacy `dialogue.placeholder.*` authoring guidance and describe the current final-key families.
- Updated `docs/api/dialogue_localization.md` status / source-scan date for Stage 4 onboarding.
- Updated `docs/VERIFICATION_SUITE.md` source-marker scan and added `StressSampleRunnerSmokeTests` as §K sampler lifecycle coverage.

## 3. Source Scan

Current source-marker scan:

- EditMode markers: 38
- PlayMode markers: 32
- PlayMode includes one `[Explicit]` manual TMP screenshot capture harness.

Current documented Unity runner baseline remains:

- EditMode: `39/39`
- PlayMode: `31 passed / 32 total`, with one explicit capture skipped

No Unity tests were rerun for this documentation-only change.

## 4. Boundary

This pass does not:

- Add or modify runtime save/load implementation.
- Change any `SaveEnvelope` field.
- Add StringTable keys or dialogue text.
- Change scene placement, visual assets, or character / map decisions.
- Turn performance baseline values into automated pass/fail thresholds.

## 5. Follow-Up

- Re-run EditMode / PlayMode after the next runtime or asset import change.
- Re-run performance baseline after major TMP/font/UI, character, or environment updates.
- Update `docs/api/save_load.md` again when filesystem slot persistence is implemented.
