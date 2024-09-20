using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;


public class PlayerSpawnObject : NetworkBehaviour
{
    public GameObject objToSpawn;
    [HideInInspector]public GameObject spawnedObject;


    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {

        }
        else
        {
            GetComponent<PlayerSpawnObject>().enabled = false;
        }
    }

    private void Update()
    {
        if(spawnedObject == null && Input.GetKeyDown(KeyCode.Q))
        {
            SpawnObject(objToSpawn, transform, this);
        }

        if (spawnedObject != null && Input.GetKeyDown(KeyCode.E))
        {
            DespawnObject(spawnedObject);
        }
    }

    [ServerRpc]
    public void SpawnObject(GameObject obj, Transform player, PlayerSpawnObject script)
    {
        GameObject spawned = Instantiate(obj, player.position + player.forward, Quaternion.identity);
        ServerManager.Spawn(spawned);
        SetSpawnedObject(spawned, script);
    }

    [ObserversRpc]
    public void SetSpawnedObject(GameObject spawned, PlayerSpawnObject script)
    {
        script.spawnedObject = spawned;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObject(GameObject obj)
    {
        ServerManager.Despawn(obj);
    }
}
