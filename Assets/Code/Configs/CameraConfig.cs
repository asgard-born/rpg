using System;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Configs/Camera Config")]
    public class CameraConfig : ScriptableObject
    {
        public float MovingSpeed = 5f;
        public float VerticalStep = 2f;
        public float MaxDistanceFromGround = 12f;
        public float MinDistanceFromGround = 6f;
        public LayerMask GroungMask;
        public float VerticalSmoothTime = .15f;
        
        [NonSerialized] public float RayDistance;

        private void OnValidate()
        {
            RayDistance = MaxDistanceFromGround + 1;
        }
    }
}
