# Test Count Reconcile: EditMode 31 vs 32

Date: 2026-05-05

## 1. Purpose

`docs/VERIFICATION_SUITE.md` recorded 31 EditMode tests by source scan, while `docs/G5_PREFLIGHT.md` and earlier handover notes used an EditMode 32/32 baseline. This pass reconciles the current source-of-truth count for G5 pre-flight and verification docs.

## 2. Scan Method

Target:

- `Assets/Tests/EditMode/*.cs`

Count rule:

- Count each `[Test]` method as one Unity Test Runner test case.
- Count `[TestCase]` and `[TestCaseSource]` entries when present.
- No `[TestCase]` or `[TestCaseSource]` attributes are present in the current EditMode test sources.

Command basis:

```powershell
Get-ChildItem Assets\Tests\EditMode -Filter *.cs |
  ForEach-Object {
    Count lines matching '^\s*\[Test\]',
    '^\s*\[TestCase\(',
    and '^\s*\[TestCaseSource\('
  }
```

## 3. Result

Confirmed current EditMode baseline: 31/31.

| File | `[Test]` | `[TestCase]` | `[TestCaseSource]` | Runner cases |
|---|---:|---:|---:|---:|
| `ActionRecordCatalogTests.cs` | 3 | 0 | 0 | 3 |
| `ActionRecordStoreTests.cs` | 12 | 0 | 0 | 12 |
| `CharacterPrefabStructureTests.cs` | 3 | 0 | 0 | 3 |
| `DialogueAssetDataTests.cs` | 3 | 0 | 0 | 3 |
| `PortalCrossingHysteresisTests.cs` | 4 | 0 | 0 | 4 |
| `SaveEnvelopeRoundTripTests.cs` | 3 | 0 | 0 | 3 |
| `SaveMigrationTests.cs` | 3 | 0 | 0 | 3 |
| **Total** | **31** | **0** | **0** | **31** |

The 32/32 baseline is not supported by the current EditMode source tree. Existing untracked XML result files are historical snapshots with lower totals (22, 25, 28) and do not establish a current 32-test baseline.

## 4. Reconciled Documents

- `docs/G5_PREFLIGHT.md`: updated EditMode baseline from 32/32 to 31/31 in overview, build health, and checklist entries; added v0.2 revision row.
- `docs/VERIFICATION_SUITE.md`: retained 31 EditMode + 18 PlayMode = 49 total; replaced the reconcile-needed note with a reconciled baseline note and added v0.2 revision row.
- `docs/VS_SCOPE.md`: no EditMode 32/32 reference found; no edit required.

## 5. Verification

This was a source scan and documentation reconcile only. Unity EditMode / PlayMode tests were not run in this pass.
