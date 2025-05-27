using FishNet.Connection;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject spherePrefab;

    [SerializeField]
    private GameObject cubePrefab;

    private List<GameObject> _spawnedObjects = new();
    private ZenjectNetworkObjectFactory _factory;

    [Inject]
    private void Construct(ZenjectNetworkObjectFactory networkFactory)
    {
        _factory = networkFactory;
    }

    public void SpawnObject(string id, NetworkConnection conn = null)
    {
        GameObject spawnedObject;
        if (id == "sphere")
            spawnedObject = _factory.Create(spherePrefab).gameObject;
        else if (id == "cube")
        {
            spawnedObject = _factory.Create(cubePrefab).gameObject;
            Debug.Log("Spawning cube");
        }
        else
            return;
        Spawn(spawnedObject, conn);
        _spawnedObjects.Add(spawnedObject);
    }
}
