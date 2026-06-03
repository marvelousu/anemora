using Unity.Cinemachine;
using UnityEngine;

namespace Anemora.FastVS
{
    [DisallowMultipleComponent]
    public sealed class FastVsHd2dAreaCameraVolume : MonoBehaviour
    {
        [SerializeField] private FastVsHd2dAreaCameraBlendProfile profile;
        [SerializeField] private string profileEntryId = "central_plaza_wide";
        [SerializeField] private string reviewRole = "wide town";
        [SerializeField] private FastVsHouseArea area = FastVsHouseArea.CentralPlaza;
        [SerializeField] private Vector3 triggerLocalCenter;
        [SerializeField] private Vector3 triggerLocalSize = new Vector3(18f, 3f, 16f);
        [SerializeField] private int inactivePriority = 10;
        [SerializeField] private int livePriority = 120;
        [SerializeField] private BoxCollider triggerVolume;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private FastVsHd2dAreaCinemachineBlendRig ownerRig;

        public FastVsHd2dAreaCameraBlendProfile ProfileForReview => profile;
        public string ProfileEntryIdForReview => profileEntryId;
        public string RoleForReview => reviewRole;
        public FastVsHouseArea AreaForReview => area;
        public Vector3 TriggerLocalCenterForReview => triggerLocalCenter;
        public Vector3 TriggerLocalSizeForReview => triggerLocalSize;
        public bool IsTriggerForReview => triggerVolume != null && triggerVolume.isTrigger;
        public CinemachineCamera CinemachineCameraForReview => cinemachineCamera;
        public int PriorityForReview => cinemachineCamera != null ? cinemachineCamera.Priority.Value : int.MinValue;
        public float FieldOfViewForReview => cinemachineCamera != null ? cinemachineCamera.Lens.FieldOfView : 0f;
        public float NearClipPlaneForReview => cinemachineCamera != null ? cinemachineCamera.Lens.NearClipPlane : 0f;
        public FastVsHd2dAreaCinemachineBlendRig OwnerRigForReview => ownerRig;

        public void ConfigureForReview(
            FastVsHd2dAreaCameraBlendProfile cameraProfile,
            FastVsHd2dAreaCameraBlendProfile.Entry entry,
            CinemachineCamera camera,
            int inactive,
            int live,
            FastVsHd2dAreaCinemachineBlendRig owner)
        {
            profile = cameraProfile;
            profileEntryId = entry != null ? entry.EntryIdForReview : profileEntryId;
            reviewRole = entry != null ? entry.ReviewRoleForReview : reviewRole;
            area = entry != null ? entry.AreaForReview : area;
            triggerLocalCenter = entry != null ? entry.TriggerLocalCenterForReview : triggerLocalCenter;
            triggerLocalSize = entry != null ? entry.TriggerLocalSizeForReview : triggerLocalSize;
            inactivePriority = inactive;
            livePriority = Mathf.Max(live, inactive + 1);
            cinemachineCamera = camera != null ? camera : GetComponent<CinemachineCamera>();
            ownerRig = owner;

            triggerVolume = GetComponent<BoxCollider>();
            if (triggerVolume == null)
            {
                triggerVolume = gameObject.AddComponent<BoxCollider>();
            }

            triggerVolume.isTrigger = true;
            triggerVolume.center = triggerLocalCenter;
            triggerVolume.size = triggerLocalSize;
            ApplyPriorityForReview(false);
        }

        public bool ContainsLocalPositionForReview(Vector3 localPosition)
        {
            var halfSize = new Vector3(
                Mathf.Max(0.01f, Mathf.Abs(triggerLocalSize.x)) * 0.5f,
                Mathf.Max(0.01f, Mathf.Abs(triggerLocalSize.y)) * 0.5f,
                Mathf.Max(0.01f, Mathf.Abs(triggerLocalSize.z)) * 0.5f);
            var delta = localPosition - triggerLocalCenter;
            return Mathf.Abs(delta.x) <= halfSize.x &&
                   Mathf.Abs(delta.y) <= halfSize.y &&
                   Mathf.Abs(delta.z) <= halfSize.z;
        }

        public void ApplyPriorityForReview(bool live)
        {
            if (cinemachineCamera == null)
            {
                cinemachineCamera = GetComponent<CinemachineCamera>();
            }

            if (cinemachineCamera == null)
            {
                return;
            }

            cinemachineCamera.Priority.Value = live ? livePriority : inactivePriority;
            cinemachineCamera.Prioritize();
        }
    }
}
