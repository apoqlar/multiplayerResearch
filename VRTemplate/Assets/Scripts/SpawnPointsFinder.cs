using FishNet.Component.Spawning;
using FishNet.Managing.Scened;
using System.Linq;
using UnityEngine;

public class SpawnPointsFinder : MonoBehaviour
{
    [SerializeField]
    private PlayerSpawner playerSpawner;

    [SerializeField]
    private SceneManager sceneManager;

    private void Start()
    {
        sceneManager.OnLoadEnd += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (sceneManager)
            sceneManager.OnLoadEnd -= OnSceneLoaded;
    }

    private void OnSceneLoaded(SceneLoadEndEventArgs obj)
    {
        playerSpawner.Spawns = GameObject.FindGameObjectsWithTag("SpawnPoint").Select(x => x.transform).ToArray();
    }
}
