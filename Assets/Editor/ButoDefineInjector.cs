#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

[InitializeOnLoad]
internal static class ButoDefineInjector
{
    private static readonly string[] Defines =
    {
        "BUTO",
        "OCCASOFTWARE",
    };

    static ButoDefineInjector()
    {
        var present = AppDomain.CurrentDomain.GetAssemblies()
            .Any(assembly =>
                assembly.GetType("OccaSoftware.Buto.Runtime.ButoVolumetricFog") != null ||
                assembly.GetType("OccaSoftware.Buto.Runtime.ButoRenderFeature") != null);

        var target = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        var defines = PlayerSettings.GetScriptingDefineSymbols(target)
            .Split(';')
            .Where(symbol => symbol.Length > 0)
            .ToList();

        var changed = false;
        foreach (var define in Defines)
        {
            changed |= SetDefine(defines, define, present);
        }

        if (changed)
        {
            PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", defines));
        }
    }

    private static bool SetDefine(ICollection<string> defines, string define, bool enabled)
    {
        var hasDefine = defines.Contains(define);
        if (enabled && !hasDefine)
        {
            defines.Add(define);
            return true;
        }

        if (!enabled && hasDefine)
        {
            defines.Remove(define);
            return true;
        }

        return false;
    }
}
#endif
