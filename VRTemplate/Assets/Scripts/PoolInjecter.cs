using FishNet.Managing;
using UnityEngine;
using Zenject;

public class PoolInjecter : MonoBehaviour
{
    [SerializeField]
    private NetworkManager networkManager;

    [Inject]
    private void Construct(ZenjectObjectPool objectPool)
    {
        //networkManager.ObjectPool = objectPool;
    }
}
