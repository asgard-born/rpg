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

                CharacterSpawnPoint = _characterSpawnPoint,
                CameraSpawnPoint = _cameraSpawnPoint
            }).AddTo(this);
    }

    private void ValidateData()
    {
        Assert.IsNotNull(_characterSpawnPoint, "spawn point cannot be null");
        Assert.IsNotNull(_resourcesConfig, "resources config cannot be null");
        Assert.IsNotNull(_characterConfig, "character config cannot be null");
    }
}