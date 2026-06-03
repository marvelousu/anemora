# Chapter 1 — Runtime recovery v3: Niro OK, but world/NPCs invisible + time window dead (2026-06-03)

Branch `work/chapter1-continuation-map-vs-20260524`. Unity 6000.3.14f1 / URP. No commit/push/PR yet
(see R0 — we now DO take a preservation snapshot before touching anything).

## Context / what Tom actually sees in the latest build
The v2 recovery (`CHAPTER1_HD2D_RUNTIME_RECOVERY_V2_20260603.md`) DID fix the two things it targeted:
Niro renders and the camera frames the plaza. But playing the latest exe Tom reports **three new, worse
regressions**:
1. **Ground + buildings are invisible** — yet they are physically present (no crash; collision/occupancy
   still there).
2. **NPC characters are gone** (the `CreateAnimatedSpriteCharacter` villagers).
3. **The time window (時の窓) no longer works**: you can place the frame, but its contents show the
   *current* world instead of the other era; walking Niro into it just **warps** (the transition physics
   fire) with no visual era swap.

These were NOT introduced by the v2 camera fix per se. Two things happened:
- The broken camera (framing empty ground) had been **hiding** that the world/NPCs were already damaged —
  you literally could not see the buildings were missing. v2 fixed the camera, which **revealed** the
  damage. This is the P2 re-validation surfacing real regressions, as planned.
- **The autonomous run did not stop at the recovery.** `docs/HD2D_AUTONOMOUS_PROGRESS.md` shows it kept
  going from the v2 recovery (11:28) straight through **P2-64 → P2-77 → P3-78 → P3-83** (latest
  2026-06-03 09:18). The build Tom is playing is ~20 items PAST the recovery, several of which touch the
  exact systems now broken. Dirty file count went 766 → **1145**.

## Process truth (state this plainly; it governs the recovery)
There are **zero commits** between the pristine baseline `e7277f0a` (the r2 fix, before ANY HD-2D work)
and now. 80+ items are mashed into one undifferentiated 1145-file dirty tree. So there is **no clean
intermediate checkpoint** to roll back to — only `e7277f0a` is clean. The no-commit guardrail + unattended
stacking + self-acceptance via static/quasi-runtime captures (which structurally cannot see per-layer
culling, NPC renderers, or the portal/aperture era swap) is what let compounding invisible damage
accumulate undetected across TWO camera recoveries. **R0 below fixes the no-checkpoint problem first.**

## Root cause analysis (one hypothesis ties all three symptoms together)
The time-isolation culling is the common thread. `FastVsVisualDirectionGuide.ApplyCameraCulling`
(`FastVsVisualDirectionGuide.cs:411-436`) builds the main-camera `cullingMask` from the portal
controller's layer ints and, in PRESENT time, computes:
```
mask = originalCullingMask | currentBit | portalBit;   // currentBit = 1<<CurrentSpaceRenderLayer
mask = (mask & ~otherBit) | currentBit | portalBit;     // otherBit  = 1<<OtherTimeSpaceRenderLayer  -> ALWAYS REMOVED
mask = otherTime ? (mask & ~playerBit) : (mask | playerBit);
```
i.e. **the other-time space layer is always culled from the main camera; only the current-space layer +
portal frame + player are shown.** The time window shows the other era only through the dedicated portal
cameras (`currentToOtherPortalCamera` / `otherToCurrentPortalCamera`,
`TimeWindowPairedSpacePortalController.cs:154-156`), which render the *other* space root into the aperture.

**Single most likely cause:** one or more late items create their content (buildings / ground / NPCs)
**parented under the wrong space root, or stamped onto the wrong render layer** (e.g. left on `Default`/0,
or put on `OtherTimeSpaceRenderLayer`). `ApplyInitialReviewLayers(currentRoot, pastRoot, …)`
(`AnemoraFastVsHouseSliceSetup.cs:942`) only re-stamps what lives under `currentRoot`/`pastRoot`. Any object
created outside those roots, or moved onto the other-time layer, will be **culled in present time → present
but invisible**, exactly the symptom. The same scramble explains the window "showing the current world"
(the other-time root no longer holds distinct other-era content / layers are crossed) while the warp
(transition physics) still fires. NPCs gone = the same culling, or a late character item
(P2-73 scale / P2-77 card LOD / P3-81 NPC outline) disabling/re-layering the sprite renderers.

Do NOT try to prove which of 30+ layer/culling-writing partials did it by reading code — that is the trap
that has already burned 17h twice. **R1 telemetry answers it in one run.**

## R0 — PRESERVE FIRST (so nothing is lost, and we finally get checkpoints)
Before editing anything, snapshot the entire dirty tree so the 80 items are recoverable and cherry-pickable:
```
git -C <repo> checkout -b wip/hd2d-snapshot-20260603
git -C <repo> add -A
git -C <repo> commit -m "snapshot: HD-2D autonomous run P0-1..P3-83 (pre-recovery-v3), tree dirty, unverified"
git -C <repo> checkout work/chapter1-continuation-map-vs-20260524   # back to the working branch (files intact)
```
This is a preservation commit on a throwaway branch — it does NOT finalize anything and does NOT push.
(It overrides the run's "never commit" rule ONLY to create a safety net; the working branch stays dirty.)

## R1 — DIAGNOSE with ONE runtime telemetry pass (do this before any fix)
Add a TEMP `[RecoveryDiag]` `MonoBehaviour` (or extend the smoke probe) that, once at t≈3s and again at
t≈20s in the **built player, normal mode, Central Plaza**, logs to `Player.log`:
- **Main camera:** `cullingMask` (as binary), and for each of these layers print `included?`:
  `CurrentSpaceRenderLayerForReview`, `OtherTimeSpaceRenderLayerForReview`, `PortalFrameRenderLayerForReview`,
  `PlayerVisibleRenderLayerForReview` (read from the portal controller's public `…ForReview` props).
- **World content sample:** find a known building root and the ground root in Central Plaza; print each
  one's `gameObject.layer`, `activeInHierarchy`, first `Renderer.enabled` + `.isVisible`, and its top
  parent name (is it under `currentRoot`?).
- **NPCs:** for every `FastVsPaperBillboard` / animated sprite character in the scene, print name,
  `gameObject.layer`, `Renderer.enabled`, `.isVisible`.
- **Time window:** `portalController.CurrentToOtherPortalCameraCullingMaskForReview`,
  `OtherToCurrentPortalCameraCullingMaskForReview`, `CurrentToOtherApertureIncludesPlayerForReview`,
  `PlayerInOtherTime`, and the names/layers of the children under the current vs other-time space roots
  (`CurrentSpaceRootForReview` / `OtherTimeSpaceRootForReview`) so we can see if the other-time root is
  empty or duplicating current content.

Run the exe, read `%USERPROFILE%\AppData\LocalLow\<company>\<product>\Player.log`. The dump tells us
exactly: are buildings/ground/NPCs on a layer the camera culls? Is content outside currentRoot? Is the
other-time space root empty/cross-layered? Decide the fix from data, not guesses.

## R2 — TARGETED FIX (driven by R1) + the bisect lever
- If R1 shows buildings/ground/NPCs on the wrong layer or outside `currentRoot`: fix the offending
  creator to parent under `currentRoot`/`pastRoot` and let `ApplyInitialReviewLayers` stamp them, OR set
  their layer to `CurrentSpaceRenderLayer`. Re-run R1 to confirm they enter the mask.
- If R1 shows the other-time space root empty / layer-crossed: fix the time-window space population so the
  portal cameras render distinct other-era content.
- **Bisect lever (fast suspect isolation):** the late camera/character items are contiguous calls in the
  pipeline — temporarily comment them and regenerate to see if the world/NPCs return:
  - `AnemoraFastVsHouseSliceSetup.cs:926` `CreateHd2dAutonomousP2GroupTargetFraming(... camera)`
  - `:931` `CreateHd2dAutonomousP3HeroNpcOutline(... camera)`  ← NPC outline; prime suspect for NPCs gone
  - `:934` `CreateHd2dAutonomousP2CharacterSpriteScaleStandard(... materials, camera)`
  - `:936` `CreateHd2dAutonomousP2CardShadowLodBudget(... camera)` ← distance-cull of cards; suspect
  Also `:942` `ApplyInitialReviewLayers(currentRoot, pastRoot, player.transform, camera)` — confirm it runs
  AFTER every content creator (it does at 942) and that all suspect content is under currentRoot/pastRoot.
  Disable suspects → regenerate → if the world/NPCs come back, re-enable one at a time to find the culprit,
  then fix that item (not blanket-delete). Keep each suspect's objects if the validator needs them.

## R3 — HARD acceptance gate (the only acceptance; do not repeat the self-accept mistake)
Build, launch `Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe` in NORMAL mode, run 20-25s, capture
t=5/15/25s, AND **actually walk Niro into the time window and place it**. Accept ONLY when, judged BY EYE
on the real frames:
1. **Ground + buildings are visible** in Central Plaza (not just present).
2. **NPC characters are visible.**
3. **The time window swaps its contents to the other era** when placed (not the current world), and Niro
   transitioning reads as a coherent era change, not a bare warp.
"No drift / no errors / telemetry stable / validation passed" is explicitly **NOT** acceptance.
If any of the three is not visibly correct, it is NOT fixed — do not accept, do not resume the backlog.

## R4 — After the gate passes: STOP for Tom + adopt checkpoint discipline
Because items 1-3 are exactly what self-review structurally cannot verify, **the backlog does not resume
until Tom confirms the runtime by eye.** This is the verification gate, not a prohibition on what may be
attempted. When work does resume:
- **Commit per accepted milestone** (or per small batch) to the working branch, so a clean rollback point
  always exists. The "never commit, leave dirty" rule caused this no-checkpoint hole — replace it with
  "commit each accepted, runtime-verified milestone."
- Camera / player / lighting / render-pipeline / layer / culling / character-visibility items stay gated
  on the 20-25s real-player runtime + (for time-window-adjacent items) an actual window placement.

## Smoke / verify steps (numbered, with expected output)
1. `Unity -batchmode -quit -projectPath . -executeMethod AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
   → expect `Fast VS house slice validation passed.` + `Build Finished, Result: Success.` (the validator
   requires the signature Cinemachine rig OBJECT to exist — keep it; only the Brain stays runtime-disabled).
2. Run the exe (normal), read `Player.log` → expect the R1 dump with buildings/ground/NPC layers INSIDE the
   camera mask, other-time space root populated with distinct content.
3. Capture t=5/15/25s + a window-placed frame → expect ground+buildings+NPCs visible AND window showing the
   other era. This is the R3 gate.
4. Remove temp telemetry, rebuild, re-capture 25s once more to confirm clean.
5. Propagate ACCEPTED capture only: `tools\r2\r2-upload-review.ps1 -CycleDir docs\review\<folder> -Branch
   work/chapter1-continuation-map-vs-20260524` (use ABSOLUTE path; verify `uploaded N files`, N>0) → ONE
   deploy hook `…/deploy_hooks/fcf2097f-a326-4e1e-af37-4c7feeb12203`.

## Open risks
- Damage may be **diffuse** (several independent late items each hiding something). If R1/bisect shows that,
  the pragmatic path is: keep the R0 snapshot, `git reset --hard e7277f0a`, and re-apply ONLY the curated
  good subset (interiors + foliage cards + town/plaza lighting + lit windows) deliberately, with a commit
  per step. Bring Tom the R1 evidence and let him pick keep-and-fix vs reset-and-recurate before doing this.
- Do NOT blunt-revert all 1145 files (loses the whole run) and do NOT delete the Cinemachine rig OBJECT
  (validator depends on it at `AnemoraFastVsHouseSliceSetup.cs:51581+`).
- Time window has a known history of culling/aperture bugs (`project_anemora_timewindow_aperture`); verify by
  EYE through an actual placement, never by a green gate.
