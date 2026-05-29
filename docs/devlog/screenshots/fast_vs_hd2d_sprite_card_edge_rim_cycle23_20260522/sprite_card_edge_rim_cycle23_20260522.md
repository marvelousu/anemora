# Fast VS HD2D Sprite Card Edge Rim Cycle 23 Report

Deterministic paper-edge and rim shading foundation for the Fast VS sprite cards used by Niro, Reto, and Aria. The goal is to keep the existing ramp lighting intact while making the paper sprites read less like flat pasted cutouts.

- Project root: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Report file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_sprite_card_edge_rim_cycle23_20260522\sprite_card_edge_rim_cycle23_20260522.md`
- Shader: `Anemora/FastVS/SpriteCardRampUnlit`
- Result: PASS

## Representative Materials

| Material | Material Path | Texture Name | Texture Path | Shader | Render Queue | Result |
|---|---|---|---|---|---:|---|
| `niro_front_sprite` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_niro_front_sprite.mat` | `FastVS_House_niro_front_sprite_shaded` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_niro_front_sprite_shaded.asset` | `Anemora/FastVS/SpriteCardRampUnlit` | 3000 | PASS |
| `niro_walk_front_sprite` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_niro_walk_front_sprite.mat` | `FastVS_House_niro_walk_front_sprite_shaded` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_niro_walk_front_sprite_shaded.asset` | `Anemora/FastVS/SpriteCardRampUnlit` | 3000 | PASS |
| `reto_v02_writing_loop_sprite` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_reto_v02_writing_loop_sprite.mat` | `FastVS_House_reto_v02_writing_loop_sprite_shaded` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_reto_v02_writing_loop_sprite_shaded.asset` | `Anemora/FastVS/SpriteCardRampUnlit` | 3000 | PASS |
| `aria_v46_normal_loop_breath_sprite` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_aria_v46_normal_loop_breath_sprite.mat` | `FastVS_House_aria_v46_normal_loop_breath_sprite_shaded` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_aria_v46_normal_loop_breath_sprite_shaded.asset` | `Anemora/FastVS/SpriteCardRampUnlit` | 3000 | PASS |

## Property Values

| Material | Ramp Strength | Paper Edge | Paper Rim | Paper Lower Shade | Top Light | Side Shade | Floor Shade |
|---|---:|---:|---:|---:|---|---|---|
| `niro_front_sprite` | 0.180 | 0.100 | 0.070 | 0.080 | (1.080, 1.030, 0.960, 1.000) | (0.940, 0.970, 1.030, 1.000) | (0.890, 0.920, 0.960, 1.000) |
| `niro_walk_front_sprite` | 0.180 | 0.100 | 0.070 | 0.080 | (1.080, 1.030, 0.960, 1.000) | (0.940, 0.970, 1.030, 1.000) | (0.890, 0.920, 0.960, 1.000) |
| `reto_v02_writing_loop_sprite` | 0.180 | 0.100 | 0.070 | 0.080 | (1.080, 1.030, 0.960, 1.000) | (0.940, 0.970, 1.030, 1.000) | (0.890, 0.920, 0.960, 1.000) |
| `aria_v46_normal_loop_breath_sprite` | 0.180 | 0.100 | 0.070 | 0.080 | (1.080, 1.030, 0.960, 1.000) | (0.940, 0.970, 1.030, 1.000) | (0.890, 0.920, 0.960, 1.000) |
