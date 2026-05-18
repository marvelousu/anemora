using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public static class AnemoraTmpJapaneseAtlasBuilder
{
    private const string SourceFontPath = "Assets/UI/Localization/Fonts/ThirdParty/DotGothic16-Regular.ttf";
    private const string FontAssetPath = "Assets/UI/Localization/Fonts/Anemora_JP.asset";
    private const string AtlasAssetPath = "Assets/UI/Localization/Fonts/Anemora_JP_Atlas.asset";

    [MenuItem("Anemora/Build TMP Japanese Atlas v0")]
    public static void Build()
    {
        EnsureTmpSettings();
        EnsureTmpShaderReference();

        var font = AssetDatabase.LoadAssetAtPath<Font>(SourceFontPath);
        if (font == null)
        {
            throw new System.InvalidOperationException($"Source font not found: {SourceFontPath}");
        }

        AssetDatabase.DeleteAsset(FontAssetPath);
        AssetDatabase.DeleteAsset(AtlasAssetPath);

        var fontAsset = TMP_FontAsset.CreateFontAsset(
            font,
            16,
            1,
            GlyphRenderMode.SDF,
            4096,
            4096,
            AtlasPopulationMode.Dynamic,
            false);

        fontAsset.name = "Anemora_JP";
        fontAsset.atlasPopulationMode = AtlasPopulationMode.Dynamic;

        var characters = BuildJapaneseCharacterSet();
        if (!fontAsset.TryAddCharacters(characters, out var missingCharacters))
        {
            Debug.LogWarning($"Missing characters: {missingCharacters.Length}");
        }

        fontAsset.atlasPopulationMode = AtlasPopulationMode.Static;
        AssetDatabase.CreateAsset(fontAsset, FontAssetPath);

        var atlasSource = fontAsset.atlasTexture;
        var atlasCopy = Object.Instantiate(atlasSource);
        atlasCopy.name = "Anemora_JP_Atlas";
        AssetDatabase.CreateAsset(atlasCopy, AtlasAssetPath);

        fontAsset.atlasTextures = new[] { atlasCopy };
        EditorUtility.SetDirty(fontAsset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log(
            $"Anemora TMP JP Atlas built. requested={characters.Length}, missing={missingCharacters.Length}, atlas={atlasCopy.width}x{atlasCopy.height}, format={atlasCopy.format}");
    }

    private static void EnsureTmpSettings()
    {
        if (Shader.Find("TextMeshPro/Mobile/Distance Field") == null)
        {
            var packagePaths = System.IO.Directory.GetFiles(
                System.IO.Path.GetFullPath("Library/PackageCache"),
                "TMP Essential Resources.unitypackage",
                System.IO.SearchOption.AllDirectories);
            if (packagePaths.Length > 0)
            {
                AssetDatabase.ImportPackage(packagePaths[0], false);
                AssetDatabase.Refresh();
            }
        }

        const string settingsFolder = "Assets/TextMesh Pro/Resources";
        const string settingsPath = settingsFolder + "/TMP Settings.asset";
        if (AssetDatabase.LoadAssetAtPath<TMP_Settings>(settingsPath) != null)
        {
            return;
        }

        AssetDatabase.CreateFolder("Assets", "TextMesh Pro");
        AssetDatabase.CreateFolder("Assets/TextMesh Pro", "Resources");
        var settings = ScriptableObject.CreateInstance<TMP_Settings>();
        settings.name = "TMP Settings";
        AssetDatabase.CreateAsset(settings, settingsPath);
        AssetDatabase.SaveAssets();
    }

    private static void EnsureTmpShaderReference()
    {
        var shader = Shader.Find("TextMeshPro/Mobile/Distance Field") ??
            Shader.Find("Hidden/TMP/Internal/Editor/Distance Field SSD");
        if (shader == null)
        {
            return;
        }

        var shaderUtilities = typeof(TMP_Text).Assembly.GetType("TMPro.ShaderUtilities");
        var field = shaderUtilities?.GetField(
            "k_ShaderRef_MobileSDF",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        field?.SetValue(null, shader);
    }

    public static string BuildJapaneseCharacterSet()
    {
        var set = new SortedSet<char>();
        AddRange(set, 0x3041, 0x3096); // Hiragana
        AddRange(set, 0x30A1, 0x30FA); // Katakana
        AddRange(set, 0xFF01, 0xFF5E); // Full-width ASCII forms
        AddRange(set, 0x3000, 0x303F); // Japanese punctuation
        AddRange(set, 0x2010, 0x203B); // Common dashes, quotes, ellipsis, reference mark

        var shiftJis = Encoding.GetEncoding(932);
        for (var row = 16; row <= 84; row++)
        {
            for (var cell = 1; cell <= 94; cell++)
            {
                var jis1 = row + 0x20;
                var jis2 = cell + 0x20;
                var lead = ((jis1 + 1) / 2) + 0x70;
                if (lead >= 0xA0)
                {
                    lead += 0x40;
                }

                int trail;
                if ((jis1 % 2) == 1)
                {
                    trail = jis2 + 0x1F;
                    if (trail >= 0x7F)
                    {
                        trail++;
                    }
                }
                else
                {
                    trail = jis2 + 0x7E;
                }

                var decoded = shiftJis.GetString(new[] { (byte)lead, (byte)trail });
                if (decoded.Length == 1 && decoded[0] != '\uFFFD' && decoded[0] < '\uE000')
                {
                    set.Add(decoded[0]);
                }
            }
        }

        var builder = new StringBuilder(set.Count);
        foreach (var character in set)
        {
            builder.Append(character);
        }

        return builder.ToString();
    }

    private static void AddRange(ISet<char> set, int first, int last)
    {
        for (var code = first; code <= last; code++)
        {
            set.Add((char)code);
        }
    }
}
