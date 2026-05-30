# Install repo git hooks via core.hooksPath (Windows / PowerShell).
# One setting points git at the version-controlled hooks dir for this clone.
git config core.hooksPath tools/githooks
Write-Host "hooks installed: core.hooksPath = tools/githooks"
