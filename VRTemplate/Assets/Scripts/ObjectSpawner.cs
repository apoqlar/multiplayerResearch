using FishNet.Connection;
using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject spherePrefab;

    private List<GameObject> _spawnedObjects = new();

    [ServerRpc(RequireOwnership = false)]
    public void SpawnObject(string id, NetworkConnection conn = null)
    {
        GameObject spawnedObject;
        if (id == "sphere")
            spawnedObject = Instantiate(spherePrefab);
        else
            return;
        Spawn(spawnedObject, conn);
        _spawnedObjects.Add(spawnedObject);
    }
}
