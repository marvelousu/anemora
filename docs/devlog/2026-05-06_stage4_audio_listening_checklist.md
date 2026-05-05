# Stage 4 Audio Listening Checklist

Date: 2026-05-06

Scope: user-guided review checklist for Stage 4 audio polish. This file does not change audio assets, scenes, prefabs, runtime code, or asset ledger rows.

Inputs:

- `docs/devlog/2026-05-06_stage4_audio_polish_inventory.md`
- `docs/STAGE4_PHASE0_TRIAGE.md`
- `docs/STAGE4_ROADMAP.md`
- `docs/legal/asset_ledger.md` audio rows
- Current `Assets/Audio/` tree

## Review Rule

For each group below, record exactly one outcome:

- `Keep`: sound character and mix are acceptable for the Stage 4 baseline.
- `Mix only`: sound character is acceptable, but volume, true peak, fade, loop, or routing needs polish.
- `Replace`: sound character does not fit, is tiring, or should be regenerated/re-exported.

Do not replace files during the listening pass. Capture the decisions first, then dispatch a separate audio export / normalization task.

## Pass 1: BGM And Ambience Bed

| Item | Runtime asset | Listen for | Decision |
|---|---|---|---|
| Zone1 BGM loop | `Assets/Audio/Music/Zone1_Ambient.ogg` | Let the track run across the 3:04-3:05 loop boundary. Decide whether the near-silent tail reads as an intentional breath or an obvious restart. | Keep / Mix only / Replace |
| BGM with wind | `sfx_env_wind_loop_01` | Confirm wind supports the quiet town mood without masking piano/string detail. | Keep / Mix only / Replace |
| BGM with silence pad | `sfx_env_silence_pad_01` | Confirm the pad feels like subtle air, not dead silence or noise. | Keep / Mix only / Replace |
| BGM with distant water | `sfx_env_distant_water_01` | Decide whether water should be audible in normal play or kept as distant texture. | Keep / Mix only / Replace |

## Pass 2: Foreground Repetition

Review these first because they repeat in the foreground and are the highest-value replacement candidates.

| Item | Runtime assets | Listen for | Decision |
|---|---|---|---|
| NPC reactions | `sfx_npc_greeting_short_01`, `sfx_npc_interaction_ack_01`, `sfx_npc_departure_01` | Trigger several dialogue opens / advances / closes. Watch for harsh transient, toy-like tone, or fatigue. | Keep / Mix only / Replace |
| UI clicks | `sfx_ui_button_click_01`, `sfx_ui_menu_open_01`, `sfx_ui_menu_close_01` | Repeat menu/dialogue actions quickly. Confirm clicks are informative but not sharp. | Keep / Mix only / Replace |
| Time symbol hover | `sfx_time_symbol_hover_01` | Hover/retrigger repeatedly. Confirm it stays soft and does not become irritating. | Keep / Mix only / Replace |

## Pass 3: Time-Window Identity

| Item | Runtime assets | Listen for | Decision |
|---|---|---|---|
| Wheel open / close | `sfx_time_wheel_open_01`, `sfx_time_wheel_close_01` | Confirm the soft ethereal tone still fits the Stage 4 visual polish direction. | Keep / Mix only / Replace |
| Symbol select | `sfx_time_symbol_select_red_01` | Confirm selection feedback is clear enough against BGM and ambience. | Keep / Mix only / Replace |
| Portal open / flip | `sfx_time_portal_open_01`, `sfx_time_portal_flip_01` | Confirm portal sounds feel magical/temporal without overpowering movement and dialogue. | Keep / Mix only / Replace |

## Pass 4: Footsteps In Motion

Test walk, run, and land on each surface. Mark per surface unless a single variant is clearly the problem.

| Surface | Runtime assets | Listen for | Decision |
|---|---|---|---|
| Wood | `sfx_footstep_wood_walk_01`, `sfx_footstep_wood_run_01`, `sfx_footstep_wood_land_01` | Too clicky, too loud, or not wooden enough. | Keep / Mix only / Replace |
| Stone | `sfx_footstep_stone_walk_01`, `sfx_footstep_stone_run_01`, `sfx_footstep_stone_land_01` | Stone identity and volume against ambience. | Keep / Mix only / Replace |
| Grass | `sfx_footstep_grass_walk_01`, `sfx_footstep_grass_run_01`, `sfx_footstep_grass_land_01` | Whether approved grass tone still matches the current character scale. | Keep / Mix only / Replace |
| Sand | `sfx_footstep_sand_walk_01`, `sfx_footstep_sand_run_01`, `sfx_footstep_sand_land_01` | Whether sand is distinct from grass/stone and not overly dry. | Keep / Mix only / Replace |

## Pass 5: Environmental Events

| Item | Runtime assets | Listen for | Decision |
|---|---|---|---|
| Birds | `sfx_env_birds_01` | Keep as distant ambience, make clearer, or remove if mood-breaking. | Keep / Mix only / Replace |
| Dry leaves | `sfx_env_dry_leaves_01` | Decide whether the cue is too subtle under BGM + wind. | Keep / Mix only / Replace |
| Wood creak | `sfx_env_wood_creak_01` | Confirm creak reads as environmental detail, not accidental UI/NPC feedback. | Keep / Mix only / Replace |

## Dispatch After Review

1. Record user decisions in this checklist or a follow-up devlog.
2. If most foreground items are `Mix only`, run a docs-backed normalization pass first: NPC trio, UI trio, and `sfx_time_symbol_hover_01`.
3. If any group is `Replace`, regenerate/export only that group and preserve canonical Zone1 paths unless a dedicated wiring task is opened.
4. Update `docs/legal/asset_ledger.md` only when exported files or provenance actually change.
5. Keep compatibility copies under `Assets/Audio/SFX/{env,footstep,timeframe,npc,ui}/` untouched unless a separate cleanup task updates references and documentation.

## Verification Boundary

This is a documentation-only checklist. Unity tests were not run. The relevant automated guard after any future asset replacement is `Zone1AudioWiringTests`; audible loop, mix, repetition, and replacement quality remain manual review items.
