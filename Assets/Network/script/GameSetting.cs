using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameSetting : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        GetCommandLineArg();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    string Msg;
    /*
    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 300, 300), Msg);
    }
    */
    private void GetCommandLineArg()
    {
        Debug.Log(NetworkManager.singleton.networkAddress);
        //NetworkManager.singleton.StartClient();
        Msg = "-----------------";
        string[] args = System.Environment.GetCommandLineArgs();
        string input = "";
        for (int i = 0; i < args.Length; i++)
        {
            Debug.Log("ARG " + i + ": " + args[i]);
            Msg += "ARG " + i + ": " + args[i] + "\n";
            if(args[i] == "-server")
            {
                Invoke("StartServer", .3f);
                NetworkManager.singleton.StartServer();
            }
            if(args[i] == "-address")
            {
                NetworkManager.singleton.networkAddress = args[i + 1];
                Invoke("ConnectServer", .3f);
            }
            if(args[i] == "-port")
            {
                NetworkManager.singleton.networkPort = int.Parse(args[i + 1]);
            }
        }
    }
    private void ConnectServer()
    {
        NetworkManager.singleton.StartClient();
    }
    private void StartServer()
    {
        NetworkManager.singleton.StartServer();
    }
}
