# Stage 4 Review Gallery

> Date: 2026-05-08
> Scope: review workflow / image navigation
> Result: static local review gallery added; no Unity scene, runtime asset, shader, or story content changed.

## 1. Purpose

Stage 4 visual review now has many candidate sheets, scale lineups, dialogue captures, and blockout diagrams. Passing individual file paths to the user is too slow for iteration.

This change adds a local static HTML gallery so review images can be filtered, previewed, opened, and compared without manually searching through folders.

## 2. Added

- `tools/build_review_gallery.py`
- `docs/review_gallery/README.md`
- `docs/review_gallery/index.html`
- `docs/review_gallery/imports/.gitkeep`

Default scan roots:

- `docs/devlog/screenshots/`
- `docs/review_gallery/imports/`

## 3. Usage

Generate or refresh:

```powershell
python tools/build_review_gallery.py
```

Open directly:

```powershell
Start-Process .\docs\review_gallery\index.html
```

Or use a local URL:

```powershell
Start-Process -WindowStyle Hidden -FilePath python -ArgumentList @("-m","http.server","8765","--bind","127.0.0.1") -WorkingDirectory (Get-Location)
Start-Process http://127.0.0.1:8765/docs/review_gallery/index.html
```

## 4. Current Verification

- Generator indexed 28 images.
- Browser verification via local HTTP showed 28 cards, expected tags, and loaded image dimensions.
- Console errors / warnings: 0.
- No Unity tests were run because this is a static docs/tooling change.

## 5. Boundary

The gallery does not choose or import final assets. Generated character or shader review candidates should be copied into `docs/review_gallery/imports/` only when they are part of a user review packet. Runtime adoption still requires the normal review and import path.
