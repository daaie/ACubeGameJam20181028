using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkHUD : NetworkBehaviour
{
    public Canvas start;
    public Canvas selectPlayer;
    public Canvas selectMode;
    public Canvas gameStart;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnGameStart()
    {
        UnSetCanvas(start);
        SetCanvas(selectPlayer);
    }

    public void OpenServer()
    {
        NetworkManager.singleton.StartServer();
    }

    public void OpenHost()
    {
        NetworkManager.singleton.StartHost();        
    }    

    public void ConnectClientToServer()
    {
        NetworkManager.singleton.StartClient();
    }

    public void SetOnePlayer()
    {
        NetworkManager.singleton.maxConnections = 2;
        UnSetCanvas(selectPlayer);
        SetCanvas(selectMode);
    }

    public void SetMultiPlayer()
    {
        NetworkManager.singleton.maxConnections = 4;
        UnSetCanvas(selectPlayer);
        SetCanvas(selectMode);
    }

    public void SetDeathMatchMode()
    {
        NetworkManager.singleton.GetComponent<MainNetworkManager>().mode = MainNetworkManager.Mode.DeathMatch;
        UnSetCanvas(selectMode);
        SetCanvas(gameStart);
    }

    public void SetSheldMode()
    {
        NetworkManager.singleton.GetComponent<MainNetworkManager>().mode = MainNetworkManager.Mode.Sheld;
        UnSetCanvas(selectMode);
        SetCanvas(gameStart);
    }

    void UnSetCanvas(Canvas canvas)
    {
        canvas.enabled = false;
    }

    void SetCanvas(Canvas canvas)
    {
        canvas.enabled = true;
    }
}
