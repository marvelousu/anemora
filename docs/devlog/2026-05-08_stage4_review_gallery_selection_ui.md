# Stage 4 Review Gallery Selection UI

> Date: 2026-05-08
> Scope: review workflow / image selection UI
> Result: A/B comparison controls replaced with mark-driven selection actions; no Unity scene, runtime asset, shader, or story content changed.

## 1. Trigger

The A/B comparison slots became redundant once manual marks were added. User review clarified that tagging is effectively the comparison mechanism, while the gallery still needs a way to collect confirmed candidates and act on them.

The same review also identified two thumbnail issues:

- vertical images could visually spill out of the left-side preview card
- static thumbnails were less useful than a lightweight hover preview

## 2. Changed

- Removed the A/B comparison slots and `Swap A/B` control.
- Added `◎` as a final-selection mark, supporting multiple confirmed images.
- Added a sticky `Selection` panel listing images marked `◎`.
- Added selection actions:
  - copy all final Windows paths
  - download a final-selection JSON export
  - per-final-image path copy
  - per-final-image open and download links
- Added per-card image download links.
- Changed left-side thumbnails to cropped `object-fit: cover` frames with `overflow: hidden`.
- Added an explicit trash action that removes the image from Final and marks it rejected.
- Renamed card download actions to `DL`.
- Renamed the final-selection JSON action to `Export JSON`.
- Added visible toast feedback for copy actions.
- Replaced in-card hover scaling with a floating hover preview that appears outside the card and disappears on mouse leave.
- Added the same floating hover preview and click-to-zoom behavior to final-selection thumbnails.
- Changed the final mark button label to `◎ Final` and added panel help text explaining that multiple images can be Final.
- Replaced the mixed-language exclude label with a trash control.
- Changed card actions to a compact two-row grid so `Zoom`, `Copy`, `Open`, `DL`, and trash do not wrap awkwardly in narrow cards.
- Converted trash into a real `Trash` mark / filter rather than only a reject state.
- Simplified card defaults to thumbnail, filename capped at two lines, and compact review mark buttons.
- Moved per-card controls into a smooth details panel opened by clicking the filename.
- Moved the main review actions to the preview window's top-right action strip.
- Hid Trash-marked images from normal review views; they now reappear only under the Trash mark filter.
- Changed compact cards to show review mark buttons by default, while inferred auto tags now appear only in the expanded details area.
- Added a Compact / Full capsule toggle for switching all cards between collapsed and expanded density.
- Removed the duplicate card-detail Trash action because Trash is now always available in the review mark row.
- Renamed the final mark from `◎` to `Final`.
- Changed the Trash mark button to display `Restore` when the image is already in Trash.
- Made the generated gallery public-safe by default: copy/export paths now use repository-relative `copyPath` values instead of local absolute paths.
- Added publication notes and blog-material notes for external write-up preparation.

## 3. Verification

- Regenerated `docs/review_gallery/index.html` with `python tools/build_review_gallery.py`.
- Generator indexed 28 images.
- Browser verification at `http://127.0.0.1:8765/docs/review_gallery/index.html?v=selection-ui-clear`:
  - card count: 28
  - `Swap A/B` / `Compare` UI removed
  - initial final-selection panel disabled path / manifest actions at `Final: 0`
  - `◎` toggled one image into final selection and enabled path / manifest actions
  - final path text resolved to the expected Windows path
  - card download links: 28
  - final-selection download links after one final mark: 1
  - thumbnail CSS: `overflow: hidden`, `object-fit: cover`, hover `transform` transition present
  - console errors / warnings: 0
- Trash/restore label cleanup verification at `http://127.0.0.1:8765/docs/review_gallery/index.html?v=trash-restore-labels`:
  - compact default mark buttons: `Final`, `★`, `✓`, `?`, `Trash`
  - expanded card actions: `Zoom`, `Copy`, `Open`, `DL`
  - no duplicate Trash action in expanded card details
  - Trash filter showed the trashed card with `Restore` in the mark row
  - preview action switched from `Trash` to `Restore` for a trashed image
  - Restore removed the image from the Trash filter and moved focus out of Trash
  - console errors / warnings: 0
- Publication-safety verification:
  - regenerated `docs/review_gallery/index.html` with the default `--path-mode relative`
  - confirmed generated HTML contains no local absolute user paths, local username strings, or `windowsPath`
  - generated a temporary `--path-mode absolute` output to verify the private local-path mode still works, then removed the temporary file
  - browser check at `http://127.0.0.1:8765/docs/review_gallery/index.html?v=public-safe-clean`: 28 cards, `Final: 1` mark flow, Trash 28 -> 27, Trash filter `Restore`, console errors / warnings 0
- Follow-up browser verification at `http://127.0.0.1:8765/docs/review_gallery/index.html?v=final-ui-followup-clean`:
  - card action labels: `Zoom`, `Copy`, `Open`, `DL`, trash
  - final mark label: `◎ Final`
  - two images could be marked Final and appeared as `Final: 2`
  - card copy action showed visible toast feedback
  - card hover preview appeared outside the card and disappeared after mouse leave
  - final-selection thumbnail hover preview appeared outside the panel
  - final-selection thumbnail click opened the zoom lightbox
  - trash action removed the image from Final and added the reject mark
  - `Export JSON` label replaced the ambiguous manifest label
  - console errors / warnings: 0
- UI cleanup browser verification at `http://127.0.0.1:8765/docs/review_gallery/index.html?v=ui-cleanup-trash-2`:
  - card action labels: `Zoom`, `Copy`, `Open`, `DL`, trash
  - action overflow check: false
  - final mark overflow check: false
  - trash action removed the image from Final and added the reject mark
  - copy action showed visible toast feedback
  - floating hover preview still appeared outside the card and disappeared after mouse leave
  - console errors / warnings: 0
- Compact-card follow-up verification at `http://127.0.0.1:8765/docs/review_gallery/index.html?v=compact-card-trash`:
  - default card details height: 0, opacity: 0
  - filename clamp: 2 lines
  - tag row: one-line nowrap, max height `22px`
  - filename click expanded the details panel smoothly, with visible actions after expansion
  - `Trash` mark/filter count updated after card Trash action
  - preview window top-right actions showed `Zoom`, `◎ Final`, marks, `Copy`, `Open`, `DL`, and `Trash`
  - preview Trash action removed the image from Final and moved it to the Trash set
  - console errors / warnings: 0
- Trash/density follow-up verification at `http://127.0.0.1:8765/docs/review_gallery/index.html?v=trash-hide-density-5`:
  - initial card count: 28
  - compact default details height: 0
  - compact default mark buttons: `◎`, `★`, `✓`, `?`, `Trash`
  - compact default auto-tag row height: 0
  - filename expansion exposed details and auto tags
  - Trash action reduced normal-view count from 28 to 27 and moved preview focus to the next visible image
  - Trash filter showed the trashed image as the only filtered item
  - Compact / Full capsule toggle expanded and collapsed all cards
  - preview window top-right actions still showed `Zoom`, `◎ Final`, marks, `Copy`, `Open`, `DL`, and `Trash`
  - mobile viewport 390x844 had no horizontal overflow
  - console errors / warnings: 0

## 4. Boundary

The final-selection manifest is a review artifact only. Runtime asset adoption still requires the normal import, ledger, and user-review path.
