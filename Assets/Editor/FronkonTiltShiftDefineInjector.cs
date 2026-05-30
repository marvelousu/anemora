#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.Build;

[InitializeOnLoad]
internal static class FronkonTiltShiftDefineInjector
{
    private const string Define = "FRONKON_TILTSHIFT";

    static FronkonTiltShiftDefineInjector()
    {
        var present = System.AppDomain.CurrentDomain.GetAssemblies()
            .Any(assembly => assembly.GetType("FronkonGames.Artistic.TiltShift.TiltShiftVolume") != null);
        var target = NamedBuildTarget.FromBuildTargetGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        var defines = PlayerSettings.GetScriptingDefineSymbols(target)
            .Split(';')
            .Where(symbol => symbol.Length > 0)
            .ToList();
        var hasDefine = defines.Contains(Define);

        if (present && !hasDefine)
        {
            defines.Add(Define);
            PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", defines));
        }
        else if (!present && hasDefine)
        {
            defines.Remove(Define);
            PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", defines));
        }
    }
}
#endif
