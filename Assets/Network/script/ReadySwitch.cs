using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReadySwitch : InteractiveObject {
    Material mat;
    [SyncVar]
    public bool isOn = false;
    [SyncVar]
    public Color color = Color.black;
    
    private void Start()
    {
         mat = GetComponent<Renderer>().material;
        
    }
    private void SetMaterial()
    {
        //Debug.Log("OnConnectedToServer");
        mat.SetColor("_EmissionColor", color);
        mat.color = color;
    }
    
    public override void DoAction()
    {
        base.DoAction();
        Debug.Log("_EmissionColor");
        if (isServer) SetToggle();
    }
    
    private void SetOn()
    {
        isOn = true;
        color = Color.red;
        if (isServer)
            RpcSetMaterial(color);
        GameManager.instance.CheckAllReady();
    }
    private void SetOff()
    {
        isOn = false;
        color = Color.black;
        if (isServer)
            RpcSetMaterial(color);
    }
    [ClientRpc]
    private void RpcSetMaterial(Color c)
    {
        mat.SetColor("_EmissionColor", c);
        mat.color = c;
        Debug.Log("????" + color);
    }
    private void SetToggle()
    {
        if(isOn)
        {
            SetOff();
        }
        else
        {
            SetOn();
        }
    }
    public void Update()
    {
        
    }
}
