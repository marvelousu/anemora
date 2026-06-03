# Chapter 1 — Runtime-blank camera RECOVERY handoff (2026-06-02)

Branch `work/chapter1-continuation-map-vs-20260524`. No commit/push/PR.

## Symptom
Built player: build + 1s smoke pass, but after ~20s of real play the camera frames mostly
empty ground at a flat/side-view-ish angle — Central Plaza stage + Niro are out of frame.
Scene objects still render (so it is NOT culling); it is a **camera-positioning** failure.
Failing frame: `docs/review/2026-06-02T00-53_build_blank_runtime_probe/04_runtime_window_after_grounding_wait20s.png`.

## ROOT CAUSE (confirmed in code — two camera systems fight; Cinemachine wins and frames wrong)
1. The signature-rig item added a **`CinemachineBrain` onto the main camera**:
   `AnemoraFastVsHouseSliceSetup.cs:51252-51255` (`brain = camera.gameObject.AddComponent<CinemachineBrain>()`).
2. The area blend-rig item adds per-area **`CinemachineCamera` vcams** and drives them every
   frame from its OWN `LateUpdate`: `FastVsHd2dAreaCinemachineBlendRig.cs:53-68` →
   `ApplyCinemachineTransforms` (`:315-348`) sets each vcam to `anchor + entry.PositionOffsetForPitch(pitch)`,
   `ApplyVolumePriorities` (`:287-313`) raises the active-area vcam priority.
3. The PRE-EXISTING `FastVsVisualDirectionGuide.UpdateCamera` still drives the main camera
   directly in `Update` (`FastVsVisualDirectionGuide.cs:210-213`).
4. CinemachineBrain runs in `LateUpdate` (after `Update`), so **it overrides the guide's
   direct writes**. The Brain follows the highest-priority area vcam and **blends into it over
   seconds** → "fine at 1s, framing empty space by ~20s." The flat angle = a bad area-vcam
   offset/pitch. Additionally the guide's own target now routes through the blend rig's blended
   offset (`:170-175`) + bounds clamp (`:177-193`), so even without the Brain the guide's frame
   can be wrong. Before these two NEEDS-TOM items, the guide drove the camera alone with a fixed
   profile and worked.

Net: two unverified NEEDS-TOM camera items silently took camera authority from the working
guide and frame empty space at steady state.

## RECOVERY (restore the working guide-driven camera; PARK the Cinemachine integration for Tom)
Goal: main camera driven solely by the guide using the fixed signature profile (FOV 32 / pitch
29 — the P0-1 values Tom already saw look right). Cinemachine rig objects stay (the validator
requires them — see Risks) but lose runtime authority.

1. `FastVsVisualDirectionGuide.cs` — add a serialized switch, default OFF:
   ```csharp
   [SerializeField] private bool useAuthoredCinemachineRig = false; // PARKED: Cinemachine took camera authority and framed empty space; guide drives directly until Tom finalizes the rig
   ```
2. Neutralize the Brain so it cannot override the guide. In `ResolveReferences()` (runs each
   Update) after `reviewCamera` is resolved:
   ```csharp
   if (!useAuthoredCinemachineRig && reviewCamera != null)
   {
       var brain = reviewCamera.GetComponent<Unity.Cinemachine.CinemachineBrain>();
       if (brain != null && brain.enabled) brain.enabled = false; // guide is authoritative
   }
   ```
   (Add `using Unity.Cinemachine;` if not present.)
3. Bypass the blend rig + bounds clamp in the follow branch so the guide uses the fixed
   profile. In `UpdateCamera` `else` branch (`:162-198`), guard the blend/clamp block:
   ```csharp
   if (useAuthoredCinemachineRig && areaCameraBlendRig != null && ... ) { ... }   // existing blend
   if (useAuthoredCinemachineRig && dioramaCameraBoundsClamp != null && ... ) { ... } // existing clamp
   ```
   so when OFF, `followProfile = GetActiveFollowCameraProfile(activeArea)` (the fixed
   `cameraRigProfile`) is used unchanged and `anchor = rawAnchor` (player anchor).
4. Optionally stop the blend rig's own `LateUpdate` from moving vcams while parked (defensive):
   early-return in `FastVsHd2dAreaCinemachineBlendRig.LateUpdate` if a static
   `FastVsVisualDirectionGuide.AuthoredRigEnabledForReview` flag is false — or simply rely on
   the disabled Brain (vcams may move but no Brain = they don't drive the main camera).

## VERIFY — sustained runtime is the ONLY acceptance (the lesson from this miss)
1. `BuildAndValidateBatch` → must still pass (the validator at
   `AnemoraFastVsHouseSliceSetup.cs:51581+` REQUIRES the signature Cinemachine rig OBJECT to
   exist and the Brain TYPE to resolve — keep the objects; we only disable the Brain at
   runtime. Confirm `Fast VS house slice validation passed.`).
2. Add TEMP telemetry to `UpdateCamera` end: once/sec log `Time`, activeArea,
   `GetPlayerLocalCoordinateForReview`, `targetPosition`, `reviewCamera.transform.position`,
   `.eulerAngles`, `.fieldOfView`, and `reviewCamera.GetComponent<CinemachineBrain>()?.enabled`.
3. Run the built exe in NORMAL mode (no smoke flag) and capture the window at **t=5s, 15s,
   25s** (3 frames). ACCEPT ONLY IF: Central Plaza stage + Niro are framed and STABLE across
   all three (no drift to empty ground), and the telemetry camera pos/euler is steady after the
   initial settle. Player.log is at `%USERPROFILE%\AppData\LocalLow\<company>\<product>\Player.log`.
4. Remove the temp telemetry. Rebuild. Re-capture 25s once more to confirm.
5. Propagate: upload the probe folder to R2 + ONE deploy hook (accepted capture only).

## Open risks
- If the camera is still wrong with the Brain disabled + fixed profile, the regression is also
  in the guide's pre-blend path or elsewhere (e.g., a culling/grass item) — then instrument
  per the telemetry above and widen. But the dual-authority Brain is the confirmed primary.
- Validator dependency: do NOT delete the `FastVS_HD2D_SignatureCinemachineCamera` object or
  the brain type usage; only disable the Brain at runtime. Re-run BuildAndValidateBatch to be
  sure the parked state still validates.
- This PARKS the signature/area Cinemachine rigs (NEEDS-TOM). Tom finalizes the real
  Cinemachine integration later (single authority: either the Brain follows ONE vcam that
  tracks the guide anchor, or the guide drives directly — not both).
- Gravity was already removed from the guide movement (idle no longer drifts the player); keep
  that.
