# Chapter 1 — Foliage: replace box-grass with cross-quad sprite cards — Codex handoff

## Context

Working tree: `C:\Users\maro6\Documents\Unity\Anemora-fast-vs-v24-sample`, branch
`work/chapter1-continuation-map-vs-20260524`. Unity 6000.3.14f1, URP, HD-2D.

Tom's complaint: 草木が適当に見える. Approved direction: **free, project-native cross-quad
sprite cards** (no paid asset, no Unity Terrain). Validate on the Mia yard first, then roll
out.

### Root cause (verified)
Every plant in the world is a stretched/rotated primitive **cube**:
- `CreateGrassTuft` (L38629) = three `0.12 x 0.40 x 0.12` tilted cubes.
- `CreateFlowerPatch` (L38621) = four tiny cubes (leaf + bloom).
- `CreateChapter1Tree` (L19621) = trunk + 2 crown cubes.
- "underbrush"/"plant bed"/"plant mass" = flat `~0.05`-tall `CreateLandmarkCube`s.

There is **no Unity Terrain** in the project (so terrain-detail tools don't apply). The fix
is to render foliage as alpha-clipped **cross-quad cards** (2-3 intersecting planes → a
circular silhouette that survives the tilted/orbiting review camera), lit by the existing
ramp shader so they match the world's value. This is exactly how Octopath/Triangle Strategy
do ground foliage.

### What we already own (reuse, don't reinvent)
- Shader `Assets/Art/Shaders/FastVS/FastVS_SpriteCardRampLit.shader` — already alpha-clip
  (`Queue=AlphaTest`, `RenderType=TransparentCutout`, `_AlphaClip=1`, `_Cutoff`, `_ZWrite=1`),
  ramp-lit (`_TopLight`/`_SideShade`/`_FloorShade`), receives world light/shadow.
- `CreateQuad(string name, Transform parent, Vector3 localPosition, Vector3 localScale, Material material)` (L84070).
- `ConfigureSpriteCardCutoutMaterial(material, SpriteCardCutoutRenderQueue)` (L90648) and
  `const int SpriteCardCutoutRenderQueue = 2450` (L37). Sprite-card materials are **validated**
  to sit at AlphaTest queue ~2450 (L51080) — foliage cards MUST comply or batch validation fails.
- CC0 sprite already in repo: `Assets/Art/External/OpenGameArt/edomin_tree_sprites_cc0/tree3_0.png`.
- `com.unity.modules.wind` is already in the manifest (shared WindZone available).
- Post stack (Fronkon TiltShift + Buto fog) already sells the diorama framing — no change.

---

## Plan (two phases; Phase 1 is the deliverable for this pass)

**Phase 1 — grass + flowers → cross-quad cards (high volume, unambiguous, low risk).**
Reroute the two highest-traffic foliage helpers (`CreateGrassTuft`, `CreateFlowerPatch`) to
emit cross-quad clusters. These are unambiguously "grass/flowers" everywhere, so a single
helper-body change upgrades every map at once while touching only ~2 functions (NOT the
hundreds of `CreateLandmarkCube` plant-bed calls — leave those for Phase 2). Judge the look
on the Mia yard screenshot first.

**Phase 2 (defer, separate pass) — trees + box "plant beds".** `CreateChapter1Tree` →
billboard/cross-quad tree cards (or a single cohesive CC0 model family re-lit through
`FastVS_SurfaceRampLit`), and the flat box "underbrush/plant-bed" `CreateLandmarkCube`s →
card clusters. Out of scope here.

---

## Numbered mechanical fixes (Phase 1)

### 1. Foliage card art (CC0)
Author 3-4 small alpha cards under `Assets/Art/External/<source>/`:
- 2 grass-blade-clump cards, 1 small-bush card, 1 flower card.
- **Source**: Kenney "Foliage Sprites" (CC0, https://kenney.nl/assets/foliage-sprites) is
  ideal; fallback = recolor/crop the existing `edomin_tree_sprites_cc0` CC0 art. Keep them
  CC0 only (no Synty — its EULA forbids generative-AI ingestion, which conflicts with this
  pipeline).
- Import settings: `Texture Type = Default` (or Sprite), `Filter = Point`, `Alpha Is
  Transparency = on`, mipmaps off, small (≤256px). Tight alpha (minimal empty space) to
  limit overdraw.
- Record license/attribution in `Assets/Art/External/<source>/LICENSE.txt`.

### 2. Foliage card material helper
Add near `ConfigureSpriteCardCutoutMaterial` (L90648):
```csharp
        private static Material EnsureFoliageCardMaterial(string texturePath, string materialKey)
        {
            // Cache by materialKey if the file already builds cached materials elsewhere; otherwise create.
            var shader = Shader.Find("Anemora/FastVS/FoliageCardLit"); // see step 4; falls back to SpriteCardRampLit
            if (shader == null) shader = Shader.Find("Anemora/FastVS/SpriteCardRampLit");
            var mat = new Material(shader);
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
            mat.SetTexture("_BaseMap", tex);
            mat.SetTexture("_MainTex", tex);
            mat.SetFloat("_Cutoff", 0.5f);   // alpha-CLIP, not blend
            mat.SetFloat("_WindStrength", 0.06f); // 0 on character cards; >0 here
            ConfigureSpriteCardCutoutMaterial(mat, SpriteCardCutoutRenderQueue); // queue ~2450, passes validator
            return mat;
        }
```

### 3. Cross-quad cluster helper
Add near `CreateGrassTuft` (L38629). Builds 2-3 intersecting quads → circular silhouette:
```csharp
        private static void CreateFoliageCardCluster(
            Transform root, string objectName, Vector3 center, Vector2 cardSize,
            Material cardMaterial, int variantSeed, int planeCount = 3)
        {
            // Deterministic per-cluster jitter (no Random — keep regeneration reproducible).
            var baseYaw = (variantSeed * 37) % 180;
            for (var i = 0; i < planeCount; i++)
            {
                var yaw = baseYaw + i * (180f / planeCount);                 // non-parallel planes
                var scaleJitter = 0.88f + ((variantSeed + i) % 5) * 0.05f;   // 0.88..1.08
                var quad = CreateQuad(
                    $"{objectName}_Card{i}", root,
                    center + new Vector3(0f, cardSize.y * 0.5f, 0f),         // pivot at base, card stands up
                    new Vector3(cardSize.x * scaleJitter, cardSize.y * scaleJitter, 1f),
                    cardMaterial);
                quad.transform.localRotation = Quaternion.Euler(0f, yaw, 0f);
            }
        }
```
> Trap: cards are **fixed cross-quads, NOT `FastVsPaperBillboard`** (camera-facing). Grass
> must stay planted; only character sprites billboard. Single planes go edge-on and vanish
> when the review camera orbits — that's why we use ≥2 non-parallel planes.

### 4. Cheap wind (dedicated foliage shader variant)
Duplicate `FastVS_SpriteCardRampLit.shader` → `FastVS_FoliageCardLit.shader` (rename the
`Shader "Anemora/FastVS/FoliageCardLit"` line). Add a `_WindStrength`/`_WindSpeed` property
and, in the vertex stage, sway the **top** of the card by world position + time:
```hlsl
        // properties
        _WindStrength("Wind Strength", Range(0,0.3)) = 0.06
        _WindSpeed("Wind Speed", Range(0,4)) = 1.4
        // in Vert(), after computing positionWS, before TransformWorldToHClip:
        float sway = sin(_Time.y * _WindSpeed + positionWS.x + positionWS.z) * _WindStrength;
        positionWS.x += sway * saturate(IN.uv.y); // uv.y≈1 at top, 0 at base → only tips move
```
Using a dedicated variant keeps character cards (on the original shader) perfectly still.
Default `_WindStrength` low; tie to a shared `WindZone` later if desired.

### 5. Reroute the two helpers
`CreateGrassTuft` (L38629) and `CreateFlowerPatch` (L38621): replace the cube bodies with
cluster calls. Keep the same signatures so all call-sites are untouched.
```csharp
        private static void CreateGrassTuft(Transform root, string prefix, Vector3 center, Material material, int index)
        {
            // 'material' was the leaf/grass color source; pick the matching card material.
            var card = EnsureFoliageCardMaterial(GrassCardTexturePathFor(material), "foliage_grass");
            CreateFoliageCardCluster(root, $"{prefix}_GrassTuft{index}", center, new Vector2(0.42f, 0.46f), card, index, 3);
        }
```
`CreateFlowerPatch` → one bush/leaf cluster + a small flower cluster (use `flowerA`/`flowerB`
to choose the flower card tint). Match the prior footprint (~0.4 wide, ~0.2-0.46 tall) so
nothing pops in scale.
> Trap: preserve the existing `past ? ... : ...` material choices at every call-site by
> mapping the passed `Material` (e.g. `materials.PastGrass` vs `CurrentGrass`,
> `Leaf`/`CurrentLeaf`) to the right card texture/tint, so the past=lush / current=worn
> contrast the reference specifies (e.g. Kaia 草 生い茂る vs 少し剥げ) is preserved.

### 6. Validate-only-grass-first
Do NOT touch `CreateChapter1Tree`, the box "plant bed" `CreateLandmarkCube`s, or any other
helper in this pass. Only `CreateGrassTuft` + `CreateFlowerPatch` change.

---

## Smoke test steps

1. **Compile + regenerate (2-pass, keeps FilmGrain):**
   `Unity -batchmode -quit -projectPath . -executeMethod AnemoraFastVsHouseSliceSetup.BuildAndValidateBatch`
   Expect: 0 compile errors; the sprite-card material validator (L51080) passes (foliage
   materials at queue ~2450); no NRE.
2. **Mia yard look check (the gate):** capture the Mia-house-exterior current + past review
   screenshots to `docs/review/<ts>_foliage_crossquad/`. Expect: grass reads as soft
   blade clumps, not boxes; cards never go edge-on under the orbit; past=lush vs current=worn
   preserved; no z-fighting/flicker (alpha-clip writes depth).
3. **Overdraw sanity:** confirm framerate/quad count is reasonable (grass cluster = 3 quads
   vs previously 3 cubes — roughly even). Flag if any map explodes in count.
4. **Roll-out screenshots:** capture the 5-area pair (plaza/Mia/Aria/Kaia/ruins) so Tom can
   judge cohesion across maps. Report the absolute .exe path and a folder-launch note.

## Open risks

1. **Transparency in URP:** use alpha-**CLIP** (`_Cutoff` 0.5, queue 2450) NOT alpha-blend,
   so cards write depth and sort under the tilted cam. The shared shader is already cutout —
   keep it that way.
2. **Overdraw:** keep cards tight (minimal empty alpha), few planes (≤3), tight cluster
   footprints. Don't stack many parallel planes.
3. **Value cohesion:** cards MUST be lit through the ramp shader (FoliageCardLit inherits
   SpriteCardRampLit's lighting) or they float as flat overlays. Tune tint to the scene's
   muted grouped values.
4. **Scale match:** world uses ~1-unit metrics (grass tufts were ~0.4u tall). Keep
   `cardSize ≈ (0.42, 0.46)`; verify against character/building scale.
5. **Don't mass-edit the giant file:** Phase 1 touches only 2 helper bodies + 2 new helpers
   + 1 new shader. Resist swapping every box call now.
6. **License/bloat:** CC0 art only; record attribution. Stage with explicit pathspec; the
   regenerated `HouseSlice.unity` + `docs/review/*` trip the bloat guard by design — expected.
   New CC0 PNGs under `Assets/Art/External/...` are small source assets and are fine to commit.
7. **Wind on shared shader:** if you instead add `_WindStrength` to the shared
   `SpriteCardRampLit` (rather than a variant), default it to 0 so character billboards don't
   wave — but the dedicated `FoliageCardLit` variant is the safer recommendation.
