# Chapter 1 — Runtime recovery v2: camera frames empty ground + Niro invisible (2026-06-02)

Branch `work/chapter1-continuation-map-vs-20260524`. No commit/push/PR. The v1 recovery
(`CHAPTER1_HD2D_CAMERA_RECOVERY_HANDOFF_20260602.md`, "park Cinemachine + fixed profile") was
applied but **did NOT fix the user-visible problem.**

## Why v1 failed (read this first)
The v1 recovery's 25s frames (`docs/review/2026-06-02T04-05_camera_recovery_final_runtime_25s/01,03`)
are **essentially identical to the original broken frame** (`…00-53_build_blank_runtime_probe/04`):
camera framing empty blue-grey ground at a flat angle, a wooden beam + the orange contact-shadow
ellipse, particles — **no Niro sprite, no plaza stage**. v1 only stopped the *drift to sky*; the
steady-state framing is still wrong. It was wrongly accepted because self-review checked "no drift /
no errors / telemetry stable" instead of **"is Niro + the stage actually visible/framed."** The run
then kept building water/foliage on this still-broken base. This is the verification gap repeating.

## Two distinct bugs
- **(A) Camera framing wrong.** v1's fixed FOV32/pitch29 guide follow profile puts the camera at
  ~`(23.35, 3.84, 7.01)`, euler `(29,0,0)` — low height + shallow 29° pitch → looking *across* the
  ground, not the top-down diorama tilt. It frames empty ground + far blurred buildings, not the
  player. The PRE-RUN guide camera (HEAD) framed correctly.
- **(B) Niro sprite invisible.** The contact-shadow overlay renders (so the player object exists at a
  framed spot), but the Niro sprite-card renderer does not. Likely the sprite is on a layer excluded
  from the camera `cullingMask`, or disabled by the shadow rig. Candidates: P0-17 char shadow,
  P1-23 rim, P1-34/35 directional normals/bake, or `FastVsRealtimeLightShadowRig` (+155 lines).

## Approach: restore from the known-good baseline (do NOT keep forward-patching)
HEAD = `e7277f0a` is the last commit **before any Codex Unity work**; camera + Niro worked there.
The whole run is uncommitted (766 dirty files), so a targeted revert is possible WITHOUT losing the
55-item body of work — revert only the camera-framing + player-visibility regressions.

1. **See exactly what changed vs working:**
   ```
   git -C <repo> diff HEAD -- Assets/Scripts/FastVS/FastVsVisualDirectionGuide.cs \
     Assets/Scripts/FastVS/FastVsRealtimeLightShadowRig.cs \
     Assets/Scripts/FastVS/FastVsPaperBillboard.cs
   git -C <repo> show HEAD:Assets/Scripts/FastVS/FastVsVisualDirectionGuide.cs   # original working camera
   ```
   Also check the player setup in the editor:
   ```
   git -C <repo> diff HEAD -- Assets/Editor/AnemoraFastVsHouseSliceSetup.cs | grep -niE "niro|CreateNiroPlayer|CreateSpriteCard|layer|cullingMask|PlayerVisible"
   ```
2. **Restore (A) camera framing** to HEAD's working follow camera. The rig/blend/clamp additions are
   parked NEEDS-TOM anyway, so reverting the guide's camera-positioning to the HEAD logic is low-risk.
   Keep compilation intact: the editor `CreateGuide(...)` call signature and the guide's serialized
   fields must match — revert/adjust them together. Do NOT re-enable the `CinemachineBrain` authority.
3. **Restore (B) Niro visibility.** From the diff, find what hides the sprite (layer move / renderer
   disable / material) and revert that specific change so the Niro sprite-card renders again. Verify
   the sprite's layer is inside the camera `cullingMask` (see `ApplyCameraCulling`,
   `FastVsVisualDirectionGuide.cs:385-409`, current time adds `playerBit`).
4. Keep everything else (foliage, water, dressing, VFX, interiors, doors) intact.

## Telemetry (mandatory — pinpoints both bugs)
Per-second `Debug.Log` for 25s: player world position; **Niro sprite renderer `.enabled`,
`.gameObject.layer`, `.isVisible`, `.bounds`**; camera `cullingMask` and `((mask>>niroLayer)&1)`;
camera position/euler/fov; whether the player is inside the camera frustum. Player.log at
`%USERPROFILE%\AppData\LocalLow\<company>\<product>\Player.log`.

## HARD acceptance gate (the lesson — do not repeat v1's mistake)
Accept ONLY when a 20-25s built-player run (normal mode, t=5/15/25s) shows **Niro's sprite AND the
plaza stage clearly visible and properly framed (diorama tilt, player roughly centered)** — judged BY
EYE on the actual frames. "No drift / no errors / telemetry stable" is explicitly **NOT** acceptance.
If Niro or the stage is not clearly visible, it is NOT fixed: do not accept, do not resume the backlog.

## After the gate passes (per CHAPTER1_HD2D_AUTONOMOUS_BACKLOG.md PLAN UPDATE v2)
1. **P2 re-validation BEFORE new features:** 20-25s 5-area current/past run; confirm which of the ~30
   items built on the broken runtime actually hold; triage regressions.
2. Then resume the backlog under the sustained-runtime gate (camera/player/lighting/pipeline items
   accepted only via 20-25s real-player runtime, no regression).

## Risks
- Do NOT blunt-revert all 766 files (loses the whole run). Targeted revert of camera + player only.
- Keep `BuildAndValidateBatch` passing — the validator requires the Cinemachine rig OBJECTS to exist
  (`AnemoraFastVsHouseSliceSetup.cs:51581+`); keep them, just don't give the Brain authority.
- Propagate accepted captures only, one deploy hook (manifest is capped to recent 40).
