using Unity.Netcode;
using UnityEngine;
using Zenject;

public class ObjectSpawnerNgo : MonoBehaviour
{
    [SerializeField]
    private GameObject somePrefab;

    private ZenjectNgoPrefabFactory _factory;

    [Inject]
    private void Construct(ZenjectNgoPrefabFactory prefabFactory)
    {
        _factory = prefabFactory;
    }

    void Start()
    {
        foreach (var entry in NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs)
        {
            if (entry.Prefab != null)
            {
                var handler = new ZenjectPrefabInstanceHandler(_factory, entry.Prefab);
                NetworkManager.Singleton.PrefabHandler.AddHandler(entry.Prefab, handler);
            }
        }
    }

    public void SpawnObject()
    {
        var spawnedObject = _factory.Create(somePrefab).gameObject;
        spawnedObject.GetComponent<NetworkObject>().Spawn();
    }
}
