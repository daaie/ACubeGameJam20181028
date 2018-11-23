using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DamageEffect : NetworkBehaviour {

    public GameObject effectPrefab;

    public void SpawnEffect()
    {
        CmdSpawnEffect();
    }

    [Command]
    void CmdSpawnEffect()
    {
        GameObject effect = Instantiate(effectPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(effect);
    }
}
