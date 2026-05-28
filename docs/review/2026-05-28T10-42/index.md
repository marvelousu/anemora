# Fast VS HD-2D Phase A Shadow Policy Event-Driven Review

Public review set for Cycle170, covering removal of the 0.35s renderer shadow policy refresh loop.

## Images

1. `01_house_interior_sun_cycle_morning.png`
2. `02_house_exterior_sun_cycle_morning.png`
3. `03_central_plaza_sun_cycle_noon.png`
4. `04_library_sun_cycle_evening.png`

## Notes

- This cycle moves realtime renderer shadow policy application to scene load, area transition, and explicit review paths.
- The set intentionally contains only project captures for public review.
- Target HD-2D quality remains substantially below reference.
- This is not the Phase A Tom gate. The Phase A gate still needs the later 5-area screenshots and TimeWindow aperture check after the remaining Phase A implementation step.

## Build

- Build exe:
  - `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Builds\FastVS_HouseSlice\Anemora_FastVS_HouseSlice.exe`
- 起動時は `Builds\FastVS_HouseSlice\` をフォルダごと起動対象として扱う。
