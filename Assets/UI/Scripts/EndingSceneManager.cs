using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour {

    public GameObject Human;
    public GameObject Alien;

    public GameObject youLose;
    public GameObject youWin;
    

	// Use this for initialization
	void Start () {

        Human.SetActive(false);
        Alien.SetActive(false);
        youLose.SetActive(false);
        youWin.SetActive(false);

        Alien.SetActive(GameResult.Instance.myTeam == GameResult.Team.Alien);
        Human.SetActive(GameResult.Instance.myTeam == GameResult.Team.Human);
        youWin.SetActive(GameResult.Instance.winner == GameResult.Instance.myTeam);
        youLose.SetActive(GameResult.Instance.winner != GameResult.Instance.myTeam);
	}

    public void OnRestartPlay()
    {
        //Destroy(GameObject.FindObjectOfType<MainNetworkManager>().gameObject);
        Destroy(GameObject.FindObjectOfType<GameResult>().gameObject);

        GameObject.Find("NetworkManager").GetComponent<MainNetworkManager>().EndGame();
        //SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
