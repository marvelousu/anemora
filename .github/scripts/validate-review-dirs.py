#!/usr/bin/env python3
"""Validate the docs/review/ directory structure.

Rules (see docs/review/README.md):
  1. Each immediate child of docs/review/ must be a directory whose name matches
     YYYY-MM-DDTHH-MM (ISO 8601 + URL safe).
  2. Each such directory must contain a file named devlog.txt.
  3. The first non-empty, non-comment (starts with '#') line of devlog.txt must
     point at an existing .md file in the repo.
  4. The directory must contain at least one image file (.png/.jpg/.jpeg/.webp
     /.svg/.gif), searched recursively.

Exits 0 when there are no violations (or docs/review/ is absent), 1 otherwise.
"""
from __future__ import annotations

import re
import sys
from pathlib import Path

REPO_ROOT = Path(__file__).resolve().parent.parent.parent
REVIEW_DIR = REPO_ROOT / 'docs' / 'review'
ISO_RE = re.compile(r'^\d{4}-\d{2}-\d{2}T\d{2}-\d{2}$')
IMAGE_EXTS = {'.png', '.jpg', '.jpeg', '.webp', '.svg', '.gif'}


def validate_devlog(devlog_path: Path) -> str | None:
    try:
        text = devlog_path.read_text(encoding='utf-8')
    except Exception as e:
        return f'cannot read devlog.txt: {e}'
    first = None
    for line in text.splitlines():
        s = line.strip()
        if not s or s.startswith('#'):
            continue
        first = s
        break
    if not first:
        return 'devlog.txt has no non-comment content'
    target = (REPO_ROOT / first).resolve()
    try:
        target.relative_to(REPO_ROOT.resolve())
    except ValueError:
        return f'devlog target escapes the repo: {first}'
    if not target.exists():
        return f'devlog target does not exist: {first}'
    if target.suffix.lower() != '.md':
        return f'devlog target is not a .md file: {first}'
    return None


def has_image(directory: Path) -> bool:
    for p in directory.rglob('*'):
        if p.is_file() and p.suffix.lower() in IMAGE_EXTS:
            return True
    return False


def main() -> int:
    if not REVIEW_DIR.exists():
        print('docs/review/ does not exist yet; nothing to validate.')
        return 0

    errors: list[str] = []

    for child in sorted(REVIEW_DIR.iterdir()):
        rel = child.relative_to(REPO_ROOT)
        if not child.is_dir():
            if child.name in {'README.md', '.gitkeep'}:
                continue
            errors.append(f'{rel}: unexpected file at docs/review/ root '
                          f'(expected a YYYY-MM-DDTHH-MM directory)')
            continue
        if not ISO_RE.match(child.name):
            errors.append(f'{rel}: directory name does not match '
                          f'YYYY-MM-DDTHH-MM')
            continue
        devlog = child / 'devlog.txt'
        if not devlog.exists():
            errors.append(f'{rel}: missing devlog.txt')
        else:
            problem = validate_devlog(devlog)
            if problem:
                errors.append(f'{rel}/devlog.txt: {problem}')
        if not has_image(child):
            errors.append(f'{rel}: no image files (.png/.jpg/.jpeg/.webp/.svg/.gif) found')

    if errors:
        for e in errors:
            print(f'::error::{e}', file=sys.stderr)
        print(f'\n{len(errors)} violation(s).', file=sys.stderr)
        return 1
    print('All docs/review/ directories validated.')
    return 0


if __name__ == '__main__':
    raise SystemExit(main())
