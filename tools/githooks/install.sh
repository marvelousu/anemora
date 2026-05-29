#!/usr/bin/env bash
# Install repo git hooks via core.hooksPath (git-bash / Linux / macOS).
git config core.hooksPath tools/githooks && echo "hooks installed: core.hooksPath = tools/githooks"
