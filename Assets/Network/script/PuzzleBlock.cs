using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using DG.Tweening;

public class PuzzleBlock : NetworkBehaviour {
    Material mat;
    [SyncVar]
    public int blockIndex = -1;
    [SyncVar]
    public int x;
    [SyncVar]
    public int y;
    [SyncVar]
    public int parentIndex = -1;
    [SyncVar (hook = "UpdateHovered")]
    public bool isHover = false;
    public BlockStage parent;
    private Color[] indexColor = {
        Color.white,
        new Color(159f / 255f, 50f / 255f, 46f / 255f),
        new Color(49f / 255f, 181f / 255f, 49f / 255f),
        new Color(41f / 255f, 41f / 255f, 169f / 255f),
        new Color(188f / 255f, 176f / 255f, 40f / 255f)
    };
    [SyncVar (hook = "SetColor")]
    public Color blockColor = Color.white;


    // Use this for initialization
    void Start () {
        mat = GetComponent<Renderer>().material;
        //SetBlockIndex(1);
        SetColor();
    }
	
	// Update is called once per frame
	void Update () {

        //SetColor();
    }
    

    public void SetBlockIndex(int index)
    {
        blockIndex = index;
        blockColor = indexColor[index % 4 + 1];
        SetColor();
    }
    private void SetColor(Color c)
    {
        blockColor = c;
        if (mat == null) return;
        mat.color = c;
    }
    private void SetColor()
    {
        if (mat == null) return;
        mat.color = blockColor;
    }
    //[ClientRpc]
    public void RpcEnterChar()
    {
        isHover = true;
        //Debug.Log("EnterChar");
        if (blockIndex == -1)
        {
            RpcExitChar();
            return;
        }
        mat.SetColor("_EmissionColor", blockColor);
    }
    //[ClientRpc]
    public void RpcExitChar()
    {
        isHover = false;
        if (mat == null) return;
        mat.SetColor("_EmissionColor", Color.black);
    }
    public void UpdateHovered(bool isHovered)
    {
        if (isHovered) RpcEnterChar();
        else RpcExitChar();
    }
    public void SelectBlock()
    {
        blockIndex = -1;
        blockColor = Color.white;
        if(isServer)
        {
            DOVirtual.DelayedCall(3f, SetRandomIndex);
        }
    }
    private void SetRandomIndex()
    {
        int index = Random.Range(0, 4);
        SetBlockIndex(index);
    }
}
