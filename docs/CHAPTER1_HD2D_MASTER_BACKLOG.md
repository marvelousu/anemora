# Chapter 1 - HD-2D MASTER backlog (synthesized from 12-dimension survey)

Operating loop / guardrails / the MANDATORY R2-propagate step: see CHAPTER1_HD2D_AUTONOMOUS_BACKLOG.md.
Work top-down within tier. autonomousSafe=false => propose/log only, do NOT finalize unattended.

## P0  (21 items)

### 0. [coordinator-added 2026-05-31] Past-timeline background VOID — give the past space a backdrop
*Sky/Atmosphere - effort M - NEEDS-TOM*
- **Goal:** The past timeline renders pure black above the diorama (confirmed in P0-12 capture `03_past_market_fog_on`) — a glaring void; fog cannot fix it. Give the past space a backdrop so it does not read as broken. Pre-existing issue, not caused by P0-12/13.
- **Approach:** The past space is a separate render layer behind the time-window aperture. Add a dark gradient/sky backdrop plane or skybox to the past-space render only. CAUTION: the aperture/past-space rendering is delicate — ALWAYS eyeball the aperture PNG after changes (known black-aperture failure mode). Conservative neutral gradient baseline only; final backdrop art is Tom's call.
- **Acceptance:** Past-timeline overviews no longer show pure-black void above the set; a neutral gradient/sky sits behind, and the time-window aperture still renders correctly (not black). A/B capture; do NOT finalize the art unattended.

### 1. Lock the signature HD-2D rig: narrow-FOV perspective camera at fixed tilt (Cinemachine 3.1, NOT orthographic)
*Camera - effort S - NEEDS-TOM*
- **Goal:** Establish one authored CinemachineCamera rig (perspective, FOV ~22-32deg, pitch ~28-35deg, fixed distance) as the project default so every area inherits the identical diorama angle and the telephoto compression that flattens sprites while preserving vertical parallax.
- **Approach:** Unity 6 ships Cinemachine 3.1.x. Main Camera gets CinemachineBrain; a CinemachineCamera with Camera Projection=Perspective (never ortho), FOV~25, rig pitch ~30deg via transform, CinemachineFollow/Position Composer with fixed offset. Expose FOV/pitch/distance on a CameraRigProfile ScriptableObject so per-area tuning is data-driven; all other cams are priority-blended cams inheriting it. This gates DoF/tilt-shift correctness (URP Bokeh DoF is broken on ortho cameras).
- **Acceptance:** Screenshot a character at bottom vs top of a tall prop: near/far edges show vertical convergence (parallax); a vertical pole at screen-left vs right both lean slightly toward center, proving perspective not ortho. Camera Inspector shows Projection=Perspective, FOV<=32, X-rot 28-35deg.

### 2. Script-driven Bokeh DoF focus that tracks the player (URP has no built-in focus tracking)
*Camera/Post - effort M - NEEDS-TOM*
- **Goal:** Make URP Bokeh Depth of Field focus plane follow the active target every frame so foreground/background fall softly out of focus (tilt-shift miniature) while the player stays crisp, with a reusable diorama focus-band recipe.
- **Approach:** CRITICAL Unity 6 fact: Cinemachine AutoFocus / 'Focus Tracks Target' only drives Camera.focusDistance on HDRP, NOT URP. In URP write DepthOfField.focusDistance yourself in LateUpdate: profile.TryGet<DepthOfField>(out dof); compute camera-to-target distance (or viewport-center raycast for ground-aware focus); dof.focusDistance.value = Mathf.Lerp(current, target, speed*dt). Set Mode=Bokeh, Focal Length 80-120mm, Aperture f/4-5.6 (low f over-blurs/crushes readability), Blade Count 6-8, Curvature 0.8-1.0. Keep a Gaussian fallback (Max Radius 0.7-1.0, High Quality Sampling ON to kill foliage flicker) for low-end. Throttle to every N frames. Reference DjoleDzele/URP-Autofocus pattern.
- **Acceptance:** Player mid-diorama is tack-sharp; a prop ~2-3m nearer AND one ~6-10m farther are both visibly blurred; a bright lamp/portal highlight renders as a soft rounded (not square) bokeh disc. Walking toward the far prop moves the sharp band to it. If near and far blur identically regardless of focus, the camera is still ortho (item 1 failed).

### 3. Per-time-of-day Kelvin key light + intensity/angle curve on the existing sun cycle
*Lighting - effort M - NEEDS-TOM*
- **Goal:** Drive the existing sun/ToD cycle with physically-motivated color temperature and elevation so dawn/noon/dusk/night each have a distinct, art-directed key-light hue, angle and shadow length instead of one white sun rotated over time.
- **Approach:** Enable GraphicsSettings.lightsUseLinearIntensity + Light.useColorTemperature, then animate Light.colorTemperature (Kelvin) and the sun transform on the ToD controller instead of raw RGB. Keyframes: dawn ~3000-3500K low (10-20deg), noon ~6000-6500K high, dusk ~2800-3200K low, night a separate dim cool 'moon' directional ~7000-9000K crossfaded in (URP shadows only from the MAIN directional, so crossfade sun<->moon). Pair with a per-ToD AnimationCurve for intensity. Publish sun color as a global shader prop for rim/spec/foliage consumers.
- **Acceptance:** Four screenshots (dawn/noon/dusk/night) from one camera show clearly different key-light hue (warm-orange low sun at dawn/dusk, neutral-cool high noon, blue dim night) AND visibly different shadow direction/length; sampling lit ground in an editor shows distinct hue per ToD.

### 4. Adaptive Probe Volumes ambient floor + night readability safeguard (kill near-black shadow sides)
*Lighting - effort M - NEEDS-TOM*
- **Goal:** Replace flat single-color ambient with Adaptive Probe Volumes so shadowed sides of buildings/foliage/props get directional, ToD-tinted bounce/fill and never crush to unreadable black; establish a tested luminance floor so no walkable area goes below readability at night.
- **Approach:** Unity 6 URP supports APV: add a Probe Volume over the diorama, enable Baked GI, bake with Progressive GPU Lightmapper, enable Sky Occlusion so ambient re-tints from sky across the ToD cycle from one bake. Set Ambient Mode=APV; tighten probe spacing around play space. Floor ambient so darkest shadow pixel stays ~0.05-0.08 luminance with a cool tint opposing the warm key. Combine with a night-specific Volume lift + placed warm practical lights (lanterns ~1800-2700K) so warm readable pools sit against the cool ambient. Use the Rendering Debugger to inspect luminance; set a project rule (darkest walkable >=~0.05, key silhouettes >=~0.12). Depends on sane foliage/ground normals.
- **Acceptance:** At dusk/night, darkest shadow-side pixels of a building/foliage measure >~0.05 luminance with a cool tint and a directional gradient (lighter toward open sky); a night-town shot shows readable paths/silhouettes and warm practical pools while still reading as night.

### 5. Single master HDR color grade: ONE Volume stack, tonemapper, split-toning warm/cool baseline
*Post/Lighting - effort M - NEEDS-TOM*
- **Goal:** Author one shared HDR color-grade Volume stack (Tonemapping + Color Adjustments + White Balance + Split Toning) pushing warm highlights against cool shadows for the Octopath cinematic-filter cohesion, owned in a single Volume so it never double-grades with bloom/DoF/tilt-shift.
- **Approach:** On an HDR camera, Tonemapping MUST be on or values clamp at 1; start Neutral (clean base for grading), A/B vs ACES (more cinematic but can over-cook toon ramps). Add Color Adjustments (URP has NO auto-exposure, set Post Exposure manually per area; Contrast +5..15, Saturation +5..20, Color Filter tint), White Balance (amber dawn/dusk, blue night), Split Toning (highlights hue ~30-45, shadows ~200-220). Implement as a global default Volume plus per-ToD overrides blended by the ToD weight. Optionally bake a 32-cube LUT. CRITICAL: share this one stack/tonemapper with the bloom/DoF/tilt-shift items to avoid double-grading.
- **Acceptance:** Toggle grade off/on at noon and dusk: highlights skew warm, shadows skew cool (measurable hue separation in a picker), no clipped pure-white on lit foliage, darkest sprite pixels not pure (0,0,0) and brightest not blown; the four ToD states form one cohesive palette.

### 6. Vertex-color RGBA splat-blend ground shader for hand-built dioramas (no Terrain)
*Shader/Ground - effort M - auto-safe*
- **Goal:** Build one master ground Shader Graph that blends up to 4 ground materials (grass/dirt/path/rock) via the mesh's per-vertex COLOR.rgba so any diorama mesh can be painted instead of relying on hard material seams or sub-meshes.
- **Approach:** Fork the existing ramp-lit ground shader: add Vertex Color node, split R,G,B,A weights, sample 4 albedo/normal sets, blend with normalized weights, feed the existing toon/ramp lighting (wire into the ramp Sub Graph, not the Lit master node, so it matches current lighting). Subdivide low-poly ground meshes (Blender or ProBuilder) for enough verts to paint smooth transitions. Reference Daniel Ilett 2025 splat tutorial + Unity Discussions 'Splat Map for Meshes'.
- **Acceptance:** A single continuous ground mesh visibly shows >=3 distinct surface types (grass/dirt/rock) with soft blended transitions and no hard polygon-edge seam between types.

### 7. Paint diorama ground in-editor with Polybrush vertex-color brush
*Ground/Workflow - effort S - NEEDS-TOM*
- **Goal:** Adopt Polybrush as the authoring tool to paint vertex colors (and optionally sculpt) directly onto diorama ground meshes, feeding the splat shader, so worn paths/mud/grass clumps are placed by eye.
- **Approach:** Install com.unity.polybrush (1.1.8+, supported in Unity 6) via Package Manager. Use Color brush mode mapping RGBA to the 4 ground layers; point it at the splat shader from the ground-shader item (Polybrush paints mesh.colors so any vertex-color shader works). Use Sculpt mode to break perfectly flat planes. Polybrush edits the shared mesh asset, so duplicate/save diorama meshes before painting. Same brush later scatters prefab foliage along diorama rims.
- **Acceptance:** Before/after screenshot (or recording) shows a dirt path painted onto a grass mesh with the color brush appearing live via the splat shader.

### 8. Replace primitive-cube placeholder props with a cohesive CC0 model kit on the toon ramp shader
*Props/Buildings - effort L - NEEDS-TOM*
- **Goal:** Eliminate every grey-box/scaled-cube placeholder (crates, barrels, fences, stalls, carts, wells, signposts) and swap in one stylistically unified low-poly prop library so the diorama reads as authored, not blocked out.
- **Approach:** Import one CC0 megakit (Quaternius Medieval Village MegaKit + Fantasy Props MegaKit) so every prop shares one art voice. Reassign your existing ramp/toon-lit URP shader to all imported meshes via a shared material (do NOT keep imported PBR materials). Build a Prefab variant per prop, enable GPU instancing, mark static props Contribute GI/Static for baked AO and probes. Decimate aggressively at the tilt distance. CC0 = AI-pipeline safe; AVOID Synty (EULA forbids AI ingestion).
- **Acceptance:** Any populated scene contains zero recognizable axis-aligned untextured cube/cylinder primitives; every foreground prop has a distinct authored silhouette and shares the same shading ramp as the ground.

### 9. One cohesive stylized material treatment (shared ramp + handful of master texture atlases)
*Props/Buildings - effort M - NEEDS-TOM*
- **Goal:** Define a small locked palette of master materials (wood, plaster, stone, roof-tile, thatch, metal) all driven by the existing ramp/toon-lit shader so every building and prop shares one lighting response and color family.
- **Approach:** Author 6-8 stylized tiling textures (or pull CC0 from FreeStylized/ambientCG) through ONE Shader Graph master wrapping the ramp lighting so albedo varies but light response is identical. Keep smoothness low/uniform to avoid realistic specular that fights the painterly look. Use Unity 6 Material Variant hierarchy so a single ramp tweak propagates. Posterize/flatten any photoreal CC0 source so it doesn't read realistic.
- **Acceptance:** A shot of buildings+ground+props together shows the same soft ramp shadow falloff and palette family; no surface shows sharp realistic specular or a visibly different lighting model from neighbors.

### 10. Lit emissive HDR windows + interior glow on all buildings (feeds Bloom, ToD-driven)
*Props/Buildings - effort M - NEEDS-TOM*
- **Goal:** Give every building windows/door-gaps that glow warmly with believable dusk/night interior light so settlements read as inhabited across the day/night cycle.
- **Approach:** Add an emissive window submaterial with HDR emission (intensity >1 so it blooms) on your shader; enable HDR in the URP Asset and a Bloom override. Drive emission intensity from the ToD system (animate a global shader prop) so windows fade up at dusk. For interior depth, place a low-cost emissive quad or single baked point light just inside the opening and let Light Probes/APV carry warmth to nearby billboard characters; set Emission GI=Baked so it tints lightmaps on adjacent walls. Emission alone won't bloom in SDR.
- **Acceptance:** A dusk/night shot shows multiple windows emitting a warm bloom halo (soft glow beyond the rect) while the daytime shot shows the same windows dimmed/off; glow color tints the wall immediately around each window.

### 11. Enable + tune SSAO and grounded contact AO so every prop/sprite sits in the diorama
*Post/Props - effort M - auto-safe*
- **Goal:** Add the URP SSAO Renderer Feature and per-prop/per-character contact darkening so no object floats; ground flat billboard sprites and hand-built meshes onto the set.
- **Approach:** Add SSAO as a Renderer Feature (not a Volume override). Source=Depth Normals (avoids edge artifacts on hard diorama edges), Intensity ~0.3-0.6 (high AO crushes the toon ramp), Radius small (~0.1-0.3m, miniature scale), Falloff to fade before the blurred background, Samples Medium(8), Blur Medium/High, Direct Lighting Strength ~0.25. Bake AO into lightmaps for static props (Contribute GI). For hero/dynamic props and characters, add a GPU-instanced soft radial AO blob via URP Decal Projector at the base (Decal Renderer Feature; docs warn to minimize/instance it). Watch over-darkening when After-Opaque SSAO combines with baked AO.
- **Acceptance:** SSAO on vs off shows soft darkening in mesh corners, under crates/foliage bases, and where props/sprites meet ground; effect is subtle (no harsh black halos around sprite cards, no speckle on flat lit ground); every prop/character shows a contact band at its base.

### 12. Height + distance fog with art-directed near->far color gradient (atmospheric perspective)
*Atmosphere - effort M - NEEDS-TOM*
- **Goal:** Add one authoritative fog system fading distant diorama geometry toward a horizon/sky color, driven by both world-height and camera distance, tunable per scene via a Volume profile, to produce HD-2D layer separation.
- **Approach:** Full Screen Pass Renderer Feature (Unity 6 Render Graph) sampling _CameraDepthTexture (enable Depth Texture on the URP Asset). Reconstruct world position, compute fogFactor over a near/far distance band AND a world-Y height band (exp falloff). Drive fog color from a horizontal Gradient texture by normalized distance (near=warm key, far=cool/desaturated) rather than a single color (SC Post Effects gradient mode). Expose density/height/colorGradient in a custom Volume component per diorama. One fullscreen pass compositing after opaque, consistent across ground/buildings/sprite cards. Prefer free meryuhi/URPFog or SC Post Effects before buying.
- **Acceptance:** Sampling nearest ground row vs farthest visible ridge shows a measurable hue shift toward the horizon color and a luminance/contrast drop on the far layer; fog disabled shows the far ridge at near-foreground saturation; two scenes show different fog gradients from their Volume overrides.

### 13. Volumetric sun god rays / light shafts from the ToD directional light (shared with dust + dawn/dusk)
*Atmosphere/Lighting - effort L - NEEDS-TOM*
- **Goal:** Render true raymarched, shadow-occluded in-scattering shafts from the existing sun so dust-laden air glows where light streams between buildings and tree cards; intensity/color driven by the ToD curve.
- **Approach:** Render Graph-compatible Renderer Feature reading the main light's cascaded shadow map so shafts are correctly cut by buildings and alpha-clipped tree cards. Free MIT baseline: CristianQiu/Unity-URP-Volumetric-Light (main+additional lights, shadow-aware, RG-compatible). Tie shaft intensity/color to ToD (strong warm at low dawn/dusk angle, near-zero at noon, faint cool at night) via the same ToD driver. Render at half-res with depth-aware bilateral upscale (~0.2-0.5ms). Layer over existing fog; keep density low so it doesn't wash out the grade. This is the single 'shared' god-ray system; the dust-mote item lives inside these shafts.
- **Acceptance:** At a low sun angle behind tree cards, distinct brightening cones align to the light direction and change shape when an occluder is moved; cones fade out at high noon; toggling the feature off removes cones while flat lighting remains.

### 14. GPU-instanced living grass ground-carpet under the cards (RenderMeshIndirect + GPU cull)
*Foliage - effort L - NEEDS-TOM*
- **Goal:** Add a dense animated grass-blade carpet growing out of the diorama ground beneath the existing sprite cards so walkable surfaces read as living turf, not a flat ramp-lit plane with cards floating on it.
- **Approach:** Graphics.RenderMeshIndirect (NOT deprecated DrawMeshInstancedIndirect) with a GraphicsBuffer.Target.IndirectArguments buffer. Bake blade transforms by raycasting the hand-built ground mesh into a positions GraphicsBuffer at edit time; a compute shader does per-frame frustum+distance culling (cheap because tilt-shift keeps only a narrow band in focus). Blade = 2-3 tri quad with a Shader Graph doing view-space pixel-snap, alpha-clip tip, vertical ramp tinted to the toon ground. Reads the shared wind globals (control-plane item). Reference Cyanilux GPU Instanced Grass (2025) + ColinLeung-NiloCat. Verify deploy through the full Apply/Integrator path, not Refresh-only.
- **Acceptance:** A tilt screenshot of a grassy area shows discrete swaying blades (>=~50k visible) matching the ground ramp; layer off vs on is an obvious before/after; blades cull correctly outside the frustum (no edge popping in a pan).

### 15. Unified global-property wind / season / time-of-day / interaction control plane for ALL vegetation
*Foliage/Atmosphere - effort M - auto-safe*
- **Goal:** Create one C# manager + shared Shader Graph subgraph broadcasting wind, season/withered, ToD tint, and the interaction bend-map to EVERY vegetation shader (existing cards, tree cards, new ground grass, future vines/flowers) and to cloud/weather VFX, so all foliage moves and tints coherently.
- **Approach:** WindZone is not auto-bound in URP: a WindManager reads the scene WindZone and calls Shader.SetGlobalVector('_WindDirection'), SetGlobalFloat('_WindMain'/'_WindTurbulence'/'_WindPulseFrequency'), SetGlobalColor('_SeasonTint'/'_TimeOfDayTint'), SetGlobalFloat('_Witheredness'), SetGlobalTexture('_BendMap', rt). Build a SharedWind subgraph consuming these globals (non-exposed properties whose Reference matches the global names) outputting object-space vertex offset + tint; drop into card/tree/grass shaders. Same vector also feeds cloud billboard drift and rain/snow VFX velocity so the whole scene shares one atmosphere. Reference Extra-Ordinary WindZone+SetGlobal pattern; happy-turtle/foliage-wind for the base+turbulence+pulse math. Build this BEFORE other foliage items.
- **Acceptance:** Rotating the scene WindZone direction visibly changes sway of grass, cards, trees AND cloud/weather drift together in one frame; setting _Witheredness=1 tints all vegetation toward brown simultaneously; no vegetation type is unaffected.

### 16. Overdraw budget + convert blend-mode foliage cards to alpha-clip; tighten card meshes
*Performance - effort L - NEEDS-TOM*
- **Goal:** Audit and cap transparent overdraw from sprite-card foliage (the #1 GPU cost of the look); convert blend-mode foliage to alpha-clip where soft edges aren't required, and trim card meshes to the visible silhouette so the painterly density stays at framerate.
- **Approach:** Use Rendering Debugger / RenderDoc overdraw view to quantify foliage overdraw. For dense bushes/canopy where a hard cutout is acceptable, set materials to Alpha Clipping (AlphaTest queue ~2450) instead of Transparent: alpha-clip WRITES depth so occluded fragments get rejected; transparent does not, so every layer shades. Reserve true alpha-blend only for soft accents (god-ray dust, thin grass tips). Replace full quads with tight alpha-trimmed sub-quad meshes (Sprite Editor 'tight' or custom card mesh). Keep card materials on ONE shader variant so SRP Batcher stays effective. This unlocks the Depth Priming win.
- **Acceptance:** Rendering Debugger overdraw shot of a foliage-heavy view shows worst-case stacked overdraw reduced vs baseline (fewer red high-overdraw zones) with no visible loss of canopy density in the beauty shot of the same angle.

### 17. Fix billboard self-shadowing + light-facing shadow caster so sprites read cleanly and don't go razor-thin
*Character Sprites - effort L - NEEDS-TOM*
- **Goal:** Each character reads scene light cleanly (lit sun side, soft shade side) WITHOUT self-shadowing dark, keeps a recognizable body-shaped cast shadow under a high sun, and never collapses to a sliver under the tilted camera.
- **Approach:** Two coupled changes in the Sprite Custom Lit graph/HLSL. (1) Lighting: override surface Normal to a single camera-facing/upright normal so half-Lambert never self-shadows; pull light via Cyanilux URP_ShaderGraphCustomLighting (GetMainLight/GetAdditionalLight, URP 17.1+/6000.1+), apply toon ramp + half-Lambert; disable shadow RECEIVE on the character (shadow layer) to kill residual self-shadow. (2) Cast shadow: add a custom ShadowCaster LightMode pass whose vertex stage billboards toward the LIGHT (right=normalize(cross(up,lightForward))) so the shadow keeps a body silhouette under overhead sun; set Cast Shadows=TwoSided to invoke it; ALPHATEST clip ~0.5 in the shadow pass. Constrain billboards to Y-axis-only yaw so they stay full-height at ~30deg tilt; adopt a depth-rewrite billboard (reference Jiaquarium 2.5D lit shader) so feet meet ground without clipping.
- **Acceptance:** At ~08:00 and ~12:00 sun: body shows a clear lit/shade side with no hard diagonal self-shadow band in either shot; ground shadow stays a recognizable humanoid blob in both (noon width >=~40% of silhouette, not a thin line); at min/max zoom on flat/slope/near-wall the sprite is full-height, feet meet ground with no clip-through, and sorts correctly vs set pieces.

### 18. God-ray dust motes drifting inside the volumetric shafts (VFX Graph, six-way lit, soft-particle)
*Atmosphere/VFX - effort M - NEEDS-TOM*
- **Goal:** Add slow sparse dust specks that live ONLY inside the existing light shafts so volumetric fog reads as illuminated living air, the defining HD-2D 'wow' beat, tying the 2D sprites into the 3D volume.
- **Approach:** VFX Graph confined to thin box volumes aligned with each shaft (hand-placed). Drift via Turbulence (Perlin Curl, very low Intensity ~0.02-0.05, large wavelength). Lifetime 8-20s, capacity ~150-400, size 0.5-3cm. Use VFX Graph six-way lighting so motes are bright on the sun-facing side, dark on shade side (the Unity feature for smoke/dust). Gate brightness by the shaft cone (sample a spot light cookie) so they vanish outside it; modulate spawn/brightness by the ToD curve. Soft-particle depth fade (Depth + Opaque Texture; with HDR on, Depth alone suffices) so they don't hard-clip geometry. HDR + Bloom (threshold ~1.0) makes lit motes gently glow. NOTE: this is the single dust-in-shaft item (dedup of two dimensions).
- **Acceptance:** Inside a lit shaft, discrete bright motes appear only within the cone with sun-side brighter than shade-side; motes are absent/very dim outside the shaft; pausing two frames shows <2px drift; zero motes render as hard squares clipping the floor.

### 19. Pixel-correct font pipeline: raster/bitmap TMP asset + integer scaling rules
*UI/HUD - effort S - NEEDS-TOM*
- **Goal:** Establish one body font and one display/title font as crisp pixel fonts that never blur at any supported resolution, with documented size/scaling rules, since text legibility is the RPG readability backbone.
- **Approach:** For pixel fonts generate the TMP Font Asset in Render Mode=RASTER_HINTED (bitmap, NOT SDF Smooth, which is the documented main cause of pixel-font blur) with Sampling Point Size = native px (e.g. 16), Padding ~4, atlas 512+; atlas texture Filter Mode=Point, no mips. On-screen size = integer multiple of native (16/32/48). Keep SDF only for any smoothly-scaling non-pixel UI. Keep dialogue/menu canvases Screen Space (Camera/Overlay) NEVER World Space, because Pixel Perfect Camera 'Upscale Render Texture' makes World-Space canvas text illegible.
- **Acceptance:** Side-by-side 1080p and 4K of the same dialogue line show sharp pixel edges (no grey AA halo) scaling by clean integer steps; 400% zoom of a screenshot shows hard-edged pixels, not gradient-softened glyph borders.

### 20. Ornate 9-slice dialogue box + nameplate in uGUI (not UI Toolkit)
*UI/HUD - effort M - NEEDS-TOM*
- **Goal:** Replace placeholder dialogue UI with a hand-authored ornate frame: 9-slice border, inset parchment/dark fill, separated speaker nameplate, drop-shadow, typewriter reveal with blinking continue caret, the strongest HD-2D genre signal.
- **Approach:** Use uGUI (Canvas) Image Type=Sliced with the Sprite Editor border set on the frame sprite (Unity 6 uGUI honors 9-slice borders; UI Toolkit does NOT support the 'Sliced' scale mode for Sprite backgrounds, so uGUI is the lower-friction path for pixel frames). Put the box on a Screen Space - Camera/Overlay canvas (crisp text), Canvas Pixel Perfect on. Typewriter via TMP maxVisibleCharacters; nameplate as a child Sliced Image + TMP. CC0 Kenney Fantasy UI Borders as a base (recolor/pixelate to match).
- **Acceptance:** An in-game conversation shows a non-rectangular ornamented border with intact corners (no stretched/blurred corner pixels) at 1080p AND 1440p, a speaker name in its own tab/area, and body text fully inside the inset with >=1 line of safe margin; corners look identical between the two resolutions.

## P1  (31 items)

### 21. Depth-faded gradient water color (shallow->deep two-tone ramp)
*Water - effort M - NEEDS-TOM*
- **Goal:** Replace flat water with a depth-driven two-color gradient (bright shallow teal at edges -> saturated deep blue/green toward center) so ponds/rivers read as volumetric, the highest-impact stylized-water readability cue.
- **Approach:** Shader Graph: Scene Depth (Eye) minus fragment eye-space depth gives water depth in meters; saturate(depth/DeepDistance) drives a Lerp between ShallowColor and DeepColor. Requires Depth Texture (and Opaque Texture if also refracting) enabled on the URP Asset. In Unity 6 Render Graph, Scene Depth/Color sample in the transparent pass without manual blit. Expose DeepDistance per water body (trough ~0.3m, pond ~3m) as a 2-stop Gradient. Build in-graph (free, AI-safe, matches existing ramp/toon) per Ameye/Ilett tutorials, or buy Stylized Water 3.
- **Acceptance:** A pond is visibly lighter/more saturated within ~30-60px of every shoreline/bank object, transitioning to a distinctly darker hue toward center; effect off makes the surface a single flat color.

### 22. Depth-intersection shoreline foam (edges, banks, rocks, character feet)
*Water - effort M - NEEDS-TOM*
- **Goal:** Add a soft animated foam band where water intersects geometry (shores, rocks, posts, standing characters), with artist-controlled width and a noise/voronoi-broken edge, the second-biggest HD-2D water tell.
- **Approach:** Shader Graph: reuse the water-depth value; foamMask = 1 - saturate(depth/FoamDistance), break the band edge with a scrolling Voronoi/Gradient Noise + Step for the toon cutoff (Roystan/Ilett technique), add a second inner foam line, scroll by Time. Expose FoamDistance, FoamColor, FoamCutoff, NoiseScale. Works against the same Depth Texture.
- **Acceptance:** A continuous foam ring hugs every land/water boundary and around a rock in the pond; the edge is ragged/noise-broken (not a uniform ring) and shifts position between frames a few frames apart.

### 23. Rim / back light on billboard sprites driven by ToD (separate from sun)
*Lighting/Character - effort M - NEEDS-TOM*
- **Goal:** Add a dedicated rim/back-light to billboard characters so they separate from busy painterly backgrounds with a bright warm/cool edge regardless of sun position.
- **Approach:** In the sprite lit Shader Graph add a Fresnel/rim term plus a back-light term using a dedicated rim direction (side/back), not the sun, so it shows when the sun is behind/overhead; since a billboard's geometric normal is degenerate derive rim from the baked normal map's grazing regions or a screen-space alpha-edge falloff. Drive rim color per ToD (warm dusk, cool night) from the global sun-color/ToD shader property. Add it additively, gated by alpha, as a per-material toggle. Reference Cyanilux custom-lighting subgraphs + Minions Art rim math (pow(1-dot(N,V),k)).
- **Acceptance:** A character in front of a same-value background shows a continuous bright edge along the silhouette that is clearly absent when toggled off (A/B); rim color matches the scene sun tint, not pure white.

### 24. Stylized specular accents on wet/metal/water surfaces in the ramp-lit shader
*Lighting/Shader - effort M - NEEDS-TOM*
- **Goal:** Add controllable stylized specular hits (water glints, metal/roof highlights, wet-cobble sheen) to the ground/building ramp shader so the diorama gets jewel-like sparkle and material differentiation under the moving sun.
- **Approach:** Extend the ramp-lit Shader Graph with a stepped/quantized Blinn-Phong term (pow(saturate(dot(N,H)),gloss)) gated by a per-material smoothness/specular mask, layered additively over the ramp diffuse so it doesn't break toon banding. Drive specular tint from the sun color (warm dawn glints). For water, animate a scrolling normal/noise into the spec term.
- **Acceptance:** A low dawn/dusk sun shows distinct bright specular glints on water and metal/roof props tracking the sun direction; matte dirt/grass shows no glints, proving the spec mask differentiates materials.

### 25. Contact-hardening soft shadows for the diorama (tune URP first, then evaluate asset)
*Lighting - effort M - NEEDS-TOM*
- **Goal:** Upgrade the main directional shadow so it is crisp where casters touch the ground and softens with distance, and ensure sprite/prop contact shadows glue flat characters to the set.
- **Approach:** First exhaust built-in URP: soft shadows on, raise cascade count/resolution, tune bias to remove the common URP acne/peter-panning, ensure sprite shaders are two-sided so quads cast/receive correctly. For true contact-hardening (sharp at contact, soft far) that URP lacks, evaluate Kronnect Umbra (verify Unity 6 Render Graph compatibility before buying). Keep the billboard contact-shadow blob as a guaranteed grounding fallback, tuned to follow sun direction/length per ToD.
- **Acceptance:** A building/tree shadow is sharp at the base where it meets the ground and progressively softer with distance; every character sprite has a contact shadow at its feet that lengthens/rotates correctly as the ToD sun moves.

### 26. Bloom recipe: threshold-gated soft bloom for highlights, not global haze (shared Volume)
*Post - effort S - NEEDS-TOM*
- **Goal:** Tune Bloom so only genuinely bright elements (sun glints, lamps, magic FX, portal, water speculars, fireflies/embers) bloom softly while ramp-lit ground and pixel sprites stay crisp.
- **Approach:** In the Bloom override raise Threshold above lit mid-tones (start ~1.0-1.2 gamma; default 0.9 catches lit ground), Intensity ~0.15-0.4, Scatter ~0.6-0.75, faint warm Tint. Lower Clamp (default 65472) to ~8-16 to kill fireflies/sparkles on emissive sprite/foliage alpha edges. High Quality Filtering (bicubic); in 6.3 pick Filter=Gaussian for quality. Optional subtle Lens Dirt for portal/sun. Lives in the SAME shared Volume stack as the master grade (no double-grading); HDR must be on for emissive items to glow.
- **Acceptance:** Bright lamp/portal pixels show a soft glow; a mid-bright lit dirt path or character torso does NOT glow; pixel-sprite silhouettes stay crisp; no flickering bright specks on animated foliage card edges between consecutive frames.

### 27. Per-zone mood grading: Shadows-Midtones-Highlights, Split Toning, Color Curves on local Volumes
*Post - effort M - NEEDS-TOM*
- **Goal:** Layer finer location-specific tonal grading on top of the master look (cool blue shadows + warm highlights, faded painterly black point) scoped to per-area Local Volumes so caves/forests/towns each get a distinct treatment.
- **Approach:** Add SMH (shadows->cool teal, highlights->warm, tune range limits so midtone sprites stay neutral), Split Toning as a quicker warm/cool split, and Color Curves (Master, RGB, Hue-vs-Sat) to desaturate over-bright foliage greens or lift the toe. All separate URP Volume overrides stacking on the master grade; scope to Local Volumes with collider triggers. Author the master look first; these are per-zone deltas.
- **Acceptance:** Eyedropper on a neutral-gray surface reads cooler/bluer in shadow than in highlight, deepest shadows are a tinted dark (not 0,0,0), and switching between two zone Volumes yields visibly different shadow/highlight tints.

### 28. Height-aware transition masks for path/grass edges (no muddy 50% cross-fade)
*Ground - effort M - NEEDS-TOM*
- **Goal:** Upgrade the splat blend from a linear lerp to a height-aware blend so transitions follow the textures' own height maps, producing crisp interlocking edges (grass blades poking into dirt) instead of a gray smear, the biggest quality jump for stylized ground edges.
- **Approach:** In the ground Shader Graph sample each layer's height/grayscale; replace lerp t with heightlerp: blend=saturate((vertexWeight+layerHeight-1)/transitionContrast) or compare h1+w1 vs h2+w2 with a contrast/sharpness slider. Expose Transition Sharpness. Pack height into albedo alpha or a mask texture to save samplers. Reference Jason Tuttle / jensnt TerrainHeightBlend.
- **Acceptance:** A close-up grass-to-dirt boundary shows an irregular interlocking edge driven by texture detail (not a straight or uniformly soft gradient), with an adjustable sharpness that visibly changes edge crispness between two screenshots.

### 29. Triplanar projection for cliff faces, slopes, and steep diorama edges
*Ground/Shader - effort M - auto-safe*
- **Goal:** Add a triplanar sampling path so rock/dirt project correctly on vertical/steep diorama faces without UV stretching, blending to top-down ground UVs on flats.
- **Approach:** Use Shader Graph's Triplanar node (world-space) for the rock/cliff layer; blend triplanar vs top-down-UV by world-normal.y (smoothstep ~0.6-0.8) so flats use cheap planar UVs and steep faces use triplanar. Keep tiling in world units so adjacent pieces match. Limit triplanar to the rock layer (it triples samples) for Switch-class targets. Reference cathyhlshih UnityURPVertexBlendTriplanarShader.
- **Acceptance:** A vertical or ~70deg cliff face shows non-stretched rock texels (consistent texel density with the flat ground beside it) with a clean blend from cliff rock to grass at the top lip.

### 30. Macro / anti-tiling variation pass to kill the obvious ground repeat
*Ground - effort M - NEEDS-TOM*
- **Goal:** Break visible tiling repetition on large ground areas with low-frequency macro color/value variation and/or stochastic tile rotation, so the tilted camera framing a lot of ground doesn't show an asset-flip grid.
- **Approach:** Cheap: sample large-scale (~0.05 tiling) noise, multiply into albedo value + slight hue shift. Stronger: Inigo Quilez / Ben Cloward (Shader Graph Basics Ep.47) stochastic 'texture tiling without repetition' (per-tile random rotation/offset blended over a hex grid). Optionally a distance-based second larger-tiled macro map. Implement as a Sub Graph before the splat blend so every layer benefits.
- **Acceptance:** Before/after of the same large grass area: 'before' shows a clearly repeating tile grid, 'after' shows no obvious repeating motif across the full framed ground at gameplay camera distance.

### 31. Art-directed foliage clustering & scatter rules (clumps, worn paths, slope/mask suppression)
*Foliage - effort M - NEEDS-TOM*
- **Goal:** Replace uniform/random scatter with authored density: clumps near rocks/tree bases/water edges, bare worn paths, falloff by slope and a hand-paintable mask, so vegetation looks composed not sprinkled.
- **Approach:** Drive the instanced scatter generator from layered masks: a hand-painted RGBA density/biome splat sampled in object space, plus rules (suppress on slope>N via ground normal, suppress within radius of tagged props to carve paths, boost near water/rock tags, ground vertex-color as extra mask). Use Poisson/blue-noise cluster jitter, not pure uniform random. Keep deterministic (seeded) and deploy through the full Apply/Integrator path, not Refresh-only. Mix 2-3 variants per cell for silhouette variety.
- **Acceptance:** A top-down capture shows authored composition: bare path/clearing negative space, denser grass hugging tree bases/rock edges, zero grass on steep slopes/cliffs; re-running with the same seed produces an identical layout.

### 32. Interactive grass bend / trample via a top-down bend-map RenderTexture
*Foliage - effort M - auto-safe*
- **Goal:** Make grass and low foliage bend away from and flatten under the player/NPCs and the time-window portal, springing back over time, so the world reacts to presence and the billboard sprite is anchored to the ground.
- **Approach:** Each frame render trampler positions (player, NPCs, portal) as soft circular splats into a top-down ortho RenderTexture tracking the focus area, with per-frame decay for spring-back (additive write + multiply-down). Publish via Shader.SetGlobalTexture('_BendMap',rt) + a world-to-UV matrix global. The SharedWind subgraph samples _BendMap in world-XZ to offset blade tops away from trample centers and reduce height under contact. Works identically for instanced grass and existing cards. Reference gamedev.center interactive grass + mozankatip/InteractiveGrass; or buy GrassFlow.
- **Acceptance:** Walking through grass shows a parted/flattened trail following the character that recovers within ~1-2s; the portal pushes nearby grass outward; standing still leaves a persistent flattened disc.

### 33. Tree canopy improvement: multi-layer card clusters with backlit translucency + interior AO
*Foliage - effort L - NEEDS-TOM*
- **Goal:** Upgrade single tree-cards to layered canopy clusters (multiple offset cross-quads) with subsurface translucency where the sun backlights leaves and soft self-shadow/AO in the canopy interior, plus per-card wind phase, since trees are the largest silhouettes the tilt-shift lands on.
- **Approach:** Compose each tree from 3-6 alpha-clip canopy cards at varied offsets/rotations (or a low-poly canopy frustum skinned with leaf cards) so the silhouette has depth. In the leaf Shader Graph add a translucency/back-light term (boost transmitted color where backlit, NedMakesGames foliage translucency), baked canopy AO via vertex color/AO mask darkening the interior, and per-card wind phase offset from the SharedWind subgraph so cards don't sway in unison. Pixel-snap + ramp-light to match existing cards; add a soft ground contact AO disc under each tree.
- **Acceptance:** With the sun behind a tree, warm light transmits through leaf edges (rim glow), the canopy interior is visibly darker than lit outer leaves, panning reveals canopy depth (layers parallax) rather than a flat plane, and cards sway with slightly different phases.

### 34. Per-sprite normal map (and optional mask) for directional ToD shading on flat cards
*Character Sprites - effort M - NEEDS-TOM*
- **Goal:** Make characters pick up light DIRECTION (highlight on the sun-facing shoulder, falloff on the far side) and shift believably as the sun arcs, instead of a uniform tint.
- **Approach:** In the Sprite Custom Lit graph add a Sample Texture 2D (Type=Normal) feeding Normal(Tangent Space); expose NormalMap (+ optional MaskTex). Because the quad billboards, rotate the sampled tangent-space normal into the billboard basis so the lit side tracks the world light vector; clamp the dark side to ambient so it never goes pure black. Bake the normal per direction in the same render as the diffuse (see 3D-to-sprite item) so they stay registered. Feed main light direction via GetMainLight; combine with the half-Lambert toon ramp.
- **Acceptance:** Rotating the directional light 180deg around a stationary character swaps the bright/dark sides of the sprite (highlight moves shoulder to shoulder), confirming the normal map drives directional shading, not a flat tint.

### 35. 3D-model-to-sprite bake pipeline: registered 8/4-direction sheets (diffuse + normal)
*Character Sprites/Workflow - effort L - NEEDS-TOM*
- **Goal:** Generate authentic HD-2D sprites by rendering rigged 3D models to flipbook sheets covering 8 (or 4) facings with matching normal/depth bakes and idle/walk/run sets, at the game's camera tilt, the way Octopath got lighting-consistent sprites.
- **Approach:** In-engine bake rig: an orthographic camera at the game's pitch, model on a turntable rotated to N facings (8=45deg, 4=90deg), render each anim frame to a RenderTexture, pack a grid sheet. Use the Shader Graph Flipbook node (drive Tile by anim time); index the facing row at runtime from signedAngle(cameraForward, characterForward). Bake diffuse + normal (+ optional depth/AO) in the same camera so all maps stay pixel-registered. Evaluate buy-vs-build: RetroRender / QuickSprites 3D automate the directional capture. If source models are Synty, do NOT route them through any AI step (EULA).
- **Acceptance:** Walking a character in a circle (or orbiting the camera) shows the sprite swapping through all authored directions with no missing/mirrored-wrong facing, and the implied light on the sprite matches the scene sun direction in each facing.

### 36. World-space contextual interaction prompts (icon + button glyph, depth-aware)
*UI/HUD - effort M - NEEDS-TOM*
- **Goal:** Add floating examine/talk/enter prompts above interactables when the player is in range, anchored in world space but rendered crisply, with controller/keyboard glyph swap, a core HD-2D navigability affordance.
- **Approach:** Render prompts on a Screen Space - Camera canvas whose RectTransform is driven by Camera.WorldToScreenPoint of a world anchor (keeps TMP crisp, avoids World-Space-canvas blur under Pixel Perfect Camera). Use a TMP SpriteAsset for the button glyph sheet so keyboard/gamepad icons swap by inline sprite tag. Fade/scale in via a short tween when the trigger reports in-range. Keep the glyph sharp even if the underlying prop is tilt-shift-blurred. Use CC0 Kenney Input Prompts (Pixel).
- **Acceptance:** With the player next to an interactable a crisp prompt (icon + readable button glyph) hovers over that object and NOT over non-interactables; moving away removes it; glyph art is sharp (no AA blur) at 1080p.

### 37. Per-area zoom/framing via priority-blended CinemachineCameras + trigger volumes
*Camera - effort M - NEEDS-TOM*
- **Goal:** Define multiple cams (wide town, tight interior, mid-combat) that auto-blend when the player enters trigger volumes, each with its own FOV/distance but the SAME tilt so the world feels continuous and the camera never clips tight geometry.
- **Approach:** Cinemachine 3.1: one Brain picks the highest-Priority live cam; author one cam per area sharing the rig profile but overriding FOV/Follow distance. Brain Default Blend EaseInOut ~0.6-1.0s, or a CinemachineBlenderSettings asset for per-pair blends (Cut for instant rooms). BoxCollider triggers + a tiny script raise a cam's Priority on enter. The Brain interpolates lens FOV across blends.
- **Acceptance:** Walking from open field through a door into an interior smoothly zooms in (FOV/distance decreases) over <1s with no hard pop, identical tilt, no near-plane clipping; Brain Inspector shows the interior cam Live with the field cam at lower Priority.

### 38. Keep the camera inside the diorama: 3D bounds clamp so the void is never visible
*Camera - effort M - auto-safe*
- **Goal:** Constrain the camera so the visible frame never extends past a hand-built diorama edge (no skybox void, no off-mesh black) at any zoom, a recurring HD-2D defect on this project.
- **Approach:** IMPORTANT: Cinemachine Confiner 2D requires camera-forward parallel to the bound normal, which a tilted rig violates, so it mis-clamps. Use CinemachineConfiner3D with a Collider sized to the diorama interior, OR a custom LateUpdate clamp limiting the rig's XZ target to an authored Rect/AnimationCurve per area (simpler and exact for a fixed tilt). Compute the bound from area mesh extents minus a margin equal to half the camera's ground-plane view width (from FOV/distance/tilt).
- **Acceptance:** Pushing the player into every corner/edge at widest and tightest zoom shows no frame with skybox/void/black off-mesh visible past the intended set edge.

### 39. Scripted cutscene cameras: side-view auto-pan + dolly on Cinemachine Spline + Timeline
*Camera/Cinematics - effort L - NEEDS-TOM*
- **Goal:** Build a reusable cutscene system (including the requested side-view auto-pan ending) using Spline Dolly cameras driven by Timeline that temporarily override gameplay framing then return cleanly.
- **Approach:** Cinemachine 3.1 Dolly Camera with Spline. For the side-view pan, lay a straight horizontal SplineContainer parallel to the set, animate CinemachineSplineDolly Camera Position 0->1 via a Timeline track for a constant lateral pan; lock a side-on yaw and drop tilt toward 0 for a true 2D side view. Activate by raising the cutscene cam Priority from a Timeline Signal; blend back via Brain default blend. Known CM3 quirk: set Automatic Dolly OFF and explicitly key Spline Position or Timeline rotation gets overridden. A CutsceneDirector disables player input + HUD during the sequence.
- **Acceptance:** Triggering the ending cutscene: the camera smoothly pans horizontally across the set in a side-on (near-zero tilt) view at constant speed for the authored duration with no stutter, then blends back to the tilted gameplay cam and restores input/HUD; start/mid/end shots show only lateral translation at consistent height.

### 40. Layered billboard cloud bands + glowing sun/moon on a Shader-Graph gradient skybox
*Sky/Weather - effort M - NEEDS-TOM*
- **Goal:** Rebuild the skybox as a procedural Unlit Shader Graph gradient dome that reads the sun direction live (day/night color ramps), add a glowing sun disc + phased moon locked to the directional light, and soft alpha-card cloud bands tinted by ToD drifting on the shared wind.
- **Approach:** Unlit Shader Graph skybox assigned via Environment; sample world view direction Y to Lerp horizon<->zenith and day<->night gradient pairs driven by sun height (Main Light node). Set Environment Source=Skybox + Realtime ambient so RenderSettings.ambientLight/fogColor regenerate from the sky each frame. Sun disc = smoothstep on dot(viewDir, lightDir) with an emissive halo lobe; moon on -dir with a phase mask; cross-fade by sun height. Clouds = large camera-facing billboard quads at distance using cloud PNGs, multiplied by the current horizon color (Shader.SetGlobalColor), alpha fading to zero at the card bottom (vertical UV gradient) so they melt into the gradient, drifting on _GlobalWind. Prefer billboard/gradient over true volumetric clouds for the pixel-art bar and frame budget.
- **Acceptance:** Shots at sun elevations +60/0/-10deg show a continuous banded gradient with no cubemap seams, warm sunset horizon and deep-blue night, ambient/fog matching the horizon band; a single soft sun disc + halo at noon, a crescent moon at night on the opposite side; clouds blend invisibly at their lower edge, tint orange at sunset, and drift laterally without popping.

### 41. Distant parallax backdrop ring (painted mountains / haze layers) behind the diorama
*Sky/Weather - effort M - NEEDS-TOM*
- **Goal:** Build 2-3 concentric rings of painted backdrop cards (far mountains, mid hills, atmospheric haze band) behind the playable diorama so the hand-built stage sits in a believable far landscape and the tilt-shift gets a far plane to blur.
- **Approach:** Painterly mountain/hill silhouette PNGs with alpha on large vertical quads at increasing distance, parented to the camera rig on XZ only (or a follow script copying camera XZ not rotation) so they stay put but reveal subtle parallax. Lerp each layer's tint toward the shared horizon color with distance and push far layers into URP fog. Render before transparents, just inside the far plane so DoF treats them as distant; put them on a dedicated render/sort layer so fog + tilt-shift affect them consistently.
- **Acceptance:** A wide shot shows >=2 distant terrain silhouette layers between the diorama edge and the sky, each progressively hazier/bluer; moving the camera laterally shows the near backdrop layer shifting more than the far (parallax); no hard seam where playable mesh meets backdrop.

### 42. Adopt a modular building kit + grid-snapping workflow for fast, varied, cohesive structures
*Props/Buildings - effort L - NEEDS-TOM*
- **Goal:** Move from monolithic one-off buildings to a grid-snapping modular kit (walls, corners, roofs, doors, windows, floors, stairs) for combinatorial variety with guaranteed cohesion, including interior-included walls that support the lit-window item.
- **Approach:** Use a CC0 modular kit whose walls include interiors (Quaternius Medieval Village MegaKit). Set up grid snapping; build buildings as nested Prefabs so a shared-material change updates all. Apply the ramp shader to the kit's shared materials, enable GPU instancing, use Prefab Variants for facade variety (signage/window/roof combos). Mark assembled buildings Static for batching + baked AO/GI.
- **Acceptance:** A built street shows 5+ visibly distinct building configurations (varied footprints/heights/roof+door+window combos) sharing one consistent material/lighting style; no two adjacent buildings are identical copies.

### 43. Roof-tile and wall surface detailing pass (stylized normal/parallax + edge accents)
*Props/Buildings - effort M - NEEDS-TOM*
- **Goal:** Replace flat untextured roof/wall planes with tiled stylized roof-tile rows, plank/timber-frame walls, and stucco so building surfaces have readable rhythmic detail and silhouette-supporting edge accents at the diorama distance.
- **Approach:** Use stylized tiling roof-tile/wall textures with a normal map through the toon shader so the sun creates per-tile shadow banding. Model timber-frame beams as low-poly geometry (silhouette matters at the tilt), not texture-only. Add dark edge/cavity accents via baked AO in the texture or a Fresnel-darkened ramp band. Keep tiling scale consistent across all buildings for unified texel density. CC0 FreeStylized roof/brick/wood.
- **Acceptance:** A zoomed building shows individual roof-tile rows casting a consistent directional micro-shadow under the current sun, and walls show readable material banding (stone/plaster/timber) rather than a single flat color fill.

### 44. Signage, banners, lanterns and hanging fixtures for a lived-in town
*Props/Buildings - effort M - NEEDS-TOM*
- **Goal:** Add the human-use set dressing (shop signs, hanging tavern brackets, cloth banners/flags, awnings, paper lanterns, rope lines, goods crates) that signals an inhabited place, doubles as warm light, and reinforces verticality the tilted camera rewards.
- **Approach:** Mix 3D kit props (signs, lanterns, awnings) with the existing alpha-clip sprite-card pipeline for cloth banners/flags so they get the same wind sway + toon ramp as foliage. Lanterns emissive (HDR) so they bloom, optionally with a cheap warm point light + animated emission flicker. Batch banners as instanced sprite cards. Place dressing along sightlines the tilt-shift focal band keeps sharp. CC0 Quaternius/Kenney.
- **Acceptance:** A town-street shot shows >=8 distinct dressing types (signs, banners, lanterns, awnings, crates, barrels, ropes, etc.) in frame; at dusk lanterns visibly emit warm bloom.

### 45. Scene-wide ambient dust/pollen layer (whole-scene atmosphere, CPU Shuriken)
*VFX - effort S - NEEDS-TOM*
- **Goal:** Add a scene-wide faint drifting dust/pollen layer independent of light shafts so even shaded regions have constant subtle motion and parallax depth.
- **Approach:** One CPU Shuriken system parented to the camera, World simulation space, large Box emission matching the play area, ~80-200 particles, very low alpha (~0.05-0.12) unlit. Gentle motion via the Noise module (low Strength/Frequency, Damping ~0.5) + small world-up/wind Velocity over Lifetime. Keep CPU/Shuriken (tiny count) and reserve VFX Graph for the higher-count god-ray dust. Camera-attached so density stays constant. Tint warm/cool to match ToD.
- **Acceptance:** A shadowed/non-shaft area still shows faint moving specks; a 1s capture shows continuous drift with no spawn 'pops' at frame edges; the dust never reads as snow/noise (alpha <~0.15).

### 46. Fireflies / glowing pollen for dusk and night (emissive bloom particles, ToD-gated)
*VFX - effort M - NEEDS-TOM*
- **Goal:** Add wandering glowing points that fade in at dusk, peak at night and blink softly to make evening dioramas magical and guide the eye, a signature Sea of Stars night cue.
- **Approach:** Small particle system (Shuriken or VFX Graph) with additive material and HDR color (Intensity >1) so the existing Bloom blooms them. Pulse brightness via Color-over-Lifetime for the blink; drift via low Curl noise within a bounded volume. Gate emission rate off the ToD/night value so they fade in at sunset, out at dawn. Counts 20-60. Optionally attach a tiny URP point light to 1-3 hero fireflies (respect per-object light limits). DIY from CC0 Kenney glow sprites to stay AI-safe.
- **Acceptance:** A night shot shows distinct glowing dots with a soft bloom halo (not flat dots); the same spot in daytime shows zero/near-zero; two frames show individual dots changing brightness (blink) and position (drift).

### 47. Falling leaves / petals seasonal drift overlay (biome-swappable)
*VFX - effort M - NEEDS-TOM*
- **Goal:** Add sparse leaves/petals that spawn above the frustum and tumble/sway down across the screen, swappable per biome (autumn leaves, sakura, ash, snow), a top-tier HD-2D mood setter that reads as soft bokeh through the tilt-shift.
- **Approach:** Camera-parented emitter spawning in a box just above/in front of the view; textured quad particles with Rotation over Lifetime + by Speed for tumble, Noise + gravity Velocity for the sway-and-fall arc (or VFX Graph Turbulence Curl). Render with the existing alpha-clip/unlit particle shader so leaves match the toon look; place some between camera and subject so the tilt-shift blurs them into foreground bokeh. Drive density/sprite selection from a per-zone config. Use VFX Library pack or CC0.
- **Acceptance:** A shot shows 5-15 leaves at varied rotations falling across frame; foreground leaves render visibly blurred by tilt-shift while mid-depth ones are sharp; switching the zone config swaps the sprite (green leaf -> pink petal) without code changes.

### 48. Unify ambient VFX, clouds, and weather under one wind + ToD director
*VFX/Atmosphere - effort M - auto-safe*
- **Goal:** One lightweight manager feeds a shared wind vector/strength (with gusts) and the ToD value to all ambient particle systems, clouds, and weather so leaves/smoke/dust/fireflies/embers/rain respond coherently and density auto-tunes by zone.
- **Approach:** An AmbientVFXDirector exposing a world wind Vector3 (slow-noise gusts) + a normalized day/night value from the existing sun cycle, published via Shader.SetGlobalVector/Float and VisualEffect.SetVector3 on exposed VFX Graph properties. Leaves/smoke/dust add wind to Velocity over Lifetime; fireflies/embers scale emission off day/night; a per-zone config sets max particle budgets and biome sprite set. Throttle total on-screen counts (LOD: disable far/offscreen emitters) to protect frame budget and keep the frame readable. Reuses the same wind global as the foliage control plane and skybox clouds.
- **Acceptance:** Changing global wind redirects leaves AND smoke AND dust in the same direction within one shot; advancing ToD to night raises firefly/ember density and lowers daytime dust; a zone swap changes particle density/sprite set with no per-system manual edits.

### 49. Enable URP Depth Priming to depth-reject occluded foliage/ground fragments
*Performance - effort M - auto-safe*
- **Goal:** Turn on Depth Priming so opaque + alpha-clipped geometry populates depth first and the color pass skips shading hidden fragments, recovering GPU budget for the tilt-shift/volumetrics/ramp lighting.
- **Approach:** In the Universal Renderer set Depth Priming Mode=Auto (or Forced for testing). Depends on the alpha-clip foliage conversion (blend transparents can't participate). Verify foliage/tree-card shaders include a correct depth-only/ShadowCaster pass applying the SAME alpha clip (share via include/macro so the prepass silhouette matches the color pass). Confirm the depth texture is still available for the portal/tilt-shift that sample _CameraDepthTexture; watch CopyDepth interactions.
- **Acceptance:** Frame Debugger/GPU profiler before vs after shows a measurable drop in forward opaque fragment cost on a heavy diorama view, AND a beauty shot of the same frame is visually identical (no z-fighting, no missing foliage edges).

### 50. SRP Batcher compatibility + GPU Resident Drawer for diorama/foliage meshes
*Performance - effort M - auto-safe*
- **Goal:** Make all diorama/foliage-card materials SRP-Batcher-compatible, eliminate batch-breakers, and enable the GPU Resident Drawer for environment mesh renderers so CPU draw-call submission stops being the bottleneck.
- **Approach:** Confirm Forward+ path, SRP Batcher on, BatchRendererGroup Variants='Keep All', then GPU Resident Drawer=Instanced Drawing. KEY LIMITS: GPU Resident Drawer only accelerates Mesh Renderers on compute-capable APIs (not GLES); it does NOT help SpriteRenderers, billboard character sprites, or SkinnedMeshRenderers, so target environment meshes (and tree-card meshes if MeshRenderers). Audit batch-breakers: any per-instance tint/wind via MaterialPropertyBlock breaks BOTH SRP Batcher AND instancing, so move per-instance data into the instanced cbuffer (UNITY_INSTANCING) or vertex colors. Keep environment shaders to a minimal variant set. Accept/document longer build times.
- **Acceptance:** Frame Debugger shows long SRP/instanced batches (few small breakers); settings show Forward+, SRP Batcher on, BRG Variants Keep All, GPU Resident Drawer=Instanced; profiler shows reduced main-thread RenderLoop/draw-submission time vs baseline.

### 51. Render Graph correctness + half-res volumetrics: pass ordering and compositing audit
*Performance - effort M - auto-safe*
- **Goal:** Make the stacked atmosphere effects (fog, shafts, dust, DoF) cheap and correctly ordered in the Unity 6 Render Graph so they composite right with sprite cards and contact shadows and stay within a solo-dev frame budget.
- **Approach:** Author custom atmosphere passes as Render Graph Renderer Features ordered: opaque -> volumetric light (half-res + bilateral depth-aware upscale) -> fog fullscreen -> transparents (sprite cards) -> DoF/tilt-shift -> UI. Ensure depth texture + sprite-card depth writes are correct so fog/shafts respect alpha-clipped foliage. Render volumetrics at 1/2 or 1/4 res. Validate no fog bleed onto UI and no dark/bright halos around billboards/contact shadows. Profile with the Render Graph Viewer / Frame Debugger.
- **Acceptance:** Render Graph Viewer shows passes in the intended order with volumetric buffers at reduced resolution; A/B shows no fog tint on UI, no halo ring around billboard sprites or their contact shadows, and shafts correctly cut by alpha-clipped tree cards; GPU frame-time delta from enabling all atmosphere effects stays within the documented budget.

## P2  (26 items)

### 52. Soft blob contact shadow under each character (independent of the directional cast shadow)
*Character Sprites - effort S - auto-safe*
- **Goal:** Add a small soft always-present darkening under the character's feet that anchors it to the ground separately from the directional sun shadow (which moves/stretches with ToD).
- **Approach:** URP Decal Projector (Decal Renderer Feature enabled) projecting a soft radial blob straight down onto the ground at the character pivot, scaled to foot size, faded by an AO value; cheap, reads correctly on hand-built mesh dioramas (no Terrain), independent of sun direction so it persists at all times. Alternatively a small downward soft-shadow quad with a multiply material. Lower opacity when airborne for grounding feedback.
- **Acceptance:** At a low-sun time of day where the directional shadow rakes far away, a soft dark contact patch is still visible directly beneath the feet (~foot-width, soft-edged); toggling it off makes the character visibly float.

### 53. Light cookies / gobos for dappled canopy shade and shaped window light
*Lighting - effort S - NEEDS-TOM*
- **Goal:** Add light cookies on the sun and key local lights to break up flat lit areas with canopy dapple, window/grate patterns, and shaped pools, reinforcing the diorama-under-a-spotlight reading without extra geometry.
- **Approach:** Assign a leaf/branch dapple cookie to the main directional light for canopy shade across the diorama floor; Spotlight cookies on interior/lantern lights for window-grate and shaped pools. Scroll the directional cookie subtly for wind-moving dapple. Note the known URP issue where a spot cookie can reduce directional intensity, so validate intensities after assigning. CC0 Kenney light cookies.
- **Acceptance:** Under tree canopy the ground shows broken leaf-shaped light/shadow patches (not a flat uniform lit area); an interior/window scene shows a clearly shaped window-pattern light pool on the floor.

### 54. Panini Projection to tame wide-FOV edge distortion
*Post - effort S - NEEDS-TOM*
- **Goal:** Add a Panini Projection override so the wide-FOV diorama camera keeps vertical lines (buildings, trees, sprite cards) upright at screen edges instead of leaning/stretching.
- **Approach:** Add the Panini Projection Volume override, Distance ~0.4-0.8 (push only as far as edge buildings stop leaning), Crop to Fit ~0.5-1.0 to avoid black edges. Pair specifically with the narrow-FOV perspective rig.
- **Acceptance:** Side-by-side Panini off vs on: a vertical building/tree card near the screen edge is leaning/stretched with it off and stands vertical with it on, with no black border introduced.

### 55. World-space gradient tint + painted AO darkening on the ground to pop the tilt-shift
*Ground - effort S - NEEDS-TOM*
- **Goal:** Add a subtle world-space gradient tint and crevice/contact darkening to the ground material so the diorama has painterly value falloff (lighter centers, darker recesses) rather than uniform flat albedo, reading strongly under tilt-shift/fog.
- **Approach:** In the ground Shader Graph add a world-space Position-driven gradient (remap world Y or distance-from-center) and multiply a cool-shadow/warm-light tint into the ramp-lit output before lighting so it composites with the toon ramp. Add a vertex-color channel (painted via Polybrush) to darken crevices, wall bases, under-foliage. Keep tint subtle (value ~0.85-1.0). 80.lv stylized diorama master-material technique.
- **Acceptance:** The ground shows visible value variation (recesses/wall bases darker, open areas lighter) that reads through the tilt-shift blur, toggleable to confirm it is the cause.

### 56. Aerial-perspective tint baked into the ramp shader for far ground/buildings
*Shader/Atmosphere - effort M - NEEDS-TOM*
- **Goal:** Add an optional distance-tint term inside the ramp-lit ground/building shader so far surfaces shift toward the horizon color even before fullscreen fog, preserving the toon ramp banding so distant buildings stay painterly rather than gray mush.
- **Approach:** In the ramp-lit shader compute camera distance, then lerp the post-ramp lit color toward an exposed _AerialTint by saturate((dist-start)/range). Do this AFTER the ramp lookup so banding survives; keep strength low (let fullscreen fog do the heavy lifting beyond a threshold). Drive _AerialTint/start/range from the same ToD/fog manager so it stays in sync with the Volume fog. Apply to ground+building variants only, not sprites.
- **Acceptance:** A row of identical receding buildings shows the toon ramp bands still visible on far buildings (not flat gray) yet each tinted further toward the horizon color; disabling _AerialTint snaps far buildings back to full-saturation ramp.

### 57. Time-of-day atmosphere preset blending (dawn/noon/dusk/night fog + scatter)
*Atmosphere - effort M - NEEDS-TOM*
- **Goal:** Drive fog color/density, shaft intensity, aerial tint and dust spawn from the existing sun cycle via blended presets so atmosphere changes believably across the day instead of being static.
- **Approach:** Author 3-4 atmosphere presets (fog gradient/density/height, shaft intensity, dust rate, aerial tint) as ScriptableObjects keyed to sun elevation; a manager samples the ToD curve each frame and lerps the active URP Volume fog override + global shader props + VFX spawn between presets. Use Volume weight blending where possible. Key transitions on sun angle, not wall-clock, to match the directional-light rotation.
- **Acceptance:** Four shots at dawn/noon/dusk/night show distinctly different fog hue/density and shaft strength (warm low-sun shafts at dawn/dusk, minimal+cooler at noon, dense cool fog at night); intermediate sun angles interpolate smoothly with no popping.

### 58. Seasonal / ToD vegetation tinting (lush <-> withered, day/night) via the control plane
*Foliage - effort M - NEEDS-TOM*
- **Goal:** Drive a global lush-to-withered and day-to-night color shift across all vegetation from the existing ToD/season state (desaturation, autumn hue rotation, cooler night tint) to make the diorama feel like a living place across a day cycle and differentiate biomes/chapters with the same meshes.
- **Approach:** Entirely through the control plane: Shader.SetGlobalColor('_SeasonTint'/'_TimeOfDayTint') + SetGlobalFloat('_Witheredness') updated by the ToD cycle. In the SharedWind subgraph lerp base albedo toward _SeasonTint by _Witheredness, add per-instance hue/value variation (instance ID or world-pos noise) so withering isn't uniform, multiply by _TimeOfDayTint synced to the sun color; optionally raise alpha-clip threshold with _Witheredness so withered grass looks sparser.
- **Acceptance:** Sweeping _Witheredness 0->1 shifts all vegetation green->desaturated brown with non-uniform per-clump variation; advancing ToD to dusk visibly cools/dims foliage in lockstep with the sun color.

### 59. Variety pass: ground flowers, fallen leaves, vines, and moss decals
*Foliage - effort M - NEEDS-TOM*
- **Goal:** Break green monotony with curated accents: scattered wildflowers, drifting/settled fallen leaves, climbing vines on walls/rocks, and moss decals at ground/stone seams (which also hide hard diorama mesh joints).
- **Approach:** Add flowers/leaves as extra instance variants in the indirect scatter (separate low-density mask so they read as accents) sharing the SharedWind subgraph. Vines = alpha-clip cross-quad strips on walls/rocks reading the same wind. Moss = URP Decal Projectors at mesh-contact seams, or a vertex-color-masked moss blend in the ground/building ramp. Fallen leaves can add a light drifting particle system that settles. CC0 atlases (Kenney Foliage Sprites, ambientCG, verified-CC0 OpenGameArt); pixel-snap to match card art.
- **Acceptance:** A grassy/forest capture shows >=3 distinct accent types (flowers + fallen leaves + vines/moss) with warm/cool color pops against the green; vines/moss visibly sit on at least one diorama mesh seam, softening the joint.

### 60. Directional river/trough flow (flow map + dual-phase scroll)
*Water - effort L - NEEDS-TOM*
- **Goal:** Make rivers/troughs move downstream along the channel via a flow map with seamless dual-phase blending (no UV-reset pop) while ponds/wells stay still or gentle ripple, since a static river breaks the diorama.
- **Approach:** Shader Graph: paint/author a flow map (RG=2D flow direction) for the river mesh; offset normal/foam/noise UVs by flowDir*(Time*FlowSpeed) using the two-phase trick (two time-offset samples cross-faded by a triangle wave of frac(time)) to hide the wrap. Sample normals from flowed UVs for specular/foam streaks. Ponds = simple two-layer scrolling normal. Reference Scrawk Tiled-Directional-Flow; or Stylized Water 3's river/height-query system if bought.
- **Acceptance:** A river bend capture (or two shots ~0.5s apart) shows foam/normal detail translating along the channel direction (following the bend, not a straight screen-space line) with no abrupt jump/reset.

### 61. Fake refraction via screen-color UV distortion (cheap stylized wobble)
*Water - effort M - auto-safe*
- **Goal:** Add subtle wobble to submerged ground/pebbles/fish so shallow water ripples without a real refraction pass, the difference between water that looks like glass and water that looks wet.
- **Approach:** Transparent water Shader Graph: sample the URP Opaque Texture (Scene Color) using screen-space UVs offset by a scrolling normal/noise vector, magnitude scaled down by depth so shallow edges distort less and you don't grab pixels above the waterline. Requires Opaque Texture enabled; water in the Transparent queue after opaques. Clamp offset to a few pixels; combine multiplicatively with the depth-color.
- **Acceptance:** A shallow pond with a visible submerged texture/pebble shows the detail bent/wobbled vs its real position and animating between frames; distortion does not bleed objects from above the waterline into the water.

### 62. Toon-stepped water specular highlights + gentle vertex ripple waves
*Water - effort M - NEEDS-TOM*
- **Goal:** Add hard-edged cel-stepped sun glints and low-frequency vertex displacement to pond/river surfaces for the chunky toon sparkle and subtle undulation of the target games.
- **Approach:** Shader Graph: compute Blinn-Phong specular from the URP main light, Posterize/Step the highlight to 1-2 bands for the toon glint, modulate by the flowed normal so glints travel with flow. For waves, displace vertices with summed low-freq sine/Gerstner offsets (tiny amplitude for ponds) and recompute normals, or perturb normals if geo is dense. Pair with the existing ramp/toon lighting so water matches surrounding surfaces.
- **Acceptance:** Distinct hard-edged white specular glints (clearly banded, not a soft blob) appear near the sun direction; a side-on capture shows gentle non-flat undulation; both glints and surface shift between frames.

### 63. Stylized caustics on the submerged floor (decal or light-cookie)
*Water - effort M - NEEDS-TOM*
- **Goal:** Project animated dual-layer caustic patterns onto the diorama mesh under shallow water (pond/trough/well bottoms) fading with depth and at the shoreline for the dappled sun-lit shimmer of polished stylized scenes.
- **Approach:** Two URP paths: (1) a downward URP Decal Projector with a caustics Shader Graph sampling two caustic textures at different scales/scroll speeds and taking min() for the non-tiling look; or (2) a directional Light Cookie using a caustic texture so caustics follow the sun. Mask by reconstructed depth-below-water (or confine the decal volume to the water bounds) so caustics only appear under water and fade near the surface. Decal route is more art-directable and confines to the footprint.
- **Acceptance:** The pond/trough floor shows moving dappled bright/dark caustic patterns confined to the wet area, animating between frames; dry ground outside the waterline shows no caustics and no hard rectangular projector edge.

### 64. Reflections tuned for the tilted camera (probe baseline + optional planar for hero ponds)
*Water - effort M - NEEDS-TOM*
- **Goal:** Give still water believable sky/scene reflection appropriate to the fixed tilted camera (which sees a lot of surface) without expensive full SSR.
- **Approach:** Baseline: a boxed Reflection Probe sampled in the water Shader Graph, blended by a Fresnel term (more reflection at grazing angles, which dominate on a tilted cam). For hero ponds where you want buildings/trees mirrored, add a URP Planar Reflection ScriptableRendererFeature (eldskald/planar-reflections-unity) rendering a flipped camera to a RenderTexture; gate it to specific water bodies (it doubles reflected draws). Avoid SSR (not built into URP).
- **Acceptance:** A still pond shows the sky color and a recognizable reflection of nearby tall geometry, strongest near the grazing edge; reflection off makes the surface visibly flatter and darker.

### 65. Localized volumetric fog volumes for interiors and the time-window portal
*Atmosphere - effort L - NEEDS-TOM*
- **Goal:** Place bounded fog/mist volumes (not just global fog) for interiors, low valleys, and especially around the 'peek into the past' portal so it reads as a soft luminous threshold rather than a flat cutout, addressing the project's portal-readability problem.
- **Approach:** Use a volumetric solution supporting local box/ellipsoid volumes (Buto/AERO/Ethereal) layered on the global height fog. For the portal, place a thin local fog volume + a slightly emissive in-scattering boost at the portal plane so backlight bleeds through; tie density to portal-open state. Ensure the chosen solution runs in the Render Graph path and composites before the fullscreen tilt-shift so portal haze also gets the diorama blur. Keep volumes shadow-aware so the sun still streaks through interior windows. BUY the legitimate Kronnect Ethereal on the Asset Store, NOT the nulled aggregator copy.
- **Acceptance:** An interior shows denser mist confined to the room bounds (sharp falloff at the doorway) vs thinner exterior fog; the open portal shows a soft luminous haze hugging the portal plane that is absent when closed.

### 66. Footstep dust / scuff puffs driven by animation events (surface-reactive)
*VFX - effort M - auto-safe*
- **Goal:** Fire small surface-reactive puffs (dust/splash/snow/grass-flick) exactly when the billboard character's feet contact the ground, grounding the 2.5D character into the 3D diorama.
- **Approach:** Add Animation Events on walk/run footplant frames calling PlayFootstepFX(footTransform), or a step-cadence timer synced to move speed for billboards without skeletal feet. The handler positions a pooled one-shot Shuriken burst (6-12 particles, ~0.4s, size-over-lifetime grow+fade, slight up+out velocity) at the foot/ground point. Raycast down to read the surface and pick a variant. Optionally a URP Decal footprint on soft ground. Pool to avoid per-step instantiate cost. CC0 Kenney dust sprites.
- **Acceptance:** At a footplant a small puff appears at the correct foot/ground contact point (not the sprite pivot/center); stepping onto water produces a splash variant; puffs fully fade within ~0.5s leaving no lingering particles.

### 67. Chimney / cookfire smoke and ambient steam columns
*VFX - effort S - NEEDS-TOM*
- **Goal:** Add soft rising smoke from chimneys/campfires/vents plus steam from pots/grates with gentle wind drift as persistent looping emitters, the classic 'this town is inhabited' signal that adds motion to the upper third the tilt camera exposes.
- **Approach:** Looping Shuriken emitters with soft smoke sprites, low alpha, Color-over-Lifetime fade to transparent, Size-over-Lifetime expand on rise, upward Velocity + small world-wind X/Z lean. Soft-particle depth fade (Depth + Opaque Texture) so smoke behind rooftops softly intersects. Per-emitter count ~20-40, distance-cull far chimneys. Tint slightly toward sky ambient, brighten the sun-facing side for a painterly read. Drift on the shared wind. CC0 Kenney or VFX Library smoke.
- **Acceptance:** A town shot shows soft smoke rising and leaning consistently from chimneys in one wind direction; smoke fades fully transparent before a fixed height (no hard cutoff); smoke passing behind a roof edge softly fades rather than showing a hard sprite seam.

### 68. Embers / sparks for fires, forges, and torches
*VFX - effort S - NEEDS-TOM*
- **Goal:** Add upward-rising glowing embers from campfires/braziers/forges/torches that flicker and bloom, with a few drifting off into the dark, adding warm high-frequency sparkle the Bloom/HDR pipeline amplifies.
- **Approach:** Small looping emitter with HDR additive emissive color (Intensity >1 for Bloom), upward Velocity-over-Lifetime with buoyancy ease-out, slight Noise wander, short lifetime, Size+Color-over-Lifetime fading hot-orange->dark-red->transparent. Flicker spawn rate (animated emission or randomized burst). Counts 10-30 per fire. Optionally couple a flickering URP point light (noise-curve intensity) so embers and nearby toon-ramp surfaces share the warm flicker (mind per-object light limits). Soft particles so embers near geometry don't hard-clip. CC0 Kenney spark sprites.
- **Acceptance:** A dark-scene torch shows distinct bright embers with bloom halos rising and fading; a frame-pair shows embers moving up and changing brightness (flicker); embers fade to invisible before a set height with no abrupt disappearance.

### 69. Water splashes, ripples, and waterfall mist via sub-emitters
*VFX/Water - effort M - NEEDS-TOM*
- **Goal:** Add reactive water FX: expanding ripple rings + droplet splashes where things enter water, plus continuous mist at waterfall/fountain bases, so water surfaces feel wet and dimensional and the mist catches the volumetric light.
- **Approach:** A ripple particle system on the water surface using a Sub Emitter (Birth) to spawn a small splash droplet burst per ripple (Unity's documented sub-emitter pattern). For entry, trigger a one-shot ripple+splash at the contact point (raycast to water layer). Waterfall/fountain base = looping low-alpha mist with slight up+out velocity and soft-particle depth fade, placed inside light shafts where possible so it brightens. Reserve particles for splash crowns; let the water shader normal-map handle surface ripple. Reference keijiro/RippleEffect.
- **Acceptance:** An object entering water shows an expanding ring plus a droplet crown at the contact point; the waterfall base shows soft mist that fades with distance and does not hard-clip the rock geometry; ripples expand and fade fully within ~1s.

### 70. Group/target framing for combat and multi-character scenes (auto-recompose)
*Camera - effort M - NEEDS-TOM*
- **Goal:** Auto-frame multiple actors (a battle line, an NPC conversation) keeping all relevant sprites in view and balanced, with dialogue headroom, since manual framing per encounter doesn't scale for a solo dev.
- **Approach:** Cinemachine 3.1: use CinemachineTargetGroup as Follow/LookAt populated with participant transforms (weights/radii); add CinemachineGroupFraming to keep the group framed with configurable padding (fit horizontally+vertically). For dialogue add a CinemachineRecomposer on a conversation cam to apply headroom/screen offset so the speaker sits on a third. Switch via Priority on battle/dialogue start, blending from the gameplay cam.
- **Acceptance:** A battle with 3 enemies + 3 allies spread across the set shows all 6 sprites fully visible inside a safe margin with none clipped; adding a 7th wider enemy auto-pulls the camera back to include it; in a 2-character dialogue the speaker sits off-center on a rule-of-thirds line with headroom.

### 71. Cohesive HD-2D menu layout system: thirds-grid panels, focus dimming, depth-blur backdrop
*UI/HUD - effort L - NEEDS-TOM*
- **Goal:** Build main/pause/inventory menus as a consistent component set: ornate framed panels on a thirds grid, selected-item highlight, and a tilt-shift/blur of the live 3D scene behind the menu, an Octopath-style perceived-polish multiplier.
- **Approach:** Author menu panels from the same uGUI 9-slice frame components as the dialogue box (shared Prefab variants) for consistency. On open, push a fullscreen URP blur (reuse the existing tilt-shift fullscreen effect or a Scriptable Renderer Feature gaussian/kawase pass) over the game render + a semi-transparent dark fill so panels pop. Selection highlight via a 9-slice 'selected' frame swap + TMP color/scale. Lay panels on an explicit thirds grid (anchors at 1/3, 2/3), Screen Space canvas at integer pixel snapping; the blurred world stays full-res so it doesn't fight the pixel UI. CC0 Kenney UI Pack + RPG Expansion.
- **Acceptance:** An open inventory/pause menu shows framed panels aligned to clean screen thirds, the live 3D scene visibly blurred/darkened behind, and the selected entry clearly highlighted (distinct frame/color) vs unselected; frame style matches the dialogue box.

### 72. Stylized snow weather state with top-face accumulation tint and wind drift
*Sky/Weather - effort M - NEEDS-TOM*
- **Goal:** Add a snow weather state: drifting wind-curved snowflake VFX, a cooler/brighter overcast sky, and a top-down accumulation tint whitening world-up faces of ground/buildings, the trick that sells stylized snow far more than particles alone.
- **Approach:** Reuse the weather-controller pattern with slower wind-curved snowflakes (VFX Graph turbulence/wind force) and a brighter overcast sky tint. For accumulation, in the ramp-lit ground/building Shader Graph lerp albedo toward a snow color by saturate(dot(worldNormal,up)) raised to a power, gated by a global _SnowAmount float (Shader.SetGlobalFloat); optional slight smoothness for icy sheen. Tie sky desaturation + fog to _SnowAmount.
- **Acceptance:** Enabling snow shows flakes drifting (not straight down) under wind, a bright cool overcast sky, and upward-facing tops of ground/buildings turning white while vertical faces keep their base color; _SnowAmount=0 fully removes accumulation and flakes.

### 73. Establish world-scale + pivot + texel-density standard for character sprites
*Character Sprites - effort S - auto-safe*
- **Goal:** Document and enforce a standard for character sprite world height, pixels-per-unit, foot pivot at quad bottom, and on-screen texel density so every character sits at correct scale vs buildings/props and stays crisp at the fixed camera distance.
- **Approach:** World unit=1m; quad height = real height (e.g. 1.8 units), pivot at bottom-center aligned to feet. Set sprite import PPU so on-screen texel size is intentional at the camera distance/ortho size; Point filtering, controlled mips. Add a tiny editor validation script flagging any character renderer whose quad height or PPU deviates from the standard. Snap the camera to a fixed pitch/ortho size so texel density is constant across scenes.
- **Acceptance:** A single shot lining up 3+ characters next to a known-size doorway/prop shows correct relative heights (no giant/tiny outliers); a 2x crop shows uniform texel size across all characters (none noticeably blurrier/sharper).

### 74. Prop density / clutter scatter pass for a lived-in look
*Props/Buildings - effort M - NEEDS-TOM*
- **Goal:** Layer secondary clutter (pots, sacks, firewood, hanging laundry, weeds at wall bases, puddles, fallen leaves) so streets and building bases feel occupied and the eye always has detail in the tilt-shift focal band.
- **Approach:** Use a scatter/placement workflow (Unity 6 prefab brush or a simple instancing scatter) to seed small props + reuse the existing alpha-clip foliage cards at wall bases and along paths, all under the ramp shader and SSAO. Keep scatter in the sharp band; GPU-instance repeated clutter; mark static for GI; vary rotation/scale slightly to avoid repetition; cull far scatter aggressively given the fixed camera. CC0 Quaternius/itch.
- **Acceptance:** Before/after of the same street: 'after' has no large empty flat ground patches in the focal band, grass/weeds break up >=60% of wall-to-ground seams, and several small clutter props occupy previously bare corners, without reading as cluttered/unreadable.

### 75. Edge-of-diorama treatment: cliff lip, foliage skirt, value drop-off into fog
*Ground - effort M - NEEDS-TOM*
- **Goal:** Disguise the outer edges of each hand-built diorama (where the mesh ends) so they don't read as a floating slab, using a darkened/rocky cliff lip, a foliage/grass-card skirt over the seam, and a value fade into the volumetric fog.
- **Approach:** Compose existing systems: triplanar cliff layer on the down-facing edge geometry; Polybrush prefab-scatter of the existing alpha-clip foliage cards along the rim to hide the seam; a world-space height gradient to darken and desaturate the lowest edge so it sinks into the fog; a slight vertex-color-painted AO band at the very edge. No new systems.
- **Acceptance:** A wide gameplay shot of a diorama's outer edge shows the boundary disguised by rock lip + foliage and fading into fog, with no visible flat unadorned ground-slab edge in frame.

### 76. Idle breathing + secondary motion and emissive accents (eyes, magic, lanterns) that bloom
*Character Sprites - effort M - NEEDS-TOM*
- **Goal:** Ensure characters never stand perfectly still (subtle breathing/sway/blink in idle) and have HDR emissive regions (glowing eyes, weapon enchant, held lantern, magic charge) that catch the scene's bloom and tie into the volumetric fog.
- **Approach:** Idle motion: author a 2-4 frame breathing/blink loop into the flipbook idle row, or do it cheap in the vertex stage (subtle sine vertical squash/sway driven by _Time, phase-offset per instance so a crowd isn't synchronized). Emissive: add an Emission texture/mask multiplied by an HDR color+intensity to the Emission block so URP Bloom picks it up; optionally place a small URP point/spot Light at the lantern/eyes so the emissive actually casts light and ties into the fog. Pulse magic emissive with _Time.
- **Acceptance:** A 2-frame capture a fraction of a second apart of an 'idle' character shows measurable vertex movement (silhouette differs), not a frozen image; in a dim/night shot the emissive regions show a clear bloom halo and, where a light is attached, a visible pool of light/fog cone around the lantern.

### 77. Shadow + LOD budget for foliage/character cards (cull mode, distance fade, atlas)
*Performance - effort L - NEEDS-TOM*
- **Goal:** Tame shadow cost and far-distance overdraw: distance-fade/cull foliage-card shadows, set correct cull mode for two-sided cards, add LOD/billboard-merge for distant foliage and a texture atlas to collapse materials.
- **Approach:** For two-sided foliage cards use Cull Off only where needed and ensure the ShadowCaster pass applies the same alpha clip so shadows match the cutout (no rectangular full-quad shadow). Use URP Asset shadow distance + cascades to keep foliage-card shadows close and fade beyond; disable self-shadowing on thin cards to kill acne. Add LODGroup/distance billboard-merge so far foliage collapses to fewer cards. Atlas foliage/prop albedo into shared atlases so cards share one material (fewer batch breaks/texture binds). Keep character contact shadows as the cheap blob technique rather than full shadow-map casters if they cost too much.
- **Acceptance:** A shadows shot shows foliage-card shadows matching the leaf cutout (no rectangular full-quad shadow, no heavy acne) near camera and gracefully gone in the distance; Frame Debugger shows reduced shadow-pass draw count vs baseline; foliage material/texture binding count reduced after atlasing.

## P3  (8 items)

### 78. Procedural twinkling star field that fades in only at night
*Sky/Weather - effort S - NEEDS-TOM*
- **Goal:** Add a star layer in the skybox that is invisible by day, fades in as the sun drops, twinkles subtly, and is masked to the upper hemisphere so stars never appear below the horizon, the cheapest high-impact night-mood cue.
- **Approach:** In the skybox Shader Graph use a tiled Voronoi/hashed-noise thresholded high (Step) for sparse point stars from the view-direction UV; multiply by a slow Simple Noise (Time) for twinkle and a horizon mask (saturate of view.Y) so density falls off toward the horizon; multiply the whole layer by the inverse of the sun-height value (1=full night). Optional faint Milky Way band via large stretched noise. Keep it in the same Unlit graph (no extra draw calls).
- **Acceptance:** A night shot shows hundreds of crisp star points concentrated overhead and thinning toward the horizon with none below the horizon line; two night frames a few seconds apart show individual stars changing brightness (twinkle); a daytime shot shows zero stars.

### 79. Stylized rain weather state (VFX Graph) with wet-look and sky/fog coupling
*Sky/Weather - effort L - NEEDS-TOM*
- **Goal:** Add a toggleable rain state: streaked rain VFX, darkened/grayed sky and lower fog, optional lightning flash on the directional light, and a subtle wet specular bump on ground materials, all from one weather controller and cohesive with the sky/fog/sun systems.
- **Approach:** Rain via a URP VFX Graph emitter parented to the camera spawning camera-facing stretched-quad streaks in a box volume ahead of the player, spawn rate gated by a weather-intensity float on the shared director. A weather controller lerps RenderSettings.fogDensity up, pushes the skybox day gradient toward desaturated gray, reduces directional intensity. Lightning = brief light spike + fullscreen flash. Wet look = raise smoothness / darken via a wetness term on the ramp ground gated by Shader.SetGlobalFloat('_Wetness').
- **Acceptance:** Toggling rain on produces falling streaks within a second, the sky/fog desaturate and darken, ground surfaces look wetter (shinier/darker), and an optional lightning event shows a single-frame brightness spike; toggling off returns the clear-weather look.

### 80. Subtle living-camera motion: idle parallax breathing + soft follow damping
*Camera - effort S - NEEDS-TOM*
- **Goal:** Add gentle continuous low-amplitude camera motion (breathing drift) and soft follow damping so the world feels alive and hand-held rather than rigidly locked, without inducing motion sickness, behind an accessibility toggle.
- **Approach:** Cinemachine 3.1: add a CinemachineBasicMultiChannelPerlin Noise stage with a custom low-frequency profile (tiny position Amplitude, Frequency ~0.1-0.3; keep rotation noise near zero). Tune Follow/Position Composer X/Y/Z Damping (~0.3-0.8) so the camera eases behind the player. Optional scripted parallax look-ahead (Position Composer Look Ahead Time). Gate all motion behind an Options toggle. Author the breathing profile as a shared NoiseSettings asset reused by all area cams.
- **Acceptance:** Standing still, a 5s capture shows the camera slowly drifting (~1-2px-equivalent) with no rotation jitter and returning, background parallax shifting subtly vs foreground; moving and stopping eases to rest (no hard snap); an Options toggle disables the motion.

### 81. Optional readability outline (sprite-space alpha or URP edge-detect post) for hero NPCs/washed scenes
*Character Sprites - effort M - NEEDS-TOM*
- **Goal:** Provide an adjustable subtle outline as a fallback/accent for important NPCs or high-contrast scenes (snow/sand/fog) where rim light washes out, without baking a thick stroke into every sprite.
- **Approach:** Evaluate two options: (a) sprite-space outline in Shader Graph sampling alpha at 4/8 neighbor texels at 1-texel offset, drawing outline where center alpha is low but a neighbor is high (Febucci/Ilett), giving a crisp per-sprite stroke that scales with the sprite, preferred for per-character control; (b) full-screen edge detection on the URP Render Graph path via the Sample Buffer node or CONTOUR (built for Unity 6 URP Render Graph), masked to a character rendering layer. Use (a) for per-character; (b) only for a uniform scene-wide stylized edge.
- **Acceptance:** A/B of the same character outline off vs on: the silhouette gains a uniform ~1-2px contrasting edge fully enclosing the sprite with no gaps at thin features (sword/hair tips) and no outline bleeding into the interior.

### 82. Optional camera-lens micro-detail: faint film grain + tiny chromatic aberration (cinematic toggle)
*Post - effort S - NEEDS-TOM*
- **Goal:** Add very low film grain and a touch of chromatic aberration to mimic a physical camera photographing the diorama and to hide banding on fog/gradient skies, exposed as a user toggle and disabled for promo screenshots.
- **Approach:** Film Grain override: Type Medium/Thin1, Intensity ~0.1-0.25, Response ~0.7-0.8 so grain fades in highlights/stays in shadows. Chromatic Aberration override: Intensity ~0.05-0.15 only, just enough to color-fringe high-contrast edges at the periphery; optional custom spectral LUT. Disable both for in-engine screenshots/promo if pixel sharpness is the priority; expose a 'cinematic' toggle.
- **Acceptance:** At 100% zoom a flat fog/sky region shows fine animated grain rather than visible color banding, and high-contrast edges near corners show a faint R/B fringe; central pixel-sprite edges remain readable (grain/CA not strong enough to smear them).

### 83. Subtle vignette to frame the diorama (shared Volume)
*Post - effort S - NEEDS-TOM*
- **Goal:** Add a restrained Vignette darkening corners to reinforce the tilt-shift 'looking into a lit box' feel and pull the eye to the centered party, without an obvious black oval.
- **Approach:** Vignette Volume override in the shared stack: Mode Procedural, Color near-black or a dark scene-tinted color (not pure black), Intensity ~0.25-0.4, Smoothness ~0.5-0.7, Roundness ~0.7-1.0, Rounded ON, Center at screen center. Keep intensity low so corner gameplay stays readable.
- **Acceptance:** Corners are noticeably but softly darker than center with no hard ring; gameplay-relevant elements near corners remain clearly visible; toggling off makes the framing feel flatter.

### 84. Underwater / waterline screen tint for camera dipping below a water surface
*Water - effort M - NEEDS-TOM*
- **Goal:** When the camera or focal point goes below a water surface (deep pond, well shaft, swim area), apply a fullscreen tint + gentle distortion + denser fog so 'under water' reads distinctly from 'above water'. Only relevant if the camera ever dips below a water plane.
- **Approach:** A URP Full Screen Pass Renderer Feature with a Fullscreen Shader Graph tinting scene color, adding depth-based fog density and slight scrolling distortion; trigger via a Volume override or trigger volume when the camera enters water. Optionally underwater god rays via volumetric fog. Switch on/off by water region so non-water areas are unaffected; reference meryuhi/URPFog region-switching.
- **Acceptance:** With the camera below the water plane the whole frame is washed with the water tint, fog increases toward the far plane, and a faint surface line/vignette shows; just above the surface the frame is normal/untinted.

### 85. Evaluate Staggart Stylized Grass Shader as a ground-grass accelerator (buy-vs-build)
*Foliage - effort S - NEEDS-TOM*
- **Goal:** Consider dropping in a mature stylized grass shader to deliver the grass-carpet/interaction/tint items faster (wind, trail flattening, hue/per-object variation, translucency) instead of fully hand-rolling, then restyle to the palette.
- **Approach:** Import Staggart Stylized Grass Shader (Unity 6 edition v2.x, NOT the 2021-2023 one). It provides wind, trail/object/particle bending (covers the trample item), per-object+per-vertex color variation and surface color blending, translucency. Feed its bend/trail input from the same player/portal tramplers; override its tint inputs from the global _SeasonTint/_TimeOfDayTint so it still obeys the control plane. It is a shader/material system, so placement still needs the indirect scatter or a detail system; it complements rather than replaces the grass-carpet item. Standard EULA, low AI risk as a tool (do NOT feed its demo textures into generative tools).
- **Acceptance:** Side-by-side: the asset-based grass field shows wind sway + a flattened trail behind the moving character and per-clump hue variation, color-matched to the ground; its tint visibly responds when the global season tint changes (proving control-plane integration, not a parallel system).

## Asset shopping list

- **Quaternius Medieval Village MegaKit + Fantasy Props MegaKit** (CC0 low-poly 3D model/modular building kit; Free (Standard/Pro); optional Patreon ~$10-50/mo for Unity-URP-prebuilt + .blend source; CC0 1.0 Universal) -- for Replace cube placeholder props (rank 8) and adopt a modular grid-snapping building kit with interior-included walls for the lit-window item (ranks 42, 44, 74). One unified art voice across the whole town.. <https://quaternius.com/packs/medievalvillagemegakit.html> -- WARN: CC0 = no AI-training restriction, fully safe for the AI-assisted pipeline. Imported PBR materials won't match the toon ramp, so reassign your own ramp shader.
- **Kenney asset bundle (Particle Pack, Fantasy UI Borders, UI Pack + RPG Expansion, Input Prompts Pixel, Foliage Sprites, Medieval Town/Pirate)** (CC0 sprite/particle/UI/prop packs + light cookies; Free (donation optional); CC0 1.0) -- for Dust/spark/smoke/glow sprites + light cookies for all VFX (ranks 18,45,46,47,66,67,68); 9-slice UI frames + panels for dialogue/menus (ranks 20,71); pixel button glyphs for prompts (rank 36); foliage accent sprites (rank 59); far/mid prop dressing.. <https://kenney.nl/assets> -- WARN: CC0, no attribution, no AI restriction. Clean-modern style; recolor/pixelate UI and flatten/posterize to match the HD-2D pixel/painterly bar.
- **ambientCG (ground + tiling PBR materials) + FreeStylized (stylized textures, free tier)** (CC0 / custom-CC0 tiling PBR textures (albedo/normal/height/AO); Free; CC0 1.0 (ambientCG); custom CC0 (FreeStylized free tier, commercial OK, attribution optional)) -- for Ground splat layers + height maps for the vertex-color shader (ranks 6,28), and stylized wood/stone/roof-tile/plaster for the cohesive building materials and surface detailing (ranks 9,43).. <https://ambientcg.com/list?type=material&category=ground> -- WARN: Free CC0 items are AI-safe. ambientCG is photoreal, so stylize (flatten normals, posterize albedo) to fit the toon look; FreeStylized Patreon-tier content is NOT CC0 and has no explicit AI clause, so verify before AI use.
- **Voxel Core Lab Watercolor Terrain Textures** (CC0 stylized seamless ground textures (16x 1024: grass/stone/dirt/water); Pay-what-you-want / effectively $0; CC0 1.0) -- for Stylized ground texture library sized for the splat shader (rank 6 prerequisite, ground texture set).. <https://voxelcorelab.itch.io/watercolor-terrain-textures> -- WARN: CC0, no AI/ML ingestion limits; pack states no generative AI was used to make it. Painterly style matches HD-2D out of the box.
- **CristianQiu Unity-URP-Volumetric-Light** (Free open-source raymarched volumetric lighting (Render Graph); Free; MIT (verify repo)) -- for The single shared god-rays/light-shafts system from the ToD sun, shadow-occluded by buildings and tree cards (rank 13); foundation the dust-mote item sits inside (rank 18).. <https://github.com/CristianQiu/Unity-URP-Volumetric-Light> -- WARN: MIT, no AI restriction. RG-compatible up to 6000.4 per repo; confirm against the exact 6000.3.x in use. Best free starting point before buying LSPP/Buto.
- **Cyanilux URP_ShaderGraphCustomLighting** (Free MIT Shader Graph custom-lighting subgraphs (GetMainLight/GetAdditionalLight, rim/Fresnel); Free; MIT) -- for Custom lighting plumbing for the billboard self-shadow fix (rank 17), sprite rim/back light (rank 23), and any custom toon math; URP v17.1+/Unity 6000.1+.. <https://github.com/Cyanilux/URP_ShaderGraphCustomLighting> -- WARN: MIT, no AI restriction. Code reference only; you write the billboard + shadow-caster passes yourself.
- **Free URP shader/code references (meryuhi/URPFog, Scrawk Tiled-Directional-Flow, eldskald planar-reflections-unity, MatrixRex Uber-Stylized-Water, keijiro RippleEffect/MiniBokeh)** (Free open-source URP shader/effect references; Free; Mostly MIT/Unlicense (verify each repo LICENSE before shipping verbatim)) -- for Build-it-yourself bases for fog (rank 12), river flow (rank 60), planar reflections (rank 64), stylized water (ranks 21,22,62), ripples (rank 69), and planar DoF (keijiro MiniBokeh, rank 2 reference).. <https://github.com/meryuhi/URPFog> -- WARN: Not Asset Store items, so no AS AI-training clause; but verify each repo's actual LICENSE file before copying code wholesale, and confirm Unity 6 Render Graph compatibility (some predate it). keijiro MiniBokeh needs Render Graph (no Compatibility Mode).
- **Staggart Creations Stylized Grass Shader (for Unity 6)** (Paid URP grass shader (wind, trail bending, hue variation, translucency); $39 (v2.0.1, Unity 6/URP only); Unity Asset Store Standard EULA (Single Entity)) -- for Optional accelerator to deliver the grass-carpet/interaction/tint items faster (rank 85), integrated under the global wind/season control plane.. <https://assetstore.unity.com/packages/vfx/shaders/stylized-grass-shader-for-unity-6-357954> -- WARN: Standard EULA has no anti-AI clause and it's a shader (not training-ingested art) so low AI-pipeline risk; do NOT feed its bundled demo textures into generative tools. Buy the Unity 6 edition (357954), not the 2021-2023 one (143830).
- **Alexander Ameye Stylized Water 3 (+ Underwater extension)** (Paid URP water shader/system (Unity 6 / Render Graph native); ~$49 (base); underwater extension separate; Unity Asset Store Standard EULA (Single Entity)) -- for Optional all-in-one if you want river height-query, waterfall prefabs, and underwater as a package instead of hand-building water items (ranks 21,22,60,84).. <https://assetstore.unity.com/packages/vfx/shaders/stylized-water-3-287769> -- WARN: AS EULA explicitly PROHIBITS using the asset to train AI/ML models (locally or via 3rd parties); runtime use is fine. Do NOT feed its source/textures/graphs into any model or codegen tool. Requires Unity 6000.0.60f1+ with Render Graph. The free Ameye/Ilett tutorial route is the AI-pipeline-safest alternative.
- **OccaSoftware Altos (Volumetric Clouds, Skybox, Weather) 窶・reference/fallback only** (Paid URP sky/cloud/day-night/weather system; ~$2.70 on sale (reg $45), v7.17.x supports Unity 6 URP + Render Graph; Unity Asset Store Standard EULA) -- for Reference/fallback for the gradient skybox + sun/moon/stars day-night module (ranks 40,78); at $2.70 worth grabbing just to study its sky module.. <https://assetstore.unity.com/packages/tools/particles-effects/altos-volumetric-clouds-dynamic-sky-sun-moon-stars-day-night-cyc-221227> -- WARN: Standard EULA, no anti-AI clause. Its volumetric clouds read too realistic for the pixel-art bar and stack full-screen cost on top of tilt-shift + volumetric fog; prefer the billboard/gradient skybox approach and use Altos only for reference or its sky/day-night submodule.
- **Kronnect Umbra (Better Directional Shadows) + Ethereal URP (Volumetric Fog) 窶・paid options** (Paid URP shadow/volumetric assets; Umbra ~EUR 35; Ethereal ~$30-45; Unity Asset Store / Kronnect EULA) -- for Umbra = contact-hardening soft shadows (rank 25) if built-in URP tuning is insufficient; Ethereal = local volumetric fog volumes for interiors and the time-window portal (rank 65).. <https://store.kronnect.com/products/umbra-better-directional-shadows-for-urp> -- WARN: Standard EULA, no AI-art concern (shaders/tools). VERIFY current Unity 6 Render Graph compatibility before buying (Umbra page said RG support 'coming next update'). For Ethereal, BUY the legitimate Kronnect 'Ethereal URP' on the Asset Store, NOT the unityassetcollection.com 'free download' (a nulled/piracy aggregator).
- **Misc paid VFX/UI/sprite packs (VFX Library Env&Ambient $15, Fireflies VFX Graph ~$5-10, Rain VFX STYLE $9.99, RetroRender/QuickSprites 3D ~$10-25, HD 8-Dir character pack, Ornate Fantasy Pixel UI ~$7.99, Fronkon Artistic Tilt Shift ~$15)** (Paid VFX / sprite-bake tools / UI / character sprite packs; ~$5-25 each (verify live; AS prices fluctuate); Unity Asset Store Standard EULA / itch per-listing license) -- for Drop-in leaves/smoke/mist/fireflies/rain VFX (ranks 46,47,67,79); 3D-to-sprite bake tools (rank 35); ornate pixel UI frames (rank 20); ready directional character sprites; fullscreen tilt-shift (post stack).. <https://assetstore.unity.com/packages/vfx/particles/environment/vfx-library-environmental-and-ambient-fx-collection-209648> -- WARN: Standard AS EULA has no explicit anti-AI clause and these are used as runtime art/tools (not training inputs), so risk is low 窶・but verify each itch listing permits modification and check no publisher added a 2024-2025 AI clause; verify Unity 6 / VFX Graph version compiles on import (many target 2021-2022.3).
- **AVOID: Synty Studios POLYGON kits / Particle FX / UI (any Synty art)** (Paid stylized 3D/VFX/UI packs 窶・flagged as a caution, NOT a recommendation; ~$10-100 per pack; Synty One-Time Purchase Licence) -- for N/A 窶・listed only to flag the EULA conflict; would otherwise tempt as prop/character-source/VFX/UI art.. <https://syntystore.com/pages/one-time-purchase-licence> -- WARN: BLOCKER for this AI-assisted pipeline: Synty's EULA EXPLICITLY prohibits using assets in datasets for, in development of, or as inputs to Generative AI Programs (and even in AI-related marketing). Do NOT route any Synty art through Meshy/image-gen/training/AI-tooled promo. Even a non-AI sprite bake is risky to manage; prefer CC0 (Quaternius/Kenney) or obtain a custom Synty licence.

## Quick wins

- Enable the prerequisite URP textures FIRST: Depth Texture + Opaque Texture on the URP Asset, and HDR on the URP Asset. Almost every effect (fog depth-fade, water depth color, foam, fake refraction, soft particles, SSAO DepthNormals, bloom on emissive) silently fails without these. ~10 minutes, unblocks a dozen items.
- Flip the gameplay camera to Perspective with a narrow FOV (~25deg) at ~30deg pitch (rank 1). One Inspector change is THE precondition for Bokeh DoF working at all (URP DoF is broken on ortho) and instantly gives the diorama-compression read.
- Add the SSAO Renderer Feature with Source=DepthNormals, low Intensity (rank 11) 窶・one renderer-feature add, objectively verifiable, immediately stops props/sprites from floating.
- Tune the master HDR grade Volume: Tonemapping=Neutral on, a gentle Split Toning (warm highlights / cool shadows) + a small Contrast/Saturation bump (rank 5) 窶・biggest before/after cohesion jump for the time spent, and it must exist before bloom/DoF tuning to avoid double-grading.
- Set the bloom Threshold above lit mid-tones (~1.0-1.2) and lower Clamp to ~8-16 (rank 26) 窶・kills the most common amateur HD-2D failure (global haze + foliage-edge fireflies) in two slider moves.
- Switch pixel-font TMP assets to RASTER_HINTED at native point size with Point filtering (rank 19) 窶・small change that removes blurry-text, the instant 'not set up correctly' tell.
- Drop in CristianQiu volumetric light (free MIT) wired to the sun (rank 13) before evaluating any paid god-ray asset 窶・biggest single 'wow' from the existing fog at zero cost.
- Grab Quaternius + Kenney CC0 kits and bulk-reassign your ramp shader to replace cube placeholders (rank 8) 窶・cohesion beats fidelity; even a fast pass kills the 'blocked out' look.

## Human-eye items (do NOT finalize unattended)

- Camera framing feel: FOV, pitch, and focus-band / aperture for the Bokeh DoF (ranks 1, 2) 窶・taste-defining; over-blur crushes readability. Eyeball the aperture/screenshot, never finalize unattended.
- Time-of-day key-light Kelvin values, intensity curve, and the warm/cool HDR grade including split-toning and per-zone tints (ranks 3, 5, 27) 窶・art direction; the luminance/hue acceptance numbers are only guardrails, final values are a taste call.
- APV ambient floor + night readability: confirm by eye that shadow sides stay readable AND the scene still reads as night, not just that a luminance number is met (rank 4).
- Polybrush ground painting, height-blend transition crispness, anti-tiling, world-space tint, and edge-of-diorama treatment (ranks 7, 28, 30, 55, 75) 窶・composition/painting tasks; review path placement and value falloff visually.
- Rim-light intensity/color and the billboard self-shadow + cast-shadow fix balance (ranks 17, 23) 窶・verify A/B that sprites separate from background without looking stickered, and that shadows read as a body at noon.
- Water color gradient, foam width/edge break-up, toon-spec banding, and reflections (ranks 21, 22, 62, 64) 窶・must match the existing ramp/toon look against an Octopath/Sea of Stars reference frame.
- Dust-mote / firefly / leaf density and alpha (ranks 18, 45, 46, 47) 窶・must stay faint and never read as snow/noise; density is a pure taste call and the dominant way ambient VFX fails.
- Building material cohesion, modular building variety, signage/dressing density, and clutter scatter (ranks 8, 9, 42, 44, 74) 窶・palette and lived-in density are art-director judgments; confirm no two adjacent buildings look identical and the frame isn't over-cluttered.
- Skybox gradient, sun/moon discs, cloud cards, and parallax backdrop tinting (ranks 40, 41, 78) 窶・painterly bar judged by eye; verify clouds melt into the gradient and backdrops haze believably.
- UI: dialogue-box ornamentation, font choice, menu thirds-grid composition and focus dimming (ranks 19, 20, 71) 窶・readability + genre-signal taste; check corners at multiple resolutions.
- Cutscene side-pan timing/composition and group-framing padding (ranks 39, 70) 窶・directed beats; review the actual motion, not just that it runs.
- Restraint pass on grain/CA/vignette and any outline width (ranks 81, 82, 83) 窶・these muddy pixel art when overdone; sign off subtlety per scene and disable for promo screenshots.

## Synthesizer notes

SYNTHESIS METHOD: Merged 12 dimensions (~100 raw items) into 85 deduped, globally-ranked items. Tiers: P0=readability/illusion foundations that gate everything (camera rig, focus tracking, ToD key light, APV ambient floor, master grade, ground splat+paint, cube replacement, cohesive materials, lit windows, SSAO grounding, atmospheric fog, god rays, grass carpet, wind control plane, overdraw budget, billboard shadow fix, shaft dust, font pipeline, dialogue box); P1=core HD-2D signature polish; P2=atmospheric/dressing richness; P3=nice-to-have flourishes.

KEY DEDUPES (overlaps collapsed into one canonical item, cross-referenced in approach text): (1) God rays / light shafts appeared in LIGHTING, ATMOSPHERE, and AMBIENT VFX 竊・one shared CristianQiu-based shafts system (rank 13) with the dust-mote item (rank 18) living inside it. (2) HDR color grade / split toning / per-zone grading appeared in LIGHTING and POST-PROCESSING 竊・one master grade Volume stack (rank 5) + per-zone deltas (rank 27), with an explicit 'ONE Volume stack / one tonemapper' constraint so bloom/DoF/tilt-shift never double-grade. (3) Perspective camera + Bokeh DoF appeared in POST and CAMERA 竊・unified (ranks 1,2). (4) Contact shadows / SSAO grounding appeared in 4 dimensions 竊・consolidated SSAO+contact-AO (rank 11) plus the separate character blob shadow (rank 52). (5) Global wind appeared in SKY, FOLIAGE, and AMBIENT VFX 竊・ONE control plane (rank 15) consumed by foliage, clouds, and the VFX director (rank 48). (6) Aerial perspective / fog appeared in ATMOSPHERE and SHADER 竊・fullscreen fog (rank 12) + a complementary in-shader aerial tint (rank 56) that preserves toon banding on far buildings. (7) Rim light appeared in LIGHTING and CHARACTER 竊・one item (rank 23). (8) Render-Graph perf/ordering + overdraw + SRP batcher consolidated into a performance cluster (ranks 16,49,50,51,77).

CRITICAL UNITY-6 GOTCHAS embedded in items (load-bearing): URP Bokeh DoF is effectively broken on ORTHOGRAPHIC cameras (must be perspective); Cinemachine 'Focus Tracks Target' does NOT drive DoF focusDistance in URP (only HDRP) so you must script it; Cinemachine Confiner 2D mis-clamps a tilted camera (use Confiner3D or a custom XZ clamp); MaterialPropertyBlock breaks BOTH SRP Batcher and GPU instancing (use instanced cbuffer / vertex colors for per-instance wind/tint); GPU Resident Drawer only helps Mesh Renderers, NOT SpriteRenderers/billboards/SkinnedMesh; alpha-clip writes depth (depth-rejectable) but alpha-blend does not (the overdraw lever); HDR + Bloom + emission-in-HDR are all required together or emissive items just blur, not glow; UI Toolkit does not support 9-slice 'Sliced' Sprite backgrounds so use uGUI for ornate pixel frames; Pixel Perfect Camera 'Upscale Render Texture' makes World-Space canvas text illegible (keep UI on Screen Space).

EULA / AI-PIPELINE (decisive for this dev): SYNTY is the one hard BLOCKER 窶・its EULA explicitly forbids using assets in/for/as inputs to Generative AI Programs, conflicting with the Meshy/AI-assisted pipeline; flagged as AVOID in the shopping list (rank-relevant for props/characters/VFX/UI sources). CC0 (Quaternius, Kenney, ambientCG, Voxel Core Lab) is the safe default 窶・CC0 places no limits on AI/ML ingestion. Unity Asset Store Standard EULA does NOT broadly ban AI training BUT specific shader assets (Stylized Water 3, Pixel Art Water) DO prohibit training ON the asset 窶・runtime use is fine, just never feed their source/textures into a model. For maximum safety the self-authored Shader Graph route (grounded in free Ameye/Ilett/Roystan/Cyanilux references) keeps water/fog/lighting fully AI-pipeline-safe and consistent with the existing ramp/toon ecosystem. One piracy note: buy Kronnect Ethereal from the official Asset Store, not the unityassetcollection.com nulled 'free download'.

AUTONOMOUS-SAFE LOGIC: marked autonomousSafe=true only for objectively verifiable, mechanical/plumbing work (shader graph splatting, SSAO setup, wind control-plane wiring, triplanar correctness, bend-map RT, depth priming, SRP batcher, Render Graph ordering, contact-shadow decal, world-scale validation, fake refraction which has a clear too-much bound, VFX director architecture, camera void clamp). Everything taste-heavy (lighting values, color grade, composition, density, framing feel, palette, font/UI ornamentation) is autonomousSafe=false and listed in humanEyeItems 窶・consistent with the project's standing 'always eyeball the aperture/screenshot, don't conclude with a generous 'acceptable'' practice. Prices are approximate and fluctuate with Asset Store sales 窶・re-verify at purchase. All asset Render Graph compatibility claims need a final check against the exact 6000.3.x build before buying.