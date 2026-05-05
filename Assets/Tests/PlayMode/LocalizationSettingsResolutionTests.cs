using System.Collections;
using Anemora.Game.Dialogue;
using NUnit.Framework;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.TestTools;

namespace Anemora.Tests.PlayMode
{
    public sealed class LocalizationSettingsResolutionTests
    {
        const string TableName = "Anemora_Strings";

        [UnityTest]
        public IEnumerator PlaceholderDialogueKey_ResolvesForJapaneseLocale()
        {
            yield return LocalizationSettings.InitializationOperation;

            var ja = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("ja-JP"));
            Assert.That(ja, Is.Not.Null);

            LocalizationSettings.SelectedLocale = ja;
            yield return LocalizationSettings.SelectedLocaleAsync;

            var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
                TableName,
                "dialogue.placeholder.resident_a.greet",
                ja,
                FallbackBehavior.UseFallback);
            yield return operation;

            Assert.That(operation.Result, Is.EqualTo("[TBD: Resident_A greet line]"));
        }

        [UnityTest]
        public IEnumerator PlaceholderDialogueKey_ResolvesForEnglishLocale()
        {
            yield return LocalizationSettings.InitializationOperation;

            var en = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("en"));
            Assert.That(en, Is.Not.Null);

            LocalizationSettings.SelectedLocale = en;
            yield return LocalizationSettings.SelectedLocaleAsync;

            var operation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
                TableName,
                "dialogue.placeholder.resident_b.idle",
                en,
                FallbackBehavior.UseFallback);
            yield return operation;

            Assert.That(operation.Result, Is.EqualTo("[TBD: Resident_B idle line]"));
        }

        [UnityTest]
        public IEnumerator MissingDialogueKey_FallsBackToProvidedKeyString()
        {
            yield return LocalizationSettings.InitializationOperation;

            var ja = LocalizationSettings.AvailableLocales.GetLocale(new LocaleIdentifier("ja-JP"));
            Assert.That(ja, Is.Not.Null);

            var missingKey = "dialogue.placeholder.missing_key";
            var turn = new DialogueTurnSO
            {
                speakerId = "resident_a",
                text = new LocalizedString(TableName, missingKey)
                {
                    LocaleOverride = ja,
                    FallbackState = FallbackBehavior.UseFallback
                }
            };

            Assert.That(turn.GetLocalizedTextOrFallback(missingKey), Is.EqualTo(missingKey));
        }
    }
}
