using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Character_Config", menuName = "Configs/Character_Config")]
    public class CharacterConfig : ScriptableObject
    {
        public float RunningSpeed = 2f;
    }
}
