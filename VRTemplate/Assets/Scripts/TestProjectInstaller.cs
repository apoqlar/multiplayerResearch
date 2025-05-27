using FishNet.Object;
using UnityEngine;
using Zenject;

public class TestProjectInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.BindFactory<UnityEngine.Object, NetworkObject, ZenjectNetworkObjectFactory>().FromFactory<PrefabFactory<NetworkObject>>();
    }
}
