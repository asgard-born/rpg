using Configs;
using NUnit.Framework;
using Root;
using UnityEngine;

public class EnterPoint : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ResourcesConfig _resourcesConfig;
    [SerializeField] private CharacterConfig _characterConfig;

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
        _root = new GameRoot(
            new GameRoot.Ctx
            {
                ResourcesConfig = _resourcesConfig,
                CharacterConfig = _characterConfig,
                SpawnPoint = _spawnPoint,
            });
    }

    private void ValidateData()
    {
        Assert.IsNotNull(_spawnPoint, "spawn point cannot be null");
        Assert.IsNotNull(_resourcesConfig, "resources config cannot be null");
        Assert.IsNotNull(_characterConfig, "character config cannot be null");
    }
}