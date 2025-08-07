using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Camera_Config", menuName = "Configs/Camera_Config")]
    public class CameraConfig : ScriptableObject
    {
        public float MovingSpeed = 5f;
        public float VerticalStep = 2f;
        public float MaxHeight = 12f;
        public float MinHeight = 6f;
        public float CheckingSphereRadius = 5f;
        public float VerticalSmoothTime = .15f;
        public float MaxDistanceFromPlayerXZ = 20f;
    }
}
