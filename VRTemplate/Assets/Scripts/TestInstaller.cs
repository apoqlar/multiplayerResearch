using FishNet.Object;
using UnityEngine;
using Zenject;

public class TestInstaller : MonoInstaller
{
    [SerializeField]
    private TestInjectable testInjectable;

    [SerializeField]
    private TestSphereObject testSphere;

    [SerializeField]
    private ScenePooler scenePooler;

    public override void InstallBindings()
    {
        Container.BindInstance(scenePooler);
        Container.BindInstance(testInjectable);
        Container.BindFactory<TestSphereObject, TestSphereObject.TestSphereObjectFactory>().FromComponentInNewPrefab(testSphere);
        Container.BindFactory<UnityEngine.Object, NetworkObject, ZenjectNetworkObjectFactory>().FromFactory<PrefabFactory<NetworkObject>>();
    }
}
