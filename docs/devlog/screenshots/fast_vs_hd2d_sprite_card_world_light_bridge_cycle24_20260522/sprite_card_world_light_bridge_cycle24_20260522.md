# Fast VS HD2D Sprite Card World Light Bridge Cycle 24 Report

Restraint-first bridge from the current sprite-card ramp into scene lighting for Niro, Reto, Aria, and the sprite-card vegetation. The goal is to keep the existing paper-edge and rim shading while letting the cards read a small amount of URP main-light color and shadow attenuation.

- Project root: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work`
- Report file: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\docs\devlog\screenshots\fast_vs_hd2d_sprite_card_world_light_bridge_cycle24_20260522\sprite_card_world_light_bridge_cycle24_20260522.md`
- Shader: `Anemora/FastVS/SpriteCardRampUnlit`
- Result: PASS

## Representative Materials

| Material | Material Path | Texture Name | Texture Path | Shader | Render Queue | Ramp Strength | Paper Edge | Paper Rim | Paper Lower Shade | World Light | World Shadow Receive | Result |
|---|---|---|---|---|---:|---:|---:|---:|---:|---:|---:|---|
| `niro_front_sprite` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_niro_front_sprite.mat` | `FastVS_House_niro_front_sprite_shaded` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_niro_front_sprite_shaded.asset` | `Anemora/FastVS/SpriteCardRampUnlit` | 3000 | 0.180 | 0.100 | 0.070 | 0.080 | 0.080 | 0.050 | PASS |
| `niro_walk_front_sprite` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_niro_walk_front_sprite.mat` | `FastVS_House_niro_walk_front_sprite_shaded` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_niro_walk_front_sprite_shaded.asset` | `Anemora/FastVS/SpriteCardRampUnlit` | 3000 | 0.180 | 0.100 | 0.070 | 0.080 | 0.080 | 0.050 | PASS |
| `reto_v02_writing_loop_sprite` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_reto_v02_writing_loop_sprite.mat` | `FastVS_House_reto_v02_writing_loop_sprite_shaded` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_reto_v02_writing_loop_sprite_shaded.asset` | `Anemora/FastVS/SpriteCardRampUnlit` | 3000 | 0.180 | 0.100 | 0.070 | 0.080 | 0.080 | 0.050 | PASS |
| `aria_v46_normal_loop_breath_sprite` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Materials\FastVS\HouseSlice\FastVS_House_aria_v46_normal_loop_breath_sprite.mat` | `FastVS_House_aria_v46_normal_loop_breath_sprite_shaded` | `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-hd2d-work\Assets\Art\Textures\FastVS\HouseSlice\FastVS_House_aria_v46_normal_loop_breath_sprite_shaded.asset` | `Anemora/FastVS/SpriteCardRampUnlit` | 3000 | 0.180 | 0.100 | 0.070 | 0.080 | 0.080 | 0.050 | PASS |
