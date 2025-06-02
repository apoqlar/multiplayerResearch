using Unity.Netcode;
using UnityEngine;
using Zenject;

public class TestNgoInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindFactory<Object, NetworkObject, ZenjectNgoPrefabFactory>().FromFactory<PrefabFactory<NetworkObject>>();
    }
}
