using FishNet.Connection;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject spherePrefab;

    private List<GameObject> _spawnedObjects = new();
    private TestSphereObject.TestSphereObjectFactory _factory;

    [Inject]
    private void Construct(TestSphereObject.TestSphereObjectFactory sphereFactory)
    {
        _factory = sphereFactory;
    }

    public void SpawnObject(string id, NetworkConnection conn = null)
    {
        GameObject spawnedObject;
        if (id == "sphere")
            spawnedObject = _factory.Create().gameObject;
        else
            return;
        Spawn(spawnedObject, conn);
        _spawnedObjects.Add(spawnedObject);
    }
}
