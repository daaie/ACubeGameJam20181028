using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : Singleton<GameResult> {

    public enum Team { Unknown, Human, Alien }

    public Team myTeam = Team.Unknown;
    public Team winner = Team.Unknown;   

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance != null)
            Destroy(gameObject);
    }
}
