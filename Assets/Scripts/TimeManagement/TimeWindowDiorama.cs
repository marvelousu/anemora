using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Anemora.Audio;
using Anemora.TimeManagement.Reflectors;
using UnityEngine;

namespace Anemora.TimeManagement
{
    public sealed class TimeWindowDiorama : MonoBehaviour
    {
        [SerializeField] private Transform scalableRoot;
        [SerializeField] private Transform contentRoot;
        [SerializeField] private Vector2 size = new Vector2(2.4f, 2f);
        [SerializeField] private Transform player;
        [SerializeField] private bool gateInteractionsUntilPlayerInside = true;
        [SerializeField] private string sourceRootName = "Root_Past";
        [SerializeField] private string currentRootName = "Root_Current";
        [SerializeField] private int visibleLayer = 10;
        [SerializeField] private bool replaceCurrentSpace = true;
        [SerializeField] private bool showSpaceVeil = true;
        [SerializeField] private Material spaceVeilMaterial;
        [SerializeField] private Color spaceVeilColor = new Color(0.18f, 0.64f, 1f, 0.16f);
        [SerializeField] private Color pastContentTint = new Color(0.62f, 0.78f, 0.98f, 1f);
        [SerializeField, Range(0f, 1f)] private float pastContentTintStrength = 1f;
        [SerializeField] private Color pastEmissionTint = new Color(0.14f, 0.42f, 0.78f, 1f);
        [SerializeField, Range(0f, 1f)] private float pastEmissionStrength = 0.18f;
        [SerializeField] private float veilHeight = 1.55f;
        [SerializeField] private float boundsPadding = 0.12f;
        [SerializeField] private float floorOverlayLift = 0.025f;
        [SerializeField] private float openAnimationDuration = 0.18f;
        [SerializeField] private float closeAnimationDuration = 0.14f;
        [SerializeField, Range(0.01f, 0.5f)] private float animationStartScale = 0.08f;

        private readonly List<GameObject> spawnedContent = new();
        private readonly List<GameObject> spawnedVeil = new();
        private readonly Dictionary<Renderer, float> veilAlphaMultipliers = new();
        private readonly List<Renderer> hiddenCurrentRenderers = new();
        private MonoBehaviour[] gatedInteractables;
        private Coroutine visualAnimationRoutine;
        private bool playerInside;
        private bool closing;

        public Vector2 Size => size;
        public bool IsPlayerInside => playerInside;
        public int RuntimeContentCount => spawnedContent.Count;
        public int RuntimeVeilCount => spawnedVeil.Count;

        private void Awake()
        {
            ResolveGatedInteractables();
            SetGatedInteractablesEnabled(!gateInteractionsUntilPlayerInside || playerInside);
        }

        private void OnDestroy()
        {
            if (playerInside)
            {
                ActionRecordRuntime.Instance?.ReflectUnreflected();
            }

            ClearSpawnedContent();
            ClearSpaceVeil();
            RestoreHiddenCurrentRenderers();
        }

        private void Update()
        {
            if (closing)
            {
                return;
            }

            if (player == null)
            {
                var playerObject = GameObject.FindWithTag("Player");
                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }

            var inside = player != null && ContainsWorldPosition(player.position);
            if (inside == playerInside)
            {
                return;
            }

            playerInside = inside;
            SetGatedInteractablesEnabled(!gateInteractionsUntilPlayerInside || playerInside);
            if (inside)
            {
                Zone1AudioController.Instance?.PlayTimePortalFlip();
            }
            else
            {
                ActionRecordRuntime.Instance?.ReflectUnreflected();
            }
        }

        public void Configure(Vector3 center, Vector2 windowSize, Transform playerTransform)
        {
            Configure(center, windowSize, playerTransform, transform.rotation);
        }

        public void Configure(Vector3 center, Vector2 windowSize, Transform playerTransform, Quaternion rotation)
        {
            closing = false;
            transform.localScale = Vector3.one;
            transform.position = center;
            transform.rotation = rotation;
            size = new Vector2(Mathf.Max(0.75f, windowSize.x), Mathf.Max(0.75f, windowSize.y));
            player = playerTransform;

            if (scalableRoot != null)
            {
                scalableRoot.localScale = new Vector3(size.x, 1f, size.y);
            }

            RebuildWorldAlignedContent();
            HideCurrentContentInVolume();
            RebuildSpaceVeil();
            playerInside = player != null && ContainsWorldPosition(player.position);
            ResolveGatedInteractables();
            SetGatedInteractablesEnabled(!gateInteractionsUntilPlayerInside || playerInside);
            PlayOpenAnimation();
        }

        public bool ContainsWorldPosition(Vector3 worldPosition)
        {
            var local = transform.InverseTransformPoint(worldPosition);
            return Mathf.Abs(local.x) <= size.x * 0.5f &&
                   Mathf.Abs(local.z) <= size.y * 0.5f;
        }

        private void RebuildWorldAlignedContent()
        {
            ClearSpawnedContent();
            RestoreHiddenCurrentRenderers();

            var sourceRoot = GameObject.Find(sourceRootName);
            if (sourceRoot == null)
            {
                return;
            }

            var destination = ResolveContentRoot();
            var cloneRoots = sourceRoot
                .GetComponentsInChildren<Renderer>(true)
                .Where(renderer => renderer != null && renderer.enabled && renderer.gameObject.activeInHierarchy)
                .Where(RendererIntersectsVolume)
                .Select(renderer => ResolveCloneRoot(renderer.transform, sourceRoot.transform))
                .Where(root => root != null && root.gameObject != gameObject)
                .Where(root => !IsExcludedSourceRoot(root))
                .Where(root => !IsFloorLikeObject(root))
                .Distinct()
                .OrderBy(root => root.name)
                .ToArray();

            foreach (var cloneRoot in cloneRoots)
            {
                var clone = Instantiate(cloneRoot.gameObject, destination, true);
                clone.name = $"TimeVolume_{cloneRoot.name}";
                SetLayerRecursively(clone, visibleLayer);
                RemoveCollidersRecursively(clone);
                ConfigureClonedInteractables(clone);
                ApplyPastSpaceTint(clone);
                spawnedContent.Add(clone);
            }
        }

        private void RebuildSpaceVeil()
        {
            ClearSpaceVeil();
            if (!showSpaceVeil)
            {
                return;
            }

            var material = ResolveVeilMaterial();
            if (material == null)
            {
                return;
            }

            var height = Mathf.Max(0.4f, veilHeight);
            var halfHeight = height * 0.5f;
            var panelThickness = 0.018f;
            var footprintHeight = 0.012f;
            var rimThickness = 0.045f;

            CreateVeilPanel("TimeVolume_SpaceVeil_Footprint", new Vector3(0f, floorOverlayLift + footprintHeight * 0.5f, 0f), new Vector3(size.x, footprintHeight, size.y), material, 0.95f);
            CreateVeilPanel("TimeVolume_SpaceVeil_Left", new Vector3(-size.x * 0.5f, halfHeight, 0f), new Vector3(panelThickness, height, size.y), material, 0.5f);
            CreateVeilPanel("TimeVolume_SpaceVeil_Right", new Vector3(size.x * 0.5f, halfHeight, 0f), new Vector3(panelThickness, height, size.y), material, 0.5f);
            CreateVeilPanel("TimeVolume_SpaceVeil_Back", new Vector3(0f, halfHeight, size.y * 0.5f), new Vector3(size.x, height, panelThickness), material, 0.5f);
            CreateVeilPanel("TimeVolume_SpaceVeil_FloorEdge_Front", new Vector3(0f, 0.065f, -size.y * 0.5f), new Vector3(size.x, rimThickness, rimThickness), material, 1.55f);
            CreateVeilPanel("TimeVolume_SpaceVeil_FloorEdge_Back", new Vector3(0f, 0.065f, size.y * 0.5f), new Vector3(size.x, rimThickness, rimThickness), material, 1.55f);
            CreateVeilPanel("TimeVolume_SpaceVeil_FloorEdge_Left", new Vector3(-size.x * 0.5f, 0.065f, 0f), new Vector3(rimThickness, rimThickness, size.y), material, 1.55f);
            CreateVeilPanel("TimeVolume_SpaceVeil_FloorEdge_Right", new Vector3(size.x * 0.5f, 0.065f, 0f), new Vector3(rimThickness, rimThickness, size.y), material, 1.55f);
        }

        private void CreateVeilPanel(string objectName, Vector3 localPosition, Vector3 localScale, Material material, float alphaMultiplier)
        {
            var panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            panel.name = objectName;
            panel.transform.SetParent(transform, false);
            panel.transform.localPosition = localPosition;
            panel.transform.localRotation = Quaternion.identity;
            panel.transform.localScale = localScale;
            SetLayerRecursively(panel, visibleLayer);
            RemoveCollidersRecursively(panel);

            var renderer = panel.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = material;
                veilAlphaMultipliers[renderer] = alphaMultiplier;
                ApplyVeilColor(renderer, alphaMultiplier, 1f);
            }

            spawnedVeil.Add(panel);
        }

        private void ApplyVeilColor(Renderer renderer, float alphaMultiplier, float animationAlpha)
        {
            if (renderer == null)
            {
                return;
            }

            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            var color = spaceVeilColor;
            color.a = Mathf.Clamp01(color.a * alphaMultiplier * animationAlpha);
            block.SetColor("_BaseColor", color);
            block.SetColor("_Color", color);
            renderer.SetPropertyBlock(block);
        }

        private Material ResolveVeilMaterial()
        {
            return spaceVeilMaterial;
        }

        private void ClearSpaceVeil()
        {
            for (var index = spawnedVeil.Count - 1; index >= 0; index--)
            {
                var spawned = spawnedVeil[index];
                if (spawned == null)
                {
                    continue;
                }

                if (Application.isPlaying)
                {
                    Destroy(spawned);
                }
                else
                {
                    DestroyImmediate(spawned);
                }
            }

            spawnedVeil.Clear();
            veilAlphaMultipliers.Clear();
        }

        private void HideCurrentContentInVolume()
        {
            if (!replaceCurrentSpace)
            {
                return;
            }

            var currentRoot = GameObject.Find(currentRootName);
            if (currentRoot == null)
            {
                return;
            }

            var playerRoot = player != null ? player.root : null;
            foreach (var renderer in currentRoot.GetComponentsInChildren<Renderer>(true))
            {
                if (renderer == null ||
                    !renderer.enabled ||
                    !renderer.gameObject.activeInHierarchy ||
                    IsPlayerRenderer(renderer, playerRoot) ||
                    IsFloorLikeRenderer(renderer) ||
                    !RendererIntersectsVolume(renderer))
                {
                    continue;
                }

                renderer.enabled = false;
                hiddenCurrentRenderers.Add(renderer);
            }
        }

        private void RestoreHiddenCurrentRenderers()
        {
            foreach (var renderer in hiddenCurrentRenderers)
            {
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
            }

            hiddenCurrentRenderers.Clear();
        }

        private Transform ResolveContentRoot()
        {
            if (contentRoot != null)
            {
                return contentRoot;
            }

            var contentObject = new GameObject("TimeVolume_RuntimeContent");
            contentObject.transform.SetParent(transform, false);
            contentRoot = contentObject.transform;
            return contentRoot;
        }

        private void ClearSpawnedContent()
        {
            for (var index = spawnedContent.Count - 1; index >= 0; index--)
            {
                var spawned = spawnedContent[index];
                if (spawned == null)
                {
                    continue;
                }

                if (Application.isPlaying)
                {
                    Destroy(spawned);
                }
                else
                {
                    DestroyImmediate(spawned);
                }
            }

            spawnedContent.Clear();
        }

        private bool RendererIntersectsVolume(Renderer renderer)
        {
            var bounds = renderer.bounds;
            return ContainsWorldPosition(bounds.center) ||
                   ContainsWorldPosition(new Vector3(bounds.min.x, bounds.center.y, bounds.min.z)) ||
                   ContainsWorldPosition(new Vector3(bounds.min.x, bounds.center.y, bounds.max.z)) ||
                   ContainsWorldPosition(new Vector3(bounds.max.x, bounds.center.y, bounds.min.z)) ||
                   ContainsWorldPosition(new Vector3(bounds.max.x, bounds.center.y, bounds.max.z)) ||
                   BoundsContainsWindowCenter(bounds);
        }

        private bool BoundsContainsWindowCenter(Bounds bounds)
        {
            var padded = bounds;
            padded.Expand(new Vector3(boundsPadding, 0f, boundsPadding));
            var center = transform.position;
            return center.x >= padded.min.x &&
                   center.x <= padded.max.x &&
                   center.z >= padded.min.z &&
                   center.z <= padded.max.z;
        }

        private static Transform ResolveCloneRoot(Transform rendererTransform, Transform sourceRoot)
        {
            var candidate = rendererTransform;
            while (candidate.parent != null &&
                   candidate.parent != sourceRoot &&
                   candidate.parent.name != "DemoZone1_Past")
            {
                candidate = candidate.parent;
            }

            return candidate;
        }

        private static bool IsFloorLikeObject(Transform root)
        {
            return IsFloorLikeName(root.name);
        }

        private static bool IsExcludedSourceRoot(Transform root)
        {
            return root.name == "Past_Floor" ||
                   root.name == "Current_Floor";
        }

        private static bool IsFloorLikeRenderer(Renderer renderer)
        {
            var current = renderer.transform;
            while (current != null)
            {
                if (IsFloorLikeName(current.name))
                {
                    return true;
                }

                current = current.parent;
            }

            return false;
        }

        private static bool IsFloorLikeName(string objectName)
        {
            return objectName.Contains("Floor") ||
                   objectName.Contains("Tile");
        }

        private static bool IsPlayerRenderer(Renderer renderer, Transform playerRoot)
        {
            return playerRoot != null && renderer.transform.IsChildOf(playerRoot);
        }

        private static void ConfigureClonedInteractables(GameObject root)
        {
            foreach (var book in root.GetComponentsInChildren<PastBookInteractable>(true))
            {
                book.SetLocalWindowMode(true);
            }
        }

        private void ApplyPastSpaceTint(GameObject root)
        {
            var tint = Color.Lerp(Color.white, pastContentTint, Mathf.Clamp01(pastContentTintStrength));
            var emission = pastEmissionTint * Mathf.Clamp01(pastEmissionStrength);
            foreach (var renderer in root.GetComponentsInChildren<Renderer>(true))
            {
                if (renderer == null || IsFloorLikeRenderer(renderer))
                {
                    continue;
                }

                var block = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(block);
                block.SetColor("_BaseColor", tint);
                block.SetColor("_Color", tint);
                block.SetColor("_EmissionColor", emission);
                renderer.SetPropertyBlock(block);
            }
        }

        public void PlayCloseAndDestroy()
        {
            if (closing)
            {
                return;
            }

            closing = true;
            SetGatedInteractablesEnabled(false);

            if (visualAnimationRoutine != null)
            {
                StopCoroutine(visualAnimationRoutine);
                visualAnimationRoutine = null;
            }

            if (!Application.isPlaying || !gameObject.activeInHierarchy || closeAnimationDuration <= 0f)
            {
                DestroyWindowObject();
                return;
            }

            visualAnimationRoutine = StartCoroutine(PlayCloseAnimationRoutine());
        }

        private void PlayOpenAnimation()
        {
            if (visualAnimationRoutine != null)
            {
                StopCoroutine(visualAnimationRoutine);
                visualAnimationRoutine = null;
            }

            if (!Application.isPlaying || openAnimationDuration <= 0f)
            {
                SetAnimatedVisualState(1f, 1f);
                return;
            }

            SetAnimatedVisualState(animationStartScale, 0f);
            visualAnimationRoutine = StartCoroutine(PlayOpenAnimationRoutine());
        }

        private IEnumerator PlayOpenAnimationRoutine()
        {
            yield return AnimateVisuals(animationStartScale, 1f, openAnimationDuration);
            visualAnimationRoutine = null;
        }

        private IEnumerator PlayCloseAnimationRoutine()
        {
            yield return AnimateVisuals(1f, animationStartScale, closeAnimationDuration);
            RestoreHiddenCurrentRenderers();
            DestroyWindowObject();
            visualAnimationRoutine = null;
        }

        private IEnumerator AnimateVisuals(float fromScale, float toScale, float duration)
        {
            var elapsed = 0f;
            while (elapsed < duration)
            {
                var normalized = Mathf.Clamp01(elapsed / duration);
                var eased = SmoothStep(normalized);
                var visualScale = Mathf.Lerp(fromScale, toScale, eased);
                var alpha = Mathf.InverseLerp(animationStartScale, 1f, visualScale);
                SetAnimatedVisualState(visualScale, alpha);
                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            SetAnimatedVisualState(toScale, Mathf.InverseLerp(animationStartScale, 1f, toScale));
        }

        private void SetAnimatedVisualState(float visualScale, float alpha)
        {
            var clampedScale = Mathf.Max(0.01f, visualScale);
            transform.localScale = new Vector3(clampedScale, Mathf.Lerp(0.35f, 1f, clampedScale), clampedScale);
            foreach (var pair in veilAlphaMultipliers)
            {
                ApplyVeilColor(pair.Key, pair.Value, alpha);
            }
        }

        private void DestroyWindowObject()
        {
            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        private static float SmoothStep(float value)
        {
            return value * value * (3f - 2f * value);
        }

        private void ResolveGatedInteractables()
        {
            gatedInteractables = GetComponentsInChildren<MonoBehaviour>(true)
                .Where(IsWindowInteractable)
                .ToArray();
        }

        private static bool IsWindowInteractable(MonoBehaviour behaviour)
        {
            if (behaviour == null)
            {
                return false;
            }

            return behaviour is PastBookInteractable ||
                   behaviour.GetType().FullName == "Anemora.Dialogue.NpcInteractable";
        }

        private void SetGatedInteractablesEnabled(bool enabled)
        {
            if (gatedInteractables == null)
            {
                return;
            }

            foreach (var behaviour in gatedInteractables)
            {
                if (behaviour != null)
                {
                    behaviour.enabled = enabled;
                }
            }
        }

        private static void SetLayerRecursively(GameObject root, int layer)
        {
            root.layer = layer;
            foreach (Transform child in root.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        private static void RemoveCollidersRecursively(GameObject root)
        {
            foreach (var collider in root.GetComponentsInChildren<Collider>(true))
            {
                Destroy(collider);
            }
        }
    }
}
