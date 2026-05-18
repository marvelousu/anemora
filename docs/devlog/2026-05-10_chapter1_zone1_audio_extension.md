# Chapter 1 Zone1 Audio Extension

Date: 2026-05-10
Worktree: `<worktree>`

## Summary

Implemented the Chapter 1 audio extension for Zone1 proposal A: reuse `Zone1_Ambient.ogg`, add Chapter 1 SFX hooks, and support a scene 5 low-pass mood shift without replacing the Stage 3 audio path.

## Runtime

- Extended `Assets/Scripts/Audio/Zone1AudioController.cs`.
- Added Chapter 1 one-shot clip fields and play methods:
  - `PlayTimeBrushReact()`
  - `PlayRuinDust()`
  - `PlayRuinWind()`
  - `PlayChapterClose()`
  - `PlayNutsFall()`
- Added BGM low-pass control for scene 5:
  - `SetChapterFiveLowPassActive(bool active)`
  - `SetChapterFiveLowPassBlend(float blend)`
  - Runtime fallback creates / resolves `AudioLowPassFilter` on the music source.
- Scene wiring now also serializes the `musicLowPassFilter` reference on `Zone1_Audio` so the production scene is not dependent on runtime fallback for the normal path.
- Added Scene 2 sightline audio route coverage:
  - three 3D positional sightline sources
  - wind / bird / distant footstep cue clips
  - batchmode-safe route verification through `PlayScene2SightlineReveal(...)`

## Assets

Added provisional WAV assets under `Assets/Audio/SFX/Zone1/chapter1/`:

- `sfx_env_ruin_dust_01.wav`
- `sfx_env_ruin_wind_01.wav`
- `sfx_env_chapter_close_01.wav`
- `sfx_time_brush_react_01.wav`
- `sfx_nuts_fall_01.wav`

These are implementation placeholders for wiring and timing. Final listening polish is still required.

## Scene Wiring

- Updated `Assets/Editor/Zone1AudioSceneSetup.cs`.
- Added menu / batch support:
  - `Anemora/Audio/Configure Chapter1 Audio`
  - `Anemora/Audio/Verify Chapter1 Audio`
  - `Zone1AudioSceneSetup.ConfigureChapter1Scene()`
- Applied the setup to `Assets/Scenes/Anemora_Chapter1.unity`.
- Scene YAML confirms the Chapter 1 clip references are assigned.
- `Music_Source` now has an `AudioLowPassFilter` component serialized with `enabled = false`, cutoff `22000`, and resonance `1.1`.
- `Zone1AudioController.musicLowPassFilter` now points at the scene filter component instead of `{fileID: 0}`.

## Verification

- Unity batchmode compile/import succeeded after integration.
- `Zone1AudioSceneSetup.ConfigureChapter1Scene` completed successfully.
- `Zone1AudioSceneSetup.ConfigureChapter1Scene` completed successfully after the low-pass setup refresh.
  - Log: `<temp>\anemora_ch1_impl_zone1_audio_reconfigure_after_lowpass_setup_fix.log`
- `Zone1AudioSceneSetup.VerifyChapter1Scene` completed successfully after low-pass serialization.
  - Log: `<temp>\anemora_ch1_impl_zone1_audio_verify_after_lowpass_serialized.log`
- `Zone1AudioWiringTests` covers Chapter 1 audio clip presence.
- `Zone1AudioWiringTests.Chapter1SceneHasSightlineAudioRouteAndLowPassFilter`: `1/1` passed.
  - XML: `<temp>\anemora_ch1_impl_zone1_audio_wiring_after_sightline_route.xml`
- `Zone1AudioWiringTests`: `4/4` passed after low-pass serialization refresh.
  - XML: `<temp>\anemora_ch1_impl_zone1_audio_wiring_after_lowpass_serialized.xml`
- Runtime validator after low-pass serialization and progression/save test import:
  - `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - Log: `<temp>\anemora_ch1_impl_runtime_validator_after_lowpass_and_progression_import.log`
- Full EditMode after runtime Phase AE-AH progression/save tests and low-pass serialization refresh: `172 total / 150 passed / 0 failed / 22 skipped`.
  - XML: `<temp>\anemora_ch1_impl_editmode_after_runtime_ae_ah_lowpass.xml`
- Full PlayMode after sightline audio route coverage, Scene 1 book trace runtime coverage, Scene 4 trace runtime coverage, Scene 4 auto-trigger runtime coverage, and SymbolWheel scene progression coverage: `63/63` passed.
  - XML: `<temp>\anemora_ch1_impl_playmode_after_scene1_book_trace.xml`
- Later Full PlayMode after section-center occupiable smoke coverage: `64/64` passed.
  - XML: `<temp>\anemora_ch1_impl_playmode_after_route_occupiable.xml`
- Later Full PlayMode after trigger-polling scene-wired chapter transition smoke coverage: `65/65` passed.
  - XML: `<temp>\anemora_ch1_impl_playmode_after_transition_polling.xml`

Later review-fix validation after chapter-transition hardening:

- Full EditMode: `172 total / 150 passed / 0 failed / 22 skipped`
  - XML: `<temp>\anemora_ch1_impl_editmode_after_reviewfix.xml`
- Full PlayMode: `66/66` passed
  - XML: `<temp>\anemora_ch1_impl_playmode_after_reviewfix.xml`
- Runtime validator: `Info=107`, `Warning=3`, `Error=0`, `PendingWiring=0`
  - log: `<temp>\anemora_ch1_impl_runtime_validator_after_reviewfix.log`
- Windows build smoke: success
  - output: `<temp>\anemora_ch1_build_smoke_after_reviewfix\Anemora.exe`
  - log: `<temp>\anemora_ch1_impl_build_smoke_after_reviewfix.log`

## Remaining Work

- In-editor listening pass.
- Scene 5 low-pass timing / blend tuning.
- Final SFX replacement if provisional clips are not acceptable.
- Playable timing review with scene 5 transition.
