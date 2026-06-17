# HD2D environment asset persistence

Area: Fast VS / HD2D
Branch: `wip/hd2d-point15-recovery-20260612`
Date: 2026-06-17

## Context

- After the nature realism stands cycle, the working tree still contained generated environment material and texture assets that were referenced by the current setup code but not tracked.
- Leaving those assets local-only would make a clean checkout depend on Unity regenerating the same material and texture files, which breaks the approved-asset discipline and makes review/build reproducibility weaker.
- The affected assets were limited to `Assets/Art/Materials/FastVS/HouseSlice/` and `Assets/Art/Textures/FastVS/HouseSlice/`.

## Change

- Persisted the generated distant vista, production-depth, waterline, foreground breakup, nearfield dressing, and terrain surface quilt material/texture assets.
- Kept the 2K terrain surface quilt PNG sources, but recompressed them so every tracked PNG stays under the 5 MB bloat guard limit.
- No renderer feature, scene, gameplay, or generated review image change was intended in this commit.

## Verification

- Asset validation: `Logs/asset_persist_asset_validation_r1.log` passed with `[AssetValidation] OK` and return code 0.
- Unity side effects: `Assets/AddressableAssetsData/link.xml` and `.meta` were restored after the batch validation side effect.

## Next

- Continue the natural graphics uplift with higher-fidelity distant and near/mid-distance tree forms.
- Keep committing accepted generated assets immediately so the viewer, R2 review packets, and clean checkouts stay aligned.
