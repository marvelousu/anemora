using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Anemora.Game.Dialogue
{
    [CreateAssetMenu(menuName = "Anemora/Dialogue", fileName = "Dialogue")]
    public sealed class DialogueAsset : ScriptableObject
    {
        public string npcId;
        public List<DialogueVariantSO> variants = new List<DialogueVariantSO>();
    }

    [Serializable]
    public sealed class DialogueVariantSO
    {
        public string variantId;
        public List<DialogueTurnSO> turns = new List<DialogueTurnSO>();
        public List<string> requiredFlags = new List<string>();
        public List<string> excludedFlags = new List<string>();
    }

    [Serializable]
    public sealed class DialogueTurnSO
    {
        public string speakerId;
        public LocalizedString text = new LocalizedString();
        public List<DialogueChoiceSO> choices = new List<DialogueChoiceSO>();

        public string GetLocalizedTextOrFallback(string fallback)
        {
            return DialogueLocalization.ResolveOrFallback(text, fallback);
        }
    }

    [Serializable]
    public sealed class DialogueChoiceSO
    {
        public string emotion;
        public LocalizedString label = new LocalizedString();
        public string nextTurnId;

        public string GetLocalizedLabelOrFallback(string fallback)
        {
            return DialogueLocalization.ResolveOrFallback(label, fallback);
        }
    }

    internal static class DialogueLocalization
    {
        public static string ResolveOrFallback(LocalizedString localizedString, string fallback)
        {
            if (localizedString == null || localizedString.IsEmpty)
            {
                return fallback ?? string.Empty;
            }

            if (!LocalizationSettings.HasSettings)
            {
                return fallback ?? string.Empty;
            }

            if (Application.isBatchMode)
            {
                return fallback ?? string.Empty;
            }

            try
            {
                var localeOperation = LocalizationSettings.SelectedLocaleAsync;
                if (!localeOperation.IsDone)
                {
                    localeOperation.WaitForCompletion();
                }

                var locale = localizedString.LocaleOverride != null
                    ? localizedString.LocaleOverride
                    : localeOperation.Result;
                var tableEntry = LocalizationSettings.StringDatabase.GetTableEntry(
                    localizedString.TableReference,
                    localizedString.TableEntryReference,
                    locale,
                    localizedString.FallbackState == FallbackBehavior.DontUseFallback
                        ? FallbackBehavior.DontUseFallback
                        : FallbackBehavior.UseFallback);
                var resolved = tableEntry.Entry?.GetLocalizedString();
                return string.IsNullOrEmpty(resolved) ? fallback ?? string.Empty : resolved;
            }
            catch (Exception)
            {
                return fallback ?? string.Empty;
            }
        }
    }
}
