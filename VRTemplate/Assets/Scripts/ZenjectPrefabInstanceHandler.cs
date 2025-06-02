using Unity.Netcode;
using UnityEngine;

public class ZenjectPrefabInstanceHandler : INetworkPrefabInstanceHandler
{
    private readonly GameObject _prefab;
    private readonly ZenjectNgoPrefabFactory _networkObjectFactory;

    public ZenjectPrefabInstanceHandler(ZenjectNgoPrefabFactory factory, GameObject prefab)
    {
        _prefab = prefab;
        _networkObjectFactory = factory;
    }

    public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
    {
        var instance = _networkObjectFactory.Create(_prefab);
        _prefab.transform.SetPositionAndRotation(position, rotation);
        return instance;
    }

    public void Destroy(NetworkObject networkObject)
    {
        Object.Destroy(networkObject.gameObject);
    }
}
