using UnityEngine;

public class StartupController : MonoBehaviour
{
    private void Start()
    {
        var (isServer, port) = IsServer();
        if (isServer)
            SessionManager.Instance.StartServerFlow(port);
    }

    private (bool, int) IsServer()
    {
        var isServer = false;
        int port = 0;
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log("ARG " + i + ": " + args);
            if (args[i] == "ServerInstance")
                isServer = true;
            if (args[i].Contains("port"))
                port = int.Parse(args[i].Split(':')[1]);
        }
        return (isServer, port);
    }
}
