# Audio Compatibility SFX Duplicate Investigation

Date: 2026-05-06

Scope: investigate the 30 compatibility SFX copies under `Assets/Audio/SFX/{env,footstep,timeframe,npc,ui}/` that duplicate the 30 primary Zone1 SFX under `Assets/Audio/SFX/Zone1/{environment,footsteps,time_window,npc,ui}/`. This pass is report-only: no audio asset, scene, importer, or `asset_ledger` file was modified.

## Conclusion

**B. legacy**: the compatibility layer appears to be a temporary legacy/path-stability fallback from the transition between early category-root SFX paths and the final `Zone1/` canonical SFX paths. It is not currently supported by evidence as a legitimate platform override, AudioMixer routing layer, runtime fallback layer, or active Unity importer compatibility layer.

Recommended Stage 4 action: remove the compatibility copies after one final callsite/build verification pass, then update `asset_ledger` and docs to remove compatibility-copy wording. Do not assume a large player build reduction until a post-removal build is measured.

## Asset Layout

| Layer | Path pattern | Count | OGG bytes | All files bytes |
| --- | --- | ---: | ---: | ---: |
| Primary | `Assets/Audio/SFX/Zone1/**/<id>.ogg` | 30 | 705,439 | 720,744 |
| Compatibility | `Assets/Audio/SFX/{env,footstep,timeframe,npc,ui}/<id>.ogg` | 30 | 705,439 | 720,744 |

Prior QA (`2026-05-05_audio_qa_audit.md`) confirmed the 30 compatibility OGG files match the 30 primary OGG files byte-for-byte by SHA-256. This investigation confirmed the duplicate counts and byte totals remain equal.

Category mapping:

| Primary directory | Compatibility directory | Notes |
| --- | --- | --- |
| `Zone1/environment/` | `env/` | 6 files |
| `Zone1/footsteps/` | `footstep/` | 12 files |
| `Zone1/time_window/` | `timeframe/` | 6 files |
| `Zone1/npc/` | `npc/` | 3 files |
| `Zone1/ui/` | `ui/` | 3 files |

## Evidence

| Check | Result | Interpretation |
| --- | --- | --- |
| Unity AudioImporter platform overrides | 60/60 SFX `.ogg.meta` files have `platformSettingOverrides: {}` | No WebGL/iOS/Android/etc split-compression reason exists today |
| Primary vs compatibility importer settings | Settings are identical except GUID | No importer behavior reason to keep duplicate assets |
| Compatibility GUID references | 0 references outside each asset's own `.meta` | No scene/prefab/asset serialized dependency on compatibility copies |
| Runtime/editor path references under `Assets/` | 33 references to `Assets/Audio/SFX/Zone1/...`; 0 references to category-root compatibility paths | Current code and scene setup are canonical-path only |
| `Zone1AudioSceneSetup.cs` | Loads every SFX from `Assets/Audio/SFX/Zone1/...` | No fallback lookup exists |
| `Zone1AudioController.cs` | Holds assigned clips only; no path lookup | Runtime has no compatibility layer |
| AudioMixer assets | No `.mixer` / AudioMixer asset found under `Assets`; scene `OutputAudioMixerGroup` entries are `{fileID: 0}` | No mixer routing reason exists today |
| Build/devlog trace | `2026-05-05_g5_audio_rebuild.md` says scene/build references canonical `Zone1/...` set | Build-side evidence points at primary paths |
| `asset_ledger.md` | Each SFX row mentions a compatibility copy path | Ledger records existence, but not a technical requirement |

## Origin Trace

Evidence for a temporary legacy origin:

- Early docs used category-root placeholders: `Assets/Audio/SFX/env/*.ogg`, `Assets/Audio/SFX/timeframe/*.ogg`, and related category directories appear in `docs/STAGE3_G_PLAN.md`, `docs/draft/g1_opening_text.md`, and `docs/ASSET_STRUCTURE.md`.
- The untracked generation helper `tools/generate_zone1_sfx_elevenlabs.ps1` names the category-root output variable `$legacyOgg`, writes canonical `finalOgg = Assets/Audio/SFX/Zone1/...`, then copies it to `compatibilityOgg = Assets/Audio/SFX/<category>/...`.
- The audio handover explicitly says: "Keep compatibility copies until all callers are confirmed to use `Assets/Audio/SFX/Zone1/` paths."
- Current caller search now finds no runtime/editor compatibility references.

This is a coherent trace: compatibility copies were deliberately created as a short-term migration/fallback layer while older docs/callers were being reconciled, not as a long-term platform or mixer design.

## A/B/C Decision

| Candidate | Decision | Reason |
| --- | --- | --- |
| A. legitimate | Rejected for current repo state | No platform override, AudioMixer, GUID reference, runtime fallback, or scene wiring depends on compatibility paths |
| B. legacy | **Chosen** | Historical docs and script variable names point to category-root legacy paths; current runtime/editor references have migrated to `Zone1/` |
| C. unknown | Not needed | Design intent trace is sufficient: temporary compatibility until callers were confirmed |

## Size Impact

The direct source-control duplicate cost is small and measurable: compatibility OGGs are 705,439 bytes, or 720,744 bytes including `.meta` files in those category-root directories.

The often-cited player build estimate `117.853 MiB -> ~110 MiB` is not supported by the evidence gathered here. Unity generally packs referenced assets, not every file under `Assets/`, and the current G5 audio rebuild devlog lists canonical `Zone1/...` SFX in the build while stating the compatibility copies are not the scene/build references. Stage 4 should measure a clean build before and after deletion instead of assuming a multi-MiB player-size reduction.

## Recommended Stage 4 Cleanup

1. Run a final callsite check for compatibility GUIDs and category-root paths, including scenes, prefabs, Addressables, Resources, editor tooling, docs, and test fixtures.
2. Delete only the compatibility directories and their `.meta` files: `Assets/Audio/SFX/env/`, `footstep/`, `timeframe/`, `npc/`, and `ui/`.
3. Update `docs/legal/asset_ledger.md` to remove or replace "compatibility copy" notes.
4. Update generation tooling so future SFX generation writes only the canonical `Assets/Audio/SFX/Zone1/...` path unless a documented fallback option is explicitly requested.
5. Rerun `Anemora.EditorTools.Zone1AudioSceneSetup.VerifyMainScene`, PlayMode `25/25`, and a Windows build size comparison.
6. Keep the primary `Assets/Audio/SFX/Zone1/...` set unchanged until the Stage 4 normalization pass decides whether to re-export high-true-peak SFX.

## Final Recommendation

Treat the compatibility SFX layer as **legacy, removable after Stage 4 verification**. It should not be kept as an implicit long-term fallback because no code uses it and no importer/mixer difference justifies the maintenance cost.
