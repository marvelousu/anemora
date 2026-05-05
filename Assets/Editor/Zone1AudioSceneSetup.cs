using System;
using System.Collections.Generic;
using System.Linq;
using Anemora.Audio;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Anemora.EditorTools
{
    public static class Zone1AudioSceneSetup
    {
        private const string MainScenePath = "Assets/Scenes/Anemora_Main.unity";
        private const string AudioRootName = "Zone1_Audio";

        [MenuItem("Anemora/Audio/Configure Zone1 Audio")]
        public static void ConfigureMainScene()
        {
            var scene = EditorSceneManager.OpenScene(MainScenePath, OpenSceneMode.Single);
            ConfigureScene(scene);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            Debug.Log("Zone1 audio scene wiring completed.");
        }

        [MenuItem("Anemora/Audio/Verify Zone1 Audio")]
        public static void VerifyMainScene()
        {
            var scene = EditorSceneManager.OpenScene(MainScenePath, OpenSceneMode.Single);
            VerifyScene(scene);
            Debug.Log("Zone1 audio scene verification completed.");
        }

        private static void ConfigureScene(Scene scene)
        {
            var root = GameObject.Find(AudioRootName);
            if (root == null)
            {
                root = new GameObject(AudioRootName);
                SceneManager.MoveGameObjectToScene(root, scene);
            }

            var controller = root.GetComponent<Zone1AudioController>();
            if (controller == null)
            {
                controller = root.AddComponent<Zone1AudioController>();
            }

            ConfigureZone1Controller(root, controller);
            ConfigureNpcAudio(scene);
            ConfigureDialogueAudio(scene);
        }

        private static void VerifyScene(Scene scene)
        {
            var root = GameObject.Find(AudioRootName);
            if (root == null)
            {
                throw new InvalidOperationException($"Missing scene object: {AudioRootName}");
            }

            var controller = root.GetComponent<Zone1AudioController>();
            if (controller == null)
            {
                throw new InvalidOperationException("Missing Zone1AudioController component.");
            }

            var controllerObject = new SerializedObject(controller);
            RequireObject(controllerObject, "zone1AmbientClip");
            RequireObject(controllerObject, "windAmbienceClip");
            RequireObject(controllerObject, "silencePadClip");
            RequireObject(controllerObject, "portalOpenClip");
            RequireObject(controllerObject, "portalFlipClip");
            RequireArray(controllerObject, "environmentOneShotClips", 4);
            RequireArray(controllerObject, "stoneFootstepWalkClips", 1);
            RequireArray(controllerObject, "grassFootstepWalkClips", 1);
            RequireObject(controllerObject, "uiButtonClickClip");

            var npcType = ResolveType("Anemora.Dialogue.NpcInteractable, Anemora.Dialogue");
            var npcComponents = FindSceneComponents(scene, npcType);
            if (npcComponents.Count == 0)
            {
                throw new InvalidOperationException("No NpcInteractable components found in the scene.");
            }

            foreach (var component in npcComponents)
            {
                RequireObject(new SerializedObject(component), "interactionClip");
            }

            var displayType = ResolveType("Anemora.Dialogue.DialogueDisplay, Anemora.Dialogue");
            var displayComponents = FindSceneComponents(scene, displayType);
            if (displayComponents.Count == 0)
            {
                throw new InvalidOperationException("No DialogueDisplay components found in the scene.");
            }

            foreach (var component in displayComponents)
            {
                var serialized = new SerializedObject(component);
                RequireObject(serialized, "advanceClip");
                RequireObject(serialized, "closeClip");
            }
        }

        private static void ConfigureZone1Controller(GameObject root, Zone1AudioController controller)
        {
            var musicSource = EnsureSource(root, "Music_Source", true, 0.45f, 0f);
            var windSource = EnsureSource(root, "Wind_Ambience_Source", true, 0.35f, 0f);
            var padSource = EnsureSource(root, "Pad_Ambience_Source", true, 0.22f, 0f);
            var oneShotSource = EnsureSource(root, "OneShot_Source", false, 1f, 0f);

            var serialized = new SerializedObject(controller);
            SetBool(serialized, "autoPlayOnStart", true);
            SetObject(serialized, "musicSource", musicSource);
            SetObject(serialized, "windAmbienceSource", windSource);
            SetObject(serialized, "padAmbienceSource", padSource);
            SetObject(serialized, "oneShotSource", oneShotSource);

            SetObject(serialized, "zone1AmbientClip", LoadClip("Assets/Audio/Music/Zone1_Ambient.ogg"));
            SetObject(serialized, "windAmbienceClip", LoadClip("Assets/Audio/SFX/Zone1/environment/sfx_env_wind_loop_01.ogg"));
            SetObject(serialized, "silencePadClip", LoadClip("Assets/Audio/SFX/Zone1/environment/sfx_env_silence_pad_01.ogg"));
            SetClipArray(serialized, "environmentOneShotClips", new[]
            {
                "Assets/Audio/SFX/Zone1/environment/sfx_env_birds_01.ogg",
                "Assets/Audio/SFX/Zone1/environment/sfx_env_distant_water_01.ogg",
                "Assets/Audio/SFX/Zone1/environment/sfx_env_dry_leaves_01.ogg",
                "Assets/Audio/SFX/Zone1/environment/sfx_env_wood_creak_01.ogg"
            });

            SetClipArray(serialized, "stoneFootstepWalkClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_walk_01.ogg" });
            SetClipArray(serialized, "stoneFootstepRunClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_run_01.ogg" });
            SetClipArray(serialized, "stoneFootstepLandClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_stone_land_01.ogg" });
            SetClipArray(serialized, "woodFootstepWalkClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_walk_01.ogg" });
            SetClipArray(serialized, "woodFootstepRunClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_run_01.ogg" });
            SetClipArray(serialized, "woodFootstepLandClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_wood_land_01.ogg" });
            SetClipArray(serialized, "grassFootstepWalkClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_walk_01.ogg" });
            SetClipArray(serialized, "grassFootstepRunClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_run_01.ogg" });
            SetClipArray(serialized, "grassFootstepLandClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_grass_land_01.ogg" });
            SetClipArray(serialized, "sandFootstepWalkClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_walk_01.ogg" });
            SetClipArray(serialized, "sandFootstepRunClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_run_01.ogg" });
            SetClipArray(serialized, "sandFootstepLandClips", new[] { "Assets/Audio/SFX/Zone1/footsteps/sfx_footstep_sand_land_01.ogg" });

            SetObject(serialized, "wheelOpenClip", LoadClip("Assets/Audio/SFX/Zone1/time_window/sfx_time_wheel_open_01.ogg"));
            SetObject(serialized, "wheelCloseClip", LoadClip("Assets/Audio/SFX/Zone1/time_window/sfx_time_wheel_close_01.ogg"));
            SetObject(serialized, "symbolHoverClip", LoadClip("Assets/Audio/SFX/Zone1/time_window/sfx_time_symbol_hover_01.ogg"));
            SetObject(serialized, "symbolSelectRedClip", LoadClip("Assets/Audio/SFX/Zone1/time_window/sfx_time_symbol_select_red_01.ogg"));
            SetObject(serialized, "portalOpenClip", LoadClip("Assets/Audio/SFX/Zone1/time_window/sfx_time_portal_open_01.ogg"));
            SetObject(serialized, "portalFlipClip", LoadClip("Assets/Audio/SFX/Zone1/time_window/sfx_time_portal_flip_01.ogg"));

            SetObject(serialized, "npcGreetingClip", LoadClip("Assets/Audio/SFX/Zone1/npc/sfx_npc_greeting_short_01.ogg"));
            SetObject(serialized, "npcInteractionAckClip", LoadClip("Assets/Audio/SFX/Zone1/npc/sfx_npc_interaction_ack_01.ogg"));
            SetObject(serialized, "npcDepartureClip", LoadClip("Assets/Audio/SFX/Zone1/npc/sfx_npc_departure_01.ogg"));

            SetObject(serialized, "uiButtonClickClip", LoadClip("Assets/Audio/SFX/Zone1/ui/sfx_ui_button_click_01.ogg"));
            SetObject(serialized, "uiMenuOpenClip", LoadClip("Assets/Audio/SFX/Zone1/ui/sfx_ui_menu_open_01.ogg"));
            SetObject(serialized, "uiMenuCloseClip", LoadClip("Assets/Audio/SFX/Zone1/ui/sfx_ui_menu_close_01.ogg"));

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void ConfigureNpcAudio(Scene scene)
        {
            var npcType = ResolveType("Anemora.Dialogue.NpcInteractable, Anemora.Dialogue");
            var greetingClip = LoadClip("Assets/Audio/SFX/Zone1/npc/sfx_npc_greeting_short_01.ogg");
            foreach (var component in FindSceneComponents(scene, npcType))
            {
                var source = component.GetComponent<AudioSource>();
                if (source == null)
                {
                    source = component.gameObject.AddComponent<AudioSource>();
                }

                ConfigureSource(source, false, 0.7f, 0f);
                var serialized = new SerializedObject(component);
                SetObject(serialized, "audioSource", source);
                SetObject(serialized, "interactionClip", greetingClip);
                serialized.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static void ConfigureDialogueAudio(Scene scene)
        {
            var displayType = ResolveType("Anemora.Dialogue.DialogueDisplay, Anemora.Dialogue");
            var ackClip = LoadClip("Assets/Audio/SFX/Zone1/npc/sfx_npc_interaction_ack_01.ogg");
            var departureClip = LoadClip("Assets/Audio/SFX/Zone1/npc/sfx_npc_departure_01.ogg");
            foreach (var component in FindSceneComponents(scene, displayType))
            {
                var source = component.GetComponent<AudioSource>();
                if (source == null)
                {
                    source = component.gameObject.AddComponent<AudioSource>();
                }

                ConfigureSource(source, false, 0.7f, 0f);
                var serialized = new SerializedObject(component);
                SetObject(serialized, "audioSource", source);
                SetObject(serialized, "advanceClip", ackClip);
                SetObject(serialized, "closeClip", departureClip);
                serialized.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        private static AudioSource EnsureSource(GameObject root, string childName, bool loop, float volume, float spatialBlend)
        {
            var child = root.transform.Find(childName);
            if (child == null)
            {
                child = new GameObject(childName).transform;
                child.SetParent(root.transform, false);
            }

            var source = child.GetComponent<AudioSource>();
            if (source == null)
            {
                source = child.gameObject.AddComponent<AudioSource>();
            }

            ConfigureSource(source, loop, volume, spatialBlend);
            return source;
        }

        private static void ConfigureSource(AudioSource source, bool loop, float volume, float spatialBlend)
        {
            source.playOnAwake = false;
            source.loop = loop;
            source.volume = volume;
            source.spatialBlend = spatialBlend;
            source.minDistance = 1f;
            source.maxDistance = 12f;
        }

        private static IReadOnlyList<Component> FindSceneComponents(Scene scene, Type type)
        {
            return scene
                .GetRootGameObjects()
                .SelectMany(root => root.GetComponentsInChildren(type, true).Cast<Component>())
                .Where(component => component != null)
                .ToArray();
        }

        private static Type ResolveType(string assemblyQualifiedName)
        {
            var type = Type.GetType(assemblyQualifiedName, throwOnError: false);
            if (type == null)
            {
                throw new InvalidOperationException($"Could not resolve type: {assemblyQualifiedName}");
            }

            return type;
        }

        private static AudioClip LoadClip(string path)
        {
            var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            if (clip == null)
            {
                throw new InvalidOperationException($"Missing AudioClip asset: {path}");
            }

            return clip;
        }

        private static void SetBool(SerializedObject serialized, string fieldName, bool value)
        {
            var property = RequiredProperty(serialized, fieldName);
            property.boolValue = value;
        }

        private static void SetObject(SerializedObject serialized, string fieldName, UnityEngine.Object value)
        {
            var property = RequiredProperty(serialized, fieldName);
            property.objectReferenceValue = value;
        }

        private static void SetClipArray(SerializedObject serialized, string fieldName, string[] paths)
        {
            var property = RequiredProperty(serialized, fieldName);
            property.arraySize = paths.Length;
            for (var i = 0; i < paths.Length; i++)
            {
                property.GetArrayElementAtIndex(i).objectReferenceValue = LoadClip(paths[i]);
            }
        }

        private static SerializedProperty RequiredProperty(SerializedObject serialized, string fieldName)
        {
            var property = serialized.FindProperty(fieldName);
            if (property == null)
            {
                throw new InvalidOperationException(
                    $"Missing serialized field '{fieldName}' on {serialized.targetObject.name}.");
            }

            return property;
        }

        private static void RequireObject(SerializedObject serialized, string fieldName)
        {
            var property = RequiredProperty(serialized, fieldName);
            if (property.objectReferenceValue == null)
            {
                throw new InvalidOperationException(
                    $"Serialized field '{fieldName}' is not assigned on {serialized.targetObject.name}.");
            }
        }

        private static void RequireArray(SerializedObject serialized, string fieldName, int minCount)
        {
            var property = RequiredProperty(serialized, fieldName);
            if (!property.isArray || property.arraySize < minCount)
            {
                throw new InvalidOperationException(
                    $"Serialized array '{fieldName}' has {property.arraySize} entries on {serialized.targetObject.name}; expected at least {minCount}.");
            }

            for (var i = 0; i < property.arraySize; i++)
            {
                if (property.GetArrayElementAtIndex(i).objectReferenceValue == null)
                {
                    throw new InvalidOperationException(
                        $"Serialized array '{fieldName}' has a null entry at index {i} on {serialized.targetObject.name}.");
                }
            }
        }
    }
}
