using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MainNetworkManager : NetworkManager
{
    public enum Mode { DeathMatch, Sheld };

    public Mode mode;

    short connectCount = 0;

    public GameObject[] spawnPlayerList;

    public void EndGame()
    {
        StopClient();
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        StopHost();

        connectCount = 0;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*
    public override void OnClientConnect(NetworkConnection conn)
    {
        if (!clientLoadedScene)
        {
            // Ready/AddPlayer is usually triggered by a scene load completing. if no scene was loaded, then Ready/AddPlayer it here instead.
            if (connectCount == 1)
                playerPrefab = alien1;
            else
                playerPrefab = human1;
            connectCount++;
            ClientScene.Ready(conn);            
            ClientScene.AddPlayer(0);
        }

    }
    */

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {

        connectCount %= 4;
        var points = startPositions[connectCount];

        GameObject player = Instantiate(spawnPlayerList[connectCount], points.position, Quaternion.identity);

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        connectCount++;
    }
}
