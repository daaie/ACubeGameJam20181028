using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CommandLineController : MonoBehaviour {


    void Start()
    {
        GetCommandLineArg();
    }

    private void GetCommandLineArg()
    {
        
        Debug.Log(MainNetworkManager.singleton.networkAddress);
        //NetworkManager.singleton.StartClient();
        string[] args = System.Environment.GetCommandLineArgs();
        string input = "";
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log("ARG " + i + ": " + args[i]);
            //if (args[i] == "-server")
            //{
            //    Invoke("StartServer", .3f);
            //    NetworkManager.singleton.StartServer();
            //}
            if (args[i] == "ADR")
            {
                MainNetworkManager.singleton.networkAddress = args[i + 1];
               // Invoke("ConnectServer", .3f);
            }
            //if (args[i] == "-port")
            //{
            //    NetworkManager.singleton.networkPort = int.Parse(args[i + 1]);
            //}
        }
    }
}
