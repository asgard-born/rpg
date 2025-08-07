using System;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Camera_Config", menuName = "Configs/Camera_Config")]
    public class CameraConfig : ScriptableObject
    {
        public float MovingSpeed = 5f;
        public float VerticalStep = 2f;
        public float MaxDistanceFromGround = 12f;
        public float MinDistanceFromGround = 6f;
        public float VerticalSmoothTime = .15f;
        
        [NonSerialized] public float RayDistance;

        private void OnValidate()
        {
            RayDistance = MaxDistanceFromGround + 1;
        }
    }
}
