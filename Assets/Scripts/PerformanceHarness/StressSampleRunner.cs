using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Anemora.TimeManagement;
using Anemora.UI;
using Unity.Profiling;
using UnityEngine;

namespace Anemora.PerformanceHarness
{
    /// <summary>
    /// Stage 4 scaffold for deterministic stress sampling of portal, symbol, and dialogue flows.
    /// </summary>
    public sealed class StressSampleRunner : MonoBehaviour
    {
        private const string DrawObjectsPassWarning =
            "DrawObjectsPass does not have an implementation of the RecordRenderGraph method";

        [Header("Targets")]
        [SerializeField] private TimeFramePortalController portalController;
        [SerializeField] private Transform player;
        [SerializeField] private MonoBehaviour[] dialogueInteractables;

        [Header("Run")]
        [SerializeField] private bool runOnStart;
        [SerializeField] private float durationSeconds = 30f;
        [SerializeField] private float stepDelaySeconds = 1f;
        [SerializeField] private bool includePortalCrossing = true;
        [SerializeField] private bool includeDialogueTrigger = true;
        [SerializeField] private bool closePortalAfterOpen = true;
        [SerializeField] private string outputFileName = "stress_sample_result.json";

        private readonly List<float> frameTimes = new();
        private readonly List<long> gcUsedMemorySamples = new();
        private readonly List<long> totalUsedMemorySamples = new();
        private readonly List<long> monoHeapSamples = new();

        private Coroutine runRoutine;
        private ProfilerRecorder gcUsedMemory;
        private ProfilerRecorder totalUsedMemory;
        private ProfilerRecorder monoUsedMemory;
        private int urpDrawObjectsWarningCount;
        private int portalOpenCount;
        private int portalCloseCount;
        private int portalCrossingCount;
        private int dialogueTriggerCount;
        private int dialogueTriggerSuccessCount;
        private float runStartTime;
        private float nextMemorySampleTime;

        public bool IsRunning => runRoutine != null;
        public StressSampleResult LastResult { get; private set; }

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLogMessage;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLogMessage;
            StopSample();
        }

        private void Start()
        {
            if (runOnStart)
            {
                StartSample();
            }
        }

        public void StartSample()
        {
            if (runRoutine != null)
            {
                return;
            }

            ResolveTargets();
            ResetCounters();
            StartRecorders();
            runRoutine = StartCoroutine(RunSampleRoutine());
        }

        public void StopSample()
        {
            if (runRoutine != null)
            {
                StopCoroutine(runRoutine);
                runRoutine = null;
            }

            StopRecorders();
        }

        public void RunSingleStepForSmoke()
        {
            ResolveTargets();
            ResetCounters();
            OpenPortal();
            ClosePortal();
            LastResult = BuildResult(Mathf.Max(Time.unscaledDeltaTime, 0.0001f));
        }

        private IEnumerator RunSampleRoutine()
        {
            runStartTime = Time.realtimeSinceStartup;
            nextMemorySampleTime = runStartTime;

            while (Time.realtimeSinceStartup - runStartTime < durationSeconds)
            {
                var stepStart = Time.realtimeSinceStartup;
                OpenPortal();
                yield return WaitAndSample(stepDelaySeconds);

                if (includePortalCrossing)
                {
                    TriggerPortalCrossing();
                    yield return WaitAndSample(stepDelaySeconds);
                }

                if (includeDialogueTrigger)
                {
                    TriggerDialogue();
                    yield return WaitAndSample(stepDelaySeconds);
                }

                if (closePortalAfterOpen)
                {
                    ClosePortal();
                    yield return WaitAndSample(stepDelaySeconds);
                }

                if (Time.realtimeSinceStartup <= stepStart)
                {
                    yield return null;
                }
            }

            var duration = Mathf.Max(Time.realtimeSinceStartup - runStartTime, 0.0001f);
            LastResult = BuildResult(duration);
            WriteResult(LastResult);
            StopRecorders();
            runRoutine = null;
        }

        private IEnumerator WaitAndSample(float seconds)
        {
            var end = Time.realtimeSinceStartup + Mathf.Max(0f, seconds);
            do
            {
                frameTimes.Add(Time.unscaledDeltaTime);
                if (Time.realtimeSinceStartup >= nextMemorySampleTime)
                {
                    RecordMemorySample();
                    nextMemorySampleTime += 1f;
                }

                yield return null;
            }
            while (Time.realtimeSinceStartup < end);
        }

        private void OpenPortal()
        {
            if (portalController == null)
            {
                return;
            }

            portalController.HandleSymbolSelected(SymbolType.Red);
            portalOpenCount++;
        }

        private void ClosePortal()
        {
            if (portalController == null)
            {
                return;
            }

            portalController.ClosePortal();
            portalCloseCount++;
        }

        private void TriggerPortalCrossing()
        {
            if (portalController == null)
            {
                return;
            }

            portalController.TriggerCrossingForTests();
            portalCrossingCount++;
        }

        private void TriggerDialogue()
        {
            if (dialogueInteractables == null || dialogueInteractables.Length == 0)
            {
                return;
            }

            foreach (var interactable in dialogueInteractables)
            {
                if (interactable == null)
                {
                    continue;
                }

                MovePlayerNear(interactable.transform);
                dialogueTriggerCount++;
                if (TryInvokeBoolMethod(interactable, "TryInteract"))
                {
                    dialogueTriggerSuccessCount++;
                    return;
                }
            }
        }

        private void MovePlayerNear(Transform target)
        {
            if (player == null || target == null)
            {
                return;
            }

            player.position = target.position + new Vector3(0.5f, 0f, 0.5f);
        }

        private void ResolveTargets()
        {
            if (portalController == null)
            {
                portalController = FindFirstObjectByType<TimeFramePortalController>();
            }

            if (player == null)
            {
                var playerObject = GameObject.FindWithTag("Player");
                if (playerObject != null)
                {
                    player = playerObject.transform;
                }
            }

            if (dialogueInteractables == null || dialogueInteractables.Length == 0)
            {
                dialogueInteractables = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                    .Where(IsNpcInteractable)
                    .ToArray();
            }
        }

        private static bool IsNpcInteractable(MonoBehaviour behaviour)
        {
            return behaviour != null && behaviour.GetType().FullName == "Anemora.Dialogue.NpcInteractable";
        }

        private static bool TryInvokeBoolMethod(MonoBehaviour target, string methodName)
        {
            var method = target.GetType().GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            if (method == null || method.ReturnType != typeof(bool))
            {
                return false;
            }

            return (bool)method.Invoke(target, Array.Empty<object>());
        }

        private void ResetCounters()
        {
            frameTimes.Clear();
            gcUsedMemorySamples.Clear();
            totalUsedMemorySamples.Clear();
            monoHeapSamples.Clear();
            urpDrawObjectsWarningCount = 0;
            portalOpenCount = 0;
            portalCloseCount = 0;
            portalCrossingCount = 0;
            dialogueTriggerCount = 0;
            dialogueTriggerSuccessCount = 0;
            LastResult = null;
        }

        private void StartRecorders()
        {
            StopRecorders();
            gcUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Used Memory");
            totalUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory");
            monoUsedMemory = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Mono Used Memory");
        }

        private void StopRecorders()
        {
            if (gcUsedMemory.Valid)
            {
                gcUsedMemory.Dispose();
            }

            if (totalUsedMemory.Valid)
            {
                totalUsedMemory.Dispose();
            }

            if (monoUsedMemory.Valid)
            {
                monoUsedMemory.Dispose();
            }
        }

        private void RecordMemorySample()
        {
            AddRecorderValue(gcUsedMemory, gcUsedMemorySamples);
            AddRecorderValue(totalUsedMemory, totalUsedMemorySamples);
            AddRecorderValue(monoUsedMemory, monoHeapSamples);
        }

        private static void AddRecorderValue(ProfilerRecorder recorder, List<long> samples)
        {
            if (recorder.Valid)
            {
                samples.Add(recorder.LastValue);
            }
        }

        private StressSampleResult BuildResult(float duration)
        {
            var sortedFrameTimes = frameTimes.OrderBy(value => value).ToArray();
            return new StressSampleResult
            {
                durationSeconds = duration,
                frameCount = frameTimes.Count,
                averageFps = frameTimes.Count / Mathf.Max(duration, 0.0001f),
                averageFrameMs = AverageSeconds(frameTimes) * 1000f,
                p95FrameMs = Percentile(sortedFrameTimes, 0.95f) * 1000f,
                p99FrameMs = Percentile(sortedFrameTimes, 0.99f) * 1000f,
                maxFrameMs = sortedFrameTimes.Length == 0 ? 0f : sortedFrameTimes[sortedFrameTimes.Length - 1] * 1000f,
                gcUsedMemoryStartMiB = FirstMiB(gcUsedMemorySamples),
                gcUsedMemoryEndMiB = LastMiB(gcUsedMemorySamples),
                gcUsedMemoryPeakMiB = PeakMiB(gcUsedMemorySamples),
                totalUsedMemoryPeakMiB = PeakMiB(totalUsedMemorySamples),
                monoHeapPeakMiB = PeakMiB(monoHeapSamples),
                urpDrawObjectsWarningCount = urpDrawObjectsWarningCount,
                portalOpenCount = portalOpenCount,
                portalCloseCount = portalCloseCount,
                portalCrossingCount = portalCrossingCount,
                dialogueTriggerCount = dialogueTriggerCount,
                dialogueTriggerSuccessCount = dialogueTriggerSuccessCount
            };
        }

        private void WriteResult(StressSampleResult result)
        {
            if (string.IsNullOrWhiteSpace(outputFileName))
            {
                return;
            }

            var path = Path.IsPathRooted(outputFileName)
                ? outputFileName
                : Path.Combine(Application.persistentDataPath, outputFileName);
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(path, ToJson(result), Encoding.UTF8);
        }

        private static string ToJson(StressSampleResult result)
        {
            var builder = new StringBuilder();
            builder.AppendLine("{");
            Append(builder, "durationSeconds", result.durationSeconds, true);
            Append(builder, "frameCount", result.frameCount, true);
            Append(builder, "averageFps", result.averageFps, true);
            Append(builder, "averageFrameMs", result.averageFrameMs, true);
            Append(builder, "p95FrameMs", result.p95FrameMs, true);
            Append(builder, "p99FrameMs", result.p99FrameMs, true);
            Append(builder, "maxFrameMs", result.maxFrameMs, true);
            Append(builder, "gcUsedMemoryStartMiB", result.gcUsedMemoryStartMiB, true);
            Append(builder, "gcUsedMemoryEndMiB", result.gcUsedMemoryEndMiB, true);
            Append(builder, "gcUsedMemoryPeakMiB", result.gcUsedMemoryPeakMiB, true);
            Append(builder, "totalUsedMemoryPeakMiB", result.totalUsedMemoryPeakMiB, true);
            Append(builder, "monoHeapPeakMiB", result.monoHeapPeakMiB, true);
            Append(builder, "urpDrawObjectsWarningCount", result.urpDrawObjectsWarningCount, true);
            Append(builder, "portalOpenCount", result.portalOpenCount, true);
            Append(builder, "portalCloseCount", result.portalCloseCount, true);
            Append(builder, "portalCrossingCount", result.portalCrossingCount, true);
            Append(builder, "dialogueTriggerCount", result.dialogueTriggerCount, true);
            Append(builder, "dialogueTriggerSuccessCount", result.dialogueTriggerSuccessCount, false);
            builder.AppendLine("}");
            return builder.ToString();
        }

        private static void Append(StringBuilder builder, string key, float value, bool comma)
        {
            builder
                .Append("  \"")
                .Append(key)
                .Append("\": ")
                .Append(value.ToString("0.###", CultureInfo.InvariantCulture))
                .AppendLine(comma ? "," : string.Empty);
        }

        private static void Append(StringBuilder builder, string key, int value, bool comma)
        {
            builder
                .Append("  \"")
                .Append(key)
                .Append("\": ")
                .Append(value.ToString(CultureInfo.InvariantCulture))
                .AppendLine(comma ? "," : string.Empty);
        }

        private static float AverageSeconds(IReadOnlyCollection<float> values)
        {
            return values.Count == 0 ? 0f : values.Sum() / values.Count;
        }

        private static float Percentile(IReadOnlyList<float> sortedValues, float percentile)
        {
            if (sortedValues.Count == 0)
            {
                return 0f;
            }

            var index = Mathf.Clamp(Mathf.CeilToInt(sortedValues.Count * percentile) - 1, 0, sortedValues.Count - 1);
            return sortedValues[index];
        }

        private static float FirstMiB(IReadOnlyList<long> samples)
        {
            return samples.Count == 0 ? 0f : BytesToMiB(samples[0]);
        }

        private static float LastMiB(IReadOnlyList<long> samples)
        {
            return samples.Count == 0 ? 0f : BytesToMiB(samples[samples.Count - 1]);
        }

        private static float PeakMiB(IReadOnlyList<long> samples)
        {
            return samples.Count == 0 ? 0f : BytesToMiB(samples.Max());
        }

        private static float BytesToMiB(long bytes)
        {
            return bytes / 1048576f;
        }

        private void HandleLogMessage(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Warning && condition.Contains(DrawObjectsPassWarning))
            {
                urpDrawObjectsWarningCount++;
            }
        }
    }

    [Serializable]
    public sealed class StressSampleResult
    {
        public float durationSeconds;
        public int frameCount;
        public float averageFps;
        public float averageFrameMs;
        public float p95FrameMs;
        public float p99FrameMs;
        public float maxFrameMs;
        public float gcUsedMemoryStartMiB;
        public float gcUsedMemoryEndMiB;
        public float gcUsedMemoryPeakMiB;
        public float totalUsedMemoryPeakMiB;
        public float monoHeapPeakMiB;
        public int urpDrawObjectsWarningCount;
        public int portalOpenCount;
        public int portalCloseCount;
        public int portalCrossingCount;
        public int dialogueTriggerCount;
        public int dialogueTriggerSuccessCount;
    }
}
