# Fast VS HD2D Exterior Occlusion Backdrop Cycle 27

- Result: PASS
- Branch: `work/fast-vs-hd2d-shading-foundation-20260522`
- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Report path: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_exterior_occlusion_backdrop_cycle27_20260522\exterior_occlusion_backdrop_cycle27_20260522.md`
- House occlusion object count: `16`
- Plaza occlusion object count: `14`
- Backdrop object count: `8`

## Scene Objects

### Current house occlusion
- `Current_HouseExterior_OcclusionShell_BackPlate`
- `Current_HouseExterior_OcclusionShell_LeftReturnWall`
- `Current_HouseExterior_OcclusionShell_RightReturnWall`
- `Current_HouseExterior_OcclusionShell_RoofDepthCap`
- `Current_HouseExterior_OcclusionShell_UnderEaveMask`
- `Current_HouseExterior_OcclusionShell_DoorwayDarkMask`
- `Current_HouseExterior_OcclusionShell_DoorJambFillLeft`
- `Current_HouseExterior_OcclusionShell_DoorJambFillRight`

### Past house occlusion
- `Past_HouseExterior_OcclusionShell_BackPlate`
- `Past_HouseExterior_OcclusionShell_LeftReturnWall`
- `Past_HouseExterior_OcclusionShell_RightReturnWall`
- `Past_HouseExterior_OcclusionShell_RoofDepthCap`
- `Past_HouseExterior_OcclusionShell_UnderEaveMask`
- `Past_HouseExterior_OcclusionShell_DoorwayDarkMask`
- `Past_HouseExterior_OcclusionShell_DoorJambFillLeft`
- `Past_HouseExterior_OcclusionShell_DoorJambFillRight`

### Current plaza occlusion
- `Current_CentralPlaza_LibraryOcclusionShell_BackVolume`
- `Current_CentralPlaza_LibraryOcclusionShell_WestSideReturn`
- `Current_CentralPlaza_LibraryOcclusionShell_EastSideReturn`
- `Current_CentralPlaza_LibraryOcclusionShell_RoofBackCap`
- `Current_CentralPlaza_LibraryOcclusionShell_UnderEaveDepthMask`
- `Current_CentralPlaza_LibraryOcclusionShell_WindowBackingLeft`
- `Current_CentralPlaza_LibraryOcclusionShell_WindowBackingRight`

### Past plaza occlusion
- `Past_CentralPlaza_LibraryOcclusionShell_BackVolume`
- `Past_CentralPlaza_LibraryOcclusionShell_WestSideReturn`
- `Past_CentralPlaza_LibraryOcclusionShell_EastSideReturn`
- `Past_CentralPlaza_LibraryOcclusionShell_RoofBackCap`
- `Past_CentralPlaza_LibraryOcclusionShell_UnderEaveDepthMask`
- `Past_CentralPlaza_LibraryOcclusionShell_WindowBackingLeft`
- `Past_CentralPlaza_LibraryOcclusionShell_WindowBackingRight`

### Current house backdrop
- `Current_HouseExterior_BackdropFoundation_SkyBackPlane`
- `Current_HouseExterior_BackdropFoundation_HorizonTreeLine`

### Past house backdrop
- `Past_HouseExterior_BackdropFoundation_SkyBackPlane`
- `Past_HouseExterior_BackdropFoundation_HorizonTreeLine`

### Current plaza backdrop
- `Current_CentralPlaza_BackdropFoundation_SkyBackPlane`
- `Current_CentralPlaza_BackdropFoundation_HorizonRoofline`

### Past plaza backdrop
- `Past_CentralPlaza_BackdropFoundation_SkyBackPlane`
- `Past_CentralPlaza_BackdropFoundation_HorizonRoofline`

## Validation Notes

- PASS: the exterior shells block internal/behind-the-scenes visibility and the backdrop foundation gives the map a grounded horizon.
