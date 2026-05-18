# Chapter 1 Public Preview Blocker Fix 2 Recovery

Date: 2026-05-15
Recovered: 2026-05-19
Source note:
- `notes/_handover/anemora-chapter1-public-preview-blocker-fix2-procedure-2026-05-15.md`

## Recovery note

No root-level `docs/devlog/2026-05-15_*.md` file was found in the recovered Anemora worktrees,
Linux repo, notes repo history, or local refs. This file reconstructs the 2026-05-15 public
preview blocker-fix record from the surviving procedure note.

## User-reported blockers

The 2026-05-15 user review found that the Chapter 1 VS public-preview candidate still had
multiple severe blockers:

1. In Library, after inspecting, the dialogue/log could not be dismissed immediately and felt
   like a freeze.
2. If Niro was inside the TimeWindow generation range, rendering glitched and Niro ghosting
   spread across the screen.
3. The TimeWindow had no frame, or the frame was too weak.
4. TimeWindow generation had no readable effect.
5. Collision/containment appeared to leak outside the window.
6. After window generation, the current-side player could become stuck.
7. Niro's house-exit line had changed unexpectedly.
8. Niro's exterior house area felt too narrow.
9. The playable walkable area was unclear.

The procedure explicitly prioritized stability and readable public-preview behavior over
new content.

## Fix scope

The fix was scoped to public-preview blockers rather than final story polish:

- Restore or preserve canonical house-exit text if it had drifted.
- Prevent Niro / player visual instability when a TimeWindow is committed near the character.
- Make the TimeWindow frame and generation effect visible enough to read as intentional.
- Ensure current-side position after commit is safe and not inside generated blocking
  geometry.
- Repair Library log dismissal so the player can close the repeat inspect / completed log
  promptly.
- Preserve movement after log dismissal.
- Add evidence captures for current-side safety, library log dismissal, drag preview, and
  generated frame effect.

## Recorded verification

The source note records these expected / reported outputs:

- Output build: `<temp>/anemora_ch1_playable_publicfix2_20260515/Anemora_Chapter1.exe`
- Smoke evidence directory: `<temp>/anemora_ch1_publicfix2_smoke_20260515_visible/`
- Player log: `<temp>/anemora_ch1_publicfix2_smoke_20260515_visible_player.log`
- Built-player smoke: `RESULT PASS`
- Touched-file `git diff --check`: passed

Named evidence frames in the procedure included:

- `tw_current_side_safe_after_commit.png`
- `library_log_dismiss_ready.png`
- `library_log_dismissed_movement_restored.png`
- `tw_timewindow_drag_frame_preview.png`
- `tw_timewindow_generated_frame_effect.png`

## Outcome recorded by handover

The source note includes the orchestration report label:

`[Claude -> orchestration] Chapter1 publicfix2 blockers fixed`

The report template called out:

- Current-side safe after commit.
- Library dismiss readiness and restored movement.
- TimeWindow drag frame preview.
- TimeWindow generated frame effect.
- A reminder to ship only if the first four blocker groups were fixed.

## Boundaries

The 2026-05-15 pass retained the same constraints as the 2026-05-14 public-preview passes:

- Do not commit/push unless Tom explicitly asks.
- Do not do broad story rewrite.
- Do not import new characters or generic NPCs.
- Do not implement final oblique/stencil portal rendering.
- Do not re-enable old TimeFrame / TimeWindow / Diorama systems.

This record is therefore a retroactive project-history reconstruction from the surviving
procedure and its verification summary.
