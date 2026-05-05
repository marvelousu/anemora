using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Anemora.Tests.EditMode
{
    public sealed class CharacterPrefabStructureTests
    {
        [Test]
        public void CharacterPrefabsContainSpriteRendererAndAnimator()
        {
            AssertCharacterPrefab("Assets/Prefabs/Characters/Hero.prefab", requiresBinder: true);
            AssertCharacterPrefab("Assets/Prefabs/Characters/Resident_A.prefab", requiresBinder: false);
            AssertCharacterPrefab("Assets/Prefabs/Characters/Resident_B.prefab", requiresBinder: false);
        }

        [Test]
        public void LocomotionControllersExposeExpectedStatesAndParameters()
        {
            AssertLocomotionController("Assets/Animators/HeroLocomotion.controller", requiresWalk: true, requiresParameters: true);
            AssertLocomotionController("Assets/Animators/ResidentALocomotion.controller", requiresWalk: true, requiresParameters: true);
            AssertLocomotionController("Assets/Animators/ResidentBIdle.controller", requiresWalk: false, requiresParameters: false);
        }

        [Test]
        public void F2CharacterSpritesAreSlicedForAnimatorClips()
        {
            AssertSpriteSheet("Assets/Art/Sprites/Hero/v1/hero_idle.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/Hero/v1/hero_walk_front.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/Hero/v1/hero_walk_back.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/Hero/v1/hero_walk_right.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_idle.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_walk_front.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/NPC/Resident_A/v1/resident_a_walk_back.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/NPC/Resident_B/v1/resident_b_idle.png", 4);
        }

        [Test]
        public void HeroV2SpritesAreSlicedForAnimatorClips()
        {
            AssertSingleSprite("Assets/Art/Sprites/Hero/v2/hero_stand.png");
            AssertSpriteSheet("Assets/Art/Sprites/Hero/v2/hero_idle.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/Hero/v2/hero_walk_front.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/Hero/v2/hero_walk_back.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/Hero/v2/hero_walk_left.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/Hero/v2/hero_walk_right.png", 4);
            AssertSingleSprite("Assets/Art/Sprites/Hero/v2/hero_hands_d7.png");
        }

        [Test]
        public void ResidentV2SpritesAreSlicedForAnimatorClips()
        {
            AssertSpriteSheet("Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_idle.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_front.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_back.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_left.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/NPC/Resident_A/v2/resident_a_walk_right.png", 4);
            AssertSpriteSheet("Assets/Art/Sprites/NPC/Resident_B/v2/resident_b_idle.png", 4);
        }

        [Test]
        public void CharacterPrefabsUseV2SpriteSets()
        {
            const string prefabPath = "Assets/Prefabs/Characters/Hero.prefab";
            const string spriteRoot = "Assets/Art/Sprites/Hero/v2/";

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            Assert.That(prefab, Is.Not.Null, prefabPath);

            var spriteRenderer = prefab.GetComponentInChildren<SpriteRenderer>(true);
            Assert.That(spriteRenderer, Is.Not.Null, prefabPath);
            AssertSpritePathUnder(spriteRenderer.sprite, spriteRoot, "SpriteRenderer.sprite");

            var binderType = System.Type.GetType("Anemora.Player.HeroAnimatorBinder, Assembly-CSharp", throwOnError: true);
            var binder = prefab.GetComponentInChildren(binderType, true);
            Assert.That(binder, Is.Not.Null, prefabPath);

            var serializedBinder = new SerializedObject(binder);
            AssertSpriteArrayPathUnder(serializedBinder, "idleFrontSprites", spriteRoot);
            AssertSpriteArrayPathUnder(serializedBinder, "idleSideSprites", spriteRoot);
            AssertSpriteArrayPathUnder(serializedBinder, "idleBackSprites", spriteRoot);
            AssertSpriteArrayPathUnder(serializedBinder, "walkFrontSprites", spriteRoot);
            AssertSpriteArrayPathUnder(serializedBinder, "walkSideSprites", spriteRoot);
            AssertSpriteArrayPathUnder(serializedBinder, "walkBackSprites", spriteRoot);

            AssertPrefabSpritePathUnder(
                "Assets/Prefabs/Characters/Resident_A.prefab",
                "Assets/Art/Sprites/NPC/Resident_A/v2/");
            AssertPrefabSpritePathUnder(
                "Assets/Prefabs/Characters/Resident_B.prefab",
                "Assets/Art/Sprites/NPC/Resident_B/v2/");

            AssertClipSpritePathsUnder("Assets/Animators/Clips/Hero_Idle.anim", spriteRoot);
            AssertClipSpritePathsUnder("Assets/Animators/Clips/Hero_Walk.anim", spriteRoot);
            AssertClipSpritePathsUnder(
                "Assets/Animators/Clips/Resident_A_Idle.anim",
                "Assets/Art/Sprites/NPC/Resident_A/v2/");
            AssertClipSpritePathsUnder(
                "Assets/Animators/Clips/Resident_A_Walk.anim",
                "Assets/Art/Sprites/NPC/Resident_A/v2/");
            AssertClipSpritePathsUnder(
                "Assets/Animators/Clips/Resident_B_Idle.anim",
                "Assets/Art/Sprites/NPC/Resident_B/v2/");
        }

        private static void AssertCharacterPrefab(string path, bool requiresBinder)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Assert.That(prefab, Is.Not.Null, path);
            Assert.That(prefab.GetComponentInChildren<SpriteRenderer>(true), Is.Not.Null, path);
            Assert.That(prefab.GetComponentInChildren<Animator>(true), Is.Not.Null, path);

            if (requiresBinder)
            {
                var binderType = System.Type.GetType("Anemora.Player.HeroAnimatorBinder, Assembly-CSharp", throwOnError: true);
                Assert.That(prefab.GetComponentInChildren(binderType, true), Is.Not.Null, path);
            }
        }

        private static void AssertLocomotionController(
            string path,
            bool requiresWalk,
            bool requiresParameters)
        {
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
            Assert.That(controller, Is.Not.Null, path);

            var stateNames = controller.layers[0].stateMachine.states
                .Select(state => state.state.name)
                .ToArray();
            Assert.That(stateNames, Does.Contain("Idle"), path);
            if (requiresWalk)
            {
                Assert.That(stateNames, Does.Contain("Walk"), path);
            }

            if (requiresParameters)
            {
                Assert.That(controller.parameters.Any(parameter =>
                    parameter.name == "isMoving" && parameter.type == AnimatorControllerParameterType.Bool), Is.True, path);
                Assert.That(controller.parameters.Any(parameter =>
                    parameter.name == "facing" && parameter.type == AnimatorControllerParameterType.Int), Is.True, path);
            }
        }

        private static void AssertSpriteSheet(string path, int expectedCount)
        {
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            Assert.That(importer, Is.Not.Null, path);
            Assert.That(importer.textureType, Is.EqualTo(TextureImporterType.Sprite), path);
            Assert.That(importer.filterMode, Is.EqualTo(FilterMode.Point), path);
            Assert.That(importer.mipmapEnabled, Is.False, path);
            Assert.That(importer.alphaIsTransparency, Is.True, path);
            Assert.That(importer.wrapMode, Is.EqualTo(TextureWrapMode.Clamp), path);
            Assert.That(importer.textureCompression, Is.EqualTo(TextureImporterCompression.Uncompressed), path);

            var defaultSettings = importer.GetDefaultPlatformTextureSettings();
            Assert.That(defaultSettings.maxTextureSize, Is.EqualTo(1024), path);
            Assert.That(defaultSettings.textureCompression, Is.EqualTo(TextureImporterCompression.Uncompressed), path);

            var standaloneSettings = importer.GetPlatformTextureSettings("Standalone");
            Assert.That(standaloneSettings.maxTextureSize, Is.EqualTo(1024), path);
            Assert.That(standaloneSettings.textureCompression, Is.EqualTo(TextureImporterCompression.Uncompressed), path);

            var sprites = AssetDatabase.LoadAllAssetsAtPath(path)
                .OfType<Sprite>()
                .ToArray();
            Assert.That(sprites.Length, Is.EqualTo(expectedCount), path);
            Assert.That(sprites.All(sprite => Mathf.Approximately(sprite.pixelsPerUnit, 32f)), Is.True, path);
            Assert.That(sprites.All(sprite => Mathf.Approximately(sprite.pivot.x, sprite.rect.width * 0.5f)), Is.True, path);
            Assert.That(sprites.All(sprite => Mathf.Approximately(sprite.pivot.y, 0f)), Is.True, path);
        }

        private static void AssertSingleSprite(string path)
        {
            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            Assert.That(importer, Is.Not.Null, path);
            Assert.That(importer.textureType, Is.EqualTo(TextureImporterType.Sprite), path);
            Assert.That(importer.filterMode, Is.EqualTo(FilterMode.Point), path);
            Assert.That(importer.mipmapEnabled, Is.False, path);
            Assert.That(importer.alphaIsTransparency, Is.True, path);
            Assert.That(importer.wrapMode, Is.EqualTo(TextureWrapMode.Clamp), path);
            Assert.That(importer.textureCompression, Is.EqualTo(TextureImporterCompression.Uncompressed), path);

            var sprites = AssetDatabase.LoadAllAssetsAtPath(path)
                .OfType<Sprite>()
                .ToArray();
            Assert.That(sprites.Length, Is.EqualTo(1), path);
            Assert.That(sprites.All(sprite => Mathf.Approximately(sprite.pixelsPerUnit, 32f)), Is.True, path);
        }

        private static void AssertSpriteArrayPathUnder(SerializedObject serializedObject, string fieldName, string spriteRoot)
        {
            var property = serializedObject.FindProperty(fieldName);
            Assert.That(property, Is.Not.Null, fieldName);
            Assert.That(property.isArray, Is.True, fieldName);
            Assert.That(property.arraySize, Is.EqualTo(4), fieldName);

            for (var index = 0; index < property.arraySize; index++)
            {
                var element = property.GetArrayElementAtIndex(index);
                AssertSpritePathUnder(element.objectReferenceValue as Sprite, spriteRoot, $"{fieldName}[{index}]");
            }
        }

        private static void AssertPrefabSpritePathUnder(string prefabPath, string spriteRoot)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            Assert.That(prefab, Is.Not.Null, prefabPath);

            var spriteRenderer = prefab.GetComponentInChildren<SpriteRenderer>(true);
            Assert.That(spriteRenderer, Is.Not.Null, prefabPath);
            AssertSpritePathUnder(spriteRenderer.sprite, spriteRoot, $"{prefabPath} SpriteRenderer.sprite");
        }

        private static void AssertClipSpritePathsUnder(string clipPath, string spriteRoot)
        {
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
            Assert.That(clip, Is.Not.Null, clipPath);

            var bindings = AnimationUtility.GetObjectReferenceCurveBindings(clip);
            Assert.That(bindings.Length, Is.GreaterThan(0), clipPath);

            foreach (var binding in bindings)
            {
                var keyframes = AnimationUtility.GetObjectReferenceCurve(clip, binding);
                foreach (var keyframe in keyframes)
                {
                    if (keyframe.value is Sprite sprite)
                    {
                        AssertSpritePathUnder(sprite, spriteRoot, $"{clipPath} {binding.propertyName}");
                    }
                }
            }
        }

        private static void AssertSpritePathUnder(Sprite sprite, string spriteRoot, string context)
        {
            Assert.That(sprite, Is.Not.Null, context);
            var path = AssetDatabase.GetAssetPath(sprite);
            Assert.That(path, Does.StartWith(spriteRoot), context);
        }
    }
}
