# 2026-05-20 Fast VS HD2D Character Foot Contact Cycle

## Scope

- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-polish-20260520`
- Primary edit target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`
- Screenshot output target: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_character_foot_contact_20260520`

This cycle tightens character grounding for Niro, Reto, and Aria by adding small foot-contact ellipses with the existing transparent contact-shadow material. It does not change story, dialogue, font, controls, Time Window behavior, map transition behavior, sprite asset paths, or animation state logic. No external, Meshy, or paid assets were used.

## Implementation

Updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`:

- Added `CreateCharacterFootContactShadow(...)` as a dedicated helper that forwards to `CreateCharacterContactShadow(...)`.
- Added `FastVS_PlayerFootContact_Niro`, `Current_Library_Reto_FootContact`, and `Past_Library_Aria_FootContact` with the requested parentage, placement, scale, rotation, and material wiring.
- Added `ValidateFastVsHd2dFortyFifthCycleCharacterFootContact()` and wired it into `ValidateHouseSliceBatch()`.
- Added `CaptureHd2dFortyFifthCycleScreenshotsBatch()` and `CaptureHd2dFortyFifthCycleScreenshotsToDirectory(...)`.

Preserved existing scene objects and rejected overlays:

- `FastVS_PlayerContactShadow_Niro`
- `FastVS_PlayerGroundBounce_Niro`
- `Current_Library_Reto_ContactShadow`
- `Current_Library_Reto_GroundBounce`
- `Past_Library_Aria_ContactShadow`
- `Past_Library_Aria_GroundBounce`
- `FastVS_PlayerVisual_NiroShadingOverlay` remains absent
- `FastVS_PlayerSpriteShadingOverlay_Niro` remains absent

## Verification

Parent review:

- Reviewed the four Cycle45 screenshots and confirmed the added contact quads read as small foot-grounding shadows rather than a full-body dark overlay.
- Confirmed the screenshot capture path is repo-relative: `docs/devlog/screenshots/fast_vs_hd2d_character_foot_contact_20260520`.

Worker verification:

- Validation command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle45_worker_validate_20260520.log`
- Validation result: passed with `Fast VS house slice validation passed.`
- Capture command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortyFifthCycleScreenshotsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle45_worker_capture_20260520.log`
- Capture result: passed and wrote 4 PNGs.
- Validation log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle45_worker_validate_20260520.log`
- Capture log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle45_worker_capture_20260520.log`

Parent verification:

- Validation command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle45_parent_validate_20260520.log`
- Validation result: passed with `Fast VS house slice validation passed.`
- Capture command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.CaptureHd2dFortyFifthCycleScreenshotsBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle45_parent_capture_20260520.log`
- Capture result: passed and rewrote the four PNGs.
- Build command:
  - `C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe -batchmode -quit -projectPath C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle45_parent_build_20260520.log`
- Build result: passed with `Build Finished, Result: Success.` and updated `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`.
- EXE smoke command:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe -batchmode -nographics -logFile C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle45_parent_smoke_20260520.log`
- EXE smoke result: ran for 20 seconds, stopped by the verifier, and produced `match_count=0` for `Error|Exception|NullReference|MissingReference|Failed|Crash|Font Atlas Texture|LiberationSans|ScreenSpaceAmbientOcclusion|DrawObjectsPass|RenderGraph`.

## Screenshots

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_character_foot_contact_20260520\01_niro_interior_foot_contact.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_character_foot_contact_20260520\02_niro_exterior_foot_contact.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_character_foot_contact_20260520\03_reto_library_foot_contact.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_character_foot_contact_20260520\04_aria_past_library_foot_contact.png`

## Risks / Next Checks

- The new foot-contact quads are intentionally restrained; if a later review reads them too softly, adjust only the local scale or offset in `AnemoraFastVsHouseSliceSetup.cs`.
- The Niro exterior framing is the most likely shot to need a camera nudge if the body starts to occlude the contact ellipse in a future polish pass.
