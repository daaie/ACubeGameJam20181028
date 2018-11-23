using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
    private Dictionary<int, PuzzleBlockInfo> standedBlockList = new Dictionary<int, PuzzleBlockInfo>();
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public ReadySwitch[] readySwitchArr;
    public Transform[] gameStartPositionArr;

    public BlockStage stage_0;
    public BlockStage stage_1;

    public bool isQuad = false;

    // Use this for initialization
    void Start () {
        //CreateBlockStage();
        if (NetworkManager.singleton.maxConnections == 4) isQuad = true;
        else isQuad = false;
    }
    
	
	// Update is called once per frame
	void Update () {
        foreach(int key in standedBlockList.Keys)
        {
            PuzzleBlockInfo info = standedBlockList[key];
            if(info != null)
            {
                info.bs.OnHover(info.pb);
            }
        }
        
    }
    public bool CheckAllReady()
    {
        if (!isServer) return false;

        int checkCount = 0;
        foreach (ReadySwitch rs in readySwitchArr)
        {
            if (rs.isOn)
                checkCount++;
            //-----------Test
            /*
            if (rs.isOn)
            {
                WarpAllPlayer();
                CreateBlockStage();
                return true;
            }
            */
            
            //-------------- -

            //if (!rs.isOn) return false;
        }

        if ((isQuad && checkCount == 4) | (!isQuad && checkCount == 2))
        {
            WarpAllPlayer();
            CreateBlockStage();
            return true;
        }
        else return false;

    }
    private void WarpAllPlayer()
    {
        PlayerMovement[] plays = FindObjectsOfType<PlayerMovement>();
        foreach(PlayerMovement p in plays)
        {
            p.RpcWarpPlayer(gameStartPositionArr[p.myID%4].position);
        }
    }
    private void CreateBlockStage()
    {
        stage_0.Init();
        stage_1.Init();
    }
    public BlockStage GetBlockStage(int idx)
    {
        if (stage_0.index == idx) return stage_0;
        return stage_1;
    }
    public void SetStatedBlocks(int playerID, PuzzleBlockInfo info)
    {
        if(standedBlockList.ContainsKey(playerID))
        {
            standedBlockList[playerID] = info;
        }
        else
        {
            standedBlockList.Add(playerID, info);
        }
    }
}