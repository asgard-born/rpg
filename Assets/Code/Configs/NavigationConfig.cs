using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Navigation_Config", menuName = "Configs/Navigation_Config")]
    public class NavigationConfig : ScriptableObject
    {
        public LayerMask GroundLayer;
        public float MaxSearchDistance = 10f;
    }
}