# 2026-05-18 Fast VS public repo promotion

## Scope

- Project: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`
- Public VS build: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- Public VS scene: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`

## Decision

- Promote the current Fast VS V24 branch state as the public VS baseline.
- Keep all devlogs in the repository history.
- Preserve old / unrelated uncommitted work locally before cleaning the public branch state.
- Keep `main` as the stable public VS baseline, and continue future work from a separate branch.

## Public Baseline Commits

- `f31608e Baseline Fast VS V24 sample state`
- `64b5407 Show only Time Window creation hint`
- `2ec4edb Skip Fast VS opening wake line`
- This repository organization record is the final documentation-only commit before tagging the public VS baseline.

## Verification

- Latest build and validation log:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Logs\fast_vs_build_validate_20260518_skip_opening_wake_line.log`
- The log contains:
  - `Fast VS house slice validation passed.`
  - `Build Finished, Result: Success.`
- The Windows EXE was rebuilt at:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`

## Repository Cleanup Plan Applied

- Create a public baseline tag after this record:
  - `vs-public-2026-05-18`
- Move local `main` to that tag / commit.
- Create a continuation branch for future work:
  - `work/post-vs-public-20260518`
- Archive unrelated dirty leftovers in a local branch before cleaning the worktree:
  - `archive/pre-vs-public-uncommitted-leftovers-20260518`

## Notes

- No remote push is part of this record. Pushing `main` and the public tag to GitHub should be a separate explicit operation.
- Deleting a merged local feature branch does not delete the devlogs or commits once the commits are reachable from `main` and the public tag.
