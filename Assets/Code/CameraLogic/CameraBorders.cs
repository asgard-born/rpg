using System;
using UnityEngine;

namespace CameraLogic
{
    [Serializable]
    public struct CameraBorders
    {
        [SerializeField] private float _minX;
        [SerializeField] private float _maxX;
        [SerializeField] private float _minZ;
        [SerializeField] private float _maxZ;
        
        public float MinX => _minX;
        public float MaxX => _maxX;
        public float MinZ => _minZ;
        public float MaxZ => _maxZ;
    }
}