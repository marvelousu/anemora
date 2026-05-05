# Verification Suite Catalog

Status: Stage 3 closeout baseline (2026-05-06)

## 1. 概要

本 catalog は、Anemora の automated test を navigation し、`docs/G5_ACCEPTANCE_MATRIX.md` の各検証項目と対応付けるための文書である。G5 実行時は、本 catalog で automated coverage を確認し、manual verification が必要な箇所を `docs/G5_ACCEPTANCE_MATRIX.md` の実測 / pass-fail / 備考欄へ記録する。

関連 doc:

- `docs/G5_ACCEPTANCE_MATRIX.md` — G5 通し体験の acceptance matrix。
- `docs/G5_PREFLIGHT.md` — G5 実行前の Go / No-Go 判定手順。
- `CHANGELOG.md` — test 追加や milestone 完了の履歴。

更新方針:

- 新規 test を追加したら、本 catalog の §2 または §3 に class / method / verifies / milestone / matrix section を追記する。
- `docs/G5_ACCEPTANCE_MATRIX.md` の section が増減したら §4 / §5 を更新する。
- 本 catalog は source scan に基づく。推測で test を追加しない。

Source scan (2026-05-06, `cab5a59`):

- `Assets/Tests/EditMode/`: 34 test method markers discovered.
- `Assets/Tests/PlayMode/`: 29 test method markers discovered.
- Total: 63 test method markers discovered.
- EditMode parameterized test attributes: none (`[TestCase]` / `[TestCaseSource]` not present).

Reconcile note (2026-05-06): latest Unity Test Runner execution is EditMode `35/35`, PlayMode `29/29`. Character v2 added three `CharacterPrefabStructureTests` EditMode `[Test]` methods, so the previous source marker count moves from 31 to 34. The historical +1 Unity runner/source distinction remains; for acceptance, use the Test Runner result and keep source-marker totals as scan metadata.

## 2. EditMode tests

### 2.1 DataLayer / Localization (POCO)

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `DialogueAssetDataTests` | `RoundTrip_PreservesNpcVariantTurnChoiceAndFlags`; `DialogueTextAndChoiceLabels_AreKeysOnly`; `MultipleVariants_CanBeFilteredByFlags` | DialogueAssetData の POCO round-trip、dialogue text / choice label が string key のみであること、flag による variant filtering | A1 DialogueAsset | §G |

### 2.2 SaveLayer tests

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `SaveEnvelopeRoundTripTests` | `SaveEnvelope_RoundTrip_PreservesAllFields`; `SaveEnvelope_TimeFrameCrossingState_RoundTrips`; `SettingsEnvelope_RoundTrip_PreservesAllFields` | SaveEnvelope / TimeFrameCrossingState / SettingsEnvelope の JSON round-trip | E5 / Save layer | §J |
| `SaveMigrationTests` | `Migration_TransformsField`; `Migration_HasSequentialVersionStep`; `Migration_PreservesUnrelatedFields` | Save migration の field transform、version step、unrelated fields preservation | Save layer | §J |

### 2.3 Reflector tests

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `ActionRecordStoreTests` | `Add_AppendsEntry`; `Add_NullEntry_Throws`; `Add_EmptyActionId_Throws`; `GetUnreflected_ReturnsOnlyUnreflected`; `GetReflected_ReturnsOnlyReflected`; `MarkReflected_SetsFlagAndReturnsTrue`; `MarkReflected_AlreadyReflected_ReturnsFalse`; `MarkReflected_UnknownId_ReturnsFalse`; `MarkReflected_PreventsDoubleReflection`; `RoundTrip_PreservesEntries`; `RoundTrip_DoesNotShareReferences`; `LoadFromSaveData_NullClearsStore` | ActionRecordStore の追加、validation、unreflected/reflected query、reflected flag、二重反映防止、save data round-trip | E5 ActionRecord | §D / §J |

### 2.4 Catalog tests

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `ActionRecordCatalogTests` | `AddEntry_AppendsEntryAndFindsByActionId`; `TryGetEntry_MissingKey_ReturnsFalse`; `AddEntry_EmptyActionId_Throws` | ActionRecordCatalog entry 追加、action id lookup、empty action id validation | E5 ActionRecord | §D |

### 2.5 Animator / prefab structure tests

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `CharacterPrefabStructureTests` | `CharacterPrefabsContainSpriteRendererAndAnimator`; `LocomotionControllersExposeExpectedStatesAndParameters`; `F2CharacterSpritesAreSlicedForAnimatorClips`; `HeroV2SpritesAreSlicedForAnimatorClips`; `ResidentV2SpritesAreSlicedForAnimatorClips`; `CharacterPrefabsUseV2SpriteSets` | Hero / Resident prefab structure、AnimatorController states / parameters、F2 and v2 sprite slicing for animation clips、prefab / clip references use v2 sprite sets | F4 prefab / Stage 4 character v2 | §F |

### 2.6 Portal crossing data tests

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `PortalCrossingHysteresisTests` | `LateralMovementWithUnchangedSignedDistanceDoesNotFlip`; `HysteresisBandSuppressesNearPlaneSignChange`; `NormalMovementPastMinimumDistanceFlips`; `LastStableSignedDistanceCanBeResetAfterFlip` | Portal crossing hysteresis、minimum normal movement、near-plane suppression、flip 後 reset | E4 PortalCrossing | §C |

## 3. PlayMode tests

### 3.1 Portal / wiring tests

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `PortalStencilFeatureSmokeTest` | `SandboxSceneEnqueuesPortalStencilPasses` | PortalStencilFeature が sandbox scene で stencil pass を enqueue すること | E1 PortalStencil | §A |
| `SceneRootRegistrySmokeTest` | `MainSceneContainsSceneRootsAndDisabledPastCamera` | Anemora_Main の SceneRootRegistry / scene roots / disabled `Camera_Past` skeleton | E2 Hierarchy | §B |
| `SceneSidePolarityTests` | `FlipToOppositeRaisesOneEventAndSameSideIsNoop` | SceneSidePolarity flip event と same-side noop | E4 PortalCrossing | §C |
| `TimeFramePortalControllerIntegrationTests` | `SymbolWheelBurstCreatesOnlyOnePortalAndRestoresTimeScale`; `AtomicFlipAppliesCameraPlayerAndStencilBeforeSideEvent`; `CrossingRunsThroughCrossingAndFlippingStates` | SymbolWheel burst、timeScale restore、atomic flip ordering、camera / player / stencil 切替、Crossing / Flipping state flow | E3 / E4 Portal | §C |
| `AnemoraMainPortalWiringRoundTripTests` | `MainScenePortalWiringSupportsBoundaryRoundTrip` | Anemora_Main で Current -> Past -> Current の境界往復が成立すること | A2 wiring | §C |
| `DemoPlayableSmokeTests` | `MainSceneHasVisibleDemoEnvironmentAndTopmostUi`; `BrushPortalAndNpcDialogueAreUsableInMainScene` | DemoPlayable scene environment、topmost UI、runtime brush hint、local time-window quick/drag flow、brush preview / generated window footprint一致、NPC dialogue usability | G5 closeout / Stage 3 demo repair / Stage 4 brush UX | §C / §I / §L / §M |

### 3.2 ActionRecord E2E

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `BookReflectorIntegrationTests` | `ReflectKnownAction_SpawnsBookAtBed`; `MissingActionId_IsIgnored`; `NullPrefab_IsNoOp`; `RuntimeDispatchesUnreflectedRecordsOnceAndMarksReflected` | Catalog -> BookReflector -> Bed spawn、missing action / null prefab の no-op、unreflected record dispatch、reflected marking | E5 + G4 | §D |
| `G4ActionRecordReflectionE2ETests` | `PastBookInteractionReflectsOneCurrentBookOnReturn` | Past で本取得 -> Current 帰還 -> Bed spawn -> 二重反映防止 | G4 | §D |

### 3.3 Animator / character behavior

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `HeroAnimatorBinderTests` | `BinderUpdatesMovingAndFacingFromObservedMovement`; `MainSceneUsesHeroPrefabInstancesForCurrentAndPastVisuals` | HeroAnimatorBinder が movement から Animator parameter を更新すること、Anemora_Main が Hero prefab instance を Current / Past visual に使うこと | F4 | §F |

### 3.4 Localization integration

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `DialogueAssetIntegrationTests` | `ScriptableObjectInstance_HoldsLocalizedStringDialogueTree`; `EmptyLocalizationTables_FallBackToStringKeys` | DialogueAsset SO が LocalizedString dialogue tree を保持すること、empty localization table で string key fallback できること | A1 DialogueAsset | §G |
| `LocalizationSettingsResolutionTests` | `FinalDialogueKey_ResolvesForJapaneseLocale`; `FinalDialogueKey_ResolvesForEnglishLocale`; `MissingKey_FallsBackToKeyString` | ja-JP / en StringTable resolution と missing key fallback | A1 Localization seed | §G / §I |

### 3.5 Dialog flow

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `NpcDialogueFlowTests` | `SceneContainsResidentNpcInstancesWithFinalDialogueAssets`; `ResidentAInteractionShowsAdvancesAndClosesDialoguePanel`; `ResidentADialogueResolvesFinalTextAfterLocaleSwitch` | Resident_A / Resident_B scene instances、DialogueAsset assignment、Resident_A interaction -> DialogueDisplay show -> advance -> close flow、locale switch 後の final text resolution | G3 final dialogue | §G / §I |

### 3.6 Audio wiring

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `Zone1AudioWiringTests` | `MainSceneHasZone1AudioControllerWithCoreClips`; `MainSceneHasNpcDialogueAudioClips` | Zone1AudioController と NPC dialogue audio clip references が scene に存在すること | A4 Audio | §H |

### 3.7 Save/Load round trip

| Test class | Method | Verifies | 関連 milestone | Matrix セクション |
|---|---|---|---|---|
| `SaveLoadRoundTripE2ETests` | `BookReflectionSurvivesSaveEnvelopeJsonRoundTripAndSceneReload` | Book reflection 済み ActionRecord の SaveEnvelope round-trip、scene reload 後の duplicate prevention | Save / Load E2E | §J |
| `SaveLoadLocaleIntegrationTests` | `ResidentADialogueUsesCurrentLocaleAfterSaveEnvelopeRoundTripAndSceneReload` | Locale は SaveEnvelope に永続化せず、現在 selected locale を維持したまま dialogue resolve できること | Save / Load + Localization | §G / §I / §J |

## 4. Matrix cross-reference

| Matrix セクション | Topic | Automated test (catalog 参照) | Manual verification 必要部分 |
|---|---|---|---|
| §A Engine / Pipeline | URP / Stencil | `PortalStencilFeatureSmokeTest`; `TimeFramePortalControllerIntegrationTests` | URP pipeline 起動、Game view の portal visual、RenderGraph / lighting warning の確認 |
| §B Scene / Hierarchy | 常駐 hierarchy | `SceneRootRegistrySmokeTest`; `HeroAnimatorBinderTests.MainSceneUsesHeroPrefabInstancesForCurrentAndPastVisuals` | Hierarchy 目視、layer assignment、missing reference / missing script の確認 |
| §C Symbol / Portal | E3-E4 portal flip | `PortalCrossingHysteresisTests`; `SceneSidePolarityTests`; `TimeFramePortalControllerIntegrationTests`; `AnemoraMainPortalWiringRoundTripTests`; `PortalStencilFeatureSmokeTest`; `DemoPlayableSmokeTests` | Symbol UI 表示、白 / 青 disabled 表示、flash 演出、境界付近の体感確認 |
| §D ActionRecord | E5 + G4 | `ActionRecordCatalogTests`; `ActionRecordStoreTests`; `BookReflectorIntegrationTests`; `G4ActionRecordReflectionE2ETests` | Bed spawn の視認、実 scene 内の interactable 操作、UI / VFX / SFX との接続確認 |
| §E Buildings / Environment | A3 | None | Zone1 building prefab 14 個の配置、Meshy / Blender 修復 asset の visual quality、scale / material / collider 目視 |
| §F Character | F4 | `CharacterPrefabStructureTests`; `HeroAnimatorBinderTests` | Sprite 表示品質、Idle / Walk の見た目、NPC visual placement、import settings 目視 |
| §G Dialogue / NPC | A1 + G3 final | `DialogueAssetDataTests`; `DialogueAssetIntegrationTests`; `LocalizationSettingsResolutionTests`; `NpcDialogueFlowTests`; `SaveLoadLocaleIntegrationTests`; `DemoPlayableSmokeTests` | Dialogue UI 表示、文字 fit、JP / EN 表示の実 UI、content polish |
| §H Audio (BGM + SFX) | A4 | `Zone1AudioWiringTests` | BGM loop、time-window modulation、SFX trigger の listen test |
| §I UI / Localization | UI 基盤 + atlas | `DialogueAssetIntegrationTests`; `LocalizationSettingsResolutionTests`; `NpcDialogueFlowTests`; `SaveLoadLocaleIntegrationTests`; `DemoPlayableSmokeTests` | TMP atlas visual quality、palette v0 の最終採否、画面上の text fit |
| §J Save / Load | POCO + Save layer | `SaveEnvelopeRoundTripTests`; `SaveMigrationTests`; `ActionRecordStoreTests`; `SaveLoadRoundTripE2ETests`; `SaveLoadLocaleIntegrationTests` | Standalone UI save/load flow は Stage 4。 |
| §K Build / Performance | build / profiler baseline | None | Windows Standalone build、FPS、VRAM / heap profiler 計測 |
| §L 通し体験 | E2E manual playthrough | Supporting coverage from §C / §D / §F / §G / §J PlayMode and EditMode tests | Full playthrough 5-8 分、softlock、flow、違和感、録画確認 |
| §M 層 2 片鱗 | VS_SCOPE §5.x | `DemoPlayableSmokeTests` | Minimum hint mechanics are covered; authored final visual beat remains Stage 4 manual/content work. |

Automated support by matrix section:

- Automated or partial automated support: 10 / 13 sections (§A, §B, §C, §D, §F, §G, §I, §J, §L, §M).
- Manual-only sections: 3 / 13 sections (§E, §H, §K).
- Stage 3 closeout manual observations are recorded in `docs/G5_ACCEPTANCE_MATRIX.md`; Stage 4 keeps quality polish observations as backlog.

## 5. Coverage gaps

The following are observed gaps in automated coverage. This list records coverage state only; adding tests is a separate task decision.

| Gap | Matrix section | Current state | Possible test idea |
|---|---|---|---|
| URP pipeline startup / RenderGraph warning | §A | Stencil pass smoke test exists, but full scene pipeline startup and visual warning state are manual | Scene launch smoke test that opens `Anemora_Main` and asserts no startup exceptions |
| Buildings / Environment | §E | No automated prefab loadability or Zone1 placement test | Prefab load test for `Assets/Prefabs/Zone1/` and missing mesh / material assertions |
| Audio playback / routing | §H | `Zone1AudioWiringTests` covers references, but audible mix / loop quality remains manual | AudioMixer parameter smoke test or runtime audio event counter if Stage 4 needs regression coverage |
| TMP atlas / UI rendering | §I | Locale resolution and demo UI visibility are tested, but rendered glyph / readability quality remains manual | TMP FontAsset / StringTable key coverage test or screenshot-based UI smoke test |
| Build / Performance | §K | No threshold-based automated build / FPS / memory test | Batch build smoke test and profiler baseline comparison after G5 build pipeline is stable |
| Full playthrough | §L | DemoPlayable smoke covers the core local time-window / NPC flow; final feel remains manual | Keep manual playthrough as source of truth; add scenario runner only if Stage 4 regressions recur |
| 層 2 片鱗 | §M | DemoPlayable smoke covers minimum hint mechanics, not a authored final cinematic beat | Scene marker / trigger smoke test if Stage 4 implements a stable authored event |

Gap count: 7.

## 6. Test の実行方法

Unity Editor:

- EditMode: Unity Editor -> Window -> General -> Test Runner -> EditMode -> Run All
- PlayMode: Unity Editor -> Window -> General -> Test Runner -> PlayMode -> Run All

CLI (batchmode examples):

```powershell
Unity.exe -batchmode -projectPath "C:\Users\maro6\Documents\Unity\Anemora" -runTests -testPlatform EditMode -testResults editmode_results.xml
Unity.exe -batchmode -projectPath "C:\Users\maro6\Documents\Unity\Anemora" -runTests -testPlatform PlayMode -testResults playmode_results.xml
```

Windows build:

- Unity Editor -> File -> Build Settings -> Windows -> Build
- Batchmode form, when build scripting is available:

```powershell
Unity.exe -batchmode -projectPath "C:\Users\maro6\Documents\Unity\Anemora" -buildWindows64Player "<output>\Anemora.exe"
```

## 7. 更新履歴

| 版 | 日付 | 変更 |
|---|---|---|
| v1.1 | 2026-05-06 | Stage 4 character v2 test count reconcile: latest Unity runner baseline is EditMode `35/35`, PlayMode `29/29`; source markers are EditMode 34 and PlayMode 29. |
| v1.0 | 2026-05-06 | Stage 3 closeout baseline に更新。Latest Unity run は EditMode `32/32`、PlayMode `29/29` pass。DemoPlayable / SaveLoad / Locale integration coverage を追加。 |
| v0.2 | 2026-05-05 | EditMode 31/31 baseline を確定。`docs/G5_PREFLIGHT.md` の 32/32 表記との差分を解消し、`[TestCase]` / `[TestCaseSource]` 不在を明記。 |
| v0.1 | 2026-05-05 | 初版起草。Source scan に基づき EditMode 31 + PlayMode 18 = 49 件を catalog 化し、G5 matrix cross-reference と coverage gaps を記録。EditMode 32 baseline との差分は reconcile 対象として明記 |
