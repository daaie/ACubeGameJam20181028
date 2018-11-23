using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BlockStage : NetworkBehaviour {
    public GameObject block;
    public GameObject ballObj;
    public int index = 0;
    private PuzzleBlock[,] blockMap = new PuzzleBlock[8, 12];

    public Transform enemyTarget;
    
    private List<PuzzleBlock> hoveredBlock;
    // Use this for initialization
    void Start()
    {
        hoveredBlock = new List<PuzzleBlock>();
    }
    private void Update()
    {
        if(isServer)
        {
            DisableAllBlock();

            foreach (PuzzleBlock pb in hoveredBlock)
            {
                if(pb !=null)
                {
                    foreach (PuzzleBlock block in GetAroundBlock(pb))
                    {
                        block.RpcEnterChar();
                    }
                }
                
            }
            //UpdateHoveredAllBlock();
            hoveredBlock.Clear();
        }
        
    }
    private bool isInit = false;
    public void Init()
    {
        isInit = true;
        if (isServer) CreateBlocks();
    }
    
    //[Command]
    public void CreateBlocks()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 12; y++)
            {
                GameObject go = Instantiate(block);
                go.transform.parent = transform;
                go.transform.localScale = Vector3.one * .9f;
                go.transform.localPosition = new Vector3(x, 0, y);
                PuzzleBlock pb = go.GetComponent<PuzzleBlock>();
                pb.x = x;
                pb.y = y;
                pb.SetBlockIndex(Random.Range(0, 4));

                pb.parent = this;
                pb.parentIndex = this.index;
                blockMap[x, y] = pb;

                NetworkServer.Spawn(go);
            }
        }
    }
    /*
    private void UpdateHoveredAllBlock()
    {
        if(isServer)
        {
            foreach (PuzzleBlock block in blockMap)
            {
                block.UpdateHovered();
            }
        }
    }
    */
    private void DisableAllBlock()
    {
        if (!isInit) return;
        foreach(PuzzleBlock block in blockMap)
        {
            block.RpcExitChar();
        }
    }
    public void SelectBlock(int x, int y)
    {
        PuzzleBlock pb = GetBlock(x, y);
        if (pb.blockIndex == -1) return;
        List<PuzzleBlock> blockList = GetAroundBlock(pb);
        bool isFire = blockList.Count >= 2;
        foreach (PuzzleBlock block in blockList)
        {
            if (isFire)
            {
                GameObject go = Instantiate(ballObj);
                //Debug.Log("---->    " + block.blockIndex);
                go.transform.position = block.transform.position + new Vector3(0, 1, 0);
                go.GetComponent<AttackBall>().Fire(index == 0 ? 1 : -1, block.blockColor);

                NetworkServer.Spawn(go);
            }
            block.SelectBlock();
        }

        if (isFire)
            RpcShotSound();

    }

    [ClientRpc]
    void RpcShotSound()
    {
        SoundEffectManager.Instance.PlaySound(EffectType.ShotCube);
    }

    //[Command]
    public void CmdOnHover(int x, int y)
    {
        //Debug.Log("CmdOnHover " + x + "   " + y);
        PuzzleBlock pb = GetBlock(x, y);
        hoveredBlock.Add(pb);
        /*
        DisableAllBlock();
        foreach ( PuzzleBlock block in GetAroundBlock(pb))
        {
            block.EnterChar();
        }
        */
    }
    public void OnHover(PuzzleBlock pb)
    {
        hoveredBlock.Add(pb);
    }
    public List<PuzzleBlock> GetAroundBlock(PuzzleBlock pb)
    {
        List<PuzzleBlock> ret = new List<PuzzleBlock>();
        CheckAround(pb.x, pb.y, pb.blockIndex, ret);
        return ret;
    }

    private void CheckAround(int x, int y, int idx, List<PuzzleBlock> list)
    {
        PuzzleBlock pb = GetBlock(x, y);
        if (pb == null) return;
        if (list.Contains(pb)) return;
        if (pb.blockIndex == idx)
        {
            list.Add(pb);
            CheckAround(x + 1, y, idx, list);
            CheckAround(x - 1, y, idx, list);
            CheckAround(x, y + 1, idx, list);
            CheckAround(x, y - 1, idx, list);
        }
    }

    public PuzzleBlock GetBlock(int x, int y)
    {
        if (x < 0||x>=8||y<0||y>=12)return null;
        return blockMap[x, y];
    }
}
public class PuzzleBlockInfo
{
    public BlockStage bs;
    public PuzzleBlock pb;
}