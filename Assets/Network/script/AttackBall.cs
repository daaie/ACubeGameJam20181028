using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;

public class AttackBall : NetworkBehaviour {
    private Material mat;
    private int _dir = 0;

    [SyncVar (hook ="SetColor")]
    public Color blockColor = Color.white;

    // Use this for initialization
    void Start () {
        if (mat == null) mat = GetComponent<MeshRenderer>().material;
        mat = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetColor(Color c)
    {
        if (mat == null) mat = GetComponent<MeshRenderer>().material;
        mat.color = c;
        mat.SetColor("_EmissionColor", c);
    }
    public void Fire(int dir, Color c)
    {
        
        if (mat == null) mat = GetComponent<MeshRenderer>().material;
        _dir = dir;
        blockColor = c;
        SetColor(c);
        //Debug.Log(dir);
        Vector3 pos = transform.position + (transform.right * 30f * dir);
        transform.DOMove(pos, 3f).OnComplete(OnComplete);
    }
    private void OnComplete()
    {
        Debug.Log("DMG");
        DestroyImmediate(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        PlayerMovement p = other.GetComponent<PlayerMovement>();
        
        if (p != null)
        {
            if (p.isDead) return;
            if (_dir == 1)
            {
                if(p.myID%2 == 1)
                {
                    //Hit
                    
                    p.RpcDemage(5);
                    p.GetComponent<DamageEffect>().SpawnEffect();
                    transform.DOKill();
                    Destroy(gameObject);
                }
            }
            else
            {
                if (p.myID % 2 == 0)
                {
                    //Hit
                    p.RpcDemage(5);
                    p.GetComponent<DamageEffect>().SpawnEffect();
                    transform.DOKill();
                    Destroy(gameObject);
                }
            }
        }
    }
}
