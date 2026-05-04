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
    }
}
