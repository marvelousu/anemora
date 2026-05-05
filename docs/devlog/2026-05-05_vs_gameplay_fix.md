# VS Gameplay Fix

Report date: 2026-05-05

Execution date: 2026-05-06 JST

Base commit:

`f35b1aa Rebuild Windows player after VS playable fix and verify Anemora_Main startup`

Temporary worktree:

`C:\Users\maro6\Documents\Unity\Anemora-vs-gameplay-fix`

## 1. Scope

This pass diagnosed and repaired the seven gameplay issues reported after the scene-order rebuild. The fix stays in the light / medium range: input gating, brush gesture wiring, NPC interaction guards, TMP font material repair, and scene hierarchy cleanup.

No large controller rewrite, new gameplay design, or Stage 4 mechanic redesign is included.

## 2. Issue Results

| # | Reported issue | Cause | Fix | Status |
| ---: | --- | --- | --- | --- |
| 1 | Sometimes movement required Enter first. | `NiroMonologueController.showIntroOnStart` opened dialogue on scene start and froze `PrototypePlayerController`; SymbolWheel also listened to Enter/Space globally. | Disabled blocking intro on start in `Anemora_Main`; gated SymbolWheel keyboard selection off by default. | Fixed |
| 2 | Brush mechanic did not fire from user input. | No runtime mouse-drag brush path existed; portal generation only listened to SymbolWheel selection. | Added Shift + left-drag brush gesture handling to `TimeFramePortalController`; accepted strokes call SymbolWheel Red selection / portal generation. | Fixed in code path; Win32 synthetic drag was inconclusive, temporary PlayMode smoke verified portal open. |
| 3 | Era transition happened automatically at a location. | Accidental SymbolWheel Enter/Space selection could open the portal without intentional brush input; after that, crossing the portal plane could flip side. | Disabled global keyboard selection and requires brush stroke for user-driven portal generation. Crossing an already-open portal remains expected behavior. | Fixed for unintended trigger. |
| 4 | NPC could not be spoken to with E. | Dialogue UI font asset had no TMP material / atlas material binding, producing runtime TMP atlas warnings and `NullReferenceException`; interaction range was also tight. | Created TMP distance-field materials, assigned them to JP/EN font assets, preserved atlas data on build, widened NPC interaction range to `2.0`. | Fixed by PlayMode E-flow tests. |
| 5 | Pressing E produced unclear "rustling". | Hidden-side interactables could still receive E based only on distance, and NPC audio could play without a readable dialogue panel. | NPC user input now requires its layer to be visible to Main Camera; Past book interaction requires Past side and ignores E while dialogue is visible. | Fixed. |
| 6 | White / gray box artifact around Hero. | The visible boxes were SymbolWheel UI icons rendered over the center of the screen, not Hero mesh geometry. | SymbolWheel Canvas is hidden on Awake; brush can still select Red programmatically. | Fixed; final screenshot has no center UI boxes. |
| 7 | Duplicate `Player_Visual_Current` / `Player_Visual_Past` instances. | `Anemora_Main` had six Hero prefab instances under `Player`, including four stale unnamed prefab roots. | Pruned Player children to exactly one `Player_Visual_Current` and one `Player_Visual_Past`, both at local `(0, -0.62, 0)`. | Fixed. |

## 3. Implementation Summary

Changed files:

- `Assets/Scripts/TimeManagement/TimeFramePortalController.cs`
- `Assets/UI/Scripts/SymbolWheelController.cs`
- `Assets/Scripts/Dialogue/NpcInteractable.cs`
- `Assets/Scripts/TimeManagement/Reflectors/PastBookInteractable.cs`
- `Assets/Scenes/Anemora_Main.unity`
- `Assets/UI/Localization/Fonts/Anemora_JP.asset`
- `Assets/UI/Localization/Fonts/Anemora_EN.asset`
- `Assets/UI/Localization/Fonts/Anemora_JP_DistanceField.mat`
- `Assets/UI/Localization/Fonts/Anemora_EN_DistanceField.mat`

The `Anemora_Main` scene now has only two Hero visual children under `Player`:

| Child | Layer |
| --- | ---: |
| `Player_Visual_Current` | `10` |
| `Player_Visual_Past` | `11` |

## 4. Verification

Automated tests:

| Suite | Result |
| --- | --- |
| EditMode | `32/32` passed, `0` failed |
| PlayMode | `27/27` passed, `0` failed |
| Temporary brush smoke | `28/28` passed before deleting the temporary test; log contained `Red symbol selected` and portal state reached `Open`. |

Final PlayMode log checks:

| Pattern | Count |
| --- | ---: |
| `Font Atlas Texture` | `0` |
| `NullReferenceException` | `0` |
| `MissingReferenceException` | `0` |
| `DrawObjectsPass` | `6` |

## 5. Build Result

Output:

`C:\Users\maro6\Documents\Unity\Anemora-vs-gameplay-fix\Builds\VSGameplayFix\Anemora_VS_Gameplay_Fix.exe`

| Item | Result |
| --- | --- |
| Build result | Success |
| Build duration | `12.619s` |
| Build payload size | `117.876 MiB` |
| Build payload files | `193` |
| Player ready time | `1.341s` |
| Avg working set | `262.265 MiB` |
| Peak working set | `263.062 MiB` |
| Avg private bytes | `371.335 MiB` |
| Peak private bytes | `371.344 MiB` |
| Avg CPU | `1.675%` |
| All samples responding | `true` |

Runtime screenshot paths:

- `C:\Users\maro6\Documents\Unity\Anemora-vs-gameplay-fix\Builds\VSGameplayFix\runtime_final_start.png`
- `C:\Users\maro6\Documents\Unity\Anemora-vs-gameplay-fix\Builds\VSGameplayFix\runtime_final_after_w.png`
- `C:\Users\maro6\Documents\Unity\Anemora-vs-gameplay-fix\Builds\VSGameplayFix\runtime_final_after_brush.png`

Observed runtime behavior:

- Initial scene rendered with Hero, bed placeholder, and Resident_B visible.
- The previous center white / gray UI boxes were gone.
- W input moved the Hero forward and changed facing / animation.
- Win32 synthetic Shift + left-drag did not emit `Red symbol selected`; the committed code path was instead verified by the temporary PlayMode smoke. Manual user brush verification is still recommended because OS-level synthetic mouse input was inconclusive.

Player log checks for final build sample:

| Pattern | Count |
| --- | ---: |
| `Font Atlas Texture` | `0` |
| `NullReferenceException` | `0` |
| `MissingReferenceException` | `0` |
| `DrawObjectsPass` | `4264` |

## 6. Remaining Caveats

- URP RenderGraph `DrawObjectsPass` warnings remain and are outside this gameplay-fix scope.
- Audible audio was not human-verified in this Codex session; no audio-load error was found, and audio wiring remains covered by PlayMode tests.
- Shift + left-drag should be manually checked by the user in the new build because the Win32 synthetic drag did not reach Unity input despite the PlayMode brush path passing.

## 7. Conclusion

The seven reported gameplay issues are repaired or narrowed to manual verification where OS-level synthetic input could not prove the path. The new build starts in `Anemora_Main`, no longer freezes on an invisible intro, accepts W movement, removes the Hero-area UI box artifact, removes duplicate Hero visuals, repairs dialogue font runtime errors, and keeps EditMode / PlayMode green.
