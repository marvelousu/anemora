# 2026-05-23 Fast VS HD2D Plaza Library Roof Underside Shadow Cycle 59

## Scope

- Project: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Scene: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Scenes\Anemora_FastVS_HouseSlice.unity`
- Setup source: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Editor\AnemoraFastVsHouseSliceSetup.cs`

Cycle59 continued the plaza library exterior work after Cycle58 side relief. The goal was to keep the shadow direction close to `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_close_review_20260520` while adding roof-under and side-wall depth without making the building look like it has black holes.

## Implementation

Added `CreateCentralPlazaLibraryRoofUndersideShadowCycle59(...)` and called it after `CreateCentralPlazaLibrarySideReliefCycle58(...)`.

The new pass adds current/past non-arrival visual pieces for:

- long eave underside depth;
- roof cast bands under the red roof mass;
- side-wall cool falloff strips;
- lower base occlusion strips.

The initial worker implementation used `materials.Shadow` for all shadow strips. Parent visual review rejected that as too dark because the oblique screenshots showed large black bands along the far side. The final version uses `EnsureHd2dDepthShadowMaterial()` for broad wall/eave falloff and keeps the black shadow material out of the broad surfaces.

## Verification

Structural validation:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle59_plaza_library_roof_underside_shadow_validate_parent_20260523_retry2.log`
- Result: `Fast VS house slice validation passed.`

Oblique screenshot capture:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_cycle59_plaza_library_roof_underside_shadow_oblique_parent_20260523_retry2.log`
- Result: `Fast VS cycle57 plaza library oblique screenshots captured: C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle57_plaza_library_oblique_review_20260523`

Cycle59 evidence copies:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle59_plaza_library_roof_underside_shadow_parent_review_20260523_01\01_current_plaza_library_roof_underside_shadow_left_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle59_plaza_library_roof_underside_shadow_parent_review_20260523_01\02_past_plaza_library_roof_underside_shadow_left_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle59_plaza_library_roof_underside_shadow_parent_review_20260523_01\03_current_plaza_library_roof_underside_shadow_right_oblique.png`
- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_cycle59_plaza_library_roof_underside_shadow_parent_review_20260523_01\04_past_plaza_library_roof_underside_shadow_right_oblique.png`

## Result

The final screenshots keep the plaza library side mass readable without the black-edge artifact from the first attempt. The change is still a foundation pass: it improves depth and underside read, but later cycles should keep building real side-wall material variation and background composition.
