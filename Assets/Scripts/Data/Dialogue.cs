using System;
using System.Collections.Generic;

namespace Anemora.Data
{
    [Serializable]
    public sealed class DialogueAssetData
    {
        public string npcId;
        public List<DialogueVariantData> variants = new List<DialogueVariantData>();
    }

    [Serializable]
    public sealed class DialogueVariantData
    {
        public string variantId;
        public List<DialogueTurnData> turns = new List<DialogueTurnData>();
        public List<string> requiredFlags = new List<string>();
        public List<string> excludedFlags = new List<string>();
    }

    [Serializable]
    public sealed class DialogueTurnData
    {
        public string speakerId;
        public string textKey;
        public List<DialogueChoiceData> choices = new List<DialogueChoiceData>();
    }

    [Serializable]
    public sealed class DialogueChoiceData
    {
        public string emotion;
        public string labelKey;
        public string nextTurnId;
    }
}
