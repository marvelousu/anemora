# Chapter 1 — HD-2D AUTONOMOUS run (operating rules)

Tom is out for the day. Run **self-paced and unattended**. The item queue is the 85-item
**`docs/CHAPTER1_HD2D_MASTER_BACKLOG.md`** (P0→P3, each with goal/approach/acceptance and an
`auto-safe` vs `NEEDS-TOM` tag). This file is the *how*; that file is the *what*.

Branch `work/chapter1-continuation-map-vs-20260524`. Unity 6000.3.14f1 / URP / HD-2D.

## ⚠ PLAN UPDATE 2026-06-03 v3 (GOVERNS ABOVE ALL — compounding-invisible-damage postmortem)

**What went wrong (worse than v2 thought).** After the v2 camera recovery the run did **not** stop; it
kept stacking P2-64 → P3-83 on top, self-accepting each via static/quasi-runtime captures. The latest build
now shows: **ground+buildings invisible-but-present, NPCs gone, the time window no longer swaps eras** (warp
fires, no visual swap). Root pattern: late items create/re-layer content onto layers the time-isolation
culling drops, and the capture harness structurally cannot see per-layer culling / NPC renderers / the
portal era swap — so it stayed green. See `docs/CHAPTER1_HD2D_RUNTIME_RECOVERY_V3_20260603.md`.

**The no-checkpoint hole (fix this permanently).** There are ZERO commits between baseline `e7277f0a` and
now (80+ items, 1145 dirty files), so there is no clean middle to roll back to. The old "never commit, leave
the tree dirty" guardrail CAUSED this. **New rule: commit each accepted, runtime-verified milestone (or
small batch) to the working branch.** Preserve the current tree first via a `wip/hd2d-snapshot-*` branch.

**Hard stop-for-Tom after recovery (verification gate, not a prohibition).** Ground/buildings/NPC
visibility and the time-window era swap are exactly what self-review cannot verify. So the backlog does NOT
resume until Tom confirms, by eye, a 20-25s real-player run with: (1) ground+buildings visible, (2) NPCs
visible, (3) the time window swapping to the other era on placement. Attempting things is fine; *accepting*
and *resuming* require that Tom-eye confirmation.

## ⚠ PLAN UPDATE 2026-06-02 v2 (runtime-blank postmortem; superseded by v3 where conflicting)

**Postmortem.** The built runtime went blank-ish (camera framed empty space after ~20s) starting at
**P1-37** (it enabled a `CinemachineBrain` on the main camera while the existing guide still drove the
camera directly → dual authority). It stayed undetected for ~30 items / ~17h because acceptance used
**static / edit-mode review captures**, which render through the review-camera path and bypass the
runtime Brain/timing — so the break never showed. Separately: the flat ground is still unfixed because
the visible top ground layer is **overdraw/stacked geometry**, not the material the shader items edited;
and Depth Priming (P1-49) broke beauty and was cut off. Lesson: *green in a static capture ≠ works when
you play it for 20 seconds.*

**THE GATE (the real safeguard — not a prohibition on what to attempt).** Any item that touches camera,
player, lighting, render pipeline, or runtime behavior is **ACCEPTED ONLY after a sustained real-player
run**: build, launch `Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe` in normal mode, let it run
**20-25s**, capture at t=5s/15s/25s across the affected areas, and confirm the framing/scene are stable
with **no regression vs the last-good runtime**. Static/edit-mode review captures are necessary but NOT
sufficient. Use per-second `Debug.Log` telemetry (camera pos/euler/fov, player local, active area) in
`%USERPROFILE%\AppData\LocalLow\<company>\<product>\Player.log` to confirm steady state.

**High-blast-radius changes** (camera authority, render pipeline, global draw) are allowed, but: keep them
isolated and easy to revert, and verify the **whole runtime** (all 5 areas, 20-25s) — not just the item's
local capture — through the gate above. If the sustained run regresses, revert/park that item and log it.

**Phases (do in order):**
- **P0 — camera recovery** (in progress, separate handoff): restore guide-driven camera, park Cinemachine.
- **P1 — establish the sustained-runtime gate** as the standing acceptance method (above). The earlier
  "capture-harness proof" (post visible in static capture) is necessary but does NOT satisfy this.
- **P2 — RE-VALIDATE the ~30 items built on the broken runtime BEFORE any new feature.** With the camera
  fixed, run a 20-25s 5-area current/past pass and confirm which "done" items actually hold (VFX, water,
  lighting, dressing, emissive windows, foliage). Triage regressions. This precedes new work.
- **P3 — fix the flat ground via the overdraw/top-layer route**: Frame-Debugger which renderer is actually
  topmost/visible, remove or consolidate the stacked ground overlays, then apply value/AO/texture to the
  real visible geometry. (The shader-prop route already failed twice — it's a layering problem.)
- **P4 — resume backlog**, but only auto-safe, additive, sustained-runtime-verifiable, base-look items.

**NEEDS-TOM consolidation.** Stop scattering taste baselines across cycles and stop regenerating 0-pixel
A/B churn for them. Collect every taste decision (camera feel, color grade, lighting curves, water, sprite
normals, panini, gobos, sky/backdrop, etc.) into ONE `docs/HD2D_TOM_DECISION_SHEET.md` — each row: item,
A/B capture link, conservative current value, recommended value — for Tom to approve/tune in a single pass.

## ⚠ PLAN UPDATE 2026-06-02 (v1 — still in effect: ops + capture-harness + reprioritization)
Review of the run found: real wins (town/plaza lighting, lit windows, dressing, foliage/tree
cards) BUT a low-yield tail — P2-52/53/54/55 each captured **0-pixel A/B** (no provable effect),
and **P2-55 ground value/AO is a confirmed NO-OP** (its shader props don't reach the rendered
ground material). Root causes: (a) the review-capture harness can't validate effects, (b) the
"propagate every pass" rule created ~75 cycle folders/day + deploy-hook `304` coalescing that
leaves the viewer one build behind. New rules:

1. **Propagate ACCEPTED captures only.** Do NOT upload/deploy superseded pass1/pass2 folders.
   One accepted A/B cycle per item.
2. **One deploy hook per accepted item** (or batch a few, then one hook). Never per-pass. After
   the LAST item of a sitting, fire one final hook so nothing is stranded (the per-pass hooks
   were coalescing to `304` and dropping the latest cycle).
3. **DO THESE TWO FIRST (new top priority, before any more effect items):**
   - **FIX THE CAPTURE HARNESS** so self-review actually works: the review camera must have
     post-processing ON, use the **diorama framing** (not the near-ground low angle), and frame
     the region where the tested effect appears. The character SPRITE does NOT need to render
     (sprites are existing assets, not a graphics deliverable) — but for grounding/shadow items
     the character's SHADOW (contact / soft-blob / directional cast) must be in-frame and
     visible so it can be A/B-toggled. Add an A/B "effect proof" capture. Acceptance: toggling a
     known post effect (e.g. bloom) yields an obvious non-zero pixel delta, and for a shadow
     item the shadow is visibly in-frame and changes when toggled. Without this, effect items
     cannot be verified and must not be attempted.
   - **FIX THE FLAT GROUND FOR REAL** (highest base-look value; P2-55 was a no-op). Determine
     which material/shader variant the rendered ground meshes ACTUALLY use (the value/AO props
     were added to a variant the ground isn't using). Apply real tonal/texture variation +
     AO there — or bake vertex-color AO via the P0-6 splat shader actually assigned to the
     ground. Acceptance: ground in an area overview shows visible tonal/texture variation vs the
     current flat grey, A/B delta clearly non-zero. This is the #1 remaining base-look item.
4. **PARK the unverifiable NEEDS-TOM micro-effects** — Panini (P2-54), light cookies/gobos
   (P2-53), soft-blob contact shadow tuning (P2-52), parallax-backdrop/sky sub-tuning
   (P1-40/41) are taste-heavy AND can't be auto-verified. Leave their conservative baselines as
   already logged; do NOT keep regenerating empty 0-pixel A/B cycles for them. They wait for Tom.
5. **Re-focus autonomous effort** on auto-safe, capture-verifiable, base-look items only:
   real ground, foliage density, SSAO grounding, prop-cube replacement, emissive-window spread.
6. Optional infra (medium priority): the Pages build fetches the whole manifest (~840+ images,
   117+ cycles) each build; cap the manifest to the most recent ~40 accepted cycles so builds
   stay fast as cycles accumulate.

## Current state (already good — do NOT regress)
- Interiors + time-conditional doors (E2 Kaia / F4 ruin both-time; F2/F3/F5 past-only) — working.
- Cross-quad grass + tree sprite cards with DOF/tilt-shift + bloom — genuinely HD-2D; keep.
- Rich scaffold already exists: post stack (Bloom/DoF/ColorAdjustments/Vignette/Tonemapping),
  volumetric fog, sun/ToD cycle, ramp-lit shaders, billboard chars + contact shadows, time
  window. So most work is **tuning / density / asset quality**, not adding missing systems.

## THE OPERATING LOOP (every item, no exceptions)
1. Pick the next item from the master backlog (top-down within tier; finish P0 before P1, etc.).
2. Implement (small, modular, localized edits).
3. `Unity -batchmode -quit -projectPath . -executeMethod AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
   — compile 0 errors + validation pass (2-pass; keeps FilmGrain).
4. Capture review images to `docs/review/<ts>_<item_slug>/` (current+past of affected areas).
5. **PROPAGATE TO THE VIEWER (mandatory — Tom reviews remotely):**
   ```
   pwsh tools\r2\r2-upload-review.ps1 -CycleDir docs/review/<ts>_<item_slug> -Branch work/chapter1-continuation-map-vs-20260524
   curl -X POST "https://api.cloudflare.com/client/v4/pages/webhooks/deploy_hooks/fcf2097f-a326-4e1e-af37-4c7feeb12203"
   ```
   `~/.cf_token` is in place so auth works. **Verify the script's last line says `uploaded N
   files` with N>0** — N=0 means auth failed, do not silently continue. (You can batch several
   items' cycles then fire one deploy hook to save build minutes, but never skip the upload.)
6. Self-review the captures vs the item's Acceptance. Default stance: "probably still short."

## auto-safe vs NEEDS-TOM (critical for unattended work)
- **`auto-safe` items:** implement fully. If acceptance clearly met → done. If ambiguous after
  **at most 2 passes**, stop, log "needs Tom's eye", move on. Never loop an item >2 passes.
- **`NEEDS-TOM` items (taste-heavy: camera FOV/pitch, DoF feel, Kelvin lighting curve, color
  grade, ambient/exposure):** these are the biggest wins but must not be finalized blind.
  Implement a **conservative, data-driven** version (expose values on a ScriptableObject /
  profile so Tom can retune without code), produce an **A/B capture** (before vs after), write
  a one-line **recommendation** in the progress log, and **leave it in the conservative state**.
  Do NOT chase a subjective ideal. The readability floor (P0 ambient/exposure so the *current*
  timeline isn't near-black) is the one place to be assertive — current maps are objectively
  unreadable; lift them, using the past timeline's brightness as the ceiling reference.
- Append a paragraph per item to **`docs/HD2D_AUTONOMOUS_PROGRESS.md`** (create it): item,
  what changed, validate log path, R2 cycle folder, self-verdict (done / needs-Tom / skipped),
  key files.

## GUARDRAILS (hard rules)
- One item at a time; complete its full loop (incl. R2 propagate + log) before the next.
- **No commit/push/PR.** Leave the tree dirty; Tom reviews on return.
- No scope outside the master backlog. No refactors/renames/"while I'm here". Spotted something?
  Add it under "Observations" in the progress log and move on. (Last run drifted into a
  multi-hour grounding loop — do not repeat.)
- Don't touch the door/interior mechanic, `FastVsAreaDoorTransition`, the area enum, or the
  proven foliage-card pipeline except where an item explicitly extends them.
- Big-file edits (`AnemoraFastVsHouseSliceSetup.cs`) stay behind new helpers; no mass call-site
  rewrites. Asset imports: CC0 only; **never Synty** (EULA forbids AI ingestion). Record
  attribution in `Assets/Art/External/<source>/LICENSE.txt`.
- Suggested order for a day: P0 readability/ground/SSAO/emissive-windows (auto-safe parts) →
  P1 ground splat + foliage density + water + particles → prep the NEEDS-TOM camera/lighting/
  grade items with A/B captures for Tom. Skip the master's "Human-eye items" list for finalizing.
- **Stop conditions:** master backlog exhausted, OR `BuildAndValidateBatch` fails twice on one
  item (skip+log+continue), OR every remaining item is NEEDS-TOM and already prepped. On stop,
  write a final summary in the progress log.

## FINAL REPORT (write at stop, in docs/HD2D_AUTONOMOUS_PROGRESS.md)
Per item: done / needs-Tom(+recommendation) / skipped(+why), validate log, R2 cycle folder,
files touched. Then: "viewer now shows cycles: <list>", and the latest .exe path if you built
one (else note the scene is ahead of the .exe and a rebuild is needed for in-engine review).
