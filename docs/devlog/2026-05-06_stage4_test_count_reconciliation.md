# Stage 4 Test Count Reconciliation

Date: 2026-05-06

## 1. Purpose

Resolve the Stage 4 Phase 0 open item asking whether the old EditMode `32/32` Unity runner vs 31 source-marker ambiguity was cleared by the character v2 import.

## 2. Inputs

- Current `origin/main` / `HEAD`: `cab5a59` (`Import Stage 4 character v2 sprite sets`).
- Recorded post-import run: full EditMode `35/35`, full PlayMode `29/29`.
- Recorded pre-import baseline: EditMode `32/32`, PlayMode `29/29`.

## 3. Source Scan

Commands used:

```powershell
rg -n "\[(Test|UnityTest|TestCase|TestCaseSource|Theory)\b" Assets\Tests
git grep -c -E "\[(Test|UnityTest|TestCase|TestCaseSource|Theory)\b" cab5a59^ -- Assets/Tests/EditMode
git grep -c -E "\[(Test|UnityTest|TestCase|TestCaseSource|Theory)\b" HEAD -- Assets/Tests/EditMode
git grep -c -E "\[(Test|UnityTest|TestCase|TestCaseSource|Theory)\b" HEAD -- Assets/Tests/PlayMode
```

Current source-marker totals:

| Suite | Source markers | Recorded Unity runner |
|---|---:|---:|
| EditMode | 34 | 35/35 |
| PlayMode | 29 | 29/29 |

Current EditMode source-marker breakdown:

| File | Markers |
|---|---:|
| `ActionRecordCatalogTests.cs` | 3 |
| `ActionRecordStoreTests.cs` | 12 |
| `CharacterPrefabStructureTests.cs` | 6 |
| `DialogueAssetDataTests.cs` | 3 |
| `PortalCrossingHysteresisTests.cs` | 4 |
| `SaveEnvelopeRoundTripTests.cs` | 3 |
| `SaveMigrationTests.cs` | 3 |
| **Total** | **34** |

## 4. Conclusion

The character v2 import added exactly three EditMode `[Test]` methods to `CharacterPrefabStructureTests.cs`: `HeroV2SpritesAreSlicedForAnimatorClips`, `ResidentV2SpritesAreSlicedForAnimatorClips`, and `CharacterPrefabsUseV2SpriteSets`.

That explains the movement from the old source-marker total 31 to the current source-marker total 34, and it aligns with the recorded Unity runner movement from `32/32` to `35/35`. It does not eliminate the historical +1 runner/source distinction. The durable documentation rule is therefore:

- Acceptance baseline: latest Unity Test Runner result, currently EditMode `35/35` and PlayMode `29/29`.
- Scan metadata: source markers, currently EditMode 34 and PlayMode 29.

## 5. Verification

This was a source scan and documentation reconciliation only. Full Unity tests were not rerun because the latest post-import run already recorded EditMode `35/35` and PlayMode `29/29`.
