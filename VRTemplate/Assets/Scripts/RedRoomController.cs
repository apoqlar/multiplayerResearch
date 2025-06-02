using FishNet.Object;
using UnityEngine;

public class RedRoomController : NetworkBehaviour
{
    [SerializeField]
    private ObjectSpawner spawner;

    [SerializeField]
    private ObjectSpawnerNgo spawnerNgo;

    public void SpawnSphere()
    {
        spawner.SpawnObject("sphere", LocalConnection);
    }

    public void SpawnCube()
    {
        spawner.SpawnObject("cube", LocalConnection);
    }

    public void SpawnCubeNgo()
    {
        spawnerNgo.SpawnObject();
    }
}
