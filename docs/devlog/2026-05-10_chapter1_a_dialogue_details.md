# Chapter 1 A Dialogue Details Implementation

Date: 2026-05-10
Worktree: `<worktree>`

## Scope

Reflected the Linux story/spec A handoff from `chapter1_s1_s2_handover_2026-05-08.md` v1.8:

- Scene 3 [3.C] medieval market dialogue for Dario, Kairo, Luna, and unnamed customers.
- Scene 3 [3.D] Karla + Aria merchant lesson final dialogue.
- Scene 4 [4.B] / [4.D] / [4.E] / [4.F] / [4.G] / [4.H] Kaia and Dario dialogue.
- Chapter 1 Erythria references are kept to the handoff's intended locations:
  Scene 3 [3.C] Dario customer pair 1, Scene 3 [3.D] Karla, and Scene 4 [4.D] Niro thought.

## Implementation

Added `Assets/Editor/AnemoraChapter1DialogueSetup.cs`.

The setup utility writes DialogueAsset assets and `Anemora_Strings` localization entries from a single source of truth. `AnemoraChapter1SceneSetup.Apply()` now invokes the dialogue setup first so production scene regeneration does not revert to the previous Stage 3-era dialogue set.

Created or updated DialogueAssets:

- `Resident_A_Past_AriaHouse_Lesson.asset`
- `Resident_J_Scene3_D_KarlaAriaLesson.asset`
- `Resident_D_Scene3_C_MarketMonologue.asset`
- `Resident_D_Scene3_C_CustomerPair1.asset`
- `Resident_D_Scene3_C_CustomerPair2.asset`
- `Resident_K_Scene3_C_KairoSong.asset`
- `Resident_L_Scene3_C_LunaCalls.asset`
- `Resident_C_Scene4_B_SeedDelivery.asset`
- `Resident_D_Scene4_D_PastFieldArrival.asset`
- `Niro_Scene4_D_ErythriaThought.asset`
- `Resident_D_Scene4_E_SpiceInterference.asset`
- `Resident_C_Scene4_F_FieldChanged.asset`
- `Resident_D_Scene4_G_CostDiscovery.asset`
- `Resident_C_Scene4_H_Aftermath.asset`

Added speaker keys:

- `dialogue.speaker.resident_c`
- `dialogue.speaker.resident_d`
- `dialogue.speaker.resident_k`
- `dialogue.speaker.resident_l`
- `dialogue.speaker.customer`

## Scene Wiring

Added `Assets/Scripts/Dialogue/DialogueProximityTrigger.cs` for Scene 3 [3.C] position-based past-market dialogue playback. The component configures a trigger collider, checks horizontal player range, supports one-shot/cooldown playback, and routes playback through `DialogueDisplay.Instance`.

`TimeWindowDiorama` now treats `DialogueProximityTrigger` as a gated time-window interactable. Source triggers in `Root_Past` are disabled in the base scene and are enabled only on the local-window clone while the player is inside the window. This preserves the v3.2 local-window rule instead of making past ambient dialogue play in the current world.

The proximity trigger now also accepts the runtime-session contract fields (`dialogueAssets`, `audioClips`, sequential/random playback, `showDialoguePanel`, `pastNpcOverheardOnly`) while preserving the legacy single `dialogueAsset` / `triggerRadius` scene data. This avoids overwriting already placed Scene 3 source markers when the runtime validator worktree is merged later.

`AnemoraChapter1SceneSetup` now creates placeholder past-market dialogue sources under `Chapter1_DialogueSources_Past`:

- `Chapter1_S3C_Dario_Monologue_Source`
- `Chapter1_S3C_Dario_CustomerPair1_Source`
- `Chapter1_S3C_Dario_CustomerPair2_Source`
- `Chapter1_S3C_Kairo_Song_Source`
- `Chapter1_S3C_Luna_Calls_Source`
- `Chapter1_S3D_AriaHouse_Lesson_Source`

These are production wiring markers, not final character presentation. Final sprite / animation replacement remains a character/scene polish task.

Scene setup writes both the legacy and canonical serialized fields. Scene 3 text dialogue source markers now use `showDialoguePanel=true` with `freezePlayerOnDialogue=false`, so overheard market text can be displayed without freezing Niro's local-window movement. The market ambient source remains audio-only with `showDialoguePanel=false`.

The Scene 3 market ambient route is now wired as `Chapter1_Scene3_Market_Ambient_Source` with `chapter1.scene3.market.ambient`, `zone1_env_wind_breeze.ogg`, and `zone1_env_bird_chirp.ogg`.

B-5 Niro home-past monologue hooks are placed under the same local-window source root:

- `Chapter1_B5_NiroHomePast_SomeoneHere_Source`
- `Chapter1_B5_NiroHomePast_FaceNotVisible_Source`

Both use non-freezing `DialogueDisplay` playback so the hook can surface the Niro thought without regressing local-window movement.

## Tests

Updated `Chapter1DialogueAssetTests` to cover the new Scene 3/4 assets and all A-handoff localization keys.

Validation:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -projectPath '<worktree>' -runTests -testPlatform EditMode -testFilter Chapter1DialogueAssetTests -testResults '<temp>\anemora_ch1_dialogue_tests2.xml' -logFile '<temp>\anemora_ch1_dialogue_tests2.log'
```

Result: `9/9 passed`.

Additional validation:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -projectPath '<worktree>' -runTests -testPlatform PlayMode -testFilter DialogueProximityTriggerTests -testResults '<temp>\anemora_ch1_dialogue_proximity_tests.xml' -logFile '<temp>\anemora_ch1_dialogue_proximity_tests.log'
```

Initial result: `3/3 passed`.

Compatibility result after adding the runtime-session contract fields:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -projectPath '<worktree>' -runTests -testPlatform PlayMode -testFilter DialogueProximityTriggerTests -testResults '<temp>\anemora_ch1_dialogue_proximity_compat_tests.xml' -logFile '<temp>\anemora_ch1_dialogue_proximity_compat_tests.log'
```

Result: `6/6 passed`.

Latest proximity playback validation after enabling non-freezing text display:

```powershell
& 'C:\Program Files\Unity\Hub\Editor\6000.3.14f1\Editor\Unity.exe' -batchmode -projectPath '<worktree>' -runTests -testPlatform PlayMode -testFilter DialogueProximityTriggerTests -testResults '<temp>\anemora_ch1_impl_dialogue_proximity_playmode3.xml' -logFile '<temp>\anemora_ch1_impl_dialogue_proximity_playmode3.log'
```

Result: `7/7 passed`.

Runtime validator after scene setup:

```text
Info=215, Warning=3, Error=0, PendingWiring=0
```

The three warnings are the accepted S4D/S4G placement deltas already tracked by the runtime validator.

## Notes

- This pass establishes assets, localization, and initial Scene 3 [3.C] / [3.D] proximity-source wiring.
- Scene 4 dialogue sequencing still needs production story-flow wiring around the existing CP-1/CP-2 runtime hooks.
- Background market ambient is wired through the Scene 3 audio-only proximity source using the available Zone1 chapter SFX clips.
- Commit, push, PR, and staging were not performed.
