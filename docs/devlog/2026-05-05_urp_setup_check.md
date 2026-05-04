# 2026-05-05 URP setup check

## Scope
Windows-side Stage 3 Day 1 check for the Unity `6000.3.14f1` project after pulling the Linux-side merge commit `e95a42f`.

## Result
- Pulled the merged Anemora history into `C:\Users\maro6\Documents\Unity\Anemora`.
- Unity Personal license is active and batchmode starts successfully.
- Windows Standalone support is registered by the Editor.
- `com.unity.render-pipelines.universal` resolves as URP `17.3.0`.
- Package resolution initially failed because `com.unity.collab-proxy@2.9.8` could not be found. Removed `com.unity.collab-proxy` from `Packages/manifest.json` because Unity Version Control is not required for Stage 3.
- Unity regenerated `Packages/packages-lock.json` successfully after the manifest fix.
- A later import generated Unity's URP global support assets (`Assets/UniversalRenderPipelineGlobalSettings.asset`, `Assets/DefaultVolumeProfile.asset`) and Shader Graph project settings.

## URP settings status
- URP package is installed and resolved.
- URP global settings are registered in `ProjectSettings/GraphicsSettings.asset`.
- `Assets/Settings/` does not yet contain URP Pipeline or Renderer assets.
- `ProjectSettings/GraphicsSettings.asset` still has `m_CustomRenderPipeline: {fileID: 0}`.
- `ProjectSettings/QualitySettings.asset` still has `customRenderPipeline: {fileID: 0}` for all quality levels.

## Next
Create and assign the URP Pipeline Asset and Forward Renderer asset before implementing the Time Frame portal prototype. This should be done as part of the Stage 3 E-track Unity setup work, alongside the Renderer Feature / stencil design from ADR-0002.
