using System.Linq;
using Anemora.Data;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Anemora.Tests.EditMode
{
    public sealed class DialogueAssetDataTests
    {
        [Test]
        public void RoundTrip_PreservesNpcVariantTurnChoiceAndFlags()
        {
            var source = CreateSample();

            var restored = JsonConvert.DeserializeObject<DialogueAssetData>(
                JsonConvert.SerializeObject(source));

            Assert.That(restored.npcId, Is.EqualTo("resident_a"));
            Assert.That(restored.variants, Has.Count.EqualTo(1));
            Assert.That(restored.variants[0].variantId, Is.EqualTo("post_take_book_family_001"));
            CollectionAssert.AreEqual(new[] { "take_book_family_001" }, restored.variants[0].requiredFlags);
            CollectionAssert.AreEqual(new[] { "time_erosion" }, restored.variants[0].excludedFlags);
            Assert.That(restored.variants[0].turns, Has.Count.EqualTo(1));
            Assert.That(restored.variants[0].turns[0].textKey, Is.EqualTo("dialogue.npc.resident_a.post_take_book_family_001_01"));
            Assert.That(restored.variants[0].turns[0].choices, Has.Count.EqualTo(1));
            Assert.That(restored.variants[0].turns[0].choices[0].labelKey, Is.EqualTo("dialogue.choice.resident_a.ask_family_book"));
        }

        [Test]
        public void DialogueTextAndChoiceLabels_AreKeysOnly()
        {
            var source = CreateSample();
            var json = JsonConvert.SerializeObject(source);

            Assert.That(json, Does.Contain("\"textKey\""));
            Assert.That(json, Does.Contain("\"labelKey\""));
            Assert.That(json, Does.Not.Contain("LocalizedString"));
            Assert.That(json, Does.Not.Contain("tableReference"));
        }

        [Test]
        public void MultipleVariants_CanBeFilteredByFlags()
        {
            var source = CreateSample();
            source.variants.Add(new DialogueVariantData
            {
                variantId = "initial",
                turns =
                {
                    new DialogueTurnData
                    {
                        speakerId = "resident_a",
                        textKey = "dialogue.npc.resident_a.initial_01"
                    }
                }
            });

            var matching = source.variants
                .Where(variant => variant.requiredFlags.Contains("take_book_family_001"))
                .Select(variant => variant.variantId)
                .ToList();

            CollectionAssert.AreEqual(new[] { "post_take_book_family_001" }, matching);
        }

        private static DialogueAssetData CreateSample()
        {
            return new DialogueAssetData
            {
                npcId = "resident_a",
                variants =
                {
                    new DialogueVariantData
                    {
                        variantId = "post_take_book_family_001",
                        requiredFlags = { "take_book_family_001" },
                        excludedFlags = { "time_erosion" },
                        turns =
                        {
                            new DialogueTurnData
                            {
                                speakerId = "resident_a",
                                textKey = "dialogue.npc.resident_a.post_take_book_family_001_01",
                                choices =
                                {
                                    new DialogueChoiceData
                                    {
                                        emotion = "concerned",
                                        labelKey = "dialogue.choice.resident_a.ask_family_book",
                                        nextTurnId = "resident_a.post_book.02"
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
