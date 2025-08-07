using Navigation;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Configs
{
    [CreateAssetMenu(fileName = "Resources_Config", menuName = "Configs/Resources_Config")]
    public class ResourcesConfig : ScriptableObject
    {
        public AssetReference CharacterViewReference;
        public AssetReference CameraViewReference;
        public NavigationArrows NavigationArrowsPrefab;
    }
}