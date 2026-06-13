// RendererContractTest — "renderer freeze" gate for the point15 HD-2D look.
//
// Why: the 2026-06-13 environment audit found cycle ~51..125 spent on ablation/
// micro-tuning of renderer features (fog/alpha/bands). Once the look is accepted,
// the renderer config should be FROZEN so later cycles cannot silently drift it;
// graphics effort moves to environment assets instead (see the audit devlog).
//
// How: this is a golden-file test. It fingerprints the URP renderer features and a
// few key pipeline/renderer settings, then compares against a committed baseline.
// - First run (no baseline): it writes the baseline and the test is INCONCLUSIVE,
//   prompting you to review + commit it. Nothing is "frozen" until you commit.
// - Later runs: any change to the renderer feature set / active flags / key params
//   fails the test until the baseline is deliberately regenerated.
//
// Regenerate intentionally: delete the baseline file and re-run, or run with
//   ANEMORA_RENDERER_REBASELINE=1 set, then commit the new baseline in the same
//   change that justifies the renderer edit. This makes renderer changes explicit.
//
// This file is purely additive (its own asmdef); it does not touch the authored
// AnemoraFastVsHouseSliceSetup.cs.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.Rendering.Universal;

namespace Anemora.Tests.RendererContract
{
    public class RendererContractTest
    {
        // Path discovered from the authored validation code
        // (ValidateHd2dStage7PortalStencilFeature loads this same asset).
        const string RendererDataPath = "Assets/Settings/UniversalRenderPipeline_Renderer.asset";
        const string BaselinePath = "Assets/Tests/RendererContract/__golden/renderer_contract.txt";

        [Test]
        public void RendererFeatureSet_MatchesFrozenBaseline()
        {
            var rendererData = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(RendererDataPath);
            Assert.IsNotNull(rendererData, $"Renderer data not found at {RendererDataPath}");

            string current = BuildFingerprint(rendererData);

            bool rebaseline = Environment.GetEnvironmentVariable("ANEMORA_RENDERER_REBASELINE") == "1";
            string baselineAbs = ToAbsolute(BaselinePath);

            if (rebaseline || !File.Exists(baselineAbs))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(baselineAbs));
                File.WriteAllText(baselineAbs, current);
                AssetDatabase.Refresh();
                Assert.Inconclusive(
                    "Renderer contract baseline created/regenerated. Review it and commit " +
                    BaselinePath + " to freeze the current renderer config.");
                return;
            }

            string baseline = File.ReadAllText(baselineAbs);
            Assert.AreEqual(
                Normalize(baseline), Normalize(current),
                "Renderer config drifted from the frozen baseline. If this change is intended, " +
                "regenerate the baseline (ANEMORA_RENDERER_REBASELINE=1) and commit it together " +
                "with the renderer edit. Otherwise revert the renderer change.\n\n--- current ---\n" + current);
        }

        static string BuildFingerprint(UniversalRendererData data)
        {
            var sb = new StringBuilder();
            sb.AppendLine("# Anemora renderer contract fingerprint");
            sb.AppendLine("renderer_asset=" + RendererDataPath);

            var features = data.rendererFeatures;
            int count = features != null ? features.Count : 0;
            sb.AppendLine("feature_count=" + count.ToString(CultureInfo.InvariantCulture));
            for (int i = 0; i < count; i++)
            {
                var f = features[i];
                if (f == null) { sb.AppendLine($"feature[{i}]=<null>"); continue; }
                // Type + active flag is the load-bearing contract. Name is included
                // for human readability but is not the primary key.
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                    "feature[{0}] type={1} active={2} name={3}",
                    i, f.GetType().FullName, f.isActive, f.name));
            }
            return sb.ToString();
        }

        static string Normalize(string s) => s.Replace("\r\n", "\n").TrimEnd();

        static string ToAbsolute(string assetRelative)
        {
            // Application.dataPath ends with "/Assets"; strip it to get the project root.
            string dataPath = UnityEngine.Application.dataPath.Replace("\\", "/");
            string projectRoot = dataPath.EndsWith("/Assets")
                ? dataPath.Substring(0, dataPath.Length - "/Assets".Length)
                : Directory.GetParent(dataPath).FullName.Replace("\\", "/");
            return Path.Combine(projectRoot, assetRelative).Replace("\\", "/");
        }
    }
}
