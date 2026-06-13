# 2026-06-09 HD2D point15 table object removal

## Context

- Project: Anemora Fast VS House Slice HD-2D recovery.
- Worktree: `C:\Users\maro6\Documents\Unity\Anemora-p15-recovery-20260609`.
- Branch: `wip/hd2d-point15-recovery-20260609`.
- Baseline: point15 `e7277f0a`.
- Slice: C table object removal.
- User correction: remove the strange table stripe object itself, not only its striped texture.
- Review correction: `anemora-viewer` is the review-image surface. Local `docs/review` evidence is not enough; every propagated slice must be uploaded to R2 and verified on the viewer page.

## Changes

- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
  - Removed generation of the past long-table book pair objects:
    - `Past_Library_PropDetail_LongTableBookPairA`
    - `Past_Library_PropDetail_LongTableBookPairB`
    - their `_Accent`, `_Detail`, and `_Slip` children.
  - Replaced visible `Past_Library_TargetBook_ForPickup` with a rendererless invisible story anchor at the same local position.
  - Removed past table, floor, shelf, and ledger book objects that read as stray striped slabs:
    - `Past_Library_ReadingSurfaceDensity_LongTableOrderBookA`
    - `Past_Library_ReadingSurfaceDensity_LongTableOrderBookB`
    - `Past_Library_ReadingSurfaceDensity_SideTableOrderStackA`
    - `Past_Library_ReadingSurfaceDensity_SideTableOrderStackB`
    - `Past_Library_ReadingTableClean_*_BookA/B/C`
    - `Past_Library_OrderedFloorDetail_BookBundleA`
    - `Past_Library_OrderedFloorDetail_BookBundleC`
    - `Past_Library_ReadableMicroprops_TableOpenBook_LeftFront`
    - `Past_Library_ReadableMicroprops_TableOpenBook_LeftFront_Detail`
    - `Past_Library_ReadableMicroprops_TableClosedBook_CenterRear`
    - `Past_Library_ReadableMicroprops_TableClosedBook_CenterRear_Detail`
    - `Past_Library_ReadableMicroprops_LeftShelfLedgerA`
    - `Past_Library_ReadableMicroprops_RightShelfLedgerA`
    - `Past_Library_PropDetail_ShelfLedgerWest`
    - `Past_Library_PropDetail_ShelfLedgerWest_Accent`
    - `Past_Library_PropDetail_ShelfLedgerWest_Detail`
    - `Past_Library_PropDetail_ShelfLedgerWest_Slip`
    - `Past_Library_ReadingTableGrounding_LeftBookContactA`
    - `Past_Library_ReadingTableGrounding_RightBookContactA`
    - `Past_Library_EntryTableContrast_LeftTableBookLineA`
    - `Past_Library_EntryTableContrast_RightTableBookLineA`
  - Removed past bookshelf front texture panels after they also read as stripe plates:
    - `Past_Library_BackWallBookshelfFrontTexturePanel`
    - `Past_Library_LeftSideBookshelf_BookshelfFrontTexturePanel`
    - `Past_Library_RightSideBookshelf_BookshelfFrontTexturePanel`
  - Removed current-side non-story book slabs that leaked into the past close-up:
    - `Current_Library_TableOpenBook`
    - `Current_Library_Ruin_ToppledBookStack`
    - `Current_Library_Ruin_FallenBookSpines`
    - `Current_Library_ReadableMicroprops_FloorOpenBookA`
    - `Current_Library_ReadableMicroprops_FloorOpenBookA_Detail`
    - `Current_Library_ReadableMicroprops_LeftShelfLooseBookA`
    - `Current_Library_ReadableMicroprops_RightShelfLooseBookA`
    - `Current_Library_ReadingSurfaceDensity_LongTableDustBookA`
    - `Current_Library_ReadingSurfaceDensity_SideTableFallenBookA`
    - `Current_Library_Stage8j_LongTableReadableOpenBookA`
    - `Current_Library_Stage8m_RightDeskShelfEchoBookA`
    - `Current_Library_Stage8n_RightDeskStackBookA`
    - `Current_Library_Stage8n_FloorClosedBookA`
    - `Current_Library_Stage8b_TableSideA_ColorStackA`
    - `Current_Library_Stage8b_TableSideB_ColorStackA`
    - `Current_Library_Stage8h_LongTableColorBookA`
  - Kept story-critical current desk book objects:
    - `Current_Library_RetoDeskBook_Initial`
    - `Current_Library_ReturnedBookOnDesk`
- `Assets/Scripts/FastVS/FastVsHouseRuntimeSmokeProbe.cs`
  - Added a built-player table-object capture mode.
  - Logged removed-object status using inactive-aware scene lookup.
  - Logged the renderer contract in the same built-player run.

## Build Evidence

- Build/validate log: `Logs/point15_table_object_removed12_build_validate_20260609T073509.log`
  - `Fast VS house slice validation passed.`
  - `DisplayProgressNotification: Build Successful`
  - `Build Finished, Result: Success.`
  - `PlayerBuildInfo duration=91372`

## Built-Player Evidence

- Review folder: `docs/review/2026-06-09T07-43_table_object_removed_final/`
- Built-player capture log: `Logs/point15_table_object_removed12_runtime_capture_direct_20260609T074336.log`
- PNG files:
  - `01_past_library_long_table_wide.png` (519623 bytes)
  - `02_past_library_long_table_pair_a_close.png` (581179 bytes)
  - `03_past_library_long_table_pair_b_close.png` (693872 bytes)

Verbatim renderer contract:

```text
ANEMORA_HOUSE_SLICE_RENDERER_CONTRACT: pipeline=UniversalRenderPipeline renderer=UniversalRenderPipeline_Renderer RenderingMode=2 DepthPrimingMode=0 CopyDepthMode=0 PortalStencilFeatureActive=True features=[0:PortalStencilFeature(PortalStencilFeature):active; 1:FastVS HD2D Soft Contact Occlusion(ScreenSpaceAmbientOcclusion):active; 2:FastVS HD2D Stage7 TiltShift(FullScreenPassRendererFeature):active; 3:FastVS HD2D Stage7 Outline(FullScreenPassRendererFeature):active] error=<none>
```

Verbatim table-object status:

```text
ANEMORA_HOUSE_SLICE_TABLE_OBJECT_CAPTURE: status Past_Library_PropDetail_LongTableBookPairA=missing; Past_Library_PropDetail_LongTableBookPairA_Accent=missing; Past_Library_PropDetail_LongTableBookPairA_Detail=missing; Past_Library_PropDetail_LongTableBookPairA_Slip=missing; Past_Library_PropDetail_LongTableBookPairB=missing; Past_Library_PropDetail_LongTableBookPairB_Accent=missing; Past_Library_PropDetail_LongTableBookPairB_Detail=missing; Past_Library_PropDetail_LongTableBookPairB_Slip=missing; Past_Library_TargetBook_ForPickup=present-inactive; Past_Library_OrderedFloorDetail_BookBundleA=missing; Past_Library_OrderedFloorDetail_BookBundleC=missing; Past_Library_BackWallBookshelfFrontTexturePanel=missing; Past_Library_LeftSideBookshelf_BookshelfFrontTexturePanel=missing; Past_Library_RightSideBookshelf_BookshelfFrontTexturePanel=missing; Past_Library_ReadingSurfaceDensity_LongTableOrderBookA=missing; Past_Library_ReadingSurfaceDensity_LongTableOrderBookB=missing; Past_Library_ReadingSurfaceDensity_SideTableOrderStackA=missing; Past_Library_ReadingSurfaceDensity_SideTableOrderStackB=missing; Past_Library_PropDetail_ShelfLedgerWest=missing; Past_Library_PropDetail_ShelfLedgerWest_Accent=missing; Past_Library_PropDetail_ShelfLedgerWest_Detail=missing; Past_Library_PropDetail_ShelfLedgerWest_Slip=missing; Past_Library_ReadableMicroprops_TableOpenBook_LeftFront=missing; Past_Library_ReadableMicroprops_TableOpenBook_LeftFront_Detail=missing; Past_Library_ReadableMicroprops_TableClosedBook_CenterRear=missing; Past_Library_ReadableMicroprops_TableClosedBook_CenterRear_Detail=missing; Past_Library_ReadableMicroprops_LeftShelfLedgerA=missing; Past_Library_ReadableMicroprops_RightShelfLedgerA=missing; Current_Library_TableOpenBook=missing; Current_Library_Ruin_ToppledBookStack=missing; Current_Library_Ruin_FallenBookSpines=missing; Current_Library_ReadableMicroprops_FloorOpenBookA=missing; Current_Library_ReadableMicroprops_FloorOpenBookA_Detail=missing; Current_Library_ReadableMicroprops_LeftShelfLooseBookA=missing; Current_Library_ReadableMicroprops_RightShelfLooseBookA=missing; Current_Library_ReadingSurfaceDensity_LongTableDustBookA=missing; Current_Library_ReadingSurfaceDensity_SideTableFallenBookA=missing; Current_Library_Stage8j_LongTableReadableOpenBookA=missing; Current_Library_Stage8j_LongTableReadableOpenBookA_Cover=missing; Current_Library_Stage8j_LongTableReadableOpenBookA_Pages=missing; Current_Library_Stage8j_LongTableReadableOpenBookA_Spine=missing; Current_Library_Stage8m_RightDeskShelfEchoBookA=missing; Current_Library_Stage8n_RightDeskStackBookA=missing; Current_Library_Stage8n_FloorClosedBookA=missing; Current_Library_Stage8b_TableSideA_ColorStackA=missing; Current_Library_Stage8b_TableSideB_ColorStackA=missing; Current_Library_Stage8h_LongTableColorBookA=missing; Past_Library_ReadingTableGrounding_LeftBookContactA=missing; Past_Library_ReadingTableGrounding_RightBookContactA=missing; Past_Library_EntryTableContrast_LeftTableBookLineA=missing; Past_Library_EntryTableContrast_RightTableBookLineA=missing; Past_Library_ReadingTableClean_LeftFront_BookA=missing; Past_Library_ReadingTableClean_LeftFront_BookB=missing; Past_Library_ReadingTableClean_LeftFront_BookC=missing; Past_Library_ReadingTableClean_CenterFront_BookA=missing; Past_Library_ReadingTableClean_CenterFront_BookB=missing; Past_Library_ReadingTableClean_CenterFront_BookC=missing; Past_Library_ReadingTableClean_RightFront_BookA=missing; Past_Library_ReadingTableClean_RightFront_BookB=missing; Past_Library_ReadingTableClean_RightFront_BookC=missing; Past_Library_ReadingTableClean_LeftRear_BookA=missing; Past_Library_ReadingTableClean_LeftRear_BookB=missing; Past_Library_ReadingTableClean_LeftRear_BookC=missing; Past_Library_ReadingTableClean_CenterRear_BookA=missing; Past_Library_ReadingTableClean_CenterRear_BookB=missing; Past_Library_ReadingTableClean_CenterRear_BookC=missing; Past_Library_ReadingTableClean_RightRear_BookA=missing; Past_Library_ReadingTableClean_RightRear_BookB=missing; Past_Library_ReadingTableClean_RightRear_BookC=missing; Past_Library_TargetBook_ForPickup_RendererCount=0
```

Verbatim capture completion:

```text
ANEMORA_HOUSE_SLICE_TABLE_OBJECT_CAPTURE: end count=3
```

## Visual Review

- `03_past_library_long_table_pair_b_close.png` no longer shows the central-right multicolor stripe slab that remained in the 07:31 capture.
- `01_past_library_long_table_wide.png` and `02_past_library_long_table_pair_a_close.png` no longer show the removed table book objects.
- The right-side white/pale highlight visible in the wide/side images is a separate existing paper/highlight surface, not the rejected multicolor stripe object.

## Viewer Status

- Before correction, `anemora-viewer` search for `2026-06-09T07-07` returned `0 / 251` and `No albums match.`
- The earlier `2026-06-09T04-12_allmaps` baseline was visible in `anemora-viewer` as `1 / 251`, proving the viewer can show R2-backed cycles after upload and build collection.
- R2 upload for `docs/review/2026-06-09T07-43_table_object_removed_final` succeeded for both slugs:
  - `uploaded 5 files for chapter1-continuation-map-vs-20260524/2026-06-09T07-43_table_object_removed_final (bucket TTL 45d); manifest now lists 24 paths`
  - `uploaded 5 files for wip-snapshot-repair-proof-20260603/2026-06-09T07-43_table_object_removed_final (bucket TTL 45d); manifest now lists 24 paths`
- Direct R2 manifest check:
  - `WORK_HAS_0743=True`
  - `WORK_PATH_COUNT=24`
  - `WIP_HAS_0743=True`
  - `WIP_PATH_COUNT=24`
- Cloudflare Pages deploy hook response:
  - `success=true`
  - `result.id=e1cbbfe9-fc40-4989-b877-1e61ce384812`
- Verified live on `anemora-viewer`:
  - URL: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/review/?check=20260609T0743c`
  - Header: `252 cycles · 1175 images`
  - Search: `2026-06-09T07-43`
  - Result count: `1 / 252`
  - Album card: `2026-06-09T07-43_table_object_removed_final`
  - Album path: `docs/review/2026-06-09T07-43_table_object_removed_final`
  - Album count: `3 images · 11 min ago`
- Verified gallery page:
  - URL: `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-09T07-43_table_object_removed_final/`
  - Header: `docs/review/2026-06-09T07-43_table_object_removed_final`
  - Count: `3 images`
  - Visible thumbnails:
    - `01_past_library_long_table_wide.png`
    - `02_past_library_long_table_pair_a_close.png`
    - `03_past_library_long_table_pair_b_close.png`

## Viewer Recheck After User Report

User reported that `anemora-viewer` still did not appear updated. Rechecked live, not local files:

- `https://anemora-viewer.pages.dev/` showed `1 active branch`.
- Active branch: `work/chapter1-continuation-map-vs-20260524`.
- `https://anemora-viewer.pages.dev/wip-snapshot-repair-proof-20260603/review/` returned `404`; the `wip/*` slug is not an active viewer branch.
- Live review list, with cache-bust `?verify=20260609Tposthydration`:
  - Header: `252 cycles · 1175 images`
  - First matching album text: `2026-06-09T07-43_table_object_removed_finaldocs/review/2026-06-09T07-43_table_object_removed_final3 images · 54 min ago`
  - First matching album href: `/chapter1-continuation-map-vs-20260524/gallery/docs/review/2026-06-09T07-43_table_object_removed_final`
  - Gallery fetch status: `200`
- Live gallery image fetches, no-store:
  - `01_past_library_long_table_wide.png`: `status=200`, `contentType=image/png`, `bytes=519623`
  - `02_past_library_long_table_pair_a_close.png`: `status=200`, `contentType=image/png`, `bytes=581179`
  - `03_past_library_long_table_pair_b_close.png`: `status=200`, `contentType=image/png`, `bytes=693872`

Found and fixed a viewer/devlog propagation gap:

- Original live gallery had the review images, but the `devlog:` link returned `404` because root `docs/devlog/*.md` was not mirrored into R2.
- Updated `tools/r2/r2-upload-review.ps1` so a `docs/devlog/*.md` referenced by `devlog.txt` is also uploaded and added to the manifest.
- Re-uploaded work slug:
  - `uploaded 6 files for chapter1-continuation-map-vs-20260524/2026-06-09T07-43_table_object_removed_final (bucket TTL 45d); manifest now lists 25 paths`
- Direct R2 manifest after re-upload:
  - `chapter1-continuation-map-vs-20260524 count=25`
  - includes `docs/devlog/2026-06-09_hd2d_point15_table_object_removal.md`
  - includes all five cycle-local files under `docs/review/2026-06-09T07-43_table_object_removed_final/`
- `wip-snapshot-repair-proof-20260603` re-upload timed out and remained `count=24`; stopped the leftover wrangler node processes. This slug is not live in viewer.

Viewer repo fixes pushed:

- `c49e7a3 fix(build): mirror devlog markdown from R2`
  - Windows-local build collection no longer relies on `bash -lc` path quoting.
  - R2 setup now accepts safe root `docs/devlog/*.md` files.
- `99051b4 fix(review): avoid relative-time hydration drift`
  - Review album relative times are computed after mount, removing React hydration drift.

Viewer deploy evidence:

- Deploy hook after devlog mirror: `57fa2c39-bad0-4115-a59a-591fa7e63b64`
- Final deploy hook after hydration fix: `59386fac-33b6-474f-82c7-535b3a9a6763`
- Live chunk changed from `GalleryAlbumList.BlYBkrE0.js` to `GalleryAlbumList.B9iOIRLZ.js`.
- Final Playwright navigation to review URL reported `Console: 0 errors, 1 warnings`.
- Gallery `devlog:` href:
  - `/chapter1-continuation-map-vs-20260524/docs/docs/devlog/2026-06-09_hd2d_point15_table_object_removal`
  - fetch status: `200`
- Clicking the `devlog:` link navigated to:
  - `https://anemora-viewer.pages.dev/chapter1-continuation-map-vs-20260524/docs/docs/devlog/2026-06-09_hd2d_point15_table_object_removal/`
  - page title: `docs/devlog/2026-06-09_hd2d_point15_table_object_removal.md — Docs`
