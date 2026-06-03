using System;
using System.Collections.Generic;
using System.Reflection;
using Anemora.FastVS;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

#if BUTO
using OccaSoftware.Buto.Runtime;
#endif

#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
#endif

namespace Anemora.FastVS.SunCycle
{
    public enum SunPreset
    {
        Morning = 0,
        Noon = 1,
        Evening = 2,
        Night = 3,
    }

    [AddComponentMenu("Anemora/HD2D/Sun Cycle Driver")]
    [DisallowMultipleComponent]
    public sealed class AnemoraSunCycleDriver : MonoBehaviour
    {
        private const int PresetCount = 4;
        private const float MinTransitionDuration = 0.05f;
        private const float MinColorTemperatureKelvin = 1500f;
        private const float MaxColorTemperatureKelvin = 20000f;
        private const float KelvinKeyColorBlend = 0.35f;
        private static readonly int Hd2dSunKeyColorId = Shader.PropertyToID("_AnemoraHd2dSunKeyColor");
        private static readonly int Hd2dSunKeyDirectionId = Shader.PropertyToID("_AnemoraHd2dSunKeyDirection");
        private static readonly int Hd2dSunKeyKelvinId = Shader.PropertyToID("_AnemoraHd2dSunKeyKelvin");
        private static readonly int Hd2dSunKeyIntensityId = Shader.PropertyToID("_AnemoraHd2dSunKeyIntensity");
        private static readonly int Hd2dReadabilityFloorId = Shader.PropertyToID("_AnemoraHd2dReadabilityFloor");
        private static readonly int Hd2dReadabilityFloorTintId = Shader.PropertyToID("_AnemoraHd2dReadabilityFloorTint");
        private static readonly int Hd2dReadabilityFloorStrengthId = Shader.PropertyToID("_AnemoraHd2dReadabilityFloorStrength");
        private static readonly int Hd2dWindowEmissionStrengthId = Shader.PropertyToID("_AnemoraHd2dWindowEmissionStrength");

        [Header("References")]
        [SerializeField] private Light directionalSunLight;
        [SerializeField] private Volume globalVolume;

        [Header("Presets")]
        [SerializeField] private SunPresetData[] presets = new SunPresetData[PresetCount];

        [Header("Defaults")]
        [SerializeField] private SunPreset defaultPreset = SunPreset.Morning;
        [SerializeField, Range(MinTransitionDuration, 10f)] private float transitionDuration = 1.8f;
        [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private SunRuntimeValues currentValues;
        private SunRuntimeValues transitionStartValues;
        private SunRuntimeValues transitionTargetValues;
        private bool hasAppliedValues;
        private float transitionElapsed;
        private MapSunAnchor activeAnchor;
        private FastVsHouseAreaVisibility areaVisibility;

        public static AnemoraSunCycleDriver Instance { get; private set; }

        public SunPreset CurrentPreset { get; private set; }
        public SunPreset TargetPreset { get; private set; }
        public bool IsTransitioning { get; private set; }
        public bool IndoorSunSuppressionActiveForReview => IsIndoorAreaActive();

        public event Action<SunPreset, SunPreset> OnPresetChanged;
        public event Action<SunPreset, SunPreset> OnPresetTransitionStart;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[AnemoraSunCycleDriver] Duplicate driver found; destroying the newer instance.", this);
                enabled = false;
                Destroy(gameObject);
                return;
            }

            Instance = this;
            CurrentPreset = defaultPreset;
            TargetPreset = defaultPreset;
            EnsurePresetArraySize();
            ResolveSceneReferences();
        }

        private void OnEnable()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded += HandleSceneLoaded;
            }
        }

        private void Start()
        {
            if (Instance == this)
            {
                ApplySceneAnchorOrDefault(true);
            }
        }

        private void Update()
        {
            if (!IsTransitioning)
            {
                return;
            }

            transitionElapsed += Time.deltaTime;
            var rawT = transitionDuration > Mathf.Epsilon ? Mathf.Clamp01(transitionElapsed / transitionDuration) : 1f;
            var easedT = transitionCurve != null ? Mathf.Clamp01(transitionCurve.Evaluate(rawT)) : rawT;

            var blended = SunRuntimeValues.Lerp(transitionStartValues, transitionTargetValues, easedT);
            ApplyValues(blended);

            if (rawT < 1f)
            {
                return;
            }

            var from = CurrentPreset;
            CurrentPreset = TargetPreset;
            IsTransitioning = false;
            currentValues = transitionTargetValues;
            ApplyValues(currentValues);
            OnPresetChanged?.Invoke(from, CurrentPreset);
        }

        private void OnDisable()
        {
            if (Instance == this)
            {
                SceneManager.sceneLoaded -= HandleSceneLoaded;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void OnValidate()
        {
            EnsurePresetArraySize();
            transitionDuration = Mathf.Max(MinTransitionDuration, transitionDuration);

            if (transitionCurve == null || transitionCurve.length == 0)
            {
                transitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
            }
        }

        public void ApplyPreset(SunPreset preset, bool instant = false)
        {
            var targetData = GetPresetData(preset);
            if (targetData == null)
            {
                Debug.LogWarning($"[AnemoraSunCycleDriver] Missing SunPresetData for {preset}; preset was not applied.", this);
                return;
            }

            ResolveSceneReferences();

            var targetValues = SunRuntimeValues.FromPresetData(targetData);
            TargetPreset = preset;

            if (instant || !hasAppliedValues || transitionDuration <= MinTransitionDuration)
            {
                var from = CurrentPreset;
                IsTransitioning = false;
                transitionElapsed = 0f;
                CurrentPreset = preset;
                currentValues = targetValues;
                hasAppliedValues = true;
                ApplyValues(currentValues);
                OnPresetChanged?.Invoke(from, preset);
                return;
            }

            transitionStartValues = currentValues;
            transitionTargetValues = targetValues;
            transitionElapsed = 0f;
            IsTransitioning = true;
            OnPresetTransitionStart?.Invoke(CurrentPreset, preset);
        }

        public void RegisterMapSunAnchor(MapSunAnchor anchor)
        {
            if (anchor == null)
            {
                return;
            }

            var anchorScene = anchor.gameObject.scene;
            if (anchorScene.IsValid() && anchorScene != SceneManager.GetActiveScene())
            {
                return;
            }

            if (activeAnchor != null && activeAnchor != anchor && activeAnchor.Priority > anchor.Priority)
            {
                return;
            }

            activeAnchor = anchor;
            ApplyPreset(anchor.SunPreset, !anchor.TransitionFromPrevious);
        }

        public SunPresetData GetPresetData(SunPreset preset)
        {
            if (presets == null)
            {
                return null;
            }

            var index = (int)preset;
            if (index >= 0 && index < presets.Length)
            {
                var indexed = presets[index];
                if (indexed != null && indexed.preset == preset)
                {
                    return indexed;
                }
            }

            for (var i = 0; i < presets.Length; i++)
            {
                var candidate = presets[i];
                if (candidate != null && candidate.preset == preset)
                {
                    return candidate;
                }
            }

            return null;
        }

        public static Vector3 GetReferenceDirectionEuler(SunPreset preset)
        {
            switch (preset)
            {
                case SunPreset.Morning:
                    return new Vector3(28f, -128f, 0f);
                case SunPreset.Evening:
                    return new Vector3(24f, -28f, 0f);
                case SunPreset.Night:
                    return new Vector3(-32f, 12f, 0f);
                case SunPreset.Noon:
                default:
                    return new Vector3(50f, -78f, 0f);
            }
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Additive)
            {
                return;
            }

            activeAnchor = null;
            ApplySceneAnchorOrDefault(true);
        }

        private void ApplySceneAnchorOrDefault(bool instant)
        {
            var anchor = FindHighestPriorityAnchorInActiveScene();
            if (anchor != null)
            {
                activeAnchor = anchor;
                ApplyPreset(anchor.SunPreset, instant || !anchor.TransitionFromPrevious);
                return;
            }

            ApplyPreset(defaultPreset, instant);
        }

        private MapSunAnchor FindHighestPriorityAnchorInActiveScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            var anchors = FindObjectsByType<MapSunAnchor>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            MapSunAnchor best = null;

            for (var i = 0; i < anchors.Length; i++)
            {
                var anchor = anchors[i];
                if (anchor == null)
                {
                    continue;
                }

                var anchorScene = anchor.gameObject.scene;
                if (anchorScene.IsValid() && anchorScene != activeScene)
                {
                    continue;
                }

                if (best == null || anchor.Priority >= best.Priority)
                {
                    best = anchor;
                }
            }

            return best;
        }

        private void ResolveSceneReferences()
        {
            if (directionalSunLight == null)
            {
                directionalSunLight = RenderSettings.sun;
            }

            if (directionalSunLight == null)
            {
                var lights = FindObjectsByType<Light>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                for (var i = 0; i < lights.Length; i++)
                {
                    if (lights[i] != null && lights[i].type == LightType.Directional)
                    {
                        directionalSunLight = lights[i];
                        break;
                    }
                }
            }

            if (globalVolume == null)
            {
                var volumes = FindObjectsByType<Volume>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
                for (var i = 0; i < volumes.Length; i++)
                {
                    if (volumes[i] != null && volumes[i].isGlobal)
                    {
                        globalVolume = volumes[i];
                        break;
                    }
                }

                if (globalVolume == null && volumes.Length > 0)
                {
                    globalVolume = volumes[0];
                }
            }

            if (areaVisibility == null)
            {
                areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            }
        }

        private void ApplyValues(SunRuntimeValues values)
        {
            currentValues = values;
            hasAppliedValues = true;
            var effectiveValues = ApplyActiveAreaSunPolicy(values);
            EnableKelvinLightingProjectSettings();

            if (directionalSunLight != null)
            {
                directionalSunLight.transform.rotation = effectiveValues.lightRotation;
                directionalSunLight.useColorTemperature = effectiveValues.useColorTemperature;
                directionalSunLight.colorTemperature = Mathf.Clamp(effectiveValues.colorTemperatureKelvin, MinColorTemperatureKelvin, MaxColorTemperatureKelvin);
                directionalSunLight.color = effectiveValues.lightColor;
                directionalSunLight.intensity = effectiveValues.lightIntensity;
                directionalSunLight.cookie = effectiveValues.cookieTexture;
                directionalSunLight.cookieSize2D = new Vector2(effectiveValues.cookieSize, effectiveValues.cookieSize);
            }

            PublishSunKeyShaderGlobals(effectiveValues);
            ApplyVolumeValues(effectiveValues);
            ApplyOptionalSunEffects(effectiveValues);

            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = effectiveValues.ambientLightColor;
            RenderSettings.fogColor = effectiveValues.fogColor;
            RenderSettings.fogDensity = effectiveValues.fogDensity;

            var skybox = RenderSettings.skybox;
            if (skybox == null)
            {
                return;
            }

            if (skybox.HasProperty("_SkyTint"))
            {
                skybox.SetColor("_SkyTint", effectiveValues.skyTint);
            }

            if (skybox.HasProperty("_SunSize"))
            {
                skybox.SetFloat("_SunSize", effectiveValues.skySunSize);
            }

            if (skybox.HasProperty("_SunSizeConvergence"))
            {
                skybox.SetFloat("_SunSizeConvergence", effectiveValues.skySunSizeConvergence);
            }
        }

        private static void PublishSunKeyShaderGlobals(SunRuntimeValues values)
        {
            var keyColor = EvaluateKeyLightColor(values.lightColor, values.useColorTemperature, values.colorTemperatureKelvin);
            var direction = values.lightRotation * Vector3.forward;
            Shader.SetGlobalColor(Hd2dSunKeyColorId, keyColor);
            Shader.SetGlobalVector(Hd2dSunKeyDirectionId, new Vector4(direction.x, direction.y, direction.z, 0f));
            Shader.SetGlobalFloat(Hd2dSunKeyKelvinId, values.useColorTemperature ? Mathf.Clamp(values.colorTemperatureKelvin, MinColorTemperatureKelvin, MaxColorTemperatureKelvin) : 0f);
            Shader.SetGlobalFloat(Hd2dSunKeyIntensityId, values.lightIntensity);
            Shader.SetGlobalFloat(Hd2dReadabilityFloorId, Mathf.Clamp(values.readabilityFloor, 0f, 0.12f));
            Shader.SetGlobalColor(Hd2dReadabilityFloorTintId, values.readabilityFloorTint);
            Shader.SetGlobalFloat(Hd2dReadabilityFloorStrengthId, Mathf.Clamp01(values.readabilityFloorStrength));
            Shader.SetGlobalFloat(Hd2dWindowEmissionStrengthId, Mathf.Clamp(values.windowEmissionStrength, 0f, 1.5f));
        }

        private static void EnableKelvinLightingProjectSettings()
        {
            GraphicsSettings.lightsUseLinearIntensity = true;
            var useColorTemperatureProperty = typeof(GraphicsSettings).GetProperty(
                "lightsUseColorTemperature",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (useColorTemperatureProperty != null && useColorTemperatureProperty.CanWrite)
            {
                useColorTemperatureProperty.SetValue(null, true);
            }
        }

        private static Color EvaluateKeyLightColor(Color artTint, bool useColorTemperature, float colorTemperatureKelvin)
        {
            if (!useColorTemperature)
            {
                return artTint;
            }

            var kelvinTint = Mathf.CorrelatedColorTemperatureToRGB(Mathf.Clamp(colorTemperatureKelvin, MinColorTemperatureKelvin, MaxColorTemperatureKelvin));
            var kelvinKeyColor = new Color(
                artTint.r * kelvinTint.r,
                artTint.g * kelvinTint.g,
                artTint.b * kelvinTint.b,
                artTint.a);
            return Color.Lerp(artTint, kelvinKeyColor, KelvinKeyColorBlend);
        }

        private SunRuntimeValues ApplyActiveAreaSunPolicy(SunRuntimeValues values)
        {
            if (!IsIndoorAreaActive())
            {
                return values;
            }

            values.lightIntensity *= 0.08f;
            values.skySunSize = 0.001f;
            values.skySunSizeConvergence = Mathf.Max(values.skySunSizeConvergence, 10f);
            values.fogDensity = 0f;
            values.volumetricFogEnabled = false;
            values.screenSpaceLensFlareIntensity = 0f;
            values.sunLensFlareIntensity = 0f;
            return values;
        }

        private bool IsIndoorAreaActive()
        {
            if (areaVisibility == null)
            {
                areaVisibility = FindFirstObjectByType<FastVsHouseAreaVisibility>();
            }

            return areaVisibility != null &&
                   (areaVisibility.ActiveAreaForReview == FastVsHouseArea.Interior ||
                    areaVisibility.ActiveAreaForReview == FastVsHouseArea.Library ||
                    areaVisibility.ActiveAreaForReview == FastVsHouseArea.MiaInterior ||
                    areaVisibility.ActiveAreaForReview == FastVsHouseArea.AriaInterior);
        }

        private void ApplyVolumeValues(SunRuntimeValues values)
        {
            var profile = GetVolumeProfile();
            if (profile == null)
            {
                return;
            }

            if (profile.TryGet<ColorLookup>(out var colorLookup))
            {
                colorLookup.active = true;
                colorLookup.texture.overrideState = true;
                colorLookup.texture.value = values.colorLookup;
                colorLookup.contribution.overrideState = true;
                colorLookup.contribution.value = values.lutContribution;
            }

            if (profile.TryGet<WhiteBalance>(out var whiteBalance))
            {
                whiteBalance.active = true;
                whiteBalance.temperature.overrideState = true;
                whiteBalance.temperature.value = values.volumeTemperature;
                whiteBalance.tint.overrideState = true;
                whiteBalance.tint.value = values.volumeTint;
            }

            if (profile.TryGet<Bloom>(out var bloom))
            {
                bloom.active = true;
                bloom.tint.overrideState = true;
                bloom.tint.value = values.bloomTint;
            }
        }

        private void ApplyOptionalSunEffects(SunRuntimeValues values)
        {
            ApplyDirectionalLightVolumetricScattering(values);
            ApplyDirectionalLightLensFlare(values);
            ApplyScreenSpaceLensFlare(values);
            ApplyVolumetricFog(values);
        }

        private void ApplyDirectionalLightVolumetricScattering(SunRuntimeValues values)
        {
            if (directionalSunLight == null)
            {
                return;
            }

#if BUTO
            var butoLight = directionalSunLight.GetComponent<ButoLight>();
            if (butoLight == null)
            {
                return;
            }

            butoLight.SetInheritance(true);
            butoLight.enabled = values.volumetricFogEnabled;
            butoLight.SetDirty();
#else
            var applied = TrySetMemberValue(directionalSunLight, "useVolumetricScattering", values.volumetricFogEnabled);
            applied |= TrySetMemberValue(directionalSunLight, "useVolumetricLight", values.volumetricFogEnabled);
            applied |= TrySetMemberValue(directionalSunLight, "volumetricScattering", values.volumetricFogEnabled);
            applied |= TrySetMemberValue(directionalSunLight, "volumetricLight", values.volumetricFogEnabled);

            if (!applied)
            {
                return;
            }
#endif
        }

        private void ApplyDirectionalLightLensFlare(SunRuntimeValues values)
        {
            if (directionalSunLight == null)
            {
                return;
            }

            var lensFlareComponent = FindComponentByTypeName(directionalSunLight.gameObject, "UnityEngine.Rendering.LensFlareComponentSRP");
            if (lensFlareComponent == null)
            {
                return;
            }

            if (!TryGetMemberValue(lensFlareComponent, "data", out var lensFlareData) &&
                !TryGetMemberValue(lensFlareComponent, "lensFlareData", out lensFlareData))
            {
                return;
            }

            if (lensFlareData == null || !string.Equals(lensFlareData.GetType().Name, "LensFlareDataSRP", StringComparison.Ordinal))
            {
                return;
            }

            TrySetMemberValue(lensFlareComponent, "intensity", values.sunLensFlareIntensity);
            TrySetMemberValue(lensFlareComponent, "data", lensFlareData);
            TrySetMemberValue(lensFlareComponent, "lensFlareData", lensFlareData);
        }

        private void ApplyScreenSpaceLensFlare(SunRuntimeValues values)
        {
            var profile = GetVolumeProfile();
            if (profile == null)
            {
                return;
            }

            if (profile.TryGet<ScreenSpaceLensFlare>(out var screenSpaceLensFlare))
            {
                screenSpaceLensFlare.active = true;
                screenSpaceLensFlare.intensity.overrideState = true;
                screenSpaceLensFlare.intensity.value = values.screenSpaceLensFlareIntensity;
            }
        }

        private void ApplyVolumetricFog(SunRuntimeValues values)
        {
            var profile = GetVolumeProfile();
            if (profile == null)
            {
                return;
            }

#if BUTO
            if (!profile.TryGet<ButoVolumetricFog>(out var butoFog))
            {
                return;
            }

            var enabled = values.volumetricFogEnabled;
            butoFog.active = true;
            butoFog.mode.overrideState = true;
            butoFog.mode.value = enabled ? VolumetricFogMode.On : VolumetricFogMode.Off;
            butoFog.qualityLevel.overrideState = true;
            butoFog.qualityLevel.value = OccaSoftware.Buto.Runtime.QualityLevel.High;
            butoFog.anisotropy.overrideState = true;
            butoFog.anisotropy.value = Mathf.Clamp(values.volumetricAnisotropy, -1f, 1f);
            butoFog.fogDensity.overrideState = true;
            butoFog.fogDensity.value = enabled ? Mathf.Clamp(1f / Mathf.Max(1f, values.volumetricMeanFreePath), 0f, 5f) : 0f;
            butoFog.baseHeight.overrideState = true;
            butoFog.baseHeight.value = values.volumetricBaseHeight;
            butoFog.maxDistanceVolumetric.overrideState = true;
            butoFog.maxDistanceVolumetric.value = Mathf.Max(10f, values.volumetricMaximumHeight);
            butoFog.attenuationBoundarySize.overrideState = true;
            butoFog.attenuationBoundarySize.value = Mathf.Max(1f, values.volumetricMaximumHeight);
            butoFog.litColor.overrideState = true;
            butoFog.litColor.value = values.fogColor;
            butoFog.shadowedColor.overrideState = true;
            butoFog.shadowedColor.value = new Color(
                values.fogColor.r * 0.35f,
                values.fogColor.g * 0.35f,
                values.fogColor.b * 0.40f,
                values.fogColor.a);
            butoFog.directionalForward.overrideState = true;
            butoFog.directionalForward.value = Color.Lerp(values.fogColor, values.lightColor, 0.45f);
            butoFog.directionalBack.overrideState = true;
            butoFog.directionalBack.value = Color.Lerp(values.fogColor, values.ambientLightColor, 0.45f);
#else
            var volumetricFog = FindVolumeComponentByTypeName(profile, "UnityEngine.Rendering.Universal.VolumetricFog");
            if (volumetricFog == null)
            {
                return;
            }

            TrySetMemberValue(volumetricFog, "active", values.volumetricFogEnabled);
            TrySetMemberValue(volumetricFog, "volumetricFogEnabled", values.volumetricFogEnabled);
            TrySetMemberValue(volumetricFog, "volumetricAnisotropy", values.volumetricAnisotropy);
            TrySetMemberValue(volumetricFog, "volumetricMeanFreePath", values.volumetricMeanFreePath);
            TrySetMemberValue(volumetricFog, "volumetricBaseHeight", values.volumetricBaseHeight);
            TrySetMemberValue(volumetricFog, "volumetricMaximumHeight", values.volumetricMaximumHeight);
#endif
        }

        private VolumeProfile GetVolumeProfile()
        {
            if (globalVolume == null)
            {
                return null;
            }

            if (globalVolume.profile != null)
            {
                return globalVolume.profile;
            }

            return globalVolume.sharedProfile;
        }

        private void EnsurePresetArraySize()
        {
            if (presets == null)
            {
                presets = new SunPresetData[PresetCount];
                return;
            }

            if (presets.Length != PresetCount)
            {
                Array.Resize(ref presets, PresetCount);
            }
        }

        private static object FindComponentByTypeName(GameObject gameObject, string typeName)
        {
            if (gameObject == null)
            {
                return null;
            }

            var components = gameObject.GetComponents<Component>();
            for (var i = 0; i < components.Length; i++)
            {
                var component = components[i];
                if (component == null)
                {
                    continue;
                }

                var componentType = component.GetType();
                if (string.Equals(componentType.FullName, typeName, StringComparison.Ordinal) ||
                    string.Equals(componentType.Name, typeName, StringComparison.Ordinal))
                {
                    return component;
                }
            }

            return null;
        }

        private static object FindVolumeComponentByTypeName(VolumeProfile profile, string typeName)
        {
            if (profile == null || profile.components == null)
            {
                return null;
            }

            for (var i = 0; i < profile.components.Count; i++)
            {
                var component = profile.components[i];
                if (component == null)
                {
                    continue;
                }

                var componentType = component.GetType();
                if (string.Equals(componentType.FullName, typeName, StringComparison.Ordinal) ||
                    string.Equals(componentType.Name, typeName, StringComparison.Ordinal))
                {
                    return component;
                }
            }

            return null;
        }

        private static bool TryGetMemberValue(object target, string memberName, out object value)
        {
            value = null;
            if (target == null)
            {
                return false;
            }

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var type = target.GetType();

            var property = type.GetProperty(memberName, flags);
            if (property != null && property.CanRead)
            {
                value = property.GetValue(target);
                return true;
            }

            var field = type.GetField(memberName, flags);
            if (field != null)
            {
                value = field.GetValue(target);
                return true;
            }

            return false;
        }

        private static bool TrySetMemberValue(object target, string memberName, object value)
        {
            if (target == null)
            {
                return false;
            }

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var type = target.GetType();

            var property = type.GetProperty(memberName, flags);
            if (property != null && property.CanWrite)
            {
                if (TryConvertValue(value, property.PropertyType, out var convertedValue))
                {
                    property.SetValue(target, convertedValue);
                    return true;
                }

                if (TrySetNestedParameter(property.GetValue(target), value))
                {
                    return true;
                }
            }

            var field = type.GetField(memberName, flags);
            if (field != null)
            {
                if (TryConvertValue(value, field.FieldType, out var convertedFieldValue))
                {
                    field.SetValue(target, convertedFieldValue);
                    return true;
                }

                if (TrySetNestedParameter(field.GetValue(target), value))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TrySetNestedParameter(object target, object value)
        {
            if (target == null)
            {
                return false;
            }

            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var type = target.GetType();

            var overrideState = type.GetProperty("overrideState", flags);
            if (overrideState != null && overrideState.CanWrite && overrideState.PropertyType == typeof(bool))
            {
                overrideState.SetValue(target, true);
            }

            var valueProperty = type.GetProperty("value", flags);
            if (valueProperty != null && valueProperty.CanWrite && TryConvertValue(value, valueProperty.PropertyType, out var convertedValue))
            {
                valueProperty.SetValue(target, convertedValue);
                return true;
            }

            return false;
        }

        private static bool TryConvertValue(object value, Type targetType, out object convertedValue)
        {
            convertedValue = null;
            if (targetType == null || value == null)
            {
                return false;
            }

            if (targetType.IsInstanceOfType(value))
            {
                convertedValue = value;
                return true;
            }

            var underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (underlyingType.IsEnum)
            {
                if (value is string enumText)
                {
                    convertedValue = Enum.Parse(underlyingType, enumText);
                    return true;
                }

                convertedValue = Enum.ToObject(underlyingType, value);
                return true;
            }

            if (underlyingType == typeof(bool))
            {
                convertedValue = Convert.ToBoolean(value);
                return true;
            }

            if (underlyingType == typeof(float))
            {
                convertedValue = Convert.ToSingle(value);
                return true;
            }

            if (underlyingType == typeof(double))
            {
                convertedValue = Convert.ToDouble(value);
                return true;
            }

            if (underlyingType == typeof(int))
            {
                convertedValue = Convert.ToInt32(value);
                return true;
            }

            if (underlyingType == typeof(long))
            {
                convertedValue = Convert.ToInt64(value);
                return true;
            }

            if (underlyingType == typeof(string))
            {
                convertedValue = Convert.ToString(value);
                return true;
            }

            if (underlyingType.IsAssignableFrom(value.GetType()))
            {
                convertedValue = value;
                return true;
            }

            return false;
        }

        private struct SunRuntimeValues
        {
            public Quaternion lightRotation;
            public Color lightColor;
            public float lightIntensity;
            public bool useColorTemperature;
            public float colorTemperatureKelvin;
            public Texture cookieTexture;
            public Color cookieTint;
            public float cookieSize;
            public Color skyTint;
            public float skySunSize;
            public float skySunSizeConvergence;
            public Color fogColor;
            public float fogDensity;
            public Color bloomTint;
            public Color ambientLightColor;
            public float readabilityFloor;
            public Color readabilityFloorTint;
            public float readabilityFloorStrength;
            public float windowEmissionStrength;
            public Texture2D colorLookup;
            public float lutContribution;
            public float volumeTemperature;
            public float volumeTint;
            public bool volumetricFogEnabled;
            public float volumetricAnisotropy;
            public float volumetricMeanFreePath;
            public float volumetricBaseHeight;
            public float volumetricMaximumHeight;
            public float screenSpaceLensFlareIntensity;
            public float sunLensFlareIntensity;

            public static SunRuntimeValues FromPresetData(SunPresetData data)
            {
                return new SunRuntimeValues
                {
                    lightRotation = Quaternion.Euler(data.directionEuler),
                    lightColor = data.lightColor,
                    lightIntensity = data.lightIntensity,
                    useColorTemperature = data.useColorTemperature,
                    colorTemperatureKelvin = data.colorTemperatureKelvin,
                    cookieTexture = data.cookieTexture,
                    cookieTint = data.cookieTint,
                    cookieSize = data.cookieSize,
                    skyTint = data.skyTint,
                    skySunSize = data.skySunSize,
                    skySunSizeConvergence = data.skySunSizeConvergence,
                    fogColor = data.fogColor,
                    fogDensity = data.fogDensity,
                    bloomTint = data.bloomTint,
                    ambientLightColor = data.ambientLightColor,
                    readabilityFloor = data.readabilityFloor,
                    readabilityFloorTint = data.readabilityFloorTint,
                    readabilityFloorStrength = data.readabilityFloorStrength,
                    windowEmissionStrength = data.windowEmissionStrength,
                    colorLookup = data.colorLookup,
                    lutContribution = data.lutContribution,
                    volumeTemperature = data.volumeTemperature,
                    volumeTint = data.volumeTint,
                    volumetricFogEnabled = data.volumetricFogEnabled,
                    volumetricAnisotropy = data.volumetricAnisotropy,
                    volumetricMeanFreePath = data.volumetricMeanFreePath,
                    volumetricBaseHeight = data.volumetricBaseHeight,
                    volumetricMaximumHeight = data.volumetricMaximumHeight,
                    screenSpaceLensFlareIntensity = data.screenSpaceLensFlareIntensity,
                    sunLensFlareIntensity = data.sunLensFlareIntensity,
                };
            }

            public static SunRuntimeValues Lerp(SunRuntimeValues from, SunRuntimeValues to, float t)
            {
                var useTargetTexture = t >= 0.5f;
                return new SunRuntimeValues
                {
                    lightRotation = Quaternion.Slerp(from.lightRotation, to.lightRotation, t),
                    lightColor = Color.Lerp(from.lightColor, to.lightColor, t),
                    lightIntensity = Mathf.Lerp(from.lightIntensity, to.lightIntensity, t),
                    useColorTemperature = t >= 0.5f ? to.useColorTemperature : from.useColorTemperature,
                    colorTemperatureKelvin = Mathf.Lerp(from.colorTemperatureKelvin, to.colorTemperatureKelvin, t),
                    cookieTexture = useTargetTexture ? to.cookieTexture : from.cookieTexture,
                    cookieTint = Color.Lerp(from.cookieTint, to.cookieTint, t),
                    cookieSize = Mathf.Lerp(from.cookieSize, to.cookieSize, t),
                    skyTint = Color.Lerp(from.skyTint, to.skyTint, t),
                    skySunSize = Mathf.Lerp(from.skySunSize, to.skySunSize, t),
                    skySunSizeConvergence = Mathf.Lerp(from.skySunSizeConvergence, to.skySunSizeConvergence, t),
                    fogColor = Color.Lerp(from.fogColor, to.fogColor, t),
                    fogDensity = Mathf.Lerp(from.fogDensity, to.fogDensity, t),
                    bloomTint = Color.Lerp(from.bloomTint, to.bloomTint, t),
                    ambientLightColor = Color.Lerp(from.ambientLightColor, to.ambientLightColor, t),
                    readabilityFloor = Mathf.Lerp(from.readabilityFloor, to.readabilityFloor, t),
                    readabilityFloorTint = Color.Lerp(from.readabilityFloorTint, to.readabilityFloorTint, t),
                    readabilityFloorStrength = Mathf.Lerp(from.readabilityFloorStrength, to.readabilityFloorStrength, t),
                    windowEmissionStrength = Mathf.Lerp(from.windowEmissionStrength, to.windowEmissionStrength, t),
                    colorLookup = useTargetTexture ? to.colorLookup : from.colorLookup,
                    lutContribution = Mathf.Lerp(from.lutContribution, to.lutContribution, t),
                    volumeTemperature = Mathf.Lerp(from.volumeTemperature, to.volumeTemperature, t),
                    volumeTint = Mathf.Lerp(from.volumeTint, to.volumeTint, t),
                    volumetricFogEnabled = t >= 0.5f ? to.volumetricFogEnabled : from.volumetricFogEnabled,
                    volumetricAnisotropy = Mathf.Lerp(from.volumetricAnisotropy, to.volumetricAnisotropy, t),
                    volumetricMeanFreePath = Mathf.Lerp(from.volumetricMeanFreePath, to.volumetricMeanFreePath, t),
                    volumetricBaseHeight = Mathf.Lerp(from.volumetricBaseHeight, to.volumetricBaseHeight, t),
                    volumetricMaximumHeight = Mathf.Lerp(from.volumetricMaximumHeight, to.volumetricMaximumHeight, t),
                    screenSpaceLensFlareIntensity = Mathf.Lerp(from.screenSpaceLensFlareIntensity, to.screenSpaceLensFlareIntensity, t),
                    sunLensFlareIntensity = Mathf.Lerp(from.sunLensFlareIntensity, to.sunLensFlareIntensity, t),
                };
            }
        }

#if UNITY_EDITOR
        private const string PresetAssetDirectory = "Assets/Settings/SunCycle";
        private const string CaptureOutputDirectory = "docs/devlog/screenshots/fast_vs_hd2d_phase_a_sun_cycle_runtime_api_cycle165";
        private const string RuntimeApiReportPath = CaptureOutputDirectory + "/sun_cycle_runtime_api_diagnostics.md";
        private const string RuntimeApiStripPath = CaptureOutputDirectory + "/sun_cycle_preset_strip.png";
        private const string PhaseBCaptureOutputDirectory = "docs/devlog/screenshots/fast_vs_hd2d_phase_b_alpha_sun_cycle_runtime_cycle174";
        private const string PhaseBReportFileName = "sun_cycle_runtime_diagnostics.md";
        private const string PhaseBStripFileName = "sun_cycle_preset_strip.png";
        private const int CookieTextureSize = 128;
        private const int DiagnosticStripWidth = 1024;
        private const int DiagnosticStripHeight = 256;

        [MenuItem("Tools/Anemora/Validate HD2D Phase A Sun Cycle Runtime API")]
        public static void ValidateHd2dPhaseASunCycleRuntimeApiBatch()
        {
            EnsurePresetAssets();

            var issues = new List<string>();
            for (var i = 0; i < PresetAssetSpecs.Length; i++)
            {
                ValidatePresetAsset(PresetAssetSpecs[i], issues);
            }

            if (issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D Phase A Sun Cycle runtime API validation failed:\n- " + string.Join("\n- ", issues));
            }

            Debug.Log("HD2D Phase A Sun Cycle runtime API validation passed.");
        }

        [MenuItem("Tools/Anemora/Capture HD2D Phase A Sun Cycle Runtime API Diagnostics")]
        public static void CaptureHd2dPhaseASunCycleRuntimeApiCycle165ScreenshotsBatch()
        {
            ValidateHd2dPhaseASunCycleRuntimeApiBatch();

            Directory.CreateDirectory(GetAbsoluteProjectPath(CaptureOutputDirectory));
            File.WriteAllText(GetAbsoluteProjectPath(RuntimeApiReportPath), BuildRuntimeApiReport(), Encoding.UTF8);
            File.WriteAllBytes(GetAbsoluteProjectPath(RuntimeApiStripPath), BuildPresetDiagnosticStripPng());
            AssetDatabase.Refresh();

            Debug.Log($"HD2D Phase A Sun Cycle diagnostics written: {GetAbsoluteProjectPath(CaptureOutputDirectory)}");
        }

        [MenuItem("Tools/Anemora/Validate HD2D Phase B Alpha Sun Cycle Runtime")]
        public static void ValidateHd2dPhaseBAlphaSunCycleRuntimeBatch()
        {
            EnsurePresetAssets();

            var issues = new List<string>();
            for (var i = 0; i < PresetAssetSpecs.Length; i++)
            {
                ValidatePresetAsset(PresetAssetSpecs[i], issues);
            }

            if (issues.Count > 0)
            {
                throw new InvalidOperationException("HD2D Phase B-alpha Sun Cycle runtime validation failed:\n- " + string.Join("\n- ", issues));
            }

            Debug.Log("HD2D Phase B-alpha Sun Cycle runtime validation passed.");
        }

        [MenuItem("Tools/Anemora/Capture HD2D Phase B Alpha Sun Cycle Runtime Cycle 174 Screenshots")]
        public static void CaptureHd2dPhaseBAlphaSunCycleRuntimeCycle174ScreenshotsBatch()
        {
            ValidateHd2dPhaseBAlphaSunCycleRuntimeBatch();

            Directory.CreateDirectory(GetAbsoluteProjectPath(PhaseBCaptureOutputDirectory));
            File.WriteAllText(GetAbsoluteProjectPath($"{PhaseBCaptureOutputDirectory}/{GetCaptureFileName(PhaseBReportFileName)}"), BuildPhaseBAlphaRuntimeReport(), Encoding.UTF8);
            File.WriteAllBytes(GetAbsoluteProjectPath($"{PhaseBCaptureOutputDirectory}/{GetCaptureFileName(PhaseBStripFileName)}"), BuildPresetDiagnosticStripPng());
            AssetDatabase.Refresh();

            Debug.Log($"HD2D Phase B-alpha Sun Cycle diagnostics written: {GetAbsoluteProjectPath(PhaseBCaptureOutputDirectory)}");
        }

        public static void EnsureHd2dSunPresetAssetsForEditor()
        {
            EnsurePresetAssets();
        }

        private static void EnsurePresetAssets()
        {
            EnsureFolder(PresetAssetDirectory);

            for (var i = 0; i < PresetAssetSpecs.Length; i++)
            {
                EnsurePresetAsset(PresetAssetSpecs[i]);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void EnsurePresetAsset(PresetAssetSpec spec)
        {
            var asset = AssetDatabase.LoadAssetAtPath<SunPresetData>(spec.AssetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<SunPresetData>();
                asset.name = $"SunPreset_{spec.Preset}";
                AssetDatabase.CreateAsset(asset, spec.AssetPath);
            }

            asset.preset = spec.Preset;
            asset.directionEuler = spec.DirectionEuler;
            asset.lightColor = spec.LightColor;
            asset.lightIntensity = spec.LightIntensity;
            asset.useColorTemperature = spec.UseColorTemperature;
            asset.colorTemperatureKelvin = spec.ColorTemperatureKelvin;
            asset.cookieTint = spec.CookieTint;
            asset.cookieSize = spec.CookieSize;
            asset.skyTint = spec.SkyTint;
            asset.skySunSize = spec.SkySunSize;
            asset.skySunSizeConvergence = spec.SkySunSizeConvergence;
            asset.fogColor = spec.FogColor;
            asset.fogDensity = spec.FogDensity;
            asset.bloomTint = spec.BloomTint;
            asset.ambientLightColor = spec.AmbientLightColor;
            asset.readabilityFloor = spec.ReadabilityFloor;
            asset.readabilityFloorTint = spec.ReadabilityFloorTint;
            asset.readabilityFloorStrength = spec.ReadabilityFloorStrength;
            asset.windowEmissionStrength = spec.WindowEmissionStrength;
            asset.colorLookup = AssetDatabase.LoadAssetAtPath<Texture2D>(spec.LutPath);
            asset.lutContribution = spec.LutContribution;
            asset.volumeTemperature = spec.VolumeTemperature;
            asset.volumeTint = spec.VolumeTint;
            asset.volumetricFogEnabled = spec.VolumetricFogEnabled;
            asset.volumetricAnisotropy = spec.VolumetricAnisotropy;
            asset.volumetricMeanFreePath = spec.VolumetricMeanFreePath;
            asset.volumetricBaseHeight = spec.VolumetricBaseHeight;
            asset.volumetricMaximumHeight = spec.VolumetricMaximumHeight;
            asset.screenSpaceLensFlareIntensity = spec.ScreenSpaceLensFlareIntensity;
            asset.sunLensFlareIntensity = spec.SunLensFlareIntensity;
            asset.cookieTexture = EnsureCookieSubAsset(spec, asset);

            EditorUtility.SetDirty(asset);
        }

        private static Texture2D EnsureCookieSubAsset(PresetAssetSpec spec, SunPresetData owner)
        {
            var cookieName = $"SunCookie_{spec.Preset}";
            var path = AssetDatabase.GetAssetPath(owner);
            var subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);

            for (var i = 0; i < subAssets.Length; i++)
            {
                if (subAssets[i] is Texture2D existing && string.Equals(existing.name, cookieName, StringComparison.Ordinal))
                {
                    ApplyCookiePixels(existing, spec);
                    EditorUtility.SetDirty(existing);
                    return existing;
                }
            }

            var cookie = new Texture2D(CookieTextureSize, CookieTextureSize, TextureFormat.RGBA32, false, true)
            {
                name = cookieName,
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
            };

            ApplyCookiePixels(cookie, spec);
            AssetDatabase.AddObjectToAsset(cookie, owner);
            EditorUtility.SetDirty(cookie);
            return cookie;
        }

        private static void ApplyCookiePixels(Texture2D texture, PresetAssetSpec spec)
        {
            if (texture.width != CookieTextureSize || texture.height != CookieTextureSize)
            {
                texture.Reinitialize(CookieTextureSize, CookieTextureSize, TextureFormat.RGBA32, false);
            }

            for (var y = 0; y < CookieTextureSize; y++)
            {
                var v = y / (float)(CookieTextureSize - 1);
                for (var x = 0; x < CookieTextureSize; x++)
                {
                    var u = x / (float)(CookieTextureSize - 1);
                    texture.SetPixel(x, y, SampleCookieColor(u, v, spec));
                }
            }

            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.Apply(false, false);
        }

        private static Color SampleCookieColor(float u, float v, PresetAssetSpec spec)
        {
            var centerU = u - 0.5f;
            var centerV = v - 0.5f;
            var radius = Mathf.Sqrt((centerU * centerU) + (centerV * centerV));
            var vignette = 1f - Mathf.SmoothStep(0.25f, 0.72f, radius);
            var diagonalA = Mathf.SmoothStep(0.0f, 0.07f, Mathf.Abs((u * 0.95f) + (v * 0.62f) - 0.72f));
            var diagonalB = Mathf.SmoothStep(0.0f, 0.055f, Mathf.Abs((u * -0.80f) + (v * 0.90f) - 0.08f));
            var streakMask = 1f - Mathf.Min(diagonalA, diagonalB);
            var cell = Mathf.PerlinNoise((u * 7.0f) + ((int)spec.Preset * 3.1f), (v * 6.0f) + 1.7f);
            var luma = Mathf.Clamp01(Mathf.Lerp(0.72f, 1.0f, cell) * Mathf.Lerp(0.88f, 1.08f, vignette) + (streakMask * 0.18f));
            var tint = spec.CookieTint;
            return new Color(tint.r * luma, tint.g * luma, tint.b * luma, 1f);
        }

        private static void ValidatePresetAsset(PresetAssetSpec spec, List<string> issues)
        {
            var asset = AssetDatabase.LoadAssetAtPath<SunPresetData>(spec.AssetPath);
            if (asset == null)
            {
                issues.Add($"Missing preset asset: {spec.AssetPath}");
                return;
            }

            if (asset.preset != spec.Preset)
            {
                issues.Add($"{spec.AssetPath} preset must be {spec.Preset}, found {asset.preset}.");
            }

            RequireVector3(asset.directionEuler, spec.DirectionEuler, 0.001f, issues, $"{spec.Preset} directionEuler");
            RequireColor(asset.lightColor, spec.LightColor, 0.001f, issues, $"{spec.Preset} lightColor");
            RequireFloat(asset.lightIntensity, spec.LightIntensity, 0.001f, issues, $"{spec.Preset} lightIntensity");
            RequireBool(asset.useColorTemperature, spec.UseColorTemperature, issues, $"{spec.Preset} useColorTemperature");
            RequireFloat(asset.colorTemperatureKelvin, spec.ColorTemperatureKelvin, 0.001f, issues, $"{spec.Preset} colorTemperatureKelvin");
            RequireColor(asset.cookieTint, spec.CookieTint, 0.001f, issues, $"{spec.Preset} cookieTint");
            RequireFloat(asset.cookieSize, spec.CookieSize, 0.001f, issues, $"{spec.Preset} cookieSize");
            RequireColor(asset.skyTint, spec.SkyTint, 0.001f, issues, $"{spec.Preset} skyTint");
            RequireFloat(asset.skySunSize, spec.SkySunSize, 0.001f, issues, $"{spec.Preset} skySunSize");
            RequireFloat(asset.skySunSizeConvergence, spec.SkySunSizeConvergence, 0.001f, issues, $"{spec.Preset} skySunSizeConvergence");
            RequireColor(asset.fogColor, spec.FogColor, 0.001f, issues, $"{spec.Preset} fogColor");
            RequireFloat(asset.fogDensity, spec.FogDensity, 0.001f, issues, $"{spec.Preset} fogDensity");
            RequireColor(asset.bloomTint, spec.BloomTint, 0.001f, issues, $"{spec.Preset} bloomTint");
            RequireColor(asset.ambientLightColor, spec.AmbientLightColor, 0.001f, issues, $"{spec.Preset} ambientLightColor");
            RequireFloat(asset.readabilityFloor, spec.ReadabilityFloor, 0.001f, issues, $"{spec.Preset} readabilityFloor");
            RequireColor(asset.readabilityFloorTint, spec.ReadabilityFloorTint, 0.001f, issues, $"{spec.Preset} readabilityFloorTint");
            RequireFloat(asset.readabilityFloorStrength, spec.ReadabilityFloorStrength, 0.001f, issues, $"{spec.Preset} readabilityFloorStrength");
            RequireFloat(asset.windowEmissionStrength, spec.WindowEmissionStrength, 0.001f, issues, $"{spec.Preset} windowEmissionStrength");
            RequireFloat(asset.lutContribution, spec.LutContribution, 0.001f, issues, $"{spec.Preset} lutContribution");
            RequireFloat(asset.volumeTemperature, spec.VolumeTemperature, 0.001f, issues, $"{spec.Preset} volumeTemperature");
            RequireFloat(asset.volumeTint, spec.VolumeTint, 0.001f, issues, $"{spec.Preset} volumeTint");
            RequireBool(asset.volumetricFogEnabled, spec.VolumetricFogEnabled, issues, $"{spec.Preset} volumetricFogEnabled");
            RequireFloat(asset.volumetricAnisotropy, spec.VolumetricAnisotropy, 0.001f, issues, $"{spec.Preset} volumetricAnisotropy");
            RequireFloat(asset.volumetricMeanFreePath, spec.VolumetricMeanFreePath, 0.001f, issues, $"{spec.Preset} volumetricMeanFreePath");
            RequireFloat(asset.volumetricBaseHeight, spec.VolumetricBaseHeight, 0.001f, issues, $"{spec.Preset} volumetricBaseHeight");
            RequireFloat(asset.volumetricMaximumHeight, spec.VolumetricMaximumHeight, 0.001f, issues, $"{spec.Preset} volumetricMaximumHeight");
            RequireFloat(asset.screenSpaceLensFlareIntensity, spec.ScreenSpaceLensFlareIntensity, 0.001f, issues, $"{spec.Preset} screenSpaceLensFlareIntensity");
            RequireFloat(asset.sunLensFlareIntensity, spec.SunLensFlareIntensity, 0.001f, issues, $"{spec.Preset} sunLensFlareIntensity");

            if (asset.cookieTexture == null)
            {
                issues.Add($"{spec.Preset} cookieTexture must be generated as a sub-asset.");
            }

            var lut = AssetDatabase.LoadAssetAtPath<Texture2D>(spec.LutPath);
            if (lut == null)
            {
                issues.Add($"{spec.Preset} LUT is missing: {spec.LutPath}");
            }
            else if (asset.colorLookup != lut)
            {
                issues.Add($"{spec.Preset} colorLookup must reference {spec.LutPath}.");
            }
            else if (lut.width != 1024 || lut.height != 32)
            {
                issues.Add($"{spec.Preset} LUT must be 1024x32, found {lut.width}x{lut.height}: {spec.LutPath}");
            }
        }

        private static string BuildRuntimeApiReport()
        {
            var builder = new StringBuilder();
            builder.AppendLine("# HD2D Phase A Sun Cycle Runtime API Diagnostics");
            builder.AppendLine();
            builder.AppendLine("- Authored runtime file: `Assets/Scripts/FastVS/SunCycle/AnemoraSunCycleDriver.cs`");
            builder.AppendLine("- Preset assets: `Assets/Settings/SunCycle/SunPreset_<Name>.asset`");
            builder.AppendLine("- Static validate entry: `Anemora.FastVS.SunCycle.AnemoraSunCycleDriver.ValidateHd2dPhaseASunCycleRuntimeApiBatch`");
            builder.AppendLine("- Static capture entry: `Anemora.FastVS.SunCycle.AnemoraSunCycleDriver.CaptureHd2dPhaseASunCycleRuntimeApiCycle165ScreenshotsBatch`");
            builder.AppendLine();
            builder.AppendLine("| Preset | Asset | Direction Euler | Intensity | Kelvin | Readability Floor | Window Emission | LUT | LUT Contribution | White Balance |");
            builder.AppendLine("|---|---|---:|---:|---:|---:|---:|---|---:|---:|");

            for (var i = 0; i < PresetAssetSpecs.Length; i++)
            {
                var spec = PresetAssetSpecs[i];
                builder.AppendLine(
                    $"| {spec.Preset} | `{spec.AssetPath}` | {FormatVector3(spec.DirectionEuler)} | {spec.LightIntensity:0.###} | {(spec.UseColorTemperature ? spec.ColorTemperatureKelvin.ToString("0") : "off")} | {spec.ReadabilityFloor:0.###} / {spec.ReadabilityFloorStrength:0.###} | {spec.WindowEmissionStrength:0.###} | `{spec.LutPath}` | {spec.LutContribution:0.###} | temp {spec.VolumeTemperature:0.###}, tint {spec.VolumeTint:0.###} |");
            }

            return builder.ToString();
        }

        private static byte[] BuildPresetDiagnosticStripPng()
        {
            var texture = new Texture2D(DiagnosticStripWidth, DiagnosticStripHeight, TextureFormat.RGBA32, false, true)
            {
                name = "SunCyclePresetDiagnosticStrip",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp,
            };

            var segmentWidth = DiagnosticStripWidth / PresetAssetSpecs.Length;
            for (var i = 0; i < PresetAssetSpecs.Length; i++)
            {
                PaintDiagnosticSegment(texture, PresetAssetSpecs[i], i * segmentWidth, segmentWidth);
            }

            texture.Apply(false, false);
            var bytes = texture.EncodeToPNG();
            DestroyImmediate(texture);
            return bytes;
        }

        private static void PaintDiagnosticSegment(Texture2D texture, PresetAssetSpec spec, int xStart, int width)
        {
            for (var y = 0; y < texture.height; y++)
            {
                var v = y / (float)(texture.height - 1);
                for (var x = 0; x < width; x++)
                {
                    var u = x / (float)(width - 1);
                    var lightBand = Mathf.SmoothStep(0.18f, 0.98f, u);
                    var skyFogBlend = Mathf.SmoothStep(0.35f, 0.92f, v);
                    var skyFog = Color.Lerp(spec.FogColor, spec.SkyTint, skyFogBlend);
                    var keyColor = EvaluateKeyLightColor(spec.LightColor, spec.UseColorTemperature, spec.ColorTemperatureKelvin);
                    var color = Color.Lerp(skyFog, keyColor, lightBand * 0.42f);

                    var sunDir = Quaternion.Euler(spec.DirectionEuler) * Vector3.forward;
                    var diagonal = Mathf.Abs(((u - 0.5f) * sunDir.x) + ((v - 0.5f) * Mathf.Max(0.2f, Mathf.Abs(sunDir.y))));
                    var beam = 1f - Mathf.SmoothStep(0.015f, 0.105f, diagonal);
                    color = Color.Lerp(color, spec.BloomTint, beam * 0.30f);
                    texture.SetPixel(xStart + x, y, new Color(color.r, color.g, color.b, 1f));
                }
            }
        }

        private static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }

            var parts = path.Split('/');
            var current = parts[0];
            for (var i = 1; i < parts.Length; i++)
            {
                var next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }

        private static void RequireFloat(float actual, float expected, float tolerance, List<string> issues, string label)
        {
            if (Mathf.Abs(actual - expected) > tolerance)
            {
                issues.Add($"{label} must be {expected:0.###}, found {actual:0.###}.");
            }
        }

        private static void RequireBool(bool actual, bool expected, List<string> issues, string label)
        {
            if (actual != expected)
            {
                issues.Add($"{label} must be {expected}, found {actual}.");
            }
        }

        private static void RequireVector3(Vector3 actual, Vector3 expected, float tolerance, List<string> issues, string label)
        {
            if (Vector3.Distance(actual, expected) > tolerance)
            {
                issues.Add($"{label} must be {FormatVector3(expected)}, found {FormatVector3(actual)}.");
            }
        }

        private static void RequireColor(Color actual, Color expected, float tolerance, List<string> issues, string label)
        {
            if (Mathf.Abs(actual.r - expected.r) > tolerance ||
                Mathf.Abs(actual.g - expected.g) > tolerance ||
                Mathf.Abs(actual.b - expected.b) > tolerance ||
                Mathf.Abs(actual.a - expected.a) > tolerance)
            {
                issues.Add($"{label} must be {FormatColor(expected)}, found {FormatColor(actual)}.");
            }
        }

        private static string FormatVector3(Vector3 value)
        {
            return $"({value.x:0.###}, {value.y:0.###}, {value.z:0.###})";
        }

        private static string FormatColor(Color value)
        {
            return $"({value.r:0.###}, {value.g:0.###}, {value.b:0.###}, {value.a:0.###})";
        }

        private static string GetCaptureFileName(string fileName)
        {
            var audience = Environment.GetEnvironmentVariable("CYCLE_AUDIENCE");
            if (string.IsNullOrWhiteSpace(audience))
            {
                return fileName;
            }

            return $"{audience}_{fileName}";
        }

        private static string GetAbsoluteProjectPath(string relativePath)
        {
            return Path.GetFullPath(Path.Combine(Application.dataPath, "..", relativePath));
        }

        private readonly struct PresetAssetSpec
        {
            public PresetAssetSpec(
                SunPreset preset,
                Vector3 directionEuler,
                Color lightColor,
                float lightIntensity,
                bool useColorTemperature,
                float colorTemperatureKelvin,
                Color cookieTint,
                float cookieSize,
                Color skyTint,
                float skySunSize,
                float skySunSizeConvergence,
                Color fogColor,
                float fogDensity,
                Color bloomTint,
                Color ambientLightColor,
                float readabilityFloor,
                Color readabilityFloorTint,
                float readabilityFloorStrength,
                float windowEmissionStrength,
                string lutPath,
                float lutContribution,
                float volumeTemperature,
                float volumeTint,
                bool volumetricFogEnabled,
                float volumetricAnisotropy,
                float volumetricMeanFreePath,
                float volumetricBaseHeight,
                float volumetricMaximumHeight,
                float screenSpaceLensFlareIntensity,
                float sunLensFlareIntensity)
            {
                Preset = preset;
                DirectionEuler = directionEuler;
                LightColor = lightColor;
                LightIntensity = lightIntensity;
                UseColorTemperature = useColorTemperature;
                ColorTemperatureKelvin = colorTemperatureKelvin;
                CookieTint = cookieTint;
                CookieSize = cookieSize;
                SkyTint = skyTint;
                SkySunSize = skySunSize;
                SkySunSizeConvergence = skySunSizeConvergence;
                FogColor = fogColor;
                FogDensity = fogDensity;
                BloomTint = bloomTint;
                AmbientLightColor = ambientLightColor;
                ReadabilityFloor = readabilityFloor;
                ReadabilityFloorTint = readabilityFloorTint;
                ReadabilityFloorStrength = readabilityFloorStrength;
                WindowEmissionStrength = windowEmissionStrength;
                LutPath = lutPath;
                LutContribution = lutContribution;
                VolumeTemperature = volumeTemperature;
                VolumeTint = volumeTint;
                VolumetricFogEnabled = volumetricFogEnabled;
                VolumetricAnisotropy = volumetricAnisotropy;
                VolumetricMeanFreePath = volumetricMeanFreePath;
                VolumetricBaseHeight = volumetricBaseHeight;
                VolumetricMaximumHeight = volumetricMaximumHeight;
                ScreenSpaceLensFlareIntensity = screenSpaceLensFlareIntensity;
                SunLensFlareIntensity = sunLensFlareIntensity;
            }

            public SunPreset Preset { get; }
            public Vector3 DirectionEuler { get; }
            public Color LightColor { get; }
            public float LightIntensity { get; }
            public bool UseColorTemperature { get; }
            public float ColorTemperatureKelvin { get; }
            public Color CookieTint { get; }
            public float CookieSize { get; }
            public Color SkyTint { get; }
            public float SkySunSize { get; }
            public float SkySunSizeConvergence { get; }
            public Color FogColor { get; }
            public float FogDensity { get; }
            public Color BloomTint { get; }
            public Color AmbientLightColor { get; }
            public float ReadabilityFloor { get; }
            public Color ReadabilityFloorTint { get; }
            public float ReadabilityFloorStrength { get; }
            public float WindowEmissionStrength { get; }
            public string LutPath { get; }
            public float LutContribution { get; }
            public float VolumeTemperature { get; }
            public float VolumeTint { get; }
            public bool VolumetricFogEnabled { get; }
            public float VolumetricAnisotropy { get; }
            public float VolumetricMeanFreePath { get; }
            public float VolumetricBaseHeight { get; }
            public float VolumetricMaximumHeight { get; }
            public float ScreenSpaceLensFlareIntensity { get; }
            public float SunLensFlareIntensity { get; }
            public string AssetPath => $"{PresetAssetDirectory}/SunPreset_{Preset}.asset";
        }

        private static readonly PresetAssetSpec[] PresetAssetSpecs =
        {
            new PresetAssetSpec(
                SunPreset.Morning,
                new Vector3(28f, -128f, 0f),
                new Color(1.00f, 0.84f, 0.70f, 1f),
                1.5f,
                true,
                3400f,
                new Color(0.99f, 0.91f, 0.78f, 1f),
                10.5f,
                new Color(0.84f, 0.71f, 0.54f, 1f),
                0.042f,
                5f,
                new Color(0.91f, 0.77f, 0.59f, 1f),
                0.011f,
                new Color(0.99f, 0.91f, 0.78f, 1f),
                new Color(0.54f, 0.49f, 0.41f, 1f),
                0.050f,
                new Color(0.20f, 0.22f, 0.28f, 1f),
                0.26f,
                0.25f,
                "Assets/Art/LUT/LUT_Morning_Warm.png",
                0.58f,
                12f,
                0f,
                true,
                0.6f,
                100f,
                0f,
                30f,
                0.4f,
                0.48f),
            new PresetAssetSpec(
                SunPreset.Noon,
                new Vector3(50f, -78f, 0f),
                new Color(0.86f, 0.84f, 0.78f, 1f),
                0.90f,
                true,
                6200f,
                new Color(0.84f, 0.82f, 0.78f, 1f),
                10.2f,
                new Color(0.56f, 0.70f, 0.86f, 1f),
                0.036f,
                8f,
                new Color(0.64f, 0.70f, 0.76f, 1f),
                0.001f,
                new Color(0.76f, 0.74f, 0.68f, 1f),
                new Color(0.26f, 0.29f, 0.32f, 1f),
                0.050f,
                new Color(0.22f, 0.23f, 0.26f, 1f),
                0.20f,
                0.05f,
                "Assets/Art/LUT/LUT_Daylight.png",
                0.32f,
                -2f,
                -4f,
                true,
                0.6f,
                100f,
                0f,
                30f,
                0.4f,
                0.25f),
            new PresetAssetSpec(
                SunPreset.Evening,
                new Vector3(24f, -28f, 0f),
                new Color(1.00f, 0.62f, 0.42f, 1f),
                1.55f,
                true,
                3000f,
                new Color(1.00f, 0.71f, 0.48f, 1f),
                11.5f,
                new Color(0.91f, 0.56f, 0.43f, 1f),
                0.050f,
                4f,
                new Color(0.79f, 0.53f, 0.43f, 1f),
                0.017f,
                new Color(1.00f, 0.71f, 0.48f, 1f),
                new Color(0.49f, 0.38f, 0.32f, 1f),
                0.055f,
                new Color(0.18f, 0.22f, 0.30f, 1f),
                0.36f,
                0.85f,
                "Assets/Art/LUT/LUT_GoldenHour.png",
                0.66f,
                16f,
                3f,
                true,
                0.6f,
                100f,
                0f,
                30f,
                0.4f,
                0.72f),
            new PresetAssetSpec(
                SunPreset.Night,
                new Vector3(-32f, 12f, 0f),
                new Color(0.42f, 0.55f, 0.72f, 1f),
                0.40f,
                true,
                8000f,
                new Color(0.55f, 0.65f, 0.85f, 1f),
                13.0f,
                new Color(0.10f, 0.12f, 0.22f, 1f),
                0.020f,
                10f,
                new Color(0.18f, 0.22f, 0.30f, 1f),
                0.022f,
                new Color(0.78f, 0.82f, 1.00f, 1f),
                new Color(0.18f, 0.22f, 0.28f, 1f),
                0.065f,
                new Color(0.16f, 0.22f, 0.34f, 1f),
                0.55f,
                1.15f,
                "Assets/Art/LUT/LUT_Night_CoolBlue.png",
                0.7f,
                -12f,
                -4f,
                true,
                0.6f,
                100f,
                0f,
                30f,
                0.4f,
                0.26f),
        };

        private static string BuildPhaseBAlphaRuntimeReport()
        {
            var builder = new StringBuilder();
            builder.AppendLine("# HD2D Phase B-alpha Sun Cycle Runtime Diagnostics");
            builder.AppendLine();
            builder.AppendLine("- Authored runtime file: `Assets/Scripts/FastVS/SunCycle/AnemoraSunCycleDriver.cs`");
            builder.AppendLine("- Preset assets: `Assets/Settings/SunCycle/SunPreset_<Name>.asset`");
            builder.AppendLine("- Static validate entry: `Anemora.FastVS.SunCycle.AnemoraSunCycleDriver.ValidateHd2dPhaseBAlphaSunCycleRuntimeBatch`");
            builder.AppendLine("- Static capture entry: `Anemora.FastVS.SunCycle.AnemoraSunCycleDriver.CaptureHd2dPhaseBAlphaSunCycleRuntimeCycle174ScreenshotsBatch`");
            builder.AppendLine();
            builder.AppendLine("| Preset | Asset | Direction Euler | Intensity | Kelvin | Volumetric | Screen Flare | Sun Flare | LUT | LUT Contribution | White Balance |");
            builder.AppendLine("|---|---|---:|---:|---:|---|---:|---:|---|---:|---:|");

            for (var i = 0; i < PresetAssetSpecs.Length; i++)
            {
                var spec = PresetAssetSpecs[i];
                builder.AppendLine(
                    $"| {spec.Preset} | `{spec.AssetPath}` | {FormatVector3(spec.DirectionEuler)} | {spec.LightIntensity:0.###} | {(spec.UseColorTemperature ? spec.ColorTemperatureKelvin.ToString("0") : "off")} | {(spec.VolumetricFogEnabled ? "enabled" : "disabled")}, aniso {spec.VolumetricAnisotropy:0.###}, mfp {spec.VolumetricMeanFreePath:0.###}, base {spec.VolumetricBaseHeight:0.###}, max {spec.VolumetricMaximumHeight:0.###} | {spec.ScreenSpaceLensFlareIntensity:0.###} | {spec.SunLensFlareIntensity:0.###} | `{spec.LutPath}` | {spec.LutContribution:0.###} | temp {spec.VolumeTemperature:0.###}, tint {spec.VolumeTint:0.###} |");
            }

            return builder.ToString();
        }

#endif
    }

}
