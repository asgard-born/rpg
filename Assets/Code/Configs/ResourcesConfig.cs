using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs
{
    [CreateAssetMenu(fileName = "ResourcesConfig", menuName = "Configs/Resources_Config")]
    public class ResourcesConfig : ScriptableObject
    {
        public AssetReference CharacterPrefab;
    }
}
