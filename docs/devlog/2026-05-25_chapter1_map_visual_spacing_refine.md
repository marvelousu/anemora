# 2026-05-25 Chapter 1 map visual spacing refine

Branch: `work/chapter1-continuation-map-vs-20260524`

## Scope

Follow-up blockout pass for the Chapter 1 continuation maps after visual review.

- Street corner / Aria area:
  - Replaced the uniform current-time stall row with collapsed counters, fallen awnings, loose posts, and dust.
  - Split the top fence guide into shorter segments so it reads less like a literal rectangular border.
  - Added broken-wall, door-gap, roof-shard, and debris cues to the street-side ruins.
  - Reworked the right-side ruin near Aria's house as a partial house form instead of a single wall slab.
- Kaia farm:
  - Moved the farm house left.
  - Moved the front yard directly against the house's right side and shifted the door, arbor, furrows, dry patch, flowers, and trees with it.
  - Split long fence lines into interrupted segments so the farm is not fully boxed in.
  - Reduced the far-right grass patches and right-side reference strips.
- Ruins:
  - Tightened the left ruin-house groups and narrowed the road/plaza space in front of them.
  - Reworked the right-side ruin pair with roof shards, side wall, posts, door gap, and debris.
  - Kept the Scene 6 side-view auto map unchanged.

## Validation

- `ValidateChapter1AllMapsBatch` passed after making decorative Aria-street ruin pieces non-blocking.
- `CaptureChapter1AllMapsCycle05ScreenshotsBatch` passed.

## Review outputs

- Source capture set: `docs/devlog/screenshots/chapter1_all_maps_cycle05/`
- Curated review set: `docs/review/2026-05-25T15-44/`
