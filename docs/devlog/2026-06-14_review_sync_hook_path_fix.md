# Review Sync Hook Path Fix

Date: 2026-06-14
Branch: `wip/hd2d-point15-recovery-20260612`
Scope: follow-up fix for the review-sync pre-push hook.

## Change

After enabling `tools/githooks`, manual execution of `tools/githooks/pre-push`
from the available bash resolved the repo root as `/mnt/c/...` and then invoked
Windows PowerShell. Windows PowerShell could not open that WSL-style path.

The hook now converts the repo root with `cygpath -w` or `wslpath -w` when it is
about to call `powershell.exe` / `powershell`, while leaving native `pwsh` paths
alone.

## Verification

- `bash tools/githooks/pre-push`: pass.
- `tools/review/validate-devlog-review-sync.ps1`: pass through the hook, using
  `origin/wip/hd2d-point15-recovery-20260612` as the comparison base.
