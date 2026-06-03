# Chapter 1 — Kaia house + Ruins interiors (E2 / F4 / F2·F3·F5 past-only) — Codex handoff

## Context

Working tree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`, branch
`work/chapter1-continuation-map-vs-20260524`. Unity 6000.3.14f1, URP, HD-2D.

The Chapter-1 map source-of-truth is the reference deck
`C:\Users\maro6\OneDrive\work\projects\anemora_reference\map_chapter_1\map_chapter_1.pptx`
(PNG export `…\map_chapter_1\スライドN.PNG`, 14 slides). Enterable buildings are marked
by numbered door nodes:

| Node | Building | Enterable | Status |
|---|---|---|---|
| A1 | Niro house | both times | done |
| B2 | Library | both times | done |
| C2 | Mia house | both times | done (this cycle) |
| D2 | Aria house | both times | done (this cycle) |
| **E2** | **Kaia house** | **both times** | **TODO** |
| **F4** | **1 ruin (top-center)** | **both times** | **TODO** |
| **F2 / F3 / F5** | **3 ruins** | **PAST (other-time) only** | **TODO** |

In the *current* timeline the ruins district is rubble and only **F4** is enterable. In
the *past* (slide 14 「廃墟、過去」) those same shells are intact 家 and **F2/F3/F5** also
open. This handoff builds all of it.

This is a direct extension of the proven Mia/Aria interior work
(`MiaInterior`/`AriaInterior`). Re-use that exact pattern; the only genuinely new piece
is the **time-conditional door** for F2/F3/F5.

All edits are in two files unless noted:
- `Assets/Scripts/FastVS/FastVsHouseAreaVisibility.cs`
- `Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`
- (new mechanic) `Assets/Scripts/FastVS/FastVsAreaDoorTransition.cs`

## Scope decision (from Tom)

「全部一気に（過去限定扉も今実装）」 — implement E2 + F4 (both-time) **and** F2/F3/F5
(past-only) in this pass. Interiors are bare placeholders — Tom only wants a 「気持ち横長め
の長方形」 reserved per house (`CreateSmallHouseInteriorShell` is already 6.4×5.4 = exactly
that). Do **not** decorate; props/lighting polish is a later cycle. Tom will refine
placement.

---

## Numbered mechanical fixes

### 1. Enum — add 5 interior areas
`Assets/Scripts/FastVS/FastVsHouseAreaVisibility.cs`, enum `FastVsHouseArea` (currently
ends `…, MiaInterior, AriaInterior`). Append:

```csharp
        MiaInterior,
        AriaInterior,
        KaiaInterior,
        RuinsF4Interior,
        RuinsF2Interior,
        RuinsF3Interior,
        RuinsF5Interior
```

### 2. Visibility fields + wiring — mirror MiaInterior/AriaInterior exactly
Same file. For EACH of the 5 new areas, replicate the four MiaInterior touch-points:

1. SerializeField pair (near L32):
   `[SerializeField] private GameObject currentKaiaInteriorMap;` / `pastKaiaInteriorMap;`
   … and the same for `RuinsF4Interior`, `RuinsF2Interior`, `RuinsF3Interior`,
   `RuinsF5Interior`.
2. `…ActiveForReview` property (near L55):
   `public bool KaiaInteriorActiveForReview => IsActive(currentKaiaInteriorMap) && IsActive(pastKaiaInteriorMap);`
3. `HasAllMapSetsForReview` (L61): add `&& currentKaiaInteriorMap != null && pastKaiaInteriorMap != null` (×5).
4. `ApplyVisibility` (L102): add the two `SetActive(currentKaiaInteriorMap, activeArea == FastVsHouseArea.KaiaInterior);` lines (×5).

> Trap: the indoor clear-color branch in `ApplyCameraClearColor` (L142) treats anything
> not Exterior/CentralPlaza as indoor — correct for all 5, no change needed.

### 3. Interior centers + door constants
`Assets/Editor/AnemoraFastVsHouseSliceSetup.cs`, in the constant block ~L405-465 (where
`MiaInteriorCenter`/`AriaInteriorCenter` and `MiaInteriorDoor*` live). Interiors live in
the negative-X "backstage" row (Mia=-21.20, Aria=-32.95, z=-8.35). Continue the grid:

```csharp
        private static readonly Vector3 KaiaInteriorCenter     = new Vector3(-44.70f, 0f, -8.35f);
        private static readonly Vector3 RuinsF4InteriorCenter  = new Vector3(-56.45f, 0f, -8.35f);
        private static readonly Vector3 RuinsF2InteriorCenter  = new Vector3(-21.20f, 0f, -19.85f);
        private static readonly Vector3 RuinsF3InteriorCenter  = new Vector3(-32.95f, 0f, -19.85f);
        private static readonly Vector3 RuinsF5InteriorCenter  = new Vector3(-44.70f, 0f, -19.85f);

        // interior-side door (mirror MiaInteriorDoor*: trigger near exit glow pad, exit just inside)
        private static readonly Vector3 KaiaInteriorDoorTriggerCenter    = KaiaInteriorCenter    + new Vector3(0.82f, 0.70f, -2.32f);
        private static readonly Vector3 KaiaInteriorDoorExitTarget       = KaiaInteriorCenter    + new Vector3(0.82f, 0.02f, -1.52f);
        private static readonly Vector3 RuinsF4InteriorDoorTriggerCenter = RuinsF4InteriorCenter + new Vector3(0.82f, 0.70f, -2.32f);
        private static readonly Vector3 RuinsF4InteriorDoorExitTarget    = RuinsF4InteriorCenter + new Vector3(0.82f, 0.02f, -1.52f);
        private static readonly Vector3 RuinsF2InteriorDoorTriggerCenter = RuinsF2InteriorCenter + new Vector3(0.82f, 0.70f, -2.32f);
        private static readonly Vector3 RuinsF2InteriorDoorExitTarget    = RuinsF2InteriorCenter + new Vector3(0.82f, 0.02f, -1.52f);
        private static readonly Vector3 RuinsF3InteriorDoorTriggerCenter = RuinsF3InteriorCenter + new Vector3(0.82f, 0.70f, -2.32f);
        private static readonly Vector3 RuinsF3InteriorDoorExitTarget    = RuinsF3InteriorCenter + new Vector3(0.82f, 0.02f, -1.52f);
        private static readonly Vector3 RuinsF5InteriorDoorTriggerCenter = RuinsF5InteriorCenter + new Vector3(0.82f, 0.70f, -2.32f);
        private static readonly Vector3 RuinsF5InteriorDoorExitTarget    = RuinsF5InteriorCenter + new Vector3(0.82f, 0.02f, -1.52f);

        // exterior-side door anchors (VERIFY each visually against the facade object named below)
        // Kaia house (E2). Front door faces +X; player walks the stepping stones from the east.
        // Facade: Current/Past_CentralPlaza_Chapter1_E2_Door at frontyard-c+(-4.64,*,0.12),
        //   frontyard-c = CentralPlazaVsCenter+(28,0,0.95). Chapter1E2RouteTriggerCenter is the anchor.
        private static readonly Vector3 KaiaHouseDoorTriggerCenter = CentralPlazaVsCenter + new Vector3(23.95f, 0.70f, 1.07f);
        private static readonly Vector3 KaiaHouseDoorExitTarget    = CentralPlazaVsCenter + new Vector3(24.55f, 0.02f, 1.07f);

        // Ruins facades carry F2/F3/F4/F5 name prefixes already (CreateRuinsSideHomesContinuation, L16013).
        // RuinsCenter == Chapter1RuinsMapCenter == CentralPlazaVsCenter+(45.50,0,0.05).
        // Top row faces -Z (player approaches from south); bottom row faces +Z.
        // F4 = F4_TopCenterHouse @ RuinsCenter+(-15.20,1.02,6.00)
        private static readonly Vector3 RuinsF4HouseDoorTriggerCenter = Chapter1RuinsMapCenter + new Vector3(-15.20f, 0.70f, 5.42f);
        private static readonly Vector3 RuinsF4HouseDoorExitTarget    = Chapter1RuinsMapCenter + new Vector3(-15.20f, 0.02f, 4.88f);
        // F3 = F3_TopLeftHouse @ RuinsCenter+(-19.35,1.02,6.00)
        private static readonly Vector3 RuinsF3HouseDoorTriggerCenter = Chapter1RuinsMapCenter + new Vector3(-19.35f, 0.70f, 5.42f);
        private static readonly Vector3 RuinsF3HouseDoorExitTarget    = Chapter1RuinsMapCenter + new Vector3(-19.35f, 0.02f, 4.88f);
        // F2 = F2_BottomLeftHouse @ RuinsCenter+(-23.35,0.88,-7.08), faces +Z
        private static readonly Vector3 RuinsF2HouseDoorTriggerCenter = Chapter1RuinsMapCenter + new Vector3(-23.35f, 0.70f, -6.40f);
        private static readonly Vector3 RuinsF2HouseDoorExitTarget    = Chapter1RuinsMapCenter + new Vector3(-23.35f, 0.02f, -5.86f);
        // F5 = F5_RightHouse @ RuinsCenter+(8.55,0.98,3.18), faces roughly -Z
        private static readonly Vector3 RuinsF5HouseDoorTriggerCenter = Chapter1RuinsMapCenter + new Vector3(8.55f, 0.70f, 2.52f);
        private static readonly Vector3 RuinsF5HouseDoorExitTarget    = Chapter1RuinsMapCenter + new Vector3(8.55f, 0.02f, 1.98f);
```

> Trap: `Chapter1RuinsMapCenter` and `CentralPlazaVsCenter` are declared earlier in the
> block (L451 / L408). Order your new lines after their declarations or use the literal.

### 4. Map-set roots + interior builders
`AnemoraFastVsHouseSliceSetup.cs` ~L9957-9985, where `miaInteriorRoot`/`ariaInteriorRoot`
are created and `CreateMiaInterior`/`CreateAriaInterior` are called. Add, mirroring:

```csharp
            var kaiaInteriorRoot   = CreateMapSetRoot(root, $"{prefix}_KaiaInteriorMap_SeparateSpace");
            var ruinsF4InteriorRoot = CreateMapSetRoot(root, $"{prefix}_RuinsF4InteriorMap_SeparateSpace");
            var ruinsF2InteriorRoot = CreateMapSetRoot(root, $"{prefix}_RuinsF2InteriorMap_SeparateSpace");
            var ruinsF3InteriorRoot = CreateMapSetRoot(root, $"{prefix}_RuinsF3InteriorMap_SeparateSpace");
            var ruinsF5InteriorRoot = CreateMapSetRoot(root, $"{prefix}_RuinsF5InteriorMap_SeparateSpace");
            ...
            CreateBareHouseInterior(kaiaInteriorRoot,   prefix, past, materials, KaiaInteriorCenter,    FastVsHouseArea.KaiaInterior,    "KaiaInterior",    "kaia_interior");
            CreateBareHouseInterior(ruinsF4InteriorRoot, prefix, past, materials, RuinsF4InteriorCenter, FastVsHouseArea.RuinsF4Interior, "RuinsF4Interior", "ruins_f4_interior");
            CreateBareHouseInterior(ruinsF2InteriorRoot, prefix, past, materials, RuinsF2InteriorCenter, FastVsHouseArea.RuinsF2Interior, "RuinsF2Interior", "ruins_f2_interior");
            CreateBareHouseInterior(ruinsF3InteriorRoot, prefix, past, materials, RuinsF3InteriorCenter, FastVsHouseArea.RuinsF3Interior, "RuinsF3Interior", "ruins_f3_interior");
            CreateBareHouseInterior(ruinsF5InteriorRoot, prefix, past, materials, RuinsF5InteriorCenter, FastVsHouseArea.RuinsF5Interior, "RuinsF5Interior", "ruins_f5_interior");
```

Add the thin builder (near `CreateMiaInterior` at L10243). It is just the shell — no
furniture — which is exactly the requested 「ひとまず長方形」:

```csharp
        private static void CreateBareHouseInterior(Transform root, string prefix, bool past, Materials materials, Vector3 c, FastVsHouseArea area, string objectToken, string landmarkToken)
        {
            // Reserve the room only: floor + 3 walls + exit glow pad + front drop guard.
            // CreateSmallHouseInteriorShell already builds a 6.40 (X) x 5.42 (Z) horizontal
            // rectangle and registers the HD-2D surface profiles + the interior exit pad.
            CreateSmallHouseInteriorShell(root, prefix, past, materials, c, area, objectToken, landmarkToken);
            // Optional: widen X a touch for the "気持ち横長め" feel without touching the shell helper.
        }
```

### 5. Areas struct population — mirror MiaInterior
Same file, ~L10064-10073 wires `currentAreas.MiaInterior`/`pastAreas.MiaInterior` into the
visibility component via `SerializedSet(visibility, "currentMiaInteriorMap", …)`. The
`currentAreas`/`pastAreas` holder is the struct that exposes `.MiaInterior`/`.AriaInterior`
(search `MiaInterior` to find its definition). For each of the 5 new areas:
1. Add a member to that struct (mirror `MiaInterior`), assigned from the map root created
   in step 4.
2. Add `SerializedSet(visibility, "currentKaiaInteriorMap", currentAreas.KaiaInterior);`
   and the `past…` line (×5, mirroring L10064-10069).

> Trap: the map root must be assigned into BOTH the current pass and the past pass holders
> (the builder runs once per timeline). Follow exactly how MiaInterior is threaded — do not
> invent a new path.

### 6. NEW MECHANIC — time-conditional door
`Assets/Scripts/FastVS/FastVsAreaDoorTransition.cs`. Add a tri-state requirement keyed off
the existing `portalController.PlayerInOtherTime` (the controller already exposes it,
L107, and `IsPlayerInsideTrigger` already branches on it, L120).

Add the enum + field (after the other `[SerializeField]`s, ~L23):
```csharp
        public enum DoorTimeRequirement { Any, PresentOnly, OtherTimeOnly }
        [SerializeField] private DoorTimeRequirement requiredTime = DoorTimeRequirement.Any;
        public DoorTimeRequirement RequiredTimeForReview => requiredTime;
```

Add this gate as the FIRST check inside `Update()` (after `ResolveReferences();`, before
the existing early-return block at L65) AND at the top of `TryEvaluateCurrentPlayerForReview`:
```csharp
            if (!IsRequiredTimeSatisfied())
            {
                return; // (return false; in TryEvaluateCurrentPlayerForReview)
            }
```
And the helper:
```csharp
        private bool IsRequiredTimeSatisfied()
        {
            if (requiredTime == DoorTimeRequirement.Any) return true;
            if (portalController == null) return requiredTime == DoorTimeRequirement.PresentOnly;
            var inOther = portalController.PlayerInOtherTime;
            return requiredTime == DoorTimeRequirement.OtherTimeOnly ? inOther : !inOther;
        }
```

> Trap: only the *entry* doors (Ruins→RuinsFNInterior) need `OtherTimeOnly`. The *exit*
> doors (RuinsFNInterior→Ruins) stay `Any` — the player is already inside.

### 7. Door factory param
`AnemoraFastVsHouseSliceSetup.cs`, `CreateAreaDoorTransition` (L39020). Add a trailing
optional param and serialize it:
```csharp
        private static void CreateAreaDoorTransition(
            string name, ... , FastVsStoryFlowController storyFlow,
            FastVsAreaDoorTransition.DoorTimeRequirement requiredTime = FastVsAreaDoorTransition.DoorTimeRequirement.Any)
        {
            ...
            SerializedSet(transition, "transitionHoldSeconds", 0.08f);
            SerializedSet(transition, "requiredTime", requiredTime);
        }
```

### 8. Door entries
`CreateHouseDoorTransitions` (L38782). Append 10 door calls (5 pairs), mirroring the
Mia/Aria pairs at L38808-38855. Both-time pairs (E2, F4) omit `requiredTime`. Past-only
entry doors (F2/F3/F5) pass `DoorTimeRequirement.OtherTimeOnly` on the
`Ruins → RuinsFNInterior` direction only:

```csharp
            // E2 — Kaia house (both times)
            CreateAreaDoorTransition("FastVS_DoorTransition_KaiaFarm_To_KaiaInterior", controller, player, areaVisibility,
                FastVsHouseArea.KaiaFarm, FastVsHouseArea.KaiaInterior,
                KaiaHouseDoorTriggerCenter, DoorTriggerSize, KaiaInteriorDoorExitTarget, "Door: Kaia house to interior local", storyFlow);
            CreateAreaDoorTransition("FastVS_DoorTransition_KaiaInterior_To_KaiaFarm", controller, player, areaVisibility,
                FastVsHouseArea.KaiaInterior, FastVsHouseArea.KaiaFarm,
                KaiaInteriorDoorTriggerCenter, DoorTriggerSize, KaiaHouseDoorExitTarget, "Door: Kaia interior to farm local", storyFlow);

            // F4 — ruin (both times)
            CreateAreaDoorTransition("FastVS_DoorTransition_Ruins_To_RuinsF4Interior", controller, player, areaVisibility,
                FastVsHouseArea.Ruins, FastVsHouseArea.RuinsF4Interior,
                RuinsF4HouseDoorTriggerCenter, DoorTriggerSize, RuinsF4InteriorDoorExitTarget, "Door: ruin F4 to interior local", storyFlow);
            CreateAreaDoorTransition("FastVS_DoorTransition_RuinsF4Interior_To_Ruins", controller, player, areaVisibility,
                FastVsHouseArea.RuinsF4Interior, FastVsHouseArea.Ruins,
                RuinsF4InteriorDoorTriggerCenter, DoorTriggerSize, RuinsF4HouseDoorExitTarget, "Door: ruin F4 interior to ruins local", storyFlow);

            // F2 / F3 / F5 — past (other-time) only on ENTRY
            CreateAreaDoorTransition("FastVS_DoorTransition_Ruins_To_RuinsF2Interior", controller, player, areaVisibility,
                FastVsHouseArea.Ruins, FastVsHouseArea.RuinsF2Interior,
                RuinsF2HouseDoorTriggerCenter, DoorTriggerSize, RuinsF2InteriorDoorExitTarget, "Door: ruin F2 (past) to interior local", storyFlow,
                FastVsAreaDoorTransition.DoorTimeRequirement.OtherTimeOnly);
            CreateAreaDoorTransition("FastVS_DoorTransition_RuinsF2Interior_To_Ruins", controller, player, areaVisibility,
                FastVsHouseArea.RuinsF2Interior, FastVsHouseArea.Ruins,
                RuinsF2InteriorDoorTriggerCenter, DoorTriggerSize, RuinsF2HouseDoorExitTarget, "Door: ruin F2 interior to ruins local", storyFlow);
            // …repeat identically for F3 (RuinsF3*) and F5 (RuinsF5*).
```

---

## Smoke test steps (Unity batchmode, then runtime)

1. **Compile + regenerate** (2-pass, keeps FilmGrain — see project memory):
   `Unity -batchmode -quit -projectPath . -executeMethod AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
   Expect: 0 compile errors; validator passes; no `NullReferenceException` in the log.
2. **Enum/visibility coverage**: `ValidateHouseSliceBatch` must report all map sets present
   (`HasAllMapSetsForReview == true`). Expect log line confirming all areas incl. the 5 new
   interiors are non-null.
3. **Door round-trips (review harness)**: drive each new door via
   `TryEvaluateCurrentPlayerForReview` and assert the active area flips and warps back.
   Expect: KaiaFarm⇄KaiaInterior and Ruins⇄RuinsF4Interior succeed in BOTH timelines.
4. **Past-only gate**: with `PlayerInOtherTime == false`, the Ruins→RuinsF2/F3/F5Interior
   doors must NOT fire (player stands on the trigger, area unchanged). With
   `PlayerInOtherTime == true`, they must fire. Add/extend a validation probe asserting both.
5. **Runtime .exe**: build + boot, walk to each door.
   - Kaia farmhouse: enter → bare room → exit returns to the front yard.
   - Ruins F4: enter in present → bare room → exit.
   - Ruins F2/F3/F5 in present: walking the door does nothing (rubble). Open the time
     window to the past, walk the same door: it opens.
   Capture 5-area review screenshots + the new interiors to
   `docs/review/<ts>_chapter1_kaia_ruins_interiors/` and report the absolute .exe path.

## Open risks

1. **Time-state across the F2/F3/F5 transition (highest).** Entering a past-only interior
   calls `WarpPlayerToLocalForReview` + `SetActiveAreaWithLightingTransitionForReview`.
   Verify in-editor whether the player remains "other-time" inside and on exit. Acceptable
   MVP: door only *opens* in past; once inside it is a normal room; on exit you return to
   the ruins (rubble) — Tom signed off on a placeholder. If it feels wrong, use
   `portalController.ForcePlayerOtherTimeLocalForReview` / keep `PlayerInOtherTime` on exit.
   Do NOT block the whole task on perfect time-state polish — ship the gate + round-trip
   first, note residual behavior.
2. **Exterior door anchors are approximate.** The `*HouseDoorTriggerCenter` Z/X offsets put
   the trigger ~0.6 in front of each facade; confirm against the named facade object that it
   sits just outside the visible door and the exit target lands on walkable ground (not
   inside a wall/fence). Adjust per the review screenshots.
3. **F2/F3/F4/F5 ⇄ reference mapping.** The code's facade object names already use these
   prefixes; cross-check against slides 7 & 14 that F4=top-center (both-time) and
   F2/F3/F5 are the past-settlement homes before committing coordinates.
4. **Enum-order serialization.** Appending enum values (not inserting) keeps existing
   serialized `activeArea`/`sourceArea`/`targetArea` ints stable. Append only — never
   reorder.
5. **Bloat guard / EULA**: stage with explicit pathspec (no `git add -A`); the regenerated
   `HouseSlice.unity` + `docs/review/*` are blocked by the bloat guard by design — that's
   expected, don't fight it. No paid-asset files involved here.
