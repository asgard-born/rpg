using Configs;
using NUnit.Framework;
using Root;
using UniRx;
using UnityEngine;

public class EnterPoint : MonoBehaviour
{
    [SerializeField] private Transform _characterSpawnPoint;
    [SerializeField] private Transform _cameraSpawnPoint;
    [SerializeField] private ResourcesConfig _resourcesConfig;
    [SerializeField] private CharacterConfig _characterConfig;
    [SerializeField] private CameraConfig _cameraConfig;
    [SerializeField] private NavigationConfig _navigationConfig;

    private GameRoot _root;

    private void OnValidate()
    {
        ValidateData();
    }

    private void Start()
    {
        ValidateData();
        InitializeGameRoot();
    }

    private void InitializeGameRoot()
    {
        new GameRoot(
            new GameRoot.Ctx
            {
                ResourcesConfig = _resourcesConfig,
                CharacterConfig = _characterConfig,
                CameraConfig = _cameraConfig,
                NavigationConfig = _navigationConfig,

                CharacterSpawnPoint = _characterSpawnPoint,
                CameraSpawnPoint = _cameraSpawnPoint
            }).AddTo(this);
    }

    private void ValidateData()
    {
        Assert.IsNotNull(_characterSpawnPoint, "character spawn point cannot be null");
        Assert.IsNotNull(_cameraSpawnPoint, "camera spawn point cannot be null");
        Assert.IsNotNull(_resourcesConfig, "Resources config cannot be null");
        Assert.IsNotNull(_characterConfig, "Character config cannot be null");
        Assert.IsNotNull(_cameraConfig, "Camera config cannot be null");
        Assert.IsNotNull(_navigationConfig, "Navigation config cannot be null");
    }
}