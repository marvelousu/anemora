# 2026-05-22 Fast VS HD2D Character Directional Shadow Cycle 05

Scope: Fast VS / HD2D character directional cast shadow pass.

Branch:

- `work/fast-vs-hd2d-shading-foundation-20260522`

Worktree:

- `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`

## Goal

The earlier cycles already established URP, SSAO, sprite-card lighting, and lighting profile transitions, but the characters still read mainly through round contact shadows, foot contact, and bounce. This cycle adds a thin directional cast shadow layer so the characters lean into the scene lighting direction without losing the existing grounding cues.

## Implementation

`AnemoraFastVsHouseSliceSetup` now creates a new shared directional-cast shadow material and generated texture for the HD2D character layer, then places it under each character that needed the stronger directional cue.

Added scene objects:

- `FastVS_PlayerDirectionalCastShadow_Niro` under `FastVS_Player_NiroHouseSlice`
- `Current_Library_Reto_DirectionalCastShadow` under `Current_LibraryMap_SeparateSpace`
- `Past_Library_Aria_DirectionalCastShadow` under `Past_LibraryMap_SeparateSpace`

The new helper keeps the original round contact shadow, foot contact, and ground bounce objects intact. The directional shadow is a separate transparent unlit quad with a thin soft alpha shape, rotated onto the ground plane and yawed to match the house slice lighting direction.

`AnemoraFastVsHd2dMaterialRoleFoundationAudit` now accepts `FastVS_House_character_directional_cast_shadow` as a contact-shadow role asset.

## Validation

Commands run:

```powershell
git diff --check -- . ':(exclude)Assets/Scenes/Anemora_FastVS_HouseSlice.unity'
```

Result:

- Pass

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -quit -projectPath 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work' -executeMethod Anemora.EditorTools.AnemoraFastVsHouseSliceSetup.ValidateHouseSliceBatch -logFile 'C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_directional_shadow_cycle05_validate_worker_20260522.log'
```

Result:

- Pass: `Fast VS house slice validation passed.`
- Log: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Logs\fast_vs_hd2d_character_directional_shadow_cycle05_validate_worker_20260522.log`

## Cleanup

Unity side effects on existing scene/project settings and unrelated material metadata were reverted after validation. The new directional shadow material and PNG texture asset remain as the intended generated outputs for this cycle.
