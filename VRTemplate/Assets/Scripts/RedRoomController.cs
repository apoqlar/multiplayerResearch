using FishNet.Object;
using UnityEngine;

public class RedRoomController : NetworkBehaviour
{
    [SerializeField]
    private ObjectSpawner spawner;

    public void SpawnSphere()
    {
        spawner.SpawnObject("sphere", LocalConnection);
    }

    public void SpawnCube()
    {
        spawner.SpawnObject("cube", LocalConnection);
    }
}
