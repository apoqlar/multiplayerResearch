using FishNet.Managing.Object;
using FishNet.Object;
using FishNet.Utility.Performance;
using UnityEngine;
using Zenject;
using FishNet.Utility.Extension;
using System;

public class ScenePooler : MonoBehaviour
{
    public static event Action<ScenePooler> PoolCreated;

    private ZenjectNetworkObjectFactory _factory;

    [Inject]
    private void Construct(ZenjectNetworkObjectFactory prefabFactory)
    {
        Debug.Log("Constructing scene pool");
        _factory = prefabFactory;
        PoolCreated?.Invoke(this);
    }

    public NetworkObject RetrieveObject(NetworkObject prefab, ObjectPoolRetrieveOption options, Transform parent = null, Vector3? nullablePosition = null, Quaternion? nullableRotation = null, Vector3? nullableScale = null, bool asServer = true)
    {
        bool makeActive = options.FastContains(ObjectPoolRetrieveOption.MakeActive);
        bool localSpace = options.FastContains(ObjectPoolRetrieveOption.LocalSpace);

        if (prefab == null)
        {
            return null;
        }
        else
        {
            NetworkObject result = _factory.Create(prefab);
            Vector3 scale;

            if (localSpace)
            {
                prefab.transform.OutLocalPropertyValues(nullablePosition, nullableRotation, nullableScale, out Vector3 pos, out Quaternion rot, out scale);
                if (parent != null)
                {
                    //Convert pos and rot to world values for the instantiate.
                    pos = parent.TransformPoint(pos);
                    rot = (parent.rotation * rot);
                }
                result.transform.SetParent(parent);
                result.transform.SetPositionAndRotation(pos, rot);
            }
            else
            {
                prefab.transform.OutWorldPropertyValues(nullablePosition, nullableRotation, nullableScale, out Vector3 pos, out Quaternion rot, out scale);
                result.transform.SetParent(parent);
                result.transform.SetPositionAndRotation(pos, rot);
            }

            result.transform.localScale = scale;

            if (makeActive)
                result.gameObject.SetActive(true);
            return result;
        }
    }
}
