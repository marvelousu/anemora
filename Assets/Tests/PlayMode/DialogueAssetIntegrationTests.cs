using System.Collections;
using Anemora.Game.Dialogue;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class DialogueAssetIntegrationTests
    {
        [UnityTest]
        public IEnumerator ScriptableObjectInstance_HoldsLocalizedStringDialogueTree()
        {
            var asset = ScriptableObject.CreateInstance<DialogueAsset>();
            Locale jaJp = null;
            try
            {
                jaJp = Locale.CreateLocale("ja-JP");
                var turn = new DialogueTurnSO
                {
                    speakerId = "resident_a",
                    text = new LocalizedString("Anemora_Dialogue", "dialogue.npc.resident_a.initial_01")
                    {
                        LocaleOverride = jaJp
                    },
                    choices =
                    {
                        new DialogueChoiceSO
                        {
                            emotion = "neutral",
                            label = new LocalizedString("Anemora_Dialogue", "dialogue.choice.resident_a.continue")
                            {
                                LocaleOverride = jaJp
                            },
                            nextTurnId = "resident_a.initial.02"
                        }
                    }
                };
                asset.npcId = "resident_a";
                asset.variants.Add(new DialogueVariantSO
                {
                    variantId = "initial",
                    turns = { turn }
                });

                Assert.That(asset.npcId, Is.EqualTo("resident_a"));
                Assert.That(asset.variants, Has.Count.EqualTo(1));
                Assert.That(asset.variants[0].turns[0].speakerId, Is.EqualTo("resident_a"));
                Assert.That(asset.variants[0].turns[0].text.LocaleOverride.Identifier.Code, Is.EqualTo("ja-JP"));
                Assert.That(asset.variants[0].turns[0].choices[0].label.LocaleOverride.Identifier.Code, Is.EqualTo("ja-JP"));
                Assert.That(
                    turn.GetLocalizedTextOrFallback("dialogue.npc.resident_a.initial_01"),
                    Is.EqualTo("dialogue.npc.resident_a.initial_01"));
                Assert.That(
                    turn.choices[0].GetLocalizedLabelOrFallback("dialogue.choice.resident_a.continue"),
                    Is.EqualTo("dialogue.choice.resident_a.continue"));
                yield return null;
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(jaJp);
                UnityEngine.Object.DestroyImmediate(asset);
            }
        }

        [UnityTest]
        public IEnumerator EmptyLocalizationTables_FallBackToStringKeys()
        {
            var asset = ScriptableObject.CreateInstance<DialogueAsset>();
            Locale jaJp = null;
            try
            {
                jaJp = Locale.CreateLocale("ja-JP");
                var turn = new DialogueTurnSO
                {
                    speakerId = "resident_a",
                    text = new LocalizedString("Anemora_Dialogue_Empty", "dialogue.npc.resident_a.initial_01")
                    {
                        LocaleOverride = jaJp
                    },
                    choices =
                    {
                        new DialogueChoiceSO
                        {
                            emotion = "neutral",
                            label = new LocalizedString("Anemora_Dialogue_Empty", "dialogue.choice.resident_a.continue")
                            {
                                LocaleOverride = jaJp
                            },
                            nextTurnId = "resident_a.initial.02"
                        }
                    }
                };
                asset.variants.Add(new DialogueVariantSO
                {
                    variantId = "initial",
                    turns = { turn }
                });

                Assert.That(
                    turn.GetLocalizedTextOrFallback("dialogue.npc.resident_a.initial_01"),
                    Is.EqualTo("dialogue.npc.resident_a.initial_01"));
                Assert.That(
                    turn.choices[0].GetLocalizedLabelOrFallback("dialogue.choice.resident_a.continue"),
                    Is.EqualTo("dialogue.choice.resident_a.continue"));
                yield return null;
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(jaJp);
                UnityEngine.Object.DestroyImmediate(asset);
            }
        }
    }
}
