using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.IO;

public class TitleSceneManager : MonoBehaviour {

    public GameObject gameStart;
    public GameObject selectPlay;
    public GameObject selectMode;

    public void OnGameStart()
    {
        SoundEffectManager.Instance.PlaySound(EffectType.Button);
        gameStart.SetActive(false);
        selectPlay.SetActive(true);
    }

    public void SetOnePlayer()
    {
        SoundEffectManager.Instance.PlaySound(EffectType.Button);
        NetworkManager.singleton.maxConnections = 2;
        selectPlay.SetActive(false);
        selectMode.SetActive(true);
    }

    public void SetMultiPlayer()
    {
        SoundEffectManager.Instance.PlaySound(EffectType.Button);
        NetworkManager.singleton.maxConnections = 4;
        selectPlay.SetActive(false);
        selectMode.SetActive(true);       
    }

    public void SetDeathMatchMode()
    {
        SoundEffectManager.Instance.PlaySound(EffectType.Button);
        NetworkManager.singleton.GetComponent<MainNetworkManager>().mode = MainNetworkManager.Mode.DeathMatch;
        GameStart();
    }

    public void SetSheldMode()
    {
        SoundEffectManager.Instance.PlaySound(EffectType.Button);
        NetworkManager.singleton.GetComponent<MainNetworkManager>().mode = MainNetworkManager.Mode.Sheld;
        GameStart();
    }

    public void GameStart()
    {
        if(SettingManager.Instance.Setting.NetworkAddress == "Host")
        {
            NetworkManager.singleton.StartHost();
        }
        else
        {
            NetworkManager.singleton.networkAddress = SettingManager.Instance.Setting.NetworkAddress;
            NetworkManager.singleton.StartClient();
        }
    }
}
