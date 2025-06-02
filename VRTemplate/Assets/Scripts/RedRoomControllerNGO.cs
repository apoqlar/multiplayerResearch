using Unity.Netcode;
using UnityEngine;

public class RedRoomControllerNGO : NetworkBehaviour
{
    [SerializeField]
    private ObjectSpawnerNgo spawnerNgo;

    public void SpawnCubeNgo()
    {
        spawnerNgo.SpawnObject();
    }
}
