using FishNet.Managing.Object;
using FishNet.Object;
using FishNet.Utility.Performance;
using UnityEngine;
using Zenject;
using FishNet.Utility.Extension;
using System;

public class ZenjectObjectPool : ObjectPool
{
    private ScenePooler _scenePooler;

    private void Awake()
    {
        Debug.Log("Awaking pool");
        ScenePooler.PoolCreated += OnPoolCreated;
    }

    private void OnDestroy()
    {
        ScenePooler.PoolCreated -= OnPoolCreated;
    }

    private void OnPoolCreated(ScenePooler obj)
    {
        _scenePooler = obj;
    }

    public override void StoreObject(NetworkObject instantiated, bool asServer)
    {
        Destroy(instantiated.gameObject);
    }

    public override NetworkObject RetrieveObject(int prefabId, ushort collectionId, ObjectPoolRetrieveOption options, Transform parent = null, Vector3? nullablePosition = null, Quaternion? nullableRotation = null, Vector3? nullableScale = null, bool asServer = true)
    {
        NetworkObject prefab = GetPrefab(prefabId, collectionId, asServer);
        return _scenePooler.RetrieveObject(prefab, options, parent, nullablePosition, nullableRotation, nullableScale, asServer);
    }
}
