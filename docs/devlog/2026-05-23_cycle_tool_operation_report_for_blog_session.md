# Cycle Tool Operation Report For Blog Session

Audience: blog-session handoff  
Project branch: `work/fast-vs-hd2d-shading-foundation-20260522`  
Repository: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Summary

The new cycle tooling is useful for Anemora's current HD-2D work, but only after adding repository-specific safety guards. The strongest value is not automatic visual quality judgment; it is disciplined repetition: scoped worker prompt, bounded authored files, batch validation, screenshot capture, build/smoke, commit, and push as one repeatable cycle.

The important conclusion for a blog-session writeup is:

- The `cycle-start` skill and `cycle-worker` agent are useful for small, bounded Unity implementation cycles.
- The `tools/cycle-runner.ps1` orchestrator became usable after hardening it against Unity side effects and destructive rollback behavior.
- The visual gate is still mandatory. Cycle 70 and Cycle 71 both passed validate/capture/build/smoke, but the Cycle 71 house exterior screenshots still exposed a visual defect around the house facade framing.

## Tooling Paths

- Agent definition: `C:\Users\maro6\.codex\agents\cycle-worker.toml`
- Skill entry point: `C:\Users\maro6\.codex\skills\cycle-start\SKILL.md`
- Scoped prompt template: `C:\Users\maro6\.codex\skills\cycle-start\references\scoped-prompt-template.md`
- Unity reference workflow: `C:\Users\maro6\.codex\skills\cycle-start\references\reference-implementation-unity.md`
- Runner script: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\tools\cycle-runner.ps1`

## What Was Verified

Cycle 70 and Cycle 71 were both run through the hardened runner on the work branch and pushed.

- Cycle 70 commit: `80dfabd` / `feat(hd2d): add outdoor directional shadow pass`
- Cycle 71 commit: `4bec89e` / `feat(hd2d): soften outdoor directional shadows`
- Cycle 71 devlog: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\2026-05-23_fast_vs_hd2d_outdoor_shadow_softening_cycle71.md`
- Cycle 71 screenshots: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle71_outdoor_shadow_softening_parent_review_20260523_01`

The Cycle 71 runner completed all phases:

- Validate: passed.
- Capture: passed.
- Build: passed.
- Smoke: passed.
- Commit and push: completed to `origin/work/fast-vs-hd2d-shading-foundation-20260522`.

## Runner Fixes Required Before It Was Safe

The first version of the runner was a good skeleton, but too risky for this repo without changes. The following fixes were made and pushed before relying on it for ongoing work:

- Added `-NoRollback` so the runner does not use `git reset --hard HEAD` on failure.
- Added `-CommitPath` so commits can be limited to authored files instead of staging Unity-generated side effects with `git add -A`.
- Fixed Unity batch argument passing for `-executeMethod`.
- Fixed process waiting so the runner waits for Unity batch phases before judging logs.
- Fixed failure-log Markdown code fence generation.
- Added the runner itself to the repository at `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\tools\cycle-runner.ps1`.

Required command shape for this repository:

```powershell
& 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\tools\cycle-runner.ps1' `
  -CycleNumber <N> `
  -ValidateMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch' `
  -CaptureMethod '<capture method returned by worker>' `
  -BuildMethod 'Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch' `
  -DevlogPath '<cycle devlog path>' `
  -Audience parent_review `
  -CaptureOutputDir '<cycle screenshot output directory>' `
  -CommitPath '<authored file>','docs/devlog/INDEX.md','<cycle devlog path>','<cycle screenshot output directory>' `
  -NoRollback
```

## Operational Judgment

The cycle system is worth keeping, but it should be treated as an implementation discipline tool, not a taste or art-direction tool.

What worked:

- The parent session can keep strategy, quality bar, and visual review.
- The worker can handle bounded edits in `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`.
- The runner reliably packages validation, capture, build, smoke, commit, and push once the method names and commit paths are explicit.
- Full-path logs and screenshot output make it much easier to audit what happened after long cycles.

What still needs human/parent review:

- Whether the screenshot actually looks better.
- Whether a capture camera is showing the intended defect.
- Whether a structurally valid scene has a wrong visual result.
- Whether the worker changed the wrong visual priority.

## Visual Gate Result After Cycle 71

Runner status alone was green, but the parent visual gate was mixed.

Useful evidence:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle71_outdoor_shadow_softening_parent_review_20260523_01\parent_review_01_current_house_exterior_shadow_softening_overview.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle71_outdoor_shadow_softening_parent_review_20260523_01\parent_review_03_current_house_exterior_shadow_softening_close.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_shadow_foundation_cycle71_outdoor_shadow_softening_parent_review_20260523_01\parent_review_05_current_plaza_shadow_softening_overview.png`

Observed result:

- Plaza shadowing is broadly acceptable as a foundation pass.
- House exterior screenshots still show a strong visual problem around the facade framing / black slab area.
- The close house capture did not frame the main problem well enough, so the next cycle should improve the review camera or target the geometry directly before more shadow-tuning.

## Remaining Tooling Issues

- Runner-generated commit subjects currently show a BOM-like prefix in `git log` for some runner commits. The next runner hardening pass should ensure UTF-8 without BOM when reading the first H1 line from the devlog.
- Unity batch mode still dirties unrelated project and Addressables files. `-CommitPath` prevents accidental commits, but the parent still needs to clean side effects after runner execution.
- The runner's failure behavior is now safer with `-NoRollback`, but failure handling should eventually append a structured failure section without modifying unrelated worktree state.

## Recommended Next Cycle

Cycle 72 should not continue simply making shadows stronger. It should first target the house exterior visual defect:

1. Improve or add a review capture that clearly frames the house doorway / porch / facade problem.
2. Remove or replace the black slab / misframed facade element if it is real geometry.
3. Preserve the working outdoor shadow foundation from Cycle 70 and Cycle 71.
4. Run validate, capture, build, smoke, then visually inspect before pushing.

After that, the next useful direction is a dedicated exaggerated-but-readable HD-2D shadow pass rather than more generic material edits.

