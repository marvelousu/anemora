using System.Collections.Generic;
using UnityEngine;

namespace Anemora.FastVS
{
    public enum FastVsDirectionalSpriteBakeFacingMode
    {
        Four = 4,
        Eight = 8,
    }

    [CreateAssetMenu(menuName = "Anemora/Fast VS/Directional Sprite Bake Set")]
    public sealed class FastVsDirectionalSpriteBakeSet : ScriptableObject
    {
        [SerializeField] private string bakeSetId = "niro_p1_directional_bake";
        [SerializeField] private FastVsDirectionalSpriteBakeFacingMode facingMode = FastVsDirectionalSpriteBakeFacingMode.Four;
        [SerializeField] private int frameWidth = 64;
        [SerializeField] private int frameHeight = 96;
        [SerializeField] private int frameCount = 4;
        [SerializeField] private float cameraPitchDegrees = 30f;
        [SerializeField] private float turntableStepDegrees = 90f;
        [SerializeField] private string sourceModelPath = "NEEDS-TOM";
        [SerializeField] private string bakeMethod = "orthographic turntable RenderTexture bake contract";
        [SerializeField] private string[] facingLabels = { "Front", "Back", "Left", "Right" };
        [SerializeField] private Texture2D[] diffuseSheets = new Texture2D[4];
        [SerializeField] private Texture2D[] normalSheets = new Texture2D[4];
        [SerializeField] private Texture2D registeredDiffuseSheet;
        [SerializeField] private Texture2D registeredNormalSheet;
        [SerializeField] private string reviewNotes = string.Empty;

        public string BakeSetIdForReview => bakeSetId;
        public FastVsDirectionalSpriteBakeFacingMode FacingModeForReview => facingMode;
        public int FacingCountForReview => Mathf.Clamp((int)facingMode, 1, 8);
        public int FrameWidthForReview => frameWidth;
        public int FrameHeightForReview => frameHeight;
        public int FrameCountForReview => frameCount;
        public float CameraPitchDegreesForReview => cameraPitchDegrees;
        public float TurntableStepDegreesForReview => turntableStepDegrees;
        public string SourceModelPathForReview => sourceModelPath;
        public string BakeMethodForReview => bakeMethod;
        public Texture2D RegisteredDiffuseSheetForReview => registeredDiffuseSheet;
        public Texture2D RegisteredNormalSheetForReview => registeredNormalSheet;
        public string ReviewNotesForReview => reviewNotes;

        public bool HasRegisteredDiffuseAndNormalSheetsForReview =>
            registeredDiffuseSheet != null &&
            registeredNormalSheet != null &&
            MissingSlotCountForReview == 0;

        public int MissingSlotCountForReview
        {
            get
            {
                var count = FacingCountForReview;
                var missing = 0;
                for (var index = 0; index < count; index++)
                {
                    if (ResolveDiffuseSheetForReview(index) == null)
                    {
                        missing++;
                    }

                    if (ResolveNormalSheetForReview(index) == null)
                    {
                        missing++;
                    }
                }

                return missing;
            }
        }

        public void ConfigureForReview(
            string nextBakeSetId,
            FastVsDirectionalSpriteBakeFacingMode nextFacingMode,
            int nextFrameWidth,
            int nextFrameHeight,
            int nextFrameCount,
            float nextCameraPitchDegrees,
            string nextSourceModelPath,
            string nextBakeMethod,
            IReadOnlyList<string> nextFacingLabels,
            IReadOnlyList<Texture2D> nextDiffuseSheets,
            IReadOnlyList<Texture2D> nextNormalSheets,
            Texture2D nextRegisteredDiffuseSheet,
            Texture2D nextRegisteredNormalSheet,
            string nextReviewNotes)
        {
            bakeSetId = string.IsNullOrWhiteSpace(nextBakeSetId) ? bakeSetId : nextBakeSetId;
            facingMode = nextFacingMode;
            frameWidth = Mathf.Max(1, nextFrameWidth);
            frameHeight = Mathf.Max(1, nextFrameHeight);
            frameCount = Mathf.Max(1, nextFrameCount);
            cameraPitchDegrees = nextCameraPitchDegrees;
            turntableStepDegrees = 360f / Mathf.Max(1, (int)nextFacingMode);
            sourceModelPath = string.IsNullOrWhiteSpace(nextSourceModelPath) ? "NEEDS-TOM" : nextSourceModelPath;
            bakeMethod = string.IsNullOrWhiteSpace(nextBakeMethod)
                ? "orthographic turntable RenderTexture bake contract"
                : nextBakeMethod;
            facingLabels = CopyStrings(nextFacingLabels, FacingCountForReview);
            diffuseSheets = CopyTextures(nextDiffuseSheets, FacingCountForReview);
            normalSheets = CopyTextures(nextNormalSheets, FacingCountForReview);
            registeredDiffuseSheet = nextRegisteredDiffuseSheet;
            registeredNormalSheet = nextRegisteredNormalSheet;
            reviewNotes = nextReviewNotes ?? string.Empty;
        }

        public Texture2D ResolveDiffuseSheetForReview(int facingIndex)
        {
            return ResolveSheet(diffuseSheets, facingIndex);
        }

        public Texture2D ResolveNormalSheetForReview(int facingIndex)
        {
            return ResolveSheet(normalSheets, facingIndex);
        }

        public string ResolveFacingLabelForReview(int facingIndex)
        {
            if (facingLabels == null || facingLabels.Length == 0)
            {
                return facingIndex.ToString();
            }

            return facingLabels[Mathf.Clamp(facingIndex, 0, facingLabels.Length - 1)];
        }

        public int ResolveFacingIndexForReview(Vector3 cameraForward, Vector3 characterForward)
        {
            var facingCount = FacingCountForReview;
            var projectedCharacterForward = ProjectHorizontal(characterForward);
            var projectedViewToCamera = ProjectHorizontal(-cameraForward);
            if (projectedCharacterForward.sqrMagnitude < 0.0001f || projectedViewToCamera.sqrMagnitude < 0.0001f)
            {
                return 0;
            }

            var signedAngle = Vector3.SignedAngle(projectedCharacterForward, projectedViewToCamera, Vector3.up);
            var step = 360f / facingCount;
            return Mathf.FloorToInt(Mathf.Repeat(signedAngle + step * 0.5f, 360f) / step) % facingCount;
        }

        private static Texture2D ResolveSheet(IReadOnlyList<Texture2D> sheets, int facingIndex)
        {
            if (sheets == null || sheets.Count == 0)
            {
                return null;
            }

            return sheets[Mathf.Clamp(facingIndex, 0, sheets.Count - 1)];
        }

        private static string[] CopyStrings(IReadOnlyList<string> source, int count)
        {
            var copy = new string[count];
            for (var index = 0; index < count; index++)
            {
                copy[index] = source != null && index < source.Count && !string.IsNullOrWhiteSpace(source[index])
                    ? source[index]
                    : index.ToString();
            }

            return copy;
        }

        private static Texture2D[] CopyTextures(IReadOnlyList<Texture2D> source, int count)
        {
            var copy = new Texture2D[count];
            for (var index = 0; index < count; index++)
            {
                copy[index] = source != null && index < source.Count ? source[index] : null;
            }

            return copy;
        }

        private static Vector3 ProjectHorizontal(Vector3 direction)
        {
            direction.y = 0f;
            return direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector3.zero;
        }
    }

}
